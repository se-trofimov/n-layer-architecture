using CartingService.BusinessLogicLayer;
using CartingService.DataAccessLayer.Entities;
using CartingService.UIContracts;
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
            var changedCart = await _service.AddItemToCart(cartId, item);
            var addedItem = changedCart.items.FirstOrDefault(x => x.Id == item.Id);
            return CreatedAtRoute(nameof(GetItemById), new { cartId, id = item.Id }, addedItem);
        }

        [HttpGet("{id:int}", Name = nameof(GetItemById))]
        public async Task<IActionResult> GetItemById(Guid cartId, int id)
        {
            var cart = await _service.GetCart(cartId);
            var item = cart.items.FirstOrDefault(x => x.Id == id);
            return Ok(item);
        }

        [HttpGet()]
        public async Task<IActionResult> GetItems(Guid cartId)
        {
            var cart = await _service.GetCart(cartId);
            return Ok(cart.items);
        }
    }
}
