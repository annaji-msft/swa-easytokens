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
using System.Text.Json;

namespace Company.Function
{
    public static class Providers
    {
        [FunctionName("Providers")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ".token/providers")] HttpRequest req,
            ILogger log)
        {
            req.Headers.TryGetValue("X-MS-CLIENT-PRINCIPAL-ID", out var principalId);
            log.LogInformation($"principalId - {principalId}");

            if (string.IsNullOrEmpty(principalId.ToString())) 
            {
                 return  new ContentResult { StatusCode =  403, Content =  "Please Authenticate!" };
            }

            var providers = await ConnectionManager.ListTokenProvidersAsync();

            return new ContentResult { Content = System.Text.Json.JsonSerializer.Serialize(providers).ToString(), StatusCode =  200 };
        }
    }
}
