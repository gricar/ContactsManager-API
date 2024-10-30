namespace Contacts37.Application.Common.Exceptions
{
    public class DuplicateEmailException : ApplicationException
    {
        public DuplicateEmailException(string email)
            : base($"A contact with the same Email '{email}' already exists.")
        { }
    }
}
