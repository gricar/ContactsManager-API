namespace Contacts37.Application.Usecases.Contacts.Queries.GetAll
{
    public sealed record GetAllContactsResponse(string Name, int DDDCode, string Phone, string Email);
}