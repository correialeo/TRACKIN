using Microsoft.AspNetCore.Mvc;
using Trackin.API.Common;
using Trackin.API.Domain.Entity;
using Trackin.API.Domain.Enums;
using Trackin.API.DTOs;
using Trackin.API.Services;

namespace Trackin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class MotoController : ControllerBase
    {
        private readonly MotoService _motoService;

        public MotoController(MotoService motoService)
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
            if (response.Message == "Moto não encontrada.")
            {
                return NotFound(response.Message);
            }

            if (!response.Success)
            {
                return StatusCode(500, response.Message);
            }

            return Ok(response.Data);
        }

        /// <summary>
        /// Retorna todas as motos
        /// </summary>
        /// <returns>Lista de todas as motos</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            ServiceResponse<IEnumerable<Moto>> response = await _motoService.GetAllMotosAsync();
            if (!response.Success)
            {
                return StatusCode(500, response.Message);
            }

            return Ok(response.Data);
        }

        /// <summary>
        /// Retorna todas as motos de um determinado pátio
        /// </summary>
        /// <param name="patioId">ID do pátio</param>
        /// <returns>Lista de motos do pátio</returns>
        [HttpGet("patio/{patioId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByFilial(long patioId)
        {
            ServiceResponse<IEnumerable<Moto>> response = await _motoService.GetAllMotosByPatioAsync(patioId);
            if (!response.Success)
            {
                return StatusCode(500, response.Message);
            }

            return Ok(response.Data);
        }

        /// <summary>
        /// Retorna todas as motos com um determinado status
        /// </summary>
        /// <param name="status">Status da moto (enum)</param>
        /// <returns>Lista de motos com o status especificado</returns>
        [HttpGet("status/{status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByStatus(MotoStatus status)
        {
            ServiceResponse<IEnumerable<Moto>> response = await _motoService.GetAllMotosByStatusAsync(status);
            if (!response.Success)
            {
                return StatusCode(500, response.Message);
            }

            return Ok(response.Data);
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
            ServiceResponse<Moto> result = await _motoService.UpdateMotoAsync(id, motoDto);
            if (result.Message == "Moto não encontrada.")
            {
                return NotFound(result.Message);
            }

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

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
            if (result.Message == "Moto não encontrada.")
            {
                return NotFound(result.Message);
            }

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// Adiciona uma imagem base64 como referência para uma moto
        /// </summary>
        /// <param name="id">ID da moto.</param>
        /// <param name="imageB64">Imagem codificada em Base64</param>
        /// <returns>Retorna a moto com a imagem adicionada</returns>
        [HttpPost("{id}/imagem")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PostImagem(long id, [FromBody] string imageB64)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ServiceResponse<Moto> result = await _motoService.CadastrarImagemReferenciaAsync(id, imageB64);
            if (result.Message == "Moto não encontrada.")
            {
                return NotFound(result.Message);
            }

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
        }
    }
}
