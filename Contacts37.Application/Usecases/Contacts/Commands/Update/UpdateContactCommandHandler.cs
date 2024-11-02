using Contacts37.Application.Common.Exceptions;
using Contacts37.Application.Contracts.Persistence;
using Contacts37.Domain.Entities;
using MediatR;

namespace Contacts37.Application.Usecases.Contacts.Commands.Update
{
    public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, Unit>
    {
        private readonly IContactRepository _contactRepository;

        public UpdateContactCommandHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
        }

        public async Task<Unit> Handle(UpdateContactCommand command, CancellationToken cancellationToken)
        {
            var existingContact = await _contactRepository.GetAsync(command.Id)
                ?? throw new ContactNotFoundException(command.Id);

            await EnsureContactIsUpdatableAsync(command, existingContact);

            await _contactRepository.UpdateAsync(existingContact);

            return Unit.Value;
        }

        private async Task EnsureContactIsUpdatableAsync(UpdateContactCommand command, Contact existingContact)
        {
            if (HasPhoneChanged(command, existingContact))
            {
                await ValidatePhoneIsUniqueAsync(command.DDDCode, command.Phone);

                existingContact.UpdateRegion(command.DDDCode);
                existingContact.UpdatePhone(command.Phone);
            }

            if (!string.IsNullOrWhiteSpace(command.Email) && HasEmailChanged(command.Email!, existingContact.Email!))
            {
                await ValidateEmailIsUniqueAsync(command.Email!);
                existingContact.UpdateEmail(command.Email);
            }

            if (HasNameChanged(command.Name, existingContact.Name))
            {
                existingContact.UpdateName(command.Name);
            }
        }

        private bool HasPhoneChanged(UpdateContactCommand command, Contact existingContact) =>
            command.DDDCode != existingContact.Region.DddCode ||
            command.Phone != existingContact.Phone;

        private bool HasEmailChanged(string newEmail, string existingEmail) =>
            !string.Equals(newEmail, existingEmail, StringComparison.OrdinalIgnoreCase);

        private bool HasNameChanged(string newName, string existingName) =>
            !string.Equals(newName, existingName, StringComparison.OrdinalIgnoreCase);

        private async Task ValidatePhoneIsUniqueAsync(int dddCode, string phone)
        {
            if (!await _contactRepository.IsDddAndPhoneUniqueAsync(dddCode, phone))
            {
                throw new DuplicateContactException(dddCode, phone);
            }
        }

        private async Task ValidateEmailIsUniqueAsync(string email)
        {
            if (!await _contactRepository.IsEmailUniqueAsync(email))
            {
                throw new DuplicateEmailException(email);
            }
        }
    }
}
