using OvertimeManagement.Domain.Models;

namespace OvertimeManagement.Domain.Interfaces;

public interface IOvertimeRepository : IRepository<Overtime>
{
    Task<List<Overtime>> GetOvertimeAsync(int month, int? initialDay, int? finalDay);
    Task<List<Overtime>> GetOvertimeByMonthAsync(int month);
}
