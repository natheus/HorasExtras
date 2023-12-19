using ControleDeHorasExtras.Application.DI;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
Initializer.Configure(builder, builder.Configuration.GetConnectionString("DefaultConnection")!);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseAuthConfiguration();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
