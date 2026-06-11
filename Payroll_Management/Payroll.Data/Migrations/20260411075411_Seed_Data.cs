using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Payroll.Data.Migrations
{
    /// <inheritdoc />
    public partial class Seed_Data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "PermissionId", "PermissionName" },
                values: new object[,]
                {
                    { 1, "VIEW_PERSONAL_INFO" },
                    { 2, "APPLY_LEAVE" },
                    { 3, "UPDATE_LEAVE" },
                    { 4, "DELETE_LEAVE" },
                    { 5, "APPROVE_LEAVE" },
                    { 6, "REJECT_LEAVE" },
                    { 7, "VIEW_MY_LEAVES" },
                    { 8, "VIEW_TEAM_LEAVES" },
                    { 9, "VIEW_TEAM" },
                    { 10, "ADD_EMPLOYEE" },
                    { 11, "UPDATE_EMPLOYEE" },
                    { 12, "DELETE_EMPLOYEE" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "Name" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), "Admin" },
                    { new Guid("b2c3d4e5-f678-90ab-cdef-234567890abc"), "Manager" },
                    { new Guid("c3d4e5f6-7890-abcd-ef12-34567890abcd"), "Employee" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Department", "Email", "FirstName", "LastName", "ManagerId", "PasswordHash", "RoleId", "Salary", "UserName" },
                values: new object[] { new Guid("fd1fd133-c723-4bbe-a1c7-882deb3f8be0"), "HR", "JohnDoe@cyb.com", "John", "Doe", null, "AQAAAAIAAYagAAAAEIotcWyC8jKEkLNi33llIMl7NxGqWzBFlBxTv6OHymh1JdT4YwVacTZ4W/YEaeo86Q==", new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), 50000m, "JohnDoe" });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 1, new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890") },
                    { 5, new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890") },
                    { 6, new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890") },
                    { 8, new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890") },
                    { 9, new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890") },
                    { 10, new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890") },
                    { 11, new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890") },
                    { 12, new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890") },
                    { 1, new Guid("b2c3d4e5-f678-90ab-cdef-234567890abc") },
                    { 2, new Guid("b2c3d4e5-f678-90ab-cdef-234567890abc") },
                    { 3, new Guid("b2c3d4e5-f678-90ab-cdef-234567890abc") },
                    { 4, new Guid("b2c3d4e5-f678-90ab-cdef-234567890abc") },
                    { 5, new Guid("b2c3d4e5-f678-90ab-cdef-234567890abc") },
                    { 6, new Guid("b2c3d4e5-f678-90ab-cdef-234567890abc") },
                    { 7, new Guid("b2c3d4e5-f678-90ab-cdef-234567890abc") },
                    { 8, new Guid("b2c3d4e5-f678-90ab-cdef-234567890abc") },
                    { 9, new Guid("b2c3d4e5-f678-90ab-cdef-234567890abc") },
                    { 1, new Guid("c3d4e5f6-7890-abcd-ef12-34567890abcd") },
                    { 2, new Guid("c3d4e5f6-7890-abcd-ef12-34567890abcd") },
                    { 3, new Guid("c3d4e5f6-7890-abcd-ef12-34567890abcd") },
                    { 4, new Guid("c3d4e5f6-7890-abcd-ef12-34567890abcd") },
                    { 7, new Guid("c3d4e5f6-7890-abcd-ef12-34567890abcd") }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Department", "Email", "FirstName", "LastName", "ManagerId", "PasswordHash", "RoleId", "Salary", "UserName" },
                values: new object[,]
                {
                    { new Guid("68d32acc-9c38-435b-9218-7fe99593519c"), "IT", "JaneSmi@cyb.com", "Jane", "Smith", new Guid("fd1fd133-c723-4bbe-a1c7-882deb3f8be0"), "AQAAAAIAAYagAAAAEKDex9h2c0RxJ6PSOIQ+UKCwEitRbSZGIKZSzQjBDvQhc8ck+BuGhbRKGkUH8wBdGA==", new Guid("b2c3d4e5-f678-90ab-cdef-234567890abc"), 45000m, "JaneSmi" },
                    { new Guid("23680711-e03c-4c1b-a965-26e5b5f12a3a"), "IT", "AliceJo@cyb.com", "Alice", "Johnson", new Guid("68d32acc-9c38-435b-9218-7fe99593519c"), "AQAAAAIAAYagAAAAEPGNNvgYytdkPGMbSs7XZdWTPmmjW8CKVIo5bDMDGfLxKTdMaEMZdjXfMPwyfWZrkw==", new Guid("c3d4e5f6-7890-abcd-ef12-34567890abcd"), 30000m, "AliceJo" }
                });

            migrationBuilder.InsertData(
                table: "Leaves",
                columns: new[] { "LeaveId", "ActionBy", "EmployeeId", "EndDate", "Reason", "StartDate", "Status" },
                values: new object[,]
                {
                    { new Guid("9f5cb020-df86-4658-9250-d29c1aaf96f0"), null, new Guid("68d32acc-9c38-435b-9218-7fe99593519c"), new DateTime(2023, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "SickLeave", new DateTime(2023, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pending" },
                    { new Guid("7747e180-5612-46d4-9d91-e1787298ea22"), new Guid("68d32acc-9c38-435b-9218-7fe99593519c"), new Guid("23680711-e03c-4c1b-a965-26e5b5f12a3a"), new DateTime(2023, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "CasualLeave", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approved" },
                    { new Guid("823e1a65-227f-416a-bc2d-41ab554f6752"), new Guid("68d32acc-9c38-435b-9218-7fe99593519c"), new Guid("23680711-e03c-4c1b-a965-26e5b5f12a3a"), new DateTime(2023, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "VacationLeave", new DateTime(2023, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rejected" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Leaves",
                keyColumn: "LeaveId",
                keyValue: new Guid("7747e180-5612-46d4-9d91-e1787298ea22"));

            migrationBuilder.DeleteData(
                table: "Leaves",
                keyColumn: "LeaveId",
                keyValue: new Guid("823e1a65-227f-416a-bc2d-41ab554f6752"));

            migrationBuilder.DeleteData(
                table: "Leaves",
                keyColumn: "LeaveId",
                keyValue: new Guid("9f5cb020-df86-4658-9250-d29c1aaf96f0"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 1, new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 5, new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 6, new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 8, new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 9, new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 10, new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 11, new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 12, new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 1, new Guid("b2c3d4e5-f678-90ab-cdef-234567890abc") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 2, new Guid("b2c3d4e5-f678-90ab-cdef-234567890abc") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 3, new Guid("b2c3d4e5-f678-90ab-cdef-234567890abc") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 4, new Guid("b2c3d4e5-f678-90ab-cdef-234567890abc") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 5, new Guid("b2c3d4e5-f678-90ab-cdef-234567890abc") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 6, new Guid("b2c3d4e5-f678-90ab-cdef-234567890abc") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 7, new Guid("b2c3d4e5-f678-90ab-cdef-234567890abc") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 8, new Guid("b2c3d4e5-f678-90ab-cdef-234567890abc") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 9, new Guid("b2c3d4e5-f678-90ab-cdef-234567890abc") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 1, new Guid("c3d4e5f6-7890-abcd-ef12-34567890abcd") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 2, new Guid("c3d4e5f6-7890-abcd-ef12-34567890abcd") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 3, new Guid("c3d4e5f6-7890-abcd-ef12-34567890abcd") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 4, new Guid("c3d4e5f6-7890-abcd-ef12-34567890abcd") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 7, new Guid("c3d4e5f6-7890-abcd-ef12-34567890abcd") });

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("23680711-e03c-4c1b-a965-26e5b5f12a3a"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("68d32acc-9c38-435b-9218-7fe99593519c"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("c3d4e5f6-7890-abcd-ef12-34567890abcd"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("fd1fd133-c723-4bbe-a1c7-882deb3f8be0"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("b2c3d4e5-f678-90ab-cdef-234567890abc"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"));
        }
    }
}
