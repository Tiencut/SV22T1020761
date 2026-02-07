using Microsoft.AspNetCore.Mvc;
using SV22T1020761.Models;
//using SV22T1020761.BusinessLayers;

namespace SV22T1020761.Admin.Controllers
{
    public class EmployeeController : Controller
    {
        // Hiển thị danh sách nhân viên
        public IActionResult Index(string searchValue = "", int page = 1, int pageSize = 10)
        {
            return View();
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
            return View();
        }

        // Chỉnh sửa thông tin nhân viên theo ID
        public IActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee model)
        {
            return View();
        }

        // Xóa nhân viên theo ID
        public IActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            return View();
        }

        // Thay đổi mật khẩu cho nhân viên theo ID
        public IActionResult ChangePassword(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(int id, string newPassword)
        {
            return View();
        }

        // Thay đổi vai trò (role) của nhân viên theo ID
        public IActionResult ChangeRole(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeRole(int id, string newRole)
        {
            return View();
        }
    }
}
