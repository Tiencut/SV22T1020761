using System.Data.SqlClient;
using SV22T1020761.Models;

namespace SV22T1020761.DataLayers
{
   /// <summary>
   /// Implementation của IOrderDAL sử dụng SQL Server
   /// </summary>
   public class OrderDB : IOrderDAL
   {
       private string connectionString = "Server=.;Database=LiteCommerceDB;User Id=sa;Password=123;"; // Thay đổi theo config thực tế

       public IList<Order> List(int page = 1, int pageSize = 0, string searchValue = "")
       {
           List<Order> list = new List<Order>();
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = @"SELECT * FROM Orders WHERE (@searchValue = '' OR DeliveryProvince LIKE @searchValue)
                              ORDER BY OrderTime DESC
                              OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@searchValue", $"%{searchValue}%");
               cmd.Parameters.AddWithValue("@offset", (page - 1) * pageSize);
               cmd.Parameters.AddWithValue("@pageSize", pageSize > 0 ? pageSize : int.MaxValue);

               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                   while (reader.Read())
                   {
                       list.Add(new Order
                       {
                           OrderID = reader.GetInt32("OrderID"),
                           CustomerID = reader.IsDBNull("CustomerID") ? null : reader.GetInt32("CustomerID"),
                           OrderTime = reader.GetDateTime("OrderTime"),
                           DeliveryProvince = reader.GetString("DeliveryProvince"),
                           DeliveryAddress = reader.GetString("DeliveryAddress"),
                           EmployeeID = reader.IsDBNull("EmployeeID") ? null : reader.GetInt32("EmployeeID"),
                           AcceptTime = reader.IsDBNull("AcceptTime") ? null : reader.GetDateTime("AcceptTime"),
                           ShipperID = reader.IsDBNull("ShipperID") ? null : reader.GetInt32("ShipperID"),
                           ShippedTime = reader.IsDBNull("ShippedTime") ? null : reader.GetDateTime("ShippedTime"),
                           FinishedTime = reader.IsDBNull("FinishedTime") ? null : reader.GetDateTime("FinishedTime"),
                           Status = reader.GetInt32("Status")
                       });
                   }
               }
           }
           return list;
       }

       public Order? Get(int orderID)
       {
           Order? order = null;
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = "SELECT * FROM Orders WHERE OrderID = @orderID";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@orderID", orderID);

               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                   if (reader.Read())
                   {
                       order = new Order
                       {
                           OrderID = reader.GetInt32("OrderID"),
                           CustomerID = reader.IsDBNull("CustomerID") ? null : reader.GetInt32("CustomerID"),
                           OrderTime = reader.GetDateTime("OrderTime"),
                           DeliveryProvince = reader.GetString("DeliveryProvince"),
                           DeliveryAddress = reader.GetString("DeliveryAddress"),
                           EmployeeID = reader.IsDBNull("EmployeeID") ? null : reader.GetInt32("EmployeeID"),
                           AcceptTime = reader.IsDBNull("AcceptTime") ? null : reader.GetDateTime("AcceptTime"),
                           ShipperID = reader.IsDBNull("ShipperID") ? null : reader.GetInt32("ShipperID"),
                           ShippedTime = reader.IsDBNull("ShippedTime") ? null : reader.GetDateTime("ShippedTime"),
                           FinishedTime = reader.IsDBNull("FinishedTime") ? null : reader.GetDateTime("FinishedTime"),
                           Status = reader.GetInt32("Status")
                       };
                   }
               }
           }
           return order;
       }

       public int Add(Order data)
       {
           int id = 0;
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = @"INSERT INTO Orders (CustomerID, OrderTime, DeliveryProvince, DeliveryAddress, EmployeeID, AcceptTime, ShipperID, ShippedTime, FinishedTime, Status)
                              VALUES (@customerID, @orderTime, @deliveryProvince, @deliveryAddress, @employeeID, @acceptTime, @shipperID, @shippedTime, @finishedTime, @status);
                              SELECT SCOPE_IDENTITY();";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@customerID", data.CustomerID ?? (object)DBNull.Value);
               cmd.Parameters.AddWithValue("@orderTime", data.OrderTime);
               cmd.Parameters.AddWithValue("@deliveryProvince", data.DeliveryProvince);
               cmd.Parameters.AddWithValue("@deliveryAddress", data.DeliveryAddress);
               cmd.Parameters.AddWithValue("@employeeID", data.EmployeeID ?? (object)DBNull.Value);
               cmd.Parameters.AddWithValue("@acceptTime", data.AcceptTime ?? (object)DBNull.Value);
               cmd.Parameters.AddWithValue("@shipperID", data.ShipperID ?? (object)DBNull.Value);
               cmd.Parameters.AddWithValue("@shippedTime", data.ShippedTime ?? (object)DBNull.Value);
               cmd.Parameters.AddWithValue("@finishedTime", data.FinishedTime ?? (object)DBNull.Value);
               cmd.Parameters.AddWithValue("@status", data.Status);

               id = Convert.ToInt32(cmd.ExecuteScalar());
           }
           return id;
       }

       public bool Update(Order data)
       {
           bool result = false;
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = @"UPDATE Orders SET CustomerID = @customerID, OrderTime = @orderTime,
                              DeliveryProvince = @deliveryProvince, DeliveryAddress = @deliveryAddress,
                              EmployeeID = @employeeID, AcceptTime = @acceptTime, ShipperID = @shipperID,
                              ShippedTime = @shippedTime, FinishedTime = @finishedTime, Status = @status
                              WHERE OrderID = @orderID";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@orderID", data.OrderID);
               cmd.Parameters.AddWithValue("@customerID", data.CustomerID ?? (object)DBNull.Value);
               cmd.Parameters.AddWithValue("@orderTime", data.OrderTime);
               cmd.Parameters.AddWithValue("@deliveryProvince", data.DeliveryProvince);
               cmd.Parameters.AddWithValue("@deliveryAddress", data.DeliveryAddress);
               cmd.Parameters.AddWithValue("@employeeID", data.EmployeeID ?? (object)DBNull.Value);
               cmd.Parameters.AddWithValue("@acceptTime", data.AcceptTime ?? (object)DBNull.Value);
               cmd.Parameters.AddWithValue("@shipperID", data.ShipperID ?? (object)DBNull.Value);
               cmd.Parameters.AddWithValue("@shippedTime", data.ShippedTime ?? (object)DBNull.Value);
               cmd.Parameters.AddWithValue("@finishedTime", data.FinishedTime ?? (object)DBNull.Value);
               cmd.Parameters.AddWithValue("@status", data.Status);

               result = cmd.ExecuteNonQuery() > 0;
           }
           return result;
       }

       public bool Delete(int orderID)
       {
           bool result = false;
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = "DELETE FROM Orders WHERE OrderID = @orderID";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@orderID", orderID);

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
               string sql = "SELECT COUNT(*) FROM Orders WHERE (@searchValue = '' OR DeliveryProvince LIKE @searchValue)";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@searchValue", $"%{searchValue}%");

               count = (int)cmd.ExecuteScalar();
           }
           return count;
       }
   }
}