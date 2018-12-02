using System;
using System.Threading.Tasks;

namespace PW.Core.Cqs
{
	public interface ICommandHandler<in TCommand> : IDisposable
		where TCommand : ICommand
	{
		Task<CommandResult> ExecuteAsync(TCommand command);
	}
}