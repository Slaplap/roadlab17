using System.Collections.Generic;
using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class RootUsersInGroup
    {
        [JsonProperty("items")]
        public List<Item> Items { get; set; }

        [JsonProperty("incomplete_results")]
        public bool IncompleteResults { get; set; }

        [JsonProperty("total_count")]
        public int TotalCount { get; set; }
    }
}