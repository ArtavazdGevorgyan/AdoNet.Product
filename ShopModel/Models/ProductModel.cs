using System;
using System.Collections.Generic;
using System.Text;

namespace AdoNet.Product.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
    }
}
