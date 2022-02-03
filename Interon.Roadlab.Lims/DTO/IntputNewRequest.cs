using System.Collections.Generic;
using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class InputNewRequest
    {
        [JsonProperty("companyName")]
        public string CompanyName { get; set; }

        [JsonProperty("contactName")]
        public string ContactName { get; set; }

        [JsonProperty("contactSurname")]
        public string ContactSurname { get; set; }

        [JsonProperty("contactEmail")]
        public string ContactEmail { get; set; }

        [JsonProperty("siteAddress")]
        public int SiteAddress { get; set; }

        [JsonProperty("requestDetails")]
        public string RequestDetails { get; set; }

        [JsonProperty("contact")]
        public int Contact { get; set; }

        [JsonProperty("branch")]
        public int Branch { get; set; }

        [JsonProperty("next")]
        public string Next { get; set; }

        [JsonProperty("services")]
        public List<InputNewRequestService> Services { get; set; }
    }
    public class InputNewRequestService
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("requested_date")]
        public string RequestedDate { get; set; }
    }
}