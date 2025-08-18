namespace OvertimeManagement.Test._Util
{
    public static class AssertExtension
    {
        public static void WithMessage(this ArgumentException exception, string message)
        {
            if (exception.Message == message)
                Assert.True(true);
            else
                Assert.Fail($"Expected message '{message}'");
        }
    }
}
