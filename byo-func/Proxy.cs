using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using MSHA.ApiConnections;
using Microsoft.AspNetCore.Routing;

namespace Company.Function
{
    public static class Proxy
    {
        [FunctionName("Proxy")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", "patch", "delete",
             Route = "proxy/{*path}")] 
            HttpRequest req,
            ILogger log)
        {
            var httpClient = new HttpClient();

            req.Headers.TryGetValue("X-MS-CLIENT-PRINCIPAL-ID", out var principalId);

            var connectionId = principalId.ToString();
            log.LogInformation($"connectionId - {connectionId}");

            req.Headers.TryGetValue("X-MS-TOKENPROVIDER-ID", out var tokenProviderId);
            log.LogInformation($"connectorName - {tokenProviderId}");

            if (string.IsNullOrEmpty(connectionId)) 
            {
                 return  new ContentResult { StatusCode =  403, Content =  "Please authenticate!" };
            }

            req.Headers.TryGetValue("X-MS-PROXY-BACKEND-HOST", out var backendHost);
            log.LogInformation($"backendHost - {backendHost}");

            var routeData = req.HttpContext.GetRouteData();
            var backendApi = routeData?.Values["path"]?.ToString();
            log.LogInformation($"backendApi - {backendApi}");
                
            var gatewayUrl =  Environment.GetEnvironmentVariable("APIMGATEWAYURL");
            var gatewayKey =  Environment.GetEnvironmentVariable("APIMKEY");

            log.LogInformation($"gatewayUrl - {gatewayUrl}");

            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", gatewayKey);

            var connection = await ConnectionManager.GetConnectionAsync(tokenProviderId.ToString(), connectionId);

            if (connection != null && connection.Properties.Status.ToUpper().Equals("CONNECTED")) 
            {
                var runtimeURL = $"{gatewayUrl}/GenericProxy/{backendApi.ToString()}";
                log.LogInformation($"Calling Url - {runtimeURL}");

                var httpMethod = new HttpMethod(req.Method);
                using(var httpRequestMessage = new HttpRequestMessage(httpMethod, runtimeURL)) 
                {
                     if (req.ContentLength.GetValueOrDefault() > 0)
                    {
                        httpRequestMessage.Content = new StreamContent(req.Body);
                        httpRequestMessage.Content.Headers.ContentLength = req.ContentLength;

                        if (!string.IsNullOrEmpty(req.ContentType))
                        {
                            httpRequestMessage.Content.Headers.TryAddWithoutValidation("Content-Type", "application/json");
                        }
                    }

                   httpRequestMessage.Headers.Add("connector-id", tokenProviderId.ToString());
                   httpRequestMessage.Headers.Add("connection-id", connectionId.ToString());
                   httpRequestMessage.Headers.Add("backend-host", backendHost.ToString());

                    var result = await httpClient.SendAsync(httpRequestMessage);

                    return  new ContentResult { StatusCode =  (int)result.StatusCode, Content =  await result.Content.ReadAsStringAsync() };
                }
            }
            else 
            {
                return new BadRequestResult();
            }
        }
    }
}
