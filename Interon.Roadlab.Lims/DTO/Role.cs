using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    
    public class Role
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("parent_id")]
        public int ParentId { get; set; }

        [JsonProperty("sort_order")]
        public object SortOrder { get; set; }

        [JsonProperty("sort_style")]
        public int SortStyle { get; set; }

        [JsonProperty("list_index")]
        public int? ListIndex { get; set; }

        [JsonProperty("has_filter")]
        public object HasFilter { get; set; }

        [JsonProperty("enabled")]
        public object Enabled { get; set; }
    }
}
