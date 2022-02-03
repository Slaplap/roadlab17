using System.Collections.Generic;
using Interon.Roadlab.LIMS.DTO.Quicktype;
using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    // GroupsUsersCategories myDeserializedClass = JsonConvert.DeserializeObject<GroupsUsersCategories>(myJsonResponse); 
    public class GroupsUsersCategories
    {
        [JsonProperty("groups")]
        public List<Group> Groups { get; set; }

        [JsonProperty("categories")]
        public List<Category> Categories { get; set; }

        [JsonProperty("userGroups")]
        public List<UserGroup> UserGroups { get; set; }
    }
}
