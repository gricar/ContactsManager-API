using MediatR;

namespace Contacts37.Application.Usecases.Contacts.Commands.Create
{
    public sealed record CreateContactCommand(
        string Name,
        int DDDCode,
        string Phone,
        string? Email = null
        ) : IRequest<CreateContactCommandResponse>
    {
    }
}
