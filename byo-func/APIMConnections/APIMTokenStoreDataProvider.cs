using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace MSHA.ApiConnections
{
	public class APIMTokenStoreDataProvider : AzureResourceManagerDataProvider, IAPIMTokenStoreDataProvider
	{
		public APIMTokenStoreDataProvider(IDiagnosticsTracing logger, HttpClient httpClient)
			: base(logger, httpClient)
		{
		}

		private static Uri ListTokenProvidersUri(
			string subscriptionId, 
			string resourceGroupName, 
			string serviceName)
		{
			return new Uri(
				baseUri: AzureResourceManagerDataProvider.AzureResourceManagerApiEndpoint,
				relativeUri: $"subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.ApiManagement/service/{serviceName}/authorizationProviders?api-version=2021-04-01-preview");
		}

		private static Uri GetCreateApiConnectionUri(
			string subscriptionId, 
			string resourceGroupName, 
			string serviceName,
			string tokenProviderName,
			string connectionName)
		{
			return new Uri(
				baseUri: AzureResourceManagerDataProvider.AzureResourceManagerApiEndpoint,
				relativeUri: $"subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.ApiManagement/service/{serviceName}/authorizationProviders/{tokenProviderName}/authorizations/{connectionName}?api-version=2021-04-01-preview");
		}

		private static Uri GetCreatePermissionUri(
			string subscriptionId, 
			string resourceGroupName, 
			string serviceName,
			string tokenProviderName,
			string connectionName,
			string permissionName)
		{
			return new Uri(
				baseUri: AzureResourceManagerDataProvider.AzureResourceManagerApiEndpoint,
				relativeUri: $"subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.ApiManagement/service/{serviceName}/authorizationProviders/{tokenProviderName}/authorizations/{connectionName}/accessPolicies/{permissionName}?api-version=2021-04-01-preview");
		}

		private static Uri GetConsentLinksApiConnectionUri(
			string subscriptionId, 
			string resourceGroupName, 
			string serviceName,
			string tokenProviderName,
			string connectionName)
		{
			return new Uri(
				baseUri: AzureResourceManagerDataProvider.AzureResourceManagerApiEndpoint,
				relativeUri: $"subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.ApiManagement/service/{serviceName}/authorizationProviders/{tokenProviderName}/authorizations/{connectionName}/getLoginLinks?api-version=2021-04-01-preview");
		}

		public async Task<AzureResourceList<AuthorizationProviderResource>> ListAuthorizationProvidersAsync(
			string accessToken,
			string subscriptionId,
			string resourceGroupId,
			string serviceName)
		{
			//subscriptionId.CheckArgumentForNullOrWhiteSpace(nameof(subscriptionId));
			//resourceGroupId.CheckArgumentForNullOrWhiteSpace(nameof(resourceGroupId));
			//location.CheckArgumentForNullOrWhiteSpace(nameof(location));
			//connectorType.CheckArgumentForNullOrWhiteSpace(nameof(connectorType));

			 var requestUri = APIMTokenStoreDataProvider.ListTokenProvidersUri(
				subscriptionId,
				resourceGroupId,
				serviceName);

			//TODO: need to add request content (body.json)
			var result = await base.CallAzureResourceManagerAsync<AzureResourceList<AuthorizationProviderResource>>(
				accessToken: accessToken,
				requestUri: requestUri,
				httpMethod: HttpMethod.Get)
				.ConfigureAwait(continueOnCapturedContext: false);

			if (!result.HttpStatusCode.IsSuccessfulRequest())
			{
				throw new InvalidOperationException(
					message: string.Format("Call to get connection failed with '{0}'", result.Error));
			}

			return result.Response;
		}

		public async Task<AuthorizationResource> CreateAuthorizationAsync(
			string accessToken,
			string subscriptionId,
			string resourceGroupId,
			string serviceName,
			string tokenProviderName,
			string connectionName,
			string tenantId)
		{
			//subscriptionId.CheckArgumentForNullOrWhiteSpace(nameof(subscriptionId));
			//resourceGroupId.CheckArgumentForNullOrWhiteSpace(nameof(resourceGroupId));
			//location.CheckArgumentForNullOrWhiteSpace(nameof(location));
			//connectorType.CheckArgumentForNullOrWhiteSpace(nameof(connectorType));

			 var requestUri = APIMTokenStoreDataProvider.GetCreateApiConnectionUri(
				subscriptionId,
				resourceGroupId,
				serviceName,
				tokenProviderName,
				connectionName);

			//TODO: need to add request content (body.json)
			var result = await base.CallAzureResourceManagerAsync<AuthorizationResource, AuthorizationResource>(
				accessToken: accessToken,
				requestUri: requestUri,
				httpMethod: HttpMethod.Put,
				requestContent: new AuthorizationResource { Properties = new AuthorizationResourceProperties() })
				.ConfigureAwait(continueOnCapturedContext: false);

			if (!result.HttpStatusCode.IsSuccessfulRequest())
			{
				throw new InvalidOperationException(
					message: string.Format("Call to create connection failed with '{0}'", result.Error));
			}

			return result.Response;
		}

		public async Task<AuthorizationResource> GetAuthorizationAsync(
			string accessToken,
			string subscriptionId,
			string resourceGroupId,
			string serviceName,
			string tokenProviderName,
			string connectionName)
		{
			//subscriptionId.CheckArgumentForNullOrWhiteSpace(nameof(subscriptionId));
			//resourceGroupId.CheckArgumentForNullOrWhiteSpace(nameof(resourceGroupId));
			//location.CheckArgumentForNullOrWhiteSpace(nameof(location));
			//connectorType.CheckArgumentForNullOrWhiteSpace(nameof(connectorType));

			 var requestUri = APIMTokenStoreDataProvider.GetCreateApiConnectionUri(
				subscriptionId,
				resourceGroupId,
				serviceName,
				tokenProviderName,
				connectionName);

			//TODO: need to add request content (body.json)
			var result = await base.CallAzureResourceManagerAsync<AuthorizationResource>(
				accessToken: accessToken,
				requestUri: requestUri,
				httpMethod: HttpMethod.Get)
				.ConfigureAwait(continueOnCapturedContext: false);

			// This should be NotFound
			if(result.HttpStatusCode == HttpStatusCode.NotFound) return null;

			if (!result.HttpStatusCode.IsSuccessfulRequest())
			{
				throw new InvalidOperationException(
					message: string.Format("Call to get connection failed with '{0}'", result.Error));
			}

			return result.Response;
		}

		public async Task<LoginLinkResponse> GetConsentLinkAsync(
			string accessToken, 
			string subscriptionId, 
			string resourceGroupId, 
			string serviceName,
			string tokenProviderName,
			string connectionName, 
			string redirectUrl)
		{
			//subscriptionId.CheckArgumentForNullOrWhiteSpace(nameof(subscriptionId));
			//resourceGroupId.CheckArgumentForNullOrWhiteSpace(nameof(resourceGroupId));
			//connectorType.CheckArgumentForNullOrWhiteSpace(nameof(connectorType));

			var requestUri = APIMTokenStoreDataProvider.GetConsentLinksApiConnectionUri(
				subscriptionId,
				resourceGroupId,
				serviceName,
				tokenProviderName,
				connectionName);

			var requestContent = new LoginLinkRequest();
			requestContent.PostLoginRedirectUrl = redirectUrl;

			var result = await base.CallAzureResourceManagerAsync<LoginLinkRequest, LoginLinkResponse>(
				accessToken: accessToken,
				requestUri: requestUri,
				httpMethod: HttpMethod.Post,
				requestContent: requestContent)
				.ConfigureAwait(continueOnCapturedContext: false);

			if (!result.HttpStatusCode.IsSuccessfulRequest())
			{
				throw new InvalidOperationException(
					message: string.Format("Call to get consent link failed with '{0}'", result.Error));
			}

			return result.Response;
		}

		public async Task<object> DeleteAuthorizationAsync(
			string accessToken,
			string subscriptionId,
			string resourceGroupId,
			string serviceName,
			string tokenProviderName,
			string connectionName)
		{
			var requestUri = APIMTokenStoreDataProvider.GetCreateApiConnectionUri(
			   subscriptionId,
			   resourceGroupId,
			   serviceName,
			   tokenProviderName,
			   connectionName);

			//TODO: need to add request content (body.json)
			var result = await base.CallAzureResourceManagerAsync(
				accessToken: accessToken,
				requestUri: requestUri,
				httpMethod: HttpMethod.Delete)
				.ConfigureAwait(continueOnCapturedContext: false);

			if (!result.HttpStatusCode.IsSuccessfulRequest())
			{
				throw new InvalidOperationException(
					message: string.Format("Call to create connection failed with '{0}'", result.Error));
			}

			return result.Response;
		}

		public async Task<object> CreatePermissionAsync(
			string accessToken,
			string subscriptionId,
			string resourceGroupId,
			string serviceName,
			string tokenProviderName,
			string connectionName,
			string permissionName,
			PermissionResource permissionResource)
		{
			var requestUri = APIMTokenStoreDataProvider.GetCreatePermissionUri(
			   subscriptionId,
			   resourceGroupId,
			   serviceName,
			   tokenProviderName,
			   connectionName, 
			   permissionName);

			//TODO: need to add request content (body.json)
			var result = await base.CallAzureResourceManagerAsync<PermissionResource, PermissionResource>(
				accessToken: accessToken,
				requestUri: requestUri,
				httpMethod: HttpMethod.Put,
				requestContent: permissionResource)
				.ConfigureAwait(continueOnCapturedContext: false);

			if (!result.HttpStatusCode.IsSuccessfulRequest())
			{
				throw new InvalidOperationException(
					message: string.Format("Call to create permission failed with '{0}'", result.Error));
			}

			return result.Response;
		}

	}
}
