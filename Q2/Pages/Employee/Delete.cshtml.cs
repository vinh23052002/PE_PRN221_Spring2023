using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Q2.Models;

namespace Q2.Pages.Employee
{
    public class DeleteModel : PageModel
    {
        PRN_Spr23_B1Context _context = new PRN_Spr23_B1Context();   

        public IActionResult OnGet(int id)
        {
            var Employee = _context.Employees.SingleOrDefault(p => p.EmployeeId == id);
            _context.Employees.Remove(Employee);
            _context.SaveChanges();

            return RedirectToPage("/Employee/EmployeeList");
        }
    }
}
