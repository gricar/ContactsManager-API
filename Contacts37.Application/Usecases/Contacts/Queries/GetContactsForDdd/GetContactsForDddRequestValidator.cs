using Contacts37.Application.Usecases.Contacts.Queries.GetAll;
using FluentValidation;

public class GetContactsForDddRequestValidator : AbstractValidator<GetContactsForDddRequest>
{
    public GetContactsForDddRequestValidator()
    {
        RuleFor(c => c.DddCode)
            .NotEmpty().WithMessage("DDD code is required")
            .InclusiveBetween(10, 99)
            .WithMessage("DDDCode must be a valid 2 numeric digits.");
    }
}
