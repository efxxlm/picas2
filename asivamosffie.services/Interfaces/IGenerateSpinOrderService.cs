using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;

namespace asivamosffie.services.Interfaces
{
    public interface IGenerateSpinOrderService
    {
        Task<Respuesta> DeleteOrdenGiroDetalleTerceroCausacionDescuento(List<int> pOrdenGiroDetalleTerceroCausacionDescuentoId, string pAuthor);

        Task<Respuesta> DeleteOrdenGiroDetalleTerceroCausacionAportante(int pOrdenGiroDetalleTerceroCausacionAportanteId, string pAuthor);

        Task<Respuesta> DeleteOrdenGiroDetalleDescuentoTecnica(int pOrdenGiroDetalleDescuentoTecnicaId, string pAuthor);

        Task<Respuesta> DeleteOrdenGiro(int OrdenGiroId, string pAuthor);

        Task<Respuesta> DeleteOrdenGiroDetalleDescuentoTecnicaAportante(int pOrdenGiroDetalleDescuentoTecnicaAportanteId, string pAuthor);

        Task<dynamic> GetFuentesDeRecursosPorAportanteId(int pAportanteId);

        Task<dynamic> GetValorConceptoByAportanteId(int pAportanteId, int pSolicitudPagoId, string pConceptoPago);

        Task<SolicitudPago> GetSolicitudPagoBySolicitudPagoId(int pOrdenGiroId);

        Task<dynamic> GetListOrdenGiro(int pMenuId);

        Task<Respuesta> CreateEditOrdenGiro(OrdenGiro pOrdenGiro);
    }
}
