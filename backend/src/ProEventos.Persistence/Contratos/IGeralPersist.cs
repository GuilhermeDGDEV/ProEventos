using ProEventos.Domain;

namespace ProEventos.Persistence.Contratos;

public interface IGeralPersist
{
    void Add<T>(T entity) where T: class;
    void Update<T>(T entity) where T: class;
    void Delete<T>(T entity) where T: class;
    void DeleteRange<T>(T[] entities) where T: class;
    Task<bool> SaveChangesAsync();
}