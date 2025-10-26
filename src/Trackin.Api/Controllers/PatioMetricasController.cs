using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Trackin.Application.Interfaces;

namespace Trackin.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion(1.0)]
    
    public class PatioMetricasController : ControllerBase
    {
        private readonly IPatioMetricasService _patioMetricasService;

        public PatioMetricasController(IPatioMetricasService patioMetricasService)
        {
            _patioMetricasService = patioMetricasService;
        }

        /// <summary>
        /// Obtém a taxa de ocupação de um pátio específico.
        /// </summary>
        /// <param name="id">ID do pátio</param>
        /// <returns>Taxa de ocupação em percentual (0 a 100)</returns>
        [HttpGet("{id}/taxa-ocupacao")]
        public async Task<IActionResult> GetTaxaOcupacao(long id)
        {
            var response = await _patioMetricasService.GetTaxaOcupacaoAsync(id);

            if (!response.Success)
                return NotFound(new { message = response.Message });

            return Ok(new
            {
                taxaOcupacao = response.Data,
                message = response.Message
            });
        }
    }
}