using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application;

public class EventoService(IEventoPersist eventoPersist, IMapper mapper) : IEventoService
{
    private readonly IEventoPersist _eventoPersist = eventoPersist;
    private readonly IMapper _mapper = mapper;

    public async Task<EventoDto> AddEventos(int userId, EventoDto model)
    {
        try
        {
            var evento = _mapper.Map<Evento>(model);
            evento.UserId = userId;
            _eventoPersist.Add(evento);
            if (await _eventoPersist.SaveChangesAsync())
            {
                var eventoRetorno = await _eventoPersist.GetEventoByIdAsync(userId, evento.Id, false);
                return _mapper.Map<EventoDto>(eventoRetorno);
            }
            return null;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto model)
    {
        try
        {
            var evento = await _eventoPersist.GetEventoByIdAsync(userId, eventoId, false);
            if (evento == null) return null;

            model.Id = evento.Id;
            model.UserId = userId;

            _mapper.Map(model, evento);

            _eventoPersist.Update(evento);

            if (await _eventoPersist.SaveChangesAsync())
            {
                var eventoRetorno = await _eventoPersist.GetEventoByIdAsync(userId, evento.Id, false);
                return _mapper.Map<EventoDto>(eventoRetorno);
            }
            return null;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> DeleteEvento(int userId, int eventoId)
    {
        try
        {
            var evento = await _eventoPersist.GetEventoByIdAsync(userId, eventoId, false)
                ?? throw new Exception("Evento não encontrado.");
            _eventoPersist.Delete(evento);
            return await _eventoPersist.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<EventoDto[]> GetAllEventosAsync(int userId, bool includePalestrantes = false)
    {
        try
        {
            var eventos = await _eventoPersist.GetAllEventosAsync(userId, includePalestrantes);
            if (eventos == null) return null;
            return _mapper.Map<EventoDto[]>(eventos);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<EventoDto[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false)
    {
        try
        {
            var eventos = await _eventoPersist.GetAllEventosByTemaAsync(userId, tema, includePalestrantes);
            if (eventos == null) return null;
            return _mapper.Map<EventoDto[]>(eventos);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<EventoDto> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false)
    {
        try
        {
            var evento = await _eventoPersist.GetEventoByIdAsync(userId, eventoId, includePalestrantes);
            if (evento == null) return null;
            return _mapper.Map<EventoDto>(evento);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

}