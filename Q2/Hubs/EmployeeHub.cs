using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Q2.Models;

namespace Q2.Hubs
{
    public class EmployeeHub :Hub
    {
        PRN_Spr23_B1Context _context = new PRN_Spr23_B1Context();

        public void DeleteEmployee(int id)
        {
            var Employee = _context.Employees.SingleOrDefault(p => p.EmployeeId == id);
            _context.Employees.Remove(Employee);
            _context.SaveChanges();
            this.Clients.All.SendAsync("LoadEmployee",id);
        }
    }
}
