using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Constants;
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
    public class GenerateSpinOrderService : IGenerateSpinOrderService
    {
        #region constructor
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly IRegisterValidatePaymentRequierementsService _registerValidatePayment;

        public GenerateSpinOrderService(IRegisterValidatePaymentRequierementsService registerValidatePaymentRequierementsService, devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;
            _registerValidatePayment = registerValidatePaymentRequierementsService;
        }
        #endregion

        #region Create 

        public async Task<bool> ValidarRegistroCompleto(int pSolicitudPago, string pAuthor)
        {
            bool blRegistroCompleto = false;
            try
            {
                SolicitudPago solicitudPago = await GetSolicitudPagoBySolicitudPagoId(pSolicitudPago);
                blRegistroCompleto = ValidarRegistroCompletoOrdenGiro(solicitudPago.OrdenGiro);
                _context.Set<OrdenGiro>()
                        .Where(o => o.OrdenGiroId == solicitudPago.OrdenGiroId)
                        .Update(o => new OrdenGiro
                        {
                            RegistroCompleto = blRegistroCompleto,
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = pAuthor
                        });
            }
            catch (Exception)
            {
                return false;
            }

            return blRegistroCompleto;
        }

        public async Task<Respuesta> CreateEditOrdenGiro(OrdenGiro pOrdenGiro)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Orden_Giro, (int)EnumeratorTipoDominio.Acciones);

            try
            { 
                decimal? ValorNetoGiro = pOrdenGiro?.OrdenGiroDetalle?.FirstOrDefault()?.OrdenGiroDetalleTerceroCausacion?.FirstOrDefault()?.ValorNetoGiro;
               
                if (pOrdenGiro.OrdenGiroId == 0)
                {
                    pOrdenGiro.ConsecutivoOrigen = await _commonService.EnumeradorOrigenOrdenGiro();
                    pOrdenGiro.NumeroSolicitud = await _commonService.EnumeradorOrdenGiro((int)pOrdenGiro?.SolicitudPagoId);
                    pOrdenGiro.FechaCreacion = DateTime.Now;
                    pOrdenGiro.Eliminado = false;
                    pOrdenGiro.RegistroCompleto = ValidarRegistroCompletoOrdenGiro(pOrdenGiro);
                    pOrdenGiro.EstadoCodigo = ((int)EnumEstadoOrdenGiro.En_Proceso_Generacion).ToString();
                    pOrdenGiro.ValorNetoGiro = ValorNetoGiro;
                    _context.OrdenGiro.Add(pOrdenGiro);
                    _context.SaveChanges();
                     
                    await _context.Set<SolicitudPago>()
                                    .Where(o => o.SolicitudPagoId == pOrdenGiro.SolicitudPagoId)
                                                                                        .UpdateAsync(r => new SolicitudPago()
                                                                                        {
                                                                                            FechaModificacion = DateTime.Now,
                                                                                            UsuarioModificacion = pOrdenGiro.UsuarioCreacion,

                                                                                            OrdenGiroId = pOrdenGiro.OrdenGiroId
                                                                                        });
                }
                else
                {
                    _context.Set<OrdenGiro>()
                            .Where(o => o.OrdenGiroId == pOrdenGiro.OrdenGiroId)
                            .Update(o => new OrdenGiro
                            {
                                ValorNetoGiro = ValorNetoGiro,
                                EstadoCodigo = ((int)EnumEstadoOrdenGiro.En_Proceso_Generacion).ToString(),
                                FechaModificacion = DateTime.Now,
                                UsuarioModificacion = pOrdenGiro.UsuarioCreacion
                            });
                }
                if (pOrdenGiro?.OrdenGiroTercero.Count() > 0)
                    CreateEditOrdenGiroTercero(pOrdenGiro.OrdenGiroTercero.FirstOrDefault(), pOrdenGiro.UsuarioCreacion);

                if (pOrdenGiro?.OrdenGiroDetalle.Count() > 0)
                    CreateEditOrdenGiroDetalle(pOrdenGiro.OrdenGiroDetalle.FirstOrDefault(), pOrdenGiro.UsuarioCreacion);

                Respuesta respuesta =
                     new Respuesta
                     {
                         IsSuccessful = true,
                         IsException = false,
                         IsValidation = false,
                         Code = GeneralCodes.OperacionExitosa,
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo(
                             (int)enumeratorMenu.Generar_Orden_de_giro,
                             GeneralCodes.OperacionExitosa,
                             idAccion,
                             pOrdenGiro.UsuarioCreacion,
                             ConstantCommonMessages.SpinOrder.REGISTRAR_ORDENES_GIRO)
                     };

                await ValidarRegistroCompleto(pOrdenGiro.SolicitudPagoId, pOrdenGiro.UsuarioCreacion);

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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Generar_Orden_de_giro, GeneralCodes.Error, idAccion, pOrdenGiro.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        private void CreateEditOrdenGiroDetalleObservacion(OrdenGiroDetalleObservacion pOrdenGiroDetalleObservacion, string pUsuarioCreacion)
        {
            if (pOrdenGiroDetalleObservacion.OrdenGiroObservacionId == 0)
            {
                pOrdenGiroDetalleObservacion.UsuarioCreacion = pUsuarioCreacion;
                pOrdenGiroDetalleObservacion.FechaCreacion = DateTime.Now;
                pOrdenGiroDetalleObservacion.Eliminado = false;
                pOrdenGiroDetalleObservacion.RegistroCompleto = !string.IsNullOrEmpty(pOrdenGiroDetalleObservacion.Observacion);
                _context.OrdenGiroDetalleObservacion.Add(pOrdenGiroDetalleObservacion);
            }
            else
            {
                _context.Set<OrdenGiroDetalleObservacion>()
                        .Where(o => o.OrdenGiroObservacionId == pOrdenGiroDetalleObservacion.OrdenGiroObservacionId)
                        .Update(o => new OrdenGiroDetalleObservacion
                        {
                            UsuarioModificacion = pUsuarioCreacion,
                            FechaModificacion = DateTime.Now,
                            RegistroCompleto = !string.IsNullOrEmpty(pOrdenGiroDetalleObservacion.Observacion),
                            Observacion = pOrdenGiroDetalleObservacion.Observacion
                        });
            }
        }

        private void CreateEditOrdenGiroDetalle(OrdenGiroDetalle pOrdenGiroDetalle, string pUsuarioCreacion)
        {
            if (pOrdenGiroDetalle?.OrdenGiroDetalleEstrategiaPago?.Count() > 0)
                CreateEditOrdenGiroDetalleEstrategiaPago(pOrdenGiroDetalle.OrdenGiroDetalleEstrategiaPago.FirstOrDefault(), pUsuarioCreacion);

            if (pOrdenGiroDetalle?.OrdenGiroDetalleObservacion?.Count() > 0)
                CreateEditOrdenGiroDetalleObservacion(pOrdenGiroDetalle.OrdenGiroDetalleObservacion.FirstOrDefault(), pUsuarioCreacion);

            if (pOrdenGiroDetalle?.OrdenGiroSoporte?.Count() > 0)
                CreateEditOrdenGiroSoporte(pOrdenGiroDetalle.OrdenGiroSoporte.FirstOrDefault(), pUsuarioCreacion);

            if (pOrdenGiroDetalle?.OrdenGiroDetalleEstrategiaPago.Count() > 0)
                CreateEditOrdenGiroDetalleEstrategiaPago(pOrdenGiroDetalle.OrdenGiroDetalleEstrategiaPago.FirstOrDefault(), pUsuarioCreacion);

            if (pOrdenGiroDetalle?.OrdenGiroDetalleDescuentoTecnica.Count() > 0)
                CreateEditOrdenGiroDetalleDescuentoTecnica(pOrdenGiroDetalle.OrdenGiroDetalleDescuentoTecnica.ToList(), pUsuarioCreacion);

            if (pOrdenGiroDetalle?.OrdenGiroDetalleTerceroCausacion.Count() > 0)
                CreateEditOrdenGiroDetalleTerceroCausacion(pOrdenGiroDetalle?.OrdenGiroDetalleTerceroCausacion.ToList(), pUsuarioCreacion);

            if (pOrdenGiroDetalle?.OrdenGiroDetalleId == 0)
            {
                pOrdenGiroDetalle.UsuarioCreacion = pUsuarioCreacion;
                pOrdenGiroDetalle.FechaCreacion = DateTime.Now;
                pOrdenGiroDetalle.Eliminado = false;
                pOrdenGiroDetalle.RegistroCompleto = ValidarRegistroCompletoOrdenGiroDetalle(pOrdenGiroDetalle);

                _context.OrdenGiroDetalle.Add(pOrdenGiroDetalle);
            }

            else if (pOrdenGiroDetalle?.OrdenGiroDetalleId > 0)
            {
                _context.Set<OrdenGiroDetalle>()
                        .Where(o => o.OrdenGiroDetalleId == pOrdenGiroDetalle.OrdenGiroDetalleId)
                        .Update(r => new OrdenGiroDetalle()
                        {
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = pUsuarioCreacion,
                            RegistroCompleto = ValidarRegistroCompletoOrdenGiroDetalle(pOrdenGiroDetalle)
                        });
            }
        }

        private void CreateEditOrdenGiroSoporte(OrdenGiroSoporte ordenGiroSoporte, string pUsuarioCreacion)
        {
            if (ordenGiroSoporte.OrdenGiroSoporteId == 0)
            {
                ordenGiroSoporte.UsuarioCreacion = pUsuarioCreacion;
                ordenGiroSoporte.Eliminado = false;
                ordenGiroSoporte.FechaCreacion = DateTime.Now;
                ordenGiroSoporte.RegistroCompleto = !string.IsNullOrEmpty(ordenGiroSoporte.UrlSoporte);
                _context.OrdenGiroSoporte.Add(ordenGiroSoporte);
            }
            else
            {
                _context.Set<OrdenGiroSoporte>()
                        .Where(o => o.OrdenGiroSoporteId == ordenGiroSoporte.OrdenGiroSoporteId)
                        .Update(o => new OrdenGiroSoporte
                        {
                            UsuarioModificacion = pUsuarioCreacion,
                            FechaModificacion = DateTime.Now,
                            RegistroCompleto = !string.IsNullOrEmpty(ordenGiroSoporte.UrlSoporte),
                            UrlSoporte = ordenGiroSoporte.UrlSoporte
                        });
            }
        }

        private void CreateEditOrdenGiroDetalleTerceroCausacion(List<OrdenGiroDetalleTerceroCausacion> pListOrdenGiroDetalleTerceroCausacion, string pUsuarioCreacion)
        {
            foreach (var pOrdenGiroDetalleTerceroCausacion in pListOrdenGiroDetalleTerceroCausacion)
            {
                if (pOrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionId == 0)
                {
                    pOrdenGiroDetalleTerceroCausacion.UsuarioCreacion = pUsuarioCreacion;
                    pOrdenGiroDetalleTerceroCausacion.FechaCreacion = DateTime.Now;
                    pOrdenGiroDetalleTerceroCausacion.Eliminado = false;
                    pOrdenGiroDetalleTerceroCausacion.RegistroCompleto = ValidarRegistroCompletoOrdenGiroDetalleTerceroCausacion(pOrdenGiroDetalleTerceroCausacion);

                    _context.OrdenGiroDetalleTerceroCausacion.Add(pOrdenGiroDetalleTerceroCausacion);
                }
                else
                {
                    _context.Set<OrdenGiroDetalleTerceroCausacion>()
                            .Where(o => o.OrdenGiroDetalleTerceroCausacionId == pOrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionId)
                            .Update(r => new OrdenGiroDetalleTerceroCausacion()
                            {
                                TieneDescuento = pOrdenGiroDetalleTerceroCausacion.TieneDescuento,
                                ValorNetoGiro = pOrdenGiroDetalleTerceroCausacion.ValorNetoGiro,
                                OrdenGiroDetalleId = pOrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleId,
                                ConceptoPagoCriterio = pOrdenGiroDetalleTerceroCausacion.ConceptoPagoCriterio,
                                TipoPagoCodigo = pOrdenGiroDetalleTerceroCausacion.TipoPagoCodigo,

                                FechaModificacion = DateTime.Now,
                                UsuarioModificacion = pUsuarioCreacion,
                                RegistroCompleto = ValidarRegistroCompletoOrdenGiroDetalleTerceroCausacion(pOrdenGiroDetalleTerceroCausacion)
                            });
                }

                if (pOrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionDescuento.Count() > 0)
                    CreateEditOrdenGiroDetalleTerceroCausacionDescuento(pOrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionDescuento, pUsuarioCreacion);

                if (pOrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionAportante.Count() > 0)
                    CreateEditOrdenGiroDetalleTerceroCausacionAportante(pOrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionAportante, pUsuarioCreacion);
            }
        }

        private void CreateEditOrdenGiroDetalleTerceroCausacionAportante(ICollection<OrdenGiroDetalleTerceroCausacionAportante> pOrdenGiroDetalleTerceroCausacionAportante, string pUsuarioCreacion)
        {
            foreach (var TerceroCausacionAportante in pOrdenGiroDetalleTerceroCausacionAportante)
            {
                if (TerceroCausacionAportante.OrdenGiroDetalleTerceroCausacionAportanteId == 0)
                {
                    TerceroCausacionAportante.UsuarioCreacion = pUsuarioCreacion;
                    TerceroCausacionAportante.Eliminado = false;
                    TerceroCausacionAportante.FechaCreacion = DateTime.Now;
                    TerceroCausacionAportante.RegistroCompleto = ValidarRegistroCompletoOrdenGiroDetalleTerceroCausacionAportante(TerceroCausacionAportante);
                    _context.OrdenGiroDetalleTerceroCausacionAportante.Add(TerceroCausacionAportante);
                }
                else
                {
                    if (TerceroCausacionAportante.CuentaBancariaId != null)
                    {
                        _context.Set<OrdenGiroDetalleTerceroCausacionAportante>()
                                 .Where(r => r.OrdenGiroDetalleTerceroCausacionAportanteId == TerceroCausacionAportante.OrdenGiroDetalleTerceroCausacionAportanteId)
                                 .Update(r => new OrdenGiroDetalleTerceroCausacionAportante
                                 {
                                     FechaModificacion = DateTime.Now,
                                     UsuarioModificacion = pUsuarioCreacion,
                                     CuentaBancariaId = TerceroCausacionAportante.CuentaBancariaId,
                                     RegistroCompletoOrigen = TerceroCausacionAportante.CuentaBancariaId > 0
                                 });
                    }
                    else
                    {
                        _context.Set<OrdenGiroDetalleTerceroCausacionAportante>()
                                .Where(r => r.OrdenGiroDetalleTerceroCausacionAportanteId == TerceroCausacionAportante.OrdenGiroDetalleTerceroCausacionAportanteId)
                                .Update(r => new OrdenGiroDetalleTerceroCausacionAportante
                                {
                                    FechaModificacion = DateTime.Now,
                                    UsuarioModificacion = pUsuarioCreacion,
                                    CuentaBancariaId = TerceroCausacionAportante.CuentaBancariaId,
                                    FuenteRecursoCodigo = TerceroCausacionAportante.FuenteRecursoCodigo,
                                    AportanteId = TerceroCausacionAportante.AportanteId,
                                    ConceptoPagoCodigo = TerceroCausacionAportante.ConceptoPagoCodigo,
                                    ValorDescuento = TerceroCausacionAportante.ValorDescuento,
                                    FuenteFinanciacionId = TerceroCausacionAportante.FuenteFinanciacionId,
                                    RegistroCompleto = ValidarRegistroCompletoOrdenGiroDetalleTerceroCausacionAportante(TerceroCausacionAportante)
                                });
                    }
                }
            }
        }

        private bool ValidarRegistroCompletoOrdenGiroDetalleTerceroCausacionAportante(OrdenGiroDetalleTerceroCausacionAportante terceroCausacionAportante)
        {
            if (
                    string.IsNullOrEmpty(terceroCausacionAportante.FuenteRecursoCodigo)
                  || terceroCausacionAportante.AportanteId == 0
                  || string.IsNullOrEmpty(terceroCausacionAportante.ConceptoPagoCodigo)
                  || terceroCausacionAportante.ValorDescuento == 0
                  || terceroCausacionAportante.FuenteFinanciacionId == 0)
                return false;

            return true;
        }

        private void CreateEditOrdenGiroDetalleTerceroCausacionDescuento(ICollection<OrdenGiroDetalleTerceroCausacionDescuento> pListOrdenGiroDetalleTerceroCausacionDescuento, string pUsuarioCreacion)
        {
            foreach (var OrdenGiroDetalleTerceroCausacionDescuento in pListOrdenGiroDetalleTerceroCausacionDescuento)
            {
                if (OrdenGiroDetalleTerceroCausacionDescuento.OrdenGiroDetalleTerceroCausacionDescuentoId == 0)
                {
                    OrdenGiroDetalleTerceroCausacionDescuento.UsuarioCreacion = pUsuarioCreacion;
                    OrdenGiroDetalleTerceroCausacionDescuento.FechaCreacion = DateTime.Now;
                    OrdenGiroDetalleTerceroCausacionDescuento.Eliminado = false;
                    OrdenGiroDetalleTerceroCausacionDescuento.RegistroCompleto = ValidarRegistroCompletoOrdenGiroDetalleTerceroCausacionDescuento(OrdenGiroDetalleTerceroCausacionDescuento);

                    _context.OrdenGiroDetalleTerceroCausacionDescuento.Add(OrdenGiroDetalleTerceroCausacionDescuento);
                }
                else
                {
                    _context.Set<OrdenGiroDetalleTerceroCausacionDescuento>()
                            .Where(o => o.OrdenGiroDetalleTerceroCausacionDescuentoId == OrdenGiroDetalleTerceroCausacionDescuento.OrdenGiroDetalleTerceroCausacionDescuentoId)
                            .Update(o => new OrdenGiroDetalleTerceroCausacionDescuento
                            {
                                TipoDescuentoCodigo = OrdenGiroDetalleTerceroCausacionDescuento.TipoDescuentoCodigo,
                                ValorDescuento = OrdenGiroDetalleTerceroCausacionDescuento.ValorDescuento,
                                RegistroCompleto = ValidarRegistroCompletoOrdenGiroDetalleTerceroCausacionDescuento(OrdenGiroDetalleTerceroCausacionDescuento)

                            }); ;



                }
            }
        }

        private void CreateEditOrdenGiroDetalleDescuentoTecnica(List<OrdenGiroDetalleDescuentoTecnica> pListOrdenGiroDetalleDescuentoTecnica, string pUsuarioCreacion)
        {
            foreach (var pOrdenGiroDetalleDescuentoTecnica in pListOrdenGiroDetalleDescuentoTecnica)
            {
                if (pOrdenGiroDetalleDescuentoTecnica.OrdenGiroDetalleDescuentoTecnicaId == 0)
                {
                    pOrdenGiroDetalleDescuentoTecnica.UsuarioCreacion = pUsuarioCreacion;
                    pOrdenGiroDetalleDescuentoTecnica.FechaCreacion = DateTime.Now;
                    pOrdenGiroDetalleDescuentoTecnica.Eliminado = false;
                    pOrdenGiroDetalleDescuentoTecnica.RegistroCompleto = ValidarRegistroCompletoOrdenGiroDetalleDescuentoTecnica(pOrdenGiroDetalleDescuentoTecnica);
                    _context.OrdenGiroDetalleDescuentoTecnica.Add(pOrdenGiroDetalleDescuentoTecnica);
                }
                else
                {
                    _context.Set<OrdenGiroDetalleDescuentoTecnica>()
                                 .Where(o => o.OrdenGiroDetalleDescuentoTecnicaId == pOrdenGiroDetalleDescuentoTecnica.OrdenGiroDetalleDescuentoTecnicaId)
                                 .Update(r => new OrdenGiroDetalleDescuentoTecnica()
                                 {
                                     SolicitudPagoFaseFacturaDescuentoId = pOrdenGiroDetalleDescuentoTecnica.SolicitudPagoFaseFacturaDescuentoId,
                                     TipoPagoCodigo = pOrdenGiroDetalleDescuentoTecnica.TipoPagoCodigo,
                                     CriterioCodigo = pOrdenGiroDetalleDescuentoTecnica.CriterioCodigo,
                                     FechaModificacion = DateTime.Now,
                                     UsuarioModificacion = pUsuarioCreacion,
                                     RegistroCompleto = ValidarRegistroCompletoOrdenGiroDetalleDescuentoTecnica(pOrdenGiroDetalleDescuentoTecnica)
                                 });
                }
                CreateEditOrdenGiroDetalleDescuentoTecnicaAportante(pOrdenGiroDetalleDescuentoTecnica.OrdenGiroDetalleDescuentoTecnicaAportante, pUsuarioCreacion);
            }
        }

        private void CreateEditOrdenGiroDetalleDescuentoTecnicaAportante(ICollection<OrdenGiroDetalleDescuentoTecnicaAportante> pOrdenGiroDetalleDescuentoTecnicaAportanteList, string pUsuarioCreacion)
        {
            foreach (var pOrdenGiroDetalleDescuentoTecnicaAportante in pOrdenGiroDetalleDescuentoTecnicaAportanteList)
            {
                if (pOrdenGiroDetalleDescuentoTecnicaAportante.OrdenGiroDetalleDescuentoTecnicaAportanteId == 0)
                {
                    pOrdenGiroDetalleDescuentoTecnicaAportante.UsuarioCreacion = pUsuarioCreacion;
                    pOrdenGiroDetalleDescuentoTecnicaAportante.FechaCreacion = DateTime.Now;
                    pOrdenGiroDetalleDescuentoTecnicaAportante.Eliminado = false;
                    pOrdenGiroDetalleDescuentoTecnicaAportante.RegistroCompleto = ValidarRegistroCompletoOrdenGiroDetalleDescuentoTecnicaAportante(pOrdenGiroDetalleDescuentoTecnicaAportante);

                    _context.OrdenGiroDetalleDescuentoTecnicaAportante.Add(pOrdenGiroDetalleDescuentoTecnicaAportante);
                }
                else
                {
                    _context.Set<OrdenGiroDetalleDescuentoTecnicaAportante>()
                                 .Where(o => o.OrdenGiroDetalleDescuentoTecnicaAportanteId == pOrdenGiroDetalleDescuentoTecnicaAportante.OrdenGiroDetalleDescuentoTecnicaAportanteId)
                                 .Update(r => new OrdenGiroDetalleDescuentoTecnicaAportante()
                                 {
                                     FechaModificacion = DateTime.Now,
                                     UsuarioModificacion = pUsuarioCreacion,
                                     RegistroCompleto = ValidarRegistroCompletoOrdenGiroDetalleDescuentoTecnicaAportante(pOrdenGiroDetalleDescuentoTecnicaAportante),
                                     AportanteId = pOrdenGiroDetalleDescuentoTecnicaAportante.AportanteId,
                                     ValorDescuento = pOrdenGiroDetalleDescuentoTecnicaAportante.ValorDescuento,
                                     ConceptoPagoCodigo = pOrdenGiroDetalleDescuentoTecnicaAportante.ConceptoPagoCodigo,
                                     RequiereDescuento = pOrdenGiroDetalleDescuentoTecnicaAportante.RequiereDescuento,
                                     FuenteRecursosCodigo = pOrdenGiroDetalleDescuentoTecnicaAportante.FuenteRecursosCodigo,


                                 });
                }
            }

        }

        private void CreateEditOrdenGiroDetalleEstrategiaPago(OrdenGiroDetalleEstrategiaPago pOrdenGiroDetalleEstrategiaPago, string pUsuarioCreacion)
        {
            if (pOrdenGiroDetalleEstrategiaPago?.OrdenGiroDetalleEstrategiaPagoId == 0)
            {
                pOrdenGiroDetalleEstrategiaPago.UsuarioCreacion = pUsuarioCreacion;
                pOrdenGiroDetalleEstrategiaPago.FechaCreacion = DateTime.Now;
                pOrdenGiroDetalleEstrategiaPago.Eliminado = false;
                pOrdenGiroDetalleEstrategiaPago.RegistroCompleto = !string.IsNullOrEmpty(pOrdenGiroDetalleEstrategiaPago.EstrategiaPagoCodigo);

                _context.OrdenGiroDetalleEstrategiaPago.Add(pOrdenGiroDetalleEstrategiaPago);
            }
            else
            {
                _context.Set<OrdenGiroDetalleEstrategiaPago>()
                        .Where(o => o.OrdenGiroDetalleEstrategiaPagoId == pOrdenGiroDetalleEstrategiaPago.OrdenGiroDetalleEstrategiaPagoId)
                        .Update(r => new OrdenGiroDetalleEstrategiaPago()
                        {
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = pUsuarioCreacion,
                            RegistroCompleto = !string.IsNullOrEmpty(pOrdenGiroDetalleEstrategiaPago.EstrategiaPagoCodigo),
                            EstrategiaPagoCodigo = pOrdenGiroDetalleEstrategiaPago.EstrategiaPagoCodigo
                        });
            }
        }

        private void CreateEditOrdenGiroTercero(OrdenGiroTercero pOrdenGiroTercero, string pUsuarioCreacion)
        {
            if (pOrdenGiroTercero.OrdenGiroTerceroId == 0)
            {
                pOrdenGiroTercero.UsuarioCreacion = pUsuarioCreacion;
                pOrdenGiroTercero.FechaCreacion = DateTime.Now;
                pOrdenGiroTercero.Eliminado = false;
                pOrdenGiroTercero.RegistroCompleto = ValidarRegistroCompletoOrdenGiroTercero(pOrdenGiroTercero);

                _context.OrdenGiroTercero.Add(pOrdenGiroTercero);
                _context.SaveChanges();
            }
            else
            {
                _context.Set<OrdenGiroTercero>()
                       .Where(o => o.OrdenGiroTerceroId == pOrdenGiroTercero.OrdenGiroTerceroId)
                       .Update(o => new OrdenGiroTercero
                       {
                           RegistroCompleto = ValidarRegistroCompletoOrdenGiroTercero(pOrdenGiroTercero),
                           MedioPagoGiroCodigo = pOrdenGiroTercero.MedioPagoGiroCodigo,
                           FechaModificacion = DateTime.Now,
                           UsuarioModificacion = pUsuarioCreacion
                       });
            }
            if (pOrdenGiroTercero.MedioPagoGiroCodigo == ConstanCodigoMedioPagoGiroTercero.Transferencia_electronica)
                CreateEditOrdenGiroTerceroTransferenciaElectronica(pOrdenGiroTercero.OrdenGiroTerceroTransferenciaElectronica.FirstOrDefault(), pUsuarioCreacion);

            if (pOrdenGiroTercero.MedioPagoGiroCodigo == ConstanCodigoMedioPagoGiroTercero.Cheque_de_gerencia)
                CreateEditOrdenGiroTerceroChequeGerencia(pOrdenGiroTercero.OrdenGiroTerceroChequeGerencia.FirstOrDefault(), pUsuarioCreacion);
        }

        private void CreateEditOrdenGiroTerceroChequeGerencia(OrdenGiroTerceroChequeGerencia pOrdenGiroTerceroChequeGerencia, string pUsuarioCreacion)
        {
            if (pOrdenGiroTerceroChequeGerencia.OrdenGiroTerceroChequeGerenciaId == 0)
            {
                pOrdenGiroTerceroChequeGerencia.UsuarioCreacion = pUsuarioCreacion;
                pOrdenGiroTerceroChequeGerencia.FechaCreacion = DateTime.Now;
                pOrdenGiroTerceroChequeGerencia.Eliminado = false;
                pOrdenGiroTerceroChequeGerencia.RegistroCompleto = ValidarRegistroCompletoOrdenGiroTerceroChequeGerencia(pOrdenGiroTerceroChequeGerencia);

                _context.OrdenGiroTerceroChequeGerencia.Add(pOrdenGiroTerceroChequeGerencia);
            }
            else
            {
                _context.Set<OrdenGiroTerceroChequeGerencia>()
                             .Where(o => o.OrdenGiroTerceroChequeGerenciaId == pOrdenGiroTerceroChequeGerencia.OrdenGiroTerceroChequeGerenciaId)
                             .Update(r => new OrdenGiroTerceroChequeGerencia()
                             {
                                 FechaModificacion = DateTime.Now,
                                 UsuarioModificacion = pUsuarioCreacion,
                                 RegistroCompleto = ValidarRegistroCompletoOrdenGiroTerceroChequeGerencia(pOrdenGiroTerceroChequeGerencia),

                                 NombreBeneficiario = pOrdenGiroTerceroChequeGerencia.NombreBeneficiario,
                                 NumeroIdentificacionBeneficiario = pOrdenGiroTerceroChequeGerencia.NumeroIdentificacionBeneficiario,
                             });
            }
        }

        private void CreateEditOrdenGiroTerceroTransferenciaElectronica(OrdenGiroTerceroTransferenciaElectronica pOrdenGiroTerceroTransferenciaElectronica, string pUsuarioCreacion)
        {
            if (pOrdenGiroTerceroTransferenciaElectronica.OrdenGiroTerceroTransferenciaElectronicaId == 0)
            {
                pOrdenGiroTerceroTransferenciaElectronica.UsuarioCreacion = pUsuarioCreacion;
                pOrdenGiroTerceroTransferenciaElectronica.FechaCreacion = DateTime.Now;
                pOrdenGiroTerceroTransferenciaElectronica.Eliminado = false;
                pOrdenGiroTerceroTransferenciaElectronica.RegistroCompleto = ValidarRegistroCompletoOrdenGiroTerceroTransferenciaElectronica(pOrdenGiroTerceroTransferenciaElectronica);

                _context.OrdenGiroTerceroTransferenciaElectronica.Add(pOrdenGiroTerceroTransferenciaElectronica);
            }
            else
            {
                _context.Set<OrdenGiroTerceroTransferenciaElectronica>()
                        .Where(o => o.OrdenGiroTerceroTransferenciaElectronicaId == pOrdenGiroTerceroTransferenciaElectronica.OrdenGiroTerceroTransferenciaElectronicaId)
                        .Update(r => new OrdenGiroTerceroTransferenciaElectronica()
                        {
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = pUsuarioCreacion,
                            RegistroCompleto = ValidarRegistroCompletoOrdenGiroTerceroTransferenciaElectronica(pOrdenGiroTerceroTransferenciaElectronica),

                            TitularCuenta = pOrdenGiroTerceroTransferenciaElectronica.TitularCuenta,
                            TitularNumeroIdentificacion = pOrdenGiroTerceroTransferenciaElectronica.TitularNumeroIdentificacion,
                            NumeroCuenta = pOrdenGiroTerceroTransferenciaElectronica.NumeroCuenta,
                            BancoCodigo = pOrdenGiroTerceroTransferenciaElectronica.BancoCodigo,
                            EsCuentaAhorros = pOrdenGiroTerceroTransferenciaElectronica.EsCuentaAhorros,
                        });
            }
        }

        public async Task<Respuesta> DeleteOrdenGiroDetalleDescuentoTecnicaAportante(int pOrdenGiroDetalleDescuentoTecnicaAportanteId, string pAuthor)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Aportante_Orden_Giro, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                await _context.Set<OrdenGiroDetalleDescuentoTecnicaAportante>()
                          .Where(o => o.OrdenGiroDetalleDescuentoTecnicaAportanteId == pOrdenGiroDetalleDescuentoTecnicaAportanteId)
                          .UpdateAsync(o => new OrdenGiroDetalleDescuentoTecnicaAportante
                          {
                              Eliminado = true,
                              FechaModificacion = DateTime.Now,
                              UsuarioModificacion = pAuthor
                          });

                return
                 new Respuesta
                 {
                     IsSuccessful = true,
                     IsException = false,
                     IsValidation = false,
                     Code = GeneralCodes.OperacionExitosa,
                     Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Generar_Orden_de_giro, GeneralCodes.OperacionExitosa, idAccion, pAuthor, ConstantCommonMessages.SpinOrder.ELIMINAR_APORTANTE_ORDENES_GIRO)
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
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Generar_Orden_de_giro, GeneralCodes.Error, idAccion, "", ex.InnerException.ToString())
                     };
            }
        }
        #endregion

        #region get
        /// <summary>
        /// TODO : VALIDAR SOLICITUDES DE PAGO QUE YA TENGAN APROBACION 
        /// </summary>
        /// <returns></returns>
        /// 
        public async Task<dynamic> GetValorConceptoByAportanteId(int pAportanteId, int pSolicitudPagoId, string pConceptoPago)
        {
            return _context.VValorUsoXcontratoAportante
                           .Where(v => v.AportanteId == pAportanteId
                               && v.ConceptoPagoCodigo == pConceptoPago
                               && v.SolicitudPagoId == pSolicitudPagoId
                               ).Select(v => v.ValorUso);
        }

        public async Task<dynamic> GetFuentesDeRecursosPorAportanteId(int pAportanteId)
        {
            List<Dominio> ListNameFuenteFinanciacion = await _commonService.GetListDominioByIdTipoDominio((int)EnumeratorTipoDominio.Fuentes_de_financiacion);

            List<FuenteFinanciacion> ListFuenteFinanciacion = _context.FuenteFinanciacion.Where(r => r.AportanteId == pAportanteId && r.Eliminado != true).ToList();

            List<dynamic> ListDynamics = new List<dynamic>();

            ListFuenteFinanciacion.ForEach(ff =>
            {
                ListDynamics.Add(new
                {
                    Nombre = ListNameFuenteFinanciacion.Where(l => l.Codigo == ff.FuenteRecursosCodigo).FirstOrDefault().Nombre,
                    Codigo = ff.FuenteRecursosCodigo,
                    FuenteFinanciacionId = ff.FuenteFinanciacionId
                });
            });
            return ListDynamics;
        }

        public async Task<dynamic> GetListOrdenGiro(int pMenuId)
        {
            return pMenuId switch
            {
                (int)enumeratorMenu.Generar_Orden_de_giro => await _context.VOrdenGiro.Where(s =>
                                             s.IntEstadoCodigo >= (int)EnumEstadoOrdenGiro.Enviada_A_Order_Giro)
                                                                   .OrderByDescending(r => r.FechaModificacion)
                                                                   .ToListAsync(),

                (int)enumeratorMenu.Verificar_orden_de_giro => await _context.VOrdenGiro.Where(s =>
                                             s.IntEstadoCodigo >= (int)EnumEstadoOrdenGiro.Enviada_Para_Verificacion_Orden_Giro)
                                                                   .OrderByDescending(r => r.FechaModificacion)
                                                                   .ToListAsync(),

                (int)enumeratorMenu.Aprobar_orden_de_giro => await _context.VOrdenGiro.Where(s =>
                                             s.IntEstadoCodigo >= (int)EnumEstadoOrdenGiro.Enviada_Para_Aprobacion_Orden_Giro)
                                                                   .OrderByDescending(r => r.FechaModificacion)
                                                                   .ToListAsync(),

                (int)enumeratorMenu.Tramitar_orden_de_giro => await _context.VOrdenGiro.Where(s =>
                                             s.IntEstadoCodigo >= (int)EnumEstadoOrdenGiro.Enviada_para_tramite_ante_fiduciaria)
                                                                   .OrderByDescending(r => r.FechaModificacion)
                                                                   .ToListAsync(),

                _ => new { },
            };
        }

        public async Task<dynamic> GetListSolicitudPagoOLD()
        {
            var result = await _context.SolicitudPago
                 .Include(r => r.Contrato)
                 .Include(r => r.OrdenGiro).Where(s => s.Eliminado != true)
                                                                            .Select(s => new
                                                                            {
                                                                                s.FechaAprobacionFinanciera,
                                                                                s.NumeroSolicitud,
                                                                                s.Contrato.ModalidadCodigo,
                                                                                s.Contrato.NumeroContrato,
                                                                                s.EstadoCodigo,
                                                                                s.ContratoId,
                                                                                s.SolicitudPagoId,
                                                                                s.OrdenGiro
                                                                            }).OrderByDescending(r => r.SolicitudPagoId).ToListAsync();
            List<dynamic> grind = new List<dynamic>();
            List<Dominio> ListParametricas = _context.Dominio.Where(
                                                                         d => d.TipoDominioId == (int)EnumeratorTipoDominio.Modalidad_Contrato
                                                                      || d.TipoDominioId == (int)EnumeratorTipoDominio.Estados_Solicitud_Pago
                                                                      || d.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Orden_Giro
                                                              ).ToList();

            result.ForEach(r =>
            {
                bool RegistroCompleto = false;
                string EstadoOrdenGiro = string.Empty;
                if (r.OrdenGiro == null)
                    EstadoOrdenGiro = ((int)EnumEstadoOrdenGiro.Enviada_A_Order_Giro).ToString();
                else
                {
                    EstadoOrdenGiro = r.OrdenGiro.EstadoCodigo;
                    RegistroCompleto = r.OrdenGiro.RegistroCompleto ?? false;
                }
                EstadoOrdenGiro = ListParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estados_Solicitud_Pago && r.Codigo == EstadoOrdenGiro).FirstOrDefault().Nombre;

                grind.Add(new
                {
                    r.FechaAprobacionFinanciera,
                    r.NumeroSolicitud,
                    Modalidad = !string.IsNullOrEmpty(r.ModalidadCodigo) ? ListParametricas.Where(l => l.Codigo == r.ModalidadCodigo && l.TipoDominioId == (int)EnumeratorTipoDominio.Modalidad_Contrato).FirstOrDefault().Nombre : "No aplica",
                    NumeroContrato = r.NumeroContrato ?? "No Aplica",
                    r.OrdenGiro,
                    EstadoOrdenGiro,
                    RegistroCompleto,
                    r.SolicitudPagoId,
                });
            });
            return grind;
        }

        public async Task<SolicitudPago> GetSolicitudPagoBySolicitudPagoId(int SolicitudPagoId)
        {
            SolicitudPago SolicitudPago = await _registerValidatePayment.GetSolicitudPago(SolicitudPagoId);

            try
            {
                if (SolicitudPago.ContratoId > 0)
                {
                    SolicitudPago.ContratoSon = await _registerValidatePayment.GetContratoByContratoId((int)SolicitudPago.ContratoId, SolicitudPagoId);
                    SolicitudPago.ContratoSon.ListProyectos = await _registerValidatePayment.GetProyectosByIdContrato((int)SolicitudPago.ContratoId);
                }
                if (SolicitudPago.OrdenGiroId != null)
                {
                    SolicitudPago.OrdenGiro = _context.OrdenGiro
                        .Where(o => o.OrdenGiroId == SolicitudPago.OrdenGiroId)
                            .Include(t => t.OrdenGiroTercero).ThenInclude(o => o.OrdenGiroTerceroChequeGerencia)
                            .Include(t => t.OrdenGiroTercero).ThenInclude(o => o.OrdenGiroTerceroTransferenciaElectronica)
                            .Include(d => d.OrdenGiroDetalle).ThenInclude(e => e.OrdenGiroDetalleEstrategiaPago)
                            .Include(d => d.OrdenGiroDetalle).ThenInclude(e => e.OrdenGiroDetalleTerceroCausacion).ThenInclude(r => r.OrdenGiroDetalleTerceroCausacionDescuento)
                            .Include(d => d.OrdenGiroDetalle).ThenInclude(e => e.OrdenGiroDetalleTerceroCausacion).ThenInclude(r => r.OrdenGiroDetalleTerceroCausacionAportante).ThenInclude(r => r.FuenteFinanciacion).ThenInclude(r => r.CuentaBancaria)
                            .Include(d => d.OrdenGiroDetalle).ThenInclude(e => e.OrdenGiroDetalleTerceroCausacion).ThenInclude(r => r.OrdenGiroDetalleTerceroCausacionAportante).ThenInclude(r => r.CuentaBancaria)
                            .Include(d => d.OrdenGiroDetalle).ThenInclude(e => e.OrdenGiroSoporte)
                            .Include(d => d.OrdenGiroDetalle).ThenInclude(e => e.OrdenGiroDetalleObservacion)
                            .Include(d => d.OrdenGiroDetalle).ThenInclude(e => e.OrdenGiroDetalleDescuentoTecnica).ThenInclude(e => e.OrdenGiroDetalleDescuentoTecnicaAportante)
                            .Include(d => d.SolicitudPago)
                        .AsNoTracking().FirstOrDefault();
                }


                SolicitudPago.TablaDRP = GetDrpContrato(SolicitudPago);
            }
            catch (Exception ex)
            {
            }
            return SolicitudPago;
        }

        private List<TablaDRP> GetDrpContrato(SolicitudPago SolicitudPago)
        {
            String strTipoSolicitud = SolicitudPago.ContratoSon.Contratacion.TipoSolicitudCodigo;
            List<TablaDRP> ListTablaDrp = new List<TablaDRP>();

            decimal ValorFacturado = SolicitudPago?.OrdenGiro?.TieneBalance == false ? SolicitudPago?.OrdenGiro?.ValorNetoGiro ?? 0 : SolicitudPago?.OrdenGiro?.ValorNetoGiroBalance ?? 0 ;


            List<VRpsPorContratacion> vRpsPorContratacion =
                                                           _context.VRpsPorContratacion
                                                           .Where(c => c.ContratacionId == SolicitudPago.ContratoSon.ContratacionId)
                                                           .OrderBy(C => C.ContratacionId)
                                                           .ToList();

            int Enum = 1;
            foreach (var DPR in vRpsPorContratacion)
            {
                ValorFacturado = (DPR.ValorSolicitud - ValorFacturado) > 0 ? (DPR.ValorSolicitud - ValorFacturado) : DPR.ValorSolicitud;

                ListTablaDrp.Add(new TablaDRP
                {
                    Enum = Enum,
                    NumeroDRP = DPR.NumeroDrp,
                    Valor = '$' + String.Format("{0:n0}", DPR.ValorSolicitud),
                    Saldo = '$' + String.Format("{0:n0}", ValorFacturado)
                });
                Enum++;
            }


            return ListTablaDrp;
        }
        #endregion

        #region validate 

        private bool ValidarRegistroCompletoOrdenGiroDetalleTerceroCausacionDescuento(OrdenGiroDetalleTerceroCausacionDescuento ordenGiroDetalleTerceroCausacionDescuento)
        {
            if (string.IsNullOrEmpty(ordenGiroDetalleTerceroCausacionDescuento.TipoDescuentoCodigo)
               || ordenGiroDetalleTerceroCausacionDescuento.ValorDescuento == 0
               || string.IsNullOrEmpty(ordenGiroDetalleTerceroCausacionDescuento.TipoDescuentoCodigo)
                ) return false;

            return true;
        }

        private bool ValidarRegistroCompletoOrdenGiroDetalleTerceroCausacion(OrdenGiroDetalleTerceroCausacion pOrdenGiroDetalleTerceroCausacion)
        {
            if (pOrdenGiroDetalleTerceroCausacion.ValorNetoGiro == 0
               || string.IsNullOrEmpty(pOrdenGiroDetalleTerceroCausacion.ConceptoPagoCriterio)
               || string.IsNullOrEmpty(pOrdenGiroDetalleTerceroCausacion.TipoPagoCodigo)
               || pOrdenGiroDetalleTerceroCausacion.ValorNetoGiro == 0
               || !pOrdenGiroDetalleTerceroCausacion.TieneDescuento.HasValue
                ) return false;

            if (pOrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionDescuento.Count() == 0)
                return false;

            foreach (var item in pOrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionDescuento)
            {
                if (!ValidarRegistroCompletoOrdenGiroDetalleTerceroCausacionDescuento(item))
                    return false;
            }
            return true;
        }

        private bool ValidarRegistroCompletoOrdenGiroTerceroTransferenciaElectronica(OrdenGiroTerceroTransferenciaElectronica pOrdenGiroTerceroTransferenciaElectronica)
        {
            if (string.IsNullOrEmpty(pOrdenGiroTerceroTransferenciaElectronica.TitularCuenta)
                || string.IsNullOrEmpty(pOrdenGiroTerceroTransferenciaElectronica.TitularNumeroIdentificacion)
                || string.IsNullOrEmpty(pOrdenGiroTerceroTransferenciaElectronica.NumeroCuenta)
                || string.IsNullOrEmpty(pOrdenGiroTerceroTransferenciaElectronica.BancoCodigo)
                || !pOrdenGiroTerceroTransferenciaElectronica.EsCuentaAhorros.HasValue
                )
                return false;
            return true;
        }

        private bool ValidarRegistroCompletoOrdenGiroTerceroChequeGerencia(OrdenGiroTerceroChequeGerencia pOrdenGiroTerceroChequeGerencia)
        {
            if (string.IsNullOrEmpty(pOrdenGiroTerceroChequeGerencia.NombreBeneficiario)
               || string.IsNullOrEmpty(pOrdenGiroTerceroChequeGerencia.NumeroIdentificacionBeneficiario)
               || string.IsNullOrEmpty(pOrdenGiroTerceroChequeGerencia.NumeroIdentificacionBeneficiario)
                )
                return false;

            return true;
        }

        private bool ValidarRegistroCompletoOrdenGiroTercero(OrdenGiroTercero pOrdenGiroTercero)
        {
            if (string.IsNullOrEmpty(pOrdenGiroTercero.MedioPagoGiroCodigo))
                return false;

            if (pOrdenGiroTercero.MedioPagoGiroCodigo == ConstanCodigoMedioPagoGiroTercero.Transferencia_electronica)
            {
                if (pOrdenGiroTercero.OrdenGiroTerceroTransferenciaElectronica.Count() == 0)
                    return false;
                if (!ValidarRegistroCompletoOrdenGiroTerceroTransferenciaElectronica(pOrdenGiroTercero.OrdenGiroTerceroTransferenciaElectronica.FirstOrDefault()))
                    return false;
            }
            else
            {
                if (pOrdenGiroTercero.OrdenGiroTerceroChequeGerencia.Count() == 0)
                    return false;
                if (!ValidarRegistroCompletoOrdenGiroTerceroChequeGerencia(pOrdenGiroTercero.OrdenGiroTerceroChequeGerencia.FirstOrDefault()))
                    return false;
            }

            return true;
        }

        private bool ValidarRegistroCompletoOrdenGiro(OrdenGiro pOrdenGiro)
        {
            if (
                   pOrdenGiro.OrdenGiroDetalle == null
                || pOrdenGiro.OrdenGiroTercero == null
                ) return false;

            if (pOrdenGiro.OrdenGiroDetalle.Count() == 0
                || pOrdenGiro.OrdenGiroTercero.Count() == 0
                ) return false;

            foreach (var item in pOrdenGiro.OrdenGiroDetalle)
            {
                if (!ValidarRegistroCompletoOrdenGiroDetalle(item))
                    return false;
            }

            foreach (var item in pOrdenGiro.OrdenGiroTercero)
            {
                if (!ValidarRegistroCompletoOrdenGiroTercero(item))
                    return false;
            }

            return true;
        }

        private bool ValidarRegistroCompletoOrdenGiroDetalleDescuentoTecnicaAportante(OrdenGiroDetalleDescuentoTecnicaAportante pOrdenGiroDetalleDescuentoTecnicaAportante)
        {
            if (
                  pOrdenGiroDetalleDescuentoTecnicaAportante.AportanteId == 0
               || pOrdenGiroDetalleDescuentoTecnicaAportante.ValorDescuento == null
               || string.IsNullOrEmpty(pOrdenGiroDetalleDescuentoTecnicaAportante.ConceptoPagoCodigo)
               || string.IsNullOrEmpty(pOrdenGiroDetalleDescuentoTecnicaAportante.FuenteRecursosCodigo)
                )
                return false;

            return true;
        }

        private bool ValidarRegistroCompletoOrdenGiroDetalleDescuentoTecnica(OrdenGiroDetalleDescuentoTecnica pOrdenGiroDetalleDescuentoTecnica)
        {
            if (string.IsNullOrEmpty(pOrdenGiroDetalleDescuentoTecnica.TipoPagoCodigo)
                || pOrdenGiroDetalleDescuentoTecnica.SolicitudPagoFaseFacturaDescuentoId == 0)
                return false;

            if (pOrdenGiroDetalleDescuentoTecnica?.OrdenGiroDetalleDescuentoTecnicaAportante?.Count() == 0)
                return false;

            foreach (var OrdenGiroDetalleDescuentoTecnicaAportante in pOrdenGiroDetalleDescuentoTecnica.OrdenGiroDetalleDescuentoTecnicaAportante)
            {
                if (!ValidarRegistroCompletoOrdenGiroDetalleDescuentoTecnicaAportante(OrdenGiroDetalleDescuentoTecnicaAportante))
                    return false;
            }
            return true;
        }

        private bool ValidarRegistroCompletoOrdenGiroDetalle(OrdenGiroDetalle pOrdenGiroDetalle)
        {
            if (
                   pOrdenGiroDetalle?.OrdenGiroDetalleTerceroCausacion == null
                || pOrdenGiroDetalle?.OrdenGiroDetalleObservacion == null
                || pOrdenGiroDetalle?.OrdenGiroSoporte == null
                || pOrdenGiroDetalle?.OrdenGiroDetalleDescuentoTecnica == null
                || pOrdenGiroDetalle?.OrdenGiroDetalleEstrategiaPago == null
                ) return false;


            foreach (var item in pOrdenGiroDetalle.OrdenGiroDetalleTerceroCausacion)
            {
                if (!ValidarRegistroCompletoOrdenGiroDetalleTerceroCausacion(item))
                    return false;
            }

            foreach (var item in pOrdenGiroDetalle.OrdenGiroDetalleObservacion)
            {
                if (string.IsNullOrEmpty(item.Observacion))
                    return false;
            }

            foreach (var item in pOrdenGiroDetalle.OrdenGiroSoporte)
            {
                if (string.IsNullOrEmpty(item.UrlSoporte))
                    return false;
            }

            foreach (var item in pOrdenGiroDetalle.OrdenGiroDetalleDescuentoTecnica)
            {
                if (!ValidarRegistroCompletoOrdenGiroDetalleDescuentoTecnica(item))
                    return false;
            }

            foreach (var item in pOrdenGiroDetalle.OrdenGiroDetalleEstrategiaPago)
            {
                if (string.IsNullOrEmpty(item.EstrategiaPagoCodigo))
                    return false;
            }

            return true;
        }

        #endregion 
    }
}
