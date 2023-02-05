using Microsoft.EntityFrameworkCore;
using ProEventos.API.Models;

namespace ProEventos.API.Data;

public class DataContext : DbContext
{
    public DbSet<Evento>? Eventos { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options) {}

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={System.IO.Path.Join(Environment.CurrentDirectory, "ProEventos.db")}");
}