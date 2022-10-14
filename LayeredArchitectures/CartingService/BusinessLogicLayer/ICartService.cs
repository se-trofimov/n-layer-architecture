using CartingService.UIContracts;

namespace CartingService.BusinessLogicLayer;

public interface ICartService
{
    Task<Cart> AddCart(NewCart cart);
    Task<Cart> AddItemToCart(Guid id, Item item);
    Task<Cart> GetCart(Guid id);
}