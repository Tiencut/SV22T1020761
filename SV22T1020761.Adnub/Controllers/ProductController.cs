using Microsoft.AspNetCore.Mvc;
using SV22T1020761.Models;
//using SV22T1020761.BusinessLayers;

namespace SV22T1020761.Admin.Controllers
{
    public class ProductController : Controller
    {
        // Hiển thị danh sách sản phẩm
        public IActionResult Index(string searchValue = "", int page = 1, int pageSize = 10)
        {
            return View();
        }

        // Xem chi tiết sản phẩm theo ID
        public IActionResult Detail(int id)
        {
            return View();
        }

        // Thêm mới sản phẩm
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product model)
        {
            return View();
        }

        // Chỉnh sửa sản phẩm theo ID
        public IActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product model)
        {
            return View();
        }

        // Xóa sản phẩm theo ID
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

        // Các hành động liên quan đến thuộc tính/ảnh sản phẩm (tạm giản)
        public IActionResult ListAttributes(int id) => View();
        public IActionResult CreateAttributes(int id) => View();
        [HttpPost]
        public IActionResult CreateAttributes(int id, ProductAttribute model) => View();
        public IActionResult EditAttributes(int id, int attributeId) => View();
        [HttpPost]
        public IActionResult EditAttributes(ProductAttribute model) => View();
        public IActionResult DeleteAttribute(int id, int attributeId) => View();
        [HttpPost, ActionName("DeleteAttribute")]
        public IActionResult DeleteAttributeConfirmed(int id, int attributeId) => View();

        public IActionResult ListPhotos(int id) => View();
        public IActionResult CreatePhoto(int id) => View();
        [HttpPost]
        public IActionResult CreatePhoto(int id, ProductPhoto model) => View();
        public IActionResult EditPhotos(int id, int photoId) => View();
        [HttpPost]
        public IActionResult EditPhotos(ProductPhoto model) => View();
        public IActionResult DeletePhoto(int id, int photoId) => View();
        [HttpPost, ActionName("DeletePhoto")]
        public IActionResult DeletePhotoConfirmed(int id, int photoId) => View();
    }
}
