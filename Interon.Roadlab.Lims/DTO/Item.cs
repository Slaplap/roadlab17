using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class Item
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("roleID")]
        public int RoleID { get; set; }

        [JsonProperty("admin")]
        public int Admin { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("private")]
        public int Private { get; set; }

        [JsonProperty("membership_id")]
        public int MembershipId { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }
    }
}