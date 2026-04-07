using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using SV22T1020761.Models.Common;
using SV22T1020761.Models.Catalog;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace SV22T1020761.Shop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ICompositeViewEngine _viewEngine;

        public ProductsController(ICompositeViewEngine viewEngine)
        {
            _viewEngine = viewEngine;
        }

        private string RenderPartialViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = _viewEngine.FindView(ControllerContext, viewName, false);
                if (viewResult.Success)
                {
                    var viewContext = new ViewContext(
                        ControllerContext,
                        viewResult.View,
                        ViewData,
                        TempData,
                        sw,
                        new HtmlHelperOptions()
                    );
                    viewResult.View.RenderAsync(viewContext).Wait();
                    return sw.ToString();
                }
                else
                {
                    throw new Exception($"View {viewName} not found");
                }
            }
        }

        // GET: /Products
        public IActionResult Index(int categoryId = 0, string searchValue = "", decimal minPrice = 0, decimal maxPrice = 0, int page = 1, int pageSize = 10)
        {
            try
            {
                var input = new ProductSearchInput
                {
                    SearchValue = searchValue,
                    Page = page,
                    PageSize = pageSize,
                    CategoryID = categoryId,
                    MinPrice = minPrice,
                    MaxPrice = maxPrice
                };
                var model = SV22T1020761.BusinessLayers.CatalogDataService.ListProducts(input);

                // load categories for filter
                var cats = SV22T1020761.BusinessLayers.CatalogDataService.ListCategories(new PaginationSearchInput { Page = 1, PageSize = 1000 });
                ViewBag.Categories = cats.DataItems;

                // pass filter values
                ViewBag.FilterCategoryId = categoryId;
                ViewBag.FilterMinPrice = minPrice;
                ViewBag.FilterMaxPrice = maxPrice;
                ViewBag.FilterSearchValue = searchValue;

                return View(model);
            }
            catch (Exception ex)
            {
                // Log full error details
                string errorMsg = $"ProductsController.Index error - Message: {ex.Message} | StackTrace: {ex.StackTrace}";
                System.Diagnostics.Trace.TraceError(errorMsg);
                Console.WriteLine($"❌ ERROR: {errorMsg}");
                
                TempData["Error"] = $"Lỗi: {ex.Message}";
                ViewBag.ErrorMessage = ex.Message;
                ViewBag.ErrorStackTrace = ex.StackTrace;
                
                var empty = new PagedResult<Product> { Page = page, PageSize = pageSize, RowCount = 0, DataItems = new System.Collections.Generic.List<Product>() };
                ViewBag.Categories = new System.Collections.Generic.List<Category>();
                return View(empty);
            }
        }

        [HttpPost]
        public IActionResult Search(ProductSearchInput input)
        {
            Console.WriteLine($"🔍 Search called! Page={input?.Page}, PageSize={input?.PageSize}, SearchValue={input?.SearchValue}, CategoryID={input?.CategoryID}");
            
            try
            {
                var result = SV22T1020761.BusinessLayers.CatalogDataService.ListProducts(input);
                Console.WriteLine($"✅ Search result: RowCount={result.RowCount}, Items={result.DataItems?.Count}");

                // Render only the product grid partial view
                return PartialView("../Shared/_ProductGrid", result);
            }
            catch (Exception ex)
            {
                string errorMsg = $"ProductsController.Search error - Message: {ex.Message} | StackTrace: {ex.StackTrace}";
                System.Diagnostics.Trace.TraceError(errorMsg);
                Console.WriteLine($"❌ ERROR: {errorMsg}");

                // Return an empty product grid in case of error
                var empty = new PagedResult<Product> { Page = input.Page, PageSize = input.PageSize, RowCount = 0, DataItems = new System.Collections.Generic.List<Product>() };
                return PartialView("_ProductGrid", empty);
            }
        }

        // GET: /Products/Details/5
        public IActionResult Details(int id)
        {
            var product = SV22T1020761.BusinessLayers.CatalogDataService.GetProduct(id);
            if (product == null) return NotFound();

            var photos = SV22T1020761.BusinessLayers.CatalogDataService.ListProductPhotos(id);
            var attrs = SV22T1020761.BusinessLayers.CatalogDataService.ListAttributes(id);
            var catName = SV22T1020761.BusinessLayers.CatalogDataService.GetCategoryName(product.CategoryID);
            var supName = SV22T1020761.BusinessLayers.CatalogDataService.GetSupplierName(product.SupplierID);

            var vm = new ProductDetailsViewModel
            {
                Product = product,
                Photos = photos ?? new List<ProductPhoto>(),
                Attributes = attrs ?? new List<ProductAttribute>(),
                CategoryName = catName,
                SupplierName = supName
            };

            return View(vm);
        }

        // GET: /Products/QuickView/5
        public IActionResult QuickView(int id)
        {
            var product = SV22T1020761.BusinessLayers.CatalogDataService.GetProduct(id);
            if (product == null) return NotFound();
            return PartialView("_ProductQuickView", product);
        }
    }
}
