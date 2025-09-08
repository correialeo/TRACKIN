using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Trackin.Application.Common;

namespace Trackin.API.Controllers;

[ApiController] 
public abstract class BaseController : ControllerBase
{
        /// <summary>
        /// Mapeia ServiceResponse<T> para IActionResult (Ok/BadRequest/NotFound).
        /// usar em gets por id e listagens simples não paginadas.
        /// </summary>

        protected IActionResult FromService<T>(ServiceResponse<T> response)
        {
                if (response is null)
                        return StatusCode(StatusCodes.Status500InternalServerError, "Erro: Resposta nula do serviço.");
                if (!response.Success)
                {
                        var msg = response.Message ?? "Erro ao processar requisição.";
                        if (Regex.IsMatch(msg, @"encontrad[oa]", RegexOptions.IgnoreCase))
                                return NotFound(msg);
                        return BadRequest(msg);
                }
                return Ok(response.Data);
        }

        ///<sumary>
        /// Mapeia ServiceResponsePaginado<T> para IActionResult (Ok/BadRequest/NotFound).
        /// Use em GETs paginados.
        /// </summary>

        protected IActionResult FromServicePaged<T>(ServiceResponsePaginado<T> response)
        {
                if(response is null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro: Resposta nula de serviço.");

                if (!response.Success)
                {
                        var msg = response.Message ?? "Erro ao processar a requisição.";
                        if (msg.Contains("não encontrado."))
                                return NotFound(msg);
                        return BadRequest(msg);
                }
                return Ok(response.Data);
                
        }

      
        
}