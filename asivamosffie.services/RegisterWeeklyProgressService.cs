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

namespace asivamosffie.services
{
    public class RegisterWeeklyProgressService : IRegisterWeeklyProgressService
    {
        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;

        public RegisterWeeklyProgressService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;
        }

        #region Get
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
            return await _context.VRegistrarAvanceSemanal.ToListAsync();
        }

        public async Task<SeguimientoSemanal> GetLastSeguimientoSemanalByContratacionProyectoIdOrSeguimientoSemanalId(int pContratacionProyectoId, int pSeguimientoSemanalId)
        {
            List<Dominio> TipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();
            List<InstitucionEducativaSede> ListInstitucionEducativaSede = _context.InstitucionEducativaSede.ToList();
            List<Localizacion> ListLocalizacion = _context.Localizacion.ToList();
            try
            {
                if (pContratacionProyectoId > 0)
                {
                    SeguimientoSemanal seguimientoSemanal = await _context.SeguimientoSemanal.Where(r => r.ContratacionProyectoId == pContratacionProyectoId && !(bool)r.Eliminado && !(bool)r.RegistroCompleto)

                       .Include(r => r.ContratacionProyecto)
                          .ThenInclude(r => r.Contratacion)
                              .ThenInclude(r => r.Contrato)
                       .Include(r => r.ContratacionProyecto)
                          .ThenInclude(r => r.Proyecto)
                              .ThenInclude(r => r.InstitucionEducativa)
                       .Include(r => r.SeguimientoDiario)
                              .ThenInclude(r => r.SeguimientoDiarioObservaciones)
                       .Include(r => r.SeguimientoSemanalAvanceFinanciero)


                       .Include(r => r.SeguimientoSemanalAvanceFisico)

                       //Gestion Obra
                       //Gestion Obra Ambiental
                       .Include(r => r.SeguimientoSemanalGestionObra)
                            .ThenInclude(r => r.SeguimientoSemanalGestionObraAmbiental)
                         // Gestion Obra Calidad
                         .Include(r => r.SeguimientoSemanalGestionObra)
                            .ThenInclude(r => r.SeguimientoSemanalGestionObraCalidad)
                                .ThenInclude(r => r.GestionObraCalidadEnsayoLaboratorio)
                                    .ThenInclude(r => r.EnsayoLaboratorioMuestra)

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


                    List<Dominio> CausaBajaDisponibilidadMaterial = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Causa_Baja_Disponibilidad_Material).ToList();

                    List<Dominio> CausaBajaDisponibilidadEquipo = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Causa_Baja_Disponibilidad_Equipo).ToList();

                    List<Dominio> CausaBajaDisponibilidadProductividad = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Causa_Baja_Disponibilidad_Productividad).ToList();

                    foreach (var SeguimientoDiario in seguimientoSemanal.SeguimientoDiario)
                    {
                        SeguimientoDiario.CausaIndisponibilidadMaterialCodigo = !string.IsNullOrEmpty(SeguimientoDiario.CausaIndisponibilidadMaterialCodigo) ? CausaBajaDisponibilidadMaterial.Where(r => r.Codigo == SeguimientoDiario.CausaIndisponibilidadMaterialCodigo).FirstOrDefault().Nombre : "---";

                        SeguimientoDiario.CausaIndisponibilidadEquipoCodigo = !string.IsNullOrEmpty(SeguimientoDiario.CausaIndisponibilidadEquipoCodigo) ? CausaBajaDisponibilidadEquipo.Where(r => r.Codigo == SeguimientoDiario.CausaIndisponibilidadEquipoCodigo).FirstOrDefault().Nombre : "---";

                        SeguimientoDiario.CausaIndisponibilidadProductividadCodigo = !string.IsNullOrEmpty(SeguimientoDiario.CausaIndisponibilidadProductividadCodigo) ? CausaBajaDisponibilidadProductividad.Where(r => r.Codigo == SeguimientoDiario.CausaIndisponibilidadProductividadCodigo).FirstOrDefault().Nombre : "---";
                    }

                    //Crear Comite Obra numerador

                    seguimientoSemanal.ComiteObraGenerado = await _commonService.EnumeradorComiteObra();
                    ///incluir Gestion Obra
                    /// 
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
                            {
                                seguimientoSemanal.SeguimientoSemanalGestionObra.FirstOrDefault().SeguimientoSemanalGestionObraAmbiental.FirstOrDefault().ManejoMaterialesInsumo = _context.ManejoMaterialesInsumos.Include(r => r.ManejoMaterialesInsumosProveedor).Where(r => r.ManejoMaterialesInsumosId == ManejoMaterialesInsumoId).FirstOrDefault();
                            }

                            if (ManejoResiduosConstruccionDemolicionId != null)
                            {
                                seguimientoSemanal.SeguimientoSemanalGestionObra.FirstOrDefault().SeguimientoSemanalGestionObraAmbiental.FirstOrDefault().ManejoResiduosConstruccionDemolicion = _context.ManejoResiduosConstruccionDemolicion.Include(r => r.ManejoResiduosConstruccionDemolicionGestor).Where(r => r.ManejoResiduosConstruccionDemolicionId == ManejoResiduosConstruccionDemolicionId).FirstOrDefault();
                            }

                            if (ManejoResiduosPeligrososEspecialesId != null)
                            {
                                seguimientoSemanal.SeguimientoSemanalGestionObra.FirstOrDefault().SeguimientoSemanalGestionObraAmbiental.FirstOrDefault().ManejoResiduosPeligrososEspeciales = _context.ManejoResiduosPeligrososEspeciales.Find(ManejoResiduosPeligrososEspecialesId);
                            }

                            if (ManejoOtroId != null)
                            {
                                seguimientoSemanal.SeguimientoSemanalGestionObra.FirstOrDefault().SeguimientoSemanalGestionObraAmbiental.FirstOrDefault().ManejoOtro = _context.ManejoOtro.Find(ManejoOtroId);
                            }
                        }
                    }

                    seguimientoSemanal.FlujoInversion = _context.FlujoInversion.Include(r => r.Programacion).Where(r => r.SeguimientoSemanalId == seguimientoSemanal.SeguimientoSemanalId && r.Programacion.TipoActividadCodigo == "C").ToList();

                    foreach (var FlujoInversion in seguimientoSemanal.FlujoInversion)
                    {
                        FlujoInversion.Programacion.RangoDias = (FlujoInversion.Programacion.FechaFin - FlujoInversion.Programacion.FechaInicio).TotalDays;
                    }

                    List<dynamic> AvanceAcumulado = new List<dynamic>();
                    List<int> ListSeguimientoSemanalId = _context.SeguimientoSemanal.Where(r => r.ContratacionProyectoId == seguimientoSemanal.ContratacionProyectoId).Select(r => r.SeguimientoSemanalId).ToList();

                    List<FlujoInversion> ListProgramacion = _context.FlujoInversion
                      .Include(r => r.Programacion)
                      .Where(r => r.Programacion.TipoActividadCodigo == "C").ToList();

                    foreach (var idSeguimientoSemanal in ListSeguimientoSemanalId)
                    {
                        foreach (var item in ListProgramacion)
                        {
                            if (item.SeguimientoSemanalId == idSeguimientoSemanal)
                                AvanceAcumulado.Add(new
                                {
                                    item.Programacion.Actividad,
                                    item.Programacion.AvanceFisicoCapitulo
                                });
                        }
                    }

                    seguimientoSemanal.AvanceAcumulado = AvanceAcumulado;

                    //Eliminar del get Las tablas eliminadas Logicamente
                    foreach (var SeguimientoSemanalGestionObra in seguimientoSemanal.SeguimientoSemanalGestionObra)
                    {
                        foreach (var SeguimientoSemanalGestionObraAmbiental in SeguimientoSemanalGestionObra.SeguimientoSemanalGestionObraAmbiental)
                        {
                            //No incluir los ManejoMaterialesInsumosProveedor eliminados
                            if (SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo != null || SeguimientoSemanalGestionObraAmbiental?.ManejoMaterialesInsumo?.ManejoMaterialesInsumosProveedor.Count() > 0)
                                SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.ManejoMaterialesInsumosProveedor = SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.ManejoMaterialesInsumosProveedor.Where(r => !(bool)r.Eliminado).ToList();
                            //No incluir los Manejo Residuos Construccion Demolicion Gestor eliminados
                            if (SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion != null || SeguimientoSemanalGestionObraAmbiental?.ManejoResiduosConstruccionDemolicion?.ManejoResiduosConstruccionDemolicionGestor.Count() > 0)
                                SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.ManejoResiduosConstruccionDemolicionGestor = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.ManejoResiduosConstruccionDemolicionGestor.Where(r => !(bool)r.Eliminado).ToList();
                        }

                        foreach (var SeguimientoSemanalGestionObraCalidad in SeguimientoSemanalGestionObra.SeguimientoSemanalGestionObraCalidad)
                        {
                            if (SeguimientoSemanalGestionObraCalidad.GestionObraCalidadEnsayoLaboratorio != null || SeguimientoSemanalGestionObraCalidad?.GestionObraCalidadEnsayoLaboratorio.Count() > 0)
                                SeguimientoSemanalGestionObraCalidad.GestionObraCalidadEnsayoLaboratorio = SeguimientoSemanalGestionObraCalidad.GestionObraCalidadEnsayoLaboratorio.Where(r => !(bool)r.Eliminado).ToList();
                        }
                    }

                    Localizacion Municipio = ListLocalizacion.Where(r => r.LocalizacionId == seguimientoSemanal.ContratacionProyecto.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                    InstitucionEducativaSede Sede = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == seguimientoSemanal.ContratacionProyecto.Proyecto.SedeId).FirstOrDefault();
                    InstitucionEducativaSede institucionEducativa = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == Sede.PadreId).FirstOrDefault();

                    seguimientoSemanal.ContratacionProyecto.Proyecto.tipoIntervencionString = TipoIntervencion.Where(r => r.Codigo == seguimientoSemanal.ContratacionProyecto.Proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
                    seguimientoSemanal.ContratacionProyecto.Proyecto.MunicipioObj = Municipio;
                    seguimientoSemanal.ContratacionProyecto.Proyecto.DepartamentoObj = ListLocalizacion.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
                    seguimientoSemanal.ContratacionProyecto.Proyecto.Sede = Sede;
                    seguimientoSemanal.ContratacionProyecto.Proyecto.InstitucionEducativa = institucionEducativa;
                    return seguimientoSemanal;
                }
                else
                {
                    SeguimientoSemanal seguimientoSemanal = await _context.SeguimientoSemanal.Where(r => r.SeguimientoSemanalId == pSeguimientoSemanalId)

                       .Include(r => r.ContratacionProyecto)
                          .ThenInclude(r => r.Contratacion)
                              .ThenInclude(r => r.Contrato)
                       .Include(r => r.ContratacionProyecto)
                          .ThenInclude(r => r.Proyecto)
                              .ThenInclude(r => r.InstitucionEducativa)
                       .Include(r => r.SeguimientoDiario)
                              .ThenInclude(r => r.SeguimientoDiarioObservaciones)
                       .Include(r => r.SeguimientoSemanalAvanceFinanciero)


                       .Include(r => r.SeguimientoSemanalAvanceFisico)

                       //Gestion Obra
                       //Gestion Obra Ambiental
                       .Include(r => r.SeguimientoSemanalGestionObra)
                            .ThenInclude(r => r.SeguimientoSemanalGestionObraAmbiental)
                         // Gestion Obra Calidad
                         .Include(r => r.SeguimientoSemanalGestionObra)
                            .ThenInclude(r => r.SeguimientoSemanalGestionObraCalidad)
                                .ThenInclude(r => r.GestionObraCalidadEnsayoLaboratorio)
                                    .ThenInclude(r => r.EnsayoLaboratorioMuestra)

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

                    ///incluir Gestion Obra
                    /// 

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
                            {
                                seguimientoSemanal.SeguimientoSemanalGestionObra.FirstOrDefault().SeguimientoSemanalGestionObraAmbiental.FirstOrDefault().ManejoMaterialesInsumo = _context.ManejoMaterialesInsumos.Include(r => r.ManejoMaterialesInsumosProveedor).Where(r => r.ManejoMaterialesInsumosId == ManejoMaterialesInsumoId).FirstOrDefault();
                            }

                            if (ManejoResiduosConstruccionDemolicionId != null)
                            {
                                seguimientoSemanal.SeguimientoSemanalGestionObra.FirstOrDefault().SeguimientoSemanalGestionObraAmbiental.FirstOrDefault().ManejoResiduosConstruccionDemolicion = _context.ManejoResiduosConstruccionDemolicion.Include(r => r.ManejoResiduosConstruccionDemolicionGestor).Where(r => r.ManejoResiduosConstruccionDemolicionId == ManejoResiduosConstruccionDemolicionId).FirstOrDefault();
                            }

                            if (ManejoResiduosPeligrososEspecialesId != null)
                            {
                                seguimientoSemanal.SeguimientoSemanalGestionObra.FirstOrDefault().SeguimientoSemanalGestionObraAmbiental.FirstOrDefault().ManejoResiduosPeligrososEspeciales = _context.ManejoResiduosPeligrososEspeciales.Find(ManejoResiduosPeligrososEspecialesId);
                            }

                            if (ManejoOtroId != null)
                            {
                                seguimientoSemanal.SeguimientoSemanalGestionObra.FirstOrDefault().SeguimientoSemanalGestionObraAmbiental.FirstOrDefault().ManejoOtro = _context.ManejoOtro.Find(ManejoOtroId);
                            }
                        }
                    }



                    seguimientoSemanal.FlujoInversion = _context.FlujoInversion.Include(r => r.Programacion).Where(r => r.SeguimientoSemanalId == seguimientoSemanal.SeguimientoSemanalId && r.Programacion.TipoActividadCodigo == "C").ToList();

                    foreach (var FlujoInversion in seguimientoSemanal.FlujoInversion)
                    {
                        FlujoInversion.Programacion.RangoDias = (FlujoInversion.Programacion.FechaFin - FlujoInversion.Programacion.FechaInicio).TotalDays;
                    }

                    List<dynamic> AvanceAcumulado = new List<dynamic>();
                    List<int> ListSeguimientoSemanalId = _context.SeguimientoSemanal.Where(r => r.ContratacionProyectoId == seguimientoSemanal.ContratacionProyectoId).Select(r => r.SeguimientoSemanalId).ToList();

                    List<FlujoInversion> ListProgramacion = _context.FlujoInversion
                      .Include(r => r.Programacion)
                      .Where(r => r.Programacion.TipoActividadCodigo == "C").ToList();

                    foreach (var idSeguimientoSemanal in ListSeguimientoSemanalId)
                    {
                        foreach (var item in ListProgramacion)
                        {
                            if (item.SeguimientoSemanalId == idSeguimientoSemanal)
                                AvanceAcumulado.Add(new
                                {
                                    item.Programacion.Actividad,
                                    item.Programacion.AvanceFisicoCapitulo
                                });
                        }
                    }

                    seguimientoSemanal.AvanceAcumulado = AvanceAcumulado;

                    //Eliminar del get Las tablas eliminadas Logicamente
                    foreach (var SeguimientoSemanalGestionObra in seguimientoSemanal.SeguimientoSemanalGestionObra)
                    {
                        foreach (var SeguimientoSemanalGestionObraAmbiental in SeguimientoSemanalGestionObra.SeguimientoSemanalGestionObraAmbiental)
                        {
                            //No incluir los ManejoMaterialesInsumosProveedor eliminados
                            if (SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo != null || SeguimientoSemanalGestionObraAmbiental?.ManejoMaterialesInsumo?.ManejoMaterialesInsumosProveedor.Count() > 0)
                                SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.ManejoMaterialesInsumosProveedor = SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.ManejoMaterialesInsumosProveedor.Where(r => !(bool)r.Eliminado).ToList();
                            //No incluir los Manejo Residuos Construccion Demolicion Gestor eliminados
                            if (SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion != null || SeguimientoSemanalGestionObraAmbiental?.ManejoResiduosConstruccionDemolicion?.ManejoResiduosConstruccionDemolicionGestor.Count() > 0)
                                SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.ManejoResiduosConstruccionDemolicionGestor = SeguimientoSemanalGestionObraAmbiental.ManejoResiduosConstruccionDemolicion.ManejoResiduosConstruccionDemolicionGestor.Where(r => !(bool)r.Eliminado).ToList();
                        }

                        foreach (var SeguimientoSemanalGestionObraCalidad in SeguimientoSemanalGestionObra.SeguimientoSemanalGestionObraCalidad)
                        {
                            if (SeguimientoSemanalGestionObraCalidad.GestionObraCalidadEnsayoLaboratorio != null || SeguimientoSemanalGestionObraCalidad?.GestionObraCalidadEnsayoLaboratorio.Count() > 0)
                                SeguimientoSemanalGestionObraCalidad.GestionObraCalidadEnsayoLaboratorio = SeguimientoSemanalGestionObraCalidad.GestionObraCalidadEnsayoLaboratorio.Where(r => !(bool)r.Eliminado).ToList();
                        }
                    }

                    Localizacion Municipio = ListLocalizacion.Where(r => r.LocalizacionId == seguimientoSemanal.ContratacionProyecto.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                    InstitucionEducativaSede Sede = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == seguimientoSemanal.ContratacionProyecto.Proyecto.SedeId).FirstOrDefault();
                    InstitucionEducativaSede institucionEducativa = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == Sede.PadreId).FirstOrDefault();

                    seguimientoSemanal.ContratacionProyecto.Proyecto.tipoIntervencionString = TipoIntervencion.Where(r => r.Codigo == seguimientoSemanal.ContratacionProyecto.Proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
                    seguimientoSemanal.ContratacionProyecto.Proyecto.MunicipioObj = Municipio;
                    seguimientoSemanal.ContratacionProyecto.Proyecto.DepartamentoObj = ListLocalizacion.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
                    seguimientoSemanal.ContratacionProyecto.Proyecto.Sede = Sede;
                    seguimientoSemanal.ContratacionProyecto.Proyecto.InstitucionEducativa = institucionEducativa;
                    return seguimientoSemanal;
                }
            }
            catch (Exception ex)
            {
                return new SeguimientoSemanal();
            }

        }

        public async Task<List<dynamic>> GetListSeguimientoSemanalByContratacionProyectoId(int pContratacionProyectoId)
        {
            List<SeguimientoSemanal> ListseguimientoSemanal = await _context.SeguimientoSemanal.Where(r => r.ContratacionProyectoId == pContratacionProyectoId && r.RegistroCompleto == true)
                .Include(r => r.ContratacionProyecto)
                   .ThenInclude(r => r.Proyecto)
                .Include(r => r.ContratacionProyecto)
                   .ThenInclude(r => r.Contratacion)
                       .ThenInclude(r => r.Contrato)
                .ToListAsync();

            List<dynamic> ListBitaCora = new List<dynamic>();

            List<Dominio> TipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();
            List<InstitucionEducativaSede> ListInstitucionEducativaSede = _context.InstitucionEducativaSede.ToList();

            foreach (var item in ListseguimientoSemanal)
            {
                InstitucionEducativaSede Sede = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == item.ContratacionProyecto?.Proyecto?.SedeId).FirstOrDefault();
                InstitucionEducativaSede institucionEducativa = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == Sede.PadreId).FirstOrDefault();

                ListBitaCora.Add(new
                {
                    item.ContratacionProyecto?.Proyecto?.LlaveMen,
                    item.ContratacionProyecto?.Contratacion?.Contrato?.FirstOrDefault().NumeroContrato,
                    TipoIntervencion = !string.IsNullOrEmpty(item.ContratacionProyecto?.Proyecto.TipoIntervencionCodigo) ? TipoIntervencion.Where(r => r.Codigo == item.ContratacionProyecto?.Proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre : " ",
                    institucionEducativa.Nombre,
                    Sede = Sede.Nombre,
                    item.FechaModificacion,
                    EstadoObra = item.EstadoObraCodigo
                });


            }
            return ListBitaCora;
        }
        #endregion

        #region Save Edit
        public async Task<Respuesta> CreateEditEnsayoLaboratorioMuestra(GestionObraCalidadEnsayoLaboratorio pGestionObraCalidadEnsayoLaboratorio)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Ensayo_Laboratorio_Muestra, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                foreach (var EnsayoLaboratorioMuestra in pGestionObraCalidadEnsayoLaboratorio.EnsayoLaboratorioMuestra)
                {
                    if (EnsayoLaboratorioMuestra.EnsayoLaboratorioMuestraId == 0)
                    {
                        EnsayoLaboratorioMuestra.UsuarioCreacion = pGestionObraCalidadEnsayoLaboratorio.UsuarioCreacion;
                        EnsayoLaboratorioMuestra.FechaCreacion = DateTime.Now;
                        EnsayoLaboratorioMuestra.Eliminado = false;

                        EnsayoLaboratorioMuestra.RegistroCompleto = !string.IsNullOrEmpty(EnsayoLaboratorioMuestra.NombreMuestra)
                            && !string.IsNullOrEmpty(EnsayoLaboratorioMuestra.Observacion)
                            && EnsayoLaboratorioMuestra.FechaEntregaResultado.HasValue
                            ? true : false;
                        _context.EnsayoLaboratorioMuestra.Add(EnsayoLaboratorioMuestra);

                    }
                    else
                    {
                        EnsayoLaboratorioMuestra EnsayoLaboratorioMuestraOld = _context.EnsayoLaboratorioMuestra.Find(EnsayoLaboratorioMuestra.EnsayoLaboratorioMuestraId);
                        EnsayoLaboratorioMuestraOld.FechaEntregaResultado = EnsayoLaboratorioMuestra.FechaEntregaResultado;
                        EnsayoLaboratorioMuestraOld.NombreMuestra = EnsayoLaboratorioMuestra.NombreMuestra;
                        EnsayoLaboratorioMuestraOld.Observacion = EnsayoLaboratorioMuestra.Observacion;
                        EnsayoLaboratorioMuestraOld.RegistroCompleto =
                               !string.IsNullOrEmpty(EnsayoLaboratorioMuestra.NombreMuestra)
                            && !string.IsNullOrEmpty(EnsayoLaboratorioMuestra.Observacion)
                            && EnsayoLaboratorioMuestra.FechaEntregaResultado.HasValue
                            ? true : false;
                        ;
                        EnsayoLaboratorioMuestraOld.UsuarioModificacion = pGestionObraCalidadEnsayoLaboratorio.UsuarioCreacion;
                        EnsayoLaboratorioMuestraOld.FechaModificacion = DateTime.Now;
                    }
                }
                GestionObraCalidadEnsayoLaboratorio gestionObraCalidadEnsayoLaboratorioOld = _context.GestionObraCalidadEnsayoLaboratorio.Find(pGestionObraCalidadEnsayoLaboratorio.GestionObraCalidadEnsayoLaboratorioId);
                gestionObraCalidadEnsayoLaboratorioOld.RegistroCompletoMuestras = ValidarRegistroCompletoMuestasLaboratorio(pGestionObraCalidadEnsayoLaboratorio);
                gestionObraCalidadEnsayoLaboratorioOld.UsuarioModificacion = pGestionObraCalidadEnsayoLaboratorio.UsuarioCreacion;
                gestionObraCalidadEnsayoLaboratorioOld.FechaModificacion = DateTime.Now;

                _context.SaveChanges();


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

        public async Task<Respuesta> SaveUpdateSeguimientoSemanal(SeguimientoSemanal pSeguimientoSemanal)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Seguimiento_Semanal, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                SeguimientoSemanal seguimientoSemanalMod = await _context.SeguimientoSemanal.FindAsync(pSeguimientoSemanal.SeguimientoSemanalId);
                seguimientoSemanalMod.UsuarioModificacion = pSeguimientoSemanal.UsuarioCreacion;
                seguimientoSemanalMod.FechaModificacion = DateTime.Now;

                if (pSeguimientoSemanal.SeguimientoSemanalAvanceFisico.Count() > 0)
                    SaveUpdateAvanceFisico(pSeguimientoSemanal.SeguimientoSemanalAvanceFisico.FirstOrDefault(), pSeguimientoSemanal.FlujoInversion, pSeguimientoSemanal.UsuarioCreacion);

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

        private bool ValidarRegistroCompletoSeguimientoSemanal(SeguimientoSemanal seguimientoSemanalRetorno)
        {
            return false;
        }

        private void SaveUpdateAvanceFisico(SeguimientoSemanalAvanceFisico pSeguimientoSemanalAvanceFisico, ICollection<FlujoInversion> pListFlujoInversion, string usuarioCreacion)
        {
            bool RegistroCompleto = true;

            foreach (var FlujoInversion in pListFlujoInversion)
            {
                Programacion programacionOld = _context.Programacion.Where(r => r.ProgramacionId == FlujoInversion.ProgramacionId).FirstOrDefault();
                programacionOld.AvanceFisicoCapitulo = FlujoInversion.Programacion.AvanceFisicoCapitulo;

                if (!programacionOld.AvanceFisicoCapitulo.HasValue)
                {
                    RegistroCompleto = false;
                }
            }

            if (pSeguimientoSemanalAvanceFisico.SeguimientoSemanalAvanceFisicoId == 0)
            {
                pSeguimientoSemanalAvanceFisico.RegistroCompleto = RegistroCompleto;
                pSeguimientoSemanalAvanceFisico.UsuarioCreacion = usuarioCreacion;
                pSeguimientoSemanalAvanceFisico.FechaCreacion = DateTime.Now;
                pSeguimientoSemanalAvanceFisico.Eliminado = false;
            }
            {
                SeguimientoSemanalAvanceFisico seguimientoSemanalAvanceFisicoOld = _context.SeguimientoSemanalAvanceFisico.Find();

                seguimientoSemanalAvanceFisicoOld.RegistroCompleto = RegistroCompleto;
                pSeguimientoSemanalAvanceFisico.UsuarioModificacion = usuarioCreacion;
                pSeguimientoSemanalAvanceFisico.FechaModificacion = DateTime.Now;
            }

            //EstadosDisponibilidad codigo =  7 6 cuando esta estos estados de obra desabilitar 

            ///Validar Estado De obra 
            /////Programación acumulada de la obra: 2% == Avance acumulado ejecutado de la obra: 1%  = normal
            /////Programación acumulada de la obra: 2% < Avance acumulado ejecutado de la obra: 1%  = avanzada
            /////Programación acumulada de la obra: 2% > Avance acumulado ejecutado de la obra: 1%  = retrazado
            /////Programación acumulada de la obra: 2% > Avance acumulado ejecutado de la obra: 1%  = critico
            ///
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
                            EnsayoLaboratorioMuestra.RegistroCompleto = ValidarRegistroCompletoGestionEnsayoLaboratorioMuestra(EnsayoLaboratorioMuestra);

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
                    SeguimientoSemanalGestionObraAmbiental SeguimientoSemanalGestionObraAmbientalOld = _context.SeguimientoSemanalGestionObraAmbiental.Find(SeguimientoSemanalGestionObraAmbiental.SeguimientoSemanalGestionObraAmbientalId);

                    SeguimientoSemanalGestionObraAmbientalOld.UsuarioModificacion = pUsuarioCreacion;
                    SeguimientoSemanalGestionObraAmbientalOld.FechaModificacion = DateTime.Now;
                    SeguimientoSemanalGestionObraAmbientalOld.SeEjecutoGestionAmbiental = SeguimientoSemanalGestionObraAmbiental.SeEjecutoGestionAmbiental;

                    SeguimientoSemanalGestionObraAmbientalOld.TieneManejoMaterialesInsumo = SeguimientoSemanalGestionObraAmbiental.TieneManejoMaterialesInsumo;
                    SeguimientoSemanalGestionObraAmbientalOld.TieneManejoResiduosConstruccionDemolicion = SeguimientoSemanalGestionObraAmbiental.TieneManejoMaterialesInsumo;
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
                            manejoMaterialesInsumosOld.RegistroCompleto = ValidarRegistroCompletoManejoMaterialesInsumo(manejoMaterialesInsumosOld);
                        }
                        foreach (var ManejoMaterialesInsumosProveedor in SeguimientoSemanalGestionObraAmbiental.ManejoMaterialesInsumo.ManejoMaterialesInsumosProveedor)
                        {
                            if (ManejoMaterialesInsumosProveedor.ManejoMaterialesInsumosProveedorId == 0)
                            {
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

                            manejoResiduosConstruccionDemolicionOld.RegistroCompleto = ValidarRegistroCompletoManejoResiduosConstruccionDemolicion(manejoResiduosConstruccionDemolicionOld);
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
                                EnsayoLaboratorioMuestra.RegistroCompleto = ValidarRegistroCompletoGestionEnsayoLaboratorioMuestra(EnsayoLaboratorioMuestra);

                                _context.EnsayoLaboratorioMuestra.Add(EnsayoLaboratorioMuestra);
                            }
                            else
                            {
                                EnsayoLaboratorioMuestra ensayoLaboratorioMuestraOld = _context.EnsayoLaboratorioMuestra.Find(EnsayoLaboratorioMuestra.EnsayoLaboratorioMuestraId);
                                ensayoLaboratorioMuestraOld.RegistroCompleto = ValidarRegistroCompletoGestionEnsayoLaboratorioMuestra(EnsayoLaboratorioMuestra);
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
                       pSeguimientoSemanalRegistrarComiteObra.FechaComite.HasValue
                    && !string.IsNullOrEmpty(pSeguimientoSemanalRegistrarComiteObra.UrlSoporteComite)
                    ? true : false;

                _context.SeguimientoSemanalRegistrarComiteObra.Add(pSeguimientoSemanalRegistrarComiteObra);
            }
            else
            {
                SeguimientoSemanalRegistrarComiteObra SeguimientoSemanalRegistrarComiteObraOld = _context.SeguimientoSemanalRegistrarComiteObra.Find(pSeguimientoSemanalRegistrarComiteObra.SeguimientoSemanalRegistrarComiteObraId);
                SeguimientoSemanalRegistrarComiteObraOld.UsuarioModificacion = pUsuarioCreacion;
                SeguimientoSemanalRegistrarComiteObraOld.FechaModificacion = DateTime.Now;
                SeguimientoSemanalRegistrarComiteObraOld.RegistroCompleto = pSeguimientoSemanalRegistrarComiteObra.FechaComite.HasValue
                    && !string.IsNullOrEmpty(pSeguimientoSemanalRegistrarComiteObra.UrlSoporteComite)
                    ? true : false;

                SeguimientoSemanalRegistrarComiteObraOld.FechaComite = pSeguimientoSemanalRegistrarComiteObra.FechaComite;
                SeguimientoSemanalRegistrarComiteObraOld.UrlSoporteComite = pSeguimientoSemanalRegistrarComiteObra.UrlSoporteComite;
            }
        }

        #endregion

        #region Validar Registros Completos

        private bool ValidarRegistroCompletoSeguimientoSemanalGestionObra(SeguimientoSemanalGestionObra pSeguimientoSemanalGestionObra)
        {
            return false;
        }

        #region Gestion Obra Ambiental
        private bool ValidarRegistroCompletoSeguimientoSemanalGestionObraAlerta(SeguimientoSemanalGestionObraAlerta seguimientoSemanalGestionObraAlerta)
        {
            return false;
        }
        private bool ValidarRegistroCompletoMuestasLaboratorio(GestionObraCalidadEnsayoLaboratorio pGestionObraCalidadEnsayoLaboratorio)
        {
            foreach (var item in pGestionObraCalidadEnsayoLaboratorio.EnsayoLaboratorioMuestra)
            {
                if (!item.RegistroCompleto.HasValue || !(bool)item.RegistroCompleto)
                    return false;
            }

            return true;
        }
        private bool ValidarRegistroCompletoSeguimientoSemanalGestionObraSocial(SeguimientoSemanalGestionObraSocial seguimientoSemanalGestionObraSocial)
        {
            if (
                    !seguimientoSemanalGestionObraSocial.CantidadEmpleosDirectos.HasValue
                 || !seguimientoSemanalGestionObraSocial.CantidadEmpleosIndirectos.HasValue
                 || !seguimientoSemanalGestionObraSocial.CantidadTotalEmpleos.HasValue
                 || !seguimientoSemanalGestionObraSocial.SeRealizaronReuniones.HasValue
                 || (seguimientoSemanalGestionObraSocial.SeRealizaronReuniones == true &&
                    (string.IsNullOrEmpty(seguimientoSemanalGestionObraSocial.TemaComunidad) || string.IsNullOrEmpty(seguimientoSemanalGestionObraSocial.Conclusion)))
                )
            {
                return false;
            }
            return true;
        }

        private bool ValidarRegistroCompletoSeguimientoSemanalGestionObraSeguridadSalud(SeguimientoSemanalGestionObraSeguridadSalud seguimientoSemanalGestionObraSeguridadSalud)
        {
            return false;
        }

        private bool ValidarRegistroCompletoGestionEnsayoLaboratorioMuestra(EnsayoLaboratorioMuestra ensayoLaboratorioMuestra)
        {
            return false;
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
            if (!seguimientoSemanalGestionObraCalidad.SeRealizaronEnsayosLaboratorio.HasValue
                || seguimientoSemanalGestionObraCalidad.GestionObraCalidadEnsayoLaboratorio.Count() == 0
                )
            {
                return false;
            }


            foreach (var GestionObraCalidadEnsayoLaboratorio in seguimientoSemanalGestionObraCalidad.GestionObraCalidadEnsayoLaboratorio)
            {
                if (!GestionObraCalidadEnsayoLaboratorio.RegistroCompleto.HasValue || !(bool)GestionObraCalidadEnsayoLaboratorio.RegistroCompleto)
                {
                    return false;
                }
            }

            return true;
        }

        private bool ValidarRegistroCompletoSeguimientoSemanalGestionObraAmbiental(SeguimientoSemanalGestionObraAmbiental pSeguimientoSemanalGestionObraAmbiental)
        {
            return false;
        }

        private bool ValidarRegistroCompletoManejoMaterialesInsumo(ManejoMaterialesInsumos pManejoMaterialesInsumo)
        {
            return false;
        }

        private bool ValidarRegistroCompletoManejoMaterialesInsumosProveedor(ManejoMaterialesInsumosProveedor pManejoMaterialesInsumosProveedor)
        {
            return false;
        }

        private bool ValidarRegistroCompletoManejoResiduosConstruccionDemolicion(ManejoResiduosConstruccionDemolicion pManejoResiduosConstruccionDemolicion)
        {
            return false;
        }

        private bool ValidarRegistroCompletoManejoResiduosConstruccionDemolicionGestor(ManejoResiduosConstruccionDemolicionGestor pManejoResiduosConstruccionDemolicionGestor)
        {
            return false;
        }

        private bool ValidarRegistroCompletoManejoResiduosPeligrososEspeciales(ManejoResiduosPeligrososEspeciales pManejoResiduosPeligrososEspeciales)
        {
            if (
                !pManejoResiduosPeligrososEspeciales.EstanClasificados.HasValue
                || !pManejoResiduosPeligrososEspeciales.RequiereObservacion.HasValue
                || pManejoResiduosPeligrososEspeciales.RequiereObservacion.HasValue && (bool)pManejoResiduosPeligrososEspeciales.RequiereObservacion && string.IsNullOrEmpty(pManejoResiduosPeligrososEspeciales.Observacion)
               )
            {
                return false;
            }
            return true;
        }

        private bool ValidarRegistroCompletoManejoOtro(ManejoOtro pManejoOtro)
        {

            if (
                !pManejoOtro.FechaActividad.HasValue
                || string.IsNullOrEmpty(pManejoOtro.Actividad)
                || string.IsNullOrEmpty(pManejoOtro.UrlSoporteGestion)
                )
            {
                return false;
            }

            return true;
        }

        #endregion

        #endregion
    }
}
