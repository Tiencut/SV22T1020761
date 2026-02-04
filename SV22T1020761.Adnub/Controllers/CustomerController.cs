using Microsoft.AspNetCore.Mvc;
using SV22T1020761.Models;
using SV22T1020761.BusinessLayers;

namespace SV22T1020761.Admin.Controllers
{
    public class CustomerController : Controller
    {
        // Hiển thị danh sách khách hàng dưới dạng phân trang, hỗ trợ tìm kiếm theo tên, và điều hướng đến các chức năng liên quan (như chỉnh sửa, xóa)
        public IActionResult Index(string searchValue = "", int page = 1, int pageSize = 10)
        {
            var data = CustomerService.ListCustomers(searchValue, page, pageSize);
            return View(data);
        }

        // Thêm mới khách hàng
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Customer model)
        {
            if (ModelState.IsValid)
            {
                int id = CustomerService.AddCustomer(model);
                if (id > 0)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("Error", "Không thể thêm khách hàng");
            }
            return View(model);
        }

        // Chỉnh sửa thông tin khách hàng theo ID
        public IActionResult Edit(int id)
        {
            var customer = CustomerService.GetCustomer(id);
            if (customer == null)
            {
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        [HttpPost]
        public IActionResult Edit(Customer model)
        {
            if (ModelState.IsValid)
            {
                bool result = CustomerService.UpdateCustomer(model);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("Error", "Không thể cập nhật khách hàng");
            }
            return View(model);
        }

        // Xóa khách hàng theo ID
        public IActionResult Delete(int id)
        {
            var customer = CustomerService.GetCustomer(id);
            if (customer == null)
            {
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            bool result = CustomerService.DeleteCustomer(id);
            return RedirectToAction("Index");
        }

        // Thay đổi mật khẩu cho khách hàng theo ID
        public IActionResult ChangePassword(int id)
        {
            var customer = CustomerService.GetCustomer(id);
            if (customer == null)
            {
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        [HttpPost]
        public IActionResult ChangePassword(int id, string newPassword)
        {
            bool result = CustomerService.ChangePassword(id, newPassword);
            if (result)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("Error", "Không thể thay đổi mật khẩu");
            return View();
        }
    }
}
