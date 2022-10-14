using CartingService.BusinessLogicLayer;
using CartingService.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Item = CartingService.UIContracts.Item;

namespace CartingService.PresentationLayer.Controllers
{
    [Route("Carts/{cartId:guid}/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly ICartService _service;

        public ItemsController(ICartService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpPost]
        public async Task<IActionResult> AddItemToCart(Guid cartId, [FromBody] Item item)
        {
            try
            {
                var changedCart = await _service.AddItemToCart(cartId, item);
                var addedItem = changedCart.items.FirstOrDefault(x => x.Id == item.Id);
                return CreatedAtRoute(nameof(GetItemById), new { cartId, id = item.Id }, addedItem);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Errors);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("{id:int}", Name = nameof(GetItemById))]
        public async Task<IActionResult> GetItemById(Guid cartId, int id)
        {
            try
            {
                var cart = await _service.GetCart(cartId);
                var item = cart.items.FirstOrDefault(x => x.Id == id);
                return Ok(item);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet()]
        public async Task<IActionResult> GetItems(Guid cartId)
        {
            try
            {
                var cart = await _service.GetCart(cartId);
                return Ok(cart.items);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
