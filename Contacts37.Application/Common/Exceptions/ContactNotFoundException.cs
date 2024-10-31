using System.Net;

namespace Contacts37.Application.Common.Exceptions
{
    public class ContactNotFoundException : ApplicationException
    {
        public ContactNotFoundException(Guid id)
            : base($"Contact with ID '{id}' not found.", HttpStatusCode.NotFound)
        { }
    }
}
