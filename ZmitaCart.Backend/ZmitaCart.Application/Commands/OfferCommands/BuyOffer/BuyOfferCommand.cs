using FluentResults;
using MediatR;

namespace ZmitaCart.Application.Commands.OfferCommands.BuyOffer;

public record BuyOfferCommand : IRequest<Result>
{
	public int OfferId { get; init; }
	public int Quantity { get; init; }
}