namespace OvertimeManagement.Domain.Models.ViewModels.Response
{
    public class OverTimeMonthResponse
    {
        public int OvertimeDays { get; set; }
        public List<Overtime>? Overtime { get; set; }
    }
}
