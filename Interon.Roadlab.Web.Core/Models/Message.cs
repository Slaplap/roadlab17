using System;
using NPoco;

namespace Interon.Roadlab.Web.Core.Models
{
    [NPoco.TableName("Messages")]
    [PrimaryKey("Key", AutoIncrement = false)]
    public class Message:Syncable
    {

        [Column("Key")]
        public Guid Key { get; set; }
        public int MemberId { get; set; }
        public string Payload { get; set; }
        public string MessageType { get; set; }
       
        
    }
}