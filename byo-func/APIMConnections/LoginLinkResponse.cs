using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MSHA.ApiConnections
{
	public class LoginLinkResponse
	{
		[JsonProperty(PropertyName = "loginLink")]
		public string LoginLink { get; set; }
	}
}
