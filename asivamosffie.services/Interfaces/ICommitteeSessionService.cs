using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface ICommitteeSessionService
    {
        Task<Respuesta> CreateOrEditCommitteeSession(SesionComiteTema sesionComiteTema);
        Task<Respuesta> CreateOrEditGuest(SesionInvitado sesionInvitado);
        Task<Respuesta> CreateOrEditSesioncomment(SesionComentario sesionComentario);
        Task<Respuesta> CreateOrEditSubjects(TemaCompromiso temaCompromiso);
        Task<ActionResult<IEnumerable<GridCommitteeSession>>> GetCommitteeSession(int? sessionId);
        Task<SesionInvitado> GetSesionGuesById(int sesionInvitadoId);
        Task<ActionResult<List<Sesion>>> GetSesionSinActa();
        Task<ActionResult<List<GridCommitteeSession>>> GetCommitteeSessionFiduciario();
        Task<ActionResult<IEnumerable<GridCommitteeSession>>> GetCommitteeSessionTemaById(int sessionTemaId);
        Task<bool> SessionPostpone(int sesionId, DateTime newDate, string usuarioModifico);
        Task<bool> SessionDeclaredFailed(int sesionId, string usuarioModifico);
        Task<bool> DeleteTema(int temaId);

    }
}
