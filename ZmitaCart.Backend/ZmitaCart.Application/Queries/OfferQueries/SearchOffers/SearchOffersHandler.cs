using FluentResults;
using MapsterMapper;
using MediatR;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Interfaces.Repositories;
using ZmitaCart.Application.Interfaces.Services;

namespace ZmitaCart.Application.Queries.OfferQueries.SearchOffers;

public class SearchOffersHandler : IRequestHandler<SearchOffersQuery, Result<PaginatedList<OfferInfoDto>>>
{
	private readonly IOfferRepository _offerRepository;
	private readonly IMapper _mapper;
	private readonly ICurrentUserService _currentUserService;

	public SearchOffersHandler(IOfferRepository offerRepository, IMapper mapper, ICurrentUserService currentUserService)
	{
		_offerRepository = offerRepository;
		_mapper = mapper;
		_currentUserService = currentUserService;
	}

	public async Task<Result<PaginatedList<OfferInfoDto>>> Handle(SearchOffersQuery request, CancellationToken cancellationToken)
	{
		var dto = _mapper.Map<SearchOfferDto>(request);
		dto.UserId = int.Parse(_currentUserService.UserId ?? "0");
		
		if (_currentUserService.UserId is null)
		{
			return await _offerRepository.SearchOffersAsync(dto, request.PageNumber, request.PageSize); 
		}
		
		var userId = int.Parse(_currentUserService.UserId);
		
		var favorites = await _offerRepository.GetFavoritesOffersIdsAsync(userId);
		if(favorites.IsFailed)
		{
			return Result.Fail(favorites.Errors);
		}
		
		var offers = await _offerRepository.SearchOffersAsync(dto, request.PageNumber, request.PageSize);
		if(offers.IsFailed)
		{
			return Result.Fail(offers.Errors);
		}

		offers.Value.Items.ForEach(o =>
		{
			o.IsFavourite = favorites.Value.Contains(o.Id);
		});
		
		return offers;
	}
}