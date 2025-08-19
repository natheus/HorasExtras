using Microsoft.EntityFrameworkCore;
using OvertimeManagement.Domain.Interfaces;
using OvertimeManagement.Domain.Models;
using OvertimeManagement.Infra.Context;

namespace OvertimeManagement.Infra.Repositories;

public class OvertimeRepository(AppDbContext context) : Repository<Overtime>(context), IOvertimeRepository
{
    public async Task<List<Overtime>> GetOvertimeByMonthAsync(int month)
    {
        return await _context.Overtimes
            .Where(h => h.StartTime.Month == month)
            .OrderBy(h => h.StartTime)
            .ToListAsync();
    }

    public async Task<List<Overtime>> GetOvertimeAsync(int month, int? initialDay, int? finalDay)
    {
        var currentDate = DateTime.Now;

        return await _context.Overtimes
            .Where(h => h.StartTime.Year == currentDate.Year &&
                        h.StartTime.Month == month &&
                        (!initialDay.HasValue || h.StartTime.Day >= initialDay) &&
                        (!finalDay.HasValue || h.EndTime.Day <= finalDay)
            ).ToListAsync();
    }
}
