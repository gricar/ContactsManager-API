using Contacts37.Application.Contracts.Persistence;
using Contacts37.Domain.Specifications;
using FluentValidation;

namespace Contacts37.Application.Usecases.Contacts.Commands.Delete
{
    public class DeleteContactCommandValidator : AbstractValidator<DeleteContactCommand>
    {
        private readonly IContactRepository _contactRepository;
        private readonly IRegionValidator _regionValidator;

        public DeleteContactCommandValidator(IContactRepository contactRepository,
            IRegionValidator regionValidator)
        {
            _contactRepository = contactRepository;
            _regionValidator = regionValidator;

            // ToDo: Validar IfExists aqui?
		}
    }
}
