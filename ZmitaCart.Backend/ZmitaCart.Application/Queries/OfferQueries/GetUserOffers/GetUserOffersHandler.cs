using FluentResults;
using MediatR;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Common.Errors;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Interfaces.Repositories;
using ZmitaCart.Application.Interfaces.Services;

namespace ZmitaCart.Application.Queries.OfferQueries.GetUserOffers;

public class GetUserOffersHandler : IRequestHandler<GetUserOffersQuery, Result<PaginatedList<OfferInfoDto>>>
{
	private readonly IOfferRepository _offerRepository;
	private readonly ICurrentUserService _currentUserService;

	public GetUserOffersHandler(IOfferRepository offerRepository, ICurrentUserService currentUserService)
	{
		_offerRepository = offerRepository;
		_currentUserService = currentUserService;
	}

	public async Task<Result<PaginatedList<OfferInfoDto>>> Handle(GetUserOffersQuery request, CancellationToken cancellationToken)
	{
		if (_currentUserService.UserId is null)
		{
			return Result.Fail(new UnauthorizedError("You must be logged in to access this resource"));
		}

		var userId = int.Parse(_currentUserService.UserId);
		
		return await _offerRepository.GetUserOffersAsync(userId, request.PageNumber, request.PageSize);
	}
}