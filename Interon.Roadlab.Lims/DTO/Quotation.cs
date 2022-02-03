using System.Collections.Generic;
using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class Quotation
    {
        [JsonProperty("services")]
        public List<Service> Services { get; set; }

        [JsonProperty("equipment")]
        public List<Equipment> Equipment { get; set; }
    }
}