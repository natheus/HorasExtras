using Bogus;
using ExpectedObjects;
using OvertimeManagement.Domain.Models;
using OvertimeManagement.Test._Builders;
using OvertimeManagement.Test._Util;
using Xunit.Abstractions;

namespace OvertimeManagement.Test.overTimes;

public class OvertimeTest : IDisposable
{
    private readonly ITestOutputHelper _output;
    private readonly DateTime _startTime;
    private readonly DateTime _endTime;
    private readonly decimal _percentage;

    public OvertimeTest(ITestOutputHelper output)
    {
        _output = output;
        _output.WriteLine("Constructor being executed.");
        var faker = new Faker();

        var randomDate = faker.Date.Between(DateTime.Parse("2025-12-01"), DateTime.Parse("2025-12-01"));

        _startTime = new DateTime(randomDate.Year, randomDate.Month, randomDate.Day, 18, 0, 0);
        _endTime = _startTime.AddHours(2);
        _percentage = faker.Random.Bool() ? 100 : 150;
    }

    [Fact]
    public void ShouldCreateOvertime()
    {
        var expectedOvertime = new
        {
            StartTime = _startTime,
            EndTime = _endTime,
            Percentage = _percentage,
        };

        var overtime = new Overtime
            (
                expectedOvertime.StartTime,
                expectedOvertime.EndTime,
                expectedOvertime.Percentage
            );

        expectedOvertime.ToExpectedObject().ShouldMatch(overtime);
    }

    [Theory]
    [InlineData("2025-12-31T18:00:00", "2025-12-31T16:00:00")]
    public void ShouldNotHaveEndTimeEarlierThanStartTime(DateTime startTime, DateTime endTime)
    {
        Assert.ThrowsAny<ArgumentException>(() =>
            OvertimeBuilder.New().WithTimes(startTime, endTime).Build())
            .WithMessage("The start time must be earlier than the end time.");
    }

    [Theory]
    [InlineData(50)]
    public void ShouldNotHaveOvertimeWithPercentageDifferentFrom100Or150(decimal percentage)
    {
        Assert.ThrowsAny<ArgumentException>(() =>
            OvertimeBuilder.New().WithPercentage(percentage).Build())
            .WithMessage("The percentage must be 100 or 150.");
    }

    public void Dispose()
    {
        _output.WriteLine("Dispose being executed.");
        GC.SuppressFinalize(this);
    }
}
