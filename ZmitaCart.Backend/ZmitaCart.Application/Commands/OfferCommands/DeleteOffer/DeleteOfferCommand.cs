using FluentResults;
using MediatR;

namespace ZmitaCart.Application.Commands.OfferCommands.DeleteOffer;

public record DeleteOfferCommand(int Id) : IRequest<Result>;