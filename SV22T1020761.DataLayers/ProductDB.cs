using System.Data.SqlClient;
using SV22T1020761.Models;

namespace SV22T1020761.DataLayers
{
   /// <summary>
   /// Implementation của IProductDAL sử dụng SQL Server
   /// </summary>
   public class ProductDB : IProductDAL
   {
       private string connectionString = "Server=.;Database=LiteCommerceDB;User Id=sa;Password=123;"; // Thay đổi theo config thực tế

       public IList<Product> List(int page = 1, int pageSize = 0, string searchValue = "")
       {
           List<Product> list = new List<Product>();
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = @"SELECT * FROM Products WHERE (@searchValue = '' OR ProductName LIKE @searchValue)
                              ORDER BY ProductName
                              OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@searchValue", $"%{searchValue}%");
               cmd.Parameters.AddWithValue("@offset", (page - 1) * pageSize);
               cmd.Parameters.AddWithValue("@pageSize", pageSize > 0 ? pageSize : int.MaxValue);

               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                   while (reader.Read())
                   {
                       list.Add(new Product
                       {
                           ProductID = reader.GetInt32("ProductID"),
                           ProductName = reader.GetString("ProductName"),
                           ProductDescription = reader.GetString("ProductDescription"),
                           SupplierID = reader.IsDBNull("SupplierID") ? null : reader.GetInt32("SupplierID"),
                           CategoryID = reader.IsDBNull("CategoryID") ? null : reader.GetInt32("CategoryID"),
                           Unit = reader.GetString("Unit"),
                           Price = reader.GetDecimal("Price"),
                           Photo = reader.GetString("Photo"),
                           IsSelling = reader.GetBoolean("IsSelling")
                       });
                   }
               }
           }
           return list;
       }

       public Product? Get(int productID)
       {
           Product? product = null;
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = "SELECT * FROM Products WHERE ProductID = @productID";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@productID", productID);

               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                   if (reader.Read())
                   {
                       product = new Product
                       {
                           ProductID = reader.GetInt32("ProductID"),
                           ProductName = reader.GetString("ProductName"),
                           ProductDescription = reader.GetString("ProductDescription"),
                           SupplierID = reader.IsDBNull("SupplierID") ? null : reader.GetInt32("SupplierID"),
                           CategoryID = reader.IsDBNull("CategoryID") ? null : reader.GetInt32("CategoryID"),
                           Unit = reader.GetString("Unit"),
                           Price = reader.GetDecimal("Price"),
                           Photo = reader.GetString("Photo"),
                           IsSelling = reader.GetBoolean("IsSelling")
                       };
                   }
               }
           }
           return product;
       }

       public int Add(Product data)
       {
           int id = 0;
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = @"INSERT INTO Products (ProductName, ProductDescription, SupplierID, CategoryID, Unit, Price, Photo, IsSelling)
                              VALUES (@productName, @productDescription, @supplierID, @categoryID, @unit, @price, @photo, @isSelling);
                              SELECT SCOPE_IDENTITY();";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@productName", data.ProductName);
               cmd.Parameters.AddWithValue("@productDescription", data.ProductDescription);
               cmd.Parameters.AddWithValue("@supplierID", data.SupplierID ?? (object)DBNull.Value);
               cmd.Parameters.AddWithValue("@categoryID", data.CategoryID ?? (object)DBNull.Value);
               cmd.Parameters.AddWithValue("@unit", data.Unit);
               cmd.Parameters.AddWithValue("@price", data.Price);
               cmd.Parameters.AddWithValue("@photo", data.Photo);
               cmd.Parameters.AddWithValue("@isSelling", data.IsSelling);

               id = Convert.ToInt32(cmd.ExecuteScalar());
           }
           return id;
       }

       public bool Update(Product data)
       {
           bool result = false;
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = @"UPDATE Products SET ProductName = @productName, ProductDescription = @productDescription,
                              SupplierID = @supplierID, CategoryID = @categoryID, Unit = @unit, Price = @price,
                              Photo = @photo, IsSelling = @isSelling
                              WHERE ProductID = @productID";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@productID", data.ProductID);
               cmd.Parameters.AddWithValue("@productName", data.ProductName);
               cmd.Parameters.AddWithValue("@productDescription", data.ProductDescription);
               cmd.Parameters.AddWithValue("@supplierID", data.SupplierID ?? (object)DBNull.Value);
               cmd.Parameters.AddWithValue("@categoryID", data.CategoryID ?? (object)DBNull.Value);
               cmd.Parameters.AddWithValue("@unit", data.Unit);
               cmd.Parameters.AddWithValue("@price", data.Price);
               cmd.Parameters.AddWithValue("@photo", data.Photo);
               cmd.Parameters.AddWithValue("@isSelling", data.IsSelling);

               result = cmd.ExecuteNonQuery() > 0;
           }
           return result;
       }

       public bool Delete(int productID)
       {
           bool result = false;
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = "DELETE FROM Products WHERE ProductID = @productID";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@productID", productID);

               result = cmd.ExecuteNonQuery() > 0;
           }
           return result;
       }

       public int Count(string searchValue = "")
       {
           int count = 0;
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = "SELECT COUNT(*) FROM Products WHERE (@searchValue = '' OR ProductName LIKE @searchValue)";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@searchValue", $"%{searchValue}%");

               count = (int)cmd.ExecuteScalar();
           }
           return count;
       }
   }
}