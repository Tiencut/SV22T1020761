namespace SV22T1020761.Models
{
    /// <summary>
    /// Danh mục sản phẩm
    /// </summary>
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = "";
        public string Description { get; set; } = "";
    }
}