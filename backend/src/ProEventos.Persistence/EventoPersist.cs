using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;
using ProEventos.Persistence.Contextos;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Persistence
{
    public class EventoPersist : IEventoPersist
    {
        private readonly ProEventosContext _context;

        public EventoPersist(ProEventosContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        async Task<Evento> IEventoPersist.GetEventoById(int eventoId, bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais)
                .OrderBy(e => e.Id);

            if (includePalestrantes)
                query = query.Include(e => e.PalestranteEventos).ThenInclude(pe => pe.Palestrante);

            return await query.FirstOrDefaultAsync(e => e.Id == eventoId);
        }

        async Task<Evento[]> IEventoPersist.GetAllEventosAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais)
                .OrderBy(e => e.Id);

            if (includePalestrantes)
                query = query.Include(e => e.PalestranteEventos).ThenInclude(pe => pe.Palestrante);

            return await query.ToArrayAsync();
        }

        async Task<Evento[]> IEventoPersist.GetAllEventosByTemaAsync(string tema, bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais)
                .OrderBy(e => e.Id)
                .Where(e => e.Tema.ToLower().Contains(tema.ToLower()));

            if (includePalestrantes)
                query = query.Include(e => e.PalestranteEventos).ThenInclude(pe => pe.Palestrante);

            return await query.ToArrayAsync();
        }
    }
}
