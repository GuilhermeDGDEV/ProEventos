using ProEventos.Persistence.Contratos;
using ProEventos.Persistence.Contextos;
using System.Threading.Tasks;

namespace ProEventos.Persistence
{
    public class GeralPersist : IGeralPersist
    {
        private readonly ProEventosContext _context;

        public GeralPersist(ProEventosContext context)
        {
            _context = context;
        }

        async void IGeralPersist.Add<T>(T Entity) where T : class
        {
            await _context.AddAsync(Entity);
        }

        void IGeralPersist.Update<T>(T Entity) where T : class
        {
            _context.Update(Entity);
        }

        void IGeralPersist.Delete<T>(T Entity) where T : class
        {
            _context.Remove(Entity);
        }

        void IGeralPersist.DeleteRange<T>(T[] EntityArray) where T : class
        {
            _context.RemoveRange(EntityArray);
        }

        async Task<bool> IGeralPersist.SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
