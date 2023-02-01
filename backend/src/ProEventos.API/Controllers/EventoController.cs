using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Models;

namespace ProEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventoController : ControllerBase
{
    public IEnumerable<Evento> _eventos = new Evento[]
        {
            new Evento()
            {
                EventoId = 1,
                Tema = "Angular 11 e .NET 6",
                Local = "São Paulo",
                Lote = "1º Lote",
                QtdPessoas = 250,
                DateEvento = DateTime.Now.AddDays(250).ToString(),
                ImagemURL = "foto.png"
            },
            new Evento()
            {
                EventoId = 2,
                Tema = "Angular 11 e .NET 6",
                Local = "São Paulo",
                Lote = "1º Lote",
                QtdPessoas = 250,
                DateEvento = DateTime.Now.AddDays(250).ToString(),
                ImagemURL = "foto.png"
            },
        };

    public EventoController()
    { 
    }

    [HttpGet]
    public IEnumerable<Evento> Get()
    {
        return _eventos;
    }

    [HttpGet("{id}")]
    public Evento? GetById(int id)
    {
        return _eventos.FirstOrDefault(evento => evento.EventoId == id);
    }

    [HttpPost]
    public string Post()
    {
        return "Exemplo Post";
    }

    [HttpPut("{id}")]
    public string Put(int id)
    {
        return $"Exemplo Put com id = {id}";
    }

    [HttpDelete("{id}")]
    public string Delete(int id)
    {
        return $"Exemplo Delete com id = {id}";
    }
}
