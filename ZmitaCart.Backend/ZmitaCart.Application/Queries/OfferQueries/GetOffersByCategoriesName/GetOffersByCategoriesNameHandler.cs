using FluentResults;
using MediatR;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Interfaces.Repositories;
using ZmitaCart.Application.Interfaces.Services;

namespace ZmitaCart.Application.Queries.OfferQueries.GetOffersByCategoriesName;

public class GetOffersByCategoriesNameHandler : IRequestHandler<GetOffersByCategoriesNameQuery,
	Result<List<NamedList<string, OfferInfoDto>>>>
{
	private readonly IOfferRepository _offerRepository;
	private readonly ICurrentUserService _currentUserService;

	public GetOffersByCategoriesNameHandler(IOfferRepository offerRepository, ICurrentUserService currentUserService)
	{
		_offerRepository = offerRepository;
		_currentUserService = currentUserService;
	}

	public async Task<Result<List<NamedList<string, OfferInfoDto>>>> Handle(GetOffersByCategoriesNameQuery request,
		CancellationToken cancellationToken)
	{
		var offers = await _offerRepository.GetOffersByCategoriesNameAsync(
			request.CategoriesNames, int.Parse(_currentUserService.UserId ?? "0"), request.Size);

		if (offers.IsFailed)
		{
			return Result.Fail<List<NamedList<string, OfferInfoDto>>>(offers.Errors);
		}

		if (_currentUserService.UserId is null)
		{
			return offers;
		}

		var userId = int.Parse(_currentUserService.UserId);

		var favorites = await _offerRepository.GetFavoritesOffersIdsAsync(userId);
		if (favorites.IsFailed)
		{
			return Result.Fail(favorites.Errors);
		}

		offers.Value.ForEach(l => 
			l.Data.ForEach(o =>
			{
				o.IsFavourite = favorites.Value.Contains(o.Id);
			}));

		return offers;
	}
}