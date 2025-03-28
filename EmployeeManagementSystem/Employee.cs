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

        public Employee(string name, int age, decimal salary, JopTitles jopTitles, Department department)
        {
            ID = EmployeeCounter++;
            Name = name;
            Age = age;
            Salary = salary;
            Department = department;
            Department.AddEmployeeToDepartment(this);
            Terminate = false;
            Rate = Rate.Unrated;
            JopTitle = jopTitles;
            EmploymentDate = DateOnly.FromDateTime(DateTime.Now);
        }

        public void UpdateJopTitle(JopTitles jopTitle)
        {
            JopTitle = jopTitle;
        }

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
            Console.WriteLine($"{ID}\t {Name.PadRight(15)}\t {Age}\t {Salary} EGP \t {Department.GetDepartmentName()}\t\t {EmploymentDate}\t {Rate}\t {(IsEligible() ? "Eligible" : "Not Eligible")} \t {JopTitle.ToString().PadRight(12)}");
        }
        //public void DisplayEmployeeInfo()
        //{
        //    Console.WriteLine($"{ID}\t {Name.PadRight(15)}\t {Age}\t {Salary} EGP \t {Department.GetDepartmentName()}\t\t {EmploymentDate}\t {Rate}\t {(Rate > Rate.MeetsExpectations ? "Eligible" : "Not Eligible")} \t {JopTitle.ToString().PadRight(12)}");
        //}



        public string GetEmployeeName()
        {
            return Name;
        }

        public int GetEmployeeId()
        {
            return ID;
        }
        public Rate GetEmployeeRate()
        {
            return Rate;
        }
        
        public decimal GetSalary()
        {
            return Salary;
        }

        public DateOnly GetEmployementDate()
        {
            return EmploymentDate;
        }
        public void SetSalary(decimal salary)
        {
            Salary = salary;
        }
        public JopTitles GetJopTitle()
        {
            return JopTitle ;
        }
        public int GetAge()
        {
            return Age;
        }
        public void SetJopTitle(JopTitles jopTitle)
        {
            JopTitle=jopTitle;
        }
        public bool IsEligible()
        {
            //if(Rate!=Rate.Unrated||Rate!=Rate.Unacceptable|| Rate != Rate.NeedsImprovement)
            if(Rate>Rate.NeedsImprovement)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
