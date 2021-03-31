using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;


namespace asivamosffie.services.Interfaces
{
    public interface IDailyFollowUpService
    {
        Task<List<VProyectosXcontrato>> gridRegisterDailyFollowUp(int usuarioId);
        Task<Respuesta> CreateEditDailyFollowUp( SeguimientoDiario pSeguimientoDiario );
        Task<SeguimientoDiario> GetDailyFollowUpById( int pId );
        Task<List<SeguimientoDiario>> GetDailyFollowUpByContratacionProyectoId( int pId );
        Task<List<string>> GetDatesAvailableByContratacioProyectoId( int pId );
        Task<Respuesta> DeleteDailyFollowUp( int pId, string pUsuario );
        Task<Respuesta> SendToSupervisionSupport( int pId, string pUsuario );
        Task<List<VProyectosXcontrato>> gridVerifyDailyFollowUp(int usuarioId);
        Task<Respuesta> CreateEditObservacion( SeguimientoDiario pSeguimientoDiario, bool esSupervisor );
        Task<Respuesta> SendToSupervision( int pId, string pUsuario );
        Task<List<VProyectosXcontrato>> gridValidateDailyFollowUp(int usuarioId);
        Task<Respuesta> ApproveDailyFollowUp(int pId, string pUsuario);
        Task<Respuesta> ReturnToComptroller(int pId, string pUsuario);
    }
}
