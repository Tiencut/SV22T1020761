using Microsoft.AspNetCore.Mvc;

namespace SV22T1020761.Admin.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Logout()
        {
            return View();
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login");  
        }

        public IActionResult ChangePassword()
        {
            return View();
        }
    }
}
