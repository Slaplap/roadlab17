using System.Collections.Generic;
using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class Group
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("short_name")]
        public string ShortName { get; set; }

        [JsonProperty("roles")]
        public List<Role> Roles { get; set; }

        [JsonProperty("default_role_id")]
        public string DefaultRoleId { get; set; }

        [JsonProperty("status_id")]
        public int StatusId { get; set; }

        [JsonProperty("open_status")]
        public object OpenStatus { get; set; }

        [JsonProperty("current_location")]
        public CurrentLocation CurrentLocation { get; set; }

        [JsonProperty("store_product_categories")]
        public string StoreProductCategories { get; set; }

        [JsonProperty("logo_url")]
        public string LogoUrl { get; set; }

        [JsonProperty("banner_url")]
        public string BannerUrl { get; set; }

        [JsonProperty("category_id")]
        public object CategoryId { get; set; }

        [JsonProperty("description_html")]
        public string DescriptionHtml { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("contact_number")]
        public string ContactNumber { get; set; }

        [JsonProperty("website_url")]
        public string WebsiteUrl { get; set; }

        [JsonProperty("distance")]
        public int? Distance { get; set; }

        [JsonProperty("user_properties")]
        public List<object> UserProperties { get; set; }
    }
}
