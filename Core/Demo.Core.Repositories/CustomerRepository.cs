using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Demo.Core.Interfaces;
using Demo.Core.Models;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Demo.Core.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ReservationContext context, ILogger<Customer> logger) : base(context, logger)
        {

        }

        public async Task<Customer> GetCustomerWithOrders(int customerId)
        {
            var result = await base.Get(filter: m => m.Id == customerId, orderBy: null, includeProperties: "Orders");
            return result.FirstOrDefault();
        }

        public override Task<List<Customer>> Get(Expression<Func<Customer, bool>> filter = null, Func<IQueryable<Customer>, IOrderedQueryable<Customer>> orderBy = null, string includeProperties = "")
        {
            if (!string.IsNullOrEmpty(includeProperties))
            {
                includeProperties = includeProperties + ",Orders";
            }
            else
            {
                includeProperties = "Orders";
            }
            return base.Get(filter, orderBy, includeProperties);

        }

        public override Customer Insert(Customer entity)
        {
            Context.Database.Log = msg => Logger?.LogInformation(2, msg);
            return base.Insert(entity);
        }
    }
}
