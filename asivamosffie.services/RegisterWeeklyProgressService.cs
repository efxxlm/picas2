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
using System.IO;
using Z.EntityFramework.Plus;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Runtime.InteropServices.WindowsRuntime;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Microsoft.EntityFrameworkCore.Internal;
using asivamosffie.services.Helpers.Constants;
using asivamosffie.services.Helpers;
using asivamosffie.model.AditionalModels;
using Microsoft.Extensions.Options;

namespace asivamosffie.services
{
    public class RegisterWeeklyProgressService : IRegisterWeeklyProgressService
    {
        #region constructor

        private ICommonService _commonService;
        private readonly IDocumentService _documentService;
        private readonly IContractualControversy _contractualControversy;
        //private readonly ICheckWeeklyProgressService _checkWeeklyProgressService;
        private readonly devAsiVamosFFIEContext _context;

        public RegisterWeeklyProgressService(
            devAsiVamosFFIEContext context,
            ICommonService commonService,
            //     ICheckWeeklyProgressService checkWeeklyProgressService,
            IDocumentService documentService,
            IContractualControversy contractualControversy)
        {
            //     _checkWeeklyProgressService = checkWeeklyProgressService;
            _documentService = documentService;
            _commonService = commonService;
            _context = context;
            _contractualControversy = contractualControversy;
        }

        #endregion

        #region Get
        private dynamic GetTableFinanciera(SeguimientoSemanal pSeguimientoSemanal)
        {
            int ContratoConstruccionId = _context.FlujoInversion
                                            .Where(f => f.SeguimientoSemanalId == pSeguimientoSemanal.SeguimientoSemanalId)
                                            .Select(r => r.ContratoConstruccionId)
                                            .FirstOrDefault();

            decimal ValorTotalProyecto = (decimal)(_context.FlujoInversion
                                            .Where(f => f.ContratoConstruccionId == ContratoConstruccionId)
                                            .Sum(f => f.Valor));

            List<FlujoInversion> flujoInversions = _context.FlujoInversion
                                                          .Include(r => r.Programacion)
                                                          .Where(r => r.SeguimientoSemanal.ContratacionProyectoId == pSeguimientoSemanal.ContratacionProyectoId
                                                              && r.SeguimientoSemanal.NumeroSemana < pSeguimientoSemanal.NumeroSemana)
                                                          .Take(4)
                                                          .ToList();

            var GroupByProgramacionId = flujoInversions
             .GroupBy(r => r.ProgramacionId)
             .Select(r => new
             {
                 Item = r.FirstOrDefault().Programacion.Actividad,
                 Valor = String.Format("{0:n}", Math.Round((decimal)r.Sum(r => r.Valor), 2)),
                 Porcentaje = Math.Round((decimal)r.Sum(r => r.Valor) * 100 / ValorTotalProyecto, 2) + "%"
             });

            return GroupByProgramacionId;
        }

        public async Task<dynamic> GetObservacionBy(int pSeguimientoSemanalId, int pPadreId, string pTipoCodigo)
        {
            try
            {
                return await _context.SeguimientoSemanalObservacion
                    .Where(
                           r => r.SeguimientoSemanalId == pSeguimientoSemanalId
                           && r.ObservacionPadreId == pPadreId
                           && r.TipoObservacionCodigo == pTipoCodigo
                          )
                        .Select(r =>
                                new
                                {
                                    r.ObservacionPadreId,
                                    r.TieneObservacion,
                                    r.TipoObservacionCodigo,
                                    r.SeguimientoSemanalId,
                                    r.SeguimientoSemanalObservacionId,
                                    r.Observacion,
                                    r.EsSupervisor,
                                    r.FechaCreacion,
                                    r.Archivada
                                }
                               ).ToListAsync();

            }
            catch (Exception e)
            {
                var result = new { };
                return result;
            }
        }

        public async Task<GestionObraCalidadEnsayoLaboratorio> GetEnsayoLaboratorioMuestras(int pGestionObraCalidadEnsayoLaboratorioId)
        {
            GestionObraCalidadEnsayoLaboratorio GestionObraCalidadEnsayoLaboratorio = await _context.GestionObraCalidadEnsayoLaboratorio
               .Where(r => r.GestionObraCalidadEnsayoLaboratorioId == pGestionObraCalidadEnsayoLaboratorioId)
               .Include(r => r.EnsayoLaboratorioMuestra)
               .Include(r => r.SeguimientoSemanalGestionObraCalidad)
                  .ThenInclude(r => r.SeguimientoSemanalGestionObra)
                      .ThenInclude(r => r.SeguimientoSemanal)
                          .ThenInclude(r => r.ContratacionProyecto)
                               .ThenInclude(r => r.Proyecto)
               .Include(r => r.SeguimientoSemanalGestionObraCalidad)
                             .ThenInclude(r => r.GestionObraCalidadEnsayoLaboratorio)
               .FirstOrDefaultAsync();

            if (GestionObraCalidadEnsayoLaboratorio.EnsayoLaboratorioMuestra != null && GestionObraCalidadEnsayoLaboratorio.EnsayoLaboratorioMuestra.Count() > 0)
                GestionObraCalidadEnsayoLaboratorio.EnsayoLaboratorioMuestra = GestionObraCalidadEnsayoLaboratorio.EnsayoLaboratorioMuestra.Where(r => !(bool)r.Eliminado).ToList();

            List<Dominio> ListTipoEnsayo = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipos_De_Ensayos_De_Laboratorio).ToList();
            GestionObraCalidadEnsayoLaboratorio.TipoEnsayoCodigo = !string.IsNullOrEmpty(GestionObraCalidadEnsayoLaboratorio.TipoEnsayoCodigo) ? ListTipoEnsayo.Where(r => r.Codigo == GestionObraCalidadEnsayoLaboratorio.TipoEnsayoCodigo).FirstOrDefault().Nombre : " ";
            GestionObraCalidadEnsayoLaboratorio.LlaveMen = GestionObraCalidadEnsayoLaboratorio.SeguimientoSemanalGestionObraCalidad.SeguimientoSemanalGestionObra.SeguimientoSemanal.ContratacionProyecto.Proyecto.LlaveMen;
            int NumeroLaboratorio = 1;

            foreach (var item in GestionObraCalidadEnsayoLaboratorio.SeguimientoSemanalGestionObraCalidad.GestionObraCalidadEnsayoLaboratorio.Where(r => !(bool)r.Eliminado).OrderBy(r => r.GestionObraCalidadEnsayoLaboratorioId))
            {
                if (item.GestionObraCalidadEnsayoLaboratorioId == GestionObraCalidadEnsayoLaboratorio.GestionObraCalidadEnsayoLaboratorioId)
                    GestionObraCalidadEnsayoLaboratorio.NumeroLaboratorio = NumeroLaboratorio;
                NumeroLaboratorio++;
            }
            return GestionObraCalidadEnsayoLaboratorio;
        }

        public async Task<List<VRegistrarAvanceSemanalNew>> GetVRegistrarAvanceSemanalNew()
        {

            List<VRegistrarAvanceSemanalNew> vRegistrarAvanceSemanal = await _context.VRegistrarAvanceSemanalNew.OrderByDescending(r => r.FechaUltimoReporte).ToListAsync();
            vRegistrarAvanceSemanal.ForEach(r =>{
                //Nueva restricción control de cambios
                r.CumpleCondicionesTai = _contractualControversy.ValidarCumpleTaiContratista(r.ContratoId,false);
            });
            return vRegistrarAvanceSemanal;
        }
        public async Task<List<VRegistrarAvanceSemanal>> GetVRegistrarAvanceSemanal()
        {
            return await _context.VRegistrarAvanceSemanal.OrderByDescending(r => r.FechaUltimoReporte).ToListAsync();
        }

        public List<dynamic> GetPeriodoReporteMensualFinanciero(SeguimientoSemanal pSeguimientoSemanal)
        {
            List<SeguimientoSemanal> seguimientoSemanals =
                                   _context.SeguimientoSemanal.Where(r => r.ContratacionProyectoId == pSeguimientoSemanal.ContratacionProyectoId && r.NumeroSemana < pSeguimientoSemanal.NumeroSemana)
                                                                                                       .OrderBy(s => s.NumeroSemana)
                                                                                                                                    .Take(4).ToList();

            return new List<dynamic>
            {
                seguimientoSemanals.FirstOrDefault().FechaInicio,
                seguimientoSemanals.LastOrDefault().FechaFin
            };
        }

        public List<Programacion> GetListProgramacionBySeguimientoSemanal(SeguimientoSemanal pSeguimientoSemanal)
        {
            List<Programacion> ListProgramacionTipoC = _context.Programacion
                                                .Include(r => r.ContratoConstruccion)
                                                .Where(r => r.TipoActividadCodigo == ConstanCodigoTipoActividadProgramacion.Capitulo)
                                                .OrderByDescending(r => r.ProgramacionId).ToList();

            List<Programacion> ListProgramacionTipoI = _context.Programacion
                                                .Include(r => r.ContratoConstruccion)
                                                .Where(r => r.TipoActividadCodigo == ConstanCodigoTipoActividadProgramacion.Item)
                                                .OrderByDescending(r => r.ProgramacionId).ToList();


            List<Programacion> ListProgramacion = new List<Programacion>();

            List<VProgramacionBySeguimientoSemanal> ListProgramaciones =
                _context.VProgramacionBySeguimientoSemanal
                                                        .Where(
                                                            r => r.SeguimientoSemanalId == pSeguimientoSemanal.SeguimientoSemanalId
                                                            && ((r.FechaInicio.Date >= ((DateTime)pSeguimientoSemanal.FechaInicio).Date && r.FechaInicio.Date <= ((DateTime)pSeguimientoSemanal.FechaFin).Date)
                                                                 || (r.FechaFin.Date >= ((DateTime)pSeguimientoSemanal.FechaInicio).Date && r.FechaInicio.Date <= ((DateTime)pSeguimientoSemanal.FechaFin).Date)
                                                                  )
                                                              ).ToList();
            Parallel.ForEach(ListProgramaciones, Programacion =>
            {
                Programacion programacionItem = ListProgramacionTipoI.Where(r => r.ProgramacionId == Programacion.ProgramacionId).FirstOrDefault();

                for (int i = (Programacion.ProgramacionId); i < ListProgramacionTipoI.FirstOrDefault().ProgramacionId + 1; i--)
                {
                    Programacion programacionCapitulo = new Programacion();
                    programacionCapitulo = ListProgramacionTipoC.Where(r => r.ProgramacionId == i).FirstOrDefault();

                    programacionItem.ContratoConstruccion = null;
                    programacionItem.FlujoInversion = null;
                    if (programacionCapitulo != null)
                    {
                        programacionCapitulo.FlujoInversion = null;
                        programacionCapitulo.ContratoConstruccion = null;
                        programacionItem.Capitulo = new
                        {
                            programacionCapitulo.ProgramacionId,
                            programacionCapitulo.Actividad,
                        };
                        break;
                    }
                }
                ListProgramacion.Add(programacionItem);
            });
            return ListProgramacion.Distinct().ToList();
        }

        public async Task<SeguimientoSemanal> GetLastSeguimientoSemanalByContratacionProyectoIdOrSeguimientoSemanalId(int pContratacionProyectoId, int pSeguimientoSemanalId)
        {
            try
            {
                if (pContratacionProyectoId > 0)
                    pSeguimientoSemanalId = _context.SeguimientoSemanal.Where(r => r.ContratacionProyectoId == pContratacionProyectoId && !(bool)r.Eliminado && !(bool)r.RegistroCompleto).FirstOrDefault().SeguimientoSemanalId;

                SeguimientoSemanal seguimientoSemanal = await _context.SeguimientoSemanal.Where(r => r.SeguimientoSemanalId == pSeguimientoSemanalId)
                      .Include(r => r.SeguimientoDiario)
                              .ThenInclude(r => r.SeguimientoDiarioObservaciones)
                          //Financiero
                          .Include(r => r.SeguimientoSemanalAvanceFinanciero)
                       //Fisico
                       .Include(r => r.SeguimientoSemanalAvanceFisico)
                          .ThenInclude(r => r.SeguimientoSemanalAvanceFisicoProgramacion)
                                .ThenInclude(r => r.Programacion)
                       //Gestion Obra
                       //Gestion Obra Ambiental
                       .Include(r => r.SeguimientoSemanalGestionObra)
                            .ThenInclude(r => r.SeguimientoSemanalGestionObraAmbiental)
                         // Gestion Obra Calidad
                         .Include(r => r.SeguimientoSemanalGestionObra)
                            .ThenInclude(r => r.SeguimientoSemanalGestionObraCalidad)
                                .ThenInclude(r => r.GestionObraCalidadEnsayoLaboratorio)
                                    .ThenInclude(r => r.EnsayoLaboratorioMuestra)

                      // Gestion Obra Calidad + Observaciones
                      .Include(r => r.SeguimientoSemanalGestionObra)
                         .ThenInclude(r => r.SeguimientoSemanalGestionObraCalidad)
                             .ThenInclude(r => r.GestionObraCalidadEnsayoLaboratorio)
                                 .ThenInclude(r => r.ObservacionSupervisor)
                      .Include(r => r.SeguimientoSemanalGestionObra)
                         .ThenInclude(r => r.SeguimientoSemanalGestionObraCalidad)
                             .ThenInclude(r => r.GestionObraCalidadEnsayoLaboratorio)
                                 .ThenInclude(r => r.ObservacionApoyo)

                         //   Gestion Obra Calidad
                         .Include(r => r.SeguimientoSemanalGestionObra)
                            .ThenInclude(r => r.SeguimientoSemanalGestionObraSeguridadSalud)
                                .ThenInclude(r => r.SeguridadSaludCausaAccidente)

                        //Gestion Obra Social
                        .Include(r => r.SeguimientoSemanalGestionObra)
                           .ThenInclude(r => r.SeguimientoSemanalGestionObraSocial)

                       .Include(r => r.SeguimientoSemanalGestionObra)
                           .ThenInclude(r => r.SeguimientoSemanalGestionObraAlerta)

                       .Include(r => r.SeguimientoSemanalReporteActividad)
                       .Include(r => r.SeguimientoSemanalRegistroFotografico)
                       .Include(r => r.SeguimientoSemanalRegistrarComiteObra)

                       .FirstOrDefaultAsync();

                await GetModInfoSeguimientoSemanal(seguimientoSemanal);

                return seguimientoSemanal;
            }
            catch (Exception ex)
            {
                return new SeguimientoSemanal();
            }
        }

        private async Task<SeguimientoSemanal> GetModInfoSeguimientoSemanal(SeguimientoSemanal seguimientoSemanal)
        {
            List<Dominio> EstadoDeObraSeguimientoSemanal = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Obra_Avance_Semanal).ToList();

            //Agregar Programacion por seguimiento semanal
            seguimientoSemanal.ListProgramacion = GetListProgramacionBySeguimientoSemanal(seguimientoSemanal);
             
            //enviar periodo reporte financiero
            if (seguimientoSemanal.NumeroSemana % 5 == 0)
            {
                seguimientoSemanal.PeriodoReporteMensualFinanciero = GetPeriodoReporteMensualFinanciero(seguimientoSemanal);
                seguimientoSemanal.TablaFinanciera = GetTableFinanciera(seguimientoSemanal);
            }

            seguimientoSemanal.SeguimientoSemanalObservacion = null;

            if (seguimientoSemanal.SeguimientoSemanalAvanceFisico.Count() > 0)
                seguimientoSemanal.SeguimientoSemanalAvanceFisico.FirstOrDefault().EstadoObraNombre = !string.IsNullOrEmpty(seguimientoSemanal.SeguimientoSemanalAvanceFisico.FirstOrDefault().EstadoObraCodigo) ? EstadoDeObraSeguimientoSemanal.Where(r => r.Codigo == seguimientoSemanal.SeguimientoSemanalAvanceFisico.FirstOrDefault().EstadoObraCodigo).FirstOrDefault().Nombre : "En ejecución";

            List<Dominio> CausaBajaDisponibilidadMaterial = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Causa_Baja_Disponibilidad_Material).ToList();

            List<Dominio> CausaBajaDisponibilidadEquipo = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Causa_Baja_Disponibilidad_Equipo).ToList();

            List<Dominio> CausaBajaDisponibilidadProductividad = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Causa_Baja_Disponibilidad_Productividad).ToList();

            Parallel.ForEach(seguimientoSemanal.SeguimientoDiario, SeguimientoDiario =>
            {
                SeguimientoDiario.CausaBajaDisponibilidadMaterialNombre = !string.IsNullOrEmpty(SeguimientoDiario.CausaIndisponibilidadMaterialCodigo) ? CausaBajaDisponibilidadMaterial.Where(r => r.Codigo == SeguimientoDiario.CausaIndisponibilidadMaterialCodigo).FirstOrDefault().Nombre : "---";

                SeguimientoDiario.CausaBajaDisponibilidadEquipoNombre = !string.IsNullOrEmpty(SeguimientoDiario.CausaIndisponibilidadEquipoCodigo) ? CausaBajaDisponibilidadEquipo.Where(r => r.Codigo == SeguimientoDiario.CausaIndisponibilidadEquipoCodigo).FirstOrDefault().Nombre : "---";

                SeguimientoDiario.CausaBajaDisponibilidadProductividadNombre = !string.IsNullOrEmpty(SeguimientoDiario.CausaIndisponibilidadProductividadCodigo) ? CausaBajaDisponibilidadProductividad.Where(r => r.Codigo == SeguimientoDiario.CausaIndisponibilidadProductividadCodigo).FirstOrDefault().Nombre : "---";

            });

            //Crear Comite Obra numerador 
            seguimientoSemanal.ComiteObraGenerado = await _commonService.EnumeradorComiteObra();

            int? ManejoMaterialesInsumoId, ManejoResiduosConstruccionDemolicionId, ManejoResiduosPeligrososEspecialesId, ManejoOtroId = null;

            if (seguimientoSemanal.SeguimientoSemanalGestionObra.FirstOrDefault() != null)
            {
                if (seguimientoSemanal.SeguimientoSemanalGestionObra.FirstOrDefault().SeguimientoSemanalGestionObraAmbiental.FirstOrDefault() != null)
                {
                    ManejoMaterialesInsumoId = seguimientoSemanal?.SeguimientoSemanalGestionObra?.FirstOrDefault().SeguimientoSemanalGestionObraAmbiental?.FirstOrDefault().ManejoMaterialesInsumoId;
                    ManejoResiduosConstruccionDemolicionId = seguimientoSemanal?.SeguimientoSemanalGestionObra?.FirstOrDefault().SeguimientoSemanalGestionObraAmbiental?.FirstOrDefault().ManejoResiduosConstruccionDemolicionId;
                    ManejoResiduosPeligrososEspecialesId = seguimientoSemanal?.SeguimientoSemanalGestionObra?.FirstOrDefault().SeguimientoSemanalGestionObraAmbiental?.FirstOrDefault().ManejoResiduosPeligrososEspecialesId;
                    ManejoOtroId = seguimientoSemanal?.SeguimientoSemanalGestionObra?.FirstOrDefault().SeguimientoSemanalGestionObraAmbiental?.FirstOrDefault().ManejoOtroId;

                    if (ManejoMaterialesInsumoId != null)
                        seguimientoSemanal.SeguimientoSemanalGestionObra.FirstOrDefault().SeguimientoSemanalGestionObraAmbiental.FirstOrDefault().ManejoMaterialesInsumo = _context.ManejoMaterialesInsumos.Include(r => r.ManejoMaterialesInsumosProveedor).Where(r => r.ManejoMaterialesInsumosId == ManejoMaterialesInsumoId).FirstOrDefault();

                    if (ManejoResiduosConstruccionDemolicionId != null)
                        seguimientoSemanal.SeguimientoSemanalGestionObra.FirstOrDefault().SeguimientoSemanalGestionObraAmbiental.FirstOrDefault().ManejoResiduosConstruccionDemolicion = _context.ManejoResiduosConstruccionDemolicion.Include(r => r.ManejoResiduosConstruccionDemolicionGestor).Where(r => r.ManejoResiduosConstruccionDemolicionId == ManejoResiduosConstruccionDemolicionId).FirstOrDefault();

                    if (ManejoResiduosPeligrososEspecialesId != null)
                        seguimientoSemanal.SeguimientoSemanalGestionObra.FirstOrDefault().SeguimientoSemanalGestionObraAmbiental.FirstOrDefault().ManejoResiduosPeligrososEspeciales = _context.ManejoResiduosPeligrososEspeciales.Find(ManejoResiduosPeligrososEspecialesId);

                    if (ManejoOtroId != null)
                        seguimientoSemanal.SeguimientoSemanalGestionObra.FirstOrDefault().SeguimientoSemanalGestionObraAmbiental.FirstOrDefault().ManejoOtro = _context.ManejoOtro.Find(ManejoOtroId);

                }
            }

            seguimientoSemanal.FlujoInversion = _context.FlujoInversion
                                .Include(r => r.Programacion)
                                .Where(r => r.SeguimientoSemanalId == seguimientoSemanal.SeguimientoSemanalId)
                                .ToList();

            List<int> ListSeguimientoSemanalId =
                _context.SeguimientoSemanal
                .Where(r => r.ContratacionProyectoId == seguimientoSemanal.ContratacionProyectoId)
                .Select(r => r.SeguimientoSemanalId)
                                                   .ToList();

            List<Programacion> ListProgramacion = new List<Programacion>();

            Parallel.ForEach(seguimientoSemanal.SeguimientoSemanalAvanceFisico.ToList(), item =>
            {
                ListProgramacion.AddRange(item.SeguimientoSemanalAvanceFisicoProgramacion
                                         .Select(s => s.Programacion)
                                         .ToList());
            });

            if (ListProgramacion.Count() > 0)
            {
                seguimientoSemanal.CantidadTotalDiasActividades = ListProgramacion.Sum(r => r.Duracion);
            }

            seguimientoSemanal.AvanceAcumulado = ListProgramacion
                .GroupBy(r => r.Actividad)
                .Select(r => new
                {
                    Actividad = r.Key,
                    AvanceAcumulado = Math.Truncate((decimal)r.Sum(r => r.SeguimientoSemanalAvanceFisicoProgramacion.FirstOrDefault().AvanceFisicoCapitulo)) + "%",
                    AvanceFisicoCapitulo = seguimientoSemanal.CantidadTotalDiasActividades > 0 ? Math.Truncate((((decimal)r.Sum(r => r.Duracion) / seguimientoSemanal.CantidadTotalDiasActividades) * 100)) + "%" : "0 %",
                });

            //Eliminar Las tablas eliminadas Logicamente

            Parallel.ForEach(seguimientoSemanal.SeguimientoSemanalGestionObra, SeguimientoSemanalGestionObra =>
            {
                Parallel.ForEach(SeguimientoSemanalGestionObra.SeguimientoSemanalGestionObraAmbiental, SeguimientoSemanalGestionObraAmbiental =>
                {
                    //No incluir los ManejoMaterialesInsumosProveedor eliminados
                    if (SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo != null || SeguimientoSemanalGestionObraAmbiental?.ManejoMaterialesInsumo?.ManejoMaterialesInsumosProveedor.Count() > 0)
                        SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.ManejoMaterialesInsumosProveedor = SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.ManejoMaterialesInsumosProveedor.Where(r => !(bool)r.Eliminado).ToList();
                    //No incluir los Manejo Residuos Construccion Demolicion Gestor eliminados
                    if (SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion != null || SeguimientoSemanalGestionObraAmbiental?.ManejoResiduosConstruccionDemolicion?.ManejoResiduosConstruccionDemolicionGestor.Count() > 0)
                        SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.ManejoResiduosConstruccionDemolicionGestor = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.ManejoResiduosConstruccionDemolicionGestor.Where(r => !(bool)r.Eliminado).ToList();
                });
                Parallel.ForEach(SeguimientoSemanalGestionObra.SeguimientoSemanalGestionObraCalidad, SeguimientoSemanalGestionObraCalidad =>
                {
                    if (SeguimientoSemanalGestionObraCalidad.GestionObraCalidadEnsayoLaboratorio != null || SeguimientoSemanalGestionObraCalidad?.GestionObraCalidadEnsayoLaboratorio.Count() > 0)
                        SeguimientoSemanalGestionObraCalidad.GestionObraCalidadEnsayoLaboratorio = SeguimientoSemanalGestionObraCalidad.GestionObraCalidadEnsayoLaboratorio.Where(r => !(bool)r.Eliminado).ToList();
                });
            });
             
            seguimientoSemanal.InfoProyecto = GetInfoProyectoBySeguimientoContratacionProyectoId(seguimientoSemanal.ContratacionProyectoId);
            seguimientoSemanal.ContratacionProyecto = null;
             
            Parallel.ForEach(seguimientoSemanal.SeguimientoDiario.ToList(), item =>
             {
                 item.SeguimientoDiarioObservaciones = null;
                 item.ContratacionProyecto = null;
             });
             
            Parallel.ForEach(seguimientoSemanal.FlujoInversion.ToList(), item =>
              {
                  item.Programacion.FlujoInversion = null;
                  item.Programacion.ContratoConstruccion = null;
                  item.ContratoConstruccion = null;
              });

            return seguimientoSemanal;
        }

        private dynamic GetInfoProyectoBySeguimientoContratacionProyectoId(int ContratacionProyectoId)
        {
            List<Dominio> TipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();
            List<Localizacion> ListLocalizacion = _context.Localizacion.ToList();

            ContratacionProyecto ContratacionProyecto = _context.ContratacionProyecto
                                .Where(r => r.ContratacionProyectoId == ContratacionProyectoId)
                                     .Include(c => c.Contratacion)
                                        .ThenInclude(c => c.Contrato)
                                   .Include(r => r.Proyecto)
                                   .ThenInclude(r => r.InstitucionEducativa)
                                       .Include(r => r.Proyecto)
                                   .ThenInclude(r => r.Sede)
                                   .Include(s => s.SeguimientoSemanal)
                                   .FirstOrDefault();

            if (string.IsNullOrEmpty(ContratacionProyecto.EstadoObraCodigo))
                ContratacionProyecto.EstadoObraCodigo = ConstanCodigoEstadoObraSeguimientoSemanal.En_ejecucion;

            string strEstadoObra = _context.Dominio.
                Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Obra_Avance_Semanal
                        && r.Codigo == ContratacionProyecto.EstadoObraCodigo)
                                                                            .FirstOrDefault().Nombre;

            return new
            {
                TipoIntervencion = TipoIntervencion.Where(t => t.Codigo == ContratacionProyecto.Proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre,
                Departamento = ListLocalizacion.Where(r => r.LocalizacionId == ContratacionProyecto.Proyecto.LocalizacionIdMunicipioNavigation.IdPadre).FirstOrDefault().Descripcion,
                Municipio = ContratacionProyecto.Proyecto.LocalizacionIdMunicipioNavigation.Descripcion,
                InstitucionEducativa = ContratacionProyecto.Proyecto.InstitucionEducativa.Nombre,
                Sede = ContratacionProyecto.Proyecto.Sede.Nombre,
                NumeroContrato = ContratacionProyecto.Contratacion.Contrato.FirstOrDefault().NumeroContrato,
                LlaveMen = ContratacionProyecto.Proyecto.LlaveMen,
                AvanceFisicoSemanal = ContratacionProyecto.AvanceFisicoSemanal,
                ProgramacionSemanal = ContratacionProyecto.ProgramacionSemanal,
                EstadoObra = strEstadoObra,
                FechaUltimoReporte = ContratacionProyecto.SeguimientoSemanal.Where(r => r.FechaModificacion.HasValue).LastOrDefault()
            };
        }

        /// <summary>
        /// Bitacora
        /// </summary>
        /// <param name="pContratacionProyectoId"></param>
        /// <returns></returns>
        public async Task<List<dynamic>> GetListSeguimientoSemanalByContratacionProyectoId(int pContratacionProyectoId)
        {
            List<dynamic> ListBitaCora = new List<dynamic>();
            List<Dominio> ListEstadoObra = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Obra_Avance_Semanal).ToList();
            List<Dominio> ListEstadoSeguimientoSemanal = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Reporte_Semanal_Y_Muestras).ToList();
            List<SeguimientoSemanal> ListseguimientoSemanal = await _context.SeguimientoSemanal
                                                                    .Where(r => r.ContratacionProyectoId == pContratacionProyectoId
                                                                       && r.RegistroCompleto == true)
                                                                    .Include(r => r.ContratacionProyecto)
                                                                       .ThenInclude(r => r.Proyecto)
                                                                    .Include(r => r.ContratacionProyecto)
                                                                       .ThenInclude(r => r.Contratacion)
                                                                           .ThenInclude(r => r.Contrato)
                                                                    .Include(r => r.SeguimientoSemanalAvanceFisico)
                                                                    .Include(r => r.SeguimientoSemanalGestionObra)
                                                                       .ThenInclude(r => r.SeguimientoSemanalGestionObraCalidad)
                                                                           .ThenInclude(r => r.GestionObraCalidadEnsayoLaboratorio)
                                                                               .ThenInclude(r => r.EnsayoLaboratorioMuestra)
                                                                    .ToListAsync();
            try
            {
                int UltimaSemana = _context.SeguimientoSemanal.Count(s => s.ContratacionProyectoId == pContratacionProyectoId);

                foreach (var item in ListseguimientoSemanal)
                {
                    decimal? ProgramacionAcumulada = 0, AvanceFisico = 0;
                    string strCodigoEstadoObra = string.Empty;
                    string strCodigoEstadoMuestas = string.Empty;
                    if (item.SeguimientoSemanalAvanceFisico.Count() > 0)
                    {
                        if (item.SeguimientoSemanalAvanceFisico.FirstOrDefault().ProgramacionSemanal.HasValue)
                            ProgramacionAcumulada = item.SeguimientoSemanalAvanceFisico.FirstOrDefault().ProgramacionSemanal;

                        if (item.SeguimientoSemanalAvanceFisico.FirstOrDefault().AvanceFisicoSemanal.HasValue)
                            AvanceFisico = item.SeguimientoSemanalAvanceFisico.FirstOrDefault().AvanceFisicoSemanal;

                        if (!string.IsNullOrEmpty(item.SeguimientoSemanalAvanceFisico.FirstOrDefault().EstadoObraCodigo))
                            strCodigoEstadoObra = ListEstadoObra.Where(r => r.Codigo == item.SeguimientoSemanalAvanceFisico.FirstOrDefault().EstadoObraCodigo).FirstOrDefault().Nombre;
                    }

                    if (string.IsNullOrEmpty(item.EstadoMuestrasCodigo))
                        item.EstadoMuestrasCodigo = ConstanCodigoEstadoSeguimientoSemanal.Sin_Muestras;

                    strCodigoEstadoMuestas = ListEstadoSeguimientoSemanal.Where(r => r.Codigo == item.EstadoMuestrasCodigo).FirstOrDefault().Nombre;

                    bool? RegistroCompletoMuestrasVerificar =
                         item.SeguimientoSemanalGestionObra?
                        .FirstOrDefault()?.SeguimientoSemanalGestionObraCalidad?
                        .FirstOrDefault()?.GestionObraCalidadEnsayoLaboratorio?
                        .FirstOrDefault()?.EnsayoLaboratorioMuestra?
                        .FirstOrDefault()?.RegistroCompletoObservacionApoyo;

                    if (RegistroCompletoMuestrasVerificar == null)
                        RegistroCompletoMuestrasVerificar = false;

                    bool? RegistroCompletoMuestrasValidar =
                         item.SeguimientoSemanalGestionObra?
                        .FirstOrDefault()?.SeguimientoSemanalGestionObraCalidad?
                        .FirstOrDefault()?.GestionObraCalidadEnsayoLaboratorio?
                        .FirstOrDefault()?.EnsayoLaboratorioMuestra?
                        .FirstOrDefault()?.RegistroCompletoObservacionApoyo;
                    if (RegistroCompletoMuestrasValidar == null)
                        RegistroCompletoMuestrasValidar = false;

                    bool? verDetalleEditar = !item.SeguimientoSemanalGestionObra.FirstOrDefault()?.SeguimientoSemanalGestionObraCalidad.FirstOrDefault()?.SeRealizaronEnsayosLaboratorio;

                    if (verDetalleEditar == false)
                        verDetalleEditar = item.RegistroCompletoMuestras.HasValue ? !item.RegistroCompletoMuestras : false;

                    ListBitaCora.Add(new
                    {
                        UltimoReporte = item.FechaModificacion,
                        item.SeguimientoSemanalId,
                        RegistroCompletoMuestras = verDetalleEditar,
                        item.NumeroSemana,
                        UltimaSemana,
                        item.FechaInicio,
                        item.FechaFin,
                        EstadoObra = strCodigoEstadoObra,
                        ProgramacionAcumulada = Math.Truncate((decimal)ProgramacionAcumulada),
                        AvanceFisico = Math.Truncate((decimal)AvanceFisico),
                        EstadoRegistro = item.RegistroCompletoMuestras.HasValue ? item.RegistroCompletoMuestras : false,
                        item.ContratacionProyecto?.Proyecto?.LlaveMen,
                        item.ContratacionProyecto?.Contratacion?.Contrato?.FirstOrDefault()?.NumeroContrato,
                        EstadoReporteSemanal = !string.IsNullOrEmpty(item.EstadoSeguimientoSemanalCodigo) ? ListEstadoSeguimientoSemanal.Where(r => r.Codigo == item.EstadoSeguimientoSemanalCodigo).FirstOrDefault().Nombre : "---",
                        EstadoMuestrasReporteSemanal = strCodigoEstadoMuestas,
                        RegistroCompletoMuestrasVerificar = RegistroCompletoMuestrasVerificar,
                        RegistroCompletoMuestrasValidar = RegistroCompletoMuestrasValidar
                    });
                }
                return ListBitaCora;
            }
            catch (Exception ex)
            {
                return ListBitaCora;
            }
        }
        #endregion

        #region Save Edit

        #region delete
        public async Task<Respuesta> DeleteGestionObraCalidadEnsayoLaboratorio(int GestionObraCalidadEnsayoLaboratorioId, string pUsuarioModificacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Gestion_Obra_Calidad_Ensayo_Laboratorio, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                GestionObraCalidadEnsayoLaboratorio GestionObraCalidadEnsayoLaboratorioOld = await _context.GestionObraCalidadEnsayoLaboratorio
                    .Where(r => r.GestionObraCalidadEnsayoLaboratorioId == GestionObraCalidadEnsayoLaboratorioId).Include(r => r.EnsayoLaboratorioMuestra).FirstOrDefaultAsync();

                if (GestionObraCalidadEnsayoLaboratorioOld == null)
                {
                    return new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = true,
                        IsValidation = false,
                        Code = ConstanMessagesRegisterWeeklyProgress.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, ConstanMessagesRegisterWeeklyProgress.Error, idAccion, pUsuarioModificacion, "GestionObraCalidadEnsayoLaboratorio no encontrado".ToUpper())
                    };
                }
                if (GestionObraCalidadEnsayoLaboratorioOld.EnsayoLaboratorioMuestra != null && GestionObraCalidadEnsayoLaboratorioOld.EnsayoLaboratorioMuestra.Where(r => !(bool)r.Eliminado).Count() > 0)
                {
                    return new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = true,
                        IsValidation = false,
                        Code = ConstanMessagesRegisterWeeklyProgress.NoEliminarLaboratorio,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, ConstanMessagesRegisterWeeklyProgress.NoEliminarLaboratorio, idAccion, pUsuarioModificacion, "El registro tiene información que depende de él, no se puede eliminar".ToUpper())
                    };
                }
                GestionObraCalidadEnsayoLaboratorioOld.UsuarioModificacion = pUsuarioModificacion;
                GestionObraCalidadEnsayoLaboratorioOld.FechaModificacion = DateTime.Now;
                GestionObraCalidadEnsayoLaboratorioOld.Eliminado = true;

                await _context.SaveChangesAsync();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.EliminacionExitosa,
                    Message = await
                    _commonService.GetMensajesValidacionesByModuloAndCodigo(
                                                                        (int)enumeratorMenu.Registrar_Avance_Semanal,
                                                                        ConstanMessagesRegisterWeeklyProgress.OperacionExitosa,
                                                                        idAccion,
                                                                        pUsuarioModificacion,
                                                                        "Eliminar Gestion Obra Calidad Ensayo Laboratorio".ToUpper()
                                                                        )
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, ConstantSesionComiteTecnico.Error, idAccion, pUsuarioModificacion, ex.InnerException.ToString())
                };
            }


        }

        public async Task<Respuesta> DeleteResiduosConstruccionDemolicionGestor(int ResiduosConstruccionDemolicionGestorId, string pUsuarioModificacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Residuos_Construccion_Demolicion_Gestor, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ManejoResiduosConstruccionDemolicionGestor ManejoResiduosConstruccionDemolicionGestorOld = _context.ManejoResiduosConstruccionDemolicionGestor.Find(ResiduosConstruccionDemolicionGestorId);

                if (ManejoResiduosConstruccionDemolicionGestorOld == null)
                {
                    return new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = true,
                        IsValidation = false,
                        Code = ConstanMessagesRegisterWeeklyProgress.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, ConstanMessagesRegisterWeeklyProgress.Error, idAccion, pUsuarioModificacion, "ManejoResiduosConstruccionDemolicionGestor no encontrado".ToUpper())
                    };
                }
                ManejoResiduosConstruccionDemolicionGestorOld.UsuarioModificacion = pUsuarioModificacion;
                ManejoResiduosConstruccionDemolicionGestorOld.FechaModificacion = DateTime.Now;
                ManejoResiduosConstruccionDemolicionGestorOld.Eliminado = true;

                await _context.SaveChangesAsync();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.EliminacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, GeneralCodes.EliminacionExitosa, idAccion, pUsuarioModificacion, "Eliminar Manejo Materiales Insumo Proveedor".ToUpper())
                };

            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstanMessagesRegisterWeeklyProgress.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, ConstanMessagesRegisterWeeklyProgress.Error, idAccion, pUsuarioModificacion, ex.InnerException.ToString())
                };
            }


        }

        public async Task<Respuesta> DeleteManejoMaterialesInsumosProveedor(int ManejoMaterialesInsumosProveedorId, string pUsuarioModificacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Manejo_Materiales_Insumo_Proveedor, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ManejoMaterialesInsumosProveedor manejoMaterialesInsumosProveedorDelete = _context.ManejoMaterialesInsumosProveedor.Find(ManejoMaterialesInsumosProveedorId);

                if (manejoMaterialesInsumosProveedorDelete == null)
                {
                    return new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = true,
                        IsValidation = false,
                        Code = ConstanMessagesRegisterWeeklyProgress.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, ConstanMessagesRegisterWeeklyProgress.Error, idAccion, pUsuarioModificacion, "ManejoMaterialesInsumosProveedor no encontrado".ToUpper())
                    };
                }
                manejoMaterialesInsumosProveedorDelete.UsuarioModificacion = pUsuarioModificacion;
                manejoMaterialesInsumosProveedorDelete.FechaModificacion = DateTime.Now;
                manejoMaterialesInsumosProveedorDelete.Eliminado = true;

                await _context.SaveChangesAsync();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.EliminacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, GeneralCodes.EliminacionExitosa, idAccion, pUsuarioModificacion, "Eliminar Manejo Materiales Insumo Proveedor".ToUpper())
                };

            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = GeneralCodes.EliminacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, GeneralCodes.EliminacionExitosa, idAccion, pUsuarioModificacion, ex.InnerException.ToString())
                };
            }


        }

        private void EliminarGestionObraCalidadEnsayoLaboratorioAndEnsayoLaboratorioMuestra(int pSeguimientoSemanalGestionObraCalidadId, int pSeguimientoSemanalGestionObraId)
        {
            GestionObraCalidadEnsayoLaboratorio gestionObraCalidadEnsayoLaboratorio = _context.GestionObraCalidadEnsayoLaboratorio
                                              .Where(r => r.SeguimientoSemanalGestionObraCalidadId == pSeguimientoSemanalGestionObraCalidadId)
                                              .Include(r => r.EnsayoLaboratorioMuestra).FirstOrDefault();

            if (gestionObraCalidadEnsayoLaboratorio != null)
            {
                gestionObraCalidadEnsayoLaboratorio.Eliminado = true;

                foreach (var EnsayoLaboratorioMuestra in gestionObraCalidadEnsayoLaboratorio.EnsayoLaboratorioMuestra)
                {
                    EnsayoLaboratorioMuestra.Eliminado = true;
                }
            }

            //Pasar Registro Completo Ensayo Laboratorio Muestras
            // Y estado Muestras = sin muestras
            int SeguimientoSemanalId = _context.SeguimientoSemanalGestionObra.Find(pSeguimientoSemanalGestionObraId).SeguimientoSemanalId;

            _context.Set<SeguimientoSemanal>()
                                .Where(s => s.SeguimientoSemanalId == SeguimientoSemanalId)
                                                                                            .Update(s => new SeguimientoSemanal
                                                                                            {
                                                                                                EstadoMuestrasCodigo = ConstanCodigoEstadoSeguimientoSemanal.Sin_Muestras,
                                                                                                RegistroCompletoMuestras = true
                                                                                            });




        }

        #endregion

        #region validate 
        private string ValidarEstadoDeObraBySeguimientoSemanalId(int SeguimientoSemanalId)
        {

            //EstadosDisponibilidad codigo =  7 6 cuando esta estos estados de obra desabilitar 
            ///Validar Estado De obra 
            //Actualizar estado obra 


            SeguimientoSemanal seguimientoSemanal =
                _context.SeguimientoSemanal.Where(r => r.SeguimientoSemanalId == SeguimientoSemanalId)
                        .Include(c => c.ContratacionProyecto)
                        .Include(s => s.SeguimientoSemanalAvanceFisico)
                            .ThenInclude(r => r.SeguimientoSemanalAvanceFisicoProgramacion)
                            .ThenInclude(r => r.Programacion)
                            .FirstOrDefault();

            decimal? ProgramacionAcumuladaObra = seguimientoSemanal.SeguimientoSemanalAvanceFisico.Sum(s => s.ProgramacionSemanal);
            decimal? ProgramacionEjecutadaObra = seguimientoSemanal.SeguimientoSemanalAvanceFisico.Sum(s => s.AvanceFisicoSemanal); ;


            int CantidadDeSeguimientosSemanales = _context.SeguimientoSemanal.Where(r => r.ContratacionProyectoId == seguimientoSemanal.ContratacionProyectoId).ToList().Count();
            decimal PrimerTercio = decimal.Round(CantidadDeSeguimientosSemanales / 3);
            decimal SegundoTercio = PrimerTercio * 2;

            if (ProgramacionAcumuladaObra.HasValue && ProgramacionEjecutadaObra.HasValue)
            {
                /////Programación acumulada de la obra: == Avance acumulado ejecutado de la obra:   = normal
                if (ProgramacionAcumuladaObra == ProgramacionEjecutadaObra)
                    return ConstanCodigoEstadoObraSeguimientoSemanal.Con_ejecucion_normal;

                /////Programación acumulada de la obra: <  Avance acumulado ejecutado de la obra:   = avanzada
                if (ProgramacionAcumuladaObra < ProgramacionEjecutadaObra)
                    return ConstanCodigoEstadoObraSeguimientoSemanal.Con_ejecucion_avanzada;

                /////Programación acumulada de la obra: >  Avance acumulado ejecutado de la obra:   = retrazado
                if (ProgramacionAcumuladaObra > ProgramacionEjecutadaObra)
                    return ConstanCodigoEstadoObraSeguimientoSemanal.Con_ejecucion_retrazada;

                //primer tercio  => avance del proyecto no debe ser menor al 20%   = critico
                if (seguimientoSemanal.NumeroSemana >= PrimerTercio && seguimientoSemanal.NumeroSemana < SegundoTercio)
                    if (ProgramacionEjecutadaObra < 20)
                        return ConstanCodigoEstadoObraSeguimientoSemanal.Con_ejecucion_critica;

                //segunto tercio  => avance del proyecto no debe ser menor al 60%   critico
                if (seguimientoSemanal.NumeroSemana >= SegundoTercio)
                    if (ProgramacionEjecutadaObra < 60)
                        return ConstanCodigoEstadoObraSeguimientoSemanal.Con_ejecucion_critica;

                return ConstanCodigoEstadoObraSeguimientoSemanal.En_ejecucion;
            }
            else
            {
                return ConstanCodigoEstadoObraSeguimientoSemanal.En_ejecucion;
            }
        }

        private bool ValidarRegistroCompletoAvanceFisico(SeguimientoSemanalAvanceFisico seguimientoSemanalAvanceFisico)
        {
            bool EsCompleto = true;
            Parallel.ForEach(seguimientoSemanalAvanceFisico.SeguimientoSemanalAvanceFisicoProgramacion, item =>
            {
                if (item.AvanceFisicoCapitulo == null)
                    EsCompleto = false;
            });
            return EsCompleto;
        }


        #endregion

        public async Task<Respuesta> SaveUpdateSeguimientoSemanal(SeguimientoSemanal pSeguimientoSemanal)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Seguimiento_Semanal, (int)EnumeratorTipoDominio.Acciones);
            bool blActualizarContratacionProyecto = false;
            try
            {
                if (pSeguimientoSemanal.SeguimientoSemanalAvanceFisico.Count() > 0)
                {
                    SaveUpdateAvanceFisico(pSeguimientoSemanal, pSeguimientoSemanal.UsuarioCreacion);
                    blActualizarContratacionProyecto = true;
                }
                if (pSeguimientoSemanal.SeguimientoSemanalAvanceFinanciero.Count() > 0)
                    SaveUpdateAvanceFinanciero(pSeguimientoSemanal?.SeguimientoSemanalAvanceFinanciero?.FirstOrDefault(), pSeguimientoSemanal.UsuarioCreacion);

                if (pSeguimientoSemanal.SeguimientoSemanalGestionObra.Count() > 0)
                    SaveUpdateGestionObra(pSeguimientoSemanal?.SeguimientoSemanalGestionObra?.FirstOrDefault(), pSeguimientoSemanal.UsuarioCreacion);

                if (pSeguimientoSemanal.SeguimientoSemanalReporteActividad.Count() > 0)
                    SaveUpdateReporteActividades(pSeguimientoSemanal?.SeguimientoSemanalReporteActividad?.FirstOrDefault(), pSeguimientoSemanal.UsuarioCreacion);

                if (pSeguimientoSemanal.SeguimientoSemanalRegistroFotografico.Count() > 0)
                    SaveUpdateRegistroFotografico(pSeguimientoSemanal?.SeguimientoSemanalRegistroFotografico?.FirstOrDefault(), pSeguimientoSemanal.UsuarioCreacion);

                if (pSeguimientoSemanal.SeguimientoSemanalRegistrarComiteObra.Count() > 0)
                    SaveUpdateComiteObra(pSeguimientoSemanal?.SeguimientoSemanalRegistrarComiteObra?.FirstOrDefault(), pSeguimientoSemanal.UsuarioCreacion);

                await _context.SaveChangesAsync();

                SeguimientoSemanal seguimientoSemanalMod = await _context.SeguimientoSemanal.FindAsync(pSeguimientoSemanal.SeguimientoSemanalId);
                seguimientoSemanalMod.UsuarioModificacion = pSeguimientoSemanal.UsuarioCreacion;
                seguimientoSemanalMod.FechaModificacion = DateTime.Now;

                if (ValidarRegistroCompletoSeguimientoSemanal(seguimientoSemanalMod))
                    seguimientoSemanalMod.FechaRegistroCompletoInterventor = DateTime.Now;
                else
                    seguimientoSemanalMod.FechaRegistroCompletoInterventor = null;

                _context.Update(seguimientoSemanalMod);
                _context.SaveChanges();

                if (blActualizarContratacionProyecto)
                    UpdateContratacionProyecto(pSeguimientoSemanal);

                return new Respuesta
                {

                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstanMessagesRegisterWeeklyProgress.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, ConstanMessagesRegisterWeeklyProgress.OperacionExitosa, idAccion, pSeguimientoSemanal.UsuarioCreacion, pSeguimientoSemanal.FechaModificacion.HasValue ? "CREAR SEGUIMIENTO SEMANAL" : "EDITAR SEGUIMIENTO SEMANAL")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstanMessagesRegisterWeeklyProgress.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, ConstanMessagesRegisterWeeklyProgress.Error, idAccion, pSeguimientoSemanal.UsuarioCreacion, ex.InnerException.ToString())
                };
            }
        }

        private void UpdateContratacionProyecto(SeguimientoSemanal pSeguimientoSemanal)
        {
            decimal? AvanceFisicoSemanal = _context.SeguimientoSemanalAvanceFisico.Where(r => r.SeguimientoSemanalId == pSeguimientoSemanal.SeguimientoSemanalId).Sum(s => s.AvanceFisicoSemanal);
            decimal? ProgramacionSemanal = _context.SeguimientoSemanalAvanceFisico.Where(r => r.SeguimientoSemanalId == pSeguimientoSemanal.SeguimientoSemanalId).Sum(s => s.ProgramacionSemanal);

            string strEstadoObraCodigo = ValidarEstadoDeObraBySeguimientoSemanalId(pSeguimientoSemanal.SeguimientoSemanalId);

            _context.Set<ContratacionProyecto>()
                    .Where(r => r.ContratacionProyectoId == pSeguimientoSemanal.ContratacionProyectoId)
                    .Update(r => new ContratacionProyecto
                    {
                        AvanceFisicoSemanal = AvanceFisicoSemanal,
                        ProgramacionSemanal = ProgramacionSemanal,
                        EstadoObraCodigo = strEstadoObraCodigo
                    });

            _context.Set<SeguimientoSemanalAvanceFisico>()
                     .Where(r => r.SeguimientoSemanalId == pSeguimientoSemanal.SeguimientoSemanalId)
                                      .Update(r => new SeguimientoSemanalAvanceFisico
                                      {
                                          EstadoObraCodigo = strEstadoObraCodigo
                                      });

        }

        public async Task<Respuesta> UploadContractTerminationCertificate(ContratacionProyecto pContratacionProyecto, AppSettingsService appSettingsService)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cargar_Acta_Terminacion_Contrato, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ContratacionProyecto contratacionProyecto = _context.ContratacionProyecto.Find(pContratacionProyecto.ContratacionProyectoId);
                contratacionProyecto.UsuarioModificacion = pContratacionProyecto.UsuarioCreacion;
                contratacionProyecto.FechaModificacion = DateTime.Now;

                //Para Contratos Tipo B (string URL)
                if (!string.IsNullOrEmpty(pContratacionProyecto.RutaCargaActaTerminacionContrato))
                    contratacionProyecto.RutaCargaActaTerminacionContrato = pContratacionProyecto.RutaCargaActaTerminacionContrato;

                //Para Contratos Tipo A (File)
                else
                {
                    contratacionProyecto.RutaCargaActaTerminacionContrato =
                        Path.Combine(appSettingsService.DirectoryBase,
                                     appSettingsService.DirectoryRutaCargaActaTerminacionContrato,
                                     pContratacionProyecto.ContratacionProyectoId.ToString(),
                                     pContratacionProyecto.pFile.FileName);

                    await _documentService.SaveFileContratacion(pContratacionProyecto.pFile, Path.Combine(appSettingsService.DirectoryBase,
                                                                                                           appSettingsService.DirectoryRutaCargaActaTerminacionContrato,
                                                                                                           pContratacionProyecto.ContratacionProyectoId.ToString()), pContratacionProyecto.pFile.FileName);
                }
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstanMessagesRegisterWeeklyProgress.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, ConstanMessagesRegisterWeeklyProgress.OperacionExitosa, idAccion, pContratacionProyecto.UsuarioCreacion, "CARGAR ACTA TERMINACION DEL CONTRATO")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstanMessagesRegisterWeeklyProgress.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, ConstanMessagesRegisterWeeklyProgress.Error, idAccion, pContratacionProyecto.UsuarioCreacion, ex.InnerException.ToString())
                };
            }
        }

        public async Task<Respuesta> ChangueStatusSeguimientoSemanal(int pContratacionProyectoId, string pEstadoMod, string pUsuarioMod)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Seguimiento_Semanal, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                SeguimientoSemanal seguimientoSemanalMod = _context.SeguimientoSemanal.Find(pContratacionProyectoId);

                seguimientoSemanalMod.EstadoSeguimientoSemanalCodigo = pEstadoMod;
                seguimientoSemanalMod.UsuarioModificacion = pUsuarioMod;
                seguimientoSemanalMod.FechaModificacion = DateTime.Now;

                if (pEstadoMod == ConstanCodigoEstadoSeguimientoSemanal.Enviado_Verificacion)
                {
                    await SendEmailWhenCompleteWeeklyProgress(seguimientoSemanalMod.SeguimientoSemanalId);

                    GetEliminarRegistrCompletoObservacionesSeguimientoSemanal(seguimientoSemanalMod.SeguimientoSemanalId);
                    GetArchivarObservacionesSeguimientoSemanal(seguimientoSemanalMod.SeguimientoSemanalId, pUsuarioMod);
                    seguimientoSemanalMod.RegistroCompleto = true;
                    seguimientoSemanalMod.FechaRegistroCompletoInterventor = DateTime.Now;
                }
                if (pEstadoMod == ConstanCodigoEstadoSeguimientoSemanal.Enviado_Validacion)
                {
                    await SendEmailToAproved(seguimientoSemanalMod.SeguimientoSemanalId);
                    seguimientoSemanalMod.FechaRegistroCompletoApoyo = DateTime.Now;
                }

                if (pEstadoMod == ConstanCodigoEstadoSeguimientoSemanal.Validado_Supervisor)
                {
                    seguimientoSemanalMod.FechaRegistroCompletoSupervisor = DateTime.Now;
                }

                if (pEstadoMod == ConstanCodigoEstadoSeguimientoSemanal.Devuelto_Supervisor)
                {
                    seguimientoSemanalMod.FechaRegistroCompletoInterventor = null;
                    seguimientoSemanalMod.RegistroCompleto = false;
                    seguimientoSemanalMod.RegistroCompletoVerificar = false;
                    seguimientoSemanalMod.RegistroCompletoAvalar = false;
                    seguimientoSemanalMod.EstadoSeguimientoSemanalCodigo = ConstanCodigoEstadoSeguimientoSemanal.Devuelto_Supervisor;

                    //List<SeguimientoSemanalObservacion> ListSeguimientoSemanalObservacion =
                    //                        _context.SeguimientoSemanalObservacion
                    //                        .Where(r => r.SeguimientoSemanalId == seguimientoSemanalMod.SeguimientoSemanalId)
                    //                                                                                    .ToList();

                    //ListSeguimientoSemanalObservacion.ForEach(s =>
                    //{
                    //    s.Archivada = true;
                    //    s.FechaModificacion = DateTime.Now;
                    //    s.UsuarioModificacion = pUsuarioMod;
                    //});
                }

                string strNombreSEstadoObraCodigo = _context.Dominio.Where(r => r.Codigo == pEstadoMod && r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Reporte_Semanal_Y_Muestras).FirstOrDefault().Nombre;

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstanMessagesRegisterWeeklyProgress.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, ConstanMessagesRegisterWeeklyProgress.OperacionExitosa, idAccion, pUsuarioMod, "EL ESTADO DEL SEGUIMIENTO SEMANAL CAMBIO A: " + strNombreSEstadoObraCodigo.ToUpper())
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstanMessagesRegisterWeeklyProgress.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, ConstanMessagesRegisterWeeklyProgress.Error, idAccion, pUsuarioMod, ex.InnerException.ToString())
                };
            }
        }

        private void GetEliminarRegistrCompletoObservacionesSeguimientoSemanal(int seguimientoSemanalId)
        {
            List<SeguimientoSemanalObservacion> seguimientoSemanalObservacions =
                _context.SeguimientoSemanalObservacion
                .Where(s => s.SeguimientoSemanalId == seguimientoSemanalId).ToList();

            foreach (var item in seguimientoSemanalObservacions)
            {
                CreateEditSeguimientoSemanalObservacion(item, true);
            }

        }

        public Respuesta CreateEditSeguimientoSemanalObservacion(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion, bool pEliminarRegistroCompleto)
        {
            try
            {
                switch (pSeguimientoSemanalObservacion.TipoObservacionCodigo)
                {

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.AVANCE_FISICO:
                        CreateOrEditObservacionAvanceFisico(pSeguimientoSemanalObservacion, pEliminarRegistroCompleto);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.AVANCE_FINANCIERO:
                        CreateOrEditObservacionAvanceFinanciero(pSeguimientoSemanalObservacion, pEliminarRegistroCompleto);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.GESTION_OBRA_AMBIENTAL:
                        CreateOrEditObservacionGestionObraAmbiental(pSeguimientoSemanalObservacion, pEliminarRegistroCompleto);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.GESTION_OBRA_AMBIENTAL_MANEJO_MATERIALES:
                        CreateOrEditObservacionGestionObraAmbientalManejoMateriales(pSeguimientoSemanalObservacion, pEliminarRegistroCompleto);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.GESTION_OBRA_AMBIENTAL_MANEJO_CONSTRUCCION_DEMOLICION:
                        CreateOrEditObservacionGestionObraAmbientalManejoConstruccionDemolicion(pSeguimientoSemanalObservacion, pEliminarRegistroCompleto);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.GESTION_OBRA_AMBIENTAL_MANEJO_RESIDUOS_PELIGROSOS:
                        CreateOrEditObservacionGestionObraAmbientalManejoResiduosPeligrosos(pSeguimientoSemanalObservacion, pEliminarRegistroCompleto);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.GESTION_OBRA_AMBIENTAL_MANEJO_OTRA:
                        CreateOrEditObservacionGestionObraAmbientalManejoOtra(pSeguimientoSemanalObservacion, pEliminarRegistroCompleto);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.GESTION_CALIDAD:
                        CreateOrEditObservacionGestionCalidad(pSeguimientoSemanalObservacion, pEliminarRegistroCompleto);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.GESTION_CALIDAD_ENSAYO_LABORATORIO:
                        CreateOrEditObservacionGestionCalidadEnsayoLaboratorio(pSeguimientoSemanalObservacion, pEliminarRegistroCompleto);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.GESTION_CALIDAD_ENSAYO_LABORATORIO_MUESTRAS:
                        CreateOrEditObservacionGestionCalidadEnsayoLaboratorioMuestras(pSeguimientoSemanalObservacion, pEliminarRegistroCompleto);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.GESTION_SEGURIDAD_Y_SALUD:
                        CreateOrEditObservacionGestionSeguridadSalud(pSeguimientoSemanalObservacion, pEliminarRegistroCompleto);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.GESTION_SOCIAL:
                        CreateOrEditObservacionGestionSocial(pSeguimientoSemanalObservacion, pEliminarRegistroCompleto);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.ALERTAS_RELEVANTES:
                        CreateOrEditObservacionAlertasRelevantes(pSeguimientoSemanalObservacion, pEliminarRegistroCompleto);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.REPORTE_ACTIVIDADES:
                        CreateOrEditObservacionReporteActividades(pSeguimientoSemanalObservacion, pEliminarRegistroCompleto);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.REPORTE_ACTIVIDADES_ESTADO_OBRA:
                        CreateOrEditObservacionReporteActividadesEstadoContrato(pSeguimientoSemanalObservacion, pEliminarRegistroCompleto);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.REPORTE_ACTIVIDADES_ACTIVIDADES_REALIZADAS:
                        CreateOrEditObservacionReporteActividadesRealizadas(pSeguimientoSemanalObservacion, pEliminarRegistroCompleto);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.REPORTE_ACTIVIDADES_ACTIVIDADES_REALIZADAS_SIGUIENTE_SEMANA:
                        CreateOrEditObservacionReporteActividadesRealizadasSiguienteSemana(pSeguimientoSemanalObservacion, pEliminarRegistroCompleto);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.REGISTRO_FOTOGRAFICO:
                        CreateOrEditObservacionRegistroFotografico(pSeguimientoSemanalObservacion, pEliminarRegistroCompleto);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.COMITE_OBRA:
                        CreateOrEditObservacionComiteObra(pSeguimientoSemanalObservacion, pEliminarRegistroCompleto);
                        break;

                    default:
                        break;
                }

                _context.SaveChanges();

                return new Respuesta();

            }
            catch (Exception ex)
            {
                return new Respuesta();
            }
        }

        private void CreateOrEditObservacionAlertasRelevantes(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion, bool pEliminarRegistrCompleto)
        {
            SeguimientoSemanalGestionObraAlerta seguimientoSemanalGestionObraAlertaOld = _context.SeguimientoSemanalGestionObraAlerta.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);

            seguimientoSemanalGestionObraAlertaOld.FechaModificacion = DateTime.Now;
            seguimientoSemanalGestionObraAlertaOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pEliminarRegistrCompleto)
            {
                seguimientoSemanalGestionObraAlertaOld.RegistroCompletoObservacionApoyo = null;
                seguimientoSemanalGestionObraAlertaOld.RegistroCompletoObservacionSupervisor = null;
            }

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
                // seguimientoSemanalGestionObraAlertaOld.RegistroCompleto = false;
                seguimientoSemanalGestionObraAlertaOld.TieneObservacionSupervisor = pSeguimientoSemanalObservacion.TieneObservacion;
                seguimientoSemanalGestionObraAlertaOld.ObservacionSupervisorId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                seguimientoSemanalGestionObraAlertaOld.RegistroCompletoObservacionSupervisor = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
            else
            {
                seguimientoSemanalGestionObraAlertaOld.TieneObservacionApoyo = pSeguimientoSemanalObservacion.TieneObservacion;
                seguimientoSemanalGestionObraAlertaOld.ObservacionApoyoId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                seguimientoSemanalGestionObraAlertaOld.RegistroCompletoObservacionApoyo = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }

        }

        private void CreateOrEditObservacionGestionSocial(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion, bool pEliminarRegistrCompleto)
        {
            SeguimientoSemanalGestionObraSocial seguimientoSemanalGestionObraSocialOld = _context.SeguimientoSemanalGestionObraSocial.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);

            seguimientoSemanalGestionObraSocialOld.FechaModificacion = DateTime.Now;
            seguimientoSemanalGestionObraSocialOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pEliminarRegistrCompleto)
            {
                seguimientoSemanalGestionObraSocialOld.RegistroCompletoObservacionApoyo = null;
                seguimientoSemanalGestionObraSocialOld.RegistroCompletoObservacionSupervisor = null;
            }

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
                // seguimientoSemanalGestionObraSocialOld.RegistroCompleto = false;
                seguimientoSemanalGestionObraSocialOld.TieneObservacionSupervisor = pSeguimientoSemanalObservacion.TieneObservacion;
                seguimientoSemanalGestionObraSocialOld.ObservacionSupervisorId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                seguimientoSemanalGestionObraSocialOld.RegistroCompletoObservacionSupervisor = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
            else
            {
                seguimientoSemanalGestionObraSocialOld.TieneObservacionApoyo = pSeguimientoSemanalObservacion.TieneObservacion;
                seguimientoSemanalGestionObraSocialOld.ObservacionApoyoId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                seguimientoSemanalGestionObraSocialOld.RegistroCompletoObservacionApoyo = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
        }

        private void CreateOrEditObservacionGestionSeguridadSalud(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion, bool pEliminarRegistrCompleto)
        {
            SeguimientoSemanalGestionObraSeguridadSalud seguimientoSemanalGestionObraSeguridadSaludOld = _context.SeguimientoSemanalGestionObraSeguridadSalud.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);

            seguimientoSemanalGestionObraSeguridadSaludOld.FechaModificacion = DateTime.Now;
            seguimientoSemanalGestionObraSeguridadSaludOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pEliminarRegistrCompleto)
            {
                seguimientoSemanalGestionObraSeguridadSaludOld.RegistroCompletoObservacionApoyo = null;
                seguimientoSemanalGestionObraSeguridadSaludOld.RegistroCompletoObservacionSupervisor = null;
            }

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
                // seguimientoSemanalGestionObraSeguridadSaludOld.RegistroCompleto = false;
                seguimientoSemanalGestionObraSeguridadSaludOld.TieneObservacionSupervisor = pSeguimientoSemanalObservacion.TieneObservacion;
                seguimientoSemanalGestionObraSeguridadSaludOld.ObservacionSupervisorId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                seguimientoSemanalGestionObraSeguridadSaludOld.RegistroCompletoObservacionSupervisor = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
            else
            {
                seguimientoSemanalGestionObraSeguridadSaludOld.TieneObservacionApoyo = pSeguimientoSemanalObservacion.TieneObservacion;
                seguimientoSemanalGestionObraSeguridadSaludOld.ObservacionApoyoId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                seguimientoSemanalGestionObraSeguridadSaludOld.RegistroCompletoObservacionApoyo = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
        }

        private void CreateOrEditObservacionComiteObra(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion, bool pEliminarRegistrCompleto)
        {
            SeguimientoSemanalRegistrarComiteObra SeguimientoSemanalRegistrarComiteObraOld = _context.SeguimientoSemanalRegistrarComiteObra.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);
            SeguimientoSemanalRegistrarComiteObraOld.FechaModificacion = DateTime.Now;
            SeguimientoSemanalRegistrarComiteObraOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pEliminarRegistrCompleto)
            {
                SeguimientoSemanalRegistrarComiteObraOld.RegistroCompletoObservacionApoyo = null;
                SeguimientoSemanalRegistrarComiteObraOld.RegistroCompletoObservacionSupervisor = null;
            }

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
                // SeguimientoSemanalRegistrarComiteObraOld.RegistroCompleto = false;
                SeguimientoSemanalRegistrarComiteObraOld.TieneObservacionSupervisor = pSeguimientoSemanalObservacion.TieneObservacion;
                SeguimientoSemanalRegistrarComiteObraOld.ObservacionSupervisorId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                SeguimientoSemanalRegistrarComiteObraOld.RegistroCompletoObservacionSupervisor = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
            else
            {
                SeguimientoSemanalRegistrarComiteObraOld.TieneObservacionApoyo = pSeguimientoSemanalObservacion.TieneObservacion;
                SeguimientoSemanalRegistrarComiteObraOld.ObservacionApoyoId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                SeguimientoSemanalRegistrarComiteObraOld.RegistroCompletoObservacionApoyo = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
        }

        private void CreateOrEditObservacionRegistroFotografico(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion, bool pEliminarRegistrCompleto)
        {
            SeguimientoSemanalRegistroFotografico SeguimientoSemanalRegistroFotograficoOld = _context.SeguimientoSemanalRegistroFotografico.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);
            SeguimientoSemanalRegistroFotograficoOld.FechaModificacion = DateTime.Now;
            SeguimientoSemanalRegistroFotograficoOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pEliminarRegistrCompleto)
            {
                SeguimientoSemanalRegistroFotograficoOld.RegistroCompletoObservacionApoyo = null;
                SeguimientoSemanalRegistroFotograficoOld.RegistroCompletoObservacionSupervisor = null;
            }

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
                // SeguimientoSemanalRegistroFotograficoOld.RegistroCompleto = false;

                SeguimientoSemanalRegistroFotograficoOld.TieneObservacionSupervisor = pSeguimientoSemanalObservacion.TieneObservacion;
                SeguimientoSemanalRegistroFotograficoOld.ObservacionSupervisorId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                SeguimientoSemanalRegistroFotograficoOld.RegistroCompletoObservacionSupervisor = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
            else
            {
                SeguimientoSemanalRegistroFotograficoOld.TieneObservacionApoyo = pSeguimientoSemanalObservacion.TieneObservacion;
                SeguimientoSemanalRegistroFotograficoOld.ObservacionApoyoId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                SeguimientoSemanalRegistroFotograficoOld.RegistroCompletoObservacionApoyo = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
        }

        private void CreateOrEditObservacionReporteActividadesRealizadasSiguienteSemana(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion, bool pEliminarRegistrCompleto)
        {
            SeguimientoSemanalReporteActividad seguimientoSemanalReporteActividad = _context.SeguimientoSemanalReporteActividad.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);

            seguimientoSemanalReporteActividad.FechaModificacion = DateTime.Now;
            seguimientoSemanalReporteActividad.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pEliminarRegistrCompleto)
            {
                seguimientoSemanalReporteActividad.RegistroCompletoObservacionApoyoActividad = null;
                seguimientoSemanalReporteActividad.RegistroCompletoObservacionApoyoActividadSiguiente = null;
                seguimientoSemanalReporteActividad.RegistroCompletoObservacionApoyoEstadoContrato = null;

                seguimientoSemanalReporteActividad.RegistroCompletoObservacionSupervisorActividad = null;
                seguimientoSemanalReporteActividad.RegistroCompletoObservacionSupervisorActividadSiguiente = null;
                seguimientoSemanalReporteActividad.RegistroCompletoObservacionSupervisorEstadoContrato = null;
            }


            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
                //seguimientoSemanalReporteActividad.RegistroCompleto = false;
                //seguimientoSemanalReporteActividad.RegistroCompletoActividadSiguiente = false;

                seguimientoSemanalReporteActividad.TieneObservacionSupervisorActividadSiguiente = pSeguimientoSemanalObservacion.TieneObservacion;
                seguimientoSemanalReporteActividad.ObservacionSupervisorIdActividadSiguiente = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                seguimientoSemanalReporteActividad.RegistroCompletoObservacionSupervisorActividadSiguiente = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
            else
            {
                seguimientoSemanalReporteActividad.TieneObservacionApoyoActividadSiguiente = pSeguimientoSemanalObservacion.TieneObservacion;
                seguimientoSemanalReporteActividad.ObservacionApoyoIdActividadSiguiente = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                seguimientoSemanalReporteActividad.RegistroCompletoObservacionApoyoActividadSiguiente = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
        }

        private void CreateOrEditObservacionReporteActividadesRealizadas(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion, bool pEliminarRegistrCompleto)
        {
            SeguimientoSemanalReporteActividad seguimientoSemanalReporteActividad = _context.SeguimientoSemanalReporteActividad.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);

            seguimientoSemanalReporteActividad.FechaModificacion = DateTime.Now;
            seguimientoSemanalReporteActividad.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pEliminarRegistrCompleto)
            {
                seguimientoSemanalReporteActividad.RegistroCompletoObservacionApoyoActividad = null;
                seguimientoSemanalReporteActividad.RegistroCompletoObservacionApoyoActividadSiguiente = null;
                seguimientoSemanalReporteActividad.RegistroCompletoObservacionApoyoEstadoContrato = null;

                seguimientoSemanalReporteActividad.RegistroCompletoObservacionSupervisorActividad = null;
                seguimientoSemanalReporteActividad.RegistroCompletoObservacionSupervisorActividadSiguiente = null;
                seguimientoSemanalReporteActividad.RegistroCompletoObservacionSupervisorEstadoContrato = null;
            }

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
                //  seguimientoSemanalReporteActividad.RegistroCompleto = false;
                //  seguimientoSemanalReporteActividad.RegistroCompletoActividad = false;

                seguimientoSemanalReporteActividad.TieneObservacionSupervisorActividad = pSeguimientoSemanalObservacion.TieneObservacion;
                seguimientoSemanalReporteActividad.ObservacionSupervisorIdActividad = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                seguimientoSemanalReporteActividad.RegistroCompletoObservacionSupervisorActividad = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
            else
            {
                seguimientoSemanalReporteActividad.TieneObservacionApoyoActividad = pSeguimientoSemanalObservacion.TieneObservacion;
                seguimientoSemanalReporteActividad.ObservacionApoyoIdActividad = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                seguimientoSemanalReporteActividad.RegistroCompletoObservacionApoyoActividad = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
        }

        private void CreateOrEditObservacionReporteActividadesEstadoContrato(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion, bool pEliminarRegistrCompleto)
        {
            SeguimientoSemanalReporteActividad seguimientoSemanalReporteActividad = _context.SeguimientoSemanalReporteActividad.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);

            seguimientoSemanalReporteActividad.FechaModificacion = DateTime.Now;
            seguimientoSemanalReporteActividad.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pEliminarRegistrCompleto)
            {
                seguimientoSemanalReporteActividad.RegistroCompletoObservacionApoyoActividad = null;
                seguimientoSemanalReporteActividad.RegistroCompletoObservacionApoyoActividadSiguiente = null;
                seguimientoSemanalReporteActividad.RegistroCompletoObservacionApoyoEstadoContrato = null;

                seguimientoSemanalReporteActividad.RegistroCompletoObservacionSupervisorActividad = null;
                seguimientoSemanalReporteActividad.RegistroCompletoObservacionSupervisorActividadSiguiente = null;
                seguimientoSemanalReporteActividad.RegistroCompletoObservacionSupervisorEstadoContrato = null;
            }
            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
                //  seguimientoSemanalReporteActividad.RegistroCompletoEstadoContrato = false;
                //    seguimientoSemanalReporteActividad.RegistroCompleto = false;

                seguimientoSemanalReporteActividad.TieneObservacionSupervisorEstadoContrato = pSeguimientoSemanalObservacion.TieneObservacion;
                seguimientoSemanalReporteActividad.ObservacionSupervisorIdEstadoContrato = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                seguimientoSemanalReporteActividad.RegistroCompletoObservacionSupervisorEstadoContrato = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
            else
            {
                seguimientoSemanalReporteActividad.TieneObservacionApoyoEstadoContrato = pSeguimientoSemanalObservacion.TieneObservacion;
                seguimientoSemanalReporteActividad.ObservacionApoyoIdEstadoContrato = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                seguimientoSemanalReporteActividad.RegistroCompletoObservacionApoyoEstadoContrato = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
        }

        private void CreateOrEditObservacionReporteActividades(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion, bool pEliminarRegistrCompleto)
        {
            SeguimientoSemanalReporteActividad seguimientoSemanalReporteActividad = _context.SeguimientoSemanalReporteActividad.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);

            seguimientoSemanalReporteActividad.FechaModificacion = DateTime.Now;
            seguimientoSemanalReporteActividad.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pEliminarRegistrCompleto)
            {
                seguimientoSemanalReporteActividad.RegistroCompletoObservacionApoyoActividad = null;
                seguimientoSemanalReporteActividad.RegistroCompletoObservacionApoyoActividadSiguiente = null;
                seguimientoSemanalReporteActividad.RegistroCompletoObservacionApoyoEstadoContrato = null;

                seguimientoSemanalReporteActividad.RegistroCompletoObservacionSupervisorActividad = null;
                seguimientoSemanalReporteActividad.RegistroCompletoObservacionSupervisorActividadSiguiente = null;
                seguimientoSemanalReporteActividad.RegistroCompletoObservacionSupervisorEstadoContrato = null;
            }

            //if (pSeguimientoSemanalObservacion.EsSupervisor)
            //{
            //    seguimientoSemanalReporteActividad.TieneObservacionSupervisor = pSeguimientoSemanalObservacion.TieneObservacion;
            //    seguimientoSemanalReporteActividad.ObservacionSupervisorId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
            //    seguimientoSemanalReporteActividad.RegistroCompletoObservacionSupervisor = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            //}
            //else
            //{
            //    seguimientoSemanalReporteActividad.TieneObservacionApoyo = pSeguimientoSemanalObservacion.TieneObservacion;
            //    seguimientoSemanalReporteActividad.ObservacionApoyoId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
            //    seguimientoSemanalReporteActividad.RegistroCompletoObservacionApoyo = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            //}
        }

        private void CreateOrEditObservacionGestionCalidadEnsayoLaboratorioMuestras(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion, bool pEliminarRegistrCompleto)
        {
            EnsayoLaboratorioMuestra ensayoLaboratorioMuestraOld = _context.EnsayoLaboratorioMuestra.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);
            ensayoLaboratorioMuestraOld.FechaModificacion = DateTime.Now;
            ensayoLaboratorioMuestraOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pEliminarRegistrCompleto)
            {
                ensayoLaboratorioMuestraOld.RegistroCompletoObservacionApoyo = null;
                ensayoLaboratorioMuestraOld.RegistroCompletoObservacionSupervisor = null;
            }

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
                //  ensayoLaboratorioMuestraOld.RegistroCompleto = false;
                ensayoLaboratorioMuestraOld.TieneObservacionSupervisor = pSeguimientoSemanalObservacion.TieneObservacion;
                ensayoLaboratorioMuestraOld.ObservacionSupervisorId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                ensayoLaboratorioMuestraOld.RegistroCompletoObservacionSupervisor = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
            else
            {
                ensayoLaboratorioMuestraOld.TieneObservacionApoyo = pSeguimientoSemanalObservacion.TieneObservacion;
                ensayoLaboratorioMuestraOld.ObservacionApoyoId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                ensayoLaboratorioMuestraOld.RegistroCompletoObservacionApoyo = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }

        }

        private void CreateOrEditObservacionGestionCalidadEnsayoLaboratorio(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion, bool pEliminarRegistrCompleto)
        {
            GestionObraCalidadEnsayoLaboratorio gestionObraCalidadEnsayoLaboratorioOld = _context.GestionObraCalidadEnsayoLaboratorio.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);

            gestionObraCalidadEnsayoLaboratorioOld.FechaModificacion = DateTime.Now;
            gestionObraCalidadEnsayoLaboratorioOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pEliminarRegistrCompleto)
            {
                gestionObraCalidadEnsayoLaboratorioOld.RegistroCompletoObservacionApoyo = null;
                gestionObraCalidadEnsayoLaboratorioOld.RegistroCompletoObservacionSupervisor = null;
            }

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
                // gestionObraCalidadEnsayoLaboratorioOld.RegistroCompleto = false;
                gestionObraCalidadEnsayoLaboratorioOld.TieneObservacionSupervisor = pSeguimientoSemanalObservacion.TieneObservacion;
                gestionObraCalidadEnsayoLaboratorioOld.ObservacionSupervisorId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                gestionObraCalidadEnsayoLaboratorioOld.RegistroCompletoObservacionSupervisor = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
            else
            {
                gestionObraCalidadEnsayoLaboratorioOld.TieneObservacionApoyo = pSeguimientoSemanalObservacion.TieneObservacion;
                gestionObraCalidadEnsayoLaboratorioOld.ObservacionApoyoId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                gestionObraCalidadEnsayoLaboratorioOld.RegistroCompletoObservacionApoyo = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
        }

        private void CreateOrEditObservacionGestionCalidad(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion, bool pEliminarRegistrCompleto)
        {
            SeguimientoSemanalGestionObraCalidad seguimientoSemanalGestionObraCalidad = _context.SeguimientoSemanalGestionObraCalidad.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);

            seguimientoSemanalGestionObraCalidad.FechaModificacion = DateTime.Now;
            seguimientoSemanalGestionObraCalidad.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pEliminarRegistrCompleto)
            {
                seguimientoSemanalGestionObraCalidad.RegistroCompletoObservacionApoyo = null;
                seguimientoSemanalGestionObraCalidad.RegistroCompletoObservacionSupervisor = null;
            }

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
                // seguimientoSemanalGestionObraCalidad.RegistroCompleto = false;
                seguimientoSemanalGestionObraCalidad.TieneObservacionSupervisor = pSeguimientoSemanalObservacion.TieneObservacion;
                seguimientoSemanalGestionObraCalidad.ObservacionSupervisorId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                seguimientoSemanalGestionObraCalidad.RegistroCompletoObservacionSupervisor = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
            else
            {
                seguimientoSemanalGestionObraCalidad.TieneObservacionApoyo = pSeguimientoSemanalObservacion.TieneObservacion;
                seguimientoSemanalGestionObraCalidad.ObservacionApoyoId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                seguimientoSemanalGestionObraCalidad.RegistroCompletoObservacionApoyo = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
        }

        private void CreateOrEditObservacionGestionObraAmbientalManejoOtra(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion, bool pEliminarRegistrCompleto)
        {
            ManejoOtro manejoOtroOld = _context.ManejoOtro.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);

            manejoOtroOld.FechaModificacion = DateTime.Now;
            manejoOtroOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pEliminarRegistrCompleto)
            {
                manejoOtroOld.RegistroCompletoObservacionApoyo = null;
                manejoOtroOld.RegistroCompletoObservacionSupervisor = null;
            }

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
                //manejoOtroOld.RegistroCompleto = false;
                manejoOtroOld.TieneObservacionSupervisor = pSeguimientoSemanalObservacion.TieneObservacion;
                manejoOtroOld.ObservacionSupervisorId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                manejoOtroOld.RegistroCompletoObservacionSupervisor = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
            else
            {
                manejoOtroOld.TieneObservacionApoyo = pSeguimientoSemanalObservacion.TieneObservacion;
                manejoOtroOld.ObservacionApoyoId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                manejoOtroOld.RegistroCompletoObservacionApoyo = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
        }

        private void CreateOrEditObservacionGestionObraAmbientalManejoResiduosPeligrosos(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion, bool pEliminarRegistrCompleto)
        {
            ManejoResiduosPeligrososEspeciales manejoResiduosPeligrososEspecialesOld = _context.ManejoResiduosPeligrososEspeciales.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);

            manejoResiduosPeligrososEspecialesOld.FechaModificacion = DateTime.Now;
            manejoResiduosPeligrososEspecialesOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pEliminarRegistrCompleto)
            {
                manejoResiduosPeligrososEspecialesOld.RegistroCompletoObservacionApoyo = null;
                manejoResiduosPeligrososEspecialesOld.RegistroCompletoObservacionSupervisor = null;
            }

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
                //manejoResiduosPeligrososEspecialesOld.RegistroCompleto = false;
                manejoResiduosPeligrososEspecialesOld.TieneObservacionSupervisor = pSeguimientoSemanalObservacion.TieneObservacion;
                manejoResiduosPeligrososEspecialesOld.ObservacionSupervisorId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                manejoResiduosPeligrososEspecialesOld.RegistroCompletoObservacionSupervisor = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
            else
            {
                manejoResiduosPeligrososEspecialesOld.TieneObservacionApoyo = pSeguimientoSemanalObservacion.TieneObservacion;
                manejoResiduosPeligrososEspecialesOld.ObservacionApoyoId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                manejoResiduosPeligrososEspecialesOld.RegistroCompletoObservacionApoyo = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
        }

        private void CreateOrEditObservacionGestionObraAmbientalManejoConstruccionDemolicion(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion, bool pEliminarRegistrCompleto)
        {
            ManejoResiduosConstruccionDemolicion manejoResiduosConstruccionDemolicionOld = _context.ManejoResiduosConstruccionDemolicion.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);

            manejoResiduosConstruccionDemolicionOld.FechaModificacion = DateTime.Now;
            manejoResiduosConstruccionDemolicionOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pEliminarRegistrCompleto)
            {
                manejoResiduosConstruccionDemolicionOld.RegistroCompletoObservacionApoyo = null;
                manejoResiduosConstruccionDemolicionOld.RegistroCompletoObservacionSupervisor = null;
            }


            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
                //  manejoResiduosConstruccionDemolicionOld.RegistroCompleto = false;
                manejoResiduosConstruccionDemolicionOld.TieneObservacionSupervisor = pSeguimientoSemanalObservacion.TieneObservacion;
                manejoResiduosConstruccionDemolicionOld.ObservacionSupervisorId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                manejoResiduosConstruccionDemolicionOld.RegistroCompletoObservacionSupervisor = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
            else
            {
                manejoResiduosConstruccionDemolicionOld.TieneObservacionApoyo = pSeguimientoSemanalObservacion.TieneObservacion;
                manejoResiduosConstruccionDemolicionOld.ObservacionApoyoId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                manejoResiduosConstruccionDemolicionOld.RegistroCompletoObservacionApoyo = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }

        }

        private void CreateOrEditObservacionGestionObraAmbientalManejoMateriales(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion, bool pEliminarRegistrCompleto)
        {
            ManejoMaterialesInsumos manejoMaterialesInsumosOld = _context.ManejoMaterialesInsumos.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);

            manejoMaterialesInsumosOld.FechaModificacion = DateTime.Now;
            manejoMaterialesInsumosOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pEliminarRegistrCompleto)
            {
                manejoMaterialesInsumosOld.RegistroCompletoObservacionApoyo = null;
                manejoMaterialesInsumosOld.RegistroCompletoObservacionSupervisor = null;
            }

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
                // manejoMaterialesInsumosOld.RegistroCompleto = false;
                manejoMaterialesInsumosOld.TieneObservacionSupervisor = pSeguimientoSemanalObservacion.TieneObservacion;
                manejoMaterialesInsumosOld.ObservacionSupervisorId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                manejoMaterialesInsumosOld.RegistroCompletoObservacionSupervisor = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
            else
            {
                manejoMaterialesInsumosOld.TieneObservacionApoyo = pSeguimientoSemanalObservacion.TieneObservacion;
                manejoMaterialesInsumosOld.ObservacionApoyoId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                manejoMaterialesInsumosOld.RegistroCompletoObservacionApoyo = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
        }

        private bool CompleteRecordObservation(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion)
        {
            if (pSeguimientoSemanalObservacion.TieneObservacion == false)
                return true;

            if (pSeguimientoSemanalObservacion.TieneObservacion == true && string.IsNullOrEmpty(pSeguimientoSemanalObservacion.Observacion))
                return false;

            if (pSeguimientoSemanalObservacion.TieneObservacion == true && !string.IsNullOrEmpty(Helpers.Helpers.HtmlConvertirTextoPlano(Helpers.Helpers.HtmlConvertirTextoPlano(pSeguimientoSemanalObservacion.Observacion))))
                return true;

            return false;
        }

        private void CreateOrEditObservacionGestionObraAmbiental(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion, bool pEliminarRegistrCompleto)
        {
            SeguimientoSemanalGestionObraAmbiental seguimientoSemanalGestionObraAmbientalOld = _context.SeguimientoSemanalGestionObraAmbiental.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);


            seguimientoSemanalGestionObraAmbientalOld.FechaModificacion = DateTime.Now;
            seguimientoSemanalGestionObraAmbientalOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pEliminarRegistrCompleto)
            {
                seguimientoSemanalGestionObraAmbientalOld.RegistroCompletoObservacionApoyo = false;
                seguimientoSemanalGestionObraAmbientalOld.RegistroCompletoObservacionSupervisor = false;
            }

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
                // seguimientoSemanalGestionObraAmbientalOld.RegistroCompleto = false;

                seguimientoSemanalGestionObraAmbientalOld.TieneObservacionSupervisor = pSeguimientoSemanalObservacion.TieneObservacion;
                seguimientoSemanalGestionObraAmbientalOld.ObservacionSupervisorId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                seguimientoSemanalGestionObraAmbientalOld.RegistroCompletoObservacionSupervisor = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
            else
            {
                seguimientoSemanalGestionObraAmbientalOld.TieneObservacionApoyo = pSeguimientoSemanalObservacion.TieneObservacion;
                seguimientoSemanalGestionObraAmbientalOld.ObservacionApoyoId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                seguimientoSemanalGestionObraAmbientalOld.RegistroCompletoObservacionApoyo = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
        }

        private void CreateOrEditObservacionAvanceFinanciero(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion, bool pEliminarRegistrCompleto)
        {
            SeguimientoSemanalAvanceFinanciero seguimientoSemanalAvanceFinancieroOld = _context.SeguimientoSemanalAvanceFinanciero.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);

            seguimientoSemanalAvanceFinancieroOld.FechaModificacion = DateTime.Now;
            seguimientoSemanalAvanceFinancieroOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pEliminarRegistrCompleto)
            {
                seguimientoSemanalAvanceFinancieroOld.RegistroCompletoObservacionApoyo = false;
                seguimientoSemanalAvanceFinancieroOld.RegistroCompletoObservacionSupervisor = false;
            }

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
                //seguimientoSemanalAvanceFinancieroOld.RegistroCompleto = false; 
                seguimientoSemanalAvanceFinancieroOld.TieneObservacionSupervisor = pSeguimientoSemanalObservacion.TieneObservacion;
                seguimientoSemanalAvanceFinancieroOld.ObservacionSupervisorId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                seguimientoSemanalAvanceFinancieroOld.RegistroCompletoObservacionSupervisor = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
            else
            {
                seguimientoSemanalAvanceFinancieroOld.TieneObservacionApoyo = pSeguimientoSemanalObservacion.TieneObservacion;
                seguimientoSemanalAvanceFinancieroOld.ObservacionApoyoId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                seguimientoSemanalAvanceFinancieroOld.RegistroCompletoObservacionApoyo = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
        }

        private void CreateOrEditObservacionAvanceFisico(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion, bool pEliminarRegistrCompleto)
        {
            SeguimientoSemanalAvanceFisico seguimientoSemanalAvanceFisicoOld = _context.SeguimientoSemanalAvanceFisico.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);

            seguimientoSemanalAvanceFisicoOld.FechaModificacion = DateTime.Now;
            seguimientoSemanalAvanceFisicoOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pEliminarRegistrCompleto)
            {
                seguimientoSemanalAvanceFisicoOld.RegistroCompletoObservacionApoyo = false;
                seguimientoSemanalAvanceFisicoOld.RegistroCompletoObservacionSupervisor = false;
            }

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
                // seguimientoSemanalAvanceFisicoOld.RegistroCompleto = false;
                seguimientoSemanalAvanceFisicoOld.TieneObservacionSupervisor = pSeguimientoSemanalObservacion.TieneObservacion;
                seguimientoSemanalAvanceFisicoOld.ObservacionSupervisorId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                seguimientoSemanalAvanceFisicoOld.RegistroCompletoObservacionSupervisor = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
            else
            {
                seguimientoSemanalAvanceFisicoOld.ObservacionSupervisorId = null;
                seguimientoSemanalAvanceFisicoOld.TieneObservacionApoyo = pSeguimientoSemanalObservacion.TieneObservacion;
                seguimientoSemanalAvanceFisicoOld.ObservacionApoyoId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                seguimientoSemanalAvanceFisicoOld.RegistroCompletoObservacionApoyo = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
        }

        private void GetArchivarObservacionesSeguimientoSemanal(int seguimientoSemanalId, string pAuthor)
        {
            _context.Set<SeguimientoSemanalObservacion>()
                    .Where(s => s.SeguimientoSemanalId == seguimientoSemanalId)
                    .Update(s => new SeguimientoSemanalObservacion
                    {
                        UsuarioModificacion = pAuthor,
                        Archivada = true,
                        FechaModificacion = DateTime.Now
                    });
        }

        public async Task<Respuesta> ChangueStatusMuestrasSeguimientoSemanal(int pSeguimientoSemanalID, string pEstadoMod, string pUsuarioMod)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Muestras_Seguimiento_Semanal, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                SeguimientoSemanal seguimientoSemanalMod = _context.SeguimientoSemanal.Find(pSeguimientoSemanalID);

                seguimientoSemanalMod.EstadoMuestrasCodigo = pEstadoMod;
                seguimientoSemanalMod.UsuarioModificacion = pUsuarioMod;
                seguimientoSemanalMod.FechaModificacion = DateTime.Now;

                if (pEstadoMod == ConstanCodigoEstadoSeguimientoSemanal.Enviado_Verificacion)
                {
                    seguimientoSemanalMod.RegistroCompleto = true;
                }

                _context.SaveChanges();

                string strNombreSEstadoObraCodigo = _context.Dominio.Where(r => r.Codigo == pEstadoMod && r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Reporte_Semanal_Y_Muestras).FirstOrDefault().Nombre;

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstanMessagesRegisterWeeklyProgress.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, ConstanMessagesRegisterWeeklyProgress.OperacionExitosa, idAccion, pUsuarioMod, "EL ESTADO MUESTRAS DE LABORATORIO CAMBIO A: " + strNombreSEstadoObraCodigo.ToUpper())
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstanMessagesRegisterWeeklyProgress.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, ConstanMessagesRegisterWeeklyProgress.Error, idAccion, pUsuarioMod, ex.InnerException.ToString())
                };
            }
        }

        public async Task<Respuesta> CreateEditEnsayoLaboratorioMuestra(GestionObraCalidadEnsayoLaboratorio pGestionObraCalidadEnsayoLaboratorio)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Ensayo_Laboratorio_Muestra, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                bool RegistroCompletoMuestras = true;

                foreach (var EnsayoLaboratorioMuestra in pGestionObraCalidadEnsayoLaboratorio.EnsayoLaboratorioMuestra)
                {
                    if (EnsayoLaboratorioMuestra.EnsayoLaboratorioMuestraId == 0)
                    {
                        EnsayoLaboratorioMuestra.UsuarioCreacion = pGestionObraCalidadEnsayoLaboratorio.UsuarioCreacion;
                        EnsayoLaboratorioMuestra.FechaCreacion = DateTime.Now;
                        EnsayoLaboratorioMuestra.Eliminado = false;
                        EnsayoLaboratorioMuestra.RegistroCompleto =
                               !string.IsNullOrEmpty(EnsayoLaboratorioMuestra.NombreMuestra)
                            && !string.IsNullOrEmpty(EnsayoLaboratorioMuestra.Observacion)
                            && EnsayoLaboratorioMuestra.FechaEntregaResultado.HasValue
                            ? true : false;
                        _context.EnsayoLaboratorioMuestra.Add(EnsayoLaboratorioMuestra);

                        if (EnsayoLaboratorioMuestra.RegistroCompleto == false)
                            RegistroCompletoMuestras = false;
                    }
                    else
                    {
                        EnsayoLaboratorioMuestra EnsayoLaboratorioMuestraOld = _context.EnsayoLaboratorioMuestra.Find(EnsayoLaboratorioMuestra.EnsayoLaboratorioMuestraId);
                        EnsayoLaboratorioMuestraOld.FechaEntregaResultado = EnsayoLaboratorioMuestra.FechaEntregaResultado;
                        EnsayoLaboratorioMuestraOld.NombreMuestra = EnsayoLaboratorioMuestra.NombreMuestra;
                        EnsayoLaboratorioMuestraOld.Observacion = EnsayoLaboratorioMuestra.Observacion;
                        EnsayoLaboratorioMuestraOld.UsuarioModificacion = pGestionObraCalidadEnsayoLaboratorio.UsuarioCreacion;
                        EnsayoLaboratorioMuestraOld.FechaModificacion = DateTime.Now;
                        EnsayoLaboratorioMuestraOld.RegistroCompleto =
                               !string.IsNullOrEmpty(EnsayoLaboratorioMuestra.NombreMuestra)
                            && !string.IsNullOrEmpty(EnsayoLaboratorioMuestra.Observacion)
                            && EnsayoLaboratorioMuestra.FechaEntregaResultado.HasValue
                            ? true : false;
                        if (EnsayoLaboratorioMuestraOld.RegistroCompleto == false)
                            RegistroCompletoMuestras = false;
                    }
                }


                //Actualizar estado ensayo laboratorio
                GestionObraCalidadEnsayoLaboratorio gestionObraCalidadEnsayoLaboratorioOld =
                    _context.GestionObraCalidadEnsayoLaboratorio
                    .Where(r => r.GestionObraCalidadEnsayoLaboratorioId == pGestionObraCalidadEnsayoLaboratorio.GestionObraCalidadEnsayoLaboratorioId)
                    .Include(r => r.SeguimientoSemanalGestionObraCalidad)
                        .ThenInclude(r => r.SeguimientoSemanalGestionObra)
                        .FirstOrDefault();

                gestionObraCalidadEnsayoLaboratorioOld.RegistroCompletoMuestras = RegistroCompletoMuestras;
                gestionObraCalidadEnsayoLaboratorioOld.UsuarioModificacion = pGestionObraCalidadEnsayoLaboratorio.UsuarioCreacion;
                gestionObraCalidadEnsayoLaboratorioOld.FechaModificacion = DateTime.Now;


                SeguimientoSemanal seguimientoSemanalOld = _context.SeguimientoSemanal.Find(gestionObraCalidadEnsayoLaboratorioOld.SeguimientoSemanalGestionObraCalidad.SeguimientoSemanalGestionObra.SeguimientoSemanalId);

                seguimientoSemanalOld.FechaModificacion = DateTime.Now;
                seguimientoSemanalOld.UsuarioModificacion = pGestionObraCalidadEnsayoLaboratorio.UsuarioCreacion;
                seguimientoSemanalOld.RegistroCompletoMuestras = RegistroCompletoMuestras;
                if (RegistroCompletoMuestras)
                    seguimientoSemanalOld.EstadoMuestrasCodigo = ConstanCodigoEstadoSeguimientoSemanal.Validado_Supervisor;


                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstanMessagesRegisterWeeklyProgress.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, ConstanMessagesRegisterWeeklyProgress.OperacionExitosa, idAccion, pGestionObraCalidadEnsayoLaboratorio.UsuarioModificacion, pGestionObraCalidadEnsayoLaboratorio.FechaModificacion.HasValue ? "EDITAR ENSAYO LABORATORIO MUESTRA" : "CREAR ENSAYO LABORATORIO MUESTRA")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstanMessagesRegisterWeeklyProgress.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, ConstanMessagesRegisterWeeklyProgress.Error, idAccion, pGestionObraCalidadEnsayoLaboratorio.UsuarioModificacion, ex.InnerException.ToString())
                };
            }
        }

        private void SaveUpdateAvanceFisico(SeguimientoSemanal pSeguimientoSemanal, string usuarioCreacion)
        {
            bool RegistroCompleto = ValidarRegistroCompletoAvanceFisico(pSeguimientoSemanal.SeguimientoSemanalAvanceFisico.FirstOrDefault());

            if (pSeguimientoSemanal.SeguimientoSemanalAvanceFisico.FirstOrDefault().SeguimientoSemanalAvanceFisicoId == 0)
            {
                pSeguimientoSemanal.SeguimientoSemanalAvanceFisico.FirstOrDefault().RegistroCompleto = RegistroCompleto;
                pSeguimientoSemanal.SeguimientoSemanalAvanceFisico.FirstOrDefault().UsuarioCreacion = usuarioCreacion;
                pSeguimientoSemanal.SeguimientoSemanalAvanceFisico.FirstOrDefault().FechaCreacion = DateTime.Now;
                pSeguimientoSemanal.SeguimientoSemanalAvanceFisico.FirstOrDefault().Eliminado = false;
                pSeguimientoSemanal.SeguimientoSemanalAvanceFisico.FirstOrDefault().EstadoObraCodigo = ValidarEstadoDeObraBySeguimientoSemanalId(pSeguimientoSemanal.SeguimientoSemanalId);

                _context.SeguimientoSemanalAvanceFisico.Add(pSeguimientoSemanal.SeguimientoSemanalAvanceFisico.FirstOrDefault());

                CrearEditarSeguimientoSemanalAvanceFisicoProgramacion(pSeguimientoSemanal.SeguimientoSemanalAvanceFisico.FirstOrDefault().SeguimientoSemanalAvanceFisicoProgramacion, pSeguimientoSemanal.SeguimientoSemanalAvanceFisico.FirstOrDefault().UsuarioCreacion);

            }
            else
            {
                SeguimientoSemanalAvanceFisico seguimientoSemanalAvanceFisicoOld = _context.SeguimientoSemanalAvanceFisico.Find(pSeguimientoSemanal.SeguimientoSemanalAvanceFisico.FirstOrDefault().SeguimientoSemanalAvanceFisicoId);

                seguimientoSemanalAvanceFisicoOld.RegistroCompleto = RegistroCompleto;
                seguimientoSemanalAvanceFisicoOld.UsuarioModificacion = usuarioCreacion;
                seguimientoSemanalAvanceFisicoOld.FechaModificacion = DateTime.Now;
                seguimientoSemanalAvanceFisicoOld.EstadoObraCodigo = ValidarEstadoDeObraBySeguimientoSemanalId(pSeguimientoSemanal.SeguimientoSemanalId);
                seguimientoSemanalAvanceFisicoOld.ProgramacionSemanal = pSeguimientoSemanal.SeguimientoSemanalAvanceFisico.FirstOrDefault().ProgramacionSemanal;
                seguimientoSemanalAvanceFisicoOld.AvanceFisicoSemanal = pSeguimientoSemanal.SeguimientoSemanalAvanceFisico.FirstOrDefault().AvanceFisicoSemanal;

                CrearEditarSeguimientoSemanalAvanceFisicoProgramacion(pSeguimientoSemanal.SeguimientoSemanalAvanceFisico.FirstOrDefault().SeguimientoSemanalAvanceFisicoProgramacion, pSeguimientoSemanal.SeguimientoSemanalAvanceFisico.FirstOrDefault().UsuarioCreacion);

            }
        }

        private void CrearEditarSeguimientoSemanalAvanceFisicoProgramacion(ICollection<SeguimientoSemanalAvanceFisicoProgramacion> List, string strUsuario)
        {
            foreach (var item in List)
            {
                if (item.SeguimientoSemanalAvanceFisicoProgramacionId == 0)
                {

                    item.UsuarioCreacion = strUsuario;
                    item.FechaCreacion = DateTime.Now;
                    item.Eliminado = false;
                    _context.SeguimientoSemanalAvanceFisicoProgramacion.Add(item);
                }
                else
                {
                    SeguimientoSemanalAvanceFisicoProgramacion itemOld = _context.SeguimientoSemanalAvanceFisicoProgramacion
                                                                         .Find(item.SeguimientoSemanalAvanceFisicoProgramacionId);

                    itemOld.UsuarioModificacion = strUsuario;
                    itemOld.FechaModificacion = DateTime.Now;
                    itemOld.AvanceFisicoCapitulo = item.AvanceFisicoCapitulo;
                    itemOld.ProgramacionCapitulo = item.ProgramacionCapitulo;
                }
            }
        }

        private void SaveUpdateAvanceFinanciero(SeguimientoSemanalAvanceFinanciero pSeguimientoSemanalAvanceFinanciero, string pUsuarioCreacion)
        {
            pSeguimientoSemanalAvanceFinanciero.RegistroCompleto =
                       !pSeguimientoSemanalAvanceFinanciero.RequiereObservacion.HasValue
                    || !pSeguimientoSemanalAvanceFinanciero.GenerarAlerta.HasValue
                    || (pSeguimientoSemanalAvanceFinanciero.RequiereObservacion.HasValue
                    && (bool)pSeguimientoSemanalAvanceFinanciero.RequiereObservacion && string.IsNullOrEmpty(pSeguimientoSemanalAvanceFinanciero.Observacion))
                    ? false : true;

            if (pSeguimientoSemanalAvanceFinanciero.SeguimientoSemanalAvanceFinancieroId == 0)
            {
                pSeguimientoSemanalAvanceFinanciero.UsuarioCreacion = pUsuarioCreacion;
                pSeguimientoSemanalAvanceFinanciero.FechaCreacion = DateTime.Now;
                pSeguimientoSemanalAvanceFinanciero.Eliminado = false;

                _context.SeguimientoSemanalAvanceFinanciero.Add(pSeguimientoSemanalAvanceFinanciero);
            }
            else
            {
                SeguimientoSemanalAvanceFinanciero seguimientoSemanalAvanceFinancieroOld = _context.SeguimientoSemanalAvanceFinanciero.Find(pSeguimientoSemanalAvanceFinanciero.SeguimientoSemanalAvanceFinancieroId);

                seguimientoSemanalAvanceFinancieroOld.UsuarioModificacion = pUsuarioCreacion;
                seguimientoSemanalAvanceFinancieroOld.FechaModificacion = DateTime.Now;
                seguimientoSemanalAvanceFinancieroOld.RegistroCompleto = pSeguimientoSemanalAvanceFinanciero.RegistroCompleto;

                seguimientoSemanalAvanceFinancieroOld.RequiereObservacion = pSeguimientoSemanalAvanceFinanciero.RequiereObservacion;
                seguimientoSemanalAvanceFinancieroOld.Observacion = pSeguimientoSemanalAvanceFinanciero.Observacion;
                seguimientoSemanalAvanceFinancieroOld.GenerarAlerta = pSeguimientoSemanalAvanceFinanciero.GenerarAlerta;

            }
        }

        private void SaveUpdateGestionObra(SeguimientoSemanalGestionObra pSeguimientoSemanalGestionObra, string pUsuarioCreacion)
        {
            //New
            if (pSeguimientoSemanalGestionObra.SeguimientoSemanalGestionObraId == 0)
            {
                //Auditoria
                pSeguimientoSemanalGestionObra.UsuarioCreacion = pUsuarioCreacion;
                pSeguimientoSemanalGestionObra.Eliminado = false;
                pSeguimientoSemanalGestionObra.FechaCreacion = DateTime.Now;

                //Gestion Ambiental
                foreach (var SeguimientoSemanalGestionObraAmbiental in pSeguimientoSemanalGestionObra.SeguimientoSemanalGestionObraAmbiental)
                {
                    try
                    {
                        SeguimientoSemanalGestionObraAmbiental.UsuarioCreacion = pUsuarioCreacion;
                        SeguimientoSemanalGestionObraAmbiental.Eliminado = false;
                        SeguimientoSemanalGestionObraAmbiental.FechaCreacion = DateTime.Now;
                        SeguimientoSemanalGestionObraAmbiental.RegistroCompleto = ValidarRegistroCompletoSeguimientoSemanalGestionObraAmbiental(SeguimientoSemanalGestionObraAmbiental);

                        _context.SeguimientoSemanalGestionObraAmbiental.Add(SeguimientoSemanalGestionObraAmbiental);

                    }
                    catch (Exception e)
                    {

                        throw;
                    }
                    //Manejo Materiales e Insumos
                    if (SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo != null)
                    {
                        SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.UsuarioCreacion = pUsuarioCreacion;
                        SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.Eliminado = false;
                        SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.FechaCreacion = DateTime.Now;
                        SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.RegistroCompleto = ValidarRegistroCompletoManejoMaterialesInsumo(SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo);

                        _context.ManejoMaterialesInsumos.Add(SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo);

                        foreach (var ManejoMaterialesInsumosProveedor in SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.ManejoMaterialesInsumosProveedor)
                        {
                            ManejoMaterialesInsumosProveedor.UsuarioCreacion = pUsuarioCreacion;
                            ManejoMaterialesInsumosProveedor.Eliminado = false;
                            ManejoMaterialesInsumosProveedor.FechaCreacion = DateTime.Now;
                            ManejoMaterialesInsumosProveedor.RegistroCompleto = ValidarRegistroCompletoManejoMaterialesInsumosProveedor(ManejoMaterialesInsumosProveedor);

                            _context.ManejoMaterialesInsumosProveedor.Add(ManejoMaterialesInsumosProveedor);
                        }
                    }

                    //Manejo Residuos Construccion Demolicion
                    if (SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion != null)
                    {
                        SeguimientoSemanalGestionObraAmbiental.UsuarioCreacion = pUsuarioCreacion;
                        SeguimientoSemanalGestionObraAmbiental.Eliminado = false;
                        SeguimientoSemanalGestionObraAmbiental.FechaCreacion = DateTime.Now;
                        SeguimientoSemanalGestionObraAmbiental.RegistroCompleto = ValidarRegistroCompletoManejoResiduosConstruccionDemolicion(SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion);
                             
                        _context.ManejoResiduosConstruccionDemolicion.Add(SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion);
       
                        foreach (var ManejoResiduosConstruccionDemolicionGestor in SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.ManejoResiduosConstruccionDemolicionGestor)
                        { 
                            ManejoResiduosConstruccionDemolicionGestor.UsuarioCreacion = pUsuarioCreacion;
                            ManejoResiduosConstruccionDemolicionGestor.Eliminado = false;
                            ManejoResiduosConstruccionDemolicionGestor.FechaCreacion = DateTime.Now;
                            ManejoResiduosConstruccionDemolicionGestor.RegistroCompleto = ValidarRegistroCompletoManejoResiduosConstruccionDemolicionGestor(ManejoResiduosConstruccionDemolicionGestor);

                            _context.ManejoResiduosConstruccionDemolicionGestor.Add(ManejoResiduosConstruccionDemolicionGestor);
                        }
                    }

                    //Manejo Residuo Peligrosos Especiales
                    if (SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales != null)
                    {

                        SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.UsuarioCreacion = pUsuarioCreacion;
                        SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.Eliminado = false;
                        SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.FechaCreacion = DateTime.Now;
                        SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.RegistroCompleto = ValidarRegistroCompletoManejoResiduosPeligrososEspeciales(SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales);

                        _context.ManejoResiduosPeligrososEspeciales.Add(SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales);

                    }

                    //Manejo Otro
                    if (SeguimientoSemanalGestionObraAmbiental.ManejoOtro != null)
                    {
                        SeguimientoSemanalGestionObraAmbiental.ManejoOtro.UsuarioCreacion = pUsuarioCreacion;
                        SeguimientoSemanalGestionObraAmbiental.ManejoOtro.Eliminado = false;
                        SeguimientoSemanalGestionObraAmbiental.ManejoOtro.FechaCreacion = DateTime.Now;
                        SeguimientoSemanalGestionObraAmbiental.ManejoOtro.RegistroCompleto = ValidarRegistroCompletoManejoOtro(SeguimientoSemanalGestionObraAmbiental.ManejoOtro);

                        _context.ManejoOtro.Add(SeguimientoSemanalGestionObraAmbiental.ManejoOtro);
                    }


                    SeguimientoSemanalGestionObraAmbiental.RegistroCompleto = ValidarRegistroCompletoSeguimientoSemanalGestionObraAmbiental(SeguimientoSemanalGestionObraAmbiental);
                }

                //Gestion Calidad
                foreach (var SeguimientoSemanalGestionObraCalidad in pSeguimientoSemanalGestionObra.SeguimientoSemanalGestionObraCalidad)
                {
                    SeguimientoSemanalGestionObraCalidad.UsuarioCreacion = pUsuarioCreacion;
                    SeguimientoSemanalGestionObraCalidad.Eliminado = false;
                    SeguimientoSemanalGestionObraCalidad.FechaCreacion = DateTime.Now;
                    SeguimientoSemanalGestionObraCalidad.RegistroCompleto = ValidarRegistroCompletoSeguimientoSemanalGestionObraCalidad(SeguimientoSemanalGestionObraCalidad);

                    _context.SeguimientoSemanalGestionObraCalidad.Add(SeguimientoSemanalGestionObraCalidad);

                    //Ensayo Laboratorio
                    foreach (var GestionObraCalidadEnsayoLaboratorio in SeguimientoSemanalGestionObraCalidad.GestionObraCalidadEnsayoLaboratorio)
                    {
                        GestionObraCalidadEnsayoLaboratorio.UsuarioCreacion = pUsuarioCreacion;
                        GestionObraCalidadEnsayoLaboratorio.Eliminado = false;
                        GestionObraCalidadEnsayoLaboratorio.FechaCreacion = DateTime.Now;
                        GestionObraCalidadEnsayoLaboratorio.RegistroCompleto = ValidarRegistroCompletoGestionObraCalidadEnsayoLaboratorio(GestionObraCalidadEnsayoLaboratorio);

                        _context.GestionObraCalidadEnsayoLaboratorio.Add(GestionObraCalidadEnsayoLaboratorio);

                        foreach (var EnsayoLaboratorioMuestra in GestionObraCalidadEnsayoLaboratorio.EnsayoLaboratorioMuestra)
                        {
                            EnsayoLaboratorioMuestra.UsuarioCreacion = pUsuarioCreacion;
                            EnsayoLaboratorioMuestra.Eliminado = false;
                            EnsayoLaboratorioMuestra.FechaCreacion = DateTime.Now;
                            //  EnsayoLaboratorioMuestra.RegistroCompleto = ValidarRegistroCompletoGestionEnsayoLaboratorioMuestra(EnsayoLaboratorioMuestra);

                            _context.EnsayoLaboratorioMuestra.Add(EnsayoLaboratorioMuestra);
                        }
                    }
                }

                //Gestion Seguridad Salud
                foreach (var SeguimientoSemanalGestionObraSeguridadSalud in pSeguimientoSemanalGestionObra.SeguimientoSemanalGestionObraSeguridadSalud)
                {
                    SeguimientoSemanalGestionObraSeguridadSalud.UsuarioCreacion = pUsuarioCreacion;
                    SeguimientoSemanalGestionObraSeguridadSalud.Eliminado = false;
                    SeguimientoSemanalGestionObraSeguridadSalud.FechaCreacion = DateTime.Now;
                    SeguimientoSemanalGestionObraSeguridadSalud.RegistroCompleto = ValidarRegistroCompletoSeguimientoSemanalGestionObraSeguridadSalud(SeguimientoSemanalGestionObraSeguridadSalud);

                    _context.SeguimientoSemanalGestionObraSeguridadSalud.Add(SeguimientoSemanalGestionObraSeguridadSalud);

                    foreach (var SeguridadSaludCausaAccidente in SeguimientoSemanalGestionObraSeguridadSalud.SeguridadSaludCausaAccidente)
                    {
                        SeguridadSaludCausaAccidente.UsuarioCreacion = pUsuarioCreacion;
                        SeguridadSaludCausaAccidente.Eliminado = false;
                        SeguridadSaludCausaAccidente.FechaCreacion = DateTime.Now;

                        _context.SeguridadSaludCausaAccidente.Add(SeguridadSaludCausaAccidente);

                    }
                }

                //Gestion Obra Social
                foreach (var SeguimientoSemanalGestionObraSocial in pSeguimientoSemanalGestionObra.SeguimientoSemanalGestionObraSocial)
                {
                    SeguimientoSemanalGestionObraSocial.UsuarioCreacion = pUsuarioCreacion;
                    SeguimientoSemanalGestionObraSocial.Eliminado = false;
                    SeguimientoSemanalGestionObraSocial.FechaCreacion = DateTime.Now;
                    SeguimientoSemanalGestionObraSocial.RegistroCompleto = ValidarRegistroCompletoSeguimientoSemanalGestionObraSocial(SeguimientoSemanalGestionObraSocial);

                    _context.SeguimientoSemanalGestionObraSocial.Add(SeguimientoSemanalGestionObraSocial);
                }

                //Gestion Alerta
                foreach (var SeguimientoSemanalGestionObraAlerta in pSeguimientoSemanalGestionObra.SeguimientoSemanalGestionObraAlerta)
                {
                    SeguimientoSemanalGestionObraAlerta.UsuarioCreacion = pUsuarioCreacion;
                    SeguimientoSemanalGestionObraAlerta.Eliminado = false;
                    SeguimientoSemanalGestionObraAlerta.FechaCreacion = DateTime.Now;
                    SeguimientoSemanalGestionObraAlerta.RegistroCompleto = ValidarRegistroCompletoSeguimientoSemanalGestionObraAlerta(SeguimientoSemanalGestionObraAlerta);

                    _context.SeguimientoSemanalGestionObraAlerta.Add(SeguimientoSemanalGestionObraAlerta);
                }

                pSeguimientoSemanalGestionObra.RegistroCompleto = ValidarRegistroCompletoSeguimientoSemanalGestionObra(pSeguimientoSemanalGestionObra);
                _context.SeguimientoSemanalGestionObra.Add(pSeguimientoSemanalGestionObra);
            }
            //Update
            else
            {
                SeguimientoSemanalGestionObra seguimientoSemanalGestionObraOld = _context.SeguimientoSemanalGestionObra.Find(pSeguimientoSemanalGestionObra.SeguimientoSemanalGestionObraId);
                //Auditoria
                seguimientoSemanalGestionObraOld.UsuarioModificacion = pUsuarioCreacion;
                seguimientoSemanalGestionObraOld.FechaModificacion = DateTime.Now;

                //Gestion Ambiental
                foreach (var SeguimientoSemanalGestionObraAmbiental in pSeguimientoSemanalGestionObra.SeguimientoSemanalGestionObraAmbiental)
                {

                    if (SeguimientoSemanalGestionObraAmbiental.SeguimientoSemanalGestionObraAmbientalId == 0)
                    {
                        SeguimientoSemanalGestionObraAmbiental.UsuarioCreacion = pUsuarioCreacion;
                        SeguimientoSemanalGestionObraAmbiental.Eliminado = false;
                        SeguimientoSemanalGestionObraAmbiental.FechaCreacion = DateTime.Now;
                        SeguimientoSemanalGestionObraAmbiental.RegistroCompleto = ValidarRegistroCompletoSeguimientoSemanalGestionObraAmbiental(SeguimientoSemanalGestionObraAmbiental);

                        _context.SeguimientoSemanalGestionObraAmbiental.Add(SeguimientoSemanalGestionObraAmbiental);

                        //Manejo Materiales e Insumos
                        if (SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo != null)
                        {
                            SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.UsuarioCreacion = pUsuarioCreacion;
                            SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.Eliminado = false;
                            SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.FechaCreacion = DateTime.Now;
                            SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.RegistroCompleto = ValidarRegistroCompletoManejoMaterialesInsumo(SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo);

                            _context.ManejoMaterialesInsumos.Add(SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo);

                            foreach (var ManejoMaterialesInsumosProveedor in SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.ManejoMaterialesInsumosProveedor)
                            {
                                ManejoMaterialesInsumosProveedor.UsuarioCreacion = pUsuarioCreacion;
                                ManejoMaterialesInsumosProveedor.Eliminado = false;
                                ManejoMaterialesInsumosProveedor.FechaCreacion = DateTime.Now;
                                ManejoMaterialesInsumosProveedor.RegistroCompleto = ValidarRegistroCompletoManejoMaterialesInsumosProveedor(ManejoMaterialesInsumosProveedor);

                                _context.ManejoMaterialesInsumosProveedor.Add(ManejoMaterialesInsumosProveedor);
                            }
                        }

                        //Manejo Residuos Construccion Demolicion
                        if (SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion != null)
                        {

                            SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.UsuarioCreacion = pUsuarioCreacion;
                            SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.Eliminado = false;
                            SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.FechaCreacion = DateTime.Now;
                            SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.RegistroCompleto = ValidarRegistroCompletoManejoResiduosConstruccionDemolicion(SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion);

                            _context.ManejoResiduosConstruccionDemolicion.Add(SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion);

                            foreach (var ManejoResiduosConstruccionDemolicionGestor in SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.ManejoResiduosConstruccionDemolicionGestor)
                            {
                                ManejoResiduosConstruccionDemolicionGestor.UsuarioCreacion = pUsuarioCreacion;
                                ManejoResiduosConstruccionDemolicionGestor.Eliminado = false;
                                ManejoResiduosConstruccionDemolicionGestor.FechaCreacion = DateTime.Now;
                                ManejoResiduosConstruccionDemolicionGestor.RegistroCompleto = ValidarRegistroCompletoManejoResiduosConstruccionDemolicionGestor(ManejoResiduosConstruccionDemolicionGestor);

                                _context.ManejoResiduosConstruccionDemolicionGestor.Add(ManejoResiduosConstruccionDemolicionGestor);
                            }
                        }

                        //Manejo Residuo Peligrosos Especiales
                        if (SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales != null)
                        {

                            SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.UsuarioCreacion = pUsuarioCreacion;
                            SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.Eliminado = false;
                            SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.FechaCreacion = DateTime.Now;
                            SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.RegistroCompleto = ValidarRegistroCompletoManejoResiduosPeligrososEspeciales(SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales);

                            _context.ManejoResiduosPeligrososEspeciales.Add(SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales);

                        }

                        //Manejo Otro
                        if (SeguimientoSemanalGestionObraAmbiental.ManejoOtro != null)
                        {
                            SeguimientoSemanalGestionObraAmbiental.ManejoOtro.UsuarioCreacion = pUsuarioCreacion;
                            SeguimientoSemanalGestionObraAmbiental.ManejoOtro.Eliminado = false;
                            SeguimientoSemanalGestionObraAmbiental.ManejoOtro.FechaCreacion = DateTime.Now;
                            SeguimientoSemanalGestionObraAmbiental.ManejoOtro.RegistroCompleto = ValidarRegistroCompletoManejoOtro(SeguimientoSemanalGestionObraAmbiental.ManejoOtro);

                            _context.ManejoOtro.Add(SeguimientoSemanalGestionObraAmbiental.ManejoOtro);
                        }


                        SeguimientoSemanalGestionObraAmbiental.RegistroCompleto = ValidarRegistroCompletoSeguimientoSemanalGestionObraAmbiental(SeguimientoSemanalGestionObraAmbiental);

                    }
                    else
                    {
                        SeguimientoSemanalGestionObraAmbiental SeguimientoSemanalGestionObraAmbientalOld = _context.SeguimientoSemanalGestionObraAmbiental.Find(SeguimientoSemanalGestionObraAmbiental.SeguimientoSemanalGestionObraAmbientalId);


                        SeguimientoSemanalGestionObraAmbientalOld.UsuarioModificacion = pUsuarioCreacion;
                        SeguimientoSemanalGestionObraAmbientalOld.FechaModificacion = DateTime.Now;
                        SeguimientoSemanalGestionObraAmbientalOld.SeEjecutoGestionAmbiental = SeguimientoSemanalGestionObraAmbiental.SeEjecutoGestionAmbiental;

                        SeguimientoSemanalGestionObraAmbientalOld.TieneManejoMaterialesInsumo = SeguimientoSemanalGestionObraAmbiental.TieneManejoMaterialesInsumo;
                        SeguimientoSemanalGestionObraAmbientalOld.TieneManejoResiduosConstruccionDemolicion = SeguimientoSemanalGestionObraAmbiental.TieneManejoResiduosConstruccionDemolicion;
                        SeguimientoSemanalGestionObraAmbientalOld.TieneManejoResiduosPeligrososEspeciales = SeguimientoSemanalGestionObraAmbiental.TieneManejoResiduosPeligrososEspeciales;
                        SeguimientoSemanalGestionObraAmbientalOld.TieneManejoOtro = SeguimientoSemanalGestionObraAmbiental.TieneManejoOtro;

                        if (SeguimientoSemanalGestionObraAmbiental.TieneManejoMaterialesInsumo == false)
                            SeguimientoSemanalGestionObraAmbientalOld.ManejoMaterialesInsumoId = null;

                        if (SeguimientoSemanalGestionObraAmbiental.TieneManejoOtro == false)
                            SeguimientoSemanalGestionObraAmbientalOld.ManejoOtroId = null;

                        if (SeguimientoSemanalGestionObraAmbiental.TieneManejoResiduosPeligrososEspeciales == false)
                            SeguimientoSemanalGestionObraAmbientalOld.ManejoResiduosPeligrososEspecialesId = null;

                        if (SeguimientoSemanalGestionObraAmbiental.TieneManejoResiduosConstruccionDemolicion == false)
                            SeguimientoSemanalGestionObraAmbientalOld.ManejoResiduosConstruccionDemolicionId = null;

                        //Manejo Materiales e Insumos
                        if (SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo != null)
                        {
                            //New ManejoMaterialesInsumos
                            if (SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.ManejoMaterialesInsumosId == 0)
                            {
                                ManejoMaterialesInsumos manejoMaterialesInsumosNew =
                                                                        new ManejoMaterialesInsumos
                                                                        {
                                                                            EstanProtegidosDemarcadosMateriales = SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.EstanProtegidosDemarcadosMateriales,
                                                                            RequiereObservacion = SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.RequiereObservacion,
                                                                            Observacion = SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.Observacion,
                                                                            Url = SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.Url,
                                                                            UsuarioCreacion = pUsuarioCreacion,
                                                                            Eliminado = false,
                                                                            FechaCreacion = DateTime.Now,
                                                                            RegistroCompleto = ValidarRegistroCompletoManejoMaterialesInsumo(SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo)
                                                                        };
                                _context.ManejoMaterialesInsumos.Add(manejoMaterialesInsumosNew);
                                _context.SaveChanges();
                                SeguimientoSemanalGestionObraAmbientalOld.ManejoMaterialesInsumoId = manejoMaterialesInsumosNew.ManejoMaterialesInsumosId;
                            }
                            //Update
                            else
                            {
                                ManejoMaterialesInsumos manejoMaterialesInsumosOld = _context.ManejoMaterialesInsumos.Find(SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.ManejoMaterialesInsumosId);

                                manejoMaterialesInsumosOld.FechaModificacion = DateTime.Now;
                                manejoMaterialesInsumosOld.UsuarioModificacion = pUsuarioCreacion;

                                manejoMaterialesInsumosOld.EstanProtegidosDemarcadosMateriales = SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.EstanProtegidosDemarcadosMateriales;
                                manejoMaterialesInsumosOld.RequiereObservacion = SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.RequiereObservacion;
                                manejoMaterialesInsumosOld.Observacion = SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.Observacion;
                                manejoMaterialesInsumosOld.Url = SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.Url;
                                manejoMaterialesInsumosOld.RegistroCompleto = ValidarRegistroCompletoManejoMaterialesInsumo(SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo);
                            }
                            foreach (var ManejoMaterialesInsumosProveedor in SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.ManejoMaterialesInsumosProveedor)
                            {
                                if (ManejoMaterialesInsumosProveedor.ManejoMaterialesInsumosProveedorId == 0)
                                {
                                    ManejoMaterialesInsumosProveedor.ManejoMaterialesInsumosId = (int)SeguimientoSemanalGestionObraAmbientalOld.ManejoMaterialesInsumoId;
                                    ManejoMaterialesInsumosProveedor.UsuarioCreacion = pUsuarioCreacion;
                                    ManejoMaterialesInsumosProveedor.Eliminado = false;
                                    ManejoMaterialesInsumosProveedor.FechaCreacion = DateTime.Now;
                                    ManejoMaterialesInsumosProveedor.RegistroCompleto = ValidarRegistroCompletoManejoMaterialesInsumosProveedor(ManejoMaterialesInsumosProveedor);

                                    _context.ManejoMaterialesInsumosProveedor.Add(ManejoMaterialesInsumosProveedor);
                                }
                                else
                                {
                                    ManejoMaterialesInsumosProveedor manejoMaterialesInsumosProveedorOld = _context.ManejoMaterialesInsumosProveedor.Find(ManejoMaterialesInsumosProveedor.ManejoMaterialesInsumosProveedorId);
                                    manejoMaterialesInsumosProveedorOld.UsuarioModificacion = pUsuarioCreacion;
                                    ManejoMaterialesInsumosProveedor.FechaModificacion = DateTime.Now;

                                    manejoMaterialesInsumosProveedorOld.Proveedor = ManejoMaterialesInsumosProveedor.Proveedor;
                                    manejoMaterialesInsumosProveedorOld.RequierePermisosAmbientalesMineros = ManejoMaterialesInsumosProveedor.RequierePermisosAmbientalesMineros;
                                    manejoMaterialesInsumosProveedorOld.UrlRegistroFotografico = ManejoMaterialesInsumosProveedor.UrlRegistroFotografico;
                                    manejoMaterialesInsumosProveedorOld.RegistroCompleto = ValidarRegistroCompletoManejoMaterialesInsumosProveedor(manejoMaterialesInsumosProveedorOld);
                                }
                            }
                        }

                        //Manejo Residuos Construccion Demolicion
                        if (SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion != null)
                        {
                            int idManejoResiduosConstruccionDemolicion = 0;
                            if (SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.ManejoResiduosConstruccionDemolicionId == 0)
                            {
                                ManejoResiduosConstruccionDemolicion manejoResiduosConstruccionDemolicion =

                                new ManejoResiduosConstruccionDemolicion
                                {
                                    EstaCuantificadoRcd = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.EstaCuantificadoRcd,
                                    RequiereObservacion = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.RequiereObservacion,
                                    Observacion = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.Observacion,
                                    SeReutilizadorResiduos = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.SeReutilizadorResiduos,
                                    CantidadToneladas = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.CantidadToneladas,

                                    UsuarioCreacion = pUsuarioCreacion,
                                    Eliminado = false,
                                    FechaCreacion = DateTime.Now,
                                    RegistroCompleto = ValidarRegistroCompletoManejoResiduosConstruccionDemolicion(SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion),
                                };
                                _context.ManejoResiduosConstruccionDemolicion.Add(manejoResiduosConstruccionDemolicion);
                                _context.SaveChanges();
                                SeguimientoSemanalGestionObraAmbientalOld.ManejoResiduosConstruccionDemolicionId = manejoResiduosConstruccionDemolicion.ManejoResiduosConstruccionDemolicionId;
                                idManejoResiduosConstruccionDemolicion = manejoResiduosConstruccionDemolicion.ManejoResiduosConstruccionDemolicionId;
                            }
                            else
                            {
                                ManejoResiduosConstruccionDemolicion manejoResiduosConstruccionDemolicionOld = _context.ManejoResiduosConstruccionDemolicion.Find(SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.ManejoResiduosConstruccionDemolicionId);
                                idManejoResiduosConstruccionDemolicion = manejoResiduosConstruccionDemolicionOld.ManejoResiduosConstruccionDemolicionId;
                                manejoResiduosConstruccionDemolicionOld.RegistroCompleto = ValidarRegistroCompletoManejoResiduosConstruccionDemolicion(SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion);
                                manejoResiduosConstruccionDemolicionOld.UsuarioModificacion = pUsuarioCreacion;
                                manejoResiduosConstruccionDemolicionOld.FechaModificacion = DateTime.Now;

                                manejoResiduosConstruccionDemolicionOld.EstaCuantificadoRcd = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.EstaCuantificadoRcd;
                                manejoResiduosConstruccionDemolicionOld.RequiereObservacion = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.RequiereObservacion;
                                manejoResiduosConstruccionDemolicionOld.Observacion = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.Observacion;
                                manejoResiduosConstruccionDemolicionOld.SeReutilizadorResiduos = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.SeReutilizadorResiduos;
                                manejoResiduosConstruccionDemolicionOld.CantidadToneladas = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.CantidadToneladas;

                            }

                            foreach (var ManejoResiduosConstruccionDemolicionGestor in SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.ManejoResiduosConstruccionDemolicionGestor)
                            {

                                if (ManejoResiduosConstruccionDemolicionGestor.ManejoResiduosConstruccionDemolicionGestorId == 0)
                                {
                                    ManejoResiduosConstruccionDemolicionGestor.ManejoResiduosConstruccionDemolicionId = idManejoResiduosConstruccionDemolicion;
                                    ManejoResiduosConstruccionDemolicionGestor.UsuarioCreacion = pUsuarioCreacion;
                                    ManejoResiduosConstruccionDemolicionGestor.Eliminado = false;
                                    ManejoResiduosConstruccionDemolicionGestor.FechaCreacion = DateTime.Now;
                                    ManejoResiduosConstruccionDemolicionGestor.RegistroCompleto = ValidarRegistroCompletoManejoResiduosConstruccionDemolicionGestor(ManejoResiduosConstruccionDemolicionGestor);

                                    _context.ManejoResiduosConstruccionDemolicionGestor.Add(ManejoResiduosConstruccionDemolicionGestor);
                                }
                                else
                                {
                                    ManejoResiduosConstruccionDemolicionGestor ManejoResiduosConstruccionDemolicionGestorOld = _context.ManejoResiduosConstruccionDemolicionGestor.Find(ManejoResiduosConstruccionDemolicionGestor.ManejoResiduosConstruccionDemolicionGestorId);
                                    ManejoResiduosConstruccionDemolicionGestorOld.RegistroCompleto = ValidarRegistroCompletoManejoResiduosConstruccionDemolicionGestor(ManejoResiduosConstruccionDemolicionGestor);
                                    ManejoResiduosConstruccionDemolicionGestorOld.UsuarioModificacion = pUsuarioCreacion;
                                    ManejoResiduosConstruccionDemolicionGestorOld.FechaModificacion = DateTime.Now;

                                    ManejoResiduosConstruccionDemolicionGestorOld.NombreGestorResiduos = ManejoResiduosConstruccionDemolicionGestor.NombreGestorResiduos;
                                    ManejoResiduosConstruccionDemolicionGestorOld.TienePermisoAmbiental = ManejoResiduosConstruccionDemolicionGestor.TienePermisoAmbiental;
                                    ManejoResiduosConstruccionDemolicionGestorOld.Url = ManejoResiduosConstruccionDemolicionGestor.Url;
                                }
                            }

                        }

                        //Manejo Residuo Peligrosos Especiales
                        if (SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales != null)
                        {
                            if (SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.ManejoResiduosPeligrososEspecialesId == 0)
                            {
                                ManejoResiduosPeligrososEspeciales manejoResiduosPeligrososEspeciales = new ManejoResiduosPeligrososEspeciales
                                {
                                    EstanClasificados = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.EstanClasificados,
                                    RequiereObservacion = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.RequiereObservacion,
                                    Observacion = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.Observacion,
                                    UrlRegistroFotografico = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.UrlRegistroFotografico,
                                    UsuarioCreacion = pUsuarioCreacion,
                                    Eliminado = false,
                                    FechaCreacion = DateTime.Now,
                                    RegistroCompleto = ValidarRegistroCompletoManejoResiduosPeligrososEspeciales(SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales)
                                };

                                _context.ManejoResiduosPeligrososEspeciales.Add(manejoResiduosPeligrososEspeciales);
                                _context.SaveChanges();

                                SeguimientoSemanalGestionObraAmbientalOld.ManejoResiduosPeligrososEspecialesId = manejoResiduosPeligrososEspeciales.ManejoResiduosPeligrososEspecialesId;
                            }
                            else
                            {
                                ManejoResiduosPeligrososEspeciales ManejoResiduosPeligrososEspecialesOld = _context.ManejoResiduosPeligrososEspeciales.Find(SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.ManejoResiduosPeligrososEspecialesId);

                                ManejoResiduosPeligrososEspecialesOld.RegistroCompleto = ValidarRegistroCompletoManejoResiduosPeligrososEspeciales(SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales);
                                ManejoResiduosPeligrososEspecialesOld.UsuarioModificacion = pUsuarioCreacion;
                                ManejoResiduosPeligrososEspecialesOld.FechaModificacion = DateTime.Now;

                                ManejoResiduosPeligrososEspecialesOld.UrlRegistroFotografico = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.UrlRegistroFotografico;
                                ManejoResiduosPeligrososEspecialesOld.EstanClasificados = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.EstanClasificados;
                                ManejoResiduosPeligrososEspecialesOld.RequiereObservacion = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.RequiereObservacion;
                                ManejoResiduosPeligrososEspecialesOld.Observacion = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.Observacion;
                            }
                        }

                        //Manejo Otro
                        if (SeguimientoSemanalGestionObraAmbiental.ManejoOtro != null)
                        {
                            if (SeguimientoSemanalGestionObraAmbiental.ManejoOtro.ManejoOtroId == 0)
                            {
                                ManejoOtro manejoOtro = new ManejoOtro
                                {
                                    FechaActividad = SeguimientoSemanalGestionObraAmbiental.ManejoOtro.FechaActividad,
                                    Actividad = SeguimientoSemanalGestionObraAmbiental.ManejoOtro.Actividad,
                                    UrlSoporteGestion = SeguimientoSemanalGestionObraAmbiental.ManejoOtro.UrlSoporteGestion,
                                    UsuarioCreacion = pUsuarioCreacion,
                                    Eliminado = false,
                                    FechaCreacion = DateTime.Now,
                                    RegistroCompleto = ValidarRegistroCompletoManejoOtro(SeguimientoSemanalGestionObraAmbiental.ManejoOtro)
                                };
                                _context.ManejoOtro.Add(manejoOtro);
                                _context.SaveChanges();
                                SeguimientoSemanalGestionObraAmbientalOld.ManejoOtroId = manejoOtro.ManejoOtroId;
                            }
                            else
                            {
                                ManejoOtro manejoOtroOld = _context.ManejoOtro.Find(SeguimientoSemanalGestionObraAmbiental.ManejoOtro.ManejoOtroId);

                                manejoOtroOld.RegistroCompleto = ValidarRegistroCompletoManejoOtro(SeguimientoSemanalGestionObraAmbiental.ManejoOtro);
                                manejoOtroOld.UsuarioModificacion = pUsuarioCreacion;
                                manejoOtroOld.FechaModificacion = DateTime.Now;

                                manejoOtroOld.FechaActividad = SeguimientoSemanalGestionObraAmbiental.ManejoOtro.FechaActividad;
                                manejoOtroOld.Actividad = SeguimientoSemanalGestionObraAmbiental.ManejoOtro.Actividad;
                                manejoOtroOld.UrlSoporteGestion = SeguimientoSemanalGestionObraAmbiental.ManejoOtro.UrlSoporteGestion;
                            }
                        }

                        //Validar Registro Completo
                        SeguimientoSemanalGestionObraAmbientalOld.RegistroCompleto = ValidarRegistroCompletoSeguimientoSemanalGestionObraAmbiental(SeguimientoSemanalGestionObraAmbiental);

                    }
                }

                //Gestion Calidad
                foreach (var SeguimientoSemanalGestionObraCalidad in pSeguimientoSemanalGestionObra.SeguimientoSemanalGestionObraCalidad)
                {
                    //Ensayo Laboratorio
                    foreach (var GestionObraCalidadEnsayoLaboratorio in SeguimientoSemanalGestionObraCalidad.GestionObraCalidadEnsayoLaboratorio)
                    {
                        if (GestionObraCalidadEnsayoLaboratorio.GestionObraCalidadEnsayoLaboratorioId == 0)
                        {
                            GestionObraCalidadEnsayoLaboratorio.UsuarioCreacion = pUsuarioCreacion;
                            GestionObraCalidadEnsayoLaboratorio.Eliminado = false;
                            GestionObraCalidadEnsayoLaboratorio.FechaCreacion = DateTime.Now;
                            GestionObraCalidadEnsayoLaboratorio.RegistroCompleto = ValidarRegistroCompletoGestionObraCalidadEnsayoLaboratorio(GestionObraCalidadEnsayoLaboratorio);

                            _context.GestionObraCalidadEnsayoLaboratorio.Add(GestionObraCalidadEnsayoLaboratorio);
                        }
                        else
                        {
                            GestionObraCalidadEnsayoLaboratorio gestionObraCalidadEnsayoLaboratorioOld = _context.GestionObraCalidadEnsayoLaboratorio.Find(GestionObraCalidadEnsayoLaboratorio.GestionObraCalidadEnsayoLaboratorioId);
                            gestionObraCalidadEnsayoLaboratorioOld.RegistroCompleto = ValidarRegistroCompletoGestionObraCalidadEnsayoLaboratorio(GestionObraCalidadEnsayoLaboratorio);
                            gestionObraCalidadEnsayoLaboratorioOld.UsuarioModificacion = pUsuarioCreacion;
                            gestionObraCalidadEnsayoLaboratorioOld.FechaModificacion = DateTime.Now;

                            gestionObraCalidadEnsayoLaboratorioOld.TipoEnsayoCodigo = GestionObraCalidadEnsayoLaboratorio.TipoEnsayoCodigo;
                            gestionObraCalidadEnsayoLaboratorioOld.NumeroMuestras = GestionObraCalidadEnsayoLaboratorio.NumeroMuestras;
                            gestionObraCalidadEnsayoLaboratorioOld.FechaTomaMuestras = GestionObraCalidadEnsayoLaboratorio.FechaTomaMuestras;
                            gestionObraCalidadEnsayoLaboratorioOld.FechaEntregaResultados = GestionObraCalidadEnsayoLaboratorio.FechaEntregaResultados;
                            gestionObraCalidadEnsayoLaboratorioOld.RealizoControlMedicion = GestionObraCalidadEnsayoLaboratorio.RealizoControlMedicion;
                            gestionObraCalidadEnsayoLaboratorioOld.Observacion = GestionObraCalidadEnsayoLaboratorio.Observacion;
                            gestionObraCalidadEnsayoLaboratorioOld.UrlSoporteGestion = GestionObraCalidadEnsayoLaboratorio.UrlSoporteGestion;
                        }
                        foreach (var EnsayoLaboratorioMuestra in GestionObraCalidadEnsayoLaboratorio.EnsayoLaboratorioMuestra)
                        {
                            if (EnsayoLaboratorioMuestra.EnsayoLaboratorioMuestraId == 0)
                            {
                                EnsayoLaboratorioMuestra.UsuarioCreacion = pUsuarioCreacion;
                                EnsayoLaboratorioMuestra.Eliminado = false;
                                EnsayoLaboratorioMuestra.FechaCreacion = DateTime.Now;
                                // EnsayoLaboratorioMuestra.RegistroCompleto = ValidarRegistroCompletoGestionEnsayoLaboratorioMuestra(EnsayoLaboratorioMuestra);

                                _context.EnsayoLaboratorioMuestra.Add(EnsayoLaboratorioMuestra);
                            }
                            else
                            {
                                EnsayoLaboratorioMuestra ensayoLaboratorioMuestraOld = _context.EnsayoLaboratorioMuestra.Find(EnsayoLaboratorioMuestra.EnsayoLaboratorioMuestraId);
                                //  ensayoLaboratorioMuestraOld.RegistroCompleto = ValidarRegistroCompletoGestionEnsayoLaboratorioMuestra(EnsayoLaboratorioMuestra);
                                ensayoLaboratorioMuestraOld.UsuarioModificacion = pUsuarioCreacion;
                                ensayoLaboratorioMuestraOld.FechaModificacion = DateTime.Now;

                                ensayoLaboratorioMuestraOld.FechaEntregaResultado = EnsayoLaboratorioMuestra.FechaEntregaResultado;
                                ensayoLaboratorioMuestraOld.NombreMuestra = EnsayoLaboratorioMuestra.NombreMuestra;
                                ensayoLaboratorioMuestraOld.Observacion = EnsayoLaboratorioMuestra.Observacion;
                            }
                        }
                    }
                    if (SeguimientoSemanalGestionObraCalidad.SeguimientoSemanalGestionObraCalidadId == 0)
                    {
                        SeguimientoSemanalGestionObraCalidad.UsuarioCreacion = pUsuarioCreacion;
                        SeguimientoSemanalGestionObraCalidad.Eliminado = false;
                        SeguimientoSemanalGestionObraCalidad.FechaCreacion = DateTime.Now;
                        SeguimientoSemanalGestionObraCalidad.RegistroCompleto = ValidarRegistroCompletoSeguimientoSemanalGestionObraCalidad(SeguimientoSemanalGestionObraCalidad);

                        _context.SeguimientoSemanalGestionObraCalidad.Add(SeguimientoSemanalGestionObraCalidad);
                    }
                    else
                    {
                        SeguimientoSemanalGestionObraCalidad seguimientoSemanalGestionObraCalidadOld = _context.SeguimientoSemanalGestionObraCalidad.Find(SeguimientoSemanalGestionObraCalidad.SeguimientoSemanalGestionObraCalidadId);
                        seguimientoSemanalGestionObraCalidadOld.UsuarioModificacion = pUsuarioCreacion;
                        seguimientoSemanalGestionObraCalidadOld.FechaModificacion = DateTime.Now;
                        seguimientoSemanalGestionObraCalidadOld.RegistroCompleto = ValidarRegistroCompletoSeguimientoSemanalGestionObraCalidad(SeguimientoSemanalGestionObraCalidad);

                        seguimientoSemanalGestionObraCalidadOld.SeRealizaronEnsayosLaboratorio = SeguimientoSemanalGestionObraCalidad.SeRealizaronEnsayosLaboratorio;

                        if (SeguimientoSemanalGestionObraCalidad.SeRealizaronEnsayosLaboratorio == false)
                            EliminarGestionObraCalidadEnsayoLaboratorioAndEnsayoLaboratorioMuestra(SeguimientoSemanalGestionObraCalidad.SeguimientoSemanalGestionObraCalidadId, pSeguimientoSemanalGestionObra.SeguimientoSemanalGestionObraId);


                    }
                }

                //Gestion Seguridad Salud
                foreach (var SeguimientoSemanalGestionObraSeguridadSalud in pSeguimientoSemanalGestionObra.SeguimientoSemanalGestionObraSeguridadSalud)
                {
                    if (SeguimientoSemanalGestionObraSeguridadSalud.SeguimientoSemanalGestionObraSeguridadSaludId == 0)
                    {
                        SeguimientoSemanalGestionObraSeguridadSalud.UsuarioCreacion = pUsuarioCreacion;
                        SeguimientoSemanalGestionObraSeguridadSalud.Eliminado = false;
                        SeguimientoSemanalGestionObraSeguridadSalud.FechaCreacion = DateTime.Now;
                        SeguimientoSemanalGestionObraSeguridadSalud.RegistroCompleto = ValidarRegistroCompletoSeguimientoSemanalGestionObraSeguridadSalud(SeguimientoSemanalGestionObraSeguridadSalud);

                        _context.SeguimientoSemanalGestionObraSeguridadSalud.Add(SeguimientoSemanalGestionObraSeguridadSalud);

                        List<SeguridadSaludCausaAccidente> seguridadSaludCausaAccidentesDelete = _context.SeguridadSaludCausaAccidente.Where(r => r.SeguimientoSemanalGestionObraSeguridadSaludId == r.SeguimientoSemanalGestionObraSeguridadSalud.SeguimientoSemanalGestionObraSeguridadSaludId).ToList();

                        if (seguridadSaludCausaAccidentesDelete.Count() > 0)
                        {
                            _context.SeguridadSaludCausaAccidente.RemoveRange(seguridadSaludCausaAccidentesDelete);
                        }

                        foreach (var SeguridadSaludCausaAccidente in SeguimientoSemanalGestionObraSeguridadSalud.SeguridadSaludCausaAccidente)
                        {
                            SeguridadSaludCausaAccidente.UsuarioCreacion = pUsuarioCreacion;
                            SeguridadSaludCausaAccidente.FechaCreacion = DateTime.Now;
                            _context.SeguridadSaludCausaAccidente.Add(SeguridadSaludCausaAccidente);
                        }
                    }
                    else
                    {
                        SeguimientoSemanalGestionObraSeguridadSalud SeguimientoSemanalGestionObraSeguridadSaludOld = _context.SeguimientoSemanalGestionObraSeguridadSalud.Find(SeguimientoSemanalGestionObraSeguridadSalud.SeguimientoSemanalGestionObraSeguridadSaludId);

                        SeguimientoSemanalGestionObraSeguridadSaludOld.RegistroCompleto = ValidarRegistroCompletoSeguimientoSemanalGestionObraSeguridadSalud(SeguimientoSemanalGestionObraSeguridadSalud);
                        SeguimientoSemanalGestionObraSeguridadSaludOld.UsuarioModificacion = pUsuarioCreacion;
                        SeguimientoSemanalGestionObraSeguridadSaludOld.FechaModificacion = DateTime.Now;

                        SeguimientoSemanalGestionObraSeguridadSaludOld.SeRealizoCapacitacion = SeguimientoSemanalGestionObraSeguridadSalud.SeRealizoCapacitacion;
                        SeguimientoSemanalGestionObraSeguridadSaludOld.TemaCapacitacion = SeguimientoSemanalGestionObraSeguridadSalud.TemaCapacitacion;
                        SeguimientoSemanalGestionObraSeguridadSaludOld.SeRealizoRevisionElementosProteccion = SeguimientoSemanalGestionObraSeguridadSalud.SeRealizoRevisionElementosProteccion;
                        SeguimientoSemanalGestionObraSeguridadSaludOld.CumpleRevisionElementosProyeccion = SeguimientoSemanalGestionObraSeguridadSalud.CumpleRevisionElementosProyeccion;
                        SeguimientoSemanalGestionObraSeguridadSaludOld.SeRealizoRevisionSenalizacion = SeguimientoSemanalGestionObraSeguridadSalud.SeRealizoRevisionSenalizacion;
                        SeguimientoSemanalGestionObraSeguridadSaludOld.CumpleRevisionSenalizacion = SeguimientoSemanalGestionObraSeguridadSalud.CumpleRevisionSenalizacion;
                        SeguimientoSemanalGestionObraSeguridadSaludOld.UrlSoporteGestion = SeguimientoSemanalGestionObraSeguridadSalud.UrlSoporteGestion;
                        SeguimientoSemanalGestionObraSeguridadSaludOld.CantidadAccidentes = SeguimientoSemanalGestionObraSeguridadSalud.CantidadAccidentes;
                        SeguimientoSemanalGestionObraSeguridadSaludOld.ObservacionCapacitacion = SeguimientoSemanalGestionObraSeguridadSalud.ObservacionCapacitacion;
                        SeguimientoSemanalGestionObraSeguridadSaludOld.ObservacionRevisionElementosProteccion = SeguimientoSemanalGestionObraSeguridadSalud.ObservacionRevisionElementosProteccion;
                        SeguimientoSemanalGestionObraSeguridadSaludOld.ObservacionRevisionSenalizacion = SeguimientoSemanalGestionObraSeguridadSalud.ObservacionRevisionSenalizacion;

                        List<SeguridadSaludCausaAccidente> seguridadSaludCausaAccidentesDelete = _context.SeguridadSaludCausaAccidente.Where(r => r.SeguimientoSemanalGestionObraSeguridadSaludId == r.SeguimientoSemanalGestionObraSeguridadSalud.SeguimientoSemanalGestionObraSeguridadSaludId).ToList();

                        if (seguridadSaludCausaAccidentesDelete.Count() > 0)
                        {
                            _context.SeguridadSaludCausaAccidente.RemoveRange(seguridadSaludCausaAccidentesDelete);
                        }

                        foreach (var SeguridadSaludCausaAccidente in SeguimientoSemanalGestionObraSeguridadSalud.SeguridadSaludCausaAccidente)
                        {
                            SeguridadSaludCausaAccidente.UsuarioCreacion = pUsuarioCreacion;
                            SeguridadSaludCausaAccidente.FechaCreacion = DateTime.Now;
                            _context.SeguridadSaludCausaAccidente.Add(SeguridadSaludCausaAccidente);
                        }
                    }
                }

                //Gestion Obra Social
                foreach (var SeguimientoSemanalGestionObraSocial in pSeguimientoSemanalGestionObra.SeguimientoSemanalGestionObraSocial)
                {
                    if (SeguimientoSemanalGestionObraSocial.SeguimientoSemanalGestionObraSocialId == 0)
                    {
                        SeguimientoSemanalGestionObraSocial.UsuarioCreacion = pUsuarioCreacion;
                        SeguimientoSemanalGestionObraSocial.Eliminado = false;
                        SeguimientoSemanalGestionObraSocial.FechaCreacion = DateTime.Now;
                        SeguimientoSemanalGestionObraSocial.RegistroCompleto = ValidarRegistroCompletoSeguimientoSemanalGestionObraSocial(SeguimientoSemanalGestionObraSocial);

                        _context.SeguimientoSemanalGestionObraSocial.Add(SeguimientoSemanalGestionObraSocial);
                    }
                    else
                    {
                        SeguimientoSemanalGestionObraSocial SeguimientoSemanalGestionObraSocialOld = _context.SeguimientoSemanalGestionObraSocial.Find(SeguimientoSemanalGestionObraSocial.SeguimientoSemanalGestionObraSocialId);

                        SeguimientoSemanalGestionObraSocialOld.RegistroCompleto = ValidarRegistroCompletoSeguimientoSemanalGestionObraSocial(SeguimientoSemanalGestionObraSocial);
                        SeguimientoSemanalGestionObraSocialOld.UsuarioModificacion = pUsuarioCreacion;
                        SeguimientoSemanalGestionObraSocialOld.FechaModificacion = DateTime.Now;

                        SeguimientoSemanalGestionObraSocialOld.SeRealizaronReuniones = SeguimientoSemanalGestionObraSocial.SeRealizaronReuniones;
                        SeguimientoSemanalGestionObraSocialOld.TemaComunidad = SeguimientoSemanalGestionObraSocial.TemaComunidad;
                        SeguimientoSemanalGestionObraSocialOld.Conclusion = SeguimientoSemanalGestionObraSocial.Conclusion;
                        SeguimientoSemanalGestionObraSocialOld.CantidadEmpleosDirectos = SeguimientoSemanalGestionObraSocial.CantidadEmpleosDirectos;
                        SeguimientoSemanalGestionObraSocialOld.CantidadEmpleosIndirectos = SeguimientoSemanalGestionObraSocial.CantidadEmpleosIndirectos;
                        SeguimientoSemanalGestionObraSocialOld.CantidadTotalEmpleos = SeguimientoSemanalGestionObraSocial.CantidadTotalEmpleos;
                        SeguimientoSemanalGestionObraSocialOld.UrlSoporteGestion = SeguimientoSemanalGestionObraSocial.UrlSoporteGestion;
                        SeguimientoSemanalGestionObraSocialOld.ObservacionRealizaronReuniones = SeguimientoSemanalGestionObraSocial.ObservacionRealizaronReuniones;
                    }
                }

                //Gestion Alerta
                foreach (var SeguimientoSemanalGestionObraAlerta in pSeguimientoSemanalGestionObra.SeguimientoSemanalGestionObraAlerta)
                {
                    if (SeguimientoSemanalGestionObraAlerta.SeguimientoSemanalGestionObraAlertaId == 0)
                    {
                        SeguimientoSemanalGestionObraAlerta.UsuarioCreacion = pUsuarioCreacion;
                        SeguimientoSemanalGestionObraAlerta.Eliminado = false;
                        SeguimientoSemanalGestionObraAlerta.FechaCreacion = DateTime.Now;
                        SeguimientoSemanalGestionObraAlerta.RegistroCompleto = ValidarRegistroCompletoSeguimientoSemanalGestionObraAlerta(SeguimientoSemanalGestionObraAlerta);

                        _context.SeguimientoSemanalGestionObraAlerta.Add(SeguimientoSemanalGestionObraAlerta);
                    }
                    else
                    {
                        SeguimientoSemanalGestionObraAlerta seguimientoSemanalGestionObraAlertaOld = _context.SeguimientoSemanalGestionObraAlerta.Find(SeguimientoSemanalGestionObraAlerta.SeguimientoSemanalGestionObraAlertaId);
                        seguimientoSemanalGestionObraAlertaOld.RegistroCompleto = ValidarRegistroCompletoSeguimientoSemanalGestionObraAlerta(SeguimientoSemanalGestionObraAlerta);
                        seguimientoSemanalGestionObraAlertaOld.UsuarioModificacion = pUsuarioCreacion;
                        seguimientoSemanalGestionObraAlertaOld.FechaModificacion = DateTime.Now;
                        seguimientoSemanalGestionObraAlertaOld.Eliminado = false;

                        seguimientoSemanalGestionObraAlertaOld.SeIdentificaronAlertas = SeguimientoSemanalGestionObraAlerta.SeIdentificaronAlertas;
                        seguimientoSemanalGestionObraAlertaOld.Alerta = SeguimientoSemanalGestionObraAlerta.Alerta;
                    }
                }
                seguimientoSemanalGestionObraOld.RegistroCompleto = ValidarRegistroCompletoSeguimientoSemanalGestionObra(pSeguimientoSemanalGestionObra);
            }
        }

        private void SaveUpdateReporteActividades(SeguimientoSemanalReporteActividad pSeguimientoSemanalReporteActividad, string pUsuarioCreacion)
        {
            if (pSeguimientoSemanalReporteActividad.SeguimientoSemanalReporteActividadId == 0)
            {
                //Auditoria
                pSeguimientoSemanalReporteActividad.UsuarioCreacion = pUsuarioCreacion;
                pSeguimientoSemanalReporteActividad.Eliminado = false;
                pSeguimientoSemanalReporteActividad.FechaCreacion = DateTime.Now;

                pSeguimientoSemanalReporteActividad.RegistroCompletoActividad =
                       !string.IsNullOrEmpty(pSeguimientoSemanalReporteActividad.ActividadTecnica)
                    && !string.IsNullOrEmpty(pSeguimientoSemanalReporteActividad.ActividadLegal)
                    && !string.IsNullOrEmpty(pSeguimientoSemanalReporteActividad.ActividadAdministrativaFinanciera) ? true : false;

                pSeguimientoSemanalReporteActividad.RegistroCompletoActividadSiguiente =
                       !string.IsNullOrEmpty(pSeguimientoSemanalReporteActividad.ActividadTecnicaSiguiente)
                    && !string.IsNullOrEmpty(pSeguimientoSemanalReporteActividad.ActividadLegalSiguiente)
                    && !string.IsNullOrEmpty(pSeguimientoSemanalReporteActividad.ActividadAdministrativaFinancieraSiguiente) ? true : false;

                pSeguimientoSemanalReporteActividad.RegistroCompletoEstadoContrato = !string.IsNullOrEmpty(pSeguimientoSemanalReporteActividad.ResumenEstadoContrato) ? true : false;

                pSeguimientoSemanalReporteActividad.RegistroCompleto =
                    pSeguimientoSemanalReporteActividad.RegistroCompletoActividad == true &&
                    pSeguimientoSemanalReporteActividad.RegistroCompletoActividadSiguiente == true &&
                    pSeguimientoSemanalReporteActividad.RegistroCompletoEstadoContrato == true ? true : false;

                _context.SeguimientoSemanalReporteActividad.Add(pSeguimientoSemanalReporteActividad);
            }
            else
            {
                SeguimientoSemanalReporteActividad seguimientoSemanalReporteActividadOld = _context.SeguimientoSemanalReporteActividad.Find(pSeguimientoSemanalReporteActividad.SeguimientoSemanalReporteActividadId);
                seguimientoSemanalReporteActividadOld.UsuarioModificacion = pUsuarioCreacion;
                seguimientoSemanalReporteActividadOld.FechaModificacion = DateTime.Now;

                seguimientoSemanalReporteActividadOld.ResumenEstadoContrato = pSeguimientoSemanalReporteActividad.ResumenEstadoContrato;
                seguimientoSemanalReporteActividadOld.ActividadTecnica = pSeguimientoSemanalReporteActividad.ActividadTecnica;
                seguimientoSemanalReporteActividadOld.ActividadLegal = pSeguimientoSemanalReporteActividad.ActividadLegal;
                seguimientoSemanalReporteActividadOld.ActividadAdministrativaFinanciera = pSeguimientoSemanalReporteActividad.ActividadAdministrativaFinanciera;
                seguimientoSemanalReporteActividadOld.ActividadTecnicaSiguiente = pSeguimientoSemanalReporteActividad.ActividadTecnicaSiguiente;
                seguimientoSemanalReporteActividadOld.ActividadLegalSiguiente = pSeguimientoSemanalReporteActividad.ActividadLegalSiguiente;
                seguimientoSemanalReporteActividadOld.ActividadAdministrativaFinancieraSiguiente = pSeguimientoSemanalReporteActividad.ActividadAdministrativaFinancieraSiguiente;

                seguimientoSemanalReporteActividadOld.RegistroCompletoActividad =
                         !string.IsNullOrEmpty(pSeguimientoSemanalReporteActividad.ActividadTecnica)
                      && !string.IsNullOrEmpty(pSeguimientoSemanalReporteActividad.ActividadLegal)
                      && !string.IsNullOrEmpty(pSeguimientoSemanalReporteActividad.ActividadAdministrativaFinanciera) ? true : false;

                seguimientoSemanalReporteActividadOld.RegistroCompletoActividadSiguiente =
                       !string.IsNullOrEmpty(pSeguimientoSemanalReporteActividad.ActividadTecnicaSiguiente)
                    && !string.IsNullOrEmpty(pSeguimientoSemanalReporteActividad.ActividadLegalSiguiente)
                    && !string.IsNullOrEmpty(pSeguimientoSemanalReporteActividad.ActividadAdministrativaFinancieraSiguiente) ? true : false;

                seguimientoSemanalReporteActividadOld.RegistroCompletoEstadoContrato =
                    !string.IsNullOrEmpty(pSeguimientoSemanalReporteActividad.ResumenEstadoContrato) ? true : false;

                seguimientoSemanalReporteActividadOld.RegistroCompleto =
                                pSeguimientoSemanalReporteActividad.RegistroCompletoActividad == true &&
                                pSeguimientoSemanalReporteActividad.RegistroCompletoActividadSiguiente == true &&
                                pSeguimientoSemanalReporteActividad.RegistroCompletoEstadoContrato == true ? true : false;
            }
        }

        private void SaveUpdateRegistroFotografico(SeguimientoSemanalRegistroFotografico pSeguimientoSemanalRegistroFotografico, string pUsuarioCreacion)
        {
            if (pSeguimientoSemanalRegistroFotografico.SeguimientoSemanalRegistroFotograficoId == 0)
            {
                //Auditoria
                pSeguimientoSemanalRegistroFotografico.UsuarioCreacion = pUsuarioCreacion;
                pSeguimientoSemanalRegistroFotografico.Eliminado = false;
                pSeguimientoSemanalRegistroFotografico.FechaCreacion = DateTime.Now;

                pSeguimientoSemanalRegistroFotografico.RegistroCompleto =
                       !string.IsNullOrEmpty(pSeguimientoSemanalRegistroFotografico.UrlSoporteFotografico)
                    && !string.IsNullOrEmpty(pSeguimientoSemanalRegistroFotografico.Descripcion) ? true : false;

                _context.SeguimientoSemanalRegistroFotografico.Add(pSeguimientoSemanalRegistroFotografico);

            }
            else
            {
                SeguimientoSemanalRegistroFotografico seguimientoSemanalRegistroFotograficoOld = _context.SeguimientoSemanalRegistroFotografico.Find(pSeguimientoSemanalRegistroFotografico.SeguimientoSemanalRegistroFotograficoId);
                seguimientoSemanalRegistroFotograficoOld.UsuarioModificacion = pUsuarioCreacion;
                seguimientoSemanalRegistroFotograficoOld.FechaModificacion = DateTime.Now;
                seguimientoSemanalRegistroFotograficoOld.RegistroCompleto =
                       !string.IsNullOrEmpty(pSeguimientoSemanalRegistroFotografico.UrlSoporteFotografico)
                    && !string.IsNullOrEmpty(pSeguimientoSemanalRegistroFotografico.Descripcion) ? true : false;

                seguimientoSemanalRegistroFotograficoOld.UrlSoporteFotografico = pSeguimientoSemanalRegistroFotografico.UrlSoporteFotografico;
                seguimientoSemanalRegistroFotograficoOld.Descripcion = pSeguimientoSemanalRegistroFotografico.Descripcion;
            }
        }

        private void SaveUpdateComiteObra(SeguimientoSemanalRegistrarComiteObra pSeguimientoSemanalRegistrarComiteObra, string pUsuarioCreacion)
        {
            if (pSeguimientoSemanalRegistrarComiteObra.SeguimientoSemanalRegistrarComiteObraId == 0)
            {
                pSeguimientoSemanalRegistrarComiteObra.UsuarioCreacion = pUsuarioCreacion;
                pSeguimientoSemanalRegistrarComiteObra.Eliminado = false;
                pSeguimientoSemanalRegistrarComiteObra.FechaCreacion = DateTime.Now;

                pSeguimientoSemanalRegistrarComiteObra.RegistroCompleto =
                pSeguimientoSemanalRegistrarComiteObra.FechaComite.HasValue ? true : false;

                _context.SeguimientoSemanalRegistrarComiteObra.Add(pSeguimientoSemanalRegistrarComiteObra);
            }
            else
            {
                SeguimientoSemanalRegistrarComiteObra SeguimientoSemanalRegistrarComiteObraOld = _context.SeguimientoSemanalRegistrarComiteObra.Find(pSeguimientoSemanalRegistrarComiteObra.SeguimientoSemanalRegistrarComiteObraId);
                SeguimientoSemanalRegistrarComiteObraOld.UsuarioModificacion = pUsuarioCreacion;
                SeguimientoSemanalRegistrarComiteObraOld.FechaModificacion = DateTime.Now;
                SeguimientoSemanalRegistrarComiteObraOld.RegistroCompleto = pSeguimientoSemanalRegistrarComiteObra.FechaComite.HasValue ? true : false;

                SeguimientoSemanalRegistrarComiteObraOld.FechaComite = pSeguimientoSemanalRegistrarComiteObra.FechaComite;
                SeguimientoSemanalRegistrarComiteObraOld.UrlSoporteComite = pSeguimientoSemanalRegistrarComiteObra.UrlSoporteComite;
            }
        }

        #endregion

        #region Validate Complete Records

        private bool ValidarRegistroCompletoSeguimientoSemanal(SeguimientoSemanal pSeguimientoSemanal)
        {
            try
            {
                if (pSeguimientoSemanal == null)
                    return false;

                //Financiero solo se valida cada 5 semanas 
                if (pSeguimientoSemanal.NumeroSemana % 5 == 0)
                {
                    if (pSeguimientoSemanal?.SeguimientoSemanalAvanceFinanciero.Count() == 0)
                        return false;
                    if (pSeguimientoSemanal?.SeguimientoSemanalAvanceFinanciero?.FirstOrDefault().RegistroCompleto == false)
                        return false;
                }
                //Si tiene flujo Inversion no tiene actividades
                if (pSeguimientoSemanal.FlujoInversion.Count() > 0)
                {
                    if (pSeguimientoSemanal?.SeguimientoSemanalAvanceFisico.Count() == 0)
                        return false;
                    if (pSeguimientoSemanal?.SeguimientoSemanalAvanceFisico?.FirstOrDefault().RegistroCompleto == false)
                        return false;
                }


                if (pSeguimientoSemanal?.SeguimientoSemanalGestionObra.Count() == 0)
                    return false;
                if (pSeguimientoSemanal?.SeguimientoSemanalGestionObra?.FirstOrDefault().RegistroCompleto == false)
                    return false;

                if (pSeguimientoSemanal?.SeguimientoSemanalReporteActividad?.Count() == 0)
                    return false;
                if (pSeguimientoSemanal?.SeguimientoSemanalReporteActividad?.FirstOrDefault().RegistroCompleto == false)
                    return false;

                if (pSeguimientoSemanal?.SeguimientoSemanalRegistroFotografico?.Count() == 0)
                    return false;
                if (pSeguimientoSemanal?.SeguimientoSemanalRegistroFotografico?.FirstOrDefault().RegistroCompleto == false)
                    return false;

                if (pSeguimientoSemanal?.SeguimientoSemanalRegistrarComiteObra?.Count() == 0)
                    return false;
                if (pSeguimientoSemanal?.SeguimientoSemanalRegistrarComiteObra?.FirstOrDefault().RegistroCompleto == false)
                    return false;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private bool ValidarRegistroCompletoSeguimientoSemanalGestionObra(SeguimientoSemanalGestionObra pSeguimientoSemanalGestionObra)
        {
            if (pSeguimientoSemanalGestionObra == null)
                return false;

            if (pSeguimientoSemanalGestionObra?.SeguimientoSemanalGestionObraAmbiental.Count() == 0)
                return false;
            if (pSeguimientoSemanalGestionObra?.SeguimientoSemanalGestionObraAmbiental.FirstOrDefault().RegistroCompleto == false)
                return false;

            if (pSeguimientoSemanalGestionObra?.SeguimientoSemanalGestionObraCalidad.Count() == 0)
                return false;
            if (pSeguimientoSemanalGestionObra?.SeguimientoSemanalGestionObraCalidad.FirstOrDefault().RegistroCompleto == false)
                return false;

            if (pSeguimientoSemanalGestionObra?.SeguimientoSemanalGestionObraSeguridadSalud.Count() == 0)
                return false;
            if (pSeguimientoSemanalGestionObra?.SeguimientoSemanalGestionObraSeguridadSalud?.FirstOrDefault().RegistroCompleto == false)
                return false;

            if (pSeguimientoSemanalGestionObra?.SeguimientoSemanalGestionObraSocial.Count() == 0)
                return false;
            if (pSeguimientoSemanalGestionObra?.SeguimientoSemanalGestionObraSocial?.FirstOrDefault().RegistroCompleto == false)
                return false;

            if (pSeguimientoSemanalGestionObra?.SeguimientoSemanalGestionObraAlerta?.Count() == 0)
                return false;
            if (pSeguimientoSemanalGestionObra?.SeguimientoSemanalGestionObraAlerta?.FirstOrDefault().RegistroCompleto == false)
                return false;

            return true;
        }

        #region Gestion Obra Ambiental
        private bool ValidarRegistroCompletoSeguimientoSemanalGestionObraAlerta(SeguimientoSemanalGestionObraAlerta seguimientoSemanalGestionObraAlerta)
        {
            if (seguimientoSemanalGestionObraAlerta == null)
                return false;

            if (!seguimientoSemanalGestionObraAlerta.SeIdentificaronAlertas.HasValue)
                return false;

            if (seguimientoSemanalGestionObraAlerta.SeIdentificaronAlertas == true && string.IsNullOrEmpty(seguimientoSemanalGestionObraAlerta.Alerta))
                return false;

            return true;
        }

        private bool ValidarRegistroCompletoSeguimientoSemanalGestionObraSocial(SeguimientoSemanalGestionObraSocial seguimientoSemanalGestionObraSocial)
        {
            if (seguimientoSemanalGestionObraSocial == null)
                return false;

            if (seguimientoSemanalGestionObraSocial.CantidadEmpleosDirectos.HasValue
               && seguimientoSemanalGestionObraSocial.CantidadEmpleosIndirectos.HasValue
               && seguimientoSemanalGestionObraSocial.CantidadTotalEmpleos.HasValue
               && seguimientoSemanalGestionObraSocial.SeRealizaronReuniones == false)
                return true;

            if (
                    !seguimientoSemanalGestionObraSocial.CantidadEmpleosDirectos.HasValue
                 || !seguimientoSemanalGestionObraSocial.CantidadEmpleosIndirectos.HasValue
                 || !seguimientoSemanalGestionObraSocial.CantidadTotalEmpleos.HasValue
                 || !seguimientoSemanalGestionObraSocial.SeRealizaronReuniones.HasValue
                 || (seguimientoSemanalGestionObraSocial.SeRealizaronReuniones == true &&
                    (string.IsNullOrEmpty(seguimientoSemanalGestionObraSocial.TemaComunidad) || string.IsNullOrEmpty(seguimientoSemanalGestionObraSocial.Conclusion)))
                )
                return false;

            return true;
        }

        #region Validar Obra Calidad

        #endregion
        private bool ValidarRegistroCompletoSeguimientoSemanalGestionObraSeguridadSalud(SeguimientoSemanalGestionObraSeguridadSalud seguimientoSemanalGestionObraSeguridadSalud)
        {
            if (seguimientoSemanalGestionObraSeguridadSalud == null)
                return false;

            if (seguimientoSemanalGestionObraSeguridadSalud.CantidadAccidentes > 0)
                if (seguimientoSemanalGestionObraSeguridadSalud.SeguridadSaludCausaAccidente.Count() == 0)
                    return false;

            if (!seguimientoSemanalGestionObraSeguridadSalud.CantidadAccidentes.HasValue

                || !seguimientoSemanalGestionObraSeguridadSalud.SeRealizoCapacitacion.HasValue
                || !seguimientoSemanalGestionObraSeguridadSalud.SeRealizoRevisionElementosProteccion.HasValue
                || !seguimientoSemanalGestionObraSeguridadSalud.SeRealizoRevisionSenalizacion.HasValue
                )
                return false;

            if ((seguimientoSemanalGestionObraSeguridadSalud.SeRealizoCapacitacion == true && string.IsNullOrEmpty(seguimientoSemanalGestionObraSeguridadSalud.TemaCapacitacion))
                || (seguimientoSemanalGestionObraSeguridadSalud.SeRealizoRevisionElementosProteccion == true && !seguimientoSemanalGestionObraSeguridadSalud.CumpleRevisionElementosProyeccion.HasValue)
                || (seguimientoSemanalGestionObraSeguridadSalud.SeRealizoRevisionSenalizacion == true && !seguimientoSemanalGestionObraSeguridadSalud.CumpleRevisionSenalizacion.HasValue)
               )
                return false;

            return true;
        }

        private bool ValidarRegistroCompletoGestionObraCalidadEnsayoLaboratorio(GestionObraCalidadEnsayoLaboratorio gestionObraCalidadEnsayoLaboratorio)
        {
            if (gestionObraCalidadEnsayoLaboratorio == null)
                return false;

            if (
                 string.IsNullOrEmpty(gestionObraCalidadEnsayoLaboratorio.TipoEnsayoCodigo)
              || string.IsNullOrEmpty(gestionObraCalidadEnsayoLaboratorio.NumeroMuestras.ToString())
              || !gestionObraCalidadEnsayoLaboratorio.FechaTomaMuestras.HasValue
              || !gestionObraCalidadEnsayoLaboratorio.FechaEntregaResultados.HasValue
              || !gestionObraCalidadEnsayoLaboratorio.RealizoControlMedicion.HasValue
                )
                return false;
            return true;
        }

        private bool ValidarRegistroCompletoSeguimientoSemanalGestionObraCalidad(SeguimientoSemanalGestionObraCalidad seguimientoSemanalGestionObraCalidad)
        {
            if (seguimientoSemanalGestionObraCalidad == null || !seguimientoSemanalGestionObraCalidad.SeRealizaronEnsayosLaboratorio.HasValue)
                return false;

            if (seguimientoSemanalGestionObraCalidad.SeRealizaronEnsayosLaboratorio == false) 
                return true;

            if (seguimientoSemanalGestionObraCalidad.GestionObraCalidadEnsayoLaboratorio.Count() == 0)
                return false;
             
            foreach (var GestionObraCalidadEnsayoLaboratorio in seguimientoSemanalGestionObraCalidad.GestionObraCalidadEnsayoLaboratorio)
            { 
                if (!ValidarRegistroCompletoGestionObraCalidadEnsayoLaboratorio(GestionObraCalidadEnsayoLaboratorio))
                    return false;
            }

           return true;
        }

        private bool ValidarRegistroCompletoSeguimientoSemanalGestionObraAmbiental(SeguimientoSemanalGestionObraAmbiental pSeguimientoSemanalGestionObraAmbiental)
        {
            if (pSeguimientoSemanalGestionObraAmbiental == null)
                return false;

            if (pSeguimientoSemanalGestionObraAmbiental.SeEjecutoGestionAmbiental == false)
                return true;

            if (!pSeguimientoSemanalGestionObraAmbiental.TieneManejoMaterialesInsumo.HasValue
                && !pSeguimientoSemanalGestionObraAmbiental.TieneManejoResiduosConstruccionDemolicion.HasValue
                && !pSeguimientoSemanalGestionObraAmbiental.TieneManejoResiduosPeligrososEspeciales.HasValue
                && !pSeguimientoSemanalGestionObraAmbiental.TieneManejoOtro.HasValue)
                return false;

            if (pSeguimientoSemanalGestionObraAmbiental.TieneManejoMaterialesInsumo == true)
                if (ValidarRegistroCompletoManejoMaterialesInsumo(pSeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo) == false)
                    return false;

            if (pSeguimientoSemanalGestionObraAmbiental.TieneManejoResiduosConstruccionDemolicion == true)
                if (ValidarRegistroCompletoManejoResiduosConstruccionDemolicion(pSeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion) == false)
                    return false;

            if (pSeguimientoSemanalGestionObraAmbiental.TieneManejoResiduosPeligrososEspeciales == true)
                if (ValidarRegistroCompletoManejoResiduosPeligrososEspeciales(pSeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales) == false)
                    return false;

            if (pSeguimientoSemanalGestionObraAmbiental.TieneManejoOtro == true)
                if (ValidarRegistroCompletoManejoOtro(pSeguimientoSemanalGestionObraAmbiental.ManejoOtro) == false)
                    return false;

            return true;
        }

        private bool ValidarRegistroCompletoManejoMaterialesInsumo(ManejoMaterialesInsumos pManejoMaterialesInsumo)
        {
            if (pManejoMaterialesInsumo == null)
                return false;
            if (!pManejoMaterialesInsumo.EstanProtegidosDemarcadosMateriales.HasValue
                || !pManejoMaterialesInsumo.RequiereObservacion.HasValue
                || (pManejoMaterialesInsumo.RequiereObservacion == true && string.IsNullOrEmpty(pManejoMaterialesInsumo.Observacion))
                )
                return false;

            foreach (var ManejoMaterialesInsumosProveedor in pManejoMaterialesInsumo.ManejoMaterialesInsumosProveedor)
            {
                if (ValidarRegistroCompletoManejoMaterialesInsumosProveedor(ManejoMaterialesInsumosProveedor) == false)
                    return false;
            }
            return true;
        }

        private bool ValidarRegistroCompletoManejoMaterialesInsumosProveedor(ManejoMaterialesInsumosProveedor pManejoMaterialesInsumosProveedor)
        {
            if (pManejoMaterialesInsumosProveedor == null)
                return false;

            if (string.IsNullOrEmpty(pManejoMaterialesInsumosProveedor.Proveedor)
                || !pManejoMaterialesInsumosProveedor.RequierePermisosAmbientalesMineros.HasValue)
                return false;
            return true;
        }

        private bool ValidarRegistroCompletoManejoResiduosConstruccionDemolicion(ManejoResiduosConstruccionDemolicion pManejoResiduosConstruccionDemolicion)
        {
            if (pManejoResiduosConstruccionDemolicion == null)
                return false;

            if (!pManejoResiduosConstruccionDemolicion.EstaCuantificadoRcd.HasValue
                   || !pManejoResiduosConstruccionDemolicion.RequiereObservacion.HasValue
                   || (pManejoResiduosConstruccionDemolicion.RequiereObservacion == true && string.IsNullOrEmpty(pManejoResiduosConstruccionDemolicion.Observacion)))
                return false;

            foreach (var ManejoResiduosConstruccionDemolicionGestor in pManejoResiduosConstruccionDemolicion.ManejoResiduosConstruccionDemolicionGestor)
            {
                if (ValidarRegistroCompletoManejoResiduosConstruccionDemolicionGestor(ManejoResiduosConstruccionDemolicionGestor) == false)
                    return false;
            }

            return true;
        }

        private bool ValidarRegistroCompletoManejoResiduosConstruccionDemolicionGestor(ManejoResiduosConstruccionDemolicionGestor pManejoResiduosConstruccionDemolicionGestor)
        {
            if (pManejoResiduosConstruccionDemolicionGestor == null)
                return false;

            if (string.IsNullOrEmpty(pManejoResiduosConstruccionDemolicionGestor.NombreGestorResiduos)
                || !pManejoResiduosConstruccionDemolicionGestor.TienePermisoAmbiental.HasValue)
                return false;
            return true;
        }

        private bool ValidarRegistroCompletoManejoResiduosPeligrososEspeciales(ManejoResiduosPeligrososEspeciales pManejoResiduosPeligrososEspeciales)
        {
            if (pManejoResiduosPeligrososEspeciales == null)
                return false;

            if (
                !pManejoResiduosPeligrososEspeciales.EstanClasificados.HasValue
                || !pManejoResiduosPeligrososEspeciales.RequiereObservacion.HasValue
                || pManejoResiduosPeligrososEspeciales.RequiereObservacion.HasValue && (bool)pManejoResiduosPeligrososEspeciales.RequiereObservacion && string.IsNullOrEmpty(pManejoResiduosPeligrososEspeciales.Observacion)
               )
                return false;
            return true;
        }

        private bool ValidarRegistroCompletoManejoOtro(ManejoOtro pManejoOtro)
        {
            if (pManejoOtro == null)
                return false;

            if (
                !pManejoOtro.FechaActividad.HasValue
                || string.IsNullOrEmpty(pManejoOtro.Actividad)
                )
                return false;

            return true;
        }

        #endregion

        #endregion

        #region Notificaciones Alertas 
        private string ReplaceVariablesSeguimientoSemanal(string template, int pSeguimientoSemanalId)
        {
            List<Dominio> ListTipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion && r.Activo == true).ToList();

            SeguimientoSemanal seguimientoSemanal = _context.SeguimientoSemanal
                .Where(ss => ss.SeguimientoSemanalId == pSeguimientoSemanalId)
                .Include(cp => cp.ContratacionProyecto).ThenInclude(c => c.Contratacion).ThenInclude(ctr => ctr.Contrato)
                .Include(cp => cp.ContratacionProyecto).ThenInclude(p => p.Proyecto).ThenInclude(id => id.InstitucionEducativa)
                .Include(cp => cp.ContratacionProyecto).ThenInclude(p => p.Proyecto).ThenInclude(id => id.Sede).AsNoTracking().FirstOrDefault();

            template = template
                      .Replace("[LLAVE_MEN]", seguimientoSemanal.ContratacionProyecto.Proyecto.LlaveMen)
                      .Replace("[NUMERO_CONTRATO]", seguimientoSemanal.ContratacionProyecto.Contratacion.Contrato.FirstOrDefault().NumeroContrato)
                      .Replace("[INTITUCION_EDUCATIVA]", seguimientoSemanal.ContratacionProyecto.Proyecto.InstitucionEducativa.Nombre)
                      .Replace("[SEDE]", seguimientoSemanal.ContratacionProyecto.Proyecto.Sede.Nombre)
                      .Replace("[TIPO_INTERVENCION]", ListTipoIntervencion.Where(lti => lti.Codigo == seguimientoSemanal.ContratacionProyecto.Proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre)
                      .Replace("[FECHA_ULTIMO_REPORTE]", seguimientoSemanal.FechaModificacion.HasValue ? Convert.ToDateTime(seguimientoSemanal.FechaModificacion).ToString("dd/MM/yyy") : "Sin fecha de reporte")
                      .Replace("[FECHA_INICIAl]", seguimientoSemanal.FechaInicio.HasValue ? Convert.ToDateTime(seguimientoSemanal.FechaModificacion).ToString("dd/MM/yyy") : "Sin fecha inicial")
                      .Replace("[FECHA_FINAL]", seguimientoSemanal.FechaFin.HasValue ? Convert.ToDateTime(seguimientoSemanal.FechaModificacion).ToString("dd/MM/yyy") : "Sin fecha final");

            return template;
        }

        #region Correos 
        /// <summary> 4.1.12
        /// Envio de correo cuando envian un seguimiento semanal a validación
        /// </summary> 
        private async Task<bool> SendEmailWhenCompleteWeeklyProgress(int pSeguimientoSemanalId)
        {
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.Seguimiento_Semanal_Completo));
            string StrContenido = ReplaceVariablesSeguimientoSemanal(template.Contenido, pSeguimientoSemanalId);

            List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                          {
                                                EnumeratorPerfil.Apoyo
                                          };

            return _commonService.EnviarCorreo(perfilsEnviarCorreo, StrContenido, template.Asunto);
        }

        /// <summary> 4.1.12
        /// Si ha pasado una semana sin que se haya realizado el registro de seguimiento 
        /// semanal deberá enviarse una notificación de alerta al usuario.
        /// </summary> 
        public async Task SendEmailWhenNoWeeklyProgress()
        {
            DateTime dateTimeOneWeeklyOverdue = await _commonService.CalculardiasLaborales(5, DateTime.Now);

            List<SeguimientoSemanal> ListSeguimientoSemanal =
                _context.SeguimientoSemanal
                                        .Where(
                                                 r => r.RegistroCompleto != true
                                                 && r.FechaFin > dateTimeOneWeeklyOverdue
                                               ).OrderByDescending(r => r.SeguimientoSemanalId).ToList();

            List<EnumeratorPerfil> perfilsEnviarCorreo =
                              new List<EnumeratorPerfil>{
                                                          EnumeratorPerfil.Interventor
                                                        };

            Template templatePlaceHolder = _context.Template.Find((int)(enumeratorTemplate.Sin_Seguimiento_Semanal_X_Una_Semana));

            ListSeguimientoSemanal.ForEach(ss =>
              {
                  Template templateReplace = new Template
                  {
                      Asunto = templatePlaceHolder.Asunto + " # " + ss.NumeroSemana,
                      Contenido = ReplaceVariablesSeguimientoSemanal(templatePlaceHolder.Contenido, ss.SeguimientoSemanalId)
                  };
                  _commonService.EnviarCorreo(perfilsEnviarCorreo, templateReplace.Contenido, templateReplace.Asunto);
              });
        }

        /// <summary> 4.1.20
        /// Envio de correo cuando enviar a supervisor
        /// </summary>
        private async Task<bool> SendEmailToAproved(int pSeguimientoSemanalId)
        {

            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.Enviar_Supervisor_4_1_20));
            string strContenido = ReplaceVariablesSeguimientoSemanal(template.Contenido, pSeguimientoSemanalId);

            List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                          {
                                                EnumeratorPerfil.Supervisor
                                          };
            return _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto);

        }

        /// <summary> 4.1.20
        /// Si han pasado un (1) día hábil desde el registro del seguimiento 
        /// semanal por parte del interventor y el apoyo a la supervisión no
        /// ha validado el seguimiento, el sistema deberá remitir una notificación 
        /// de alerta al apoyo a la supervisión y al supervisor.
        /// </summary>
        public async Task SendEmailWhenNoWeeklyValidate()
        {
            DateTime dateTimeOneWeeklyOverdue = await _commonService.CalculardiasLaborales(1, DateTime.Now);

            List<SeguimientoSemanal> ListSeguimientoSemanal =
                _context.SeguimientoSemanal
                                            .Where(
                                                     r => r.RegistroCompleto == true
                                                     && r.FechaRegistroCompletoInterventor.HasValue
                                                     && r.RegistroCompletoVerificar == false
                                                     && r.FechaRegistroCompletoInterventor > dateTimeOneWeeklyOverdue
                                                   )
                                            .OrderByDescending(r => r.SeguimientoSemanalId)
                                            .ToList();

            List<EnumeratorPerfil> perfilsEnviarCorreo =
                              new List<EnumeratorPerfil>{
                                                          EnumeratorPerfil.Apoyo,
                                                          EnumeratorPerfil.Supervisor,
                                                        };

            Template templatePlaceHolder = _context.Template.Find((int)(enumeratorTemplate.Alerta_4_1_20));

            ListSeguimientoSemanal.ForEach(ss =>
            {
                Template templateReplace = new Template
                {
                    Asunto = templatePlaceHolder.Asunto + " # " + ss.NumeroSemana,
                    Contenido = ReplaceVariablesSeguimientoSemanal(templatePlaceHolder.Contenido, ss.SeguimientoSemanalId)
                };
                _commonService.EnviarCorreo(perfilsEnviarCorreo, templateReplace.Contenido, templateReplace.Asunto);
            });
        }

        /// <summary> 4.1.1
        ///Si han pasado un (1) día hábil desde la validación del seguimiento 
        ///semanal por parte del apoyo a la supervisión y el supervisor no ha
        ///enviado la retroalimentación, el sistema deberá remitir una 
        ///notificación de alerta al apoyo a la supervisión y al supervisor.
        /// </summary>
        public async Task SendEmailWhenNoWeeklyAproved()
        {
            DateTime dateTimeOneWeeklyOverdue = await _commonService.CalculardiasLaborales(1, DateTime.Now);

            List<SeguimientoSemanal> ListSeguimientoSemanal =
                _context.SeguimientoSemanal
                                        .Where(
                                                    r => r.RegistroCompletoVerificar == true
                                                 && r.FechaModificacionVerificar > dateTimeOneWeeklyOverdue
                                                 && r.RegistroCompletoAvalar != true
                                               )
                                                .OrderByDescending(r => r.SeguimientoSemanalId).ToList();

            List<EnumeratorPerfil> perfilsEnviarCorreo =
                              new List<EnumeratorPerfil>{
                                                          EnumeratorPerfil.Apoyo,
                                                          EnumeratorPerfil.Supervisor
                                                        };

            Template templatePlaceHolder = _context.Template.Find((int)(enumeratorTemplate.Alerta_4_1_21));

            ListSeguimientoSemanal.ForEach(ss =>
            {
                Template templateReplace = new Template
                {
                    Asunto = templatePlaceHolder.Asunto + " # " + ss.NumeroSemana,
                    Contenido = ReplaceVariablesSeguimientoSemanal(templatePlaceHolder.Contenido, ss.SeguimientoSemanalId)
                };
                _commonService.EnviarCorreo(perfilsEnviarCorreo, templateReplace.Contenido, templateReplace.Asunto);
            });
        }
        #endregion

        #endregion


    }
}
