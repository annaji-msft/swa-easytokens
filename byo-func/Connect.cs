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
    public static class Connect
    {
        [FunctionName("Connect")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = ".token/create/{tokenProviderId}")] HttpRequest req,
            ILogger log)
        {
            req.Headers.TryGetValue("X-MS-CLIENT-PRINCIPAL-ID", out var principalId);
            req.Headers.TryGetValue("X-MS-ORIGINAL-URL", out var swaUrl);
            
             var routeData = req.HttpContext.GetRouteData();
            var tokenProviderId = routeData?.Values["tokenProviderId"]?.ToString();
            log.LogInformation($"connectorName - {tokenProviderId}");

            // Redirect can be passed in as well?
            var redirectUrl = swaUrl.ToString().Replace($"/api/.token/create/{tokenProviderId}", string.Empty);

            log.LogInformation($"principalId - {principalId}");
            log.LogInformation($"swaurl - {swaUrl}");
            log.LogInformation($"redirecturl - {redirectUrl}");
             log.LogInformation($"connectorId - {tokenProviderId}");
            
            AuthorizationResource connection = null;

            var connectionId = principalId.ToString();
            log.LogInformation($"connectionId - {connectionId}");

            if (string.IsNullOrEmpty(connectionId)) 
            {
                 return  new ContentResult { StatusCode =  403, Content =  "Please Authenticate!" };
            }

            connection = await ConnectionManager.GetConnectionAsync(tokenProviderId, connectionId);

            if (connection == null) 
            {
                log.LogInformation("connection not found!");
                connection = await ConnectionManager.CreateConnectionAsync(tokenProviderId, connectionId, "72f988bf-86f1-41af-91ab-2d7cd011db47");
                
                // add apim system identity
                var permission = await ConnectionManager.CreatePermissionAsync(
                    tokenProviderId, 
                    connectionId, 
                    "72f988bf-86f1-41af-91ab-2d7cd011db47",
                    "3445f948-0790-4954-b27a-495d14333969");

                var consentLinks = await ConnectionManager.GetConsentLinkAsync(tokenProviderId, connectionId, redirectUrl);
                return new ContentResult { Content =  consentLinks.LoginLink, StatusCode =  401 };
            } 
            else if (connection.Properties.Status.ToUpper().Equals("ERROR")) 
            {
                log.LogInformation("connection found but not authenticated!");
                var consentLinks = await ConnectionManager.GetConsentLinkAsync(tokenProviderId, connectionId, redirectUrl);
                return new ContentResult { Content =  consentLinks.LoginLink, StatusCode =  401 };
            } 
            else 
            {
                 log.LogInformation("connection found and authenticated!");
                return new OkObjectResult("Success!");
            }     
        }
    }
}
