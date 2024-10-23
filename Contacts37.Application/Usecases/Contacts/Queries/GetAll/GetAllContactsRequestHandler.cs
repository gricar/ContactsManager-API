using AutoMapper;
using Contacts37.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts37.Application.Usecases.Contacts.Queries.GetAll
{
    public record GetAllContactsRequestHandler : IRequestHandler<GetAllContactsRequest, IEnumerable<GetAllContactsResponse>>
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public GetAllContactsRequestHandler(IContactRepository contactRepository,
            IMapper mapper)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GetAllContactsResponse>> Handle(GetAllContactsRequest request, CancellationToken cancellationToken)
        {
            var contacts = await _contactRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<GetAllContactsResponse>>(contacts);
        }
    }
}
