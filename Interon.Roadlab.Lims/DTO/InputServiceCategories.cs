using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class InputServiceCategories
    {
        [JsonProperty("role_id")]
        public int RoleId { get; set; }

        [JsonProperty("parent_id")]
        public int ParentId { get; set; }

        [JsonProperty("providerGroupId")]
        public int ProviderGroupId { get; set; }
    }
}