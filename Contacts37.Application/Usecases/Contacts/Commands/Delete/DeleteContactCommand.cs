using MediatR;

namespace Contacts37.Application.Usecases.Contacts.Commands.Delete
{
    public sealed record DeleteContactCommand(
        Guid Id
        ) : IRequest<Unit>
    {
    }
}
