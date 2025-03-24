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
        private DateOnly EmploymentDate; // what type we need
        private double Rate;
        private bool Terminate;
        private string JopTitle;

        public Employee(string id, string name, int age, decimal salary, Department department)
        {
            ID = id;
            Name = name;
            Age = age;
            Salary = salary;
            Department = department;
            Terminate = false;
            Rate = 0;//اول ما هيبدل خيبدا ب صفر؟؟؟
            JopTitle = "joiner";
            EmploymentDate = new DateOnly();

        }
        public void TransferDepartment(Department department)
        {
            if (department != null && Terminate == false)
            {
                Department = department;
            }

        }
    }
}
