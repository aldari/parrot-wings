using FluentValidation;

namespace PW.Application.Accounts.Commands.AddAccount
{
    public class AddAccountCommandValidator : AbstractValidator<AddAccountCommand>
    {
        public AddAccountCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Name).MaximumLength(30).NotEmpty();
        }
    }
}
