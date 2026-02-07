using System.Data.SqlClient;
using SV22T1020761.Models;

namespace SV22T1020761.DataLayers
{
   /// <summary>
   /// Implementation của ICustomerDAL sử dụng SQL Server
   /// </summary>
   public class CustomerDB : ICustomerDAL
   {
       private string connectionString = "Server=.;Database=LiteCommerceDB;User Id=sa;Password=123;"; // Thay đổi theo config thực tế

       public IList<Customer> List(int page = 1, int pageSize = 0, string searchValue = "")
       {
           List<Customer> list = new List<Customer>();
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = @"SELECT * FROM Customers WHERE (@searchValue = '' OR CustomerName LIKE @searchValue)
                              ORDER BY CustomerName
                              OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@searchValue", $"%{searchValue}%");
               cmd.Parameters.AddWithValue("@offset", (page - 1) * pageSize);
               cmd.Parameters.AddWithValue("@pageSize", pageSize > 0 ? pageSize : int.MaxValue);

               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                   while (reader.Read())
                   {
                       list.Add(new Customer()
                       {
                           CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                           CustomerName = reader.GetString(reader.GetOrdinal("CustomerName")),
                           ContactName = reader.GetString(reader.GetOrdinal("ContactName")),
                           Province = reader.GetString(reader.GetOrdinal("Province")),
                           Address = reader.GetString(reader.GetOrdinal("Address")),
                           Phone = reader.GetString(reader.GetOrdinal("Phone")),
                           Email = reader.GetString(reader.GetOrdinal("Email")),
                           Password = reader.GetString(reader.GetOrdinal("Password"))
                       });
                   }
               }
           }
           return list;
       }

       public Customer? Get(int customerID)
       {
           Customer? customer = null;
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = "SELECT * FROM Customers WHERE CustomerID = @customerID";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@customerID", customerID);

               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                   if (reader.Read())
                   {
                       customer = new Customer()
                       {
                           CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                           CustomerName = reader.GetString(reader.GetOrdinal("CustomerName")),
                           ContactName = reader.GetString(reader.GetOrdinal("ContactName")),
                           Province = reader.GetString(reader.GetOrdinal("Province")),
                           Address = reader.GetString(reader.GetOrdinal("Address")),
                           Phone = reader.GetString(reader.GetOrdinal("Phone")),
                           Email = reader.GetString(reader.GetOrdinal("Email")),
                           Password = reader.GetString(reader.GetOrdinal("Password"))
                       };
                   }
               }
           }
           return customer;
       }

       public int Add(Customer data)
       {
           int id = 0;
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = @"INSERT INTO Customers (CustomerName, ContactName, Province, Address, Phone, Email, Password, IsLocked)
                              VALUES (@customerName, @contactName, @province, @address, @phone, @email, @password, @isLocked);
                              SELECT SCOPE_IDENTITY();";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@customerName", data.CustomerName);
               cmd.Parameters.AddWithValue("@contactName", data.ContactName);
               cmd.Parameters.AddWithValue("@province", data.Province);
               cmd.Parameters.AddWithValue("@address", data.Address);
               cmd.Parameters.AddWithValue("@phone", data.Phone);
               cmd.Parameters.AddWithValue("@email", data.Email);
               cmd.Parameters.AddWithValue("@password", data.Password);
               cmd.Parameters.AddWithValue("@isLocked", data.IsLocked);

               id = Convert.ToInt32(cmd.ExecuteScalar());
           }
           return id;
       }

       public bool Update(Customer data)
       {
           bool result = false;
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = @"UPDATE Customers SET CustomerName = @customerName, ContactName = @contactName,
                              Province = @province, Address = @address, Phone = @phone, Email = @email,
                              Password = @password, IsLocked = @isLocked
                              WHERE CustomerID = @customerID";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@customerID", data.CustomerID);
               cmd.Parameters.AddWithValue("@customerName", data.CustomerName);
               cmd.Parameters.AddWithValue("@contactName", data.ContactName);
               cmd.Parameters.AddWithValue("@province", data.Province);
               cmd.Parameters.AddWithValue("@address", data.Address);
               cmd.Parameters.AddWithValue("@phone", data.Phone);
               cmd.Parameters.AddWithValue("@email", data.Email);
               cmd.Parameters.AddWithValue("@password", data.Password);
               cmd.Parameters.AddWithValue("@isLocked", data.IsLocked);

               result = cmd.ExecuteNonQuery() > 0;
           }
           return result;
       }

       public bool Delete(int customerID)
       {
           bool result = false;
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = "DELETE FROM Customers WHERE CustomerID = @customerID";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@customerID", customerID);

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
               string sql = "SELECT COUNT(*) FROM Customers WHERE (@searchValue = '' OR CustomerName LIKE @searchValue)";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@searchValue", $"%{searchValue}%");

               count = (int)cmd.ExecuteScalar();
           }
           return count;
       }
   }
}