
using Humanizer;
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
    public class SensorRFIDController : ControllerBase
    {
        private readonly ISensorRFIDRepository _sensorRFIDRepository;

        public SensorRFIDController(ISensorRFIDRepository sensorRFIDRepository)
        {
            _sensorRFIDRepository = sensorRFIDRepository;
        }

        // Updated method to fix CS0029 error
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SensorRFID>>> GetSensoresRFID()
        {
            IEnumerable<SensorRFID> sensores = await _sensorRFIDRepository.GetAllAsync();
            return Ok(sensores); 
        }

        // GET: api/SensorRFID/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SensorRFID>> GetSensorRFID(long id)
        {
            var sensorRFID = await _sensorRFIDRepository.GetByIdAsync(id);

            if (sensorRFID == null)
            {
                return NotFound();
            }

            return sensorRFID;
        }

        // PUT: api/SensorRFID/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSensorRFID(long id, CriarSensorRFIdDTO sensorRFIDDTO)
        {
            var sensorRFID = await _sensorRFIDRepository.GetByIdAsync(id);
            if (sensorRFID == null)
            {
                return NotFound();
            }

            sensorRFID.ZonaPatioId = sensorRFIDDTO.ZonaPatioId;
            sensorRFID.PatioId = sensorRFIDDTO.PatioId;
            sensorRFID.Posicao = sensorRFIDDTO.Posicao;
            sensorRFID.PosicaoX = sensorRFIDDTO.PosicaoX;
            sensorRFID.PosicaoY = sensorRFIDDTO.PosicaoY;
            sensorRFID.Altura = sensorRFIDDTO.Altura;
            sensorRFID.AnguloVisao = sensorRFIDDTO.AnguloVisao;


            try
            {
                await _sensorRFIDRepository.SaveChangesAsync();
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
            
            await _sensorRFIDRepository.AddAsync(sensor);
            await _sensorRFIDRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSensorRFID), new { id = sensor.Id }, sensor);
        }

        // DELETE: api/SensorRFID/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSensorRFID(long id)
        {
            var sensorRFID = await _sensorRFIDRepository.GetByIdAsync(id);
            if (sensorRFID == null)
            {
                return NotFound();
            }

            await _sensorRFIDRepository.RemoveAsync(sensorRFID);
            await _sensorRFIDRepository.SaveChangesAsync();

            return NoContent();
        }

        private bool SensorRFIDExists(long id)
        {
            return _sensorRFIDRepository.GetAllAsync().Result.Any(e => e.Id == id);
        }
    }
}
