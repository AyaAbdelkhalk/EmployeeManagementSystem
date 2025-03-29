using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EmployeeManagementSystem
{
    class EMSContext : DbContext
    {
        public EMSContext() { }
        public EMSContext(DbContextOptions<EMSContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //add all properties to the model
            modelBuilder.Entity<Employee>().Property<string>("Name");
            modelBuilder.Entity<Employee>().Property<int>("Age");
            modelBuilder.Entity<Employee>().Property<decimal>("Salary");
            modelBuilder.Entity<Employee>().Property<DateOnly>("EmploymentDate");
            modelBuilder.Entity<Employee>().Property<bool>("Terminate").HasDefaultValue(false);

            modelBuilder.Entity<Employee>()
               .Property(e => e.JopTitle)
               .HasConversion<string>();

            modelBuilder.Entity<Employee>()
                .Property(e => e.Rate)
                .HasConversion<string>();

            modelBuilder.Entity<Employee>()
               .HasOne(e => e.Department)
               .WithMany(d => d.Employees)
               .HasForeignKey(e => e.DepartmentId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Department>()
                .HasOne(d => d.DepartmentHead)
                .WithMany()
                .HasForeignKey(d => d.DepartmentHeadId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Employee>()
            .Property(e => e.ID)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<Department>()
                .Property(d => d.ID)
                .ValueGeneratedOnAdd();

            #region Seeding Data
            Department ITDepartement = new Department() { ID = -1, Name = "IT" };
            Department HRDepartement = new Department() { ID = -2, Name = "HR" };
            Department FinanceDepartment = new Department() { ID = -3, Name = "Finance" };
            Department MarketingDepartment = new Department() { ID = -4, Name = "Marketing" };

            modelBuilder.Entity<Department>().HasData(
                ITDepartement, HRDepartement, FinanceDepartment, MarketingDepartment
                );

            Employee employee1 = new Employee("Ahmed Fahmy", 25, 5000, JopTitles.Mid, ITDepartement) { ID = -1};
            Employee employee2 = new Employee("Ali Saad", 30, 7000, JopTitles.Mid, HRDepartement) { ID = -2 };
            Employee employee3 = new Employee("Omar Abdelbaset", 35, 9000, JopTitles.Junior, ITDepartement) { ID = -3 };
            Employee employee4 = new Employee("Momen Ahmed", 40, 10000, JopTitles.Senior, HRDepartement) { ID = -4 };
            Employee employee5 = new Employee("Ashraf Khaled", 40, 10000, JopTitles.Principal, HRDepartement) { ID = -5 };
            Employee employee6 = new Employee("Taha Ragab", 40, 10000, JopTitles.Fresher, HRDepartement) { ID = -6 };
            Employee employee7 = new Employee("Fouad Magdy", 40, 10000, JopTitles.Senior, HRDepartement) { ID = -7 };
            modelBuilder.Entity<Employee>().HasData(employee1, employee2, employee3, employee4, employee5, employee6, employee7);
            # endregion
        }
        override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=EMS;Trusted_Connection=True;TrustServerCertificate=True;");
        }



    }
}
