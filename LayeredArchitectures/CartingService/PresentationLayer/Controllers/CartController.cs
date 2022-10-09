using CartingService.DataAccessLayer;
using CartingService.UIContracts;
using Microsoft.AspNetCore.Mvc;

namespace CartingService.PresentationLayer.Controllers
{
    [Route("[controller]")]
    public class CartController: ControllerBase
    {
           
        public CartController()
        {
            
        }

        [HttpPost]
        public async Task<IActionResult> CreateCart(Cart cart)
        {
            
            return CreatedAtRoute("", cart);
        }
    }
}
