using System.Collections.Generic;
using Newtonsoft.Json;

namespace MSHA.ApiConnections
{
    /// <summary>
    /// The azure resource envelope.
    /// </summary>
    /// <typeparam name="TProperties">The properties of the resource.</typeparam>
    public class AzureResource<TProperties> where TProperties : class
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the kind.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string Kind { get; set; }

        /// <summary>
        /// Gets or sets the types.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the <c>sku</c> of the resource.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public Sku Sku { get; set; }

        /// <summary>
        /// Gets or sets the <c>etag</c>.
        /// </summary>
        [JsonProperty(Required = Required.Default, PropertyName = "etag")]
        public string ETag { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public Dictionary<string, string> Tags { get; set; }

        /// <summary>
        /// Gets or sets the resource properties.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public TProperties Properties { get; set; }
    }
}
