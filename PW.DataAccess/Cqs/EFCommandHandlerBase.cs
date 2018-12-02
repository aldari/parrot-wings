using PW.Core.Cqs;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace PW.DataAccess.Cqs
{
	public abstract class EfCommandHandlerBase<TCommand, TContext> : ICommandHandler<TCommand>
		where TCommand : ICommand
		where TContext : DbContext
	{
		protected readonly TContext DbContext;

		protected EfCommandHandlerBase(TContext dbContext)
		{
			DbContext = dbContext;
		}

		public abstract Task<CommandResult> ExecuteAsync(TCommand command);

		public void Dispose()
		{
			DbContext.Dispose();
		}
	}
}