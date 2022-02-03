using Newtonsoft.Json;

namespace Interon.Roadlab.LIMS.DTO
{
    public class PastelOrder
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("description")]
        public object Description { get; set; }

        [JsonProperty("user_id")]
        public object UserId { get; set; }

        [JsonProperty("event_id")]
        public object EventId { get; set; }

        [JsonProperty("type_id")]
        public int TypeId { get; set; }

        [JsonProperty("model_id")]
        public object ModelId { get; set; }

        [JsonProperty("thumb_url")]
        public object ThumbUrl { get; set; }

        [JsonProperty("favourite")]
        public object Favourite { get; set; }

        [JsonProperty("date_created")]
        public int DateCreated { get; set; }

        [JsonProperty("date_updated")]
        public int DateUpdated { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("enabled")]
        public int Enabled { get; set; }

        [JsonProperty("impression_count")]
        public int ImpressionCount { get; set; }

        [JsonProperty("parent_system_type_id")]
        public object ParentSystemTypeId { get; set; }

        [JsonProperty("author_id")]
        public int AuthorId { get; set; }
    }
}