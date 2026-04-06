using SV22T1020761.BusinessLayers;
using SV22T1020761.Models.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace SV22T1020761.Shop.AppCodes
{
    public static class SelectListHelper
    {
        /// <summary>
        /// Lấy danh sách tỉnh/thành phố
        /// </summary>
        public static List<SelectListItem> GetProvinces()
        {
            var list = new List<SelectListItem>()
            {
                new SelectListItem() { Value = "", Text = "-- Chọn Tỉnh/Thành phố --" }
            };
            
            try
            {
                var input = new PaginationSearchInput() { Page = 1, PageSize = 0, SearchValue = "" };
                var result = DictionaryDataService.ListProvinces(input);
                
                if (result?.DataItems != null)
                {
                    foreach (var item in result.DataItems)
                    {
                        list.Add(new SelectListItem()
                        {
                            Value = item,
                            Text = item
                        });
                    }
                }
            }
            catch { }
            
            return list;
        }
    }
}
