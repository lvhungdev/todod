using Application.Entities;
using Application.Ports;
using Microsoft.EntityFrameworkCore;

namespace Adapter.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Todo> Todos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Todo>().Property(m => m.Id).HasMaxLength(255);
        modelBuilder.Entity<Todo>().Property(m => m.Name).HasMaxLength(1024);
    }
}
