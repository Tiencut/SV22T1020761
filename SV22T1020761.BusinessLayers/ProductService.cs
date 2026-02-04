using System.Collections.Generic;
// Ensure the correct namespace is used for DataLayers
using SV22T1020761.DataLayers; // Corrected namespace
using SV22T1020761.Models;

namespace SV22T1020761.BusinessLayers
{
   /// <summary>
   /// Service cho Product
   /// </summary>
   public static class ProductService
   {
       private static IProductDAL productDB;

       /// <summary>
       /// Khởi tạo service
       /// </summary>
       static ProductService()
       {
           productDB = new ProductDB();
       }

       /// <summary>
       /// Lấy danh sách product
       /// </summary>
       public static IList<Product> ListProducts(string searchValue = "", int page = 1, int pageSize = 0)
       {
           return productDB.List(page, pageSize, searchValue);
       }

       /// <summary>
       /// Lấy product theo ID
       /// </summary>
       public static Product? GetProduct(int productID)
       {
           return productDB.Get(productID);
       }

       /// <summary>
       /// Thêm product
       /// </summary>
       public static int AddProduct(Product data)
       {
           return productDB.Add(data);
       }

       /// <summary>
       /// Cập nhật product
       /// </summary>
       public static bool UpdateProduct(Product data)
       {
           return productDB.Update(data);
       }

       /// <summary>
       /// Xóa product
       /// </summary>
       public static bool DeleteProduct(int productID)
       {
           return productDB.Delete(productID);
       }

       /// <summary>
       /// Đếm số lượng product
       /// </summary>
       public static int CountProducts(string searchValue = "")
       {
           return productDB.Count(searchValue);
       }
   }
}