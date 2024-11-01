using AutoMapper;
using Contacts37.Application.Common.Exceptions;
using Contacts37.Application.Contracts.Persistence;
using Contacts37.Domain.Entities;
using MediatR;

namespace Contacts37.Application.Usecases.Contacts.Commands.Create
{
    public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, CreateContactCommandResponse>
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public CreateContactCommandHandler(IContactRepository contactRepository,
            IMapper mapper)
        {
            _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        public async Task<CreateContactCommandResponse> Handle(CreateContactCommand command, CancellationToken cancellationToken)
        {
            await EnsureContactIsUniqueAsync(command);
            
            var contact = Contact.Create(command.Name, command.DDDCode, command.Phone, command.Email);

            await _contactRepository.AddAsync(contact);

            return _mapper.Map<CreateContactCommandResponse>(contact);
        }

        private async Task EnsureContactIsUniqueAsync(CreateContactCommand command)
        {
            await CheckForUniqueEmailAsync(command.Email);

            await CheckForUniqueContactAsync(command.DDDCode, command.Phone);
        }

        private async Task CheckForUniqueEmailAsync(string? email)
        {
            if (!string.IsNullOrEmpty(email) && !await _contactRepository.IsEmailUniqueAsync(email))
            {
                throw new DuplicateEmailException(email!);
            }
        }

        private async Task CheckForUniqueContactAsync(int dddCode, string phone)
        {
            if (!await _contactRepository.IsDddAndPhoneUniqueAsync(dddCode, phone))
            {
                throw new DuplicateContactException(dddCode, phone);
            }
        }
    }
}
