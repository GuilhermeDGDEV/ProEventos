using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application;

public class EventoService(IGeralPersist geralPersist, IEventoPersist eventoPersist, IMapper mapper) : IEventoService
{
    private readonly IGeralPersist _geralPersist = geralPersist;
    private readonly IEventoPersist _eventoPersist = eventoPersist;
    private readonly IMapper _mapper = mapper;

    public async Task<EventoDto> AddEventos(EventoDto model)
    {
        try
        {
            var evento = _mapper.Map<Evento>(model);
            _geralPersist.Add(evento);
            if (await _geralPersist.SaveChangesAsync())
            {
                var eventoRetorno = await _eventoPersist.GetEventoByIdAsync(evento.Id, false);
                return _mapper.Map<EventoDto>(eventoRetorno);
            }
            return null;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<EventoDto> UpdateEvento(int eventoId, EventoDto model)
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
                var eventoRetorno = await _eventoPersist.GetEventoByIdAsync(evento.Id, false);
                return _mapper.Map<EventoDto>(eventoRetorno);
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
                ?? throw new Exception("Evento não encontrado.");
            _geralPersist.Delete(evento);
            return await _geralPersist.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes = false)
    {
        try
        {
            var eventos = await _eventoPersist.GetAllEventosAsync(includePalestrantes);
            if (eventos == null) return null;
            return _mapper.Map<EventoDto[]>(eventos);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
    {
        try
        {
            var eventos = await _eventoPersist.GetAllEventosByTemaAsync(tema, includePalestrantes);
            if (eventos == null) return null;
            return _mapper.Map<EventoDto[]>(eventos);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<EventoDto> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
    {
        try
        {
            var evento = await _eventoPersist.GetEventoByIdAsync(eventoId, includePalestrantes);
            if (evento == null) return null;
            return _mapper.Map<EventoDto>(evento);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

}