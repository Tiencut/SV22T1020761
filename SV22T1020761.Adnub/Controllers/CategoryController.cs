using Microsoft.AspNetCore.Mvc;
using SV22T1020761.Models;
using SV22T1020761.BusinessLayers;

namespace SV22T1020761.Admin.Controllers
{
    public class CategoryController : Controller
    {
        // Hiển thị danh sách danh mục sản phẩm
        public IActionResult Index(string searchValue = "", int page = 1, int pageSize = 10)
        {
            var data = CategoryService.ListCategories(searchValue, page, pageSize);
            return View(data);
        }

        // Thêm mới danh mục
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category model)
        {
            if (ModelState.IsValid)
            {
                int id = CategoryService.AddCategory(model);
                if (id > 0)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("Error", "Không thể thêm danh mục");
            }
            return View(model);
        }

        // Chỉnh sửa danh mục theo ID
        public IActionResult Edit(int id)
        {
            var category = CategoryService.GetCategory(id);
            if (category == null)
            {
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                bool result = CategoryService.UpdateCategory(model);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("Error", "Không thể cập nhật danh mục");
            }
            return View(model);
        }

        // Xóa danh mục theo ID
        public IActionResult Delete(int id)
        {
            var category = CategoryService.GetCategory(id);
            if (category == null)
            {
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            bool result = CategoryService.DeleteCategory(id);
            return RedirectToAction("Index");
        }
    }
}
