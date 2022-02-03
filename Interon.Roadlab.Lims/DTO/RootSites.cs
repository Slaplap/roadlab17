using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class RootSites
    {
        [JsonProperty("items")]
        public List<Site> Sites { get; set; }    

        [JsonProperty("incomplete_results")]
        public bool IncompleteResults { get; set; }

        [JsonProperty("total_count")]
        public int TotalCount { get; set; }
    }
}
