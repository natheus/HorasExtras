using ControleDeHorasExtras.Models;
using Microsoft.EntityFrameworkCore;

public class HorasExtrasDb : DbContext
{
    public HorasExtrasDb(DbContextOptions<HorasExtrasDb> options)
    : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false);

        var configuration = builder.Build();

        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
    }

    public DbSet<HoraExtra> HorasExtras => Set<HoraExtra>();
}
