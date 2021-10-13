﻿using asivamosffie.model.APIModels;
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
                                da.Saldo = valorActual;
                            }
                            else
                            {
                                ComponenteUsoHistorico cuh = _context.ComponenteUsoHistorico.Find(da.ComponenteUsoHistoricoId);
                                da.Saldo = cuh.Saldo;
                            }
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
                        DisponibilidadPresupuestalProyecto dpp = null;
                        List<ContratacionProyecto> contratacionProyectos = _context.ContratacionProyecto.Where(r => r.ProyectoId == balanceFinanciero.ProyectoId && r.Eliminado != true).ToList();
                        List<ProyectoAportante> proyectoAportantesData = new List<ProyectoAportante>();

                        foreach (var cp in contratacionProyectos)
                        {
                            List<VSaldoAliberar> usos = _context.VSaldoAliberar.Where(r => r.ProyectoId == balanceFinanciero.ProyectoId && r.ContratacionId == cp.ContratacionId).ToList();
                            if (usos.FirstOrDefault()?.DisponibilidadPresupuestalId != 0)
                            {
                                drp = _context.DisponibilidadPresupuestal.Find(usos.FirstOrDefault().DisponibilidadPresupuestalId);
                                dpp = _context.DisponibilidadPresupuestalProyecto.Where(r => r.DisponibilidadPresupuestalId == usos.FirstOrDefault().DisponibilidadPresupuestalId && r.ProyectoId == balanceFinanciero.ProyectoId).FirstOrDefault();
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

                                    GestionFuenteFinanciacion gff = null;
                                    if (dpp != null)
                                        gff = _context.GestionFuenteFinanciacion.Where(r => r.DisponibilidadPresupuestalProyectoId == dpp.DisponibilidadPresupuestalProyectoId && r.FuenteFinanciacionId == componenteUso.FuenteFinanciacionId).FirstOrDefault();

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
                                    //actualizar fuentes de financiacion con el nuevo valor

                                    //Actualizar gestiones fuentes de financiación fuente del componente Uso
                                    //crear el registro histórico de gestion fuente financiación, con el valor actual del gff
                                    if (gff != null)
                                    {
                                        //valores actuales gff

                                        decimal A_SaldoActual = gff.SaldoActual;
                                        decimal A_ValorSolicitado = gff.ValorSolicitado;
                                        decimal A_NuevoSaldo = gff.NuevoSaldo;

                                        //Valores nuevos gff

                                        decimal N_SaldoActual = A_SaldoActual;
                                        decimal N_ValorSolicitado = valorUsoNuevo;
                                        decimal N_NuevoSaldo = A_SaldoActual - valorUsoNuevo;

                                        await _context.Set<GestionFuenteFinanciacion>()
                                                        .Where(r => r.GestionFuenteFinanciacionId == gff.GestionFuenteFinanciacionId)
                                                        .UpdateAsync(r => new GestionFuenteFinanciacion()
                                                        {
                                                            FechaModificacion = DateTime.Now,
                                                            UsuarioModificacion = user,
                                                            SaldoActual = N_SaldoActual,
                                                            ValorSolicitado = N_ValorSolicitado,
                                                            NuevoSaldo = N_NuevoSaldo
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
                                                if (index != -1)
                                                {
                                                    proyectoAportantesData[index].ValorInterventoriaNuevo = valorUsoNuevo;
                                                    proyectoAportantesData[index].ValorInterventoriaActual = valorUsoActual;

                                                }
                                                else
                                                {
                                                    ProyectoAportante pa = new ProyectoAportante
                                                    {
                                                        AportanteId = cpa.CofinanciacionAportanteId ?? 0,
                                                        ProyectoId = balanceFinanciero.ProyectoId,
                                                        ValorInterventoriaNuevo = valorUsoNuevo,
                                                        ValorInterventoriaActual = valorUsoActual
                                                    };

                                                    proyectoAportantesData.Add(pa);
                                                }
                                            }
                                            else
                                            {
                                                int index = proyectoAportantesData.FindIndex(item => item.AportanteId == cpa.CofinanciacionAportanteId);
                                                if (index != -1)
                                                {
                                                    proyectoAportantesData[index].ValorObraNuevo = valorUsoNuevo;
                                                    proyectoAportantesData[index].ValorObraActual = valorUsoActual;

                                                }
                                                else
                                                {
                                                    ProyectoAportante pa = new ProyectoAportante
                                                    {
                                                        AportanteId = cpa.CofinanciacionAportanteId ?? 0,
                                                        ProyectoId = balanceFinanciero.ProyectoId,
                                                        ValorObraNuevo = valorUsoNuevo,
                                                        ValorObraActual = valorUsoActual
                                                    };

                                                    proyectoAportantesData.Add(pa);
                                                }
                                            }

                                            await _context.Set<ContratacionProyectoAportante>()
                                                        .Where(r => r.ContratacionProyectoAportanteId == cpa.ContratacionProyectoAportanteId)
                                                        .UpdateAsync(r => new ContratacionProyectoAportante()
                                                        {
                                                            FechaModificacion = DateTime.Now,
                                                            UsuarioModificacion = user,
                                                            ValorAporte = valorUsoNuevo
                                                        });
                                            //creo un histórico con el valor actual
                                            ContratacionProyectoAportanteHistorico contratacionProyectoAportanteHistorico = new ContratacionProyectoAportanteHistorico
                                            {
                                                FechaCreacion = DateTime.Now,
                                                UsuarioCreacion = user,
                                                ValorAporte = valorUsoActual,
                                                ContratacionProyectoAportanteId = cpa.ContratacionProyectoAportanteId
                                            };
                                            _context.ContratacionProyectoAportanteHistorico.Add(contratacionProyectoAportanteHistorico);
                                        }
                                    }
                                }
                                //crear el registro histórico del ddp, con el valor actual del drp
                                DisponibilidadPresupuestalHistorico disponibilidadPresupuestalHistorico = new DisponibilidadPresupuestalHistorico
                                {
                                    FechaCreacion = DateTime.Now,
                                    UsuarioCreacion = user,
                                    DisponibilidadPresupuestalId = drp.DisponibilidadPresupuestalId,
                                    ValorSolicitud = valorDrpActual
                                };
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
                        //actualizo proyecto aportantes con la lista que llene arriba 
                        //obtengo los poryectos
                        foreach (var pad in proyectoAportantesData)
                        {
                            //actualizo el drp con el nuevo valor de los usos
                            ProyectoAportante pa = _context.ProyectoAportante.Where(r => r.ProyectoId == balanceFinanciero.ProyectoId && r.AportanteId == pad.AportanteId).FirstOrDefault();
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
                                    EstadoBalanceCodigo = ConstanCodigoEstadoBalanceFinanciero.Con_balance_aprobado,
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
