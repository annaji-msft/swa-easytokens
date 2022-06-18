using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MSHA.ApiConnections
{
	public class PermissionResourceProperties
	{
		[JsonProperty(PropertyName = "objectId")]
		public string ObjectId { get; set; }

		[JsonProperty(PropertyName = "tenantId")]
		public string TenantId { get; set; }
	}
}
