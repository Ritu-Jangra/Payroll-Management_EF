namespace Payroll.Data
{
    using Microsoft.EntityFrameworkCore;
    using Payroll.Domain;

    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly PayrollDbContext _dbContext;

        public EmployeeRepository(PayrollDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets all employees with optional filtering by adminId, managerId, or employeeId.
        /// If adminId is provided, the admin user will be excluded from the results.
        /// If managerId is provided, only employees reporting to that manager will be included.
        /// If employeeId is provided, only that specific employee will be included.
        /// </summary>
        /// <param name="adminId">The ID of the admin user</param>
        /// <param name="managerId">The ID of the manager</param>
        /// <param name="employeeId">The ID of the employee</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Returns a list of employees</returns>
        public async Task<List<Employee>> GetAllEmployeesAsync(Guid? adminId = null,
                                                               Guid? managerId = null,
                                                               Guid? employeeId = null,
                                                               CancellationToken cancellationToken = default)

        {
            var employees = await _dbContext.Employees.Include(e => e.Manager)
                                                      .Include(e => e.Role)
                                                      .Include(e => e.Leaves)
                                                      .ToListAsync(cancellationToken);

            // Exclude the admin user if adminId is provided
            if (adminId.HasValue)
            {
                employees = employees.Where(e => e.EmployeeId != adminId).ToList();
            }

            // Filter by managerId if provided
            if (managerId.HasValue)
            {
                employees = employees.Where(l => l.ManagerId == managerId.Value).ToList();
            }

            // Filter by employeeId if provided
            if (employeeId.HasValue)
            {
                employees = employees.Where(e => e.EmployeeId == employeeId.Value).ToList();
            }

            return employees;
        }

        /// <summary>
        /// Get an employee by their username, including their role and permissions for authentication purposes.
        /// </summary>
        /// <param name="username">The username of the employee</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Returns the employee if found, otherwise null</returns>
        public async Task<Employee?> GetEmployeeUsernameAsync(string username, CancellationToken cancellationToken)
        {
            return await GetEmployeeWithAuthQuery().FirstOrDefaultAsync(e => e.UserName == username, cancellationToken);
        }

        /// <summary>
        /// Get an employee by their user ID, including their role and permissions for authentication purposes.
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Returns the employee if found, otherwise null</returns>
        public async Task<Employee?> GetEmployeeWithPermissionsAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await GetEmployeeWithAuthQuery().FirstOrDefaultAsync(e => e.EmployeeId == userId, cancellationToken);
        }

        /// <summary>
        /// Get an employee by their ID, including their manager, role, and leaves for detailed information purposes.
        /// </summary>
        /// <param name="employeeId">The ID of the employee</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Returns the employee if found, otherwise null</returns>
        public async Task<Employee?> GetEmployeeByIdAsync(Guid employeeId, CancellationToken cancellationToken)
        {
            return await _dbContext.Employees.FirstOrDefaultAsync(e => e.EmployeeId == employeeId, cancellationToken);
        }

        /// <summary>
        /// Get an employee by their ID, including their manager, role, and leaves for detailed information purposes.
        /// </summary>
        /// <param name="employeeId">The ID of the employee</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Returns the employee if found, otherwise null</returns>
        public async Task<Employee?> GetEmployeeDetailsAsync(Guid employeeId, CancellationToken cancellationToken)
        {
            return await _dbContext.Employees.Include(e => e.Manager)
                                             .Include(e => e.Role)
                                             .Include(e => e.Leaves)
                                             .FirstOrDefaultAsync(e => e.EmployeeId == employeeId, cancellationToken);
        }

        /// <summary>
        /// Adds a new employee to the database. 
        /// The employee object should have all required properties set, including the RoleId for the employee's role.
        /// </summary>
        /// <param name="employee">The employee to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Returns the added employee entity</returns>
        public async Task AddEmployeeAsync(Employee employee, CancellationToken cancellationToken)
        {
            await _dbContext.Employees.AddAsync(employee, cancellationToken);
        }

        /// <summary>
        /// Deletes an existing employee from the database. 
        /// The employee object should have a valid EmployeeId that exists in the database.
        /// </summary>
        /// <param name="employee">The employee to delete</param>
        public void DeleteEmployee(Employee employee)
        {
            _dbContext.Employees.Remove(employee);
        }

        /// <summary>
        /// Updates an existing employee in the database.
        /// </summary>
        /// <param name="employee">The employee to update</param>
        public void UpdateEmployee(Employee employee)
        {
            _dbContext.Employees.Update(employee);
        }

        /// <summary>
        /// Ensures that the provided username is unique across all employees in the database, ignoring case sensitivity.
        /// </summary>
        /// <param name="userName">Username to check</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the username exists, otherwise false</returns>
        public async Task<bool> EmployeeUserNameExistsAsync(string userName, CancellationToken cancellationToken)
        {
            return await _dbContext.Employees.AnyAsync(e => e.UserName.ToLower() == userName.ToLower(), cancellationToken);
        }

        /// <summary>
        /// Ensures that the provided email is unique across all employees in the database, ignoring case sensitivity.
        /// </summary>
        /// <param name="email">Email to check</param>
        /// <param name="excludeEmployeeId">Employee ID to exclude from the check</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the email exists, otherwise false</returns>
        public async Task<bool> EmployeeEmailExistsAsync(string email, Guid? excludeEmployeeId = null, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Employees.AnyAsync(e => e.Email.ToLower() == email.ToLower()
                                                         && e.EmployeeId != excludeEmployeeId, cancellationToken);
        }

        /// <summary>
        /// Ensures that the provided username is unique within the specified department, ignoring case sensitivity.
        /// </summary>
        /// <param name="userName">Username to check</param>
        /// <param name="department">Department to check</param>
        /// <param name="excludeEmployeeId">Employee ID to exclude from the check</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the username exists in the department, otherwise false</returns>
        public async Task<bool> EmployeeUserNameExistsInDepartmentAsync(string userName,
                                                                        DepartmentType department,
                                                                        Guid? excludeEmployeeId = null,
                                                                        CancellationToken cancellationToken = default)
        {
            return await _dbContext.Employees.AnyAsync(e => e.UserName.ToLower() == userName.ToLower()
                                                         && e.Department == department
                                                         && e.EmployeeId != excludeEmployeeId, cancellationToken);
        }

        /// <summary>
        /// Gets the employee ID associated with the provided username, ignoring case sensitivity.
        /// </summary>
        /// <param name="userName">Username to check</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The employee ID if found, otherwise Guid.Empty</returns>
        public async Task<Guid> GetEmployeeIdByUserNameAsync(string userName, CancellationToken cancellationToken)
        {
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.UserName.ToLower() == userName.ToLower(), cancellationToken);
            return employee != null ? employee.EmployeeId : Guid.Empty;
        }

        /// <summary>
        /// Gets the role ID associated with the provided role type.
        /// </summary>
        /// <param name="roleType">Role type to check</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The role ID if found, otherwise Guid.Empty</returns>
        public async Task<Guid> GetRoleIdByRoleTypeAsync(RoleType roleType, CancellationToken cancellationToken)
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleType, cancellationToken);
            return role != null ? role.RoleId : Guid.Empty;
        }

        #region Private helper methods

        private IQueryable<Employee> GetEmployeeWithAuthQuery()
        {
            return _dbContext.Employees.AsNoTracking()
                                       .Include(e => e.Role)
                                       .ThenInclude(r => r!.RolePermissions)
                                       .ThenInclude(rp => rp.Permission);
        }

        #endregion Private helper methods
    }
}
