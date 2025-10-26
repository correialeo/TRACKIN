using Microsoft.AspNetCore.Mvc;
using Trackin.API.Controllers;
using Trackin.Application.Common;
using Trackin.Application.DTOs;
using Trackin.Application.Interfaces;
using Trackin.Domain.Entity;
using Trackin.Domain.Enums;

namespace Trackin.Api.Controllers
{
    /// <summary>
    /// Controller para operações CRUD de Moto usando MongoDB
    /// </summary>
    [ApiController]
    [Route("api/v1/mongo/[controller]")]
    public class MotoMongoController : BaseController
    {
        private readonly IMotoService _motoService;

        public MotoMongoController(IMotoService motoService)
        {
            _motoService = motoService;
        }

        /// <summary>
        /// Criar uma nova moto
        /// </summary>
        /// <param name="motoDTO">Dados da moto</param>
        /// <returns>Moto criada</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ServiceResponse<MotoDTO>), 200)]
        [ProducesResponseType(typeof(ServiceResponse<MotoDTO>), 400)]
        public async Task<IActionResult> CreateMoto([FromBody] MotoDTO motoDTO)
        {
            ServiceResponse<MotoDTO> result = await _motoService.CreateMotoAsync(motoDTO);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Obter moto por ID
        /// </summary>
        /// <param name="id">ID da moto</param>
        /// <returns>Dados da moto</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ServiceResponse<Moto>), 200)]
        [ProducesResponseType(typeof(ServiceResponse<Moto>), 404)]
        public async Task<IActionResult> GetMotoById(long id)
        {
            ServiceResponse<Moto> result = await _motoService.GetMotoByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Listar todas as motos
        /// </summary>
        /// <returns>Lista de motos</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ServiceResponse<IEnumerable<Moto>>), 200)]
        public async Task<IActionResult> GetAllMotos()
        {
            ServiceResponse<IEnumerable<Moto>> result = await _motoService.GetAllMotosAsync();
            return Ok(result);
        }

        /// <summary>
        /// Listar motos com paginação
        /// </summary>
        /// <param name="pageNumber">Número da página</param>
        /// <param name="pageSize">Tamanho da página</param>
        /// <param name="ordering">Campo para ordenação</param>
        /// <param name="descendingOrder">Ordem decrescente</param>
        /// <returns>Lista paginada de motos</returns>
        [HttpGet("paginated")]
        [ProducesResponseType(typeof(ServiceResponsePaginado<Moto>), 200)]
        public async Task<IActionResult> GetMotosPaginated(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? ordering = null,
            [FromQuery] bool descendingOrder = false)
        {
            ServiceResponsePaginado<Moto> result = await _motoService.GetAllMotosPaginatedAsync(pageNumber, pageSize, ordering, descendingOrder);
            return Ok(result);
        }

        /// <summary>
        /// Listar motos por pátio com paginação
        /// </summary>
        /// <param name="patioId">ID do pátio</param>
        /// <param name="pageNumber">Número da página</param>
        /// <param name="pageSize">Tamanho da página</param>
        /// <param name="ordering">Campo para ordenação</param>
        /// <param name="descendingOrder">Ordem decrescente</param>
        /// <returns>Lista paginada de motos do pátio</returns>
        [HttpGet("patio/{patioId}/paginated")]
        [ProducesResponseType(typeof(ServiceResponsePaginado<Moto>), 200)]
        public async Task<IActionResult> GetMotosByPatioPaginated(
            long patioId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? ordering = null,
            [FromQuery] bool descendingOrder = false)
        {
            ServiceResponsePaginado<Moto> result = await _motoService.GetMotosByPatioPaginatedAsync(patioId, pageNumber, pageSize, ordering, descendingOrder);
            return Ok(result);
        }

        /// <summary>
        /// Listar motos por status com paginação
        /// </summary>
        /// <param name="status">Status da moto</param>
        /// <param name="pageNumber">Número da página</param>
        /// <param name="pageSize">Tamanho da página</param>
        /// <param name="ordering">Campo para ordenação</param>
        /// <param name="descendingOrder">Ordem decrescente</param>
        /// <returns>Lista paginada de motos por status</returns>
        [HttpGet("status/{status}/paginated")]
        [ProducesResponseType(typeof(ServiceResponsePaginado<Moto>), 200)]
        public async Task<IActionResult> GetMotosByStatusPaginated(
            MotoStatus status,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? ordering = null,
            [FromQuery] bool descendingOrder = false)
        {
            ServiceResponsePaginado<Moto> result = await _motoService.GetMotosByStatusPaginatedAsync(status, pageNumber, pageSize, ordering, descendingOrder);
            return Ok(result);
        }

        /// <summary>
        /// Atualizar moto
        /// </summary>
        /// <param name="id">ID da moto</param>
        /// <returns>Moto atualizada</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ServiceResponse<Moto>), 200)]
        [ProducesResponseType(typeof(ServiceResponse<Moto>), 404)]
        public async Task<IActionResult> UpdateMoto(long id)
        {
            ServiceResponse<Moto> result = await _motoService.UpdateMotoAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Deletar moto
        /// </summary>
        /// <param name="id">ID da moto</param>
        /// <returns>Confirmação da exclusão</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ServiceResponse<Moto>), 200)]
        [ProducesResponseType(typeof(ServiceResponse<Moto>), 404)]
        public async Task<IActionResult> DeleteMoto(long id)
        {
            ServiceResponse<Moto> result = await _motoService.DeleteMotoAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}
