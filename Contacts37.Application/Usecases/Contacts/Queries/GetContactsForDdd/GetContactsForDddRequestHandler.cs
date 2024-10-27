using AutoMapper;
using Contacts37.Application.Contracts.Persistence;
using MediatR;

namespace Contacts37.Application.Usecases.Contacts.Queries.GetAll
{
    public record GetContactsForDddRequestHandler : IRequestHandler<GetContactsForDddRequest, IEnumerable<GetContactsForDddResponse>>
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public GetContactsForDddRequestHandler(IContactRepository contactRepository,
            IMapper mapper)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GetContactsForDddResponse>> Handle(GetContactsForDddRequest request, CancellationToken cancellationToken)
        {
            var contacts = await _contactRepository.GetContactsDddCode(request.DddCode);

            return _mapper.Map<IEnumerable<GetContactsForDddResponse>>(contacts);
        }
    }
}
