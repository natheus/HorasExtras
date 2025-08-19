using Microsoft.EntityFrameworkCore;
using NetDevPack.Identity.Jwt;
using OvertimeManagement.Application.Applications;
using OvertimeManagement.Application.Interfaces;
using OvertimeManagement.Infra.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IOvertimeApp, OvertimeApp>();

Initializer.ConfigureDb(builder, builder.Configuration.GetConnectionString("DefaultConnection")!);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentityEntityFrameworkContextConfiguration(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("OvertimeManagement")));

builder.Services.AddIdentityConfiguration();
builder.Services.AddJwtConfiguration(builder.Configuration, "AppSettings");

builder.Services.AddAuthentication()
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
        options.AccessDeniedPath = "/AccessDenied";
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("user", policy => policy.RequireClaim("Store", "user"))
    .AddPolicy("admin", policy => policy.RequireClaim("Store", "admin"));

builder.Services.AddMvc(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseAuthConfiguration();
app.MapControllers();

app.Run();
