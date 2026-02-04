using SV22T1020761.Models;

namespace SV22T1020761.DataLayers
{
    /// <summary>
    /// Interface cho Employee Data Access Layer
    /// </summary>
    public interface IEmployeeDAL
    {
        /// <summary>
        /// Lấy danh sách employee
        /// </summary>
        /// <param name="searchValue">Giá trị tìm kiếm</param>
        /// <param name="page">Trang</param>
        /// <param name="pageSize">Số bản ghi mỗi trang</param>
        /// <returns></returns>
        IList<Employee> List(int page = 1, int pageSize = 0, string searchValue = "");

        /// <summary>
        /// Lấy employee theo ID
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        Employee? Get(int employeeID);

        /// <summary>
        /// Thêm employee
        /// </summary>
        /// <param name="data"></param>
        /// <returns>ID của employee mới</returns>
        int Add(Employee data);

        /// <summary>
        /// Cập nhật employee
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Employee data);

        /// <summary>
        /// Xóa employee
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        bool Delete(int employeeID);

        /// <summary>
        /// Đếm số lượng employee
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue = "");
    }
}