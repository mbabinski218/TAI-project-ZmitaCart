using FluentResults;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using ZmitaCart.API.Common;
using ZmitaCart.Application.Interfaces.Services;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using TextFormat = MimeKit.Text.TextFormat;

namespace ZmitaCart.API.Services;

public class EmailServiceProvider : IEmailServiceProvider
{
	private readonly ILogger<EmailServiceProvider> _logger;
	private readonly EmailServiceSettings _emailServiceSettings;

	public EmailServiceProvider(ILogger<EmailServiceProvider> logger, IOptions<EmailServiceSettings> emailServiceSettings)
	{
		_logger = logger;
		_emailServiceSettings = emailServiceSettings.Value;
	}

	public async Task<Result> SendAsync(string email, string subject, string text, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Sending email to {email} with subject: {subject}.", email, subject);

		try
		{
			var message = new MimeMessage
			{
				From = { MailboxAddress.Parse(_emailServiceSettings.Email) },
				To = { MailboxAddress.Parse(email) },
				Subject = subject,
				Body = new TextPart(TextFormat.Text) { Text = text }
			};

			using var smtp = new SmtpClient();
			await smtp.ConnectAsync(_emailServiceSettings.Host, _emailServiceSettings.Port, SecureSocketOptions.StartTls, cancellationToken);
			await smtp.AuthenticateAsync(_emailServiceSettings.Email, _emailServiceSettings.Password, cancellationToken);
			await smtp.SendAsync(message, cancellationToken);
			await smtp.DisconnectAsync(true, cancellationToken);
		}
		catch (Exception ex)
		{
			_logger.LogError("Exception when sending email: {ex}", ex.Message);
			return Result.Fail("Failed to send email.");
		}
		
		return Result.Ok();
	}

	public async Task<Result> SendEmailConfirmationAsync(string email, string link, CancellationToken cancellationToken)
	{
		const string subject = "Welcome to ZmitaCart!";
		var text = $"""
		            Welcome to ZmitaCart!
		            You have successfully registered your account.
		            Verify your email address by clicking the link below:
		            {link}

		            See you soon!
		            """;
		
		return await SendAsync(email, subject, text, cancellationToken);
	}
}