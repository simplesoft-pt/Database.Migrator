using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace SimpleSoft.Database.Migrator
{
    public partial class RelationalMigrationContext
    {
        /// <inheritdoc />
        public async Task<int> ExecuteAsync(
            string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            int timeout;
            AssertCommandParameters(commandTimeout, out timeout);

            LogQuery(sql, Transaction, timeout);

            return await Connection.ExecuteAsync(sql, param, Transaction, timeout, commandType);
        }
    }
}