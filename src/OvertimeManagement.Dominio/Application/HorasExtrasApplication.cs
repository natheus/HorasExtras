using ControleDeHorasExtras.Dominio.Domain.Models;
using ControleDeHorasExtras.Dominio.Domain.Models.ViewModels.Response;
using Microsoft.EntityFrameworkCore;

namespace ControleDeHorasExtras.Application
{
    public class HorasExtrasApplication(HorasExtrasDb context)
    {
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
            var currentDate = DateTime.Now;
            return await context.HorasExtras
                .Where(h => h.HorarioInicial.Year == currentDate.Year &&
                            h.HorarioInicial.Month == month &&
                            (!initialDay.HasValue || h.HorarioInicial.Day >= initialDay) &&
                            (!finalDay.HasValue || h.HorarioFinal.Day <= finalDay)
                ).ToListAsync();
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
