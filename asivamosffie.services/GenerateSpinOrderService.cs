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
        #region Constructor
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly IBudgetAvailabilityService _budgetAvailabilityService;
        private readonly IRegisterValidatePaymentRequierementsService _registerValidatePayment;
        private int Enum = 1;
        public GenerateSpinOrderService(IBudgetAvailabilityService budgetAvailabilityService, IRegisterValidatePaymentRequierementsService registerValidatePaymentRequierementsService, devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _budgetAvailabilityService = budgetAvailabilityService;
            _commonService = commonService;
            _context = context;
            _registerValidatePayment = registerValidatePaymentRequierementsService;
        }
        #endregion

        #region Get
        /// <summary>
        /// TODO : VALIDAR SOLICITUDES DE PAGO QUE YA TENGAN APROBACION 
        /// </summary>
        /// <returns></returns>
        /// 


        public async Task<List<VPlantillaOrdenGiro>> GetInfoPlantilla(int pOrdenGiroId)
        {
            return await _context.VPlantillaOrdenGiro.Where(r => r.OrdenGiroId == pOrdenGiroId).OrderBy(r => r.LlaveMen).ToListAsync();
        }

        public async Task<dynamic> GetValorConceptoByAportanteId(int pAportanteId, int pSolicitudPagoId, string pConceptoPago)
        {
            return _context.VValorUsoXcontratoAportante
                                                   .Where(v => v.AportanteId == pAportanteId
                                                       && v.ConceptoPagoCodigo == pConceptoPago
                                                       && v.SolicitudPagoId == pSolicitudPagoId)
                                                   .Select(v => v.ValorUso);
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
                                             s.IntEstadoCodigo >= (int)EnumEstadoOrdenGiro.Enviada_Para_Verificacion_Orden_Giro
                                              //&& s.FechaRegistroCompletoAprobar.HasValue
                                              )
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

        private void ValidateTerceroGiro(SolicitudPago pSolicitudPago)
        {
            SolicitudPago solicitudPago = _context.SolicitudPago
                                                              .Where(r => r.ContratoId == pSolicitudPago.ContratoId && r.Eliminado != true)
                                                              .Include(r => r.OrdenGiro).ThenInclude(r => r.OrdenGiroTercero).ThenInclude(r => r.OrdenGiroTerceroChequeGerencia)
                                                              .Include(r => r.OrdenGiro).ThenInclude(r => r.OrdenGiroTercero).ThenInclude(r => r.OrdenGiroTerceroTransferenciaElectronica)
                                                              .AsNoTracking()
                                                              .FirstOrDefault();

            pSolicitudPago.MedioPagoCodigo = solicitudPago?.OrdenGiro?.OrdenGiroTercero?.FirstOrDefault()?.MedioPagoGiroCodigo;
            pSolicitudPago.PrimerOrdenGiroTerceroChequeGerencia = solicitudPago?.OrdenGiro?.OrdenGiroTercero?.FirstOrDefault()?.OrdenGiroTerceroChequeGerencia?.FirstOrDefault();
            pSolicitudPago.PrimerOrdenGiroTerceroTransferenciaElectronica = solicitudPago?.OrdenGiro?.OrdenGiroTercero?.FirstOrDefault()?.OrdenGiroTerceroTransferenciaElectronica?.FirstOrDefault();
            if (pSolicitudPago.PrimerOrdenGiroTerceroChequeGerencia != null)
                pSolicitudPago.PrimerOrdenGiroTerceroChequeGerencia.OrdenGiroTerceroChequeGerenciaId = 0;
            if (pSolicitudPago.PrimerOrdenGiroTerceroTransferenciaElectronica != null)
                pSolicitudPago.PrimerOrdenGiroTerceroTransferenciaElectronica.OrdenGiroTerceroTransferenciaElectronicaId = 0;
        }

        public async Task<SolicitudPago> GetSolicitudPagoBySolicitudPagoId(int SolicitudPagoId)
        {
            SolicitudPago SolicitudPago = await _registerValidatePayment.GetSolicitudPago(SolicitudPagoId);

            if (SolicitudPago.ContratoId > 0)
            {
                SolicitudPago.VConceptosUsosXsolicitudPagoId = _context.VConceptosUsosXsolicitudPagoId.Where(r => r.SolicitudPagoId == SolicitudPagoId).ToList();
                SolicitudPago.ContratoSon = await _registerValidatePayment.GetContratoByContratoId((int)SolicitudPago.ContratoId, SolicitudPagoId);
                SolicitudPago.ContratoSon.ListProyectos = await _registerValidatePayment.GetProyectosByIdContrato((int)SolicitudPago.ContratoId);
                SolicitudPago.ValorXProyectoXFaseXAportanteXConcepto = GetInfoValorValorXProyectoXFaseXAportanteXConcepto(SolicitudPago.ContratoSon.ContratacionId);
            }
            ValidateTerceroGiro(SolicitudPago);

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
                    .AsNoTracking()
                    .FirstOrDefault();

                foreach (var OrdenGiroDetalle in SolicitudPago.OrdenGiro.OrdenGiroDetalle)
                {
                    if (OrdenGiroDetalle.OrdenGiroDetalleDescuentoTecnica.Count > 0)
                    {
                        OrdenGiroDetalle.OrdenGiroDetalleDescuentoTecnica = OrdenGiroDetalle.OrdenGiroDetalleDescuentoTecnica.Where(r => r.Eliminado != true).ToList();


                        foreach (var item in OrdenGiroDetalle.OrdenGiroDetalleDescuentoTecnica)
                        {
                            if (item.SolicitudPagoFaseFacturaDescuentoId != null)
                                item.SolicitudPagoFaseFacturaDescuento = _context.SolicitudPagoFaseFacturaDescuento.Find(item.SolicitudPagoFaseFacturaDescuentoId);
                        }
                    }

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
            }
            try
            {
                SolicitudPago.TablaDrpUso = GetDrpContrato(SolicitudPago.ContratoSon.ContratacionId);

                //SolicitudPago.TablaDRP = GetDrpContrato(SolicitudPago);
                SolicitudPago.TablaUsoFuenteAportante = GetTablaUsoFuenteAportante(SolicitudPago);

                SolicitudPago.TablaPorcentajeParticipacion = GetTablaPorcentajeParticipacion(SolicitudPago);
                SolicitudPago.TablaInformacionFuenteRecursos = GetTablaInformacionFuenteRecursos(SolicitudPago);

            }
            catch (Exception ex)
            {
            }
            return SolicitudPago;
        }

        private dynamic GetInfoValorValorXProyectoXFaseXAportanteXConcepto(int contratacionId)
        {
            return _context.VFacturadoXodgXcontratacionXproyectoXaportanteXfaseXconcepXuso
                                                                                        .Where(r => r.ContratacionId == contratacionId)
                                                                                        .Select(r => new
                                                                                        {
                                                                                            TipoUsoCodigo = r.UsoCodigo,
                                                                                            UsoNombre = r.UsoNombre,
                                                                                            r.ProyectoId,
                                                                                            EsPreConstruccion = r.EsPreconstruccion ,
                                                                                            r.AportanteId,
                                                                                            ConceptoCodigo = r.ConceptoCodigo , 
                                                                                            ConceptoNombre = "",
                                                                                            r.ValorUso,
                                                                                            r.ValorDescuento,
                                                                                            Saldo = r.SaldoUso
                                                                                        });
        }

        private dynamic GetTablaInformacionFuenteRecursos(SolicitudPago solicitudPago)
        {
            List<dynamic> List = new List<dynamic>();
            bool OrdenGiroAprobada = _context.OrdenGiro.Where(r => r.SolicitudPago.FirstOrDefault().Contrato.ContratoId == solicitudPago.ContratoId).FirstOrDefault()?.RegistroCompletoAprobar ?? false;

            List<VDescuentosOdgxFuenteFinanciacionXaportante> ListDescuentos =
                                                                            _context.VDescuentosOdgxFuenteFinanciacionXaportante
                                                                            .Where(r => r.OrdenGiroId == solicitudPago.OrdenGiroId)
                                                                            .ToList();

            List<VAportanteFuente> vAportanteFuenteUsos = _context.VAportanteFuente
                                                              .Where(r => r.ContratoId == solicitudPago.ContratoId)
                                                              .ToList();

            List<Dominio> FuenteRecursos = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).ToList();
            foreach (var item in vAportanteFuenteUsos)
            {
                decimal Descuento = ListDescuentos.Where(r => r.AportanteId == item.CofinanciacionAportanteId).Sum(r => r.ValorDescuento) ?? 0;
                CofinanciacionAportante cofinanciacionAportante = _context.CofinanciacionAportante.Find(item.CofinanciacionAportanteId);
                string NombreAportante = _budgetAvailabilityService.getNombreAportante(cofinanciacionAportante);

                List<dynamic> List2 = new List<dynamic>();

                Decimal SaldoAfectado = item.Valor ?? 0;
                if (OrdenGiroAprobada)
                    SaldoAfectado -= Descuento;
                List.Add(new
                {
                    CofinanciacionAportanteId = item.CofinanciacionAportanteId,
                    TipoAportanteId = cofinanciacionAportante.TipoAportanteId,
                    FuenteRecursos = FuenteRecursos.Where(r => r.Codigo == item.FuenteRecursosCodigo).FirstOrDefault().Nombre,
                    NombreAportante = NombreAportante,
                    SaldoActual = item.Valor,
                    SaldoAfectado = SaldoAfectado
                });
            }

            return List;
        }

        private dynamic GetTablaPorcentajeParticipacion(SolicitudPago solicitudPago)
        {
            List<VRpsPorContratacion> vRpsPorContratacions = _context.VRpsPorContratacion.Where(r => r.ContratacionId == solicitudPago.ContratoSon.ContratacionId).ToList();
            List<VDrpXcontratoXaportante> ListVDrpXcontratoXaportante = _context.VDrpXcontratoXaportante.Where(f => f.ContratoId == solicitudPago.ContratoSon.ContratoId).ToList();

            List<dynamic> List = new List<dynamic>();
            int Enum = 1;
            foreach (var drp in vRpsPorContratacions)
            {
                List<dynamic> ListAportantes = new List<dynamic>();

                decimal ValorDRP = drp.ValorSolicitud;

                List<VDrpXcontratoXaportante> VDrpXcontratoXaportante = ListVDrpXcontratoXaportante.Where(r => r.NumeroDrp.Equals(drp.NumeroDrp)).ToList();


                foreach (var aportantes in VDrpXcontratoXaportante)
                {
                    CofinanciacionAportante cofinanciacionAportante = _context.CofinanciacionAportante.Find(aportantes.CofinanciacionAportanteId);

                    string NombreAportante = _budgetAvailabilityService.getNombreAportante(_context.CofinanciacionAportante.Find(aportantes.CofinanciacionAportanteId));
                    decimal ValorAportante = VDrpXcontratoXaportante.Where(r => r.CofinanciacionAportanteId == aportantes.CofinanciacionAportanteId).Sum(r => r.ValorUso) ?? 0;

                    decimal TotalParticipacion = (ValorAportante / ValorDRP) * 100;
                    string NomTotalParticipacion = String.Format("{0:n0}", (TotalParticipacion)) + "%";
                    ListAportantes.Add(new
                    {
                        NombreAportante,
                        NomTotalParticipacion
                    });

                }
                List.Add(new
                {
                    Enum,
                    drp.NumeroDrp,
                    ListAportantes
                });
                Enum++;
            }

            return List;
        }

        public TablaUsoFuenteAportante GetTablaUsoFuenteAportante(SolicitudPago solicitudPago)
        {
            List<VFuentesUsoXcontratoId> List =
                                               _context.VFuentesUsoXcontratoId
                                               .Where(c => c.ContratoId == solicitudPago.ContratoSon.ContratoId)
                                               .ToList();

            List<VFuentesUsoXcontratoId> ListVFuentesUsoXcontratoId = new List<VFuentesUsoXcontratoId>();

            foreach (var item in List)
            {
                if (!ListVFuentesUsoXcontratoId
                    .Any(r => r.TipoUso == item.TipoUso && r.FuenteFinanciacion == item.FuenteFinanciacion))
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
                  .Where(f => f.ContratoId == solicitudPago.ContratoSon.ContratoId)
                  .ToList();

            List<Usos> ListUsos = new List<Usos>();

            foreach (var usos in tabla.Usos)
            {
                if (!ListUsos.Any(r => r.TipoUsoCodigo == usos.TipoUsoCodigo && r.FuenteFinanciacion == usos.FuenteFinanciacion))
                {
                    ListUsos.Add(usos);

                    List<VFuentesUsoXcontratoId> List2 =
                                                        List
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
                                        .Where(
                                               r => r.Nombre == usos.NombreUso
                                            && r.CofinanciacionAportanteId == Aportante.AportanteId
                                              )
                                        .Select(s => s.ValorUso)
                                        .FirstOrDefault() ?? 0;

                                    decimal Descuento = _context.VOrdenGiroPagosXusoAportante.Where(v => v.AportanteId == Aportante.AportanteId && v.TipoUsoCodigo == usos.TipoUsoCodigo).Sum(v => v.ValorDescuento) ?? 0;

                                    Aportante.NombreAportante = _budgetAvailabilityService.getNombreAportante(_context.CofinanciacionAportante.Find(Aportante.AportanteId));
                                    Aportante.Valor = String.Format("{0:n0}", (ValorUso - Descuento));

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

        private List<TablaDRP> GetDrpContrato(SolicitudPago SolicitudPago)
        {
            if (SolicitudPago.ContratoSon == null)
                return new List<TablaDRP>();
            String strTipoSolicitud = SolicitudPago.ContratoSon.Contratacion.TipoSolicitudCodigo;
            List<TablaDRP> ListTablaDrp = new List<TablaDRP>();

            decimal ValorFacturado = SolicitudPago?.OrdenGiro?.TieneTraslado == false ? SolicitudPago?.OrdenGiro?.ValorNetoGiro ?? 0 : SolicitudPago?.OrdenGiro?.ValorNetoGiroTraslado ?? 0;

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

        public dynamic GetDrpContrato(int pContratacionId)
        {
            bool OrdenGiroAprobada = _context.OrdenGiro.Where(r => r.SolicitudPago.FirstOrDefault().Contrato.ContratacionId == pContratacionId).FirstOrDefault()?.RegistroCompletoAprobar ?? false;

            List<VDrpXproyectoXusos> List = _context.VDrpXproyectoXusos.Where(r => r.ContratacionId == pContratacionId).OrderBy(r => r.FechaCreacion).ToList();

            var ListDrp = List.GroupBy(drp => drp.NumeroDrp)
                              .Select(d =>
                                          d.OrderBy(p => p.NumeroDrp)
                                           .FirstOrDefault())
                              .ToList();

            List<dynamic> ListTablaDrp = new List<dynamic>();

            List<VPagosSolicitudXcontratacionXproyectoXuso> ListPagos =
                    _context.VPagosSolicitudXcontratacionXproyectoXuso.Where(v => v.ContratacionId == pContratacionId)
                                                                      .ToList();

            List<VDescuentosXordenGiroXproyectoXaportanteXconceptoXuso> DescuentosOrdenGiro = _context.VDescuentosXordenGiroXproyectoXaportanteXconceptoXuso.Where(r => r.ContratacionId == pContratacionId).ToList();

            foreach (var Drp in ListDrp)
            {
                var ListProyectosId = List.Where(r => r.NumeroDrp == Drp.NumeroDrp)
                                          .GroupBy(id => id.ProyectoId)
                                          .Select(d =>
                                                      d.OrderBy(p => p.ProyectoId)
                                                       .FirstOrDefault())
                                          .ToList();

                List<dynamic> ListDyProyectos = new List<dynamic>();
                foreach (var ProyectoId in ListProyectosId)
                {
                    Proyecto proyecto = _context.Proyecto
                                                        .Where(r => r.ProyectoId == ProyectoId.ProyectoId)
                                                        .Include(ie => ie.InstitucionEducativa)
                                                        .FirstOrDefault();

                    var ListTipoUso = List.Where(r => r.NumeroDrp == Drp.NumeroDrp
                                                   && r.ProyectoId == ProyectoId.ProyectoId)
                                          .GroupBy(id => id.TipoUsoCodigo)
                                          .Select(d => d.OrderBy(p => p.TipoUsoCodigo).FirstOrDefault())
                                          .ToList();

                    List<dynamic> ListDyUsos = new List<dynamic>();
                    foreach (var TipoUso in ListTipoUso)
                    {
                        VDrpXproyectoXusos Uso =
                                                List
                                                .Where(r => r.NumeroDrp == Drp.NumeroDrp
                                                         && r.ProyectoId == ProyectoId.ProyectoId
                                                         && r.TipoUsoCodigo == TipoUso.TipoUsoCodigo)
                                                .FirstOrDefault();

                        decimal? ValorUso = List
                                                .Where(r => r.NumeroDrp == Drp.NumeroDrp
                                                         && r.ProyectoId == ProyectoId.ProyectoId
                                                         && r.TipoUsoCodigo == TipoUso.TipoUsoCodigo)
                                                .Sum(v => v.ValorUso) ?? 0;

                        decimal Saldo = ListPagos
                                                .Where(r => r.ProyectoId == ProyectoId.ProyectoId
                                                         && r.TipoUsoCodigo == TipoUso.TipoUsoCodigo
                                                         && r.Pagado == false)
                                                .Sum(r => r.SaldoUso) ?? 0;

                        decimal Descuentos = DescuentosOrdenGiro
                                                            .Where(r => r.ProyectoId == ProyectoId.ProyectoId
                                                             && r.UsoCodigo == TipoUso.TipoUsoCodigo)
                                                .Sum(r => r.ValorDescuento);

                        decimal ValorUsoResta = ValorUso ?? 0 - Descuentos;

                        if (OrdenGiroAprobada)
                        {
                            foreach (var item in ListPagos.Where(r => r.ProyectoId == ProyectoId.ProyectoId
                                                            && r.TipoUsoCodigo == TipoUso.TipoUsoCodigo).ToList())
                            {
                                if (OrdenGiroAprobada)
                                {
                                    if (ValorUsoResta > item.SaldoUso)
                                    {
                                        ValorUsoResta -= (decimal)item.SaldoUso;
                                        item.SaldoUso = ValorUsoResta;
                                        item.Pagado = true;
                                    }
                                    else
                                    {
                                        item.SaldoUso -= ValorUsoResta;
                                    }
                                }
                                else
                                {
                                    item.SaldoUso = Saldo;
                                }
                            }

                            ListDyUsos.Add(new
                            {
                                Uso.Nombre,
                                ValorUso = String.Format("{0:n0}", ValorUso),
                                Saldo = String.Format("{0:n0}", ValorUso > Saldo ? ValorUso - Saldo : 0)
                            });
                        }

                        else
                        {
                            ListDyUsos.Add(new
                            {
                                Uso.Nombre,
                                ValorUso = String.Format("{0:n0}", ValorUso),
                                Saldo = String.Format("{0:n0}", ValorUso)
                            });
                        }
                    }

                    ListDyProyectos.Add(new
                    {
                        proyecto.InstitucionEducativa.Nombre,
                        ListDyUsos
                    });
                }

                ListTablaDrp.Add(new
                {
                    Enum,
                    Drp.NumeroDrp,
                    ListDyProyectos
                });
                Enum++;
            }
            return ListTablaDrp;
        }

        private void ValidateValorNeto(int pOrdenGiroId)
        {
            OrdenGiro ordenGiro1 =
                _context.OrdenGiro
                .Where(o => o.OrdenGiroId == pOrdenGiroId)
                .Include(o => o.OrdenGiroDetalle).ThenInclude(o => o.OrdenGiroDetalleTerceroCausacion)
                .AsNoTracking()
                .FirstOrDefault();

            if (ordenGiro1.ValorNetoGiro == null)
            {
                foreach (var OrdenGiroDetalle in ordenGiro1.OrdenGiroDetalle)
                {
                    foreach (var OrdenGiroDetalleTerceroCausacion in OrdenGiroDetalle.OrdenGiroDetalleTerceroCausacion)
                    {

                        if (OrdenGiroDetalleTerceroCausacion.ValorNetoGiro != null)
                        {
                            _context.Set<OrdenGiro>()
                                    .Where(o => o.OrdenGiroId == pOrdenGiroId)
                                    .Update(o => new OrdenGiro
                                    {
                                        ValorNetoGiro = OrdenGiroDetalleTerceroCausacion.ValorNetoGiro
                                    });
                        }
                    }
                }
            }
        }

        #endregion
        #region Validate 

        public async Task<bool> ValidarRegistroCompleto(int pSolicitudPago, string pAuthor)
        {
            bool blRegistroCompleto = false;
            try
            {
                SolicitudPago solicitudPago = await GetSolicitudPagoBySolicitudPagoId(pSolicitudPago);
                blRegistroCompleto = ValidarRegistroCompletoOrdenGiro(solicitudPago.OrdenGiro);

                DateTime? CompleteRecordDate = null;
                if (blRegistroCompleto)
                    CompleteRecordDate = DateTime.Now;

                _context.Set<OrdenGiro>()
                        .Where(o => o.OrdenGiroId == solicitudPago.OrdenGiroId)
                        .Update(o => new OrdenGiro
                        {
                            RegistroCompleto = blRegistroCompleto,
                            FechaRegistroCompleto = CompleteRecordDate,
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = pAuthor
                        });
            }
            catch (Exception e)
            {
                return false;
            }
            return blRegistroCompleto;
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


        private bool ValidarRegistroCompletoOrdenGiroDetalleTerceroCausacionAportanteOrigen(OrdenGiroDetalleTerceroCausacionAportante terceroCausacionAportante)
        {
            if (terceroCausacionAportante.CuentaBancariaId == null || terceroCausacionAportante.CuentaBancariaId == 0)
                return false;

            return true;
        }


        private bool ValidarRegistroCompletoOrdenGiroDetalleTerceroCausacionDescuento(OrdenGiroDetalleTerceroCausacionDescuento ordenGiroDetalleTerceroCausacionDescuento)
        {
            if (string.IsNullOrEmpty(ordenGiroDetalleTerceroCausacionDescuento.TipoDescuentoCodigo)
               || ordenGiroDetalleTerceroCausacionDescuento.ValorDescuento == 0
               || ordenGiroDetalleTerceroCausacionDescuento.AportanteId == 0
               || string.IsNullOrEmpty(ordenGiroDetalleTerceroCausacionDescuento.TipoDescuentoCodigo)
               || string.IsNullOrEmpty(ordenGiroDetalleTerceroCausacionDescuento.TipoDescuentoCodigo)
                ) return false;

            return true;
        }

        private bool ValidarRegistroCompletoOrdenGiroDetalleTerceroCausacion(OrdenGiroDetalleTerceroCausacion pOrdenGiroDetalleTerceroCausacion)
        {
            if (pOrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionAportante.Where(r => r.Eliminado != true).Sum(r => r.ValorDescuento) != pOrdenGiroDetalleTerceroCausacion.ValorFacturadoConcepto)
                return false;


            if (pOrdenGiroDetalleTerceroCausacion.TieneDescuento == false)
                return true;

            if (pOrdenGiroDetalleTerceroCausacion.ValorNetoGiro == 0
               || string.IsNullOrEmpty(pOrdenGiroDetalleTerceroCausacion.ConceptoPagoCriterio)
               || string.IsNullOrEmpty(pOrdenGiroDetalleTerceroCausacion.ConceptoCodigo)
               || string.IsNullOrEmpty(pOrdenGiroDetalleTerceroCausacion.TipoPagoCodigo)
               || pOrdenGiroDetalleTerceroCausacion.ValorNetoGiro == 0
               || !pOrdenGiroDetalleTerceroCausacion.TieneDescuento.HasValue
                ) return false;
            if (pOrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionDescuento.Count() == 0)
                return false;


            if (pOrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionAportante.Count() == 0)
                return false;


            foreach (var item in pOrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionAportante)
            {
                if (!ValidarRegistroCompletoOrdenGiroDetalleTerceroCausacionAportante(item))
                    return false;

                if (!ValidarRegistroCompletoOrdenGiroDetalleTerceroCausacionAportanteOrigen(item))
                    return false;
            }

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


            decimal ValorDescuento = _context.SolicitudPagoFaseFacturaDescuento.Find(pOrdenGiroDetalleDescuentoTecnica.SolicitudPagoFaseFacturaDescuentoId).ValorDescuento ?? 0;

            if (pOrdenGiroDetalleDescuentoTecnica.OrdenGiroDetalleDescuentoTecnicaAportante.Where(r => r.Eliminado != true).Sum(r => r.ValorDescuento) > ValorDescuento)
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
            if (pOrdenGiroDetalle.OrdenGiroDetalleTerceroCausacion.Count() == 0)
                return false;

            if (pOrdenGiroDetalle.OrdenGiroDetalleTerceroCausacion.FirstOrDefault().ConceptoPagoCriterio != ConstanCodigoCriterioPago.Anticipo)
            {
                if (
                       pOrdenGiroDetalle?.OrdenGiroDetalleTerceroCausacion.Count() == 0
                    || pOrdenGiroDetalle?.OrdenGiroDetalleObservacion.Count() == 0
                    || pOrdenGiroDetalle?.OrdenGiroSoporte.Count() == 0
                    || pOrdenGiroDetalle?.OrdenGiroDetalleEstrategiaPago.Count() == 0
                    ) return false;

            }
            else
            {



            }


            if (pOrdenGiroDetalle.OrdenGiroDetalleTerceroCausacion.Count() == 0)
                return false;

            foreach (var item in pOrdenGiroDetalle.OrdenGiroDetalleTerceroCausacion)
            {
                if (!ValidarRegistroCompletoOrdenGiroDetalleTerceroCausacion(item))
                    return false;
            }

            if (pOrdenGiroDetalle.OrdenGiroDetalleObservacion.Count() == 0)
                return false;

            foreach (var item in pOrdenGiroDetalle.OrdenGiroDetalleObservacion)
            {
                if (string.IsNullOrEmpty(item.Observacion))
                    return false;
            }

            if (pOrdenGiroDetalle.OrdenGiroSoporte.Count() == 0)
                return false;

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

            if (pOrdenGiroDetalle.OrdenGiroDetalleEstrategiaPago.Count() == 0)
                return false;

            foreach (var item in pOrdenGiroDetalle.OrdenGiroDetalleEstrategiaPago)
            {
                if (string.IsNullOrEmpty(item.EstrategiaPagoCodigo))
                    return false;
            }

            return true;
        }

        #endregion
        #region C R U D

        #region Create
        public async Task<Respuesta> CreateEditOrdenGiro(OrdenGiro pOrdenGiro)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Orden_Giro, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                if (pOrdenGiro.OrdenGiroId == 0)
                {
                    pOrdenGiro.ConsecutivoOrigen = await _commonService.EnumeradorOrigenOrdenGiro();
                    pOrdenGiro.NumeroSolicitud = await _commonService.EnumeradorOrdenGiro((int)pOrdenGiro?.SolicitudPagoId);
                    pOrdenGiro.FechaCreacion = DateTime.Now;
                    pOrdenGiro.Eliminado = false;
                    pOrdenGiro.RegistroCompleto = ValidarRegistroCompletoOrdenGiro(pOrdenGiro);
                    pOrdenGiro.EstadoCodigo = ((int)EnumEstadoOrdenGiro.En_Proceso_Generacion).ToString();
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

                    ValidateValorNeto(pOrdenGiro.OrdenGiroId);

                    _context.Set<OrdenGiro>()
                            .Where(o => o.OrdenGiroId == pOrdenGiro.OrdenGiroId)
                            .Update(o => new OrdenGiro
                            {
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

                _context.SaveChanges();
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
                                ContratacionProyectoId = pOrdenGiroDetalleTerceroCausacion.ContratacionProyectoId,
                                TieneDescuento = pOrdenGiroDetalleTerceroCausacion.TieneDescuento,
                                ValorNetoGiro = pOrdenGiroDetalleTerceroCausacion.ValorNetoGiro,
                                OrdenGiroDetalleId = pOrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleId,
                                ConceptoPagoCriterio = pOrdenGiroDetalleTerceroCausacion.ConceptoPagoCriterio,
                                TipoPagoCodigo = pOrdenGiroDetalleTerceroCausacion.TipoPagoCodigo,
                                EsPreconstruccion = pOrdenGiroDetalleTerceroCausacion.EsPreconstruccion,
                                FechaModificacion = DateTime.Now,
                                UsuarioModificacion = pUsuarioCreacion,
                                ConceptoCodigo = pOrdenGiroDetalleTerceroCausacion.ConceptoCodigo,
                                ValorFacturadoConcepto = pOrdenGiroDetalleTerceroCausacion.ValorFacturadoConcepto,
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
                    TerceroCausacionAportante.RegistroCompletoOrigen = ValidarRegistroCompletoOrdenGiroDetalleTerceroCausacionAportanteOrigen(TerceroCausacionAportante);
                    _context.OrdenGiroDetalleTerceroCausacionAportante.Add(TerceroCausacionAportante);
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
                                RegistroCompleto = ValidarRegistroCompletoOrdenGiroDetalleTerceroCausacionAportante(TerceroCausacionAportante),
                                RegistroCompletoOrigen = ValidarRegistroCompletoOrdenGiroDetalleTerceroCausacionAportanteOrigen(TerceroCausacionAportante)
                            });
                }
            }
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
                    OrdenGiroDetalleTerceroCausacionDescuento.AportanteId = OrdenGiroDetalleTerceroCausacionDescuento.AportanteId;
                    OrdenGiroDetalleTerceroCausacionDescuento.FuenteRecursosCodigo = OrdenGiroDetalleTerceroCausacionDescuento.FuenteRecursosCodigo;
                    OrdenGiroDetalleTerceroCausacionDescuento.FuenteFinanciacionId = OrdenGiroDetalleTerceroCausacionDescuento.FuenteFinanciacionId;
                    OrdenGiroDetalleTerceroCausacionDescuento.RegistroCompleto = ValidarRegistroCompletoOrdenGiroDetalleTerceroCausacionDescuento(OrdenGiroDetalleTerceroCausacionDescuento);

                    _context.OrdenGiroDetalleTerceroCausacionDescuento.Add(OrdenGiroDetalleTerceroCausacionDescuento);
                }
                else
                {
                    _context.Set<OrdenGiroDetalleTerceroCausacionDescuento>()
                            .Where(o => o.OrdenGiroDetalleTerceroCausacionDescuentoId == OrdenGiroDetalleTerceroCausacionDescuento.OrdenGiroDetalleTerceroCausacionDescuentoId)
                            .Update(o => new OrdenGiroDetalleTerceroCausacionDescuento
                            {
                                UsuarioModificacion = pUsuarioCreacion,
                                FechaModificacion = DateTime.Now,
                                FuenteFinanciacionId = OrdenGiroDetalleTerceroCausacionDescuento.FuenteFinanciacionId,
                                FuenteRecursosCodigo = OrdenGiroDetalleTerceroCausacionDescuento.FuenteRecursosCodigo,
                                AportanteId = OrdenGiroDetalleTerceroCausacionDescuento.AportanteId,
                                TipoDescuentoCodigo = OrdenGiroDetalleTerceroCausacionDescuento.TipoDescuentoCodigo,
                                ValorDescuento = OrdenGiroDetalleTerceroCausacionDescuento.ValorDescuento,
                                RegistroCompleto = ValidarRegistroCompletoOrdenGiroDetalleTerceroCausacionDescuento(OrdenGiroDetalleTerceroCausacionDescuento)
                            });
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
                                     ContratacionProyectoId = pOrdenGiroDetalleDescuentoTecnica.ContratacionProyectoId,
                                     SolicitudPagoFaseFacturaDescuentoId = pOrdenGiroDetalleDescuentoTecnica.SolicitudPagoFaseFacturaDescuentoId,
                                     TipoPagoCodigo = pOrdenGiroDetalleDescuentoTecnica.TipoPagoCodigo,
                                     CriterioCodigo = pOrdenGiroDetalleDescuentoTecnica.CriterioCodigo,
                                     FechaModificacion = DateTime.Now,
                                     UsuarioModificacion = pUsuarioCreacion,
                                     EsPreconstruccion = pOrdenGiroDetalleDescuentoTecnica.EsPreconstruccion,
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
                                     FuenteFinanciacionId = pOrdenGiroDetalleDescuentoTecnicaAportante.FuenteFinanciacionId
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
        #endregion


        #region Delete
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
                     Code = GeneralCodes.EliminacionExitosa,
                     Message =
                     await _commonService.GetMensajesValidacionesByModuloAndCodigo(
                                                                                     (int)enumeratorMenu.Generar_Orden_de_giro,
                                                                                     GeneralCodes.EliminacionExitosa,
                                                                                     idAccion,
                                                                                     pAuthor,
                                                                                     ConstantCommonMessages.SpinOrder.ELIMINAR_APORTANTE_ORDENES_GIRO
                                                                                 )
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

        public async Task<Respuesta> DeleteOrdenGiroDetalleDescuentoTecnicaByConcepto(int pOrdenGiroDetalleDescuentoTecnicaId, string pConceptoPagoCodigo, string pAuthor)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Aportante_Orden_Giro, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                await _context.Set<OrdenGiroDetalleDescuentoTecnicaAportante>()
                          .Where(o => o.OrdenGiroDetalleDescuentoTecnicaId == pOrdenGiroDetalleDescuentoTecnicaId && o.ConceptoPagoCodigo == pConceptoPagoCodigo)
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
                     Code = GeneralCodes.EliminacionExitosa,
                     Message =
                     await _commonService.GetMensajesValidacionesByModuloAndCodigo(
                                                                                     (int)enumeratorMenu.Generar_Orden_de_giro,
                                                                                     GeneralCodes.EliminacionExitosa,
                                                                                     idAccion,
                                                                                     pAuthor,
                                                                                     ConstantCommonMessages.SpinOrder.ELIMINAR_APORTANTE_ORDENES_GIRO
                                                                                 )
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

        public async Task<Respuesta> DeleteOrdenGiroDetalleTerceroCausacionAportante(int pOrdenGiroDetalleTerceroCausacionAportanteId, string pAuthor)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Orden_Giro, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                _context.Set<OrdenGiroDetalleTerceroCausacionAportante>()
                        .Where(o => o.OrdenGiroDetalleTerceroCausacionAportanteId == pOrdenGiroDetalleTerceroCausacionAportanteId)
                        .Update(o => new OrdenGiroDetalleTerceroCausacionAportante
                        {
                            Eliminado = true,
                            UsuarioModificacion = pAuthor,
                            FechaModificacion = DateTime.Now
                        });


                OrdenGiroDetalleTerceroCausacionAportante ordenGiroDetalleTerceroCausacionAportante =
                                                                                              _context.OrdenGiroDetalleTerceroCausacionAportante
                                                                                                                                     .Where(r => r.OrdenGiroDetalleTerceroCausacionAportanteId == pOrdenGiroDetalleTerceroCausacionAportanteId)
                                                                                                                                     .AsNoTracking()
                                                                                                                                     .FirstOrDefault();


                List<OrdenGiroDetalleTerceroCausacionDescuento> ListOrdenGiroDetalleTerceroCausacionDescuento =
                                                                  _context.OrdenGiroDetalleTerceroCausacionDescuento
                                                                                                        .Where(o => o.AportanteId == ordenGiroDetalleTerceroCausacionAportante.AportanteId
                                                                                                                 && o.OrdenGiroDetalleTerceroCausacionId == ordenGiroDetalleTerceroCausacionAportante.OrdenGiroDetalleTerceroCausacionId)
                                                                                                        .ToList();

                ///Eliminar Descuentos del aportante eliminado
                await DeleteOrdenGiroDetalleTerceroCausacionDescuento(ListOrdenGiroDetalleTerceroCausacionDescuento.Select(r => r.OrdenGiroDetalleTerceroCausacionDescuentoId).ToList(), pAuthor);


                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.EliminacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo(
                            (int)enumeratorMenu.Generar_Orden_de_giro,
                            GeneralCodes.EliminacionExitosa,
                            idAccion,
                            pAuthor,
                            ConstantCommonMessages.SpinOrder.ELIMINAR_ORDEN_GIRO_DESCUENTO_TECNICA)
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo(
                         (int)enumeratorMenu.Generar_Orden_de_giro,
                         GeneralCodes.Error,
                         idAccion,
                         pAuthor,
                         ConstantCommonMessages.SpinOrder.REGISTRAR_ORDENES_GIRO)
                };
            }
        }

        public async Task<Respuesta> DeleteOrdenGiroDetalleTerceroCausacionDescuento(List<int> pOrdenGiroDetalleTerceroCausacionDescuentoId, string pAuthor)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Orden_Giro, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                foreach (var id in pOrdenGiroDetalleTerceroCausacionDescuentoId)
                {
                    _context.Set<OrdenGiroDetalleTerceroCausacionDescuento>()
                     .Where(o => o.OrdenGiroDetalleTerceroCausacionDescuentoId == id)
                     .Update(o => new OrdenGiroDetalleTerceroCausacionDescuento
                     {
                         Eliminado = true,
                         UsuarioModificacion = pAuthor,
                         FechaModificacion = DateTime.Now
                     });
                }

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.EliminacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo(
                            (int)enumeratorMenu.Generar_Orden_de_giro,
                            GeneralCodes.EliminacionExitosa,
                            idAccion,
                            pAuthor,
                            ConstantCommonMessages.SpinOrder.ELIMINAR_ORDEN_GIRO_DESCUENTO_TECNICA)
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.Error,
                    Message =
                      await _commonService.GetMensajesValidacionesByModuloAndCodigo
                      (
                         (int)enumeratorMenu.Generar_Orden_de_giro,
                         GeneralCodes.Error,
                         idAccion,
                         pAuthor,
                         ConstantCommonMessages.SpinOrder.REGISTRAR_ORDENES_GIRO
                       )
                };
            }
        }

        public async Task<Respuesta> DeleteOrdenGiroDetalleDescuentoTecnica(int pOrdenGiroDetalleDescuentoTecnicaId, string pAuthor)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Orden_Giro, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                _context.Set<OrdenGiroDetalleDescuentoTecnica>()
                        .Where(o => o.OrdenGiroDetalleDescuentoTecnicaId == pOrdenGiroDetalleDescuentoTecnicaId)
                        .Update(o => new OrdenGiroDetalleDescuentoTecnica
                        {
                            Eliminado = true,
                            UsuarioModificacion = pAuthor,
                            FechaModificacion = DateTime.Now
                        });

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.EliminacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo(
                            (int)enumeratorMenu.Generar_Orden_de_giro,
                            GeneralCodes.EliminacionExitosa,
                            idAccion,
                            pAuthor,
                            ConstantCommonMessages.SpinOrder.ELIMINAR_ORDEN_GIRO_DESCUENTO_TECNICA)
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo(
                         (int)enumeratorMenu.Generar_Orden_de_giro,
                         GeneralCodes.Error,
                         idAccion,
                         pAuthor,
                         ConstantCommonMessages.SpinOrder.REGISTRAR_ORDENES_GIRO)
                };
            }

        }

        public async Task<Respuesta> DeleteOrdenGiro(int OrdenGiroId, string pAuthor)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Orden_Giro, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                //_context.Set<OrdedeGiro>()
                //        .Where(o => o.OrdenGiroId == OrdenGiroId)
                //        .Update(o => new OrdenGiro
                //        {
                //            Eliminado = true,
                //            UsuarioModificacion = pAuthor,
                //            FechaModificacion = DateTime.Now
                //        });

                _context.Set<SolicitudPago>()
                     .Where(o => o.OrdenGiroId == OrdenGiroId)
                     .Update(o => new SolicitudPago
                     {
                         OrdenGiroId = null,
                         UsuarioModificacion = pAuthor,
                         FechaModificacion = DateTime.Now
                     });

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.EliminacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo(
                            (int)enumeratorMenu.Generar_Orden_de_giro,
                            GeneralCodes.OperacionExitosa,
                            idAccion,
                            pAuthor,
                            ConstantCommonMessages.SpinOrder.REGISTRAR_ORDENES_GIRO)
                };
            }
            catch (Exception ex)
            {

                throw;
            }

        }



        #endregion
        #endregion





    }
}
