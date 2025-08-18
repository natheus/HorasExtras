namespace OvertimeManagement.Domain.Models.ViewModels.Response
{
    public class OvertimeResponse
    {
        public int? Total { get; set; }
        public int? DaysWorked { get; set; }
        public decimal? HourlyRate { get; set; }
        public decimal? EstimatedEarningsPerHour { get; set; }
        public decimal? EstimatedEarningsPerMonth { get; set; }
    }
}
