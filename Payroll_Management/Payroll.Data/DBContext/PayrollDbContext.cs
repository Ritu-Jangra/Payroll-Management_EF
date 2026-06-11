namespace Payroll.Data
{
    using Microsoft.EntityFrameworkCore;
    using Payroll.Domain;
    using System.Collections.Generic;

    public class PayrollDbContext : DbContext
    {
        public PayrollDbContext(DbContextOptions<PayrollDbContext> options) : base(options) { }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Permissions> Permissions { get; set; }

        public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Leave> Leaves { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // add database constraints such as unique indexes and relationships
            ConfigureDatabaseConstraints(modelBuilder);

            // enum conversions for RoleType, DepartmentType, ReasonType, and StatusType to store them as strings in db
            ConfigureEnumConversions(modelBuilder);

            // seed the database with initial data for testing
            SeedData(modelBuilder);
        }

        #region Database Constraints

        private static void ConfigureDatabaseConstraints(ModelBuilder modelBuilder)
        {
            // Add constraints on RolePermission table
            AddRolePermissionTableConstraints(modelBuilder);

            // Add constraints on Employee table
            AddEmployeeTableConstraints(modelBuilder);

            // Add constraints on Leave table
            AddLeaveTableConstraints(modelBuilder);
        }

        private  static void AddRolePermissionTableConstraints(ModelBuilder modelBuilder)
        {
            // Composite primary key on RolePermission table to prevent duplicate role-permission assignments
            modelBuilder.Entity<RolePermission>()
                        .HasKey(rp => new { rp.RoleId, rp.PermissionId });
        }

        private static void AddEmployeeTableConstraints(ModelBuilder modelBuilder)
        {
            // Unique constraint on Employee table for UserName to prevent duplicate accounts
            modelBuilder.Entity<Employee>()
                        .HasIndex(e => e.UserName)
                        .IsUnique();

            // Unique constraint on Employee table for Email to prevent duplicate accounts
            modelBuilder.Entity<Employee>()
                        .HasIndex(e => e.Email)
                        .IsUnique();

            // Unique constraint on Employee table for combination of UserName and Department to prevent duplicate usernames within the same department
            modelBuilder.Entity<Employee>()
                        .HasIndex(e => new { e.UserName, e.Department })
                        .IsUnique();

            // Check constraint to prevent an employee from being their own manager
            modelBuilder.Entity<Employee>()
                        .ToTable(t => t.HasCheckConstraint("CK_Employee_Manager", "[EmployeeId] <> [ManagerId]"));

            // Check constraint to ensure Salary is non-negative
            modelBuilder.Entity<Employee>()
                        .ToTable(t => t.HasCheckConstraint("CK_Employee_Salary", "[Salary] >= 0"));

            // Precision constraint on Salary to ensure it has 2 decimal places
            modelBuilder.Entity<Employee>()
                        .Property(e => e.Salary)
                        .HasPrecision(18, 2);
        }

        private static void AddLeaveTableConstraints(ModelBuilder modelBuilder)
        {
            // Unique constraint on Leave table for EmployeeId, StartDate, and EndDate to prevent overlapping leaves
            modelBuilder.Entity<Leave>()
                        .HasIndex(l => new { l.EmployeeId, l.StartDate, l.EndDate })
                        .IsUnique();

            // Check constraint to ensure StartDate is before EndDate
            modelBuilder.Entity<Leave>()
                        .ToTable(t => t.HasCheckConstraint("CK_Leave_Dates", "[StartDate] <= [EndDate]"));            

            // Applicant relationship (Employee → Leaves)
            modelBuilder.Entity<Leave>()
                        .HasOne(l => l.Employee)
                        .WithMany(e => e.Leaves)
                        .HasForeignKey(l => l.EmployeeId)
                        .OnDelete(DeleteBehavior.Restrict);

            // Approver relationship (Employee → Leave)
            modelBuilder.Entity<Leave>()
                        .HasOne(l => l.Approver)
                        .WithMany() // no navigation property in Employee
                        .HasForeignKey(l => l.ActionBy)
                        .OnDelete(DeleteBehavior.Restrict);
        }

        #endregion Database Constraints

        #region Enum Conversions

        private static void ConfigureEnumConversions(ModelBuilder modelBuilder)
        {
            // Convert RoleType enum to string in DB
            modelBuilder.Entity<Role>()
                        .Property(r => r.Name)
                        .HasConversion<string>();

            // Convert PermissionType enum to string in DB for the Permissions entity
            modelBuilder.Entity<Permissions>()
                        .Property(p => p.PermissionName)
                        .HasConversion<string>();

            // Convert DepartmentType enum to string in DB
            modelBuilder.Entity<Employee>()
                        .Property(e => e.Department)
                        .HasConversion<string>();

            // Convert ReasonType enum to string in DB
            modelBuilder.Entity<Leave>()
                        .Property(l => l.Reason)
                        .HasConversion<string>();

            // Convert StatusType enum to string in DB
            modelBuilder.Entity<Leave>()
                        .Property(l => l.Status)
                        .HasConversion<string>();
        }

        #endregion Enum Conversions

        #region SeedData

        private static void SeedData(ModelBuilder modelBuilder)
        {
            //seed the roles into the database
            modelBuilder.Entity<Role>().HasData(GetRolesTestData());

            //seed the permissions into the database
            modelBuilder.Entity<Permissions>().HasData(GetPermissionsTestData());

            //seed the role-permissions into the database
            modelBuilder.Entity<RolePermission>().HasData(GetRolePermissionsTestData());

            //seed the employees into the database
            modelBuilder.Entity<Employee>().HasData(GetEmployeesTestData());

            //seed the leaves into the database
            modelBuilder.Entity<Leave>().HasData(GetLeavesTestData());
        }

        #endregion SeedData

        #region TestData

        #region Private Fields

        private static readonly Guid AdminRoleId = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        private static readonly Guid ManagerRoleId = Guid.Parse("b2c3d4e5-f678-90ab-cdef-234567890abc");
        private static readonly Guid EmployeeRoleId = Guid.Parse("c3d4e5f6-7890-abcd-ef12-34567890abcd");
        private static readonly Guid JohnEmployeeId = Guid.Parse("fd1fd133-c723-4bbe-a1c7-882deb3f8be0");
        private static readonly Guid JaneEmployeeId = Guid.Parse("68d32acc-9c38-435b-9218-7fe99593519c");
        private static readonly Guid AliceEmployeeId = Guid.Parse("23680711-e03c-4c1b-a965-26e5b5f12a3a");

        #endregion Private Fields

        private static List<Role> GetRolesTestData()
        {
            return new List<Role>
            {
                // ADMIN → FULL ACCESS
                new Role
                {
                    RoleId = AdminRoleId,
                    Name = RoleType.Admin
                },

                // MANAGER
                new Role
                {
                    RoleId = ManagerRoleId,
                    Name = RoleType.Manager
                },

                // EMPLOYEE
                new Role
                {
                    RoleId = EmployeeRoleId,
                    Name = RoleType.Employee
                }
            };
        }

        private static List<Permissions> GetPermissionsTestData()
        {
            return new List<Permissions>
            {
                new Permissions { PermissionId = 1, PermissionName = PermissionType.VIEW_PERSONAL_INFO },
                new Permissions { PermissionId = 2, PermissionName = PermissionType.APPLY_LEAVE },
                new Permissions { PermissionId = 3, PermissionName = PermissionType.UPDATE_LEAVE },
                new Permissions { PermissionId = 4, PermissionName = PermissionType.DELETE_LEAVE },
                new Permissions { PermissionId = 5, PermissionName = PermissionType.APPROVE_LEAVE},
                new Permissions { PermissionId = 6, PermissionName = PermissionType.REJECT_LEAVE},
                new Permissions { PermissionId = 7, PermissionName = PermissionType.VIEW_MY_LEAVES},
                new Permissions { PermissionId = 8, PermissionName = PermissionType.VIEW_TEAM_LEAVES},
                new Permissions { PermissionId = 9, PermissionName = PermissionType.VIEW_TEAM},
                new Permissions { PermissionId = 10, PermissionName = PermissionType.ADD_EMPLOYEE},
                new Permissions { PermissionId = 11, PermissionName = PermissionType.UPDATE_EMPLOYEE},
                new Permissions { PermissionId = 12, PermissionName = PermissionType.DELETE_EMPLOYEE},
            };
        }

        private static List<RolePermission> GetRolePermissionsTestData()
        {
            return new List<RolePermission>
            {
                // Admin has all permissions
                new RolePermission { RoleId = AdminRoleId, PermissionId = 1 },
                new RolePermission { RoleId = AdminRoleId, PermissionId = 5 },
                new RolePermission { RoleId = AdminRoleId, PermissionId = 6 },
                new RolePermission { RoleId = AdminRoleId, PermissionId = 8 },
                new RolePermission { RoleId = AdminRoleId, PermissionId = 9 },
                new RolePermission { RoleId = AdminRoleId, PermissionId = 10 },
                new RolePermission { RoleId = AdminRoleId, PermissionId = 11 },
                new RolePermission { RoleId = AdminRoleId, PermissionId = 12 },

                // Manager has most permissions except for editing salary and managing employees
                new RolePermission { RoleId = ManagerRoleId, PermissionId = 1 },
                new RolePermission { RoleId = ManagerRoleId, PermissionId = 2 },
                new RolePermission { RoleId = ManagerRoleId, PermissionId = 3 },
                new RolePermission { RoleId = ManagerRoleId, PermissionId = 4 },
                new RolePermission { RoleId = ManagerRoleId, PermissionId = 5 },
                new RolePermission { RoleId = ManagerRoleId, PermissionId = 6 },
                new RolePermission { RoleId = ManagerRoleId, PermissionId = 7 },
                new RolePermission { RoleId = ManagerRoleId, PermissionId = 8 },
                new RolePermission { RoleId = ManagerRoleId, PermissionId = 9 },

                // Employee has only basic permissions
                new RolePermission { RoleId = EmployeeRoleId, PermissionId = 1 },
                new RolePermission { RoleId = EmployeeRoleId, PermissionId = 2 },
                new RolePermission { RoleId = EmployeeRoleId, PermissionId = 3 },
                new RolePermission { RoleId = EmployeeRoleId, PermissionId = 4 },
                new RolePermission { RoleId = EmployeeRoleId, PermissionId = 7 },
            };
        }

        private static List<Employee> GetEmployeesTestData()
        {
            List<Employee> result = new()
            {
                new Employee
                {
                    EmployeeId = JohnEmployeeId,
                    FirstName = "John",
                    LastName = "Doe",
                    UserName = "JohnDoe",
                    Email = "JohnDoe@cyb.com",
                    Salary = 50000,
                    Department = DepartmentType.HR,
                    PasswordHash = "AQAAAAIAAYagAAAAEIotcWyC8jKEkLNi33llIMl7NxGqWzBFlBxTv6OHymh1JdT4YwVacTZ4W/YEaeo86Q=="!,
                    RoleId = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), // Admin
                },
                new Employee
                {
                    EmployeeId = JaneEmployeeId,
                    FirstName = "Jane",
                    LastName = "Smith",
                    UserName = "JaneSmi",
                    Email = "JaneSmi@cyb.com",
                    Salary = 45000,
                    Department = DepartmentType.IT,
                    PasswordHash = "AQAAAAIAAYagAAAAEKDex9h2c0RxJ6PSOIQ+UKCwEitRbSZGIKZSzQjBDvQhc8ck+BuGhbRKGkUH8wBdGA=="!,
                    RoleId = ManagerRoleId, // Manager
                    ManagerId = JohnEmployeeId // John is the manager of Jane
                },
                new Employee
                {
                    EmployeeId = AliceEmployeeId,
                    FirstName = "Alice",
                    LastName = "Johnson",
                    UserName = "AliceJo",
                    Email = "AliceJo@cyb.com",
                    Salary = 30000,
                    Department = DepartmentType.IT,
                    PasswordHash = "AQAAAAIAAYagAAAAEPGNNvgYytdkPGMbSs7XZdWTPmmjW8CKVIo5bDMDGfLxKTdMaEMZdjXfMPwyfWZrkw=="!,
                    RoleId = EmployeeRoleId, // Employee
                    ManagerId = JaneEmployeeId, // Jane is the manager of Alice
                }
            };

            return result;
        }

        private static List<Leave> GetLeavesTestData()
        {
            return new List<Leave>
            {
                new Leave
                {
                    LeaveId = Guid.Parse("7747e180-5612-46d4-9d91-e1787298ea22"),
                    Reason = ReasonType.CasualLeave,
                    StartDate = GetUtcDateTime(2023, 1, 1),
                    EndDate = GetUtcDateTime(2023, 1, 5),
                    Status = StatusType.Approved,
                    EmployeeId = AliceEmployeeId, // Alice's leave
                    ActionBy = JaneEmployeeId // Jane approved Alice's leave
                },
                new Leave
                {
                    LeaveId = Guid.Parse("9f5cb020-df86-4658-9250-d29c1aaf96f0"),
                    Reason = ReasonType.SickLeave,
                    StartDate = GetUtcDateTime(2023, 2, 10),
                    EndDate = GetUtcDateTime(2023, 2, 20),
                    Status = StatusType.Pending,
                    EmployeeId = JaneEmployeeId, // Jane's leave
                    ActionBy = null // Jane's leave is still pending approval
                },
                new Leave
                {
                    LeaveId = Guid.Parse("823e1a65-227f-416a-bc2d-41ab554f6752"),
                    Reason = ReasonType.VacationLeave,
                    StartDate = GetUtcDateTime(2023, 3, 15),
                    EndDate = GetUtcDateTime(2023, 3, 18),
                    Status = StatusType.Rejected,
                    EmployeeId = AliceEmployeeId, // Alice's leave
                    ActionBy = JaneEmployeeId // Jane rejected Alice's leave
                }
            };
        }

        private static DateTime GetUtcDateTime(int year, int month, int day)
        {
            return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
        }

        #endregion TestData
    }
}
