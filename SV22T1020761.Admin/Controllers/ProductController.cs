using Microsoft.AspNetCore.Mvc;
using SV22T1020761.Models;

namespace SV22T1020761.Admin.Controllers
{
    public class ProductController : Controller
    {
        // =====================================================
        // Product/Index
        // =====================================================
        public IActionResult Index()
        {
            return View();
        }

        // =====================================================
        // Product/Create
        // =====================================================
        public IActionResult Create()
        {
            return View(new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product model)
        {
            if (!ModelState.IsValid) return View(model);
            // TODO: lưu product
            return RedirectToAction(nameof(Index));
        }

        // =====================================================
        // Product/Edit/{id}
        // =====================================================
        public IActionResult Edit(int id)
        {
            return View(new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product model)
        {
            if (!ModelState.IsValid) return View(model);
            // TODO: cập nhật product
            return RedirectToAction(nameof(Index));
        }

        // =====================================================
        // Product/Delete/{id}
        // =====================================================
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

        // =====================================================
        // Product/ListAttributes/{id}
        // =====================================================
        public IActionResult ListAttributes(int id)
        {
            return View();
        }

        // =====================================================
        // Product/CreateAttribute/{id}
        // =====================================================
        public IActionResult CreateAttribute(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAttribute(int id, ProductAttribute model)
        {
            return View();
        }

        // =====================================================
        // Product/EditAttribute/{id}?attributeId={attributeId}
        // =====================================================
        public IActionResult EditAttribute(int id, int attributeId)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAttribute(int id, ProductAttribute model)
        {
            return View();
        }

        // =====================================================
        // Product/DeleteAttribute/{id}?attributeId={attributeId}
        // =====================================================
        public IActionResult DeleteAttribute(int id, int attributeId)
        {
            return View();
        }

        [HttpPost, ActionName("DeleteAttribute")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAttributeConfirmed(int id, int attributeId)
        {
            return View();
        }

        // =====================================================
        // Product/ListPhotos/{id}
        // =====================================================
        public IActionResult ListPhotos(int id)
        {
            return View();
        }

        // =====================================================
        // Product/CreatePhoto/{id}
        // =====================================================
        public IActionResult CreatePhoto(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePhoto(int id, ProductPhoto model)
        {
            return View();
        }

        // =====================================================
        // Product/EditPhoto/{id}?photoId={photoId}
        // =====================================================
        public IActionResult EditPhoto(int id, int photoId)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPhoto(int id, ProductPhoto model)
        {
            return View();
        }

        // =====================================================
        // Product/DeletePhoto/{id}?photoId={photoId}
        // =====================================================
        public IActionResult DeletePhoto(int id, int photoId)
        {
            return View();
        }

        [HttpPost, ActionName("DeletePhoto")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePhotoConfirmed(int id, int photoId)
        {
            return View();
        }
    }
}