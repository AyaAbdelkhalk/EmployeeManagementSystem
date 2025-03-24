using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem
{
    class Employee
    {
        private string ID;
        private string Name;
        private int Age;
        private decimal Salary;
        private Department Department;
        private DateOnly EmploymentDate; 
        private Rate Rate;
        private bool Terminate;
        private JopTitles JopTitle;

        public Employee(string id, string name, int age, decimal salary, Department department)
        {
            ID = id;
            Name = name;
            Age = age;
            Salary = salary;
            Department = department; 
            Department.AddEmployee(this);
            Terminate = false;
            Rate = 0;
            JopTitle = JopTitles.Junior;
            EmploymentDate = DateOnly.FromDateTime(DateTime.Now);
        }
        public void UpdateJopTitle(JopTitles jopTitle)
        {
            JopTitle = jopTitle;
        }
        public void SetRate(Rate rate)
        {
            Rate = rate;
        }
        public void TransferDepartment(Department department)
        {
            if (department != null && Terminate == false)
            {
                Department.RemoveEmployee(this);
                Department = department;
                Department.AddEmployee(this);
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
            foreach (Employee employee in Department.Employee)
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
            foreach (Employee employee in Department.Employee)
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
            Console.WriteLine("ID: " + ID);
            Console.WriteLine("Name: " + Name);
            Console.WriteLine("Age: " + Age);
            Console.WriteLine("Salary: " + Salary);
            Console.WriteLine("Department: " + Department.GetDepartmentName());
            Console.WriteLine("Employment Date: " + EmploymentDate);
            Console.WriteLine("Rate: " + Rate);
            Console.WriteLine("Jop Title: " + JopTitle);
        }
        public string GetEmployeeName()
        {
            return Name;
        }

    }
}
