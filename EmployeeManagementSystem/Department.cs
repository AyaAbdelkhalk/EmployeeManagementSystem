using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem
{
    class Department
    {
        private string Name;
        private string DepartmentHead;  // i think it's better to use Employee type here
        public List<Employee> Employee = new List<Employee>();

        public Department(string name, string departmentHead)
        {
            Name = name;
            DepartmentHead = departmentHead;
        }
        public void AddEmployee(Employee employee)
        {
            Employee.Add(employee);
        }

        public void RemoveEmployee(Employee employee)
        {
            Employee.Remove(employee);
        }
    }
}
