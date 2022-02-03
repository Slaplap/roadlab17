using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Core.Migrations;
using Umbraco.Core.Migrations.Upgrade;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;

namespace Interon.Roadlab.Web.App.Migrations
{
    public class MigrationComponent : IComponent
    {
        private readonly IScopeProvider scopeProvider;
        private readonly IMigrationBuilder migrationBuilder;
        private readonly IKeyValueService keyValueService;
        private readonly ILogger logger;

        public MigrationComponent(
            IScopeProvider scopeProvider,
            IMigrationBuilder migrationBuilder,
            IKeyValueService keyValueService,
            ILogger logger)
        {
            this.scopeProvider = scopeProvider;
            this.migrationBuilder = migrationBuilder;
            this.keyValueService = keyValueService;
            this.logger = logger;
        }

        public void Initialize()
        {
            // perform any upgrades (as needed)
            var upgrader = new Upgrader(new MyMigrationPlan());
            upgrader.Execute(scopeProvider, migrationBuilder, keyValueService, logger);

        }

        public void Terminate()
        {
           
        }
    }
}