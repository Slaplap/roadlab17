using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class Equipment
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("state")]
        public int State { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("maintained")]
        public string Maintained { get; set; }

        [JsonProperty("maintenanceRequired")]
        public string MaintenanceRequired { get; set; }
    }
}