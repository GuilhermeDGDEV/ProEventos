using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application;

public class LoteService(ILotePersist lotePersist, IMapper mapper) : ILoteService
{
    private readonly ILotePersist _lotePersist = lotePersist;
    private readonly IMapper _mapper = mapper;

    public async Task<bool> DeleteLote(int eventoId, int loteId)
    {
        try
        {
            var lote = await _lotePersist.GetLoteByIds(eventoId, loteId) 
                ?? throw new Exception("Lote não encontrado.");
            _lotePersist.Delete(lote);
            return await _lotePersist.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<LoteDto> GetLoteByIdsAsync(int eventoId, int loteId)
    {
        try
        {
            var lote = await _lotePersist.GetLoteByIds(eventoId, loteId);
            if (lote == null) return null;
            return _mapper.Map<LoteDto>(lote);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId)
    {
        try
        {
            var lotes = await _lotePersist.GetLotesByEventoId(eventoId);
            if (lotes == null) return null;
            return _mapper.Map<LoteDto[]>(lotes);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<LoteDto[]> SaveLotes(int eventoId, LoteDto[] models)
    {
        try
        {
            var lotes = await _lotePersist.GetLotesByEventoId(eventoId);
            if (lotes == null) return null;

            foreach (var model in models) 
            {
                if (model.Id == 0)
                {
                    await AddLotes(eventoId, model);
                }
                else
                {
                    await UpdateLote(eventoId, model, lotes);
                }
            }
            var lotesRetorno = await _lotePersist.GetLotesByEventoId(eventoId);
            return _mapper.Map<LoteDto[]>(lotesRetorno);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private async Task AddLotes(int eventoId, LoteDto model)
    {
        try
        {
            var lote = _mapper.Map<Lote>(model);
            lote.EventoId = eventoId;
            _lotePersist.Add(lote);
            await _lotePersist.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private async Task UpdateLote(int eventoId, LoteDto model, Lote[] lotes)
    {
        try
        {
            var lote = lotes.FirstOrDefault(l => l.Id == model.Id);
            model.EventoId = eventoId;
            _mapper.Map(model, lote);
            _lotePersist.Update(lote);
            await _lotePersist.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}