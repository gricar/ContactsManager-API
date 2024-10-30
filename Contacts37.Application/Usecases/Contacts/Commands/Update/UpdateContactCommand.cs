using MediatR;

namespace Contacts37.Application.Usecases.Contacts.Commands.Update
{
    public sealed record UpdateContactCommand(
        Guid Id,
        string Name,
        int DDDCode,
        string Phone,
        string? Email = null
        ) : IRequest<Unit>;
}