using FluentResults;
using MediatR;
using ZmitaCart.Application.Common.Errors;
using ZmitaCart.Application.Interfaces.Repositories;
using ZmitaCart.Application.Interfaces.Services;

namespace ZmitaCart.Application.Commands.OfferCommands.DeleteOffer;

public class DeleteOfferHandler : IRequestHandler<DeleteOfferCommand, Result>
{
	private readonly IOfferRepository _offerRepository;
	private readonly ICurrentUserService _currentUserService;
	private readonly IPictureRepository _pictureRepository;

	public DeleteOfferHandler(IOfferRepository offerRepository, ICurrentUserService currentUserService, IPictureRepository pictureRepository)
	{
		_offerRepository = offerRepository;
		_currentUserService = currentUserService;
		_pictureRepository = pictureRepository;
	}

	public async Task<Result> Handle(DeleteOfferCommand request, CancellationToken cancellationToken)
	{
		var user = _currentUserService.UserId;
		if (user is null)
		{
			return Result.Fail(new UnauthorizedError("You are not authorized to delete an offer."));
		}

		var userId = int.Parse(user);
		
		var removePic = await _pictureRepository.DeleteAsync(userId, request.Id);
		if (removePic.IsFailed)
		{
			return Result.Fail(removePic.Errors);
		}
		
		var removeOffer = await _offerRepository.DeleteAsync(userId, request.Id);
		if (removeOffer.IsFailed)
		{
			return Result.Fail(removeOffer.Errors);
		}

		return Result.Ok();
	}
}