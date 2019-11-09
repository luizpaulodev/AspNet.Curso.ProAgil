using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProAgil.Repository;

namespace ProAgil.WebApi.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class EventoController : ControllerBase
    {
        public ProAgilContext context { get; }

        public EventoController(ProAgilContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await context.Eventos.ToListAsync();

                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            } 
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var results = await context.Eventos.FirstOrDefaultAsync(x => x.Id == id);

                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
        }
    }
}
