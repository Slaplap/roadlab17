using System.Collections.Generic;
using SQLite;

namespace Interon.Roadlab.App.Core.Models
{
    public class Member

    {
        [PrimaryKey] public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CellNo { get; set; }
        public string Email { get; set; }
        public string Clients { get; set; }

        public string Fullname
        {
            get { return Name + " " + Surname; } 
           
        }



    }
}