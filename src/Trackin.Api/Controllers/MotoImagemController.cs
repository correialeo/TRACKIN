using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Trackin.Application.Interfaces;
using Trackin.Domain.Entity;
using Trackin.Application.Common;

namespace Trackin.API.Controllers
{
    
    
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion(1.0)]
    [Produces("application/json")]
    public class MotoImagemController : ControllerBase
    {
        private readonly IMotoImagemService _motoImagemService;

        public MotoImagemController(IMotoImagemService motoImagemService)
        {
            _motoImagemService = motoImagemService;
        }

        /// <summary>
        /// Adiciona uma imagem base64 como referência para uma moto
        /// </summary>
        /// <param name="id">ID da moto.</param>
        /// <param name="imageB64">Imagem codificada em Base64</param>
        /// <returns>Retorna a moto com a imagem adicionada</returns>
        [HttpPost("{id}/imagem")]
        [ProducesResponseType(typeof(Moto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CadastrarImagem(long id, [FromBody] string imagemReferencia)
        {
            var response = await _motoImagemService.CadastrarImagemReferenciaAsync(id, imagemReferencia);

            if (!response.Success)
            {
                if (response.Message.Contains("não encontrada"))
                    return NotFound(response.Message);

                return BadRequest(response.Message);
            }

            return Ok(response.Data);
        }
    }
}