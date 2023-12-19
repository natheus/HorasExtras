using Bogus;
using ControleDeHorasExtras.Domain.Models;
using ExpectedObjects;
using Xunit.Abstractions;

namespace HorasExtras.Api.Tests
{
    public class HorasExtrasTest : IDisposable
    {
        private readonly ITestOutputHelper _output;
        private readonly DateTime _horarioInicial;
        private readonly DateTime _horarioFinal;
        private readonly decimal _porcentagem;

        public HorasExtrasTest(ITestOutputHelper output)
        {
            _output = output;
            _output.WriteLine("Construtor sendo executado.");
            var faker = new Faker();

            var dataAleatoria = faker.Date.Between(DateTime.Parse("2023-12-01"), DateTime.Parse("2023-12-01"));

            _horarioInicial = new DateTime(dataAleatoria.Year, dataAleatoria.Month, dataAleatoria.Day, 18, 0, 0);
            _horarioFinal = _horarioInicial.AddHours(2);
            _porcentagem = faker.Random.Bool() ? 100 : 150;
        }

        [Fact]
        public void DeveCriarHoraExtra()
        {
            var horaExtraEsperada = new
            {
                HorarioInicial = _horarioInicial,
                HorarioFinal = _horarioFinal,
                Porcentagem = _porcentagem,
            };

            var horaExtra = new HoraExtra
                (
                    horaExtraEsperada.HorarioInicial,
                    horaExtraEsperada.HorarioFinal,
                    horaExtraEsperada.Porcentagem
                );

            horaExtraEsperada.ToExpectedObject().ShouldMatch(horaExtra);
        }

        [Theory]
        [InlineData("2023-12-31T18:00:00", "2023-12-31T16:00:00")]
        public void NaoDeveTerHorarioFinalMenorQueInicial(DateTime horarioInicial, DateTime horarioFinal)
        {
            Assert.ThrowsAny<ArgumentException>(() =>
                HoraExtraBuilder.Novo().ComHorarios(horarioInicial, horarioFinal).Build())
                .ComMensagem("O horário inicial deve ser anterior ao horário final.");
        }

        [Theory]
        [InlineData(50)]
        public void NaoDeveTerHoraExtraDiferenteDe100Ou150(decimal porcentagem)
        {
            Assert.ThrowsAny<ArgumentException>(() =>
                HoraExtraBuilder.Novo().ComPorcentagem(porcentagem).Build())
                .ComMensagem("A porcentagem deve ser 100 ou 150.");
        }

        public void Dispose()
        {
            _output.WriteLine("Dispose sendo executado.");
            GC.SuppressFinalize(this);
        }
    }
}
