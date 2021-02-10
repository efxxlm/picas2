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
using Z.EntityFramework.Plus;

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

        public async Task<Respuesta> CreateUpdateSolicitudPagoObservacion(SolicitudPagoObservacion pSolicitudPagoObservacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Actualizar_Solicitud_Pago_Observacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                if (pSolicitudPagoObservacion.SolicitudPagoObservacionId > 0)
                {
                    SolicitudPagoObservacion solicitudPagoObservacionOld = _context.SolicitudPagoObservacion.Find(pSolicitudPagoObservacion.SolicitudPagoObservacionId);

                    solicitudPagoObservacionOld.FechaModificacion = DateTime.Now;
                    solicitudPagoObservacionOld.UsuarioModificacion = pSolicitudPagoObservacion.UsuarioCreacion;
                    solicitudPagoObservacionOld.Archivada = false;

                    solicitudPagoObservacionOld.RegistroCompleto = ValidateCompleteRecordSolicitudPagoObservacion(pSolicitudPagoObservacion);
                    solicitudPagoObservacionOld.TieneObservacion = pSolicitudPagoObservacion.TieneObservacion;
                    solicitudPagoObservacionOld.Observacion = pSolicitudPagoObservacion.Observacion;
                }
                else
                {
                    pSolicitudPagoObservacion.FechaCreacion = DateTime.Now;
                    pSolicitudPagoObservacion.Eliminado = true;
                    pSolicitudPagoObservacion.RegistroCompleto = ValidateCompleteRecordSolicitudPagoObservacion(pSolicitudPagoObservacion);

                    _context.SolicitudPagoObservacion.Add(pSolicitudPagoObservacion);
                }

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

        public async Task<dynamic> GetObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(int pMenuId, int pSolicitudPagoId, int pPadreId)
        {
            return await _context.SolicitudPagoObservacion
                                           .Where(s => s.MenuId == pMenuId && s.SolicitudPagoId == pSolicitudPagoId && s.IdPadre == pPadreId)
                                                                                                                       .Select(p => new
                                                                                                                       {
                                                                                                                           p.TieneObservacion,
                                                                                                                           p.Archivada,
                                                                                                                           p.FechaCreacion,
                                                                                                                           p.Observacion,
                                                                                                                           p.RegistroCompleto
                                                                                                                       }).ToListAsync();
        }

        public async Task<dynamic> GetListSolicitudPago(int pMenuId)
        {
            var result = await _context.SolicitudPago.Where(s => s.Eliminado != true)
                .Include(r => r.Contrato)
                             .Select(s => new
                             {
                                 s.TipoSolicitudCodigo,
                                 s.FechaCreacion,
                                 s.NumeroSolicitud,
                                 s.Contrato.ModalidadCodigo,
                                 s.Contrato.NumeroContrato,
                                 s.EstadoCodigo,
                                 s.ContratoId,
                                 s.SolicitudPagoId,
                                 RegistroCompleto = s.RegistroCompleto ?? false
                             }).OrderByDescending(r => r.SolicitudPagoId).ToListAsync();

            List<dynamic> grind = new List<dynamic>();
            List<Dominio> ListParametricas = _context.Dominio.Where(d => d.TipoDominioId == (int)EnumeratorTipoDominio.Modalidad_Contrato || d.TipoDominioId == (int)EnumeratorTipoDominio.Estados_Solicitud_Pago).ToList();

            result.ForEach(r =>
            {
                grind.Add(new
                {
                    r.RegistroCompleto,
                    r.TipoSolicitudCodigo,
                    r.ContratoId,
                    r.SolicitudPagoId,
                    r.FechaCreacion,
                    r.NumeroSolicitud,
                    NumeroContrato = r.NumeroContrato ?? "No Aplica",
                    Estado = !string.IsNullOrEmpty(r.EstadoCodigo) ? ListParametricas.Where(l => l.Codigo == r.EstadoCodigo && l.TipoDominioId == (int)EnumeratorTipoDominio.Estados_Solicitud_Pago).FirstOrDefault().Nombre : " - ",
                    Modalidad = !string.IsNullOrEmpty(r.ModalidadCodigo) ? ListParametricas.Where(l => l.Codigo == r.ModalidadCodigo && l.TipoDominioId == (int)EnumeratorTipoDominio.Modalidad_Contrato).FirstOrDefault().Nombre : "No aplica"
                });
            });
            return grind;
        }

        public async Task<Respuesta> ChangueStatusSolicitudPago(SolicitudPago pSolicitudPago)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Solicitud_Pago, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                await _context.Set<SolicitudPago>()
                                       .Where(o => o.SolicitudPagoId == pSolicitudPago.SolicitudPagoId)
                                                                                                       .UpdateAsync(r => new SolicitudPago()
                                                                                                       {
                                                                                                           FechaModificacion = DateTime.Now,
                                                                                                           UsuarioModificacion = pSolicitudPago.UsuarioCreacion,
                                                                                                           EstadoCodigo = pSolicitudPago.EstadoCodigo
                                                                                                       });

                string strEstadoSolicitudPago = _context.Dominio.Where(
                                                                          d => d.TipoDominioId == (int)EnumeratorTipoDominio.Estados_Solicitud_Pago
                                                                       && d.Codigo == pSolicitudPago.EstadoCodigo)
                                                                                                                  .FirstOrDefault().Nombre;

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_requisitos_de_pago, GeneralCodes.OperacionExitosa, idAccion, pSolicitudPago.UsuarioCreacion, "EL ESTADO DE LA SOLICITUD DE PAGO CAMBIO A: " + strEstadoSolicitudPago.ToUpper())
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_requisitos_de_pago, GeneralCodes.Error, idAccion, pSolicitudPago.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

    }
}