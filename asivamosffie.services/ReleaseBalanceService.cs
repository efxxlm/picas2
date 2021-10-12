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
                        List<VSaldoAliberar> datosAportante = _context.VSaldoAliberar.Where(r => r.ProyectoId == pProyectoId && r.DisponibilidadPresupuestalId == drp.DisponibilidadPresupuestalId).ToList();
                        BalanceFinanciero balanceFinanciero = _context.BalanceFinanciero.Where(r => r.ProyectoId == pProyectoId).FirstOrDefault();
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


                                Decimal Total = Math.Abs(da.ValorUso - Descuento);

                                if (Total == 0)
                                    Total = da.ValorUso;

                                valorActual =  Total;
                            }
                            da.Saldo = valorActual;
                        });
                        drps.Add(new
                        {
                            NumeroDrp = drp.NumeroDrp,
                            DisponibilidadPresupuestalId = drp.DisponibilidadPresupuestalId,
                            ProyectoId = cp.ProyectoId,
                            ContratacionId = cp.ContratacionId,
                            AportantesGrid = datosAportante,
                            ValorSolicitud = drp.ValorSolicitud,
                            BalanceFinanciero = balanceFinanciero
                        });
                    }

                }
            }

            return drps;
        }

        private bool RegitroCompletoDrpsTai(int pProyectoId, bool esNovedad)
        {
            bool esCompleto = false;
            int totalRegistrosDrpALiberar = 0;
            int totalRegistrosDrp = _context.VSaldoAliberar.Where(r => r.ProyectoId == pProyectoId).Count();
            if (esNovedad != true)
            {
                totalRegistrosDrpALiberar = _context.VSaldoAliberar.Where(r => r.ProyectoId == pProyectoId && r.ComponenteUsoHistoricoId != null && r.ComponenteUsoHistoricoId > 0).Count();
            }

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
                        ComponenteUso componenteUso = _context.ComponenteUso.Find(usoHistorico.ComponenteUsoId);
                        if (componenteUso != null)
                        {
                            if (usoHistorico.EsNovedad != true)
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
                                                            ValorUso = componenteUso.ValorUso - usoHistorico.ValorLiberar
                                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                _context.SaveChanges();

                bool registroCompleto = RegitroCompletoDrpsTai(pUsosHistorico.ProyectoId, false);

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
                        List<VSaldoAliberar> usos = _context.VSaldoAliberar.Where(r => r.ProyectoId == balanceFinanciero.ProyectoId).ToList();
                        if (usos.FirstOrDefault()?.DisponibilidadPresupuestalId != 0)
                        {
                            drp = _context.DisponibilidadPresupuestal.Find(usos.FirstOrDefault().DisponibilidadPresupuestalId);
                        }

                        if (drp != null)
                        {
                            decimal valorDrpNuevo = 0;
                            decimal valorDrpActual = drp.ValorSolicitud;

                            foreach (var uso in usos)
                            {
                                //Acá se hace el cambio, los valores que estban en el histórico pasan a la tabla principal y en el histórico queda el 
                                //valor del uso original

                                ComponenteUso componenteUso = _context.ComponenteUso.Find(uso.ComponenteUsoId);
                                ComponenteUsoHistorico componenteUsoHistorico = _context.ComponenteUsoHistorico.Find(uso.ComponenteUsoHistoricoId);
                                decimal valorUsoActual = componenteUso.ValorUso;
                                decimal valorUsoNuevo = componenteUsoHistorico.ValorUso;
                                valorDrpNuevo += valorUsoNuevo;

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

                            }
                            //crear el registro histórico del ddp, con el valor actual del drp
                            DisponibilidadPresupuestalHistorico disponibilidadPresupuestalHistorico = new DisponibilidadPresupuestalHistorico();

                            disponibilidadPresupuestalHistorico.FechaCreacion = DateTime.Now;
                            disponibilidadPresupuestalHistorico.UsuarioCreacion = user;
                            disponibilidadPresupuestalHistorico.DisponibilidadPresupuestalId = drp.DisponibilidadPresupuestalId;
                            disponibilidadPresupuestalHistorico.ValorSolicitud = valorDrpActual;
                            _context.DisponibilidadPresupuestalHistorico.Add(disponibilidadPresupuestalHistorico);

                            //actualizo el drp con el nuevo valor de los usos
                            await _context.Set<DisponibilidadPresupuestal>()
                                            .Where(r => r.DisponibilidadPresupuestalId == drp.DisponibilidadPresupuestalId)
                                            .UpdateAsync(r => new DisponibilidadPresupuestal()
                                            {
                                                FechaModificacion = DateTime.Now,
                                                UsuarioModificacion = user,
                                                ValorSolicitud = valorDrpNuevo
                                            });
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

    }
}
