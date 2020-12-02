using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface ICommitteeSessionFiduciarioService
    {

        #region "ORDEN DEL DIA";
        Task<List<dynamic>> GetCommitteeSessionFiduciario();
        Task<Respuesta> CreateEditComiteTecnicoAndSesionComiteTemaAndSesionComiteSolicitud(ComiteTecnico pComiteTecnico);
        Task<List<ComiteGrilla>> GetCommitteeSession();
        Task<ComiteTecnico> GetRequestCommitteeSessionById(int comiteTecnicoId);
        Task<Respuesta> ConvocarComiteTecnico(ComiteTecnico pComiteTecnico, string pDominio, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender);
        Task<ComiteTecnico> GetComiteTecnicoByComiteTecnicoId(int pComiteTecnicoId);
        Task<byte[]> GetPlantillaByTablaIdRegistroId(string pTablaId, int pRegistroId);
        Task<Respuesta> CreateEditSesionComiteTema(List<SesionComiteTema> ListSesionComiteTemas);
        Task<Respuesta> AplazarSesionComite(ComiteTecnico pComiteTecnico, string pDominio, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender);
        Task<List<SesionParticipante>> GetSesionParticipantesByIdComite(int pComiteId);
        Task<Respuesta> DeleteSesionInvitado(int pSesionInvitadoId, string pUsuarioModificacion);
        Task<Respuesta> CreateEditSesionInvitadoAndParticipante(ComiteTecnico pComiteTecnico);
        Task<Respuesta> GetNoRequiereVotacionSesionComiteSolicitud(SesionComiteSolicitud pSesionComiteSolicitud);
        Task<Respuesta> NoRequiereVotacionSesionComiteTema(int idSesionComiteTema, bool pRequiereVotacion, string pUsuarioCreacion);
        Task<Respuesta> CreateEditSesionSolicitudVoto(SesionComiteSolicitud pSesionComiteSolicitud);
        Task<Respuesta> CambiarEstadoComiteTecnico(ComiteTecnico pComiteTecnico);
        Task<byte[]> GetPlantillaActaIdComite(int ComiteId);
        Task<Respuesta> CreateEditActasSesionSolicitudCompromiso(SesionComiteSolicitud pSesionComiteSolicitud);
        Task<Respuesta> CreateEditTemasCompromiso(SesionComiteTema pSesionComiteTema);
        Task<Respuesta> CreateEditSesionTemaVoto(SesionComiteTema pSesionComiteTema);
        Task<ComiteTecnico> GetCompromisosByComiteTecnicoId(int ComiteTecnicoId);
        Task<Respuesta> VerificarTemasCompromisos(ComiteTecnico pComiteTecnico);
        Task<Respuesta> DeleteComiteTecnicoByComiteTecnicoId(int pComiteTecnicoId, string pUsuarioModifico);
        Task<ProcesoSeleccionMonitoreo> GetProcesoSeleccionMonitoreo( int pProcesoSeleccionMonitoreoId );

        #endregion

    }
}
