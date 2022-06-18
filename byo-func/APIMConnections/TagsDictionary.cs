using System;
using System.Collections.Generic;

namespace MSHA.ApiConnections
{
    /// <summary>
    /// Represents a resource tag dictionary.
    /// </summary>
    [JsonPreserveCaseDictionary]
    public class TagsDictionary : Dictionary<string, string>
    {
        /// <summary>
        /// The empty tags dictionary.
        /// </summary>
        public static readonly TagsDictionary Empty = new TagsDictionary();

        /// <summary>
        /// Initializes a new instance of the <see cref="TagsDictionary" /> class.
        /// </summary>
        public TagsDictionary()
            : base(StringComparer.InvariantCultureIgnoreCase)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TagsDictionary" /> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        public TagsDictionary(IDictionary<string, string> dictionary)
            : base(dictionary, StringComparer.InvariantCultureIgnoreCase)
        {
        }
    }
}
