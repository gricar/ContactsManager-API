using MediatR;

namespace Contacts37.Application.Usecases.Contacts.Queries.GetAll
{
    public sealed record GetContactsForDddRequest(int DddCode) : IRequest<IEnumerable<GetContactsForDddResponse>>;
}
