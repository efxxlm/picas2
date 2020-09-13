using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.PostParameters;
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
        Task<ActionResult<List<ListAportantes>>> GetAportantesByProyectoId(int proyectoId);
        Task<Respuesta> CreateOrEditReportProgress(CompromisoSeguimiento compromisoSeguimiento);
        Task<Respuesta> CreateOrEditInfoAdditional(DisponibilidadPresupuestal pDisponibilidad, string user);
        Task<DisponibilidadPresupuestal> GetDetailInfoAdditionalById(int disponibilidadPresupuestalId);
        Task<Respuesta> CreateOrEditDDPRequest(DisponibilidadPresupuestal disponibilidadPresupuestal, int proyectoId, int disponibilidadPresupuestalId);
        Task<ActionResult<List<Proyecto>>> SearchLlaveMEN(string LlaveMEN);
        Task<List<CustonReuestCommittee>> GetReuestCommittee();
        Task<ActionResult<List<DisponibilidadPresupuestal>>> GetDDPEspecial();
        CustonReuestCommittee MapToValue(SqlDataReader reader);
        Task<HTMLContent> GetHTMLString(DetailValidarDisponibilidadPresupuesal detailValidarDisponibilidadPresupuesal);
        Task<Respuesta> CreateOrEditServiceCosts(DisponibilidadPresupuestal disponibilidadPresupuestal, int proyectoId);
        Task<Respuesta> SendRequest(int disponibilidadPresupuestalId);
    }
    
}
