using FluentValidation;

namespace Contacts37.Application.Usecases.Contacts.Commands.CreateContact
{
    public class CreateContactCommandValidator : AbstractValidator<CreateContactCommand>
    {
        public CreateContactCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty();

            RuleFor(c => c.DDDCode)
                .NotEmpty()
                .Length(2)
                .WithMessage("DddCode must be 2 numeric digits.");

            RuleFor(c => c.Phone)
                .NotEmpty()
                .Length(9)
                .WithMessage("Phone must be 9 numeric digits.");

            RuleFor(c => c.Email)
                .EmailAddress();
        }
    }
}
