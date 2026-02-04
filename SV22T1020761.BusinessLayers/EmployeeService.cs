using System.Collections.Generic;
// Ensure the correct namespace is used for DataLayers
using SV22T1020761.DataLayers; // Corrected namespace
using SV22T1020761.Models;

namespace SV22T1020761.BusinessLayers
{
   /// <summary>
   /// Service cho Employee
   /// </summary>
   public static class EmployeeService
   {
       private static IEmployeeDAL employeeDB;

       /// <summary>
       /// Khởi tạo service
       /// </summary>
       static EmployeeService()
       {
           employeeDB = new EmployeeDB();
       }

       /// <summary>
       /// Lấy danh sách employee
       /// </summary>
       public static IList<Employee> ListEmployees(string searchValue = "", int page = 1, int pageSize = 0)
       {
           return employeeDB.List(page, pageSize, searchValue);
       }

       /// <summary>
       /// Lấy employee theo ID
       /// </summary>
       public static Employee? GetEmployee(int employeeID)
       {
           return employeeDB.Get(employeeID);
       }

       /// <summary>
       /// Thêm employee
       /// </summary>
       public static int AddEmployee(Employee data)
       {
           return employeeDB.Add(data);
       }

       /// <summary>
       /// Cập nhật employee
       /// </summary>
       public static bool UpdateEmployee(Employee data)
       {
           return employeeDB.Update(data);
       }

       /// <summary>
       /// Xóa employee
       /// </summary>
       public static bool DeleteEmployee(int employeeID)
       {
           return employeeDB.Delete(employeeID);
       }

       /// <summary>
       /// Đếm số lượng employee
       /// </summary>
       public static int CountEmployees(string searchValue = "")
       {
           return employeeDB.Count(searchValue);
       }
   }
}