﻿using CartingService.BusinessLogicLayer;
using CartingService.UIContracts;
using Microsoft.AspNetCore.Mvc;

namespace CartingService.PresentationLayer.Controllers
{
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly CartService _service;

        public CartController(CartService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCart([FromBody] Cart cart)
        {
            var created = await _service.AddCart(cart);
            return CreatedAtRoute(nameof(GetCartById), new { created.Id }, created);
        }

        [HttpGet("{id:guid}", Name = nameof(GetCartById))]
        public async Task<IActionResult> GetCartById(Guid id)
        {
            var cart = await _service.GetCart(id);
            return Ok(cart);
        }
    }

}
