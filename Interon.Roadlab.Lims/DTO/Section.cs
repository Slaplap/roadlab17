using System.Collections.Generic;
using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class Section
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("publicId")]
        public string PublicId { get; set; }

        [JsonProperty("questions")]
        public List<Question> Questions { get; set; }

        [JsonProperty("isRequired")]
        public bool IsRequired { get; set; }

        [JsonProperty("configuration")]
        public Configuration Configuration { get; set; }

        [JsonProperty("formVersionId")]
        public string FormVersionId { get; set; }

        [JsonProperty("isDefaultSection")]
        public bool? IsDefaultSection { get; set; }
    }
}