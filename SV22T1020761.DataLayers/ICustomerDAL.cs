using SV22T1020761.Models;

namespace SV22T1020761.DataLayers
{
    /// <summary>
    /// Interface cho Customer Data Access Layer
    /// </summary>
    public interface ICustomerDAL
    {
        /// <summary>
        /// Lấy danh sách customer
        /// </summary>
        /// <param name="searchValue">Giá trị tìm kiếm</param>
        /// <param name="page">Trang</param>
        /// <param name="pageSize">Số bản ghi mỗi trang</param>
        /// <returns></returns>
        IList<Customer> List(int page = 1, int pageSize = 0, string searchValue = "");

        /// <summary>
        /// Lấy customer theo ID
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        Customer? Get(int customerID);

        /// <summary>
        /// Thêm customer
        /// </summary>
        /// <param name="data"></param>
        /// <returns>ID của customer mới</returns>
        int Add(Customer data);

        /// <summary>
        /// Cập nhật customer
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Customer data);

        /// <summary>
        /// Xóa customer
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        bool Delete(int customerID);

        /// <summary>
        /// Đếm số lượng customer
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue = "");
    }
}