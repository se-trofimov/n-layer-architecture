using CartingService.UIContracts;

namespace CartingService.BusinessLogicLayer;

public interface ICartService
{
    Task<Cart> AddCartAsync(NewCart cart);
    Task<Cart> AddItemToCartAsync(Guid id, Item item);
    Task<Cart> GetCartAsync(Guid id);
    Task DeleteItemAsync(Guid cartId, int itemId);
}