using FluentResults;
using MediatR;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Common.Errors;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Interfaces.Repositories;
using ZmitaCart.Application.Interfaces.Services;

namespace ZmitaCart.Application.Queries.OfferQueries.GetBoughtOffers;

public class GetBoughtOffersHandler : IRequestHandler<GetBoughtOffersQuery, Result<PaginatedList<BoughtOfferDto>>>
{
	private readonly IOfferRepository _offerRepository;
	private readonly ICurrentUserService _currentUserService;

	public GetBoughtOffersHandler(IOfferRepository offerRepository, ICurrentUserService currentUserService)
	{
		_offerRepository = offerRepository;
		_currentUserService = currentUserService;
	}

	public async Task<Result<PaginatedList<BoughtOfferDto>>> Handle(GetBoughtOffersQuery request, CancellationToken cancellationToken)
	{
		if (_currentUserService.UserId is null)
		{
			return Result.Fail(new UnauthorizedError("You are not authorized to get bought offers."));
		}
		
		var userId = int.Parse(_currentUserService.UserId);
		
		var offers =  await _offerRepository.GetBoughtAsync(userId, request.PageNumber, request.PageSize);

		if (offers.IsFailed)
		{
			return Result.Fail(offers.Errors);
		}
		
		var favorites = await _offerRepository.GetFavoritesOffersIdsAsync(userId);
		
		if (favorites.IsFailed)
		{
			return Result.Fail(favorites.Errors);
		}
		
		offers.Value.Items.ForEach(b => b.Offer.IsFavourite = favorites.Value.Contains(b.Offer.Id));
		
		return offers;
	}
}