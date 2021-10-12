using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.model.APIModels;
using System.Globalization;
using Microsoft.Extensions.Hosting;
using Z.EntityFramework.Plus;
using asivamosffie.services.Helpers.Constants;

namespace asivamosffie.services
{
    public class FinancialBalanceService : IFinalBalanceService
    {
        #region Constructor
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly IRegisterValidatePaymentRequierementsService _registerValidatePaymentRequierementsService;
        private readonly IBudgetAvailabilityService _budgetAvailabilityService;
        private readonly IContractualControversy _contractualControversy;

        public FinancialBalanceService(devAsiVamosFFIEContext context,
                                       ICommonService commonService,
                                       IBudgetAvailabilityService budgetAvailabilityService,
                                       IRegisterValidatePaymentRequierementsService registerValidatePaymentRequierementsService,
                                       IContractualControversy contractualControversy
            )
        {
            _budgetAvailabilityService = budgetAvailabilityService;
            _context = context;
            _commonService = commonService;
            _registerValidatePaymentRequierementsService = registerValidatePaymentRequierementsService;
            _contractualControversy = contractualControversy;

        }
        #endregion

        #region balance

        #region C R U D

        #region create
        public async Task<Respuesta> ValidateCompleteBalanceFinanciero(int pBalanceFinancieroTrasladoId, bool pEstaCompleto)
        {
            await _context.Set<BalanceFinancieroTraslado>()
                      .Where(b => b.BalanceFinancieroTrasladoId == pBalanceFinancieroTrasladoId)
                      .UpdateAsync
                      (b => new BalanceFinancieroTraslado
                      {
                          RegistroCompleto = pEstaCompleto
                      });

            //Validar registro Completo BalanceFinanciero
            BalanceFinanciero balanceFinanciero = _context.BalanceFinanciero
                .Include(b => b.BalanceFinancieroTraslado)
                .Where(b => b.BalanceFinancieroTraslado
                  .Any(r => r.BalanceFinancieroTrasladoId == pBalanceFinancieroTrasladoId))
                .AsNoTracking()
                .FirstOrDefault();

            string strEstadoBalanceCodigo = balanceFinanciero.EstadoBalanceCodigo;

            bool BlRegistroCompleto = balanceFinanciero.BalanceFinancieroTraslado.All(r => r.RegistroCompleto == true);
            if (BlRegistroCompleto)
                strEstadoBalanceCodigo = ConstanCodigoEstadoBalanceFinanciero.Con_balance_validado;


            await _context.Set<BalanceFinanciero>()
                          .Where(b => b.BalanceFinancieroId == balanceFinanciero.BalanceFinancieroId)
                          .UpdateAsync(b => new BalanceFinanciero
                          {
                              EstadoBalanceCodigo = strEstadoBalanceCodigo,
                              RegistroCompleto = pEstaCompleto
                          });

            return new Respuesta();
        }

        public async Task<Respuesta> CreateEditBalanceFinanciero(BalanceFinanciero pBalanceFinanciero)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Balance_Financiero, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            try
            {
                if (pBalanceFinanciero.RequiereTransladoRecursos == false)
                {
                    if (pBalanceFinanciero.CumpleCondicionesTai != true)
                    {
                        pBalanceFinanciero.EstadoBalanceCodigo = ConstanCodigoEstadoBalanceFinanciero.Con_balance_validado;
                        pBalanceFinanciero.RegistroCompleto = true;
                    }
                    else
                    {
                        pBalanceFinanciero.EstadoBalanceCodigo = ConstanCodigoEstadoBalanceFinanciero.En_proceso_de_validacion;
                    }

                }
                else
                    pBalanceFinanciero.EstadoBalanceCodigo = ConstanCodigoEstadoBalanceFinanciero.Con_necesidad_de_traslado;


                if (pBalanceFinanciero.BalanceFinancieroId == 0)
                {
                    pBalanceFinanciero.FechaCreacion = DateTime.Now;
                    _context.BalanceFinanciero.Add(pBalanceFinanciero);
                }
                else
                {
                    await _context.Set<BalanceFinanciero>().Where(r => r.BalanceFinancieroId == pBalanceFinanciero.BalanceFinancieroId)
                                                                   .UpdateAsync(r => new BalanceFinanciero()
                                                                   {
                                                                       FechaModificacion = DateTime.Now,
                                                                       UsuarioModificacion = pBalanceFinanciero.UsuarioCreacion,
                                                                       RequiereTransladoRecursos = pBalanceFinanciero.RequiereTransladoRecursos,
                                                                       JustificacionTrasladoAportanteFuente = pBalanceFinanciero.JustificacionTrasladoAportanteFuente,
                                                                       UrlSoporte = pBalanceFinanciero.UrlSoporte,
                                                                       RegistroCompleto = pBalanceFinanciero.RegistroCompleto,
                                                                       EstadoBalanceCodigo = pBalanceFinanciero.EstadoBalanceCodigo
                                                                   });
                    if (pBalanceFinanciero.CumpleCondicionesTai != true)
                    {
                        CreateEditBalanceFinancieroTraslado(pBalanceFinanciero.BalanceFinancieroTraslado, pBalanceFinanciero.UsuarioCreacion);
                    }
                }
                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_balance_financiero_traslados_de_recursos, GeneralCodes.OperacionExitosa, idAccion, pBalanceFinanciero.UsuarioCreacion, !pBalanceFinanciero.FechaCreacion.HasValue ? "CREAR BALANCE FINANCIERO" : "ACTUALIZAR BALANCE FINANCIERO")
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_balance_financiero_traslados_de_recursos, GeneralCodes.Error, idAccion, pBalanceFinanciero.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        private void CreateEditBalanceFinancieroTraslado(ICollection<BalanceFinancieroTraslado> ListBalanceFinancieroTraslado, string pAuthor)
        {
            foreach (var BalanceFinancieroTraslado in ListBalanceFinancieroTraslado)
            {
                if (BalanceFinancieroTraslado.BalanceFinancieroTrasladoId == 0)
                {
                    BalanceFinancieroTraslado.FechaCreacion = DateTime.Now;
                    BalanceFinancieroTraslado.UsuarioCreacion = pAuthor;
                    BalanceFinancieroTraslado.Eliminado = false;
                    BalanceFinancieroTraslado.EstadoCodigo = ConstanCodigoEstadoTraslado.Con_registro;
                    BalanceFinancieroTraslado.ValorTraslado = BalanceFinancieroTraslado.BalanceFinancieroTrasladoValor.Sum(r => r.ValorTraslado);
                    BalanceFinancieroTraslado.NumeroTraslado = _commonService.EnumeradorTrasladoBalanceFinanciero();
                    _context.BalanceFinancieroTraslado.Add(BalanceFinancieroTraslado);
                }
                else
                {
                    _context.Set<BalanceFinancieroTraslado>()
                          .Where(r => r.BalanceFinancieroTrasladoId == BalanceFinancieroTraslado.BalanceFinancieroTrasladoId)
                          .Update(r => new BalanceFinancieroTraslado
                          {
                              UsuarioModificacion = pAuthor,
                              FechaModificacion = DateTime.Now,
                              RegistroCompleto = BalanceFinancieroTraslado.RegistroCompleto,
                              ValorTraslado = BalanceFinancieroTraslado.BalanceFinancieroTrasladoValor.Sum(r => r.ValorTraslado)
                          });
                }

                CreateEditBalanceFinancieroTrasladoValor(BalanceFinancieroTraslado.BalanceFinancieroTrasladoValor, pAuthor);


                BalanceFinancieroTraslado.OrdenGiro.ValorNetoGiroTraslado =
                //     await CreateEditOrdenGiroTraslado(BalanceFinancieroTraslado.OrdenGiro);

                _context.Set<OrdenGiro>()
                             .Where(o => o.OrdenGiroId == BalanceFinancieroTraslado.OrdenGiroId)
                             .Update(o => new OrdenGiro
                             {
                                 ValorNetoGiroTraslado = BalanceFinancieroTraslado.BalanceFinancieroTrasladoValor.Sum(r => r.ValorTraslado) ?? 0
                                 // , TieneTraslado = true
                             });
            }
        }

        private void CreateEditBalanceFinancieroTrasladoValor(ICollection<BalanceFinancieroTrasladoValor> ListBalanceFinancieroTrasladoValor, string pAuthor)
        {
            foreach (var BalanceFinancieroTrasladoValor in ListBalanceFinancieroTrasladoValor)
            {
                if (BalanceFinancieroTrasladoValor.BalanceFinancieroTrasladoValorId == 0)
                {
                    BalanceFinancieroTrasladoValor.UsuarioCreacion = pAuthor;
                    BalanceFinancieroTrasladoValor.FechaCreacion = DateTime.Now;
                    BalanceFinancieroTrasladoValor.Eliminado = false;
                    BalanceFinancieroTrasladoValor.RegistroCompleto = BalanceFinancieroTrasladoValor.ValorTraslado != null;

                    _context.BalanceFinancieroTrasladoValor.Add(BalanceFinancieroTrasladoValor);
                }
                else
                {
                    _context.Set<BalanceFinancieroTrasladoValor>()
                            .Where(r => r.BalanceFinancieroTrasladoValorId == BalanceFinancieroTrasladoValor.BalanceFinancieroTrasladoValorId)
                            .Update(r => new BalanceFinancieroTrasladoValor
                            {
                                EsPreconstruccion = BalanceFinancieroTrasladoValor.EsPreconstruccion,
                                UsuarioModificacion = pAuthor,
                                FechaModificacion = DateTime.Now,
                                ValorTraslado = BalanceFinancieroTrasladoValor.ValorTraslado,
                                RegistroCompleto = BalanceFinancieroTrasladoValor.ValorTraslado != null
                            });
                }
            }
        }
        #endregion

        private async Task<bool> RegistroCompletoBalanceFinanciero(BalanceFinanciero balanceFinanciero)
        {
            BalanceFinanciero balanceFinancieroOld = await _context.BalanceFinanciero.Where(r => r.BalanceFinancieroId == balanceFinanciero.BalanceFinancieroId).FirstOrDefaultAsync();
            bool state = false;
            if (balanceFinanciero != null)
            {
                if (balanceFinanciero.RequiereTransladoRecursos == false)
                    return true;
                else
                    return balanceFinanciero.RegistroCompleto;
            }
            return state;
        }

        public async Task<Respuesta> ApproveBalance(int pProyectoId, string pUsuario)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Aprobar_Balance_Financiero, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                _context.Set<BalanceFinanciero>()
                        .Where(b => b.ProyectoId == pProyectoId)
                        .Update(b => new BalanceFinanciero
                        {
                            FechaAprobacion = DateTime.Now,
                            EstadoBalanceCodigo = ConstanCodigoEstadoBalanceFinanciero.Con_balance_aprobado,
                            UsuarioModificacion = pUsuario,
                            FechaModificacion = DateTime.Now,
                        });

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.OperacionExitosa, idAccion, pUsuario, "APROBAR BALANCE FINANCIERO")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.Error, idAccion, pUsuario, ex.InnerException.ToString())
                };
            }
        }

        public async Task<dynamic> GetVerDetalleOrdenGiro(int pSolicitudPagoId)
        {
            SolicitudPago solicitudPago = _context.SolicitudPago
                .Where(r => r.SolicitudPagoId == pSolicitudPagoId)
                .Include(r => r.Contrato)
                .ThenInclude(r => r.Contratacion).FirstOrDefault();

            List<dynamic> OrdenGiro = new List<dynamic>();

            return OrdenGiro;
        }

        private string ReplaceTemplate(int pBalanceFinancieroTrasladoId)
        {
            BalanceFinancieroTraslado balanceFinancieroTraslado = _context.BalanceFinancieroTraslado
                .Where(r => r.BalanceFinancieroTrasladoId == pBalanceFinancieroTrasladoId)
                .Include(r => r.OrdenGiro)
                .Include(r => r.BalanceFinanciero)
                .ThenInclude(r => r.Proyecto)
                .FirstOrDefault();

            return string.Empty;
        }
        #endregion

        #region Get

        public async Task<OrdenGiro> SeeDetailOdg(int pOrdenGiroId)
        {
            OrdenGiro ordenGiro = await
                _context.OrdenGiro
                .Where(r => r.OrdenGiroId == pOrdenGiroId)
                .Include(r => r.SolicitudPago)
                .ThenInclude(c => c.Contrato)
                .ThenInclude(c => c.Contratacion)
                .ThenInclude(c => c.Contratista)
                .FirstOrDefaultAsync();

            ordenGiro.TablaFacturado = GetTablaFacturado(ordenGiro.OrdenGiroId);
            ordenGiro.TablaDescuento = GetTablaDescuento(ordenGiro.OrdenGiroId);
            ordenGiro.TablaOtroDescuento = GetTablaOtroDescuento(ordenGiro.OrdenGiroId);
            return ordenGiro;
        }

        private dynamic GetTablaOtroDescuento(int ordenGiroId)
        {
            List<VTablaOdgOtroDescuento> List = _context.VTablaOdgOtroDescuento.Where(r => r.OrdenGiroId == ordenGiroId).ToList();

            var ListConceptoPago = List.GroupBy(drp => drp.ConceptoPago)
                                       .Select(d =>
                                                   d.OrderBy(p => p.ConceptoPago)
                                                    .FirstOrDefault())
                                       .ToList();

            List<dynamic> ListTablaDescuento = new List<dynamic>();

            foreach (var ConceptoPago in ListConceptoPago)
            {
                var ListAportante = List.Where(c => c.ConceptoPago == ConceptoPago.ConceptoPago)
                                        .GroupBy(drp => drp.AportanteId)
                                        .Select(d =>
                                                  d.OrderBy(p => p.AportanteId)
                                                   .FirstOrDefault())
                                        .ToList();

                List<dynamic> ListDyAportante = new List<dynamic>();

                foreach (var Aportante in ListAportante)
                {
                    var ListDescuento = List.Where(c => c.ConceptoPago == ConceptoPago.ConceptoPago && c.AportanteId == Aportante.AportanteId)
                                             .GroupBy(drp => drp.Descuento)
                                             .Select(d =>
                                                     d.OrderBy(p => p.Descuento)
                                                      .FirstOrDefault())
                                             .ToList();

                    List<dynamic> ListDyDescuento = new List<dynamic>();

                    foreach (var Descuento in ListDescuento)
                    {
                        ListDyDescuento.Add(new
                        {
                            Descuento.Descuento,
                            Descuento.ValorDescuento
                        });
                    }

                    ListDyAportante.Add(new
                    {
                        Aportante = _budgetAvailabilityService.getNombreAportante(_context.CofinanciacionAportante.Find(Aportante.AportanteId)),
                        ListDyDescuento
                    });
                }

                ListTablaDescuento.Add(new
                {
                    ConceptoPago.ConceptoPago,
                    ListDyAportante
                });
            }

            return ListTablaDescuento;
        }

        private dynamic GetTablaDescuento(int ordenGiroId)
        {
            List<VTablaOdgDescuento> List = _context.VTablaOdgDescuento.Where(r => r.OrdenGiroId == ordenGiroId).ToList();

            var ListConceptoPago = List.GroupBy(drp => drp.ConceptoPago)
                                       .Select(d =>
                                                   d.OrderBy(p => p.ConceptoPago)
                                                    .FirstOrDefault())
                                       .ToList();

            List<dynamic> ListTablaDescuento = new List<dynamic>();

            foreach (var ConceptoPago in ListConceptoPago)
            {
                var ListAportante = List.Where(r => r.ConceptoPago == ConceptoPago.ConceptoPago)
                                        .GroupBy(drp => drp.AportanteId)
                                        .Select(d =>
                                                    d.OrderBy(p => p.AportanteId)
                                                     .FirstOrDefault())
                                        .ToList();

                List<dynamic> ListDyAportante = new List<dynamic>();

                foreach (var Aportante in ListAportante)
                {
                    ListDyAportante.Add(new
                    {
                        Aportante = _budgetAvailabilityService.getNombreAportante(_context.CofinanciacionAportante.Find(Aportante.AportanteId)),
                        AnsAplicado = List.Where(c => c.DescuentoCodigo == ConstanCodigoTipoDescuentoOrdenGiro.ANS && c.ConceptoPago == ConceptoPago.ConceptoPago && c.AportanteId == Aportante.AportanteId).Sum(c => c.ValorDescuento),
                        ReteGarantia = List.Where(c => c.DescuentoCodigo == ConstanCodigoTipoDescuentoOrdenGiro.Retegarantia && c.ConceptoPago == ConceptoPago.ConceptoPago && c.AportanteId == Aportante.AportanteId).Sum(c => c.ValorDescuento),
                        OtrosDescuentos = List.Where(c => c.DescuentoCodigo != ConstanCodigoTipoDescuentoOrdenGiro.Retegarantia && c.DescuentoCodigo != ConstanCodigoTipoDescuentoOrdenGiro.ANS && c.ConceptoPago == ConceptoPago.ConceptoPago && c.AportanteId == Aportante.AportanteId).Sum(c => c.ValorDescuento),
                        ValorTotalDescuento = List.Where(c => c.ConceptoPago == ConceptoPago.ConceptoPago && c.AportanteId == Aportante.AportanteId).Sum(c => c.ValorDescuento)
                    });
                }

                ListTablaDescuento.Add(new
                {
                    ConceptoPago.ConceptoPago,
                    ListDyAportante
                });
            }

            return ListTablaDescuento;
        }

        private dynamic GetTablaFacturado(int ordenGiroId)
        {
            List<VTablaOdgFacturado> List = _context.VTablaOdgFacturado.Where(r => r.OrdenGiroId == ordenGiroId).ToList();

            var ListUso = List.GroupBy(drp => drp.Uso)
                                    .Select(d =>
                                                 d.OrderBy(p => p.Uso)
                                                  .FirstOrDefault())
                                    .ToList();

            List<dynamic> ListTablaFacturado = new List<dynamic>();

            foreach (var Uso in ListUso)
            {
                var ListTipoPago = List.Where(r => r.Uso == Uso.Uso)
                                  .GroupBy(drp => drp.TipoPago)
                                  .Select(d => d.OrderBy(p => p.TipoPago).FirstOrDefault())
                                  .ToList();

                List<dynamic> ListDyTipoPago = new List<dynamic>();

                foreach (var TipoPago in ListTipoPago)
                {
                    var ListConceptoPago = List.Where(r => r.Uso == Uso.Uso && r.TipoPago == TipoPago.TipoPago)
                                 .GroupBy(drp => drp.ConceptoPagoCodigo)
                                 .Select(d => d.OrderBy(p => p.ConceptoPagoCodigo).FirstOrDefault())
                                 .ToList();

                    List<dynamic> ListDyConceptoPago = new List<dynamic>();

                    foreach (var ConceptoPago in ListConceptoPago)
                    {
                        var ListAportante = List.Where(r => r.Uso == Uso.Uso && r.TipoPago == TipoPago.TipoPago && r.ConceptoPagoCodigo == ConceptoPago.ConceptoPagoCodigo)
                                 .GroupBy(drp => drp.AportanteId)
                                 .Select(d => d.OrderBy(p => p.AportanteId).FirstOrDefault())
                                 .ToList();

                        List<dynamic> ListDyAportante = new List<dynamic>();

                        foreach (var Aportante in ListAportante)
                        {
                            ListDyAportante.Add(new
                            {
                                ValorFacturado = Aportante.ValorFacturado,
                                Aportante = _budgetAvailabilityService.getNombreAportante(_context.CofinanciacionAportante.Find(Aportante.AportanteId))
                            });
                        }
                        ListDyConceptoPago.Add(new
                        {
                            ConceptoPago.ConceptoPago,
                            ValorConceptoPago = ListAportante.Sum(v => v.ValorFacturado),
                            ListDyAportante
                        });
                    }
                    ListDyTipoPago.Add(new
                    {
                        TipoPago.TipoPago,
                        ListDyConceptoPago
                    });
                }
                ListTablaFacturado.Add(new
                {
                    Uso.Uso,
                    ListDyTipoPago
                });
            }
            return ListTablaFacturado;
        }

        public async Task<OrdenGiro> GetOrdenGiroById(int pOrdenGiroId)
        {
            OrdenGiro OrdenGiro = await _context.OrdenGiro
                                .Where(o => o.OrdenGiroId == pOrdenGiroId)
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
                                .AsNoTracking()
                                .FirstOrDefaultAsync();

            foreach (var OrdenGiroDetalle in OrdenGiro.OrdenGiroDetalle)
            {
                if (OrdenGiroDetalle.OrdenGiroDetalleDescuentoTecnica.Count > 0)
                    OrdenGiroDetalle.OrdenGiroDetalleDescuentoTecnica = OrdenGiroDetalle.OrdenGiroDetalleDescuentoTecnica.Where(r => r.Eliminado != true).ToList();

                foreach (var OrdenGiroDetalleDescuentoTecnica in OrdenGiroDetalle.OrdenGiroDetalleDescuentoTecnica)
                {
                    if (OrdenGiroDetalleDescuentoTecnica.OrdenGiroDetalleDescuentoTecnicaAportante.Count() > 0)
                        OrdenGiroDetalleDescuentoTecnica.OrdenGiroDetalleDescuentoTecnicaAportante = OrdenGiroDetalleDescuentoTecnica.OrdenGiroDetalleDescuentoTecnicaAportante.Where(r => r.Eliminado != true).ToList();
                }

                foreach (var OrdenGiroDetalleTerceroCausacion in OrdenGiroDetalle.OrdenGiroDetalleTerceroCausacion)
                {
                    if (OrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionAportante.Count() > 0)
                        OrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionAportante = OrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionAportante.Where(r => r.Eliminado != true).ToList();

                    if (OrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionDescuento.Count() > 0)
                        OrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionDescuento = OrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionDescuento.Where(r => r.Eliminado != true).ToList();
                }
            }

            return OrdenGiro;
        }

        //private async Task<dynamic> GetTablaOtrosDescuentos(OrdenGiro pOrdenGiro)
        //{
        //    List<dynamic> dynamics = new List<dynamic>();
        //    List<Dominio> ListDominio = await _commonService.GetListDominioByIdTipoDominio((int)EnumeratorTipoDominio.Tipo_Descuento);

        //    foreach (var OrdenGiroDetalle in pOrdenGiro.OrdenGiroDetalle)
        //    {
        //        foreach (var OrdenGiroDetalleDescuentoTecnica in OrdenGiroDetalle.OrdenGiroDetalleDescuentoTecnica)
        //        {
        //            dynamics.Add(new
        //            { 

        //                Tipo
        //                Descuento = FormattedAmount(OrdenGiroDetalleDescuentoTecnica.OrdenGiroDetalleDescuentoTecnicaAportante.FirstOrDefault().ValorDescuento),

        //            });
        //        }
        //    }
        //}


        public async Task<Respuesta> ChangeStatudBalanceFinanciero(BalanceFinanciero pBalanceFinanciero)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Balance_Financiero, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                _context.Set<BalanceFinanciero>()
                        .Where(b => b.BalanceFinancieroId == pBalanceFinanciero.BalanceFinancieroId)
                        .Update(b => new BalanceFinanciero
                        {
                            EstadoBalanceCodigo = pBalanceFinanciero.EstadoBalanceCodigo,
                            UsuarioModificacion = pBalanceFinanciero.UsuarioModificacion,
                            FechaModificacion = DateTime.Now
                        });

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_balance_financiero_traslados_de_recursos, GeneralCodes.OperacionExitosa, idAccion, pBalanceFinanciero.UsuarioCreacion, "CAMBIAR ESTADO BALANCE FINANCIERO")
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_balance_financiero_traslados_de_recursos, GeneralCodes.Error, idAccion, pBalanceFinanciero.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        public async Task<Respuesta> ChangeStatudBalanceFinancieroTraslado(BalanceFinancieroTraslado pBalanceFinancieroTraslado)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Traslado_Balance_Financiero, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                _context.Set<BalanceFinancieroTraslado>()
                        .Where(b => b.BalanceFinancieroTrasladoId == pBalanceFinancieroTraslado.BalanceFinancieroTrasladoId)
                        .Update(b => new BalanceFinancieroTraslado
                        {
                            EstadoCodigo = pBalanceFinancieroTraslado.EstadoCodigo,
                            UsuarioModificacion = pBalanceFinancieroTraslado.UsuarioModificacion,
                            FechaModificacion = DateTime.Now
                        });

                switch (pBalanceFinancieroTraslado.EstadoCodigo)
                {
                    case ConstanCodigoEstadoBalanceFinancieroTraslado.Traslado_Aprobado:
                        GetRestaurarFuentesPorAportante(pBalanceFinancieroTraslado, false);
                        break;

                    case ConstanCodigoEstadoBalanceFinancieroTraslado.Anulado:
                        GetRemoveFuentesXBalanceFinancieroTraslado(pBalanceFinancieroTraslado);
                        break;

                    case ConstanCodigoEstadoBalanceFinancieroTraslado.Notificado_a_fiduciaria:
                        GetNotificarFiduciariaxTraslado(pBalanceFinancieroTraslado);
                        break;
                }

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_balance_financiero_traslados_de_recursos, GeneralCodes.OperacionExitosa, idAccion, pBalanceFinancieroTraslado.UsuarioCreacion, "CAMBIAR ESTADO BALANCE FINANCIERO TRASLADO")
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_balance_financiero_traslados_de_recursos, GeneralCodes.Error, idAccion, pBalanceFinancieroTraslado.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        private async Task<bool> GetNotificarFiduciariaxTraslado(BalanceFinancieroTraslado pBalanceFinancieroTraslado)
        {
            pBalanceFinancieroTraslado = _context.BalanceFinancieroTraslado
                .Where(b => b.BalanceFinancieroTrasladoId == pBalanceFinancieroTraslado.BalanceFinancieroTrasladoId)
                .AsNoTracking()
                .FirstOrDefault();

            Template template = _context.Template.Find((int)(enumeratorTemplate.EnviarBalanceFinanciero));

            string strContenido = ReplaceVariables(template.Contenido, pBalanceFinancieroTraslado);

            List<EnumeratorPerfil> perfilsEnviarCorreo =
                            new List<EnumeratorPerfil>
                                                      {
                                                        EnumeratorPerfil.Fiduciaria,
                                                        EnumeratorPerfil.Fiduciaria_Equipo_Financiero
                                                      };
            return _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto);
        }

        private string ReplaceVariables(string template, BalanceFinancieroTraslado pBalanceFinancieroTraslado)
        {
            BalanceFinanciero balanceFinanciero =
                           _context.BalanceFinanciero
                           .Where(b => b.BalanceFinancieroId == pBalanceFinancieroTraslado.BalanceFinancieroId)
                           .Include(b => b.BalanceFinancieroTraslado)
                           .ThenInclude(r => r.OrdenGiro)
                           .ThenInclude(r => r.SolicitudPago)
                           .ThenInclude(r => r.Contrato)
                           .Include(b => b.Proyecto).FirstOrDefault();

            string LlaveMen = balanceFinanciero.Proyecto.LlaveMen;
            string NumContrato = balanceFinanciero.BalanceFinancieroTraslado.FirstOrDefault().OrdenGiro?.SolicitudPago?.FirstOrDefault()?.Contrato?.NumeroContrato ?? "";
            string FechaTraslado = ((DateTime)balanceFinanciero.BalanceFinancieroTraslado.Where(b => b.BalanceFinancieroTrasladoId == pBalanceFinancieroTraslado.BalanceFinancieroTrasladoId).FirstOrDefault().FechaCreacion).ToString("dd-MM-yyyy");
            string NumeroTraslado = balanceFinanciero?.BalanceFinancieroTraslado?.Where(b => b.BalanceFinancieroTrasladoId == pBalanceFinancieroTraslado.BalanceFinancieroTrasladoId)?.FirstOrDefault()?.NumeroTraslado ?? "";
            string NumeroOrdenGiro = balanceFinanciero?.BalanceFinancieroTraslado?.Where(b => b.BalanceFinancieroTrasladoId == pBalanceFinancieroTraslado.BalanceFinancieroTrasladoId).FirstOrDefault()?.OrdenGiro?.NumeroSolicitud ?? "";
            string ValorOrdenGiro = String.Format("{0:n0}", (balanceFinanciero?.BalanceFinancieroTraslado?.Where(b => b.BalanceFinancieroTrasladoId == pBalanceFinancieroTraslado.BalanceFinancieroTrasladoId)?.FirstOrDefault()?.ValorTraslado) ?? 0);
            template = template
                     .Replace("[LLAVE_MEN]", LlaveMen)
                     .Replace("[NUMERO_CONTRATO]", NumContrato)
                     .Replace("[FECHA_TRASLADO]", FechaTraslado)
                     .Replace("[NUMERO_TRASLADO]", NumeroTraslado)
                     .Replace("[NUMERO_ORDEN_GIRO]", NumeroOrdenGiro)
                     .Replace("[VALOR_TRASLADO]", ValorOrdenGiro);

            return template;
        }

        private void GetRemoveFuentesXBalanceFinancieroTraslado(BalanceFinancieroTraslado pBalanceFinancieroTraslado)
        {
            GestionFuenteFinanciacion gestionFuenteFinanciacion =
                _context.GestionFuenteFinanciacion
                .Where(r => r.BalanceFinancieroTrasladoValorId == pBalanceFinancieroTraslado.BalanceFinancieroTrasladoId)
                .FirstOrDefault();

            GetRestaurarFuentesPorAportante(pBalanceFinancieroTraslado, true);
        }

        private void GetRestaurarFuentesPorAportante(BalanceFinancieroTraslado pBalanceFinancieroTraslado, bool pEliminar)
        {
            pBalanceFinancieroTraslado.BalanceFinancieroTrasladoValor =
                _context.BalanceFinancieroTrasladoValor
                                                       .Where(r => r.BalanceFinancieroTrasladoId == pBalanceFinancieroTraslado.BalanceFinancieroTrasladoId)
                                                       .Include(r => r.OrdenGiroDetalleTerceroCausacionAportante)
                                                       .Include(r => r.OrdenGiroDetalleTerceroCausacionDescuento)
                                                       .Include(r => r.OrdenGiroDetalleDescuentoTecnicaAportante)
                                                       .Include(r => r.OrdenGiroDetalleDescuentoTecnica)
                                                       .Include(r => r.BalanceFinancieroTraslado)
                                                       .ThenInclude(r => r.OrdenGiro)
                                                       .ThenInclude(r => r.SolicitudPago)
                                                       .ThenInclude(r => r.Contrato)
                                                       .ThenInclude(r => r.Contratacion)
                                                       .ThenInclude(r => r.DisponibilidadPresupuestal)
                                                       .ToList();

            //Actualizar ODG
            BalanceFinancieroTraslado balanceFinancieroTrasladoOLd =
                _context.BalanceFinancieroTraslado
                .Where(b => b.BalanceFinancieroTrasladoId == pBalanceFinancieroTraslado.BalanceFinancieroTrasladoId)
                .AsNoTracking()
                .FirstOrDefault();


            UpdateOdgTraslado((int)balanceFinancieroTrasladoOLd.OrdenGiroId, !pEliminar);

            foreach (var BalanceFinancieroTrasladoValor in pBalanceFinancieroTraslado.BalanceFinancieroTrasladoValor)
            {
                int FuenteFinanciacionId = 0;
                switch (BalanceFinancieroTrasladoValor.TipoTrasladoCodigo)
                {
                    case ConstantCodigoTipoTrasladoCodigo.Aportante_Tercero_Causacion:
                        FuenteFinanciacionId = BalanceFinancieroTrasladoValor?.OrdenGiroDetalleTerceroCausacionAportante?.FuenteFinanciacionId ?? 0;
                        break;

                    case ConstantCodigoTipoTrasladoCodigo.Descuento_Tercero_Causacion:
                        FuenteFinanciacionId = BalanceFinancieroTrasladoValor?.OrdenGiroDetalleTerceroCausacionDescuento?.FuenteFinanciacionId ?? 0;
                        break;

                    case ConstantCodigoTipoTrasladoCodigo.Descuento_Direccion_Tecnica:
                        FuenteFinanciacionId = BalanceFinancieroTrasladoValor?.OrdenGiroDetalleDescuentoTecnicaAportante?.FuenteFinanciacionId ?? 0;
                        break;
                }

                GetTrasladarRecursosxAportantexFuente(
                    pEliminar,
                   0,
                   FuenteFinanciacionId,
                   BalanceFinancieroTrasladoValor.BalanceFinancieroTrasladoValorId,
                   pBalanceFinancieroTraslado.UsuarioCreacion,
                   BalanceFinancieroTrasladoValor?.BalanceFinancieroTraslado?.OrdenGiro?.SolicitudPago?.FirstOrDefault()?.Contrato?.Contratacion?.DisponibilidadPresupuestal?.FirstOrDefault()?.DisponibilidadPresupuestalId ?? 0,
                   BalanceFinancieroTrasladoValor.ValorTraslado ?? 0
                   );
            }
        }

        private void UpdateOdgTraslado(int OrdenGiroId, bool TieneTraslado)
        {
            //Actualizar ODG Con traslado
            _context.Set<OrdenGiro>()
                     .Where(o => o.OrdenGiroId == OrdenGiroId)
                     .Update(o => new OrdenGiro
                     {
                         TieneTraslado = TieneTraslado
                     });
        }

        private bool GetTrasladarRecursosxAportantexFuente(
            bool Eliminar,
            int pAportanteId,
            int pFuenteFinanciacionId,
            int pBalanceFinancieroTrasladoValorId,
            string pAuthor,
            int pDisponibilidadPresupuestalId,
            decimal pValorTraslado)
        {
            try
            {
                GestionFuenteFinanciacion gestionFuenteFinanciacion =
                    _context.GestionFuenteFinanciacion
                    .Where(r => r.FuenteFinanciacionId == pFuenteFinanciacionId)
                    .AsNoTracking()
                    .OrderByDescending(r => r.GestionFuenteFinanciacionId)
                    .FirstOrDefault();
                GestionFuenteFinanciacion gestionFuenteFinanciacionNew = new GestionFuenteFinanciacion();
                gestionFuenteFinanciacionNew = new GestionFuenteFinanciacion
                {
                    FechaCreacion = DateTime.Now,
                    UsuarioCreacion = pAuthor,
                    Eliminado = false,

                    FuenteFinanciacionId = pFuenteFinanciacionId,
                    SaldoActual = gestionFuenteFinanciacion.SaldoActual,
                    ValorSolicitado = gestionFuenteFinanciacion.ValorSolicitado,
                    NuevoSaldo = gestionFuenteFinanciacion.NuevoSaldo,
                    DisponibilidadPresupuestalId = pDisponibilidadPresupuestalId,

                    SaldoActualGenerado = gestionFuenteFinanciacion.NuevoSaldoGenerado,
                    ValorSolicitadoGenerado = pValorTraslado,
                    NuevoSaldoGenerado = gestionFuenteFinanciacion.NuevoSaldoGenerado + pValorTraslado,
                    BalanceFinancieroTrasladoValorId = pBalanceFinancieroTrasladoValorId
                };

                if (Eliminar)
                    gestionFuenteFinanciacionNew.NuevoSaldoGenerado = gestionFuenteFinanciacionNew.NuevoSaldoGenerado - pValorTraslado;

                _context.GestionFuenteFinanciacion.Add(gestionFuenteFinanciacionNew);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {

            }
            return false;
        }

        public async Task<List<VProyectosBalance>> GridBalance()
        {
            return await _context.VProyectosBalance
                                 .OrderByDescending(r => r.FechaTerminacionProyecto)
                                 .ToListAsync();
        }

        public async Task<dynamic> GetDataByProyectoId(int pProyectoId)
        {
            String numeroContratoObra = string.Empty, numeroContratoInterventoria = string.Empty;

            List<dynamic> ProyectoAjustado = new List<dynamic>();
            List<Dominio> TipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();
            List<InstitucionEducativaSede> ListInstitucionEducativaSede = _context.InstitucionEducativaSede.ToList();
            List<Dominio> TipoObraIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_por_contratar).ToList();

            List<Localizacion> ListLocalizacion = _context.Localizacion.ToList();
            Proyecto proyecto = await _context.Proyecto.Where(r => r.ProyectoId == pProyectoId)
                                                       .Include(r => r.ContratacionProyecto)
                                                           .ThenInclude(r => r.Contratacion)
                                                                .ThenInclude(r => r.Contrato)
                                                                     .ThenInclude(r => r.SolicitudPago)
                                                                         .ThenInclude(r => r.OrdenGiro)
                                                       .Include(r => r.InstitucionEducativa)
                                                       .Include(r => r.BalanceFinanciero)
                                                          .ThenInclude(r => r.BalanceFinancieroTraslado)
                                                              .ThenInclude(r => r.BalanceFinancieroTrasladoValor)
                                                       .FirstOrDefaultAsync();

            InstitucionEducativaSede Sede = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == proyecto.SedeId).FirstOrDefault();
            Localizacion Municipio = ListLocalizacion.Where(r => r.LocalizacionId == proyecto.LocalizacionIdMunicipio).FirstOrDefault();
            proyecto.MunicipioObj = Municipio;
            proyecto.DepartamentoObj = ListLocalizacion.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
            proyecto.tipoIntervencionString = TipoIntervencion.Where(r => r.Codigo == proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
            proyecto.Sede = Sede;
            List<ContratacionProyecto> ListContratacion = await _context.ContratacionProyecto
                                                        .Where(r => r.ProyectoId == pProyectoId)
                                                        .Include(r => r.Contratacion).ThenInclude(r => r.Contratista)
                                                        .Include(r => r.Contratacion).ThenInclude(r => r.Contrato)
                                                        .ToListAsync();

            ListContratacion.FirstOrDefault().Contratacion.TipoContratacionCodigo = TipoObraIntervencion.Where(r => r.Codigo == ListContratacion.FirstOrDefault().Contratacion.TipoSolicitudCodigo).Select(r => r.Nombre).FirstOrDefault();
            bool cumpleCondicionesTai = false;

            foreach (var item in ListContratacion)
            {
                Contrato contrato = await _context.Contrato.Where(r => r.ContratacionId == item.ContratacionId).FirstOrDefaultAsync();
                Contratacion contratacion = await _context.Contratacion.Where(r => r.ContratacionId == item.ContratacionId).FirstOrDefaultAsync();

                if (contrato != null)
                {
                    if(!cumpleCondicionesTai)
                        cumpleCondicionesTai = _contractualControversy.ValidarCumpleTaiContratista(contrato.ContratoId, false);

                    if (contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Obra)
                        numeroContratoObra = contrato.NumeroContrato ?? string.Empty;

                    else if (contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Interventoria)
                        numeroContratoInterventoria = contrato.NumeroContrato ?? string.Empty;
                }
            }

            bool TieneOrdenGiro = proyecto?.ContratacionProyecto?.FirstOrDefault()?.Contratacion?.Contrato?.FirstOrDefault()?.SolicitudPago?.Any(r => r.OrdenGiro?.OrdenGiroId > 0) ?? false;

            ProyectoAjustado.Add(new
            {
                TieneOrdenGiro = TieneOrdenGiro,
                llaveMen = proyecto.LlaveMen,
                tipoIntervencion = proyecto.tipoIntervencionString,
                institucionEducativa = proyecto.InstitucionEducativa.Nombre,
                sedeEducativa = proyecto.Sede.Nombre,
                departamento = proyecto.DepartamentoObj.Descripcion,
                municipio = proyecto.MunicipioObj.Descripcion,
                numeroContratoObra = numeroContratoObra,
                numeroContratoInterventoria = numeroContratoInterventoria,
                proyecto.BalanceFinanciero,
                CumpleCondicionesTai = cumpleCondicionesTai
            });

            return ProyectoAjustado;
        }

        public async Task<dynamic> GetOrdenGiroBy(string pTipoSolicitudCodigo, string pNumeroOrdenGiro, string pLLaveMen)
        {
            if (string.IsNullOrEmpty(pNumeroOrdenGiro))
            {
                List<VOrdenGiroXproyecto> ListOrdenGiroId =
                       _context.VOrdenGiroXproyecto
                       .Where(r => r.LlaveMen == pLLaveMen
                           && r.TipoSolicitudCodigo == pTipoSolicitudCodigo)
                       .ToList();

                List<VOrdenGiroXproyecto> vOrdenGiroXproyectosNoRepetidos = new List<VOrdenGiroXproyecto>();
                List<VOrdenGiro> VOrdenGiro = new List<VOrdenGiro>();

                List<VOrdenGiro> ListOrdenGiro = await _context.VOrdenGiro.ToListAsync();

                foreach (var item in ListOrdenGiroId)
                {

                    if (!vOrdenGiroXproyectosNoRepetidos.Any(r => r.OrdenGiroId == item.OrdenGiroId))
                    {
                        vOrdenGiroXproyectosNoRepetidos.Add(item);
                        VOrdenGiro vOrdenGiro = (ListOrdenGiro.Where(o => o.OrdenGiroId == item.OrdenGiroId).FirstOrDefault());

                        VOrdenGiro.Add(vOrdenGiro);
                    }
                }
                return (VOrdenGiro.Select(v => new
                {
                    v.Modalidad,
                    v.FechaPagoFiduciaria,
                    v.NumeroContrato,
                    v.FechaAprobacionFinanciera,
                    v.TipoSolicitud,
                    v.NumeroSolicitudOrdenGiro,
                    v.OrdenGiroId,
                    v.SolicitudPagoId
                }).ToList());
            }
            else
            {
                List<VOrdenGiroXproyecto> ListOrdenGiroId =
                          _context.VOrdenGiroXproyecto
                          .Where(r => r.LlaveMen == pLLaveMen
                              && r.NumeroOrdenGiro == pNumeroOrdenGiro)
                          .ToList();

                List<VOrdenGiroXproyecto> vOrdenGiroXproyectosNoRepetidos = new List<VOrdenGiroXproyecto>();
                List<VOrdenGiro> VOrdenGiro = new List<VOrdenGiro>();

                List<VOrdenGiro> ListOrdenGiro = await _context.VOrdenGiro.ToListAsync();

                foreach (var item in ListOrdenGiroId)
                {
                    if (!vOrdenGiroXproyectosNoRepetidos.Any(r => r.OrdenGiroId == item.OrdenGiroId))
                    {
                        vOrdenGiroXproyectosNoRepetidos.Add(item);
                        VOrdenGiro.Add(ListOrdenGiro.Where(o => o.OrdenGiroId == item.OrdenGiroId).FirstOrDefault());
                    }
                }
                return (VOrdenGiro.Select(v =>
                new
                {
                    v.Modalidad,
                    v.FechaPagoFiduciaria,
                    v.NumeroContrato,
                    v.FechaAprobacionFinanciera,
                    v.TipoSolicitud,
                    v.NumeroSolicitudOrdenGiro,
                    v.OrdenGiroId,
                    v.SolicitudPagoId
                }).ToList());
            }
        }

        public async Task<dynamic> GetOrdenGiroByNumeroOrdenGiro(string pTipoSolicitudCodigo, string pNumeroOrdenGiro, string pLLaveMen)
        {
            return (
               _context.OrdenGiro
                .Where(r => r.NumeroSolicitud == pNumeroOrdenGiro)
                .Include(r => r.SolicitudPago)
                .ThenInclude(r => r.Contrato)
                .ThenInclude(r => r.Contratacion).Select(v =>
             new
             {
                 FechaAprobacionFinanciera = v.SolicitudPago.FirstOrDefault().FechaRegistroCompletoValidacionFinanciera,
                 TipoSolicitud = v.SolicitudPago.FirstOrDefault().Contrato.Contratacion.TipoSolicitudCodigo,
                 NumeroSolicitudOrdenGiro = v.NumeroSolicitud,
                 v.OrdenGiroId,
                 v.SolicitudPagoId,
                 OrdenGiro = v,
                 SolicitudPago = v.SolicitudPago,
                 Contrato = v.SolicitudPago.FirstOrDefault().Contrato
             }));
        }

        public async Task<BalanceFinanciero> GetBalanceFinanciero(int pProyectoId)
        {
            BalanceFinanciero balanceFinanciero = await _context.BalanceFinanciero
                                                    .Where(r => r.ProyectoId == pProyectoId)
                                                    .Include(r => r.BalanceFinancieroTraslado)
                                                          .ThenInclude(r => r.BalanceFinancieroTrasladoValor)
                                                    .FirstOrDefaultAsync();

            if (balanceFinanciero != null)
            {
                foreach (var BalanceFinancieroTraslado in balanceFinanciero.BalanceFinancieroTraslado)
                {
                    OrdenGiro OrdenGiro = _context.OrdenGiro
                        .Where(o => o.OrdenGiroId == BalanceFinancieroTraslado.OrdenGiroId)
                        .Include(r => r.SolicitudPago).ThenInclude(c => c.Contrato).ThenInclude(r => r.Contratacion)
                        .FirstOrDefault();
                    BalanceFinancieroTraslado.SolicitudPagoId = OrdenGiro.SolicitudPago.FirstOrDefault().SolicitudPagoId;
                    BalanceFinancieroTraslado.NumeroContrato = OrdenGiro?.SolicitudPago?.FirstOrDefault()?.Contrato?.NumeroContrato;
                    BalanceFinancieroTraslado.NumeroOrdenGiro = OrdenGiro.NumeroSolicitud;
                    BalanceFinancieroTraslado.TablaDRP = GetDrpContrato(OrdenGiro.SolicitudPago.FirstOrDefault());
                }
            }
            return balanceFinanciero;
        }

        private List<TablaDRP> GetDrpContrato(SolicitudPago SolicitudPago)
        {
            String strTipoSolicitud = SolicitudPago.Contrato.Contratacion.TipoSolicitudCodigo;
            List<TablaDRP> ListTablaDrp = new List<TablaDRP>();

            decimal ValorFacturado = SolicitudPago?.OrdenGiro?.TieneTraslado == false ? SolicitudPago?.OrdenGiro?.ValorNetoGiro ?? 0 : SolicitudPago?.OrdenGiro?.ValorNetoGiroTraslado ?? 0;

            List<VRpsPorContratacion> vRpsPorContratacion =
                                                           _context.VRpsPorContratacion
                                                           .Where(c => c.ContratacionId == SolicitudPago.Contrato.ContratacionId)
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

        public async Task<List<dynamic>> GetContratoByProyectoId(int pProyectoId)
        {
            try
            {
                List<dynamic> ListContratos = new List<dynamic>();

                List<ContratacionProyecto> ListContratacion = await _context.ContratacionProyecto
                                            .Where(r => r.ProyectoId == pProyectoId)
                                            .Include(r => r.Contratacion).ThenInclude(r => r.Contratista)
                                            .Include(r => r.Contratacion).ThenInclude(r => r.Contrato)
                                            .OrderBy(r => r.Contratacion.TipoSolicitudCodigo)
                                            .ToListAsync();

                foreach (var contratacionProyecto in ListContratacion)
                {
                    Contrato contrato = await _context.Contrato
                   .Where(c => c.ContratoId == contratacionProyecto.Contratacion.Contrato
                   .FirstOrDefault().ContratoId)
                   .Include(c => c.ContratoConstruccion)
                   .Include(c => c.ContratoPoliza)
                   .Include(c => c.Contratacion).ThenInclude(c => c.Contratista)
                   .Include(c => c.Contratacion).ThenInclude(cp => cp.DisponibilidadPresupuestal)
                   .Include(r => r.SolicitudPago).ThenInclude(r => r.SolicitudPagoCargarFormaPago)
                   .Include(r => r.SolicitudPago).ThenInclude(r => r.OrdenGiro)
                   .Include(c => c.Contratacion).ThenInclude(c => c.ContratacionProyecto).ThenInclude(t => t.ContratacionProyectoAportante).ThenInclude(t => t.CofinanciacionAportante).ThenInclude(t => t.FuenteFinanciacion).ThenInclude(t => t.CuentaBancaria)
                   .Include(c => c.Contratacion).ThenInclude(c => c.ContratacionProyecto).ThenInclude(t => t.ContratacionProyectoAportante).ThenInclude(t => t.CofinanciacionAportante).ThenInclude(t => t.NombreAportante)
                   .Include(c => c.Contratacion).ThenInclude(c => c.ContratacionProyecto).ThenInclude(t => t.ContratacionProyectoAportante).ThenInclude(t => t.CofinanciacionAportante).ThenInclude(t => t.Municipio)
                   .Include(c => c.Contratacion).ThenInclude(c => c.ContratacionProyecto).ThenInclude(t => t.ContratacionProyectoAportante).ThenInclude(t => t.CofinanciacionAportante).ThenInclude(t => t.Departamento)
                   .Include(c => c.Contratacion).ThenInclude(c => c.ContratacionProyecto).ThenInclude(t => t.ContratacionProyectoAportante).ThenInclude(t => t.ComponenteAportante).ThenInclude(t => t.ComponenteUso)
                   .AsNoTracking()
                   .FirstOrDefaultAsync();

                    if (contrato.SolicitudPago.Count() > 0)
                        contrato.SolicitudPago = contrato.SolicitudPago.Where(s => s.Eliminado != true).ToList();

                    contrato.ValorFacturadoContrato =
                        _context.VValorFacturadoContrato
                        .Where(v => v.ContratoId == contratacionProyecto.Contratacion.Contrato.FirstOrDefault().ContratoId)
                        .ToList();

                    contrato.VContratoPagosRealizados =
                        _context.VContratoPagosRealizados
                           .Where(v => v.ContratoId == contratacionProyecto.Contratacion.Contrato.FirstOrDefault().ContratoId)
                           .ToList();

                    contrato.TablaDRP = _registerValidatePaymentRequierementsService.GetDrpContrato(contrato);


                    contrato.ValorPagadoContratista = contrato.SolicitudPago.Sum(r => r.ValorFacturado);

                    contrato.TablaRecursosComprometidos = GetTablaRecursosComprometidos(contrato.ContratacionId, pProyectoId);


                    ListContratos.Add(new
                    {
                        tablaOrdenGiroValorTotal = GetTablaOrdenGiroValorTotal(contrato.SolicitudPago),
                        contrato,
                        tipoSolicitudCodigo = contratacionProyecto.Contratacion.TipoSolicitudCodigo
                    });
                }

                return ListContratos;
            }
            catch (Exception ex)
            {
                return new List<dynamic>();
            }
        }

        private dynamic GetTablaRecursosComprometidos(int pContratacionId, int pProyectoId)
        {
            List<VDrpXcontratacionXproyectoXaportante> ListVDrpXcontratacionXproyectoXaportante =
                                                                                                _context.VDrpXcontratacionXproyectoXaportante
                                                                                                            .Where(r => r.ContratacionId == pContratacionId && r.ProyectoId == pProyectoId)
                                                                                                            .ToList();

            List<dynamic> Tabla = new List<dynamic>();

            foreach (var item in ListVDrpXcontratacionXproyectoXaportante)
            {
                Tabla.Add(new
                {
                    Aportante = _budgetAvailabilityService.getNombreAportante(_context.CofinanciacionAportante.Find(item.AportanteId)),
                    ValorAportante = item.ValorAporte,
                    ValorTotalAportantes = ListVDrpXcontratacionXproyectoXaportante.Sum(r => r.ValorAporte)
                });
            }

            return Tabla;
        }

        public async Task<TablaUsoFuenteAportante> GetTablaUsoFuenteAportanteXContratoId(int pContratoId, int pProyectoId)
        {
            SolicitudPago solicitudPago = _context.SolicitudPago.Where(r => r.ContratoId == pContratoId)
                .Include(r => r.Contrato)
                .FirstOrDefault();

            if (solicitudPago == null)
                return new TablaUsoFuenteAportante();

            solicitudPago.OrdenGiro = await _context.OrdenGiro
                    .Where(o => o.OrdenGiroId == solicitudPago.OrdenGiroId)
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
                    .AsNoTracking().FirstOrDefaultAsync();

            List<VFuentesUsoXcontratoIdXproyecto> ListaCompleta =
                                               _context.VFuentesUsoXcontratoIdXproyecto
                                               .Where(c => c.ContratoId == solicitudPago.ContratoId && c.ProyectoId == pProyectoId)
                                               .ToList();

            List<VFuentesUsoXcontratoIdXproyecto> ListVFuentesUsoXcontratoId = new List<VFuentesUsoXcontratoIdXproyecto>();

            Proyecto proyecto = _context.Proyecto.Find(pProyectoId);

            foreach (var item in ListaCompleta)
            {
                if (!ListVFuentesUsoXcontratoId.Any(r => r.TipoUso == item.TipoUso && r.FuenteFinanciacion == item.FuenteFinanciacion))
                {
                    ListVFuentesUsoXcontratoId.Add(item);
                }
            }

            TablaUsoFuenteAportante tabla = new TablaUsoFuenteAportante
            {
                Usos = ListVFuentesUsoXcontratoId
                           .ConvertAll(x => new Usos
                           {
                               NombreUso = x.NombreUso,
                               TipoUsoCodigo = x.TipoUso,
                               FuenteFinanciacion = x.FuenteFinanciacion,
                               FuenteFinanciacionId = x.FuenteFinanciacionId
                           }).ToList()
            };
            List<VAportanteFuenteUsoProyecto> ListVAportanteFuenteUso =
                                                                        _context.VAportanteFuenteUsoProyecto
                                                                              .Where(f => f.ContratoId == solicitudPago.ContratoId
                                                                                       && f.ProyectoId == pProyectoId)
                                                                              .ToList();

            List<Usos> ListUsos = new List<Usos>();

            foreach (var usos in tabla.Usos)
            {
                if (!ListUsos.Any(r => r.TipoUsoCodigo == usos.TipoUsoCodigo && r.FuenteFinanciacion == usos.FuenteFinanciacion))
                {
                    ListUsos.Add(usos);

                    List<VFuentesUsoXcontratoIdXproyecto> List2 =
                                                        ListaCompleta
                                                        .Where(r => r.NombreUso == usos.NombreUso
                                                        && r.FuenteFinanciacion == usos.FuenteFinanciacion
                                                        )
                                                        .ToList();

                    usos.Fuentes = List2
                                        .ConvertAll(x => new Fuentes
                                        {
                                            TipoUsoCodigo = usos.TipoUsoCodigo,
                                            FuenteFinanciacionId = x.FuenteFinanciacionId,
                                            NombreFuente = x.FuenteFinanciacion,
                                            NombreUso = usos.NombreUso
                                        });


                    foreach (var Fuentes in usos.Fuentes)
                    {

                        List<VAportanteFuenteUsoProyecto> ListVAportanteFuenteUso2 =
                                                                ListVAportanteFuenteUso
                                                                .Where(r => r.FuenteFinanciacionId == Fuentes.FuenteFinanciacionId)
                                                                .ToList();

                        foreach (var item in ListVAportanteFuenteUso2)
                        {
                            if (Fuentes.Aportante == null || !Fuentes.Aportante.Any(r => r.AportanteId == item.CofinanciacionAportanteId))
                            {

                                if (Fuentes.Aportante == null)
                                    Fuentes.Aportante = new List<Aportante>();


                                List<VAportanteFuenteUsoProyecto> ListVAportanteFuenteUso3 =
                                    ListVAportanteFuenteUso.Where(r => r.CofinanciacionAportanteId == item.CofinanciacionAportanteId).ToList();

                                Fuentes.Aportante.Add(new Aportante
                                {
                                    FuenteFinanciacionId = Fuentes.FuenteFinanciacionId,
                                    AportanteId = item.CofinanciacionAportanteId

                                });
                                foreach (var Aportante in Fuentes.Aportante)
                                {
                                    decimal ValorUso = ListVAportanteFuenteUso3
                                                                            .Where(r => r.Nombre == usos.NombreUso
                                                                                     && r.CofinanciacionAportanteId == Aportante.AportanteId)
                                                                            .Select(s => s.ValorUso)
                                                                            .FirstOrDefault() ?? 0;


                                    List<VPlantillaOrdenGiroUsos> VPlantillaOrdenGiroUsos = _context.VPlantillaOrdenGiroUsos
                                                                                             .Where(r => r.ContratoId == pContratoId
                                                                                                      && r.AportanteId == Aportante.AportanteId
                                                                                                      && r.LlaveMen == proyecto.LlaveMen
                                                                                                      && r.UsoCodigo == usos.TipoUsoCodigo
                                                                                                      && r.EstaAprobada == true
                                                                                              ).ToList();

                                    Aportante.NombreAportante = _budgetAvailabilityService.getNombreAportante(_context.CofinanciacionAportante.Find(Aportante.AportanteId));
                                    if (Aportante.ValorUso == null)
                                        Aportante.ValorUso = new List<ValorUso>();

                                    if (VPlantillaOrdenGiroUsos != null)
                                    {

                                        if (usos.NombreUso == "Diseño - Obras Complementarias")
                                        { 
                                            string String = "string".ToString();
                                        }
                                        decimal Descuento = (VPlantillaOrdenGiroUsos.Sum(r => r.ValorConcepto - r.DescuentoReteFuente - r.DescuentoOtros - r.DescuentoAns)) ?? 0;

                                        decimal ValorConcepto = VPlantillaOrdenGiroUsos.Sum(r => r.ValorConcepto) ?? 0;
                                        if (ValorConcepto == 0)
                                            ValorConcepto = ValorUso;

                                        Decimal Total = Math.Abs(ValorUso - Descuento);

                                        if (Total == 0)
                                            Total = ValorUso;
                                        Aportante.ValorUso.Add(new ValorUso
                                        {
                                            AportanteId = Aportante.AportanteId,
                                            Valor = String.Format("{0:n0}", ValorUso),
                                            ValorActual = String.Format("{0:n0}", Total)
                                        });
                                    }
                                    else
                                    {
                                        Aportante.ValorUso.Add(new ValorUso
                                        {
                                            AportanteId = Aportante.AportanteId,
                                            Valor = String.Format("{0:n0}", ValorUso),
                                            ValorActual = String.Format("{0:n0}", ValorUso)
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return tabla;
        }

        public async Task<TablaUsoFuenteAportante> GetTablaUsoFuenteAportanteXContratoId(int pContratoId)
        {
            SolicitudPago solicitudPago = _context.SolicitudPago.Where(r => r.ContratoId == pContratoId)
                .Include(r => r.Contrato)
                .FirstOrDefault();

            if (solicitudPago == null)
                return new TablaUsoFuenteAportante();

            solicitudPago.OrdenGiro = await _context.OrdenGiro
                    .Where(o => o.OrdenGiroId == solicitudPago.OrdenGiroId)
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
                    .AsNoTracking().FirstOrDefaultAsync();

            List<VFuentesUsoXcontratoId> ListaCompleta =
                                               _context.VFuentesUsoXcontratoId
                                               .Where(c => c.ContratoId == solicitudPago.ContratoId)
                                               .ToList();

            List<VFuentesUsoXcontratoId> ListVFuentesUsoXcontratoId = new List<VFuentesUsoXcontratoId>();

            foreach (var item in ListaCompleta)
            {
                if (!ListVFuentesUsoXcontratoId.Any(r => r.TipoUso == item.TipoUso && r.FuenteFinanciacion == item.FuenteFinanciacion))
                {
                    ListVFuentesUsoXcontratoId.Add(item);
                }
            }

            TablaUsoFuenteAportante tabla = new TablaUsoFuenteAportante
            {
                Usos = ListVFuentesUsoXcontratoId
                           .ConvertAll(x => new Usos
                           {
                               NombreUso = x.NombreUso,
                               TipoUsoCodigo = x.TipoUso,
                               FuenteFinanciacion = x.FuenteFinanciacion,
                               FuenteFinanciacionId = x.FuenteFinanciacionId
                           }).ToList()
            };
            List<VAportanteFuenteUso> ListVAportanteFuenteUso = _context.VAportanteFuenteUso
                  .Where(f => f.ContratoId == solicitudPago.ContratoId)
                  .ToList();

            List<Usos> ListUsos = new List<Usos>();

            foreach (var usos in tabla.Usos)
            {
                if (!ListUsos.Any(r => r.TipoUsoCodigo == usos.TipoUsoCodigo && r.FuenteFinanciacion == usos.FuenteFinanciacion))
                {
                    ListUsos.Add(usos);

                    List<VFuentesUsoXcontratoId> List2 =
                                                        ListaCompleta
                                                        .Where(r => r.NombreUso == usos.NombreUso
                                                        && r.FuenteFinanciacion == usos.FuenteFinanciacion
                                                        )
                                                        .ToList();

                    usos.Fuentes = List2
                            .ConvertAll(x => new Fuentes
                            {
                                TipoUsoCodigo = usos.TipoUsoCodigo,
                                FuenteFinanciacionId = x.FuenteFinanciacionId,
                                NombreFuente = x.FuenteFinanciacion,
                                NombreUso = usos.NombreUso
                            });


                    foreach (var Fuentes in usos.Fuentes)
                    {

                        List<VAportanteFuenteUso> ListVAportanteFuenteUso2 =
                            ListVAportanteFuenteUso
                            .Where(r => r.FuenteFinanciacionId == Fuentes.FuenteFinanciacionId).ToList();

                        foreach (var item in ListVAportanteFuenteUso2)
                        {
                            if (Fuentes.Aportante == null || !Fuentes.Aportante.Any(r => r.AportanteId == item.CofinanciacionAportanteId))
                            {

                                if (Fuentes.Aportante == null)
                                    Fuentes.Aportante = new List<Aportante>();


                                List<VAportanteFuenteUso> ListVAportanteFuenteUso3 =
                                    ListVAportanteFuenteUso.Where(r => r.CofinanciacionAportanteId == item.CofinanciacionAportanteId).ToList();

                                Fuentes.Aportante.Add(new Aportante
                                {
                                    FuenteFinanciacionId = Fuentes.FuenteFinanciacionId,
                                    AportanteId = item.CofinanciacionAportanteId

                                });
                                foreach (var Aportante in Fuentes.Aportante)
                                {
                                    decimal ValorUso = ListVAportanteFuenteUso3
                                        .Where(r => r.Nombre == usos.NombreUso
                                        && r.CofinanciacionAportanteId == Aportante.AportanteId
                                        ).Select(s => s.ValorUso).FirstOrDefault() ?? 0;

                                    decimal Descuento = _context.VOrdenGiroPagosXusoAportante.Where(v => v.AportanteId == Aportante.AportanteId && v.TipoUsoCodigo == usos.TipoUsoCodigo).Sum(v => v.ValorDescuento) ?? 0;

                                    Aportante.NombreAportante = _budgetAvailabilityService.getNombreAportante(_context.CofinanciacionAportante.Find(Aportante.AportanteId));

                                    if (Aportante.ValorUso == null)
                                        Aportante.ValorUso = new List<ValorUso>();

                                    Aportante.ValorUso.Add(new ValorUso
                                    {
                                        AportanteId = Aportante.AportanteId,
                                        Valor = String.Format("{0:n0}", ValorUso),
                                        ValorActual = String.Format("{0:n0}", (ValorUso - Descuento))
                                    });
                                }
                            }
                        }
                    }
                }
            }
            return tabla;
        }

        private object GetTablaOrdenGiroValorTotal(ICollection<SolicitudPago> pListSolicitudPago)
        {
            List<dynamic> TablaOrdenesGiro = new List<dynamic>();

            if (pListSolicitudPago.Count() > 0)
                pListSolicitudPago = pListSolicitudPago.Where(s => s.OrdenGiro?.RegistroCompletoTramitar == true).ToList();

            List<VDescuentosXordenGiro> ListvDescuentosXordenGiro = _context.VDescuentosXordenGiro.ToList();
            List<VDescuentoTecnicaXordenGiro> ListVDescuentoTecnicaXordenGiro = _context.VDescuentoTecnicaXordenGiro.ToList();

            foreach (var SolicitudPago in pListSolicitudPago)
            {
                List<VDescuentosXordenGiro> descuentosXordenGiro = ListvDescuentosXordenGiro.Where(r => r.OrdenGiroId == SolicitudPago.OrdenGiroId).ToList();
                List<VDescuentoTecnicaXordenGiro> vDescuentosXordenGiro = ListVDescuentoTecnicaXordenGiro.Where(r => r.OrdenGiroId == SolicitudPago.OrdenGiroId).ToList();

                string NombreContratista = _context.SolicitudPago
                     .Where(r => r.SolicitudPagoId == SolicitudPago.SolicitudPagoId)
                     .Include(r => r.Contrato)
                        .ThenInclude(r => r.Contratacion)
                            .ThenInclude(r => r.Contratista)
                     .AsNoTracking()
                     .FirstOrDefault().Contrato.Contratacion.Contratista.Nombre;


                Decimal DescuentosANS = 0;
                if (descuentosXordenGiro.Count(r => r.TipoDescuentoCodigo == ConstanCodigoTipoDescuentoOrdenGiro.ANS) > 0)
                    DescuentosANS = (descuentosXordenGiro.Where(r => r.TipoDescuentoCodigo == ConstanCodigoTipoDescuentoOrdenGiro.ANS).Sum(s => s.ValorDescuento));

                Decimal DescuentosReteGarantia = 0;
                if (descuentosXordenGiro.Count(r => r.TipoDescuentoCodigo == ConstanCodigoTipoDescuentoOrdenGiro.Retegarantia) > 0)
                    DescuentosReteGarantia = (descuentosXordenGiro.Where(r => r.TipoDescuentoCodigo == ConstanCodigoTipoDescuentoOrdenGiro.Retegarantia).Sum(s => s.ValorDescuento));

                Decimal ValorDescuentosTecnica = 0;
                if (descuentosXordenGiro.Count(r => r.TipoDescuentoCodigo != ConstanCodigoTipoDescuentoOrdenGiro.ANS && r.TipoDescuentoCodigo != ConstanCodigoTipoDescuentoOrdenGiro.Retegarantia) > 0)
                    ValorDescuentosTecnica = descuentosXordenGiro.Where(r => r.TipoDescuentoCodigo != ConstanCodigoTipoDescuentoOrdenGiro.ANS && r.TipoDescuentoCodigo != ConstanCodigoTipoDescuentoOrdenGiro.Retegarantia).Sum(c => c.ValorDescuento);

                decimal ValorDescuentosTecnica2 = 0;
                if (vDescuentosXordenGiro != null)
                {
                    ValorDescuentosTecnica += (decimal)vDescuentosXordenGiro.Sum(r => r.ValorDescuento);
                    ValorDescuentosTecnica2 = (decimal)vDescuentosXordenGiro.Sum(r => r.ValorDescuento);
                }


                //decimal ValorFacturado = (SolicitudPago?.OrdenGiro?.ValorNetoGiro + ValorDescuentosTecnica2 + descuentosXordenGiro.Sum(r => r.ValorDescuento)) ?? 0;
                decimal ValorFacturado = (SolicitudPago?.OrdenGiro?.ValorNetoGiro - ValorDescuentosTecnica2) ?? 0;

                List<VPlantillaOrdenGiro> vPlantillaOrdenGiros = _context.VPlantillaOrdenGiro.Where(r => r.OrdenGiroId == SolicitudPago.OrdenGiroId).ToList();
                TablaOrdenesGiro.Add(
                    new
                    {
                        NumeroOrdenGiro = SolicitudPago.OrdenGiro.NumeroSolicitud,
                        Contratista = NombreContratista,
                        Facturado = FormattedAmount(vPlantillaOrdenGiros.Sum(r => r.ValorConcepto)),
                        AnsAplicado = FormattedAmount(vPlantillaOrdenGiros.Sum(r => r.DescuentoAns)),
                        ReteGarantia = FormattedAmount(vPlantillaOrdenGiros.Sum(r => r.DescuentoReteFuente)),
                        OtrosDescuentos = FormattedAmount(vPlantillaOrdenGiros.Sum(r => r.DescuentoOtros)),
                        ApagarAntesImpuestos = FormattedAmount(vPlantillaOrdenGiros.Sum(r => r.ValorConcepto) - vPlantillaOrdenGiros.Sum(r => r.DescuentoAns) - vPlantillaOrdenGiros.Sum(r => r.DescuentoReteFuente) - vPlantillaOrdenGiros.Sum(r => r.DescuentoOtros)),
                        SolicitudId = SolicitudPago.SolicitudPagoId,
                        OrdenGiro = SolicitudPago.OrdenGiroId
                    });
            }
            return TablaOrdenesGiro;
        }

        public string FormattedAmount(decimal? pValue)
        {
            return string.Concat("$", String.Format("{0:n0}", pValue));
        }

        public async Task<dynamic> GetEjecucionFinancieraXProyectoId(int pProyectoId)
        {
            return new List<dynamic>
            {
              await  _context.VEjecucionPresupuestalXproyecto.Where(r => r.ProyectoId == pProyectoId).ToListAsync(),
              await  _context.VEjecucionFinancieraXproyecto.Where(r => r.ProyectoId == pProyectoId).ToListAsync()
            };
        }

        //private List<VEjecucionFinancieraXproyecto> GetEjecucionFinanciera(int pProyectoId)
        //{
        //    List<VEjecucionFinancieraXproyecto> list = _context.VEjecucionFinancieraXproyecto.Where(r => r.ProyectoId == pProyectoId).ToList();

        //    foreach (var item in list)
        //    {
        //        item.Descuento = _context.VPlantillaOrdenGiro.Where(r => r.ContratacionId == item.ContratacionId).Sum(r => r.DescuentoAns + r.DescuentoOtros + r.DescuentoReteFuente);
        //    }

        //    return list;
        //}
        #endregion

        #endregion

        #region Save ODG
        public async Task CreateEditOrdenGiroTraslado(OrdenGiro pOrdenGiro)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Orden_Giro, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                _context.Set<OrdenGiro>()
                        .Where(o => o.OrdenGiroId == pOrdenGiro.OrdenGiroId)
                        .Update(o => new OrdenGiro
                        {
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = pOrdenGiro.UsuarioCreacion
                        });

                if (pOrdenGiro?.OrdenGiroDetalle.Count() > 0)
                    CreateEditOrdenGiroDetalle(pOrdenGiro.OrdenGiroDetalle.FirstOrDefault(), pOrdenGiro.UsuarioCreacion);

            }
            catch (Exception e)
            {

            }
        }

        private void CreateEditOrdenGiroDetalle(OrdenGiroDetalle pOrdenGiroDetalle, string pUsuarioCreacion)
        {
            if (pOrdenGiroDetalle?.OrdenGiroDetalleDescuentoTecnica.Count() > 0)
                CreateEditOrdenGiroDetalleDescuentoTecnica(pOrdenGiroDetalle.OrdenGiroDetalleDescuentoTecnica.ToList(), pUsuarioCreacion);

            if (pOrdenGiroDetalle?.OrdenGiroDetalleTerceroCausacion.Count() > 0)
                CreateEditOrdenGiroDetalleTerceroCausacion(pOrdenGiroDetalle?.OrdenGiroDetalleTerceroCausacion.ToList(), pUsuarioCreacion);


        }
        private void CreateEditOrdenGiroDetalleDescuentoTecnica(List<OrdenGiroDetalleDescuentoTecnica> pListOrdenGiroDetalleDescuentoTecnica, string pUsuarioCreacion)
        {
            foreach (var pOrdenGiroDetalleDescuentoTecnica in pListOrdenGiroDetalleDescuentoTecnica)
            {
                CreateEditOrdenGiroDetalleDescuentoTecnicaAportante(pOrdenGiroDetalleDescuentoTecnica.OrdenGiroDetalleDescuentoTecnicaAportante, pUsuarioCreacion);
            }
        }

        private void CreateEditOrdenGiroDetalleDescuentoTecnicaAportante(ICollection<OrdenGiroDetalleDescuentoTecnicaAportante> pOrdenGiroDetalleDescuentoTecnicaAportanteList, string pUsuarioCreacion)
        {
            foreach (var pOrdenGiroDetalleDescuentoTecnicaAportante in pOrdenGiroDetalleDescuentoTecnicaAportanteList)
            {
                _context.Set<OrdenGiroDetalleDescuentoTecnicaAportante>()
                             .Where(o => o.OrdenGiroDetalleDescuentoTecnicaAportanteId == pOrdenGiroDetalleDescuentoTecnicaAportante.OrdenGiroDetalleDescuentoTecnicaAportanteId)
                             .Update(r => new OrdenGiroDetalleDescuentoTecnicaAportante()
                             {
                                 FechaModificacion = DateTime.Now,
                                 UsuarioModificacion = pUsuarioCreacion,

                                 ValorDescuento = pOrdenGiroDetalleDescuentoTecnicaAportante.ValorDescuento
                             });
            }
        }

        private void CreateEditOrdenGiroDetalleTerceroCausacion(List<OrdenGiroDetalleTerceroCausacion> pListOrdenGiroDetalleTerceroCausacion, string pUsuarioCreacion)
        {
            foreach (var pOrdenGiroDetalleTerceroCausacion in pListOrdenGiroDetalleTerceroCausacion)
            {
                _context.Set<OrdenGiroDetalleTerceroCausacion>()
                        .Where(o => o.OrdenGiroDetalleTerceroCausacionId == pOrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionId)
                        .Update(r => new OrdenGiroDetalleTerceroCausacion()
                        {
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = pUsuarioCreacion,

                            ValorFacturadoConcepto = pOrdenGiroDetalleTerceroCausacion.ValorFacturadoConcepto
                        });


                if (pOrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionDescuento.Count() > 0)
                    CreateEditOrdenGiroDetalleTerceroCausacionDescuento(pOrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionDescuento, pUsuarioCreacion);

                if (pOrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionAportante.Count() > 0)
                    CreateEditOrdenGiroDetalleTerceroCausacionAportante(pOrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionAportante, pUsuarioCreacion);
            }
        }

        private void CreateEditOrdenGiroDetalleTerceroCausacionDescuento(ICollection<OrdenGiroDetalleTerceroCausacionDescuento> pListOrdenGiroDetalleTerceroCausacionDescuento, string pUsuarioCreacion)
        {
            foreach (var OrdenGiroDetalleTerceroCausacionDescuento in pListOrdenGiroDetalleTerceroCausacionDescuento)
            {
                _context.Set<OrdenGiroDetalleTerceroCausacionDescuento>()
                        .Where(o => o.OrdenGiroDetalleTerceroCausacionDescuentoId == OrdenGiroDetalleTerceroCausacionDescuento.OrdenGiroDetalleTerceroCausacionDescuentoId)
                        .Update(o => new OrdenGiroDetalleTerceroCausacionDescuento
                        {
                            UsuarioModificacion = pUsuarioCreacion,
                            FechaModificacion = DateTime.Now,
                            ValorDescuento = OrdenGiroDetalleTerceroCausacionDescuento.ValorDescuento
                        });
            }
        }

        private void CreateEditOrdenGiroDetalleTerceroCausacionAportante(ICollection<OrdenGiroDetalleTerceroCausacionAportante> pOrdenGiroDetalleTerceroCausacionAportante, string pUsuarioCreacion)
        {
            foreach (var TerceroCausacionAportante in pOrdenGiroDetalleTerceroCausacionAportante)
            {
                _context.Set<OrdenGiroDetalleTerceroCausacionAportante>()
                        .Where(r => r.OrdenGiroDetalleTerceroCausacionAportanteId == TerceroCausacionAportante.OrdenGiroDetalleTerceroCausacionAportanteId)
                        .Update(r => new OrdenGiroDetalleTerceroCausacionAportante
                        {
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = pUsuarioCreacion,

                            ValorDescuento = TerceroCausacionAportante.ValorDescuento
                        });
            }
        }


        #endregion
    }
}
