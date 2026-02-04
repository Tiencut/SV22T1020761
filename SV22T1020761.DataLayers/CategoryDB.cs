using System.Data.SqlClient;
using SV22T1020761.Models;

namespace SV22T1020761.DataLayers
{
    /// <summary>
    /// Implementation của ICategoryDAL sử dụng SQL Server
    /// </summary>
    public class CategoryDB : ICategoryDAL
    {
        private string connectionString = "Server=.;Database=LiteCommerceDB;User Id=sa;Password=123;";

        public IList<Category> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Category> data = new List<Category>();
            if (!string.IsNullOrWhiteSpace(searchValue))
                searchValue = "%" + searchValue + "%";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = @"SELECT *
                                        FROM (
                                            SELECT *, ROW_NUMBER() OVER(ORDER BY CategoryName) AS RowNumber
                                            FROM Categories
                                            WHERE (@searchValue = N'' OR CategoryName LIKE @searchValue)
                                        ) AS t
                                        WHERE (@pageSize = 0) OR (t.RowNumber BETWEEN (@page - 1) * @pageSize + 1 AND @page * @pageSize)
                                        ORDER BY t.RowNumber";
                    cmd.Parameters.AddWithValue("@page", page);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);
                    cmd.Parameters.AddWithValue("@searchValue", searchValue ?? "");

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            data.Add(new Category()
                            {
                                CategoryID = reader.GetInt32("CategoryID"),
                                CategoryName = reader.GetString("CategoryName"),
                                Description = reader.GetString("Description")
                            });
                        }
                    }
                }
                connection.Close();
            }
            return data;
        }

        public Category? Get(int categoryID)
        {
            Category? data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT * FROM Categories WHERE CategoryID = @categoryID";
                    cmd.Parameters.AddWithValue("@categoryID", categoryID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            data = new Category()
                            {
                                CategoryID = reader.GetInt32("CategoryID"),
                                CategoryName = reader.GetString("CategoryName"),
                                Description = reader.GetString("Description")
                            };
                        }
                    }
                }
                connection.Close();
            }
            return data;
        }

        public int Add(Category data)
        {
            int id = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = @"INSERT INTO Categories (CategoryName, Description)
                                        VALUES (@categoryName, @description);
                                        SELECT SCOPE_IDENTITY();";
                    cmd.Parameters.AddWithValue("@categoryName", data.CategoryName ?? "");
                    cmd.Parameters.AddWithValue("@description", data.Description ?? "");

                    id = Convert.ToInt32(cmd.ExecuteScalar());
                }
                connection.Close();
            }
            return id;
        }

        public bool Update(Category data)
        {
            bool result = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = @"UPDATE Categories
                                        SET CategoryName = @categoryName, Description = @description
                                        WHERE CategoryID = @categoryID";
                    cmd.Parameters.AddWithValue("@categoryID", data.CategoryID);
                    cmd.Parameters.AddWithValue("@categoryName", data.CategoryName ?? "");
                    cmd.Parameters.AddWithValue("@description", data.Description ?? "");

                    result = cmd.ExecuteNonQuery() > 0;
                }
                connection.Close();
            }
            return result;
        }

        public bool Delete(int categoryID)
        {
            bool result = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "DELETE FROM Categories WHERE CategoryID = @categoryID";
                    cmd.Parameters.AddWithValue("@categoryID", categoryID);

                    result = cmd.ExecuteNonQuery() > 0;
                }
                connection.Close();
            }
            return result;
        }

        public int Count(string searchValue = "")
        {
            int count = 0;
            if (!string.IsNullOrWhiteSpace(searchValue))
                searchValue = "%" + searchValue + "%";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = @"SELECT COUNT(*)
                                        FROM Categories
                                        WHERE (@searchValue = N'' OR CategoryName LIKE @searchValue)";
                    cmd.Parameters.AddWithValue("@searchValue", searchValue ?? "");

                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }
                connection.Close();
            }
            return count;
        }
    }
}