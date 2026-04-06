using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV22T1020761.Models.Sales;
using SV22T1020761.Shop.AppCodes;
using SV22T1020761.BusinessLayers;
using SV22T1020761.DataLayers.SQLServer.Sales;
using SV22T1020761.Shop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SV22T1020761.Shop.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var username = User?.Identity?.Name ?? "";
            var ua = AccountService.GetUser(username);
            if (ua == null || !int.TryParse(ua.UserId, out var customerId)) return Unauthorized();

            var cart = await GetCartFromDB(customerId);
            var summary = (cart?.Sum(c => c.Qty) ?? 0, cart?.Sum(c => c.Price * c.Qty) ?? 0);
            
            ViewBag.Cart = cart;
            ViewBag.Summary = summary;
            ViewBag.Provinces = SelectListHelper.GetProvinces();
            
            // Prefill customer province and address
            ViewBag.CustomerProvince = ua.Address ?? "";
            ViewBag.CustomerAddress = ua.Address ?? "";
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Place(string deliveryProvince, string deliveryAddress)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(deliveryProvince))
            {
                TempData["Error"] = "Vui lòng chọn Tỉnh/Thành phố";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(deliveryAddress))
            {
                TempData["Error"] = "Vui lòng nhập địa chỉ giao hàng";
                return RedirectToAction("Index");
            }

            var username = User?.Identity?.Name ?? "";
            var ua = AccountService.GetUser(username);
            if (ua == null || !int.TryParse(ua.UserId, out var customerId)) return Unauthorized();

            var cart = await GetCartFromDB(customerId);
            if (cart == null || cart.Count == 0)
            {
                TempData["Error"] = "Giỏ hàng rỗng";
                return RedirectToAction("Index", "Cart");
            }

            try
            {
                // Get Draft order from DB
                var orders = SalesDataService.ListOrders(
                    new OrderSearchInput 
                    { 
                        Page = 1, 
                        PageSize = 100,
                        Status = OrderStatusEnum.New,
                        CustomerID = customerId
                    });
                
                var draftOrder = orders?.DataItems?.FirstOrDefault(o => 
                    o.CustomerID == customerId && 
                    o.Status == OrderStatusEnum.New);

                if (draftOrder != null)
                {
                    // Update New → Accepted
                    var orderToUpdate = new Order
                    {
                        OrderID = draftOrder.OrderID,
                        CustomerID = draftOrder.CustomerID,
                        OrderTime = draftOrder.OrderTime,
                        DeliveryProvince = deliveryProvince,
                        DeliveryAddress = deliveryAddress,
                        Status = OrderStatusEnum.Accepted
                    };
                    
                    var repo = new OrderRepository(Configuration.ConnectionString);
                    await repo.UpdateAsync(orderToUpdate);
                    
                    TempData["Success"] = "Đặt hàng thành công";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Error"] = "Không tìm thấy đơn hàng";
                    return RedirectToAction("Index", "Checkout");
                }
            }
            catch
            {
                TempData["Error"] = "Không thể tạo đơn hàng. Vui lòng thử lại.";
                return RedirectToAction("Index", "Checkout");
            }
        }

        private async Task<List<CartItem>> GetCartFromDB(int customerId)
        {
            var cartItems = new List<CartItem>();

            try
            {
                var repo = new OrderRepository(Configuration.ConnectionString);
                var orders = SalesDataService.ListOrders(
                    new OrderSearchInput 
                    { 
                        Page = 1, 
                        PageSize = 100,
                        Status = OrderStatusEnum.New,
                        CustomerID = customerId
                    });
                
                var draftOrder = orders?.DataItems?.FirstOrDefault(o => 
                    o.CustomerID == customerId && 
                    o.Status == OrderStatusEnum.New);

                if (draftOrder != null)
                {
                    var details = await repo.ListDetailsAsync(draftOrder.OrderID);

                    if (details != null && details.Count > 0)
                    {
                        foreach (var detail in details)
                        {
                            cartItems.Add(new CartItem
                            {
                                ProductID = detail.ProductID,
                                ProductName = detail.ProductName ?? "",
                                Price = detail.SalePrice,
                                Qty = detail.Quantity,
                                Photo = detail.Photo
                            });
                        }
                    }
                }
            }
            catch { }

            return cartItems;
        }
    }
}
