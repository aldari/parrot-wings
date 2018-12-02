using Microsoft.EntityFrameworkCore;
using PW.Core.Account.Query;
using PW.DataAccess.ApplicationData;
using PW.DataAccess.Cqs;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PW.DataAccess.Account.Query
{
    public class GetAccountBalanceQuery : EfQueryBase<ApplicationDbContext>, IGetAccountBalanceQuery
    {
        public GetAccountBalanceQuery(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<int> Execute(Guid accountId)
        {
            using (var command = DbContext.Database.GetDbConnection().CreateCommand())
            {
                command.Parameters.Add(new SqlParameter("@account", SqlDbType.UniqueIdentifier) { Value = accountId });
                command.CommandText = @"SELECT
                SUM(t.qty) AS balance
                FROM(
                    SELECT CASE WHEN[DebitAccountId] = @account
                THEN[Amount]
                ELSE - 1 *[Amount]
                END AS qty
                FROM [AccountTransactions]
                WHERE( [DebitAccountId] = @account OR[CreditAccountId] = @account)
                ) as t";
                DbContext.Database.OpenConnection();
                var result = (int)await command.ExecuteScalarAsync();
                return result;
            }
        }
    }
}
