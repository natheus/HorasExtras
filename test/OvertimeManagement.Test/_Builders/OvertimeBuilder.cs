using OvertimeManagement.Domain.Models;

namespace OvertimeManagement.Test._Builders
{
    public class OvertimeBuilder
    {
        private DateTime _startTime = new(2025, 12, 01, 18, 0, 0);
        private DateTime _endTime = new(2025, 12, 01, 20, 0, 0);
        private decimal _percentage = 150;

        public static OvertimeBuilder New()
        {
            return new OvertimeBuilder();
        }

        public OvertimeBuilder WithTimes(DateTime startTime, DateTime endTime)
        {
            _startTime = startTime;
            _endTime = endTime;
            return this;
        }

        public OvertimeBuilder WithPercentage(decimal percentage)
        {
            _percentage = percentage;
            return this;
        }

        public Overtime Build()
        {
            return new Overtime(_startTime, _endTime, _percentage);
        }
    }
}
