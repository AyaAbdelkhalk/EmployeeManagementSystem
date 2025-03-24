namespace EmployeeManagementSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Company company = new Company();
            Department ITDepartement = new Department("IT");
            company.AddDepartment(ITDepartement);
            Department HRDepartement = new Department("HR");
            company.AddDepartment(HRDepartement);
            Employee employee1 = new Employee("1", "Ahmed", 25, 5000, ITDepartement);
            Employee employee2 = new Employee("2", "Ali", 30, 7000, HRDepartement);
            Employee employee3 = new Employee("3", "Omar", 35, 9000, ITDepartement);
            Employee employee4 = new Employee("4", "Mohamed", 40, 10000, HRDepartement);
            ITDepartement.setDepartmentHead(employee3);
            HRDepartement.setDepartmentHead(employee4);
            List<Employee> employees= ITDepartement.DisplayDepartmentEmployees();
            foreach (Employee employee in employees)
            {
                employee.DisplayEmployeeInfo();
                Console.WriteLine("---------------------------------");
            }
            company.DisplayCompanyDepartments();
            
        }
    }
}
