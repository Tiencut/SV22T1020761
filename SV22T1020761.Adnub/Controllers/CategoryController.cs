using Microsoft.AspNetCore.Mvc;
using SV22T1020761.Models;
//using SV22T1020761.BusinessLayers;

namespace SV22T1020761.Admin.Controllers
{
    public class CategoryController : Controller
    {
        // Hiển thị danh sách danh mục sản phẩm
        public IActionResult Index(string searchValue = "", int page = 1, int pageSize = 10)
        {
            return View();
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
            return View();
        }

        // Chỉnh sửa danh mục theo ID
        public IActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category model)
        {
            return View();
        }

        // Xóa danh mục theo ID
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
