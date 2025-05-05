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
    public class MotoController : ControllerBase
    {
        private readonly MotoService _motoService;
        public MotoController(MotoService motoService)
        {
            _motoService = motoService;
        }

        [HttpPost]
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
        public async Task<IActionResult> Put(long id, [FromBody] EditarMotoDTO motoDto)
        {
            ServiceResponse<Moto> result = await _motoService.UpdateMotoAsync(id, motoDto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            ServiceResponse<Moto> result = await _motoService.DeleteMotoAsync(id);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return NoContent();
        }

        [HttpPost("{id}/imagem")]
        public async Task<IActionResult> PostImagem(long id, string imageB64)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _motoService.CadastrarImagemReferenciaAsync(id, imageB64);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
        }
    }
}
