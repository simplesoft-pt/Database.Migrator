using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Database.Migrator.Tests.SqlServer.Migrations
{
    public class Version003 : Migration<ApplyMigrationsContext>
    {
        public Version003(ApplyMigrationsContext context) : base(context)
        {
            RunInTransaction = false;
        }

        #region Overrides of Migration<ApplyMigrationsContext>

        /// <inheritdoc />
        public override async Task ApplyAsync(CancellationToken ct)
        {
            await Context.ExecuteAsync(@"
alter table Version001Table 
add Description nvarchar(1024) not null default 'hello world'");
        }

        #endregion
    }
}