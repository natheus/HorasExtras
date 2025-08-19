using OvertimeManagement.Domain.Models;
using OvertimeManagement.Domain.Models.ViewModels.Response;

namespace OvertimeManagement.Application.Interfaces
{
    public interface IOvertimeApp
    {
        Task<OvertimeResponse> Calculate(decimal salary, int month, int? initialDay = null, int? finalDay = null);
        Task<Overtime?> GetOvertimeByIdAsync(int id);
        Task<IEnumerable<Overtime>> GetAllAsync();
        Task<OverTimeMonthResponse> GetOvertimeByMonthAsync(int month);
        Task SaveAsync(Overtime overTimes);
        Task PutOvertimeAsync(Overtime overtime);
        Task DeleteOvertimeAsync(int id);
    }
}
