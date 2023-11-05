using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Q2.Hubs;
using Q2.Models;
using System.Text.Json;

namespace Q2.Pages.Employee
{
    public class AddEmployeeModel : PageModel
    {
        PRN_Spr23_B1Context _context = new PRN_Spr23_B1Context();
        IHubContext<EmployeeHub> _hubContext;
        [BindProperty]
        public Models.Employee employee { get; set; }

        public AddEmployeeModel(IHubContext<EmployeeHub> hubContext) {
            _hubContext = hubContext;
        }
        public void OnGet()
        {
            ViewData["Department"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
            ViewData["TitleOfCourtesy"] = new SelectList(_context.Employees.Select(p => p.TitleOfCourtesy).Distinct().ToList());
        }

        public IActionResult OnPost()
        {
            //HttpContext.Session.SetString("employee",JsonSerializer.Serialize(employee));
            //var e = JsonSerializer.Deserialize<Models.Employee>(HttpContext.Session.GetString("employee"));

            var department = _context.Departments.SingleOrDefault(p  => p.DepartmentId == employee.DepartmentId);

            _context.Employees.Add(employee);
            _context.SaveChanges();
            _hubContext.Clients.All.SendAsync("AddEmployee",employee.EmployeeId,employee.FirstName+" "+employee.LastName,employee.Title,employee.BirthDate?.ToString("dd/MM/yyyy"), department.DepartmentName);
            return RedirectToPage("/Employee/EmployeeList");
        }

        [HttpPost]
        public IActionResult Create(String LastName, String FirstName)
        {

            return null;
        }
    }
}
