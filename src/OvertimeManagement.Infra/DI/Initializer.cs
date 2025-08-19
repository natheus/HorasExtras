using Microsoft.EntityFrameworkCore;
using OvertimeManagement.Domain.Interfaces;
using OvertimeManagement.Domain.Models;
using OvertimeManagement.Infra.Context;
using OvertimeManagement.Infra.Repositories;

namespace OvertimeManagement.Infra.DI;

public class Initializer
{
    public static void ConfigureDb(WebApplicationBuilder builder, string connection)
    {
        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connection));
        builder.Services.AddScoped(typeof(IRepository<Overtime>), typeof(OvertimeRepository));
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddScoped<IOvertimeRepository, OvertimeRepository>();
    }
}
