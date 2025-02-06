using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZmitaCart.API.Common;
using ZmitaCart.Application.Commands.UserCommands.AddRole;
using ZmitaCart.Application.Commands.UserCommands.ConfirmEmail;
using ZmitaCart.Application.Commands.UserCommands.DeleteFeedback;
using ZmitaCart.Application.Commands.UserCommands.GiveFeedback;
using ZmitaCart.Application.Commands.UserCommands.LoginUser;
using ZmitaCart.Application.Commands.UserCommands.Register;
using ZmitaCart.Application.Commands.UserCommands.ResendEmailConfirmation;
using ZmitaCart.Application.Commands.UserCommands.UpdateCredentials;
using ZmitaCart.Application.Commands.UserCommands.UpdateFeedback;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.LogDtos;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Dtos.UserDtos;
using ZmitaCart.Application.Queries.LogQueries.GetLogs;
using ZmitaCart.Application.Queries.OfferQueries.GetUserOffers;
using ZmitaCart.Application.Queries.UserQueries.GetData;
using ZmitaCart.Application.Queries.UserQueries.GetFeedback;
using ZmitaCart.Application.Queries.UserQueries.LogoutUser;
using ZmitaCart.Domain.Common.Types;

namespace ZmitaCart.API.Controllers;

[Route("user")]
public class UserController : ApiController
{
	public UserController(IMediator mediator) : base(mediator)
	{
	}

	[HttpGet]
	[RoleAuthorize]
	public async Task<ActionResult<UserDataDto>> GetUserData()
	{
		return await mediator.Send(new GetDataQuery()).Then(
			s => Ok(s.Value),
			err => StatusCode(err.StatusCode, err.ToList()));
	}
	
	[HttpPost("register")]
	public async Task<ActionResult> Register([FromBody] RegisterUserCommand command)
	{
		return await mediator.Send(command).Then(
			s => Ok(),
			err => StatusCode(err.StatusCode, err.ToList()));
	}

	[HttpPost("login")]
	public async Task<ActionResult<TokensDto>> Login([FromBody] LoginUserCommand command)
	{
		return await mediator.Send(command).Then(
			s => Ok(s.Value),
			err => StatusCode(err.StatusCode, err.ToList()));
	}

	[HttpPost("logout")]
	[RoleAuthorize]
	public async Task<ActionResult> Logout()
	{
		return await mediator.Send(new LogoutUserQuery()).Then(
			s => Ok(),
			err => StatusCode(err.StatusCode, err.ToList()));
	}

	[HttpPost("addRole")]
	[RoleAuthorize(Role.administrator)]
	public async Task<ActionResult> AddRoleForUser([FromBody] AddRoleForUserCommand command)
	{
		return await mediator.Send(command).Then(
			s => Ok(),
			err => StatusCode(err.StatusCode, err.ToList()));
	}

	[HttpPut("updateCredentials")]
	[RoleAuthorize]
	public async Task<ActionResult> UpdateCredentials([FromBody] UpdateCredentialsCommand command)
	{
		return await mediator.Send(command).Then(
			s => Ok(),
			err => StatusCode(err.StatusCode, err.ToList()));
	}

	[HttpPost("feedback")]
	[RoleAuthorize]
	public async Task<ActionResult<int>> GiveFeedback([FromBody] GiveFeedbackCommand command)
	{
		return await mediator.Send(command).Then(
			s => Ok(s.Value),
			err => StatusCode(err.StatusCode, err.ToList()));
	}

	[HttpPut("feedback")]
	[RoleAuthorize]
	public async Task<ActionResult<int>> UpdateFeedback([FromBody] UpdateFeedbackCommand command)
	{
		return await mediator.Send(command).Then(
			s => Ok(s.Value),
			err => StatusCode(err.StatusCode, err.ToList()));
	}

	[HttpDelete("feedback/{id:int}")]
	[RoleAuthorize]
	public async Task<ActionResult> DeleteFeedback([FromRoute] int id)
	{
		return await mediator.Send(new DeleteFeedbackCommand(id)).Then(
			s => Ok(),
			err => NotFound(err.ToList()));
	}

	[HttpGet("feedback")]
	public async Task<ActionResult<PaginatedList<FeedbackDto>>> GetFeedback([FromQuery] GetFeedbackQuery query)
	{
		return await mediator.Send(query).Then(
			s => Ok(s.Value),
			err => StatusCode(err.StatusCode, err.ToList()));
	}
	
	[HttpGet("offer")]
	[RoleAuthorize]
	public async Task<ActionResult<PaginatedList<OfferInfoDto>>> GetUserOffers([FromQuery] GetUserOffersQuery query)
	{
		return await mediator.Send(query).Then(
			s => Ok(s.Value),
			err => StatusCode(err.StatusCode, err.ToList()));
	}
	
	[AllowAnonymous]
	[HttpPost("resend-email-confirmation")]
	public async Task<ActionResult> ResendEmailConfirmation([FromQuery] ResendEmailConfirmationCommand command, CancellationToken cancellationToken) => 
		await mediator.Send(command, cancellationToken).Then(
			Ok,
			err => StatusCode(err.StatusCode, err.ToList()));
	
	[AllowAnonymous]
	[HttpPost("confirm-email")]
	public async Task<ActionResult> ConfirmEmail([FromQuery] ConfirmEmailCommand command, CancellationToken cancellationToken) =>
		await mediator.Send(command, cancellationToken).Then(
			Ok,
			err => StatusCode(err.StatusCode, err.ToList()));
	
	[RoleAuthorize(Role.administrator)]
	[HttpGet("log")]
	public async Task<ActionResult<PaginatedList<LogDto>>> GetLogs([FromQuery] GetLogsQuery query, CancellationToken cancellationToken) =>
		await mediator.Send(query, cancellationToken).Then(
			s => Ok(s.Value),
			err => StatusCode(err.StatusCode, err.ToList()));
}