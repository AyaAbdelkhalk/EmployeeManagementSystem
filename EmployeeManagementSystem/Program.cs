using EmployeeManagementSystem.Utilities;

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
            company.AddDepartment(ITDepartement);
            Department HRDepartement = new Department("HR");
            company.AddDepartment(HRDepartement);
            Employee employee1 = new Employee("Ahmed", 25, 5000, JopTitles.Mid, ITDepartement);


            Employee employee2 = new Employee("Ali", 30, 7000, JopTitles.Mid, HRDepartement);
            Employee employee3 = new Employee("Omar", 35, 9000, JopTitles.Junior, ITDepartement);
            Employee employee4 = new Employee("Mohamed Anwar", 40, 10000, JopTitles.Senior, HRDepartement);
            //employee4.SetRate(Rate.MeetsExpectations);////////////////for Check
            //employee4.SetJopTitle(JopTitles.Junior);
            //employee4.DisplayEmployeeInfo();
            //PerformenceReview.GivePromotion(employee4);
            //employee4.DisplayEmployeeInfo();
            ITDepartement.setDepartmentHead(employee3);
            HRDepartement.setDepartmentHead(employee4);
            List<Employee> employees = ITDepartement.DisplayDepartmentEmployees();


            while (true)
            {
                CommandManager.DisplayMainMenu();
                char keyPressed = Console.ReadKey(intercept: true).KeyChar;

                switch (keyPressed)
                {
                    case '1':
                        CommandManager.AddEmployee(company);
                        break;
                    case '2':
                        CommandManager.DisplayEmployees(company);
                        break;
                    case '3':
                        CommandManager.PromoteEmployee();
                        break;
                    case '4':
                        CommandManager.AddDepartment(company);
                        break;
                    case '5':
                        CommandManager.DisplayDepartments(company);
                        break;
                    case '6':
                        CommandManager.DisplayReportMenu();
                        char reportType = Console.ReadKey(intercept: true).KeyChar;
                        switch (reportType)
                        {
                            case '1':
                                CommandManager.GenerateEmployeesPerDepartmentReport();
                                break;
                            case '2':
                                CommandManager.GenerateTopPerformersReport();
                                break;
                            case '3':
                                CommandManager.GenerateSalaryDistributionReport();
                                break;
                            case '4':
                                Console.Clear();
                                break;
                            default:
                                Console.WriteLine("Invalid Command");
                                break;
                        }
                        break;
                    case '7':
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
