using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EmployeeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class SeedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "ID", "DepartmentHeadId", "Name" },
                values: new object[,]
                {
                    { -4, null, "Marketing" },
                    { -3, null, "Finance" },
                    { -2, null, "HR" },
                    { -1, null, "IT" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "ID", "Age", "DepartmentId", "EmploymentDate", "JopTitle", "Name", "Rate", "Salary" },
                values: new object[,]
                {
                    { -7, 40, -2, new DateOnly(2025, 3, 29), "Senior", "Fouad Magdy", "Unrated", 10000m },
                    { -6, 40, -2, new DateOnly(2025, 3, 29), "Fresher", "Taha Ragab", "Unrated", 10000m },
                    { -5, 40, -2, new DateOnly(2025, 3, 29), "Principal", "Ashraf Khaled", "Unrated", 10000m },
                    { -4, 40, -2, new DateOnly(2025, 3, 29), "Senior", "Momen Ahmed", "Unrated", 10000m },
                    { -3, 35, -1, new DateOnly(2025, 3, 29), "Junior", "Omar Abdelbaset", "Unrated", 9000m },
                    { -2, 30, -2, new DateOnly(2025, 3, 29), "Mid", "Ali Saad", "Unrated", 7000m },
                    { -1, 25, -1, new DateOnly(2025, 3, 29), "Mid", "Ahmed Fahmy", "Unrated", 5000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "ID",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "ID",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "ID",
                keyValue: -7);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "ID",
                keyValue: -6);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "ID",
                keyValue: -5);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "ID",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "ID",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "ID",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "ID",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "ID",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "ID",
                keyValue: -1);
        }
    }
}
