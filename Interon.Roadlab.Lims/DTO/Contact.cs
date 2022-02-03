using System;
using System.Text;
using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class Contact
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("nick")]
        public object Nick { get; set; }

        [JsonProperty("birthdate")]
        public object Birthdate { get; set; }

        [JsonProperty("user_type")]
        public int UserType { get; set; }

        [JsonProperty("last_logon_date")]
        public int LastLogonDate { get; set; }

        [JsonProperty("vat_number")]
        public string VatNumber { get; set; }

        [JsonProperty("physical_address")]
        public string PhysicalAddress { get; set; }

        [JsonProperty("home_phone")]
        public string HomePhone { get; set; }

        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        [JsonProperty("mobile_phone")]
        public string MobilePhone { get; set; }

        [JsonProperty("privacy")]
        public int Privacy { get; set; }

        [JsonProperty("fb_id")]
        public object FbId { get; set; }

        [JsonProperty("last_known_lat")]
        public object LastKnownLat { get; set; }

        [JsonProperty("last_known_lon")]
        public object LastKnownLon { get; set; }

        [JsonProperty("last_know_location_date")]
        public object LastKnowLocationDate { get; set; }

        [JsonProperty("last_known_location_name_id")]
        public object LastKnownLocationNameId { get; set; }

        [JsonProperty("date_created")]
        public int DateCreated { get; set; }

        [JsonProperty("date_updated")]
        public int DateUpdated { get; set; }

        [JsonProperty("designation_id")]
        public object DesignationId { get; set; }

        [JsonProperty("reports_to_id")]
        public object ReportsToId { get; set; }

        [JsonProperty("bio")]
        public string Bio { get; set; }

        [JsonProperty("external_id")]
        public string ExternalId { get; set; }

        [JsonProperty("accepted_tos")]
        public int AcceptedTos { get; set; }

        [JsonProperty("accepted_eula")]
        public int AcceptedEula { get; set; }
    }
}
