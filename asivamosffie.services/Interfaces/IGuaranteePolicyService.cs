using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IGuaranteePolicyService
    {
        Task<Respuesta> CreateEditPolizaObservacion(PolizaObservacion pPolizaObservacion, AppSettingsService appSettingsService);

        Task<Respuesta> InsertContratoPoliza(ContratoPoliza contratoPoliza, AppSettingsService appSettingsService);

        Task<Respuesta> EditarContratoPoliza(ContratoPoliza contratoPoliza);

        Task<List<VistaContratoGarantiaPoliza>> ListVistaContratoGarantiaPoliza(int pContratoId);

        Task<Respuesta> CambiarEstadoPoliza(int pContratoPolizaId, string pCodigoNuevoEstadoPoliza, string pUsuarioModifica);

        Task<Respuesta> CambiarEstadoPolizaByContratoId(int pContratoId, string pCodigoNuevoEstadoPoliza, string pUsuarioModifica);

        Task<bool> ConsultarRegistroCompletoCumple(int ContratoPolizaId);

        Task<List<VGestionarGarantiasPolizas>> ListGrillaContratoGarantiaPolizaOptz();

        Task<List<GrillaContratoGarantiaPoliza>> ListGrillaContratoGarantiaPoliza();

        Task<List<PolizaGarantia>> GetListPolizaGarantiaByContratoPolizaId(int pContratoPolizaId);

        Task<List<PolizaObservacion>> GetListPolizaObservacionByContratoPolizaId(int pContratoPolizaId);

        Task<ContratoPoliza> GetContratoPolizaByIdContratoPolizaId(int pContratoPolizaId);

        Task<ContratoPoliza> GetContratoPolizaByIdContratoId(int pContratoId);

        Task<NotificacionMensajeGestionPoliza> GetNotificacionContratoPolizaByIdContratoId(int pContratoId);

        Task<Respuesta> AprobarContratoByIdContrato(int pIdContrato, AppSettingsService settings, string pUsuario);

        Task EnviarCorreoSupervisor4dPolizaNoAprobada2(string dominioFront, string mailServer, int mailPort, bool enableSSL, string password, string sender);

        Task EnviarCorreoSupervisor4dPolizaNoAprobada(string dominioFront, string mailServer, int mailPort, bool enableSSL, string password, string sender);

        Task<Respuesta> InsertEditPolizaObservacion(PolizaObservacion polizaObservacion, AppSettingsService appSettingsService);

        Task<Respuesta> InsertEditPolizaGarantia(PolizaGarantia polizaGarantia);

    }
}
