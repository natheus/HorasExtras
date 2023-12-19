using ControleDeHorasExtras.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleDeHorasExtras.Infra.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<HoraExtra> HorasExtras { get; set; }
    }
}