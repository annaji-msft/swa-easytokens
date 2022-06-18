using System.Collections.Generic;
using Newtonsoft.Json;

namespace MSHA.ApiConnections
{
    public class AzureResourceList<T>
    {
        [JsonProperty(Required = Required.Default, PropertyName = "value")]
        public List<T> Values { get; set; }

        [JsonProperty(Required = Required.Default, PropertyName = "nextLink")]
        public string NextLink { get; set; }
    }
}