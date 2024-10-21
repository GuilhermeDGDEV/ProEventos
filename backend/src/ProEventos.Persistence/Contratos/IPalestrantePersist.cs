using ProEventos.Domain;

namespace ProEventos.Persistence.Contratos;

public interface IPalestrantePersist
{
    Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos = false);
    Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos = false);
    Task<Palestrante> GetPalestranteByIdAsync(int palestranteId, bool includeEventos = false);
}