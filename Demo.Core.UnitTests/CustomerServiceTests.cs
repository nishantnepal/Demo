using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Core.Interfaces;
using Demo.Core.Models;
using Demo.Core.Repositories;
using Demo.Core.Services;
using Moq;
using Xunit;

namespace Demo.Core.UnitTests
{
    public class CustomerServiceTests
    {
        [Fact]
        public async Task GetCustomers_ShouldReturn_AllCustomers()
        {
            var data = new List<Customer>
            {
                new Customer {
                    FirstName = "Hello1",
                LastName = "World1"
                },
                new Customer {
                    FirstName = "Hello2",
                LastName = "World2"
                },
                new Customer {
                    FirstName = "Hello3",
                LastName = "World3"
                },
            }.AsQueryable();

            //var mockSet = new Mock<DbSet<Customer>>();
            //mockSet.As<IDbAsyncEnumerable<Customer>>()
            //    .Setup(m => m.GetAsyncEnumerator())
            //    .Returns(new TestDbAsyncEnumerator<Customer>(data.GetEnumerator()));

            //mockSet.As<IQueryable<Customer>>()
            //    .Setup(m => m.Provider)
            //    .Returns(new TestDbAsyncQueryProvider<Customer>(data.Provider));

            //mockSet.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(data.Expression);
            //mockSet.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(data.ElementType);
            //mockSet.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            //var mockContext = new Mock<ReservationContext>();
            //mockContext.Setup(c => c.Customers).Returns(mockSet.Object);


            ////AsNoTracking will fail otherwise 
            ////http://stackoverflow.com/questions/27038253/mock-asnotracking-entity-framework/27087604#27087604
            //mockSet.Setup(x => x.AsNoTracking()).Returns(mockSet.Object);

            //var mockCusRepo = new Mock<ICustomerRepository>();
            //var mockCusRepo = new Mock<CustomerRepository>(mockContext.Object);
            //mockCusRepo.Setup(c => c.Get(null, null, null)).ReturnsAsync(data.ToList());

            var resUow = new Mock<IReservationUnitOfWork>();
            resUow.Setup(c => c.CustomerRepository.Get(null, null, null)).ReturnsAsync(data.ToList());

            var service = new CustomerService(resUow.Object);
            var customers = await service.GetCustomers("");


            Assert.Equal(3, customers.Count);
            Assert.Equal("World1", customers[0].LastName);
            Assert.Equal("World3", customers[2].LastName);
            Assert.Equal("Hello3", customers[2].FirstName);

            //Customer customer = new Customer()
            //{
            //    FirstName = "Hello",
            //    LastName = "World"
            //};
            //CustomerService service = new CustomerService();
            //customer = await service.InsertCustomer(customer);
            //Assert.Equal(true, customer.Id > 0);
        }
    }
}
