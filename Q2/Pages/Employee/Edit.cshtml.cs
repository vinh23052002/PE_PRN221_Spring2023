using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Q2.Hubs;
using Q2.Models;

namespace Q2.Pages.Employee
{
    public class EditModel : PageModel
    {
        PRN_Spr23_B1Context _context = new PRN_Spr23_B1Context();
        IHubContext<EmployeeHub> _hubContext;

        [BindProperty]
        public Models.Employee employee { get; set; }
        public int eId;
        public EditModel(IHubContext<EmployeeHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public void OnGet(int id)
        {
            eId = id;
            employee = _context.Employees
                .Include(p => p.Department)
                .SingleOrDefault(p => p.EmployeeId == id);

            ViewData["Department"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
            ViewData["TitleOfCourtesy"] = new SelectList(_context.Employees.Select(p => p.TitleOfCourtesy).Distinct().ToList());
        }

        public IActionResult OnPost()
        {

            var department = _context.Departments.SingleOrDefault(p => p.DepartmentId == employee.DepartmentId);

            Models.Employee eUpdate = _context.Employees.SingleOrDefault(p => p.EmployeeId == employee.EmployeeId);

            eUpdate.FirstName = employee.FirstName;
            eUpdate.LastName = employee.LastName;
            eUpdate.Title = employee.Title;
            eUpdate.BirthDate = employee.BirthDate;
            eUpdate.DepartmentId = employee.DepartmentId;
            eUpdate.TitleOfCourtesy = employee.TitleOfCourtesy;
            _context.Employees.Update(eUpdate);
            _context.SaveChanges();
            _hubContext.Clients.All.SendAsync("EditEmployee", employee.EmployeeId, employee.FirstName + " " + employee.LastName, employee.Title, employee.BirthDate?.ToString("dd/MM/yyyy"), department.DepartmentName);
            return RedirectToPage("/Employee/EmployeeList");
        }

       
    }
}
