using AutoMapper;
using Contacts37.Application.Contracts.Persistence;
using Contacts37.Domain.Entities;
using MediatR;

namespace Contacts37.Application.Usecases.Contacts.Commands.CreateContact
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

        public async Task<CreateContactCommandResponse> Handle(CreateContactCommand request, CancellationToken cancellationToken)
        {
            // add pipeline behavior later - Fluent Validator
            var validator = new CreateContactCommandValidator();

            var validatorRequest = await validator.ValidateAsync(request, cancellationToken);

            if (validatorRequest.Errors.Any())
            {
                // add Result.Failure
                throw new Exception("Invalid contact");
            }

            var contact = _mapper.Map<Contact>(request);

            await _contactRepository.AddAsync(contact);

            return _mapper.Map<CreateContactCommandResponse>(contact);
        }
    }
}
