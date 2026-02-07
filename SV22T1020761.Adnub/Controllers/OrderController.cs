using Microsoft.AspNetCore.Mvc;
using SV22T1020761.Models;
//using SV22T1020761.BusinessLayers;

namespace SV22T1020761.Admin.Controllers
{
    public class OrderController : Controller
    {
        // Hiển thị danh sách đơn hàng với tìm kiếm (mock)
        public IActionResult Index(int? status = null, DateTime? fromDate = null, DateTime? toDate = null, string? customerName = null, int page = 1, int pageSize = 10)
        {
            // create mock data
            var mock = new List<SV22T1020761.Adnub.Models.OrderListItem>
            {
                new SV22T1020761.Adnub.Models.OrderListItem { OrderID = 101, CustomerName = "Nguyễn Văn A", OrderTime = DateTime.Now.AddDays(-2), DeliveryProvince = "Hanoi", DeliveryAddress = "123", Status = 0 },
                new SV22T1020761.Adnub.Models.OrderListItem { OrderID = 100, CustomerName = "Trần Thị B", OrderTime = DateTime.Now.AddDays(-1), DeliveryProvince = "HCMC", DeliveryAddress = "456", Status = 3 },
                new SV22T1020761.Adnub.Models.OrderListItem { OrderID = 99, CustomerName = "Phạm C", OrderTime = DateTime.Now.AddDays(-3), DeliveryProvince = "Da Nang", DeliveryAddress = "789", Status = 4 }
            };

            // apply filters
            var q = mock.AsQueryable();
            if (status.HasValue)
                q = q.Where(x => x.Status == status.Value);
            if (fromDate.HasValue)
                q = q.Where(x => x.OrderTime.Date >= fromDate.Value.Date);
            if (toDate.HasValue)
                q = q.Where(x => x.OrderTime.Date <= toDate.Value.Date);
            if (!string.IsNullOrWhiteSpace(customerName))
                q = q.Where(x => x.CustomerName.Contains(customerName, StringComparison.OrdinalIgnoreCase));

            var list = q.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return View(list);
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
            return View();
        }

        // Chỉnh sửa đơn hàng theo ID
        public IActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Order model)
        {
            return View();
        }

        // Xóa đơn hàng theo ID
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

        // Xem chi tiết đơn hàng theo ID
        public IActionResult Detail(int id)
        {
            return View();
        }

        // Cập nhật trạng thái của đơn hàng (ví dụ: đang xử lý, đã giao, hủy)
        public IActionResult UpdateStatus(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int id, string status)
        {
            return View();
        }
    }
}
