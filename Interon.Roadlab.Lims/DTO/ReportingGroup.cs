using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class ReportingGroup
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("header_img_url")]
        public object HeaderImgUrl { get; set; }

        [JsonProperty("icon_img_url")]
        public object IconImgUrl { get; set; }

        [JsonProperty("calendar_id")]
        public object CalendarId { get; set; }

        [JsonProperty("parent_id")]
        public object ParentId { get; set; }

        [JsonProperty("short_name")]
        public string ShortName { get; set; }

        [JsonProperty("private")]
        public int Private { get; set; }

        [JsonProperty("visibility")]
        public int Visibility { get; set; }

        [JsonProperty("website_url")]
        public string WebsiteUrl { get; set; }

        [JsonProperty("logo_url")]
        public string LogoUrl { get; set; }

        [JsonProperty("banner_url")]
        public string BannerUrl { get; set; }

        [JsonProperty("type_id")]
        public int TypeId { get; set; }

        [JsonProperty("administrative_user_count")]
        public object AdministrativeUserCount { get; set; }

        [JsonProperty("other_user_count")]
        public object OtherUserCount { get; set; }

        [JsonProperty("group_code")]
        public string GroupCode { get; set; }

        [JsonProperty("wizard_step")]
        public object WizardStep { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("status_id")]
        public int StatusId { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("industry_id")]
        public object IndustryId { get; set; }

        [JsonProperty("service_admin_user_id")]
        public int ServiceAdminUserId { get; set; }

        [JsonProperty("open")]
        public object Open { get; set; }

        [JsonProperty("current_location_id")]
        public int CurrentLocationId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("contact_number")]
        public string ContactNumber { get; set; }

        [JsonProperty("business_registration_number")]
        public string BusinessRegistrationNumber { get; set; }
    }
}