using Microsoft.AspNetCore.Mvc;
using SV22T1020761.Models;
using System.Collections.Generic;
using System.Linq;

namespace SV22T1020761.Adnub.Controllers
{
    public class SupplierController : Controller
    {
        // Small mock dataset
        private static List<Supplier> _mockSuppliers = new List<Supplier>
        {
            new Supplier { SupplierID = 1, SupplierName = "NCC A", ContactName = "Nguyen A", Province = "Hanoi", Address = "123 A St", Phone = "0123456789", Email = "a@example.com" },
            new Supplier { SupplierID = 2, SupplierName = "NCC B", ContactName = "Tran B", Province = "HCMC", Address = "456 B St", Phone = "0987654321", Email = "b@example.com" },
            new Supplier { SupplierID = 3, SupplierName = "NCC C", ContactName = "Le C", Province = "Danang", Address = "789 C St", Phone = "0112233445", Email = "c@example.com" },
            new Supplier { SupplierID = 4, SupplierName = "NCC D", ContactName = "Pham D", Province = "Hue", Address = "10 D St", Phone = "0223344556", Email = "d@example.com" },
            new Supplier { SupplierID = 5, SupplierName = "NCC E", ContactName = "Ho E", Province = "Can Tho", Address = "11 E St", Phone = "0334455667", Email = "e@example.com" },
            new Supplier { SupplierID = 6, SupplierName = "NCC F", ContactName = "Dang F", Province = "Nha Trang", Address = "12 F St", Phone = "0445566778", Email = "f@example.com" }
        };

        // Show small set (max 5) using a for-loop in the view
        public IActionResult Index(string searchValue = "")
        {
            var list = string.IsNullOrWhiteSpace(searchValue)
                ? _mockSuppliers
                : _mockSuppliers.Where(s => s.SupplierName.Contains(searchValue) || s.ContactName.Contains(searchValue)).ToList();

            return View(list);
        }

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Supplier model)
        {
            if (!ModelState.IsValid) return View(model);
            model.SupplierID = (_mockSuppliers.Any() ? _mockSuppliers.Max(s => s.SupplierID) + 1 : 1);
            _mockSuppliers.Add(model);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id) => View(_mockSuppliers.FirstOrDefault(s => s.SupplierID == id));

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Supplier model)
        {
            if (!ModelState.IsValid) return View(model);
            var item = _mockSuppliers.FirstOrDefault(s => s.SupplierID == model.SupplierID);
            if (item == null) return NotFound();
            item.SupplierName = model.SupplierName;
            item.ContactName = model.ContactName;
            item.Province = model.Province;
            item.Address = model.Address;
            item.Phone = model.Phone;
            item.Email = model.Email;
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id) => View(_mockSuppliers.FirstOrDefault(s => s.SupplierID == id));

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var item = _mockSuppliers.FirstOrDefault(s => s.SupplierID == id);
            if (item != null) _mockSuppliers.Remove(item);
            return RedirectToAction(nameof(Index));
        }
    }
}

