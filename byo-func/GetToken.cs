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
    public static class GetToken
    {
        [FunctionName("GetToken")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = ".token/{tokenProviderId}")] HttpRequest req,
            ILogger log)
        {
            var httpClient = new HttpClient();

            req.Headers.TryGetValue("X-MS-CLIENT-PRINCIPAL-ID", out var principalId);

            var connectionId = principalId.ToString();
            log.LogInformation($"connectionId - {connectionId}");

            if (string.IsNullOrEmpty(connectionId)) 
            {
                 return  new ContentResult { StatusCode =  403, Content =  "Please Authenticate!" };
            }

            var routeData = req.HttpContext.GetRouteData();
            var tokenProviderId = routeData?.Values["tokenProviderId"]?.ToString();
            log.LogInformation($"connectorName - {tokenProviderId}");
                
            var gatewayUrl =  Environment.GetEnvironmentVariable("APIMGATEWAYURL");
            var gatewayKey =  Environment.GetEnvironmentVariable("APIMKEY");

            log.LogInformation($"gatewayUrl - {gatewayUrl}");

            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", gatewayKey);

            var connection = await ConnectionManager.GetConnectionAsync(tokenProviderId.ToString(), connectionId);

            if (connection != null && connection.Properties.Status.ToUpper().Equals("CONNECTED")) 
            {
                var runtimeURL = $"{gatewayUrl}/GenericProxy/token";
                log.LogInformation($"Calling Url - {runtimeURL}");

                var httpMethod = new HttpMethod("GET");
                using(var httpRequestMessage = new HttpRequestMessage(httpMethod, runtimeURL)) 
                {
                   httpRequestMessage.Headers.Add("connector-id", tokenProviderId.ToString());
                   httpRequestMessage.Headers.Add("connection-id", connectionId.ToString());

                    var result = await httpClient.SendAsync(httpRequestMessage);

                    var statusCode = (int)result.StatusCode;
                    var content = await result.Content.ReadAsStringAsync();

                     log.LogInformation($"statusCode - {statusCode}");
                     // log.LogInformation($"content - {content}");

                    return  new ContentResult { StatusCode =  statusCode, Content =  content };
                }
            }
            else 
            {
                return new BadRequestResult();
            }
        }
    }
}
