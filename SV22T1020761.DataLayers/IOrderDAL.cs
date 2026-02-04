using SV22T1020761.Models;

namespace SV22T1020761.DataLayers
{
    /// <summary>
    /// Interface cho Order Data Access Layer
    /// </summary>
    public interface IOrderDAL
    {
        /// <summary>
        /// Lấy danh sách order
        /// </summary>
        /// <param name="searchValue">Giá trị tìm kiếm</param>
        /// <param name="page">Trang</param>
        /// <param name="pageSize">Số bản ghi mỗi trang</param>
        /// <returns></returns>
        IList<Order> List(int page = 1, int pageSize = 0, string searchValue = "");

        /// <summary>
        /// Lấy order theo ID
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        Order? Get(int orderID);

        /// <summary>
        /// Thêm order
        /// </summary>
        /// <param name="data"></param>
        /// <returns>ID của order mới</returns>
        int Add(Order data);

        /// <summary>
        /// Cập nhật order
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Order data);

        /// <summary>
        /// Xóa order
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        bool Delete(int orderID);

        /// <summary>
        /// Đếm số lượng order
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue = "");
    }
}