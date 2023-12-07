using ControleDeHorasExtras.Models;

namespace HorasExtras.Api.Tests
{
    public class HoraExtraBuilder
    {
        private DateTime _horarioInicial = new(2023, 12, 01, 18, 0, 0);
        private DateTime _horarioFinal = new(2023, 12, 01, 20, 0, 0);
        private decimal _porcentagem = 150;

        public static HoraExtraBuilder Novo()
        {
            return new HoraExtraBuilder();
        }

        public HoraExtraBuilder ComHorarios(DateTime horarioInicial, DateTime horarioFinal)
        {
            _horarioInicial = horarioInicial;
            _horarioFinal = horarioFinal;
            return this;
        }

        public HoraExtraBuilder ComPorcentagem(decimal porcentagem)
        {
            _porcentagem = porcentagem;
            return this;
        }

        public HoraExtra Build()
        {
            return new HoraExtra(_horarioInicial, _horarioFinal, _porcentagem);
        }
    }
}
