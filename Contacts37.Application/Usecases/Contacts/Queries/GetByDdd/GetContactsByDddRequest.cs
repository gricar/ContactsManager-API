using MediatR;

namespace Contacts37.Application.Usecases.Contacts.Queries.GetByDdd
{
    public sealed record GetContactsByDddRequest(int DddCode) : IRequest<IEnumerable<GetContactsByDddResponse>>;
}
