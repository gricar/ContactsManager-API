using Contacts37.Application.Common.Exceptions;
using Contacts37.Application.Contracts.Persistence;
using MediatR;

namespace Contacts37.Application.Usecases.Contacts.Commands.Delete
{
	public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand, Unit>
    {
        private readonly IContactRepository _contactRepository;

        public DeleteContactCommandHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
        }

        public async Task<Unit> Handle(DeleteContactCommand command, CancellationToken cancellationToken)
		{
            var contact = await _contactRepository.GetAsync(command.Id);

            if (contact == null)
				throw new ContactNotFoundException(command.Id);

			await _contactRepository.DeleteAsync(contact);

            return Unit.Value;
        }
	}
}
