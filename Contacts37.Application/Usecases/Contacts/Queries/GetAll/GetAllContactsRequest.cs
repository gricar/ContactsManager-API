using MediatR;

namespace Contacts37.Application.Usecases.Contacts.Queries.GetAll
{
    public sealed record GetAllContactsRequest() : IRequest<IEnumerable<GetAllContactsResponse>>
    {
    }
}
