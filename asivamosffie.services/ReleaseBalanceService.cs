using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class ReleaseBalanceService : IReleaseBalanceService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly IRequestBudgetAvailabilityService _requestBudgetAvailabilityService;
        private readonly IFinalBalanceService _finalBalanceService;

        public ReleaseBalanceService(devAsiVamosFFIEContext context, ICommonService commonService, IRequestBudgetAvailabilityService requestBudgetAvailabilityService, IFinalBalanceService finalBalanceService)
        {
            _commonService = commonService;
            _context = context;
            _requestBudgetAvailabilityService = requestBudgetAvailabilityService;
            _finalBalanceService = finalBalanceService;
    }

    //traer los DRP
    public async Task<List<dynamic>> GetDrpByProyectoId(int pProyectoId)
        {
            List<dynamic> drps = new List<dynamic>();

            List<ContratacionProyecto> contratacionProyectos = _context.ContratacionProyecto.Where(r => r.ProyectoId == pProyectoId).ToList();
            if (contratacionProyectos != null)
            {
                foreach (var cp in contratacionProyectos)
                {
                    DisponibilidadPresupuestal drp = _context.DisponibilidadPresupuestal
                        .Where(r => r.Eliminado != true && r.ContratacionId == cp.ContratacionId && r.NumeroDrp != null && (r.EstadoSolicitudCodigo == "5" || r.EstadoSolicitudCodigo == "8")).FirstOrDefault();
                    if (drp != null)
                    {
                        DisponibilidadPresupuestalHistorico dpph = _context.DisponibilidadPresupuestalHistorico.Where(r => r.DisponibilidadPresupuestalId == drp.DisponibilidadPresupuestalId).FirstOrDefault();
                            
                        List<VSaldoAliberar> datosAportante = _context.VSaldoAliberar.Where(r => r.ProyectoId == pProyectoId && r.DisponibilidadPresupuestalId == drp.DisponibilidadPresupuestalId && r.EsNovedad != true).ToList();
                        BalanceFinanciero balanceFinanciero = _context.BalanceFinanciero.Where(r => r.ProyectoId == pProyectoId).FirstOrDefault();
                        bool liberado = false;
                        if (balanceFinanciero != null)
                        {
                            if (balanceFinanciero.EstadoBalanceCodigo == ConstanCodigoEstadoBalanceFinanciero.Con_balance_aprobado)
                                liberado = true;
                        }
                        datosAportante.ForEach(da =>{
                            decimal valorActual = 0;
                            CofinanciacionAportante ca = _context.CofinanciacionAportante.Find(da.CofinanciacionAportanteId);
                            if(ca != null)
                                da.NombreAportante = _requestBudgetAvailabilityService.getNombreAportante(ca);
                            da.NombreFuente = _context.Dominio.Where(x => x.Codigo == da.FuenteRecursosCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre;

                            List<VPlantillaOrdenGiroUsos> VPlantillaOrdenGiroUsos = _context.VPlantillaOrdenGiroUsos
                                                                                     .Where(r => r.ContratoId == da.ContratoId
                                                                                              && r.AportanteId == da.CofinanciacionAportanteId
                                                                                              && r.LlaveMen == da.LlaveMen
                                                                                              && r.UsoCodigo == da.CodigoUso
                                                                                              && r.EstaAprobada == true
                                                                                      ).ToList();

                            if (VPlantillaOrdenGiroUsos != null)
                            {

                                decimal Descuento = (VPlantillaOrdenGiroUsos.Sum(r => r.ValorConcepto - r.DescuentoReteFuente - r.DescuentoOtros - r.DescuentoAns)) ?? 0;

                                decimal ValorConcepto = VPlantillaOrdenGiroUsos.Sum(r => r.ValorConcepto) ?? 0;


                                Decimal Total = Math.Abs(da.ValorUso ?? 0 - Descuento);

                                if (Total == 0)
                                    Total = da.ValorUso ?? 0;

                                valorActual =  Total;
                            }
                            if (!liberado)
                            {
                                da.SaldoPresupuestal = valorActual;
                                da.SaldoTesoral = GetSaldoByDrp(cp.ContratacionId, drp.NumeroDrp, cp.ProyectoId);

                            }
                            else
                            {
                                ComponenteUsoHistorico cuh = _context.ComponenteUsoHistorico.Find(da.ComponenteUsoHistoricoId);
                                if (cuh != null)
                                    da.SaldoPresupuestal = cuh.Saldo;
                            }
                        });
                        if (datosAportante.Count() > 0)
                        {
                            drps.Add(new
                            {
                                DisponibilidadPresupuestalHistoricoId = dpph != null ? dpph.DisponibilidadPresupuestalHistoricoId : 0,
                                NovedadContractualRegistroPresupuestalHistoricoId = 0,
                                drp.NumeroDrp,
                                drp.DisponibilidadPresupuestalId,
                                NovedadContractualRegistroPresupuestalId = 0,
                                cp.ProyectoId,
                                cp.ContratacionId,
                                AportantesGrid = datosAportante,
                                drp.ValorSolicitud,
                                BalanceFinanciero = balanceFinanciero,
                                EsNovedad = false
                            });
                        }
                        //Novedades
                        List<NovedadContractualRegistroPresupuestal> drpNs = _context.NovedadContractualRegistroPresupuestal
                        .Where(r => r.Eliminado != true && r.DisponibilidadPresupuestalId == drp.DisponibilidadPresupuestalId && r.NumeroDrp != null && (r.EstadoSolicitudCodigo == "5" || r.EstadoSolicitudCodigo == "8")).ToList();
                        foreach (var drpN in drpNs)
                        {
                            if (drpN != null)
                            {
                                NovedadContractualRegistroPresupuestalHistorico dpphN = _context.NovedadContractualRegistroPresupuestalHistorico.Where(r => r.NovedadContractualRegistroPresupuestalId == drpN.NovedadContractualRegistroPresupuestalId).FirstOrDefault();

                                List<VSaldoAliberar> datosAportanteNovedad = _context.VSaldoAliberar.Where(r => r.ProyectoId == pProyectoId && r.DisponibilidadPresupuestalId == drp.DisponibilidadPresupuestalId && r.NovedadContractualRegistroPresupuestalId == drpN.NovedadContractualRegistroPresupuestalId).ToList();
                                if (balanceFinanciero != null)
                                {
                                    if (balanceFinanciero.EstadoBalanceCodigo == ConstanCodigoEstadoBalanceFinanciero.Con_balance_aprobado)
                                        liberado = true;
                                }
                                datosAportanteNovedad.ForEach(da => {
                                    decimal valorActual = 0;
                                    CofinanciacionAportante ca = _context.CofinanciacionAportante.Find(da.CofinanciacionAportanteId);
                                    if (ca != null)
                                        da.NombreAportante = _requestBudgetAvailabilityService.getNombreAportante(ca);
                                    da.NombreFuente = _context.Dominio.Where(x => x.Codigo == da.FuenteRecursosCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre;

                                    List<VPlantillaOrdenGiroUsos> VPlantillaOrdenGiroUsos = _context.VPlantillaOrdenGiroUsos
                                                                                             .Where(r => r.ContratoId == da.ContratoId
                                                                                                      && r.AportanteId == da.CofinanciacionAportanteId
                                                                                                      && r.LlaveMen == da.LlaveMen
                                                                                                      && r.UsoCodigo == da.CodigoUso
                                                                                                      && r.EstaAprobada == true
                                                                                              ).ToList();

                                    if (VPlantillaOrdenGiroUsos != null)
                                    {

                                        decimal Descuento = (VPlantillaOrdenGiroUsos.Sum(r => r.ValorConcepto - r.DescuentoReteFuente - r.DescuentoOtros - r.DescuentoAns)) ?? 0;

                                        decimal ValorConcepto = VPlantillaOrdenGiroUsos.Sum(r => r.ValorConcepto) ?? 0;


                                        Decimal Total = Math.Abs(da.ValorUso ?? 0 - Descuento);

                                        if (Total == 0)
                                            Total = da.ValorUso ?? 0;

                                        valorActual = Total;
                                    }
                                    if (!liberado)
                                    {
                                        da.SaldoPresupuestal = valorActual;
                                        da.SaldoTesoral = GetSaldoByDrp(cp.ContratacionId, drpN.NumeroDrp, cp.ProyectoId);
                                    }
                                    else
                                    {
                                        ComponenteUsoNovedadHistorico cuhn = _context.ComponenteUsoNovedadHistorico.Find(da.ComponenteUsoNovedadHistoricoId);
                                        if (cuhn != null)
                                            da.SaldoPresupuestal = cuhn.Saldo;
                                    }
                                });
                                if (datosAportanteNovedad.Count() > 0)
                                {
                                    drps.Add(new
                                    {
                                        DisponibilidadPresupuestalHistoricoId = 0,
                                        NovedadContractualRegistroPresupuestalHistoricoId = dpphN != null ? dpphN.NovedadContractualRegistroPresupuestalHistoricoId : 0,
                                        drpN.NumeroDrp,
                                        drpN.DisponibilidadPresupuestalId,
                                        drpN.NovedadContractualRegistroPresupuestalId,
                                        cp.ProyectoId,
                                        cp.ContratacionId,
                                        AportantesGrid = datosAportanteNovedad,
                                        drpN.ValorSolicitud,
                                        BalanceFinanciero = balanceFinanciero,
                                        EsNovedad = true
                                    });
                                }
                            }
                        }
                    }

                }
            }

            return drps;
        }

        public decimal GetSaldoByDrp(int pContratacionId, string numeroDrp, int proyectoId)
        {
            decimal saldo = 0;

            bool OrdenGiroAprobada = _context.OrdenGiro.Where(r => r.SolicitudPago.FirstOrDefault().Contrato.ContratacionId == pContratacionId).FirstOrDefault()?.RegistroCompletoAprobar ?? false;

            List<VDrpXproyectoXusos> List = _context.VDrpXproyectoXusos.Where(r => r.ContratacionId == pContratacionId).OrderBy(r => r.FechaCreacion).ToList();

            var ListDrp = List.Where(r => r.NumeroDrp == numeroDrp).GroupBy(drp => drp.NumeroDrp)
                              .Select(d =>
                                          d.OrderBy(p => p.NumeroDrp)
                                           .FirstOrDefault())
                              .ToList();

            List<dynamic> ListTablaDrp = new List<dynamic>();

            List<VPagosSolicitudXcontratacionXproyectoXuso> ListPagos =
                    _context.VPagosSolicitudXcontratacionXproyectoXuso.Where(v => v.ContratacionId == pContratacionId)
                                                                      .ToList();

            //  List<VDescuentosXordenGiroXproyectoXaportanteXconceptoXuso> DescuentosOrdenGiro = _context.VDescuentosXordenGiroXproyectoXaportanteXconceptoXuso.Where(r => r.ContratacionId == pContratacionId).ToList();

            List<VDescuentosXordenGiroXproyectoXaportanteXconceptoXuso> DescuentosOrdenGiro = new List<VDescuentosXordenGiroXproyectoXaportanteXconceptoXuso>();

            foreach (var Drp in ListDrp)
            {
                var ListProyectosId = List.Where(r => r.NumeroDrp == Drp.NumeroDrp && r.ProyectoId == proyectoId)
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

                        decimal? Saldo = ListPagos
                                                .Where(r => r.ProyectoId == ProyectoId.ProyectoId
                                                         && r.TipoUsoCodigo == TipoUso.TipoUsoCodigo
                                                          && r.Pagado == false
                                                         )
                                                .Sum(r => r.SaldoUso) ?? 0;


                        decimal Descuentos = 0;

                        decimal ValorUsoResta = ValorUso ?? 0 - Descuentos;

                        if (true)
                        {
                            foreach (var item in ListPagos.Where(r => r.ProyectoId == ProyectoId.ProyectoId
                                                            && r.TipoUsoCodigo == TipoUso.TipoUsoCodigo).ToList())
                            {
                                if (true)
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
                                        break;
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
                                Saldo = Saldo > 0 ? (ValorUso - Saldo) < 0 ? 0 : ValorUso - Saldo : ValorUso
                            });
                        }
                        else
                        {
                            ListDyUsos.Add(new
                            {
                                Uso.Nombre,
                                ValorUso = String.Format("{0:n0}", ValorUso),
                                Saldo = ValorUso
                            });
                        }
                    }

                    ListDyProyectos.Add(new
                    {
                        ListDyUsos
                    });
                }

                ListTablaDrp.Add(new
                {
                    Drp.NumeroDrp,
                    ListDyProyectos
                });
            }

            foreach (var list1 in ListTablaDrp)
            {
                foreach (var uso1 in list1.ListDyProyectos)
                {
                    foreach (var uso in uso1.ListDyUsos)
                    {
                        saldo += uso.Saldo;
                    }
                }
            }

            return saldo;
        }


        private bool RegitroCompletoDrpsTai(int pProyectoId)
        {
            bool esCompleto = false;
            int totalRegistrosDrpALiberar = 0;
            int totalRegistrosDrp = _context.VSaldoAliberar.Where(r => r.ProyectoId == pProyectoId).Count();
            totalRegistrosDrpALiberar = _context.VSaldoAliberar.Where(r => r.ProyectoId == pProyectoId && ((r.ComponenteUsoHistoricoId != null && r.ComponenteUsoHistoricoId > 0) || (r.ComponenteUsoNovedadHistoricoId != null && r.ComponenteUsoNovedadHistoricoId > 0))).Count();

            if (totalRegistrosDrpALiberar == totalRegistrosDrp)
            {
                esCompleto = true;
            }
            return esCompleto;
        }


        public async Task<Respuesta> CreateEditHistoricalReleaseBalance(VUsosHistorico pUsosHistorico, string user)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Componente_uso_Historico, (int)EnumeratorTipoDominio.Acciones);

            Respuesta respuesta = new Respuesta();
            try
            {
                string strCrearEditar = string.Empty;
                if (pUsosHistorico != null)
                {

                    foreach (var usoHistorico in pUsosHistorico.UsosHistorico)
                    {
                        if (usoHistorico.EsNovedad != true)
                        {
                            ComponenteUso componenteUso = _context.ComponenteUso.Find(usoHistorico.ComponenteUsoId);
                            if (componenteUso != null)
                            {
                                if (usoHistorico.ComponenteUsoHistoricoId == 0)
                                {
                                    ComponenteUsoHistorico componenteUsoHistorico = new ComponenteUsoHistorico();

                                    strCrearEditar = "CREAR HISTÓRICO COMPONENTE USO ";
                                    componenteUsoHistorico.FechaCreacion = DateTime.Now;
                                    componenteUsoHistorico.UsuarioCreacion = user;
                                    componenteUsoHistorico.ComponenteUsoId = usoHistorico.ComponenteUsoId;
                                    //Este valor se calcula del valor Uso actual -  valor Liberar
                                    //En este momento, no es histórico, se convierte en histórico cuando se de clic en liberar saldo y la columna Liberado sea true
                                    componenteUsoHistorico.ValorUso = componenteUso.ValorUso - usoHistorico.ValorLiberar;
                                    componenteUsoHistorico.Saldo = usoHistorico.Saldo;
                                    _context.ComponenteUsoHistorico.Add(componenteUsoHistorico);
                                }
                                else
                                {
                                    strCrearEditar = "ACTUALIZAR HISTÓRICO COMPONENTE USO ";
                                    ComponenteUsoHistorico componenteUsoHistoricoOld = _context.ComponenteUsoHistorico.Find(usoHistorico.ComponenteUsoHistoricoId);
                                    if (componenteUsoHistoricoOld != null)
                                    {
                                        await _context.Set<ComponenteUsoHistorico>()
                                                        .Where(r => r.ComponenteUsoHistoricoId == usoHistorico.ComponenteUsoHistoricoId && r.ComponenteUsoId == usoHistorico.ComponenteUsoId)
                                                        .UpdateAsync(r => new ComponenteUsoHistorico()
                                                        {
                                                            FechaModificacion = DateTime.Now,
                                                            UsuarioModificacion = user,
                                                            Liberado = false,
                                                            ValorUso = componenteUso.ValorUso - usoHistorico.ValorLiberar,
                                                            Saldo = usoHistorico.Saldo
                                                        });
                                    }
                                }
                            }
                        }
                        else
                        {
                            ComponenteUsoNovedad componenteUsoNovedad = _context.ComponenteUsoNovedad.Find(usoHistorico.ComponenteUsoNovedadId);
                            if (componenteUsoNovedad != null)
                            {
                                if (usoHistorico.ComponenteUsoNovedadHistoricoId == 0)
                                {
                                    ComponenteUsoNovedadHistorico componenteUsoNovedadHistorico = new ComponenteUsoNovedadHistorico();

                                    strCrearEditar = "CREAR HISTÓRICO COMPONENTE USO ";
                                    componenteUsoNovedadHistorico.FechaCreacion = DateTime.Now;
                                    componenteUsoNovedadHistorico.UsuarioCreacion = user;
                                    componenteUsoNovedadHistorico.ComponenteUsoNovedadId = usoHistorico.ComponenteUsoNovedadId;
                                    componenteUsoNovedadHistorico.ValorUso = componenteUsoNovedad.ValorUso - usoHistorico.ValorLiberar;
                                    componenteUsoNovedadHistorico.Saldo = usoHistorico.Saldo;
                                    _context.ComponenteUsoNovedadHistorico.Add(componenteUsoNovedadHistorico);
                                }
                                else
                                {
                                    strCrearEditar = "ACTUALIZAR HISTÓRICO COMPONENTE USO ";
                                    ComponenteUsoNovedadHistorico componenteUsoNovedadHistoricoOld = _context.ComponenteUsoNovedadHistorico.Find(usoHistorico.ComponenteUsoNovedadHistoricoId);
                                    if (componenteUsoNovedadHistoricoOld != null)
                                    {
                                        await _context.Set<ComponenteUsoNovedadHistorico>()
                                                        .Where(r => r.ComponenteUsoNovedadHistoricoId == usoHistorico.ComponenteUsoNovedadHistoricoId && r.ComponenteUsoNovedadId == usoHistorico.ComponenteUsoNovedadId)
                                                        .UpdateAsync(r => new ComponenteUsoNovedadHistorico()
                                                        {
                                                            FechaModificacion = DateTime.Now,
                                                            UsuarioModificacion = user,
                                                            Liberado = false,
                                                            ValorUso = componenteUsoNovedad.ValorUso - usoHistorico.ValorLiberar,
                                                            Saldo = usoHistorico.Saldo
                                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                _context.SaveChanges();

                bool registroCompleto = RegitroCompletoDrpsTai(pUsosHistorico.ProyectoId);

                if (pUsosHistorico.BalanceFinancieroId == 0)
                {
                    BalanceFinanciero balanceFinanciero = new BalanceFinanciero();
                    balanceFinanciero.RequiereTransladoRecursos = false;
                    balanceFinanciero.UsuarioCreacion = user;
                    balanceFinanciero.CumpleCondicionesTai = true;
                    balanceFinanciero.ProyectoId = pUsosHistorico.ProyectoId;
                    balanceFinanciero.RegistroCompleto = registroCompleto;
                    await _finalBalanceService.CreateEditBalanceFinanciero(balanceFinanciero);
                }
                else
                {
                    BalanceFinanciero balanceFinanciero = _context.BalanceFinanciero.Find(pUsosHistorico.BalanceFinancieroId);
                    if (balanceFinanciero != null)
                    {
                        balanceFinanciero.RegistroCompleto = registroCompleto;
                        balanceFinanciero.CumpleCondicionesTai = true;
                        await _finalBalanceService.CreateEditBalanceFinanciero(balanceFinanciero);
                    }

                }

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_balance_financiero_traslados_de_recursos, GeneralCodes.OperacionExitosa, idAccion, user, strCrearEditar)
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_balance_financiero_traslados_de_recursos, GeneralCodes.Error, idAccion, user, ex.InnerException.ToString())
                    };
            }
        }

        public async Task<Respuesta> ReleaseBalance(int pBalanceFinancieroId, string user)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Liberar_saldo, (int)EnumeratorTipoDominio.Acciones);
            Respuesta respuesta = new Respuesta();
            try
            {
                string strCrearEditar = "LIBERAR SALDO";
                if (pBalanceFinancieroId != 0)
                {
                    BalanceFinanciero balanceFinanciero = _context.BalanceFinanciero.Find(pBalanceFinancieroId);
                    if (balanceFinanciero != null)
                    {
                        DisponibilidadPresupuestal drp = null;
                        List<DisponibilidadPresupuestalProyecto> dpps = new List<DisponibilidadPresupuestalProyecto>();

                        List<ContratacionProyecto> contratacionProyectos = _context.ContratacionProyecto.Where(r => r.ProyectoId == balanceFinanciero.ProyectoId && r.Eliminado != true).ToList();
                        List<ProyectoAportante> proyectoAportantesData = new List<ProyectoAportante>();

                        foreach (var cp in contratacionProyectos)
                        {
                            List<VSaldoAliberar> usos = _context.VSaldoAliberar.Where(r => r.ProyectoId == balanceFinanciero.ProyectoId && r.ContratacionId == cp.ContratacionId && r.EsNovedad != true).ToList();

                            if (usos.FirstOrDefault()?.DisponibilidadPresupuestalId > 0)
                            {
                                drp = _context.DisponibilidadPresupuestal.Find(usos.FirstOrDefault().DisponibilidadPresupuestalId);
                                dpps = _context.DisponibilidadPresupuestalProyecto.Where(r => r.DisponibilidadPresupuestalId == usos.FirstOrDefault().DisponibilidadPresupuestalId && r.ProyectoId == balanceFinanciero.ProyectoId && r.Eliminado != true).ToList();
                            }

                            #region ddp
                            if (drp != null)
                            {
                                decimal valorDrpNuevo = 0;
                                decimal valorDrpActual = drp.ValorSolicitud;
                                decimal valorALiberarTotal = 0;

                                foreach (var uso in usos)
                                {
                                    if (uso.EsNovedad != true)
                                    {
                                        //Acá se hace el cambio, los valores que estban en el histórico pasan a la tabla principal y en el histórico queda el 
                                        //valor del uso original

                                        ComponenteUso componenteUso = _context.ComponenteUso.Find(uso.ComponenteUsoId);
                                        ComponenteUsoHistorico componenteUsoHistorico = _context.ComponenteUsoHistorico.Find(uso.ComponenteUsoHistoricoId);
                                        decimal valorUsoActual = componenteUso.ValorUso;
                                        decimal valorUsoNuevo = componenteUsoHistorico.ValorUso;
                                        valorDrpNuevo += valorUsoNuevo;
                                        valorALiberarTotal += (valorUsoActual - valorUsoNuevo);

                                        await _context.Set<ComponenteUsoHistorico>()
                                                            .Where(r => r.ComponenteUsoHistoricoId == uso.ComponenteUsoHistoricoId)
                                                            .UpdateAsync(r => new ComponenteUsoHistorico()
                                                            {
                                                                FechaModificacion = DateTime.Now,
                                                                UsuarioModificacion = user,
                                                                Liberado = true,
                                                                ValorUso = valorUsoActual
                                                            });

                                        await _context.Set<ComponenteUso>()
                                                            .Where(r => r.ComponenteUsoId == uso.ComponenteUsoId)
                                                            .UpdateAsync(r => new ComponenteUso()
                                                            {
                                                                FechaModificacion = DateTime.Now,
                                                                UsuarioModificacion = user,
                                                                ValorUso = valorUsoNuevo
                                                            });


                                        //obtengo componente Aportante
                                        ComponenteAportante componenteAportante = _context.ComponenteAportante.Find(componenteUso.ComponenteAportanteId);
                                        if (componenteAportante != null)
                                        {
                                            ContratacionProyectoAportante cpa = _context.ContratacionProyectoAportante.Find(componenteAportante.ContratacionProyectoAportanteId);
                                            if (cpa != null)
                                            {
                                                Contratacion contratacionDP = _context.Contratacion.Where(r => r.ContratacionId == cp.ContratacionId).FirstOrDefault();

                                                if (contratacionDP.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Interventoria)
                                                {
                                                    int index = proyectoAportantesData.FindIndex(item => item.AportanteId == cpa.CofinanciacionAportanteId);
                                                    if (index > -1)
                                                    {
                                                        proyectoAportantesData[index].ValorInterventoriaNuevo += valorUsoNuevo;
                                                        proyectoAportantesData[index].ValorInterventoriaActual += valorUsoActual;

                                                    }
                                                    else
                                                    {
                                                        ProyectoAportante pa = new ProyectoAportante
                                                        {
                                                            AportanteId = cpa.CofinanciacionAportanteId ?? 0,
                                                            ProyectoId = balanceFinanciero.ProyectoId,
                                                            ValorInterventoriaNuevo = valorUsoNuevo,
                                                            ValorInterventoriaActual = valorUsoActual,
                                                            ValorObraNuevo = 0,
                                                            ValorObraActual = 0
                                                        };

                                                        proyectoAportantesData.Add(pa);
                                                    }
                                                }
                                                else
                                                {
                                                    int index = proyectoAportantesData.FindIndex(item => item.AportanteId == cpa.CofinanciacionAportanteId);
                                                    if (index > -1)
                                                    {
                                                        proyectoAportantesData[index].ValorObraNuevo += valorUsoNuevo;
                                                        proyectoAportantesData[index].ValorObraActual += valorUsoActual;

                                                    }
                                                    else
                                                    {
                                                        ProyectoAportante pa = new ProyectoAportante
                                                        {
                                                            AportanteId = cpa.CofinanciacionAportanteId ?? 0,
                                                            ProyectoId = balanceFinanciero.ProyectoId,
                                                            ValorObraNuevo = valorUsoNuevo,
                                                            ValorObraActual = valorUsoActual,
                                                            ValorInterventoriaNuevo = 0,
                                                            ValorInterventoriaActual = 0,
                                                        };

                                                        proyectoAportantesData.Add(pa);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                DisponibilidadPresupuestalHistorico disponibilidadPresupuestalHistorico = null;
                                disponibilidadPresupuestalHistorico = _context.DisponibilidadPresupuestalHistorico.Where(r => r.DisponibilidadPresupuestalId == drp.DisponibilidadPresupuestalId).FirstOrDefault();

                                if (disponibilidadPresupuestalHistorico == null)
                                {
                                    //crear el registro histórico del ddp, con el valor actual del drp
                                    disponibilidadPresupuestalHistorico = new DisponibilidadPresupuestalHistorico
                                    {
                                        FechaCreacion = DateTime.Now,
                                        UsuarioCreacion = user,
                                        DisponibilidadPresupuestalId = drp.DisponibilidadPresupuestalId,
                                        ValorSolicitud = valorDrpActual
                                    };
                                    _context.DisponibilidadPresupuestalHistorico.Add(disponibilidadPresupuestalHistorico);
                                }

                                //actualizo el drp con el nuevo valor de los usos
                                await _context.Set<DisponibilidadPresupuestal>()
                                                .Where(r => r.DisponibilidadPresupuestalId == drp.DisponibilidadPresupuestalId)
                                                .UpdateAsync(r => new DisponibilidadPresupuestal()
                                                {
                                                    FechaModificacion = DateTime.Now,
                                                    UsuarioModificacion = user,
                                                    ValorSolicitud = valorDrpActual - valorALiberarTotal
                                                });

                                //crear el registro histórico de gestion fuente financiación, con el valor actual del gff
                                List<ContratacionProyectoAportante> cpaList = new List<ContratacionProyectoAportante>();
                                if (dpps.Count() > 0)
                                    cpaList = _context.ContratacionProyectoAportante.Where(r => r.ContratacionProyectoId == cp.ContratacionProyectoId && r.Eliminado != true).ToList();

                                foreach (var cppa in cpaList)
                                {
                                    decimal ValorUsoNuevo = _context.VSaldoAliberar.Where(r => r.ProyectoId == balanceFinanciero.ProyectoId && r.ContratacionId == cp.ContratacionId && r.EsNovedad != true && r.DisponibilidadPresupuestalId == drp.DisponibilidadPresupuestalId && r.CofinanciacionAportanteId == cppa.CofinanciacionAportanteId).ToList().Sum(r => r.ValorUso ?? 0);
                                    decimal ValorALiberarNuevo = _context.VSaldoAliberar.Where(r => r.ProyectoId == balanceFinanciero.ProyectoId && r.ContratacionId == cp.ContratacionId && r.EsNovedad != true && r.DisponibilidadPresupuestalId == drp.DisponibilidadPresupuestalId && r.CofinanciacionAportanteId == cppa.CofinanciacionAportanteId).ToList().Sum(r => r.ValorLiberar ?? 0);
                                    decimal ValorAporteNuevo = ValorUsoNuevo - ValorALiberarNuevo;

                                    decimal ValorAporteActual = cppa.ValorAporte;

                                    await _context.Set<ContratacionProyectoAportante>()
                                                .Where(r => r.ContratacionProyectoAportanteId == cppa.ContratacionProyectoAportanteId)
                                                .UpdateAsync(r => new ContratacionProyectoAportante()
                                                {
                                                    FechaModificacion = DateTime.Now,
                                                    UsuarioModificacion = user,
                                                    ValorAporte = ValorAporteNuevo
                                                });
                                    //creo un histórico con el valor actual
                                    ContratacionProyectoAportanteHistorico contratacionProyectoAportanteHistorico = new ContratacionProyectoAportanteHistorico
                                    {
                                        FechaCreacion = DateTime.Now,
                                        UsuarioCreacion = user,
                                        ValorAporte = ValorAporteActual,
                                        ContratacionProyectoAportanteId = cppa.ContratacionProyectoAportanteId
                                    };
                                    _context.ContratacionProyectoAportanteHistorico.Add(contratacionProyectoAportanteHistorico);

                                }

                                //actualizar fuentes de financiacion con el nuevo valor

                                //Actualizar gestiones fuentes de financiación fuente del componente Uso
                                //crear el registro histórico de gestion fuente financiación, con el valor actual del gff
                                List<GestionFuenteFinanciacion> gffList = new List<GestionFuenteFinanciacion>();

                                foreach (var dpp in dpps)
                                {
                                    gffList.AddRange(_context.GestionFuenteFinanciacion.Where(r => r.DisponibilidadPresupuestalProyectoId == dpp.DisponibilidadPresupuestalProyectoId && r.EsNovedad != true && r.Eliminado != true).ToList());

                                }

                                foreach (var gff in gffList)
                                {
                                    List<VSaldoAliberar> usosF = _context.VSaldoAliberar.Where(r => r.ProyectoId == balanceFinanciero.ProyectoId && r.ContratacionId == cp.ContratacionId && r.EsNovedad != true && r.DisponibilidadPresupuestalId == drp.DisponibilidadPresupuestalId && r.FuenteFinanciacionId == gff.FuenteFinanciacionId).ToList();


                                    decimal A_SaldoActual = gff.SaldoActual;
                                    decimal A_ValorSolicitado = gff.ValorSolicitado;
                                    decimal A_NuevoSaldo = gff.NuevoSaldo;

                                    decimal ValorUsoXFuenteA = 0;
                                    decimal ValorUsoXFuenteN = 0;
                                    decimal N_SaldoActual = 0;

                                    foreach (var uso in usosF)
                                    {
                                        ComponenteUso cu = _context.ComponenteUso.Find(uso.ComponenteUsoId);
                                        //obtengo componente Aportante
                                        ComponenteAportante componenteAportante = _context.ComponenteAportante.Find(cu.ComponenteAportanteId);
                                        if (componenteAportante != null)
                                        {
                                            ContratacionProyectoAportante cpaa = _context.ContratacionProyectoAportante.Find(componenteAportante.ContratacionProyectoAportanteId);
                                            if (cpaa != null)
                                            {
                                                N_SaldoActual = _context.VSaldosFuenteXaportanteId.Where(r => r.CofinanciacionAportanteId == cpaa.CofinanciacionAportanteId).FirstOrDefault().SaldoActual ?? 0;
                                            }
                                        }
                                        if (cu != null)
                                        {
                                            ComponenteUsoHistorico cuh = _context.ComponenteUsoHistorico.Where(r => r.ComponenteUsoId == cu.ComponenteUsoId).FirstOrDefault();
                                            if (cu.FuenteFinanciacionId == gff.FuenteFinanciacionId)
                                            {
                                                ValorUsoXFuenteA += cu.ValorUso;
                                                ValorUsoXFuenteN += cuh.ValorUso;
                                            }
                                        }

                                    }

                                    await _context.Set<GestionFuenteFinanciacion>()
                                                    .Where(r => r.GestionFuenteFinanciacionId == gff.GestionFuenteFinanciacionId)
                                                    .UpdateAsync(r => new GestionFuenteFinanciacion()
                                                    {
                                                        FechaModificacion = DateTime.Now,
                                                        UsuarioModificacion = user,
                                                        SaldoActual = N_SaldoActual,
                                                        ValorSolicitado = ValorUsoXFuenteN,
                                                        NuevoSaldo = N_SaldoActual + (ValorUsoXFuenteA - ValorUsoXFuenteN),
                                                        ValorLiberado = ValorUsoXFuenteA - ValorUsoXFuenteN
                                                    });

                                    GestionFuenteFinanciacionHistorico gestionFuenteFinanciacionHistorico = new GestionFuenteFinanciacionHistorico
                                    {
                                        FechaCreacion = DateTime.Now,
                                        UsuarioCreacion = user,
                                        GestionFuenteFinanciacionId = gff.GestionFuenteFinanciacionId,
                                        SaldoActual = A_SaldoActual,
                                        ValorSolicitado = A_ValorSolicitado,
                                        NuevoSaldo = A_NuevoSaldo
                                    };

                                    _context.GestionFuenteFinanciacionHistorico.Add(gestionFuenteFinanciacionHistorico);

                                }

                            }
                            #endregion
                            #region novedades

                            List<NovedadContractualRegistroPresupuestal> nrps = new List<NovedadContractualRegistroPresupuestal>();

                            if (drp != null)
                            {
                                List<VSaldoAliberar> listNView = _context.VSaldoAliberar.Where(r => r.ProyectoId == balanceFinanciero.ProyectoId && r.ContratacionId == cp.ContratacionId && r.EsNovedad == true).ToList();
                                foreach (var i in listNView)
                                {
                                    if (i.NovedadContractualRegistroPresupuestalId > 0)
                                    {
                                        if (!nrps.Any(r => r.NovedadContractualRegistroPresupuestalId == i.NovedadContractualRegistroPresupuestalId))
                                        {
                                            NovedadContractualRegistroPresupuestal n = _context.NovedadContractualRegistroPresupuestal.Find(i.NovedadContractualRegistroPresupuestalId);
                                            nrps.Add(n);
                                        }
                                    }
                                }
                            }
                            foreach (var nrp in nrps)
                            {
                                List<VSaldoAliberar> usosN = _context.VSaldoAliberar.Where(r => r.ProyectoId == balanceFinanciero.ProyectoId && r.ContratacionId == cp.ContratacionId && r.EsNovedad == true && r.NovedadContractualRegistroPresupuestalId == nrp.NovedadContractualRegistroPresupuestalId).ToList();
                                
                                if (nrp != null)
                                {
                                    decimal valorDrpNuevoN = 0;
                                    decimal valorDrpActualN = nrp != null ? nrp.ValorSolicitud : 0;
                                    decimal valorALiberarTotalN = 0;

                                    foreach (var uso in usosN)
                                    {
                                        if (uso.EsNovedad == true)
                                        {

                                            ComponenteUsoNovedad componenteUsoNovedad = _context.ComponenteUsoNovedad.Find(uso.ComponenteUsoNovedadId);
                                            ComponenteUsoNovedadHistorico componenteUsoNovedadHistorico = _context.ComponenteUsoNovedadHistorico.Find(uso.ComponenteUsoNovedadHistoricoId);
                                            decimal valorUsoActual = componenteUsoNovedad.ValorUso;
                                            decimal valorUsoNuevo = componenteUsoNovedadHistorico.ValorUso;
                                            valorDrpNuevoN += valorUsoNuevo;
                                            valorALiberarTotalN += (valorUsoActual - valorUsoNuevo);

                                            await _context.Set<ComponenteUsoNovedadHistorico>()
                                                                .Where(r => r.ComponenteUsoNovedadHistoricoId == uso.ComponenteUsoNovedadHistoricoId)
                                                                .UpdateAsync(r => new ComponenteUsoNovedadHistorico()
                                                                {
                                                                    FechaModificacion = DateTime.Now,
                                                                    UsuarioModificacion = user,
                                                                    Liberado = true,
                                                                    ValorUso = valorUsoActual
                                                                });

                                            await _context.Set<ComponenteUsoNovedad>()
                                                                .Where(r => r.ComponenteUsoNovedadId == uso.ComponenteUsoNovedadId)
                                                                .UpdateAsync(r => new ComponenteUsoNovedad()
                                                                {
                                                                    FechaModificacion = DateTime.Now,
                                                                    UsuarioModificacion = user,
                                                                    ValorUso = valorUsoNuevo
                                                                });
                                        }
                                    }

                                    //actualizar fuentes de financiacion con el nuevo valor

                                    //Actualizar gestiones fuentes de financiación fuente del componente Uso
                                    //crear el registro histórico de gestion fuente financiación, con el valor actual del gff
                                    List<GestionFuenteFinanciacion> gffList = new List<GestionFuenteFinanciacion>();
                                    if (nrp != null)
                                        gffList = _context.GestionFuenteFinanciacion.Where(r => r.NovedadContractualRegistroPresupuestalId == nrp.NovedadContractualRegistroPresupuestalId && r.EsNovedad == true && r.Eliminado != true ).ToList();

                                    foreach (var gff in gffList)
                                    {
                                        List<VSaldoAliberar> usosF = _context.VSaldoAliberar.Where(r => r.ProyectoId == balanceFinanciero.ProyectoId && r.ContratacionId == cp.ContratacionId && r.EsNovedad == true && r.NovedadContractualRegistroPresupuestalId == nrp.NovedadContractualRegistroPresupuestalId && r.FuenteFinanciacionId == gff.FuenteFinanciacionId).ToList();


                                        decimal A_SaldoActual = gff.SaldoActual;
                                        decimal A_ValorSolicitado = gff.ValorSolicitado;
                                        decimal A_NuevoSaldo = gff.NuevoSaldo;

                                        decimal ValorUsoXFuenteA = 0;
                                        decimal ValorUsoXFuenteN = 0;
                                        decimal N_SaldoActual = 0;

                                        foreach (var uso in usosF)
                                        {
                                            N_SaldoActual = _context.VSaldosFuenteXaportanteId.Where(r => r.CofinanciacionAportanteId == uso.CofinanciacionAportanteId).FirstOrDefault().SaldoActual ?? 0;
                                            ComponenteUsoNovedad cu = _context.ComponenteUsoNovedad.Find(uso.ComponenteUsoNovedadId);
                                            if (cu != null)
                                            {
                                                ComponenteUsoNovedadHistorico cuh = _context.ComponenteUsoNovedadHistorico.Where(r => r.ComponenteUsoNovedadId == cu.ComponenteUsoNovedadId).FirstOrDefault();
                                                if (uso.FuenteFinanciacionId == gff.FuenteFinanciacionId)
                                                {
                                                    ValorUsoXFuenteA += cu.ValorUso;
                                                    ValorUsoXFuenteN += cuh.ValorUso;
                                                }
                                            }

                                        }
                                        if (usosF.Count() > 0)
                                        {
                                            await _context.Set<GestionFuenteFinanciacion>()
                                                            .Where(r => r.GestionFuenteFinanciacionId == gff.GestionFuenteFinanciacionId)
                                                            .UpdateAsync(r => new GestionFuenteFinanciacion()
                                                            {
                                                                FechaModificacion = DateTime.Now,
                                                                UsuarioModificacion = user,
                                                                SaldoActual = N_SaldoActual,
                                                                ValorSolicitado = ValorUsoXFuenteN,
                                                                NuevoSaldo = N_SaldoActual + (ValorUsoXFuenteA - ValorUsoXFuenteN),
                                                                ValorLiberado = ValorUsoXFuenteA - ValorUsoXFuenteN
                                                            });
                                            GestionFuenteFinanciacionHistorico gestionFuenteFinanciacionHistorico = new GestionFuenteFinanciacionHistorico
                                            {
                                                FechaCreacion = DateTime.Now,
                                                UsuarioCreacion = user,
                                                GestionFuenteFinanciacionId = gff.GestionFuenteFinanciacionId,
                                                SaldoActual = A_SaldoActual,
                                                ValorSolicitado = A_ValorSolicitado,
                                                NuevoSaldo = A_NuevoSaldo
                                            };

                                            _context.GestionFuenteFinanciacionHistorico.Add(gestionFuenteFinanciacionHistorico);

                                        }

                                    }
                                    NovedadContractualRegistroPresupuestalHistorico novedadContractualRegistroPresupuestalHistorico = null;
                                    novedadContractualRegistroPresupuestalHistorico = _context.NovedadContractualRegistroPresupuestalHistorico.Where(r => r.NovedadContractualRegistroPresupuestalId == nrp.NovedadContractualRegistroPresupuestalId).FirstOrDefault();
                                    
                                    if (novedadContractualRegistroPresupuestalHistorico == null)
                                    {
                                        //crear el registro histórico del ddp, con el valor actual del drp
                                        novedadContractualRegistroPresupuestalHistorico = new NovedadContractualRegistroPresupuestalHistorico
                                        {
                                            FechaCreacion = DateTime.Now,
                                            UsuarioCreacion = user,
                                            NovedadContractualRegistroPresupuestalId = nrp.NovedadContractualRegistroPresupuestalId,
                                            ValorSolicitud = valorDrpActualN
                                        };
                                        _context.NovedadContractualRegistroPresupuestalHistorico.Add(novedadContractualRegistroPresupuestalHistorico);

                                    }

                                    //actualizo el drp con el nuevo valor de los usos
                                    await _context.Set<NovedadContractualRegistroPresupuestal>()
                                                    .Where(r => r.NovedadContractualRegistroPresupuestalId == nrp.NovedadContractualRegistroPresupuestalId)
                                                    .UpdateAsync(r => new NovedadContractualRegistroPresupuestal()
                                                    {
                                                        FechaModificacion = DateTime.Now,
                                                        UsuarioModificacion = user,
                                                        ValorSolicitud = valorDrpActualN - valorALiberarTotalN
                                                    });
                                    //obtengo componente Aportante
                                    //crear el registro histórico de gestion fuente financiación, con el valor actual del gff
                                    List<NovedadContractualAportante> ncaList = new List<NovedadContractualAportante>();
                                    if (drp != null)
                                        ncaList = _context.NovedadContractualAportante.Where(r => r.NovedadContractualId == nrp.NovedadContractualId && r.Eliminado != true).ToList();

                                    foreach (var cppa in ncaList)
                                    {
                                        decimal ValorUsoNuevo = _context.VSaldoAliberar.Where(r => r.ProyectoId == balanceFinanciero.ProyectoId && r.ContratacionId == cp.ContratacionId && r.EsNovedad == true && r.NovedadContractualRegistroPresupuestalId == nrp.NovedadContractualRegistroPresupuestalId && r.CofinanciacionAportanteId == cppa.CofinanciacionAportanteId).ToList().Sum(r => r.ValorUso ?? 0);
                                        decimal ValorALiberarNuevo = _context.VSaldoAliberar.Where(r => r.ProyectoId == balanceFinanciero.ProyectoId && r.ContratacionId == cp.ContratacionId && r.EsNovedad == true && r.NovedadContractualRegistroPresupuestalId == nrp.NovedadContractualRegistroPresupuestalId && r.CofinanciacionAportanteId == cppa.CofinanciacionAportanteId).ToList().Sum(r => r.ValorLiberar ?? 0);
                                        decimal ValorAporteNuevo = ValorUsoNuevo - ValorALiberarNuevo;

                                        decimal ValorAporteActual = cppa.ValorAporte ?? 0;

                                        await _context.Set<NovedadContractualAportante>()
                                                    .Where(r => r.NovedadContractualAportanteId == cppa.NovedadContractualAportanteId)
                                                    .UpdateAsync(r => new NovedadContractualAportante()
                                                    {
                                                        FechaModificacion = DateTime.Now,
                                                        UsuarioModificacion = user,
                                                        ValorAporte = ValorAporteNuevo
                                                    });
                                        //creo un histórico con el valor actual
                                        NovedadContractualAportanteHistorico novedadContractualAportanteHistorico = new NovedadContractualAportanteHistorico
                                        {
                                            FechaCreacion = DateTime.Now,
                                            UsuarioCreacion = user,
                                            ValorAporte = ValorAporteActual,
                                            NovedadContractualAportanteId = cppa.NovedadContractualAportanteId
                                        };
                                        _context.NovedadContractualAportanteHistorico.Add(novedadContractualAportanteHistorico);

                                    }
                                }
                            }

                            #endregion
                        }
                        //actualizo proyecto aportantes con la lista que llene arriba 
                        //obtengo los poryectos
                        foreach (var pad in proyectoAportantesData)
                        {
                            //actualizo el drp con el nuevo valor de los usos
                            ProyectoAportante pa = _context.ProyectoAportante.Where(r => r.ProyectoId == balanceFinanciero.ProyectoId && r.AportanteId == pad.AportanteId && r.Eliminado != true).FirstOrDefault();
                            if (pa != null)
                            {
                                //actualizo el regitro con los valores nuevos
                                await _context.Set<ProyectoAportante>()
                                    .Where(r => r.ProyectoId == pa.ProyectoId && r.AportanteId == pa.AportanteId)
                                    .UpdateAsync(r => new ProyectoAportante()
                                    {
                                        FechaModificacion = DateTime.Now,
                                        UsuarioModificacion = user,
                                        ValorObra = pad.ValorObraNuevo,
                                        ValorInterventoria = pad.ValorInterventoriaNuevo,
                                        ValorTotalAportante = pad.ValorObraNuevo + pad.ValorInterventoriaNuevo
                                    });
                                //creo un histórico con los valores actuales
                                ProyectoAportanteHistorico proyectoAportanteHistorico = new ProyectoAportanteHistorico
                                {
                                    FechaCreacion = DateTime.Now,
                                    UsuarioCreacion = user,
                                    ProyectoAportanteId = pa.ProyectoAportanteId,
                                    ValorObra = pad.ValorObraActual,
                                    ValorInterventoria = pad.ValorInterventoriaActual,
                                    ValorTotalAportante = pad.ValorObraActual + pad.ValorInterventoriaActual
                                };

                                _context.ProyectoAportanteHistorico.Add(proyectoAportanteHistorico);

                            }

                        }
                    }
                    //cambio el estado del balance
                    _context.Set<BalanceFinanciero>()
                                .Where(b => b.BalanceFinancieroId == pBalanceFinancieroId)
                                .Update(b => new BalanceFinanciero
                                {
                                    EstadoBalanceCodigo = ConstanCodigoEstadoBalanceFinanciero.Con_balance_validado,
                                    UsuarioModificacion = user,
                                    FechaModificacion = DateTime.Now
                                });
                }
                _context.SaveChanges();

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_balance_financiero_traslados_de_recursos, GeneralCodes.OperacionExitosa, idAccion, user, strCrearEditar)
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_balance_financiero_traslados_de_recursos, GeneralCodes.Error, idAccion, user, ex.InnerException.ToString())
                    };
            }
        }

        public async Task<Respuesta> DeleteReleaseBalance(int pBalanceFinancieroId, string user)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Liberar_saldo, (int)EnumeratorTipoDominio.Acciones);
            Respuesta respuesta = new Respuesta();
            try
            {
                string strCrearEditar = "LIBERAR SALDO - BORRAR";
                if (pBalanceFinancieroId != 0)
                {
                    BalanceFinanciero balanceFinanciero = _context.BalanceFinanciero.Find(pBalanceFinancieroId);
                    if (balanceFinanciero != null)
                    {
                        DisponibilidadPresupuestal drp = null;
                        DisponibilidadPresupuestalProyecto dpp = null;
                        List<ContratacionProyecto> contratacionProyectos = _context.ContratacionProyecto.Where(r => r.ProyectoId == balanceFinanciero.ProyectoId && r.Eliminado != true).ToList();
                        List<ProyectoAportante> proyectoAportantesData = new List<ProyectoAportante>();

                        foreach (var cp in contratacionProyectos)
                        {
                            List<VTablasHistoricoLiberacion> usos = _context.VTablasHistoricoLiberacion.Where(r => r.ProyectoId == balanceFinanciero.ProyectoId && r.ContratacionId == cp.ContratacionId).ToList();
                            
                            if (usos.FirstOrDefault()?.DisponibilidadPresupuestalId > 0)
                            {
                                drp = _context.DisponibilidadPresupuestal.Find(usos.FirstOrDefault().DisponibilidadPresupuestalId);
                                dpp = _context.DisponibilidadPresupuestalProyecto.Where(r => r.DisponibilidadPresupuestalId == usos.FirstOrDefault().DisponibilidadPresupuestalId && r.ProyectoId == balanceFinanciero.ProyectoId && r.Eliminado != true).FirstOrDefault();
                            }

                            foreach (var uso in usos)
                            {
                                if (uso.EsNovedad != true)
                                {
                                    //Acá se hace el cambio, los valores que estban en el histórico pasan a la tabla principal y en el histórico queda el 
                                    //valor del uso original
                                    await _context.Set<ComponenteUsoHistorico>()
                                                        .Where(r => r.ComponenteUsoHistoricoId == uso.ComponenteUsoHistoricoId)
                                                        .UpdateAsync(r => new ComponenteUsoHistorico()
                                                        {
                                                            FechaModificacion = DateTime.Now,
                                                            UsuarioModificacion = user,
                                                            Liberado = false,
                                                            ValorUso = uso.ValorHistorico
                                                        });


                                    await _context.Set<ComponenteUso>()
                                                        .Where(r => r.ComponenteUsoId == uso.ComponenteUsoId)
                                                        .UpdateAsync(r => new ComponenteUso()
                                                        {
                                                            FechaModificacion = DateTime.Now,
                                                            UsuarioModificacion = user,
                                                            ValorUso = uso.ValorOriginal
                                                        });
                                }
                                else
                                {
                                    await _context.Set<ComponenteUsoNovedadHistorico>()
                                                        .Where(r => r.ComponenteUsoNovedadHistoricoId == uso.ComponenteUsoNovedadHistoricoId)
                                                        .UpdateAsync(r => new ComponenteUsoNovedadHistorico()
                                                        {
                                                            FechaModificacion = DateTime.Now,
                                                            UsuarioModificacion = user,
                                                            Liberado = false,
                                                            ValorUso = uso.ValorHistorico
                                                        });


                                    await _context.Set<ComponenteUsoNovedad>()
                                                        .Where(r => r.ComponenteUsoNovedadId == uso.ComponenteUsoNovedadId)
                                                        .UpdateAsync(r => new ComponenteUsoNovedad()
                                                        {
                                                            FechaModificacion = DateTime.Now,
                                                            UsuarioModificacion = user,
                                                            ValorUso = uso.ValorOriginal
                                                        });
                                }
                            }

                            if (drp != null) {

                                //actualizo el drp con el nuevo valor de los usos

                                DisponibilidadPresupuestalHistorico dph = _context.DisponibilidadPresupuestalHistorico.Where(r => r.DisponibilidadPresupuestalId == drp.DisponibilidadPresupuestalId).FirstOrDefault();
                                if (dph != null)
                                {
                                    await _context.Set<DisponibilidadPresupuestal>()
                                                    .Where(r => r.DisponibilidadPresupuestalId == dph.DisponibilidadPresupuestalId)
                                                    .UpdateAsync(r => new DisponibilidadPresupuestal()
                                                    {
                                                        FechaModificacion = DateTime.Now,
                                                        UsuarioModificacion = user,
                                                        ValorSolicitud = dph.ValorSolicitud
                                                    });

                                    _context.DisponibilidadPresupuestalHistorico.Remove(dph);
                                }

                            }

                            #region novedades

                            List<NovedadContractualRegistroPresupuestal> nrps = new List<NovedadContractualRegistroPresupuestal>();

                            if (drp != null)
                            {
                                nrps = _context.NovedadContractualRegistroPresupuestal.Where(r => r.DisponibilidadPresupuestalId == drp.DisponibilidadPresupuestalId && r.Eliminado != true && (r.EstadoSolicitudCodigo == "5" || r.EstadoSolicitudCodigo == "8")).ToList();
                            }

                            foreach (var nrp in nrps)
                            {
                                if (nrp != null)
                                {
                                    List<VTablasHistoricoLiberacion> usosN = _context.VTablasHistoricoLiberacion.Where(r => r.ProyectoId == balanceFinanciero.ProyectoId && r.ContratacionId == cp.ContratacionId && r.EsNovedad == true && r.NovedadContractualRegistroPresupuestalId == nrp.NovedadContractualRegistroPresupuestalId).ToList();

                                    List<GestionFuenteFinanciacion> gffListN = new List<GestionFuenteFinanciacion>();
                                    if (nrp != null)
                                        gffListN = _context.GestionFuenteFinanciacion.Where(r => r.NovedadContractualRegistroPresupuestalId == nrp.NovedadContractualRegistroPresupuestalId && r.EsNovedad == true && r.Eliminado != true).ToList();

                                    List<GestionFuenteFinanciacion> gffListn = new List<GestionFuenteFinanciacion>();
                                    if (dpp != null)
                                        gffListn = _context.GestionFuenteFinanciacion.Where(r => r.NovedadContractualRegistroPresupuestalId == nrp.NovedadContractualRegistroPresupuestalId && r.EsNovedad == true && r.Eliminado != true).ToList();

                                    foreach (var gff in gffListn)
                                    {
                                        List<VTablasHistoricoLiberacion> usosF = _context.VTablasHistoricoLiberacion.Where(r => r.ProyectoId == balanceFinanciero.ProyectoId && r.ContratacionId == cp.ContratacionId && r.EsNovedad == true && r.NovedadContractualRegistroPresupuestalId == nrp.NovedadContractualRegistroPresupuestalId && r.FuenteFinanciacionId == gff.FuenteFinanciacionId).ToList();

                                        foreach (var uso in usosF)
                                        {
                                            GestionFuenteFinanciacionHistorico gffh = _context.GestionFuenteFinanciacionHistorico.Find(uso.GestionFuenteFinanciacionHistoricoId);
                                            if (gffh != null)
                                            {
                                                await _context.Set<GestionFuenteFinanciacion>()
                                                            .Where(r => r.GestionFuenteFinanciacionId == gff.GestionFuenteFinanciacionId)
                                                            .UpdateAsync(r => new GestionFuenteFinanciacion()
                                                            {
                                                                FechaModificacion = DateTime.Now,
                                                                UsuarioModificacion = user,
                                                                SaldoActual = uso.SaldoActual,
                                                                ValorSolicitado = uso.ValorSolicitado,
                                                                NuevoSaldo = uso.NuevoSaldo,
                                                                ValorLiberado = 0
                                                            });

                                                _context.GestionFuenteFinanciacionHistorico.Remove(gffh);

                                            }

                                        }

                                    }

                                    NovedadContractualRegistroPresupuestalHistorico nrph = _context.NovedadContractualRegistroPresupuestalHistorico.Where(r => r.NovedadContractualRegistroPresupuestalId == nrp.NovedadContractualRegistroPresupuestalId).FirstOrDefault();
                                        
                                    if (nrph != null)
                                    {
                                        await _context.Set<NovedadContractualRegistroPresupuestal>()
                                                        .Where(r => r.NovedadContractualRegistroPresupuestalId == nrph.NovedadContractualRegistroPresupuestalId)
                                                        .UpdateAsync(r => new NovedadContractualRegistroPresupuestal()
                                                        {
                                                            FechaModificacion = DateTime.Now,
                                                            UsuarioModificacion = user,
                                                            ValorSolicitud = nrph.ValorSolicitud
                                                        });

                                        _context.NovedadContractualRegistroPresupuestalHistorico.Remove(nrph);
                                    }

                                    //obtengo componente Aportante
                                    //crear el registro histórico de gestion fuente financiación, con el valor actual del gff
                                    List<NovedadContractualAportante> ncaList = new List<NovedadContractualAportante>();
                                    if (dpp != null)
                                        ncaList = _context.NovedadContractualAportante.Where(r => r.NovedadContractualId == nrp.NovedadContractualId && r.Eliminado != true).ToList();

                                    foreach (var cppa in ncaList)
                                    {
                                        VTablasHistoricoLiberacion aporte = _context.VTablasHistoricoLiberacion.Where(r => r.ProyectoId == balanceFinanciero.ProyectoId && r.ContratacionId == cp.ContratacionId && r.EsNovedad == true && r.NovedadContractualRegistroPresupuestalId == nrp.NovedadContractualRegistroPresupuestalId).FirstOrDefault();
                                        
                                        if (aporte != null)
                                        {
                                            NovedadContractualAportanteHistorico npah = _context.NovedadContractualAportanteHistorico.Find(aporte.NovedadContractualAportanteHistoricoId);
                                            if (npah != null)
                                            {
                                                await _context.Set<NovedadContractualAportante>()
                                                                .Where(r => r.NovedadContractualAportanteId == npah.NovedadContractualAportanteId)
                                                                .UpdateAsync(r => new NovedadContractualAportante()
                                                                {
                                                                    FechaModificacion = DateTime.Now,
                                                                    UsuarioModificacion = user,
                                                                    ValorAporte = npah.ValorAporte
                                                                });

                                                _context.NovedadContractualAportanteHistorico.Remove(npah);
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion


                            List<ContratacionProyectoAportante> cpaList = new List<ContratacionProyectoAportante>();
                            if (dpp != null)
                                cpaList = _context.ContratacionProyectoAportante.Where(r => r.ContratacionProyectoId == cp.ContratacionProyectoId && r.Eliminado != true).ToList();

                            foreach (var cppa in cpaList)
                            {
                                VTablasHistoricoLiberacion aporte = _context.VTablasHistoricoLiberacion.Where(r => r.ProyectoId == balanceFinanciero.ProyectoId && r.ContratacionId == cp.ContratacionId && r.EsNovedad != true && r.DisponibilidadPresupuestalId == drp.DisponibilidadPresupuestalId && r.ContratacionProyectoAportanteId == cppa.ContratacionProyectoAportanteId).FirstOrDefault();
                                if (aporte != null)
                                {
                                    ContratacionProyectoAportanteHistorico cppah = _context.ContratacionProyectoAportanteHistorico.Find(aporte.ContratacionProyectoAportanteHistoricoId);
                                    if (cppah != null)
                                    {
                                        await _context.Set<ContratacionProyectoAportante>()
                                                        .Where(r => r.ContratacionProyectoAportanteId == cppa.ContratacionProyectoAportanteId)
                                                        .UpdateAsync(r => new ContratacionProyectoAportante()
                                                        {
                                                            FechaModificacion = DateTime.Now,
                                                            UsuarioModificacion = user,
                                                            ValorAporte = aporte.ValorAporte ?? 0
                                                        });

                                        _context.ContratacionProyectoAportanteHistorico.Remove(cppah);
                                    }

                                }
                            }

                            List<GestionFuenteFinanciacion> gffList = new List<GestionFuenteFinanciacion>();
                            if (dpp != null)
                                gffList = _context.GestionFuenteFinanciacion.Where(r => r.DisponibilidadPresupuestalProyectoId == dpp.DisponibilidadPresupuestalProyectoId && r.EsNovedad != true && r.Eliminado != true).ToList();

                            foreach (var gff in gffList)
                            {
                                List<VTablasHistoricoLiberacion> usosF = _context.VTablasHistoricoLiberacion.Where(r => r.ProyectoId == balanceFinanciero.ProyectoId && r.ContratacionId == cp.ContratacionId && r.EsNovedad != true && r.DisponibilidadPresupuestalId == drp.DisponibilidadPresupuestalId && r.FuenteFinanciacionId == gff.FuenteFinanciacionId).ToList();

                                foreach (var uso in usosF)
                                {
                                    GestionFuenteFinanciacionHistorico gffh = _context.GestionFuenteFinanciacionHistorico.Find(uso.GestionFuenteFinanciacionHistoricoId);
                                    if (gffh != null)
                                    {
                                        await _context.Set<GestionFuenteFinanciacion>()
                                                    .Where(r => r.GestionFuenteFinanciacionId == gff.GestionFuenteFinanciacionId)
                                                    .UpdateAsync(r => new GestionFuenteFinanciacion()
                                                    {
                                                        FechaModificacion = DateTime.Now,
                                                        UsuarioModificacion = user,
                                                        SaldoActual = uso.SaldoActual,
                                                        ValorSolicitado = uso.ValorSolicitado,
                                                        NuevoSaldo = uso.NuevoSaldo,
                                                        ValorLiberado = 0
                                                    });

                                        _context.GestionFuenteFinanciacionHistorico.Remove(gffh);

                                    }

                                }

                            }
                            //
                            List<VTablasHistoricoLiberacion> vpa = _context.VTablasHistoricoLiberacion.Where(r => r.ProyectoId == balanceFinanciero.ProyectoId && r.ContratacionId == cp.ContratacionId).ToList();
                            foreach (var pa in vpa)
                            {
                                ProyectoAportanteHistorico pah = _context.ProyectoAportanteHistorico.Find(pa.ProyectoAportanteHistoricoId);
                                if (pah != null)
                                {
                                    await _context.Set<ProyectoAportante>()
                                                    .Where(r => r.ProyectoAportanteId == pa.ProyectoAportanteId)
                                                    .UpdateAsync(r => new ProyectoAportante()
                                                    {
                                                        FechaModificacion = DateTime.Now,
                                                        UsuarioModificacion = user,
                                                        ValorObra = pa.ValorObra,
                                                        ValorInterventoria = pa.ValorInterventoria,
                                                        ValorTotalAportante = pa.ValorTotalAportante
                                                    });

                                    _context.ProyectoAportanteHistorico.Remove(pah);
                                }
                            }
                        }
                        
                    }
                    //cambio el estado del balance
                    _context.Set<BalanceFinanciero>()
                                .Where(b => b.BalanceFinancieroId == pBalanceFinancieroId)
                                .Update(b => new BalanceFinanciero
                                {
                                    EstadoBalanceCodigo = ConstanCodigoEstadoBalanceFinanciero.En_proceso_de_validacion,
                                    UsuarioModificacion = user,
                                    FechaModificacion = DateTime.Now
                                });
                }
                _context.SaveChanges();

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_balance_financiero_traslados_de_recursos, GeneralCodes.OperacionExitosa, idAccion, user, strCrearEditar)
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_balance_financiero_traslados_de_recursos, GeneralCodes.Error, idAccion, user, ex.InnerException.ToString())
                    };
            }
        }
    }
}
