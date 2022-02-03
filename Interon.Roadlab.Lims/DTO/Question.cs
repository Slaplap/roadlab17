using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class Question
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("options")]
        public List<Option> Options { get; set; }

        [JsonProperty("publicId")]
        public string PublicId { get; set; }

        [JsonProperty("sectionId")]
        public string SectionId { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("configuration")]
        public Configuration Configuration { get; set; }

        [JsonProperty("displayLogicConfiguration")]
        public object DisplayLogicConfiguration { get; set; }

        [JsonProperty("displayLogicConditionalQuestionId")]
        public object DisplayLogicConditionalQuestionId { get; set; }

        [JsonProperty("formId")]
        public string FormId { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("sections")]
        public List<Section> Sections { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("deletedAt")]
        public object DeletedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("isPublished")]
        public bool IsPublished { get; set; }
    }
}