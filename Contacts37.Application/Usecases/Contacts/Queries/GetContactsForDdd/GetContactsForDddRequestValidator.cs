using Contacts37.Application.Contracts.Persistence;
using Contacts37.Application.Usecases.Contacts.Queries.GetAll;
using Contacts37.Domain.Specifications;
using FluentValidation;

public class GetContactsForDddRequestValidator : AbstractValidator<GetContactsForDddRequest>
{
    private readonly IRegionValidator _regionValidator;

    public GetContactsForDddRequestValidator(
            IRegionValidator regionValidator)
    {
        _regionValidator = regionValidator;

        RuleFor(c => c.DddCode)
            .NotEmpty().WithMessage("DDD code is required")
            .Must(code => code >= 10 && code <= 99)
            .WithMessage("DDDCode must be a valid 2 numeric digits.")
            .Must(BeValidRegion)
            .WithMessage("DDDCode does not exist in any registered region.");
    }

    private bool BeValidRegion(int dddCode) => _regionValidator.IsValid(dddCode);
}
