using System.Collections.Generic;
using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class Service
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("tests")]
        public List<int> Tests { get; set; }

        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("quantity")]
        public string Quantity { get; set; }

        [JsonProperty("sku_code")]
        public string SkuCode { get; set; }

        [JsonProperty("service_name")]
        public string ServiceName { get; set; }

        [JsonProperty("requested_date")]
        public int RequestedDate { get; set; }

        [JsonProperty("service_variant_id")]
        public int ServiceVariantId { get; set; }

        [JsonProperty("store_item_listing_id")]
        public int StoreItemListingId { get; set; }
    }
}