using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            var result = await _motoService.CadastrarImagemReferenciaAsync(id, imageB64);
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
