namespace SV22T1020761.Models
{
    /// <summary>
    /// Ảnh của sản phẩm
    /// </summary>
    public class ProductPhoto
    {
        public long PhotoID { get; set; }
        public int ProductID { get; set; }
        public string Photo { get; set; } = "";
        public string Description { get; set; } = "";
        public int DisplayOrder { get; set; } = 0;
        public bool IsHidden { get; set; } = false;
    }
}