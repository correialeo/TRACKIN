using Microsoft.AspNetCore.Mvc;
using Trackin.API.Domain.Entity;
using Trackin.API.DTOs;
using Trackin.API.Infrastructure.Persistence.Repositories;

namespace Trackin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PatioController : ControllerBase
    {
        private readonly IPatioRepository _patioRepository;

        public PatioController(IPatioRepository patioRepository)
        {
            _patioRepository = patioRepository;
        }

        /// <summary>
        /// Recupera todos os pátios cadastrados no sistema
        /// </summary>
        /// <returns>Uma lista de pátios</returns>
        /// <response code="200">Retorna a lista de pátios</response>
        /// <response code="404">Quando não há pátios cadastrados</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Patio>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Patio>>> GetPatios()
        {
            IEnumerable<Patio> patios = await _patioRepository.GetAllAsync();
            if (patios == null || !patios.Any())
            {
                return NotFound("Nenhum pátio encontrado.");
            }
            return Ok(patios);
        }

        /// <summary>
        /// Recupera um pátio específico pelo seu ID
        /// </summary>
        /// <param name="id">ID do pátio</param>
        /// <returns>Os dados do pátio solicitado</returns>
        /// <response code="200">Retorna o pátio solicitado</response>
        /// <response code="404">Quando o pátio não é encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Patio))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Patio>> GetPatio(long id)
        {
            var patio = await _patioRepository.GetByIdAsync(id);

            if (patio == null)
            {
                return NotFound($"Pátio com ID {id} não encontrado.");
            }

            return Ok(patio);
        }

        /// <summary>
        /// Cria um novo pátio
        /// </summary>
        /// <param name="dto">Dados do pátio a ser criado</param>
        /// <returns>O pátio recém-criado</returns>
        /// <response code="201">Retorna o pátio recém-criado</response>
        /// <response code="400">Quando os dados fornecidos são inválidos</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Patio))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Patio>> CriarPatio([FromBody] CriarPatioDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

        /// <summary>
        /// Remove um pátio existente
        /// </summary>
        /// <param name="id">ID do pátio a ser removido</param>
        /// <returns>Sem conteúdo</returns>
        /// <response code="204">Quando o pátio é removido com sucesso</response>
        /// <response code="404">Quando o pátio não é encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePatio(long id)
        {
            var patio = await _patioRepository.GetByIdAsync(id);
            if (patio == null)
            {
                return NotFound($"Pátio com ID {id} não encontrado.");
            }

            await _patioRepository.RemoveAsync(patio);
            await _patioRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}