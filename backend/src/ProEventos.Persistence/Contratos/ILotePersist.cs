using ProEventos.Domain;

namespace ProEventos.Persistence.Contratos;

public interface ILotePersist
{
    Task<Lote[]> GetLotesByEventoId(int eventoId);
    Task<Lote> GetLoteByIds(int eventoId, int loteId);
}