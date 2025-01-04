using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;
using ProEventos.Persistence.Contextos;

namespace ProEventos.Persistence;

public class LotePersist(ProEventosContext context) : ILotePersist
{
    private readonly ProEventosContext _context = context;

    public async Task<Lote[]> GetLotesByEventoId(int eventoId)
    {
        return await _context.Lotes
            .Where(lote => lote.EventoId == eventoId)
            .AsNoTracking().ToArrayAsync();
    }

    public async Task<Lote> GetLoteByIds(int eventoId, int loteId)
    {
        return await _context.Lotes
            .FirstOrDefaultAsync(lote => lote.EventoId == eventoId && lote.Id == loteId);
    }
}