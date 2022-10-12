using AutoMapper;
using CartingService.DataAccessLayer;
using CartingService.Exceptions;
using CartingService.UIContracts;
using Cart = CartingService.UIContracts.Cart;
using Item = CartingService.UIContracts.Item;

namespace CartingService.BusinessLogicLayer
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public CartService(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Cart> AddCart(NewCart cart)
        {
            var newCart = _mapper.Map<DataAccessLayer.Entities.Cart>(cart);
            var created = await _cartRepository.AddAsync(newCart);

            return _mapper.Map<Cart>(created);
        }

        public async Task<Cart> AddItemToCart(Guid id, Item item)
        {
            var cart = await _cartRepository.GetItemById(id);
            if (cart is null)
                throw new NotFoundException($"Cart with Id {id} not exists");
            cart.AddItem(_mapper.Map<DataAccessLayer.Entities.Item>(item));
            var updated = await _cartRepository.UpdateAsync(cart);
            return _mapper.Map<Cart>(updated);
        }

        public async Task<Cart> GetCart(Guid id)
        {
            var cart = await _cartRepository.GetItemById(id);
            if (cart is null)
                throw new NotFoundException($"Cart with Id {id} not exists");
            return _mapper.Map<Cart>(cart);
        }
 
    }
}
