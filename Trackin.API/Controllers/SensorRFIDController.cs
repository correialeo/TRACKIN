
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trackin.API.Domain.Entity;
using Trackin.API.DTOs;
using Trackin.API.Infrastructure.Context;

namespace Trackin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorRFIDController : ControllerBase
    {
        private readonly TrackinContext _context;

        public SensorRFIDController(TrackinContext context)
        {
            _context = context;
        }

        // GET: api/SensorRFID
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SensorRFID>>> GetSensoresRFID()
        {
            return await _context.SensoresRFID.ToListAsync();
        }

        // GET: api/SensorRFID/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SensorRFID>> GetSensorRFID(long id)
        {
            var sensorRFID = await _context.SensoresRFID.FindAsync(id);

            if (sensorRFID == null)
            {
                return NotFound();
            }

            return sensorRFID;
        }

        // PUT: api/SensorRFID/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSensorRFID(long id, SensorRFID sensorRFID)
        {
            if (id != sensorRFID.Id)
            {
                return BadRequest();
            }

            _context.Entry(sensorRFID).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SensorRFIDExists(id))
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

        // POST: api/SensorRFID
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SensorRFID>> PostSensorRFID(CriarSensorRFIdDTO sensorRFID)
        {
            var sensor = new SensorRFID
            {
                ZonaPatioId = sensorRFID.ZonaPatioId,
                PatioId = sensorRFID.PatioId,
                Posicao = sensorRFID.Posicao,
                PosicaoX = sensorRFID.PosicaoX,
                PosicaoY = sensorRFID.PosicaoY,
                Altura = sensorRFID.Altura,
                AnguloVisao = sensorRFID.AnguloVisao
            };
            
            _context.SensoresRFID.Add(sensor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSensorRFID), new { id = sensor.Id }, sensor);
        }

        // DELETE: api/SensorRFID/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSensorRFID(long id)
        {
            var sensorRFID = await _context.SensoresRFID.FindAsync(id);
            if (sensorRFID == null)
            {
                return NotFound();
            }

            _context.SensoresRFID.Remove(sensorRFID);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SensorRFIDExists(long id)
        {
            return _context.SensoresRFID.Any(e => e.Id == id);
        }
    }
}
