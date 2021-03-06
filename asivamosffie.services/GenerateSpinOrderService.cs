﻿using asivamosffie.model.APIModels;
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
        /// <summary>
        /// create edit
        /// </summary>
        /// <param name="pOrdenGiro"></param>
        /// <returns></returns>
        public async Task<Respuesta> CreateEditOrdenGiro(OrdenGiro pOrdenGiro)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Orden_Giro, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                int? OrdenGiroTerceroId = null;
                int? OrdenGiroDetalleId = null;

                if (pOrdenGiro.OrdenGiroTercero != null)
                    OrdenGiroTerceroId = await CreateEditOrdenGiroTercero(pOrdenGiro.OrdenGiroTercero, pOrdenGiro.UsuarioCreacion);
                if (pOrdenGiro.OrdenGiroDetalle != null)
                    OrdenGiroDetalleId = await CreateEditOrdenGiroDetalle(pOrdenGiro.OrdenGiroDetalle, pOrdenGiro.UsuarioCreacion);

                if (pOrdenGiro.OrdenGiroId == 0)
                {
                    pOrdenGiro.FechaCreacion = DateTime.Now;
                    pOrdenGiro.Eliminado = false;
                    pOrdenGiro.EstadoCodigo = ConstanCodigoEstadoOrdenGiro.En_proceso_de_generacion;
                    pOrdenGiro.RegistroCompleto = ValidarRegistroCompletoOrdenGiro(pOrdenGiro);
                    pOrdenGiro.OrdenGiroTerceroId = OrdenGiroTerceroId;
                    pOrdenGiro.OrdenGiroDetalleId = OrdenGiroDetalleId;

                    _context.OrdenGiro.Add(pOrdenGiro);

                    await _context.Set<SolicitudPago>()
                                    .Where(o => o.SolicitudPagoId == pOrdenGiro.SolicitudPagoId)
                                                                                        .UpdateAsync(r => new SolicitudPago()
                                                                                        {
                                                                                            FechaModificacion = DateTime.Now,
                                                                                            UsuarioModificacion = pOrdenGiro.UsuarioModificacion,

                                                                                            OrdenGiroId = pOrdenGiro.OrdenGiroId
                                                                                        });
                }
                else
                {
                    await _context.Set<OrdenGiro>()
                                                    .Where(o => o.OrdenGiroId == pOrdenGiro.OrdenGiroId)
                                                                                                        .UpdateAsync(r => new OrdenGiro()
                                                                                                        {
                                                                                                            FechaModificacion = DateTime.Now,
                                                                                                            UsuarioModificacion = pOrdenGiro.UsuarioModificacion,
                                                                                                            RegistroCompleto = ValidarRegistroCompletoOrdenGiro(pOrdenGiro),

                                                                                                            OrdenGiroTerceroId = OrdenGiroTerceroId,
                                                                                                            OrdenGiroDetalleId = OrdenGiroDetalleId
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

        private async Task<int> CreateEditOrdenGiroDetalle(OrdenGiroDetalle pOrdenGiroDetalle, string pUsuarioCreacion)
        {
            int? OrdenGiroDetalleEstrategiaPagoId = null;
            int? OrdenGiroDetalleDescuentoTecnicaId = null;
            int? OrdenGiroDetalleTerceroCausacionId = null;

            if (pOrdenGiroDetalle.OrdenGiroDetalleEstrategiaPago != null)
                OrdenGiroDetalleEstrategiaPagoId = await CreateEditOrdenGiroDetalleEstrategiaPago(pOrdenGiroDetalle.OrdenGiroDetalleEstrategiaPago, pUsuarioCreacion);

            if (pOrdenGiroDetalle.OrdenGiroDetalleDescuentoTecnica != null)
                OrdenGiroDetalleDescuentoTecnicaId = await CreateEditOrdenGiroDetalleDescuentoTecnica(pOrdenGiroDetalle.OrdenGiroDetalleDescuentoTecnica, pUsuarioCreacion);

            if (pOrdenGiroDetalle.OrdenGiroDetalleTerceroCausacion != null)
                OrdenGiroDetalleTerceroCausacionId = await CreateEditOrdenGiroDetalleTerceroCausacion(pOrdenGiroDetalle.OrdenGiroDetalleTerceroCausacion, pUsuarioCreacion);

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

        private async Task<int> CreateEditOrdenGiroDetalleTerceroCausacion(OrdenGiroDetalleTerceroCausacion pOrdenGiroDetalleTerceroCausacion, string pUsuarioCreacion)
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
                await _context.Set<OrdenGiroDetalleTerceroCausacion>()
                                                    .Where(o => o.OrdenGiroDetalleTerceroCausacionId == pOrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionId)
                                                                                                                            .UpdateAsync(r => new OrdenGiroDetalleTerceroCausacion()
                                                                                                                            {
                                                                                                                                FechaModificacion = DateTime.Now,
                                                                                                                                UsuarioModificacion = pUsuarioCreacion,
                                                                                                                                RegistroCompleto = ValidarRegistroCompletoOrdenGiroDetalleTerceroCausacion(pOrdenGiroDetalleTerceroCausacion)
                                                                                                                            });
            }
            return pOrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionId;
        }

        private async Task<int> CreateEditOrdenGiroDetalleDescuentoTecnica(OrdenGiroDetalleDescuentoTecnica pOrdenGiroDetalleDescuentoTecnica, string pUsuarioCreacion)
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
                await _context.Set<OrdenGiroDetalleDescuentoTecnica>()
                                                    .Where(o => o.OrdenGiroDetalleDescuentoTecnicaId == pOrdenGiroDetalleDescuentoTecnica.OrdenGiroDetalleDescuentoTecnicaId)
                                                                                                                            .UpdateAsync(r => new OrdenGiroDetalleDescuentoTecnica()
                                                                                                                            {
                                                                                                                                FechaModificacion = DateTime.Now,
                                                                                                                                UsuarioModificacion = pUsuarioCreacion,
                                                                                                                                RegistroCompleto = ValidarRegistroCompletoOrdenGiroDetalleDescuentoTecnica(pOrdenGiroDetalleDescuentoTecnica)
                                                                                                                            });
            }

            CreateEditOrdenGiroDetalleDescuentoTecnicaAportante(pOrdenGiroDetalleDescuentoTecnica.OrdenGiroDetalleDescuentoTecnicaAportante, pUsuarioCreacion);

            return pOrdenGiroDetalleDescuentoTecnica.OrdenGiroDetalleDescuentoTecnicaId;
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

        private async Task<int> CreateEditOrdenGiroDetalleEstrategiaPago(OrdenGiroDetalleEstrategiaPago pOrdenGiroDetalleEstrategiaPago, string pUsuarioCreacion)
        {
            if (pOrdenGiroDetalleEstrategiaPago.OrdenGiroDetalleEstrategiaPagoId == 0)
            {
                pOrdenGiroDetalleEstrategiaPago.UsuarioCreacion = pUsuarioCreacion;
                pOrdenGiroDetalleEstrategiaPago.FechaCreacion = DateTime.Now;
                pOrdenGiroDetalleEstrategiaPago.Eliminado = false;
                pOrdenGiroDetalleEstrategiaPago.RegistroCompleto = ValidarRegistroCompletoOrdenGiroDetalleEstrategiaPago(pOrdenGiroDetalleEstrategiaPago);

                _context.OrdenGiroDetalleEstrategiaPago.Add(pOrdenGiroDetalleEstrategiaPago);
            }
            else
            {
                await _context.Set<OrdenGiroDetalleEstrategiaPago>()
                                                    .Where(o => o.OrdenGiroDetalleEstrategiaPagoId == pOrdenGiroDetalleEstrategiaPago.OrdenGiroDetalleEstrategiaPagoId)
                                                                                                                            .UpdateAsync(r => new OrdenGiroDetalleEstrategiaPago()
                                                                                                                            {
                                                                                                                                FechaModificacion = DateTime.Now,
                                                                                                                                UsuarioModificacion = pUsuarioCreacion,
                                                                                                                                RegistroCompleto = ValidarRegistroCompletoOrdenGiroDetalleEstrategiaPago(pOrdenGiroDetalleEstrategiaPago),

                                                                                                                                EstrategiaPagoCodigo = pOrdenGiroDetalleEstrategiaPago.EstrategiaPagoCodigo
                                                                                                                            });
            }
            _context.SaveChanges();
            return pOrdenGiroDetalleEstrategiaPago.OrdenGiroDetalleEstrategiaPagoId;
        }

        private async Task<int> CreateEditOrdenGiroTercero(OrdenGiroTercero pOrdenGiroTercero, string pUsuarioCreacion)
        {
            if (pOrdenGiroTercero.OrdenGiroTerceroId == 0)
            {
                pOrdenGiroTercero.UsuarioCreacion = pUsuarioCreacion;
                pOrdenGiroTercero.FechaCreacion = DateTime.Now;
                pOrdenGiroTercero.Eliminado = false;
                pOrdenGiroTercero.RegistroCompleto = ValidarRegistroCompletoOrdenGiroTercero(pOrdenGiroTercero);

                _context.OrdenGiroTercero.Add(pOrdenGiroTercero);
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
                CreateEditOrdenGiroTerceroTransferenciaElectronica(pOrdenGiroTercero.OrdenGiroTerceroTransferenciaElectronica, pUsuarioCreacion);

            if (pOrdenGiroTercero.MedioPagoGiroCodigo == ConstanCodigoMedioPagoGiroTercero.Cheque_de_gerencia)
                CreateEditOrdenGiroTerceroChequeGerencia(pOrdenGiroTercero.OrdenGiroTerceroChequeGerencia, pUsuarioCreacion);


            return pOrdenGiroTercero.OrdenGiroTerceroId;
        }

        private async Task<int> CreateEditOrdenGiroTerceroChequeGerencia(OrdenGiroTerceroChequeGerencia pOrdenGiroTerceroChequeGerencia, string pUsuarioCreacion)
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
            return pOrdenGiroTerceroChequeGerencia.OrdenGiroTerceroChequeGerenciaId;
        }

        private async Task<int> CreateEditOrdenGiroTerceroTransferenciaElectronica(OrdenGiroTerceroTransferenciaElectronica pOrdenGiroTerceroTransferenciaElectronica, string pUsuarioCreacion)
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

            return pOrdenGiroTerceroTransferenciaElectronica.OrdenGiroTerceroTransferenciaElectronicaId;
        }


        #endregion


        #region validate 
        private bool? ValidarRegistroCompletoOrdenGiroDetalleTerceroCausacion(OrdenGiroDetalleTerceroCausacion pOrdenGiroDetalleTerceroCausacion)
        {
            throw new NotImplementedException();
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

        private bool ValidarRegistroCompletoOrdenGiroDetalleEstrategiaPago(OrdenGiroDetalleEstrategiaPago pOrdenGiroDetalleEstrategiaPago)
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
        public async Task<dynamic> GetListSolicitudPago()
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
                    EstadoOrdenGiro = ConstanCodigoEstadoOrdenGiro.Sin_generacion;
                else
                {
                    EstadoOrdenGiro = r.OrdenGiro.EstadoCodigo;
                    RegistroCompleto = r.OrdenGiro.RegistroCompleto ?? false;
                }
                EstadoOrdenGiro = ListParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Orden_Giro && r.Codigo == EstadoOrdenGiro).FirstOrDefault().Nombre;

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
                            .Include(d => d.OrdenGiroDetalle).ThenInclude(e => e.OrdenGiroDetalleDescuentoTecnica).ThenInclude(r => r.OrdenGiroDetalleDescuentoTecnicaAportante)
                            .Include(d => d.SolicitudPago)
                        .FirstOrDefault();
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