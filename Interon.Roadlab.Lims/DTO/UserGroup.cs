using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class UserGroup
    {
        [JsonProperty("groupID")]
        public string GroupID { get; set; }

        [JsonProperty("roleID")]
        public string RoleID { get; set; }

        [JsonProperty("userID")]
        public string UserID { get; set; }

        [JsonProperty("locationPrivacyId")]
        public string LocationPrivacyId { get; set; }
    }
}
