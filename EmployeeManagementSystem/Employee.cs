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
    class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        private string Name;
        private int Age;
        private decimal Salary;
        private DateOnly EmploymentDate;
        public Rate Rate { get; set; }
        private bool Terminate;
        public JopTitles JopTitle { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public Employee()
        {
        }
        public Employee(string name, int age, decimal salary, JopTitles jopTitles, Department department)
        {
            Name = name;
            Age = age;
            Salary = salary;
            DepartmentId = department.ID;   
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
                using(var context = new EMSContext())
                {
                    var employee = context.Employees.Find(ID);
                    if (employee != null)
                    {
                        employee.Rate = Rate = rate;
                        context.SaveChanges();
                    }
                }
            }
        }
        public void TransferDepartment(Department department)
        {
            if (department != null && Terminate == false)
            {
                using (var context = new EMSContext())
                {
                    var employee = context.Employees.Find(ID);
                    if (employee != null)
                    {
                        employee.DepartmentId = DepartmentId = department.ID;
                        context.SaveChanges();
                    }
                }
            }
        }

        public void TerminateEmployee()
        {
            Terminate = true;
        }
        public bool IsTerminated()
        {
            return Terminate;
        }
        public List<Employee> DisplayCurrentEmployees()
        {
            return Department.Employees
                .FindAll(e => e.IsTerminated() == false)
                .ToList();

        }
        public List<Employee> DisplayPastEmployees()
        {
            return Department.Employees
                .FindAll(e => e.IsTerminated() == true)
                .ToList();
        }

        public void DisplayEmployeeInfo()
        {

            //use data from the database
            Console.WriteLine($"{ID}\t {Name.PadRight(15)}\t {Age}\t {Salary} EGP " +
                $"\t {Department.Name}\t\t {EmploymentDate}\t {Rate}\t    {(Rate > Rate.MeetsExpectations ?
                "Eligible" : "Not Eligible")} \t {JopTitle.ToString().PadRight(12)} \t {Terminate}");
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
        public string GetRate()
        {
            return Rate.ToString();
        }

        public DateOnly GetEmployementDate()
        {
            return EmploymentDate;
        }
        public void SetSalary(decimal salary)
        {
            Salary = salary;
            using (var context = new EMSContext())
            {
                var employee = context.Employees.Find(ID);
                if (employee != null)
                {
                    employee.Salary = salary;
                    context.SaveChanges();
                }
            }

        }
        public JopTitles GetJopTitle()
        {
            return JopTitle ;
        }
        public void SetJobTitle(JopTitles jopTitle)
        {
            JopTitle = jopTitle;
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
