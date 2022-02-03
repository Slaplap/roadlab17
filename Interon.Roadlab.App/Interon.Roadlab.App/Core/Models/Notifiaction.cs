using System;
using SQLite;

namespace Interon.Roadlab.App.Core.Models
{
    [Table("Notifications")]
    public class Notification
    {
        [Column("Key")]
        [PrimaryKey]
        [NotNull]
        public Guid Key { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ReadDate { get; set; }
        public DateTime SyncDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Data { get; set; }
        public string Link { get; set; }
        public int MemberId { get; set; }
        public string NotifiacionType { get; set; }
        public string PushOutcome { get; set; }
        public Guid ForeignKey { get; set; }

        [Ignore]
        public bool ShowLink
        {
            get => !string.IsNullOrWhiteSpace(Link);
        }  
    }
}
