using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZmitaCart.API.Common;
using ZmitaCart.Application.Dtos.FileDtos;
using ZmitaCart.Application.Queries.FileQueries;

namespace ZmitaCart.API.Controllers;

public class FileController : ApiController
{
	public FileController(IMediator mediator) : base(mediator)
	{
	}
	
	[HttpGet]
	public async Task<ActionResult<FileDto>> GetFile([FromQuery] GetFileQuery query)
	{
		return await mediator.Send(query).Then(
			s => File(s.Value.File, s.Value.ContentType),
			err => StatusCode(err.StatusCode, err.ToList()));
	}
}