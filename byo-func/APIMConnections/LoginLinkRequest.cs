using Newtonsoft.Json;

namespace MSHA.ApiConnections
{
	public class LoginLinkRequest
	{
		[JsonProperty(PropertyName = "postLoginRedirectUrl")]
		public string PostLoginRedirectUrl { get; set; }
	}
}
