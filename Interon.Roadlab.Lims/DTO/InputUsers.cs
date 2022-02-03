using System;
using System.Text;
using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Users
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("privacy")]
        public int Privacy { get; set; }

        [JsonProperty("mobile_phone")]
        public string MobilePhone { get; set; }

        [JsonProperty("external_id")]
        public string ExternalId { get; set; }
    }

    public class UGB
    {
        [JsonProperty("group_id")]
        public int GroupId { get; set; }

        [JsonProperty("membership_type")]
        public int MembershipType { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }
    }

    public class CCB
    {
        [JsonProperty("client_id")]
        public int ClientId { get; set; }
    }

    public class InputUsers
    {
        [JsonProperty("Users")]
        public Users Users { get; set; }

        [JsonProperty("UGB")]
        public UGB UGB { get; set; }

        [JsonProperty("CCB")]
        public CCB CCB { get; set; }
    }


}
