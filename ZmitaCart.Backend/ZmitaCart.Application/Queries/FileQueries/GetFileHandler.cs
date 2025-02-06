using FluentResults;
using MediatR;
using Microsoft.AspNetCore.StaticFiles;
using ZmitaCart.Application.Common.Errors;
using ZmitaCart.Application.Dtos.FileDtos;

namespace ZmitaCart.Application.Queries.FileQueries;

public class GetFileHandler : IRequestHandler<GetFileQuery, Result<FileDto>>
{
	public async Task<Result<FileDto>> Handle(GetFileQuery request, CancellationToken cancellationToken)
	{
		var path = Path.Combine(Path.GetFullPath("wwwroot"), request.Name);

		if (!File.Exists(path))
		{
			return Result.Fail(new ArgumentError("File not found"));
		}

		var contentProvider = new FileExtensionContentTypeProvider();
		contentProvider.TryGetContentType(path, out var contentType);
		
		if (contentType is null)
		{
			return Result.Fail(new Error("Content type not found"));
		}
		
		var file = await File.ReadAllBytesAsync(path, cancellationToken);

		return new FileDto
		{
			File = file,
			ContentType = contentType
		};
	}
}