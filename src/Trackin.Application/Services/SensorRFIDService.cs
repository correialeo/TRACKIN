using Trackin.Application.Common;
using Trackin.Application.DTOs;
using Trackin.Application.Interfaces;
using Trackin.Domain.Entity;
using Trackin.Domain.Interfaces;
using Trackin.Domain.ValueObjects;

namespace Trackin.Application.Services
{
    public class SensorRFIDService : ISensorRFIDService
    {
        private readonly ISensorRFIDRepository _sensorRFIDRepository;
        
        private const string SensorNaoEncontrado = "Não existe um sensor RFID cadastrado com o ID informado";
        
        public SensorRFIDService(ISensorRFIDRepository sensorRFIDRepository)
        {
            _sensorRFIDRepository = sensorRFIDRepository;
        }
        
        // Helper privados
        private async Task<SensorRFID?> ObterSensorRFID(long id) => await _sensorRFIDRepository.GetByIdAsync(id);

        private ServiceResponse<T> Sucesso<T>(T data, string message = "") => new() { Success = true, Data = data, Message = message };

        private ServiceResponse<T> Erro<T>(string message) => new() { Success = false, Message = message };


        public async Task<ServiceResponsePaginado<SensorRFID>> GetAllSensoresRFIDPaginatedAsync(
            int pageNumber,
            int pageSize,
            string? ordering = null,
            bool descendingOrder = false)
        {
            try
            {
                (IEnumerable<SensorRFID> items, int totalCount) = await _sensorRFIDRepository.GetAllPaginatedAsync(
                    pageNumber, pageSize, ordering, descendingOrder);

                return new ServiceResponsePaginado<SensorRFID>(items, pageNumber, pageSize, totalCount);
            }
            catch (Exception ex)
            {
                return new ServiceResponsePaginado<SensorRFID>(new List<SensorRFID>(), pageNumber, pageSize, 0)
                {
                    Success = false,
                    Message = $"Erro ao obter sensores RFID paginados: {ex.Message}"
                };
            }
        }


        public async Task<ServiceResponse<IEnumerable<SensorRFID>>> GetAllSensoresRFIDAsync()
        {
            try
            {
                IEnumerable<SensorRFID> sensores = await _sensorRFIDRepository.GetAllAsync();
                if (!sensores.Any())
                    return Erro<IEnumerable<SensorRFID>>("Nenhum sensor RFID encontrado.");

                return Sucesso(sensores);
            }
            catch (Exception ex)
            {
                return Erro<IEnumerable<SensorRFID>>($"Erro ao obter sensores RFID: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<SensorRFID>> GetSensorRFIDByIdAsync(long id)
        {
            var sensor = await ObterSensorRFID(id);
            if (sensor == null) 
                return Erro<SensorRFID>(SensorNaoEncontrado);

            return Sucesso(sensor);
            
        }

        public async Task<ServiceResponse<SensorRFID>> CreateSensorRFIDAsync(CriarSensorRFIdDTO dto)
        {
            try
            {
                Coordenada coordenada = new Coordenada(dto.PosicaoX, dto.PosicaoY);
                SensorRFID sensor = new SensorRFID(dto.ZonaPatioId, dto.PatioId, dto.Posicao, coordenada, dto.Altura, dto.AnguloVisao);


                await _sensorRFIDRepository.AddAsync(sensor);
                await _sensorRFIDRepository.SaveChangesAsync();

                return Sucesso(sensor, "Sensor RFID criado com sucesso.");
            }
            catch (ArgumentException ex)
            {
                return Erro<SensorRFID>($"Dados inválidos: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Erro<SensorRFID>($"Erro ao criar sensor RFID: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<SensorRFID>> UpdateSensorRFIDAsync(long id, CriarSensorRFIdDTO dto)
        {
            var sensor = await ObterSensorRFID(id);
            if (sensor == null) return Erro<SensorRFID>(SensorNaoEncontrado);

            try
            {
                Coordenada novaCoordenada = new Coordenada(dto.PosicaoX, dto.PosicaoY);
                sensor.AtualizarPosicao(novaCoordenada, dto.Altura);

                await _sensorRFIDRepository.SaveChangesAsync();
                return Sucesso(sensor, "Sensor RFID atualizado com sucesso.");
            }
            catch (ArgumentException ex)
            {
                return Erro<SensorRFID>($"Dados inválidos: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return Erro<SensorRFID>($"Operação inválida: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Erro<SensorRFID>($"Erro ao atualizar sensor RFID: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<SensorRFID>> DeleteSensorRFIDAsync(long id)
        {
            var sensor = await ObterSensorRFID(id);
            if (sensor == null) return Erro<SensorRFID>(SensorNaoEncontrado);

            try
            {
                if (sensor.Ativo) sensor.DesativarSensor();
                await _sensorRFIDRepository.RemoveAsync(sensor);
                await _sensorRFIDRepository.SaveChangesAsync();

                return Sucesso(sensor, "Sensor RFID removido com sucesso.");
            }
            catch (InvalidOperationException ex)
            {
                return Erro<SensorRFID>($"Operação inválida: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Erro<SensorRFID>($"Erro ao remover sensor RFID: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<SensorRFID>> AtivarSensorAsync(long id)
        {
            var sensor = await ObterSensorRFID(id);
            if (sensor == null) return Erro<SensorRFID>(SensorNaoEncontrado);

            try
            {
                sensor.AtivarSensor();
                await _sensorRFIDRepository.SaveChangesAsync();

                return Sucesso(sensor, "Sensor RFID ativado com sucesso.");
            }
            catch (InvalidOperationException ex)
            {
                return Erro<SensorRFID>(ex.Message);
            }
            catch (Exception ex)
            {
                return Erro<SensorRFID>($"Erro ao ativar sensor RFID: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<SensorRFID>> DesativarSensorAsync(long id)
        {
            var sensor = await ObterSensorRFID(id);
            if (sensor == null) return Erro<SensorRFID>(SensorNaoEncontrado);

            try
            {
                sensor.DesativarSensor();
                await _sensorRFIDRepository.SaveChangesAsync();

                return Sucesso(sensor, "Sensor RFID desativado com sucesso.");
            }
            catch (InvalidOperationException ex)
            {
                return Erro<SensorRFID>(ex.Message);
            }
            catch (Exception ex)
            {
                return Erro<SensorRFID>($"Erro ao desativar sensor RFID: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<IEnumerable<SensorRFID>>> GetSensoresComProblemaAsync(TimeSpan tempoLimiteSemLeitura)
        {
            try
            {
                var sensores = await _sensorRFIDRepository.GetAllAsync();
                var sensoresComProblema = sensores.Where(s => s.EstaComProblema(tempoLimiteSemLeitura));

                return Sucesso(sensoresComProblema);
            }
            catch (Exception ex)
            {
                return Erro<IEnumerable<SensorRFID>>($"Erro ao obter sensores com problema: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<bool>> VerificarSensorPodeSerUsadoAsync(long id)
        {
            var sensor = await ObterSensorRFID(id);
            if (sensor == null) return Erro<bool>(SensorNaoEncontrado);

            try
            {
                bool podeSerUsado = sensor.PodeLerRFID();
                return Sucesso(podeSerUsado, podeSerUsado ? "Sensor pode ser usado" : "Sensor não pode ser usado (inativo)");
            }
            catch (Exception ex)
            {
                return Erro<bool>($"Erro ao verificar status do sensor: {ex.Message}");
            }
        }

        private async Task<bool> SensorRFIDExistsAsync(long id)
        {
            try
            {
                SensorRFID sensor = await _sensorRFIDRepository.GetByIdAsync(id);
                return sensor != null;
            }
            catch
            {
                return false;
            }
        }
    }
}