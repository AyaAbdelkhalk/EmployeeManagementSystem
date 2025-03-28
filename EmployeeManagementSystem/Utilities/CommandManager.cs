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
                .AddChoices(new[] { "Add an Employee", "Display All Employees", "Promote an Employee", "Add a Department", "Display All Departments", "Generate Reports", "Exit" })
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
                { "Employees Per Department" , "1" },
                { "Top Performers" , "2" },
                { "Salary Distribution" , "3" },
                { "Return To Main Menu" , "4" }
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
            table.AddColumns("Eligibility");
            table.AddColumn("Job Title");


            AnsiConsole.Live(table)
                .Start(ctx =>
                {
                    foreach (Department department in company.GetDepartmentList())
                    {
                        foreach (Employee employee in department.DisplayDepartmentEmployees())
                        {
                            table.AddRow(
                                employee.GetEmployeeId().ToString(),
                                employee.GetEmployeeName(),
                                "25",
                                "10000",
                                department.GetDepartmentName(),
                                employee.GetSalary().ToString(),
                                "5",
                                "Eligible",
                                employee.GetJopTitle().ToString());

                            ctx.Refresh();
                            Thread.Sleep(200);

                        }
                    }

                });

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
            AnsiConsole.Status().Start("Displaying All Departments....", ctx =>
            {
                Thread.Sleep(1000);
            });

            AnsiConsole.Write(new Text("Departments List \n ").Centered());

            var table = new Table().Centered().Border(TableBorder.Double);
            table.AddColumn("Department Name");
            table.AddColumn("Department Head");

            AnsiConsole.Live(table).Start(ctx =>
            {
                foreach (Department department in company.GetDepartmentList())
                {
                    table.AddRow(department.GetDepartmentName(), department.GetDepartmentHeadName());
                    ctx.Refresh();
                    Thread.Sleep(200);
                }
            });
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
