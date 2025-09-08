using Trackin.Application.Common;
using Trackin.Application.DTOs;
using Trackin.Application.Interfaces;
using Trackin.Domain.Entity;
using Trackin.Domain.Interfaces;
using Trackin.Domain.ValueObjects;

namespace Trackin.Application.Services
{
    public class ZonaPatioService : IZonaPatioService
    {
        private readonly IZonaPatioRepository _zonaPatioRepository;
        private readonly IPatioRepository _patioRepository;
        
        private const string ZonaNaoEncontrada = "Zona de pátio não encontrada.";
        private const string PatioNaoEncontrado = "Pátio associado não encontrado.";
        
        private ServiceResponse<T> Sucesso<T>(T data, string message = "") => new() { Success = true, Data = data, Message = message };
        private ServiceResponse<T> Erro<T>(string message) => new() { Success = false, Message = message };

        private async Task<ZonaPatio?> ObterZona(long id) => await _zonaPatioRepository.GetByIdAsync(id);
        private async Task<Patio?> ObterPatio(long id) => await _patioRepository.GetByIdAsync(id);


        public ZonaPatioService(IZonaPatioRepository zonaPatioRepository, IPatioRepository patioRepository)
        {
            _zonaPatioRepository = zonaPatioRepository;
            _patioRepository = patioRepository;
        }

        public async Task<ServiceResponsePaginado<ZonaPatio>> GetAllZonasPatiosPaginatedAsync(
            int pageNumber,
            int pageSize,
            string? ordering = null,
            bool descendingOrder = false)
        {
            try
            {
                (IEnumerable<ZonaPatio> items, int totalCount) = await _zonaPatioRepository.GetAllPaginatedAsync(
                    pageNumber, pageSize, ordering, descendingOrder);

                return new ServiceResponsePaginado<ZonaPatio>(items, pageNumber, pageSize, totalCount);
            }
            catch (Exception ex)
            {
                return new ServiceResponsePaginado<ZonaPatio>(new List<ZonaPatio>(), pageNumber, pageSize, 0)
                {
                    Success = false,
                    Message = $"Erro ao obter zona de pátio paginada: {ex.Message}"
                };
            }
        }


        public async Task<ServiceResponse<IEnumerable<ZonaPatio>>> GetAllZonasPatiosAsync()
        {
            try
            {
                var zonas = await _zonaPatioRepository.GetAllAsync();
                if (zonas == null || !zonas.Any())
                    return Erro<IEnumerable<ZonaPatio>>("Nenhuma zona de pátio encontrada.");

                return Sucesso(zonas);
            }
            catch (Exception ex)
            {
                return Erro<IEnumerable<ZonaPatio>>($"Erro ao obter zonas de pátio: {ex.Message}");
            }
            
        }

        public async Task<ServiceResponse<ZonaPatio>> GetZonaPatioByIdAsync(long id)
        {
            try
            {
                var zona = await ObterZona(id);
                if (zona == null) return Erro<ZonaPatio>(ZonaNaoEncontrada);

                return Sucesso(zona);
            }
            catch (Exception ex)
            {
                return Erro<ZonaPatio>($"Erro ao obter zona de pátio: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<ZonaPatio>> CreateZonaPatioAsync(CriarZonaPatioDTO dto)
        {
            try
            {
                var patio = await ObterPatio(dto.PatioId);
                if (patio == null) return Erro<ZonaPatio>(PatioNaoEncontrado);

                var pontoInicial = new Coordenada(dto.CoordenadaInicialX, dto.CoordenadaInicialY);
                var pontoFinal = new Coordenada(dto.CoordenadaFinalX, dto.CoordenadaFinalY);

                var zona = patio.CriarZona(dto.Nome, dto.TipoZona, pontoInicial, pontoFinal, dto.Cor);
                await _patioRepository.SaveChangesAsync();

                return Sucesso(zona, "Zona de pátio criada com sucesso.");
            }
            catch (Exception ex)
            {
                return Erro<ZonaPatio>($"Erro ao criar zona de pátio: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<ZonaPatio>> UpdateZonaPatioAsync(long id, CriarZonaPatioDTO dto)
        {
            try
            {
                var zona = await ObterZona(id);
                if (zona == null) return Erro<ZonaPatio>(ZonaNaoEncontrada);

                if (!string.IsNullOrEmpty(dto.Cor) && dto.Cor != zona.Cor)
                    zona.AlterarCor(dto.Cor);

                var novoPontoInicial = new Coordenada(dto.CoordenadaInicialX, dto.CoordenadaInicialY);
                var novoPontoFinal = new Coordenada(dto.CoordenadaFinalX, dto.CoordenadaFinalY);

                var patio = await ObterPatio(zona.PatioId);
                if (patio == null) return Erro<ZonaPatio>(PatioNaoEncontrado);

                if (!patio.CoordenadaEstaValida(novoPontoInicial) || !patio.CoordenadaEstaValida(novoPontoFinal))
                    return Erro<ZonaPatio>("As coordenadas estão fora dos limites do pátio.");

                zona.RedimensionarZona(novoPontoInicial, novoPontoFinal);
                await _zonaPatioRepository.SaveChangesAsync();

                return Sucesso(zona, "Zona de pátio atualizada com sucesso.");
            }
            catch (Exception ex)
            {
                return Erro<ZonaPatio>($"Erro ao atualizar zona de pátio: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<ZonaPatio>> DeleteZonaPatioAsync(long id)
        {
            try
            {
                var zona = await ObterZona(id);
                if (zona == null) return Erro<ZonaPatio>(ZonaNaoEncontrada);

                if (zona.SensoresRFID.Any())
                    return Erro<ZonaPatio>("Não é possível remover uma zona com sensores RFID associados.");

                await _zonaPatioRepository.RemoveAsync(zona);
                await _zonaPatioRepository.SaveChangesAsync();

                return Sucesso(zona, "Zona de pátio removida com sucesso.");
            }
            catch (Exception ex)
            {
                return Erro<ZonaPatio>($"Erro ao remover zona de pátio: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<double>> CalcularAreaZonaAsync(long zonaId)
        {
            try
            {
                var zona = await ObterZona(zonaId);
                if (zona == null) return Erro<double>(ZonaNaoEncontrada);

                return Sucesso(zona.CalcularArea());
            }
            catch (Exception ex)
            {
                return Erro<double>($"Erro ao calcular área da zona: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<Coordenada>> ObterCentroZonaAsync(long zonaId)
        {
            try
            {
                var zona = await ObterZona(zonaId);
                if (zona == null) return Erro<Coordenada>(ZonaNaoEncontrada);

                return Sucesso(zona.ObterCentroZona());
            }
            catch (Exception ex)
            {
                return Erro<Coordenada>($"Erro ao obter centro da zona: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<bool>> VerificarPosicaoNaZonaAsync(long zonaId, double x, double y)
        {
            try
            {
                var zona = await ObterZona(zonaId);
                if (zona == null) return Erro<bool>(ZonaNaoEncontrada);

                var posicao = new Coordenada(x, y);
                return Sucesso(zona.ContemPosicao(posicao));
            }
            catch (Exception ex)
            {
                return Erro<bool>($"Erro ao verificar posição na zona: {ex.Message}");
            }
        }

        private async Task<bool> ZonaPatioExistsAsync(long id)
        {
            try
            {
                ZonaPatio zona = await _zonaPatioRepository.GetByIdAsync(id);
                return zona != null;
            }
            catch
            {
                return false;
            }
        }
    }
}