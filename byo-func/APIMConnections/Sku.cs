using Newtonsoft.Json;

namespace MSHA.ApiConnections
{
    /// <summary>The SKU.</summary>
    public class Sku
    {
        /// <summary>Gets or sets the name.</summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>Gets or sets the capacity.</summary>
        [JsonProperty(PropertyName = "capacity")]
        public int? Capacity { get; set; }

        /// <summary>Gets or sets the tier.</summary>
        [JsonProperty(PropertyName = "tier")]
        public string Tier { get; set; }

        /// <summary>Gets or sets the family.</summary>
        [JsonProperty(PropertyName = "family")]
        public string Family { get; set; }

        /// <summary>Gets or sets the size.</summary>
        [JsonProperty(PropertyName = "size")]
        public string Size { get; set; }
    }
}
