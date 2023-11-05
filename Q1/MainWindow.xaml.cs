using Microsoft.EntityFrameworkCore;
using Q1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Q1
{
    public partial class MainWindow : Window
    {
        PRN_Spr23_B1Context _context;
        public MainWindow()
        {
            _context = new PRN_Spr23_B1Context();
            InitializeComponent();
            start();
        }

        public void start()
        {
            loadCbo();
            loadDataGrid();
        }

        public void loadDataGrid()
        {
            dgData.ItemsSource = _context.Employees.Include(p => p.Department).ToList();
        }

        public void loadCbo()
        {
            cboDepartment.ItemsSource = _context.Departments.ToList();
            cboTitleOfCourtesty.ItemsSource = _context.Employees.Select(p => p.TitleOfCourtesy).Distinct().ToList();    
        }
        public Employee GetEmployee()
        {
            Employee employee = null;
            try
            {
                employee = new Employee
                {
                    EmployeeId = String.IsNullOrEmpty(txtEmployeeId.Text) ? 0 : Int32.Parse(txtEmployeeId.Text),
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    DepartmentId = (int?)cboDepartment.SelectedValue,
                    Title = txtTitle.Text,
                    TitleOfCourtesy = cboTitleOfCourtesty.Text,
                    BirthDate = DateTime.Parse(dtpBirthDate.Text)

                };
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }

            return employee;
        }
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            txtEmployeeId.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtTitle.Clear();
            cboDepartment.Text = String.Empty;
            cboTitleOfCourtesty.Text = String.Empty;
            dtpBirthDate.Text = DateTime.Now.ToString();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Employee employee = GetEmployee();
            employee.EmployeeId = 0;
            _context.Employees.Add(employee);
            _context.SaveChanges();
            start();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Employee employee = GetEmployee();
            Employee eUpdate = _context.Employees.SingleOrDefault(p => p.EmployeeId == employee.EmployeeId);
            eUpdate.FirstName = employee.FirstName;
            eUpdate.LastName = employee.LastName;
            eUpdate.Title = employee.Title;
            eUpdate.BirthDate = employee.BirthDate;
            eUpdate.DepartmentId = employee.DepartmentId;
            eUpdate.TitleOfCourtesy = employee.TitleOfCourtesy;
            _context.Employees.Update(eUpdate);
            _context.SaveChanges();
            start();
        }

        private void dgData_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Employee item = (sender as DataGrid).SelectedItem as Employee;
            if (item != null)
            {
                txtEmployeeId.Text = item.EmployeeId.ToString();
                txtFirstName.Text = item.FirstName.ToString();
                txtLastName.Text = item.LastName.ToString();
                txtTitle.Text = item.Title.ToString();
                cboDepartment.SelectedItem = item.Department;
                cboTitleOfCourtesty.SelectedItem = item.TitleOfCourtesy;
                dtpBirthDate.Text = item.BirthDate.ToString();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Employee employee = GetEmployee();
            var em = _context.Employees.Include(p => p.Orders).ThenInclude(o => o.OrderDetails)
                .SingleOrDefault(p => p.EmployeeId == employee.EmployeeId);
            foreach (var order in em.Orders)
            {
                _context.OrderDetails.RemoveRange(order.OrderDetails);
            }
            _context.Orders.RemoveRange(em.Orders);
            _context.Employees.Remove(em);
            _context.SaveChanges();
            loadDataGrid();
        }
    }
}
