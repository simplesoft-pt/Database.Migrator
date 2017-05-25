using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Database.Migrator.Tests.Relational.SqlServer.Migrations
{
    public class Version002 : Migration<ApplyMigrationsContext>
    {
        public Version002(ApplyMigrationsContext context) : base(context)
        {

        }

        #region Overrides of Migration<ApplyMigrationsContext>

        /// <inheritdoc />
        public override async Task ApplyAsync(CancellationToken ct)
        {
            for (var i = 0; i < 15; i++)
            {
                await Context.ExecuteAsync(@"
insert into Version001Table(Id, Value) 
values (@id, @value)", new
                {
                    id = i,
                    value = "Some value " + i.ToString("D3")
                });
            }
        }

        #endregion
    }
}