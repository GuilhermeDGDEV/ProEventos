using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;
using ProEventos.Persistence.Contextos;

namespace ProEventos.Persistence;

public class EventoPersist(ProEventosContext context) : IEventoPersist
{
    private readonly ProEventosContext _context = context;

    public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
    {
        IQueryable<Evento> query = _context.Eventos
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais)
            .OrderBy(e => e.Id)
            .AsNoTracking();

        if (includePalestrantes)
            query = query.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante);

        return await query.ToArrayAsync();
    }

    public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
    {
        IQueryable<Evento> query = _context.Eventos
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais)
            .Where(e => e.Tema.ToLower().Contains(tema.ToLower()))
            .OrderBy(e => e.Id)
            .AsNoTracking();

        if (includePalestrantes)
            query = query.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante);

        return await query.ToArrayAsync();
    }

    public async Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
    {
        IQueryable<Evento> query = _context.Eventos
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais)
            .AsNoTracking();

        if (includePalestrantes)
            query = query.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante);

        return await query.FirstOrDefaultAsync(e => e.Id == eventoId);
    }
}