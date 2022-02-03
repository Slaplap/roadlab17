using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class Delay
    {
        [JsonProperty("impact_of_delay")]
        public string ImpactOfDelay { get; set; }

        [JsonProperty("nature_of_delay")]
        public string NatureOfDelay { get; set; }
    }
}