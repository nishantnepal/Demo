using System.Collections.Generic;
using Demo.Core.Models;
using FizzWare.NBuilder;

namespace Demo.Core.Repositories.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Demo.Core.Repositories.ReservationContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Demo.Core.Repositories.ReservationContext context)
        {

            //  This method will be called after migrating to the latest version.
            var customers = Builder<Customer>.CreateListOfSize(100).All()
                            .With(c => c.FirstName = Faker.Name.First())
                            .With(c => c.LastName = Faker.Name.Last())
                            .With(c => c.EmailAddress = Faker.Internet.Email())
                            .With(c => c.Phone = Faker.Phone.Number())
                            .Build();
            context.Customers.AddOrUpdate(c => c.Id, customers.ToArray());

            var addresses = Builder<Address>.CreateListOfSize(200)
                        .All()
                        .With(c=>c.AddressType = AddressType.Home)
                        .With(c=>c.Country="USA")
                        .With(c=>c.State=Faker.Address.UsState())
                        .With(c=>c.City=Faker.Address.City())
                        .With(c=>c.PostalCode=Faker.Address.ZipCode())
                        .With(c=>c.City=Faker.Address.City())
                        .With(c=>c.Line1=Faker.Address.StreetAddress())
                        .With(o => o.CustomerId = Pick<Customer>.RandomItemFrom(customers).Id)
                        .Build();

            context.Addresses.AddOrUpdate(o => o.Id, addresses.ToArray());

            var priceGenerator = new RandomGenerator();
            var products = Builder<Product>.CreateListOfSize(50)
                .All()
                    .With(p => p.Name = "Product " + p.Id.ToString())
                    .With(p => p.Description = Faker.Lorem.Paragraph())
                    .With(p => p.Price = priceGenerator.Next(50, 500))
                .Build();

            context.Products.AddOrUpdate(p => p.Id, products.ToArray());


            var daysGenerator = new RandomGenerator();
            var orders = Builder<Order>.CreateListOfSize(200)
                .All()
                    .With(o => o.Customer = Pick<Customer>.RandomItemFrom(customers))
                    .With(o => o.OrderDate = DateTime.Now.AddDays(-daysGenerator.Next(1, 100)))
                .Build();

            context.Orders.AddOrUpdate(o => o.Id, orders.ToArray());

            var itemCountGenerator = new RandomGenerator();
            var quantityGenerator = new RandomGenerator();

            foreach (Order o in orders)
            {
                var orderItems = Builder<OrderLine>.CreateListOfSize(itemCountGenerator.Next(1, 10))
                   .All()
                       .With(oi => oi.OrderId = o.Id)
                       .With(oi => oi.ProductId = Pick<Product>.RandomItemFrom(products).Id)
                       .With(oi => oi.Quantity = quantityGenerator.Next(1, 10))
                   .Build();

                context.OrderLines.AddOrUpdate(oi => oi.Id, orderItems.ToArray());
            }

           



            //context.Customers.AddOrUpdate(c=>c.Id,
            //    new Customer()
            //    {
            //        Id = 1,
            //        FirstName = "Nishant",
            //        LastName = "Nepal",
            //        Addresses = new List<Address>()
            //        {
            //            new Address() {AddressType = AddressType.Home,City = "Rockville",Country = "USA",CustomerId = 1,Line1 = "Home Address,Apt 1",PostalCode = "1234",State = "MD"},
            //            new Address() {AddressType = AddressType.Business,City = "Washington",Country = "USA",CustomerId = 1,Line1 = "Dupont Circle",PostalCode = "1235",State = "DC"},
            //        } ,
            //        EmailAddress = "nnepal@demo.com"
            //    },
            //     new Customer()
            //     {
            //         Id = 1,
            //         FirstName = "John",
            //         LastName = "Doe",
            //         Addresses = new List<Address>()
            //        {
            //            new Address() {AddressType = AddressType.Home,City = "Rockville",Country = "USA",CustomerId = 1,Line1 = "Home Address,Apt 1",PostalCode = "1234",State = "MD"},
            //            new Address() {AddressType = AddressType.Business,City = "Washington",Country = "USA",CustomerId = 1,Line1 = "Dupont Circle",PostalCode = "1235",State = "DC"},
            //        },
            //         EmailAddress = "nnepal@demo.com"
            //     }
            //     );

            //context.Orders.AddOrUpdate(p => p.Id,
            //    new Order()
            //    {
            //        Id = 1,

            //    },
            //    new Order()
            //    {

            //    });
        }
    }
}
