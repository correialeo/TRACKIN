
using Microsoft.AspNetCore.Mvc;
using Trackin.API.Domain.Entity;
using Trackin.API.DTOs;
using Trackin.API.Infrastructure.Persistence.Repositories;

namespace Trackin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatioController : ControllerBase
    {
        private readonly IPatioRepository _patioRepository;

        public PatioController(IPatioRepository patioRepository)
        {
            _patioRepository = patioRepository;
        }

        // GET: api/Patio
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patio>>> GetPatios()
        {
            IEnumerable<Patio> patios = await _patioRepository.GetAllAsync();
            if (patios == null || !patios.Any())
            {
                return NotFound("Nenhum pátio encontrado.");
            }
            return Ok(patios);
        }

        // GET: api/Patio/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Patio>> GetPatio(long id)
        {
            var patio = await _patioRepository.GetByIdAsync(id);

            if (patio == null)
            {
                return NotFound();
            }

            return patio;
        }


        // POST: api/Patio
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> CriarPatio([FromBody] CriarPatioDto dto)
        {
            var patio = new Patio
            {
                Nome = dto.Nome,
                Endereco = dto.Endereco,
                Cidade = dto.Cidade,
                Estado = dto.Estado,
                Pais = dto.Pais,
                DimensaoX = dto.DimensaoX,
                DimensaoY = dto.DimensaoY,
                PlantaBaixa = dto.PlantaBaixa,
            };

            await _patioRepository.AddAsync(patio);
            await _patioRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPatio), new { id = patio.Id }, patio);
        }

        // DELETE: api/Patio/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatio(long id)
        {
            var patio = await _patioRepository.GetByIdAsync(id);
            if (patio == null)
            {
                return NotFound();
            }

            await _patioRepository.RemoveAsync(patio);
            await _patioRepository.SaveChangesAsync();

            return NoContent();
        }

        private bool PatioExists(long id)
        {
            return (_patioRepository.GetAllAsync().Result.Any(e => e.Id == id));
        }
    }
}
