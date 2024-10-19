namespace Contacts37.Application.Common.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }


        public BadRequestException(string[] errorDetails)
        : base("Validation errors occurred. See error details.")
        {
            ErrorDetails = errorDetails;
        }

        public string[] ErrorDetails { get; }
    }
}
