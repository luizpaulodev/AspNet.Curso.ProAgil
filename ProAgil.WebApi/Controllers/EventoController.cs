using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProAgil.WebApi.Model;

namespace ProAgil.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventoController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<EventoController> _logger;

        public EventoController(ILogger<EventoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Evento>> Get()
        {
            return new Evento[] {
                new Evento {
                    EventoID = 1,
                    Tema = "Angular",
                    Local = "BH",
                    Lote = "Primeiro Lote",
                    QtdPessoas = 250,
                    DataEvento = DateTime.Now.AddDays(2).ToShortDateString()
                },
                new Evento {
                    EventoID = 2,
                    Tema = "Angular",
                    Local = "SP",
                    Lote = "Segundo Lote",
                    QtdPessoas = 350,
                    DataEvento = DateTime.Now.AddDays(3).ToShortDateString()
                }
            };
        }

        [HttpGet("{id}")]
        public ActionResult<Evento> Get(int id)
        {
            return new Evento[] {
                new Evento {
                    EventoID = 1,
                    Tema = "Angular",
                    Local = "BH",
                    Lote = "Primeiro Lote",
                    QtdPessoas = 250,
                    DataEvento = DateTime.Now.AddDays(2).ToShortDateString()
                },
                new Evento {
                    EventoID = 2,
                    Tema = "Angular",
                    Local = "SP",
                    Lote = "Segundo Lote",
                    QtdPessoas = 350,
                    DataEvento = DateTime.Now.AddDays(3).ToShortDateString()
                }
            }.FirstOrDefault(x => x.EventoID == id);
        }
        
    }
}
