using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Models
{
    public class Order : IEntity
    {
        public Order()
        {
            OrderLines = new List<OrderLine>();
        }
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public ICollection<OrderLine> OrderLines { get; set; }
        public ICollection<IDomainEvent> Events { get; }
    }

    public class Customer
    {
        public Customer()
        {
            //Orders = new List<Order>();
            Addresses = new List<Address>();
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }

        //public int OrdersCount { get; set; }

        public ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }

    public class Address
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public AddressType AddressType { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }

    public enum AddressType
    {
        Home,
        Business
    }

    public enum OrderStatus
    {
        Active,
        Invoiced,
        Cancelled
    }

    public class OrderLine
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }

    //public class AddressType
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}
