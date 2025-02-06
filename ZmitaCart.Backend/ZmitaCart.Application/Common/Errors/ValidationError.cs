using FluentResults;

namespace ZmitaCart.Application.Common.Errors;

public class ValidationError : IError
{
	public ValidationError(string message)
	{
		Message = message;
	}

	public string Message { get; }
	public Dictionary<string, object>? Metadata { get; set; }
	public List<IError>? Reasons { get; set; }
}