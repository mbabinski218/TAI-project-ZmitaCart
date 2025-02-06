using FluentResults;
using MapsterMapper;
using MediatR;
using ZmitaCart.Application.Common.Errors;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Interfaces.Repositories;
using ZmitaCart.Application.Interfaces.Services;

namespace ZmitaCart.Application.Commands.OfferCommands.CreateOffer;

public class CreateOfferHandler : IRequestHandler<CreateOfferCommand, Result<int>>
{
	private readonly IOfferRepository _offerRepository;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;
	private readonly IPictureRepository _pictureRepository;

	public CreateOfferHandler(IOfferRepository offerRepository, ICurrentUserService currentUserService, IMapper mapper, 
		IPictureRepository pictureRepository)
	{
		_offerRepository = offerRepository;
		_currentUserService = currentUserService;
		_mapper = mapper;
		_pictureRepository = pictureRepository;
	}

	public async Task<Result<int>> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
	{
		if (_currentUserService.UserId is null)
		{
			return Result.Fail(new UnauthorizedError("You are not authorized to create an offer."));
		}
		
		var offer = _mapper.Map<CreateOfferDto>(request);
		offer.UserId = int.Parse(_currentUserService.UserId);
		offer.CreatedAt = DateTimeOffset.Now;
		offer.IsAvailable = true;
		
		var offerId = await _offerRepository.CreateAsync(offer);

		if (request.Pictures is not null && offerId.IsSuccess)
		{
			await _pictureRepository.AddAsync(offer.UserId, offerId.Value, request.Pictures);
		}

		return offerId;
	}
}