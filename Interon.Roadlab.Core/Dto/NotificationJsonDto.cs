using System;
using Newtonsoft.Json;

namespace Interon.Roadlab.Core.Models
{
    public class NotificationJsonDto

    {
        [JsonProperty("key")]
        public Guid Key { get; set; }

        [JsonProperty("memberId")]
        public int MemberId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("createDate")]
        public DateTime CreateDate { get; set; }

        [JsonProperty("readDate")]
        public DateTime? ReadDate { get; set; }
        [JsonProperty("syncDate")]
        public DateTime SyncDate { get; set; }

        [JsonProperty("notificationType")]
        public string NotificationType { get; set; }

        public string PushOutcome { get; set; }
        public DateTime UpdateDate { get; set; }
        [JsonProperty("foreignKey")]
        public Guid ForeignKey { get; set; }
    }
}