using Microsoft.AspNetCore.Mvc;
using Trackin.Application.Common;
using Trackin.Application.DTOs;
using Trackin.Application.Interfaces;
using Trackin.Domain.Entity;
using Trackin.Domain.Enums;

namespace Trackin.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class MotoController : BaseController
    {
        private readonly IMotoService _motoService;

        public MotoController(IMotoService motoService)
        {
            _motoService = motoService;
        }

        /// <summary>
        /// Cria uma nova moto
        /// </summary>
        /// <param name="motoDto">Objeto DTO com os dados da moto a ser criada</param>
        /// <returns>Retorna a moto criada com o status 201 ou erro 400</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] MotoDTO motoDto)
        {
            
            ServiceResponse<MotoDTO> result = await _motoService.CreateMotoAsync(motoDto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
        }

        /// <summary>
        /// Retorna uma moto pelo seu ID
        /// </summary>
        /// <param name="id">ID da moto</param>
        /// <returns>Objeto da moto ou erro 404 se não encontrada</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(long id)
        {
            ServiceResponse<Moto> response = await _motoService.GetMotoByIdAsync(id);
                return FromService(response);
        }

        /// <summary>
        /// Retorna todas as motos
        /// </summary>
        /// <returns>Lista de todas as motos</returns>
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            ServiceResponse<IEnumerable<Moto>> response = await _motoService.GetAllMotosAsync();
            return FromService(response);
        }

        /// <summary>
        /// Retorna motos com paginação
        /// </summary>
        /// <param name="paginacao">Parâmetros de paginação</param>
        /// <returns>Lista paginada de motos</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPaginated([FromQuery] PaginacaoDTO paginacao)
        {

            ServiceResponsePaginado<Moto> response = await _motoService.GetAllMotosPaginatedAsync(
                paginacao.PageNumber,
                paginacao.PageSize,
                paginacao.Ordering,
                paginacao.DescendingOrder);
            
            return FromServicePaged(response);
        }

        /// <summary>
        /// Retorna todas as motos de um determinado pátio com paginação
        /// </summary>
        /// <param name="patioId">ID do pátio</param>
        /// /// <param name="paginacao">Parâmetros de paginação</param>
        /// <returns>Lista de motos do pátio</returns>
        [HttpGet("patio/{patioId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByFilial(long patioId, [FromQuery] PaginacaoDTO paginacao)
        {
            ServiceResponsePaginado<Moto> response = await _motoService.GetMotosByPatioPaginatedAsync(
                patioId,
                paginacao.PageNumber,
                paginacao.PageSize,
                paginacao.Ordering,
                paginacao.DescendingOrder);
           
            return FromServicePaged(response);
        }

        /// <summary>
        /// Retorna motos por status com paginação
        /// </summary>
        /// <param name="status">Status da moto</param>
        /// <param name="paginacao">Parâmetros de paginação</param>
        /// <returns>Lista paginada de motos com o status especificado</returns>
        [HttpGet("status/{status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByStatusPaginated(
            MotoStatus status,
            [FromQuery] PaginacaoDTO paginacao)
        {
            ServiceResponsePaginado<Moto> response = await _motoService.GetMotosByStatusPaginatedAsync(
                status,
                paginacao.PageNumber,
                paginacao.PageSize,
                paginacao.Ordering,
                paginacao.DescendingOrder);

            return FromServicePaged(response);
        }

        /// <summary>
        /// Atualiza uma moto existente
        /// </summary>
        /// <param name="id">ID da moto a ser atualizada</param>
        /// <param name="motoDto">Dados atualizados da moto</param>
        /// <returns>Status 204 em caso de sucesso, ou erro apropriado</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(long id, [FromBody] EditarMotoDTO motoDto)
        {
            ServiceResponse<Moto> result = await _motoService.UpdateMotoAsync(id);
            if (!result.Success) 
                return FromService(result);
            
            return NoContent();
        }

        /// <summary>
        /// Exclui uma moto pelo ID
        /// </summary>
        /// <param name="id">ID da moto a ser excluída</param>
        /// <returns>Status 204 se deletada com sucesso</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            ServiceResponse<Moto> result = await _motoService.DeleteMotoAsync(id);
            
            if (!result.Success)
                return FromService(result);
            

            return NoContent();
        }
        
    }
}
