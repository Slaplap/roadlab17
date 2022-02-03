using System.Collections.Generic;
using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class RootContacts
    {
        [JsonProperty("items")]
        public List<Contact> ContactsList { get; set; }
    }

}