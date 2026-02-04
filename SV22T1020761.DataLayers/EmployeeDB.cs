using System.Data.SqlClient;
using SV22T1020761.Models;

namespace SV22T1020761.DataLayers
{
   /// <summary>
   /// Implementation của IEmployeeDAL sử dụng SQL Server
   /// </summary>
   public class EmployeeDB : IEmployeeDAL
   {
       private string connectionString = "Server=.;Database=LiteCommerceDB;User Id=sa;Password=123;"; // Thay đổi theo config thực tế

       public IList<Employee> List(int page = 1, int pageSize = 0, string searchValue = "")
       {
           List<Employee> list = new List<Employee>();
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = @"SELECT * FROM Employees WHERE (@searchValue = '' OR FullName LIKE @searchValue)
                              ORDER BY FullName
                              OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@searchValue", $"%{searchValue}%");
               cmd.Parameters.AddWithValue("@offset", (page - 1) * pageSize);
               cmd.Parameters.AddWithValue("@pageSize", pageSize > 0 ? pageSize : int.MaxValue);

               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                   while (reader.Read())
                   {
                       list.Add(new Employee
                       {
                           EmployeeID = reader.GetInt32("EmployeeID"),
                           FullName = reader.GetString("FullName"),
                           BirthDate = reader.IsDBNull("BirthDate") ? null : reader.GetDateTime("BirthDate"),
                           Address = reader.GetString("Address"),
                           Phone = reader.GetString("Phone"),
                           Email = reader.GetString("Email"),
                           Password = reader.GetString("Password"),
                           Photo = reader.GetString("Photo"),
                           IsWorking = reader.GetBoolean("IsWorking"),
                           RoleNames = reader.GetString("RoleNames")
                       });
                   }
               }
           }
           return list;
       }

       public Employee? Get(int employeeID)
       {
           Employee? employee = null;
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = "SELECT * FROM Employees WHERE EmployeeID = @employeeID";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@employeeID", employeeID);

               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                   if (reader.Read())
                   {
                       employee = new Employee
                       {
                           EmployeeID = reader.GetInt32("EmployeeID"),
                           FullName = reader.GetString("FullName"),
                           BirthDate = reader.IsDBNull("BirthDate") ? null : reader.GetDateTime("BirthDate"),
                           Address = reader.GetString("Address"),
                           Phone = reader.GetString("Phone"),
                           Email = reader.GetString("Email"),
                           Password = reader.GetString("Password"),
                           Photo = reader.GetString("Photo"),
                           IsWorking = reader.GetBoolean("IsWorking"),
                           RoleNames = reader.GetString("RoleNames")
                       };
                   }
               }
           }
           return employee;
       }

       public int Add(Employee data)
       {
           int id = 0;
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = @"INSERT INTO Employees (FullName, BirthDate, Address, Phone, Email, Password, Photo, IsWorking, RoleNames)
                              VALUES (@fullName, @birthDate, @address, @phone, @email, @password, @photo, @isWorking, @roleNames);
                              SELECT SCOPE_IDENTITY();";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@fullName", data.FullName);
               cmd.Parameters.AddWithValue("@birthDate", data.BirthDate ?? (object)DBNull.Value);
               cmd.Parameters.AddWithValue("@address", data.Address);
               cmd.Parameters.AddWithValue("@phone", data.Phone);
               cmd.Parameters.AddWithValue("@email", data.Email);
               cmd.Parameters.AddWithValue("@password", data.Password);
               cmd.Parameters.AddWithValue("@photo", data.Photo);
               cmd.Parameters.AddWithValue("@isWorking", data.IsWorking);
               cmd.Parameters.AddWithValue("@roleNames", data.RoleNames);

               id = Convert.ToInt32(cmd.ExecuteScalar());
           }
           return id;
       }

       public bool Update(Employee data)
       {
           bool result = false;
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = @"UPDATE Employees SET FullName = @fullName, BirthDate = @birthDate,
                              Address = @address, Phone = @phone, Email = @email,
                              Password = @password, Photo = @photo, IsWorking = @isWorking, RoleNames = @roleNames
                              WHERE EmployeeID = @employeeID";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@employeeID", data.EmployeeID);
               cmd.Parameters.AddWithValue("@fullName", data.FullName);
               cmd.Parameters.AddWithValue("@birthDate", data.BirthDate ?? (object)DBNull.Value);
               cmd.Parameters.AddWithValue("@address", data.Address);
               cmd.Parameters.AddWithValue("@phone", data.Phone);
               cmd.Parameters.AddWithValue("@email", data.Email);
               cmd.Parameters.AddWithValue("@password", data.Password);
               cmd.Parameters.AddWithValue("@photo", data.Photo);
               cmd.Parameters.AddWithValue("@isWorking", data.IsWorking);
               cmd.Parameters.AddWithValue("@roleNames", data.RoleNames);

               result = cmd.ExecuteNonQuery() > 0;
           }
           return result;
       }

       public bool Delete(int employeeID)
       {
           bool result = false;
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               connection.Open();
               string sql = "DELETE FROM Employees WHERE EmployeeID = @employeeID";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@employeeID", employeeID);

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
               string sql = "SELECT COUNT(*) FROM Employees WHERE (@searchValue = '' OR FullName LIKE @searchValue)";
               SqlCommand cmd = new SqlCommand(sql, connection);
               cmd.Parameters.AddWithValue("@searchValue", $"%{searchValue}%");

               count = (int)cmd.ExecuteScalar();
           }
           return count;
       }
   }
}