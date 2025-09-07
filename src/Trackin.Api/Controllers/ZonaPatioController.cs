using Microsoft.AspNetCore.Mvc;
using Trackin.Application.Common;
using Trackin.Application.DTOs;
using Trackin.Application.Interfaces;
using Trackin.Domain.Entity;

namespace Trackin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ZonaPatioController : BaseController
    {
        private readonly IZonaPatioService _zonaPatioService;

        public ZonaPatioController(IZonaPatioService zonaPatioService)
        {
            _zonaPatioService = zonaPatioService;
        }

        /// <summary>
        /// Recupera todas as zonas de pátio cadastradas com paginação
        /// </summary>
        /// <param name="paginacao">Parâmetros de paginação</param>
        /// <returns>Uma lista paginada de zonas de pátio</returns>
        /// <response code="200">Retorna a lista paginada de zonas de pátio</response>
        /// <response code="400">Quando os parâmetros de paginação são inválidos</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetZonasPatio([FromQuery] PaginacaoDTO paginacao)
        {

            ServiceResponsePaginado<ZonaPatio> response = await _zonaPatioService.GetAllZonasPatiosPaginatedAsync(
                paginacao.PageNumber,
                paginacao.PageSize,
                paginacao.Ordering,
                paginacao.DescendingOrder);

            return FromService(response);
        }

        /// <summary>
        /// Recupera todas as zonas de pátio cadastradas
        /// </summary>
        /// <returns>Uma lista de zonas de pátio</returns>
        /// <response code="200">Retorna a lista de zonas de pátio</response>
        /// <response code="404">Quando não há zonas de pátio cadastradas</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllZonasPatio()
        {
            ServiceResponse<IEnumerable<ZonaPatio>> response = await _zonaPatioService.GetAllZonasPatiosAsync();
            return FromService(response);
        }

        /// <summary>
        /// Recupera uma zona de pátio específica pelo seu ID
        /// </summary>
        /// <param name="id">ID da zona de pátio</param>
        /// <returns>Os dados da zona de pátio solicitada</returns>
        /// <response code="200">Retorna a zona de pátio solicitada</response>
        /// <response code="404">Quando a zona de pátio não é encontrada</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetZonaPatio(long id)
        {
            ServiceResponse<ZonaPatio> response = await _zonaPatioService.GetZonaPatioByIdAsync(id);
            return FromService(response);
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
        /// <response code="500">Erro interno do servidor</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutZonaPatio(long id, CriarZonaPatioDTO zonaPatioDto)
        {
            ServiceResponse<ZonaPatio> result = await _zonaPatioService.UpdateZonaPatioAsync(id, zonaPatioDto);
            if (!result.Success)
                return FromService(result);

            return NoContent();
        }

        /// <summary>
        /// Cria uma nova zona de pátio
        /// </summary>
        /// <param name="dto">Dados da zona de pátio a ser criada</param>
        /// <returns>A zona de pátio recém-criada</returns>
        /// <response code="201">Retorna a zona de pátio recém-criada</response>
        /// <response code="400">Quando os dados fornecidos são inválidos</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostZonaPatio([FromBody] CriarZonaPatioDTO dto)
        {
            ServiceResponse<ZonaPatio> result = await _zonaPatioService.CreateZonaPatioAsync(dto);

            if (!result.Success)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(GetZonaPatio), new { id = result.Data.Id }, result.Data);
        }

        /// <summary>
        /// Remove uma zona de pátio existente
        /// </summary>
        /// <param name="id">ID da zona de pátio a ser removida</param>
        /// <returns>Sem conteúdo</returns>
        /// <response code="204">Quando a zona de pátio é removida com sucesso</response>
        /// <response code="404">Quando a zona de pátio não é encontrada</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteZonaPatio(long id)
        {
            ServiceResponse<ZonaPatio> result = await _zonaPatioService.DeleteZonaPatioAsync(id);

            if (!result.Success)
                return FromService(result);
            
            return NoContent();
        }
    }
}