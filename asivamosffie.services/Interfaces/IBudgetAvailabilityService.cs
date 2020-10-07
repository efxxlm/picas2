using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;


namespace asivamosffie.services.Interfaces
{
    public interface IBudgetAvailabilityService
    { 
        Task<List<DisponibilidadPresupuestalGrilla>> GetListDisponibilidadPresupuestal();


        Task<List<DisponibilidadPresupuestalGrilla>> GetListDisponibilidadPresupuestalByCodigoEstadoSolicitud(string pCodigoEstadoSolicitud);
        Task<List<EstadosDisponibilidad>> GetListGenerarDisponibilidadPresupuestal();
        Task<Respuesta> SetCancelDisponibilidadPresupuestal(DisponibilidadPresupuestalObservacion pDisponibilidadPresObservacion);
        Task<Respuesta> CreateDDP(int pId, string pUsuarioModificacion, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender);
        Task<Respuesta> returnDDP(DisponibilidadPresupuestalObservacion pDisponibilidadPresObservacion);
        Task<Byte[]> GetPDFDDP(int id,string pUsurioGenero);
        Task<List<GrillaDisponibilidadPresupuestal>> GetGridBudgetAvailability(int? DisponibilidadPresupuestalId);
        Task<DisponibilidadPresupuestal> GetBudgetAvailabilityById(int id);

        Task<Respuesta> CreateEditarDisponibilidadPresupuestal(DisponibilidadPresupuestal DP);
        Task<Respuesta> SetReturnValidacionDDP(DisponibilidadPresupuestalObservacion pDisponibilidadPresObservacion, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender);
        Task<Respuesta> SetRechazarValidacionDDP(DisponibilidadPresupuestalObservacion pDisponibilidadPresObservacion, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender);
        Task<Respuesta> SetValidarValidacionDDP(int pId, string pUsuarioModificacion, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender);
        Task<Respuesta> CreateFinancialFundingGestion(GestionFuenteFinanciacion pDisponibilidadPresObservacion);
        Task<Respuesta> DeleteFinancialFundingGestion(int pIdDisponibilidadPresObservacion, string usuarioModificacion);
        Task<Respuesta> GetFinancialFundingGestionByDDPP(int pIdDisponibilidadPresupuestalProyecto, string usuarioModificacion);
        Task<Respuesta> SetCancelRegistroPresupuestal(DisponibilidadPresupuestalObservacion pDisponibilidadPresObservacion);
        Task<Respuesta> CreateDRP(int id, string usuarioModificacion, string mailServer, int mailPort, bool enableSSL, string password, string sender);
        Task<EstadosDisponibilidad> GetListGenerarRegistroPresupuestal();
    }
}
