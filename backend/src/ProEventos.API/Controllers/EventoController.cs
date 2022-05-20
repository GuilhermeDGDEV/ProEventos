using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.API.Models;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {

        public IEnumerable<Evento> _eventos = new Evento[] {
                new Evento {
                    EventoId = 1,
                    Tema = "Criando REST API",
                    Local = "São Paulo",
                    Lote = "1º Lote",
                    QtdPessoas = 250,
                    DataEvento = DateTime.Now.AddDays(2).ToString(),
                    ImagemURL = "foto.png"
                },
                new Evento {
                    EventoId = 2,
                    Tema = "Criando Front End",
                    Local = "São Paulo",
                    Lote = "2º Lote",
                    QtdPessoas = 350,
                    DataEvento = DateTime.Now.AddDays(3).ToString(),
                    ImagemURL = "foto.png"
                }
            };

        public EventoController() { }

        [HttpGet]
        public IEnumerable<Evento> Get()
        {
            return _eventos;
        }

        [HttpGet("{id}")]
        public Evento GetById(int id)
        {
            return _eventos.Where(evento => evento.EventoId == id).FirstOrDefault();
        }
        
    }
}
