using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Persistence.Contratos;
using AutoMapper;
using ProEventos.Domain;

namespace ProEventos.Application;

public class LoteService : ILoteService
{
    private readonly IGeralPersist _geralPersist;
    private readonly ILotePersist _lotePersist;
    private readonly IMapper _mapper;

    public LoteService(IGeralPersist geralPersist, ILotePersist lotePersist, IMapper mapper)
    {
        _geralPersist = geralPersist;
        _lotePersist = lotePersist;
        _mapper = mapper;
    }

    public async Task<bool> DeleteLote(int eventoId, int loteId)
    {
        try
        {
            var lote = await _lotePersist.GetLoteByIdsAsync(eventoId, loteId) 
                ?? throw new Exception("Lote para delete não encontrado!");

            _geralPersist.Delete(lote);
            return await _geralPersist.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<LoteDto?> GetLoteByIdsAsync(int eventoId, int loteId)
    {
        try
        {
            var lote = await _lotePersist.GetLoteByIdsAsync(eventoId, loteId);
            return _mapper.Map<LoteDto>(lote);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<LoteDto[]?> GetLotesByEventoIdAsync(int eventoId)
    {
        try
        {
            var lotes = await _lotePersist.GetLotesByEventoId(eventoId);
            return _mapper.Map<LoteDto[]>(lotes);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<LoteDto[]?> SaveLotes(int eventoId, LoteDto[] models)
    {
        try
        {
            var lotes = await _lotePersist.GetLotesByEventoId(eventoId);

            foreach (var model in models)
            {
                if (model.Id == 0)
                {
                    await AddLote(eventoId, model);
                }
                else
                {
                    var lote = lotes?.FirstOrDefault(lote => lote.Id == model.Id);
                    if (lote != null) await UpdateLote(eventoId, model, lote);               
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

    public async Task AddLote(int eventoId, LoteDto model)
    {
        try
        {
            var lote = _mapper.Map<Lote>(model);
            lote.EventoId = eventoId;
            _geralPersist.Add(lote);
            await _geralPersist.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task UpdateLote(int eventoId, LoteDto model, Lote lote)
    {
        try
        {
            model.EventoId = eventoId;
            _mapper.Map(model, lote);
            _geralPersist.Update(lote);
            await _geralPersist.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}