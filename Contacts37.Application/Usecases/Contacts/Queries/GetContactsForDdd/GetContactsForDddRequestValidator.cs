using Contacts37.Application.Usecases.Contacts.Queries.GetAll;
using FluentValidation;

public class GetContactsForDddRequestValidator : AbstractValidator<GetContactsForDddRequest>
{
    public GetContactsForDddRequestValidator()
    {
        RuleFor(c => c.DddCode)
            .NotEmpty().WithMessage("DDD code is required")
            .Must(code => code >= 10 && code <= 99)
            .WithMessage("DDDCode must be a valid 2 numeric digits.")
            .Must(BeValidRegion)
            .WithMessage("DDDCode does not exist in any registered region.");
    }

    private bool BeValidRegion(int dddCode)
    {
        // Verifica se o código DDD existe na lista de regiões válidas
        var validRegions = new List<int> { 11, 21, 31, 41 }; // Exemplo de códigos válidos
        return validRegions.Contains(dddCode);
    }
}
