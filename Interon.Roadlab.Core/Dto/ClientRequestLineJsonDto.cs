using System;
using Newtonsoft.Json;

namespace Interon.Roadlab.Core.Models
{
     
    public class ClientRequestLineJsonDto

    {
        [JsonProperty("key")]
        public Guid Key { get; set; }
        [JsonProperty("testId")]
        public int TestId { get; set; }
        [JsonProperty("categoryId")]
        public int CategoryId { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("qty")]
        public int Qty { get; set; }
        [JsonProperty("transactionKey")]
        public Guid  ClientRequestKey { get; set; }
    }
}