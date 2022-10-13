using CatalogService.Application.Dtos;
using CatalogService.Application.UseCases.Catalog.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[Route("[controller]")]
public class CatalogsController:ControllerBase
{
    private readonly IMediator _mediator;

    public CatalogsController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    public async Task<ActionResult<CategoryDto[]>> GetCategories()
    {
        var categories  = await _mediator.Send(new GetCatalogsQuery());
        return Ok(categories);
    }
}
