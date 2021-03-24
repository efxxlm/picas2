using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;

namespace asivamosffie.services.Interfaces
{
    public interface IPaymentRequierementsService
    {
        Task<bool> SolicitudPagoPendienteVerificacion();

        Task<bool> SolicitudPagoPendienteAutorizacion();

        Task<Respuesta> CreateEditObservacionFinancieraListaChequeo(List<SolicitudPagoListaChequeo> pSolicitudPagoListaChequeo , string pAuthor);
       
        Task<Respuesta> CreateUpdateSolicitudPagoObservacion(SolicitudPagoObservacion pSolicitudPagoObservacion);

        Task<dynamic> GetObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(int pMenuId, int pSolicitudPagoId, int pPadreId   , string pTipoObservacionCodigo);

        Task<dynamic> GetListSolicitudPago(int pMenuId);

        Task<Respuesta> ChangueStatusSolicitudPago(SolicitudPago pSolicitudPago);
    }
}
