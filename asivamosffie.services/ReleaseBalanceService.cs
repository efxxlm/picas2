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


        public ReleaseBalanceService(devAsiVamosFFIEContext context, ICommonService commonService, IRequestBudgetAvailabilityService requestBudgetAvailabilityService)
        {
            _commonService = commonService;
            _context = context;
            _requestBudgetAvailabilityService = requestBudgetAvailabilityService;
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
                            ValorSolicitud = drp.ValorSolicitud
                        });
                    }

                }
            }

            return drps;
        }

        private bool VerificarInformeFinalAprobacion(int pInformeFinalId, bool? tieneOBservaciones)
        {
            bool esCompleto = false;

            InformeFinal informeFinal = _context.InformeFinal.Where(r => r.InformeFinalId == pInformeFinalId).FirstOrDefault();
            //Validación # 1
            if (informeFinal != null)
            {
                InformeFinalInterventoria existe_no_cumple = _context.InformeFinalInterventoria.Where(r => r.InformeFinalId == pInformeFinalId && r.AprobacionCodigo == ConstantCodigoCalificacionInformeFinal.No_Cumple).FirstOrDefault();
                InformeFinalInterventoria existe_no_diligenciado = _context.InformeFinalInterventoria.Where(r => r.InformeFinalId == pInformeFinalId && (r.AprobacionCodigo == "0" || String.IsNullOrEmpty(r.AprobacionCodigo))).FirstOrDefault();

                if (existe_no_diligenciado != null)
                {
                    return false;
                }
                if (existe_no_cumple != null)
                {
                    informeFinal.EstadoAprobacion = ConstantCodigoEstadoAprobacionInformeFinal.Con_observaciones_supervisor;
                    return false;
                }
                //validar si tiene observaciones al recibo de satisfaccion
                if (tieneOBservaciones == true && existe_no_diligenciado == null && existe_no_cumple == null)
                {
                    informeFinal.EstadoAprobacion = ConstantCodigoEstadoAprobacionInformeFinal.Con_observaciones_supervisor;
                    return false;
                }
            }
            else
            {
                return false;
            }
            informeFinal.EstadoAprobacion = ConstantCodigoEstadoAprobacionInformeFinal.Con_aprobacion_supervisor;

            //_context.SaveChanges();
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
