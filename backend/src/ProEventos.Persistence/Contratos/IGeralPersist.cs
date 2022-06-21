using System.Threading.Tasks;

namespace ProEventos.Persistence.Contratos
{
    public interface IGeralPersist
    {
        void Add<T>(T Entity) where T : class;
        void Update<T>(T Entity) where T : class;
        void Delete<T>(T Entity) where T : class;
        void DeleteRange<T>(T[] EntityArray) where T : class;
        Task<bool> SaveChangesAsync();
    }
}
