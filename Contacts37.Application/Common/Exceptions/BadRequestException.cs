using System;

namespace Contacts37.Application.Common.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) {}


        public BadRequestException(IEnumerable<string> errorDetails)
        : base("Validation errors occurred. See error details.")
        {
            ErrorDetails = errorDetails;
        }

        public IEnumerable<string> ErrorDetails { get; }
    }
}
