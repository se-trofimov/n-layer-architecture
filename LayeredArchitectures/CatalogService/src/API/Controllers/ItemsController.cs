﻿using AutoMapper;
using CatalogService.Application.Dtos;
using CatalogService.Application.UseCases.Items.Commands;
using CatalogService.Application.UseCases.Items.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("categories/{categoryId:int}/items")]
public class ItemsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ItemsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public async Task<ActionResult<ItemDto[]>> GetItems(int categoryId)
    {
        var result = await _mediator.Send(new GetItemsQuery(categoryId));
        return Ok(result);
    }

    [HttpGet("{id:int}", Name = nameof(GetItemById))]
    public async Task<ActionResult<ItemDto>> GetItemById(int categoryId, int id)
    {
        var result = await _mediator.Send(new GetItemByIdQuery(id, categoryId));
        return Ok(result);
    }


    [HttpPost]
    public async Task<ActionResult<ItemDto>> CreateItem(int categoryId, [FromBody] CreateItemCommand command)
    {
        command.CategoryId = categoryId;
        var result = await _mediator.Send(command);
        return CreatedAtRoute(nameof(GetItemById), new { categoryId, result.Id }, result);
    }

    [HttpPut]
    public async Task<ActionResult> ChangeItem(int categoryId, [FromBody] ChangeItemCommand command)
    {
        var result = await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteItem(int categoryId, int id)
    {
        var result = await _mediator.Send(new DeleteItemCommand(id, categoryId));
        return NoContent();
    }
}