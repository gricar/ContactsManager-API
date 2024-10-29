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
            _contactRepository = contactRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateContactCommand command, CancellationToken cancellationToken)
        {
            var existingUser = await _contactRepository.GetAsync(command.Id);

            if (existingUser is null)
                throw new ArgumentException("Contact not found");

            await validateUserPhone(command, existingUser);
            await validateUserEmail(command.Email!, existingUser.Email!);

            _mapper.Map(command, existingUser);

            await _contactRepository.UpdateAsync(existingUser);

            return new Unit();
        }

        private async Task<bool> validateUserPhone(UpdateContactCommand command, Contact existingUser)
        {
            var phoneChanged = command.DDDCode == existingUser.DddCode &&
                command.Phone == existingUser.Phone;

            if (phoneChanged) return false;

            var isUnique = await _contactRepository.IsDddAndPhoneUniqueAsync(command.DDDCode, command.Phone);

            if (!isUnique)
                throw new BadRequestException("A contact with the same DDD code and phone number already exists.");

            return true;
        }

        private async Task<bool> validateUserEmail(string email, string existingUserEmail)
        {
            var emailChanged = email == existingUserEmail;

            if (emailChanged) return false;

            var isUnique = await _contactRepository.IsEmailUniqueAsync(email);

            if (!isUnique)
                throw new BadRequestException("Email already registered.");

            return true;
        }
    }
}
