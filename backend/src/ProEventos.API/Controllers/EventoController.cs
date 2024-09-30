using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Models;

namespace ProEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventoController
{
    public IEnumerable<Evento> _eventos =
        [
            new Evento()
            {
                EventoId = 1,
                Tema = "Angular e .NET",
                Local = "São Paulo",
                Lote = "1º Lote",
                Qtdpessoas = 250,
                DataEvento = DateTime.Now.AddDays(2).ToString("dd/MM/yyyy"),
                ImagemURL = "foto1.png"
            },
            new Evento()
            {
                EventoId = 2,
                Tema = "Angular e .NET - parte 2",
                Local = "São Paulo",
                Lote = "2º Lote",
                Qtdpessoas = 300,
                DataEvento = DateTime.Now.AddDays(3).ToString("dd/MM/yyyy"),
                ImagemURL = "foto2.png"
            }
        ];

    [HttpGet]
    public IEnumerable<Evento> Get()
    {
        return _eventos;
    }

    [HttpGet("{id}")]
    public Evento GetById(int id)
    {
        return _eventos.FirstOrDefault(evento => evento.EventoId == id);
    }
}
