using Microsoft.EntityFrameworkCore;
using OvertimeManagement.Domain.Models;

namespace OvertimeManagement.Infra.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Overtime> Overtimes { get; set; }
}