using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Q2.Models;

namespace Q2.Pages.Employee
{
    public class EmployeeListModel : PageModel
    {
        private readonly PRN_Spr23_B1Context _context;

        public EmployeeListModel(PRN_Spr23_B1Context context)
        {
            _context = context;
        }
        public IList<Models.Employee> employees { get; set; } = default;
        public void OnGet()
        {
            if (_context.Employees != null)
            {
                employees = _context.Employees
                    .Include(p => p.Department)
                    .ToList();
            }

        }
    }
}
