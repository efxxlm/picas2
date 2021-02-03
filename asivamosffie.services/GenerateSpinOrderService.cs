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
                                                                      || d.TipoDominioId == (int)EnumeratorTipoDominio.Estados_Registro_Pago
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
                    SolicitudPago.OrdenGiro = _context.OrdenGiro.Where(o => o.OrdenGiroId == SolicitudPago.OrdenGiroId)
                        .Include(t => t.OrdenGiroTercero).ThenInclude(o=> o.OrdenGiroTerceroChequeGerencia)
                        .Include(t => t.OrdenGiroTercero).ThenInclude(o => o.OrdenGiroTerceroTransferenciaElectronica)
                        .Include(d => d.OrdenGiroDetalle).ThenInclude(e => e.OrdenGiroDetalleEstrategiaPago)
                        .Include(d => d.SolicitudPago).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

            }

            return SolicitudPago;
        }

        public async Task<Respuesta> CreateEditOrdenGiro(OrdenGiro pOrdenGiro)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Orden_Giro, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                if (pOrdenGiro.OrdenGiroId == 0)
                {
                    pOrdenGiro.FechaCreacion = DateTime.Now;
                    pOrdenGiro.Eliminado = false;
                    pOrdenGiro.EstadoCodigo = ConstanCodigoEstadoOrdenGiro.En_proceso_de_generacion;
                    pOrdenGiro.RegistroCompleto = ValidarRegistroCompletoOrdenGiro(pOrdenGiro);

                    _context.OrdenGiro.Add(pOrdenGiro);
                }
                else
                {
                    await _context.Set<OrdenGiro>()
                                                    .Where(o => o.OrdenGiroId == pOrdenGiro.OrdenGiroId)
                                                                                                        .UpdateAsync(r => new OrdenGiro()
                                                                                                        {
                                                                                                            FechaModificacion = DateTime.Now,
                                                                                                            UsuarioModificacion = pOrdenGiro.UsuarioModificacion,
                                                                                                            RegistroCompleto = ValidarRegistroCompletoOrdenGiro(pOrdenGiro)
                                                                                                        });
                }

                CreateEditOrdenGiroTercero(pOrdenGiro.OrdenGiroTercero, pOrdenGiro.UsuarioCreacion);

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

        private async void CreateEditOrdenGiroTercero(OrdenGiroTercero pOrdenGiroTercero, string pUsuarioCreacion)
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
        }

        private async void CreateEditOrdenGiroTerceroChequeGerencia(OrdenGiroTerceroChequeGerencia pOrdenGiroTerceroChequeGerencia, string pUsuarioCreacion)
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

        private async void CreateEditOrdenGiroTerceroTransferenciaElectronica(OrdenGiroTerceroTransferenciaElectronica pOrdenGiroTerceroTransferenciaElectronica, string pUsuarioCreacion)
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
    }
}
