using Umbraco.Core.Migrations;

namespace Interon.Roadlab.Web.Core.Migrations
{
    public class MyMigrationPlan : MigrationPlan
    {
        public MyMigrationPlan() : base("Interon.Roadlab.Web")
        {
             From(string.Empty).To<MigrationCreateTables>("first-migration");
             From("first-migration").To<MigrationCreateTables>("create-notification");
             From("create-notification").To<MigrationCreateTables>("create-createmessage");
        }
    }
}