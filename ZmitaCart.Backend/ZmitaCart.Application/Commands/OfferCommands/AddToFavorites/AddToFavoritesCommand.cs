using FluentResults;
using MediatR;

namespace ZmitaCart.Application.Commands.OfferCommands.AddToFavorites;

public record AddToFavoritesCommand(int Id) : IRequest<Result>;