using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                .Property(e=>e.Rate)
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
        }
        override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=EMS;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}
