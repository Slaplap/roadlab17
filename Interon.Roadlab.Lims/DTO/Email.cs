using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class Email
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }
    }
}