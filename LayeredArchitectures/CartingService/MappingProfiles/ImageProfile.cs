using AutoMapper;
using CartingService.DataAccessLayer.ValueObjects;

namespace CartingService.MappingProfiles
{
    public class ImageProfile: Profile
    {
        public ImageProfile()
        {
            CreateMap<Image, UIContracts.Image>()
                .ReverseMap();
        }
    }
}
