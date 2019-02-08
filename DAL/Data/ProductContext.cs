using DAL;
using DAL.Extentions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AdoNet.Product.Models
{
    public class ProductContext : BaseDbProxy
    {
        public ProductContext() : base()
        {

        }

        public List<ProductModel> GetProducts()
        {
            List<ProductModel> products = new List<ProductModel>();
            string queryString = "SELECT Id, Name, Price, Image from dbo.BaseTable";
            SqlCommand command = null;
            SqlDataReader reader = null;
            try
            {
                this.Open();
                command = new SqlCommand(queryString, this.connection);
                reader = command.ExecuteReader();
                products = reader.DataReaderMapToList<ProductModel>();
                reader.Close();
                command.Dispose();
                this.Close();
            }
            catch (Exception ex)
            {
                reader.Close();
                command.Dispose();
                this.Close();
                //log it
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                this.Dispose();
            }
            return products;
        }

        public void InsertProduct(ProductModel product)
        {
            string query = "INSERT INTO dbo.BaseTable (Name, Price, Image) VALUES (@Name, @Price, @Image)";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, this.connection))
                {
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = product.Name;
                    cmd.Parameters.Add("@Price", SqlDbType.Decimal, 20).Value = product.Price;
                    cmd.Parameters.Add("@Image", SqlDbType.NChar, 1000).Value = product.Image;
                    this.Open();
                    cmd.ExecuteNonQuery();
                    this.Close();
                }
            }
            catch (Exception ex)
            {

                Debug.Write(ex.Message);
                this.Close();
            }
            finally
            {
                this.Dispose();
            }

        }

        public void DeleteProduct(int productId)
        {
            string query = "DELETE FROM dbo.BaseTable WHERE Id = @productId";
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, this.connection))
                {
                    this.Open();
                    cmd.Parameters.AddWithValue("@productId", productId);
                    cmd.ExecuteNonQuery();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                this.Close();
            }
            finally
            {
                this.Dispose();
            }

        }

        public bool ProductExists(int id)
        {
            bool prd = false;
            var query = "SELECT * FROM dbo.BaseTable WHERE Id = @id";
            using (SqlCommand cmd = new SqlCommand(query, this.connection))
            {
                this.Open();
                if (cmd.Parameters != null)
                {
                    prd = true;
                }
                cmd.ExecuteNonQuery();
                this.Close();
            }

            return prd;
        }

        public ProductModel FindProduct(int id)
        {
            List<ProductModel> prods = null;
            SqlDataReader reader = null;

            var query = "SELECT * FROM dbo.BaseTable WHERE Id = @id";
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, this.connection))
                {
                    this.Open();
                    cmd.Parameters.AddWithValue("@id", id);
                    reader = cmd.ExecuteReader();
                    prods = reader.DataReaderMapToList<ProductModel>();
                    cmd.ExecuteNonQuery();
                    this.Close();


                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                this.Close();
            }

            finally
            {
                this.Dispose();
            }

            return prods[0];
        }

        public void SaveUpdatedProduct(ProductModel product)
        {
            string query = "UPDATE dbo.BaseTable SET Name = @Name, Price = @Price, Image = @Image WHERE Id = @Id";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, this.connection))
                {

                    this.Open();
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = product.Id;
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = product.Name;
                    cmd.Parameters.Add("@Price", SqlDbType.Decimal, 20).Value = product.Price;
                    cmd.Parameters.Add("@Image", SqlDbType.NChar, 1000).Value = product.Image;

                    cmd.ExecuteNonQuery();
                    this.Close();
                }
            }
            catch (Exception ex)
            {

                Debug.Write(ex.Message);
                this.Close();
            }
            finally
            {
                this.Dispose();
            }
        }


    }
}
