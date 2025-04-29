
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trackin.API.Domain.Entity;
using Trackin.API.DTOs;
using Trackin.API.Infrastructure.Context;

namespace Trackin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZonaPatioController : ControllerBase
    {
        private readonly TrackinContext _context;

        public ZonaPatioController(TrackinContext context)
        {
            _context = context;
        }

        // GET: api/ZonaPatio
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ZonaPatio>>> GetZonasPatio()
        {
            return await _context.ZonasPatio.ToListAsync();
        }

        // GET: api/ZonaPatio/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ZonaPatio>> GetZonaPatio(long id)
        {
            var zonaPatio = await _context.ZonasPatio.FindAsync(id);

            if (zonaPatio == null)
            {
                return NotFound();
            }

            return zonaPatio;
        }

        // PUT: api/ZonaPatio/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutZonaPatio(long id, ZonaPatio zonaPatio)
        {
            if (id != zonaPatio.Id)
            {
                return BadRequest();
            }

            _context.Entry(zonaPatio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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

            _context.ZonasPatio.Add(zonaPatio);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetZonaPatio), new { id = zonaPatio.Id }, zonaPatio);
        }


        // DELETE: api/ZonaPatio/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteZonaPatio(long id)
        {
            var zonaPatio = await _context.ZonasPatio.FindAsync(id);
            if (zonaPatio == null)
            {
                return NotFound();
            }

            _context.ZonasPatio.Remove(zonaPatio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ZonaPatioExists(long id)
        {
            return _context.ZonasPatio.Any(e => e.Id == id);
        }
    }
}
