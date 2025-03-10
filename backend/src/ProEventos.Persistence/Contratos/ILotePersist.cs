using ProEventos.Domain;

namespace ProEventos.Persistence.Contratos;

public interface ILotePersist : IGeralPersist
{
    Task<Lote[]> GetLotesByEventoId(int eventoId);
    Task<Lote> GetLoteByIds(int eventoId, int loteId);
}