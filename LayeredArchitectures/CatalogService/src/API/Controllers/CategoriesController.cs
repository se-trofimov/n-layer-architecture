using System.Text.Json;
using AutoMapper;
using CatalogService.Application.Common;
using CatalogService.Application.Dtos;
using CatalogService.Application.UseCases.Catalog.Commands;
using CatalogService.Application.UseCases.Catalog.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[Route("categories")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<CategoryDto>>> GetCategories(QueryParams queryParams)
    {
        var result = await _mediator.Send(new GetCatalogsQuery(queryParams.PageNumber, queryParams.PageSize));
        var metadata = new
        {
            result.TotalCount,
            result.PageSize,
            result.CurrentPage,
            result.TotalPages,
            result.HasNext,
            result.HasPrevious
        };
        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));
        return Ok(result);
    }

    [HttpGet("{id:int}", Name = nameof(GetCategoryById))]
    public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
    {
        var categories = await _mediator.Send(new GetCatalogByIdQuery(id));
        return Ok(categories);
    }

    [HttpPut("{id:int}")]

    public async Task<ActionResult> ChangeCategory([FromBody] ChangeCategoryCommand command, int id)
    {
        command.Id = id;
        await _mediator.Send(command);
        return Ok();
    }

    [HttpDelete("{id:int}")]

    public async Task<ActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteCategoryCommand(id));
        return NoContent();
    }

    [HttpPost()]
    public async Task<ActionResult<CategoryDto>> Create([FromBody]CreateCategoryCommand command)
    {
        var result =  await _mediator.Send(command);
        return CreatedAtRoute(nameof(GetCategoryById), new {id = result.Id}, result);
    }
}
