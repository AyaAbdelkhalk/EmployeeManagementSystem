using EmployeeManagementSystem.Utilities;
using Spectre.Console;

namespace EmployeeManagementSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Company company = new Company();
            Validator.Company = company;
            CommandManager.company = company;

            Department ITDepartement = new Department("IT");
            Department HRDepartement = new Department("HR");
            Department FinanceDepartement = new Department("Finance");
            Department MarketingDepartement = new Department("Marketing");
            company.AddDepartment(ITDepartement);
            company.AddDepartment(HRDepartement);
            company.AddDepartment(FinanceDepartement);
            company.AddDepartment(MarketingDepartement);

            Employee employee1 = new Employee("Ahmed Fahmy", 25, 5000, JopTitles.Mid, ITDepartement);
            Employee employee2 = new Employee("Ali Saad", 30, 7000, JopTitles.Mid, HRDepartement);
            Employee employee3 = new Employee("Omar Abdelbaset", 35, 9000, JopTitles.Junior, ITDepartement);
            Employee employee4 = new Employee("Momen Ahmed", 40, 10000, JopTitles.Senior, HRDepartement);
            Employee employee5 = new Employee("Ashraf Khaled", 40, 10000, JopTitles.Principal, HRDepartement);
            Employee employee6 = new Employee("Taha Ragab", 40, 10000, JopTitles.Fresher, HRDepartement);
            Employee employee7 = new Employee("Fouad Magdy", 40, 10000, JopTitles.Senior, HRDepartement);
            ITDepartement.setDepartmentHead(employee3);
            HRDepartement.setDepartmentHead(employee4);


            employee1.SetRate(Rate.MeetsExpectations);
            employee2.SetRate(Rate.ExceedsExpectations);
            employee3.SetRate(Rate.Unacceptable);
            employee4.SetRate(Rate.Outstanding);


            CommandManager.DisplayLogo();

            while (true)
            {
                var selection = CommandManager.DisplayMainMenu();

                switch (selection)
                {
                    case "Add an Employee":
                        CommandManager.AddEmployee(company);
                        break;
                    case "Display All Employees":
                        CommandManager.DisplayEmployees(company);
                        break;
                    case "Promote an Employee":
                        CommandManager.PromoteEmployee();
                        break;
                    case "Add a Department":
                        CommandManager.AddDepartment(company);
                        break;
                    case "Display All Departments":
                        CommandManager.DisplayDepartments(company);
                        break;
                    case "Generate Reports":
                        
                        string reportType = CommandManager.DisplayReportMenu();
                        switch (reportType)
                        {
                            case "1":
                                CommandManager.GenerateEmployeesPerDepartmentReport();
                                break;
                            case "2":
                                CommandManager.GenerateTopPerformersReport();
                                break;
                            case "3":
                                CommandManager.GenerateSalaryDistributionReport();
                                break;
                            case "4":
                                Console.Clear();
                                CommandManager.DisplayLogo();
                                break;
                            default:
                                Console.WriteLine("Invalid Command");
                                break;
                        }
                        break;
                    case "Exit":
                        return;
                    default:
                        Console.WriteLine("Invalid Command");
                        break;
                }
                Console.WriteLine();
            }

        }
    }
}
