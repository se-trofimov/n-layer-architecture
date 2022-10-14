using AutoMapper;
using CartingService.DataAccessLayer.Entities;

namespace CartingService.MappingProfiles
{
    public class ItemProfile: Profile
    {
        public ItemProfile()
        {
            CreateMap<Item, UIContracts.Item>()
                .ReverseMap();
        }
    }
}
