using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;

namespace ProEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LotesController : ControllerBase
{
    private readonly ILoteService _loteService;

    public LotesController(ILoteService loteService)
    {
        _loteService = loteService;
    }

    [HttpGet("{eventoId}")]   
    public async Task<IActionResult> Get(int eventoId)
    {
        try
        {
            var lotes = await _loteService.GetLotesByEventoIdAsync(eventoId);

            if (lotes == null || lotes.Length == 0) return NoContent();

            return Ok(lotes);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar lotes. Erro: {ex.Message}"
            );
        }
    }

    [HttpPut("{eventoId}")]
    public async Task<IActionResult> SaveLotes(int eventoId, LoteDto[] models)
    {
        try
        {
            var lotes = await _loteService.SaveLotes(eventoId, models);
            if (lotes == null) return BadRequest("Erro ao tentar atualizar lotes.");
            return Ok(lotes);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                $"Erro ao tentar atualizar lotes. Erro: {ex.Message}"
            );
        }
    }

    [HttpDelete("{eventoId}/{loteId}")]
    public async Task<IActionResult> Delete(int eventoId, int loteId)
    {
        try
        {
            var lote = await _loteService.GetLoteByIdsAsync(eventoId, loteId);
            if (lote == null) return NoContent();
            return await _loteService.DeleteLote(lote.EventoId ?? 0, lote.Id) ?
                Ok(new { message = "Deletado" }) : throw new Exception("Erro ao deletar lote.");
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                $"Erro ao tentar deletar lote. Erro: {ex.Message}"
            );
        }
    }
}
