using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZmitaCart.API.Common;
using ZmitaCart.Application.Commands.OfferCommands.AddToFavorites;
using ZmitaCart.Application.Commands.OfferCommands.BuyOffer;
using ZmitaCart.Application.Commands.OfferCommands.CreateOffer;
using ZmitaCart.Application.Commands.OfferCommands.DeleteOffer;
using ZmitaCart.Application.Commands.OfferCommands.UpdateOffer;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Queries.OfferQueries.GetBoughtOffers;
using ZmitaCart.Application.Queries.OfferQueries.GetFavouritesOffers;
using ZmitaCart.Application.Queries.OfferQueries.GetFavouritesOffersCount;
using ZmitaCart.Application.Queries.OfferQueries.GetOffer;
using ZmitaCart.Application.Queries.OfferQueries.GetOfferData;
using ZmitaCart.Application.Queries.OfferQueries.GetOffersByCategoriesName;
using ZmitaCart.Application.Queries.OfferQueries.SearchOffers;

namespace ZmitaCart.API.Controllers;

[Route("offer")]
public class OfferController : ApiController
{
    public OfferController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    [RoleAuthorize]
    public async Task<ActionResult<int>> CreateOffer([FromForm] CreateOfferCommand command)
    {
        return await mediator.Send(command).Then(
            s => Ok(s.Value),
            err => StatusCode(err.StatusCode, err.ToList()));
    }

    [HttpPut]
    [RoleAuthorize]
    public async Task<ActionResult<int>> UpdateOffer([FromForm] UpdateOfferCommand command)
    {
        return await mediator.Send(command).Then(
            s => Ok(s.Value),
            err => StatusCode(err.StatusCode, err.ToList()));
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<OfferInfoDto>>> SearchOffers([FromQuery] SearchOffersQuery query)
    {
        return await mediator.Send(query).Then(
            s => Ok(s.Value),
            err => StatusCode(err.StatusCode, err.ToList()));
    }
    
    [HttpDelete("{id:int}")]
    [RoleAuthorize]
    public async Task<ActionResult> DeleteOffer([FromRoute] int id)
    {
        return await mediator.Send(new DeleteOfferCommand(id)).Then(
            s => Ok(),
            err => StatusCode(err.StatusCode, err.ToList()));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OfferDto>> GetOffer([FromRoute] int id)
    {
        return await mediator.Send(new GetOfferQuery(id)).Then(
            s => Ok(s.Value),
            err => StatusCode(err.StatusCode, err.ToList()));
    }

    [HttpGet("data/{id:int}")]
    public async Task<ActionResult<OfferDataDto>> GetOfferData([FromRoute] int id)
    {
        return await mediator.Send(new GetOfferDataQuery(id)).Then(
            s => Ok(s.Value),
            err => StatusCode(err.StatusCode, err.ToList()));
    }
    
    [HttpPost("addToFavorites/{id:int}")]
    [RoleAuthorize]
    public async Task<ActionResult> AddToFavorites([FromRoute] int id)
    {
        return await mediator.Send(new AddToFavoritesCommand(id)).Then(
            s => Ok(),
            err => StatusCode(err.StatusCode, err.ToList()));
    }
    
    [HttpGet("favorites")]
    [RoleAuthorize]
    public async Task<ActionResult<PaginatedList<OfferInfoDto>>> GetFavoritesOffers([FromQuery] GetFavouritesOffersQuery query)
    {
        return await mediator.Send(query).Then(
            s => Ok(s.Value),
            err => StatusCode(err.StatusCode, err.ToList()));
    }
    
    [HttpGet("favorites/count")]
    [RoleAuthorize]
    public async Task<ActionResult<int>> GetFavoritesOffersCount()
    {
        return await mediator.Send(new GetFavouritesOfferCountQuery()).Then(
            s => Ok(s.Value),
            err => StatusCode(err.StatusCode, err.ToList()));
    }
    
    [HttpPost("buy")]
    [RoleAuthorize]
    public async Task<ActionResult> BuyOffer([FromBody] BuyOfferCommand command)
    {
        return await mediator.Send(command).Then(
            s => Ok(),
            err => StatusCode(err.StatusCode, err.ToList()));
    }
    
    [HttpGet("bought")]
    [RoleAuthorize]
    public async Task<ActionResult<PaginatedList<BoughtOfferDto>>> GetBoughtOffers([FromQuery] GetBoughtOffersQuery query)
    {
        return await mediator.Send(query).Then(
            s => Ok(s.Value),
            err => StatusCode(err.StatusCode, err.ToList()));
    }
    
    [HttpGet("byCategoriesName")]
    public async Task<ActionResult<List<NamedList<string, OfferInfoDto>>>> GetOffersByCategoriesNameQuery([FromQuery] GetOffersByCategoriesNameQuery query)
    {
        return await mediator.Send(query).Then(
            s => Ok(s.Value),
            err => StatusCode(err.StatusCode, err.ToList()));
    }
}