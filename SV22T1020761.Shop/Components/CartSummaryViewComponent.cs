using Microsoft.AspNetCore.Mvc;
using SV22T1020761.DataLayers.SQLServer.Sales;
using SV22T1020761.Shop.AppCodes;
using SV22T1020761.Shop.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SV22T1020761.Shop.Components
{
    public class CartSummaryViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Get current user
            var username = User?.Identity?.Name ?? "";
            if (string.IsNullOrEmpty(username))
            {
                // Not authenticated - return empty cart
                return View((0, 0m));
            }

            // Get customer ID from user account
            var ua = AccountService.GetUser(username);
            if (ua == null || !int.TryParse(ua.UserId, out var customerId))
            {
                return View((0, 0m));
            }

            // Get cart from database
            try
            {
                var repo = new OrderRepository(SV22T1020761.BusinessLayers.Configuration.ConnectionString);
                var orders = await Task.Run(() => 
                    SV22T1020761.BusinessLayers.SalesDataService.ListOrders(
                        new SV22T1020761.Models.Sales.OrderSearchInput 
                        { 
                            Page = 1, 
                            PageSize = 1,
                            Status = SV22T1020761.Models.Sales.OrderStatusEnum.New, // FIXED: Explicitly set Status to New (1)
                            CustomerID = customerId
                        }));
                
                var draftOrder = orders?.DataItems?.FirstOrDefault(o => 
                    o.Status == SV22T1020761.Models.Sales.OrderStatusEnum.New);

                if (draftOrder != null)
                {
                    var details = await repo.ListDetailsAsync(draftOrder.OrderID);
                    if (details != null && details.Count > 0)
                    {
                        var count = details.Sum(d => d.Quantity);
                        var total = details.Sum(d => d.SalePrice * d.Quantity);
                        return View((count, total));
                    }
                }
            }
            catch { }

            return View((0, 0m));
        }
    }
}
