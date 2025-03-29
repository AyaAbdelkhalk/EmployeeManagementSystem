using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem
{
    class PerformenceReview
    {
        public static void GivePromotion(Employee employee)
        {

            #region Old
            //if (employee.IsEligible())
            //{
            //    decimal newSalary = employee.GetSalary() + (employee.GetSalary()) * .15m;
            //    employee.SetSalary(newSalary);

            //    if (employee.GetJopTitle()!=JopTitles.Principal)
            //    {
            //        JopTitles newJopTitle = (JopTitles)((int)employee.GetJopTitle() + 1);
            //        employee.SetJopTitle(newJopTitle);
            //    }

            //}
            //else
            //{
            //    Console.WriteLine("Can't Give Promotion");
            //} 
            #endregion

            using (var context = new EMSContext())
            {
                var emp = context.Employees.Find(employee.GetEmployeeId());
                if (emp != null)
                {
                    if (emp.IsEligible())
                    {
                        decimal newSalary = emp.GetSalary() + (emp.GetSalary()) * .15m;
                        emp.SetSalary(newSalary);
                        employee.SetSalary(newSalary);
                        if (emp.JopTitle != JopTitles.Principal)
                        {
                            JopTitles newJopTitle = (JopTitles)((int)emp.JopTitle + 1);
                            emp.JopTitle = employee.JopTitle = newJopTitle;
                        }
                        context.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("Can't Give Promotion");
                    }
                }
                else
                {
                    Console.WriteLine("Employee Not Found");
                }
            }



        }
    }
}
