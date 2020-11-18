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
        
        Task<List<ProgramacionPersonalContrato>> GetProgramacionPersonalByContratoId(int pContratoId, string pUsuario);
        
        Task<Respuesta> UpdateProgramacionContratoPersonal(ContratoConstruccion pContratoConstruccion);
       
        Task<Respuesta> ChangeStatusProgramacionContratoPersonal(int pContratoConstruccionId, string pEstadoProgramacionCodigo, string pUsuario);
    }
}
