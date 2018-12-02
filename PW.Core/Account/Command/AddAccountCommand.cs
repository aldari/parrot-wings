using System;
using PW.Core.Cqs;

namespace PW.Core.Account.Command
{
    public class AddAccountCommand: ICommand
    {
        public Guid UserId { get; }
        public string Name { get; }

        public AddAccountCommand(Guid userId, string name)
        {
            UserId = userId;
            Name = name;
        }
    }
}
