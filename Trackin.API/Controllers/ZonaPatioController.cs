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
    public class ZonaPatioController : ControllerBase
    {
        private readonly IZonaPatioRepository _zonaPatioRepository;

        public ZonaPatioController(IZonaPatioRepository zonaPatioRepository)
        {
            _zonaPatioRepository = zonaPatioRepository;
        }

        /// <summary>
        /// Recupera todas as zonas de pátio cadastradas
        /// </summary>
        /// <returns>Uma lista de zonas de pátio</returns>
        /// <response code="200">Retorna a lista de zonas de pátio</response>
        /// <response code="404">Quando não há zonas de pátio cadastradas</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ZonaPatio>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ZonaPatio>>> GetZonasPatio()
        {
            IEnumerable<ZonaPatio> zonasPatio = await _zonaPatioRepository.GetAllAsync();
            if (zonasPatio == null || !zonasPatio.Any())
            {
                return NotFound("Nenhuma zona de pátio encontrada.");
            }
            return Ok(zonasPatio);
        }

        /// <summary>
        /// Recupera uma zona de pátio específica pelo seu ID
        /// </summary>
        /// <param name="id">ID da zona de pátio</param>
        /// <returns>Os dados da zona de pátio solicitada</returns>
        /// <response code="200">Retorna a zona de pátio solicitada</response>
        /// <response code="404">Quando a zona de pátio não é encontrada</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ZonaPatio))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ZonaPatio>> GetZonaPatio(long id)
        {
            ZonaPatio? zonaPatio = await _zonaPatioRepository.GetByIdAsync(id);

            if (zonaPatio == null)
            {
                return NotFound($"Zona de pátio com ID {id} não encontrada.");
            }

            return Ok(zonaPatio);
        }

        /// <summary>
        /// Atualiza uma zona de pátio existente
        /// </summary>
        /// <param name="id">ID da zona de pátio a ser atualizada</param>
        /// <param name="zonaPatioDto">Dados atualizados da zona de pátio</param>
        /// <returns>Sem conteúdo</returns>
        /// <response code="204">Quando a zona de pátio é atualizada com sucesso</response>
        /// <response code="400">Quando os dados fornecidos são inválidos</response> 
        /// <response code="404">Quando a zona de pátio não é encontrada</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutZonaPatio(long id, CriarZonaPatioDTO zonaPatioDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ZonaPatio? zonaPatio = await _zonaPatioRepository.GetByIdAsync(id);
            if (zonaPatio == null)
            {
                return NotFound($"Zona de pátio com ID {id} não encontrada.");
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
                    return NotFound($"Zona de pátio com ID {id} não encontrada.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Cria uma nova zona de pátio
        /// </summary>
        /// <param name="dto">Dados da zona de pátio a ser criada</param>
        /// <returns>A zona de pátio recém-criada</returns>
        /// <response code="201">Retorna a zona de pátio recém-criada</response>
        /// <response code="400">Quando os dados fornecidos são inválidos</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ZonaPatio))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ZonaPatio>> CriarZonaPatio([FromBody] CriarZonaPatioDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ZonaPatio? zonaPatio = new ZonaPatio
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

        /// <summary>
        /// Remove uma zona de pátio existente
        /// </summary>
        /// <param name="id">ID da zona de pátio a ser removida</param>
        /// <returns>Sem conteúdo</returns>
        /// <response code="204">Quando a zona de pátio é removida com sucesso</response>
        /// <response code="404">Quando a zona de pátio não é encontrada</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteZonaPatio(long id)
        {
            ZonaPatio? zonaPatio = await _zonaPatioRepository.GetByIdAsync(id);
            if (zonaPatio == null)
            {
                return NotFound($"Zona de pátio com ID {id} não encontrada.");
            }

            await _zonaPatioRepository.RemoveAsync(zonaPatio);
            await _zonaPatioRepository.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Verifica se uma zona de pátio existe pelo ID
        /// </summary>
        /// <param name="id">ID da zona de pátio</param>
        /// <returns>Verdadeiro se a zona de pátio existe, falso caso contrário</returns>
        private bool ZonaPatioExists(long id)
        {
            return (_zonaPatioRepository.GetAllAsync().Result.Any(e => e.Id == id));
        }
    }
}