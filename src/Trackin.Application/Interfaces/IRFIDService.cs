using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackin.Application.Common;
using Trackin.Application.DTOs;

namespace Trackin.Application.Interfaces
{
    public interface IRFIDService
    {
        Task<ServiceResponse<LocalizacaoMotoDTO>> ProcessarLeituraRFID(RFIDLeituraDTO leitura);
    }
}
