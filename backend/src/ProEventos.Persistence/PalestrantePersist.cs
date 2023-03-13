using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;
using ProEventos.Persistence.Contextos;

namespace ProEventos.Persistence;

public class PalestrantePersist : IPalestrantePersist
{
    private readonly ProEventosContext _context;

    public PalestrantePersist(ProEventosContext context)
    {
        _context = context;
    }

    public async Task<Palestrante[]?> GetAllPalestrantesAsync(bool includeEventos = false)
    {
        var query = _context.Palestrantes?
            .Include(e => e.RedesSociais)
            .OrderBy(e => e.Id) as IQueryable<Palestrante>;

        if (includeEventos)
            query = query?.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Evento);

        return query != null ? await query.AsNoTracking().ToArrayAsync() : null;
    }

    public async Task<Palestrante[]?> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos = false)
    {
        var query = _context.Palestrantes?
            .Include(e => e.RedesSociais)
            .OrderBy(e => e.Id) as IQueryable<Palestrante>;

        if (string.IsNullOrWhiteSpace(nome))
            query = query?.Where(e => !string.IsNullOrWhiteSpace(e.Nome) && e.Nome.Contains(nome));

        if (includeEventos)
            query = query?.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Evento);

        return query != null ? await query.AsNoTracking().ToArrayAsync() : null;
    }

    public async Task<Palestrante?> GetPalestranteByIdAsync(int palestranteId, bool includeEventos = false)
    {
        var query = _context.Palestrantes?
            .Include(e => e.RedesSociais) as IQueryable<Palestrante>;

        if (includeEventos)
            query = query?.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Evento);

        return query != null ? await query.AsNoTracking().FirstOrDefaultAsync(e => e.Id == palestranteId) : null;
    }
}