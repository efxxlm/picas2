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
        Task<ActionResult<List<ComiteTecnico>>> GetRequestCommitteeSessionById(int comiteTecnicoId);
        Task<Respuesta> CreateOrEditTema(SesionComiteTema sesionComiteTema, DateTime fechaComite);
        Task<ActionResult<List<SesionComiteTema>>> GetCommitteeSessionByComiteTecnicoId(int comiteTecnicoId);
        Task<ActionResult<List<ComiteTecnico>>> GetCommitteeSession();

        Task<Respuesta> CallCommitteeSession(int comiteTecnicoId, string user);
        Task<bool> DeleteTema(int sesionTemaId, string user);
        #endregion



        #region "SESIONES DE COMITE FIDUCIARIO";
        Task<ActionResult<List<ComiteTecnico>>> GetConvokeSessionFiduciario(int? estadoComiteCodigo);
        Task<ActionResult<List<Usuario>>> GetListParticipantes();
        #endregion



        Task<Respuesta> CreateOrEditCommitteeSession(SesionComiteTema sesionComiteTema);
        Task<Respuesta> CreateOrEditGuest(SesionInvitado sesionInvitado);
        Task<Respuesta> CreateOrEditSesioncomment(SesionComentario sesionComentario);
        Task<Respuesta> CreateOrEditSubjects(TemaCompromiso temaCompromiso);
        
        Task<SesionInvitado> GetSesionGuesById(int sesionInvitadoId);
        Task<ActionResult<List<ComiteTecnico>>> GetSesionSinActa();
        Task<ActionResult<List<GridCommitteeSession>>> GetCommitteeSessionFiduciario();
        Task<ActionResult<IEnumerable<GridCommitteeSession>>> GetCommitteeSessionTemaById(int sessionTemaId);
        Task<bool> SessionPostpone(int ComiteTecnicoId, DateTime newDate, string usuarioModifico);
        Task<bool> SessionDeclaredFailed(int ComiteTecnicoId, string usuarioModifico);
         Task<List<GridValidationRequests>> GetValidationRequests();
        Task<ActionResult<List<GridValidationRequests>>> GetValidationRequests(string tipoSolicitudCodigo);
        Task<Respuesta> CreateOrEditVotacionSolicitud(List<SesionSolicitudVoto> listSolicitudVoto);
        Task<Respuesta> CreateOrEditInvitedMembers(SesionParticipante sesionParticipante);
        Task<ActionResult<List<GridComiteTecnicoCompromiso>>> GetCompromisosSolicitud();

    }
}
