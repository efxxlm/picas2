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
        Task<ComiteTecnico> GetRequestCommitteeSessionById(int comiteTecnicoId);
        Task<Respuesta> CreateOrEditTema(SesionComiteTema sesionComiteTema, DateTime fechaComite);
        Task<List<SesionComiteTema>> GetCommitteeSessionByComiteTecnicoId(int comiteTecnicoId);
        Task<List<ComiteGrilla>> GetCommitteeSession();

        Task<Respuesta> CallCommitteeSession(int comiteTecnicoId, string user);
        Task<bool> DeleteTema(int sesionTemaId, string user);
        #endregion



        #region "SESIONES DE COMITE FIDUCIARIO";
        Task<List<ComiteTecnico>> GetConvokeSessionFiduciario(int? estadoComiteCodigo);
        Task<List<Usuario>> GetListParticipantes();
        #endregion



        Task<Respuesta> CreateEditComiteTecnicoAndSesionComiteTemaAndSesionComiteSolicitud(ComiteTecnico pComiteTecnico);
        Task<Respuesta> CreateOrEditGuest(SesionInvitado sesionInvitado);
        Task<Respuesta> CreateOrEditSesioncomment(SesionComentario sesionComentario);
        Task<Respuesta> CreateOrEditSubjects(TemaCompromiso temaCompromiso);
        
        Task<SesionInvitado> GetSesionGuesById(int sesionInvitadoId);
        Task<List<ComiteTecnico>> GetSesionSinActa();
        Task<List<dynamic>> GetCommitteeSessionFiduciario();
        Task<IEnumerable<GridCommitteeSession>> GetCommitteeSessionTemaById(int sessionTemaId);
        Task<bool> SessionPostpone(int ComiteTecnicoId, DateTime newDate, string usuarioModifico);
        Task<bool> SessionDeclaredFailed(int ComiteTecnicoId, string usuarioModifico);
         Task<List<GridValidationRequests>> GetValidationRequests();
        Task<Respuesta> CreateOrEditVotacionSolicitud(List<SesionSolicitudVoto> listSolicitudVoto);
        Task<Respuesta> CreateOrEditInvitedMembers(SesionParticipante sesionParticipante);
        Task<List<GridComiteTecnicoCompromiso>> GetCompromisosSolicitud();

    }
}
