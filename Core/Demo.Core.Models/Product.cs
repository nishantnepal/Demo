using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ProductCategory ProductCategory { get; set; }
    }

    
    //public class ProductCategory
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}

    public enum ProductCategory
    {
        Fruits,
        Vegetables
    }
}

