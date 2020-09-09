using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IRequestBudgetAvailabilityService
    {
        Task<ActionResult<List<SesionComiteTecnicoCompromiso>>> GetManagementCommitteeReportById(int SesionComiteTecnicoCompromisoId);
        //Task<ActionResult<List<SesionComiteTecnicoCompromiso>>> GetManagementCommitteeReport();
        Task<ActionResult<List<GridReuestCommittee>>> GetReuestCommittee();
        Task<Respuesta> CreateOrEditReportProgress(CompromisoSeguimiento compromisoSeguimiento);
    }
    
}
