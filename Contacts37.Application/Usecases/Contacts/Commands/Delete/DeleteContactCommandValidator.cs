using Contacts37.Application.Contracts.Persistence;
using Contacts37.Domain.Specifications;
using FluentValidation;

namespace Contacts37.Application.Usecases.Contacts.Commands.Delete
{
    public class DeleteContactCommandValidator : AbstractValidator<DeleteContactCommand>
    {
        private readonly IContactRepository _contactRepository;
        public DeleteContactCommandValidator(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Id is required")
                .MustAsync(BeValidId).WithMessage("Id not found.")
                ;
        }

        private async Task<bool> BeValidId(Guid id, CancellationToken cancellationToken)
        {
            return await _contactRepository.ExistsAsync(id);
        }
    }
}