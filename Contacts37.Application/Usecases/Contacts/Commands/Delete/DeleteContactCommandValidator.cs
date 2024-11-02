using Contacts37.Application.Contracts.Persistence;
using FluentValidation;

namespace Contacts37.Application.Usecases.Contacts.Commands.Delete
{
    public class DeleteContactCommandValidator : AbstractValidator<DeleteContactCommand>
    {
        public DeleteContactCommandValidator(IContactRepository contactRepository)
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Id is required");
        }
    }
}