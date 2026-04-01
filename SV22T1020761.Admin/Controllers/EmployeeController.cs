using Microsoft.AspNetCore.Mvc;
using SV22T1020761.BusinessLayers;
using SV22T1020761.Models.Common;
using SV22T1020761.Models.HR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Logging;

namespace SV22T1020761.Admin.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string searchValue, int page = 1, int pageSize = 10)
        {
            var input = new PaginationSearchInput
            {
                SearchValue = searchValue,
                Page = page,
                PageSize = pageSize
            };

            try
            {
                var pagedResult = HRDataService.ListEmployees(input);
                return View(pagedResult);
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Error loading employees");
                TempData["Error"] = "Kh�ng th? k?t n?i t?i c� s? d? li?u. Vui l?ng ki?m tra c?u h?nh v� th? l?i.";
                var empty = new PagedResult<Employee> { Page = page, PageSize = pageSize, RowCount = 0, DataItems = new System.Collections.Generic.List<Employee>() };
                return View(empty);
            }
        }

        // Return partial form for modal (create or edit)
        [HttpGet]
        public async Task<IActionResult> Form(int? id, bool delete = false)
        {
            if (id == null || id == 0)
            {
                ViewBag.Title = "B? sung nh�n vi�n";
                var model = new Employee() { EmployeeID = 0, IsWorking = true };
                if (delete) return BadRequest();
                return PartialView("_EmployeeForm", model);
            }
            else
            {
                ViewBag.Title = "C?p nh?t th�ng tin nh�n vi�n";
                var model = await HRDataService.GetEmployeeAsync(id.Value);
                if (model == null) return NotFound();
                if (delete) return PartialView("_EmployeeDelete", model);
                return PartialView("_EmployeeForm", model);
            }
        }

        public IActionResult Create()
        {
            ViewBag.Title = "B? sung nh�n vi�n";
            var model = new Employee()
            {
                EmployeeID = 0,
                IsWorking = true
            };
            return View("Edit", model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Title = "C?p nh?t th�ng tin nh�n vi�n";
            var model = await HRDataService.GetEmployeeAsync(id);
            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveData(Employee data, IFormFile? uploadPhoto)
        {
            try
            {
                ViewBag.Title = data.EmployeeID == 0 ? "B? sung nh�n vi�n" : "C?p nh?t th�ng tin nh�n vi�n";

                //Ki?m tra d? li?u �?u v�o: FullName v� Email l� b?t bu?c, Email ch�a ��?c s? d?ng b?i nh�n vi�n kh�c
                if (string.IsNullOrWhiteSpace(data.FullName))
                    ModelState.AddModelError(nameof(data.FullName), "Vui l?ng nh?p h? t�n nh�n vi�n");

                if (string.IsNullOrWhiteSpace(data.Email))
                    ModelState.AddModelError(nameof(data.Email), "Vui l?ng nh?p email nh�n vi�n");
                else if (!await HRDataService.ValidateEmployeeEmailAsync(data.Email, data.EmployeeID))
                    ModelState.AddModelError(nameof(data.Email), "Email �? ��?c s? d?ng b?i nh�n vi�n kh�c");

                if (!ModelState.IsValid)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        return PartialView("_EmployeeForm", data);
                    return View("Edit", data);
                }

                //X? l? upload ?nh
                if (uploadPhoto != null)
                {
                    var fileName = $"{System.Guid.NewGuid()}{Path.GetExtension(uploadPhoto.FileName)}";
                    var filePath = Path.Combine(ApplicationContext.WWWRootPath, "images/employees", fileName);
                    // ensure directory exists
                    var dir = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir!);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await uploadPhoto.CopyToAsync(stream);
                    }
                    data.Photo = fileName;
                }

                //Ti?n x? l? d? li?u tr�?c khi l�u v�o database
                if (string.IsNullOrEmpty(data.Address)) data.Address = "";
                if (string.IsNullOrEmpty(data.Phone)) data.Phone = "";
                if (string.IsNullOrEmpty(data.Photo)) data.Photo = "nophoto.png";

                //L�u d? li?u v�o database (b? sung ho?c c?p nh?t)
                if (data.EmployeeID == 0)
                {
                    await HRDataService.AddEmployeeAsync(data);
                }
                else
                {
                    await HRDataService.UpdateEmployeeAsync(data);
                }

                // If AJAX, return updated table partial
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var input = new PaginationSearchInput { Page = 1, PageSize = 10, SearchValue = "" };
                    var result = HRDataService.ListEmployees(input);
                    return PartialView("_EmployeeTable", result);
                }

                TempData["Success"] = data.EmployeeID == 0 ? "B? sung nh�n vi�n th�nh c�ng." : "C?p nh?t nh�n vi�n th�nh c�ng.";
                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                // Log the error and return the Edit view with a friendly message
                _logger?.LogError(ex, "Error saving employee data (EmployeeID={EmployeeID}).", data?.EmployeeID);
                ModelState.AddModelError(string.Empty, "H? th?ng �ang b?n ho?c d? li?u kh�ng h?p l?. Vui l?ng ki?m tra d? li?u ho?c th? l?i sau");
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return PartialView("_EmployeeForm", data);
                return View("Edit", data);
            }
        }

        public IActionResult Delete(int id)
        {
            var employee = HRDataService.GetEmployee(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                HRDataService.DeleteEmployee(id);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var input = new PaginationSearchInput { Page = 1, PageSize = 10, SearchValue = "" };
                    var result = HRDataService.ListEmployees(input);
                    return PartialView("_EmployeeTable", result);
                }

                TempData["Success"] = "Xóa nhân viên thành công.";
                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Error deleting employee (EmployeeID={EmployeeID}).", id);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return BadRequest("Lỗi khi xóa: " + ex.Message);
                }
                // Try to reload the employee to show details and the error message
                var employee = HRDataService.GetEmployee(id);
                ModelState.AddModelError(string.Empty, "Không thể xóa nhân viên. Vui lòng kiểm tra dữ liệu hoặc thử lại sau.");
                return View("Delete", employee);
            }
        }

        [HttpPost]
        public IActionResult Search(PaginationSearchInput input)
        {
            // Save search conditions to session
            ApplicationContext.SetSessionData("EmployeeSearchConditions", input);

            // Fetch data based on search conditions
            var result = HRDataService.ListEmployees(input);

            // Return partial view with updated data
            return PartialView("_EmployeeTable", result);
        }
    }
}
