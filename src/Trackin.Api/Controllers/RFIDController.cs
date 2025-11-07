using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Trackin.Application.Common;
using Trackin.Application.DTOs;
using Trackin.Application.Interfaces;

namespace Trackin.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion(1.0)]
    [Produces("application/json")]
    [Authorize(Roles = "ADMINISTRADOR,GERENTE,COMUM")]
    public class RFIDController : BaseController
    {
        private readonly IRFIDService _service;
        public RFIDController(IRFIDService service)
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

            ServiceResponse<LocalizacaoMotoDTO> response = await _service.ProcessarLeituraRFID(leitura);
            return  FromService(response);
        }
    }
}