using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;


namespace MSHA.ApiConnections
{
    public class AzureResourceManagerDataProvider
    {
        private const int AsyncTimeoutInSeconds = 60 * 5;

        protected readonly HttpClient _httpClient;
        protected readonly IDiagnosticsTracing _logger;

        public AzureResourceManagerDataProvider(IDiagnosticsTracing logger, HttpClient httpClient)
        {
            logger.CheckArgumentForNull(nameof(logger));
            httpClient.CheckArgumentForNull(nameof(httpClient));

            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Gets the Azure Resource Manager base url
        /// </summary>
        public static Uri AzureResourceManagerApiEndpoint
        {
            // TODO: Get it from AppSettings
            get { return  new Uri("https://management.azure.com/"); }
        }

        /// <summary>
        /// Gets the Azure Resource Manager api version.
        /// </summary>
        public static string AzureResourceManagerApiVersion
        {
            // TODO: Get it from AppSettings
            get { return "2014-04-01"; }
        }

        public HttpClient HttpClient { get { return _httpClient; } }

        public IDiagnosticsTracing Logger { get { return _logger; } }

        public async Task<ResourceResult<T>> CallAzureResourceManagerAsync<T>(
            string accessToken, 
            Uri requestUri, 
            HttpMethod httpMethod, 
            T requestContent = null,
            bool pollAsyncOperation = false) 
            where T : class
        {
            return await
                CallAzureResourceManagerAsync<T, T>(
                    accessToken: accessToken,
                    requestUri: requestUri,
                    httpMethod: httpMethod,
                    requestContent: requestContent,
                    isAzureAsyncOperation: pollAsyncOperation);
        }

        public async Task<ResourceResult<object>> CallAzureResourceManagerAsync(
            string accessToken, 
            Uri requestUri, 
            HttpMethod httpMethod)
        {
            return await CallAzureResourceManagerAsync<object>(
                accessToken: accessToken,
                requestUri: requestUri,
                httpMethod: httpMethod);
        }

        public async Task<ResourceResult<TResponse>> CallAzureResourceManagerAsync<TRequest, TResponse>(
            string accessToken,
            Uri requestUri,
            HttpMethod httpMethod,
            TRequest requestContent = null,
            bool isAzureAsyncOperation = false)
            where TRequest : class
            where TResponse : class
        {
            // TODO: Handle Paging of resources and aggregate

            var request = new HttpRequestMessage(httpMethod, requestUri);

            if (requestContent != null)
            {
                var contentString = JsonConvert.SerializeObject(requestContent, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                request.Content = new StringContent(contentString, Encoding.UTF8, "application/json");
            }

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            try
            {
                var response = await _httpClient.SendAsync(
                    request: request)
                    .ConfigureAwait(continueOnCapturedContext: false);

                return await GetResourceResult<TResponse>(response);
            }
            catch (Exception exception)
            {
                if (exception.IsFatal())
                {
                    throw;
                }

                return new ResourceResult<TResponse>()
                {
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    Error =  string.Format("Unexpected error occurred when calling azure resource manager: '{0}'", exception.GetBaseException())
                };
            }
        }

        /// <summary>
        /// Gets the resource result from the response message.
        /// </summary>
        /// <param name="responseMessage">The response message.</param>
        /// <typeparam name="TResult">The type of the response.</typeparam>
        public static async Task<ResourceResult<TResult>> GetResourceResult<TResult>(
            HttpResponseMessage responseMessage)
            where TResult : class
        {
            if (responseMessage == null)
            {
                return new ResourceResult<TResult>()
                {
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    Error = "Null response was received."
                };
            }

            var callSuccessful = responseMessage.StatusCode.IsSuccessfulRequest();
            var responseBody = await responseMessage.Content.ReadAsStringAsync()
                    .ConfigureAwait(continueOnCapturedContext: false);

            return new ResourceResult<TResult>()
            {
                HttpStatusCode = responseMessage.StatusCode,
                Headers = responseMessage.Headers,
                Response = callSuccessful
                    ? GetResponse<TResult>(responseBody)
                    : default(TResult),
                Error = callSuccessful ? default(string) : responseBody
            };
        }

        private static TResult GetResponse<TResult>(string responseBody) where TResult : class
        {
            return (typeof(TResult) != typeof(string) && typeof(TResult) != typeof(Object))
                            ? JsonConvert.DeserializeObject<TResult>(responseBody)
                            : responseBody as TResult;
        }

    }
}
