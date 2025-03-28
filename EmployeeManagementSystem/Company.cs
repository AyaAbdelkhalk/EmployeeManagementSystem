using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem
{
    class Company
    {
        private List<Department> DepartmentList = new List<Department>();

        public void AddDepartment(Department department)
        {
            DepartmentList.Add(department);
        }

        public void RemoveDepartment(Department department)
        {
            DepartmentList.Remove(department);
        }

        public List<Department> GetDepartmentList()
        {
            return DepartmentList;
        }

        public Department? GetDepartment(string departmentName)
        {
            return DepartmentList.FirstOrDefault(x => x.GetDepartmentName() == departmentName);
        }

        public void DisplayCompanyDepartments()
        {
            foreach (Department department in DepartmentList)
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
            Console.WriteLine("\n=== Employees Per Department Report ===");
            foreach (var department in DepartmentList)
            {
                Console.WriteLine($"\nDepartment: {department.GetDepartmentName()}");
                Console.WriteLine("Employees:");

                var activeEmployees = department.DisplayDepartmentEmployees();
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
            Console.WriteLine("\n=== Salary Distrubution Report ===");
            Console.WriteLine("ID   Name               Age  Salary    Title       Eligebel   Rate");
            foreach (var department in DepartmentList)
            {
                var activeEmployees = department.DisplayDepartmentEmployees();

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
            var allEmployees = DepartmentList
                .SelectMany(d => d.DisplayDepartmentEmployees())
                .Where(e => e.GetEmployeeRate() != Rate.Unrated)
                .OrderByDescending(e => e.GetEmployeeRate())
                .Take(count)
                .ToList();

            if (allEmployees.Count() == 0)
            {
                Console.WriteLine("No rated employees found");
                return;
            }
            // Change This
            Console.WriteLine($"{"ID",-5}{"Name",-20}{"Department",-15}{"Rating",-20}{"Salary",-10}");

            foreach (var employee in allEmployees)
            {
                employee.DisplayEmployeeInfo();
            }
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
