using FluentResults;

namespace ZmitaCart.Application.Interfaces.Services;

public interface IEmailServiceProvider
{
	Task<Result> SendAsync(string email, string subject, string text, CancellationToken cancellationToken = default);
	Task<Result> SendEmailConfirmationAsync(string email, string link, CancellationToken cancellationToken = default);
}