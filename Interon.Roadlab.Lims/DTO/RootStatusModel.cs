using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class RootStatusModel
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("model_id")]
        public int ModelId { get; set; }
    }
}