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

        Task<Contrato> GetContratoByContratoId(int pContratoId);

        Task<dynamic> GetProyectosByIdContrato(int pContratoId);
    }
}
