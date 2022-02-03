using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Mobile.Server;

namespace Interon.Roadlab.Web.App.Models
{
    public class Member : EntityData
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CellNo { get; set; }
        public string Email { get; set; }
        public string CompanyId { get; set; }
      
      
    }
}