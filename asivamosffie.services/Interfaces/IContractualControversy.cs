using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IContractualControversy
    {
        Task<Respuesta> CreateEditarControversiaTAI(ControversiaContractual controversiaContractual);

        Task<List<GrillaTipoSolicitudControversiaContractual>> ListGrillaTipoSolicitudControversiaContractual();
    }
}
