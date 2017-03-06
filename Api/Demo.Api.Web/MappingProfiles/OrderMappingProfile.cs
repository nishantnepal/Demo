using System.Linq;
using System.Security.Cryptography.Pkcs;
using AutoMapper;
using Demo.Core.Dtos;
using Demo.Core.Models;
using OrderStatus = Demo.Core.Models.OrderStatus;

namespace Demo.Api.Web.MappingProfiles
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Order, OrderModel>()
                .ForMember(c => c.Url, opt => opt.ResolveUsing<OrderUrlResolver>())
                .ReverseMap()
                .ForMember(m => m.Id, opt => opt.Ignore())

                ;

            CreateMap<OrderLine, OrderLineModel>()
               .ReverseMap()
               .ForMember(m => m.Id, opt => opt.Ignore())

               ;
        }
    }
}
