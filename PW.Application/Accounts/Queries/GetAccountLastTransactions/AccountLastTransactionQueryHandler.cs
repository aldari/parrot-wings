using MediatR;
using Microsoft.EntityFrameworkCore;
using PW.Persistence;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace PW.Application.Accounts.Queries.GetAccountLastTransactions
{
    public class AccountLastTransactionQueryHandler : IRequestHandler<AccountLastTransactionQuery, List<LastTransactionDto>>
    {
        private readonly ApplicationDbContext _context;

        public AccountLastTransactionQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<LastTransactionDto>> Handle(AccountLastTransactionQuery request, CancellationToken cancellationToken)
        {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.Parameters.Add(new SqlParameter("@account", SqlDbType.UniqueIdentifier) { Value = request.AccountId });
                command.CommandText = @"SELECT TOP 10
                    CASE WHEN [DebitAccountId]=@account
                    THEN [Amount]
                    ELSE -1*[Amount]
                    END AS qty,
                    CASE WHEN [DebitAccountId]=@account
                    THEN [CreditAccountId]
                    ELSE [DebitAccountId]
                    END AS account,
                    CASE WHEN [DebitAccountId]=@account
                    THEN (Select [Name] FROM [dbo].[Accounts] where Id =[CreditAccountId])
                    ELSE (Select [Name] FROM [dbo].[Accounts] where Id =[DebitAccountId])
                    END AS accountName,
                    [TransactionDate],
                    SUM (CASE WHEN [DebitAccountId]=@account
                    THEN [Amount]
                    ELSE -1*[Amount]
                    END) OVER (ORDER BY ID ) AccumulateSum,
                    Id
                    FROM [dbo].[AccountTransactions]
                        WHERE ( [DebitAccountId]=@account OR [CreditAccountId]=@account )
                    Order by [dbo].[AccountTransactions].TransactionDate desc";
                _context.Database.OpenConnection();
                DbDataReader reader = await command.ExecuteReaderAsync();

                var list = new List<LastTransactionDto>();
                while (await reader.ReadAsync())
                {
                    list.Add(new LastTransactionDto
                    {
                        Amount = reader.GetInt32(0),
                        AccountId = reader.GetGuid(1),
                        AccountName = reader.GetString(2),
                        TransactionDate = reader.GetDateTime(3).MarkUnspecifiedDateAsUtc(),
                        AccumulateSum = reader.GetInt32(4),
                        Id = reader.GetInt32(5)
                    });
                }
                return list;
            }
        }
    }
}
