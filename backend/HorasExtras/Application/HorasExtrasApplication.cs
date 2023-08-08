using ControleDeHorasExtras.Models;
using ControleDeHorasExtras.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ControleDeHorasExtras.Application
{
    public class HorasExtrasApplication
    {
        private readonly HorasExtrasDb _context;

        public HorasExtrasApplication(HorasExtrasDb context) => _context = context;

        public async Task<HorasExtrasResponse> Calculated(decimal salario, int month)
        {
            var currentDate = DateTime.Now;
            var horasExtras = await _context.HorasExtras
                .Where(h => h.HorarioInicial.Year == currentDate.Year && h.HorarioInicial.Month == month)
                .ToListAsync();

            int totalHours = horasExtras.Sum(h => (int)(h.HorarioFinal - h.HorarioInicial).TotalMinutes);
            int diasTrabalhados = horasExtras.Select(h => h.HorarioInicial.Date).Distinct().Count();

            decimal valorHoraTrabalhada = horasExtras.Average(h => CalcularValorHoraTrabalhada(salario, 8, h.Porcentagem).valorHoraTrabalhada);
            decimal ganhosEstimadosHora = horasExtras.Average(h => CalcularValorHoraTrabalhada(salario, 8, h.Porcentagem).ganhosEstimados);
            decimal ganhosEstimadosMes = horasExtras.Sum(h => CalcularValorHoraTrabalhada(salario, 8, h.Porcentagem).ganhosEstimados * (int)((h.HorarioFinal - h.HorarioInicial).TotalMinutes) / 60);
            
            decimal despesaSocietaria = 4.5m;
            decimal desconto = ganhosEstimadosMes * (despesaSocietaria / 100);

            ganhosEstimadosMes -= desconto;

            return new HorasExtrasResponse
            {
                Total = totalHours / 60,
                DiasTrabalhados = diasTrabalhados,
                ValorHoraTrabalhada = Math.Round(valorHoraTrabalhada, 2),
                GanhosEstimadosHora = Math.Round(ganhosEstimadosHora, 2),
                GanhosEstimadosMes = Math.Round(ganhosEstimadosMes, 2)
            };
        }

        static (decimal valorHoraTrabalhada, decimal ganhosEstimados)
            CalcularValorHoraTrabalhada(decimal salario, int horasTrabalhadasPorDia, decimal porcentagem)
        {
            int diasUteisPorMes = DiasUteis();
            decimal valorHoraTrabalhada = salario / (diasUteisPorMes * horasTrabalhadasPorDia);
            decimal ganhosEstimados = valorHoraTrabalhada * porcentagem / 100;

            return (valorHoraTrabalhada, ganhosEstimados);
        }

        static int DiasUteis()
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
