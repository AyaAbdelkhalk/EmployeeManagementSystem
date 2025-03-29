using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem
{
    class Company : IDisposable
    {
        //private List<Department> DepartmentList = new List<Department>();

        private readonly EMSContext _context;
        public Company()
        {
            _context = new EMSContext();
        }



        public void AddDepartment(Department department)
        {
            //DepartmentList.Add(department);
            _context.Departments.Add(department);
            _context.SaveChanges();

        }

        public void RemoveDepartment(Department department)
        {
            //DepartmentList.Remove(department);
            _context.Departments.Remove(department);
            _context.SaveChanges();
        }

        public List<Department> GetDepartmentList()
        {
            //return DepartmentList;
            return _context.Departments
                .Include(d => d.Employees).ToList();
        }

        public Department? GetDepartment(string departmentName)
        {
            //return DepartmentList.FirstOrDefault(x => x.GetDepartmentName() == departmentName);
            return _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefault(x => x.Name == departmentName);
        }

        public void DisplayCompanyDepartments()
        {

            #region Old
            //foreach (Department department in DepartmentList)
            //{
            //    Console.WriteLine("Department Name: " + department.GetDepartmentName());
            //    if (department.GetDepartmentHead() != null)
            //    {
            //        Console.WriteLine("Department Head: " + department.GetDepartmentHead().GetEmployeeName());
            //    }
            //    else
            //    {
            //        Console.WriteLine("Department Head: No Head");
            //    }
            //    Console.WriteLine("---------------------------------");
            //} 
            #endregion

            var departments = GetDepartmentList();
            foreach (var department in departments)
            {
                Console.WriteLine("Department Name: " + department.GetDepartmentName());
                if (department.GetDepartmentHead() != null)
                {
                    Console.WriteLine("Department Head: " + department.GetDepartmentHead().GetEmployeeName());
                }
                else
                {
                    Console.WriteLine("Department Head: No Head");
                }
                Console.WriteLine("---------------------------------");
            }

        }

        // Report Generation Methods
        public void GenerateEmployeesPerDepartmentReport()
        {
            var departments = GetDepartmentList();

            Console.WriteLine("\n=== Employees Per Department Report ===");
            foreach (var department in departments)
            {
                Console.WriteLine($"\nDepartment: {department.GetDepartmentName()}");
                Console.WriteLine("Employees:");

                var activeEmployees = department.Employees;
                if (activeEmployees.Count == 0)
                {
                    Console.WriteLine("No Active employees");
                    continue;
                }

                foreach (var employee in activeEmployees)
                {
                    Console.WriteLine( $"{employee.GetEmployeeName(),-20}");
                }
            }
        }

        public void GenerateSalaryDistributionReport()
        {
            Console.WriteLine("\n=== Salary Distribution Report ===");
            var departments = GetDepartmentList();

            Console.WriteLine("ID\t Name\t\t\t Age\t Salary\t\t Department\tEmployment Date\t Rate\t\t      Eligibility\t Job Title");

            foreach (var department in departments)
            {
                var activeEmployees = department.Employees;

                foreach (var employee in activeEmployees)
                {
                    employee.DisplayEmployeeInfo();
                }
            }
        }

        public void GenerateTopPerformersReport(int count = 5)
        {
            Console.WriteLine("\n=== Top Performers Report ===");

            //Get all active employees from all departments
            var allEmployees = _context.Employees
                .Include(e => e.Department)
                .Where(e => e.Rate != Rate.Unrated)
                .OrderByDescending(e => e.Rate)
                .Take(count)
                .ToList();


            if (!allEmployees.Any())
            {
                Console.WriteLine("No rated employees found");
                return;
            }
            Console.WriteLine($"{"ID",-5}{"Name",-20}{"Department",-15}{"Rating",-20}{"Salary",-10}");


            foreach (var employee in allEmployees)
            {
                Console.WriteLine($"{employee.ID,-5}{employee.GetEmployeeName(),-20}{employee.Department.GetDepartmentName(),-15}{employee.Rate,-20}{employee.GetSalary(),-10}");
            }

        }
        public void Dispose()
        {
            _context.Dispose();
        }
        //public bool DepartmentExists(string departmentName)
        //{
        //    foreach (var department in DepartmentList)
        //    {
        //        if (string.Equals(department.GetDepartmentName(), departmentName, StringComparison.OrdinalIgnoreCase))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //public bool EmployeeExists(int employeeId)
        //{
        //    foreach (var department in DepartmentList)
        //    {
        //        foreach (var employee in department.Employees)
        //        {
        //            if (employee.GetEmployeeId() == employeeId)
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

    }
}
