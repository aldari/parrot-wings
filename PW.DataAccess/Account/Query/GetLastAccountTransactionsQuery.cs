using Microsoft.EntityFrameworkCore;
using PW.Core.Account.Dto;
using PW.Core.Account.Query;
using PW.DataAccess.ApplicationData;
using PW.DataAccess.Cqs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PW.DataAccess.Account.Query
{
    public class GetLastAccountTransactionsQuery : EfQueryBase<ApplicationDbContext>, IGetLastAccountTransactionsQuery
    {
        public GetLastAccountTransactionsQuery(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<List<LastTransactionDto>> Execute(Guid accountId)
        {
            using (var command = DbContext.Database.GetDbConnection().CreateCommand())
            {
                command.Parameters.Add(new SqlParameter("@account", SqlDbType.UniqueIdentifier) { Value = accountId });
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
                DbContext.Database.OpenConnection();
                DbDataReader reader = command.ExecuteReader();

                var list = new List<LastTransactionDto>();
                while (reader.Read())
                {
                    list.Add(new LastTransactionDto
                    {
                        Amount = reader.GetInt32(0),
                        AccountId = reader.GetGuid(1),
                        AccountName = reader.GetString(2),
                        TransactionDate = reader.GetDateTime(3),
                        AccumulateSum = reader.GetInt32(4),
                        Id = reader.GetInt32(5)
                    });
                }
                return list;
            }
        }
    }
}
