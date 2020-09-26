using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IManagementCommitteeReportService
    {
        Task<ActionResult<List<GrillaSesionComiteTecnicoCompromiso>>> GetManagementCommitteeReport(int pUserId);

        Task<ActionResult<List<GrillaSesionComiteTecnicoCompromiso>>> GetManagementCommitteeReportById(int sesionComiteTecnicoCompromisoId);
        Task<Respuesta> CreateOrEditReportProgress(CompromisoSeguimiento compromisoSeguimiento, string estadoCompromiso);
        Task<Respuesta> CreateOrEditCommentReport(SesionComentario SesionComentario);
        Task<ActionResult<List<ComiteTecnico>>> GetManagementReport();
        Task<ActionResult<List<ComiteTecnico>>> GetManagementReportById(int comiteTecnicoId);
        //Task<ActionResult<List<ComiteTecnico>>> GetManagementReport(int comiteTecnicoId);
        Task<bool> UpdateStatus(int sesionComiteTecnicoCompromisoId, string status);
        Task<Respuesta> AcceptReport(int comiteTecnicoId, Usuario puser);
        Task<HTMLContent> GetHTMLString(ActaComite obj);
    }
}
