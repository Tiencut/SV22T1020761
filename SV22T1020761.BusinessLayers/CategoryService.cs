using System.Collections.Generic;
using SV22T1020761.DataLayers;
using SV22T1020761.Models;

namespace SV22T1020761.BusinessLayers
{
    /// <summary>
    /// Service cho Category
    /// </summary>
    public static class CategoryService
    {
        private static ICategoryDAL categoryDB;

        /// <summary>
        /// Khởi tạo service
        /// </summary>
        static CategoryService()
        {
            categoryDB = new CategoryDB();
        }

        /// <summary>
        /// Lấy danh sách category
        /// </summary>
        public static IList<Category> ListCategories(string searchValue = "", int page = 1, int pageSize = 0)
        {
            return categoryDB.List(page, pageSize, searchValue);
        }

        /// <summary>
        /// Lấy category theo ID
        /// </summary>
        public static Category? GetCategory(int categoryID)
        {
            return categoryDB.Get(categoryID);
        }

        /// <summary>
        /// Thêm category
        /// </summary>
        public static int AddCategory(Category data)
        {
            return categoryDB.Add(data);
        }

        /// <summary>
        /// Cập nhật category
        /// </summary>
        public static bool UpdateCategory(Category data)
        {
            return categoryDB.Update(data);
        }

        /// <summary>
        /// Xóa category
        /// </summary>
        public static bool DeleteCategory(int categoryID)
        {
            return categoryDB.Delete(categoryID);
        }

        /// <summary>
        /// Đếm số lượng category
        /// </summary>
        public static int CountCategories(string searchValue = "")
        {
            return categoryDB.Count(searchValue);
        }
    }
}