using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class Status
    {
        [JsonProperty("success")]
        public int Success { get; set; }

        [JsonProperty("error")]
        public bool Error { get; set; }
    }
}
