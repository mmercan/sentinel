using AutoMapper;
using Microsoft.Extensions.Logging;
using Sentinel.Model.Product.Dto;
using Sentinel.Model.Product;
//using Sentinel.Api.Product.Dto;
//using Sentinel.Model.Product;

namespace Sentinel.Api.Product
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductInfo, ProductInfoDtoV1>();
            CreateMap<ProductInfoDtoV1, ProductInfo>().ForMember(m => m.UseTabs, opt => { opt.MapFrom(src => false); });


            CreateMap<ProductInfo, ProductInfoDtoV2>();
            CreateMap<ProductInfoDtoV2, ProductInfo>(); // .ForMember(m => m.useTabs, opt => { opt.UseValue(false); });
            //  CreateMap<ProductInfo, ProductInfoV1>()
            //  .ForMember(dest=>dest.)
            //CreateMap<ProductInfoDtoV1, ProductInfo>();
            // .ForMember(dest=>dest.useTabs, opt=>opt )
            //.ForMember(dest => dest., opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
            //CreateMap<ProductInfo, ProductInfoV1>().ForMember(dest => dest., opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
        }
    }
}
