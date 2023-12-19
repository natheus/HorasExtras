using ControleDeHorasExtras.Domain.Models;

namespace ControleDeHorasExtras.Domain.Interfaces
{
    public interface IHoraExtraRepository : IRepository<HoraExtra>
    {
        Task<List<HoraExtra>> ObterHorasExtras(int month, int? initialDay, int? finalDay);
    }
}
