using System.Collections.Generic;
using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class RootMessage
    {
        public string message { get; set; }
    }
    public class RootClientRequestArray
    {
        [JsonProperty("MyArray")]
        public List<RootClientRequest> MyArray { get; set; }
    }
    public class RootClientRequest
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("date_created")]
        public string DateCreated { get; set; }

        [JsonProperty("date_updated")]
        public string DateUpdated { get; set; }

        [JsonProperty("status")]
        public List<string> Status { get; set; }

        [JsonProperty("tokenData")]
        public TokenData TokenData { get; set; }
      

    }

}