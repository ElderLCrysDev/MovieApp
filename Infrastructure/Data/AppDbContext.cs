using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Usuarios { get; set; } = null!;
    public DbSet<LogEntry> Logs { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Nome).IsRequired();
            b.Property(x => x.Senha).IsRequired();
            b.Property(x => x.TipoUsuario).IsRequired();
        });

        modelBuilder.Entity<LogEntry>(b =>
        {
            b.HasKey(x => x.Id);
        });
    }
}
