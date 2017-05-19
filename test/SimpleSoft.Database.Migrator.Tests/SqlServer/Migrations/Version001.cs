using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Database.Migrator.Tests.SqlServer.Migrations
{
    public class Version001 : Migration<ApplyMigrationsContext>
    {
        public Version001(ApplyMigrationsContext context) : base(context)
        {
            RunInTransaction = false;
        }

        #region Overrides of Migration<ApplyMigrationsContext>

        /// <inheritdoc />
        public override async Task ApplyAsync(CancellationToken ct)
        {
            await Context.ExecuteAsync(@"
create table Version001Table(
	Id bigint primary key,
	Value nvarchar(256) not null
)");
        }

        #endregion
    }
}