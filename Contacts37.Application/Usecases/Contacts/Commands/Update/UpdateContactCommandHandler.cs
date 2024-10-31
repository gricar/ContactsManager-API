using AutoMapper;
using Contacts37.Application.Common.Exceptions;
using Contacts37.Application.Contracts.Persistence;
using Contacts37.Domain.Entities;
using MediatR;

namespace Contacts37.Application.Usecases.Contacts.Commands.Update
{
    public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, Unit>
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public UpdateContactCommandHandler(IContactRepository contactRepository, IMapper mapper)
        {
            _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Unit> Handle(UpdateContactCommand command, CancellationToken cancellationToken)
        {
            var existingUser = await _contactRepository.GetAsync(command.Id)
                ?? throw new ContactNotFoundException(command.Id);

            await ValidateContactAsync(command, existingUser);

            _mapper.Map(command, existingUser);

            await _contactRepository.UpdateAsync(existingUser);

            return Unit.Value;
        }

        private async Task ValidateContactAsync(UpdateContactCommand command, Contact existingUser)
        {
            if (HasPhoneChanged(command, existingUser))
            {
                await ValidatePhoneIsUniqueAsync(command.DDDCode, command.Phone);
            }

            if (HasEmailChanged(command.Email!, existingUser.Email!))
            {
                await ValidateEmailIsUniqueAsync(command.Email!);
            }
        }

        private bool HasPhoneChanged(UpdateContactCommand command, Contact existingUser) =>
            command.DDDCode != existingUser.Region.DddCode ||
            command.Phone != existingUser.Phone;

        private bool HasEmailChanged(string newEmail, string existingEmail) =>
            !string.Equals(newEmail, existingEmail, StringComparison.OrdinalIgnoreCase);

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
