using FluentResults;
using Microsoft.AspNetCore.Mvc;
using ZmitaCart.Application.Common.Errors;

namespace ZmitaCart.API.Common;

public class Success<T> : Success
{
	public T Value { get; }

	public Success(IResult<T> result) : base(result)
	{
		Value = result.Value;
	}
}

public class Success
{
	public IReadOnlyCollection<ISuccess> Reasons { get; }

	public Success(IResultBase result)
	{
		Reasons = result.Successes;
	}
}

public class Error
{
	public IReadOnlyCollection<IError> Reasons { get; }
	public int StatusCode { get; }

	public Error(IResultBase result)
	{
		Reasons = result.Errors;

		StatusCode = result.Errors.FirstOrDefault() switch
		{
			UnauthorizedError => StatusCodes.Status401Unauthorized,
			NotFoundError => StatusCodes.Status404NotFound,
			_ => StatusCodes.Status400BadRequest
		};
	}

	public override string ToString() => string.Join("\n", Reasons.Select(r => r.Message));
}

public static class FluentResultsExtensions
{
	public static async Task<ActionResult> Then<T>(this Task<Result<T>> result, Func<Success<T>, ActionResult> success, Func<Error, ActionResult> err)
	{
		var response = await result;
		var status = response.IsSuccess ? success(new Success<T>(response)) : err(new Error(response));
		return status;
	}
	
	public static async Task<ActionResult> Then(this Task<Result> result, Func<Success, ActionResult> success, Func<Error, ActionResult> err)
	{
		var response = await result;
		var status =  response.IsSuccess ? success(new Success(response)) : err(new Error(response));
		return status;
	}
	
	public static List<string> ToList(this Error error)
	{
		var errors = new List<string>();
		foreach (var reason in error.Reasons)
		{
			if (reason.Reasons != null && reason.Reasons.Any())
			{
				errors.AddRange(reason.Reasons.Select(e => e.Message).ToList());
			}
			else
			{
				errors.Add(reason.Message);
			}
		}

		return errors;
	}
}