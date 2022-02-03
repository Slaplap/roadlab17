using SQLite;

namespace Interon.Roadlab.App.Core.Models
{
    public class Branch

    {
        [PrimaryKey] public int Id { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }

        public string Code { get; set; }
        public string CellphoneNumber { get; set; }
        public string TelephoneNumber { get; set; }
        public string ManagerName { get; set; }
        public string Email { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public double Distance { get; set; }
         

       
    }
}