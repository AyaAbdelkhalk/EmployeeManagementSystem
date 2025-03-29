using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EmployeeManagementSystem.Utilities
{

    internal static class CommandManager
    {
        public static Company company = new Company();
        private readonly static EMSContext _context = new EMSContext();

        public static void DisplayMainMenu()
        {
            Console.WriteLine("Employee Management System (EMS) \n");
            Console.WriteLine("1- Add an Employee");
            Console.WriteLine("2- Display All Employees");
            Console.WriteLine("3- Promote an Employee");
            Console.WriteLine("4- Add a Department");
            Console.WriteLine("5- Display All Departments");
            Console.WriteLine("6- Generate Reports");
            Console.WriteLine("7- Exit \n");
        }

        public static void DisplayReportMenu()
        {
            Console.Clear();
            Console.WriteLine("Choose Report Type \n");
            Console.WriteLine("1- Display Employees Per Department");
            Console.WriteLine("2- Top Performers");
            Console.WriteLine("3- Salary Distribution");
            Console.WriteLine("4- Return To Main Menu\n");
        }

        public static void AddEmployee(Company company)
        {
            bool isValid = false;
            do
            {
                Console.WriteLine("Creating a New Employee... \n");
                Console.WriteLine("Choose Employee Name");
                string userName = Console.ReadLine()!;
                Console.WriteLine("Choose Age");
                string age = Console.ReadLine()!;
                Console.WriteLine("Choose Salary");
                string salary = Console.ReadLine()!;
                Console.WriteLine("Choose Job Title (Fresher = 0, Junior = 1, Mid = 2, Senior = 3, Principal = 4 )");
                string jobTitle = Console.ReadLine()!;
                Console.WriteLine("Choose Departement Name");
                string departmentName = Console.ReadLine()!;

                isValid = Validator.ValidateEmployee(userName, age, salary, jobTitle ,departmentName, out Employee employee);
                if (isValid)
                {
                    using (var _context = new EMSContext())  
                    {
                        var department = _context.Departments.FirstOrDefault(d => d.Name == departmentName);
                        if (department == null)
                        {
                            ConsoleExtension.WriteError("Department not found.");
                            return;
                        }

                        employee.DepartmentId = department.ID;
                        employee.Department = department;

                        _context.Employees.Add(employee);
                        _context.SaveChanges();
                    }
                    ConsoleExtension.WriteSuccess("Employee Added Successfully, Returning to the Main Menu ....");
                    Thread.Sleep(1500);
                    Console.Clear();
                }

            } while (!isValid);

        }

        public static void DisplayEmployees(Company company)
        {
            Console.WriteLine("Displaying All Employees....\n\n");
            Console.WriteLine($"ID\t {"Name".PadRight(15)}\t Age\t Salary \t Department \t EmploymentDate\t Rate\t\t JobTitle");
            Console.WriteLine(new string('─', 115));
            foreach (Department department in company.GetDepartmentList())
            {
                foreach (Employee employee in department.DisplayDepartmentEmployees())
                {
                    employee.DisplayEmployeeInfo();
                    Console.WriteLine(new string('─', 115));
                }
            }
        }

        public static void PromoteEmployee()
        {
            Console.WriteLine("Not Implemented Yet... !");
        }

        public static void AddDepartment(Company company)
        {
            bool isValid = false;
            do
            {
                Console.WriteLine("Choose Department Name");
                string departmentName = Console.ReadLine()!;
                Console.WriteLine("Choose Department Head ID");
                string departmentHeadID = Console.ReadLine()!;
                isValid = Validator.ValidateDepartment(departmentName, departmentHeadID, out Employee head);

                if (isValid)
                {
                    Department department =  new Department(departmentName , head);
                    company.AddDepartment(department);
                    if(head is null)
                        ConsoleExtension.WriteWarning("Warning : Department Has Been Created With no Head");
                    else
                        ConsoleExtension.WriteSuccess("Department Added Successfully, Returning to the Main Menu ....");
                    Thread.Sleep(1800);
                    Console.Clear();
                }
            }
            while (!isValid);

        }

        public static void DisplayDepartments(Company company)
        {
            Console.WriteLine("Displaying All Departments....\n\n");
            Console.WriteLine($"{"Name".PadRight(10)}\t Department Head");
            Console.WriteLine(new string('─', 35));
            foreach (Department department in company.GetDepartmentList())
            {
                Console.WriteLine($"{department.GetDepartmentName()}\t\t {department.GetDepartmentHeadName()}");
                Console.WriteLine(new string('─', 35));
            }
        }

        public static void GenerateSalaryDistributionReport()
        {
            company.GenerateSalaryDistributionReport();
        }

        public static void GenerateTopPerformersReport()
        {
            company.GenerateTopPerformersReport();
        }

        public static void GenerateEmployeesPerDepartmentReport()
        {
            company.GenerateEmployeesPerDepartmentReport();
        }

    }
}
