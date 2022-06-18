using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSHA.ApiConnections
{
    public interface IAPIMTokenStoreDataProvider
    {
		Task<AzureResourceList<AuthorizationProviderResource>> ListAuthorizationProvidersAsync(
			string accessToken,
			string subscriptionId,
			string resourceGroupId,
			string serviceName);

       Task<AuthorizationResource> CreateAuthorizationAsync(
			string accessToken,
			string subscriptionId,
			string resourceGroupId,
			string serviceName,
			string tokenProviderName,
			string connectionName,
			string tenantId);
        
         Task<AuthorizationResource> GetAuthorizationAsync(
			string accessToken,
			string subscriptionId,
			string resourceGroupId,
			string serviceName,
			string tokenProviderName,
			string connectionName);

        Task<LoginLinkResponse> GetConsentLinkAsync(
			string accessToken, 
			string subscriptionId, 
			string resourceGroupId, 
			string serviceName,
			string tokenProviderName,
			string connectionName, 
			string redirectUrl);

        Task<object> DeleteAuthorizationAsync(
			string accessToken,
			string subscriptionId,
			string resourceGroupId,
			string serviceName,
			string tokenProviderName,
			string connectionName);

		Task<object> CreatePermissionAsync(
			string accessToken,
			string subscriptionId,
			string resourceGroupId,
			string serviceName,
			string tokenProviderName,
			string connectionName,
			string permissionName,
			PermissionResource permissionResource);
    }
}
