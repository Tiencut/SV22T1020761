using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV22T1020218.BusinessLayers;
using SV22T1020218.Models;
using SV22T1020218.Models.Catalog;
using SV22T1020218.Models.Common;
using SV22T1020218.Models.Sales;

namespace SV22T1020218.Admin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        /// <summary>
        /// Hiển thị trang chủ của ứng dụng với dữ liệu thống kê
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var model = new DashboardModel();
            try
            {
                // 1. Đếm tổng số Khách hàng
                var customerData = await PartnerDataService.ListCustomersAsync(new PaginationSearchInput { Page = 1, PageSize = 1, SearchValue = "" });
                model.TotalCustomers = customerData.RowCount;

                // 2. Đếm tổng số Sản phẩm
                var productData = await CatalogDataService.ListProductsAsync(new ProductSearchInput { Page = 1, PageSize = 1, SearchValue = "" });
                model.TotalProducts = productData.RowCount;

                // 3. Đếm tổng số Đơn hàng
                var orderData = await SalesDataService.ListOrdersAsync(new OrderSearchInput { Page = 1, PageSize = 1, SearchValue = "", Status = 0 });
                model.TotalOrders = orderData.RowCount;

                // 4. Lấy danh sách 5 đơn hàng "Vừa tạo" (Status = 1) để chờ duyệt
                var pendingOrderData = await SalesDataService.ListOrdersAsync(new OrderSearchInput { Page = 1, PageSize = 5, SearchValue = "", Status = (OrderStatusEnum)1 });
                model.TotalPendingOrders = pendingOrderData.RowCount;
                model.PendingOrders = pendingOrderData.DataItems;

                return View(model);
            }
            catch (Exception ex)
            {
                // Nếu DB có vấn đề, vẫn trả về giao diện trắng để không sập web
                return View(model);
            }
        }
    }
}