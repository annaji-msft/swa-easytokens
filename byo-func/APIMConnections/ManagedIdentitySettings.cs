
namespace MSHA.ApiConnections
{
    public class ManagedIdentitySettings
    {
        /// <summary>
        /// Gets or sets the client id of user managed identity.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the resource endpoint URI.
        /// </summary>
        public string ResourceEndpoint { get; set; }
    }
}
