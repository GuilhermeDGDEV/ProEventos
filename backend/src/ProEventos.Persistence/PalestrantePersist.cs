using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;
using ProEventos.Persistence.Contextos;

namespace ProEventos.Persistence;

public class PalestrantePersist(ProEventosContext context) : IPalestrantePersist
{
    private readonly ProEventosContext _context = context;

    public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos = false)
    {
        IQueryable<Palestrante> query = _context.Palestrantes
            .Include(p => p.RedesSociais)
            .OrderBy(p => p.Id);

        if (includeEventos)
            query = query.Include(p => p.PalestrantesEventos).ThenInclude(pe => pe.Evento);

        return await query.ToArrayAsync();
    }

    public async Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos = false)
    {
        IQueryable<Palestrante> query = _context.Palestrantes
            .Include(p => p.RedesSociais)
            .Where(p => p.Nome.ToLower().Contains(nome.ToLower()))
            .OrderBy(p => p.Id);

        if (includeEventos)
            query = query.Include(p => p.PalestrantesEventos).ThenInclude(pe => pe.Evento);

        return await query.ToArrayAsync();
    }

    public async Task<Palestrante> GetPalestranteByIdAsync(int palestranteId, bool includeEventos = false)
    {
        IQueryable<Palestrante> query = _context.Palestrantes
            .Include(p => p.RedesSociais)
            .OrderBy(p => p.Id);

        if (includeEventos)
            query = query.Include(p => p.PalestrantesEventos).ThenInclude(pe => pe.Evento);

        return await query.FirstOrDefaultAsync(p => p.Id == palestranteId);
    }
}