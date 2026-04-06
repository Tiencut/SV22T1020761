namespace SV22T1020761.Models.Security
{
    /// <summary>
    /// Thông tin tài khoản người dùng
    /// </summary>
    public class UserAccount
    {
        /// <summary>
        /// Mã tài khoản
        /// </summary>
        public string UserId { get; set; } = "";
        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        public string UserName { get; set; } = "";
        /// <summary>
        /// Tên hiển thị (thường là họ tên của người dùng, hoặc có thể là tên đăng nhập nếu không có họ tên)
        /// </summary>
        public string DisplayName { get; set; } = "";
        /// <summary>
        /// Địa chỉ email (nếu có)
        /// </summary>
        public string Email { get; set; } = "";
        /// <summary>
        /// Tên file ảnh đại diện của người dùng (nếu có)
        /// </summary>
        public string Photo { get; set; } = "";
        /// <summary>
        /// Danh sách tên các vai trò/quyền của người dùng, được phân cách bởi dấu chấm phẩy (nếu có)
        /// </summary>
        public string RoleNames { get; set; } = "";
        /// <summary>
        /// Tên khách hàng (tên công ty/cá nhân)
        /// </summary>
        public string CustomerName { get; set; } = "";
        /// <summary>
        /// Tên liên hệ
        /// </summary>
        public string ContactName { get; set; } = "";
        /// <summary>
        /// Tỉnh/Thành phố
        /// </summary>
        public string Province { get; set; } = "";
        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; } = "";
        /// <summary>
        /// Điện thoại
        /// </summary>
        public string Phone { get; set; } = "";
        /// <summary>
        /// Tài khoản bị khóa hay không
        /// </summary>
        public bool? IsLocked { get; set; }
    }
}
