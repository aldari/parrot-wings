﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PW.Application.Accounts.Commands.AddTransaction
{
    public class AddTransactionCommandValidator : AbstractValidator<AddTransactionCommand>
    {
        public AddTransactionCommandValidator()
        {
            RuleFor(x => x.Amount).NotEmpty().GreaterThan(0);
            RuleFor(x => x.CreditAccount).NotEmpty();
            RuleFor(x => x.DebitAccount).NotEmpty();
        }
    }
}
