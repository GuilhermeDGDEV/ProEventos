using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Persistence.Contratos;
using ProEventos.Domain;
using AutoMapper;

namespace ProEventos.Application;

public class EventoService : IEventoService
{
    private readonly IGeralPersist _geralPersist;
    private readonly IEventoPersist _eventoPersist;
    private readonly IMapper _mapper;

    public EventoService(IGeralPersist geralPersist, IEventoPersist eventoPersist, IMapper mapper)
    {
        _geralPersist = geralPersist;
        _eventoPersist = eventoPersist;
        _mapper = mapper;
    }

    public async Task<EventoDto?> AddEventos(EventoDto model)
    {
        try
        {
            var evento = _mapper.Map<Evento>(model);

            _geralPersist.Add(evento);
            if (await _geralPersist.SaveChangesAsync())
            {
                var retorno = await _eventoPersist.GetEventoByIdAsync(evento.Id, false);
                return _mapper.Map<EventoDto>(retorno);
            }

            return null;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<EventoDto?> UpdateEvento(int eventoId, EventoDto model)
    {
        try
        {
            var evento = await _eventoPersist.GetEventoByIdAsync(eventoId, false);
            if (evento == null) return null;

            model.Id = evento.Id;

            _mapper.Map(model, evento);

            _geralPersist.Update(evento);
            if (await _geralPersist.SaveChangesAsync())
            {
                var retorno = await _eventoPersist.GetEventoByIdAsync(evento.Id, false);
                return _mapper.Map<EventoDto>(retorno);
            }

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
            var evento = await _eventoPersist.GetEventoByIdAsync(eventoId, false) 
                ?? throw new Exception("Evento a ser deletado não foi encontrado!");
                
            _geralPersist.Delete(evento);
            return await _geralPersist.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<EventoDto[]?> GetAllEventosAsync(bool includePalestrantes)
    {
        try
        {
            var eventos = await _eventoPersist.GetAllEventosAsync(includePalestrantes);
            return _mapper.Map<EventoDto[]>(eventos);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<EventoDto[]?> GetAllEventosByTemaAsync(string tema, bool includePalestrantes)
    {
        try
        {
            var eventos = await _eventoPersist.GetAllEventosByTemaAsync(tema, includePalestrantes);
            return _mapper.Map<EventoDto[]>(eventos);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<EventoDto?> GetEventoByIdAsync(int eventoId, bool includePalestrantes)
    {
        try
        {
            var evento = await _eventoPersist.GetEventoByIdAsync(eventoId, includePalestrantes);
            return _mapper.Map<EventoDto>(evento);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    
}