using FluentResults;
using MediatR;
using ZmitaCart.Application.Common.Errors;
using ZmitaCart.Application.Interfaces.Repositories;
using ZmitaCart.Application.Interfaces.Services;

namespace ZmitaCart.Application.Commands.OfferCommands.AddToFavorites;

public class AddToFavoritesHandler : IRequestHandler<AddToFavoritesCommand, Result>
{
	private readonly IOfferRepository _offerRepository;
	private readonly ICurrentUserService _currentUserService;

	public AddToFavoritesHandler(ICurrentUserService currentUserService, IOfferRepository offerRepository)
	{
		_currentUserService = currentUserService;
		_offerRepository = offerRepository;
	}

	public async Task<Result> Handle(AddToFavoritesCommand request, CancellationToken cancellationToken)
	{
		if (_currentUserService.UserId is null)
		{
			return Result.Fail(new UnauthorizedError("You are not authorized to add an offer to favorites."));
		}
		
		var userId = int.Parse(_currentUserService.UserId);
		return await _offerRepository.AddToFavoritesAsync(userId, request.Id);
	}
}