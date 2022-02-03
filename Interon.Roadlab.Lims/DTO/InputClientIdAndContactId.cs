using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class InputByClientIdAndContact 
    {
        [JsonProperty("client_id")]
        public int ClientId { get; set; }

        [JsonProperty("contact")]
        public Contact Contact { get; set; }
    }
}