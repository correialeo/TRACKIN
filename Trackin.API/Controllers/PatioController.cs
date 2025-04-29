using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trackin.API.Domain.Entity;
using Trackin.API.DTOs;
using Trackin.API.Infrastructure.Context;

namespace Trackin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatioController : ControllerBase
    {
        private readonly TrackinContext _context;

        public PatioController(TrackinContext context)
        {
            _context = context;
        }

        // GET: api/Patio
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patio>>> GetPatios()
        {
            return await _context.Patios.ToListAsync();
        }

        // GET: api/Patio/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Patio>> GetPatio(long id)
        {
            var patio = await _context.Patios.FindAsync(id);

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

            _context.Patios.Add(patio);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPatio), new { id = patio.Id }, patio);
        }

        // DELETE: api/Patio/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatio(long id)
        {
            var patio = await _context.Patios.FindAsync(id);
            if (patio == null)
            {
                return NotFound();
            }

            _context.Patios.Remove(patio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatioExists(long id)
        {
            return _context.Patios.Any(e => e.Id == id);
        }
    }
}
