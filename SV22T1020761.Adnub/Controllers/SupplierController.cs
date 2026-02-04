using Microsoft.AspNetCore.Mvc;
using SV22T1020761.Models; // Giả sử có model Supplier trong SV22T1020761.Models
using SV22T1020761.BusinessLayers; // Giả sử có service cho Supplier

namespace SV22T1020761.Admin.Controllers
{
    public class SupplierController : Controller
    {
        // Hiển thị danh sách nhà cung cấp (có thể phân trang và tìm kiếm)
        public IActionResult Index(string searchValue = "", int page = 1, int pageSize = 10)
        {
            // Lấy danh sách supplier từ business layer
            var data = SupplierService.ListSuppliers(searchValue, page, pageSize);
            return View(data);
        }

        // Thêm mới nhà cung cấp
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Supplier model)
        {
            if (ModelState.IsValid)
            {
                // Thêm supplier qua business layer
                int id = SupplierService.AddSupplier(model);
                if (id > 0)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("Error", "Không thể thêm nhà cung cấp");
            }
            return View(model);
        }

        // Chỉnh sửa thông tin nhà cung cấp theo ID
        public IActionResult Edit(int id)
        {
            var supplier = SupplierService.GetSupplier(id);
            if (supplier == null)
            {
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        [HttpPost]
        public IActionResult Edit(Supplier model)
        {
            if (ModelState.IsValid)
            {
                // Cập nhật supplier qua business layer
                bool result = SupplierService.UpdateSupplier(model);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("Error", "Không thể cập nhật nhà cung cấp");
            }
            return View(model);
        }

        // Xóa nhà cung cấp theo ID
        public IActionResult Delete(int id)
        {
            var supplier = SupplierService.GetSupplier(id);
            if (supplier == null)
            {
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            // Xóa supplier qua business layer
            bool result = SupplierService.DeleteSupplier(id);
            return RedirectToAction("Index");
        }
    }
}
