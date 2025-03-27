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
                Console.WriteLine($"Head: {department.GetDepartmentHead()?.GetEmployeeName() ?? "No Head"}");
                Console.WriteLine("Employees:");

                var activeEmployees = department.DisplayDepartmentEmployees();
                if (activeEmployees.Count == 0)
                {
                    Console.WriteLine("No Active employees");
                    continue;
                }

                Console.WriteLine("ID   Name               Age  Salary    Title          Rate");

                foreach (var employee in activeEmployees)
                {
                    Console.WriteLine(
                        $"{employee.GetEmployeeId(),-5}" +
                        $"{employee.GetEmployeeName(),-20}" +
                        $"{employee.Age,-5}" +
                        $"{employee.Salary,-10}" +
                        $"{employee.JopTitle,-15}" +
                        $"{employee.Rate}"
                    );
                }
            }
        }

        public void GenerateTopPerformersReport(int count = 5)
        {
            Console.WriteLine("\n=== Top Performers Report ===");

            // Get all active employees from all departments
            var allEmployees = DepartmentList
                .SelectMany(d => d.DisplayDepartmentEmployees())
                .Where(e => e.Rate != Rate.Unrated)
                .OrderByDescending(e => e.Rate)
                .Take(count)
                .ToList();

            if (allEmployees.Count() == 0)
            {
                Console.WriteLine("No rated employees found");
                return;
            }

            Console.WriteLine($"{"ID",-5}{"Name",-20}{"Department",-15}{"Rating",-20}{"Salary",-10}");

            foreach (var employee in allEmployees)
            {
                Console.WriteLine(
                    $"{employee.GetEmployeeId(),-5} " +
                    $"{employee.GetEmployeeName(),-20} " +
                    $"{employee.Department.GetDepartmentName(),-15} " +
                    $"{employee.Rate.ToString(),-20} " +
                    $"{employee.Salary.ToString(),-10}"
                );
            }
        }

        public bool DepartmentExists(string departmentName)
        {
            foreach (var department in DepartmentList)
            {
                if (string.Equals(department.GetDepartmentName(), departmentName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public bool EmployeeExists(int employeeId)
        {
            foreach (var department in DepartmentList)
            {
                foreach (var employee in department.Employees)
                {
                    if (employee.GetEmployeeId() == employeeId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
