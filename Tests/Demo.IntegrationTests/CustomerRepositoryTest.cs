using System;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Demo.Core.Models;
using Demo.Core.Repositories;
using Xunit;

namespace Demo.IntegrationTests
{

    public class CustomerRepositoryTest
    {
        public CustomerRepositoryTest()
        {
            // System.Configuration.ConfigurationManager.AppSettings[""];
        }
        [Fact]
        public async Task Get_Count_GreaterThanZero()
        {
            CustomerRepository repository = new CustomerRepository(new ReservationContext(),null);
            var customers = await repository.Get();
            Assert.NotEqual(customers.Count(), 0);
        }

        [Fact]
        public async Task GetCustomerWithOrders_Should_Return_Orders()
        {
            CustomerRepository repository = new CustomerRepository(new ReservationContext(), null);
            var customers = await repository.Get(m => m.Orders.Count > 0, null, "Orders");
            int customerId = customers[0].Id;
            var customer = await repository.GetCustomerWithOrders(customerId);
            Assert.NotEqual(0, customer.Orders.Count);

        }

        [Fact]
        public async Task Insert_Should_Return_NewCustomerWIthId()
        {
            var context = new ReservationContext();
            CustomerRepository repository = new CustomerRepository(context, null);
            Customer customer = new Customer()
            {
                FirstName = "Nishant",
                LastName = "Nepal",
                EmailAddress = "n@n.com"
            };

            repository.Insert(customer);
            await context.SaveChangesAsync();


            Assert.NotNull(customer.Id);
            Assert.NotEqual(0, customer.Id);
            Assert.Equal(true, customer.Id > 0);

        }

        [Fact]
        public async Task Update_Should_UpdateInfo()
        {
            var context = new ReservationContext();
            CustomerRepository repository = new CustomerRepository(context, null);
            var customer = await repository.GetById(1);
            customer.FirstName = "UpdatedFirst";
            customer.LastName = "UpdatedLast";
            repository.Update(customer);
            await context.SaveChangesAsync();

            customer = await repository.GetById(1);

            Assert.Equal("UpdatedFirst", customer.FirstName);
            Assert.Equal("UpdatedLast", customer.LastName);

        }

    }
}
