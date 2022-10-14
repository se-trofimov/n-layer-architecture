using AutoMapper;
using CartingService.UIContracts;
using Cart = CartingService.DataAccessLayer.Entities.Cart;

namespace CartingService.MappingProfiles
{
    public class CartProfile: Profile
    {
        public CartProfile()
        {
            CreateMap<Cart, UIContracts.Cart>()
                .ReverseMap();
            CreateMap<NewCart, Cart>();
        }
    }
}
