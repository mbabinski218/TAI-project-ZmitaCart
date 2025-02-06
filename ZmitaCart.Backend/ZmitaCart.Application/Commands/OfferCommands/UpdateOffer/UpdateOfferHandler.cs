using FluentResults;
using MapsterMapper;
using MediatR;
using ZmitaCart.Application.Common.Errors;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Interfaces.Repositories;
using ZmitaCart.Application.Interfaces.Services;

namespace ZmitaCart.Application.Commands.OfferCommands.UpdateOffer;

public class UpdateOfferHandler : IRequestHandler<UpdateOfferCommand, Result<int>>
{
	private readonly IOfferRepository _offerRepository;
	private readonly IMapper _mapper;
	private readonly IPictureRepository _pictureRepository;
	private readonly ICurrentUserService _currentUserService;
	
	public UpdateOfferHandler(IOfferRepository offerRepository, IMapper mapper, IPictureRepository pictureRepository, 
		ICurrentUserService currentUserService)
	{
		_offerRepository = offerRepository;
		_mapper = mapper;
		_pictureRepository = pictureRepository;
		_currentUserService = currentUserService;
	}

	public async Task<Result<int>> Handle(UpdateOfferCommand request, CancellationToken cancellationToken)
	{		
		if (_currentUserService.UserId is null)
		{
			return Result.Fail(new UnauthorizedError("You are not authorized to update an offer."));
		}
		
		var offer = _mapper.Map<UpdateOfferDto>(request);
		offer.UserId = int.Parse(_currentUserService.UserId);

		if (request.EditedPictures is null)
		{
			await _pictureRepository.DeleteAllAsync(offer.Id);
			return await _offerRepository.UpdateAsync(offer);
		}
		
		var pictures = await _pictureRepository.GetPictureNameByOfferIdAsync(offer.UserId);

		if (pictures.IsFailed)
		{
			return Result.Fail(pictures.Errors);
		}
		
		var newPictures = request.EditedPictures.Where(p => !pictures.Value.Contains(p.FileName));
		var result = await _pictureRepository.AddAsync(offer.UserId, offer.Id, newPictures);
		
		if (result.IsFailed)
		{
			return Result.Fail(result.Errors);
		}
		
		var deletePictures = pictures.Value.Where(p => request.EditedPictures.All(e => e.FileName != p));
		result = await _pictureRepository.DeleteAsync(offer.UserId, offer.Id, deletePictures);

		if (result.IsFailed)
		{
			return Result.Fail(result.Errors);
		}
		
		return await _offerRepository.UpdateAsync(offer);
	}
}