
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trackin.API.Domain.Entity;
using Trackin.API.DTOs;
using Trackin.API.Infrastructure.Context;
using Trackin.API.Infrastructure.Persistence.Repositories;

namespace Trackin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZonaPatioController : ControllerBase
    {
        private readonly IZonaPatioRepository _zonaPatioRepository;

        public ZonaPatioController(IZonaPatioRepository zonaPatioRepository)
        {
            _zonaPatioRepository = zonaPatioRepository;
        }

        // GET: api/ZonaPatio
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ZonaPatio>>> GetZonasPatio()
        {
            IEnumerable<ZonaPatio> zonasPatio = await _zonaPatioRepository.GetAllAsync();
            if (zonasPatio == null || !zonasPatio.Any())
            {
                return NotFound("Nenhuma zona de pátio encontrada.");
            }
            return Ok(zonasPatio);
        }

        // GET: api/ZonaPatio/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ZonaPatio>> GetZonaPatio(long id)
        {
            var zonaPatio = await _zonaPatioRepository.GetByIdAsync(id);

            if (zonaPatio == null)
            {
                return NotFound();
            }

            return zonaPatio;
        }

        // PUT: api/ZonaPatio/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutZonaPatio(long id, CriarZonaPatioDTO zonaPatioDto)
        {
            var zonaPatio = await _zonaPatioRepository.GetByIdAsync(id);
            if (zonaPatio == null)
            {
                return NotFound();
            }

            zonaPatio.PatioId = zonaPatioDto.PatioId;
            zonaPatio.Nome = zonaPatioDto.Nome;
            zonaPatio.TipoZona = zonaPatioDto.TipoZona;
            zonaPatio.CoordenadaInicialX = zonaPatioDto.CoordenadaInicialX;
            zonaPatio.CoordenadaInicialY = zonaPatioDto.CoordenadaInicialY;
            zonaPatio.CoordenadaFinalX = zonaPatioDto.CoordenadaFinalX;
            zonaPatio.CoordenadaFinalY = zonaPatioDto.CoordenadaFinalY;
            zonaPatio.Cor = zonaPatioDto.Cor;

            try
            {
                await _zonaPatioRepository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ZonaPatioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ZonaPatio
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> CriarZonaPatio([FromBody] CriarZonaPatioDTO dto)
        {
            var zonaPatio = new ZonaPatio
            {
                Nome = dto.Nome,
                TipoZona = dto.TipoZona,
                CoordenadaInicialX = dto.CoordenadaInicialX,
                CoordenadaInicialY = dto.CoordenadaInicialY,
                CoordenadaFinalX = dto.CoordenadaFinalX,
                CoordenadaFinalY = dto.CoordenadaFinalY,
                Cor = dto.Cor,
                PatioId = dto.PatioId
            };

            await _zonaPatioRepository.AddAsync(zonaPatio);
            await _zonaPatioRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetZonaPatio), new { id = zonaPatio.Id }, zonaPatio);
        }


        // DELETE: api/ZonaPatio/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteZonaPatio(long id)
        {
            var zonaPatio = await _zonaPatioRepository.GetByIdAsync(id);
            if (zonaPatio == null)
            {
                return NotFound();
            }

            await _zonaPatioRepository.RemoveAsync(zonaPatio);
            await _zonaPatioRepository.SaveChangesAsync();

            return NoContent();
        }

        private bool ZonaPatioExists(long id)
        {
            return (_zonaPatioRepository.GetAllAsync().Result.Any(e => e.Id == id));
        }
    }
}
