using OvertimeManagement.Application.Interfaces;
using OvertimeManagement.Domain.Interfaces;
using OvertimeManagement.Domain.Models;
using OvertimeManagement.Domain.Models.ViewModels.Response;

namespace OvertimeManagement.Application.Applications;

public class OvertimeApp(IOvertimeRepository overtimeRepository) : IOvertimeApp
{
    public async Task<OvertimeResponse> Calculate(decimal salary, int month, int? initialDay, int? finalDay)
    {
        ValidateParameters(initialDay, finalDay);

        var overtimes = await GetOvertimes(month, initialDay, finalDay);
        var totalHours = CalculateTotalHours(overtimes);
        var workedDays = CalculateWorkedDays(overtimes);
        var workedHourValue = CalculateWorkedHourValue(salary, overtimes);
        var estimatedEarningsPerHour = CalculateEstimatedEarningsPerHour(salary, overtimes);
        var estimatedEarningsPerMonth = CalculateEstimatedEarningsPerMonth(salary, overtimes);
        var discount = ApplyDiscount(estimatedEarningsPerMonth);

        estimatedEarningsPerMonth -= discount;

        return CreateResponse(totalHours, workedDays, workedHourValue, estimatedEarningsPerHour, estimatedEarningsPerMonth);
    }

    private static void ValidateParameters(int? initialDay, int? finalDay)
    {
        if (initialDay.HasValue && finalDay.HasValue && initialDay > finalDay)
            throw new ArgumentException("O dia inicial não pode ser maior que o dia final.");
    }

    private async Task<List<Overtime>> GetOvertimes(int month, int? initialDay, int? finalDay)
    {
        return await overtimeRepository.GetOvertimeAsync(month, initialDay, finalDay);
    }

    private static int CalculateTotalHours(List<Overtime> overtimes)
    {
        return overtimes.Sum(o => (int)(o.EndTime - o.StartTime).TotalMinutes) / 60;
    }

    private static int CalculateWorkedDays(List<Overtime> overtimes)
    {
        return overtimes.Select(o => o.StartTime.Date).Distinct().Count();
    }

    private static decimal CalculateWorkedHourValue(decimal salary, List<Overtime> overtimes)
    {
        return overtimes.Average(o => CalculateWorkedHourValue(salary, 8, o.Percentage).workedHourValue);
    }

    private static decimal CalculateEstimatedEarningsPerHour(decimal salary, List<Overtime> overtimes)
    {
        return overtimes.Average(o => CalculateWorkedHourValue(salary, 8, o.Percentage).estimatedEarnings);
    }

    private static decimal CalculateEstimatedEarningsPerMonth(decimal salary, List<Overtime> overtimes)
    {
        return overtimes.Sum(o => CalculateWorkedHourValue(salary, 8, o.Percentage).estimatedEarnings *
            (int)(o.EndTime - o.StartTime).TotalMinutes / 60);
    }

    private static decimal ApplyDiscount(decimal estimatedEarningsPerMonth)
    {
        decimal companyExpense = 4.5m;
        return estimatedEarningsPerMonth * (companyExpense / 100);
    }

    private static OvertimeResponse CreateResponse(int totalHours, int workedDays, decimal workedHourValue,
        decimal estimatedEarningsPerHour, decimal estimatedEarningsPerMonth)
    {
        return new OvertimeResponse
        {
            Total = totalHours,
            DaysWorked = workedDays,
            HourlyRate = Math.Round(workedHourValue, 2),
            EstimatedEarningsPerHour = Math.Round(estimatedEarningsPerHour, 2),
            EstimatedEarningsPerMonth = Math.Round(estimatedEarningsPerMonth, 2)
        };
    }

    private static (decimal workedHourValue, decimal estimatedEarnings)
        CalculateWorkedHourValue(decimal salary, int workedHoursPerDay, decimal percentage)
    {
        int businessDaysPerMonth = GetBusinessDays();
        decimal workedHourValue = salary / (businessDaysPerMonth * workedHoursPerDay);
        decimal estimatedEarnings = workedHourValue * percentage / 100;

        return (workedHourValue, estimatedEarnings);
    }

    private static int GetBusinessDays()
    {
        int currentMonth = DateTime.Today.Month;
        int currentYear = DateTime.Today.Year;

        DateTime firstDayOfMonth = new(currentYear, currentMonth, 1);
        int totalDaysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);

        int businessDaysCount = Enumerable.Range(1, totalDaysInMonth)
            .Select(day => new DateTime(currentYear, currentMonth, day))
            .Count(date => date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday);

        return businessDaysCount;
    }

    public async Task SaveAsync(Overtime overTimes)
    {
        await overtimeRepository.SaveAsync(overTimes);
    }

    public async Task<IEnumerable<Overtime>> GetAllAsync()
    {
        return await overtimeRepository.GetAllAsync();
    }

    public async Task<OverTimeMonthResponse> GetOvertimeByMonthAsync(int month)
    {
        var overtimeMonth = await overtimeRepository.GetOvertimeByMonthAsync(month);

        return new OverTimeMonthResponse() { OvertimeDays = overtimeMonth.Count, Overtime = overtimeMonth };
    }

    public async Task<Overtime?> GetOvertimeByIdAsync(int id)
    {
        return await overtimeRepository.GetByIdAsync(id);
    }

    public async Task PutOvertimeAsync(Overtime overtime)
    {
        _ = await overtimeRepository.GetByIdAsync(overtime.Id) ??
            throw new KeyNotFoundException($"Overtime with ID {overtime.Id} not found.");

        await overtimeRepository.UpdateAsync(overtime);
    }

    public async Task DeleteOvertimeAsync(int id)
    {
        _ = await overtimeRepository.GetByIdAsync(id) ??
            throw new KeyNotFoundException($"Overtime with ID {id} not found.");

        await overtimeRepository.RemoveAsync(id);
    }
}