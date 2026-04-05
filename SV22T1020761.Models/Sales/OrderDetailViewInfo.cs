namespace SV22T1020761.Models.Sales
{
    /// <summary>
    /// DTO hiển thị thông tin chi tiết của mặt hàng trong đơn hàng
    /// </summary>
    public class OrderDetailViewInfo : OrderDetail
    {
        /// <summary>
        /// Tên hàng
        /// </summary>
        public string ProductName { get; set; } = "";
        
        /// <summary>
        /// Đơn vị tính
        /// </summary>
        public string Unit { get; set; } = "";
        
        /// <summary>
        /// Tên file ảnh
        /// </summary>
        public string Photo { get; set; } = "";

        /// <summary>
        /// Constructor mặc định cho JSON deserialization
        /// </summary>
        public OrderDetailViewInfo() : base()
        {
            ProductName = "";
            Unit = "";
            Photo = "";
        }

        /// <summary>
        /// Constructor với tham số
        /// </summary>
        public OrderDetailViewInfo(int productID, int quantity, decimal salePrice, string productName, string unit, string photo)
        {
            ProductID = productID;
            Quantity = quantity;
            SalePrice = salePrice;
            ProductName = productName;
            Unit = unit;
            Photo = photo;
        }
    }
}
