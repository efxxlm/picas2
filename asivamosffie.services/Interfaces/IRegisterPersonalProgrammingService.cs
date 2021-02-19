using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;

namespace asivamosffie.services.Interfaces
{
    public interface IRegisterPersonalProgrammingService
    {
        Task<List<VRegistrarPersonalObra>> GetListProyectos();

        Task<List<SeguimientoSemanal>> GetProgramacionPersonalByContratoId(int pContratacionProyectoId, string pUsuario);

        Task<Respuesta> UpdateSeguimientoSemanalPersonalObra(List<SeguimientoSemanal> pSeguimientoSemanal);

        Task<Respuesta> ChangeStatusProgramacionContratoPersonal(int pContratoConstruccionId, string pEstadoProgramacionCodigo, string pUsuario);

        Task TareaProgramada(string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender);
    }
}
