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
            if (employee.IsEligible())
            {
                decimal newSalary = employee.GetSalary() + (employee.GetSalary()) * .15m;
                employee.SetSalary(newSalary);

                if (employee.GetJopTitle()!=JopTitles.Principal)
                {
                    JopTitles newJopTitle = (JopTitles)((int)employee.GetJopTitle() + 1);
                    employee.SetJopTitle(newJopTitle);
                }
                
            }
            else
            {
                Console.WriteLine("Can't Give Promotion");
            }


        }
    }
}
