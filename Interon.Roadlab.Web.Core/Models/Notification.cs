using System;
using NPoco;

namespace Interon.Roadlab.Web.App.Models
{
    [NPoco.TableName("Notifications")]
    [PrimaryKey("Key", AutoIncrement = false)]
    public class Notification:Syncable
    {

        [Column("Key")]
        public Guid Key { get; set; }
        public int MemberId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Link { get; set; }
        public string Data { get; set; }
        public DateTime? ReadDate { get; set; }
        public string NotificationType { get; set; }
        public string PushOutcome { get; set; }
        public Guid ForeignKey { get; set; }
    }
}