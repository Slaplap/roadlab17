using System;
using Newtonsoft.Json;

namespace Interon.Roadlab.Core.Dto
{
    public class MessageJsonDto
    {
        [JsonProperty("key")]
        public Guid Key { get; set; }

        [JsonProperty("memberId")]
        public int MemberId { get; set; }
        
        [JsonProperty("payload")]
        public string Payload { get; set; }

        [JsonProperty("messageType")]
        public string MessageType { get; set; }
 
        [JsonProperty("createDate")]
        public DateTime CreateDate { get; set; }

        [JsonProperty("updateDate")]
        public DateTime UpdateDate { get; set; }
        [JsonProperty("syncDate")]
        public DateTime SyncDate { get; set; }

     
    }
}