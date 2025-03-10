using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;
using ProEventos.Persistence.Contextos;

namespace ProEventos.Persistence;

public class EventoPersist(ProEventosContext context) : GeralPersist(context), IEventoPersist
{
    private readonly ProEventosContext _context = context;

    public async Task<Evento[]> GetAllEventosAsync(int userId, bool includePalestrantes = false)
    {
        IQueryable<Evento> query = _context.Eventos
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais)
            .OrderBy(e => e.Id)
            .AsNoTracking();

        if (includePalestrantes)
            query = query.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante);

        return await query.Where(e => e.UserId == userId).ToArrayAsync();
    }

    public async Task<Evento[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false)
    {
        IQueryable<Evento> query = _context.Eventos
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais)
            .Where(e => !string.IsNullOrWhiteSpace(tema) && e.Tema.ToLower().Contains(tema.ToLower())
                && e.UserId == userId)
            .OrderBy(e => e.Id)
            .AsNoTracking();

        if (includePalestrantes)
            query = query.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante);

        return await query.ToArrayAsync();
    }

    public async Task<Evento> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false)
    {
        IQueryable<Evento> query = _context.Eventos
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais)
            .AsNoTracking();

        if (includePalestrantes)
            query = query.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante);

        return await query.FirstOrDefaultAsync(e => e.Id == eventoId && e.UserId == userId);
    }
}