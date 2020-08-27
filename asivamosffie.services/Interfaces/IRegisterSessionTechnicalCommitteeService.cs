using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;

namespace asivamosffie.services.Interfaces
{
    public interface IRegisterSessionTechnicalCommitteeService
    {
        Task<Respuesta> DeleteSesionInvitado(int pSesionInvitadoId , string pUsuarioModificacion);

        Task<Respuesta> CreateEditComiteTecnicoAndSesionComiteTemaAndSesionComiteSolicitud(ComiteTecnico pComiteTecnico);
      
        Task<Respuesta> CambiarEstadoComiteTecnico(ComiteTecnico pComiteTecnico);
       
        Task<List<dynamic>> GetListSesionComiteSolicitudByFechaOrdenDelDia(DateTime pFechaOrdenDelDia);
       
        Task<ComiteTecnico> GetComiteTecnicoByComiteTecnicoId(int pComiteTecnicoId);
        
        Task<Respuesta> CreateSesionInvitadoAndParticipante(ComiteTecnico pComiteTecnico);
        
        Task<Respuesta> EliminarSesionComiteTema(int pSesionComiteTemaId, string pUsuarioModificacion);
        
        Task<byte[]> GetPlantillaByTablaIdRegistroId(string pTablaId, int pRegistroId);
       
        Task<List<ComiteGrilla>> GetListComiteGrilla();

        Task<List<dynamic>> GetListSesionComiteTemaByComiteTecnicoId(int pComiteTecnicoId);
    }
}
