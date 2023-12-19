using ControleDeHorasExtras.Domain.Interfaces;
using ControleDeHorasExtras.Domain.Models;
using ControleDeHorasExtras.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace ControleDeHorasExtras.Infra.Repositories
{
    public class HoraExtraRepository(AppDbContext context) : Repository<HoraExtra>(context), IHoraExtraRepository
    {
        public override HoraExtra GetById(int id)
        {
            var query = _context.Set<HoraExtra>().Where(e => e.Id == id);

            if (query.Any())
                return query.First();

            return null;
        }

        public override IEnumerable<HoraExtra> GetAll()
        {
            var query = _context.Set<HoraExtra>();

            return query.Any() ? query.ToList() : [];
        }

        public async Task<List<HoraExtra>> ObterHorasExtras(int month, int? initialDay, int? finalDay)
        {
            var currentDate = DateTime.Now;
            return await context.HorasExtras
                .Where(h => h.HorarioInicial.Year == currentDate.Year &&
                            h.HorarioInicial.Month == month &&
                            (!initialDay.HasValue || h.HorarioInicial.Day >= initialDay) &&
                            (!finalDay.HasValue || h.HorarioFinal.Day <= finalDay)
                ).ToListAsync();
        }
    }
}
