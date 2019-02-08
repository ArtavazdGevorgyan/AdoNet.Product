using AdoNet.Product.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Logic
{
    public class Logic
    {
        private readonly ProductContext db = null;
        public Logic()
        {
            db = new ProductContext();
        }
        public List<ProductModel> GetAllProducts()
        {
            List<ProductModel> products = new List<ProductModel>();
            products = db.GetProducts();
            foreach (ProductModel product in products)
            {
                if (product.Id == 0)
                {
                    product.Name = "Invalid";
                    product.Price = 0;
                    product.Image = "Invalid";
                }
            }
            return products;
        }
        public void InsertNewProduct(ProductModel product)
        {
            if (!string.IsNullOrWhiteSpace(product.Name) || product.Price > 0)
            {
                db.InsertProduct(product);
            }
            else
            {
                throw new Exception("Cannot insert null value");
            }
        }

        public void DeleteSelectedProduct(int id)
        {
            if (id != 0)
            {
                db.DeleteProduct(id);
            }
            else
            {
                throw new Exception("Cannot delete");
            }
        }

        public bool EditProductExists(int id)
        {
            bool prodexists = db.ProductExists(id);
            return prodexists;
        }

        public ProductModel FoundProduct(int id)
        {
            ProductModel returning = db.FindProduct(id);
            return returning;
        }

        public void SaveEditedProduct(ProductModel product)
        {
            db.SaveUpdatedProduct(product);
        }


    }
}
