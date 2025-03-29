using Microsoft.EntityFrameworkCore;
using Spectre.Console;
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

        public static void DisplayLogo()
        {
            AnsiConsole.Write(new FigletText(FigletFont.Load("../../../Fonts/Big Money-ne.flf"), "EM System").Centered().Color(Color.Green3_1));
            Console.WriteLine();
            AnsiConsole.Write(new Rule("[bold][green]Welcome to the Employee Management System[/][/]").Centered());
            Console.WriteLine();
        }

        public static string DisplayMainMenu()
        {
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Use Arrow Keys to Select an [green]option[/]:")
                .MoreChoicesText("[grey]Move up and down to reveal more options[/]")
                .AddChoices(new[] { "Add an Employee", "Display All Employees", "Promote an Employee", "Rate an Employee", "Transfer Departement", "Add a Department", "Display All Departments", "Generate Reports", "Exit" })
            );

            return selection;
        }

        public static string DisplayReportMenu()
        {
            Console.Clear();
            DisplayLogo();
            AnsiConsole.MarkupLine("[bold]Choose Report Type[/]\n");
            var selectionDictionary = new Dictionary<string, string>
            {
                { "Display Employees Per Department" , "1" },
                { "Display Top Performers" , "2" },
                { "Display Salary Distribution" , "3" },
                { "Save Employees Per Department Report" , "4" },
                { "Save Salary Distribution Report" , "5" },
                { "Save Top Performers Report" , "6" },
                { "Return To Main Menu" , "7" },
            };

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .AddChoices(selectionDictionary.Keys)
            );

            return selectionDictionary[selection];
        }

        public static void AddEmployee(Company company)
        {
            bool isValid = false;
            using var _context = new EMSContext();
            do
            {
                AnsiConsole.MarkupLine("[bold]Creating a New Employee...[/]");

                AnsiConsole.MarkupLine("\nEnter Employee [green]Name[/]");
                string userName = Console.ReadLine()!;

                AnsiConsole.MarkupLine("Enter Employee [green]Age[/]");
                string age = Console.ReadLine()!;

                AnsiConsole.MarkupLine("Enter Employee [green]Salary[/]");
                string salary = Console.ReadLine()!;

                var jobTitleDictionary = new Dictionary<string, string>
                {
                    { "Fresher", "0" },
                    { "Junior", "1" },
                    { "Mid", "2" },
                    { "Senior", "3" },
                    { "Principal", "4" }
                };
                string jobTitleSelection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Choose Employee [green]Job Title[/]")
                .AddChoices(jobTitleDictionary.Keys)
                );
                AnsiConsole.MarkupLine("Choose Employee [green]Job Title[/]");
                Console.WriteLine(jobTitleSelection);
                string jobTitle = jobTitleDictionary[jobTitleSelection];


                var DepartmentList = company.GetDepartmentList().Select(x => x.GetDepartmentName());
                string departmentSelection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Choose [green]DepartmentName[/]")
                .AddChoices(DepartmentList)
                );
                AnsiConsole.MarkupLine("Choose [green]DepartmentName[/]");
                Console.WriteLine(departmentSelection);
                string departmentName = departmentSelection;

                isValid = Validator.ValidateEmployee(userName, age, salary, jobTitle, departmentName, out Employee employee);
                if (isValid)
                {

                    var department = company.GetDepartment(departmentName);
                    if (department == null)
                    {
                        ConsoleExtension.WriteError("Department not found.");
                        return;
                    }

                    employee.DepartmentId = department.ID;
                    _context.Employees.Add(employee);
                    _context.SaveChanges();

                    ConsoleExtension.WriteSuccess("\nEmployee Added Successfully");
                    Console.WriteLine("Press any key to return to the Main menu...");
                    Console.ReadKey();
                    Console.Clear();
                    DisplayLogo();
                }

            } while (!isValid);

        }

        public static void DisplayEmployees(Company company)
        {
            using var _context = new EMSContext();
            var employees = _context.Employees.Include(x => x.Department);

            AnsiConsole.Status().Start("Displaying All Employees....", ctx =>
            {
                Thread.Sleep(1000);
            });

            AnsiConsole.Write(new Text("Employees List \n ").Centered());

            var table = new Table().Centered().Border(TableBorder.Double);
            table.AddColumn("ID");
            table.AddColumn("Name");
            table.AddColumn("Age");
            table.AddColumn("Salary");
            table.AddColumn("Department");
            table.AddColumn("Employment Date");
            table.AddColumn("Rate");
            table.AddColumns("Eligible");
            table.AddColumn("Job Title");

            AnsiConsole.Live(table)
                .Start(ctx =>
                {
                    foreach (Employee employee in employees)
                    {
                        table.AddRow(
                            employee.GetEmployeeId().ToString(),
                            employee.GetEmployeeName(),
                            employee.GetAge().ToString(),
                            employee.GetSalary().ToString("N0"),
                            employee.Department.GetDepartmentName(),
                            employee.GetEmployementDate().ToString(),
                            employee.GetEmployeeRate().ToString(),
                            employee.IsEligible().ToString(),
                            employee.GetJopTitle().ToString());

                        ctx.Refresh();
                        Thread.Sleep(150);
                    }

                });
        }

        public static void PromoteEmployee()
        {
            AnsiConsole.MarkupLine("[bold]Promoting an Employee...[/]");
            using var context = new EMSContext();

            string empId;
            do
            {
                AnsiConsole.MarkupLine("\nEnter Employee [green]ID[/]");
                empId = Console.ReadLine()!;
            }
            while (!Validator.ValidateEmployeeId(empId));


            Employee employee = context.Employees.FirstOrDefault(x => x.ID == int.Parse(empId))!;
            if (!employee.IsEligible())
            {
                ConsoleExtension.WriteError("\nEmployee is Not Eligible for Promotion");
                return;
            }

            var empBefore = new Table().Centered().Border(TableBorder.Double).Width(55);
            empBefore.AddColumn(employee.GetEmployeeId().ToString());
            empBefore.AddColumn(employee.GetEmployeeName());
            empBefore.AddColumn(new TableColumn(new Markup($"[red]{employee.GetSalary().ToString("N0")}[/]")));
            empBefore.AddColumn(new TableColumn(new Markup($"[red]{employee.GetJopTitle().ToString()}[/]")));
            empBefore.Columns[0].Width = 5;
            empBefore.Columns[1].Width = empBefore.Columns[2].Width = empBefore.Columns[3].Width = 10;
            AnsiConsole.Write(empBefore);

            PerformenceReview.GivePromotion(employee);
            AnsiConsole.Write(new Text("↓").Centered());

            var empAfter = new Table().Centered().Border(TableBorder.Double).Width(55);
            empAfter.AddColumn(employee.GetEmployeeId().ToString());
            empAfter.AddColumn(employee.GetEmployeeName());
            empAfter.AddColumn(new TableColumn(new Markup($"[green]{employee.GetSalary().ToString("N0")}[/]")));
            empAfter.AddColumn(new TableColumn(new Markup($"[green]{employee.GetJopTitle().ToString()}[/]")));
            empAfter.Columns[0].Width = 5;
            empAfter.Columns[1].Width = empAfter.Columns[2].Width = empAfter.Columns[3].Width = 10;
            AnsiConsole.Write(empAfter);

            ConsoleExtension.WriteSuccess("\nEmployee Promoted Successfully");

        }

        public static void AddDepartment(Company company)
        {
            bool isValid = false;
            do
            {
                AnsiConsole.MarkupLine("[bold]Create a new Department[/]\n");

                AnsiConsole.MarkupLine("Enter Department [green]Name[/]");
                string departmentName = Console.ReadLine()!;

                AnsiConsole.MarkupLine("Choose Department [green]Head ID[/] (Optional)");
                string departmentHeadID = Console.ReadLine()!;

                isValid = Validator.ValidateDepartment(departmentName, departmentHeadID, out Employee head);

                if (isValid)
                {
                    Department department = new Department(departmentName, head);
                    company.AddDepartment(department);
                    if (head is null)
                        ConsoleExtension.WriteWarning("\nWarning : Department Has Been Created With no Head");
                    else
                        ConsoleExtension.WriteSuccess("\nDepartment Added Successfully");
                    Console.WriteLine("Press any key to return to the Main menu...");
                    Console.ReadKey();
                    Console.Clear();
                    DisplayLogo();
                }
            }
            while (!isValid);

        }

        public static void DisplayDepartments(Company company)
        {
            using (var _context = new EMSContext())
            {
                AnsiConsole.Status().Start("Retrieving Departments from Database...", ctx =>
                {
                    Thread.Sleep(1000);
                });

                var departments = _context.Departments.Include(d => d.Employees).ToList();

                if (!departments.Any())
                {
                    ConsoleExtension.WriteWarning("No departments found.");
                    return;
                }

                AnsiConsole.Write(new Text("Departments List \n ").Centered());

                var table = new Table().Centered().Border(TableBorder.Double);
                table.AddColumn("Department Name");
                table.AddColumn("Department Head");

                AnsiConsole.Live(table).Start(ctx =>
                {
                    foreach (var department in departments)
                    {
                        string departmentHead = department.GetDepartmentHeadName() ?? "No Head";
                        table.AddRow(department.GetDepartmentName(), departmentHead);
                        ctx.Refresh();
                        Thread.Sleep(200);
                    }
                });
            }
        }

        #region Reports
        public static void GenerateSalaryDistributionReport()
        {
            AnsiConsole.Status().Start("Generating Salary Distribution Report...", ctx =>
            {
                Thread.Sleep(1000);
            });

            AnsiConsole.Write(new Text("Salary Distribution Report \n").Centered());

            var table = new Table().Centered().Border(TableBorder.Double);
            table.AddColumn("ID");
            table.AddColumn("Name");
            table.AddColumn("Age");
            table.AddColumn("Salary");
            table.AddColumn("Department");
            table.AddColumn("Employment Date");
            table.AddColumn("Rate");
            table.AddColumn("Eligible");
            table.AddColumn("Job Title");
            table.AddColumn("Is Terminated?");

            using (var context = new EMSContext())
            {
                var employees = context.Employees
                    .Include(e => e.Department)
                    .ToList();

                AnsiConsole.Live(table).Start(ctx =>
                {
                    foreach (var employee in employees)
                    {
                        table.AddRow(
                            employee.ID.ToString(),
                            employee.GetEmployeeName(),
                            employee.GetAge().ToString(),
                            $"{employee.GetSalary():N0} EGP",
                            employee.Department.GetDepartmentName(),
                            employee.GetEmployementDate().ToString(),
                            employee.GetRate(),
                            employee.IsEligible() ? "Yes" : "No",
                            employee.GetJopTitle().ToString(),
                            employee.IsTerminated() ? "Yes" : "No"
                        );

                        ctx.Refresh();
                        Thread.Sleep(200);
                    }
                });
            }
        }

        public static void GenerateTopPerformersReport()
        {
            company.GenerateTopPerformersReport();
        }

        public static void GenerateEmployeesPerDepartmentReport()
        {
            company.GenerateEmployeesPerDepartmentReport();
        } 
        #endregion

        #region Files
        public static void SaveSalaryDistributionReport()
        {
            company.SaveSalaryDistributionReport();
        }
        public static void SaveTopPerformersReport()
        {
            company.SaveTopPerformersReport();
        }
        public static void SaveEmployeesPerDepartmentReport()
        {
            company.SaveEmployeesPerDepartmentReport();
        }
        #endregion

        internal static void RateEmployee()
        {
            AnsiConsole.MarkupLine("[bold]Rating an Employee...[/]");
            using var context = new EMSContext();
            string empId;
            do
            {
                AnsiConsole.MarkupLine("\nEnter Employee [green]Id[/]");
                empId = Console.ReadLine()!;
            }
            while (!Validator.ValidateEmployeeId(empId));


            var RateDictionary = new Dictionary<string, int>
                {
                    { "Unacceptable", 1 },
                    { "NeedsImprovement", 2 },
                    { "MeetsExpectations", 3 },
                    { "ExceedsExpectations", 4 },
                    { "Outstanding", 5 }
                };

            string rateSelection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose Employee [green]Performance[/]")
            .AddChoices(RateDictionary.Keys)
            );

            AnsiConsole.MarkupLine("Choose Employee [green]Performance[/]");
            Console.WriteLine(rateSelection);

            Employee employee = context.Employees.FirstOrDefault(x => x.ID == int.Parse(empId))!;

            var empBefore = new Table().Centered().Border(TableBorder.Double).Width(55);
            empBefore.AddColumn(employee.GetEmployeeId().ToString());
            empBefore.AddColumn(employee.GetEmployeeName());
            empBefore.AddColumn(new TableColumn(new Markup($"[red]{employee.GetRate().ToString()}[/]")));
            empBefore.Columns[0].Width = 5;
            empBefore.Columns[1].Width = empBefore.Columns[2].Width = 15;
            AnsiConsole.Write(empBefore);

            employee.SetRate((Rate)RateDictionary[rateSelection]);
            context.SaveChanges();
            AnsiConsole.Write(new Text("↓").Centered());

            var empAfter = new Table().Centered().Border(TableBorder.Double).Width(55);
            empAfter.AddColumn(employee.GetEmployeeId().ToString());
            empAfter.AddColumn(employee.GetEmployeeName());
            empAfter.AddColumn(new TableColumn(new Markup($"[green]{employee.GetRate().ToString()}[/]")));
            empAfter.Columns[0].Width = 5;
            empAfter.Columns[1].Width = empBefore.Columns[2].Width = 15;
            AnsiConsole.Write(empAfter);

        }

        internal static void TransferDepartment()
        {
            AnsiConsole.MarkupLine("[bold]Transfer Departement ...[/]");
            using var context = new EMSContext();

            string empId;
            do
            {
                AnsiConsole.MarkupLine("\nEnter Employee [green]Id[/]");
                empId = Console.ReadLine()!;
            }
            while (!Validator.ValidateEmployeeId(empId));


            Employee employee = context.Employees.Include(x => x.Department).FirstOrDefault(x => x.ID == int.Parse(empId))!;
            var DepartmentList = context.Departments.Where(x => x.ID != employee.DepartmentId).Select(x => x.GetDepartmentName());
            string departmentSelection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Choose [green]DepartmentName[/]")
                .AddChoices(DepartmentList)
                );
            AnsiConsole.MarkupLine("Choose [green]DepartmentName[/]");
            Console.WriteLine(departmentSelection);

            var empBefore = new Table().Centered().Border(TableBorder.Double).Width(55);
            empBefore.AddColumn(employee.GetEmployeeId().ToString());
            empBefore.AddColumn(employee.GetEmployeeName());
            empBefore.AddColumn(new TableColumn(new Markup($"[red]{employee.Department.Name}[/]")));
            empBefore.Columns[0].Width = 5;
            empBefore.Columns[1].Width = empBefore.Columns[2].Width = 15;
            AnsiConsole.Write(empBefore);

            Department department = context.Departments.FirstOrDefault(x => x.Name == departmentSelection)!;
            employee.TransferDepartment(department);
            context.SaveChanges();
            AnsiConsole.Write(new Text("↓").Centered());

            var empAfter = new Table().Centered().Border(TableBorder.Double).Width(55);
            empAfter.AddColumn(employee.GetEmployeeId().ToString());
            empAfter.AddColumn(employee.GetEmployeeName());
            empAfter.AddColumn(new TableColumn(new Markup($"[green]{employee.Department.Name}[/]")));
            empAfter.Columns[0].Width = 5;
            empAfter.Columns[1].Width = empBefore.Columns[2].Width = 15;
            AnsiConsole.Write(empAfter);
        }
    }
}
