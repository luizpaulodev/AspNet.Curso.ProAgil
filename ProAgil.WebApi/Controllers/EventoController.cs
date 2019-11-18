using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using ProAgil.Domain;
using ProAgil.Repository;
using ProAgil.WebApi.Dtos;

namespace ProAgil.WebApi.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class EventoController : ControllerBase
    {
        public IProAgilRepository _repo { get; }
        public IMapper _mapper { get; }

        public EventoController(IProAgilRepository repo, IMapper mapper)
        {
            this._mapper = mapper;
            this._repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await _repo.GetAllEventoAsync(true);
                var results = _mapper.Map<EventoDto[]>(eventos);

                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
        }

        [HttpPost("upload")]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                    var fullPath = Path.Combine(pathToSave, filename.ToString().Replace("\"", " ").Trim());

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                return Ok();
            }
            catch (System.Exception)
            {
                return BadRequest("Falha ao tentar realizar o upload!");
            }
        }

        [HttpGet("{EventoId}")]
        public async Task<IActionResult> GetById(int EventoId)
        {
            try
            {
                var evento = await _repo.GetEventoAsyncById(EventoId, true);
                var result = _mapper.Map<EventoDto>(evento);

                return Ok(result);
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
                var eventos = await _repo.GetAllEventoAsyncByTema(Tema);
                var results = _mapper.Map<EventoDto[]>(eventos);

                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);

                _repo.Add(evento);

                if (await _repo.SaveChangeAsync())
                {
                    return Created($"/v1/evento/{model.Id}", _mapper.Map<Evento>(model));
                }
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Post Banco de Dados Falhou: " + ex.Message);
            }

            return BadRequest("error");
        }

        [HttpPut("{EventoId}")]
        public async Task<IActionResult> Put(int EventoId, EventoDto model)
        {
            try
            {
                var idLotes = new List<int>();
                var idRedesSociais = new List<int>();

                model.Lotes.ForEach(x => idLotes.Add(x.Id));
                model.RedesSociais.ForEach(x => idRedesSociais.Add(x.Id));

                var evento = await _repo.GetEventoAsyncById(EventoId, false);
                if (evento == null)
                    return NotFound();

                var lotes = evento.Lotes.Where(
                    lote => !idLotes.Contains(lote.Id)
                ).ToArray<Lote>();  

                var redesSociais = evento.RedesSociais.Where(
                    redeSocial => !idRedesSociais.Contains(redeSocial.Id)
                ).ToArray<RedeSocial>();    

                if (lotes.Length > 0)
                    _repo.DeleteRange(lotes);

                if (redesSociais.Length > 0) 
                    _repo.DeleteRange(redesSociais);                    

                _mapper.Map(model, evento);
                _repo.Update(evento);

                if (await _repo.SaveChangeAsync())
                {
                    return Created($"/v1/evento/{model.Id}", _mapper.Map<Evento>(model));
                }
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou: " + ex.Message);
            }

            return BadRequest();
        }

        [HttpDelete("{EventoId}")]
        public async Task<IActionResult> Put(int EventoId)
        {
            try
            {
                var evento = await _repo.GetEventoAsyncById(EventoId, false);

                if (evento == null)
                    return NotFound();

                _repo.Delete(evento);

                if (await _repo.SaveChangeAsync())
                {
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

