using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem
{
    class Company
    {
        private List<Department> DepartmentList = new List<Department>();
        public void AddDepartment(Department department)
        {
            DepartmentList.Add(department);
        }
        public void RemoveDepartment(Department department)
        {
            DepartmentList.Remove(department);
        }
        public List<Department> GetDepartmentList()
        {
            return DepartmentList;
        }
        public void DisplayCompanyDepartments()
        {
            foreach (Department department in DepartmentList)
            {
                Console.WriteLine("Department Name: " + department.GetDepartmentName());
                if (department.GetDepartmentHead() != null)
                {
                    Console.WriteLine("Department Head: " + department.GetDepartmentHead().GetEmployeeName());
                }
                else
                {
                    Console.WriteLine("Department Head: No Head");
                }
                Console.WriteLine("---------------------------------");
            }
        }

    }
}
