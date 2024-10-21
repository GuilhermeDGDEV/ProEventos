using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contratos;
using ProEventos.Domain;

namespace ProEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventosController(IEventoService eventoService) : ControllerBase
{
    private readonly IEventoService _eventoService = eventoService;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var eventos = await _eventoService.GetAllEventosAsync();
            if (eventos == null) return NotFound("Nenhum evento encontrado.");
            return Ok(eventos);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var evento = await _eventoService.GetEventoByIdAsync(id);
            if (evento == null) return NotFound("Nenhum evento encontrado.");
            return Ok(evento);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar evento. Erro: {ex.Message}");
        }
    }

    [HttpGet("{tema}/tema")]
    public async Task<IActionResult> GetByTema(string tema)
    {
        try
        {
            var eventos = await _eventoService.GetAllEventosByTemaAsync(tema);
            if (eventos == null) return NotFound("Nenhum evento buscado por tema encontrado.");
            return Ok(eventos);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post(Evento model)
    {
        try
        {
            var evento = await _eventoService.AddEventos(model);
            if (evento == null) return BadRequest("Erro ao tentar adicionar evento.");
            return Ok(evento);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar adicionar evento. Erro: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Evento model)
    {
        try
        {
            var evento = await _eventoService.UpdateEvento(id, model);
            if (evento == null) return BadRequest("Erro ao tentar atualizar evento.");
            return Ok(evento);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar atualizar evento. Erro: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            return await _eventoService.DeleteEvento(id) ?
                Ok("Deletado") :
                BadRequest("Evento não deletado");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar deletar evento. Erro: {ex.Message}");
        }
    }
}
