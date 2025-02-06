using FluentResults;
using MediatR;
using ZmitaCart.Application.Common.Errors;
using ZmitaCart.Application.Interfaces.Repositories;
using ZmitaCart.Application.Interfaces.Services;

namespace ZmitaCart.Application.Queries.OfferQueries.GetFavouritesOffersCount;

public class GetFavouritesOfferCountHandler : IRequestHandler<GetFavouritesOfferCountQuery, Result<int>>
{
	private readonly IOfferRepository _offerRepository;
	private readonly ICurrentUserService _currentUserService;
	
	public GetFavouritesOfferCountHandler(IOfferRepository offerRepository, ICurrentUserService currentUserService)
	{
		_offerRepository = offerRepository;
		_currentUserService = currentUserService;
	}

	public async Task<Result<int>> Handle(GetFavouritesOfferCountQuery request, CancellationToken cancellationToken)
	{
		if (_currentUserService.UserId is null)
		{
			return Result.Fail(new UnauthorizedError("You are not authorized"));
		}

		var userId = int.Parse(_currentUserService.UserId);
		
		return await _offerRepository.GetFavoritesOffersCountAsync(userId);
	}
}