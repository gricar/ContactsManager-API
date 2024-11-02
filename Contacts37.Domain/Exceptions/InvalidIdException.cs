namespace Contacts37.Domain.Exceptions
{
    public class InvalidIdException : DomainException
    {
        public InvalidIdException(Guid id)
            : base($"Id not found.")
        { }
    }
}