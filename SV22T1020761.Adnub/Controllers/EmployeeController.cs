using Microsoft.AspNetCore.Mvc;
using SV22T1020761.Models;
using SV22T1020761.BusinessLayers;

namespace SV22T1020761.Admin.Controllers
{
    public class EmployeeController : Controller
    {
        // Hiển thị danh sách nhân viên
        public IActionResult Index(string searchValue = "", int page = 1, int pageSize = 10)
        {
            var data = EmployeeService.ListEmployees(searchValue, page, pageSize);
            return View(data);
        }

        // Thêm mới nhân viên
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee model)
        {
            if (ModelState.IsValid)
            {
                int id = EmployeeService.AddEmployee(model);
                if (id > 0)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("Error", "Không thể thêm nhân viên");
            }
            return View(model);
        }

        // Chỉnh sửa thông tin nhân viên theo ID
        public IActionResult Edit(int id)
        {
            var employee = EmployeeService.GetEmployee(id);
            if (employee == null)
            {
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee model)
        {
            if (ModelState.IsValid)
            {
                bool result = EmployeeService.UpdateEmployee(model);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("Error", "Không thể cập nhật nhân viên");
            }
            return View(model);
        }

        // Xóa nhân viên theo ID
        public IActionResult Delete(int id)
        {
            var employee = EmployeeService.GetEmployee(id);
            if (employee == null)
            {
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            bool result = EmployeeService.DeleteEmployee(id);
            return RedirectToAction("Index");
        }

        // Thay đổi mật khẩu cho nhân viên theo ID
        public IActionResult ChangePassword(int id)
        {
            var employee = EmployeeService.GetEmployee(id);
            if (employee == null)
            {
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(int id, string newPassword)
        {
            bool result = EmployeeService.ChangePassword(id, newPassword);
            if (result)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("Error", "Không thể thay đổi mật khẩu");
            return View();
        }

        // Thay đổi vai trò (role) của nhân viên theo ID
        public IActionResult ChangeRole(int id)
        {
            var employee = EmployeeService.GetEmployee(id);
            if (employee == null)
            {
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeRole(int id, string newRole)
        {
            bool result = EmployeeService.ChangeRole(id, newRole);
            if (result)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("Error", "Không thể thay đổi vai trò");
            return View();
        }
    }
}
