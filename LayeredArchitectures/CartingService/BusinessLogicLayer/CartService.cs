using AutoMapper;
using CartingService.DataAccessLayer;
using CartingService.Exceptions;
using CartingService.UIContracts;
using CartingService.Validators;
using FluentValidation;
using Cart = CartingService.UIContracts.Cart;
using Item = CartingService.UIContracts.Item;

namespace CartingService.BusinessLogicLayer
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        private readonly ItemsValidator _itemsValidator;

        public CartService(ICartRepository cartRepository, IMapper mapper, ItemsValidator validator)
        {
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _itemsValidator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<Cart> AddCart(NewCart cart)
        {
            var newCart = _mapper.Map<DataAccessLayer.Entities.Cart>(cart);
            var created = await _cartRepository.AddAsync(newCart);

            return _mapper.Map<Cart>(created);
        }

        public async Task<Cart> AddItemToCart(Guid id, Item item)
        {
            var result = _itemsValidator.Validate(item);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);
            
            var cart = await _cartRepository.GetItemById(id);
            cart.AddItem(_mapper.Map<DataAccessLayer.Entities.Item>(item));
            var updated = await _cartRepository.UpdateAsync(cart);
            return _mapper.Map<Cart>(updated);
        }

        public async Task<Cart> GetCart(Guid id)
        {
            var cart = await _cartRepository.GetItemById(id);
            return _mapper.Map<Cart>(cart);
        }
 
    }
}
