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
        private DateOnly EmploymentDate; // what type we need -> DateOnly is fine we don't care about employement time
        private double Rate;
        private bool Terminate;
        private string JopTitle;

        public Employee(string id, string name, int age, decimal salary, Department department)
        {
            ID = id;
            Name = name;
            Age = age;
            Salary = salary;
            Department = department; // we need to add the employee to the department list
            Terminate = false;
            // In known companies, the employee starts with a rate of 0 and then after the first performance review, the rate will be updated
            // we can use an enum for the rate with the following values
            // 5– Outstanding, 4– Exceeds Expectations, 3- Meets Expectations, 2- Needs Improvement, 1- Unacceptable , 0- Unrated
            Rate = 0;//اول ما هيبدل خيبدا ب صفر؟؟؟
            JopTitle = "joiner"; // here we could use enum for the job title too
            EmploymentDate = new DateOnly();
        }
        public void TransferDepartment(Department department)
        {
            if (department != null && Terminate == false)
            {
                // we should remove the employee from the old department
                Department = department;
                // we should add the employee to the new department
            }
        }
    }
}
