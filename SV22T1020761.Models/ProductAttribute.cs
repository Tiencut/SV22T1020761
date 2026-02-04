namespace SV22T1020761.Models
{
    /// <summary>
    /// Thuộc tính của sản phẩm
    /// </summary>
    public class ProductAttribute
    {
        public long AttributeID { get; set; }
        public int ProductID { get; set; }
        public string AttributeName { get; set; } = "";
        public string AttributeValue { get; set; } = "";
        public int DisplayOrder { get; set; } = 0;
    }
}