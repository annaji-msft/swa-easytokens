using System.Net;
using System.Net.Http.Headers;

namespace MSHA.ApiConnections
{
    /// <summary>
    /// The resource result from a REST call.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    public class ResourceResult<T>
    {
        /// <summary>
        /// Gets or sets the response body for this result.
        /// </summary>
        public T Response { get; set; }

        /// <summary>
        /// Gets or sets the error (if any) for this result.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Gets or sets the response status code.
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; set; }

        /// <summary>
        /// Gets or sets the response headers.
        /// </summary>
        public HttpResponseHeaders Headers { get; set; }
    }
}
