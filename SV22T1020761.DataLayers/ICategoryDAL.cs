using SV22T1020761.Models;

namespace SV22T1020761.DataLayers
{
    /// <summary>
    /// Interface cho Category Data Access Layer
    /// </summary>
    public interface ICategoryDAL
    {
        /// <summary>
        /// Lấy danh sách category
        /// </summary>
        /// <param name="searchValue">Giá trị tìm kiếm</param>
        /// <param name="page">Trang</param>
        /// <param name="pageSize">Số bản ghi mỗi trang</param>
        /// <returns></returns>
        IList<Category> List(int page = 1, int pageSize = 0, string searchValue = "");

        /// <summary>
        /// Lấy category theo ID
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        Category? Get(int categoryID);

        /// <summary>
        /// Thêm category
        /// </summary>
        /// <param name="data"></param>
        /// <returns>ID của category mới</returns>
        int Add(Category data);

        /// <summary>
        /// Cập nhật category
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Category data);

        /// <summary>
        /// Xóa category
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        bool Delete(int categoryID);

        /// <summary>
        /// Đếm số lượng category
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue = "");
    }
}