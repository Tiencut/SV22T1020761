using Microsoft.AspNetCore.Mvc;
using SV22T1020761.BusinessLayers;
using SV22T1020761.Models.Common;
using SV22T1020761.Models.Partner;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace SV22T1020761.Admin.Controllers
{
    [Authorize]
    public class ShipperController : Controller
    {
        private readonly ILogger<ShipperController> _logger;

        public ShipperController(ILogger<ShipperController> logger)
        {
            _logger = logger;
        }

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
                var pagedResult = PartnerDataService.ListShippers(input);
                var model = new PagedResult<SV22T1020761.Models.Shipper>
                {
                    Page = pagedResult.Page,
                    PageSize = pagedResult.PageSize,
                    RowCount = pagedResult.RowCount,
                    DataItems = pagedResult.DataItems.Select(shipper => new SV22T1020761.Models.Shipper
                    {
                        ShipperID = shipper.ShipperID,
                        ShipperName = shipper.ShipperName,
                        Phone = shipper.Phone
                    }).ToList()
                };

                return View(model);
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Error loading shippers");
                TempData["Error"] = "Không thể kết nối tới CSDL. Vui lòng kiểm tra cấu hình và thử lại.";
                var empty = new PagedResult<SV22T1020761.Models.Shipper> { Page = page, PageSize = pageSize, RowCount = 0, DataItems = new System.Collections.Generic.List<SV22T1020761.Models.Shipper>() };
                return View(empty);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(PaginationSearchInput input)
        {
            var pagedResult = PartnerDataService.ListShippers(input);
            var model = new PagedResult<SV22T1020761.Models.Shipper>
            {
                Page = pagedResult.Page,
                PageSize = pagedResult.PageSize,
                RowCount = pagedResult.RowCount,
                DataItems = pagedResult.DataItems.Select(shipper => new SV22T1020761.Models.Shipper
                {
                    ShipperID = shipper.ShipperID,
                    ShipperName = shipper.ShipperName,
                    Phone = shipper.Phone
                }).ToList()
            };
            return PartialView("_ShipperTable", model);
        }

        [HttpGet]
        public IActionResult Form(int? id, bool delete = false)
        {
            if (id == null || id == 0)
            {
                var model = new Shipper();
                if (delete) return BadRequest();
                return PartialView("_ShipperForm", model);
            }
            var shipper = PartnerDataService.GetShipper(id.Value);
            if (shipper == null) return NotFound();
            if (delete) return PartialView("_ShipperDelete", shipper);
            return PartialView("_ShipperForm", shipper);
        }

        public IActionResult Create()
        {
            try
            {
                return View(new Shipper());
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Error loading create shipper form");
                TempData["Error"] = "Lỗi khi tải form. Vui lòng thử lại sau.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Shipper shipper)
        {
            try
            {
                if (!ModelState.IsValid) return View(shipper);
                PartnerDataService.AddShipper(shipper);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var input = new PaginationSearchInput { Page = 1, PageSize = 10, SearchValue = "" };
                    var pagedResult = PartnerDataService.ListShippers(input);
                    var model = new PagedResult<SV22T1020761.Models.Shipper>
                    {
                        Page = pagedResult.Page,
                        PageSize = pagedResult.PageSize,
                        RowCount = pagedResult.RowCount,
                        DataItems = pagedResult.DataItems.Select(s => new SV22T1020761.Models.Shipper { ShipperID = s.ShipperID, ShipperName = s.ShipperName, Phone = s.Phone }).ToList()
                    };
                    return PartialView("_ShipperTable", model);
                }
                TempData["Success"] = "Thêm đối tác giao hàng thành công.";
                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Error creating shipper");
                ModelState.AddModelError(string.Empty, "hệ thống đang bận. Vui lòng thử lại sau.");
                return View(shipper);
            }
        }

        public IActionResult Edit(int id)
        {
            var shipper = PartnerDataService.GetShipper(id);
            if (shipper == null)
            {
                return NotFound();
            }
            return View(shipper);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Shipper shipper)
        {
            try
            {
                if (!ModelState.IsValid) return View(shipper);
                PartnerDataService.UpdateShipper(shipper);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var input = new PaginationSearchInput { Page = 1, PageSize = 10, SearchValue = "" };
                    var pagedResult = PartnerDataService.ListShippers(input);
                    var model = new PagedResult<SV22T1020761.Models.Shipper>
                    {
                        Page = pagedResult.Page,
                        PageSize = pagedResult.PageSize,
                        RowCount = pagedResult.RowCount,
                        DataItems = pagedResult.DataItems.Select(s => new SV22T1020761.Models.Shipper { ShipperID = s.ShipperID, ShipperName = s.ShipperName, Phone = s.Phone }).ToList()
                    };
                    return PartialView("_ShipperTable", model);
                }
                TempData["Success"] = "Cập nhật đối tác giao hàng thành công.";
                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Error updating shipper (Id={ShipperId})", shipper?.ShipperID);
                ModelState.AddModelError(string.Empty, "hệ thống đang bận. Vui lòng thử lại sau.");
                return View(shipper);
            }
        }

        public IActionResult Delete(int id)
        {
            var shipper = PartnerDataService.GetShipper(id);
            if (shipper == null)
            {
                return NotFound();
            }
            return View(shipper);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                bool deleted = PartnerDataService.DeleteShipper(id);
                if (!deleted)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return BadRequest("Không thể xóa đối tác giao hàng này.");
                    }
                    TempData["Error"] = "Không thể xóa đối tác giao hàng này.";
                    return RedirectToAction("Index");
                }

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var input = new PaginationSearchInput { Page = 1, PageSize = 10, SearchValue = "" };
                    var pagedResult = PartnerDataService.ListShippers(input);
                    var model = new PagedResult<SV22T1020761.Models.Shipper>
                    {
                        Page = pagedResult.Page,
                        PageSize = pagedResult.PageSize,
                        RowCount = pagedResult.RowCount,
                        DataItems = pagedResult.DataItems.Select(s => new SV22T1020761.Models.Shipper { ShipperID = s.ShipperID, ShipperName = s.ShipperName, Phone = s.Phone }).ToList()
                    };
                    return PartialView("_ShipperTable", model);
                }
                TempData["Success"] = "Xóa đối tác giao hàng thành công.";
                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Error deleting shipper (Id={ShipperId})", id);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return BadRequest("Lỗi khi xóa: " + ex.Message);
                }
                var shipper = PartnerDataService.GetShipper(id);
                ModelState.AddModelError(string.Empty, "Không thể xóa đối tác giao hàng. Vui lòng thử lại sau.");
                return View("Delete", shipper);
            }
        }
    }
}
