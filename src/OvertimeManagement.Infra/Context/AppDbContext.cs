using OvertimeManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace OvertimeManagement.Infra.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Overtime> Overtimes { get; set; }
}