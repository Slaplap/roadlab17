using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class SectionAnswerSheet
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("item")]
        public object Item { get; set; }

        [JsonProperty("unit")]
        public object Unit { get; set; }

        [JsonProperty("itemId")]
        public object ItemId { get; set; }

        [JsonProperty("unitId")]
        public object UnitId { get; set; }

        [JsonProperty("location")]
        public object Location { get; set; }

        [JsonProperty("sectionId")]
        public string SectionId { get; set; }

        [JsonProperty("locationId")]
        public object LocationId { get; set; }

        [JsonProperty("questionAnswers")]
        public List<object> QuestionAnswers { get; set; }

        [JsonProperty("formVersionAnswerSheetId")]
        public string FormVersionAnswerSheetId { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("deletedAt")]
        public object DeletedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }
}