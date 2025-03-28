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
        private Employee? DepartmentHead;
        public List<Employee> Employees = new List<Employee>();

        public Department(string name, Employee employee)
        {
            Name = name;
            DepartmentHead = employee;
        }
        public Department(string name)
        {
            Name = name;
            DepartmentHead = null;
        }

        public void setDepartmentHead(Employee employee)
        {
            DepartmentHead = employee;
        }


        public void AddEmployeeToDepartment(Employee employee)
        {
            Employees.Add(employee);
        }

        public void RemoveEmployeeFromDepartment(Employee employee)
        {
            Employees.Remove(employee);
        }

        public List<Employee> DisplayDepartmentEmployees()
        {
            List<Employee> employees = new List<Employee>();
            foreach (Employee employee in Employees)
            {
                if (employee.IsTerminated() == false)
                {
                    employees.Add(employee);
                }
            }
            return employees;
        }
        public string GetDepartmentName()
        {
            return Name;
        }

        public Employee GetDepartmentHead()
        {
            return DepartmentHead;
        }

        public string GetDepartmentHeadName()
        {
            return DepartmentHead is null ? "No Head" : DepartmentHead.GetEmployeeName();
        }
    }
}
