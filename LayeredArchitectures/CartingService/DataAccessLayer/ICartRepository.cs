using CartingService.DataAccessLayer.Entities;

namespace CartingService.DataAccessLayer;

public interface ICartRepository
{
    Task<Cart> AddAsync(Cart cart);
    Task RemoveAsync(Cart cart);
    Task<Cart> UpdateAsync(Cart cart);
    Task<Cart> GetItemById(Guid id);
}