using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;
using ProEventos.Persistence.Contextos;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Persistence
{
    public class PalestrantePersist : IPalestrantePersist
    {
        private readonly ProEventosContext _context;

        public PalestrantePersist(ProEventosContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        async Task<Palestrante> IPalestrantePersist.GetPalestranteById(int palestranteId, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(p => p.RedeSociais)
                .OrderBy(p => p.Id);

            if (includeEventos)
                query = query.Include(p => p.PalestranteEventos).ThenInclude(pe => pe.Evento);

            return await query.FirstOrDefaultAsync(p => p.Id == palestranteId);
        }

        async Task<Palestrante[]> IPalestrantePersist.GetAllPalestrantesAsync(bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(p => p.RedeSociais)
                .OrderBy(p => p.Id);

            if (includeEventos)
                query = query.Include(p => p.PalestranteEventos).ThenInclude(pe => pe.Evento);

            return await query.ToArrayAsync();
        }

        async Task<Palestrante[]> IPalestrantePersist.GetAllPalestrantesByNomeAsync(string nome, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(p => p.RedeSociais)
                .OrderBy(p => p.Id)
                .Where(p => p.Nome.ToLower().Contains(nome.ToLower()));

            if (includeEventos)
                query = query.Include(p => p.PalestranteEventos).ThenInclude(pe => pe.Evento);

            return await query.ToArrayAsync();
        }
        
    }
}
