using SQLite;

namespace Interon.Roadlab.App.Core.Models
{
    public class Client
    {
        [PrimaryKey] 
        public string AccountNumber { get; set; }
       
        public string Name { get; set; }
        public int Id { get; set; }
    }
}