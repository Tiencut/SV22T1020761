using Microsoft.AspNetCore.Mvc;
using SV22T1020761.Models;
//using SV22T1020761.BusinessLayers;

namespace SV22T1020761.Admin.Controllers
{
    public class ShipperController : Controller
    {
        // Hiển thị danh sách người giao hàng (shipper)
        public IActionResult Index(string searchValue = "", int page = 1, int pageSize = 10)
        {
            return View();
        }

        // Thêm mới shipper
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Shipper model)
        {
            return View();
        }

        // Chỉnh sửa thông tin shipper theo ID
        public IActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Shipper model)
        {
            return View();
        }

        // Xóa shipper theo ID
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
    }
}
