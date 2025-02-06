using FluentResults;
using MediatR;
using ZmitaCart.Application.Dtos.FileDtos;

namespace ZmitaCart.Application.Queries.FileQueries;

public record GetFileQuery(string Name) : IRequest<Result<FileDto>>;