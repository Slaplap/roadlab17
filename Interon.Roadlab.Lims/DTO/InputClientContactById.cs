using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class InputByClientIdOrContactIdOrSiteId
    {
        [JsonProperty("client_id")]
        public int ClientId { get; set; }

        [JsonProperty("contact_id")]
        public int ContactId { get; set; }

        [JsonProperty("site_id")]
        public int SiteId { get; set; }
    }
}