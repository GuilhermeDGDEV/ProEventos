using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;
using ProEventos.Persistence.Contextos;

namespace ProEventos.Persistence;

public class LotePersist : ILotePersist
{
    private readonly ProEventosContext _context;

    public LotePersist(ProEventosContext context)
    {
        _context = context;
    }

    public async Task<Lote[]?> GetLotesByEventoId(int eventoId)
    {
        return _context.Lotes?.Where(l => l.EventoId == eventoId) is IQueryable<Lote> query
            ? await query.AsNoTracking().ToArrayAsync() : null;
    }

    public async Task<Lote?> GetLoteByIdsAsync(int eventoId, int loteId)
    {
        var lote = _context.Lotes?.FirstOrDefaultAsync(l => l.EventoId == eventoId && l.Id == loteId);
        return lote != null ? await lote : null;
    }
}