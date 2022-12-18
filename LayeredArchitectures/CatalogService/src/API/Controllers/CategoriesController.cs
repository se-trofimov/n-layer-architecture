using System.Text.Json;
using CatalogService.Application.Common;
using CatalogService.Application.Dtos;
using CatalogService.Application.UseCases.Catalog.Commands;
using CatalogService.Application.UseCases.Catalog.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace API.Controllers;

[Authorize]
[Route("categories")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly LinkGenerator _linkGenerator;

    public CategoriesController(IMediator mediator, LinkGenerator linkGenerator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
    }

    [AllowAnonymous]
    [HttpGet(Name = nameof(GetCategories))]
    [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
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

        var mediaType = HttpContext.Request.Headers["Accept"].FirstOrDefault();
        MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue? outMediaType);

        if (!outMediaType!.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase))
        {
            return Ok(result);
        }

        var listOfWrappedCategories = new List<LinkWrapper<CategoryDto>>(result.Count);


        foreach (var category in result)
        {
            var categoryLinks = CreateLinksForCategory(category.Id);
            listOfWrappedCategories.Add(new LinkWrapper<CategoryDto>(category, categoryLinks));
        }

        var categoriesLinks = GenerateCategoriesLinks();
        var response = new LinkWrapper<List<LinkWrapper<CategoryDto>>>(listOfWrappedCategories, categoriesLinks);
        return Ok(response);
    }

    private List<Link> GenerateCategoriesLinks()
    {
        var links = new List<Link>
        {
            new Link(_linkGenerator.GetUriByAction(HttpContext,  nameof(GetCategories)),
                "self",
                "GET"),
        };
        return links;
    }

    private List<Link> CreateLinksForCategory(int id)
    {
        var links = new List<Link>
        {
            new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetCategoryById), values: new { id }),
                "self",
                "GET"),
            new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(DeleteCategory), values: new { id }),
                "delete_category",
                "DELETE"),
            new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(ChangeCategory), values: new { id }),
                "update_category",
                "PUT")
        };
        return links;
    }

    [AllowAnonymous]
    [HttpGet("{id:int}", Name = nameof(GetCategoryById))]
    [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
    public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
    {
        var category= await _mediator.Send(new GetCategoryByIdQuery(id));

        var mediaType = HttpContext.Request.Headers["Accept"].FirstOrDefault();
        MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue? outMediaType);

        if (!outMediaType!.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase))
        {
            return Ok(category);
        }

        var categoryLinks = CreateLinksForCategory(category.Id);
        var response = new  LinkWrapper<CategoryDto>(category, categoryLinks);

        return Ok(response);
    }

    [Authorize(policy: "Category.Update")]
    [HttpPut("{id:int}", Name = nameof(ChangeCategory))]
    public async Task<ActionResult> ChangeCategory([FromBody] ChangeCategoryCommand command, int id)
    {
        command.Id = id;
        await _mediator.Send(command);
        var updated = await _mediator.Send(new GetCategoryByIdQuery(id));
        return Ok(updated);
    }

    [Authorize(policy: "Category.Delete")]
    [HttpDelete("{id:int}", Name = nameof(DeleteCategory))]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        await _mediator.Send(new DeleteCategoryCommand(id));
        return NoContent();
    }

    [Authorize(policy: "Category.Create")]
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryCommand command)
    {
        var category = await _mediator.Send(command);

        var mediaType = HttpContext.Request.Headers["Accept"].FirstOrDefault();
        MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue? outMediaType);

        if (!outMediaType!.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase))
        {
            return CreatedAtRoute(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        var categoryLinks = CreateLinksForCategory(category.Id);
        var response = new LinkWrapper<CategoryDto>(category, categoryLinks);

        return CreatedAtRoute(nameof(GetCategoryById), new { id = category.Id }, response);
    }
}
