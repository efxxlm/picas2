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
        string getNombreAportante(CofinanciacionAportante confinanciacion);

        Task<DisponibilidadPresupuestal> GetDisponibilidadPresupuestalByID(int id); 
        Task<List<DisponibilidadPresupuestalGrilla>> GetListDisponibilidadPresupuestal();
        Task<List<DisponibilidadPresupuestalGrilla>> GetListDisponibilidadPresupuestalByCodigoEstadoSolicitud(string pCodigoEstadoSolicitud);
        Task<List<EstadosDisponibilidad>> GetListGenerarDisponibilidadPresupuestal();
        Task<Respuesta> SetCancelDisponibilidadPresupuestal(DisponibilidadPresupuestalObservacion pDisponibilidadPresObservacion
            , string urlDestino, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender);
        Task<Respuesta> CreateDDP(int pId, string pUsuarioModificacionCreateDDP, bool esNovedad, int RegistroPresupuestalId, string purl, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender);
        Task<Respuesta> returnDDP(DisponibilidadPresupuestalObservacion pDisponibilidadPresObservacion);
        Task<Byte[]> GetPDFDDP(int id, string pUsurioGenero, bool esNovedad, int pRegistroPresupuestalId);
        Task<List<GrillaDisponibilidadPresupuestal>> GetGridBudgetAvailability(int? DisponibilidadPresupuestalId);
        Task<DisponibilidadPresupuestal> GetBudgetAvailabilityById(int id);

        Task<Respuesta> CreateEditarDisponibilidadPresupuestal(DisponibilidadPresupuestal DP);
        Task<Respuesta> SetReturnValidacionDDP(DisponibilidadPresupuestalObservacion pDisponibilidadPresObservacion, bool esNovedad, int RegistroPresupuestalId
            , string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender);
        Task<Respuesta> SetRechazarValidacionDDP(DisponibilidadPresupuestalObservacion pDisponibilidadPresObservacion, bool esNovedad
            , string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender);
        Task<Respuesta> SetValidarValidacionDDP(int pId, string pUsuarioModificacion, bool esNovedad, int RegistroPresupuestalId
            , string pDominioFront,string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender);
        Task<Respuesta> CreateFinancialFundingGestion(GestionFuenteFinanciacion pDisponibilidadPresObservacion);
        Task<Respuesta> DeleteFinancialFundingGestion(int pIdDisponibilidadPresObservacion, string usuarioModificacion);
        Task<Respuesta> GetFinancialFundingGestionByDDPP(int pIdDisponibilidadPresupuestalProyecto, string usuarioModificacion);
        Task<Respuesta> SetCancelRegistroPresupuestal(DisponibilidadPresupuestalObservacion pDisponibilidadPresObservacion
            , string urlDestino, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender);
        Task<Respuesta> CreateDRP(int id, string usuarioModificacion, bool esNovedad, int RegistroPresupuestalId, string urlDestino, string mailServer, int mailPort, bool enableSSL, string password, string sender);
        Task<EstadosDisponibilidad> GetListGenerarRegistroPresupuestal();
        Task<Byte[]> GetPDFDRP(int id, string usuarioModificacion, bool esNovedad, int pRegistroPresupuestalId);
         
    }
}
