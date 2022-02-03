using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    // Companies myDeserializedClass = JsonConvert.DeserializeObject<Companies>(myJsonResponse); 

    public class RootClients
    {
        [JsonProperty("items")] public IEnumerable<Client> CompanyList { get; set; } = new List<Client>();

        [JsonProperty("incomplete_results")]
        public bool IncompleteResults { get; set; }

        [JsonProperty("total_count")]
        public int TotalCount { get; set; }
    }
}
