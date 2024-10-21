using MediatR;

namespace Contacts37.Application.Usecases.Contacts.Commands.Create
{
    public sealed record CreateContactCommand(
        string Name,
        string DDDCode,
        string Phone,
        string Email
        ) : IRequest<CreateContactCommandResponse>
    {
    }
}
