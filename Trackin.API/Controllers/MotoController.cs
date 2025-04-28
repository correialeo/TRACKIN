using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trackin.API.Domain.Entity;
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
    }
}
