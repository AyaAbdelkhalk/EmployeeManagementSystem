using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem
{
    class Employee
    {
        private static int EmployeeCounter = 1;
        private int ID;
        private string Name;
        private int Age;
        private decimal Salary;
        private Department Department;
        private DateOnly EmploymentDate; 
        private Rate Rate;
        private bool Terminate;
        private JopTitles JopTitle;

        public Employee(string name, int age, decimal salary, Department department)
        {
            ID = EmployeeCounter++; 
            Name = name;
            Age = age;
            Salary = salary;
            Department = department; 
            Department.AddEmployeeToDepartment(this);
            Terminate = false;
            Rate = Rate.Unrated;
            JopTitle = JopTitles.Junior;
            EmploymentDate = DateOnly.FromDateTime(DateTime.Now);
        }
        //public void UpdateJopTitle(JopTitles jopTitle)
        //{
        //    JopTitle = jopTitle;
        //}
        public void SetRate(Rate rate)
        {
            if (rate == Rate.Unrated)
            {
                Console.WriteLine("Invalid Rate");
            }
            else
            {
                Rate = rate;
            }
        }
        public void TransferDepartment(Department department)
        {
            if (department != null && Terminate == false)
            {
                Department.RemoveEmployeeFromDepartment(this);
                Department = department;
                Department.AddEmployeeToDepartment(this);
            }
        }
        public void TerminateEmployee()
        {
            if (Terminate == false)
            {
                Terminate = true;
            }
        }
        public bool IsTerminated()
        {
            return Terminate;
        }
        public List<Employee> DisplayCurrentEmployees()
        {
            List<Employee> employees = new List<Employee>();
            foreach (Employee employee in Department.Employees)
            {
                if (employee.IsTerminated() == false)
                {
                    employees.Add(employee);
                }
            }
            return employees;
        }
        public List<Employee> DisplayPastEmployees()
        {
            List<Employee> employees = new List<Employee>();
            foreach (Employee employee in Department.Employees)
            {
                if (employee.IsTerminated() == true)
                {
                    employees.Add(employee);
                }
            }
            return employees;
        }

        public void DisplayEmployeeInfo()
        {
            Console.WriteLine($"{ID}\t {Name.PadRight(15)}\t {Age}\t {Salary} EGP \t {Department.GetDepartmentName()}\t\t {EmploymentDate}\t {Rate}\t {JopTitle.ToString().PadRight(12)}");
        }
        public string GetEmployeeName()
        {
            return Name;
        }

        public int GetEmployeeId()
        {
            return ID;
        }
        public void GivePromotion(JopTitles jopTitle)
        {
            switch (jopTitle)
            {
                    case JopTitles.Junior:
                        Salary += Salary * .15m;
                        JopTitle = jopTitle;
                        break;
                    case JopTitles.Senior:
                        Salary += Salary * .18m;
                        JopTitle = jopTitle;
                        break;
                    case JopTitles.TeamLeader:
                        Salary += Salary * .20m;
                        JopTitle = jopTitle;
                        break;
                    case JopTitles.Manager:
                        Salary += Salary * .25m;
                        JopTitle = jopTitle;
                        break;
                    case JopTitles.CEO:
                        Salary += Salary * .30m;
                        JopTitle = jopTitle;
                        break;

            }
        }

    }
}
