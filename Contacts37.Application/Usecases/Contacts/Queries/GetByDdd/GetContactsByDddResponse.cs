namespace Contacts37.Application.Usecases.Contacts.Queries.GetByDdd
{
    public sealed record GetContactsByDddResponse()
    {
        public string Name { get; set; } = string.Empty;
        public int DDDCode { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}