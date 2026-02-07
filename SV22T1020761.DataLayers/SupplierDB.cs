using System.Data.SqlClient;
using SV22T1020761.Models;

namespace SV22T1020761.DataLayers
{
   /// <summary>
   /// Implementation của ISupplierDAL sử dụng SQL Server
   /// </summary>
   public class SupplierDB : ISupplierDAL
   {
       private string connectionString = "Server=.;Database=SV22T1020761;User Id=sa;Password=123;"; // Thay đổi theo config thực tế

       public IList<Supplier> List(int page = 1, int pageSize = 0, string searchValue = "")
       {
           List<Supplier> list = new List<Supplier>();
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = @"SELECT * FROM Suppliers WHERE (@searchValue = '' OR SupplierName LIKE @searchValue)
                              ORDER BY SupplierName
                              OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@searchValue", $"%{searchValue}%");
               cmd.Parameters.AddWithValue("@offset", (page - 1) * pageSize);
               cmd.Parameters.AddWithValue("@pageSize", pageSize > 0 ? pageSize : int.MaxValue);

               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                   while (reader.Read())
                   {
                       list.Add(new Supplier
                       {
                           SupplierID = reader.GetInt32(reader.GetOrdinal("SupplierID")),
                           SupplierName = reader.GetString(reader.GetOrdinal("SupplierName")),
                           ContactName = reader.GetString(reader.GetOrdinal("ContactName")),
                           Province = reader.GetString(reader.GetOrdinal("Province")),
                           Address = reader.GetString(reader.GetOrdinal("Address")),
                           Phone = reader.GetString(reader.GetOrdinal("Phone")),
                           Email = reader.GetString(reader.GetOrdinal("Email"))
                       });
                   }
               }
           }
           return list;
       }

       public Supplier? Get(int supplierID)
       {
           Supplier? supplier = null;
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = "SELECT * FROM Suppliers WHERE SupplierID = @supplierID";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@supplierID", supplierID);

               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                   if (reader.Read())
                   {
                       supplier = new Supplier
                       {
                           SupplierID = reader.GetInt32(reader.GetOrdinal("SupplierID")),
                           SupplierName = reader.GetString(reader.GetOrdinal("SupplierName")),
                           ContactName = reader.GetString(reader.GetOrdinal("ContactName")),
                           Province = reader.GetString(reader.GetOrdinal("Province")),
                           Address = reader.GetString(reader.GetOrdinal("Address")),
                           Phone = reader.GetString(reader.GetOrdinal("Phone")),
                           Email = reader.GetString(reader.GetOrdinal("Email"))
                       };
                   }
               }
           }
           return supplier;
       }

       public int Add(Supplier data)
       {
           int id = 0;
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = @"INSERT INTO Suppliers (SupplierName, ContactName, Province, Address, Phone, Email)
                              VALUES (@supplierName, @contactName, @province, @address, @phone, @email);
                              SELECT SCOPE_IDENTITY();";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@supplierName", data.SupplierName);
               cmd.Parameters.AddWithValue("@contactName", data.ContactName);
               cmd.Parameters.AddWithValue("@province", data.Province);
               cmd.Parameters.AddWithValue("@address", data.Address);
               cmd.Parameters.AddWithValue("@phone", data.Phone);
               cmd.Parameters.AddWithValue("@email", data.Email);

               id = Convert.ToInt32(cmd.ExecuteScalar());
           }
           return id;
       }

       public bool Update(Supplier data)
       {
           bool result = false;
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = @"UPDATE Suppliers SET SupplierName = @supplierName, ContactName = @contactName,
                              Province = @province, Address = @address, Phone = @phone, Email = @email
                              WHERE SupplierID = @supplierID";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@supplierID", data.SupplierID);
               cmd.Parameters.AddWithValue("@supplierName", data.SupplierName);
               cmd.Parameters.AddWithValue("@contactName", data.ContactName);
               cmd.Parameters.AddWithValue("@province", data.Province);
               cmd.Parameters.AddWithValue("@address", data.Address);
               cmd.Parameters.AddWithValue("@phone", data.Phone);
               cmd.Parameters.AddWithValue("@email", data.Email);

               result = cmd.ExecuteNonQuery() > 0;
           }
           return result;
       }

       public bool Delete(int supplierID)
       {
           bool result = false;
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = "DELETE FROM Suppliers WHERE SupplierID = @supplierID";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@supplierID", supplierID);

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
               string sql = "SELECT COUNT(*) FROM Suppliers WHERE (@searchValue = '' OR SupplierName LIKE @searchValue)";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@searchValue", $"%{searchValue}%");

               count = (int)cmd.ExecuteScalar();
           }
           return count;
       }
   }
}