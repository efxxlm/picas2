using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IManagementCommitteeReportService
    {
        Task<ActionResult<List<SesionComiteTecnicoCompromiso>>> GetManagementCommitteeReport();
        Task<ActionResult<List<SesionComiteTecnicoCompromiso>>> GetManagementCommitteeReportById(int SesionComiteTecnicoCompromisoId);
        Task<Respuesta> CreateOrEditReportProgress(CompromisoSeguimiento compromisoSeguimiento, string estadoCompromiso);
        Task<Respuesta> CreateOrEditCommentReport(SesionComentario SesionComentario);
    }
}
