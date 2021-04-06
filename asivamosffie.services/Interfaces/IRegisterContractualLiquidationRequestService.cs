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
        Task<List<VContratacionProyectoSolicitudLiquidacion>> GridRegisterContractualLiquidationObra();
        Task<List<VContratacionProyectoSolicitudLiquidacion>> GridRegisterContractualLiquidationInterventoria();
        Task<VContratacionProyectoSolicitudLiquidacion> GetContratacionProyectoByContratacionProyectoId(int pContratacionProyectoId);
        Task<List<dynamic>> GridInformeFinal(int pContratacionProyectoId);
        Task<List<dynamic>> GetInformeFinalByProyectoId(int pProyectoId);
        Task<List<InformeFinalInterventoria>> GetInformeFinalAnexoByInformeFinalId(int pInformeFinalId);
        Task<dynamic> GetObservacionLiquidacionContratacionByMenuIdAndContratacionProyectoId(int pMenuId, int pContratacionProyectoId, int pPadreId, string pTipoObservacionCodigo);
        Task<Respuesta> CreateUpdateLiquidacionContratacionObservacion(LiquidacionContratacionObservacion pLiquidacionContratacionObservacion);
        Task<Respuesta> ChangeStatusLiquidacionContratacionProyecto(ContratacionProyecto pContratacionProyecto, int menuId);

    }
}
