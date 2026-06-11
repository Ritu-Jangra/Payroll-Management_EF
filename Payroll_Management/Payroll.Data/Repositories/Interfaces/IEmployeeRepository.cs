namespace Payroll.Data
{
    using Payroll.Domain;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllEmployeesAsync(Guid? adminId = null, Guid? managerId = null, Guid? employeeId = null, CancellationToken cancellationToken = default);

        Task<Employee?> GetEmployeeUsernameAsync(string username, CancellationToken cancellationToken);

        Task<Employee?> GetEmployeeWithPermissionsAsync(Guid userId, CancellationToken cancellationToken);

        Task<Employee?> GetEmployeeByIdAsync(Guid employeeId, CancellationToken cancellationToken);

        Task<Employee?> GetEmployeeDetailsAsync(Guid employeeId, CancellationToken cancellationToken);

        Task AddEmployeeAsync(Employee employee, CancellationToken cancellationToken);

        void UpdateEmployee(Employee employee);

        void DeleteEmployee(Employee employee);

        Task<bool> EmployeeUserNameExistsAsync(string userName, CancellationToken cancellationToken);

        Task<bool> EmployeeEmailExistsAsync(string email, Guid? excludeEmployeeId = null, CancellationToken cancellationToken = default);

        Task<bool> EmployeeUserNameExistsInDepartmentAsync(string userName, DepartmentType department, Guid? excludeEmployeeId = null, CancellationToken cancellationToken = default);

        Task<Guid> GetEmployeeIdByUserNameAsync(string userName, CancellationToken cancellationToken);

        Task<Guid> GetRoleIdByRoleTypeAsync(RoleType roleType, CancellationToken cancellationToken);
    }
}
