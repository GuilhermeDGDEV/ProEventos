using ProEventos.Application.Contratos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application;

public class EventoService(IGeralPersist geralPersist, IEventoPersist eventoPersist) : IEventoService
{
    private readonly IGeralPersist _geralPersist = geralPersist;
    private readonly IEventoPersist _eventoPersist = eventoPersist;

    public async Task<Evento> AddEventos(Evento model)
    {
        try
        {
            _geralPersist.Add(model);
            return await _geralPersist.SaveChangesAsync() ?
                await _eventoPersist.GetEventoByIdAsync(model.Id, false) : null;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Evento> UpdateEvento(int eventoId, Evento model)
    {
        try
        {
            var evento = await _eventoPersist.GetEventoByIdAsync(eventoId, false);
            if (evento == null) return null;

            model.Id = evento.Id;

            _geralPersist.Update(model);
            return await _geralPersist.SaveChangesAsync() ?
                await _eventoPersist.GetEventoByIdAsync(model.Id, false) : null;
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
            var evento = await _eventoPersist.GetEventoByIdAsync(eventoId, false)
                ?? throw new Exception("Evento não encontrado.");
            _geralPersist.Delete(evento);
            return await _geralPersist.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
    {
        try
        {
            var eventos = await _eventoPersist.GetAllEventosAsync(includePalestrantes);
            return eventos ?? null;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
    {
        try
        {
            var eventos = await _eventoPersist.GetAllEventosByTemaAsync(tema, includePalestrantes);
            return eventos ?? null;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
    {
        try
        {
            var evento = await _eventoPersist.GetEventoByIdAsync(eventoId, includePalestrantes);
            return evento ?? null;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

}