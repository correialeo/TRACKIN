using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trackin.API.DTOs;
using Trackin.API.Services;

namespace Trackin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RFIDController : ControllerBase
    {
        private readonly RFIDService _service;
        public RFIDController(RFIDService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessarLeituraRFID([FromBody] RFIDLeituraDTO leitura)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _service.ProcessarLeituraRFID(leitura);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

    }
}
