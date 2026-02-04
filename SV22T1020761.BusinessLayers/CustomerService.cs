using System.Collections.Generic;
// Ensure the correct namespace is used for DataLayers
using SV22T1020761.DataLayers; // Corrected namespace
using SV22T1020761.Models;

namespace SV22T1020761.BusinessLayers
{
   /// <summary>
   /// Service cho Customer
   /// </summary>
   public static class CustomerService
   {
       private static ICustomerDAL customerDB;

       /// <summary>
       /// Khởi tạo service
       /// </summary>
       static CustomerService()
       {
           customerDB = new CustomerDB();
       }

       /// <summary>
       /// Lấy danh sách customer
       /// </summary>
       public static IList<Customer> ListCustomers(string searchValue = "", int page = 1, int pageSize = 0)
       {
           return customerDB.List(page, pageSize, searchValue);
       }

       /// <summary>
       /// Lấy customer theo ID
       /// </summary>
       public static Customer? GetCustomer(int customerID)
       {
           return customerDB.Get(customerID);
       }

       /// <summary>
       /// Thêm customer
       /// </summary>
       public static int AddCustomer(Customer data)
       {
           return customerDB.Add(data);
       }

       /// <summary>
       /// Cập nhật customer
       /// </summary>
       public static bool UpdateCustomer(Customer data)
       {
           return customerDB.Update(data);
       }

       /// <summary>
       /// Xóa customer
       /// </summary>
       public static bool DeleteCustomer(int customerID)
       {
           return customerDB.Delete(customerID);
       }

       /// <summary>
       /// Đếm số lượng customer
       /// </summary>
       public static int CountCustomers(string searchValue = "")
       {
           return customerDB.Count(searchValue);
       }
   }
}