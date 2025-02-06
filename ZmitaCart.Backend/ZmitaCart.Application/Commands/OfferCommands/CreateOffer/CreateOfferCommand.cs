using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ZmitaCart.Application.Commands.OfferCommands.CreateOffer;

public record CreateOfferCommand : IRequest<Result<int>>
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Condition { get; set; } = null!;
    public int CategoryId { get; set; }
    public IEnumerable<IFormFile>? Pictures { get; set; }
}