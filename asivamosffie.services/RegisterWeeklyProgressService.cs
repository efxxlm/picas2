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

namespace asivamosffie.services
{
    public class RegisterWeeklyProgressService : IRegisterWeeklyProgressService
    {
        #region constructor

        private readonly ICommonService _commonService;
        private readonly IDocumentService _documentService;
        private readonly devAsiVamosFFIEContext _context;

        public RegisterWeeklyProgressService(devAsiVamosFFIEContext context, ICommonService commonService, IDocumentService documentService)
        {
            _documentService = documentService;
            _commonService = commonService;
            _context = context;
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
            seguimientoSemanal.CantidadTotalDiasActividades = ListProgramacion.Sum(r => r.Duracion);

            seguimientoSemanal.AvanceAcumulado = ListProgramacion
                .GroupBy(r => r.Actividad)
                .Select(r => new
                {
                    Actividad = r.Key,
                    AvanceAcumulado = Math.Truncate((decimal)r.Sum(r => r.SeguimientoSemanalAvanceFisicoProgramacion.FirstOrDefault().AvanceFisicoCapitulo)) + "%",
                    AvanceFisicoCapitulo = Math.Truncate((((decimal)r.Sum(r => r.Duracion) / seguimientoSemanal.CantidadTotalDiasActividades) * 100)) + "%"
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

                    ListBitaCora.Add(new
                    {
                        UltimoReporte = item.FechaModificacion,
                        item.SeguimientoSemanalId,
                        RegistroCompletoMuestras = item.RegistroCompletoMuestras.HasValue ? item.RegistroCompletoMuestras : false,
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
                    SaveUpdateAvanceFinanciero(pSeguimientoSemanal.SeguimientoSemanalAvanceFinanciero.FirstOrDefault(), pSeguimientoSemanal.UsuarioCreacion);

                if (pSeguimientoSemanal.SeguimientoSemanalGestionObra.Count() > 0)
                    SaveUpdateGestionObra(pSeguimientoSemanal.SeguimientoSemanalGestionObra.FirstOrDefault(), pSeguimientoSemanal.UsuarioCreacion);

                if (pSeguimientoSemanal.SeguimientoSemanalReporteActividad.Count() > 0)
                    SaveUpdateReporteActividades(pSeguimientoSemanal.SeguimientoSemanalReporteActividad.FirstOrDefault(), pSeguimientoSemanal.UsuarioCreacion);

                if (pSeguimientoSemanal.SeguimientoSemanalRegistroFotografico.Count() > 0)
                    SaveUpdateRegistroFotografico(pSeguimientoSemanal.SeguimientoSemanalRegistroFotografico.FirstOrDefault(), pSeguimientoSemanal.UsuarioCreacion);

                if (pSeguimientoSemanal.SeguimientoSemanalRegistrarComiteObra.Count() > 0)
                    SaveUpdateComiteObra(pSeguimientoSemanal.SeguimientoSemanalRegistrarComiteObra.FirstOrDefault(), pSeguimientoSemanal.UsuarioCreacion);

                await _context.SaveChangesAsync();

                SeguimientoSemanal seguimientoSemanalMod = await _context.SeguimientoSemanal.FindAsync(pSeguimientoSemanal.SeguimientoSemanalId);
                seguimientoSemanalMod.UsuarioModificacion = pSeguimientoSemanal.UsuarioCreacion;
                seguimientoSemanalMod.FechaModificacion = DateTime.Now;

                if (ValidarRegistroCompletoSeguimientoSemanal(seguimientoSemanalMod))
                    seguimientoSemanalMod.FechaEnvioSupervisor = DateTime.Now;
                else
                    seguimientoSemanalMod.FechaEnvioSupervisor = null;

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
                    Code = ConstantSesionComiteTecnico.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, ConstanMessagesRegisterWeeklyProgress.OperacionExitosa, idAccion, pUsuarioModificacion, "Eliminar Gestion Obra Calidad Ensayo Laboratorio".ToUpper())
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
                    Code = ConstanMessagesRegisterWeeklyProgress.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, ConstanMessagesRegisterWeeklyProgress.OperacionExitosa, idAccion, pUsuarioModificacion, "Eliminar Manejo Materiales Insumo Proveedor".ToUpper())
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
                    Code = ConstanMessagesRegisterWeeklyProgress.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, ConstanMessagesRegisterWeeklyProgress.OperacionExitosa, idAccion, pUsuarioModificacion, "Eliminar Manejo Materiales Insumo Proveedor".ToUpper())
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
                    SeguimientoSemanalAvanceFisicoProgramacion itemOld = _context.SeguimientoSemanalAvanceFisicoProgramacion.Find(item.SeguimientoSemanalAvanceFisicoProgramacionId);

                    itemOld.UsuarioModificacion = strUsuario;
                    itemOld.FechaModificacion = DateTime.Now;
                    itemOld.AvanceFisicoCapitulo = item.AvanceFisicoCapitulo;
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

                        //Manejo Materiales e Insumos
                        if (SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo != null)
                        {
                            //New
                            if (SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.ManejoMaterialesInsumosId == 0)
                            {

                                SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.UsuarioCreacion = pUsuarioCreacion;
                                SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.Eliminado = false;
                                SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.FechaCreacion = DateTime.Now;
                                SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.RegistroCompleto = ValidarRegistroCompletoManejoMaterialesInsumo(SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo);

                                _context.ManejoMaterialesInsumos.Add(SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo);
                                _context.SaveChanges();
                                SeguimientoSemanalGestionObraAmbientalOld.ManejoMaterialesInsumoId = SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.ManejoMaterialesInsumosId;
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
                            if (SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.ManejoResiduosConstruccionDemolicionId == 0)
                            {
                                SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.UsuarioCreacion = pUsuarioCreacion;
                                SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.Eliminado = false;
                                SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.FechaCreacion = DateTime.Now;
                                SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.RegistroCompleto = ValidarRegistroCompletoManejoResiduosConstruccionDemolicion(SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion);

                                _context.ManejoResiduosConstruccionDemolicion.Add(SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion);
                                _context.SaveChanges();
                                SeguimientoSemanalGestionObraAmbientalOld.ManejoResiduosConstruccionDemolicionId = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.ManejoResiduosConstruccionDemolicionId;

                            }
                            else
                            {
                                ManejoResiduosConstruccionDemolicion manejoResiduosConstruccionDemolicionOld = _context.ManejoResiduosConstruccionDemolicion.Find(SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.ManejoResiduosConstruccionDemolicionId);

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
                                    ManejoResiduosConstruccionDemolicionGestor.ManejoResiduosConstruccionDemolicionId = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.ManejoResiduosConstruccionDemolicionId;
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
                                SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.UsuarioCreacion = pUsuarioCreacion;
                                SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.Eliminado = false;
                                SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.FechaCreacion = DateTime.Now;
                                SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.RegistroCompleto = ValidarRegistroCompletoManejoResiduosPeligrososEspeciales(SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales);

                                _context.ManejoResiduosPeligrososEspeciales.Add(SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales);
                                _context.SaveChanges();

                                SeguimientoSemanalGestionObraAmbientalOld.ManejoResiduosPeligrososEspecialesId = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosPeligrososEspeciales.ManejoResiduosPeligrososEspecialesId;
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
                                SeguimientoSemanalGestionObraAmbiental.ManejoOtro.UsuarioCreacion = pUsuarioCreacion;
                                SeguimientoSemanalGestionObraAmbiental.ManejoOtro.Eliminado = false;
                                SeguimientoSemanalGestionObraAmbiental.ManejoOtro.FechaCreacion = DateTime.Now;
                                SeguimientoSemanalGestionObraAmbiental.ManejoOtro.RegistroCompleto = ValidarRegistroCompletoManejoOtro(SeguimientoSemanalGestionObraAmbiental.ManejoOtro);

                                _context.ManejoOtro.Add(SeguimientoSemanalGestionObraAmbiental.ManejoOtro);
                                _context.SaveChanges();
                                SeguimientoSemanalGestionObraAmbientalOld.ManejoOtroId = SeguimientoSemanalGestionObraAmbiental.ManejoOtro.ManejoOtroId;
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

                        if (SeguimientoSemanalGestionObraCalidad.SeRealizaronEnsayosLaboratorio != false)
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
                //Financiero solo se valida cada 5 semanas 
                if (pSeguimientoSemanal.NumeroSemana % 5 == 0)
                {
                    if (pSeguimientoSemanal?.SeguimientoSemanalAvanceFinanciero.Count() == 0)
                        return false;
                    if (pSeguimientoSemanal?.SeguimientoSemanalAvanceFinanciero?.FirstOrDefault().RegistroCompleto == false)
                        return false;
                }

                if (pSeguimientoSemanal?.SeguimientoSemanalAvanceFisico.Count() == 0)
                    return false;
                if (pSeguimientoSemanal?.SeguimientoSemanalAvanceFisico?.FirstOrDefault().RegistroCompleto == false)
                    return false;

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
            if (!seguimientoSemanalGestionObraAlerta.SeIdentificaronAlertas.HasValue)
                return false;

            if (seguimientoSemanalGestionObraAlerta.SeIdentificaronAlertas == true && string.IsNullOrEmpty(seguimientoSemanalGestionObraAlerta.Alerta))
                return false;

            return true;
        }

        private bool ValidarRegistroCompletoSeguimientoSemanalGestionObraSocial(SeguimientoSemanalGestionObraSocial seguimientoSemanalGestionObraSocial)
        {
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
            if (seguimientoSemanalGestionObraSeguridadSalud.CantidadAccidentes == 0)
                return true;

            if (!seguimientoSemanalGestionObraSeguridadSalud.CantidadAccidentes.HasValue
                || seguimientoSemanalGestionObraSeguridadSalud.SeguridadSaludCausaAccidente.Count() == 0
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
            if (
                 string.IsNullOrEmpty(gestionObraCalidadEnsayoLaboratorio.TipoEnsayoCodigo)
              || string.IsNullOrEmpty(gestionObraCalidadEnsayoLaboratorio.NumeroMuestras.ToString())
              || !gestionObraCalidadEnsayoLaboratorio.FechaTomaMuestras.HasValue
              || !gestionObraCalidadEnsayoLaboratorio.FechaEntregaResultados.HasValue
              || !gestionObraCalidadEnsayoLaboratorio.RealizoControlMedicion.HasValue
                )
            { return false; }
            return true;
        }

        private bool ValidarRegistroCompletoSeguimientoSemanalGestionObraCalidad(SeguimientoSemanalGestionObraCalidad seguimientoSemanalGestionObraCalidad)
        {

            if (seguimientoSemanalGestionObraCalidad.SeRealizaronEnsayosLaboratorio == false)
                return true;

            if (!seguimientoSemanalGestionObraCalidad.SeRealizaronEnsayosLaboratorio.HasValue
                || seguimientoSemanalGestionObraCalidad.GestionObraCalidadEnsayoLaboratorio.Count() == 0)
                return false;

            foreach (var GestionObraCalidadEnsayoLaboratorio in seguimientoSemanalGestionObraCalidad.GestionObraCalidadEnsayoLaboratorio)
            {
                if (!GestionObraCalidadEnsayoLaboratorio.RegistroCompleto.HasValue || !(bool)GestionObraCalidadEnsayoLaboratorio.RegistroCompleto)
                    return false;
            }

            return true;
        }

        private bool ValidarRegistroCompletoSeguimientoSemanalGestionObraAmbiental(SeguimientoSemanalGestionObraAmbiental pSeguimientoSemanalGestionObraAmbiental)
        {
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
                || (pManejoMaterialesInsumo.RequiereObservacion.HasValue && string.IsNullOrEmpty(pManejoMaterialesInsumo.Observacion))
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
            if (string.IsNullOrEmpty(pManejoMaterialesInsumosProveedor.Proveedor)
                || !pManejoMaterialesInsumosProveedor.RequierePermisosAmbientalesMineros.HasValue)
                return false;
            return true;
        }

        private bool ValidarRegistroCompletoManejoResiduosConstruccionDemolicion(ManejoResiduosConstruccionDemolicion pManejoResiduosConstruccionDemolicion)
        {
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
            if (string.IsNullOrEmpty(pManejoResiduosConstruccionDemolicionGestor.NombreGestorResiduos)
                || !pManejoResiduosConstruccionDemolicionGestor.TienePermisoAmbiental.HasValue)
                return false;
            return true;
        }

        private bool ValidarRegistroCompletoManejoResiduosPeligrososEspeciales(ManejoResiduosPeligrososEspeciales pManejoResiduosPeligrososEspeciales)
        {
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


        public void SendSeguimientoSemanalApoyoSupervision()
        {

        }

        #endregion
    }
}
