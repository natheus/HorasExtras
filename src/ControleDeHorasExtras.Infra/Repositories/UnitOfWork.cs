using ControleDeHorasExtras.Domain.Interfaces;
using ControleDeHorasExtras.Infra.Context;

namespace ControleDeHorasExtras.Infra.Repositories
{
    public class UnitOfWork(AppDbContext context) : IUnitOfWork
    {
        private readonly AppDbContext _context = context;

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}
