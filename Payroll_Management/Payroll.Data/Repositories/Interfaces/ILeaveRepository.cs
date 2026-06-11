namespace Payroll.Data
{
    using Payroll.Domain;

    public interface ILeaveRepository
    {
        Task<List<Leave>> GetAllLeavesAsync(Guid? employeeId = null,
                                            Guid? managerId = null,
                                            CancellationToken cancellationToken = default);

        Task<Leave?> GetLeaveByIdAsync(Guid leaveId, CancellationToken cancellationToken);

        Task<Leave?> GetLeaveDetailsAsync(Guid leaveId, CancellationToken cancellationToken);

        Task AddLeaveAsync(Leave leave, CancellationToken cancellationToken);

        void UpdateLeave(Leave leave);

        void DeleteLeave(Leave leave);

        Task<bool> ExistsOverlapAsync(Guid employeeId,
                                      DateTime startDate,
                                      DateTime endDate,
                                      Guid? excludeLeaveId = null,
                                      CancellationToken cancellationToken = default);
    }
}
