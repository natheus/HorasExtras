using Microsoft.EntityFrameworkCore;
using NetDevPack.Identity.Jwt;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<HorasExtrasDb>();
builder.Services.AddControllers();

builder.Services.AddIdentityEntityFrameworkContextConfiguration(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("ControleDeHorasExtras")));

builder.Services.AddIdentityConfiguration();

builder.Services.AddAuthentication()
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
        options.AccessDeniedPath = "/AccessDenied";
    });

builder.Services.AddJwtConfiguration(builder.Configuration, "AppSettings");
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("user", policy => policy.RequireClaim("Store", "user"));
    options.AddPolicy("admin", policy => policy.RequireClaim("Store", "admin"));
});

builder.Services.AddSwaggerGen();
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseAuthConfiguration();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();