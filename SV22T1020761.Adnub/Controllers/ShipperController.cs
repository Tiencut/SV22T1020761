using Microsoft.AspNetCore.Mvc;
using SV22T1020761.Models;
using SV22T1020761.BusinessLayers;

namespace SV22T1020761.Admin.Controllers
{
    public class ShipperController : Controller
    {
        // Hiển thị danh sách người giao hàng (shipper)
        public IActionResult Index(string searchValue = "", int page = 1, int pageSize = 10)
        {
            var data = ShipperService.ListShippers(searchValue, page, pageSize);
            return View(data);
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
            if (ModelState.IsValid)
            {
                int id = ShipperService.AddShipper(model);
                if (id > 0)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("Error", "Không thể thêm shipper");
            }
            return View(model);
        }

        // Chỉnh sửa thông tin shipper theo ID
        public IActionResult Edit(int id)
        {
            var shipper = ShipperService.GetShipper(id);
            if (shipper == null)
            {
                return RedirectToAction("Index");
            }
            return View(shipper);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Shipper model)
        {
            if (ModelState.IsValid)
            {
                bool result = ShipperService.UpdateShipper(model);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("Error", "Không thể cập nhật shipper");
            }
            return View(model);
        }

        // Xóa shipper theo ID
        public IActionResult Delete(int id)
        {
            var shipper = ShipperService.GetShipper(id);
            if (shipper == null)
            {
                return RedirectToAction("Index");
            }
            return View(shipper);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            bool result = ShipperService.DeleteShipper(id);
            return RedirectToAction("Index");
        }
    }
}
