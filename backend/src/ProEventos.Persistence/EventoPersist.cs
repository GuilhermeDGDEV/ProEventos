using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;
using ProEventos.Persistence.Contextos;

namespace ProEventos.Persistence;

public class EventoPersist : IEventoPersist
{
    private readonly ProEventosContext _context;

    public EventoPersist(ProEventosContext context)
    {
        _context = context;
    }

    public async Task<Evento[]?> GetAllEventosAsync(bool includePalestrantes = false)
    {
        var query = _context.Eventos?
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais)
            .OrderBy(e => e.Id) as IQueryable<Evento>;

        if (includePalestrantes)
            query = query?.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante);

        return query != null ? await query.AsNoTracking().ToArrayAsync() : null;
    }

    public async Task<Evento[]?> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
    {
        var query = _context.Eventos?
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais)
            .OrderBy(e => e.Id) as IQueryable<Evento>;

        if (!string.IsNullOrWhiteSpace(tema))
            query = query?.Where(e => !string.IsNullOrWhiteSpace(e.Tema) && e.Tema.ToLower().Contains(tema.ToLower()));

        if (includePalestrantes)
            query = query?.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante);

        return query != null ? await query.AsNoTracking().ToArrayAsync() : null;
    }

    public async Task<Evento?> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
    {
        var query = _context.Eventos?
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais) as IQueryable<Evento>;

        if (includePalestrantes)
            query = query?.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante);

        return query != null ? await query.AsNoTracking().FirstOrDefaultAsync(e => e.Id == eventoId) : null;
    }
}