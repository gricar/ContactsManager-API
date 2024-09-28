using FluentValidation.Results;

namespace Contacts37.Application.Exceptions
{
	public class ValidationException : ApplicationException
	{
		public List<string> Errors = new List<string>();

		public ValidationException(ValidationResult validationResult)
		{
			foreach (var error in validationResult.Errors)
				Errors.Add(error.ErrorMessage);
		}
	}
}
