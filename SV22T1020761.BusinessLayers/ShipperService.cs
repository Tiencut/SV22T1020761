using System.Collections.Generic;
// Ensure the correct namespace is used for DataLayers
using SV22T1020761.DataLayers; // Corrected namespace
using SV22T1020761.Models;

namespace SV22T1020761.BusinessLayers
{
   /// <summary>
   /// Service cho Shipper
   /// </summary>
   public static class ShipperService
   {
       private static IShipperDAL shipperDB;

       /// <summary>
       /// Khởi tạo service
       /// </summary>
       static ShipperService()
       {
           shipperDB = new ShipperDB();
       }

       /// <summary>
       /// Lấy danh sách shipper
       /// </summary>
       public static IList<Shipper> ListShippers(string searchValue = "", int page = 1, int pageSize = 0)
       {
           return shipperDB.List(page, pageSize, searchValue);
       }

       /// <summary>
       /// Lấy shipper theo ID
       /// </summary>
       public static Shipper? GetShipper(int shipperID)
       {
           return shipperDB.Get(shipperID);
       }

       /// <summary>
       /// Thêm shipper
       /// </summary>
       public static int AddShipper(Shipper data)
       {
           return shipperDB.Add(data);
       }

       /// <summary>
       /// Cập nhật shipper
       /// </summary>
       public static bool UpdateShipper(Shipper data)
       {
           return shipperDB.Update(data);
       }

       /// <summary>
       /// Xóa shipper
       /// </summary>
       public static bool DeleteShipper(int shipperID)
       {
           return shipperDB.Delete(shipperID);
       }

       /// <summary>
       /// Đếm số lượng shipper
       /// </summary>
       public static int CountShippers(string searchValue = "")
       {
           return shipperDB.Count(searchValue);
       }
   }
}