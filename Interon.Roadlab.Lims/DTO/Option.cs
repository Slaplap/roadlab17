using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class Option
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("questionId")]
        public string QuestionId { get; set; }
    }
}