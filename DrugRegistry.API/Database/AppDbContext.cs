using DrugRegistry.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace DrugRegistry.API.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options) {}
    public DbSet<Drug> Drugs => Set<Drug>();
}