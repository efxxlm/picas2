using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace asivamosffie.services
{
    public class PaymentRequierementsService : IPaymentRequierementsService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly IDocumentService _documentService;

        public PaymentRequierementsService(IDocumentService documentService, devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _documentService = documentService;
            _commonService = commonService;
            _context = context;
        }
     
        private bool ValidateCompleteRecordSolicitudPagoObservacion(SolicitudPagoObservacion pSolicitudPagoObservacion)
        {
            if (!pSolicitudPagoObservacion.TieneObservacion)
            {
                return true;
            }
            else
            {
                if (!string.IsNullOrEmpty(pSolicitudPagoObservacion.Observacion))
                    return true;
            }

            return false;
        }
   
        public async Task<Respuesta> CreateUpdateSolicitudPagoObservacion(SolicitudPagoObservacion pSolicitudPagoObservacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Actualizar_Solicitud_Pago_Observacion, (int)EnumeratorTipoDominio.Acciones);
             
            try
            {
                bool RegistroCompletoObservacion = ValidateCompleteRecordSolicitudPagoObservacion(pSolicitudPagoObservacion);

                CreateOrUpdateSolicitudPagoObservacion(pSolicitudPagoObservacion);

                ActualizarSolicitudPagoTieneObservacion(pSolicitudPagoObservacion, pSolicitudPagoObservacion.TieneObservacion);

                
                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_requisitos_de_pago, GeneralCodes.OperacionExitosa, idAccion, pSolicitudPagoObservacion.UsuarioCreacion, "CREAR OBSERVACION SOLICITUD PAGO")
                    };
            }
            catch (Exception ex)
            {
                return
                    new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = true,
                        IsValidation = false,
                        Code = GeneralCodes.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_requisitos_de_pago, GeneralCodes.Error, idAccion, pSolicitudPagoObservacion.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }
     
        private void CreateOrUpdateSolicitudPagoObservacion(SolicitudPagoObservacion pSolicitudPagoObservacion)
        {
            if (pSolicitudPagoObservacion.SolicitudPagoObservacionId > 0)
            {
                SolicitudPagoObservacion solicitudPagoObservacionOld = _context.SolicitudPagoObservacion.Find(pSolicitudPagoObservacion.SolicitudPagoObservacionId);
                solicitudPagoObservacionOld.FechaModificacion = DateTime.Now;
                solicitudPagoObservacionOld.UsuarioModificacion = pSolicitudPagoObservacion.UsuarioCreacion;

                solicitudPagoObservacionOld.TieneObservacion = pSolicitudPagoObservacion.TieneObservacion;
                solicitudPagoObservacionOld.Observacion = pSolicitudPagoObservacion.Observacion;
            }
            else
            {
                pSolicitudPagoObservacion.FechaCreacion = DateTime.Now;
                pSolicitudPagoObservacion.Eliminado = true;
            }
        }
    
        private void ActualizarSolicitudPagoTieneObservacion(SolicitudPagoObservacion pSolicitudPagoObservacion, bool TieneObservacion)
        {
            SolicitudPago solicitudPago = _context.SolicitudPago.Find(pSolicitudPagoObservacion.SolicitudPagoId);
            solicitudPago.FechaModificacion = DateTime.Now;
            solicitudPago.UsuarioModificacion = pSolicitudPagoObservacion.UsuarioCreacion;

            if (TieneObservacion)
                solicitudPago.TieneObservacion = true;
            else
            {
                if (_context.SolicitudPagoObservacion.Where(r => r.SolicitudPagoId == pSolicitudPagoObservacion.SolicitudPagoId && (bool)r.TieneObservacion).Count() > 0)
                    solicitudPago.TieneObservacion = true;
                else
                    solicitudPago.TieneObservacion = false;
            }

        }
    }
}