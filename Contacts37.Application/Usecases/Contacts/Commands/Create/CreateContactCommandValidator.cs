using Contacts37.Application.Contracts.Persistence;
using FluentValidation;

namespace Contacts37.Application.Usecases.Contacts.Commands.Create
{
    public class CreateContactCommandValidator : AbstractValidator<CreateContactCommand>
    {
        private readonly IContactRepository _contactRepository;

        public CreateContactCommandValidator(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(c => c.DDDCode)
                .NotEmpty().WithMessage("DDD code is required")
                .Matches(@"^\d{2}$").WithMessage("DddCode must be 2 numeric digits.");

            RuleFor(c => c.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\d{9}$").WithMessage("Phone number must be 9 numeric digits.");

            RuleFor(c => c.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email)).WithMessage("Email must be a valid format.")
                .MustAsync(BeUniqueEmail).When(x => !string.IsNullOrEmpty(x.Email)).WithMessage("Email already registered.");
        }

        private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        {
            return await _contactRepository.IsEmailUniqueAsync(email);
        }
    }
}
