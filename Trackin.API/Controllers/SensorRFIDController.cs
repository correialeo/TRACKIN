using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trackin.API.Domain.Entity;
using Trackin.API.DTOs;
using Trackin.API.Infrastructure.Persistence.Repositories;

namespace Trackin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class SensorRFIDController : ControllerBase
    {
        private readonly ISensorRFIDRepository _sensorRFIDRepository;

        public SensorRFIDController(ISensorRFIDRepository sensorRFIDRepository)
        {
            _sensorRFIDRepository = sensorRFIDRepository;
        }

        /// <summary>
        /// Recupera todos os sensores RFID cadastrados
        /// </summary>
        /// <returns>Uma lista de sensores RFID</returns>
        /// <response code="200">Retorna a lista de sensores</response>
        /// <response code="404">Quando não há sensores cadastrados</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SensorRFID>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<SensorRFID>>> GetSensoresRFID()
        {
            IEnumerable<SensorRFID> sensores = await _sensorRFIDRepository.GetAllAsync();
            if (sensores == null || !sensores.Any())
            {
                return NotFound("Nenhum sensor RFID encontrado.");
            }
            return Ok(sensores);
        }

        /// <summary>
        /// Recupera um sensor RFID específico pelo seu ID
        /// </summary>
        /// <param name="id">ID do sensor RFID</param>
        /// <returns>Os dados do sensor solicitado</returns>
        /// <response code="200">Retorna o sensor solicitado</response>
        /// <response code="404">Quando o sensor não é encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SensorRFID))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SensorRFID>> GetSensorRFID(long id)
        {
            SensorRFID? sensorRFID = await _sensorRFIDRepository.GetByIdAsync(id);

            if (sensorRFID == null)
            {
                return NotFound($"Sensor RFID com ID {id} não encontrado.");
            }

            return Ok(sensorRFID);
        }

        /// <summary>
        /// Atualiza um sensor RFID existente
        /// </summary>
        /// <param name="id">ID do sensor RFID a ser atualizado</param>
        /// <param name="sensorRFIDDTO">Dados atualizados do sensor</param>
        /// <returns>Sem conteúdo</returns>
        /// <response code="204">Quando o sensor é atualizado com sucesso</response>
        /// <response code="400">Quando os dados fornecidos são inválidos</response>
        /// <response code="404">Quando o sensor não é encontrado</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutSensorRFID(long id, CriarSensorRFIdDTO sensorRFIDDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SensorRFID? sensorRFID = await _sensorRFIDRepository.GetByIdAsync(id);
            if (sensorRFID == null)
            {
                return NotFound($"Sensor RFID com ID {id} não encontrado.");
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
                    return NotFound($"Sensor RFID com ID {id} não encontrado.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Cria um novo sensor RFID
        /// </summary>
        /// <param name="sensorRFID">Dados do sensor a ser criado</param>
        /// <returns>O sensor recém-criado</returns>
        /// <response code="201">Retorna o sensor recém-criado</response>
        /// <response code="400">Quando os dados fornecidos são inválidos</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SensorRFID))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SensorRFID>> PostSensorRFID(CriarSensorRFIdDTO sensorRFID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SensorRFID sensor = new SensorRFID
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

        /// <summary>
        /// Remove um sensor RFID existente
        /// </summary>
        /// <param name="id">ID do sensor a ser removido</param>
        /// <returns>Sem conteúdo</returns>
        /// <response code="204">Quando o sensor é removido com sucesso</response>
        /// <response code="404">Quando o sensor não é encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSensorRFID(long id)
        {
            SensorRFID? sensorRFID = await _sensorRFIDRepository.GetByIdAsync(id);
            if (sensorRFID == null)
            {
                return NotFound($"Sensor RFID com ID {id} não encontrado.");
            }

            await _sensorRFIDRepository.RemoveAsync(sensorRFID);
            await _sensorRFIDRepository.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Verifica se um sensor RFID existe pelo ID
        /// </summary>
        /// <param name="id">ID do sensor</param>
        /// <returns>Verdadeiro se o sensor existe, falso caso contrário</returns>
        private bool SensorRFIDExists(long id)
        {
            return _sensorRFIDRepository.GetAllAsync().Result.Any(e => e.Id == id);
        }
    }
}