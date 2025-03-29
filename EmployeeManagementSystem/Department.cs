using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem
{
    class Department : IDisposable
    {
        private readonly EMSContext _context = new EMSContext();

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
            DepartmentHeadId = employee.ID;
        }
        public Department(string name)
        {
            Name = name;
            DepartmentHead = null;
        }

        public void setDepartmentHead(Employee employee)
        {
            //DepartmentHead = employee;
            DepartmentHeadId = employee.ID;
            _context.SaveChanges();
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
            using (var context = new EMSContext())
            {
                var res= context.Employees
                    .Include(e => e.Department)
                    .Where(e => e.DepartmentId == ID)
                    .ToList();
                List<Employee> employees = new List<Employee>();
                foreach (var employee in res)
                {
                    if (employee.IsTerminated() == false)
                    {
                        employees.Add(employee);
                    }
                }
                return employees;
            }


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

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
