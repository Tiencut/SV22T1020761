using Microsoft.AspNetCore.Mvc;
using SV22T1020761.BusinessLayers;
using SV22T1020761.Models.Common;
using SV22T1020761.Models.Catalog;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace SV22T1020761.Admin.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        // =====================================================
        // Product/Index
        // =====================================================
        public IActionResult Index(string searchValue, int page = 1, int pageSize = 10)
        {
            var input = new PaginationSearchInput
            {
                SearchValue = searchValue,
                Page = page,
                PageSize = pageSize
            };

            try
            {
                var model = CatalogDataService.ListProducts(input);
                return View(model);
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Error loading products");
                TempData["Error"] = "Không thể kết nối tới cơ sở dữ liệu. Vui lòng kiểm tra cấu hình và thử lại.";
                var empty = new PagedResult<Product> { Page = page, PageSize = pageSize, RowCount = 0, DataItems = new System.Collections.Generic.List<Product>() };
                return View(empty);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(PaginationSearchInput input)
        {
            var result = CatalogDataService.ListProducts(input);
            return PartialView("_ProductTable", result);
        }

        // Partial form for modal
        [HttpGet]
        public IActionResult Form(int? id, bool delete = false)
        {
            try
            {
                if (id == null || id == 0)
                {
                    var model = new Product();
                    if (delete) return BadRequest();
                    return PartialView("_ProductForm", model);
                }
                var product = CatalogDataService.GetProduct(id.Value);
                if (product == null) return NotFound();
                if (delete) return PartialView("_ProductDelete", product);
                return PartialView("_ProductForm", product);
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Error loading form (Id={ProductId}, Delete={Delete})", id, delete);
                TempData["Error"] = "Hệ thống đang bận. Vui lòng thử lại sau.";
                return BadRequest();
            }
        }

        // =====================================================
        // Product/Create
        // =====================================================
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            try
            {
                if (!ModelState.IsValid) return View(product);
                CatalogDataService.AddProduct(product);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var input = new PaginationSearchInput { Page = 1, PageSize = 10, SearchValue = "" };
                    var result = CatalogDataService.ListProducts(input);
                    return PartialView("_ProductTable", result);
                }

                TempData["Success"] = "Thêm sản phẩm thành công.";
                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Error creating product");
                ModelState.AddModelError(string.Empty, "Hệ thống đang bận. Vui lòng thử lại sau.");
                return View(product);
            }
        }

        // =====================================================
        // Product/Edit/{id}
        // =====================================================
        public IActionResult Edit(int id)
        {
            try
            {
                var product = CatalogDataService.GetProduct(id);
                if (product == null)
                {
                    return NotFound();
                }
                return View(product);
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Error loading product for edit (Id={ProductId})", id);
                TempData["Error"] = "Không thể tải sản phẩm. Vui lòng thử lại sau.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            try
            {
                if (!ModelState.IsValid) return View(product);
                CatalogDataService.UpdateProduct(product);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var input = new PaginationSearchInput { Page = 1, PageSize = 10, SearchValue = "" };
                    var result = CatalogDataService.ListProducts(input);
                    return PartialView("_ProductTable", result);
                }

                TempData["Success"] = "Cập nhật sản phẩm thành công.";
                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Error updating product (Id={ProductId})", product?.ProductID);
                ModelState.AddModelError(string.Empty, "Hệ thống đang bận. Vui lòng thử lại sau.");
                return View(product);
            }
        }

        // =====================================================
        // Product/Delete/{id}
        // =====================================================
        public IActionResult Delete(int id)
        {
            var product = CatalogDataService.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                CatalogDataService.DeleteProduct(id);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var input = new PaginationSearchInput { Page = 1, PageSize = 10, SearchValue = "" };
                    var result = CatalogDataService.ListProducts(input);
                    return PartialView("_ProductTable", result);
                }

                TempData["Success"] = "Xóa sản phẩm thành công.";
                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Error deleting product (Id={ProductId})", id);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return BadRequest("Lỗi khi xóa: " + ex.Message);
                }
                var product = CatalogDataService.GetProduct(id);
                ModelState.AddModelError(string.Empty, "Không thể xóa sản phẩm. Vui lòng thử lại sau.");
                return View("Delete", product);
            }
        }

        // =====================================================
        // Product/ListAttributes/{id}
        // =====================================================
        public IActionResult ListAttributes(int id)
        {
            try
            {
                var attributes = CatalogDataService.ListAttributes(id);
                ViewBag.ProductID = id;
                return View(attributes);
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Error loading product attributes (Id={ProductId})", id);
                TempData["Error"] = "Không thể tải thuộc tính sản phẩm. Vui lòng thử lại sau.";
                return RedirectToAction("Index");
            }
        }

        // =====================================================
        // Product/CreateAttribute/{id}
        // =====================================================
        public IActionResult CreateAttribute(int id)
        {
            try
            {
                var product = CatalogDataService.GetProduct(id);
                if (product == null) return NotFound();
                ViewBag.ProductID = id;
                ViewBag.ProductName = product.ProductName;
                return View();
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Error loading create attribute form (Id={ProductId})", id);
                TempData["Error"] = "Lỗi khi tải form. Vui lòng thử lại sau.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAttribute(int id, ProductAttribute model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ProductID = id;
                    return View(model);
                }

                var product = CatalogDataService.GetProduct(id);
                if (product == null) return NotFound();

                model.ProductID = id;
                CatalogDataService.AddProductAttribute(model);

                TempData["Success"] = "Thêm thuộc tính sản phẩm thành công.";
                return RedirectToAction("ListAttributes", new { id });
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Error creating product attribute (Id={ProductId})", id);
                ModelState.AddModelError(string.Empty, "Lỗi khi thêm thuộc tính. Vui lòng thử lại sau.");
                ViewBag.ProductID = id;
                return View(model);
            }
        }

        // =====================================================
        // Product/EditAttribute/{id}?attributeId={attributeId}
        // =====================================================
        public IActionResult EditAttribute(int id, int attributeId)
        {
            try
            {
                var attribute = CatalogDataService.GetProductAttribute(attributeId);
                if (attribute == null || attribute.ProductID != id) return NotFound();
                ViewBag.ProductID = id;
                return View(attribute);
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Error loading edit attribute form (Id={ProductId}, AttributeId={AttributeId})", id, attributeId);
                TempData["Error"] = "Lỗi khi tải form. Vui lòng thử lại sau.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAttribute(int id, int attributeId, ProductAttribute model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ProductID = id;
                    return View(model);
                }

                var attribute = CatalogDataService.GetProductAttribute(attributeId);
                if (attribute == null || attribute.ProductID != id) return NotFound();

                model.ProductID = id;
                model.AttributeID = attributeId;
                CatalogDataService.UpdateProductAttribute(model);

                TempData["Success"] = "Cập nhật thuộc tính sản phẩm thành công.";
                return RedirectToAction("ListAttributes", new { id });
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Error updating product attribute (Id={ProductId}, AttributeId={AttributeId})", id, attributeId);
                ModelState.AddModelError(string.Empty, "Lỗi khi cập nhật thuộc tính. Vui lòng thử lại sau.");
                ViewBag.ProductID = id;
                return View(model);
            }
        }

        // =====================================================
        // Product/DeleteAttribute/{id}?attributeId={attributeId}
        // =====================================================
        public IActionResult DeleteAttribute(int id, int attributeId)
        {
            try
            {
                var attribute = CatalogDataService.GetProductAttribute(attributeId);
                if (attribute == null || attribute.ProductID != id) return NotFound();
                ViewBag.ProductID = id;
                return View(attribute);
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Error loading delete attribute form (Id={ProductId}, AttributeId={AttributeId})", id, attributeId);
                TempData["Error"] = "Lỗi khi tải form. Vui lòng thử lại sau.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost, ActionName("DeleteAttribute")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAttributeConfirmed(int id, int attributeId)
        {
            try
            {
                var attribute = CatalogDataService.GetProductAttribute(attributeId);
                if (attribute == null || attribute.ProductID != id) return NotFound();

                CatalogDataService.DeleteProductAttribute(attributeId);

                TempData["Success"] = "Xóa thuộc tính sản phẩm thành công.";
                return RedirectToAction("ListAttributes", new { id });
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Error deleting product attribute (Id={ProductId}, AttributeId={AttributeId})", id, attributeId);
                TempData["Error"] = "Lỗi khi xóa thuộc tính. Vui lòng thử lại sau.";
                return RedirectToAction("ListAttributes", new { id });
            }
        }

        // =====================================================
        // Product/ListPhotos/{id}
        // =====================================================
        public IActionResult ListPhotos(int id)
        {
            try
            {
                var photos = CatalogDataService.ListProductPhotos(id);
                return View(photos);
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Error loading product photos (Id={ProductId})", id);
                TempData["Error"] = "Không thể tải ảnh sản phẩm. Vui lòng thử lại sau.";
                return RedirectToAction("Index");
            }
        }

        // =====================================================
        // Product/CreatePhoto/{id}
        // =====================================================
        public IActionResult CreatePhoto(int id)
        {
            try
            {
                var product = CatalogDataService.GetProduct(id);
                if (product == null) return NotFound();
                ViewBag.ProductID = id;
                ViewBag.ProductName = product.ProductName;
                return View();
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Error loading create photo form (Id={ProductId})", id);
                TempData["Error"] = "Lỗi khi tải form. Vui lòng thử lại sau.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePhoto(int id, IFormFile photo, string description = "")
        {
            try
            {
                if (photo == null || photo.Length == 0)
                {
                    ModelState.AddModelError("photo", "Vui lòng chọn một tệp ảnh.");
                    ViewBag.ProductID = id;
                    return View();
                }

                var product = CatalogDataService.GetProduct(id);
                if (product == null) return NotFound();

                // Validate file type
                var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
                if (!allowedTypes.Contains(photo.ContentType))
                {
                    ModelState.AddModelError("photo", "Chỉ chấp nhận các tệp ảnh (jpg, png, gif, webp).");
                    ViewBag.ProductID = id;
                    return View();
                }

                // Save file
                var uploadsDir = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");
                if (!Directory.Exists(uploadsDir)) Directory.CreateDirectory(uploadsDir);

                var fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(photo.FileName);
                var filePath = System.IO.Path.Combine(uploadsDir, fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    photo.CopyTo(stream);
                }

                // Save to database
                var productPhoto = new ProductPhoto
                {
                    ProductID = id,
                    Photo = fileName,
                    DisplayOrder = 0,
                    Description = description
                };

                CatalogDataService.AddProductPhoto(productPhoto);

                TempData["Success"] = "Thêm ảnh sản phẩm thành công.";
                return RedirectToAction("ListPhotos", new { id });
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Error creating product photo (Id={ProductId})", id);
                ModelState.AddModelError(string.Empty, "Lỗi khi thêm ảnh. Vui lòng thử lại sau.");
                ViewBag.ProductID = id;
                return View();
            }
        }

        // =====================================================
        // Product/EditPhoto/{id}?photoId={photoId}
        // =====================================================
        public IActionResult EditPhoto(int id, long photoId)
        {
            try
            {
                var photo = CatalogDataService.GetProductPhoto(photoId);
                if (photo == null || photo.ProductID != id) return NotFound();
                ViewBag.ProductID = id;
                return View(photo);
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Error loading edit photo form (Id={ProductId}, PhotoId={PhotoId})", id, photoId);
                TempData["Error"] = "Lỗi khi tải form. Vui lòng thử lại sau.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPhoto(int id, long photoId, ProductPhoto model)
        {
            try
            {
                var photo = CatalogDataService.GetProductPhoto(photoId);
                if (photo == null || photo.ProductID != id) return NotFound();

                photo.Description = model.Description;
                photo.DisplayOrder = model.DisplayOrder;

                CatalogDataService.UpdateProductPhoto(photo);

                TempData["Success"] = "Cập nhật ảnh sản phẩm thành công.";
                return RedirectToAction("ListPhotos", new { id });
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Error updating product photo (Id={ProductId}, PhotoId={PhotoId})", id, photoId);
                ModelState.AddModelError(string.Empty, "Lỗi khi cập nhật ảnh. Vui lòng thử lại sau.");
                ViewBag.ProductID = id;
                return View(model);
            }
        }

        // =====================================================
        // Product/DeletePhoto/{id}?photoId={photoId}
        // =====================================================
        public IActionResult DeletePhoto(int id, long photoId)
        {
            try
            {
                var photo = CatalogDataService.GetProductPhoto(photoId);
                if (photo == null || photo.ProductID != id) return NotFound();
                ViewBag.ProductID = id;
                return View(photo);
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Error loading delete photo form (Id={ProductId}, PhotoId={PhotoId})", id, photoId);
                TempData["Error"] = "Lỗi khi tải form. Vui lòng thử lại sau.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost, ActionName("DeletePhoto")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePhotoConfirmed(int id, long photoId)
        {
            try
            {
                var photo = CatalogDataService.GetProductPhoto(photoId);
                if (photo == null || photo.ProductID != id) return NotFound();

                // Delete file from disk
                var filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products", photo.Photo);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // Delete from database
                CatalogDataService.DeleteProductPhoto(photoId);

                TempData["Success"] = "Xóa ảnh sản phẩm thành công.";
                return RedirectToAction("ListPhotos", new { id });
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Error deleting product photo (Id={ProductId}, PhotoId={PhotoId})", id, photoId);
                TempData["Error"] = "Lỗi khi xóa ảnh. Vui lòng thử lại sau.";
                return RedirectToAction("ListPhotos", new { id });
            }
        }
    }
}