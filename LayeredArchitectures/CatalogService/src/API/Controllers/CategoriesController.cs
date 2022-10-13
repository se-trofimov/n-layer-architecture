using AutoMapper;
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
    private readonly IMapper _mapper;

    public CategoriesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public async Task<ActionResult<CategoryDto[]>> GetCategories()
    {
        var categories = await _mediator.Send(new GetCatalogsQuery());
        return Ok(categories);
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
