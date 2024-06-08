using AutoMapper;
using API.Models;
using API.Models.Dto;

namespace API
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<OrderDetail, OrderDetailDTO>();
            CreateMap<OrderHeader, ShipOrderDTO>();
            CreateMap<Product, ProductDTO>();

            //  CreateMap<OrderDetail, VillaCreateDTO>().ReverseMap();
            //CreateMap<OrderDetail, VillaUpdateDTO>().ReverseMap();


            //  CreateMap<VillaNumber, VillaNumberDTO>().ReverseMap();
            //CreateMap<VillaNumber, VillaNumberCreateDTO>().ReverseMap();
            //CreateMap<VillaNumber, VillaNumberUpdateDTO>().ReverseMap();
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
        }
    }
}
