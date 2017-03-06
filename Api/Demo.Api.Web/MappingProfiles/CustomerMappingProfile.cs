using System.Linq;
using System.Security.Cryptography.Pkcs;
using AutoMapper;
using Demo.Core.Dtos;
using Demo.Core.Models;
using OrderStatus = Demo.Core.Models.OrderStatus;

namespace Demo.Api.Web.MappingProfiles
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            CreateMap<Customer, CustomerModel>()
                .ForMember(c => c.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(c => c.FullName, opt => opt.ResolveUsing(src => string.Concat(src.FirstName, " ", src.LastName)))
                .ForMember(c => c.ActiveOrders, opt => opt.ResolveUsing(src => src.Orders?.Count(o => o.OrderStatus != OrderStatus.Cancelled) ?? 0))
                //.ForMember(c => c.ActiveOrders, opt => opt.MapFrom(src=>src.OrdersCount))
                .ForMember(c => c.Url, opt => opt.ResolveUsing<CustomerUrlResolver>())
                .ReverseMap()
                .ForMember(m=>m.Id,opt=>opt.Ignore())

                ;
        }
    }
}
