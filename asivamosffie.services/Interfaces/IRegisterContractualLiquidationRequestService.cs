using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;


namespace asivamosffie.services.Interfaces
{
    public interface IRegisterContractualLiquidationRequestService
    {
        //Consultas
        Task<List<VContratacionProyectoSolicitudLiquidacion>> GridRegisterContractualLiquidationObra(int pMenuId);
        Task<List<VContratacionProyectoSolicitudLiquidacion>> GridRegisterContractualLiquidationInterventoria(int pMenuId);
        Task<List<dynamic>> GetContratacionByContratacionId(int pContratacionId);
        Task<List<dynamic>> GridInformeFinal(int pContratacionId, int pMenuId);
        Task<List<dynamic>> GetInformeFinalByProyectoId(int pProyectoId, int pContratacionId, int pMenuId);
        Task<List<InformeFinalInterventoria>> GetInformeFinalAnexoByInformeFinalId(int pInformeFinalId);
        Task<dynamic> GetObservacionLiquidacionContratacionByMenuIdAndContratacionId(int pMenuId, int pContratacionId, int pPadreId, string pTipoObservacionCodigo);
        Task<Respuesta> CreateUpdateLiquidacionContratacionObservacion(LiquidacionContratacionObservacion pLiquidacionContratacionObservacion);
        Task<Respuesta> ChangeStatusLiquidacionContratacion(Contratacion pContratacion, int menuId);
        Task<dynamic> GetHistoricoObservacionLiquidacionContratacionByMenuIdAndContratacionId(int pMenuId, int pContratacionId, int pPadreId, string pTipoObservacionCodigo);
        Task<ContratoPoliza> GetContratoPoliza(int pContratoPolizaId, int pMenuId, int pContratacionId);
        Task<dynamic> GetBalanceByContratacionId(int pContratacionId, int pMenuId);

        //alertas
        Task<bool> RegistroLiquidacionPendiente();
        Task<bool> RegistroLiquidacionPendienteAprobacion();
        Task<bool> RegistroLiquidacionPendienteEnviarLiquidacion();


    }
}
