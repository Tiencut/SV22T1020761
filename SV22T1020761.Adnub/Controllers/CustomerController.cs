using Microsoft.AspNetCore.Mvc;
using SV22T1020761.Models;
//using SV22T1020761.BusinessLayers;

namespace SV22T1020761.Admin.Controllers
{
    public class CustomerController : Controller
    {
        // Hiển thị danh sách khách hàng dưới dạng phân trang, hỗ trợ tìm kiếm theo tên, và điều hướng đến các chức năng liên quan (như chỉnh sửa, xóa)
        public IActionResult Index(string searchValue = "", int page = 1, int pageSize = 10)
        {
            return View();
        }

        // Thêm mới khách hàng
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Customer model)
        {
            return View();
        }

        // Chỉnh sửa thông tin khách hàng theo ID
        public IActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(Customer model)
        {
            return View();
        }

        // Xóa khách hàng theo ID
        public IActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            return View();
        }

        // Thay đổi mật khẩu cho khách hàng theo ID
        public IActionResult ChangePassword(int id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(int id, string newPassword)
        {
            return View();
        }
    }
}
