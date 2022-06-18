using System.Net;

namespace MSHA.ApiConnections
{
    public static class HttpUtility
    {
        /// <summary>
        /// Returns true if the status code corresponds to a successful request.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        public static bool IsSuccessfulRequest(this int statusCode)
        {
            return (statusCode >= 200 && statusCode <= 299) || statusCode == 304 || statusCode == 302;
        }

        /// <summary>
        /// Returns true if the status code corresponds to a successful request.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        public static bool IsSuccessfulRequest(this HttpStatusCode statusCode)
        {
            return HttpUtility.IsSuccessfulRequest((int)statusCode);
        }

        /// <summary>
        /// Returns true if the status code corresponds to client failure.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        public static bool IsClientFailureRequest(this int statusCode)
        {
            return statusCode >= 400 && statusCode < 500 && statusCode != 408;
        }

        /// <summary>
        /// Returns true if the status code corresponds to client failure.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        public static bool IsClientFailureRequest(this HttpStatusCode statusCode)
        {
            return HttpUtility.IsClientFailureRequest((int)statusCode);
        }

        /// <summary>
        /// Returns true if the status code corresponds to a server failure request.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        public static bool IsServerFailureRequest(this int statusCode)
        {
            return statusCode >= 500 || statusCode == 408 || statusCode < 100;
        }

        /// <summary>
        /// Returns true if the status code corresponds to a server failure request.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        public static bool IsServerFailureRequest(this HttpStatusCode statusCode)
        {
            return HttpUtility.IsServerFailureRequest((int)statusCode);
        }
    }
}
