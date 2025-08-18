using OvertimeManagement.Domain.Interfaces;
using OvertimeManagement.Domain.Models;
using OvertimeManagement.Infra.Context;
using OvertimeManagement.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using NetDevPack.Identity.Jwt;

namespace OvertimeManagement.Application.DI;

public class Initializer
{
    public static void Configure(WebApplicationBuilder builder, string connection)
    {
        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connection));

        builder.Services.AddIdentityEntityFrameworkContextConfiguration(options =>
            options.UseSqlite(connection,
            b => b.MigrationsAssembly("OvertimeManagement")));

        builder.Services.AddScoped(typeof(IRepository<Overtime>), typeof(OvertimeRepository));
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddScoped<IOvertimeRepository, OvertimeRepository>(); 
        builder.Services.AddScoped(typeof(OvertimeApp));

        builder.Services.AddIdentityConfiguration();

        builder.Services.AddAuthentication()
            .AddCookie("Cookies", options =>
            {
                options.LoginPath = "/login";
                options.LogoutPath = "/logout";
                options.AccessDeniedPath = "/AccessDenied";
            });

        builder.Services.AddJwtConfiguration(builder.Configuration, "AppSettings");

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("user", policy => policy.RequireClaim("Store", "user"))
            .AddPolicy("admin", policy => policy.RequireClaim("Store", "admin"));

        builder.Services.AddMvc(options =>
        {
            options.SuppressAsyncSuffixInActionNames = false;
        });
    }
}
