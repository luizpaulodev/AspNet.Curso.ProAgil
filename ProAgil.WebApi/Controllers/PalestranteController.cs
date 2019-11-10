using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.WebApi.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class PalestranteController : ControllerBase
    {
        public IProAgilRepository Repo { get; }
        public PalestranteController(IProAgilRepository repo)
        {
            this.Repo = repo;
        }

        [HttpGet("{PalestranteId}")]
        public async Task<IActionResult> GetById(int PalestranteId)
        {
            try
            {
                return Ok(await Repo.GetPalestranteAsyncById(PalestranteId));
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
        }

        [HttpGet("{Nome}")]
        public async Task<IActionResult> GetByName(string Nome)
        {
            try
            {
                return Ok(await Repo.GetAllPalestrantesAsyncByName(Nome));
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Evento model)
        {
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

        [HttpPut("{PalestranteId}")]
        public async Task<IActionResult> Put(int PalestranteId, Palestrante model)
        {
            try
            {
                var palestrante = await Repo.GetPalestranteAsyncById(PalestranteId, false);

                if(palestrante == null)
                    return NotFound();

                Repo.Update(model);

                if(await Repo.SaveChangeAsync()){
                    return Created($"/v1/[palestrante]/{model.Id}", model);
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }

            return BadRequest();
        }

        [HttpDelete("{PalestranteId}")]
        public async Task<IActionResult> Put(int PalestranteId)
        {
            try
            {
                var palestrante = await Repo.GetPalestranteAsyncById(PalestranteId, false);

                if(palestrante == null)
                    return NotFound();

                Repo.Delete(palestrante);

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