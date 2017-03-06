using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Core.Models;

namespace Demo.Core.Interfaces
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<Customer> GetCustomerWithOrders(int customerId);
    }
}
