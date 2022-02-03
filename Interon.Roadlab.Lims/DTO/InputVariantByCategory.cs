using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class InputVariantByCategory
    {
        [JsonProperty("category_id")] 
        public int CategoryId { get; set; } = 144219; // see Service categories to get a category id to pass in here.

        [JsonProperty("offset_id")]
        public object OffsetId { get; set; }

        [JsonProperty("page_size")]
        public object PageSize { get; set; }

        [JsonProperty("providerGroupId")] 
        public int ProviderGroupId { get; set; } = 506;

        [JsonProperty("role_id")]
        public object RoleId { get; set; }

        [JsonProperty("search")]
        public object Search { get; set; }

        [JsonProperty("sort_field")]
        public object SortField { get; set; }

        [JsonProperty("sort_order")]
        public object SortOrder { get; set; }
    }
}
