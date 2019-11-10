﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.WebApi.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class EventoController : ControllerBase
    {
        public IProAgilRepository Repo { get; }

        public EventoController(IProAgilRepository repo)
        {
            this.Repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await Repo.GetAllEventoAsync());
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
        }

        [HttpGet("{EventoId}")]
        public async Task<IActionResult> GetById(int EventoId)
        {
            try
            {
                return Ok(await Repo.GetEventoAsyncById(EventoId));
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
        }

        [HttpGet("tema/{Tema}")]
        public async Task<IActionResult> GetByTema(string Tema)
        {
            try
            {
                return Ok(await Repo.GetAllEventoAsyncByTema(Tema));
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Evento model)
        {
            Console.WriteLine("Task<IActionResult> Post");

            try
            {
                Repo.Add(model);

                if(await Repo.SaveChangeAsync()){
                    return Created($"/v1/evento/{model.Id}", model);
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }

            return BadRequest("error");
        }

        [HttpPut("{EventoId}")]
        public async Task<IActionResult> Put(int EventoId, Evento model)
        {
            try
            {
                var evento = await Repo.GetEventoAsyncById(EventoId, false);

                if(evento == null)
                    return NotFound();

                Repo.Update(model);

                if(await Repo.SaveChangeAsync()){
                    return Created($"/v1/evento/{model.Id}", model);
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }

            return BadRequest();
        }

        [HttpDelete("{EventoId}")]
        public async Task<IActionResult> Put(int EventoId)
        {
            try
            {
                var evento = await Repo.GetEventoAsyncById(EventoId, false);

                if(evento == null)
                    return NotFound();

                Repo.Delete(evento);

                if(await Repo.SaveChangeAsync()){
                    return Ok();
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }

            return BadRequest();
        }
    }
}

