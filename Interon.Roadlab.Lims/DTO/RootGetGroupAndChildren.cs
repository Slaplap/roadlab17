using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interon.Roadlab.LIMS.DTO
{
    
    

    public class RootGetGroupAndChildren
    {
        [JsonProperty("groups")]
        public List<Group> Groups { get; set; }

        [JsonProperty("categories")]
        public List<Category> Categories { get; set; }

        [JsonProperty("userGroups")]
        public List<UserGroup> UserGroups { get; set; }
    }


}
