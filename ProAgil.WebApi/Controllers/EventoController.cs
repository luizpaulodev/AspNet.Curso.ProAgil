using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProAgil.WebApi.Data;
using ProAgil.WebApi.Model;

namespace ProAgil.WebApi.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class EventoController : ControllerBase
    {
        public DataContext Context { get; }

        public EventoController(DataContext context)
        {
            this.Context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Evento>> Get()
        {
            return Context.Eventos.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Evento> Get(int id)
        {
            return Context.Eventos.FirstOrDefault(x => x.EventoID == id);
        }
    }
}
