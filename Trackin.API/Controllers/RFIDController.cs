using Microsoft.AspNetCore.Mvc;
using Trackin.API.Common;
using Trackin.API.DTOs;
using Trackin.API.Services;

namespace Trackin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class RFIDController : ControllerBase
    {
        private readonly RFIDService _service;
        public RFIDController(RFIDService service)
        {
            _service = service;
        }

        /// <summary>
        /// Processa uma leitura de RFID e atualiza a localização/status da moto
        /// </summary>
        /// <param name="leitura">Dados da leitura RFID</param>
        /// <response code="200">Leitura processada com sucesso</response>
        /// <response code="400">Dados inválidos ou erro de processamento</response>
        /// <response code="404">RFID ou Sensor não encontrado</response>
        [HttpPost]
        [ProducesResponseType(typeof(LocalizacaoMotoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ProcessarLeituraRFID([FromBody] RFIDLeituraDTO leitura)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState); 
            }

            ServiceResponse<LocalizacaoMotoDTO> response = await _service.ProcessarLeituraRFID(leitura);

            if (response.Message.Contains("não encontrada") || response.Message.Contains("não encontrado"))
            {
                return NotFound(response.Message);
            }

            if (!response.Success)
            {
                return BadRequest(new { Code = "PROCESSAMENTO_ERRO", Message = response.Message });
            }

            return Ok(response.Data);
        }
    }
}