using CartingService.BusinessLogicLayer;
using CartingService.Exceptions;
using CartingService.UIContracts;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CartingService.PresentationLayer.Controllers
{
    [ApiController]
    [Tags("Carts")]
    [Route("api/v{version:apiVersion}/carts")]
    [ApiVersion("1.0")]
    public class CartsV1Controller : ControllerBase
    {
        private readonly ICartService _service;

        public CartsV1Controller(ICartService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCart([FromBody] NewCart cart)
        {
            var created = await _service.AddCartAsync(cart);
            return CreatedAtRoute(nameof(GetCartByIdV1), new { created.Id }, created);
        }

        [HttpGet("{id:guid}", Name = nameof(GetCartByIdV1))]
        public async Task<IActionResult> GetCartByIdV1(Guid id)
        {
            try
            {
                var cart = await _service.GetCartAsync(id);
                return Ok(cart);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost("{cartId:guid}")]
        public async Task<IActionResult> AddItemToCart(Guid cartId, [FromBody] Item item)
        {
            try
            {
                var changedCart = await _service.AddItemToCartAsync(cartId, item);
                return Ok();
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

        [HttpDelete("{cartId:guid}")]
        public async Task<IActionResult> AddItemToCart(Guid cartId, int itemId)
        {
            try
            {
                await _service.DeleteItemAsync(cartId, itemId);
                return Ok();
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
    }
}
