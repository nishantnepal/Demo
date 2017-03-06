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
    public class CustomerUrlResolver : IValueResolver<Customer, CustomerModel, string>
    {
        private readonly IHttpContextAccessor _accessor;

        public CustomerUrlResolver(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
        public string Resolve(Customer source, CustomerModel destination, string destMember, ResolutionContext context)
        {
            var url = (IUrlHelper)_accessor.HttpContext.Items[BaseController.URLHELPER];
            return url.Link("CustomerGet", new { customerId = source.Id });
        }
    }
}
