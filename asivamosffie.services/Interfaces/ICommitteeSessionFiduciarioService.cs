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
        Task<Respuesta> CreateOrEditTema(SesionComiteTema sesionComiteTema);
        #endregion




        Task<Respuesta> CreateOrEditCommitteeSession(SesionComiteTema sesionComiteTema);
        Task<Respuesta> CreateOrEditGuest(SesionInvitado sesionInvitado);
        Task<Respuesta> CreateOrEditSesioncomment(SesionComentario sesionComentario);
        Task<Respuesta> CreateOrEditSubjects(TemaCompromiso temaCompromiso);
        Task<ActionResult<IEnumerable<GridCommitteeSession>>> GetCommitteeSession(int? sessionId);
        Task<SesionInvitado> GetSesionGuesById(int sesionInvitadoId);
        Task<ActionResult<List<ComiteTecnico>>> GetSesionSinActa();
        Task<ActionResult<List<GridCommitteeSession>>> GetCommitteeSessionFiduciario();
        Task<ActionResult<IEnumerable<GridCommitteeSession>>> GetCommitteeSessionTemaById(int sessionTemaId);
        Task<bool> SessionPostpone(int ComiteTecnicoId, DateTime newDate, string usuarioModifico);
        Task<bool> SessionDeclaredFailed(int ComiteTecnicoId, string usuarioModifico);
        Task<bool> DeleteTema(int temaId);

    }
}
