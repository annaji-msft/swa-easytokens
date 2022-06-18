using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MSHA.ApiConnections;
using Microsoft.AspNetCore.Routing;

namespace Company.Function
{
    public static class Status
    {
        [FunctionName("Status")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ".token/status/{tokenProviderId}")] HttpRequest req,
            ILogger log)
        {
            string responseMessage = JsonConvert.SerializeObject(req.Headers);
            req.Headers.TryGetValue("X-MS-CLIENT-PRINCIPAL-ID", out var principalId);

            var routeData = req.HttpContext.GetRouteData();
            var tokenProviderId = routeData?.Values["tokenProviderId"]?.ToString();
            log.LogInformation($"connectorName - {tokenProviderId}");

            log.LogInformation($"principalId - {principalId}");
            
            AuthorizationResource connection = null;

            var connectionId = principalId.ToString();
            log.LogInformation($"connectionId - {connectionId}");

            if (string.IsNullOrEmpty(connectionId)) 
            {
                 return  new ContentResult { StatusCode =  403, Content =  "Please Authenticate!" };
            }

            connection = await ConnectionManager.GetConnectionAsync(tokenProviderId, connectionId);

            if (connection != null) 
            {
                if(connection.Properties.Status.ToUpper().Equals("CONNECTED"))
                {
                    return new ContentResult { StatusCode = 200, Content = connection.Properties.Status.ToUpper() };
                }

                return new ContentResult { StatusCode = 401, Content = connection.Properties.Status.ToUpper() };
            }
            else 
            {
                return new NotFoundResult();
            }     
        }
    }
}
