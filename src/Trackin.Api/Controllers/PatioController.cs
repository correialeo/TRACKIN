using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Trackin.Application.Common;
using Trackin.Application.DTOs;
using Trackin.Application.Interfaces;
using Trackin.Domain.Entity;

namespace Trackin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(Roles = "ADMINISTRADOR,GERENTE,COMUM")]
    public class PatioController : BaseController
    {
        private readonly IPatioService _patioService;

        public PatioController(IPatioService patioService)
        {
            _patioService = patioService;
        }

        /// <summary>
        /// Recupera todos os pátios cadastrados no sistema com paginação
        /// </summary>
        /// <param name="paginacao">Parâmetros de paginação</param>
        /// <returns>Uma lista paginada de pátios</returns>
        /// <response code="200">Retorna a lista paginada de pátios</response>
        /// <response code="400">Quando os parâmetros de paginação são inválidos</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPatios([FromQuery] PaginacaoDTO paginacao)
        {
            ServiceResponsePaginado<Patio> response = await _patioService.GetAllPatiosPaginatedAsync(
                paginacao.PageNumber,
                paginacao.PageSize,
                paginacao.Ordering,
                paginacao.DescendingOrder);
            
            return FromServicePaged(response);
        }

        /// <summary>
        /// Recupera todos os pátios cadastrados no sistema
        /// </summary>
        /// <returns>Uma lista de pátios</returns>
        /// <response code="200">Retorna a lista de pátios</response>
        /// <response code="404">Quando não há pátios cadastrados</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPatios()
        {
            ServiceResponse<IEnumerable<Patio>> response = await _patioService.GetAllPatiosAsync();
            return FromService(response);
        }

        /// <summary>
        /// Recupera um pátio específico pelo seu ID
        /// </summary>
        /// <param name="id">ID do pátio</param>
        /// <returns>Os dados do pátio solicitado</returns>
        /// <response code="200">Retorna o pátio solicitado</response>
        /// <response code="404">Quando o pátio não é encontrado</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPatio(long id)
        {
            ServiceResponse<Patio> response = await _patioService.GetPatioByIdAsync(id);
            return FromService(response);
        }

        /// <summary>
        /// Cria um novo pátio
        /// </summary>
        /// <param name="dto">Dados do pátio a ser criado</param>
        /// <returns>O pátio recém-criado</returns>
        /// <response code="201">Retorna o pátio recém-criado</response>
        /// <response code="400">Quando os dados fornecidos são inválidos</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CriarPatio([FromBody] CriarPatioDto dto)
        {
            ServiceResponse<Patio> result = await _patioService.CreatePatioAsync(dto);
            if (!result.Success)
                return BadRequest(result.Message);
            
            return CreatedAtAction(nameof(GetPatio), new { id = result.Data.Id }, result.Data);
        }

        /// <summary>
        /// Remove um pátio existente
        /// </summary>
        /// <param name="id">ID do pátio a ser removido</param>
        /// <returns>Sem conteúdo</returns>
        /// <response code="204">Quando o pátio é removido com sucesso</response>
        /// <response code="404">Quando o pátio não é encontrado</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePatio(long id)
        {
            ServiceResponse<Patio> result = await _patioService.DeletePatioAsync(id);

            if (!result.Success)
                return FromService(result);

            return NoContent();
        }
    }
}

