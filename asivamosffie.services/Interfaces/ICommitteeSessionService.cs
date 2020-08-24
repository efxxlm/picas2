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
        Task<ActionResult<IEnumerable<GridCommitteeSession>>> GetCommitteeSession(int? sessionId);
        Task<ActionResult<List<GridCommitteeSession>>> GetCommitteeSessionFiduciario();
        Task<ActionResult<IEnumerable<GridCommitteeSession>>> GetCommitteeSessionTemaById(int sessionTemaId);
        Task<bool> DeleteTema(int temaId);
    }
}
