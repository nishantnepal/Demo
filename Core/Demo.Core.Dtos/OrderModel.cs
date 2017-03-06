using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Demo.Core.Dtos
{
    public class OrderModel
    {
        public string Url { get; set; }
        public int Id { get; set; }
        public int CustomerId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public List<OrderLineModel> OrderLines { get; set; }
    }

    public class CustomerModel
    {
        public string Url { get; set; }
        public int Id { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string FullName { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public int ActiveOrders { get; set; }

        public ICollection<Address> Addresses { get; set; }
        //public ICollection<Order> Orders { get; set; }
    }

    public class Address
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
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

    public class OrderLineModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        //public Product Product { get; set; }

        public int Quantity { get; set; }
    }

    //public class AddressType
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}
