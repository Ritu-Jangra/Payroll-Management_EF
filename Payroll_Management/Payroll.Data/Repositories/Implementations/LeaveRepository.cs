namespace Payroll.Data
{
    using Microsoft.EntityFrameworkCore;
    using Payroll.Domain;

    public class LeaveRepository : ILeaveRepository
    {
        private readonly PayrollDbContext _dbContext;

        public LeaveRepository(PayrollDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets all leaves, optionally filtered by employeeId or managerId.
        /// If employeeId is provided, it returns leaves for that employee.
        /// If managerId is provided, it returns leaves for employees under that manager. If both are null, it returns all leaves.
        /// </summary>
        /// <param name="employeeId">The ID of the employee to filter leaves by</param>
        /// <param name="managerId">The ID of the manager to filter leaves by</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Returns a list of leaves</returns>
        public async Task<List<Leave>> GetAllLeavesAsync(Guid? employeeId = null,
                                                         Guid? managerId = null,
                                                         CancellationToken cancellationToken = default)
        {
            var leavesQuery = _dbContext.Leaves.Include(l => l.Employee)
                                            .ThenInclude(e => e!.Manager)
                                            .Include(l => l.Approver)
                                            .AsQueryable();

            if (employeeId.HasValue)
            {
                leavesQuery = leavesQuery.Where(l => l.EmployeeId == employeeId.Value);
            }
            if (managerId.HasValue)
            {
                leavesQuery = leavesQuery.Where(l => l.Employee!.ManagerId == managerId.Value);
            }

            return await leavesQuery.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a leave by its ID, including the associated employee details. Returns null if not found.
        /// </summary>
        /// <param name="leaveId">The ID of the leave to retrieve</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Returns the leave if found, null otherwise</returns>
        public async Task<Leave?> GetLeaveByIdAsync(Guid leaveId, CancellationToken cancellationToken)
        {
            return await _dbContext.Leaves.Include(l => l.Employee).FirstOrDefaultAsync(l => l.LeaveId == leaveId, cancellationToken);
        }

        /// <summary>
        /// Gets a leave by its ID, including the associated employee and approver details. Returns null if not found.
        /// </summary>
        /// <param name="leaveId">The ID of the leave to retrieve</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Returns the leave if found, null otherwise</returns>
        public async Task<Leave?> GetLeaveDetailsAsync(Guid leaveId, CancellationToken cancellationToken)
        {
            return await _dbContext.Leaves.Include(l => l.Employee)
                                          .ThenInclude(e => e!.Manager)
                                          .Include(l => l.Approver)
                                          .FirstOrDefaultAsync(l => l.LeaveId == leaveId, cancellationToken);
        }

        /// <summary>
        /// Adds a new leave to the database context.
        /// The actual save to the database will occur when SaveChangesAsync is called on the context.
        /// </summary>
        /// <param name="leave">Leave entity to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Returns the added leave entity</returns>
        public async Task AddLeaveAsync(Leave leave, CancellationToken cancellationToken)
        {
            await _dbContext.Leaves.AddAsync(leave, cancellationToken);
        }

        /// <summary>
        /// Updates an existing leave in the database context.
        /// </summary>
        /// <param name="leave">Leave entity to update</param>
        public void UpdateLeave(Leave leave)
        {
            _dbContext.Leaves.Update(leave);
        }

        /// <summary>
        /// Ensures that there are no overlapping leave requests for the same employee within the specified date range, excluding a specific leave ID if provided (useful for updates).
        /// </summary>
        /// <param name="employeeId">The ID of the employee to check for overlapping leaves</param>
        /// <param name="startDate">The start date of the leave request</param>
        /// <param name="endDate">The end date of the leave request</param>
        /// <param name="excludeLeaveId">The ID of the leave to exclude from the check (for updates)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Returns true if there is an overlapping leave, false otherwise</returns>
        public async Task<bool> ExistsOverlapAsync(Guid employeeId,
                                                   DateTime startDate,
                                                   DateTime endDate,
                                                   Guid? excludeLeaveId = null,
                                                   CancellationToken cancellationToken = default)
        {
            //Ensure that the new leave does not overlap with existing leaves for the same employee, excluding the current leave if updating
            return await _dbContext.Leaves.AnyAsync(l => l.EmployeeId == employeeId &&
                                                         l.Status != StatusType.Rejected && l.Status != StatusType.Approved &&
                                                         (excludeLeaveId == null || l.LeaveId != excludeLeaveId) &&
                                                         startDate.Date <= l.EndDate.Date &&
                                                         endDate.Date >= l.StartDate.Date, cancellationToken);
        }

        /// <summary>
        /// Deletes a leave from the database context. 
        /// The actual deletion from the database will occur when SaveChangesAsync is called on the context.
        /// </summary>
        /// <param name="leave">Leave entity to delete</param>
        public void DeleteLeave(Leave leave)
        {
            _dbContext.Leaves.Remove(leave);
        }
    }
}