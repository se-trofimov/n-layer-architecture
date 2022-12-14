using System.Text.Json;
using CatalogService.Application.Common;
using CatalogService.Application.Dtos;
using CatalogService.Application.Notifications;
using CatalogService.Application.UseCases.Items.Commands;
using CatalogService.Application.UseCases.Items.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace API.Controllers;

[Authorize]
[Route("categories/{categoryId:int}/items")]
public class ItemsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly LinkGenerator _linkGenerator;

    public ItemsController(IMediator mediator, LinkGenerator linkGenerator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
    }

    [AllowAnonymous]
    [HttpGet(Name = nameof(GetItems))]
    public async Task<ActionResult<PagedList<ItemDto>>> GetItems(int categoryId, QueryParams queryParams)
    {
        var result = await _mediator.Send(new GetItemsQuery(categoryId, queryParams.PageNumber, queryParams.PageSize));
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

        var listOfWrappedItems = new List<LinkWrapper<ItemDto>>(result.Count);
        foreach (var item in result)
        {
            var categoryLinks = CreateLinksForItem(categoryId, item.Id);
            listOfWrappedItems.Add(new LinkWrapper<ItemDto>(item, categoryLinks));
        }

        var categoriesLinks = GenerateItemsLinks(categoryId);
        var response = new LinkWrapper<List<LinkWrapper<ItemDto>>>(listOfWrappedItems, categoriesLinks);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("{id:int}", Name = nameof(GetItemById))]
    public async Task<ActionResult<ItemDto>> GetItemById(int categoryId, int id)
    {
        var item = await _mediator.Send(new GetItemByIdQuery(id, categoryId));
        var mediaType = HttpContext.Request.Headers["Accept"].FirstOrDefault();
        MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue? outMediaType);

        if (!outMediaType!.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase))
        {
            return Ok(item);
        }

        var itemLinks = CreateLinksForItem(categoryId, item.Id);
        var response = new LinkWrapper<ItemDto>(item, itemLinks);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("{id:int}/properties", Name = nameof(GetItemProperties))]
    public async Task<ActionResult<ItemDto>> GetItemProperties(int categoryId, int id)
    {
        var items = await _mediator.Send(new GetItemPropertiesQuery(){Id = id});
        return Ok(items);
    }

    [Authorize(policy: "Items.Create")]
    [HttpPost(Name = nameof(CreateItem))]
    public async Task<ActionResult<ItemDto>> CreateItem(int categoryId, [FromBody] CreateItemCommand command)
    {
        command.CategoryId = categoryId;
        var item = await _mediator.Send(command);

        var mediaType = HttpContext.Request.Headers["Accept"].FirstOrDefault();
        MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue? outMediaType);

        if (!outMediaType!.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase))
        {
            return CreatedAtRoute(nameof(GetItemById), new { categoryId, item.Id }, item);
        }

        var itemLinks = CreateLinksForItem(categoryId, item.Id);
        var response = new LinkWrapper<ItemDto>(item, itemLinks);
        return CreatedAtRoute(nameof(GetItemById), new { categoryId, id = item.Id }, response);
    }

    [Authorize(policy: "Items.Update")]
    [HttpPut("{id:int}", Name = nameof(ChangeItem))]
    public async Task<ActionResult> ChangeItem(int categoryId, int id, [FromBody] ChangeItemCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        await _mediator.Publish(new ItemHasBeenChangedNotification(command.Id, command.Name, command.Image, command.Price));
        var updated = await _mediator.Send(new GetItemByIdQuery(id, categoryId));
        return Ok(updated);
    }

    [Authorize(policy: "Items.Delete")]
    [HttpDelete("{id:int}", Name = nameof(DeleteItem))]
    public async Task<ActionResult> DeleteItem(int categoryId, int id)
    {
        var result = await _mediator.Send(new DeleteItemCommand(id, categoryId));
        return NoContent();
    }

    private List<Link> GenerateItemsLinks(int categoryId)
    {
        var links = new List<Link>
        {
            new Link(_linkGenerator.GetUriByAction(HttpContext,  nameof(GetItems), values: new {categoryId }),
                "self",
                "GET"),
        };
        return links;
    }

    private List<Link> CreateLinksForItem(int categoryId, int id)
    {
        var links = new List<Link>
        {
            new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetItemById), values: new {categoryId, id }),
            "self",
                "GET"),
            new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetItemById), values: new {categoryId, id }),
                "properties",
                "GET"),
            new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(DeleteItem), values: new {categoryId, id }),
                "delete_item",
                "DELETE"),
            new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(ChangeItem), values: new {categoryId, id }),
                "update_item",
                "PUT")
        };
        return links;
    }
}
