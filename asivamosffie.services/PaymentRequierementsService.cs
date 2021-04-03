﻿using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        private readonly IRegisterValidatePaymentRequierementsService _registerValidatePaymentRequierementsService;
        public PaymentRequierementsService(IRegisterValidatePaymentRequierementsService registerValidatePaymentRequierementsService, IDocumentService documentService, devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _registerValidatePaymentRequierementsService = registerValidatePaymentRequierementsService;
            _documentService = documentService;
            _commonService = commonService;
            _context = context;
        }

        #region Solicitud Pago

        public async Task<dynamic> GetObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(int pMenuId, int pSolicitudPagoId, int pPadreId, string pTipoObservacionCodigo)
        {
            return await _context.SolicitudPagoObservacion
                                           .Where(s => s.MenuId == pMenuId
                                               && s.SolicitudPagoId == pSolicitudPagoId
                                               && s.IdPadre == pPadreId
                                               && s.TipoObservacionCodigo == pTipoObservacionCodigo)
                                            .Select(p => new
                                            {
                                                p.SolicitudPagoObservacionId,
                                                p.TieneObservacion,
                                                p.Archivada,
                                                p.FechaCreacion,
                                                p.Observacion,
                                                p.RegistroCompleto
                                            }).ToListAsync();
        }

        public async Task<dynamic> GetListSolicitudPago(int pMenuId)
        {
            List<VSolicitudPago> result = new List<VSolicitudPago>();

            switch (pMenuId)
            {
                case (int)enumeratorMenu.Verificar_solicitud_de_pago:
                    result = await _context.VSolicitudPago.Where(s =>
                            s.IntEstadoCodigo >= (int)EnumEstadoSolicitudPago.Enviado_para_verificacion)
                                                    .OrderByDescending(r => r.FechaModificacion)
                                                    .ToListAsync();
                    break;

                case (int)enumeratorMenu.Autorizar_solicitud_de_pago:
                    result = await _context.VSolicitudPago.Where(s =>
                       s.IntEstadoCodigo >= (int)EnumEstadoSolicitudPago.Enviada_para_autorizacion)
                                               .OrderByDescending(r => r.FechaModificacion)
                                               .ToListAsync();
                    break;

                case (int)enumeratorMenu.Verificar_Financieramente_Solicitud_De_Pago:
                    result = await _context.VSolicitudPago.Where(s =>
                       s.IntEstadoCodigo >= (int)EnumEstadoSolicitudPago.Enviada_Verificacion_Financiera)
                                               .OrderByDescending(r => r.FechaModificacion)
                                               .ToListAsync();
                    break;


                case (int)enumeratorMenu.Validar_Financieramente_Solicitud_De_Pago:
                    result = await _context.VSolicitudPago.Where(s =>
                       s.IntEstadoCodigo >= (int)EnumEstadoSolicitudPago.Enviada_Validacion_Financiera)
                                               .OrderByDescending(r => r.FechaModificacion)
                                               .ToListAsync();
                    break;

                default:
                    break;
            }

            return result.Select(r => new
            {

                r.TieneObservacion,
                r.TieneSubsanacion,
                r.RegistroCompletoAutorizar,
                r.RegistroCompletoVerificar,
                r.TipoSolicitudCodigo,
                r.ContratoId,
                r.SolicitudPagoId,
                r.FechaCreacion,
                r.NumeroSolicitud,
                r.NumeroContrato,
                r.EstadoNombre,
                r.EstadoNombre2,
                r.EstadoCodigo,
                r.ModalidadNombre,
                r.RegistroCompletoVerificacionFinanciera,
                r.RegistroCompletoValidacionFinanciera,
                r.EstaRechazada
            });

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

        public async Task<Respuesta> CreateUpdateSolicitudPagoObservacion(SolicitudPagoObservacion pSolicitudPagoObservacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Actualizar_Solicitud_Pago_Observacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                if (pSolicitudPagoObservacion.SolicitudPagoObservacionId > 0)
                {
                    if (pSolicitudPagoObservacion.Archivada == null)
                        pSolicitudPagoObservacion.Archivada = false;

                    await _context.Set<SolicitudPagoObservacion>()
                                  .Where(o => o.SolicitudPagoObservacionId == pSolicitudPagoObservacion.SolicitudPagoObservacionId)
                                  .UpdateAsync(r => new SolicitudPagoObservacion()
                                  {
                                      Archivada = pSolicitudPagoObservacion.Archivada,
                                      FechaModificacion = DateTime.Now,
                                      UsuarioModificacion = pSolicitudPagoObservacion.UsuarioCreacion,
                                      RegistroCompleto = ValidateCompleteRecordSolicitudPagoObservacion(pSolicitudPagoObservacion),
                                      TieneObservacion = pSolicitudPagoObservacion.TieneObservacion,
                                      Observacion = pSolicitudPagoObservacion.Observacion,
                                  });
                }
                else
                {
                    pSolicitudPagoObservacion.Archivada = false;
                    pSolicitudPagoObservacion.FechaCreacion = DateTime.Now;
                    pSolicitudPagoObservacion.Eliminado = false;
                    pSolicitudPagoObservacion.RegistroCompleto = ValidateCompleteRecordSolicitudPagoObservacion(pSolicitudPagoObservacion);

                    _context.Entry(pSolicitudPagoObservacion).State = EntityState.Added;
                    _context.SaveChanges();
                }

                Respuesta respuesta =
                   new Respuesta
                   {
                       IsSuccessful = true,
                       IsException = false,
                       IsValidation = false,
                       Code = GeneralCodes.OperacionExitosa,
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_requisitos_de_pago, GeneralCodes.OperacionExitosa, idAccion, pSolicitudPagoObservacion.UsuarioCreacion, "CREAR OBSERVACION SOLICITUD PAGO")
                   };

                if (pSolicitudPagoObservacion.Archivada != true)
                    await ValidateCompleteObservation(pSolicitudPagoObservacion, pSolicitudPagoObservacion.UsuarioCreacion);

                return respuesta;
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

        private async Task<bool> ValidateCompleteObservation(SolicitudPagoObservacion pSolicitudPagoObservacion, string pUsuarioMod)
        {
            try
            {
                SolicitudPago solicitudPago = await _registerValidatePaymentRequierementsService.GetSolicitudPago(pSolicitudPagoObservacion.SolicitudPagoId);
                //Sumar cantidad Listas de chequeo
                int intCantidadDependenciasSolicitudPago = solicitudPago.SolicitudPagoListaChequeo.Count(r => r.Eliminado == false);

                if (pSolicitudPagoObservacion.MenuId == (int)enumeratorMenu.Verificar_solicitud_de_pago
                    || pSolicitudPagoObservacion.MenuId == (int)enumeratorMenu.Autorizar_solicitud_de_pago)
                    intCantidadDependenciasSolicitudPago = CantidadDependenciasSolicitudPago(solicitudPago);


                int intCantidadObservacionesSolicitudPago = _context.SolicitudPagoObservacion.Where(r => r.SolicitudPagoId == pSolicitudPagoObservacion.SolicitudPagoId
                                                              && r.MenuId == pSolicitudPagoObservacion.MenuId
                                                              && r.Eliminado != true
                                                              && r.RegistroCompleto == true
                                                              && r.Archivada != true).Count();

                bool TieneObservacion =
                                 _context.SolicitudPagoObservacion.Any
                                                            (r => r.SolicitudPagoId == pSolicitudPagoObservacion.SolicitudPagoId
                                                            && r.MenuId == pSolicitudPagoObservacion.MenuId
                                                            && r.Eliminado != true
                                                            && r.Archivada != true
                                                            && r.TieneObservacion == true
                                                            );


                //Valida si la cantidad de relaciones de solicitud Pago
                //es igual a la cantidad de observaciones de esa Solicitud pago 
                bool blRegistroCompleto = false;
                DateTime? FechaRegistroCompleto = null;
                if (intCantidadObservacionesSolicitudPago >= intCantidadDependenciasSolicitudPago)
                {
                    FechaRegistroCompleto = DateTime.Now;
                    blRegistroCompleto = true;
                }
                 
                switch (pSolicitudPagoObservacion.MenuId)
                {
                    case (int)enumeratorMenu.Verificar_solicitud_de_pago:
                        await _context.Set<SolicitudPago>()
                        .Where(o => o.SolicitudPagoId == pSolicitudPagoObservacion.SolicitudPagoId)
                        .UpdateAsync(r => new SolicitudPago()
                        {
                            TieneObservacion = TieneObservacion,
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = pUsuarioMod,
                            EstadoCodigo = ((int)EnumEstadoSolicitudPago.En_proceso_de_verificacion).ToString(),
                            RegistroCompletoVerificar = blRegistroCompleto,
                            FechaRegistroCompletoVerificar = FechaRegistroCompleto
                        });
                        break;

                    case (int)enumeratorMenu.Autorizar_solicitud_de_pago:
                        await _context.Set<SolicitudPago>()
                        .Where(o => o.SolicitudPagoId == pSolicitudPagoObservacion.SolicitudPagoId)
                        .UpdateAsync(r => new SolicitudPago()
                        {
                            TieneObservacion = TieneObservacion,
                            EstadoCodigo = ((int)EnumEstadoSolicitudPago.En_proceso_de_autorizacion).ToString(),
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = pUsuarioMod,
                            RegistroCompletoAutorizar = blRegistroCompleto,
                            FechaRegistroCompletoAutorizar = FechaRegistroCompleto
                        });
                        break;

                    case (int)enumeratorMenu.Verificar_Financieramente_Solicitud_De_Pago:
                        await _context.Set<SolicitudPago>()
                       .Where(o => o.SolicitudPagoId == pSolicitudPagoObservacion.SolicitudPagoId)
                       .UpdateAsync(r => new SolicitudPago()
                       {
                           TieneObservacion = TieneObservacion,
                           EstadoCodigo = ((int)EnumEstadoSolicitudPago.En_Proceso_Verificacion_Financiera).ToString(),
                           FechaModificacion = DateTime.Now,
                           UsuarioModificacion = pUsuarioMod,
                           RegistroCompletoVerificacionFinanciera = blRegistroCompleto,
                           FechaRegistroCompletoVerificacionFinanciera = FechaRegistroCompleto
                       });
                        break;

                    case (int)enumeratorMenu.Validar_Financieramente_Solicitud_De_Pago:
                        await _context.Set<SolicitudPago>()
                       .Where(o => o.SolicitudPagoId == pSolicitudPagoObservacion.SolicitudPagoId)
                       .UpdateAsync(r => new SolicitudPago()
                       {
                           TieneObservacion = TieneObservacion,
                           EstadoCodigo = ((int)EnumEstadoSolicitudPago.En_Proceso_Validacion_Financiera).ToString(),
                           FechaModificacion = DateTime.Now,
                           UsuarioModificacion = pUsuarioMod,
                           RegistroCompletoValidacionFinanciera = blRegistroCompleto,
                           FechaRegistroCompletoValidacionFinanciera = FechaRegistroCompleto
                       });
                        break;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private int CantidadDependenciasSolicitudPago(SolicitudPago solicitudPago)
        {
            switch (solicitudPago.TipoSolicitudCodigo)
            {
                case ConstanCodigoTipoSolicitudContratoSolicitudPago.Contratos_Interventoria:
                case ConstanCodigoTipoSolicitudContratoSolicitudPago.Contratos_Obra:

                    return CantidadDependenciasTipoInterventoriaObra(solicitudPago);

                case ConstanCodigoTipoSolicitudContratoSolicitudPago.Expensas:
                case ConstanCodigoTipoSolicitudContratoSolicitudPago.Otros_Costos_Servicios:
                    return 2;
                default: return 0;
            }

        }

        private int CantidadDependenciasTipoInterventoriaObra(SolicitudPago pSolicitudPago)
        {
            //#2 pSolicitudPago.SolicitudPagoSoporteSolicitud  
            int intCantidadDependenciasSolicitudPago = 2;

            foreach (var SolicitudPagoRegistrarSolicitudPago in pSolicitudPago.SolicitudPagoRegistrarSolicitudPago.Where(r => r.Eliminado != true))
            {
                //#4 Registrar  Solicitud de pago
                intCantidadDependenciasSolicitudPago++;

                foreach (var SolicitudPagoFase in SolicitudPagoRegistrarSolicitudPago.SolicitudPagoFase.Where(r => r.Eliminado != true))
                {
                    //#5 Criterios de pago
                    intCantidadDependenciasSolicitudPago++;

                    foreach (var SolicitudPagoFaseAmortizacion in SolicitudPagoFase.SolicitudPagoFaseAmortizacion.Where(r => r.Eliminado != true))
                    {
                        //#6 Observacion Amortizacion
                        intCantidadDependenciasSolicitudPago++;
                    }
                    foreach (var SolicitudPagoFaseFactura in SolicitudPagoFase.SolicitudPagoFaseFactura.Where(r => r.Eliminado != true))
                    {
                        //#7 Factura para proyectos asociados
                        intCantidadDependenciasSolicitudPago++;

                        //#7 Factura Descuentos de la Dirección Técnica
                        intCantidadDependenciasSolicitudPago++;

                        intCantidadDependenciasSolicitudPago++;
                    }

                }
            }

            return intCantidadDependenciasSolicitudPago;
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

        private void ArchivarSolicitudPagoObservacion(SolicitudPago pSolicitudPago)
        {
            _context.Set<SolicitudPagoObservacion>()
                    .Where(s => s.SolicitudPagoId == pSolicitudPago.SolicitudPagoId
                              && s.TieneObservacion == false)
                    .Update(s => new SolicitudPagoObservacion()
                    {
                        Archivada = true,
                        FechaModificacion = DateTime.Now,
                        UsuarioModificacion = pSolicitudPago.UsuarioCreacion
                    });

            _context.Set<SolicitudPago>()
                .Where(s => s.SolicitudPagoId == pSolicitudPago.SolicitudPagoId)
                .Update(s => new SolicitudPago
                {
                    RegistroCompleto = false,
                    FechaRegistroCompleto = null,

                    RegistroCompletoVerificar = false,
                    FechaRegistroCompletoVerificar = null,

                    RegistroCompletoAutorizar = false,
                    FechaRegistroCompletoAutorizar = null,

                    RegistroCompletoVerificacionFinanciera = false,
                    FechaRegistroCompletoVerificacionFinanciera = null,

                    RegistroCompletoValidacionFinanciera = false,
                    FechaRegistroCompletoValidacionFinanciera = null,

                    FechaModificacion = DateTime.Now,
                    UsuarioModificacion = pSolicitudPago.UsuarioCreacion
                });
        }
        #endregion

        #region Financiera
        private void ActualizarSacFinanciera(SolicitudPago pSolicitudPago)
        {
            _context.Set<SolicitudPago>()
                                         .Where(o => o.SolicitudPagoId == pSolicitudPago.SolicitudPagoId)
                                                                                                         .Update(r => new SolicitudPago()
                                                                                                         {
                                                                                                             FechaRadicacionSacFinanciera = pSolicitudPago.FechaRadicacionSacFinanciera,
                                                                                                             NumeroRadicacionSacFinanciera = pSolicitudPago.NumeroRadicacionSacFinanciera,
                                                                                                             FechaAsignacionSacFinanciera = DateTime.Now
                                                                                                         });
        }

        private void ActualizarSubsanacion(SolicitudPago pSolicitudPago)
        {
            _context.Set<SolicitudPago>()
                                         .Where(o => o.SolicitudPagoId == pSolicitudPago.SolicitudPagoId)
                                                                                                         .Update(r => new SolicitudPago()
                                                                                                         {
                                                                                                             FechaSubsanacion = DateTime.Now
                                                                                                         });
        }

        private void ActualizarSacTecnica(SolicitudPago pSolicitudPago)
        {
            _context.Set<SolicitudPago>()
                    .Where(o => o.SolicitudPagoId == pSolicitudPago.SolicitudPagoId)
                    .Update(r => new SolicitudPago()
                    {
                        FechaRadicacionSacTecnica = pSolicitudPago.FechaRadicacionSacTecnica,
                        NumeroRadicacionSacTecnica = pSolicitudPago.NumeroRadicacionSacTecnica,
                        ObservacionRadicacionSacTecnica = pSolicitudPago.ObservacionRadicacionSacTecnica,
                        FechaModificacion = DateTime.Now,
                        UsuarioModificacion = pSolicitudPago.UsuarioCreacion
                    });
        }

        private bool ValidarRegistroCompletoSolicitudPagoListaChequeoRespuesta(SolicitudPagoListaChequeoRespuesta pSolicitudPagoListaChequeoRespuestaNew, bool pEsEsValidacion)
        {
            if (pEsEsValidacion)
            {
                if (string.IsNullOrEmpty(pSolicitudPagoListaChequeoRespuestaNew.ValidacionRespuestaCodigo))
                    return false;

                if (pSolicitudPagoListaChequeoRespuestaNew.ValidacionRespuestaCodigo == ConstanCodigoRespuestasListaChequeoSolictudPago.No_cumple)
                    if (string.IsNullOrEmpty(pSolicitudPagoListaChequeoRespuestaNew.ValidacionObservacion))
                        return false;
            }
            else
            {
                if (string.IsNullOrEmpty(pSolicitudPagoListaChequeoRespuestaNew.VerificacionRespuestaCodigo))
                    return false;

                if (pSolicitudPagoListaChequeoRespuestaNew.VerificacionRespuestaCodigo == ConstanCodigoRespuestasListaChequeoSolictudPago.No_cumple)
                    if (string.IsNullOrEmpty(pSolicitudPagoListaChequeoRespuestaNew.VerificacionObservacion))
                        return false;
            }
            return true;
        }

        public async Task<Respuesta> CreateEditObservacionFinancieraListaChequeo(List<SolicitudPagoListaChequeo> SolicitudPagoListaChequeo, string pAuthor)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Verificar_Solicitud_Financiera, (int)EnumeratorTipoDominio.Acciones);
            bool blRegistroCompleto = true;
            bool blTieneSubsanacion = SolicitudPagoListaChequeo.Any(r => r.SolicitudPagoListaChequeoRespuesta.Any(s => s.TieneSubsanacion == true));

            bool blrechazado = SolicitudPagoListaChequeo
             .Any(r => r.SolicitudPagoListaChequeoRespuesta
             .Any(s => s.TieneSubsanacion != true &&
                      (s.ValidacionRespuestaCodigo == ConstanCodigoRespuestasListaChequeoSolictudPago.No_cumple
                    || s.VerificacionRespuestaCodigo == ConstanCodigoRespuestasListaChequeoSolictudPago.No_cumple
                   )
             ));

            try
            {
                foreach (var pSolicitudPagoListaChequeo in SolicitudPagoListaChequeo)
                {
                    if (pSolicitudPagoListaChequeo.EsValidacion)
                    {
                        idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Validar_Solicitud_Financiera, (int)EnumeratorTipoDominio.Acciones);

                        if (pSolicitudPagoListaChequeo.SolicitudPagoListaChequeoRespuesta.Count() != pSolicitudPagoListaChequeo.SolicitudPagoListaChequeoRespuesta.Where(r => r.ValidacionRespuestaCodigo != null).ToList().Count())
                            blRegistroCompleto = false;

                        if (pSolicitudPagoListaChequeo.SolicitudPagoListaChequeoRespuesta.Any(s => s.ValidacionRespuestaCodigo == ConstanCodigoRespuestasSolicitudPago.No_Cumple))
                            blRegistroCompleto = false;

                        pSolicitudPagoListaChequeo.SolicitudPagoListaChequeoRespuesta
                                                  .ToList().ForEach(res =>
                                                  {
                                                      bool blRegistroCompletoItem = ValidarRegistroCompletoSolicitudPagoListaChequeoRespuesta(res, pSolicitudPagoListaChequeo.EsValidacion);

                                                      if (!blRegistroCompletoItem)
                                                          blRegistroCompleto = false;

                                                      _context.Set<SolicitudPagoListaChequeoRespuesta>()
                                                              .Where(s => s.SolicitudPagoListaChequeoRespuestaId == res.SolicitudPagoListaChequeoRespuestaId)
                                                              .Update(s => new SolicitudPagoListaChequeoRespuesta
                                                              {
                                                                  UsuarioModificacion = pSolicitudPagoListaChequeo.UsuarioCreacion,
                                                                  FechaModificacion = DateTime.Now,
                                                                  TieneSubsanacion = res.TieneSubsanacion,
                                                                  RegistroCompletoValidar = blRegistroCompletoItem,
                                                                  ValidacionRespuestaCodigo = res.ValidacionRespuestaCodigo,
                                                                  ValidacionObservacion = res.ValidacionObservacion
                                                              });
                                                  });
                    }
                    else
                    {
                        if (pSolicitudPagoListaChequeo.SolicitudPagoListaChequeoRespuesta.Count() != pSolicitudPagoListaChequeo.SolicitudPagoListaChequeoRespuesta.Where(r => r.VerificacionRespuestaCodigo != null).ToList().Count())
                            blRegistroCompleto = false;

                        if (pSolicitudPagoListaChequeo.SolicitudPagoListaChequeoRespuesta.Any(s => s.VerificacionRespuestaCodigo == ConstanCodigoRespuestasSolicitudPago.No_Cumple))
                            blRegistroCompleto = false;


                        pSolicitudPagoListaChequeo.SolicitudPagoListaChequeoRespuesta
                                                    .ToList().ForEach(res =>
                                                    {
                                                        bool blRegistroCompletoItem = ValidarRegistroCompletoSolicitudPagoListaChequeoRespuesta(res, pSolicitudPagoListaChequeo.EsValidacion);

                                                        if (!blRegistroCompletoItem)
                                                            blRegistroCompleto = false;

                                                        _context.Set<SolicitudPagoListaChequeoRespuesta>()
                                                                .Where(s => s.SolicitudPagoListaChequeoRespuestaId == res.SolicitudPagoListaChequeoRespuestaId)
                                                                .Update(s => new SolicitudPagoListaChequeoRespuesta
                                                                {
                                                                    UsuarioModificacion = pSolicitudPagoListaChequeo.UsuarioCreacion,
                                                                    FechaModificacion = DateTime.Now,
                                                                    RegistroCompletoVerificar = blRegistroCompletoItem,
                                                                    TieneSubsanacion = res.TieneSubsanacion,
                                                                    VerificacionRespuestaCodigo = res.VerificacionRespuestaCodigo,
                                                                    VerificacionObservacion = res.VerificacionObservacion
                                                                });
                                                    });
                    }
                    DateTime? FechaRegistroCompleto = null;
                    if (blRegistroCompleto)
                        FechaRegistroCompleto = DateTime.Now;

                    if (pSolicitudPagoListaChequeo.EsValidacion)
                    {
                        _context.Set<SolicitudPago>()
                                 .Where(s => s.SolicitudPagoId == pSolicitudPagoListaChequeo.SolicitudPagoId)
                                 .Update(s => new SolicitudPago
                                 {
                                     FechaRegistroCompletoValidacionFinanciera = FechaRegistroCompleto,
                                     RegistroCompletoValidacionFinanciera = blRegistroCompleto,
                                     TieneSubsanacion = blTieneSubsanacion,
                                     EstaRechazada = blrechazado,
                                     EstadoCodigo = ((int)EnumEstadoSolicitudPago.En_Proceso_Validacion_Financiera).ToString(),
                                     UsuarioModificacion = pSolicitudPagoListaChequeo.UsuarioCreacion,
                                     FechaModificacion = DateTime.Now
                                 });
                    }
                    else
                    {
                        _context.Set<SolicitudPago>()
                                   .Where(s => s.SolicitudPagoId == pSolicitudPagoListaChequeo.SolicitudPagoId)
                                   .Update(s => new SolicitudPago
                                   {
                                       EstadoCodigo = ((int)EnumEstadoSolicitudPago.En_Proceso_Verificacion_Financiera).ToString(),
                                       FechaRegistroCompletoVerificacionFinanciera = FechaRegistroCompleto,
                                       RegistroCompletoVerificacionFinanciera = blRegistroCompleto,
                                       TieneSubsanacion = blTieneSubsanacion,
                                       EstaRechazada = blrechazado,
                                       UsuarioModificacion = pSolicitudPagoListaChequeo.UsuarioCreacion,
                                       FechaModificacion = DateTime.Now
                                   });
                    }
                }
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_requisitos_de_pago, GeneralCodes.Error, idAccion, pAuthor, ex.InnerException.ToString())
                    };
            }
            return
                   new Respuesta
                   {
                       IsSuccessful = true,
                       IsException = false,
                       IsValidation = false,
                       Code = GeneralCodes.OperacionExitosa,
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_requisitos_de_pago, GeneralCodes.OperacionExitosa, idAccion, pAuthor, "CREAR EDITAR LISTA CHEQUEO FINANCIERA")
                   };

        }

        #endregion

        #region  Emails
        public async Task<Respuesta> ChangueStatusSolicitudPago(SolicitudPago pSolicitudPago)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Solicitud_Pago, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                int intEstadoCodigo = Int32.Parse(pSolicitudPago.EstadoCodigo);

                ///4.1.7
                if (intEstadoCodigo == (int)EnumEstadoSolicitudPago.Enviado_para_verificacion)
                    await SendEmailToAproved(pSolicitudPago.SolicitudPagoId);

                ///4.1.7
                if (intEstadoCodigo == (int)EnumEstadoSolicitudPago.Solicitud_devuelta_por_equipo_facturacion)
                    await SendEmailToDecline(pSolicitudPago.SolicitudPagoId);

                ///4.1.8
                if (intEstadoCodigo == (int)EnumEstadoSolicitudPago.Enviada_para_autorizacion)
                    await SendEmailToAprovedVerify(pSolicitudPago.SolicitudPagoId);

                ///4.1.8
                if (intEstadoCodigo == (int)EnumEstadoSolicitudPago.Solicitud_devuelta_por_apoyo_a_la_supervision)
                {
                    ArchivarSolicitudPagoObservacion(pSolicitudPago);
                    await SendEmailToDeclineVerify(pSolicitudPago.SolicitudPagoId);
                }

                ///4.1.9 
                if (intEstadoCodigo == (int)EnumEstadoSolicitudPago.Enviada_Verificacion_Financiera)
                {
                    await SendEmailToAprovedValidate(pSolicitudPago.SolicitudPagoId);
                    await SendEmailToAprovedValidateAll(pSolicitudPago.SolicitudPagoId);
                    ActualizarSacFinanciera(pSolicitudPago);
                }

                ///4.1.9
                if (intEstadoCodigo == (int)EnumEstadoSolicitudPago.Solicitud_devuelta_por_coordinardor)
                {
                    ArchivarSolicitudPagoObservacion(pSolicitudPago);
                    await SendEmailToDeclineValidate(pSolicitudPago.SolicitudPagoId);
                }

                ///4.3.1
                if (intEstadoCodigo == (int)EnumEstadoSolicitudPago.Solicitud_Rechazado_por_verificacion_financiera)
                {
                    ActualizarSacTecnica(pSolicitudPago);
                    await SendEmailRejectAutorizar(pSolicitudPago.SolicitudPagoId, true);
                }

                ///4.3.1
                if (intEstadoCodigo == (int)EnumEstadoSolicitudPago.Enviada_Validacion_Financiera)
                    await SendEmailAprovedVerificar(pSolicitudPago.SolicitudPagoId);

                ///4.3.1  ///4.3.2
                if (intEstadoCodigo == (int)EnumEstadoSolicitudPago.Enviada_para_subsanacion_por_verificacion_financiera
                 || intEstadoCodigo == (int)EnumEstadoSolicitudPago.Enviada_para_subsanacion_por_validaccion_financiera
                    )
                {
                    ArchivarSolicitudPagoObservacion(pSolicitudPago);
                    await SendEmailRejectedCorrectALL(pSolicitudPago.SolicitudPagoId);
                    await SendEmailRejectedCorrect(pSolicitudPago.SolicitudPagoId);
                    ActualizarSubsanacion(pSolicitudPago);
                }

                ///4.3.2
                if (intEstadoCodigo == (int)EnumEstadoSolicitudPago.Solicitud_Rechazado_por_validacion_financiera)
                {
                    ActualizarSacTecnica(pSolicitudPago);
                    await SendEmailAprovedValidar(pSolicitudPago.SolicitudPagoId);
                }
                ///4.3.2
                if (intEstadoCodigo == (int)EnumEstadoSolicitudPago.Solicitud_Rechazado_por_validacion_financiera)
                    await SendEmailRejectAutorizar(pSolicitudPago.SolicitudPagoId, false);

                ///4.3.2
                if (intEstadoCodigo == (int)EnumEstadoSolicitudPago.Enviada_A_Order_Giro)
                    await SendEmailAprovedValidar(pSolicitudPago.SolicitudPagoId);


                _context.Set<SolicitudPago>()
                                      .Where(o => o.SolicitudPagoId == pSolicitudPago.SolicitudPagoId)
                                                                                                      .Update(r => new SolicitudPago()
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


        ///4.3.2 Aprobar *
        private async Task<bool> SendEmailAprovedValidar(int pSolicitudPagoId)
        {
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.Aprobar_4_3_2));
            string strContenido = ReplaceVariablesSolicitudPago(template.Contenido, pSolicitudPagoId);

            List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                          {
                                                EnumeratorPerfil.Validacion_Financiera
                                          };
            return _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto);
        }

        ///4.3.1 Aprobar *
        private async Task<bool> SendEmailAprovedVerificar(int pSolicitudPagoId)
        {
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.Enviar_Validacion_4_3_1));
            string strContenido = ReplaceVariablesSolicitudPago(template.Contenido, pSolicitudPagoId);

            List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                          {
                                                EnumeratorPerfil.Validacion_Financiera
                                          };
            return _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto);
        }

        ///4.3.1  y 4.3.2 envian subsanacion *
        private async Task<bool> SendEmailRejectedCorrect(int pSolicitudPagoId)
        {
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.Enviar_Subsanacion_4_3_1_Y_4_3_));
            string strContenido = ReplaceVariablesSolicitudPago(template.Contenido, pSolicitudPagoId);

            List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                          {
                                                EnumeratorPerfil.Supervisor,
                                                EnumeratorPerfil.Apoyo,
                                                EnumeratorPerfil.Apoyo_Supervisor
                                          };
            return _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto);
        }

        ///4.3.1  y 4.3.2 envian subsanacion *
        private async Task<bool> SendEmailRejectedCorrectALL(int pSolicitudPagoId)
        {
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.Enviar_Subsanacion_TODOS_4_3_1_Y_4_3_));
            string strContenido = ReplaceVariablesSolicitudPago(template.Contenido, pSolicitudPagoId);

            List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                          {
                                                EnumeratorPerfil.Equipo_Facturacion
                                          };
            return _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto);
        }
   
        ///4.3.1 Rechaza *
        private async Task<bool> SendEmailRejectAutorizar(int pSolicitudPagoId, bool esVerificar)
        {
            Template template = new Template();
            if (esVerificar)
                template = await _commonService.GetTemplateById((int)(enumeratorTemplate.Rechazar_4_3_1));
            else
                template = await _commonService.GetTemplateById((int)(enumeratorTemplate.Rechazar_4_3_2));

            string strContenido = ReplaceVariablesSolicitudPago(template.Contenido, pSolicitudPagoId);

            List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                          {
                                                EnumeratorPerfil.Equipo_Facturacion,
                                                EnumeratorPerfil.Supervisor,
                                                EnumeratorPerfil.Apoyo,
                                                EnumeratorPerfil.Apoyo_Supervisor,
                                          };

            return _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto);
        }

        /// 4.1.9 devuelve *
        private async Task<bool> SendEmailToDeclineValidate(int pSolicitudPagoId)
        {
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.Devolver_4_1_9));
            string strContenido = ReplaceVariablesSolicitudPago(template.Contenido, pSolicitudPagoId);

            List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                          {
                                                EnumeratorPerfil.Equipo_Facturacion
                                          };
            return _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto);
        }

        /// 4.1.9 aprueba *
        private async Task<bool> SendEmailToAprovedValidate(int pSolicitudPagoId)
        {
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.EnviarTramiteFinanciera_4_1_9));
            string strContenido = ReplaceVariablesSolicitudPago(template.Contenido, pSolicitudPagoId);

            List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                          {
                                                EnumeratorPerfil.Verificacion_Financiera
                                          };
            return _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto);
        }

        /// 4.1.9 aprueba *
        private async Task<bool> SendEmailToAprovedValidateAll(int pSolicitudPagoId)
        {
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.EnviarTramiteFinanciera_4_1_9_TODOS));
            string strContenido = ReplaceVariablesSolicitudPago(template.Contenido, pSolicitudPagoId);

            List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                          {    EnumeratorPerfil.Apoyo_Supervisor,
                                                EnumeratorPerfil.Apoyo,
                                                EnumeratorPerfil.Equipo_Facturacion
                                          };
            return _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto);
        }

        /// 4.1.8 Aprueba *
        private async Task<bool> SendEmailToAprovedVerify(int pSolicitudPagoId)
        {
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.Enviar_para_autorizar_solicitud_4_1_8));
            string strContenido = ReplaceVariablesSolicitudPago(template.Contenido, pSolicitudPagoId);

            List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                          {
                                                EnumeratorPerfil.Supervisor
                                          };
            return _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto);
        }
      
        /// 4.1.8 devuelve *
        private async Task<bool> SendEmailToDeclineVerify(int pSolicitudPagoId)
        {
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.Enviar_para_autorizar_solicitud_4_1_8));
            string strContenido = ReplaceVariablesSolicitudPago(template.Contenido, pSolicitudPagoId);

            List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                          {
                                                EnumeratorPerfil.Equipo_Facturacion
                                          };
            return _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto);
        }

        /// 4.1.7 Aprueba *
        private async Task<bool> SendEmailToAproved(int pSolicitudPagoId)
        {
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.Enviar_a_aprobacion4_1_7));
            string strContenido = ReplaceVariablesSolicitudPago(template.Contenido, pSolicitudPagoId);

            List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                          {
                                                EnumeratorPerfil.Apoyo,
                                                EnumeratorPerfil.Apoyo_Supervisor
                                          };
            return _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto);
        }
      
        /// 4.1.7 devuelve *
        private async Task<bool> SendEmailToDecline(int pSolicitudPagoId)
        {
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.DevolverSolicititud4_1_7));
            string strContenido = ReplaceVariablesSolicitudPago(template.Contenido, pSolicitudPagoId);

            List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                          {
                                                EnumeratorPerfil.Equipo_Facturacion,
                                                EnumeratorPerfil.Supervisor
                                          };
            return _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto);
        }

        private string ReplaceVariablesSolicitudPago(string template, int pSolicitudPago)
        {
            List<Dominio> ListTipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Modalidad_Contrato && r.Activo == true).ToList();

            SolicitudPago solicitudPago =
                _context.SolicitudPago.Where(s => s.SolicitudPagoId == pSolicitudPago)
                .Include(r => r.Contrato)
                .FirstOrDefault();

            template = template
                      .Replace("[FECHA_SUBSANACION]", solicitudPago.FechaSubsanacion.HasValue ? ((DateTime)solicitudPago.FechaSubsanacion).ToString("dd/MM/yyyy") : string.Empty)
                      .Replace("[FECHA_RADICACION_SAC_CONTRATISTA]", solicitudPago.FechaRadicacionSacContratista.HasValue ? ((DateTime)solicitudPago.FechaRadicacionSacContratista).ToString("dd/MM/yyyy") : string.Empty)
                      .Replace("[NUMERO_RADICACION]", solicitudPago.NumeroRadicacionSacContratista ?? string.Empty)
                      .Replace("[NUMERO_SOLICITUD]", solicitudPago.NumeroSolicitud)
                      .Replace("[NUMERO_CONTRATO]", solicitudPago.Contrato.NumeroContrato)
                      .Replace("[FECHA_SOLICITUD]", solicitudPago.FechaCreacion.ToString("dd/MM/yyy"))
                      .Replace("[FECHA_VALIDACION]", DateTime.Now.ToString("dd/MM/yyy"))
                      .Replace("[MODALIDAD_CONTRATO]", ListTipoIntervencion.Where(lti => lti.Codigo == solicitudPago.Contrato.ModalidadCodigo).FirstOrDefault().Nombre);
            return template;
        }

        ///Tareas programadas  
        ///4.1.8 *
        public async Task<bool> SolicitudPagoPendienteVerificacion()
        {
            DateTime MaxDate = await _commonService.CalculardiasLaborales(2, DateTime.Now);
            List<SolicitudPago> ListSolicitudPago =
                _context.SolicitudPago
                .Where(r => r.FechaRegistroCompleto > MaxDate
                   && !r.FechaRegistroCompletoVerificar.HasValue
                   && r.Eliminado == false).ToList();

            List<Dominio> ListTipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Modalidad_Contrato && r.Activo == true).ToList();

            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.Alerta_4_1_8));

            bool SedndIsSuccessfull = true;
            foreach (var SolicitudPago in ListSolicitudPago)
            {
                string strContenido = ReplaceVariablesSolicitudPago(template.Contenido, SolicitudPago.SolicitudPagoId);
                List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                        {
                                            EnumeratorPerfil.Equipo_Facturacion,
                                            EnumeratorPerfil.Apoyo_Supervisor,
                                            EnumeratorPerfil.Apoyo
                                        };

                if (!_commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto))
                    SedndIsSuccessfull = false;
            }

            return SedndIsSuccessfull;
        }

        ///4.1.9 *
        public async Task<bool> SolicitudPagoPendienteAutorizacion()
        {
            DateTime MaxDate = await _commonService.CalculardiasLaborales(2, DateTime.Now);
            List<SolicitudPago> ListSolicitudPago =
                _context.SolicitudPago
                .Where(r => r.FechaRegistroCompletoVerificar > MaxDate
                   && !r.FechaRegistroCompletoAutorizar.HasValue
                   && r.Eliminado == false).ToList();

            List<Dominio> ListTipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Modalidad_Contrato && r.Activo == true).ToList();

            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.Alerta_4_1_9));

            bool SedndIsSuccessfull = true;
            foreach (var SolicitudPago in ListSolicitudPago)
            {
                string strContenido = ReplaceVariablesSolicitudPago(template.Contenido, SolicitudPago.SolicitudPagoId);
                List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                        {
                                            EnumeratorPerfil.Equipo_Facturacion,
                                            EnumeratorPerfil.Supervisor,
                                            EnumeratorPerfil.Apoyo
                                        };

                if (!_commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto))
                    SedndIsSuccessfull = false;
            }

            return SedndIsSuccessfull;
        }

        #endregion
    }
}