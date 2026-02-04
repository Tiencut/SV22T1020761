namespace SV22T1020761.Models
{
    /// <summary>
    /// Đơn hàng
    /// </summary>
    public class Order
    {
        public int OrderID { get; set; }
        public int? CustomerID { get; set; }
        public DateTime OrderTime { get; set; }
        public string DeliveryProvince { get; set; } = "";
        public string DeliveryAddress { get; set; } = "";
        public int? EmployeeID { get; set; }
        public DateTime? AcceptTime { get; set; }
        public int? ShipperID { get; set; }
        public DateTime? ShippedTime { get; set; }
        public DateTime? FinishedTime { get; set; }
        public int Status { get; set; }
    }
}