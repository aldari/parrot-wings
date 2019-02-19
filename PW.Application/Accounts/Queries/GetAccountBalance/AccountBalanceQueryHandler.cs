using MediatR;
using Microsoft.EntityFrameworkCore;
using PW.Persistence;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace PW.Application.Accounts.Queries.GetAccountBalance
{
    public class AccountBalanceQueryHandler: IRequestHandler<AccountBalanceQuery, int>
    {
        private readonly ApplicationDbContext _context;

        AccountBalanceQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(AccountBalanceQuery request, CancellationToken cancellationToken)
        {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.Parameters.Add(new SqlParameter("@account", SqlDbType.UniqueIdentifier) { Value = request.AccountId });
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
                _context.Database.OpenConnection();
                var result = (int)await command.ExecuteScalarAsync();
                return result;
            }
        }
    }
}
