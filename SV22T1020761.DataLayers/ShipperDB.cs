using System.Data.SqlClient;
using SV22T1020761.Models;

namespace SV22T1020761.DataLayers
{
   /// <summary>
   /// Implementation của IShipperDAL sử dụng SQL Server
   /// </summary>
   public class ShipperDB : IShipperDAL
   {
       private string connectionString = "Server=.;Database=LiteCommerceDB;User Id=sa;Password=123;"; // Thay đổi theo config thực tế

       public IList<Shipper> List(int page = 1, int pageSize = 0, string searchValue = "")
       {
           List<Shipper> list = new List<Shipper>();
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = @"SELECT * FROM Shippers WHERE (@searchValue = '' OR ShipperName LIKE @searchValue)
                              ORDER BY ShipperName
                              OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@searchValue", $"%{searchValue}%");
               cmd.Parameters.AddWithValue("@offset", (page - 1) * pageSize);
               cmd.Parameters.AddWithValue("@pageSize", pageSize > 0 ? pageSize : int.MaxValue);

               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                   while (reader.Read())
                   {
                       list.Add(new Shipper
                       {
                           ShipperID = reader.GetInt32("ShipperID"),
                           ShipperName = reader.GetString("ShipperName"),
                           Phone = reader.GetString("Phone")
                       });
                   }
               }
           }
           return list;
       }

       public Shipper? Get(int shipperID)
       {
           Shipper? shipper = null;
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = "SELECT * FROM Shippers WHERE ShipperID = @shipperID";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@shipperID", shipperID);

               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                   if (reader.Read())
                   {
                       shipper = new Shipper
                       {
                           ShipperID = reader.GetInt32("ShipperID"),
                           ShipperName = reader.GetString("ShipperName"),
                           Phone = reader.GetString("Phone")
                       };
                   }
               }
           }
           return shipper;
       }

       public int Add(Shipper data)
       {
           int id = 0;
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = @"INSERT INTO Shippers (ShipperName, Phone)
                              VALUES (@shipperName, @phone);
                              SELECT SCOPE_IDENTITY();";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@shipperName", data.ShipperName);
               cmd.Parameters.AddWithValue("@phone", data.Phone);

               id = Convert.ToInt32(cmd.ExecuteScalar());
           }
           return id;
       }

       public bool Update(Shipper data)
       {
           bool result = false;
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = @"UPDATE Shippers SET ShipperName = @shipperName, Phone = @phone
                              WHERE ShipperID = @shipperID";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@shipperID", data.ShipperID);
               cmd.Parameters.AddWithValue("@shipperName", data.ShipperName);
               cmd.Parameters.AddWithValue("@phone", data.Phone);

               result = cmd.ExecuteNonQuery() > 0;
           }
           return result;
       }

       public bool Delete(int shipperID)
       {
           bool result = false;
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = "DELETE FROM Shippers WHERE ShipperID = @shipperID";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@shipperID", shipperID);

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
               string sql = "SELECT COUNT(*) FROM Shippers WHERE (@searchValue = '' OR ShipperName LIKE @searchValue)";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@searchValue", $"%{searchValue}%");

               count = (int)cmd.ExecuteScalar();
           }
           return count;
       }
   }
}