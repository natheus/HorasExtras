using ControleDeHorasExtras.Domain.Interfaces;
using ControleDeHorasExtras.Domain.Models;
using ControleDeHorasExtras.Infra.Context;
using ControleDeHorasExtras.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using NetDevPack.Identity.Jwt;

namespace ControleDeHorasExtras.Application.DI
{
    public class Initializer
    {
        public static void Configure(WebApplicationBuilder builder, string conection)
        {
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(conection));

            builder.Services.AddIdentityEntityFrameworkContextConfiguration(options =>
                options.UseSqlite(conection,
                b => b.MigrationsAssembly("ControleDeHorasExtras")));

            builder.Services.AddScoped(typeof(IRepository<HoraExtra>), typeof(HoraExtraRepository));
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped(typeof(HoraExtraApp));
            builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            builder.Services.AddScoped<IHoraExtraRepository, HoraExtraRepository>();
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
        }
    }
}
