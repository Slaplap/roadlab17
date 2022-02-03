using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Interon.Roadlab.Web.Migrations;

namespace Interon.Roadlab.Web.App.Models
{
    public class RoadlabContext : DbContext
    {
        public RoadlabContext() : base()
        {

        }

        public DbSet<Company> Companies { get; set; }
      
    }
}