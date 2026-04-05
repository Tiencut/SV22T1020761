using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV22T1020761.Admin.AppCodes;
using SV22T1020761.BusinessLayers;
using SV22T1020761.Models;
using SV22T1020761.Models.HR;

namespace SV22T1020761.Admin.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Login");
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(SV22T1020761.Models.LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                // Validate credentials: Mock - in production, validate against database
                // TODO: Replace with actual employee validation from HRDataService
                var isValidCredentials = ValidateCredentials(model.UserName, model.Password);
                
                if (!isValidCredentials)
                {
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng.");
                    return View(model);
                }

                // Create user data with mock employee info
                var userData = new WebUserData
                {
                    UserId = "1",
                    UserName = model.UserName,
                    DisplayName = model.UserName,
                    Email = $"{model.UserName}@example.com",
                    Photo = "default.png",
                    Roles = new List<string> { WebUserRoles.Administrator }
                };

                // Create principal from user data
                var principal = userData.CreatePrincipal();

                // Sign in with cookie authentication
                await HttpContext.SignInAsync("Cookies", principal, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24)
                });

                return RedirectToAction("Index", "Home");
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = "Đăng nhập thất bại. Vui lòng thử lại sau: " + ex.Message;
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogoutPost()
        {
            try
            {
                await HttpContext.SignOutAsync("Cookies");
                return RedirectToAction("Login");
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = "Đăng xuất thất bại. Vui lòng thử lại sau.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(SV22T1020761.Admin.Models.ChangePasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                // Verify old password
                var currentUser = User.GetUserData();
                if (currentUser == null)
                {
                    ModelState.AddModelError(string.Empty, "Không thể xác định người dùng hiện tại.");
                    return View(model);
                }

                // TODO: Replace with actual employee password validation
                // For now, accept any non-empty old password
                if (string.IsNullOrWhiteSpace(model.OldPassword))
                {
                    ModelState.AddModelError(nameof(model.OldPassword), "Mật khẩu cũ không được để trống.");
                    return View(model);
                }

                if (string.IsNullOrWhiteSpace(model.NewPassword))
                {
                    ModelState.AddModelError(nameof(model.NewPassword), "Mật khẩu mới không được để trống.");
                    return View(model);
                }

                if (model.NewPassword != model.ConfirmPassword)
                {
                    ModelState.AddModelError(nameof(model.ConfirmPassword), "Xác nhận mật khẩu không khớp.");
                    return View(model);
                }

                // TODO: Update password in database
                var hashedPassword = CryptHelper.HashMD5(model.NewPassword);

                TempData["SuccessMessage"] = "Đổi mật khẩu thành công.";
                return RedirectToAction("Index", "Home");
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = "Đổi mật khẩu thất bại. Vui lòng thử lại sau: " + ex.Message;
                return View(model);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
                    TempData["Error"] = $"Dữ liệu không hợp lệ: {errors}";
                    return RedirectToAction("Login");
                }

                // Check if username/email already exists
                var existingEmployee = await HRDataService.GetEmployeeByEmailAsync(model.Email);
                if (existingEmployee != null)
                {
                    TempData["Error"] = "Email này đã được đăng ký.";
                    return RedirectToAction("Login");
                }

                if (string.IsNullOrWhiteSpace(model.UserName))
                {
                    TempData["Error"] = "Tên đăng nhập không được để trống.";
                    return RedirectToAction("Login");
                }

                if (string.IsNullOrWhiteSpace(model.Email))
                {
                    TempData["Error"] = "Email không được để trống.";
                    return RedirectToAction("Login");
                }

                if (model.Password != model.ConfirmPassword)
                {
                    TempData["Error"] = "Mật khẩu xác nhận không khớp.";
                    return RedirectToAction("Login");
                }

                // Create new employee from registration data
                var newEmployee = new SV22T1020761.Models.HR.Employee
                {
                    FullName = model.DisplayName,
                    Email = model.Email,
                    Password = CryptHelper.HashMD5(model.Password),
                    Photo = "default.png",
                    IsWorking = true,
                    RoleNames = "User" // Default role
                };

                // Save to database
                var employeeId = await HRDataService.AddEmployeeAsync(newEmployee);

                TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login");
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = "Đăng ký thất bại. Vui lòng thử lại sau: " + ex.Message;
                return RedirectToAction("Login");
            }
        }

        /// <summary>
        /// Kiểm tra thông tin đăng nhập (Mock - thay thế bằng validation từ database)
        /// </summary>
        private bool ValidateCredentials(string username, string password)
        {
            // Mock credentials for testing
            // TODO: Replace with actual employee validation from HRDataService
            var mockUser = "admin";
            var mockPassword = "admin"; // In production, store hashed password in DB

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return false;

            // Hash the input password and compare
            var hashedPassword = CryptHelper.HashMD5(password);
            var hashedMockPassword = CryptHelper.HashMD5(mockPassword);

            return username == mockUser && hashedPassword == hashedMockPassword;
        }
    }
}
