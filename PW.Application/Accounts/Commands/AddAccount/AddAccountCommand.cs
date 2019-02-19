using MediatR;
using System;

namespace PW.Application.Accounts.Commands.AddAccount
{
    public class AddAccountCommand : IRequest
    {
        public Guid UserId { get; set;  }

        public string Name { get; set; }
    }
}