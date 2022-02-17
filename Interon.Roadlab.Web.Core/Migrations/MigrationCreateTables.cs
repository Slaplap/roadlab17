using Interon.Roadlab.Web.Core.Models;
using Umbraco.Core.Migrations;

namespace Interon.Roadlab.Web.Core.Migrations
{
    public class MigrationCreateTables : MigrationBase
    {
        public MigrationCreateTables(IMigrationContext context)
            : base(context)
        { }

        public override void Migrate()
        {
            if (!TableExists("TransactionFiles"))
                Create.Table<TransactionFile>().Do();
            if (!TableExists("TransactionLines"))
                Create.Table<TransactionLine>().Do();
            if (!TableExists("Transactions"))
                Create.Table<Transaction>().Do();
           
            if (!TableExists("Notifications"))
                Create.Table<Notification>().Do();
            if (!TableExists("Messages"))
                Create.Table<Message>().Do();
        }
      

    }

   
    public class MigrationCreateTables2 : MigrationBase
    {
        public MigrationCreateTables2(IMigrationContext context)
            : base(context)
        { }

        public override void Migrate()
        {
            if (!TableExists("Transactions"))
                Create.Table<Transaction>().Do();
            if (!TableExists("TransactionFiles"))
                Create.Table<TransactionFile>().Do();
            if (!TableExists("TransactionLines"))
                Create.Table<TransactionLine>().Do();
            if (!TableExists("Notifications"))
                Create.Table<Notification>().Do();
        }


    }
}