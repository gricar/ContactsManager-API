using FluentValidation;

namespace Contacts37.Application.Usecases.Contacts.Queries.GetByDdd
{
    public class GetContactsByDddRequestValidator : AbstractValidator<GetContactsByDddRequest>
    {
        public GetContactsByDddRequestValidator()
        {
            RuleFor(c => c.DddCode)
                .NotEmpty().WithMessage("DDD code is required")
                .InclusiveBetween(10, 99)
                .WithMessage("DDDCode must be a valid 2 numeric digits.");
        }
    }
}
