using System.Collections.Generic;
using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class ApiCall
    {
        [JsonProperty("url")]
        public object Url { get; set; }

        [JsonProperty("body")]
        public object Body { get; set; }

        [JsonProperty("method")]
        public object Method { get; set; }

        [JsonProperty("headers")]
        public List<object> Headers { get; set; }

        [JsonProperty("buttonLabel")]
        public string ButtonLabel { get; set; }
    }
}