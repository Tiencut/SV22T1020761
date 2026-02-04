using Microsoft.AspNetCore.Mvc;
using SV22T1020761.Models;
using SV22T1020761.BusinessLayers;

namespace SV22T1020761.Admin.Controllers
{
    public class ProductController : Controller
    {
        // Hiển thị danh sách sản phẩm
        public IActionResult Index(string searchValue = "", int page = 1, int pageSize = 10)
        {
            var data = ProductService.ListProducts(searchValue, page, pageSize);
            return View(data);
        }

        // Xem chi tiết sản phẩm theo ID
        public IActionResult Detail(int id)
        {
            var product = ProductService.GetProduct(id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            return View(product);
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
            if (ModelState.IsValid)
            {
                int id = ProductService.AddProduct(model);
                if (id > 0)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("Error", "Không thể thêm sản phẩm");
            }
            return View(model);
        }

        // Chỉnh sửa sản phẩm theo ID
        public IActionResult Edit(int id)
        {
            var product = ProductService.GetProduct(id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product model)
        {
            if (ModelState.IsValid)
            {
                bool result = ProductService.UpdateProduct(model);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("Error", "Không thể cập nhật sản phẩm");
            }
            return View(model);
        }

        // Xóa sản phẩm theo ID
        public IActionResult Delete(int id)
        {
            var product = ProductService.GetProduct(id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            bool result = ProductService.DeleteProduct(id);
            return RedirectToAction("Index");
        }

        // Liệt kê các thuộc tính (attributes) của sản phẩm theo ID
        public IActionResult ListAttributes(int id)
        {
            var attributes = ProductService.ListProductAttributes(id);
            ViewBag.ProductId = id;
            return View(attributes);
        }

        // Thêm thuộc tính mới cho sản phẩm theo ID
        public IActionResult CreateAttributes(int id)
        {
            ViewBag.ProductId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAttributes(int id, ProductAttribute model)
        {
            model.ProductID = id;
            if (ModelState.IsValid)
            {
                int attrId = ProductService.AddProductAttribute(model);
                if (attrId > 0)
                {
                    return RedirectToAction("ListAttributes", new { id });
                }
                ModelState.AddModelError("Error", "Không thể thêm thuộc tính");
            }
            ViewBag.ProductId = id;
            return View(model);
        }

        // Chỉnh sửa thuộc tính cụ thể của sản phẩm
        public IActionResult EditAttributes(int id, int attributeId)
        {
            var attribute = ProductService.GetProductAttribute(attributeId);
            if (attribute == null || attribute.ProductID != id)
            {
                return RedirectToAction("ListAttributes", new { id });
            }
            return View(attribute);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAttributes(ProductAttribute model)
        {
            if (ModelState.IsValid)
            {
                bool result = ProductService.UpdateProductAttribute(model);
                if (result)
                {
                    return RedirectToAction("ListAttributes", new { id = model.ProductID });
                }
                ModelState.AddModelError("Error", "Không thể cập nhật thuộc tính");
            }
            return View(model);
        }

        // Xóa thuộc tính của sản phẩm
        public IActionResult DeleteAttribute(int id, int attributeId)
        {
            var attribute = ProductService.GetProductAttribute(attributeId);
            if (attribute == null || attribute.ProductID != id)
            {
                return RedirectToAction("ListAttributes", new { id });
            }
            return View(attribute);
        }

        [HttpPost, ActionName("DeleteAttribute")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAttributeConfirmed(int id, int attributeId)
        {
            bool result = ProductService.DeleteProductAttribute(attributeId);
            return RedirectToAction("ListAttributes", new { id });
        }

        // Liệt kê các ảnh của sản phẩm theo ID
        public IActionResult ListPhotos(int id)
        {
            var photos = ProductService.ListProductPhotos(id);
            ViewBag.ProductId = id;
            return View(photos);
        }

        // Thêm ảnh mới cho sản phẩm
        public IActionResult CreatePhoto(int id)
        {
            ViewBag.ProductId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePhoto(int id, ProductPhoto model)
        {
            model.ProductID = id;
            if (ModelState.IsValid)
            {
                int photoId = ProductService.AddProductPhoto(model);
                if (photoId > 0)
                {
                    return RedirectToAction("ListPhotos", new { id });
                }
                ModelState.AddModelError("Error", "Không thể thêm ảnh");
            }
            ViewBag.ProductId = id;
            return View(model);
        }

        // Chỉnh sửa ảnh của sản phẩm
        public IActionResult EditPhotos(int id, int photoId)
        {
            var photo = ProductService.GetProductPhoto(photoId);
            if (photo == null || photo.ProductID != id)
            {
                return RedirectToAction("ListPhotos", new { id });
            }
            return View(photo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPhotos(ProductPhoto model)
        {
            if (ModelState.IsValid)
            {
                bool result = ProductService.UpdateProductPhoto(model);
                if (result)
                {
                    return RedirectToAction("ListPhotos", new { id = model.ProductID });
                }
                ModelState.AddModelError("Error", "Không thể cập nhật ảnh");
            }
            return View(model);
        }

        // Xóa ảnh của sản phẩm
        public IActionResult DeletePhoto(int id, int photoId)
        {
            var photo = ProductService.GetProductPhoto(photoId);
            if (photo == null || photo.ProductID != id)
            {
                return RedirectToAction("ListPhotos", new { id });
            }
            return View(photo);
        }

        [HttpPost, ActionName("DeletePhoto")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePhotoConfirmed(int id, int photoId)
        {
            bool result = ProductService.DeleteProductPhoto(photoId);
            return RedirectToAction("ListPhotos", new { id });
        }
    }
}
