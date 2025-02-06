using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZmitaCart.API.Common;
using ZmitaCart.Application.Commands.CategoryCommands.CreateCategory;
using ZmitaCart.Application.Commands.CategoryCommands.DeleteCategory;
using ZmitaCart.Application.Queries.CategoryQueries.GetAllSuperiors;
using ZmitaCart.Application.Queries.CategoryQueries.GetCategories;
using ZmitaCart.Application.Commands.CategoryCommands.UpdateCategory;
using ZmitaCart.Application.Dtos.CategoryDtos;
using ZmitaCart.Application.Queries.CategoryQueries.GetMostPopularCategories;
using ZmitaCart.Application.Queries.CategoryQueries.GetParentCategory;
using ZmitaCart.Application.Queries.CategoryQueries.GetSuperiorsWithChildren;
using ZmitaCart.Domain.Common.Types;

namespace ZmitaCart.API.Controllers;

[Route("category")]
public class CategoryController : ApiController
{
    public CategoryController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    [RoleAuthorize(Role.administrator)]
    public async Task<ActionResult<int>> CreateCategory([FromBody] CreateCategoryCommand command)
    {
        return await mediator.Send(command).Then(
            s => Created($"category/{s.Value}", s.Value),
            err => StatusCode(err.StatusCode, err.ToList()));
    }

    [HttpGet("getBySuperiorId")]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategoriesBySuperiorId([FromQuery] GetCategoriesBySuperiorIdQuery request)
    {
        return await mediator.Send(request).Then(
            s => Ok(s.Value),
            err => StatusCode(err.StatusCode, err.ToList()));
    }

    [HttpGet("getFewBySuperiorId")]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategoriesBySuperiorId(
        [FromQuery] GetCategoriesWithChildrenBySuperiorIdQuery request)
    {
        return await mediator.Send(request).Then(
            s => Ok(s.Value),
            err => StatusCode(err.StatusCode, err.ToList()));
    }

    [HttpGet("getSuperiorsWithChildren")]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetSuperiorsWithChildren(
        [FromQuery] GetSuperiorsWithChildrenQuery request)
    {
        return await mediator.Send(request).Then(
            s => Ok(s.Value),
            err => StatusCode(err.StatusCode, err.ToList()));
    }

    [HttpGet("getAllSuperiors")]
    public async Task<ActionResult<IEnumerable<SuperiorCategoryDto>>> GetCategoriesBySuperiorId()
    {
        return await mediator.Send(new GetAllSuperiorsQuery()).Then(
            s => Ok(s.Value),
            err => StatusCode(err.StatusCode, err.ToList()));
    }

    [HttpGet("getParentCategory")]
    public async Task<ActionResult<CategoryDto?>> GetParentCategory([FromQuery] GetParentCategoryQuery request)
    {
        return await mediator.Send(request).Then(
            s => Ok(s.Value),
            err => StatusCode(err.StatusCode, err.ToList()));
    }

    [HttpPut("updateCategory")]
    [RoleAuthorize(Role.administrator)]
    public async Task<ActionResult<int>> UpdateCategory([FromBody] UpdateCategoryCommand command)
    {
        return await mediator.Send(command).Then(
            s => Ok(s.Value),
            err => StatusCode(err.StatusCode, err.ToList()));
    }

    [HttpDelete("{id:int}")]
    [RoleAuthorize(Roles = Role.administrator)]
    public async Task<ActionResult> DeleteCategory([FromRoute] int id)
    {
        return await mediator.Send(new DeleteCategoryCommand(id)).Then(
            s => Ok(),
            err => StatusCode(err.StatusCode, err.ToList()));
    }
    
    [HttpGet("getMostPopular")]
    public async Task<ActionResult> GetMostPopularCategories([FromQuery] GetMostPopularCategoriesQuery query)
    {
        return await mediator.Send(query).Then(
            s => Ok(s.Value),
            err => StatusCode(err.StatusCode, err.ToList()));
    }
}