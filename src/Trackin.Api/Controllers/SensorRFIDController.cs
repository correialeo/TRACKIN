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
    public class SensorRFIDController : BaseController
    {
        private readonly ISensorRFIDService _sensorRFIDService;

        public SensorRFIDController(ISensorRFIDService sensorRFIDService)
        {
            _sensorRFIDService = sensorRFIDService;
        }

        /// <summary>
        /// Recupera todos os sensores RFID cadastrados com paginação
        /// </summary>
        /// <param name="paginacao">Parâmetros de paginação</param>
        /// <returns>Uma lista paginada de sensores RFID</returns>
        /// <response code="200">Retorna a lista paginada de sensores</response>
        /// <response code="400">Quando os parâmetros de paginação são inválidos</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSensoresRFID([FromQuery] PaginacaoDTO paginacao)
        {

            ServiceResponsePaginado<SensorRFID> response = await _sensorRFIDService.GetAllSensoresRFIDPaginatedAsync(
                paginacao.PageNumber,
                paginacao.PageSize,
                paginacao.Ordering,
                paginacao.DescendingOrder);
            
            return FromServicePaged(response);
        }

        /// <summary>
        /// Recupera todos os sensores RFID cadastrados
        /// </summary>
        /// <returns>Uma lista de sensores RFID</returns>
        /// <response code="200">Retorna a lista de sensores</response>
        /// <response code="404">Quando não há sensores cadastrados</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllSensoresRFID()
        {
            ServiceResponse<IEnumerable<SensorRFID>> response = await _sensorRFIDService.GetAllSensoresRFIDAsync();
            return FromService(response);
        }

        /// <summary>
        /// Recupera um sensor RFID específico pelo seu ID
        /// </summary>
        /// <param name="id">ID do sensor RFID</param>
        /// <returns>Os dados do sensor solicitado</returns>
        /// <response code="200">Retorna o sensor solicitado</response>
        /// <response code="404">Quando o sensor não é encontrado</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSensorRFID(long id)
        {
            ServiceResponse<SensorRFID> response = await _sensorRFIDService.GetSensorRFIDByIdAsync(id);
            return FromService(response);
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
        /// <response code="500">Erro interno do servidor</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutSensorRFID(long id, CriarSensorRFIdDTO sensorRFIDDTO)
        {

            ServiceResponse<SensorRFID> result = await _sensorRFIDService.UpdateSensorRFIDAsync(id, sensorRFIDDTO);
            
            if (!result.Success)
                return FromService(result);

            return NoContent();
        }

        /// <summary>
        /// Cria um novo sensor RFID
        /// </summary>
        /// <param name="sensorRFID">Dados do sensor a ser criado</param>
        /// <returns>O sensor recém-criado</returns>
        /// <response code="201">Retorna o sensor recém-criado</response>
        /// <response code="400">Quando os dados fornecidos são inválidos</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostSensorRFID(CriarSensorRFIdDTO sensorRFID)
        {
            ServiceResponse<SensorRFID> result = await _sensorRFIDService.CreateSensorRFIDAsync(sensorRFID);

            if (!result.Success)
                return BadRequest(result.Message);
            

            return CreatedAtAction(nameof(GetSensorRFID), new { id = result.Data.Id }, result.Data);
        }

        /// <summary>
        /// Remove um sensor RFID existente
        /// </summary>
        /// <param name="id">ID do sensor a ser removido</param>
        /// <returns>Sem conteúdo</returns>
        /// <response code="204">Quando o sensor é removido com sucesso</response>
        /// <response code="404">Quando o sensor não é encontrado</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteSensorRFID(long id)
        {
            ServiceResponse<SensorRFID> result = await _sensorRFIDService.DeleteSensorRFIDAsync(id);
            
            if (!result.Success)
                return FromService(result);

            return NoContent();
        }
    }
}