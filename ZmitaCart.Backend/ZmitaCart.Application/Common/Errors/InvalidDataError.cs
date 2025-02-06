using FluentResults;

namespace ZmitaCart.Application.Common.Errors;

public class InvalidDataError: IError
{
	public InvalidDataError(string message)
	{
		Message = message;
	}
	
	public InvalidDataError(string message, IEnumerable<string> reasons)
	{
		Message = message;
		Reasons = new List<IError>();
		Reasons.AddRange(reasons.Select(s => new Error(s)).ToList());
	}

	public string Message { get; }
	public Dictionary<string, object>? Metadata { get; set; }
	public List<IError>? Reasons { get; set; }
}