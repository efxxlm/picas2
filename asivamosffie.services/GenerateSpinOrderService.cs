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
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly IDocumentService _documentService;
        private readonly IRegisterValidatePaymentRequierementsService _registerValidatePayment;

        public GenerateSpinOrderService(IDocumentService documentService, IRegisterValidatePaymentRequierementsService registerValidatePaymentRequierementsService, devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _documentService = documentService;
            _commonService = commonService;
            _context = context;
            _registerValidatePayment = registerValidatePaymentRequierementsService;
        }

        #region create 
        public async Task<Respuesta> CreateEditOrdenGiro(OrdenGiro pOrdenGiro)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Orden_Giro, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                if (pOrdenGiro?.OrdenGiroTercero.Count() > 0)
                    await CreateEditOrdenGiroTercero(pOrdenGiro.OrdenGiroTercero.FirstOrDefault(), pOrdenGiro.UsuarioCreacion);

                if (pOrdenGiro?.OrdenGiroDetalle != null)
                    await CreateEditOrdenGiroDetalle(pOrdenGiro.OrdenGiroDetalle.FirstOrDefault(), pOrdenGiro.UsuarioCreacion);

                if (pOrdenGiro.OrdenGiroId == 0)
                {
                    pOrdenGiro.FechaCreacion = DateTime.Now;
                    pOrdenGiro.Eliminado = false;
                    pOrdenGiro.EstadoCodigo = ((int)EnumEstadoOrdenGiro.En_Proceso_Generacion).ToString();
                    pOrdenGiro.RegistroCompleto = ValidarRegistroCompletoOrdenGiro(pOrdenGiro);
                    _context.OrdenGiro.Add(pOrdenGiro);
                    _context.SaveChanges();
                    await _context.Set<SolicitudPago>()
                                    .Where(o => o.SolicitudPagoId == pOrdenGiro.SolicitudPagoId)
                                                                                        .UpdateAsync(r => new SolicitudPago()
                                                                                        {
                                                                                            FechaModificacion = DateTime.Now,
                                                                                            UsuarioModificacion = pOrdenGiro.UsuarioModificacion,

                                                                                            OrdenGiroId = pOrdenGiro.OrdenGiroId
                                                                                        });
                }
                return
                     new Respuesta
                     {
                         IsSuccessful = true,
                         IsException = false,
                         IsValidation = false,
                         Code = GeneralCodes.OperacionExitosa,
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Generar_Orden_de_giro, GeneralCodes.OperacionExitosa, idAccion, "", "CREAR ORDEN GIRO")
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

        private void CreateEditOrdenGiroDetalleObservacion(OrdenGiroObservacion pOrdenGiroObservacion, string pUsuarioCreacion)
        {
            if (pOrdenGiroObservacion.OrdenGiroObservacionId == 0)
            {
                pOrdenGiroObservacion.UsuarioCreacion = pUsuarioCreacion;
                pOrdenGiroObservacion.FechaCreacion = DateTime.Now;
                pOrdenGiroObservacion.Eliminado = false;
                pOrdenGiroObservacion.RegistroCompleto = string.IsNullOrEmpty(pOrdenGiroObservacion.Observacion);
                _context.OrdenGiroObservacion.Add(pOrdenGiroObservacion);
            }
            else
            {
                _context.Set<OrdenGiroObservacion>()
                        .Where(o => o.OrdenGiroObservacionId == pOrdenGiroObservacion.OrdenGiroObservacionId)
                        .Update(o => new OrdenGiroObservacion
                        {
                            UsuarioModificacion = pUsuarioCreacion,
                            FechaModificacion = DateTime.Now,
                            RegistroCompleto = string.IsNullOrEmpty(pOrdenGiroObservacion.Observacion),
                            Observacion = pOrdenGiroObservacion.Observacion
                        });
            }
        }

        private async Task<int> CreateEditOrdenGiroDetalle(OrdenGiroDetalle pOrdenGiroDetalle, string pUsuarioCreacion)
        {
            int? OrdenGiroDetalleEstrategiaPagoId = null;
            int? OrdenGiroDetalleDescuentoTecnicaId = null;
            int? OrdenGiroDetalleTerceroCausacionId = null;

            if (pOrdenGiroDetalle?.OrdenGiroDetalleEstrategiaPago?.Count() > 0)
                CreateEditOrdenGiroDetalleEstrategiaPago(pOrdenGiroDetalle.OrdenGiroDetalleEstrategiaPago.FirstOrDefault(), pUsuarioCreacion);

            if (pOrdenGiroDetalle?.OrdenGiroObservacion?.Count() > 0)
                CreateEditOrdenGiroDetalleObservacion(pOrdenGiroDetalle.OrdenGiroObservacion.FirstOrDefault(), pUsuarioCreacion);

            if (pOrdenGiroDetalle?.OrdenGiroSoporte?.Count() > 0)
                CreateEditOrdenGiroSoporte(pOrdenGiroDetalle.OrdenGiroSoporte.FirstOrDefault(), pUsuarioCreacion);

            if (pOrdenGiroDetalle?.OrdenGiroDetalleEstrategiaPago != null)
                CreateEditOrdenGiroDetalleEstrategiaPago(pOrdenGiroDetalle.OrdenGiroDetalleEstrategiaPago.FirstOrDefault(), pUsuarioCreacion);

            if (pOrdenGiroDetalle?.OrdenGiroDetalleTerceroCausacion != null)
                CreateEditOrdenGiroDetalleTerceroCausacion(pOrdenGiroDetalle?.OrdenGiroDetalleTerceroCausacion?.FirstOrDefault(), pUsuarioCreacion);



            if (pOrdenGiroDetalle.OrdenGiroDetalleId == 0)
            {
                pOrdenGiroDetalle.UsuarioCreacion = pUsuarioCreacion;
                pOrdenGiroDetalle.FechaCreacion = DateTime.Now;
                pOrdenGiroDetalle.Eliminado = false;
                pOrdenGiroDetalle.RegistroCompleto = ValidarRegistroCompletoOrdenGiroDetalle(pOrdenGiroDetalle);

                _context.OrdenGiroDetalle.Add(pOrdenGiroDetalle);
            }
            else
            {
                await _context.Set<OrdenGiroDetalle>()
                                                    .Where(o => o.OrdenGiroDetalleId == pOrdenGiroDetalle.OrdenGiroDetalleId)
                                                                                                                            .UpdateAsync(r => new OrdenGiroDetalle()
                                                                                                                            {
                                                                                                                                FechaModificacion = DateTime.Now,
                                                                                                                                UsuarioModificacion = pUsuarioCreacion,
                                                                                                                                RegistroCompleto = ValidarRegistroCompletoOrdenGiroDetalle(pOrdenGiroDetalle)
                                                                                                                            });
            }


            return pOrdenGiroDetalle.OrdenGiroDetalleId;
        }

        private void CreateEditOrdenGiroSoporte(OrdenGiroSoporte ordenGiroSoporte, string pUsuarioCreacion)
        {
            if (ordenGiroSoporte.OrdenGiroSoporteId == 0)
            {
                ordenGiroSoporte.UsuarioCreacion = pUsuarioCreacion;
                ordenGiroSoporte.Eliminado = false;
                ordenGiroSoporte.FechaCreacion = DateTime.Now;
                ordenGiroSoporte.RegistroCompleto = string.IsNullOrEmpty(ordenGiroSoporte.UrlSoporte);
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
                            RegistroCompleto = string.IsNullOrEmpty(ordenGiroSoporte.UrlSoporte),
                            UrlSoporte = ordenGiroSoporte.UrlSoporte
                        });
            }
        }

        private void CreateEditOrdenGiroDetalleTerceroCausacion(OrdenGiroDetalleTerceroCausacion pOrdenGiroDetalleTerceroCausacion, string pUsuarioCreacion)
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
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = pUsuarioCreacion,
                            RegistroCompleto = ValidarRegistroCompletoOrdenGiroDetalleTerceroCausacion(pOrdenGiroDetalleTerceroCausacion)
                        });
            }
        }

        private void CreateEditOrdenGiroDetalleDescuentoTecnica(OrdenGiroDetalleDescuentoTecnica pOrdenGiroDetalleDescuentoTecnica, string pUsuarioCreacion)
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
                                 FechaModificacion = DateTime.Now,
                                 UsuarioModificacion = pUsuarioCreacion,
                                 RegistroCompleto = ValidarRegistroCompletoOrdenGiroDetalleDescuentoTecnica(pOrdenGiroDetalleDescuentoTecnica)
                             });
            }
            CreateEditOrdenGiroDetalleDescuentoTecnicaAportante(pOrdenGiroDetalleDescuentoTecnica.OrdenGiroDetalleDescuentoTecnicaAportante, pUsuarioCreacion);

        }

        private async void CreateEditOrdenGiroDetalleDescuentoTecnicaAportante(ICollection<OrdenGiroDetalleDescuentoTecnicaAportante> pOrdenGiroDetalleDescuentoTecnicaAportanteList, string pUsuarioCreacion)
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
                    await _context.Set<OrdenGiroDetalleDescuentoTecnicaAportante>()
                                                        .Where(o => o.OrdenGiroDetalleDescuentoTecnicaAportanteId == pOrdenGiroDetalleDescuentoTecnicaAportante.OrdenGiroDetalleDescuentoTecnicaAportanteId)
                                                                                                                                .UpdateAsync(r => new OrdenGiroDetalleDescuentoTecnicaAportante()
                                                                                                                                {
                                                                                                                                    FechaModificacion = DateTime.Now,
                                                                                                                                    UsuarioModificacion = pUsuarioCreacion,
                                                                                                                                    RegistroCompleto = ValidarRegistroCompletoOrdenGiroDetalleDescuentoTecnicaAportante(pOrdenGiroDetalleDescuentoTecnicaAportante),

                                                                                                                                    ValorDescuento = pOrdenGiroDetalleDescuentoTecnicaAportante.ValorDescuento
                                                                                                                                });
                }
            }

        }

        private void CreateEditOrdenGiroDetalleEstrategiaPago(OrdenGiroDetalleEstrategiaPago pOrdenGiroDetalleEstrategiaPago, string pUsuarioCreacion)
        {
            if (pOrdenGiroDetalleEstrategiaPago.OrdenGiroDetalleEstrategiaPagoId == 0)
            {
                pOrdenGiroDetalleEstrategiaPago.UsuarioCreacion = pUsuarioCreacion;
                pOrdenGiroDetalleEstrategiaPago.FechaCreacion = DateTime.Now;
                pOrdenGiroDetalleEstrategiaPago.Eliminado = false;
                pOrdenGiroDetalleEstrategiaPago.RegistroCompleto = string.IsNullOrEmpty(pOrdenGiroDetalleEstrategiaPago.EstrategiaPagoCodigo);

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
                            RegistroCompleto = string.IsNullOrEmpty(pOrdenGiroDetalleEstrategiaPago.EstrategiaPagoCodigo),
                            EstrategiaPagoCodigo = pOrdenGiroDetalleEstrategiaPago.EstrategiaPagoCodigo
                        });
            }
        }

        private async Task CreateEditOrdenGiroTercero(OrdenGiroTercero pOrdenGiroTercero, string pUsuarioCreacion)
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
                await _context.Set<OrdenGiroTercero>()
                                                    .Where(o => o.OrdenGiroTerceroId == pOrdenGiroTercero.OrdenGiroTerceroId)
                                                                                                                            .UpdateAsync(r => new OrdenGiroTercero()
                                                                                                                            {
                                                                                                                                FechaModificacion = DateTime.Now,
                                                                                                                                UsuarioModificacion = pUsuarioCreacion,
                                                                                                                                RegistroCompleto = ValidarRegistroCompletoOrdenGiroTercero(pOrdenGiroTercero),
                                                                                                                                MedioPagoGiroCodigo = pOrdenGiroTercero.MedioPagoGiroCodigo,
                                                                                                                            });
            }

            if (pOrdenGiroTercero.MedioPagoGiroCodigo == ConstanCodigoMedioPagoGiroTercero.Transferencia_electronica)
                await CreateEditOrdenGiroTerceroTransferenciaElectronica(pOrdenGiroTercero.OrdenGiroTerceroTransferenciaElectronica.FirstOrDefault(), pUsuarioCreacion);

            if (pOrdenGiroTercero.MedioPagoGiroCodigo == ConstanCodigoMedioPagoGiroTercero.Cheque_de_gerencia)
                await CreateEditOrdenGiroTerceroChequeGerencia(pOrdenGiroTercero.OrdenGiroTerceroChequeGerencia.FirstOrDefault(), pUsuarioCreacion);
        }

        private async Task CreateEditOrdenGiroTerceroChequeGerencia(OrdenGiroTerceroChequeGerencia pOrdenGiroTerceroChequeGerencia, string pUsuarioCreacion)
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
                await _context.Set<OrdenGiroTerceroChequeGerencia>()
                                              .Where(o => o.OrdenGiroTerceroChequeGerenciaId == pOrdenGiroTerceroChequeGerencia.OrdenGiroTerceroChequeGerenciaId)
                                                                                                                      .UpdateAsync(r => new OrdenGiroTerceroChequeGerencia()
                                                                                                                      {
                                                                                                                          FechaModificacion = DateTime.Now,
                                                                                                                          UsuarioModificacion = pUsuarioCreacion,
                                                                                                                          RegistroCompleto = ValidarRegistroCompletoOrdenGiroTerceroChequeGerencia(pOrdenGiroTerceroChequeGerencia),

                                                                                                                          NombreBeneficiario = pOrdenGiroTerceroChequeGerencia.NombreBeneficiario,
                                                                                                                          NumeroIdentificacionBeneficiario = pOrdenGiroTerceroChequeGerencia.NumeroIdentificacionBeneficiario,
                                                                                                                      });
            }
        }

        private async Task CreateEditOrdenGiroTerceroTransferenciaElectronica(OrdenGiroTerceroTransferenciaElectronica pOrdenGiroTerceroTransferenciaElectronica, string pUsuarioCreacion)
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
                await _context.Set<OrdenGiroTerceroTransferenciaElectronica>()
                                    .Where(o => o.OrdenGiroTerceroTransferenciaElectronicaId == pOrdenGiroTerceroTransferenciaElectronica.OrdenGiroTerceroTransferenciaElectronicaId)
                                                                                                            .UpdateAsync(r => new OrdenGiroTerceroTransferenciaElectronica()
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


        #endregion

        #region validate 
        private bool? ValidarRegistroCompletoOrdenGiroDetalleTerceroCausacion(OrdenGiroDetalleTerceroCausacion pOrdenGiroDetalleTerceroCausacion)
        {
            return false;
        }
        private bool ValidarRegistroCompletoOrdenGiroTerceroTransferenciaElectronica(OrdenGiroTerceroTransferenciaElectronica pOrdenGiroTerceroTransferenciaElectronica)
        {
            return false;
        }

        private bool ValidarRegistroCompletoOrdenGiroTerceroChequeGerencia(OrdenGiroTerceroChequeGerencia pOrdenGiroTerceroChequeGerencia)
        {
            return false;
        }

        private bool ValidarRegistroCompletoOrdenGiroTercero(OrdenGiroTercero pOrdenGiroTercero)
        {
            return false;
        }

        private bool ValidarRegistroCompletoOrdenGiro(OrdenGiro pOrdenGiro)
        {
            return false;
        }

        private bool ValidarRegistroCompletoOrdenGiroDetalleDescuentoTecnicaAportante(OrdenGiroDetalleDescuentoTecnicaAportante pOrdenGiroDetalleDescuentoTecnicaAportante)
        {
            return false;
        }

        private bool ValidarRegistroCompletoOrdenGiroDetalleDescuentoTecnica(OrdenGiroDetalleDescuentoTecnica pOrdenGiroDetalleDescuentoTecnica)
        {
            return false;
        }



        private bool ValidarRegistroCompletoOrdenGiroDetalle(OrdenGiroDetalle pOrdenGiroDetalle)
        {
            return false;
        }


        #endregion

        #region get
        /// <summary>
        /// TODO : VALIDAR SOLICITUDES DE PAGO QUE YA TENGAN APROBACION 
        /// </summary>
        /// <returns></returns>
        /// 

        public async Task<dynamic> GetListOrdenGiro(int pMenuId)
        {
            return pMenuId switch
            {
                (int)enumeratorMenu.Generar_Orden_de_giro => await _context.VOrdenGiro.Where(s =>
                                             s.IntEstadoCodigo >= (int)EnumEstadoOrdenGiro.Enviada_A_Order_Giro)
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
                    SolicitudPago.ContratoSon = await _registerValidatePayment.GetContratoByContratoId((int)SolicitudPago.ContratoId, 0);
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
                            .Include(d => d.OrdenGiroDetalle).ThenInclude(e => e.OrdenGiroObservacion)
                            .Include(d => d.OrdenGiroDetalle).ThenInclude(e => e.OrdenGiroSoporte)
                            .Include(d => d.SolicitudPago)
                        .AsNoTracking().FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
            }
            return SolicitudPago;
        }
        #endregion


    }
}
