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
            var isUnique = await _contactRepository.IsDddAndPhoneUniqueAsync(command.DDDCode, command.Phone);

            if (!isUnique)
                throw new BadRequestException("A contact with the same DDD code and phone number already exists.");
            
            var contact = _mapper.Map<Contact>(command);

            await _contactRepository.AddAsync(contact);

            return _mapper.Map<CreateContactCommandResponse>(contact);
        }
    }
}
