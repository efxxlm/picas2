using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;

namespace asivamosffie.services.Interfaces
{
    public interface IRegisterValidatePaymentRequierementsService
    {
        Task<Respuesta> DeleteSolicitudPagoFaseCriterioConceptoPago(int pSolicitudPagoFaseCriterioConceptoId, bool pEsConcepto, string pUsuarioModificacion);

        Task<dynamic> GetUsoByConceptoPagoCodigo(string pConceptoPagoCodigo);

        Task<dynamic> GetMontoMaximo(int SolicitudPagoId, bool EsPreConstruccion);

        Task<dynamic> GetFormaPagoCodigoByFase(bool pEsPreconstruccion, int pContratoId);

        Task<dynamic> GetMontoMaximoProyecto(int pContrato, int pContratacionProyectoId, bool EsPreConstruccion);

        Task<dynamic> GetMontoMaximoMontoPendiente(int SolicitudPagoId, string strFormaPago, bool EsPreConstruccion, int pContratacionProyectoId, string pCriterioCodigo, string pConceptoCodigo, string pUsoCodigo, string pTipoPago, bool pConNovedad);

        Task<Respuesta> ReturnSolicitudPago(SolicitudPago pSolicitudPago);

        Task GetValidateSolicitudPagoId(int SolicitudPagoId);

        Task<Contrato> GetContratoByContratoId(int pContratoId, int pSolicitudPago, bool esSolicitudPago);

        Task<SolicitudPago> GetSolicitudPago(int pSolicitudPagoId);

        Task<Respuesta> DeleteSolicitudPagoFaseFacturaDescuento(int pSolicitudPagoFaseFacturaDescuentoId, string pUsuarioModificacion);

        Task<dynamic> GetListProyectosByLlaveMen(string pLlaveMen);

        Task<Respuesta> DeleteSolicitudPago(int pSolicitudPagoId, string pUsuarioModificacion);

        Task<Respuesta> DeleteSolicitudLlaveCriterioProyecto(int pContratacionProyectoId, string pUsuarioModificacion);

        Task<Respuesta> DeleteSolicitudPagoFaseCriterioProyecto(int SolicitudPagoFaseCriterioProyectoId, string pUsuarioModificacion);

        Task<Respuesta> DeleteSolicitudPagoFaseCriterio(int pSolicitudPagoFaseCriterioId, string pUsuarioModificacion);

        Task<Respuesta> CreateEditOtrosCostosServicios(SolicitudPago pSolicitudPago);

        Task<Respuesta> CreateEditExpensas(SolicitudPago pSolicitudPago);

        Task<dynamic> GetConceptoPagoCriterioCodigoByTipoPagoCodigo(string TipoPagoCodigo);

        Task<dynamic> GetTipoPagoByCriterioCodigo(string pCriterioCodigo);

        Task<dynamic> GetListSolicitudPago();

        Task<dynamic> GetCriterioByFormaPagoCodigo(string pFormaPagoCodigo);

        Task<Respuesta> CreateEditNewPayment(SolicitudPago pSolicitudPago);

        Task<dynamic> GetContratoByTipoSolicitudCodigoModalidadContratoCodigoOrNumeroContrato(string pTipoSolicitud, string pModalidadContrato, string pNumeroContrato);

        Task<dynamic> GetUsoByConceptoPagoCriterioCodigo(string pConceptoPagoCodigo, int pContratoId);

        Task<dynamic> GetProyectosByIdContrato(int pContratoId);

        SolicitudPago GetSolicitudPagoComplete(SolicitudPago solicitudPago);

        List<TablaDRP> GetDrpContrato(Contrato contrato);

        dynamic GetDrpContratoGeneral(int pContratacionId, bool esSolicitudPago);

    }
}
