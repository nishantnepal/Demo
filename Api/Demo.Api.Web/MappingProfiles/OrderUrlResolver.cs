using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Api.Web.Controllers;
using Demo.Core.Dtos;
using Demo.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Web.MappingProfiles
{
    public class OrderUrlResolver : IValueResolver<Order, OrderModel, string>
    {
        private readonly IHttpContextAccessor _accessor;

        public OrderUrlResolver(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string Resolve(Order source, OrderModel destination, string destMember, ResolutionContext context)
        {
            var url = (IUrlHelper)_accessor.HttpContext.Items[BaseController.URLHELPER];
            return url.Link("OrderGet", new { id = source.Id, customerId = source.CustomerId });
        }
    }
}
