using AutoMapper;
using Microsoft.Extensions.Logging;
using Sentinel.Web.Dto.Product;
using Sentinel.Web.Model.Product;
//using Sentinel.Web.Api.Product.Dto;
//using Sentinel.Web.Model.Product;

namespace Sentinel.Web.Api.Product
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductInfo, ProductInfoDtoV1>();
            CreateMap<ProductInfoDtoV1, ProductInfo>().ForMember(m => m.useTabs, opt => { opt.UseValue(false); });


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
