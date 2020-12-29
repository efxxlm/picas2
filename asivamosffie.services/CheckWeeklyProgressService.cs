﻿using System;
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
    public class CheckWeeklyProgressService : ICheckWeeklyProgressService
    {
        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;

        public CheckWeeklyProgressService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;
        }
        public async Task<SeguimientoSemanal> GetSeguimientoSemanalBySeguimientoSemanalId(int pSeguimientoSemanalId)
        {
            List<Dominio> TipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();
            List<InstitucionEducativaSede> ListInstitucionEducativaSede = _context.InstitucionEducativaSede.ToList();
            List<Localizacion> ListLocalizacion = _context.Localizacion.ToList();
            try
            { 
                SeguimientoSemanal seguimientoSemanal = await _context.SeguimientoSemanal.Where(r => r.ContratacionProyectoId == pSeguimientoSemanalId && !(bool)r.Eliminado && !(bool)r.RegistroCompleto)

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

                //foreach (var FlujoInversion in seguimientoSemanal.FlujoInversion)
                //{
                //    FlujoInversion.Programacion.RangoDias = (FlujoInversion.Programacion.FechaFin - FlujoInversion.Programacion.FechaInicio).TotalDays;
                //}

                List<int> ListSeguimientoSemanalId = _context.SeguimientoSemanal.Where(r => r.ContratacionProyectoId == seguimientoSemanal.ContratacionProyectoId).Select(r => r.SeguimientoSemanalId).ToList();

                List<Programacion> ListProgramacion = _context.Programacion.FromSqlRaw("SELECT DISTINCT p.* FROM dbo.Programacion AS p INNER JOIN dbo.FlujoInversion AS f ON p.ProgramacionId = f.ProgramacionId INNER JOIN dbo.SeguimientoSemanal AS s ON f.SeguimientoSemanalId = s.SeguimientoSemanalId WHERE s.ContratacionProyectoId = " + seguimientoSemanal.ContratacionProyectoId + " AND p.TipoActividadCodigo = 'C'").ToList();

                seguimientoSemanal.CantidadTotalDiasActividades = ListProgramacion.Sum(r => r.Duracion);

                seguimientoSemanal.AvanceAcumulado = ListProgramacion
                    .GroupBy(r => r.Actividad)
                    .Select(r => new
                    {
                        Actividad = r.Key,
                        AvanceAcumulado = Math.Truncate((decimal)r.Sum(r => r.AvanceFisicoCapitulo)) + "%",
                        AvanceFisicoCapitulo = Math.Truncate((((decimal)r.Sum(r => r.Duracion) / seguimientoSemanal.CantidadTotalDiasActividades) * 100)) + "%"
                    });

                //Eliminar Las tablas eliminadas Logicamente
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
            catch (Exception ex)
            {
                return new SeguimientoSemanal();
            }

        }


        public async Task<Respuesta> CreateEditSeguimientoSemanalObservacion(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Observacion_Seguimiento_Semanal, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                UpdateObservation(pSeguimientoSemanalObservacion);

                switch (pSeguimientoSemanalObservacion.TipoObservacionCodigo)
                {

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.AVANCE_FISICO:
                        CreateOrEditObservacionAvanceFisico(pSeguimientoSemanalObservacion);
                        break;


                    case ConstanCodigoTipoObservacionSeguimientoSemanal.AVANCE_FINANCIERO:
                        CreateOrEditObservacionAvanceFinanciero(pSeguimientoSemanalObservacion);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.GESTION_OBRA:
                        CreateOrEditObservacionGestionObra(pSeguimientoSemanalObservacion);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.GESTION_OBRA_AMBIENTAL:
                        CreateOrEditObservacionGestionObraAmbiental(pSeguimientoSemanalObservacion);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.GESTION_OBRA_AMBIENTAL_MANEJO_MATERIALES:
                        CreateOrEditObservacionGestionObraAmbientalManejoMateriales(pSeguimientoSemanalObservacion);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.GESTION_OBRA_AMBIENTAL_MANEJO_CONSTRUCCION_DEMOLICION:
                        CreateOrEditObservacionGestionObraAmbientalManejoConstruccionDemolicion(pSeguimientoSemanalObservacion);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.GESTION_OBRA_AMBIENTAL_MANEJO_RESIDUOS_PELIGROSOS:
                        CreateOrEditObservacionGestionObraAmbientalManejoResiduosPeligrosos(pSeguimientoSemanalObservacion);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.GESTION_OBRA_AMBIENTAL_MANEJO_OTRA:
                        CreateOrEditObservacionGestionObraAmbientalManejoOtra(pSeguimientoSemanalObservacion);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.GESTION_CALIDAD:
                        CreateOrEditObservacionGestionCalidad(pSeguimientoSemanalObservacion);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.GESTION_CALIDAD_ENSAYO_LABORATORIO:
                        CreateOrEditObservacionGestionCalidadEnsayoLaboratorio(pSeguimientoSemanalObservacion);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.GESTION_CALIDAD_ENSAYO_LABORATORIO_MUESTRAS:
                        CreateOrEditObservacionGestionCalidadEnsayoLaboratorioMuestras(pSeguimientoSemanalObservacion);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.GESTION_SOCIAL:
                        CreateOrEditObservacionGestionSocial(pSeguimientoSemanalObservacion);
                        break;       
                     
                    case ConstanCodigoTipoObservacionSeguimientoSemanal.ALERTAS_RELEVANTES:
                        CreateOrEditObservacionAlertasRelevantes(pSeguimientoSemanalObservacion);
                        break;
                         
                    case ConstanCodigoTipoObservacionSeguimientoSemanal.REPORTE_ACTIVIDADES:
                        CreateOrEditObservacionReporteActividades(pSeguimientoSemanalObservacion);
                        break;


                    case ConstanCodigoTipoObservacionSeguimientoSemanal.REPORTE_ACTIVIDADES_ESTADO_OBRA:
                        CreateOrEditObservacionReporteActividadesEstadoContrato(pSeguimientoSemanalObservacion);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.REPORTE_ACTIVIDADES_ACTIVIDADES_REALIZADAS:
                        CreateOrEditObservacionReporteActividadesRealizadas(pSeguimientoSemanalObservacion);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.REPORTE_ACTIVIDADES_ACTIVIDADES_REALIZADAS_SIGUIENTE_SEMANA:
                        CreateOrEditObservacionReporteActividadesRealizadasSiguienteSemana(pSeguimientoSemanalObservacion);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.REGISTRO_FOTOGRAFICO:
                        CreateOrEditObservacionRegistroFotografico(pSeguimientoSemanalObservacion);
                        break;

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.COMITE_OBRA:
                        CreateOrEditObservacionComiteObra(pSeguimientoSemanalObservacion);
                        break;

                    default:
                        break;
                }

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstanMessagesRegisterWeeklyProgress.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, ConstanMessagesRegisterWeeklyProgress.OperacionExitosa, idAccion, pSeguimientoSemanalObservacion.UsuarioCreacion, "CREAR OBSERVACION")
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, ConstanMessagesRegisterWeeklyProgress.Error, idAccion, pSeguimientoSemanalObservacion.UsuarioCreacion, ex.InnerException.ToString())
                };
            }
        }

        private void CreateOrEditObservacionAlertasRelevantes(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion)
        {
            SeguimientoSemanalGestionObraAlerta seguimientoSemanalGestionObraAlertaOld = _context.SeguimientoSemanalGestionObraAlerta.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);

            seguimientoSemanalGestionObraAlertaOld.FechaModificacion = DateTime.Now;
            seguimientoSemanalGestionObraAlertaOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
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

        private void CreateOrEditObservacionGestionSocial(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion)
        {
            SeguimientoSemanalGestionObraSocial seguimientoSemanalGestionObraSocialOld = _context.SeguimientoSemanalGestionObraSocial.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);
             
            seguimientoSemanalGestionObraSocialOld.FechaModificacion = DateTime.Now;
            seguimientoSemanalGestionObraSocialOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
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

        private void CreateOrEditObservacionGestionSeguridadSalud(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion)
        {
            SeguimientoSemanalGestionObraSeguridadSalud seguimientoSemanalGestionObraSeguridadSaludOld = _context.SeguimientoSemanalGestionObraSeguridadSalud.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);

            seguimientoSemanalGestionObraSeguridadSaludOld.FechaModificacion = DateTime.Now;
            seguimientoSemanalGestionObraSeguridadSaludOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
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

        private void CreateOrEditObservacionComiteObra(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion)
        {
            SeguimientoSemanalRegistrarComiteObra SeguimientoSemanalRegistrarComiteObraOld = _context.SeguimientoSemanalRegistrarComiteObra.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);
            SeguimientoSemanalRegistrarComiteObraOld.FechaModificacion = DateTime.Now;
            SeguimientoSemanalRegistrarComiteObraOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
                SeguimientoSemanalRegistrarComiteObraOld.TieneObservacionSupervisor = pSeguimientoSemanalObservacion.TieneObservacion;
                SeguimientoSemanalRegistrarComiteObraOld.ObservacionSupervisorId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                // SeguimientoSemanalRegistrarComiteObraOld.RegistroCompletoObservacionSupervisor = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
            else
            {
                SeguimientoSemanalRegistrarComiteObraOld.TieneObservacionApoyo = pSeguimientoSemanalObservacion.TieneObservacion;
                SeguimientoSemanalRegistrarComiteObraOld.ObservacionApoyoId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                // SeguimientoSemanalRegistrarComiteObraOld.RegistroCompletoObservacionApoyo = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
        }

        private void CreateOrEditObservacionRegistroFotografico(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion)
        {
            SeguimientoSemanalRegistroFotografico SeguimientoSemanalRegistroFotograficoOld = _context.SeguimientoSemanalRegistroFotografico.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);
            SeguimientoSemanalRegistroFotograficoOld.FechaModificacion = DateTime.Now;
            SeguimientoSemanalRegistroFotograficoOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
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

        private void CreateOrEditObservacionReporteActividadesRealizadasSiguienteSemana(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion)
        {
            SeguimientoSemanalReporteActividad seguimientoSemanalReporteActividad = _context.SeguimientoSemanalReporteActividad.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);

            seguimientoSemanalReporteActividad.FechaModificacion = DateTime.Now;
            seguimientoSemanalReporteActividad.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
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

        private void CreateOrEditObservacionReporteActividadesRealizadas(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion)
        {
            SeguimientoSemanalReporteActividad seguimientoSemanalReporteActividad = _context.SeguimientoSemanalReporteActividad.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);

            seguimientoSemanalReporteActividad.FechaModificacion = DateTime.Now;
            seguimientoSemanalReporteActividad.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
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

        private void CreateOrEditObservacionReporteActividadesEstadoContrato(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion)
        {
            SeguimientoSemanalReporteActividad seguimientoSemanalReporteActividad = _context.SeguimientoSemanalReporteActividad.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);

            seguimientoSemanalReporteActividad.FechaModificacion = DateTime.Now;
            seguimientoSemanalReporteActividad.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
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

        private void CreateOrEditObservacionReporteActividades(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion)
        {
            SeguimientoSemanalReporteActividad seguimientoSemanalReporteActividad = _context.SeguimientoSemanalReporteActividad.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);

            seguimientoSemanalReporteActividad.FechaModificacion = DateTime.Now;
            seguimientoSemanalReporteActividad.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

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

        private void CreateOrEditObservacionGestionCalidadEnsayoLaboratorioMuestras(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion)
        {
            EnsayoLaboratorioMuestra ensayoLaboratorioMuestraOld = _context.EnsayoLaboratorioMuestra.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);
            ensayoLaboratorioMuestraOld.FechaModificacion = DateTime.Now;
            ensayoLaboratorioMuestraOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
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

        private void CreateOrEditObservacionGestionCalidadEnsayoLaboratorio(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion)
        {
            GestionObraCalidadEnsayoLaboratorio gestionObraCalidadEnsayoLaboratorioOld = _context.GestionObraCalidadEnsayoLaboratorio.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);

            gestionObraCalidadEnsayoLaboratorioOld.FechaModificacion = DateTime.Now;
            gestionObraCalidadEnsayoLaboratorioOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
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

        private void CreateOrEditObservacionGestionCalidad(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion)
        {
            SeguimientoSemanalGestionObraCalidad seguimientoSemanalGestionObraCalidad = _context.SeguimientoSemanalGestionObraCalidad.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);

            seguimientoSemanalGestionObraCalidad.FechaModificacion = DateTime.Now;
            seguimientoSemanalGestionObraCalidad.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
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

        private void CreateOrEditObservacionGestionObraAmbientalManejoOtra(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion)
        {
            ManejoOtro manejoOtroOld = _context.ManejoOtro.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);

            manejoOtroOld.FechaModificacion = DateTime.Now;
            manejoOtroOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
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

        private void CreateOrEditObservacionGestionObraAmbientalManejoResiduosPeligrosos(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion)
        {
            ManejoResiduosPeligrososEspeciales manejoResiduosPeligrososEspecialesOld = _context.ManejoResiduosPeligrososEspeciales.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);

            manejoResiduosPeligrososEspecialesOld.FechaModificacion = DateTime.Now;
            manejoResiduosPeligrososEspecialesOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
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

        private void CreateOrEditObservacionGestionObraAmbientalManejoConstruccionDemolicion(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion)
        {
            ManejoResiduosConstruccionDemolicion manejoResiduosConstruccionDemolicionOld = _context.ManejoResiduosConstruccionDemolicion.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);

            manejoResiduosConstruccionDemolicionOld.FechaModificacion = DateTime.Now;
            manejoResiduosConstruccionDemolicionOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
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

        private void CreateOrEditObservacionGestionObraAmbientalManejoMateriales(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion)
        {
            ManejoMaterialesInsumos manejoMaterialesInsumosOld = _context.ManejoMaterialesInsumos.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);

            manejoMaterialesInsumosOld.FechaModificacion = DateTime.Now;
            manejoMaterialesInsumosOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
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

        private void CreateOrEditObservacionGestionObraAmbiental(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion)
        {
            SeguimientoSemanalGestionObraAmbiental seguimientoSemanalGestionObraAmbientalOld = _context.SeguimientoSemanalGestionObraAmbiental.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);

            seguimientoSemanalGestionObraAmbientalOld.FechaModificacion = DateTime.Now;
            seguimientoSemanalGestionObraAmbientalOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
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

        private void CreateOrEditObservacionAvanceFinanciero(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion)
        {
            SeguimientoSemanalAvanceFinanciero seguimientoSemanalAvanceFinancieroOld = _context.SeguimientoSemanalAvanceFinanciero.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);


            seguimientoSemanalAvanceFinancieroOld.FechaModificacion = DateTime.Now;
            seguimientoSemanalAvanceFinancieroOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
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

        private void CreateOrEditObservacionAvanceFisico(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion)
        {
            SeguimientoSemanalAvanceFisico seguimientoSemanalAvanceFisicoOld = _context.SeguimientoSemanalAvanceFisico.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);

            seguimientoSemanalAvanceFisicoOld.FechaModificacion = DateTime.Now;
            seguimientoSemanalAvanceFisicoOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;


            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
                seguimientoSemanalAvanceFisicoOld.TieneObservacionSupervisor = pSeguimientoSemanalObservacion.TieneObservacion;
                seguimientoSemanalAvanceFisicoOld.ObservacionSupervisorId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                seguimientoSemanalAvanceFisicoOld.RegistroCompletoObservacionSupervisor = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
            else
            {
                seguimientoSemanalAvanceFisicoOld.TieneObservacionApoyo = pSeguimientoSemanalObservacion.TieneObservacion;
                seguimientoSemanalAvanceFisicoOld.ObservacionApoyoId = pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
                seguimientoSemanalAvanceFisicoOld.RegistroCompletoObservacionApoyo = CompleteRecordObservation(pSeguimientoSemanalObservacion);
            }
        }

        private bool CompleteRecordObservation(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion)
        {
            if (pSeguimientoSemanalObservacion.TieneObservacion == false)
                return true;
            if (pSeguimientoSemanalObservacion.TieneObservacion == true && !string.IsNullOrEmpty(pSeguimientoSemanalObservacion.Observacion))
                return true;

            return false;
        }

        private int UpdateObservation(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion)
        {
            if (pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId == 0)
            {
                pSeguimientoSemanalObservacion.FechaCreacion = DateTime.Now;
                pSeguimientoSemanalObservacion.Eliminado = false;
                pSeguimientoSemanalObservacion.Archivada = false;
                _context.SeguimientoSemanalObservacion.Add(pSeguimientoSemanalObservacion);
            }
            else
            {
                SeguimientoSemanalObservacion seguimientoSemanalObservacionOld = _context.SeguimientoSemanalObservacion.Find(pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId);

                seguimientoSemanalObservacionOld.Observacion = pSeguimientoSemanalObservacion.Observacion;
                seguimientoSemanalObservacionOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;
                seguimientoSemanalObservacionOld.FechaModificacion = DateTime.Now;
            }
            _context.SaveChanges();

            return pSeguimientoSemanalObservacion.SeguimientoSemanalObservacionId;
        }

        //TODO : VAlidar EXCEPTIONS
        private void CreateOrEditObservacionGestionObra(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion)
        {
            throw new NotImplementedException();
        }
    }
}
