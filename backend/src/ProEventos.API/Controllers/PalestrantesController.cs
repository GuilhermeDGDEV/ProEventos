using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contratos;
using ProEventos.Domain;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PalestrantesController : ControllerBase
    {
        private readonly IPalestranteService _service;

        public PalestrantesController(IPalestranteService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var palestrantes = await _service.GetAllPalestrantesAsync(false);
                return palestrantes != null ? Ok(palestrantes) : NotFound("Nenhum palestrante encontrado.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao recuperar palestrantes. Erro: {ex.Message}");
            }  
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var palestrante = await _service.GetPalestranteById(id, false);
                return palestrante != null ? Ok(palestrante) : NotFound("Nenhum palestrante encontrado.");
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao recuperar palestrante. Erro: {ex.Message}");
            }
        }

        [HttpGet("nome/{nome}")]
        public async Task<IActionResult> GetByNome(string nome)
        {
            try
            {
                var palestrantes = await _service.GetAllPalestrantesByNomeAsync(nome, false);
                return palestrantes != null ? Ok(palestrantes) : NotFound("Nenhum palestrante encontrado.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao recuperar palestrantes. Erro: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Palestrante model)
        {
            try
            {
                var palestrante = await _service.AddPalestrante(model);
                return palestrante != null ? Ok(palestrante) : BadRequest("Erro ao tentar adicionar palestrante.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao adicionar palestrante. Erro: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Palestrante model)
        {
            try
            {
                var palestrante = await _service.UpdatePalestrante(id, model);
                return palestrante != null ? Ok(palestrante) : BadRequest("Erro ao tentar atualizar palestrante.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao atualizar palestrante. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                return await _service.DeletePalestrante(id) ? Ok("Deletado") : BadRequest("Palestrante não deletado");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao deletar palestrante. Erro: {ex.Message}");
            }
        }

    }
}
