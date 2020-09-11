﻿using asivamosffie.model.APIModels;
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
        Task<Respuesta> CreateOrEditInfoAdditional(PostParameter postParameter, string user);
        Task<DisponibilidadPresupuestal> GetDetailInfoAdditionalById(int disponibilidadPresupuestalId);
        Task<Respuesta> CreateOrEditDDPRequest(DisponibilidadPresupuestal disponibilidadPresupuestal);
        Task<List<CustonReuestCommittee>> GetReuestCommittee();
        CustonReuestCommittee MapToValue(SqlDataReader reader);
        Task<HTMLContent> GetHTMLString(DetailValidarDisponibilidadPresupuesal detailValidarDisponibilidadPresupuesal);

    }
    
}
