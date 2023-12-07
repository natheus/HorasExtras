namespace HorasExtras.Api.Tests
{
    public static class AssertExtension
    {
        public static void ComMensagem(this ArgumentException exception, string mensagem)
        {
            if (exception.Message == mensagem)
                Assert.True(true);
            else
                Assert.Fail($"Esperava a mensagem '{mensagem}'");
        }
    }
}
