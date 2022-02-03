using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class Client
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }
    }
}