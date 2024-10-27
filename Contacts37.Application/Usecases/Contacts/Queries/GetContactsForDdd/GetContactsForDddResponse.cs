namespace Contacts37.Application.Usecases.Contacts.Queries.GetAll
{
    public sealed record GetContactsForDddResponse(string Name, int DDDCode, string Phone, string Email);
}