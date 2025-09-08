using Trackin.Application.Common;
using Trackin.Domain.Entity;
using System.Threading.Tasks;

namespace Trackin.Application.Interfaces
{
    public interface IMotoImagemService
    {
        Task<ServiceResponse<Moto>> CadastrarImagemReferenciaAsync(long motoId, string imagemReferencia);
    }
}