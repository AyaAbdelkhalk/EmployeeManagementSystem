using EmployeeManagementSystem.Utilities;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
            //var departments = GetDepartmentList();

            //Console.WriteLine("\n=== Employees Per Department Report ===");
            //foreach (var department in departments)
            //{
            //    Console.WriteLine($"\nDepartment: {department.GetDepartmentName()}");
            //    Console.WriteLine("Employees:");

            //    var activeEmployees = department.Employees;
            //    if (activeEmployees.Count == 0)
            //    {
            //        Console.WriteLine("No Active employees");
            //        continue;
            //    }

            //    foreach (var employee in activeEmployees)
            //    {
            //        Console.WriteLine( $"{employee.GetEmployeeName(),-20}");
            //    }
            //}

            AnsiConsole.Write(new Rule("Employees Per Department Report").Centered());
            AnsiConsole.WriteLine();

            foreach (var department in GetDepartmentList())
            {

                var employees = department.Employees;
                if (employees.Count == 0)
                {
                    continue;
                }

                var table = new Table().Centered().Caption(department.Name)
                    .Border(TableBorder.Square)
                    .AddColumn("ID")
                    .AddColumn("Name")
                    .AddColumn("Job Title");

                foreach (var employee in employees)
                {
                    table.AddRow(
                         Math.Abs(employee.ID).ToString(),
                        employee.GetEmployeeName(),
                        employee.GetJopTitle().ToString()
                    );
                }

                AnsiConsole.Write(table);
                AnsiConsole.WriteLine();
            }
        }

        public void GenerateSalaryDistributionReport()
        {
            AnsiConsole.Write(new Rule("Salary Distribution Report").Centered());
            AnsiConsole.WriteLine();

            foreach (var department in GetDepartmentList())
            {

                var employees = department.Employees;
                if (employees.Count == 0)
                {
                    continue;
                }

                var table = new Table().Centered().Caption(department.Name)
                    .Border(TableBorder.Square)
                    .AddColumn("ID")
                    .AddColumn("Name")
                    .AddColumn("Salary")
                    .AddColumn("Rating")
                    .AddColumn("Eligible");

                foreach (var employee in employees)
                {
                    table.AddRow(
                        Math.Abs(employee.ID).ToString(),
                        employee.GetEmployeeName(),
                        $"{employee.GetSalary():C0}",
                        employee.Rate.ToString(),
                        employee.IsEligible() ? "Yes" : "No"
                    );
                }

                AnsiConsole.Write(table);
                AnsiConsole.WriteLine();
            }
        }

        public void GenerateTopPerformersReport(int count = 5)
        {
            //AnsiConsole.Status().Start("Generating Top Performers Report...", ctx =>
            //{
            //    Thread.Sleep(1000);
            //});

            //AnsiConsole.Write(new Text("\nTop Performers Report\n").Centered());

            //var table = new Table().Centered().Border(TableBorder.Double);
            //table.AddColumn("ID");
            //table.AddColumn("Name");
            //table.AddColumn("Age");
            //table.AddColumn("Salary");
            //table.AddColumn("Department");
            //table.AddColumn("Employment Date");
            //table.AddColumn("Rate");
            //table.AddColumn("Eligible");
            //table.AddColumn("Job Title");

            //using (var context = new EMSContext())
            //{
            //    var topEmployees = context.Employees
            //        .Include(e => e.Department)
            //        .Where(e => e.Rate != Rate.Unrated)
            //        .OrderByDescending(e => e.Rate)
            //        .Take(count)
            //        .ToList();

            //    if (!topEmployees.Any())
            //    {
            //        AnsiConsole.Markup("[red]No rated employees found![/]\n");
            //        return;
            //    }

            //    AnsiConsole.Live(table)
            //        .Start(ctx =>
            //        {
            //            foreach (var employee in topEmployees)
            //            {
            //                table.AddRow(
            //                    employee.ID.ToString(),
            //                    employee.GetEmployeeName(),
            //                    employee.GetAge().ToString(),
            //                    employee.GetSalary().ToString("N0") + " EGP",
            //                    employee.Department.GetDepartmentName(),
            //                    employee.GetEmployementDate().ToString(),
            //                    employee.GetEmployeeRate().ToString(),
            //                    employee.IsEligible().ToString(),
            //                    employee.GetJopTitle().ToString()
            //                );

            //                ctx.Refresh();
            //                Thread.Sleep(200);
            //            }
            //        });
            //}
            AnsiConsole.Write(new Rule("Top Performers Report").Centered());
            AnsiConsole.WriteLine();

            var topPerformers = _context.Employees
                .Include(e => e.Department)
                .Where(e => e.Rate != Rate.Unrated)
                .OrderBy(e => e.Rate)
                .Take(count)
                .ToList();

            if (topPerformers.Count == 0)
            {
                AnsiConsole.WriteLine("No rated employees found");
                return;
            }

            var table = new Table().Centered()
                .Border(TableBorder.Square)
                .AddColumn("ID")
                .AddColumn("Name")
                .AddColumn("Department")
                .AddColumn("Rating")
                .AddColumn("Salary");

            foreach (var employee in topPerformers)
            {
                table.AddRow(
                    Math.Abs(employee.ID).ToString(),
                    employee.GetEmployeeName(),
                    employee.Department.GetDepartmentName(),
                    employee.Rate.ToString(),
                    $"{employee.GetSalary():C0}"
                );
            }

            AnsiConsole.Write(table);
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


        #region Reports
        public void SaveSalaryDistributionReport()
        {
            var departments = GetDepartmentList();
            var fileName = "../../../Reports/SalaryDistributionReport.txt";
            // var fileName = "C:\\Users\\Elnour Tech\\Desktop\\SalaryDistributionReport.txt";
            var reportData = departments.Select(department => new
            {
                DepartmentName = department.GetDepartmentName(),
                Employees = department.Employees.Select(employee => new
                {
                    EmployeeID = employee.ID,
                    Name = employee.GetEmployeeName(),
                    Salary = employee.GetSalary(),
                    Department = department.GetDepartmentName(),
                    EmploymentDate = employee.GetEmployementDate().ToString("yyyy-MM-dd")
                }).ToList()
            }).ToList();

            string json = JsonSerializer.Serialize(reportData, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(fileName, json);
            ConsoleExtension.WriteSuccess("SalaryDistributionReport.txt have been saved on disk\n");
        }

        public void SaveTopPerformersReport(int count = 5)
        {
            var fileName = "../../../Reports/TopPerformersReport.txt";

            //var fileName = "C:\\Users\\Elnour Tech\\Desktop\\TopPerformersReport.txt";
            var topPerformers = _context.Employees
                .Include(e => e.Department)
                .Where(e => e.Rate != Rate.Unrated)
                .OrderByDescending(e => e.Rate)
                .Take(count)
                .Select(e => new
                {
                    EmployeeID = e.ID,
                    Name = e.GetEmployeeName(),
                    Department = e.Department.GetDepartmentName(),
                    Rating = e.GetRate(),
                    Salary = e.GetSalary()
                }).ToList();

            string json = JsonSerializer.Serialize(topPerformers, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(fileName, json);
            ConsoleExtension.WriteSuccess("TopPerformersReport.txt have been saved on disk\n");
        }

        public void SaveEmployeesPerDepartmentReport()
        {
            var fileName = "../../../Reports/EmployeesPerDepartmentReport.txt";
            //var fileName = "C:\\Users\\Elnour Tech\\Desktop\\EmployeesPerDepartmentReport.txt";
            var departments = GetDepartmentList();

            var reportData = departments.Select(department => new
            {
                DepartmentName = department.GetDepartmentName(),
                Employees = department.Employees.Select(employee => new
                {
                    EmployeeID = employee.ID,
                    Name = employee.GetEmployeeName(),
                    Age = employee.GetAge(),
                    Salary = employee.GetSalary(),
                    JobTitle = employee.GetJopTitle().ToString()
                }).ToList()
            }).ToList();

            string json = JsonSerializer.Serialize(reportData, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(fileName, json);
            ConsoleExtension.WriteSuccess("EmployeesPerDepartmentReport.txt have been saved on disk\n");
        }


        #endregion




    }
}
