namespace EmployeeManagementSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Department ITDepartement = new Department("IT", "Mohamed");
            Department HRDepartement = new Department("HR", "Ali");
            Employee employee1 = new Employee("1", "Ahmed", 25, 5000, ITDepartement);

        }
    }
}
