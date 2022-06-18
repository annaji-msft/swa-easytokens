using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;

namespace MSHA.ApiConnections
{
    public class ManagedIdentityDataProvider : IAuthenticationDataProvider
    {
        private readonly ManagedIdentitySettings _managedIdentitySettings;
        private readonly string _authConnectionString;
        private readonly IDiagnosticsTracing _logger;

        public ManagedIdentityDataProvider(ManagedIdentitySettings managedIdentitySettings, IDiagnosticsTracing logger)
        {
            logger.CheckArgumentForNull(nameof(logger));
            managedIdentitySettings.ResourceEndpoint.CheckArgumentForNullOrEmpty(nameof(managedIdentitySettings));
            logger.CheckArgumentForNull(nameof(logger));

            _managedIdentitySettings = managedIdentitySettings;
            _logger = logger;

            _authConnectionString = !string.IsNullOrWhiteSpace(this._managedIdentitySettings.ClientId) 
                ?  $"RunAs=App;AppId={this._managedIdentitySettings.ClientId}" : null;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            return await this.GetAccessTokenAsync(this._managedIdentitySettings.ResourceEndpoint)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        public async Task<string> GetAccessTokenAsync(string resourceEndPoint)
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider(this._authConnectionString);

            this._logger.Informational($"ManagedIdentity PrincipalUsed - {azureServiceTokenProvider.PrincipalUsed}");
            return await azureServiceTokenProvider.GetAccessTokenAsync(resourceEndPoint)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}
