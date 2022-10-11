using AutoMapper;
using CartingService.DataAccessLayer.Entities;

namespace CartingService.MappingProfiles
{
    public class CartProfile: Profile
    {
        public CartProfile()
        {
            CreateMap<Cart, UIContracts.Cart>()
                .ReverseMap();
        }
    }
}
