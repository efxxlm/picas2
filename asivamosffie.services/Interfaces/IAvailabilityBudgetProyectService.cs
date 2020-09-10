using asivamosffie.model.APIModels;
using asivamosffie.services.QueryFilters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IAvailabilityBudgetProyectService
    {
        Task<ActionResult<List<GrillaValidarDisponibilidadPresupuesal>>> GetBudgetavailabilityRequests();
        Task<List<DetailValidarDisponibilidadPresupuesal>> GetDetailAvailabilityBudgetProyect(int? rubroAfinanciarId, int disponibilidadPresupuestalId);
        Task<List<DetailValidarDisponibilidadPresupuesal>> GetAvailabilityBudgetDetail(int solicitudId);
        Task<Respuesta> CreateOrEditObservations(ParametersFilter parametersFilter);
        //Task<List<DetailValidarDisponibilidadPresupuesal>> StartDownloadPDF(int? rubroAfinanciarId, int disponibilidadPresupuestalId);
        Task<HTMLContent> GetHTMLString(DetailValidarDisponibilidadPresupuesal detailValidarDisponibilidadPresupuesal);
    }
}
