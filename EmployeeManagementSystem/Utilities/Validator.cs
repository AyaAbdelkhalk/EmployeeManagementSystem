using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Utilities
{
    internal static class Validator
    {
        public static Company Company = new Company();

        public static bool ValidateDepartment(string name, string HeadId, out Employee employee)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrEmpty(name))
                errors.Add("Department Name is Required");

            if (string.IsNullOrEmpty(HeadId))
                errors.Add("Department Head ID is Required");

            if(!int.TryParse(HeadId, out int Id) && !string.IsNullOrEmpty(HeadId))
            {
                errors.Add("Department Head ID Must be a Number");
            }
            else
            {
                Employee? employeeExists = Company
                  .GetDepartmentList()
                  .Select(x => x.DisplayDepartmentEmployees())
                  .SelectMany(x => x)
                  .FirstOrDefault(x => x.GetEmployeeId() == Id);

                if (employeeExists is null && !string.IsNullOrEmpty(HeadId))
                {
                    errors.Add("Employee with this ID does not exist");
                }
            }

            if (errors.Count > 0)
            {
                ConsoleExtension.WriteError(errors);
                employee = null!;
                return false;
            }
            else
            {
                employee = Company
                  .GetDepartmentList()
                  .Select(x => x.DisplayDepartmentEmployees())
                  .SelectMany(x => x)
                  .FirstOrDefault(x => x.GetEmployeeId() == Id)!;
                return true;
            }

        }

        public static bool ValidateEmployee(string name, string age, string salary, string departmentName , out Employee employee)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrEmpty(name))
                errors.Add("Employee Name is Required");
            if (string.IsNullOrEmpty(age))
                errors.Add("Employee Age is Required");
            if (string.IsNullOrEmpty(salary))
                errors.Add("Employee Salary is Required");
            if (string.IsNullOrEmpty(departmentName))
                errors.Add("Department Name is Required");

            if(!int.TryParse(age, out int ageInt) && !string.IsNullOrEmpty(age))
                errors.Add("Employee Age must be a number");
            else
            {
                if ((ageInt < 18 || ageInt > 60) && !string.IsNullOrEmpty(age))
                    errors.Add("Employee Age must be between 18 and 60");
            }

            if(!decimal.TryParse(salary, out decimal salaryDecimal) && !string.IsNullOrEmpty(salary))
                errors.Add("Employee Salary must be a number");
            else
            {
                if (salaryDecimal < 0 && salaryDecimal > 99999)
                    errors.Add("Employee Salary must be greater than 0 and less than 99999");
            }

            Department? department = Company.GetDepartment(departmentName);
            if(department == null && departmentName != "")
                errors.Add("Department with this name does not exist");


            if (errors.Count > 0)
            {
                ConsoleExtension.WriteError(errors);
                employee = null!;
                return false;
            }
            else
            {
                employee = new Employee(name, ageInt, salaryDecimal, department!);
                return true;
            }
        }
    }
}
