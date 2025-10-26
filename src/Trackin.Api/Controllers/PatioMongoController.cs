using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Trackin.API.Controllers;
using Trackin.Application.Common;
using Trackin.Application.DTOs;
using Trackin.Application.Interfaces;
using Trackin.Domain.Entity;
using Trackin.Domain.ValueObjects;

namespace Trackin.Api.Controllers
{
    /// <summary>
    /// Controller para operações CRUD de Patio usando MongoDB
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/mongo/[controller]")]
    [ApiVersion(1.0)]
    
    public class PatioMongoController : BaseController
    {
        private readonly IPatioService _patioService;

        public PatioMongoController(IPatioService patioService)
        {
            _patioService = patioService;
        }

        /// <summary>
        /// Criar um novo pátio
        /// </summary>
        /// <param name="patioDTO">Dados do pátio</param>
        /// <returns>Pátio criado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ServiceResponse<Patio>), 200)]
        [ProducesResponseType(typeof(ServiceResponse<Patio>), 400)]
        public async Task<IActionResult> CreatePatio([FromBody] CriarPatioDto patioDTO)
        {
            try
            {
                var patio = new Patio(
                    patioDTO.Nome,
                    patioDTO.Endereco,
                    patioDTO.Cidade,
                    patioDTO.Estado,
                    patioDTO.Pais,
                    patioDTO.DimensaoY,
                    patioDTO.DimensaoX
                );

                ServiceResponse<Patio> result = await _patioService.CreatePatioAsync(patioDTO);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResponse<Patio>
                {
                    Success = false,
                    Message = $"Erro ao criar pátio: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Obter pátio por ID
        /// </summary>
        /// <param name="id">ID do pátio</param>
        /// <returns>Dados do pátio</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ServiceResponse<Patio>), 200)]
        [ProducesResponseType(typeof(ServiceResponse<Patio>), 404)]
        public async Task<IActionResult> GetPatioById(long id)
        {
            ServiceResponse<Patio> result = await _patioService.GetPatioByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Listar todos os pátios
        /// </summary>
        /// <returns>Lista de pátios</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ServiceResponse<IEnumerable<Patio>>), 200)]
        public async Task<IActionResult> GetAllPatios()
        {
            ServiceResponse<IEnumerable<Patio>> result = await _patioService.GetAllPatiosAsync();
            return Ok(result);
        }

        /// <summary>
        /// Listar pátios com paginação
        /// </summary>
        /// <param name="pageNumber">Número da página</param>
        /// <param name="pageSize">Tamanho da página</param>
        /// <param name="ordering">Campo para ordenação</param>
        /// <param name="descendingOrder">Ordem decrescente</param>
        /// <returns>Lista paginada de pátios</returns>
        [HttpGet("paginated")]
        [ProducesResponseType(typeof(ServiceResponsePaginado<Patio>), 200)]
        public async Task<IActionResult> GetPatiosPaginated(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? ordering = null,
            [FromQuery] bool descendingOrder = false)
        {
            ServiceResponsePaginado<Patio> result = await _patioService.GetAllPatiosPaginatedAsync(pageNumber, pageSize, ordering, descendingOrder);
            return Ok(result);
        }

        /// <summary>
        /// Adicionar moto ao pátio (usando agregado raiz)
        /// </summary>
        /// <param name="patioId">ID do pátio</param>
        /// <param name="placa">Placa da moto</param>
        /// <param name="modelo">Modelo da moto</param>
        /// <param name="ano">Ano da moto</param>
        /// <param name="rfidTag">RFID Tag da moto</param>
        /// <returns>Moto adicionada</returns>
        [HttpPost("{patioId}/motos")]
        [ProducesResponseType(typeof(ServiceResponse<Moto>), 200)]
        [ProducesResponseType(typeof(ServiceResponse<Moto>), 400)]
        public async Task<IActionResult> AdicionarMotoAoPatio(
            long patioId,
            [FromBody] AdicionarMotoDTO motoDTO)
        {
            try
            {
                // Obter o pátio
                ServiceResponse<Patio> patioResult = await _patioService.GetPatioByIdAsync(patioId);
                if (!patioResult.Success)
                    return NotFound(patioResult);

                Patio patio = patioResult.Data;
                if (patio == null)
                    return NotFound(new ServiceResponse<Moto> { Success = false, Message = "Pátio não encontrado" });

                // Usar o método do agregado raiz para adicionar moto
                Moto moto = patio.AdicionarMoto(motoDTO.Placa, motoDTO.Modelo, motoDTO.Ano, motoDTO.RFIDTag);

                return Ok(new ServiceResponse<Moto>
                {
                    Success = true,
                    Data = moto,
                    Message = "Moto adicionada ao pátio com sucesso"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResponse<Moto>
                {
                    Success = false,
                    Message = $"Erro ao adicionar moto: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Obter motos do pátio
        /// </summary>
        /// <param name="patioId">ID do pátio</param>
        /// <returns>Lista de motos do pátio</returns>
        [HttpGet("{patioId}/motos")]
        [ProducesResponseType(typeof(ServiceResponse<IEnumerable<Moto>>), 200)]
        public async Task<IActionResult> GetMotosDoPatio(long patioId)
        {
            try
            {
                ServiceResponse<Patio> patioResult = await _patioService.GetPatioByIdAsync(patioId);
                if (!patioResult.Success)
                    return NotFound(patioResult);

                Patio patio = patioResult.Data;
                if (patio == null)
                    return NotFound(new ServiceResponse<IEnumerable<Moto>> { Success = false, Message = "Pátio não encontrado" });

                IReadOnlyCollection<Moto> motos = patio.Motos;

                return Ok(new ServiceResponse<IEnumerable<Moto>>
                {
                    Success = true,
                    Data = motos,
                    Message = $"Encontradas {motos.Count} motos no pátio"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResponse<IEnumerable<Moto>>
                {
                    Success = false,
                    Message = $"Erro ao obter motos do pátio: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Deletar pátio
        /// </summary>
        /// <param name="id">ID do pátio</param>
        /// <returns>Confirmação da exclusão</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ServiceResponse<Patio>), 200)]
        [ProducesResponseType(typeof(ServiceResponse<Patio>), 404)]
        public async Task<IActionResult> DeletePatio(long id)
        {
            ServiceResponse<Patio> result = await _patioService.DeletePatioAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }

    /// <summary>
    /// DTO para adicionar moto ao pátio
    /// </summary>
    public class AdicionarMotoDTO
    {
        public string Placa { get; set; } = string.Empty;
        public Domain.Enums.ModeloMoto Modelo { get; set; }
        public int Ano { get; set; }
        public string RFIDTag { get; set; } = string.Empty;
    }
}