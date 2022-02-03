using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class FormSheetIds
    {
        [JsonProperty("form_id")]
        public string FormId { get; set; }

        [JsonProperty("answer_sheet_id")]
        public string AnswerSheetId { get; set; }

        [JsonProperty("form_version_id")]
        public string FormVersionId { get; set; }
    }
}