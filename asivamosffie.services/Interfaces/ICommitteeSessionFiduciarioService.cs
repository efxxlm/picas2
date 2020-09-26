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

        // Task<Respuesta> CreateOrEditTema(SesionComiteTema sesionComiteTema, DateTime fechaComite);
        // Task<List<SesionComiteTema>> GetCommitteeSessionByComiteTecnicoId(int comiteTecnicoId);
        

        // Task<Respuesta> CallCommitteeSession(int comiteTecnicoId, string user);
        // Task<bool> DeleteTema(int sesionTemaId, string user);
        #endregion



        // #region "SESIONES DE COMITE FIDUCIARIO";
        // Task<List<ComiteTecnico>> GetConvokeSessionFiduciario(int? estadoComiteCodigo);
        // Task<List<Usuario>> GetListParticipantes();
        // #endregion



        
        // Task<Respuesta> CreateOrEditGuest(SesionInvitado sesionInvitado);
        // Task<Respuesta> CreateOrEditSesioncomment(SesionComentario sesionComentario);
        // Task<Respuesta> CreateOrEditSubjects(TemaCompromiso temaCompromiso);
        
        // Task<SesionInvitado> GetSesionGuesById(int sesionInvitadoId);
        // Task<List<ComiteTecnico>> GetSesionSinActa();
        
        // Task<IEnumerable<GridCommitteeSession>> GetCommitteeSessionTemaById(int sessionTemaId);
        // Task<bool> SessionPostpone(int ComiteTecnicoId, DateTime newDate, string usuarioModifico);
        // Task<bool> SessionDeclaredFailed(int ComiteTecnicoId, string usuarioModifico);
        //  Task<List<GridValidationRequests>> GetValidationRequests();
        // Task<Respuesta> CreateOrEditVotacionSolicitud(List<SesionSolicitudVoto> listSolicitudVoto);
        // Task<Respuesta> CreateOrEditInvitedMembers(SesionParticipante sesionParticipante);
        // Task<List<GridComiteTecnicoCompromiso>> GetCompromisosSolicitud();

    }
}
