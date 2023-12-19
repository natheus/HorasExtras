using ControleDeHorasExtras.Domain.Interfaces;
using ControleDeHorasExtras.Domain.Models.ViewModels.Response;

namespace ControleDeHorasExtras.Domain.Models
{
    public class HoraExtraApp(IHoraExtraRepository horaExtraRepository)
    {
        private readonly IHoraExtraRepository _horaExtraRepository = horaExtraRepository;

        public void Create(int id, DateTime horarioInicial, DateTime horarioFinal, decimal porcentagem)
        {
            var contato = _horaExtraRepository.GetById(id);

            if (contato == null)
            {
                contato = new HoraExtra(horarioInicial, horarioFinal, porcentagem);
                _horaExtraRepository.Save(contato);
            }
            else
                contato.Update(horarioInicial, horarioFinal, porcentagem);
        }

        public async Task<HorasExtrasResponse> Calculate(decimal salario, int month, int? initialDay, int? finalDay)
        {
            ValidarParametros(initialDay, finalDay);

            var horasExtras = await ObterHorasExtras(month, initialDay, finalDay);
            var totalHoras = CalcularTotalHoras(horasExtras);
            var diasTrabalhados = CalcularDiasTrabalhados(horasExtras);
            var valorHoraTrabalhada = CalcularValorHoraTrabalhada(salario, horasExtras);
            var ganhosEstimadosHora = CalcularGanhosEstimadosHora(salario, horasExtras);
            var ganhosEstimadosMes = CalcularGanhosEstimadosMes(salario, horasExtras);
            var desconto = AplicarDesconto(ganhosEstimadosMes);

            ganhosEstimadosMes -= desconto;

            return CreateResponse(totalHoras, diasTrabalhados, valorHoraTrabalhada, ganhosEstimadosHora, ganhosEstimadosMes);
        }

        private static void ValidarParametros(int? initialDay, int? finalDay)
        {
            if (initialDay.HasValue && finalDay.HasValue && initialDay > finalDay)
                throw new ArgumentException("O dia inicial não pode ser maior que o dia final.");
        }

        private async Task<List<HoraExtra>> ObterHorasExtras(int month, int? initialDay, int? finalDay)
        {
            return await _horaExtraRepository.ObterHorasExtras(month, initialDay, finalDay);
        }

        private static int CalcularTotalHoras(List<HoraExtra> horasExtras)
        {
            return horasExtras.Sum(h => (int)(h.HorarioFinal - h.HorarioInicial).TotalMinutes) / 60;
        }

        private static int CalcularDiasTrabalhados(List<HoraExtra> horasExtras)
        {
            return horasExtras.Select(h => h.HorarioInicial.Date).Distinct().Count();
        }

        private static decimal CalcularValorHoraTrabalhada(decimal salario, List<HoraExtra> horasExtras)
        {
            return horasExtras.Average(h => CalcularValorHoraTrabalhada(salario, 8, h.Porcentagem).valorHoraTrabalhada);
        }

        private static decimal CalcularGanhosEstimadosHora(decimal salario, List<HoraExtra> horasExtras)
        {
            return horasExtras.Average(h => CalcularValorHoraTrabalhada(salario, 8, h.Porcentagem).ganhosEstimados);
        }

        private static decimal CalcularGanhosEstimadosMes(decimal salario, List<HoraExtra> horasExtras)
        {
            return horasExtras.Sum(h => CalcularValorHoraTrabalhada(salario, 8, h.Porcentagem).ganhosEstimados *
                (int)((h.HorarioFinal - h.HorarioInicial).TotalMinutes) / 60);
        }

        private static decimal AplicarDesconto(decimal ganhosEstimadosMes)
        {
            decimal despesaSocietaria = 4.5m;
            return ganhosEstimadosMes * (despesaSocietaria / 100);
        }

        private static HorasExtrasResponse
            CreateResponse(int totalHoras, int diasTrabalhados, decimal valorHoraTrabalhada, decimal ganhosEstimadosHora, decimal ganhosEstimadosMes)
        {
            return new HorasExtrasResponse
            {
                Total = totalHoras,
                DiasTrabalhados = diasTrabalhados,
                ValorHoraTrabalhada = Math.Round(valorHoraTrabalhada, 2),
                GanhosEstimadosHora = Math.Round(ganhosEstimadosHora, 2),
                GanhosEstimadosMes = Math.Round(ganhosEstimadosMes, 2)
            };
        }

        private static (decimal valorHoraTrabalhada, decimal ganhosEstimados)
            CalcularValorHoraTrabalhada(decimal salario, int horasTrabalhadasPorDia, decimal porcentagem)
        {
            int diasUteisPorMes = DiasUteis();
            decimal valorHoraTrabalhada = salario / (diasUteisPorMes * horasTrabalhadasPorDia);
            decimal ganhosEstimados = valorHoraTrabalhada * porcentagem / 100;

            return (valorHoraTrabalhada, ganhosEstimados);
        }

        private static int DiasUteis()
        {
            int mesAtual = DateTime.Today.Month;
            int anoAtual = DateTime.Today.Year;

            DateTime primeiroDiaDoMes = new(anoAtual, mesAtual, 1);
            int totalDiasNoMes = DateTime.DaysInMonth(anoAtual, mesAtual);

            int contadorDiasUteis = Enumerable.Range(1, totalDiasNoMes)
                .Select(dia => new DateTime(anoAtual, mesAtual, dia))
                .Count(data => data.DayOfWeek != DayOfWeek.Saturday && data.DayOfWeek != DayOfWeek.Sunday);

            return contadorDiasUteis;
        }
    }
}