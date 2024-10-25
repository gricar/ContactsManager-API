using AutoMapper;
using Contacts37.Application.Common.Exceptions;
using Contacts37.Application.Contracts.Persistence;
using Contacts37.Domain.Entities;
using MediatR;

namespace Contacts37.Application.Usecases.Contacts.Commands.Delete
{
    public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand, DeleteContactCommandResponse>
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public DeleteContactCommandHandler(IContactRepository contactRepository,
            IMapper mapper)
        {
            _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<DeleteContactCommandResponse> Handle(DeleteContactCommand command, CancellationToken cancellationToken)
        {
            var contact = _mapper.Map<Contact>(command);

            await _contactRepository.DeleteAsync(contact);

            return _mapper.Map<DeleteContactCommandResponse>(contact);
        }
    }
}
