using SV22T1020761.Models;

namespace SV22T1020761.DataLayers
{
    /// <summary>
    /// Interface cho Product Data Access Layer
    /// </summary>
    public interface IProductDAL
    {
        /// <summary>
        /// Lấy danh sách product
        /// </summary>
        /// <param name="searchValue">Giá trị tìm kiếm</param>
        /// <param name="page">Trang</param>
        /// <param name="pageSize">Số bản ghi mỗi trang</param>
        /// <returns></returns>
        IList<Product> List(int page = 1, int pageSize = 0, string searchValue = "");

        /// <summary>
        /// Lấy product theo ID
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        Product? Get(int productID);

        /// <summary>
        /// Thêm product
        /// </summary>
        /// <param name="data"></param>
        /// <returns>ID của product mới</returns>
        int Add(Product data);

        /// <summary>
        /// Cập nhật product
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Product data);

        /// <summary>
        /// Xóa product
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        bool Delete(int productID);

        /// <summary>
        /// Đếm số lượng product
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue = "");
    }
}