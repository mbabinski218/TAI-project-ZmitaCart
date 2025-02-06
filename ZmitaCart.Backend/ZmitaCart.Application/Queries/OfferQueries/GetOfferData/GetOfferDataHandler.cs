using FluentResults;
using MediatR;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Interfaces.Repositories;

namespace ZmitaCart.Application.Queries.OfferQueries.GetOfferData;

public class GetOfferDataHandler : IRequestHandler<GetOfferDataQuery, Result<OfferDataDto>>
{
	private readonly IOfferRepository _offerRepository;

	public GetOfferDataHandler(IOfferRepository offerRepository)
	{
		_offerRepository = offerRepository;
	}

	public async Task<Result<OfferDataDto>> Handle(GetOfferDataQuery request, CancellationToken cancellationToken)
	{
		return await _offerRepository.GetOfferDataAsync(request.Id);
	}
}