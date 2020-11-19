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
        
        Task<List<SeguimientoSemanal>> GetProgramacionPersonalByContratoId(int pContratoId, string pUsuario);
        
        Task<Respuesta> UpdateSeguimientoSemanalPersonalObra(SeguimientoSemanal pSeguimientoSemanal);
       
        Task<Respuesta> ChangeStatusProgramacionContratoPersonal(int pContratoConstruccionId, string pEstadoProgramacionCodigo, string pUsuario);
    }
}
