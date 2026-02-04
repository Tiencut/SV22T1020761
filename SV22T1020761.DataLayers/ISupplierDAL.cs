using SV22T1020761.Models;

namespace SV22T1020761.DataLayers
{
    /// <summary>
    /// Interface cho Supplier Data Access Layer
    /// </summary>
    public interface ISupplierDAL
    {
        /// <summary>
        /// Lấy danh sách supplier
        /// </summary>
        /// <param name="searchValue">Giá trị tìm kiếm</param>
        /// <param name="page">Trang</param>
        /// <param name="pageSize">Số bản ghi mỗi trang</param>
        /// <returns></returns>
        IList<Supplier> List(int page = 1, int pageSize = 0, string searchValue = "");

        /// <summary>
        /// Lấy supplier theo ID
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        Supplier? Get(int supplierID);

        /// <summary>
        /// Thêm supplier
        /// </summary>
        /// <param name="data"></param>
        /// <returns>ID của supplier mới</returns>
        int Add(Supplier data);

        /// <summary>
        /// Cập nhật supplier
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Supplier data);

        /// <summary>
        /// Xóa supplier
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        bool Delete(int supplierID);

        /// <summary>
        /// Đếm số lượng supplier
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue = "");
    }
}