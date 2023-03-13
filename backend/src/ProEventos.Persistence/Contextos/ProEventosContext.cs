using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contextos;

public class ProEventosContext : DbContext
{
    public DbSet<Evento>? Eventos { get; set; }
    public DbSet<Lote>? Lotes { get; set; }
    public DbSet<Palestrante>? Palestrantes { get; set; }
    public DbSet<PalestranteEvento>? PalestrantesEventos { get; set; }
    public DbSet<RedeSocial>? RedesSociais { get; set; }

    public ProEventosContext(DbContextOptions<ProEventosContext> options) : base(options) {}

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={Path.Join(Environment.CurrentDirectory, "ProEventos.db")}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PalestranteEvento>()
            .HasKey(PE => new { PE.EventoId, PE.PalestranteId });

        modelBuilder.Entity<Evento>()
            .HasMany(e => e.RedesSociais)
            .WithOne(rs => rs.Evento)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Palestrante>()
            .HasMany(p => p.RedesSociais)
            .WithOne(rs => rs.Palestrante)
            .OnDelete(DeleteBehavior.Cascade);
    } 
}
