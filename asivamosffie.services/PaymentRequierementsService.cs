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
        private readonly IRegisterValidatePaymentRequierementsService _registerValidatePaymentRequierementsService;

        public PaymentRequierementsService(IRegisterValidatePaymentRequierementsService registerValidatePaymentRequierementsService, IDocumentService documentService, devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _registerValidatePaymentRequierementsService = registerValidatePaymentRequierementsService;
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
                    await _context.Set<SolicitudPagoObservacion>()
                                                  .Where(o => o.SolicitudPagoObservacionId == pSolicitudPagoObservacion.SolicitudPagoObservacionId)
                                                                                                                                            .UpdateAsync(r => new SolicitudPagoObservacion()
                                                                                                                                            {
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
                    pSolicitudPagoObservacion.Eliminado = true;
                    pSolicitudPagoObservacion.RegistroCompleto = ValidateCompleteRecordSolicitudPagoObservacion(pSolicitudPagoObservacion);

                    _context.SolicitudPagoObservacion.Add(pSolicitudPagoObservacion);
                }
                _context.SaveChanges();

                await ValidateCompleteObservation(pSolicitudPagoObservacion, pSolicitudPagoObservacion.UsuarioCreacion);

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

        private async Task<bool> ValidateCompleteObservation(SolicitudPagoObservacion pSolicitudPagoObservacion, string pUsuarioMod)
        {
            SolicitudPago solicitudPago = await _registerValidatePaymentRequierementsService.GetSolicitudPago(pSolicitudPagoObservacion.SolicitudPagoId);
            solicitudPago.FechaModificacion = DateTime.Now;
            solicitudPago.UsuarioModificacion = pUsuarioMod;



            int intCantidadDependenciasSolicitudPago = CantidadDependenciasSolicitudPago(solicitudPago);

            if (pSolicitudPagoObservacion.TieneObservacion == true)
                solicitudPago.TieneObservacion = pSolicitudPagoObservacion.TieneObservacion;
             
            if (_context.SolicitudPagoObservacion.Where(r => r.SolicitudPagoId == pSolicitudPagoObservacion.SolicitudPagoId
                                                        && r.Eliminado != true
                                                        && r.Archivada != true).Count() == intCantidadDependenciasSolicitudPago)
            { 
                switch (pSolicitudPagoObservacion.MenuId)
                {
                    case (int)enumeratorMenu.Verificar_solicitud_de_pago:
                        solicitudPago.RegistroCompletoVerificar = true;
                        break;

                    case (int)enumeratorMenu.Autorizar_solicitud_de_pago:
                        solicitudPago.RegistroCompletoAutorizar = true;
                        break;
                } 
            }

            return false;
        }

        private int CantidadDependenciasSolicitudPago(SolicitudPago solicitudPago)
        {
            switch (solicitudPago.TipoSolicitudCodigo)
            {
                case ConstanCodigoTipoSolicitudContratoSolicitudPago.Contratos_Interventoria:
                case ConstanCodigoTipoSolicitudContratoSolicitudPago.Contratos_Obra:

                    return CantidadDependenciasTipoInterventoriaObra(solicitudPago);

                case ConstanCodigoTipoSolicitudContratoSolicitudPago.Expensas:

                    return CantidadDependenciasTipoInterventoriaObra(solicitudPago);

                case ConstanCodigoTipoSolicitudContratoSolicitudPago.Otros_Costos_Servicios:

                    return CantidadDependenciasTipoInterventoriaObra(solicitudPago);

                default: return 0;
            }

        }

        private int CantidadDependenciasTipoInterventoriaObra(SolicitudPago pSolicitudPago)
        {
            //#1 pSolicitudPago.SolicitudPagoCargarFormaPago
            //#2 pSolicitudPago.SolicitudPagoSoporteSolicitud  
            int intCantidadDependenciasSolicitudPago = 2;

            foreach (var SolicitudPagoRegistrarSolicitudPago in pSolicitudPago.SolicitudPagoRegistrarSolicitudPago.Where(r => r.Eliminado != true))
            {
                //#4 Registro De la Solicituud
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
            List<VSolicitudPago> result = new List<VSolicitudPago>();

            if (pMenuId == (int)enumeratorMenu.Verificar_solicitud_de_pago)
            {
                result = await _context.VSolicitudPago//.Where(s => Int32.Parse(s.EstadoCodigo) > 0)
                                                      .OrderByDescending(r => r.FechaModificacion)
                                                      .ToListAsync();
            }

            List<dynamic> grind = new List<dynamic>();

            List<Dominio> ListParametricas = _context.Dominio.Where(d => d.TipoDominioId == (int)EnumeratorTipoDominio.Modalidad_Contrato || d.TipoDominioId == (int)EnumeratorTipoDominio.Estados_Solicitud_Pago).ToList();

            result.ForEach(r =>
            {
                grind.Add(new
                {
                    r.RegistroCompletoAutorizar,
                    r.RegistroCompletoVerificar,
                    r.TipoSolicitudCodigo,
                    r.ContratoId,
                    r.SolicitudPagoId,
                    r.FechaCreacion,
                    r.NumeroSolicitud,
                    r.NumeroContrato,
                    r.EstadoNombre,
                    r.EstadoCodigo,
                    r.ModalidadNombre
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