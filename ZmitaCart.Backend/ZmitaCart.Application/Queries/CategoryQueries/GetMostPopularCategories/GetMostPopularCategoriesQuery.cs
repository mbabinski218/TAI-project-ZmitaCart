using FluentResults;
using MediatR;

namespace ZmitaCart.Application.Queries.CategoryQueries.GetMostPopularCategories;

public record GetMostPopularCategoriesQuery(int NumberOfCategories) : IRequest<Result<List<string>>>;