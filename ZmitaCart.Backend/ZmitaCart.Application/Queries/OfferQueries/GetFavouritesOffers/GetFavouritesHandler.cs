using FluentResults;
using MediatR;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Common.Errors;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Interfaces.Repositories;
using ZmitaCart.Application.Interfaces.Services;

namespace ZmitaCart.Application.Queries.OfferQueries.GetFavouritesOffers;

public class GetFavoritesHandler : IRequestHandler<GetFavouritesOffersQuery, Result<PaginatedList<OfferInfoDto>>>
{
	private readonly IOfferRepository _offerRepository;
	private readonly ICurrentUserService _currentUserService;

	public GetFavoritesHandler(IOfferRepository offerRepository, ICurrentUserService currentUserService)
	{
		_offerRepository = offerRepository;
		_currentUserService = currentUserService;
	}

	public async Task<Result<PaginatedList<OfferInfoDto>>> Handle(GetFavouritesOffersQuery request, CancellationToken cancellationToken)
	{
		if (_currentUserService.UserId is null)
		{
			return Result.Fail(new UnauthorizedError("You are not authorized to get favourites offers."));
		}
		
		var userId = int.Parse(_currentUserService.UserId);
		var offers = await _offerRepository.GetFavoritesOffersAsync(userId, request.PageNumber, request.PageSize);
		
		if (offers.IsFailed)
		{
			return Result.Fail(offers.Errors);
		}
		
		offers.Value.Items.ForEach(o => o.IsFavourite = true);
		return offers;
	}
}