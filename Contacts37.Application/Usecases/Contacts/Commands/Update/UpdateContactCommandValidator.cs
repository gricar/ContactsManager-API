using Contacts37.Application.Contracts.Persistence;
using Contacts37.Domain.Specifications;
using FluentValidation;

namespace Contacts37.Application.Usecases.Contacts.Commands.Update
{
    public class UpdateContactCommandValidator : AbstractValidator<UpdateContactCommand>
    {
        private readonly IContactRepository _contactRepository;
        private readonly IRegionValidator _regionValidator;

        public UpdateContactCommandValidator(IContactRepository contactRepository,
            IRegionValidator regionValidator)
        {
            _contactRepository = contactRepository;
            _regionValidator = regionValidator;

            RuleFor(c => c.Id)
                .NotEmpty();

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(c => c.DDDCode)
                .NotEmpty().WithMessage("DDD code is required")
                .Must(code => code >= 10 && code <= 99)
                .WithMessage("DDDCode must be a valid 2 numeric digits.")
                .Must(BeValidRegion)
                .WithMessage("DDDCode does not exist in any registered region.");

            RuleFor(c => c.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\d{9}$").WithMessage("Phone number must be 9 numeric digits.");

            RuleFor(c => c.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email)).WithMessage("Email must be a valid format.");
        }

        private bool BeValidRegion(int dddCode) => _regionValidator.IsValid(dddCode);

    }
}