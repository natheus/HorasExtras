namespace OvertimeManagement.Domain.Models;

public class Overtime : BaseEntity
{
    public Overtime(DateTime startTime, DateTime endTime, decimal percentage)
    {
        ValidateOvertime(startTime, endTime, percentage);
        StartTime = startTime;
        EndTime = endTime;
        Percentage = percentage;
    }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal Percentage { get; set; }

    public void Update(DateTime startTime, DateTime endTime, decimal percentage)
    {
        ValidateOvertime(startTime, endTime, percentage);
    }
    private static void ValidateOvertime(DateTime startTime, DateTime endTime, decimal percentage)
    {
        if (startTime >= endTime)
            throw new ArgumentException("The start time must be earlier than the end time.");

        if (percentage != 100 && percentage != 150)
            throw new ArgumentException("The percentage must be 100 or 150.");
    }
}