using Microsoft.AspNetCore.Mvc;
using SV22T1020761.Models;
using SV22T1020761.BusinessLayers;

namespace SV22T1020761.Admin.Controllers
{
    public class OrderController : Controller
    {
        // Hiển thị danh sách đơn hàng
        public IActionResult Index(string searchValue = "", int page = 1, int pageSize = 10)
        {
            var data = OrderService.ListOrders(searchValue, page, pageSize);
            return View(data);
        }

        // Tạo đơn hàng mới
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Order model)
        {
            if (ModelState.IsValid)
            {
                int id = OrderService.AddOrder(model);
                if (id > 0)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("Error", "Không thể tạo đơn hàng");
            }
            return View(model);
        }

        // Chỉnh sửa đơn hàng theo ID
        public IActionResult Edit(int id)
        {
            var order = OrderService.GetOrder(id);
            if (order == null)
            {
                return RedirectToAction("Index");
            }
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Order model)
        {
            if (ModelState.IsValid)
            {
                bool result = OrderService.UpdateOrder(model);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("Error", "Không thể cập nhật đơn hàng");
            }
            return View(model);
        }

        // Xóa đơn hàng theo ID
        public IActionResult Delete(int id)
        {
            var order = OrderService.GetOrder(id);
            if (order == null)
            {
                return RedirectToAction("Index");
            }
            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            bool result = OrderService.DeleteOrder(id);
            return RedirectToAction("Index");
        }

        // Xem chi tiết đơn hàng theo ID
        public IActionResult Detail(int id)
        {
            var order = OrderService.GetOrder(id);
            if (order == null)
            {
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // Cập nhật trạng thái của đơn hàng (ví dụ: đang xử lý, đã giao, hủy)
        public IActionResult UpdateStatus(int id)
        {
            var order = OrderService.GetOrder(id);
            if (order == null)
            {
                return RedirectToAction("Index");
            }
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int id, string status)
        {
            bool result = OrderService.UpdateOrderStatus(id, status);
            if (result)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("Error", "Không thể cập nhật trạng thái");
            return View();
        }
    }
}
