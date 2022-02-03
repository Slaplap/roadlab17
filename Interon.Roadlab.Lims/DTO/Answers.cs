using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class Answers
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("jobId")]
        public string JobId { get; set; }

        [JsonProperty("formVersionId")]
        public string FormVersionId { get; set; }

        [JsonProperty("sectionAnswerSheets")]
        public List<SectionAnswerSheet> SectionAnswerSheets { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("deletedAt")]
        public object DeletedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }
}