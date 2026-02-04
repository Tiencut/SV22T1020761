using System.Collections.Generic;
// Ensure the correct namespace is used for DataLayers
using SV22T1020761.DataLayers; // Corrected namespace
using SV22T1020761.Models;

namespace SV22T1020761.BusinessLayers
{
   /// <summary>
   /// Service cho Supplier
   /// </summary>
   public static class SupplierService
   {
       private static ISupplierDAL supplierDB;

       /// <summary>
       /// Khởi tạo service
       /// </summary>
       static SupplierService()
       {
           supplierDB = new SupplierDB();
       }

       /// <summary>
       /// Lấy danh sách supplier
       /// </summary>
       public static IList<Supplier> ListSuppliers(string searchValue = "", int page = 1, int pageSize = 0)
       {
           return supplierDB.List(page, pageSize, searchValue);
       }

       /// <summary>
       /// Lấy supplier theo ID
       /// </summary>
       public static Supplier? GetSupplier(int supplierID)
       {
           return supplierDB.Get(supplierID);
       }

       /// <summary>
       /// Thêm supplier
       /// </summary>
       public static int AddSupplier(Supplier data)
       {
           return supplierDB.Add(data);
       }

       /// <summary>
       /// Cập nhật supplier
       /// </summary>
       public static bool UpdateSupplier(Supplier data)
       {
           return supplierDB.Update(data);
       }

       /// <summary>
       /// Xóa supplier
       /// </summary>
       public static bool DeleteSupplier(int supplierID)
       {
           return supplierDB.Delete(supplierID);
       }

       /// <summary>
       /// Đếm số lượng supplier
       /// </summary>
       public static int CountSuppliers(string searchValue = "")
       {
           return supplierDB.Count(searchValue);
       }
   }
}