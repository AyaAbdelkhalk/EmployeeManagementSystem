using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem
{
    class Department
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }

        [ForeignKey("DepartmentHead")]
        public int? DepartmentHeadId { get; set; }
        public virtual Employee? DepartmentHead { get; set; }
        public List<Employee> Employees { get; set; } = new List<Employee>();

        public Department()
        {
        }
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
            DepartmentHeadId = employee.ID;
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
            //List<Employee> employees = new List<Employee>();
            //foreach (Employee employee in Employees)
            //{
            //    if (employee.IsTerminated() == false)
            //    {
            //        employees.Add(employee);
            //    }
            //}
            //return employees;
            return Employees
                .FindAll(e => e.IsTerminated() == false)
                .ToList();
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
