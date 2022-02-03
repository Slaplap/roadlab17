using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class AnswerSheetEndpointResponse
    {
        [JsonProperty("answers")]
        public Answers Answers { get; set; }
    }
}