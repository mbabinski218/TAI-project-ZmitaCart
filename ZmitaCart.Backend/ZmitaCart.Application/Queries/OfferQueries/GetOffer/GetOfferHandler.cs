using FluentResults;
using MediatR;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Interfaces.Repositories;
using ZmitaCart.Application.Interfaces.Services;

namespace ZmitaCart.Application.Queries.OfferQueries.GetOffer;

public class GetOfferHandler : IRequestHandler<GetOfferQuery, Result<OfferDto>>
{
    private readonly IOfferRepository _offerRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetOfferHandler(IOfferRepository offerRepository, ICurrentUserService currentUserService)
    {
        _offerRepository = offerRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<OfferDto>> Handle(GetOfferQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is null)
        {
            return await _offerRepository.GetOfferAsync(request.Id);
        }
		
        var userId = int.Parse(_currentUserService.UserId);
        
        var favorites = await _offerRepository.GetFavoritesOffersIdsAsync(userId);
        if (favorites.IsFailed)
        {
            return Result.Fail(favorites.Errors);
        }
        
        var offer = await _offerRepository.GetOfferAsync(request.Id);
        if (offer.IsFailed)
        {
            return Result.Fail(offer.Errors);
        }
        
        if (favorites.Value.Contains(offer.Value.Id))
        {
            offer.Value.IsFavourite = true;
        }

        return offer;
    }
}