using System.Collections.Generic;
// Ensure the correct namespace is used for DataLayers
using SV22T1020761.DataLayers; // Corrected namespace
using SV22T1020761.Models;

namespace SV22T1020761.BusinessLayers
{
   /// <summary>
   /// Service cho Order
   /// </summary>
   public static class OrderService
   {
       private static IOrderDAL orderDB;

       /// <summary>
       /// Khởi tạo service
       /// </summary>
       static OrderService()
       {
           orderDB = new OrderDB();
       }

       /// <summary>
       /// Lấy danh sách order
       /// </summary>
       public static IList<Order> ListOrders(string searchValue = "", int page = 1, int pageSize = 0)
       {
           return orderDB.List(page, pageSize, searchValue);
       }

       /// <summary>
       /// Lấy order theo ID
       /// </summary>
       public static Order? GetOrder(int orderID)
       {
           return orderDB.Get(orderID);
       }

       /// <summary>
       /// Thêm order
       /// </summary>
       public static int AddOrder(Order data)
       {
           return orderDB.Add(data);
       }

       /// <summary>
       /// Cập nhật order
       /// </summary>
       public static bool UpdateOrder(Order data)
       {
           return orderDB.Update(data);
       }

       /// <summary>
       /// Xóa order
       /// </summary>
       public static bool DeleteOrder(int orderID)
       {
           return orderDB.Delete(orderID);
       }

       /// <summary>
       /// Đếm số lượng order
       /// </summary>
       public static int CountOrders(string searchValue = "")
       {
           return orderDB.Count(searchValue);
       }
   }
}