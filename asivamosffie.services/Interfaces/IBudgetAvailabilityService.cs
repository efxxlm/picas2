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

        Task<FuenteFinanciacion> GetFuenteFinanciacionByIdAportanteId(int pAportanteId);

        Task<List<DisponibilidadPresupuestalGrilla>> GetListDisponibilidadPresupuestalByCodigoEstadoSolicitud(string pCodigoEstadoSolicitud);
        Task<List<EstadosDisponibilidad>> GetListGenerarDisponibilidadPresupuestal();
        Task<Respuesta> SetCancelDisponibilidadPresupuestal(int pId,string pUsuarioModificacion, string pObservacion);
        Task<Respuesta> CreateDDP(int pId, string pUsuarioModificacion, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender);
        Task<Respuesta> returnDDP(int pId, string pUsuarioModificacion, string pObservacion);
    }
}
