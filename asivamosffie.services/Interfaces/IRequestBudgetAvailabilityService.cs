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
        Task<List<DetailValidarDisponibilidadPresupuesal>> GetDetailAvailabilityBudgetProyectNew(int disponibilidadPresupuestalId, bool esNovedad, int RegistroNovedadId, bool esGenerar);
        Task<dynamic> GetListAportanteByTipoAportanteByProyectoId(int pProyectoId, int pTipoAportanteId);
        Task<Respuesta> CreateUpdateDisponibilidaPresupuestalEspecial(DisponibilidadPresupuestal pDisponibilidadPresupuestal);
        Task<Contrato> GetListContatoByNumeroContrato(string pNumeroContrato);
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
        Task<Contrato> GetContratoByNumeroContrato(string pNumero);
        Task<List<ListConcecutivoProyectoAdministrativo>> GetListCocecutivoProyecto();
        Task<Respuesta> SendRequest(int disponibilidadPresupuestalId, int RegistroPId, bool esNovedad, string user, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender);
        Task<List<DetailValidarDisponibilidadPresupuesal>> GetDetailAvailabilityBudgetProyect(int disponibilidadPresupuestalId, bool esNovedad, int RegistroNovedadId);
        Task<Respuesta> CreateOrEditProyectoAdministrtivo(DisponibilidadPresupuestal disponibilidad);
        Task<List<DisponibilidadPresupuestal>> GetDDPAdministrativa();
        Task<Respuesta> EliminarDisponibilidad(int disponibilidadPresupuestalId);
        Task<dynamic> GetContratos();
        Task<dynamic> getNovedadContractualByContratacionId(int contratacionId);
        Task<Respuesta> CreateOrEditInfoAdditionalNoveltly(NovedadContractualRegistroPresupuestal pRegistro, int pContratacionId, string user);
    }
    
}
