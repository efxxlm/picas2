using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IRequestBudgetAvailabilityService
    {
        Task<List<ListAportantes>> GetAportantesByProyectoId(int proyectoId);
        Task<List<ListAdminProyect>> GetAportantesByProyectoAdministrativoId(int proyectoId);
        Task<Respuesta> CreateOrEditReportProgress(CompromisoSeguimiento compromisoSeguimiento);
        Task<Respuesta> CreateOrEditInfoAdditional(DisponibilidadPresupuestal pDisponibilidad, string user);
        Task<DisponibilidadPresupuestal> GetDetailInfoAdditionalById(int disponibilidadPresupuestalId);
        Task<Respuesta> CreateOrEditDDPRequest(DisponibilidadPresupuestal disponibilidadPresupuestal);
        Task<List<Proyecto>> SearchLlaveMEN(string LlaveMEN);
        Task<List<CustonReuestCommittee>> GetReuestCommittee();
        Task<List<DisponibilidadPresupuestal>> GetDDPEspecial();
        CustonReuestCommittee MapToValue(SqlDataReader reader);
        Task<HTMLContent> GetHTMLString(DetailValidarDisponibilidadPresupuesal detailValidarDisponibilidadPresupuesal);
        Task<ActionResult<List<GrillaValidarDisponibilidadPresupuesal>>> GetBudgetavailabilityRequests();
        Task<Respuesta> CreateOrEditServiceCosts(DisponibilidadPresupuestal disponibilidadPresupuestal, int proyectoId);
        Task<List<ListConcecutivoProyectoAdministrativo>> GetListCocecutivoProyecto();
        Task<Respuesta> SendRequest(int disponibilidadPresupuestalId);
        Task<List<DetailValidarDisponibilidadPresupuesal>> GetDetailAvailabilityBudgetProyect(int? rubroAfinanciarId, int disponibilidadPresupuestalId);
        Task<Respuesta> CreateOrEditProyectoAdministrtivo(DisponibilidadPresupuestal disponibilidad);
        Task<List<DisponibilidadPresupuestal>> GetDDPAdministrativa();
        Task<Respuesta> EliminarDisponibilidad(int disponibilidadPresupuestalId);
    }
    
}
