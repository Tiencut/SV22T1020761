using SV22T1020761.Models;

namespace SV22T1020761.DataLayers
{
    /// <summary>
    /// Interface cho Shipper Data Access Layer
    /// </summary>
    public interface IShipperDAL
    {
        /// <summary>
        /// Lấy danh sách shipper
        /// </summary>
        /// <param name="searchValue">Giá trị tìm kiếm</param>
        /// <param name="page">Trang</param>
        /// <param name="pageSize">Số bản ghi mỗi trang</param>
        /// <returns></returns>
        IList<Shipper> List(int page = 1, int pageSize = 0, string searchValue = "");

        /// <summary>
        /// Lấy shipper theo ID
        /// </summary>
        /// <param name="shipperID"></param>
        /// <returns></returns>
        Shipper? Get(int shipperID);

        /// <summary>
        /// Thêm shipper
        /// </summary>
        /// <param name="data"></param>
        /// <returns>ID của shipper mới</returns>
        int Add(Shipper data);

        /// <summary>
        /// Cập nhật shipper
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Shipper data);

        /// <summary>
        /// Xóa shipper
        /// </summary>
        /// <param name="shipperID"></param>
        /// <returns></returns>
        bool Delete(int shipperID);

        /// <summary>
        /// Đếm số lượng shipper
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue = "");
    }
}