using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class RequestBudgetAvailabilityService : IRequestBudgetAvailabilityService
    {

        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly ISourceFundingService _sourceFundingService;
        private readonly string _connectionString;

        public RequestBudgetAvailabilityService(devAsiVamosFFIEContext context, ICommonService commonService, IConfiguration configuration, ISourceFundingService sourceFundingService)
        {
            _context = context;
            _commonService = commonService;
            _sourceFundingService = sourceFundingService;
            _connectionString = configuration.GetConnectionString("asivamosffieDatabase");
        }

        public async Task<List<DetailValidarDisponibilidadPresupuesal>> GetDetailAvailabilityBudgetProyectNew(int disponibilidadPresupuestalId, bool esNovedad, int RegistroNovedadId,bool esGenerar)
        {
            List<DetailValidarDisponibilidadPresupuesal> ListDetailValidarDisponibilidadPresupuesal = new List<DetailValidarDisponibilidadPresupuesal>();
            List<SesionComiteSolicitud> sesionComiteSolicitud = new List<SesionComiteSolicitud>();
            List<NovedadContractualAportante> novedadContractualAportantes = new List<NovedadContractualAportante>();

            if (esNovedad == true)
            {
                ListDetailValidarDisponibilidadPresupuesal = await GetDetailAvailabilityBudgetProyectNovelty(RegistroNovedadId, esGenerar,false);
            }
            else
            {
                DisponibilidadPresupuestal ListDP = await _context.DisponibilidadPresupuestal.
                    Where(r => r.Eliminado != true && r.DisponibilidadPresupuestalId == disponibilidadPresupuestalId).
                    //Include(x => x.DisponibilidadPresupuestalProyecto).
                    //Include(x => x.DisponibilidadPresupuestalObservacion).
                    FirstOrDefaultAsync();
                VDisponibilidadPresupuestal vdpp = null;
                if (ListDP != null)
                    ListDP.ValorTotalDisponibilidad = _commonService.GetValorTotalDisponibilidad(disponibilidadPresupuestalId,false);

                Contratacion contratacionDP = _context.Contratacion.Where(r => r.ContratacionId == ListDP.ContratacionId).Include(r => r.Contrato).FirstOrDefault();
                if (ListDP.EsNovedadContractual == true)
                {

                    novedadContractualAportantes = _context.NovedadContractualAportante
                                                    .Where(x => x.NovedadContractualId == ListDP.NovedadContractualId)
                                                        .Include(x => x.ComponenteAportanteNovedad)
                                                            .ThenInclude(x => x.ComponenteFuenteNovedad)
                                                                .ThenInclude(x => x.ComponenteUsoNovedad)
                                                    .ToList();

                    //valorproyecto = _context.ComponenteUsoNovedad.Where(x => x.ComponenteAportanteNovedad.CofinanciacionAportanteId == ppapor.AportanteId &&
                    //x.ComponenteAportanteNovedad.NovedadContractualAportante.NovedadContractualId == detailDP.NovedadContractualId).Sum(x => x.ValorUso);

                }

                decimal saldototal = 0;
                {
                    List<CofinanicacionAportanteGrilla> aportantes = new List<CofinanicacionAportanteGrilla>();
                    List<ProyectoGrilla> proyecto = new List<ProyectoGrilla>();
                    string nombreAportante = string.Empty;
                    decimal? valorAportate = 0;
                    decimal valorGestionado = 0;
                    List<DisponibilidadPresupuestalProyecto> ListDPP = _context.DisponibilidadPresupuestalProyecto.Where(r => r.DisponibilidadPresupuestalId == ListDP.DisponibilidadPresupuestalId)
                                                                                                                  .ToList();

                    foreach (var proyectospp in ListDPP)
                    {
                        List<CofinanicacionAportanteGrilla> aportantesxProyecto = new List<CofinanicacionAportanteGrilla>();
                        #region proyecto administrativo
                        if (proyectospp.ProyectoId == null) //proyecto administrativo
                        {
                            //Poner el nuevo ValorSolicitadoGenerado
                            valorGestionado += _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalId == proyectospp.DisponibilidadPresupuestalId)
                                                                                 .Sum(x => (x.ValorSolicitadoGenerado != null ? x.ValorSolicitadoGenerado : x.ValorSolicitado)) ?? 0;

                            int intaportante = 0;
                            var proyectoadministrativo = _context.ProyectoAdministrativo.Where(x => x.ProyectoAdministrativoId == proyectospp.ProyectoAdministrativoId)
                                                                                        .Include(x => x.ProyectoAdministrativoAportante)
                                                                                           .ThenInclude(x => x.AportanteFuenteFinanciacion)
                                                                                               .ThenInclude(x => x.FuenteFinanciacion)
                                                                                        .ToList();
                            if (proyectoadministrativo.Count() > 0)
                            {
                                foreach (var apo in proyectoadministrativo.FirstOrDefault().ProyectoAdministrativoAportante)
                                {
                                    List<GrillaFuentesFinanciacion> fuentes = new List<GrillaFuentesFinanciacion>();
                                    List<GrillaFuentesFinanciacion> fuentesNew = await _sourceFundingService.GetListFuentesFinanciacionByDisponibilidadPresupuestald(proyectospp.DisponibilidadPresupuestalId);
                               
                                    foreach (var fuente in fuentesNew)
                                    {
                                        fuentes.Add(new GrillaFuentesFinanciacion
                                        {
                                            Fuente = fuente.Fuente,
                                            Estado_de_las_fuentes = string.Empty,
                                            FuenteFinanciacionID = fuente.FuenteFinanciacionID,
                                            ValorFuente = fuente.ValorFuente,
                                            Valor_solicitado_de_la_fuente = fuente.Valor_solicitado_de_la_fuente,
                                            Nuevo_saldo_de_la_fuente = fuente.Nuevo_saldo_de_la_fuente,
                                            Saldo_actual_de_la_fuente = fuente.Saldo_actual_de_la_fuente,
                                            Nuevo_saldo_de_la_fuente_al_guardar = fuente.Nuevo_saldo_de_la_fuente_al_guardar,
                                            Saldo_actual_de_la_fuente_al_guardar = fuente.Saldo_actual_de_la_fuente_al_guardar
                                        });
                                    }
                                    foreach (var font in apo.AportanteFuenteFinanciacion)
                                    {
                                        nombreAportante = getNombreAportante(_context.CofinanciacionAportante.Find(font.FuenteFinanciacion.AportanteId));
                                        valorAportate = font.ValorFuente;
                                        if (!aportantes.Any(c => c.CofinanciacionAportanteId == apo.AportanteId))
                                        {
                                            aportantes.Add(new CofinanicacionAportanteGrilla
                                            {
                                                CofinanciacionAportanteId = apo.AportanteId,
                                                Nombre = nombreAportante,
                                                TipoAportante = string.Empty,
                                                ValorAportanteAlProyecto = valorAportate,
                                                FuentesFinanciacion = fuentes
                                            });
                                        }
                                    }
                                    intaportante = apo.AportanteId == null ? 0 : apo.AportanteId;
                                }
                                proyecto.Add(new ProyectoGrilla
                                {
                                    LlaveMen = proyectoadministrativo.FirstOrDefault().ProyectoAdministrativoId.ToString(),
                                    Departamento = string.Empty,
                                    Municipio = string.Empty,
                                    TipoIntervencion = string.Empty,
                                    InstitucionEducativa = string.Empty,
                                    Sede = string.Empty,
                                    NombreAportante = nombreAportante,
                                    ValorAportante = valorAportate,
                                    AportanteID = intaportante,
                                    DisponibilidadPresupuestalProyecto = 0,
                                    ValorGestionado = 0,
                                    ComponenteGrilla = null,
                                });
                            }
                        }

                        #endregion proyecto administrativo

                        #region Proyecto fisico

                        else
                        {
                            Proyecto proyectopp = _context.Proyecto.Find(proyectospp.ProyectoId);
                            valorGestionado += _context.GestionFuenteFinanciacion
                                .Where(x => !(bool)x.Eliminado &&
                                       x.DisponibilidadPresupuestalProyectoId == proyectospp.DisponibilidadPresupuestalProyectoId)
                                  .Sum(x => x.ValorSolicitado);

                            var localizacion = _context.Localizacion.Where(x => x.LocalizacionId == proyectopp.LocalizacionIdMunicipio).FirstOrDefault();
                            var sede = _context.InstitucionEducativaSede.Find(proyectopp.SedeId);
                            List<GrillaComponentes> grilla = new List<GrillaComponentes>();
                            int intaportante = 0;
                            decimal valorgestionado = 0;

                            //proyectospp.Proyecto.ProyectoAportante = proyectospp.Proyecto.ProyectoAportante.Where(r => r.Eliminado != true).ToList();
                            List<ProyectoAportante> proyectoAportantes = _context.ProyectoAportante.Where(r => r.Eliminado != true && r.ProyectoId == proyectospp.ProyectoId).Include(c => c.Aportante).ThenInclude(c => c.FuenteFinanciacion).ToList();
                            foreach (var ppapor in proyectoAportantes)
                            {
                                List<GrillaFuentesFinanciacion> fuentes = new List<GrillaFuentesFinanciacion>();
                                List<GestionFuenteFinanciacion>  fuentesNew = new List<GestionFuenteFinanciacion>();

                                int Id = proyectospp.DisponibilidadPresupuestalProyectoId;
                                List<int> fuentesxAportante = ppapor.Aportante.FuenteFinanciacion.Select(x => (int)x.FuenteFinanciacionId).ToList();

                                if (ListDP.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Especial)
                                {
                                    //fuentesNew =  _sourceFundingService.GetListFuentesFinanciacionByDisponibilidadPresupuestald(proyectospp.DisponibilidadPresupuestalId, false, 0);
                                    fuentesNew = _context.GestionFuenteFinanciacion.Where(r => r.Eliminado != true && r.EsNovedad != true && r.DisponibilidadPresupuestalProyectoId == proyectospp.DisponibilidadPresupuestalProyectoId && fuentesxAportante.Contains((int)r.FuenteFinanciacionId)).ToList();

                                }
                                else
                                {
                                    //fuentesNew =  _sourceFundingService.GetListFuentesFinanciacionByDisponibilidadPresupuestalProyectoid(Id, ppapor.AportanteId, false, 0);
                                     fuentesNew = _context.GestionFuenteFinanciacion.Where(r => r.Eliminado != true && r.EsNovedad != true && r.DisponibilidadPresupuestalProyectoId == proyectospp.DisponibilidadPresupuestalProyectoId && fuentesxAportante.Contains((int)r.FuenteFinanciacionId)).ToList();
                                }

                                foreach (var fuente in fuentesNew)
                                {
                                    FuenteFinanciacion temp = ppapor.Aportante.FuenteFinanciacion.Where(r => r.FuenteFinanciacionId == fuente.FuenteFinanciacionId).FirstOrDefault();
                                    string fuenteNombre = string.Empty;
                                    if (temp != null)
                                    {
                                        fuenteNombre = _context.Dominio.Where(x => x.Codigo == temp.FuenteRecursosCodigo
                                                                    && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre;
                                    }

                                    if (!esGenerar && ListDP.EstadoSolicitudCodigo != "5" && ListDP.EstadoSolicitudCodigo != "8") { 
                                        fuentes.Add(new GrillaFuentesFinanciacion
                                        {
                                            Fuente = fuenteNombre,
                                            Estado_de_las_fuentes = string.Empty,
                                            FuenteFinanciacionID = ppapor.Aportante.FuenteFinanciacion.FirstOrDefault().FuenteFinanciacionId,
                                            //Saldo_actual_de_la_fuente = SaldoActualFuente,
                                            Saldo_actual_de_la_fuente = fuente.SaldoActual,
                                            Valor_solicitado_de_la_fuente = fuente.ValorSolicitado,
                                            //Nuevo_saldo_de_la_fuente = NuevoSaldoFuente,
                                            Nuevo_saldo_de_la_fuente = fuente.NuevoSaldo,
                                            Nuevo_saldo_de_la_fuente_al_guardar = fuente.NuevoSaldo,
                                            Saldo_actual_de_la_fuente_al_guardar = fuente.SaldoActual,
                                        });
                                    }else if (ListDP.EstadoSolicitudCodigo == "5" && ListDP.EstadoSolicitudCodigo == "8")
                                    {
                                        fuentes.Add(new GrillaFuentesFinanciacion
                                        {
                                            Fuente = fuenteNombre,
                                            Estado_de_las_fuentes = string.Empty,
                                            FuenteFinanciacionID = ppapor.Aportante.FuenteFinanciacion.FirstOrDefault().FuenteFinanciacionId,
                                            //Saldo_actual_de_la_fuente = SaldoActualFuente,
                                            Saldo_actual_de_la_fuente = fuente.SaldoActualGenerado ?? fuente.SaldoActual,
                                            Valor_solicitado_de_la_fuente = fuente.ValorSolicitadoGenerado ?? fuente.ValorSolicitado,
                                            //Nuevo_saldo_de_la_fuente = NuevoSaldoFuente,
                                            Nuevo_saldo_de_la_fuente = fuente.NuevoSaldoGenerado ??  fuente.NuevoSaldo,
                                            Nuevo_saldo_de_la_fuente_al_guardar = fuente.NuevoSaldoGenerado ?? fuente.NuevoSaldo,
                                            Saldo_actual_de_la_fuente_al_guardar = fuente.SaldoActualGenerado ?? fuente.SaldoActual,
                                        });
                                    }
                                    else
                                    {
                                        var saldoActual = _context.VSaldosFuenteXaportanteId.Where(r => r.CofinanciacionAportanteId == ppapor.Aportante.CofinanciacionAportanteId && r.FuenteFinanciacionId == ppapor.Aportante.FuenteFinanciacion.FirstOrDefault().FuenteFinanciacionId).FirstOrDefault();
                                        decimal valor = saldoActual != null ? (decimal)saldoActual.SaldoActual : 0;
                                        fuentes.Add(new GrillaFuentesFinanciacion
                                        {
                                            Fuente = fuenteNombre,
                                            Estado_de_las_fuentes = string.Empty,
                                            FuenteFinanciacionID = ppapor.Aportante.FuenteFinanciacion.FirstOrDefault().FuenteFinanciacionId,
                                            //Saldo_actual_de_la_fuente = SaldoActualFuente,
                                            Saldo_actual_de_la_fuente = fuente.SaldoActualGenerado ?? valor,
                                            Valor_solicitado_de_la_fuente = fuente.ValorSolicitadoGenerado ?? fuente.ValorSolicitado,
                                            //Nuevo_saldo_de_la_fuente = NuevoSaldoFuente,
                                            Nuevo_saldo_de_la_fuente = fuente.NuevoSaldoGenerado ?? valor - fuente.ValorSolicitado,
                                            Nuevo_saldo_de_la_fuente_al_guardar = fuente.NuevoSaldo,
                                            Saldo_actual_de_la_fuente_al_guardar = fuente.SaldoActual,
                                        });
                                    }
                                }
                                if (ListDP.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Tradicional)
                                {
                                    valorAportate = _context.ProyectoAportante.Where(x => x.ProyectoId == proyectospp.ProyectoId && x.AportanteId == ppapor.AportanteId).Sum(x => x.ValorTotalAportante);

                                    var valorproyecto = contratacionDP.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Interventoria ? ppapor.ValorInterventoria : ppapor.ValorObra;

                                    if (novedadContractualAportantes != null)
                                    {
                                        novedadContractualAportantes.ForEach(nca =>
                                        {
                                            bool flagCofinanciacion = false;
                                            if (nca.CofinanciacionAportanteId == ppapor.AportanteId)
                                            {
                                                flagCofinanciacion = true;
                                            }
                                            //if ( nca.CofinanciacionAportanteId == ppapor.AportanteId)
                                            {
                                                nca.ComponenteAportanteNovedad.ToList().ForEach(can =>
                                                {
                                                    can.ComponenteFuenteNovedad.ToList().ForEach(cfn =>
                                                    {
                                                        cfn.ComponenteUsoNovedad.ToList().ForEach(cun =>
                                                        {
                                                            valorproyecto += cun.ValorUso;
                                                            if (flagCofinanciacion)
                                                            {
                                                                valorAportate += cun.ValorUso;
                                                            }
                                                        });
                                                    });
                                                });
                                            }
                                        });
                                    }

                                    string nombreAportantePpa = getNombreAportante(ppapor.Aportante);
                                    string tipoAportante = _context.Dominio.Where(r => (bool)r.Activo && r.DominioId.Equals(ppapor.Aportante.TipoAportanteId) && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_aportante).Select(r => r.Nombre).FirstOrDefault();
                                    aportantes.Add(new CofinanicacionAportanteGrilla
                                    {
                                        CofinanciacionAportanteId = ppapor.AportanteId,
                                        Nombre = nombreAportantePpa,
                                        TipoAportante = tipoAportante,
                                        ValorAportanteAlProyecto = valorproyecto,
                                        FuentesFinanciacion = fuentes,

                                    });
                                    aportantesxProyecto.Add(new CofinanicacionAportanteGrilla
                                    {
                                        CofinanciacionAportanteId = ppapor.AportanteId,
                                        Nombre = nombreAportantePpa,
                                        TipoAportante = tipoAportante,
                                        ValorAportanteAlProyecto = valorproyecto,
                                        FuentesFinanciacion = fuentes,
                                        ValorGestionado = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalProyectoId == proyectospp.DisponibilidadPresupuestalProyectoId && fuentes.Select(x => x.FuenteFinanciacionID).Contains(x.FuenteFinanciacionId)).Sum(x => x.ValorSolicitado)
                                    });
                                }


                                var confinanciacion = _context.CofinanciacionAportante.Where(x => x.CofinanciacionAportanteId == ppapor.AportanteId).Include(x => x.CofinanciacionDocumento).FirstOrDefault();
                                var intfuentes = _context.FuenteFinanciacion.Where(y => y.AportanteId == ppapor.AportanteId).Select(t => t.FuenteFinanciacionId).ToList();

                                if (confinanciacion != null)
                                {
                                    intaportante = confinanciacion == null ? 0 : confinanciacion.CofinanciacionAportanteId;
                                    nombreAportante = getNombreAportante(confinanciacion);
                                    var componenteAp = _context.ComponenteAportante.Where(x => x.ContratacionProyectoAportante.ContratacionProyecto.ContratacionId == ListDP.ContratacionId
                                    && x.ContratacionProyectoAportante.ContratacionProyecto.ProyectoId == proyectospp.ProyectoId &&
                                    x.ContratacionProyectoAportante.CofinanciacionAportanteId == confinanciacion.CofinanciacionAportanteId && x.Eliminado != true)
                                        .Include(x => x.ComponenteUso).ToList();
                                    foreach (var compAp in componenteAp)
                                    {
                                        List<string> uso = new List<string>();
                                        List<decimal> usovalor = new List<decimal>();
                                        decimal total = 0;

                                        //fase
                                        string strFase = string.Empty;

                                        if (!string.IsNullOrEmpty(compAp.FaseCodigo))
                                        {
                                            strFase = _context.Dominio.Where(r => r.Codigo == compAp.FaseCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Fases).FirstOrDefault().Nombre;
                                        }

                                        foreach (var comp in compAp.ComponenteUso)
                                        {
                                            var usos = _context.Dominio.Where(x => x.Codigo == comp.TipoUsoCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Usos).ToList();
                                            uso.Add(usos.Count() > 0 ? usos.FirstOrDefault().Nombre : string.Empty);
                                            usovalor.Add(comp.ValorUso);
                                            total += comp.ValorUso;
                                        }
                                        var dom = _context.Dominio.Where(x => x.Codigo == compAp.TipoComponenteCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Componentes).ToList();
                                        grilla.Add(
                                            new GrillaComponentes
                                            {
                                                ComponenteAportanteId = compAp.ComponenteAportanteId,
                                                Componente = dom.Count() > 0 ? dom.FirstOrDefault().Nombre : string.Empty,
                                                ComponenteUsoCodigo = compAp.TipoComponenteCodigo,
                                                Uso = uso,
                                                ValorTotal = total,
                                                ValorUso = usovalor,
                                                cofinanciacionAportanteId = ppapor.Aportante.CofinanciacionAportanteId,
                                                Fase = strFase
                                            });
                                    }
                                    valorgestionado = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalProyectoId == proyectospp.DisponibilidadPresupuestalProyectoId && intfuentes.Contains(x.FuenteFinanciacionId)).Sum(x => x.ValorSolicitado);


                                }
                            }
                            proyecto.Add(new ProyectoGrilla
                            {
                                LlaveMen = proyectopp.LlaveMen,
                                Departamento = _context.Localizacion.Find(localizacion.IdPadre).Descripcion,
                                Municipio = localizacion.Descripcion,
                                TipoIntervencion = proyectopp.TipoIntervencionCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(proyectopp.TipoIntervencionCodigo, (int)EnumeratorTipoDominio.Tipo_de_Intervencion) : string.Empty,
                                InstitucionEducativa = _context.InstitucionEducativaSede.Find(sede.PadreId).Nombre,
                                Sede = sede.Nombre,
                                NombreAportante = nombreAportante,
                                ValorAportante = valorAportate,
                                AportanteID = intaportante,
                                DisponibilidadPresupuestalProyecto = proyectospp.DisponibilidadPresupuestalProyectoId,
                                ValorGestionado = valorgestionado,
                                ComponenteGrilla = grilla,
                                Aportantes = aportantesxProyecto,
                                ValorProyectoxComponente = contratacionDP != null ? contratacionDP.TipoSolicitudCodigo == "2" ? proyectopp.ValorInterventoria : proyectopp.ValorObra : 0
                        });
                        }

                        #endregion Proyecto fisico
                        
                    }

                    #region busco comite técnico

                    //busco comite técnico
                    DateTime fechaComitetecnico = DateTime.Now;

                    string numerocomietetecnico = string.Empty;

                    if (ListDP.ContratacionId != null)
                    {
                        var contratacion = _context.SesionComiteSolicitud.Where(x => x.SolicitudId == ListDP.ContratacionId && x.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion).
                            Include(x => x.ComiteTecnico).ToList();
                        if (contratacion.Count() > 0)
                        {
                            sesionComiteSolicitud = contratacion;
                            if (contratacion.FirstOrDefault().ComiteTecnicoFiduciarioId > 0)
                            {
                                ComiteTecnico comiteTecnicoFiduciario = _context.ComiteTecnico.Find(contratacion.FirstOrDefault().ComiteTecnicoFiduciarioId);
                                sesionComiteSolicitud.FirstOrDefault().ComiteTecnicoFiduciario = comiteTecnicoFiduciario;
                            }
                            numerocomietetecnico = contratacion.FirstOrDefault().ComiteTecnico.NumeroComite;
                            fechaComitetecnico = Convert.ToDateTime(contratacion.FirstOrDefault().ComiteTecnico.FechaOrdenDia);
                        }
                    }

                    #endregion busco comite técnico

                    if (ListDP.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Especial)
                    {
                        #region otros costos

                        if (ListDP.NumeroContrato != null)//otros costos
                        {
                            valorGestionado = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalId == ListDP.DisponibilidadPresupuestalId).Sum(x => x.ValorSolicitado);
                            var aportanteotroscostos = _context.CofinanciacionAportante.Where(x => x.CofinanciacionAportanteId == ListDP.AportanteId).
                                Include(x => x.FuenteFinanciacion).ToList();
                            if (aportanteotroscostos.Any())
                            {
                                aportantes.Add(new CofinanicacionAportanteGrilla
                                {
                                    CofinanciacionAportanteId = aportanteotroscostos.FirstOrDefault().CofinanciacionAportanteId,
                                    Nombre = getNombreAportante(aportanteotroscostos.FirstOrDefault()),
                                    TipoAportante = _context.Dominio.Where(r => (bool)r.Activo && r.DominioId.Equals(aportanteotroscostos.FirstOrDefault().TipoAportanteId) && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_aportante).Select(r => r.Nombre).FirstOrDefault(),
                                    ValorAportanteAlProyecto = ListDP.ValorAportante,
                                    FuentesFinanciacion = null
                                });
                                saldototal = Convert.ToDecimal(aportanteotroscostos.FirstOrDefault().FuenteFinanciacion.Sum(x => x.ValorFuente));
                            }
                        }

                        #endregion otros costos

                        #region expensas

                        else//expensas
                        {
                            valorGestionado = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalId == ListDP.DisponibilidadPresupuestalId).Sum(x => x.ValorSolicitado);
                            var aportanteotroscostos = _context.CofinanciacionAportante.Where(x => x.CofinanciacionAportanteId == ListDP.AportanteId).
                                Include(x => x.FuenteFinanciacion).ToList();
                            if (aportanteotroscostos.Any())
                            {
                                List<GrillaFuentesFinanciacion> fnt = new List<GrillaFuentesFinanciacion>();
                                foreach (var fuente in aportanteotroscostos.FirstOrDefault().FuenteFinanciacion)
                                {
                                    fnt.Add(new GrillaFuentesFinanciacion
                                    {
                                        Fuente = _context.Dominio.Where(x => x.Codigo == fuente.FuenteRecursosCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre,
                                        Valor_solicitado_de_la_fuente = Convert.ToDecimal(fuente.ValorFuente),
                                        Estado_de_las_fuentes = string.Empty,
                                        FuenteFinanciacionID = fuente.FuenteFinanciacionId,
                                        GestionFuenteFinanciacionID = 0,
                                        Nuevo_saldo_de_la_fuente = 0,
                                        Saldo_actual_de_la_fuente = 0
                                    });

                                }
                                aportantes.Add(new CofinanicacionAportanteGrilla
                                {
                                    CofinanciacionAportanteId = aportanteotroscostos.FirstOrDefault().CofinanciacionAportanteId,
                                    Nombre = getNombreAportante(aportanteotroscostos.FirstOrDefault()),
                                    TipoAportante = _context.Dominio.Where(r => (bool)r.Activo && r.DominioId.Equals(aportanteotroscostos.FirstOrDefault().TipoAportanteId) && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_aportante).Select(r => r.Nombre).FirstOrDefault(),
                                    ValorAportanteAlProyecto = ListDP.ValorAportante,
                                    FuentesFinanciacion = fnt
                                });
                                saldototal = Convert.ToDecimal(aportanteotroscostos.FirstOrDefault().FuenteFinanciacion.Sum(x => x.ValorFuente));
                            }
                        }

                        #endregion expensas

                    }
                    var contrato = _context.Contrato.Where(x => x.Contratacion.ContratacionId == ListDP.ContratacionId);
                    var fechaContrato = string.Empty;

                    if (contrato.Any())
                    {
                        var fechafi = contrato.Select(x => x.FechaFirmaContrato).FirstOrDefault();
                        fechaContrato = Convert.ToDateTime(fechafi).ToString("dd/MM/yyyy");

                    }

                    string nombreEntidad = string.Empty;
                    string contratoNumero = !contrato.Any() ? string.Empty : contrato.Select(x => x.NumeroContrato).FirstOrDefault().ToString();
                    if (contrato.Any())
                    {
                        var contratacion = _context.Contratacion.Find(contrato.FirstOrDefault().ContratacionId);
                        var contratista = _context.Contratista.Find(contratacion.ContratistaId);
                        nombreEntidad = contratista == null ? string.Empty : contratista.Nombre;
                    }


                    var observaciones = _context.DisponibilidadPresupuestalObservacion.Where(x => x.DisponibilidadPresupuestalId == ListDP.DisponibilidadPresupuestalId && x.EsNovedad == esNovedad && x.NovedadContractualRegistroPresupuestalId == RegistroNovedadId).ToList();
                    string observacionString = !observaciones.Any() ? string.Empty : string.Join("<br><br>", observaciones.Select(x => x.Observacion));
                    var aportant = aportantes.Distinct();
                    var tiporubro = ListDP.TipoSolicitudEspecialCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(ListDP.TipoSolicitudEspecialCodigo, (int)EnumeratorTipoDominio.Tipo_DDP_Espacial) :
                        //si no viene el campo puede ser contratación
                        ListDP.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo ? ConstanStringTipoSolicitudContratacion.proyectoAdministrativo :
                        ConstanStringTipoSolicitudContratacion.contratacion;
                    bool blnEstado = false;

                    //si es administrativo, esta completo, si es tradicional, se verifica contra fuentes gestionadas
                    //2020-11-08 ahora los administrativos y especiales tambien estionan fuentes
                    if (ListDP.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo)
                    {
                        List<int> ddpproyectosId = ListDP.DisponibilidadPresupuestalProyecto.Select(x => x.DisponibilidadPresupuestalProyectoId).ToList();
                        if (_context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalId == ListDP.DisponibilidadPresupuestalId).Count() > 0)
                            blnEstado = true;

                    }
                    else if (ListDP.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Especial)
                    {
                        if (_context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalId == ListDP.DisponibilidadPresupuestalId).Count() > 0)
                            blnEstado = true;
                    }
                    else
                    {
                        List<int> proyectosId = ListDPP.Where(x => x.ProyectoId > 0).Select(x => (int)x.ProyectoId).ToList();
                        List<int> ddpproyectosId = ListDPP.Select(x => (int)x.DisponibilidadPresupuestalProyectoId).ToList();
                        var aportantesEstado = _context.ProyectoAportante.Where(x => proyectosId.Contains(x.ProyectoId)).ToList();
                        //var fuentes = _context.FuenteFinanciacion.Where(x => aportantes.Contains(x.AportanteId)).Count();

                        if (ListDP.EsNovedad != true)
                        {
                            decimal totalSolicitado = 0;

                            if (vdpp != null || ListDP.EstadoSolicitudCodigo == "8")
                            {
                                totalSolicitado = _context.GestionFuenteFinanciacion.Where(x => x.DisponibilidadPresupuestalProyectoId != null &&
                                               ddpproyectosId.Contains((int)x.DisponibilidadPresupuestalProyectoId) && x.Eliminado != true && x.EsNovedad != true).Sum(r => r.ValorSolicitadoGenerado ?? 0);
                                if (totalSolicitado == ListDP.ValorSolicitud)
                                    blnEstado = true;
                            }
                            else
                            {
                                totalSolicitado = _context.GestionFuenteFinanciacion.Where(x => x.DisponibilidadPresupuestalProyectoId != null &&
                                       ddpproyectosId.Contains((int)x.DisponibilidadPresupuestalProyectoId) && x.Eliminado != true && x.EsNovedad != true).Sum(r => r.ValorSolicitado);
                                if (totalSolicitado == ListDP.ValorSolicitud)
                                    blnEstado = true;
                            }
                        }
                    }
                    var plazoContratacion = new PlazoContratacion();
                    bool tieneSolicitudPago = false;
                    if (contratacionDP != null)
                    {
                        plazoContratacion = _context.PlazoContratacion.Find(contratacionDP.PlazoContratacionId);
                        if(contratacionDP.Contrato.Count() > 0)
                            tieneSolicitudPago = _context.SolicitudPago.Where(r => r.ContratoId == contratacionDP.Contrato.FirstOrDefault().ContratoId && r.Eliminado != true).Count() > 0 ? true : false;
                    }
                    int existeNovedad = _context.NovedadContractualRegistroPresupuestal.Where(r => r.Eliminado != true && r.DisponibilidadPresupuestalId == ListDP.DisponibilidadPresupuestalId).Count();
                    if (ListDP.EstadoSolicitudCodigo == "5" || ListDP.EstadoSolicitudCodigo == "8")
                    {
                        vdpp = _context.VDisponibilidadPresupuestal.Where(r => r.DisponibilidadPresupuestalId == ListDP.DisponibilidadPresupuestalId && r.EstadoSolicitudCodigo == "10" && r.TieneHistorico == true).FirstOrDefault();
                    }
                    DetailValidarDisponibilidadPresupuesal detailDisponibilidadPresupuesal = new DetailValidarDisponibilidadPresupuesal
                    {
                        //TODO:Traer estos campos { Tipo de modificacion, Valor despues de la modificacion, Plazo despues de la modificacion, Detalle de la modificacion) => se toma del caso de uso de novedades contractuales
                        Id = ListDP.DisponibilidadPresupuestalId,
                        NumeroSolicitud = ListDP.NumeroSolicitud,
                        TipoSolicitudCodigo = ListDP.TipoSolicitudCodigo,
                        NUmeroSaldoFuente = saldototal,
                        TipoSolicitudText = ListDP.TipoSolicitudCodigo != null ?
                            await _commonService.GetNombreDominioByCodigoAndTipoDominio(ListDP.TipoSolicitudCodigo,
                            (int)EnumeratorTipoDominio.Tipo_Disponibilidad_Presupuestal) : string.Empty,
                        NumeroDDP = ListDP.NumeroDdp,
                        NumeroDRP = ListDP.NumeroDrp,
                        RubroPorFinanciar = ListDP.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Especial ? tiporubro : _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Disponibilidad_Presupuestal
                                         && r.Codigo == ListDP.TipoSolicitudCodigo).FirstOrDefault().Descripcion,
                        Objeto = ListDP.Objeto,
                        ValorSolicitud = ListDP.ValorSolicitud,
                        // Si es aproboda por comite tecnico se debe mostrar la fecha en la que fue aprobada. traer desde dbo.[Sesion]
                        FechaComiteTecnico = fechaComitetecnico,
                        NumeroComite = numerocomietetecnico,
                        FechaSolicitud = ListDP.FechaSolicitud,
                        EstadoStr = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud_Presupuestal
                                    && r.Codigo == ListDP.EstadoSolicitudCodigo).FirstOrDefault().Nombre,
                        EstadoSolicitudCodigo = ListDP.EstadoSolicitudCodigo,
                        Plazo = contratacionDP != null ? plazoContratacion?.PlazoMeses.ToString() + " meses / " + plazoContratacion?.PlazoDias.ToString() + " dias" : "",
                        CuentaCarta = ListDP.CuentaCartaAutorizacion,
                        TipoSolicitudEspecial = ListDP.TipoSolicitudEspecialCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(ListDP.TipoSolicitudEspecialCodigo, (int)EnumeratorTipoDominio.Tipo_DDP_Espacial) :
                        //si no viene el campo puede ser contratación
                        ListDP.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo ? ConstanStringTipoSolicitudContratacion.proyectoAdministrativo :
                        ListDP.EsNovedadContractual == null ? ConstanStringTipoSolicitudContratacion.contratacion : !Convert.ToBoolean(ListDP.EsNovedadContractual) ? ConstanStringTipoSolicitudContratacion.contratacion : ConstanStringTipoSolicitudContratacion.novedadContractual,
                        ContratoNumero = contratoNumero,
                        NombreEntidad = nombreEntidad,
                        UrlConSoporte = ListDP.UrlSoporte,
                        Limitacion = ListDP.LimitacionEspecial,
                        /*//*las modificaciones aun no existen*/
                        ValorGestionado = valorGestionado,
                        Observaciones = observacionString,
                        Proyectos = proyecto,
                        FechaContrato = fechaContrato,
                        //Aportantes
                        Aportantes = aportant.ToList(),
                        NumeroRadicado = ListDP.NumeroRadicadoSolicitud,
                        ObservacioensCancelacion = _context.DisponibilidadPresupuestalObservacion.Where(x => x.DisponibilidadPresupuestalId == ListDP.DisponibilidadPresupuestalId && x.EsNovedad == esNovedad && x.NovedadContractualRegistroPresupuestalId == RegistroNovedadId).ToList(),
                        EsNovedad = false,
                        NovedadContractual = ListDP.NovedadContractualId != null ? _context.NovedadContractual.Where(x => x.NovedadContractualId == ListDP.NovedadContractualId).Include(x => x.NovedadContractualDescripcion).FirstOrDefault() : null,
                        EstadoRegistro = blnEstado,
                        SesionComiteSolicitud = sesionComiteSolicitud,
                        ValorTotalDisponibilidad = ListDP.ValorTotalDisponibilidad,
                        TieneNovedad = existeNovedad > 0 ? true : false,
                        TieneHistorico = vdpp != null ? true : false,
                        TieneSolicitudPago = tieneSolicitudPago
                    };

                    ListDetailValidarDisponibilidadPresupuesal.Add(detailDisponibilidadPresupuesal);
                }
            }
            return ListDetailValidarDisponibilidadPresupuesal;
        }

        public async Task<List<DetailValidarDisponibilidadPresupuesal>> GetDetailAvailabilityBudgetProyectHistorical(int disponibilidadPresupuestalId, bool esNovedad, int RegistroNovedadId, bool esGenerar)
        {
            List<DetailValidarDisponibilidadPresupuesal> ListDetailValidarDisponibilidadPresupuesal = new List<DetailValidarDisponibilidadPresupuesal>();
            List<SesionComiteSolicitud> sesionComiteSolicitud = new List<SesionComiteSolicitud>();
            if (esNovedad == true)
            {
               ListDetailValidarDisponibilidadPresupuesal = await GetDetailAvailabilityBudgetProyectNovelty(RegistroNovedadId, esGenerar,true);
            }
            else
            {
                DisponibilidadPresupuestal ListDP = await _context.DisponibilidadPresupuestal.
                    Where(r => r.Eliminado != true && r.DisponibilidadPresupuestalId == disponibilidadPresupuestalId).
                    FirstOrDefaultAsync();
                DisponibilidadPresupuestalHistorico ddph = null;

                if (ListDP != null)
                {
                    ddph = await _context.DisponibilidadPresupuestalHistorico.
                                    Where(r => r.DisponibilidadPresupuestalId == ListDP.DisponibilidadPresupuestalId).
                                    FirstOrDefaultAsync();
                    ListDP.ValorTotalDisponibilidad = _commonService.GetValorTotalDisponibilidad(disponibilidadPresupuestalId, true);
                }

                Contratacion contratacionDP = _context.Contratacion.Where(r => r.ContratacionId == ListDP.ContratacionId).FirstOrDefault();

                decimal saldototal = 0;
                {
                    List<CofinanicacionAportanteGrilla> aportantes = new List<CofinanicacionAportanteGrilla>();
                    List<ProyectoGrilla> proyecto = new List<ProyectoGrilla>();
                    string nombreAportante = string.Empty;
                    decimal? valorAportate = 0;
                    decimal valorGestionado = 0;
                    List<DisponibilidadPresupuestalProyecto> ListDPP = _context.DisponibilidadPresupuestalProyecto.Where(r => r.DisponibilidadPresupuestalId == ListDP.DisponibilidadPresupuestalId).ToList();
                    foreach (var proyectospp in ListDPP)
                    {
                        List<CofinanicacionAportanteGrilla> aportantesxProyecto = new List<CofinanicacionAportanteGrilla>();
                        if (proyectospp.ProyectoId != null) //proyecto administrativo
                        {
                            Proyecto proyectopp = _context.Proyecto.Find(proyectospp.ProyectoId);
                            List<GestionFuenteFinanciacion> gffList = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado &&
                                       x.DisponibilidadPresupuestalProyectoId == proyectospp.DisponibilidadPresupuestalProyectoId).ToList();
                            foreach (var gff in gffList)
                            {
                                GestionFuenteFinanciacionHistorico gffh = _context.GestionFuenteFinanciacionHistorico.Where(r => r.GestionFuenteFinanciacionId == gff.GestionFuenteFinanciacionId).FirstOrDefault();
                                valorGestionado += gffh != null ? gffh.ValorSolicitado : gff.ValorSolicitado;
                            }

                            var localizacion = _context.Localizacion.Where(x => x.LocalizacionId == proyectopp.LocalizacionIdMunicipio).FirstOrDefault();
                            var sede = _context.InstitucionEducativaSede.Find(proyectopp.SedeId);
                            List<GrillaComponentes> grilla = new List<GrillaComponentes>();
                            int intaportante = 0;
                            List<ProyectoAportante> proyectoAportantes = _context.ProyectoAportante.Where(r => r.Eliminado != true && r.ProyectoId == proyectospp.ProyectoId).Include(c => c.Aportante).ThenInclude(c => c.FuenteFinanciacion).ToList();
                            foreach (var ppapor in proyectoAportantes)
                            {
                                ProyectoAportanteHistorico ppaporh = _context.ProyectoAportanteHistorico.Where(r => r.ProyectoAportanteId == ppapor.ProyectoAportanteId).FirstOrDefault();
                                if (ppapor != null)
                                {
                                    List<GrillaFuentesFinanciacion> fuentes = new List<GrillaFuentesFinanciacion>();
                                    List<GestionFuenteFinanciacion> fuentesNew = new List<GestionFuenteFinanciacion>();

                                    int Id = proyectospp.DisponibilidadPresupuestalProyectoId;

                                    if (ListDP.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Especial)
                                    {
                                        fuentesNew = _context.GestionFuenteFinanciacion.Where(r => r.Eliminado != true && r.EsNovedad != true && r.DisponibilidadPresupuestalProyectoId == proyectospp.DisponibilidadPresupuestalProyectoId && r.FuenteFinanciacionId == ppapor.Aportante.FuenteFinanciacion.FirstOrDefault().FuenteFinanciacionId).ToList();

                                    }
                                    else
                                    {
                                        fuentesNew = _context.GestionFuenteFinanciacion.Where(r => r.Eliminado != true && r.EsNovedad != true && r.DisponibilidadPresupuestalProyectoId == proyectospp.DisponibilidadPresupuestalProyectoId && r.FuenteFinanciacionId == ppapor.Aportante.FuenteFinanciacion.FirstOrDefault().FuenteFinanciacionId).ToList();
                                    }

                                    foreach (var fuente in fuentesNew)
                                    {
                                        GestionFuenteFinanciacionHistorico gffh = _context.GestionFuenteFinanciacionHistorico.Where(r => r.GestionFuenteFinanciacionId == fuente.GestionFuenteFinanciacionId).FirstOrDefault();
                                            string fuenteNombre = _context.Dominio.Where(x => x.Codigo == ppapor.Aportante.FuenteFinanciacion.FirstOrDefault().FuenteRecursosCodigo
                                                        && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre;

                                        fuentes.Add(new GrillaFuentesFinanciacion
                                        {
                                            Fuente = fuenteNombre,
                                            Estado_de_las_fuentes = string.Empty,
                                            FuenteFinanciacionID = ppapor.Aportante.FuenteFinanciacion.FirstOrDefault().FuenteFinanciacionId,
                                            //Saldo_actual_de_la_fuente = SaldoActualFuente,
                                            Saldo_actual_de_la_fuente = gffh != null ? gffh.SaldoActual : fuente.SaldoActual,
                                            Valor_solicitado_de_la_fuente = gffh != null ?  gffh.ValorSolicitado : fuente.ValorSolicitado,
                                            //Nuevo_saldo_de_la_fuente = NuevoSaldoFuente,
                                            Nuevo_saldo_de_la_fuente = gffh != null ?  gffh.NuevoSaldo : fuente.NuevoSaldo,
                                            Nuevo_saldo_de_la_fuente_al_guardar = gffh != null ?  gffh.NuevoSaldo : fuente.NuevoSaldo,
                                            Saldo_actual_de_la_fuente_al_guardar = gffh != null ?  gffh.SaldoActual : fuente.SaldoActual,
                                        });
                                    }
                                    if (ListDP.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Tradicional)
                                    {
                                        List<ProyectoAportante> paList = _context.ProyectoAportante.Where(x => x.ProyectoId == proyectospp.ProyectoId && x.AportanteId == ppapor.AportanteId).ToList();
                                        foreach (var pa in paList)
                                        {
                                            ProyectoAportanteHistorico pah = _context.ProyectoAportanteHistorico.Where(r => r.ProyectoAportanteId == pa.ProyectoAportanteId).FirstOrDefault();
                                            valorAportate += pah != null ? pah.ValorTotalAportante : pa.ValorTotalAportante;

                                        }
                                        var valorproyecto = contratacionDP.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Interventoria ? ppaporh != null ? ppaporh.ValorInterventoria  : ppapor.ValorInterventoria : ppaporh != null ? ppaporh.ValorObra : ppapor.ValorObra;

                                        string nombreAportantePpa = getNombreAportante(ppapor.Aportante);
                                        string tipoAportante = _context.Dominio.Where(r => (bool)r.Activo && r.DominioId.Equals(ppapor.Aportante.TipoAportanteId) && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_aportante).Select(r => r.Nombre).FirstOrDefault();
                                        aportantes.Add(new CofinanicacionAportanteGrilla
                                        {
                                            CofinanciacionAportanteId = ppapor.AportanteId,
                                            Nombre = nombreAportantePpa,
                                            TipoAportante = tipoAportante,
                                            ValorAportanteAlProyecto = valorproyecto,
                                            FuentesFinanciacion = fuentes,

                                        });
                                        aportantesxProyecto.Add(new CofinanicacionAportanteGrilla
                                        {
                                            CofinanciacionAportanteId = ppapor.AportanteId,
                                            Nombre = nombreAportantePpa,
                                            TipoAportante = tipoAportante,
                                            ValorAportanteAlProyecto = valorproyecto,
                                            FuentesFinanciacion = fuentes,
                                            ValorGestionado = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalProyectoId == proyectospp.DisponibilidadPresupuestalProyectoId && fuentes.Select(x => x.FuenteFinanciacionID).Contains(x.FuenteFinanciacionId)).Sum(x => x.ValorSolicitado)
                                        });
                                    }


                                    var confinanciacion = _context.CofinanciacionAportante.Where(x => x.CofinanciacionAportanteId == ppapor.AportanteId).Include(x => x.CofinanciacionDocumento).FirstOrDefault();
                                    var intfuentes = _context.FuenteFinanciacion.Where(y => y.AportanteId == ppapor.AportanteId).Select(t => t.FuenteFinanciacionId).ToList();

                                    if (confinanciacion != null)
                                    {
                                        intaportante = confinanciacion == null ? 0 : confinanciacion.CofinanciacionAportanteId;
                                        nombreAportante = getNombreAportante(confinanciacion);
                                        var componenteAp = _context.ComponenteAportante.Where(x => x.ContratacionProyectoAportante.ContratacionProyecto.ContratacionId == ListDP.ContratacionId
                                        && x.ContratacionProyectoAportante.ContratacionProyecto.ProyectoId == proyectospp.ProyectoId &&
                                        x.ContratacionProyectoAportante.CofinanciacionAportanteId == confinanciacion.CofinanciacionAportanteId && x.Eliminado != true)
                                            .Include(x => x.ComponenteUso).ToList();
                                        foreach (var compAp in componenteAp)
                                        {
                                            List<string> uso = new List<string>();
                                            List<decimal> usovalor = new List<decimal>();
                                            decimal total = 0;

                                            //fase
                                            string strFase = string.Empty;

                                            if (!string.IsNullOrEmpty(compAp.FaseCodigo))
                                            {
                                                strFase = _context.Dominio.Where(r => r.Codigo == compAp.FaseCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Fases).FirstOrDefault().Nombre;
                                            }

                                            foreach (var comp in compAp.ComponenteUso)
                                            {
                                                ComponenteUsoHistorico cuh = _context.ComponenteUsoHistorico.Where(r => r.ComponenteUsoId == comp.ComponenteUsoId && r.Liberado == true).FirstOrDefault();
                                                var usos = _context.Dominio.Where(x => x.Codigo == comp.TipoUsoCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Usos).ToList();
                                                uso.Add(usos.Count() > 0 ? usos.FirstOrDefault().Nombre : string.Empty);
                                                usovalor.Add(cuh != null ? cuh.ValorUso : comp.ValorUso);
                                                total += cuh != null ? cuh.ValorUso : comp.ValorUso;
                                            }
                                            var dom = _context.Dominio.Where(x => x.Codigo == compAp.TipoComponenteCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Componentes).ToList();
                                            grilla.Add(
                                                new GrillaComponentes
                                                {
                                                    ComponenteAportanteId = compAp.ComponenteAportanteId,
                                                    Componente = dom.Count() > 0 ? dom.FirstOrDefault().Nombre : string.Empty,
                                                    ComponenteUsoCodigo = compAp.TipoComponenteCodigo,
                                                    Uso = uso,
                                                    ValorTotal = total,
                                                    ValorUso = usovalor,
                                                    cofinanciacionAportanteId = ppapor.Aportante.CofinanciacionAportanteId,
                                                    Fase = strFase
                                                });
                                        }
                                    }
                                }
                                
                            }
                            proyecto.Add(new ProyectoGrilla
                            {
                                LlaveMen = proyectopp.LlaveMen,
                                Departamento = _context.Localizacion.Find(localizacion.IdPadre).Descripcion,
                                Municipio = localizacion.Descripcion,
                                TipoIntervencion = proyectopp.TipoIntervencionCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(proyectopp.TipoIntervencionCodigo, (int)EnumeratorTipoDominio.Tipo_de_Intervencion) : string.Empty,
                                InstitucionEducativa = _context.InstitucionEducativaSede.Find(sede.PadreId).Nombre,
                                Sede = sede.Nombre,
                                NombreAportante = nombreAportante,
                                ValorAportante = valorAportate,
                                AportanteID = intaportante,
                                DisponibilidadPresupuestalProyecto = proyectospp.DisponibilidadPresupuestalProyectoId,
                                ValorGestionado = valorGestionado,
                                ComponenteGrilla = grilla,
                                Aportantes = aportantesxProyecto,
                                ValorProyectoxComponente = contratacionDP.TipoSolicitudCodigo == "2" ? proyectopp.ValorInterventoria : proyectopp.ValorObra
                            });
                        }
                    }

                    #region busco comite técnico

                    //busco comite técnico
                    DateTime fechaComitetecnico = DateTime.Now;

                    string numerocomietetecnico = string.Empty;

                    if (ListDP.ContratacionId != null)
                    {
                        var contratacion = _context.SesionComiteSolicitud.Where(x => x.SolicitudId == ListDP.ContratacionId && x.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion).
                            Include(x => x.ComiteTecnico).ToList();
                        if (contratacion.Count() > 0)
                        {
                            sesionComiteSolicitud = contratacion;
                            if (contratacion.FirstOrDefault().ComiteTecnicoFiduciarioId > 0)
                            {
                                ComiteTecnico comiteTecnicoFiduciario = _context.ComiteTecnico.Find(contratacion.FirstOrDefault().ComiteTecnicoFiduciarioId);
                                sesionComiteSolicitud.FirstOrDefault().ComiteTecnicoFiduciario = comiteTecnicoFiduciario;
                            }
                            numerocomietetecnico = contratacion.FirstOrDefault().ComiteTecnico.NumeroComite;
                            fechaComitetecnico = Convert.ToDateTime(contratacion.FirstOrDefault().ComiteTecnico.FechaOrdenDia);
                        }
                    }

                    #endregion busco comite técnico

                    if (ListDP.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Especial)
                    {
                        #region otros costos

                        if (ListDP.NumeroContrato != null)//otros costos
                        {
                            valorGestionado = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalId == ListDP.DisponibilidadPresupuestalId).Sum(x => x.ValorSolicitado);
                            var aportanteotroscostos = _context.CofinanciacionAportante.Where(x => x.CofinanciacionAportanteId == ListDP.AportanteId).
                                Include(x => x.FuenteFinanciacion).ToList();
                            if (aportanteotroscostos.Any())
                            {
                                aportantes.Add(new CofinanicacionAportanteGrilla
                                {
                                    CofinanciacionAportanteId = aportanteotroscostos.FirstOrDefault().CofinanciacionAportanteId,
                                    Nombre = getNombreAportante(aportanteotroscostos.FirstOrDefault()),
                                    TipoAportante = _context.Dominio.Where(r => (bool)r.Activo && r.DominioId.Equals(aportanteotroscostos.FirstOrDefault().TipoAportanteId) && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_aportante).Select(r => r.Nombre).FirstOrDefault(),
                                    ValorAportanteAlProyecto = ListDP.ValorAportante,
                                    FuentesFinanciacion = null
                                });
                                saldototal = Convert.ToDecimal(aportanteotroscostos.FirstOrDefault().FuenteFinanciacion.Sum(x => x.ValorFuente));
                            }
                        }

                        #endregion otros costos

                        #region expensas

                        else//expensas
                        {
                            valorGestionado = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalId == ListDP.DisponibilidadPresupuestalId).Sum(x => x.ValorSolicitado);
                            var aportanteotroscostos = _context.CofinanciacionAportante.Where(x => x.CofinanciacionAportanteId == ListDP.AportanteId).
                                Include(x => x.FuenteFinanciacion).ToList();
                            if (aportanteotroscostos.Any())
                            {
                                List<GrillaFuentesFinanciacion> fnt = new List<GrillaFuentesFinanciacion>();
                                foreach (var fuente in aportanteotroscostos.FirstOrDefault().FuenteFinanciacion)
                                {
                                    fnt.Add(new GrillaFuentesFinanciacion
                                    {
                                        Fuente = _context.Dominio.Where(x => x.Codigo == fuente.FuenteRecursosCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre,
                                        Valor_solicitado_de_la_fuente = Convert.ToDecimal(fuente.ValorFuente),
                                        Estado_de_las_fuentes = string.Empty,
                                        FuenteFinanciacionID = fuente.FuenteFinanciacionId,
                                        GestionFuenteFinanciacionID = 0,
                                        Nuevo_saldo_de_la_fuente = 0,
                                        Saldo_actual_de_la_fuente = 0
                                    });

                                }
                                aportantes.Add(new CofinanicacionAportanteGrilla
                                {
                                    CofinanciacionAportanteId = aportanteotroscostos.FirstOrDefault().CofinanciacionAportanteId,
                                    Nombre = getNombreAportante(aportanteotroscostos.FirstOrDefault()),
                                    TipoAportante = _context.Dominio.Where(r => (bool)r.Activo && r.DominioId.Equals(aportanteotroscostos.FirstOrDefault().TipoAportanteId) && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_aportante).Select(r => r.Nombre).FirstOrDefault(),
                                    ValorAportanteAlProyecto = ListDP.ValorAportante,
                                    FuentesFinanciacion = fnt
                                });
                                saldototal = Convert.ToDecimal(aportanteotroscostos.FirstOrDefault().FuenteFinanciacion.Sum(x => x.ValorFuente));
                            }
                        }

                        #endregion expensas

                    }
                    var contrato = _context.Contrato.Where(x => x.Contratacion.ContratacionId == ListDP.ContratacionId);
                    var fechaContrato = string.Empty;

                    if (contrato.Any())
                    {
                        var fechafi = contrato.Select(x => x.FechaFirmaContrato).FirstOrDefault();
                        fechaContrato = Convert.ToDateTime(fechafi).ToString("dd/MM/yyyy");

                    }

                    string nombreEntidad = string.Empty;
                    string contratoNumero = !contrato.Any() ? string.Empty : contrato.Select(x => x.NumeroContrato).FirstOrDefault().ToString();
                    if (contrato.Any())
                    {
                        var contratacion = _context.Contratacion.Find(contrato.FirstOrDefault().ContratacionId);
                        var contratista = _context.Contratista.Find(contratacion.ContratistaId);
                        nombreEntidad = contratista == null ? string.Empty : contratista.Nombre;
                    }


                    var observaciones = _context.DisponibilidadPresupuestalObservacion.Where(x => x.DisponibilidadPresupuestalId == ListDP.DisponibilidadPresupuestalId && x.EsNovedad == esNovedad && x.NovedadContractualRegistroPresupuestalId == RegistroNovedadId).ToList();
                    string observacionString = !observaciones.Any() ? string.Empty : string.Join("<br><br>", observaciones.Select(x => x.Observacion));
                    var aportant = aportantes.Distinct();
                    var tiporubro = ListDP.TipoSolicitudEspecialCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(ListDP.TipoSolicitudEspecialCodigo, (int)EnumeratorTipoDominio.Tipo_DDP_Espacial) :
                        //si no viene el campo puede ser contratación
                        ListDP.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo ? ConstanStringTipoSolicitudContratacion.proyectoAdministrativo :
                        ConstanStringTipoSolicitudContratacion.contratacion;
                    bool blnEstado = false;

                    //si es administrativo, esta completo, si es tradicional, se verifica contra fuentes gestionadas
                    //2020-11-08 ahora los administrativos y especiales tambien estionan fuentes
                    if (ListDP.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo)
                    {
                        List<int> ddpproyectosId = ListDP.DisponibilidadPresupuestalProyecto.Select(x => x.DisponibilidadPresupuestalProyectoId).ToList();
                        if (_context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalId == ListDP.DisponibilidadPresupuestalId).Count() > 0)
                            blnEstado = true;

                    }
                    else if (ListDP.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Especial)
                    {
                        if (_context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalId == ListDP.DisponibilidadPresupuestalId).Count() > 0)
                            blnEstado = true;
                    }
                    else
                    {
                        List<int> proyectosId = ListDPP.Where(x => x.ProyectoId > 0).Select(x => (int)x.ProyectoId).ToList();
                        List<int> ddpproyectosId = ListDPP.Select(x => (int)x.DisponibilidadPresupuestalProyectoId).ToList();
                        var aportantesEstado = _context.ProyectoAportante.Where(x => proyectosId.Contains(x.ProyectoId)).ToList();
                        //var fuentes = _context.FuenteFinanciacion.Where(x => aportantes.Contains(x.AportanteId)).Count();

                        if (ListDP.EsNovedad != true)
                        {
                            if (_context.GestionFuenteFinanciacion
                                .Where(x => x.DisponibilidadPresupuestalProyectoId != null &&
                                       ddpproyectosId.Contains((int)x.DisponibilidadPresupuestalProyectoId) && x.Eliminado != true && x.EsNovedad != true)
                                .Count() == aportantesEstado.Count())
                                blnEstado = true;
                        }
                    }
                    var plazoContratacion = new PlazoContratacion();

                    if (contratacionDP != null)
                    {
                        plazoContratacion = _context.PlazoContratacion.Find(contratacionDP.PlazoContratacionId);
                    }
                    int existeNovedad = _context.NovedadContractualRegistroPresupuestal.Where(r => r.Eliminado != true && r.DisponibilidadPresupuestalId == ListDP.DisponibilidadPresupuestalId).Count();
                    DetailValidarDisponibilidadPresupuesal detailDisponibilidadPresupuesal = new DetailValidarDisponibilidadPresupuesal
                    {
                        //TODO:Traer estos campos { Tipo de modificacion, Valor despues de la modificacion, Plazo despues de la modificacion, Detalle de la modificacion) => se toma del caso de uso de novedades contractuales
                        Id = ListDP.DisponibilidadPresupuestalId,
                        NumeroSolicitud = ListDP.NumeroSolicitud,
                        TipoSolicitudCodigo = ListDP.TipoSolicitudCodigo,
                        NUmeroSaldoFuente = saldototal,
                        TipoSolicitudText = ListDP.TipoSolicitudCodigo != null ?
                            await _commonService.GetNombreDominioByCodigoAndTipoDominio(ListDP.TipoSolicitudCodigo,
                            (int)EnumeratorTipoDominio.Tipo_Disponibilidad_Presupuestal) : string.Empty,
                        NumeroDDP = ListDP.NumeroDdp,
                        NumeroDRP = ListDP.NumeroDrp,
                        RubroPorFinanciar = ListDP.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Especial ? tiporubro : _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Disponibilidad_Presupuestal
                                         && r.Codigo == ListDP.TipoSolicitudCodigo).FirstOrDefault().Descripcion,
                        Objeto = ListDP.Objeto,
                        ValorSolicitud = ddph != null ? ddph.ValorSolicitud : ListDP.ValorSolicitud,
                        // Si es aproboda por comite tecnico se debe mostrar la fecha en la que fue aprobada. traer desde dbo.[Sesion]
                        FechaComiteTecnico = fechaComitetecnico,
                        NumeroComite = numerocomietetecnico,
                        FechaSolicitud = ListDP.FechaSolicitud,
                        EstadoStr = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud_Presupuestal
                                    && r.Codigo == ListDP.EstadoSolicitudCodigo).FirstOrDefault().Nombre,
                        EstadoSolicitudCodigo = ListDP.EstadoSolicitudCodigo,
                        Plazo = contratacionDP != null ? plazoContratacion?.PlazoMeses.ToString() + " meses / " + plazoContratacion?.PlazoDias.ToString() + " dias" : "",
                        CuentaCarta = ListDP.CuentaCartaAutorizacion,
                        TipoSolicitudEspecial = ListDP.TipoSolicitudEspecialCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(ListDP.TipoSolicitudEspecialCodigo, (int)EnumeratorTipoDominio.Tipo_DDP_Espacial) :
                        //si no viene el campo puede ser contratación
                        ListDP.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo ? ConstanStringTipoSolicitudContratacion.proyectoAdministrativo :
                        ListDP.EsNovedadContractual == null ? ConstanStringTipoSolicitudContratacion.contratacion : !Convert.ToBoolean(ListDP.EsNovedadContractual) ? ConstanStringTipoSolicitudContratacion.contratacion : ConstanStringTipoSolicitudContratacion.novedadContractual,
                        ContratoNumero = contratoNumero,
                        NombreEntidad = nombreEntidad,
                        UrlConSoporte = ListDP.UrlSoporte,
                        Limitacion = ListDP.LimitacionEspecial,
                        /*//*las modificaciones aun no existen*/
                        ValorGestionado = valorGestionado,
                        Observaciones = observacionString,
                        Proyectos = proyecto,
                        FechaContrato = fechaContrato,
                        //Aportantes
                        Aportantes = aportant.ToList(),
                        NumeroRadicado = ListDP.NumeroRadicadoSolicitud,
                        ObservacioensCancelacion = _context.DisponibilidadPresupuestalObservacion.Where(x => x.DisponibilidadPresupuestalId == ListDP.DisponibilidadPresupuestalId && x.EsNovedad == esNovedad && x.NovedadContractualRegistroPresupuestalId == RegistroNovedadId).ToList(),
                        EsNovedad = false,
                        NovedadContractual = ListDP.NovedadContractualId != null ? _context.NovedadContractual.Where(x => x.NovedadContractualId == ListDP.NovedadContractualId).Include(x => x.NovedadContractualDescripcion).FirstOrDefault() : null,
                        EstadoRegistro = blnEstado,
                        SesionComiteSolicitud = sesionComiteSolicitud,
                        ValorTotalDisponibilidad = ListDP.ValorTotalDisponibilidad,
                        TieneNovedad = existeNovedad > 0 ? true : false
                    };

                    ListDetailValidarDisponibilidadPresupuesal.Add(detailDisponibilidadPresupuesal);
                }
            }
            return ListDetailValidarDisponibilidadPresupuesal;
        }

        public async Task<dynamic> GetListAportanteByTipoAportanteByProyectoId(int pProyectoId, int pTipoAportanteId)
        {
            try
            {
                return await _context.ProyectoAportante
                        .Where(r => r.ProyectoId == pProyectoId)
                        .Include(r => r.Aportante)
                        .ThenInclude(r => r.Departamento)
                        .Include(r => r.Aportante)
                        .ThenInclude(r => r.Municipio)
                           .Include(r => r.Aportante)
                        .ThenInclude(r => r.NombreAportante)
                        .Select(r => r.Aportante)
                        .Where(r => r.TipoAportanteId == pTipoAportanteId)
                     .ToListAsync();
            }
            catch (Exception)
            {
                return new CofinanciacionAportante();
            }
        }

        public async Task<Contrato> GetListContatoByNumeroContrato(string pNumeroContrato)
        {
            //Si el aportante es tercero include dominio
            Contrato Contrato = await _context.Contrato
               .Where(r => r.NumeroContrato.Contains(pNumeroContrato) && !(bool)r.Eliminado)
               .Include(r => r.Contratacion)
                  .ThenInclude(r => r.Contratista)
                .Include(r => r.Contratacion)
                  .ThenInclude(r => r.ContratacionProyecto)
                         .ThenInclude(r => r.Proyecto)
                                   .ThenInclude(r => r.ProyectoAportante)
                                          .ThenInclude(r => r.Aportante)
                                                 .ThenInclude(r => r.NombreAportante)

                 //Si el aportante es Et include Depto
                 .Include(r => r.Contratacion)
                  .ThenInclude(r => r.ContratacionProyecto)
                         .ThenInclude(r => r.Proyecto)
                                   .ThenInclude(r => r.ProyectoAportante)
                                          .ThenInclude(r => r.Aportante)
                                                 .ThenInclude(r => r.Departamento)

                 //Si el aportante es Et include mun
                 .Include(r => r.Contratacion)
                  .ThenInclude(r => r.ContratacionProyecto)
                         .ThenInclude(r => r.Proyecto)
                                   .ThenInclude(r => r.ProyectoAportante)
                                          .ThenInclude(r => r.Aportante)
                                                 .ThenInclude(r => r.Municipio)

                 //incluir fuente de financiacion al aportante
                 .Include(r => r.Contratacion)
                  .ThenInclude(r => r.ContratacionProyecto)
                         .ThenInclude(r => r.Proyecto)
                                   .ThenInclude(r => r.ProyectoAportante)
                                          .ThenInclude(r => r.Aportante)
                                                 .ThenInclude(r => r.FuenteFinanciacion)

                 //incluir fuente de DDP al aportante
                 .Include(r => r.Contratacion)
                  .ThenInclude(r => r.ContratacionProyecto)
                         .ThenInclude(r => r.Proyecto)
                                   .ThenInclude(r => r.ProyectoAportante)
                                          .ThenInclude(r => r.Aportante)
                                                 .ThenInclude(r => r.DisponibilidadPresupuestal)
               .FirstOrDefaultAsync();

            foreach (var ContratacionProyecto in Contrato.Contratacion.ContratacionProyecto.Where(r => !(bool)r.Eliminado))
            {
                foreach (var ProyectoAportante in ContratacionProyecto.Proyecto.ProyectoAportante.Where(r => !(bool)r.Eliminado))
                {
                    decimal? SaldoFuentesFinanciacion, SaldoDisponibilidadPresupuestal = 0;
                    SaldoFuentesFinanciacion = ProyectoAportante.Aportante.FuenteFinanciacion.Where(r => !(bool)r.Eliminado).Sum(r => r.ValorFuente);
                    SaldoDisponibilidadPresupuestal = ProyectoAportante.Aportante.DisponibilidadPresupuestal.Where(r => !(bool)r.Eliminado).Sum(r => r.ValorAportante);

                    ProyectoAportante.Aportante.SaldoDisponible = SaldoFuentesFinanciacion - SaldoDisponibilidadPresupuestal;
                }
            }

            return Contrato;
        }

        /*jflorez, versión light del metodo anterior*/
        public async Task<Contrato> GetContratoByNumeroContrato(string pNumeroContrato)
        {

            //Si el aportante es tercero include dominio
            Contrato Contrato = await _context.Contrato
               .Where(r => r.NumeroContrato.Contains(pNumeroContrato) && !(bool)r.Eliminado)
               .Include(r => r.Contratacion)
                  .ThenInclude(r => r.ContratacionProyecto)
               .Include(r => r.Contratacion)
                  .ThenInclude(r => r.Contratista)
               .FirstOrDefaultAsync();
            try
            {
                Contrato.ListAportantes = new List<CofinanicacionAportanteGrilla>();
                foreach (var ContratacionProyecto in Contrato.Contratacion.ContratacionProyecto.Where(r => !(bool)r.Eliminado))
                {
                    var proyectoAportante = _context.ProyectoAportante.Where(x => x.Proyecto.ProyectoId == ContratacionProyecto.ProyectoId && !(bool)x.Eliminado).Include(x => x.Aportante).ThenInclude(x => x.Cofinanciacion).ToList();
                    foreach (var ProyectoAportante in proyectoAportante)
                    {
                        decimal? SaldoFuentesFinanciacion, SaldoDisponibilidadPresupuestal = 0;
                        SaldoFuentesFinanciacion = ProyectoAportante.Aportante.FuenteFinanciacion.Where(r => !(bool)r.Eliminado).Sum(r => r.ValorFuente);
                        SaldoDisponibilidadPresupuestal = ProyectoAportante.Aportante.DisponibilidadPresupuestal.Where(r => !(bool)r.Eliminado).Sum(r => r.ValorAportante);

                        ProyectoAportante.Aportante.SaldoDisponible = SaldoFuentesFinanciacion - SaldoDisponibilidadPresupuestal;
                        var nombre = "";
                        if (ProyectoAportante.Aportante.TipoAportanteId == ConstanTipoAportante.Ffie)
                        {
                            nombre = ConstanStringTipoAportante.Ffie;
                        }
                        else if (ProyectoAportante.Aportante.TipoAportanteId == ConstanTipoAportante.ET)
                        {
                            //verifico si tiene municipio
                            if (ProyectoAportante.Aportante.MunicipioId != null)
                            {
                                nombre = "Alcaldía de " + _context.Localizacion.Find(ProyectoAportante.Aportante.MunicipioId).Descripcion;
                            }
                            else//solo departamento
                            {
                                if (ProyectoAportante.Aportante.DepartamentoId == null)
                                {
                                    nombre = "";
                                }
                                else
                                {
                                    nombre = "Gobernación de " + ProyectoAportante.Aportante.DepartamentoId == null ? "" :
                                    _context.Localizacion.Find(ProyectoAportante.Aportante.DepartamentoId).Descripcion;
                                }

                            }

                        }
                        else
                        {
                            nombre = _context.Dominio.Find(ProyectoAportante.Aportante.NombreAportanteId).Nombre;
                        }
                        CofinanicacionAportanteGrilla cofinanicacionAportanteGrilla = new CofinanicacionAportanteGrilla
                        {
                            CofinanciacionAportanteId = ProyectoAportante.Aportante.CofinanciacionAportanteId,
                            Nombre = nombre,
                            TipoAportante = await _commonService.GetNombreDominioByDominioID((int)ProyectoAportante.Aportante.TipoAportanteId),
                            Vigencia = ProyectoAportante.Aportante.Cofinanciacion.VigenciaCofinanciacionId,
                            FechaCreacion = ProyectoAportante.Aportante.FechaCreacion,
                            MunicipioId = ProyectoAportante.Aportante.MunicipioId,
                            DepartamentoId = ProyectoAportante.Aportante.DepartamentoId,
                            RegistroCompleto = ProyectoAportante.Aportante.Cofinanciacion.RegistroCompleto,
                            TipoAportanteId = ProyectoAportante.Aportante.TipoAportanteId
                        };
                        if (Contrato.ListAportantes.Where(x => x.CofinanciacionAportanteId == ProyectoAportante.Aportante.CofinanciacionAportanteId).Count() == 0)
                        {
                            Contrato.ListAportantes.Add(cofinanicacionAportanteGrilla);
                        }
                    }
                }
                return Contrato;
            }
            catch (Exception e)
            {
                return Contrato;
            }
            //return Contrato;
        }

        public async Task<DisponibilidadPresupuestal> GetDisponibilidadPresupuestalByID(int id)
        {
            DisponibilidadPresupuestal disponibilidadPresupuestal = await _context.DisponibilidadPresupuestal
                .Where(r => r.DisponibilidadPresupuestalId == id)
                   .Include(r => r.DisponibilidadPresupuestalObservacion)
                   .Include(r => r.DisponibilidadPresupuestalProyecto)
                    .ThenInclude(r => r.Proyecto)
                      .ThenInclude(r => r.ProyectoAportante)
                        .ThenInclude(r => r.Aportante)
                .FirstOrDefaultAsync();

            return disponibilidadPresupuestal;
        }
        //Avance compromisos
        public async Task<Respuesta> CreateOrEditReportProgress(CompromisoSeguimiento compromisoSeguimiento)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Seguimiento_Compromiso, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                string strCrearEditar;
                if (string.IsNullOrEmpty(compromisoSeguimiento.CompromisoSeguimientoId.ToString()) || compromisoSeguimiento.CompromisoSeguimientoId == 0)
                {
                    //Auditoria
                    strCrearEditar = "REGISTRAR AVANCE COMPROMISOS";
                    compromisoSeguimiento.FechaCreacion = DateTime.Now;
                    compromisoSeguimiento.UsuarioCreacion = compromisoSeguimiento.UsuarioCreacion;
                    compromisoSeguimiento.SesionParticipanteId = compromisoSeguimiento.SesionParticipanteId;
                    compromisoSeguimiento.Eliminado = false;
                    _context.CompromisoSeguimiento.Add(compromisoSeguimiento);
                }
                else
                {
                    strCrearEditar = "EDIT AVANCE COMPROMISOS";
                    CompromisoSeguimiento compromisoSeguimientoAntiguo = _context.CompromisoSeguimiento.Find(compromisoSeguimiento.CompromisoSeguimientoId);

                    //Auditoria
                    compromisoSeguimientoAntiguo.UsuarioModificacion = compromisoSeguimiento.UsuarioModificacion;
                    compromisoSeguimientoAntiguo.Eliminado = false;


                    //Registros
                    compromisoSeguimientoAntiguo.DescripcionSeguimiento = compromisoSeguimiento.DescripcionSeguimiento;
                    compromisoSeguimientoAntiguo.TemaCompromisoId = compromisoSeguimiento.TemaCompromisoId;
                    compromisoSeguimientoAntiguo.SesionParticipanteId = compromisoSeguimiento.SesionParticipanteId;
                    _context.CompromisoSeguimiento.Update(compromisoSeguimiento);

                }

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = compromisoSeguimiento,
                    Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, compromisoSeguimiento.UsuarioCreacion, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = compromisoSeguimiento,
                    Code = ConstantMessagesSesionComiteTema.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, compromisoSeguimiento.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }

        public async Task<List<ListAportantes>> GetAportantesByProyectoId(int proyectoId)
        {
            try
            {
                return await (
                                from cp in _context.ContratacionProyecto
                                join ct in _context.Contratacion on cp.ContratacionId equals ct.ContratacionId
                                join p in _context.Proyecto on cp.ProyectoId equals p.ProyectoId
                                join ctp in _context.ContratacionProyectoAportante on cp.ContratacionProyectoId equals ctp.ContratacionProyectoId
                                join cf in _context.CofinanciacionAportante on ctp.CofinanciacionAportanteId equals cf.CofinanciacionAportanteId
                                where p.ProyectoId == proyectoId
                                select new ListAportantes
                                {
                                    ContratacionId = ct.ContratacionId,
                                    ContratacionProyectoId = ctp.ContratacionProyectoId,
                                    CofinanciacionAportanteId = cf.CofinanciacionAportanteId,
                                    ContratacionProyectoAportanteId = ctp.ContratacionProyectoAportanteId,
                                    TipoAportanteId = cf.TipoAportanteId,
                                    NombreAportanteId = cf.NombreAportanteId,
                                    NombreAportante = _context.Dominio.Where(r => (bool)r.Activo &&
                                                                             r.DominioId.Equals(cf.NombreAportanteId) &&
                                                                             r.TipoDominioId == (int)EnumeratorTipoDominio.Nombre_Aportante_Aportante)
                                                                       .Select(r => r.Nombre).FirstOrDefault(),
                                    ValorAporte = ctp.ValorAporte,
                                    TipoAportanteText = _context.Dominio
                                    .Where(r => (bool)r.Activo && r.DominioId
                                    .Equals(cf.TipoAportanteId) && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_aportante)
                                    .Select(r => r.Nombre).FirstOrDefault(),
                                }).ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //Aportantes por proyectoId
        public async Task<List<ListAdminProyect>> GetAportantesByProyectoAdministrativoId(int proyectoId)
        {
            try
            {
                using SqlConnection sql = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand("GetAportantesByProyectoId", sql)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.Add("@ProyectoId", SqlDbType.Int).Value = proyectoId;
                var response = new List<ListAdminProyect>();
                await sql.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        response.Add(MapToValueProyectoAdmnistrativo(reader));
                    }
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Crear solciitud proyecto administrativo
        public async Task<Respuesta> CreateOrEditProyectoAdministrtivo(DisponibilidadPresupuestal disponibilidad)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Seguimiento_Compromiso, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;
            DisponibilidadPresupuestal disponibilidadPresupuestalAntiguo = null;
            int countMaxId = _context.DisponibilidadPresupuestal.Max(p => p.DisponibilidadPresupuestalId);
            try
            {

                if (string.IsNullOrEmpty(disponibilidad.DisponibilidadPresupuestalId.ToString()) || disponibilidad.DisponibilidadPresupuestalId == 0)
                {
                    //Auditoria
                    strCrearEditar = "CREAR SOLICITUD DISPONIBILIDAD PRESUPUESTAL";
                    disponibilidad.FechaCreacion = DateTime.Now;
                    disponibilidad.UsuarioCreacion = disponibilidad.UsuarioCreacion;
                    disponibilidad.NumeroSolicitud = Helpers.Helpers.Consecutive("PA", countMaxId);
                    disponibilidad.FechaSolicitud = DateTime.Now;
                    disponibilidad.OpcionContratarCodigo = "";
                    disponibilidad.EstadoSolicitudCodigo = "8";
                    disponibilidad.Eliminado = false;

                    disponibilidad.DisponibilidadPresupuestalProyecto.ToList().ForEach(pro =>
                    {
                        pro.UsuarioCreacion = disponibilidad.UsuarioCreacion;
                        pro.FechaCreacion = disponibilidad.FechaCreacion;


                    });

                    _context.DisponibilidadPresupuestal.Add(disponibilidad);
                }
                else
                {
                    strCrearEditar = "EDIT SOLICITUD DISPONIBILIDAD PRESUPUESTAL";
                    disponibilidadPresupuestalAntiguo = _context.DisponibilidadPresupuestal.Find(disponibilidad.DisponibilidadPresupuestalId);
                    //Auditoria
                    disponibilidadPresupuestalAntiguo.UsuarioModificacion = disponibilidad.UsuarioModificacion;
                    disponibilidadPresupuestalAntiguo.Eliminado = false;


                    //Registros
                    disponibilidadPresupuestalAntiguo.Objeto = disponibilidad.Objeto;
                    disponibilidadPresupuestalAntiguo.AportanteId = disponibilidad.AportanteId;
                    //valor solicitud
                    disponibilidadPresupuestalAntiguo.ValorSolicitud = disponibilidad.ValorSolicitud;

                    disponibilidad.DisponibilidadPresupuestalProyecto.ToList().ForEach(pro =>
                    {
                        if (string.IsNullOrEmpty(pro.DisponibilidadPresupuestalProyectoId.ToString()) || pro.DisponibilidadPresupuestalProyectoId == 0)
                        {
                            DisponibilidadPresupuestalProyecto proyecto = new DisponibilidadPresupuestalProyecto();

                            proyecto.UsuarioModificacion = disponibilidad.UsuarioCreacion;
                            proyecto.FechaModificacion = disponibilidad.FechaModificacion;

                            proyecto.DisponibilidadPresupuestalId = disponibilidad.DisponibilidadPresupuestalId;
                            proyecto.ProyectoAdministrativoId = pro.ProyectoAdministrativoId;

                            disponibilidadPresupuestalAntiguo.DisponibilidadPresupuestalProyecto.Add(proyecto);
                        }
                        else
                        {
                            DisponibilidadPresupuestalProyecto proyecto = _context.DisponibilidadPresupuestalProyecto.Find(pro.DisponibilidadPresupuestalProyectoId);

                            proyecto.UsuarioModificacion = disponibilidad.UsuarioCreacion;
                            proyecto.FechaModificacion = disponibilidad.FechaModificacion;
                            proyecto.ProyectoAdministrativoId = pro.ProyectoAdministrativoId;
                        }

                    });


                }

                _context.SaveChanges();

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = disponibilidad,
                    Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.DisponibilidadPresupuestal, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, disponibilidad.UsuarioCreacion, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesSesionComiteTema.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.DisponibilidadPresupuestal, ConstantMessagesSesionComiteTema.Error, idAccion, disponibilidad.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }

        //Registrar informacion Adicional en una solicitud
        public async Task<Respuesta> CreateOrEditInfoAdditional(DisponibilidadPresupuestal pDisponibilidad, string user)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Registrar_Informacion_Adicional_Solicitud, (int)EnumeratorTipoDominio.Acciones);
            DisponibilidadPresupuestal disponibilidadPresupuestal = null;
            string strCrearEditar = string.Empty;

            try
            {
                disponibilidadPresupuestal = await _context.DisponibilidadPresupuestal.FindAsync(pDisponibilidad.DisponibilidadPresupuestalId);
                if (disponibilidadPresupuestal != null)
                {
                    pDisponibilidad.UsuarioModificacion = user;
                    pDisponibilidad.FechaModificacion = DateTime.Now;

                    disponibilidadPresupuestal.Objeto = pDisponibilidad.Objeto;
                    //disponibilidadPresupuestal.AportanteId = pdo.aportanteId;
                    _context.DisponibilidadPresupuestal.Update(disponibilidadPresupuestal);

                }
                else
                {
                    pDisponibilidad.UsuarioCreacion = user;
                    pDisponibilidad.FechaCreacion = DateTime.Now;
                    pDisponibilidad.Eliminado = false;
                    _context.DisponibilidadPresupuestal.Add(pDisponibilidad);
                    _context.SaveChanges();
                    /*
                     * jflorez, no estoy seguro de esto pero en estos ddp tradicionales no se estaba relacionando los proyectos que lo comprenden
                     */
                    var contratosproyecto = _context.ContratacionProyecto.Where(x => x.ContratacionId == pDisponibilidad.ContratacionId && !(bool)x.Eliminado).ToList();
                    foreach (var contratoproyecto in contratosproyecto)
                    {
                        _context.DisponibilidadPresupuestalProyecto.Add(
                            new DisponibilidadPresupuestalProyecto
                            {
                                DisponibilidadPresupuestalId = pDisponibilidad.DisponibilidadPresupuestalId,
                                Eliminado = false,
                                FechaCreacion = DateTime.Now,
                                ProyectoId = contratoproyecto.ProyectoId,
                                UsuarioCreacion = user
                            });
                    }
                }
                _context.SaveChanges();
                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = disponibilidadPresupuestal,
                    Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.DisponibilidadPresupuestal, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, user, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesSesionComiteTema.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.DisponibilidadPresupuestal, ConstantMessagesSesionComiteTema.Error, idAccion, user, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }

        //Registrar informacion Adicional en una solicitud
        public async Task<Respuesta> CreateOrEditInfoAdditionalNoveltly(NovedadContractualRegistroPresupuestal pRegistro, int pContratacionId, string user)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Registrar_Informacion_Adicional_Solicitud, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;

            try
            {
                if (pRegistro.PlazoDias > 30)
                    return respuesta = new Respuesta { IsSuccessful = false, Data = null, Code = ConstantMessagesSesionComiteTema.Error, Message = "El valor ingresado en dias no puede superior a 30" };

                DisponibilidadPresupuestal disponibilidadPresupuestal = _context.DisponibilidadPresupuestal
                                                                                    .Where(x => x.ContratacionId == pContratacionId && x.Eliminado != true)
                                                                                    .FirstOrDefault();

                NovedadContractualRegistroPresupuestal novedadContractualRegistroPresupuestal = await _context.NovedadContractualRegistroPresupuestal.FindAsync(pRegistro.NovedadContractualRegistroPresupuestalId);
                if (novedadContractualRegistroPresupuestal != null)
                {
                    novedadContractualRegistroPresupuestal.UsuarioModificacion = user;
                    novedadContractualRegistroPresupuestal.FechaModificacion = DateTime.Now;

                    novedadContractualRegistroPresupuestal.Objeto = pRegistro.Objeto;
                    novedadContractualRegistroPresupuestal.PlazoDias = pRegistro.PlazoDias;
                    novedadContractualRegistroPresupuestal.PlazoMeses = pRegistro.PlazoMeses;

                    _context.NovedadContractualRegistroPresupuestal.Update(novedadContractualRegistroPresupuestal);

                }
                else
                {
                    pRegistro.UsuarioCreacion = user;
                    pRegistro.FechaCreacion = DateTime.Now;

                    pRegistro.Eliminado = false;
                    pRegistro.DisponibilidadPresupuestalId = disponibilidadPresupuestal.DisponibilidadPresupuestalId;
                    _context.NovedadContractualRegistroPresupuestal.Add(pRegistro);
                    _context.SaveChanges();
                    /*
                     * jflorez, no estoy seguro de esto pero en estos ddp tradicionales no se estaba relacionando los proyectos que lo comprenden
                     */

                }
                _context.SaveChanges();
                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = disponibilidadPresupuestal,
                    Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.DisponibilidadPresupuestal, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, user, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesSesionComiteTema.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.DisponibilidadPresupuestal, ConstantMessagesSesionComiteTema.Error, idAccion, user, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }

        //Ver detalle
        public async Task<DisponibilidadPresupuestal> GetDetailInfoAdditionalById(int disponibilidadPresupuestalId)
        {
            try
            {
                DisponibilidadPresupuestal disponibilidad = await _context.DisponibilidadPresupuestal
                                                .Where(dp => dp.DisponibilidadPresupuestalId == disponibilidadPresupuestalId)
                                                .Include(r => r.DisponibilidadPresupuestalObservacion)
                                                .Include(r => r.DisponibilidadPresupuestalProyecto)
                                                   .ThenInclude(r => r.Proyecto)
                                                   .ThenInclude(r => r.LocalizacionIdMunicipioNavigation)
                                                .Include(r => r.Aportante)
                                                   .ThenInclude(r => r.NombreAportante)
                                                .Include(r => r.Aportante)
                                                   .ThenInclude(r => r.Departamento)
                                                .Include(r => r.Aportante)
                                                   .ThenInclude(r => r.Municipio)
                                                .OrderByDescending(r => r.DisponibilidadPresupuestalId).FirstOrDefaultAsync();

                foreach (var DisponibilidadPresupuestalProyecto in disponibilidad.DisponibilidadPresupuestalProyecto)
                {
                    if (DisponibilidadPresupuestalProyecto.Proyecto != null)
                    {
                        if (!string.IsNullOrEmpty(DisponibilidadPresupuestalProyecto.Proyecto.LocalizacionIdMunicipio))
                        {
                            Localizacion Municipio = _context.Localizacion.Find(DisponibilidadPresupuestalProyecto.Proyecto.LocalizacionIdMunicipio);
                            DisponibilidadPresupuestalProyecto.Proyecto.MunicipioObj = Municipio;
                            DisponibilidadPresupuestalProyecto.Proyecto.DepartamentoObj = _context.Localizacion.Find(Municipio.IdPadre);
                        }
                    }
                }

                return disponibilidad;
            }
            catch (Exception e)
            {

            }

            return new DisponibilidadPresupuestal();
        }

        //Registrar nueva solicitud DDp Especial 
        public async Task<Respuesta> CreateOrEditDDPRequest(DisponibilidadPresupuestal disponibilidadPresupuestal)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Solicitud_DDP_Especial, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;
            DisponibilidadPresupuestal disponibilidadPresupuestalAntiguo = null;
            int countMaxId = _context.DisponibilidadPresupuestal.Max(p => p.DisponibilidadPresupuestalId);

            try
            {
                DisponibilidadPresupuestalProyecto entity = new DisponibilidadPresupuestalProyecto();



                if (disponibilidadPresupuestal.DisponibilidadPresupuestalId == 0)
                {
                    //Auditoria

                    strCrearEditar = "";
                    disponibilidadPresupuestal.FechaCreacion = DateTime.Now;
                    disponibilidadPresupuestal.UsuarioCreacion = disponibilidadPresupuestal.UsuarioCreacion;
                    disponibilidadPresupuestal.Eliminado = false;

                    disponibilidadPresupuestal.NumeroSolicitud = Helpers.Helpers.Consecutive("DE", countMaxId);
                    disponibilidadPresupuestal.OpcionContratarCodigo = "Validando";
                    disponibilidadPresupuestal.ValorSolicitud = 0;
                    disponibilidadPresupuestal.EstadoSolicitudCodigo = ConstanCodigoSolicitudDisponibilidadPresupuestal.Sin_Registrar;
                    disponibilidadPresupuestal.FechaSolicitud = DateTime.Now;
                    disponibilidadPresupuestal.TipoSolicitudCodigo = disponibilidadPresupuestal.TipoSolicitudCodigo;
                    disponibilidadPresupuestal.Objeto = disponibilidadPresupuestal.Objeto;
                    disponibilidadPresupuestal.NumeroRadicadoSolicitud = disponibilidadPresupuestal.NumeroRadicadoSolicitud;
                    disponibilidadPresupuestal.RegistroCompleto = true;

                    disponibilidadPresupuestal.DisponibilidadPresupuestalProyecto.ToList().ForEach(p =>
                    {
                        if (p.DisponibilidadPresupuestalProyectoId == 0)
                        {
                            entity.ProyectoId = p.ProyectoId;
                            entity.DisponibilidadPresupuestalId = disponibilidadPresupuestal.DisponibilidadPresupuestalId;
                            entity.FechaCreacion = DateTime.Now;
                            entity.UsuarioCreacion = disponibilidadPresupuestal.UsuarioCreacion;
                            entity.Eliminado = false;

                            disponibilidadPresupuestal.DisponibilidadPresupuestalProyecto.Add(entity);
                        }

                    });

                    _context.DisponibilidadPresupuestal.Add(disponibilidadPresupuestal);

                }
                else
                {
                    strCrearEditar = "EDITAR SOLICITUD DDP ESPECIAL";
                    disponibilidadPresupuestalAntiguo = _context.DisponibilidadPresupuestal.Find(disponibilidadPresupuestal.DisponibilidadPresupuestalId);

                    //Auditoria
                    disponibilidadPresupuestalAntiguo.UsuarioModificacion = disponibilidadPresupuestal.UsuarioModificacion;

                    //Registros

                    disponibilidadPresupuestalAntiguo.ValorSolicitud = disponibilidadPresupuestal.ValorSolicitud;
                    disponibilidadPresupuestalAntiguo.Objeto = disponibilidadPresupuestal.Objeto;
                    disponibilidadPresupuestalAntiguo.TipoSolicitudCodigo = disponibilidadPresupuestal.TipoSolicitudCodigo;
                    disponibilidadPresupuestalAntiguo.AportanteId = disponibilidadPresupuestal.AportanteId;
                    disponibilidadPresupuestalAntiguo.NumeroRadicadoSolicitud = disponibilidadPresupuestal.NumeroRadicadoSolicitud;
                    disponibilidadPresupuestalAntiguo.CuentaCartaAutorizacion = disponibilidadPresupuestal.CuentaCartaAutorizacion;

                    disponibilidadPresupuestal.DisponibilidadPresupuestalProyecto.ToList().ForEach(p =>
                    {
                        if (p.DisponibilidadPresupuestalProyectoId == 0)
                        {
                            entity.ProyectoId = p.ProyectoId;
                            entity.DisponibilidadPresupuestalId = disponibilidadPresupuestal.DisponibilidadPresupuestalId;
                            entity.FechaCreacion = DateTime.Now;
                            entity.UsuarioCreacion = disponibilidadPresupuestal.UsuarioCreacion;
                            entity.Eliminado = false;

                            disponibilidadPresupuestalAntiguo.DisponibilidadPresupuestalProyecto.Add(entity);
                        }
                        else
                        {
                            entity = _context.DisponibilidadPresupuestalProyecto.Find(p.DisponibilidadPresupuestalProyectoId);

                            entity.UsuarioModificacion = disponibilidadPresupuestal.UsuarioCreacion;
                            entity.FechaModificacion = DateTime.Now;
                            entity.ProyectoId = p.ProyectoId;
                        }

                    });

                }

                var result = await _context.SaveChangesAsync();

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = disponibilidadPresupuestal,
                    Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.DisponibilidadPresupuestal, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, disponibilidadPresupuestal.UsuarioCreacion, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = disponibilidadPresupuestal,
                    Code = ConstantMessagesSesionComiteTema.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, disponibilidadPresupuestal.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }

        //Registrar Otros Costos/Servicio
        public async Task<Respuesta> CreateOrEditServiceCosts(DisponibilidadPresupuestal disponibilidadPresupuestal, int proyectoId)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Solicitud_DDP_OtrosCostosServicio, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;
            DisponibilidadPresupuestal disponibilidadPresupuestalAntiguo = null;
            int countMaxId = _context.DisponibilidadPresupuestal.Max(p => p.DisponibilidadPresupuestalId);

            try
            {

                if (string.IsNullOrEmpty(disponibilidadPresupuestal.DisponibilidadPresupuestalId.ToString()) || disponibilidadPresupuestal.DisponibilidadPresupuestalId == 0)
                {
                    //Auditoria

                    strCrearEditar = "REGISTRAR SOLICITUD DDP OTROS COSTOS SERVICIOS";
                    disponibilidadPresupuestal.FechaCreacion = DateTime.Now;
                    disponibilidadPresupuestal.UsuarioCreacion = disponibilidadPresupuestal.UsuarioCreacion;
                    disponibilidadPresupuestal.Eliminado = false;

                    disponibilidadPresupuestal.NumeroSolicitud = Helpers.Helpers.Consecutive("DE", countMaxId);
                    disponibilidadPresupuestal.OpcionContratarCodigo = "Validando";
                    disponibilidadPresupuestal.ValorSolicitud = 0;
                    disponibilidadPresupuestal.TipoSolicitudCodigo = "1";
                    disponibilidadPresupuestal.EstadoSolicitudCodigo = "8"; // Sin registrar => TipoDominioId = 39
                    disponibilidadPresupuestal.FechaSolicitud = DateTime.Now;
                    disponibilidadPresupuestal.Objeto = disponibilidadPresupuestal.Objeto;
                    disponibilidadPresupuestal.NumeroRadicadoSolicitud = disponibilidadPresupuestal.NumeroRadicadoSolicitud;
                    disponibilidadPresupuestal.RegistroCompleto = true;
                    _context.DisponibilidadPresupuestal.Add(disponibilidadPresupuestal);
                    var result = await _context.SaveChangesAsync();

                    var newDisponibilidadPresupuestalId = disponibilidadPresupuestal.DisponibilidadPresupuestalId;

                    if (result > 0)
                    {
                        DisponibilidadPresupuestalProyecto entity = new DisponibilidadPresupuestalProyecto
                        {
                            ProyectoId = proyectoId,
                            DisponibilidadPresupuestalId = newDisponibilidadPresupuestalId,
                            FechaCreacion = DateTime.Now,
                            UsuarioCreacion = disponibilidadPresupuestal.UsuarioCreacion,
                            Eliminado = false
                        };

                        _context.DisponibilidadPresupuestalProyecto.Add(entity);
                    }

                }
                else
                {
                    strCrearEditar = "EDIT SOLICITUD DDP ESPECIAL";
                    disponibilidadPresupuestalAntiguo = _context.DisponibilidadPresupuestal.Find(disponibilidadPresupuestal.DisponibilidadPresupuestalId);

                    //Auditoria
                    disponibilidadPresupuestalAntiguo.UsuarioModificacion = disponibilidadPresupuestal.UsuarioModificacion;
                    disponibilidadPresupuestalAntiguo.Eliminado = false;


                    //Registros
                    disponibilidadPresupuestal.OpcionContratarCodigo = disponibilidadPresupuestal.OpcionContratarCodigo;
                    disponibilidadPresupuestal.ValorSolicitud = disponibilidadPresupuestal.ValorSolicitud;
                    disponibilidadPresupuestal.NumeroSolicitud = disponibilidadPresupuestal.NumeroSolicitud;
                    disponibilidadPresupuestal.FechaSolicitud = disponibilidadPresupuestal.FechaSolicitud;
                    disponibilidadPresupuestalAntiguo.Objeto = disponibilidadPresupuestal.Objeto;
                    disponibilidadPresupuestal.EstadoSolicitudCodigo = disponibilidadPresupuestal.EstadoSolicitudCodigo;
                    disponibilidadPresupuestalAntiguo.TipoSolicitudCodigo = disponibilidadPresupuestal.TipoSolicitudCodigo;
                    disponibilidadPresupuestalAntiguo.NumeroRadicadoSolicitud = disponibilidadPresupuestal.NumeroRadicadoSolicitud;
                    _context.DisponibilidadPresupuestal.Update(disponibilidadPresupuestalAntiguo);
                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = disponibilidadPresupuestal,
                    Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.DisponibilidadPresupuestal, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, disponibilidadPresupuestal.UsuarioCreacion, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = disponibilidadPresupuestal,
                    Code = ConstantMessagesSesionComiteTema.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, disponibilidadPresupuestal.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }

        //Enviar solicitud
        public async Task<Respuesta> SendRequest(int disponibilidadPresupuestalId, int RegistroPId, bool esNovedad, string user, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {

            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Enviar_Solicitud_A_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            DisponibilidadPresupuestal disponibilidadPresupuestal = null;
            NovedadContractualRegistroPresupuestal novedadContractualRegistroPresupuestal = null;

            try
            {
                if (esNovedad == true)
                {
                    novedadContractualRegistroPresupuestal = _context.NovedadContractualRegistroPresupuestal.Find(RegistroPId);

                    if (novedadContractualRegistroPresupuestal != null)
                    {
                        novedadContractualRegistroPresupuestal.FechaModificacion = DateTime.Now;
                        novedadContractualRegistroPresupuestal.UsuarioModificacion = user;
                        novedadContractualRegistroPresupuestal.EstadoSolicitudCodigo = "1"; // Se cambia el estado a "En validación presupuestal" => TipoDominioId = 39
                        _context.NovedadContractualRegistroPresupuestal.Update(novedadContractualRegistroPresupuestal);

                        disponibilidadPresupuestal = _context.DisponibilidadPresupuestal.Find(disponibilidadPresupuestalId);
                    }
                }
                else
                {
                    disponibilidadPresupuestal = _context.DisponibilidadPresupuestal.Find(disponibilidadPresupuestalId);

                    if (disponibilidadPresupuestal != null)
                    {
                        strCrearEditar = "ENVIAR SOLICITUD A DOSPONIBILIDAD PRESUPUESAL";
                        disponibilidadPresupuestal.FechaModificacion = DateTime.Now;
                        disponibilidadPresupuestal.UsuarioModificacion = user;
                        disponibilidadPresupuestal.EstadoSolicitudCodigo = "1"; // Se cambia el estado a "En validación presupuestal" => TipoDominioId = 39
                        _context.DisponibilidadPresupuestal.Update(disponibilidadPresupuestal);
                    }
                }



                if (disponibilidadPresupuestal != null || novedadContractualRegistroPresupuestal != null)
                {

                    Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.ddpSolicitudCreada);

                    string ncontrato = "";
                    string fechaContrato = "";
                    if (disponibilidadPresupuestal.ContratacionId > 0)
                    {
                        var contrato = _context.Contratacion.Find(disponibilidadPresupuestal.ContratacionId);
                        ncontrato = contrato.NumeroSolicitud;
                        fechaContrato = contrato.FechaTramite != null ? Convert.ToDateTime(contrato.FechaTramite).ToString("dd/MM/yyy") : "";
                    }
                    string tiposolicut = _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Disponibilidad_Presupuestal && x.Codigo == disponibilidadPresupuestal.TipoSolicitudCodigo).FirstOrDefault().Nombre;
                    string template = TemplateRecoveryPassword.Contenido
                        .Replace("[NumeroContrato]", ncontrato)
                        .Replace("_LinkF_", pDominioFront)
                        .Replace("[FechaContrato]", fechaContrato)
                        .Replace("[NumeroSolicitud]", disponibilidadPresupuestal.NumeroSolicitud)
                        .Replace("[TipoDeSolicitud]", tiposolicut);


                    var usuariosadmin = _context.UsuarioPerfil
                        .Where(x => x.PerfilId == (int)EnumeratorPerfil.Financiera)
                        .Include(y => y.Usuario)
                        .ToList();

                    foreach (var usuarioadmin in usuariosadmin)
                    {
                        bool blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuarioadmin.Usuario.Email, "Solicitud de disponibilidad presupuetal creada", template, pSender, pPassword, pMailServer, pMailPort);
                    }
                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = disponibilidadPresupuestal,
                    Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.DisponibilidadPresupuestal, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, disponibilidadPresupuestal.UsuarioCreacion, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = disponibilidadPresupuestal,
                    Code = ConstantMessagesSesionComiteTema.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, disponibilidadPresupuestal.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> EliminarDisponibilidad(int disponibilidadPresupuestalId)
        {

            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.eliminar_Solicitud_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            DisponibilidadPresupuestal disponibilidadPresupuestal = null;

            try
            {

                disponibilidadPresupuestal = await _context.DisponibilidadPresupuestal.Where(d => d.DisponibilidadPresupuestalId == disponibilidadPresupuestalId).FirstOrDefaultAsync();


                if (disponibilidadPresupuestal != null)
                {

                    strCrearEditar = "Eliminar DISPONIBILIDAD PRESUPUESAL";
                    disponibilidadPresupuestal.FechaModificacion = DateTime.Now;
                    disponibilidadPresupuestal.UsuarioCreacion = disponibilidadPresupuestal.UsuarioCreacion;
                    disponibilidadPresupuestal.Eliminado = true;
                    _context.SaveChanges();
                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = disponibilidadPresupuestal,
                    Code = ConstantMessagesDisponibilidadPresupuesta.EliminacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.DisponibilidadPresupuestal, ConstantMessagesDisponibilidadPresupuesta.EliminacionExitosa, idAccion, disponibilidadPresupuestal.UsuarioCreacion, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = disponibilidadPresupuestal,
                    Code = ConstantMessagesSesionComiteTema.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, disponibilidadPresupuestal.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        //Grilla DDP Especial
        public async Task<List<DisponibilidadPresupuestal>> GetDDPEspecial()
        {
            return await (
                            from dp in _context.DisponibilidadPresupuestal
                            where dp.NumeroSolicitud.Contains("DE_") && !(bool)dp.Eliminado
                            select new DisponibilidadPresupuestal
                            {
                                DisponibilidadPresupuestalId = dp.DisponibilidadPresupuestalId,
                                FechaSolicitud = dp.FechaSolicitud,
                                NumeroSolicitud = dp.NumeroSolicitud,
                                ValorSolicitud = dp.ValorSolicitud,
                                EstadoSolicitudCodigo = dp.EstadoSolicitudCodigo,
                                RegistroCompleto = dp.RegistroCompleto,
                                EstadoSolicitudNombre = (dp.EstadoSolicitudCodigo == null || dp.EstadoSolicitudCodigo == "0" )  && string.IsNullOrEmpty(dp.TipoSolicitudCodigo) ? "Sin Registro" : (dp.EstadoSolicitudCodigo == null || dp.EstadoSolicitudCodigo == "0") && !string.IsNullOrEmpty(dp.TipoSolicitudCodigo) ? "En proceso de registro" : _context.Dominio.Where(r => (bool)r.Activo &&
                                                                             r.Codigo.Equals(dp.EstadoSolicitudCodigo) &&
                                                                             r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud_Disponibilidad_Presupuestal)
                                                                       .Select(r => r.Nombre).FirstOrDefault(),

                            }).OrderByDescending(r => r.DisponibilidadPresupuestalId).ToListAsync();
        }

        public async Task<List<DisponibilidadPresupuestal>> GetDDPAdministrativa()
        {
            return await (
                            from dp in _context.DisponibilidadPresupuestal
                            where dp.NumeroSolicitud.Contains("PA_") && dp.Eliminado != true
                            select new DisponibilidadPresupuestal
                            {
                                DisponibilidadPresupuestalId = dp.DisponibilidadPresupuestalId,
                                FechaSolicitud = dp.FechaSolicitud,
                                NumeroSolicitud = dp.NumeroSolicitud,
                                ValorSolicitud = dp.ValorSolicitud,
                                EstadoSolicitudCodigo = dp.EstadoSolicitudCodigo,
                                RegistroCompleto = dp.RegistroCompleto,
                                Objeto = dp.Objeto,
                                EstadoSolicitudNombre = _context.Dominio.Where(r => (bool)r.Activo &&
                                                                             r.Codigo.Equals(dp.EstadoSolicitudCodigo) &&
                                                                             r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud_Disponibilidad_Presupuestal)
                                                                       .Select(r => r.Nombre).FirstOrDefault(),

                            }).OrderByDescending(r => r.DisponibilidadPresupuestalId).ToListAsync();
        }

        //Solicitudes de comite tecnico
        public async Task<List<CustonReuestCommittee>> GetReuestCommittee()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetBudgetAvailabilityRequest", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<CustonReuestCommittee>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToValue(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public CustonReuestCommittee MapToValue(SqlDataReader reader)
        {
            return new CustonReuestCommittee()
            {
                ContratacionId = (int)reader["ContratacionId"],
                DisponibilidadPresupuestalId = (int)reader["DisponibilidadPresupuestalId"],
                SesionComiteSolicitudId = (int)reader["SesionComiteSolicitudId"],
                FechaSolicitud = (DateTime)reader["FechaSolicitud"],
                TipoSolicitudText = reader["TipoSolicitudText"].ToString(),
                NumeroSolicitud = reader["NumeroSolicitud"].ToString(),
                OpcionContratar = reader["OpcionContratar"].ToString(),
                ValorSolicitud = (decimal)reader["ValorSolicitud"],
                FechaComite = (DateTime)reader["FechaComite"],
                EstadoSolicitudText = reader["EstadoSolicitudText"].ToString(),
                NovedadContractualId = (int)reader["NovedadContractualId"],
                EsNovedad = (bool)reader["EsNovedad"],
                NovedadContractualRegistroPresupuestalId = (int)reader["NovedadContractualRegistroPresupuestalId"],
            };
        }

        public ListAdminProyect MapToValueProyectoAdmnistrativo(SqlDataReader reader)
        {
            return new ListAdminProyect()
            {
                ProyectoId = (int)reader["ProyectoId"],
                ValorAporte = (decimal)reader["ValorAporte"],
                AportanteId = (int)reader["AportanteId"],
                ValorFuente = (decimal)reader["ValorFuente"],
                NombreAportanteId = (int)reader["NombreAportanteId"],
                NombreAportante = reader["NombreAportante"].ToString(),
                FuenteAportanteId = (int)reader["FuenteAportanteId"],
                FuenteRecursosCodigo = reader["FuenteRecursosCodigo"].ToString(),
                NombreFuente = reader["NombreFuente"].ToString(),
            };
        }

        //Lista Consecutivo admnistrativo
        public async Task<List<ListConcecutivoProyectoAdministrativo>> GetListCocecutivoProyecto()
        {
            try
            {
                var proyectoadminapo = await _context.ProyectoAdministrativoAportante.Where(x => (bool)x.ProyectoAdminstrativo.Enviado).ToListAsync();
                List<ListConcecutivoProyectoAdministrativo> proyectoreturn = new List<ListConcecutivoProyectoAdministrativo>();
                foreach (var proy in proyectoadminapo)
                {
                    List<GrillaFuentesFinanciacion> fuentes = new List<GrillaFuentesFinanciacion>();

                    List<AportanteFuenteFinanciacion> aportantefuente = new List<AportanteFuenteFinanciacion>();
                    foreach (var apor in _context.AportanteFuenteFinanciacion.
                        Include(x => x.FuenteFinanciacion)
                        .ThenInclude(x => x.Aportante).Where(x => x.ProyectoAdministrativoAportanteId == proy.ProyectoAdministrativoAportanteId
                          && !(bool)x.FuenteFinanciacion.Eliminado
                           && !(bool)x.Eliminado).ToList())
                    {
                        apor.FuenteFinanciacionString = _context.Dominio.Where(x => x.Codigo == apor.FuenteFinanciacion.FuenteRecursosCodigo
                            && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion)?.FirstOrDefault()?.Nombre;
                        if (apor.FuenteFinanciacion.Aportante.TipoAportanteId.Equals(ConstanTipoAportante.Ffie))
                        {
                            apor.NombreAportanteString = ConstanStringTipoAportante.Ffie;
                        }
                        else if (apor.FuenteFinanciacion.Aportante.TipoAportanteId.Equals(ConstanTipoAportante.Tercero))
                        {
                            apor.NombreAportanteString = apor.FuenteFinanciacion.Aportante.NombreAportanteId == null
                                ? string.Empty :
                                _context.Dominio.Find(apor.FuenteFinanciacion.Aportante.NombreAportanteId).Nombre;
                        }
                        else
                        {
                            if (apor.FuenteFinanciacion.Aportante.MunicipioId == null)
                            {
                                apor.FuenteFinanciacion.Aportante.NombreAportanteString = apor.FuenteFinanciacion.Aportante.DepartamentoId == null
                                ? string.Empty :
                                "Gobernación " + _context.Localizacion.Find(apor.FuenteFinanciacion.Aportante.DepartamentoId).Descripcion;
                            }
                            else
                            {
                                apor.NombreAportanteString = apor.FuenteFinanciacion.Aportante.MunicipioId == null
                                ? string.Empty :
                                "Alcaldía " + _context.Localizacion.Find(apor.FuenteFinanciacion.Aportante.MunicipioId).Descripcion;
                            }
                        }

                        aportantefuente.Add(apor);
                    }
                    proyectoreturn.Add(new ListConcecutivoProyectoAdministrativo
                    {
                        ProyectoId = proy.ProyectoAdminstrativoId,
                        Concecutivo = Helpers.Helpers.Consecutive("D4", proy.ProyectoAdminstrativoId),
                        NombreAportante = ConstanStringTipoAportante.Ffie,
                        AportanteFuenteFinanciacion = aportantefuente,//_context.AportanteFuenteFinanciacion.Where(x => x.ProyectoAdministrativoAportanteId == proy.ProyectoAdministrativoAportanteId).ToList(),
                        ListaAportantes = _context.Dominio.Find(proy.AportanteId)
                    });
                }
                return proyectoreturn;
            }

            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<Proyecto>> SearchLlaveMEN(string LlaveMEN)
        {
            var Id = await _context.Proyecto
                .Where(r => r.LlaveMen == LlaveMEN.ToString().Trim() && !(bool)r.Eliminado)
                .Select(r => r.LlaveMen).FirstOrDefaultAsync();
            if (!string.IsNullOrEmpty(Id))
            {

                var proyectos = await _context.Proyecto
                .Where(r => r.LlaveMen.Equals(Id))
                .Include(r => r.InstitucionEducativa)
                .Include(r => r.Sede)
                .ToListAsync();
                foreach (var proyecto in proyectos)
                {
                    proyecto.tipoIntervencionString = _context.Dominio.Where(x => x.Codigo == proyecto.TipoIntervencionCodigo
                          && x.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).FirstOrDefault().Nombre;
                }
                return proyectos;
            }
            else
                return new List<Proyecto>();
        }

        //LCT- traer todas las llave men
        public async Task<List<Proyecto>> GetListLlaveMen()
        {
            return await _context.Proyecto.Where(r => r.Eliminado != true).ToListAsync();
        }

        //plantlla - rubro por financiar es infraestructura y el tipo de solicitud es contratación
        public async Task<HTMLContent> GetHTMLString(DetailValidarDisponibilidadPresupuesal obj)
        {
            try
            {
                //var  obj = await detailValidarDisponibilidadPresupuesal
                //DisponibilidadPresupuestal ListDP =  _context.DisponibilidadPresupuestal.Where(r => !r.Eliminado).FirstOrDefault();
                var sb = new StringBuilder();
                sb.Append(@"
                       <?xml version='1.0' encoding='utf-8'?>
                        <!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.1//EN' 'http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd'>
                        <html xmlns='http://www.w3.org/1999/xhtml'>

                        <head>
                            <meta http-equiv='Content-Type' content='application/xhtml+xml; charset=utf-8' />
                            <title>DOC</title>
                            <link href='assets/pdf-styles.css' type='text/css' rel='stylesheet' />
                        </head>
                            <body>
                                  <div class='Section0'>
                        <p class='Footer' style='text-align:center;'>
                            <img src='./assets/img-FFIE.png' width='89' height='77' align='right' alt='' />
                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>F</span>
                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>
                                    ONDO DE FINANCIAMIENTO DE INFRAESTRUCTURA EDUCATIVA - FFIE</span>
                        </p>
                        <p class='Footer' style='text-align:center;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>
                                NIT. 900900129-8</span>
                        </p>
                        <p class='Footer' style='text-align:center;'>
                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>
                                MINISTERIO DE EDUCACIÓN
                        </span>
                        </p>
                        <p class='Header' style='text-align:right;'>
                            <span style='font-family:Times New Roman;font-size:12pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'> &#xa0;
                    </span>
                </p>
                <div>
            <table cellspacing='0' style='margin-left:19.6pt;width: auto; border-collapse: collapse; '>
                <tr style='height: 31.4666672px'>
                    <td style='vertical-align:middle;background-color:#F2F2F2;border-top:none;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:926.0667px;' colspan='10'>
                        <h2><span style='font-family:Arial;font-size:11pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>DOCUMENTO DE DISPONIBILIDAD PRESUPUESTAL</span></h2>
                    </td>
                </tr>
                <tr style='height: 9.2px'>
                    <td style='vertical-align:middle;border-top:none;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:926.0667px;' colspan='10'>
                        <h2><span style='font-family:Arial;font-size:11pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>&#xa0;</span></h2>
                    </td>
                </tr>

                <tr style='height: 29.6px'>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:321.2667px;'
                        colspan='3'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Fecha: </span><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'> </span>");

                sb.AppendLine("<span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + DateTime.Now.ToString("MM/dd/yyyy") + "");
                sb.Append(@"</span>
                        </h2>
                    </td>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:311.8667px;'
                        colspan='4'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Número de solicitud: </span>");
                sb.AppendLine("<span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + obj.NumeroSolicitud + "");
                sb.Append(@"</span>
                           
                           
                        </h2>
                    </td>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:292.9333px;'
                        colspan='3'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>DDP No: </span>");
                sb.AppendLine("<span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + obj.NumeroDDP + "");
                sb.Append(@"</span>

                        </h2>
                    </td>
                </tr>
                <tr style='height: 4.66666651px'>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:321.2667px;'
                        colspan='3'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>&#xa0;</span></h2>
                    </td>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:311.8667px;'
                        colspan='4'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>&#xa0;</span></h2>
                    </td>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:292.9333px;'
                        colspan='3'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>&#xa0;</span></h2>
                    </td>
                </tr>
                <tr style='height: 20.8000011px'>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:321.2667px;'
                        colspan='3'>
                        <p style='text-align:left;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Rubro por financiar: </span>");
                sb.AppendLine("<span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + "?" + "");
                sb.Append(@"</span>

                        </p>
                    </td>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:311.8667px;'
                        colspan='4'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Tipo de Solicitud</span>");
                sb.AppendLine("<span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + obj.TipoSolicitudText + "");
                sb.Append(@"</span>
                        </h2>
                    </td>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:292.9333px;'
                        colspan='3'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Opción por contratar: </span>");
                sb.AppendLine("<span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + obj.OpcionPorContratar + "");
                sb.Append(@"</span>
                        </h2>
                    </td>
                </tr>
                <tr style='height: 18.5333328px'>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:321.2667px;'
                        colspan='3'>
                        <p style='text-align:left;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Fecha comité técnico: </span>");
                sb.AppendLine("<span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + Convert.ToDateTime(obj.FechaComiteTecnico).ToString("MM/dd/yyyy") + "");
                sb.Append(@"</span>
                        </p>
                    </td>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:311.8667px;'
                        colspan='4'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Número comité:</span>");
                sb.AppendLine("<span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + "?" + "");
                sb.Append(@"</span>
                        </h2>
                    </td>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:292.9333px;'
                        colspan='3'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>&#xa0;</span></h2>
                    </td>
                </tr>
                <tr style='height: 30.5333328px'>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:926.0667px;'
                        colspan='10'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Objeto:</span>");
                sb.AppendLine("<span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + obj.Objeto + "");
                sb.Append(@"</span>
                        </h2>
                    </td>
                </tr>
                <tr style='height: 26.2666683px'>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:926.0667px;' colspan='10'>
                        <p class='Body-Text' style='text-align:center;line-height:115.833328%;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>Se expide el presente documento de disponibilidad presupuestal para la construcción de infraestr</span>
                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>uctura educativa, conforme a lo señalado en el artículo 59 de la ley 1753 de 2015 Plan Nacional de Desarrollo </span>
                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:italic;font-variant:normal;line-height:115.833328%;'>&quot;</span>
                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:italic;font-variant:normal;line-height:115.833328%;'>T</span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:italic;font-variant:normal;line-height:115.833328%;'>odos por un nuevo </span>
                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:italic;font-variant:normal;line-height:115.833328%;'>país</span>
                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:italic;font-variant:normal;line-height:115.833328%;'>&quot;</span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'> y lo dispuesto en el conpes 3831 del Plan Nacional de Infraestructura Educativa.</span></p>
                        <h2><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>&#xa0;</span></h2>
                    </td>
                </tr>
                <tr style='height: 56.86667px'>
                    <td style='vertical-align:middle;border-top:none;border-left:none;border-right:none;border-bottom:none;padding-top:0pt;padding-bottom:0pt;width:926.0667px;' colspan='10'>
                        <div>
                            <table cellspacing='0' style='width: 100%; border-collapse: collapse; '>
                                <tr style='height: 2px'>
                                    <td style='vertical-align:middle;background-color:#E7E6E6;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:209.9333px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>N</span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>ombre </span>
                                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>del a</span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>portante</span></p>
                                    </td>
                                    <td style='vertical-align:middle;background-color:#E7E6E6;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:302px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>Fuente</span></p>
                                    </td>
                                    <td style='vertical-align:middle;background-color:#E7E6E6;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:398.7333px;'
                                        colspan='2'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>Valor sol</span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>icitado de la fuente</span></p>
                                    </td>
                                </tr>
                                <tr style='height: 2px'>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:209.9333px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;line-height:115.833328%;'>Funcionalidad </span><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;line-height:115.833328%;'>Solicitar disponibilidad presupuestal” - Aportante</span></p>
                                    </td>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:302px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;line-height:115.833328%;'>El sistema deberá </span><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;line-height:115.833328%;'>calcular el valor total de la fuente, realizando la sumatoria de los valores solicitados para todos los proyectos asociados al contrato</span>
                                            <span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;line-height:115.833328%;'>,</span><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;line-height:115.833328%;'> por fuente y por aportante, enunciados en el cuadro inferior.</span></p>
                                    </td>
                                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:198.4667px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;line-height:115.833328%;'>Funcionalidad </span><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;line-height:115.833328%;'>Solicitar disponibilidad presupuestal” – Valor del aportante</span></p>
                                    </td>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:200.2667px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='color:#BFBFBF;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>Cantidad en letra</span><span style='color:#BFBFBF;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>s (pesos moneda corriente)</span></p>
                                    </td>
                                </tr>
                                <tr style='height: 19.5333328px'>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:209.9333px;'>
                                        <p class='Body-Text' style='text-align:center;line-height:115.833328%;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>&#xa0;</span></p>
                                    </td>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:302px;'>
                                        <p class='Body-Text' style='text-align:center;line-height:115.833328%;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>&#xa0;</span></p>
                                    </td>
                                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:198.4667px;'>
                                        <p class='Body-Text' style='text-align:center;line-height:115.833328%;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>&#xa0;</span></p>
                                    </td>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:200.2667px;'>
                                        <p class='Body-Text' style='text-align:center;line-height:115.833328%;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>&#xa0;</span></p>
                                    </td>
                                </tr>
                                <tr style='height: 2px'>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:209.9333px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>&#xa0;</span></p>
                                    </td>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:302px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>&#xa0;</span></p>
                                    </td>
                                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:198.4667px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>&#xa0;</span></p>
                                    </td>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:200.2667px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>&#xa0;</span></p>
                                    </td>
                                </tr>
                                <tr style='height: 2px'>
                                    <td style='vertical-align:middle;background-color:#D9D9D9;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:511.9333px;'
                                        colspan='2'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>Total</span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'> de recu</span>
                                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>r</span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>sos con disponibilidad presupuestal</span></p>
                                    </td>
                                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:198.4667px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='color:#BFBFBF;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>Cantidad en formato</span><span style='color:#BFBFBF;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'> ($)</span></p>
                                    </td>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:200.2667px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='color:#BFBFBF;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>Cantidad en letras (pesos moneda corriente)</span></p>
                                    </td>
                                </tr>
                                <tr style='height:0px;'>
                                    <td style='width:209.9333px;border:none;padding:0pt;' />
                                    <td style='width:302px;border:none;padding:0pt;' />
                                    <td style='width:198.4666px;border:none;padding:0pt;' />
                                    <td style='width:200.2667px;border:none;padding:0pt;' />
                                </tr>
                            </table>
                        </div>
                        <p class='Body-Text' style='text-align:center;line-height:115.833328%;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>&#xa0;</span></p>
                    </td>
                </tr>
                <tr style='height: 34.4666672px'>
                    <td style='vertical-align:middle;border-top:none;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:926.0667px;' colspan='10'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>Se certifica que a la fecha de expedición del presente documento existen </span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>los recursos para darle cumplimiento al objeto. Este recurso se compromete conforme a la siguiente información:</span></p>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'>En </span><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'>el siguiente cuadro se debe mostrar la información que se encuentra en la funcionalidad Validar disponibilidad de presupuesto para ejecución de proyecto”</span></p>
                    </td>
                </tr>
                <tr style='height: 26.0666656px'>
                    <td style='vertical-align:middle;background-color:#E7E6E6;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:106.3333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'> Nombre del aportante</span>");
                sb.AppendLine("<span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + "");
                sb.Append(@"</span>
                        </p>
                    </td>
                    <td style='vertical-align:middle;background-color:#E7E6E6;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Fue</span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>nte</span></p>
                    </td>
                    <td style='vertical-align:middle;background-color:#E7E6E6;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:122.8667px;'
                        colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Saldo actual de la fuente</span></p>
                    </td>
                    <td style='vertical-align:middle;background-color:#E7E6E6;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:113.4px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Valor </span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>solicitado de la fuente</span></p>
                    </td>
                    <td style='vertical-align:middle;background-color:#E7E6E6;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:132.2667px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Nuevo saldo de la fuente</span></p>
                    </td>
                </tr>
                <tr style='height: 19.5333328px'>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:127.5333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:115.8px;'
                        colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:106.3333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:122.8667px;'
                        colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:113.4px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:132.2667px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                </tr>
                <tr style='height: 19.5333328px'>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:127.5333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:115.8px;'
                        colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:106.3333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:122.8667px;'
                        colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:113.4px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:132.2667px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                </tr>
                <tr style='height: 6.33333349px'>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:127.5333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:115.8px;'
                        colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:106.3333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:122.8667px;'
                        colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:113.4px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:132.2667px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                </tr>
                <tr style='height: 26.0666656px'>
                    <td style='vertical-align:middle;background-color:#D9D9D9;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Limitación especial</span></p>
                    </td>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:822.1333px;'
                        colspan='9'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'>Funcionalidad </span><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'>Solicitar </span>
                            <span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'>disponibilidad presupuestal</span><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'>” </span>
                            <span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'>–</span>
                            <span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'> </span><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'>Para los DDP Especiales</span>
                            <span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'>”</span>
                        </p>
                    </td>
                </tr>
                <tr style='height: 5.4666667px'>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:127.5333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:115.8px;' colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:106.3333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:122.8667px;'
                        colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:113.4px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:132.2667px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                </tr>
                <tr style='height: 44.7333336px'>
                    <td style='vertical-align:top;border-top:none;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top:none;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:127.5333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top:none;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:115.8px;' colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top:none;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:106.3333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:middle;border-top:none;border-left:none;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>Firma Director</span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'> (a) Financiera</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:368.5333px;'
                        colspan='4'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                </tr>

            </div>");



                sb.Append(@"
                              
                            </body>
                        </html>");

                obj.htmlContent = sb;
                return new HTMLContent(sb.ToString());
            }
            catch (Exception e)
            {
                return new HTMLContent("");
            }

        }

        //Grilla disponibilidad presupuestal.
        public async Task<ActionResult<List<GrillaValidarDisponibilidadPresupuesal>>> GetBudgetavailabilityRequests()
        {
            List<DisponibilidadPresupuestal> ListDP = await _context.DisponibilidadPresupuestal.Where(r => !r.Eliminado).ToListAsync();
            List<GrillaValidarDisponibilidadPresupuesal> ListGrillaDisponibilidadPresupuestal = new List<GrillaValidarDisponibilidadPresupuesal>();

            foreach (var validacionPresupuestal in ListDP)
            {
                GrillaValidarDisponibilidadPresupuesal disponibilidadPresupuestal = new GrillaValidarDisponibilidadPresupuesal
                {
                    Id = validacionPresupuestal.DisponibilidadPresupuestalId,
                    FechaSolicitud = validacionPresupuestal.FechaSolicitud,
                    NumeroSolicitud = validacionPresupuestal.NumeroSolicitud,
                    TipoSolicitudCodigo = validacionPresupuestal.TipoSolicitudCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(validacionPresupuestal.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_de_Solicitud) : "",
                    EstadoRegistro = (bool)validacionPresupuestal.RegistroCompleto,
                    EstadoRegistroText = (bool)validacionPresupuestal.RegistroCompleto ? "Completo" : "Incompleto"
                };

                ListGrillaDisponibilidadPresupuestal.Add(disponibilidadPresupuestal);
            }

            return ListGrillaDisponibilidadPresupuestal;
        }

        private async Task<List<DetailValidarDisponibilidadPresupuesal>> GetDetailAvailabilityBudgetProyectNovelty(int RegistroNovedadId, bool esGenerar, bool esLiberacion)
        {
            List<DetailValidarDisponibilidadPresupuesal> ListDetailValidarDisponibilidadPresupuesal = new List<DetailValidarDisponibilidadPresupuesal>();
            List<GestionFuenteFinanciacion> ListGestionFuenteFinanciacion = _context.GestionFuenteFinanciacion.ToList();
            List<ProyectoAportante> ListProyectoAportante = _context.ProyectoAportante.ToList();
            List<NovedadContractualAportante> ListProyectoAportantexNovedad = _context.NovedadContractualAportante.ToList();

            NovedadContractualRegistroPresupuestal detailDP = _context.NovedadContractualRegistroPresupuestal
                                                                        .Where(x => x.NovedadContractualRegistroPresupuestalId == RegistroNovedadId)
                                                                        .Include(x => x.NovedadContractual)
                                                                            .ThenInclude(x => x.NovedadContractualAportante)
                                                                                .ThenInclude(x => x.ComponenteAportanteNovedad)
                                                                                    .ThenInclude(x => x.ComponenteFuenteNovedad)
                                                                                        .ThenInclude(x => x.ComponenteUsoNovedad)
                                                                        .Include(x => x.DisponibilidadPresupuestal)
                                                                            .ThenInclude(x => x.Contratacion)
                                                                        .Include(x => x.DisponibilidadPresupuestal)
                                                                        .Include(x => x.NovedadContractual)
                                                                            .ThenInclude(x => x.NovedadContractualAportante)
                                                                                .ThenInclude(x => x.CofinanciacionAportante)
                                                                        .FirstOrDefault();

            NovedadContractualRegistroPresupuestalHistorico detailDPH = _context.NovedadContractualRegistroPresupuestalHistorico
                                                            .Where(x => x.NovedadContractualRegistroPresupuestalId == RegistroNovedadId)
                                                            .FirstOrDefault();
            VDisponibilidadPresupuestal vdpp = null;
            bool ddpGenerado = false;
            //validación multi
            if (detailDP != null)
            {
                if (detailDP.EstadoSolicitudCodigo == "5" || detailDP.EstadoSolicitudCodigo == "8")
                {
                    ddpGenerado = true;
                    vdpp = _context.VDisponibilidadPresupuestal.Where(r => r.NovedadContractualRegistroPresupuestalId == detailDP.NovedadContractualRegistroPresupuestalId && r.EstadoSolicitudCodigo == "10" && r.TieneHistorico == true).FirstOrDefault();
                }
                if (detailDP.DisponibilidadPresupuestal != null)
                {
                    //novedad aplica a proyecto
                    if (detailDP.NovedadContractual.EsAplicadaAcontrato != true)
                    {
                        List<DisponibilidadPresupuestalProyecto> disponibilidadPresupuestalProyectos = _context.DisponibilidadPresupuestalProyecto
                                                                                .Where(r => r.ProyectoId == detailDP.NovedadContractual.ProyectoId && r.DisponibilidadPresupuestalId == detailDP.DisponibilidadPresupuestalId)
                                                                                .Include(x => x.Proyecto)
                                                                                .ToList();
                        detailDP.DisponibilidadPresupuestal.DisponibilidadPresupuestalProyecto = disponibilidadPresupuestalProyectos;
                    }
                    //novedad aplica a contrato
                    else
                    {
                        List<DisponibilidadPresupuestalProyecto> disponibilidadPresupuestalProyectos = _context.DisponibilidadPresupuestalProyecto
                                                                                .Where(r => r.DisponibilidadPresupuestalId == detailDP.DisponibilidadPresupuestalId)
                                                                                .Include(x => x.Proyecto)
                                                                                .ToList();
                        detailDP.DisponibilidadPresupuestal.DisponibilidadPresupuestalProyecto = disponibilidadPresupuestalProyectos;
                    }
                }
            }

            List<CofinanicacionAportanteGrilla> aportantes = new List<CofinanicacionAportanteGrilla>();
            List<ProyectoGrilla> proyecto = new List<ProyectoGrilla>();
            string nombreAportante = "";
            decimal? valorAportate = 0;
            decimal valorGestionado = 0;

            foreach (var proyectospp in detailDP.DisponibilidadPresupuestal.DisponibilidadPresupuestalProyecto)
            {
                List<CofinanicacionAportanteGrilla> aportantesxProyecto = new List<CofinanicacionAportanteGrilla>();
                if (!esLiberacion)
                {
                    valorGestionado += _context.GestionFuenteFinanciacion
                                .Where(x => !(bool)x.Eliminado &&
                                       x.DisponibilidadPresupuestalProyectoId == proyectospp.DisponibilidadPresupuestalProyectoId &&
                                       x.EsNovedad == true &&
                                       x.NovedadContractualRegistroPresupuestalId == detailDP.NovedadContractualRegistroPresupuestalId)
                                .Sum(x => x.ValorSolicitado);
                }
                else
                {
                    List<GestionFuenteFinanciacion> gffL = _context.GestionFuenteFinanciacion
                                                    .Where(x => !(bool)x.Eliminado &&
                                                           x.DisponibilidadPresupuestalProyectoId == proyectospp.DisponibilidadPresupuestalProyectoId &&
                                                           x.EsNovedad == true &&
                                                           x.NovedadContractualRegistroPresupuestalId == detailDP.NovedadContractualRegistroPresupuestalId)
                                                    .ToList();
                    foreach (var gff in gffL)
                    {
                        GestionFuenteFinanciacionHistorico gffh = _context.GestionFuenteFinanciacionHistorico.Where(r => r.GestionFuenteFinanciacionId == gff.GestionFuenteFinanciacionId).FirstOrDefault();
                        if (gffh != null)
                        {
                            valorGestionado += gffh.ValorSolicitado;
                        }
                        else
                        {
                            valorGestionado += gff.ValorSolicitado;
                        }
                    }
                }


                decimal valorgestionado = 0;

                var localizacion = _context.Localizacion.Where(x => x.LocalizacionId == proyectospp.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();

                var sede = _context.InstitucionEducativaSede.Find(proyectospp.Proyecto.SedeId);

                List<GrillaComponentes> grilla = new List<GrillaComponentes>();
                int intaportante = 0;

                detailDP.NovedadContractual.NovedadContractualAportante = detailDP.NovedadContractual.NovedadContractualAportante.Where(x => x.Eliminado != true).ToList();

                foreach (var ppapor in detailDP.NovedadContractual.NovedadContractualAportante)
                {
                    NovedadContractualAportanteHistorico ncah = _context.NovedadContractualAportanteHistorico.Where(r => r.NovedadContractualAportanteId == ppapor.NovedadContractualAportanteId).FirstOrDefault();
                    
                    List<GrillaFuentesFinanciacion> fuentes = new List<GrillaFuentesFinanciacion>();

                    ppapor.ComponenteAportanteNovedad = ppapor.ComponenteAportanteNovedad.Where(x => x.Eliminado != true && x.NovedadContractualAportanteId == ppapor.NovedadContractualAportanteId).ToList();
                    
                    foreach (var componente in ppapor.ComponenteAportanteNovedad)
                    {
                        componente.ComponenteFuenteNovedad = componente.ComponenteFuenteNovedad.Where(x => x.Eliminado != true && x.ComponenteAportanteNovedadId == componente.ComponenteAportanteNovedadId).ToList();

                        List<string> uso = new List<string>();
                        List<decimal> usovalor = new List<decimal>();
                        decimal total = 0;

                        foreach (var font in componente.ComponenteFuenteNovedad)
                        {
                            FuenteFinanciacion fuente = _context.FuenteFinanciacion.Find(font.FuenteFinanciacionId);
                            font.ComponenteUsoNovedad = font.ComponenteUsoNovedad.Where(x => x.Eliminado != true && x.ComponenteFuenteNovedadId == font.ComponenteFuenteNovedadId).ToList();
                            var fuenteSaldo = _context.VSaldosFuenteXaportanteId.Where(r => r.CofinanciacionAportanteId == componente.CofinanciacionAportanteId && r.FuenteFinanciacionId == font.FuenteFinanciacionId).FirstOrDefault();
                            //el saldo de la fuente realmente es lo que tengo en control de recursos
                            //var saldo = _context.ControlRecurso.Where(x => x.FuenteFinanciacionId == font.FuenteFinanciacionId).Sum(x=>x.ValorConsignacion);
                            decimal saldo = Convert.ToDecimal(_context.FuenteFinanciacion.Where(x => x.FuenteFinanciacionId == font.FuenteFinanciacionId).Sum(x => x.ValorFuente));
                            GestionFuenteFinanciacion fuenteRegistro = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado &&
                                                       x.DisponibilidadPresupuestalProyectoId == proyectospp.DisponibilidadPresupuestalProyectoId &&
                                                       x.EsNovedad == true &&
                                                       x.NovedadContractualRegistroPresupuestalId == detailDP.NovedadContractualRegistroPresupuestalId && x.FuenteFinanciacionId == font.FuenteFinanciacionId).FirstOrDefault();

                            decimal valorsolicitadoxotros = _context.GestionFuenteFinanciacion
                                .Where(x => !(bool)x.Eliminado &&
                                                       x.DisponibilidadPresupuestalProyectoId == proyectospp.DisponibilidadPresupuestalProyectoId &&
                                                       x.EsNovedad == true &&
                                                       x.NovedadContractualRegistroPresupuestalId == detailDP.NovedadContractualRegistroPresupuestalId &&
                                       x.FuenteFinanciacionId == font.FuenteFinanciacionId)
                                .Sum(x => x.ValorSolicitado);

                            var gestionAlGuardar = _context.GestionFuenteFinanciacion
                                    .Where(x => !(bool)x.Eliminado &&
                                                       x.DisponibilidadPresupuestalProyectoId == proyectospp.DisponibilidadPresupuestalProyectoId &&
                                                       x.EsNovedad == true &&
                                                       x.NovedadContractualRegistroPresupuestalId == detailDP.NovedadContractualRegistroPresupuestalId &&
                                        x.FuenteFinanciacionId == font.FuenteFinanciacionId)
                                    .FirstOrDefault();
                            string codigoFuente = !string.IsNullOrEmpty(font.FuenteRecursosCodigo) ? font.FuenteRecursosCodigo : fuente != null ? fuente.FuenteRecursosCodigo : null;
                            var funtename = _context.Dominio.Where(x => x.Codigo == codigoFuente
                                      && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion);

                            string namefuente = funtename.Any() ? funtename.FirstOrDefault().Nombre : "";
                            if (!esGenerar && ddpGenerado != true)
                            {
                                GestionFuenteFinanciacionHistorico gffh = null;
                                if (esLiberacion)
                                    gffh = _context.GestionFuenteFinanciacionHistorico.Where(r => r.GestionFuenteFinanciacionId == fuenteRegistro.GestionFuenteFinanciacionId).FirstOrDefault();

                                fuentes.Add(new GrillaFuentesFinanciacion
                                {
                                    Fuente = namefuente,
                                    Estado_de_las_fuentes = "",
                                    FuenteFinanciacionID = font.FuenteFinanciacionId,
                                    Valor_solicitado_de_la_fuente = fuenteRegistro != null ? gffh != null ? gffh.ValorSolicitado : fuenteRegistro.ValorSolicitado: 0,
                                    Nuevo_saldo_de_la_fuente = fuenteRegistro != null ? gffh != null ? gffh.NuevoSaldo :  fuenteRegistro.NuevoSaldo : 0,
                                    Saldo_actual_de_la_fuente = fuenteRegistro != null ? gffh != null ? gffh.SaldoActual :  fuenteRegistro.SaldoActual : 0,
                                    Nuevo_saldo_de_la_fuente_al_guardar = fuenteRegistro != null ? gffh != null ? gffh.NuevoSaldo : fuenteRegistro.NuevoSaldo : 0,
                                    Saldo_actual_de_la_fuente_al_guardar = fuenteRegistro != null ? gffh != null ? gffh.SaldoActual :  fuenteRegistro.SaldoActual : 0,
                                });
                            }else if (ddpGenerado == true)
                            {
                                GestionFuenteFinanciacionHistorico gffh = null;
                                if (esLiberacion)
                                    gffh = _context.GestionFuenteFinanciacionHistorico.Where(r => r.GestionFuenteFinanciacionId == fuenteRegistro.GestionFuenteFinanciacionId).FirstOrDefault();

                                fuentes.Add(new GrillaFuentesFinanciacion
                                {
                                    Fuente = namefuente,
                                    Estado_de_las_fuentes = "",
                                    FuenteFinanciacionID = font.FuenteFinanciacionId,
                                    Valor_solicitado_de_la_fuente = fuenteRegistro != null ? gffh != null ? gffh.ValorSolicitado : fuenteRegistro.ValorSolicitadoGenerado ?? fuenteRegistro.ValorSolicitado : 0,
                                    Nuevo_saldo_de_la_fuente = fuenteRegistro != null ? gffh != null ? gffh.NuevoSaldo : fuenteRegistro.NuevoSaldoGenerado ??  fuenteRegistro.NuevoSaldo : 0,
                                    Saldo_actual_de_la_fuente = fuenteRegistro != null ? gffh != null ? gffh.SaldoActual : fuenteRegistro.SaldoActualGenerado ?? fuenteRegistro.SaldoActual : 0,
                                    Nuevo_saldo_de_la_fuente_al_guardar = fuenteRegistro != null ? gffh != null ? gffh.NuevoSaldo : fuenteRegistro.NuevoSaldoGenerado ?? fuenteRegistro.NuevoSaldo : 0,
                                    Saldo_actual_de_la_fuente_al_guardar = fuenteRegistro != null ? gffh != null ? gffh.SaldoActual : fuenteRegistro.SaldoActualGenerado ?? fuenteRegistro.SaldoActual : 0,
                                });
                            }
                            else
                            {
                                var saldoActual = _context.VSaldosFuenteXaportanteId.Where(r => r.CofinanciacionAportanteId == componente.CofinanciacionAportanteId && r.FuenteFinanciacionId == font.FuenteFinanciacionId).FirstOrDefault();
                                decimal valor = saldoActual != null ? (decimal)saldoActual.SaldoActual : 0;
                                fuentes.Add(new GrillaFuentesFinanciacion
                                {
                                    Fuente = namefuente,
                                    Estado_de_las_fuentes = "",
                                    FuenteFinanciacionID = font.FuenteFinanciacionId,
                                    Valor_solicitado_de_la_fuente = fuenteRegistro != null ? fuenteRegistro.ValorSolicitadoGenerado ?? fuenteRegistro.ValorSolicitado: 0,
                                    Nuevo_saldo_de_la_fuente = fuenteRegistro != null ? fuenteRegistro.NuevoSaldoGenerado ?? valor - fuenteRegistro.ValorSolicitado : 0,
                                    Saldo_actual_de_la_fuente = fuenteRegistro != null ? fuenteRegistro.SaldoActualGenerado ?? valor : 0,
                                    Nuevo_saldo_de_la_fuente_al_guardar = gestionAlGuardar != null ? gestionAlGuardar.NuevoSaldo : 0,
                                    Saldo_actual_de_la_fuente_al_guardar = gestionAlGuardar != null ? gestionAlGuardar.SaldoActual : 0,
                                });
                            }

                            foreach (var componenteUso in font.ComponenteUsoNovedad)
                            {
                                var usos = _context.Dominio.Where(x => x.Codigo == componenteUso.TipoUsoCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Usos).ToList();

                                uso.Add(usos.Count() > 0 ? usos.FirstOrDefault().Nombre : "");
                                if (!esLiberacion)
                                {
                                    usovalor.Add(componenteUso.ValorUso);
                                    total += componenteUso.ValorUso;
                                }
                                else
                                {
                                    ComponenteUsoNovedadHistorico cuh = _context.ComponenteUsoNovedadHistorico.Where(r => r.ComponenteUsoNovedadId == componenteUso.ComponenteUsoNovedadId).FirstOrDefault();
                                    if (cuh != null)
                                    {
                                        usovalor.Add(cuh.ValorUso);
                                        total += cuh.ValorUso;
                                    }
                                    else
                                    {
                                        usovalor.Add(componenteUso.ValorUso);
                                        total += componenteUso.ValorUso;
                                    }
                                }
                            }

                            var dom = _context.Dominio.Where(x => x.Codigo == componente.TipoComponenteCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Componentes).ToList();
                            grilla.Add(
                                new GrillaComponentes
                                {
                                    ComponenteAportanteId = componente.ComponenteAportanteNovedadId,
                                    Componente = dom.Count() > 0 ? dom.FirstOrDefault().Nombre : "",
                                    ComponenteUsoCodigo = componente.TipoComponenteCodigo,
                                    Fase = String.IsNullOrEmpty(componente.FaseCodigo) ? string.Empty : _context.Dominio.Where(r => r.Codigo == componente.FaseCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Fases).FirstOrDefault().Nombre,
                                    Fuente = namefuente,
                                    FuenteFinanciacionId = font.FuenteFinanciacionId,
                                    Uso = uso,
                                    ValorTotal = total,
                                    ValorUso = usovalor,
                                    cofinanciacionAportanteId = ppapor.CofinanciacionAportanteId.Value,
                                    Aportante = getNombreAportante(ppapor.CofinanciacionAportante)
                                });
                        }
                    }

                    decimal valorproyecto = 0;


                    ppapor.ComponenteAportanteNovedad.ToList().ForEach(can =>
                    {
                        can.ComponenteFuenteNovedad.ToList().ForEach(cfn =>
                        {
                            cfn.ComponenteUsoNovedad.ToList().ForEach(cun =>
                            {
                                if (!esLiberacion)
                                {
                                    valorproyecto += cun.ValorUso;
                                }
                                else
                                {
                                    ComponenteUsoNovedadHistorico cuh = _context.ComponenteUsoNovedadHistorico.Where(r => r.ComponenteUsoNovedadId == cun.ComponenteUsoNovedadId).FirstOrDefault();
                                    if(cuh != null)
                                    {
                                        valorproyecto += cuh.ValorUso;
                                    }
                                    else
                                    {
                                        valorproyecto += cun.ValorUso;
                                    }
                                }
                            });
                        });
                    });

                    aportantes.Add(new CofinanicacionAportanteGrilla
                    {
                        CofinanciacionAportanteId = ppapor.CofinanciacionAportanteId.Value,
                        Nombre = getNombreAportante(ppapor.CofinanciacionAportante),
                        TipoAportante = _context.Dominio.Where(r => (bool)r.Activo && r.DominioId.Equals(ppapor.CofinanciacionAportante.TipoAportanteId) && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_aportante).Select(r => r.Nombre).FirstOrDefault(),
                        ValorAportanteAlProyecto = valorproyecto,
                        FuentesFinanciacion = fuentes,

                    });
                    aportantesxProyecto.Add(new CofinanicacionAportanteGrilla
                    {
                        CofinanciacionAportanteId = ppapor.CofinanciacionAportanteId.Value,
                        Nombre = getNombreAportante(ppapor.CofinanciacionAportante),
                        TipoAportante = _context.Dominio.Where(r => (bool)r.Activo && r.DominioId.Equals(ppapor.CofinanciacionAportante.TipoAportanteId) && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_aportante).Select(r => r.Nombre).FirstOrDefault(),
                        ValorAportanteAlProyecto = valorproyecto,
                        FuentesFinanciacion = fuentes,
                        ValorGestionado = _context.GestionFuenteFinanciacion
                                                        .Where(x => !(bool)x.Eliminado &&
                                                               x.DisponibilidadPresupuestalProyectoId == proyectospp.DisponibilidadPresupuestalProyectoId &&
                                                               x.EsNovedad == true &&
                                                               x.NovedadContractualRegistroPresupuestalId == detailDP.NovedadContractualRegistroPresupuestalId &&
                                                               fuentes.Select(x => x.FuenteFinanciacionID).Contains(x.FuenteFinanciacionId)).Sum(x => x.ValorSolicitado)
                    });



                    var confinanciacion = _context.CofinanciacionAportante.Where(x => x.CofinanciacionAportanteId == ppapor.CofinanciacionAportanteId).Include(x => x.CofinanciacionDocumento).FirstOrDefault();
                    var intfuentes = _context.FuenteFinanciacion.Where(y => y.AportanteId == ppapor.CofinanciacionAportanteId).Select(t => t.FuenteFinanciacionId).ToList();

                    if (confinanciacion != null)
                    {
                        intaportante = confinanciacion == null ? 0 : confinanciacion.CofinanciacionAportanteId;
                        nombreAportante = getNombreAportante(confinanciacion);
                        valorAportate = 0;



                        ppapor.ComponenteAportanteNovedad.ToList().ForEach(can =>
                        {
                            can.ComponenteFuenteNovedad.ToList().ForEach(cfn =>
                            {
                                cfn.ComponenteUsoNovedad.ToList().ForEach(cun =>
                                {
                                    if (!esLiberacion)
                                    {
                                        valorAportate += cun.ValorUso;
                                    }
                                    else
                                    {
                                        ComponenteUsoNovedadHistorico cuh = _context.ComponenteUsoNovedadHistorico.Where(r => r.ComponenteUsoNovedadId == cun.ComponenteUsoNovedadId).FirstOrDefault();
                                        if (cuh != null)
                                        {
                                            valorAportate += cuh.ValorUso;

                                        }
                                        else
                                        {
                                            valorAportate += cun.ValorUso;
                                        }
                                    }
                                });
                            });
                        });
                    }
                    valorgestionado = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalProyectoId == proyectospp.DisponibilidadPresupuestalProyectoId && intfuentes.Contains(x.FuenteFinanciacionId)).Sum(x => x.ValorSolicitado);
                }

                proyecto.Add(new ProyectoGrilla
                {
                    LlaveMen = proyectospp.Proyecto.LlaveMen,
                    Departamento = _context.Localizacion.Find(localizacion.IdPadre).Descripcion,
                    Municipio = localizacion.Descripcion,
                    TipoIntervencion = proyectospp.Proyecto.TipoIntervencionCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(proyectospp.Proyecto.TipoIntervencionCodigo, (int)EnumeratorTipoDominio.Tipo_de_Intervencion) : "",
                    InstitucionEducativa = _context.InstitucionEducativaSede.Find(sede.PadreId).Nombre,
                    Sede = sede.Nombre,
                    NombreAportante = nombreAportante,
                    ValorAportante = valorAportate,
                    AportanteID = intaportante,
                    DisponibilidadPresupuestalProyecto = proyectospp.DisponibilidadPresupuestalProyectoId,
                    ValorGestionado = !esLiberacion ? valorgestionado : valorGestionado,
                    ComponenteGrilla = grilla,
                    Aportantes = aportantesxProyecto
                });

                #region busco comite técnico

                //busco comite técnico
                DateTime fechaComitetecnico = DateTime.Now;

                string numerocomietetecnico = "";

                if (detailDP.NovedadContractual != null)
                {
                    var contratacion = _context.SesionComiteSolicitud.Where(x => x.SolicitudId == detailDP.NovedadContractual.NovedadContractualId && x.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Novedad_Contractual).
                        Include(x => x.ComiteTecnico).ToList();
                    if (contratacion.Count() > 0)
                    {
                        numerocomietetecnico = contratacion.FirstOrDefault().ComiteTecnico.NumeroComite;
                        fechaComitetecnico = Convert.ToDateTime(contratacion.FirstOrDefault().ComiteTecnico.FechaOrdenDia);
                    }
                }

                #endregion busco comite técnico

                var contrato = _context.Contrato.Where(x => x.Contratacion.ContratacionId == detailDP.DisponibilidadPresupuestal.ContratacionId);
                var fechaContrato = "";

                if (contrato.Any())
                {
                    var fechafi = contrato.Select(x => x.FechaFirmaContrato).FirstOrDefault();
                    fechaContrato = Convert.ToDateTime(fechafi).ToString("dd/MM/yyyy");

                }

                string nombreEntidad = "";
                string contratoNumero = !contrato.Any() ? "" : contrato.Select(x => x.NumeroContrato).FirstOrDefault().ToString();
                if (contrato.Any())
                {
                    var contratacion = _context.Contratacion.Find(contrato.FirstOrDefault().ContratacionId);
                    var contratista = _context.Contratista.Find(contratacion.ContratistaId);
                    nombreEntidad = contratista == null ? "" : contratista.Nombre;
                }


                var observaciones = _context.DisponibilidadPresupuestalObservacion.Where(x => x.DisponibilidadPresupuestalId == detailDP.DisponibilidadPresupuestalId && x.EsNovedad == true && x.NovedadContractualRegistroPresupuestalId == RegistroNovedadId).ToList();
                string observacionString = !observaciones.Any() ? "" : string.Join("<br><br>", observaciones.Select(x => x.Observacion));
                var aportant = aportantes;
                var tiporubro = detailDP.DisponibilidadPresupuestal.TipoSolicitudEspecialCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(detailDP.DisponibilidadPresupuestal.TipoSolicitudEspecialCodigo, (int)EnumeratorTipoDominio.Tipo_DDP_Espacial) :
                    //si no viene el campo puede ser contratación
                    detailDP.DisponibilidadPresupuestal.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo ? ConstanStringTipoSolicitudContratacion.proyectoAdministrativo :
                    ConstanStringTipoSolicitudContratacion.contratacion;
                bool blnEstado = false;

                //si es administrativo, esta completo, si es tradicional, se verifica contra fuentes gestionadas
                //2020-11-08 ahora los administrativos y especiales tambien estionan fuentes
                if (detailDP.DisponibilidadPresupuestal.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo)
                {
                    List<int> ddpproyectosId = detailDP.DisponibilidadPresupuestal.DisponibilidadPresupuestalProyecto.Select(x => x.DisponibilidadPresupuestalProyectoId).ToList();
                    if (ListGestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalId == detailDP.DisponibilidadPresupuestalId).Count() > 0)
                        blnEstado = true;

                }
                else if (detailDP.DisponibilidadPresupuestal.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Especial)
                {
                    if (ListGestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalId == detailDP.DisponibilidadPresupuestalId).Count() > 0)
                        blnEstado = true;
                }
                else
                {
                    List<int> proyectosId = detailDP.DisponibilidadPresupuestal.DisponibilidadPresupuestalProyecto.Where(x => x.ProyectoId > 0).Select(x => (int)x.ProyectoId).ToList();
                    List<int> ddpproyectosId = detailDP.DisponibilidadPresupuestal.DisponibilidadPresupuestalProyecto.Select(x => (int)x.DisponibilidadPresupuestalProyectoId).ToList();
                    //toma los aportantes de novedadcontractual
                    var aportantesxNovedad = ListProyectoAportantexNovedad.Where(x => x.NovedadContractualId == detailDP.NovedadContractual.NovedadContractualId && x.Eliminado != true).ToList();

                    if (ListGestionFuenteFinanciacion
                        .Where(x => x.DisponibilidadPresupuestalProyectoId != null && x.EsNovedad == true && x.NovedadContractualRegistroPresupuestalId == detailDP.NovedadContractualRegistroPresupuestalId && x.Eliminado != true &&
                               ddpproyectosId.Contains((int)x.DisponibilidadPresupuestalProyectoId))
                        .Count() == aportantesxNovedad.Count() && aportantesxNovedad.Count() > 0)
                        blnEstado = true;

                }

                decimal ValorTotalDisponibilidad = 0;
                if (detailDP.DisponibilidadPresupuestalId != null)
                    ValorTotalDisponibilidad = _commonService.GetValorTotalDisponibilidad((int) detailDP.DisponibilidadPresupuestalId,esLiberacion);
                DetailValidarDisponibilidadPresupuesal detailDisponibilidadPresupuesal = new DetailValidarDisponibilidadPresupuesal
                {
                    //TODO:Traer estos campos { Tipo de modificacion, Valor despues de la modificacion, Plazo despues de la modificacion, Detalle de la modificacion) => se toma del caso de uso de novedades contractuales
                    Id = detailDP.DisponibilidadPresupuestalId.Value,
                    NumeroSolicitud = detailDP.NumeroSolicitud,
                    TipoSolicitudCodigo = detailDP.DisponibilidadPresupuestal.TipoSolicitudCodigo,
                    NUmeroSaldoFuente = 0,
                    TipoSolicitudText = "Novedad Contractual",
                    NumeroDDP = detailDP.DisponibilidadPresupuestal.NumeroDdp,
                    NumeroDRP = detailDP.NumeroDrp,
                    RubroPorFinanciar = detailDP.DisponibilidadPresupuestal.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Especial ? tiporubro : _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Disponibilidad_Presupuestal
                                     && r.Codigo == detailDP.DisponibilidadPresupuestal.TipoSolicitudCodigo).FirstOrDefault().Descripcion,
                    Objeto = detailDP.Objeto,
                    ValorSolicitud = esLiberacion == true ? detailDPH != null ? detailDPH.ValorSolicitud : detailDP.ValorSolicitud : detailDP.ValorSolicitud,
                    // Si es aproboda por comite tecnico se debe mostrar la fecha en la que fue aprobada. traer desde dbo.[Sesion]
                    FechaComiteTecnico = fechaComitetecnico,
                    NumeroComite = numerocomietetecnico,
                    FechaSolicitud = detailDP.DisponibilidadPresupuestal.FechaSolicitud,
                    EstadoStr = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud_Presupuestal
                                && r.Codigo == detailDP.EstadoSolicitudCodigo).FirstOrDefault().Nombre,
                    EstadoSolicitudCodigo = detailDP.EstadoSolicitudCodigo,
                    Plazo = detailDP.PlazoMeses.ToString() + " meses / " + detailDP.PlazoDias.ToString() + " dias",
                    CuentaCarta = detailDP.DisponibilidadPresupuestal.CuentaCartaAutorizacion,
                    TipoSolicitudEspecial = "Novedad Contractual",//detailDP.DisponibilidadPresupuestal.TipoSolicitudEspecialCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(detailDP.DisponibilidadPresupuestal.TipoSolicitudEspecialCodigo, (int)EnumeratorTipoDominio.Tipo_DDP_Espacial) :
                    //si no viene el campo puede ser contratación
                    //detailDP.DisponibilidadPresupuestal.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo ? ConstanStringTipoSolicitudContratacion.proyectoAdministrativo :
                    //detailDP.EsNovedadContractual = true,
                    ContratoNumero = contratoNumero,
                    NombreEntidad = nombreEntidad,
                    UrlConSoporte = detailDP.DisponibilidadPresupuestal.UrlSoporte,
                    Limitacion = detailDP.DisponibilidadPresupuestal.LimitacionEspecial,
                    /*//*las modificaciones aun no existen*/
                    ValorGestionado = valorGestionado,
                    Observaciones = observacionString,
                    Proyectos = proyecto,
                    FechaContrato = fechaContrato,
                    //Aportantes
                    Aportantes = aportant.ToList(),
                    NumeroRadicado = detailDP.DisponibilidadPresupuestal.NumeroRadicadoSolicitud,
                    ObservacioensCancelacion = _context.DisponibilidadPresupuestalObservacion.Where(x => x.DisponibilidadPresupuestalId == detailDP.DisponibilidadPresupuestalId && x.EsNovedad == true && x.NovedadContractualRegistroPresupuestalId == RegistroNovedadId).ToList(),
                    EsNovedad = true,
                    NovedadContractualRegistroPresupuestalId = detailDP.NovedadContractualRegistroPresupuestalId,
                    NovedadContractual = detailDP.NovedadContractualId != null ? _context.NovedadContractual.Where(x => x.NovedadContractualId == detailDP.NovedadContractualId).Include(x => x.NovedadContractualDescripcion).FirstOrDefault() : null,
                    EstadoRegistro = blnEstado,
                    ValorTotalDisponibilidad = ValorTotalDisponibilidad,
                    TieneHistorico = vdpp != null ? true : false

                };
                ListDetailValidarDisponibilidadPresupuesal.Add(detailDisponibilidadPresupuesal);
            }


            return ListDetailValidarDisponibilidadPresupuesal;
        }

        public async Task<List<DetailValidarDisponibilidadPresupuesal>> GetDetailAvailabilityBudgetProyect(int disponibilidadPresupuestalId, bool esNovedad, int RegistroNovedadId)
        {
            List<DetailValidarDisponibilidadPresupuesal> ListDetailValidarDisponibilidadPresupuesal = new List<DetailValidarDisponibilidadPresupuesal>();

            if (esNovedad == true)
            {
                ListDetailValidarDisponibilidadPresupuesal = await GetDetailAvailabilityBudgetProyectNovelty(RegistroNovedadId,false,false);
            }
            else
            {

                List<DisponibilidadPresupuestal> ListDP = await _context.DisponibilidadPresupuestal.
                    Where(r => !r.Eliminado && r.DisponibilidadPresupuestalId == disponibilidadPresupuestalId).
                    Include(x => x.DisponibilidadPresupuestalProyecto).
                        ThenInclude(y => y.Proyecto).
                        ThenInclude(v => v.ProyectoAportante).
                        ThenInclude(c => c.Aportante).
                            ThenInclude(c => c.FuenteFinanciacion).
                    Include(x => x.DisponibilidadPresupuestalObservacion).
                    Include(x => x.Contratacion).ToListAsync();

                decimal saldototal = 0;
                foreach (var detailDP in ListDP)
                {
                    List<CofinanicacionAportanteGrilla> aportantes = new List<CofinanicacionAportanteGrilla>();
                    List<ProyectoGrilla> proyecto = new List<ProyectoGrilla>();
                    string nombreAportante = "";
                    decimal? valorAportate = 0;
                    decimal valorGestionado = 0;
                    foreach (var proyectospp in detailDP.DisponibilidadPresupuestalProyecto)
                    {
                        List<CofinanicacionAportanteGrilla> aportantesxProyecto = new List<CofinanicacionAportanteGrilla>();

                        #region proyecto administrativo
                        if (proyectospp.ProyectoId == null) //proyecto administrativo
                        {//Poner el nuevo ValorSolicitadoGenerado
                            valorGestionado += _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalId == proyectospp.DisponibilidadPresupuestalId).Sum(x => x.ValorSolicitado);
                            int intaportante = 0;
                            var proyectoadministrativo = _context.ProyectoAdministrativo.Where(x => x.ProyectoAdministrativoId == proyectospp.ProyectoAdministrativoId).
                                Include(x => x.ProyectoAdministrativoAportante).ThenInclude(x => x.AportanteFuenteFinanciacion).ThenInclude(x => x.FuenteFinanciacion).ToList();
                            if (proyectoadministrativo.Count() > 0)
                            {

                                foreach (var apo in proyectoadministrativo.FirstOrDefault().ProyectoAdministrativoAportante)
                                {
                                    List<GrillaFuentesFinanciacion> fuentes = new List<GrillaFuentesFinanciacion>();
                                    foreach (var font in apo.AportanteFuenteFinanciacion)
                                    {
                                        //Poner el nuevo ValorSolicitadoGenerado
                                        //el saldo actual de la fuente son todas las solicitudes a la fuentes
                                        var saldofuente = _context.GestionFuenteFinanciacion.
                                            Where(x => x.FuenteFinanciacionId == font.FuenteFinanciacionId
                                            //debo quitar lo que ya tengo gestionado de esta solicitud
                                            && x.DisponibilidadPresupuestalProyectoId != proyectospp.DisponibilidadPresupuestalProyectoId
                                            ).Sum(x => x.ValorSolicitado);

                                        var gestionAlGuardar = _context.GestionFuenteFinanciacion
                                            .Where(x => x.DisponibilidadPresupuestalProyectoId == proyectospp.DisponibilidadPresupuestalProyectoId &&
                                                x.FuenteFinanciacionId == font.FuenteFinanciacionId)
                                            .FirstOrDefault();

                                        var funtename = _context.Dominio.Where(x => x.Codigo == font.FuenteFinanciacion.FuenteRecursosCodigo
                                                  && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion);

                                        string namefuente = funtename.Any() ? funtename.FirstOrDefault().Nombre : "";
                                        fuentes.Add(new GrillaFuentesFinanciacion
                                        {
                                            Fuente = namefuente,
                                            Estado_de_las_fuentes = "",
                                            FuenteFinanciacionID = font.FuenteFinanciacionId,
                                            Valor_solicitado_de_la_fuente = (decimal)font.FuenteFinanciacion.ValorFuente,
                                            Nuevo_saldo_de_la_fuente = 0,
                                            Saldo_actual_de_la_fuente = (decimal)font.FuenteFinanciacion.ValorFuente - saldofuente,
                                            Nuevo_saldo_de_la_fuente_al_guardar = gestionAlGuardar != null ? gestionAlGuardar.NuevoSaldo : 0,
                                            Saldo_actual_de_la_fuente_al_guardar = gestionAlGuardar != null ? gestionAlGuardar.SaldoActual : 0,

                                        });
                                        saldototal += (decimal)font.FuenteFinanciacion.ValorFuente - saldofuente;
                                        nombreAportante = getNombreAportante(_context.CofinanciacionAportante.Find(font.FuenteFinanciacion.AportanteId));
                                        valorAportate = font.ValorFuente;
                                        if (!aportantes.Any(c => c.CofinanciacionAportanteId == apo.AportanteId))
                                        {
                                            aportantes.Add(new CofinanicacionAportanteGrilla
                                            {
                                                CofinanciacionAportanteId = apo.AportanteId,
                                                Nombre = nombreAportante,
                                                TipoAportante = "",
                                                ValorAportanteAlProyecto = valorAportate,
                                                FuentesFinanciacion = fuentes
                                            });
                                        }
                                    }
                                    intaportante = apo.AportanteId == null ? 0 : apo.AportanteId;

                                }
                                proyecto.Add(new ProyectoGrilla
                                {
                                    LlaveMen = proyectoadministrativo.FirstOrDefault().ProyectoAdministrativoId.ToString(),
                                    Departamento = "",
                                    Municipio = "",
                                    TipoIntervencion = "",
                                    InstitucionEducativa = "",
                                    Sede = "",
                                    NombreAportante = nombreAportante,
                                    ValorAportante = valorAportate,
                                    AportanteID = intaportante,
                                    DisponibilidadPresupuestalProyecto = 0,
                                    ValorGestionado = 0,
                                    ComponenteGrilla = null
                                });
                            }
                        }

                        #endregion proyecto administrativo

                        #region Proyecto fisico

                        else
                        {
                            valorGestionado += _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalProyectoId == proyectospp.DisponibilidadPresupuestalProyectoId).Sum(x => x.ValorSolicitado);

                            var localizacion = _context.Localizacion.Where(x => x.LocalizacionId == proyectospp.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                            var sede = _context.InstitucionEducativaSede.Find(proyectospp.Proyecto.SedeId);
                            List<GrillaComponentes> grilla = new List<GrillaComponentes>();
                            int intaportante = 0;
                            decimal valorgestionado = 0;

                            proyectospp.Proyecto.ProyectoAportante = proyectospp.Proyecto.ProyectoAportante.Where(r => r.Eliminado != true).ToList();

                            foreach (var ppapor in proyectospp.Proyecto.ProyectoAportante)
                            {
                                List<GrillaFuentesFinanciacion> fuentes = new List<GrillaFuentesFinanciacion>();

                                foreach (var font in ppapor.Aportante.FuenteFinanciacion)
                                {
                                    //el saldo de la fuente realmente es lo que tengo en control de recursos
                                    //var saldo = _context.ControlRecurso.Where(x => x.FuenteFinanciacionId == font.FuenteFinanciacionId).Sum(x=>x.ValorConsignacion);
                                    decimal saldo = Convert.ToDecimal(_context.FuenteFinanciacion.Where(x => x.FuenteFinanciacionId == font.FuenteFinanciacionId).Sum(x => x.ValorFuente));
                                    //Valor nuevo SOLICITADO NUEVO
                                    decimal valorsolicitado = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalProyectoId ==
                                    proyectospp.DisponibilidadPresupuestalProyectoId && x.FuenteFinanciacionId == font.FuenteFinanciacionId).Sum(x => x.ValorSolicitado);

                                    decimal valorsolicitadoxotros = _context.GestionFuenteFinanciacion
                                        .Where(x => !(bool)x.Eliminado &&
                                               x.DisponibilidadPresupuestalProyectoId != proyectospp.DisponibilidadPresupuestalProyectoId &&
                                               x.FuenteFinanciacionId == font.FuenteFinanciacionId)
                                        .Sum(x => x.ValorSolicitado);

                                    var gestionAlGuardar = _context.GestionFuenteFinanciacion
                                            .Where(x => x.DisponibilidadPresupuestalProyectoId == proyectospp.DisponibilidadPresupuestalProyectoId &&
                                                x.FuenteFinanciacionId == font.FuenteFinanciacionId)
                                            .FirstOrDefault();

                                    var funtename = _context.Dominio.Where(x => x.Codigo == font.FuenteRecursosCodigo
                                              && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion);
                                    string namefuente = funtename.Any() ? funtename.FirstOrDefault().Nombre : "";
                                    fuentes.Add(new GrillaFuentesFinanciacion
                                    {
                                        Fuente = namefuente,
                                        Estado_de_las_fuentes = "",
                                        FuenteFinanciacionID = font.FuenteFinanciacionId,
                                        Valor_solicitado_de_la_fuente = valorsolicitado,
                                        Nuevo_saldo_de_la_fuente = saldo - valorsolicitadoxotros - valorsolicitado,
                                        Saldo_actual_de_la_fuente = saldo - valorsolicitadoxotros,
                                        Nuevo_saldo_de_la_fuente_al_guardar = gestionAlGuardar != null ? gestionAlGuardar.NuevoSaldo : 0,
                                        Saldo_actual_de_la_fuente_al_guardar = gestionAlGuardar != null ? gestionAlGuardar.SaldoActual : 0,
                                    });
                                }
                                if (detailDP.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Tradicional)
                                {
                                    var valorproyecto = detailDP.Contratacion.TipoSolicitudCodigo == "2" ? ppapor.ValorInterventoria : ppapor.ValorObra;
                                    if (detailDP.EsNovedadContractual != null && Convert.ToBoolean(detailDP.EsNovedadContractual))
                                    {

                                        List<NovedadContractualAportante> novedadContractualAportantes = _context.NovedadContractualAportante
                                                                                                            .Where(x => x.NovedadContractualId == detailDP.NovedadContractualId)
                                                                                                                .Include(x => x.ComponenteAportanteNovedad)
                                                                                                                    .ThenInclude(x => x.ComponenteFuenteNovedad)
                                                                                                                        .ThenInclude(x => x.ComponenteUsoNovedad)
                                                                                                            .ToList();

                                        novedadContractualAportantes.ForEach(nca =>
                                       {
                                           //if ( nca.CofinanciacionAportanteId == ppapor.AportanteId)
                                           {
                                               nca.ComponenteAportanteNovedad.ToList().ForEach(can =>
                                              {
                                                  can.ComponenteFuenteNovedad.ToList().ForEach(cfn =>
                                                 {
                                                     cfn.ComponenteUsoNovedad.ToList().ForEach(cun =>
                                                    {
                                                        valorproyecto = valorproyecto + cun.ValorUso;
                                                    });
                                                 });
                                              });
                                           }
                                       });

                                        //valorproyecto = _context.ComponenteUsoNovedad.Where(x => x.ComponenteAportanteNovedad.CofinanciacionAportanteId == ppapor.AportanteId &&
                                        //x.ComponenteAportanteNovedad.NovedadContractualAportante.NovedadContractualId == detailDP.NovedadContractualId).Sum(x => x.ValorUso);

                                    }
                                    aportantes.Add(new CofinanicacionAportanteGrilla
                                    {
                                        CofinanciacionAportanteId = ppapor.AportanteId,
                                        Nombre = getNombreAportante(ppapor.Aportante),
                                        TipoAportante = _context.Dominio.Where(r => (bool)r.Activo && r.DominioId.Equals(ppapor.Aportante.TipoAportanteId) && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_aportante).Select(r => r.Nombre).FirstOrDefault(),
                                        ValorAportanteAlProyecto = valorproyecto,
                                        FuentesFinanciacion = fuentes,

                                    });
                                    aportantesxProyecto.Add(new CofinanicacionAportanteGrilla
                                    {
                                        CofinanciacionAportanteId = ppapor.AportanteId,
                                        Nombre = getNombreAportante(ppapor.Aportante),
                                        TipoAportante = _context.Dominio.Where(r => (bool)r.Activo && r.DominioId.Equals(ppapor.Aportante.TipoAportanteId) && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_aportante).Select(r => r.Nombre).FirstOrDefault(),
                                        ValorAportanteAlProyecto = valorproyecto,
                                        FuentesFinanciacion = fuentes,
                                        ValorGestionado = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalProyectoId == proyectospp.DisponibilidadPresupuestalProyectoId && fuentes.Select(x => x.FuenteFinanciacionID).Contains(x.FuenteFinanciacionId)).Sum(x => x.ValorSolicitado)
                                    });
                                }


                                var confinanciacion = _context.CofinanciacionAportante.Where(x => x.CofinanciacionAportanteId == ppapor.AportanteId).Include(x => x.CofinanciacionDocumento).FirstOrDefault();
                                var intfuentes = _context.FuenteFinanciacion.Where(y => y.AportanteId == ppapor.AportanteId).Select(t => t.FuenteFinanciacionId).ToList();

                                if (confinanciacion != null)
                                {
                                    intaportante = confinanciacion == null ? 0 : confinanciacion.CofinanciacionAportanteId;
                                    nombreAportante = getNombreAportante(confinanciacion);
                                    valorAportate = _context.ProyectoAportante.Where(x => x.ProyectoId == proyectospp.ProyectoId && x.AportanteId == ppapor.AportanteId).Sum(x => x.ValorTotalAportante);
                                    if (detailDP.EsNovedadContractual != null && Convert.ToBoolean(detailDP.EsNovedadContractual))
                                    {
                                        List<NovedadContractualAportante> novedadContractualAportantes = _context.NovedadContractualAportante
                                                                                                            .Where(x => x.NovedadContractualId == detailDP.NovedadContractualId)
                                                                                                                .Include(x => x.ComponenteAportanteNovedad)
                                                                                                                    .ThenInclude(x => x.ComponenteFuenteNovedad)
                                                                                                                        .ThenInclude(x => x.ComponenteUsoNovedad)
                                                                                                            .ToList();

                                        novedadContractualAportantes.ForEach(nca =>
                                        {
                                            if (nca.CofinanciacionAportanteId == ppapor.AportanteId)
                                            {
                                                nca.ComponenteAportanteNovedad.ToList().ForEach(can =>
                                                {
                                                    can.ComponenteFuenteNovedad.ToList().ForEach(cfn =>
                                                    {
                                                        cfn.ComponenteUsoNovedad.ToList().ForEach(cun =>
                                                        {
                                                            valorAportate = valorAportate + cun.ValorUso;
                                                        });
                                                    });
                                                });
                                            }
                                        });

                                        //valorAportate = _context.ComponenteUsoNovedad.Where(x => x.ComponenteAportanteNovedad.CofinanciacionAportanteId == ppapor.AportanteId &&
                                        //x.ComponenteAportanteNovedad.NovedadContractualAportante.NovedadContractualId == detailDP.NovedadContractualId).Sum(x => x.ValorUso);
                                    }


                                    var componenteAp = _context.ComponenteAportante.Where(x => x.ContratacionProyectoAportante.ContratacionProyecto.ContratacionId == detailDP.ContratacionId
                                    && x.ContratacionProyectoAportante.ContratacionProyecto.ProyectoId == proyectospp.ProyectoId &&
                                    x.ContratacionProyectoAportante.CofinanciacionAportanteId == confinanciacion.CofinanciacionAportanteId)
                                        .Include(x => x.ComponenteUso).ToList();
                                    foreach (var compAp in componenteAp)
                                    {
                                        List<string> uso = new List<string>();
                                        List<decimal> usovalor = new List<decimal>();
                                        decimal total = 0;
                                        foreach (var comp in compAp.ComponenteUso)
                                        {
                                            var usos = _context.Dominio.Where(x => x.Codigo == comp.TipoUsoCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Usos).ToList();
                                            uso.Add(usos.Count() > 0 ? usos.FirstOrDefault().Nombre : "");
                                            usovalor.Add(comp.ValorUso);
                                            total += comp.ValorUso;
                                        }
                                        var dom = _context.Dominio.Where(x => x.Codigo == compAp.TipoComponenteCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Componentes).ToList();
                                        grilla.Add(
                                            new GrillaComponentes
                                            {
                                                ComponenteAportanteId = compAp.ComponenteAportanteId,
                                                Componente = dom.Count() > 0 ? dom.FirstOrDefault().Nombre : "",
                                                ComponenteUsoCodigo = compAp.TipoComponenteCodigo,
                                                Uso = uso,
                                                ValorTotal = total,
                                                ValorUso = usovalor,
                                                cofinanciacionAportanteId = ppapor.Aportante.CofinanciacionAportanteId,

                                            });
                                    }
                                    valorgestionado = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalProyectoId == proyectospp.DisponibilidadPresupuestalProyectoId && intfuentes.Contains(x.FuenteFinanciacionId)).Sum(x => x.ValorSolicitado);


                                }
                            }
                            proyecto.Add(new ProyectoGrilla
                            {
                                LlaveMen = proyectospp.Proyecto.LlaveMen,
                                Departamento = _context.Localizacion.Find(localizacion.IdPadre).Descripcion,
                                Municipio = localizacion.Descripcion,
                                TipoIntervencion = proyectospp.Proyecto.TipoIntervencionCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(proyectospp.Proyecto.TipoIntervencionCodigo, (int)EnumeratorTipoDominio.Tipo_de_Intervencion) : "",
                                InstitucionEducativa = _context.InstitucionEducativaSede.Find(sede.PadreId).Nombre,
                                Sede = sede.Nombre,
                                NombreAportante = nombreAportante,
                                ValorAportante = valorAportate,
                                AportanteID = intaportante,
                                DisponibilidadPresupuestalProyecto = proyectospp.DisponibilidadPresupuestalProyectoId,
                                ValorGestionado = valorgestionado,
                                ComponenteGrilla = grilla,
                                Aportantes = aportantesxProyecto
                            });
                        }

                        #endregion Proyecto fisico
                    }

                    #region busco comite técnico

                    //busco comite técnico
                    DateTime fechaComitetecnico = DateTime.Now;

                    string numerocomietetecnico = "";

                    if (detailDP.ContratacionId != null)
                    {
                        var contratacion = _context.SesionComiteSolicitud.Where(x => x.SolicitudId == detailDP.ContratacionId && x.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion).
                            Include(x => x.ComiteTecnico).ToList();
                        if (contratacion.Count() > 0)
                        {
                            numerocomietetecnico = contratacion.FirstOrDefault().ComiteTecnico.NumeroComite;
                            fechaComitetecnico = Convert.ToDateTime(contratacion.FirstOrDefault().ComiteTecnico.FechaOrdenDia);
                        }
                    }

                    #endregion busco comite técnico

                    if (detailDP.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Especial)
                    {
                        #region otros costos

                        if (detailDP.NumeroContrato != null)//otros costos
                        {
                            valorGestionado = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalId == detailDP.DisponibilidadPresupuestalId).Sum(x => x.ValorSolicitado);
                            var aportanteotroscostos = _context.CofinanciacionAportante.Where(x => x.CofinanciacionAportanteId == detailDP.AportanteId).
                                Include(x => x.FuenteFinanciacion).ToList();
                            if (aportanteotroscostos.Any())
                            {
                                aportantes.Add(new CofinanicacionAportanteGrilla
                                {
                                    CofinanciacionAportanteId = aportanteotroscostos.FirstOrDefault().CofinanciacionAportanteId,
                                    Nombre = getNombreAportante(aportanteotroscostos.FirstOrDefault()),
                                    TipoAportante = _context.Dominio.Where(r => (bool)r.Activo && r.DominioId.Equals(aportanteotroscostos.FirstOrDefault().TipoAportanteId) && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_aportante).Select(r => r.Nombre).FirstOrDefault(),
                                    ValorAportanteAlProyecto = detailDP.ValorAportante,
                                    FuentesFinanciacion = null
                                });
                                saldototal = Convert.ToDecimal(aportanteotroscostos.FirstOrDefault().FuenteFinanciacion.Sum(x => x.ValorFuente));
                            }
                        }

                        #endregion otros costos

                        #region expensas

                        else//expensas
                        {
                            valorGestionado = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.DisponibilidadPresupuestalId == detailDP.DisponibilidadPresupuestalId).Sum(x => x.ValorSolicitado);
                            var aportanteotroscostos = _context.CofinanciacionAportante.Where(x => x.CofinanciacionAportanteId == detailDP.AportanteId).
                                Include(x => x.FuenteFinanciacion).ToList();
                            if (aportanteotroscostos.Any())
                            {
                                List<GrillaFuentesFinanciacion> fnt = new List<GrillaFuentesFinanciacion>();
                                foreach (var fuente in aportanteotroscostos.FirstOrDefault().FuenteFinanciacion)
                                {
                                    fnt.Add(new GrillaFuentesFinanciacion
                                    {
                                        Fuente = _context.Dominio.Where(x => x.Codigo == fuente.FuenteRecursosCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre,
                                        Valor_solicitado_de_la_fuente = Convert.ToDecimal(fuente.ValorFuente),
                                        Estado_de_las_fuentes = "",
                                        FuenteFinanciacionID = fuente.FuenteFinanciacionId,
                                        GestionFuenteFinanciacionID = 0,
                                        Nuevo_saldo_de_la_fuente = 0,
                                        Saldo_actual_de_la_fuente = 0
                                    });

                                }
                                aportantes.Add(new CofinanicacionAportanteGrilla
                                {
                                    CofinanciacionAportanteId = aportanteotroscostos.FirstOrDefault().CofinanciacionAportanteId,
                                    Nombre = getNombreAportante(aportanteotroscostos.FirstOrDefault()),
                                    TipoAportante = _context.Dominio.Where(r => (bool)r.Activo && r.DominioId.Equals(aportanteotroscostos.FirstOrDefault().TipoAportanteId) && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_aportante).Select(r => r.Nombre).FirstOrDefault(),
                                    ValorAportanteAlProyecto = detailDP.ValorAportante,
                                    FuentesFinanciacion = fnt
                                });
                                saldototal = Convert.ToDecimal(aportanteotroscostos.FirstOrDefault().FuenteFinanciacion.Sum(x => x.ValorFuente));
                            }
                        }

                        #endregion expensas

                    }
                    var contrato = _context.Contrato.Where(x => x.Contratacion.ContratacionId == detailDP.ContratacionId);
                    var fechaContrato = "";

                    if (contrato.Any())
                    {
                        var fechafi = contrato.Select(x => x.FechaFirmaContrato).FirstOrDefault();
                        fechaContrato = Convert.ToDateTime(fechafi).ToString("dd/MM/yyyy");

                    }

                    string nombreEntidad = "";
                    string contratoNumero = !contrato.Any() ? "" : contrato.Select(x => x.NumeroContrato).FirstOrDefault().ToString();
                    if (contrato.Any())
                    {
                        var contratacion = _context.Contratacion.Find(contrato.FirstOrDefault().ContratacionId);
                        var contratista = _context.Contratista.Find(contratacion.ContratistaId);
                        nombreEntidad = contratista == null ? "" : contratista.Nombre;
                    }


                    var observaciones = _context.DisponibilidadPresupuestalObservacion.Where(x => x.DisponibilidadPresupuestalId == detailDP.DisponibilidadPresupuestalId).ToList();
                    string observacionString = !observaciones.Any() ? "" : string.Join("<br><br>", observaciones.Select(x => x.Observacion));
                    var aportant = aportantes.Distinct();
                    var tiporubro = detailDP.TipoSolicitudEspecialCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(detailDP.TipoSolicitudEspecialCodigo, (int)EnumeratorTipoDominio.Tipo_DDP_Espacial) :
                        //si no viene el campo puede ser contratación
                        detailDP.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo ? ConstanStringTipoSolicitudContratacion.proyectoAdministrativo :
                        ConstanStringTipoSolicitudContratacion.contratacion;
                    decimal ValorTotalDisponibilidad = 0;
                    if (detailDP != null)
                        ValorTotalDisponibilidad = _commonService.GetValorTotalDisponibilidad(detailDP.DisponibilidadPresupuestalId,false);

                    DetailValidarDisponibilidadPresupuesal detailDisponibilidadPresupuesal = new DetailValidarDisponibilidadPresupuesal
                    {
                        //TODO:Traer estos campos { Tipo de modificacion, Valor despues de la modificacion, Plazo despues de la modificacion, Detalle de la modificacion) => se toma del caso de uso de novedades contractuales
                        Id = detailDP.DisponibilidadPresupuestalId,
                        NumeroSolicitud = detailDP.NumeroSolicitud,
                        TipoSolicitudCodigo = detailDP.TipoSolicitudCodigo,
                        NUmeroSaldoFuente = saldototal,
                        TipoSolicitudText = detailDP.TipoSolicitudCodigo != null ?
                            await _commonService.GetNombreDominioByCodigoAndTipoDominio(detailDP.TipoSolicitudCodigo,
                            (int)EnumeratorTipoDominio.Tipo_Disponibilidad_Presupuestal) : "",
                        NumeroDDP = detailDP.NumeroDdp,
                        NumeroDRP = detailDP.NumeroDrp,
                        RubroPorFinanciar = detailDP.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Especial ? tiporubro : _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Disponibilidad_Presupuestal
                                         && r.Codigo == detailDP.TipoSolicitudCodigo).FirstOrDefault().Descripcion,
                        Objeto = detailDP.Objeto,
                        ValorSolicitud = detailDP.ValorSolicitud,
                        // Si es aproboda por comite tecnico se debe mostrar la fecha en la que fue aprobada. traer desde dbo.[Sesion]
                        FechaComiteTecnico = fechaComitetecnico,
                        NumeroComite = numerocomietetecnico,
                        FechaSolicitud = detailDP.FechaSolicitud,
                        EstadoStr = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud_Presupuestal
                                    && r.Codigo == detailDP.EstadoSolicitudCodigo).FirstOrDefault().Nombre,
                        Plazo = detailDP.Contratacion?.PlazoContratacion.PlazoMeses.ToString() + " meses / " + detailDP.Contratacion?.PlazoContratacion.PlazoDias.ToString() + " dias",
                        CuentaCarta = detailDP.CuentaCartaAutorizacion,
                        TipoSolicitudEspecial = detailDP.TipoSolicitudEspecialCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(detailDP.TipoSolicitudEspecialCodigo, (int)EnumeratorTipoDominio.Tipo_DDP_Espacial) :
                        //si no viene el campo puede ser contratación
                        detailDP.TipoSolicitudCodigo == ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Administrativo ? ConstanStringTipoSolicitudContratacion.proyectoAdministrativo :
                        detailDP.EsNovedadContractual == null ? ConstanStringTipoSolicitudContratacion.contratacion : !Convert.ToBoolean(detailDP.EsNovedadContractual) ? ConstanStringTipoSolicitudContratacion.contratacion : ConstanStringTipoSolicitudContratacion.novedadContractual,
                        ContratoNumero = contratoNumero,
                        NombreEntidad = nombreEntidad,
                        UrlConSoporte = detailDP.UrlSoporte,
                        Limitacion = detailDP.LimitacionEspecial,
                        /*//*las modificaciones aun no existen*/
                        ValorGestionado = valorGestionado,
                        Observaciones = observacionString,
                        Proyectos = proyecto,
                        FechaContrato = fechaContrato,
                        //Aportantes
                        Aportantes = aportant.ToList(),
                        NumeroRadicado = detailDP.NumeroRadicadoSolicitud,
                        ObservacioensCancelacion = _context.DisponibilidadPresupuestalObservacion.Where(x => x.DisponibilidadPresupuestalId == detailDP.DisponibilidadPresupuestalId).ToList(),
                        EsNovedad = false,
                        NovedadContractual = detailDP.NovedadContractualId != null ? _context.NovedadContractual.Where(x => x.NovedadContractualId == detailDP.NovedadContractualId).Include(x => x.NovedadContractualDescripcion).FirstOrDefault() : null,
                        ValorTotalDisponibilidad = ValorTotalDisponibilidad
                    };

                    ListDetailValidarDisponibilidadPresupuesal.Add(detailDisponibilidadPresupuesal);
                }
            }


            return ListDetailValidarDisponibilidadPresupuesal;
        }

        public string getNombreAportante(CofinanciacionAportante confinanciacion)
        {
            string nombreAportante;
            if (confinanciacion.TipoAportanteId.Equals(ConstanTipoAportante.Ffie))
            {
                nombreAportante = ConstanStringTipoAportante.Ffie;
            }
            else if (confinanciacion.TipoAportanteId.Equals(ConstanTipoAportante.Tercero))
            {
                nombreAportante = confinanciacion.NombreAportanteId == null
                    ? "Error" :
                    _context.Dominio.Find(confinanciacion.NombreAportanteId).Nombre;
            }
            else
            {
                if (confinanciacion.MunicipioId == null)
                {
                    nombreAportante = confinanciacion.DepartamentoId == null
                    ? "Error" :
                    "Gobernación " + _context.Localizacion.Find(confinanciacion.DepartamentoId).Descripcion;
                }
                else
                {
                    nombreAportante = confinanciacion.MunicipioId == null
                    ? "Error" :
                    "Alcaldía " + _context.Localizacion.Find(confinanciacion.MunicipioId).Descripcion;
                }
            }
            return nombreAportante;
        }

        //Crear DDP Especial 
        public async Task<Respuesta> CreateUpdateDisponibilidaPresupuestalEspecial(DisponibilidadPresupuestal pDisponibilidadPresupuestal)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_DDP, (int)EnumeratorTipoDominio.Acciones);
            DisponibilidadPresupuestalProyecto entity = new DisponibilidadPresupuestalProyecto();
            Contrato contrato = _context.Contrato
                .Where(r => r.NumeroContrato == pDisponibilidadPresupuestal.NumeroContrato)
                .Include(r => r.Contratacion)
                .FirstOrDefault();
            try
            {
                if (pDisponibilidadPresupuestal.DisponibilidadPresupuestalId == 0)
                {
                    pDisponibilidadPresupuestal.FechaCreacion = DateTime.Now;
                    pDisponibilidadPresupuestal.Eliminado = false;
                    pDisponibilidadPresupuestal.FechaSolicitud = DateTime.Now;
                    pDisponibilidadPresupuestal.RegistroCompleto = ValidarDisponibilidadPresupuestal(pDisponibilidadPresupuestal);
                    //pDisponibilidadPresupuestal.EstadoSolicitudCodigo = ConstanCodigoSolicitudDisponibilidadPresupuestal.Sin_Registrar;
                    if (contrato != null)
                    {
                        pDisponibilidadPresupuestal.OpcionContratarCodigo = contrato.TipoContratoCodigo;
                    }
                    pDisponibilidadPresupuestal.TipoSolicitudCodigo = ConstanCodigoTipoDisponibilidadPresupuestal.DDP_Especial;
                    pDisponibilidadPresupuestal.NumeroSolicitud = Helpers.Helpers.Consecutive("DE", _context.DisponibilidadPresupuestal.Count((r => r.NumeroSolicitud.Contains("DE"))));

                    if (pDisponibilidadPresupuestal.ValorAportante != null)
                        pDisponibilidadPresupuestal.ValorSolicitud = (decimal)pDisponibilidadPresupuestal.ValorAportante;

                    pDisponibilidadPresupuestal.DisponibilidadPresupuestalProyecto.ToList().ForEach(p =>
                    {
                        if (p.DisponibilidadPresupuestalProyectoId == 0)
                        {
                            entity.ProyectoId = p.ProyectoId;
                            entity.DisponibilidadPresupuestalId = pDisponibilidadPresupuestal.DisponibilidadPresupuestalId;
                            entity.FechaCreacion = DateTime.Now;
                            entity.UsuarioCreacion = pDisponibilidadPresupuestal.UsuarioCreacion;
                            entity.Eliminado = false;
                            pDisponibilidadPresupuestal.DisponibilidadPresupuestalProyecto.Add(entity);
                        }


                    });

                    _context.DisponibilidadPresupuestal.Add(pDisponibilidadPresupuestal);
                }
                else
                {
                    DisponibilidadPresupuestal disponibilidadPresupuestalOld = _context.DisponibilidadPresupuestal.Find(pDisponibilidadPresupuestal.DisponibilidadPresupuestalId);

                    //disponibilidadPresupuestalOld.FechaSolicitud = pDisponibilidadPresupuestal.FechaSolicitud;
                    disponibilidadPresupuestalOld.TipoSolicitudCodigo = pDisponibilidadPresupuestal.TipoSolicitudCodigo;
                    //disponibilidadPresupuestalOld.NumeroSolicitud = pDisponibilidadPresupuestal.NumeroSolicitud;
                    //disponibilidadPresupuestalOld.OpcionContratarCodigo = pDisponibilidadPresupuestal.OpcionContratarCodigo;
                    //disponibilidadPresupuestalOld.EstadoSolicitudCodigo = pDisponibilidadPresupuestal.EstadoSolicitudCodigo;
                    disponibilidadPresupuestalOld.Objeto = pDisponibilidadPresupuestal.Objeto;
                    //disponibilidadPresupuestalOld.FechaDdp = pDisponibilidadPresupuestal.FechaDdp;
                    //disponibilidadPresupuestalOld.NumeroDdp = pDisponibilidadPresupuestal.NumeroDdp;
                    //disponibilidadPresupuestalOld.RutaDdp = pDisponibilidadPresupuestal.RutaDdp;
                    disponibilidadPresupuestalOld.FechaModificacion = DateTime.Now;
                    disponibilidadPresupuestalOld.UsuarioModificacion = pDisponibilidadPresupuestal.UsuarioCreacion;
                    //disponibilidadPresupuestalOld.ContratacionId = pDisponibilidadPresupuestal.ContratacionId;
                    //disponibilidadPresupuestalOld.NumeroDrp = pDisponibilidadPresupuestal.NumeroDrp;
                    //disponibilidadPresupuestalOld.PlazoMeses = pDisponibilidadPresupuestal.PlazoMeses;
                    ///disponibilidadPresupuestalOld.PlazoDias = pDisponibilidadPresupuestal.PlazoDias;
                    disponibilidadPresupuestalOld.LimitacionEspecial = pDisponibilidadPresupuestal.LimitacionEspecial == null ? disponibilidadPresupuestalOld.LimitacionEspecial : pDisponibilidadPresupuestal.LimitacionEspecial;
                    disponibilidadPresupuestalOld.UrlSoporte = pDisponibilidadPresupuestal.UrlSoporte == null ? disponibilidadPresupuestalOld.UrlSoporte : pDisponibilidadPresupuestal.UrlSoporte;
                    disponibilidadPresupuestalOld.CuentaCartaAutorizacion = pDisponibilidadPresupuestal.CuentaCartaAutorizacion == null ? disponibilidadPresupuestalOld.CuentaCartaAutorizacion : pDisponibilidadPresupuestal.CuentaCartaAutorizacion;
                    disponibilidadPresupuestalOld.AportanteId = pDisponibilidadPresupuestal.AportanteId;
                    disponibilidadPresupuestalOld.ValorAportante = pDisponibilidadPresupuestal.ValorAportante;
                    if (pDisponibilidadPresupuestal.ValorAportante != null)
                        disponibilidadPresupuestalOld.ValorSolicitud = (decimal)pDisponibilidadPresupuestal.ValorAportante;
                    disponibilidadPresupuestalOld.NumeroContrato = pDisponibilidadPresupuestal.NumeroContrato == null ? disponibilidadPresupuestalOld.NumeroContrato : pDisponibilidadPresupuestal.NumeroContrato;
                    disponibilidadPresupuestalOld.RegistroCompleto = ValidarDisponibilidadPresupuestal(disponibilidadPresupuestalOld);
                    //disponibilidadPresupuestalOld.EstadoSolicitudCodigo = ConstanCodigoSolicitudDisponibilidadPresupuestal.Sin_Registrar;
                    _context.DisponibilidadPresupuestal.Update(disponibilidadPresupuestalOld);
                    pDisponibilidadPresupuestal.DisponibilidadPresupuestalProyecto.ToList().ForEach(p =>
                    {
                        if (p.DisponibilidadPresupuestalProyectoId == 0)
                        {
                            entity.ProyectoId = p.ProyectoId;
                            entity.DisponibilidadPresupuestalId = pDisponibilidadPresupuestal.DisponibilidadPresupuestalId;
                            entity.FechaCreacion = DateTime.Now;
                            entity.UsuarioCreacion = pDisponibilidadPresupuestal.UsuarioCreacion;
                            entity.Eliminado = false;

                            disponibilidadPresupuestalOld.DisponibilidadPresupuestalProyecto.Add(entity);
                        }
                        else
                        {
                            entity = _context.DisponibilidadPresupuestalProyecto.Find(p.DisponibilidadPresupuestalProyectoId);

                            entity.UsuarioModificacion = pDisponibilidadPresupuestal.UsuarioCreacion;
                            entity.FechaModificacion = DateTime.Now;
                            entity.FechaCreacion = entity.FechaCreacion == null ? DateTime.Now : entity.FechaCreacion;
                            entity.ProyectoId = p.ProyectoId;
                            _context.DisponibilidadPresupuestalProyecto.Update(entity);
                        }
                    });
                }

                await _context.SaveChangesAsync();
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.DisponibilidadPresupuestal, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, pDisponibilidadPresupuestal.UsuarioCreacion, ConstantCommonMessages.CREAR_EDITAR_DDP_ESPECIAL)

                };
            }

            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesSesionComiteTema.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, pDisponibilidadPresupuestal.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        private bool? ValidarDisponibilidadPresupuestal(DisponibilidadPresupuestal pDisponibilidadPresupuestal)
        {
            if (
                     string.IsNullOrEmpty(pDisponibilidadPresupuestal.Objeto)
                  || string.IsNullOrEmpty(pDisponibilidadPresupuestal.NumeroRadicadoSolicitud)
                  //|| string.IsNullOrEmpty(pDisponibilidadPresupuestal.NumeroContrato)
                  || pDisponibilidadPresupuestal.AportanteId == null
                  || pDisponibilidadPresupuestal.ValorAportante == null)
            {
                return false;
            }
            return true;
        }

        public async Task<dynamic> GetContratos()
        {
            return await _context.Contrato.Where(x => !(bool)x.Eliminado).ToListAsync();
        }

        public async Task<dynamic> getNovedadContractualByContratacionId(int contratacionId)
        {
            List<NovedadContractual> novedades = await _context.NovedadContractual.Where(x => x.Contrato.ContratacionId == contratacionId).
                Include(x => x.Contrato).
                    ThenInclude(x => x.Contratacion).
                    ThenInclude(x => x.DisponibilidadPresupuestal).
                Include(x => x.NovedadContractualDescripcion).
                Include(x => x.NovedadContractualAportante).
                    ThenInclude(x => x.ComponenteAportanteNovedad)
               .ToListAsync();
            if(novedades.Count() > 0)
            {
                //solo a una
                decimal valorSolicitud = 0;
                DisponibilidadPresupuestal dpp = novedades.FirstOrDefault().Contrato?.Contratacion?.DisponibilidadPresupuestal.FirstOrDefault();
                if (dpp != null)
                {
                    valorSolicitud = _commonService.GetValorTotalDisponibilidad(dpp.DisponibilidadPresupuestalId,false);
                }

                novedades.ForEach(r =>
                {
                    foreach (var ddp in r.Contrato.Contratacion.DisponibilidadPresupuestal)
                    {
                        ddp.ValorTotalDisponibilidad = valorSolicitud;
                    }
                });


            }

            return novedades;
        }
    }
}
