using ProEventos.Application.Contratos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application;

public class EventoService : IEventoService
{
    public readonly IGeralPersist _geralPersist;
    public readonly IEventoPersist _eventoPersist;

    public EventoService(IGeralPersist geralPersist, IEventoPersist eventoPersist)
    {
        _geralPersist = geralPersist;
        _eventoPersist = eventoPersist;
    }

    public async Task<Evento?> AddEventos(Evento model)
    {
        try
        {
            _geralPersist.Add<Evento>(model);
            if (await _geralPersist.SaveChangesAsync())
                return await _eventoPersist.GetEventoByIdAsync(model.Id, false);

            return null;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Evento?> UpdateEvento(int eventoId, Evento model)
    {
        try
        {
            var evento = await _eventoPersist.GetEventoByIdAsync(eventoId, false);
            if (evento == null) return null;

            model.Id = evento.Id;

            _geralPersist.Update<Evento>(model);
            if (await _geralPersist.SaveChangesAsync())
                return await _eventoPersist.GetEventoByIdAsync(model.Id, false);

            return null;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> DeleteEvento(int eventoId)
    {
        try
        {
            var evento = await _eventoPersist.GetEventoByIdAsync(eventoId, false);
            if (evento == null) throw new Exception("Evento a ser deletado não foi encontrado!");

            _geralPersist.Delete<Evento>(evento);
            return await _geralPersist.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Evento[]?> GetAllEventosAsync(bool includePalestrantes)
    {
        try
        {
            return await _eventoPersist.GetAllEventosAsync(includePalestrantes);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Evento[]?> GetAllEventosByTemaAsync(string tema, bool includePalestrantes)
    {
        try
        {
            return await _eventoPersist.GetAllEventosByTemaAsync(tema, includePalestrantes);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Evento?> GetEventoByIdAsync(int eventoId, bool includePalestrantes)
    {
        return await _eventoPersist.GetEventoByIdAsync(eventoId, includePalestrantes);
    }
    
}