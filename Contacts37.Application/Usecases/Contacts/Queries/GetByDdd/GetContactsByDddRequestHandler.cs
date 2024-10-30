using AutoMapper;
using Contacts37.Application.Contracts.Persistence;
using MediatR;

namespace Contacts37.Application.Usecases.Contacts.Queries.GetByDdd
{
    public record GetContactsByDddRequestHandler :
        IRequestHandler<GetContactsByDddRequest, IEnumerable<GetContactsByDddResponse>>
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public GetContactsByDddRequestHandler(IContactRepository contactRepository,
            IMapper mapper)
        {
            _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<IEnumerable<GetContactsByDddResponse>> Handle(GetContactsByDddRequest request, CancellationToken cancellationToken)
        {
            var contacts = await _contactRepository.GetContactsDddCode(request.DddCode);

            return _mapper.Map<IEnumerable<GetContactsByDddResponse>>(contacts);
        }
    }
}
