using PW.Core.Cqs;
using Microsoft.EntityFrameworkCore;

namespace PW.DataAccess.Cqs
{
    public abstract class EfQueryBase<TContext> : IQuery
        where TContext : DbContext
    {
        protected readonly TContext DbContext;

        protected EfQueryBase(TContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
