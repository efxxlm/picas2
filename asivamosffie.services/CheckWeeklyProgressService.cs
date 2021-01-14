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
        #region Construcctor
        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;
        private readonly IRegisterWeeklyProgressService _registerWeeklyProgressService;

        public CheckWeeklyProgressService(devAsiVamosFFIEContext context, ICommonService commonService, IRegisterWeeklyProgressService registerWeeklyProgressService)
        {
            _commonService = commonService;
            _registerWeeklyProgressService = registerWeeklyProgressService;
            _context = context;
        }
        #endregion region 

        #region Business
        public async Task<bool> GetValidarRegistroCompletoObservaciones(int pSeguimientoSemanalId, bool esSupervisor)
        {
            try
            { 
                SeguimientoSemanal seguimientoSemanal = _context.SeguimientoSemanal.Find(pSeguimientoSemanalId);
                _context.SeguimientoSemanal.Attach(seguimientoSemanal);
                if (esSupervisor)
                {
                    seguimientoSemanal.RegistroCompletoAvalar = await ValidarRegistroCompletoObservacion(seguimientoSemanal, esSupervisor);
                    _context.Entry(seguimientoSemanal).Property(x => x.RegistroCompletoAvalar).IsModified = true;
                }
                else
                {
                    seguimientoSemanal.RegistroCompletoVerificar = await ValidarRegistroCompletoObservacion(seguimientoSemanal, esSupervisor);
                    _context.Entry(seguimientoSemanal).Property(x => x.RegistroCompletoVerificar).IsModified = true;
                }

                _context.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private async Task<bool> ValidarRegistroCompletoObservacion(SeguimientoSemanal pSeguimientoSemanal, bool pEsSupervisor)
        {
            bool RegistroCompleto = true;

            SeguimientoSemanal seguimientoSemanal = GetSeguimientoSemanalBySeguimientoSemanalIdClean(pSeguimientoSemanal.SeguimientoSemanalId);
            //Validar Avance Financiero
            if (seguimientoSemanal.NumeroSemana % 5 == 0)
            {
                RegistroCompleto = ValidateCompleteObservationAvanceFinanciero(seguimientoSemanal.SeguimientoSemanalAvanceFinanciero.FirstOrDefault(), pEsSupervisor);
                if (!RegistroCompleto)
                    return RegistroCompleto;
            }
            //Validar Avance Fisico
            RegistroCompleto = ValidateCompleteObservationAvanceFisico(seguimientoSemanal.SeguimientoSemanalAvanceFisico.FirstOrDefault(), pEsSupervisor);
            if (!RegistroCompleto)
                return RegistroCompleto;

            //Validar Gestion Obra  Ambiental
            RegistroCompleto = ValidateCompleteObservationGestionObraGestionAmbiental(seguimientoSemanal.SeguimientoSemanalGestionObra.FirstOrDefault().SeguimientoSemanalGestionObraAmbiental, pEsSupervisor);
            if (!RegistroCompleto)
                return RegistroCompleto;

            //Validar Gestion Obra Calidad Ensayo  Laboratorio y Muestras
            RegistroCompleto = ValidateCompleteObservationGestionObraGestionCalidadMuestras(seguimientoSemanal.SeguimientoSemanalGestionObra.FirstOrDefault().SeguimientoSemanalGestionObraCalidad, pEsSupervisor);
            if (!RegistroCompleto)
                return RegistroCompleto;

            //Validar Gestion Obra Calidad
            //RegistroCompleto = ValidateCompleteObservationGestionObraGestionCalidad(seguimientoSemanal.SeguimientoSemanalGestionObra.FirstOrDefault().SeguimientoSemanalGestionObraCalidad, pEsSupervisor);
            //if (!RegistroCompleto)
            //    return RegistroCompleto;

            //Validar Gestion Obra Seguridad Salud
            RegistroCompleto = ValidateCompleteObservationGestionObraGestionSeguridadSalud(seguimientoSemanal.SeguimientoSemanalGestionObra.FirstOrDefault().SeguimientoSemanalGestionObraSeguridadSalud, pEsSupervisor);
            if (!RegistroCompleto)
                return RegistroCompleto;

            //Validar Gestion Obra Gestion Social
            RegistroCompleto = ValidateCompleteObservationGestionObraGestionSocial(seguimientoSemanal.SeguimientoSemanalGestionObra.FirstOrDefault().SeguimientoSemanalGestionObraSocial, pEsSupervisor);
            if (!RegistroCompleto)
                return RegistroCompleto;

            //Validar Gestion Obra Alertas Relevantes
            RegistroCompleto = ValidateCompleteObservationGestionObraAlertasRelevantes(seguimientoSemanal.SeguimientoSemanalGestionObra.FirstOrDefault().SeguimientoSemanalGestionObraAlerta, pEsSupervisor);
            if (!RegistroCompleto)
                return RegistroCompleto;

            //Validar Reporte Actividades
            RegistroCompleto = ValidateCompleteObservationValidarReporteActividades(seguimientoSemanal.SeguimientoSemanalReporteActividad.FirstOrDefault(), pEsSupervisor);
            if (!RegistroCompleto)
                return RegistroCompleto;

            //Validar Registro Registro Fotografico
            RegistroCompleto = ValidateCompleteObservationValidarRegistroFotografico(seguimientoSemanal.SeguimientoSemanalRegistroFotografico.FirstOrDefault(), pEsSupervisor);
            if (!RegistroCompleto)
                return RegistroCompleto;

            //Validar Registro Comite Obra
            RegistroCompleto = ValidateCompleteObservationValidarComiteObra(seguimientoSemanal.SeguimientoSemanalRegistrarComiteObra.FirstOrDefault(), pEsSupervisor);
            if (!RegistroCompleto)
                return RegistroCompleto;

            return RegistroCompleto;
        }

        private bool ValidateCompleteObservationValidarComiteObra(SeguimientoSemanalRegistrarComiteObra seguimientoSemanalRegistrarComiteObra, bool pEsSupervisor)
        {
            if (pEsSupervisor)
            {
                if (seguimientoSemanalRegistrarComiteObra.RegistroCompletoObservacionSupervisor != true)
                    return false;
            }
            else
            {
                if (seguimientoSemanalRegistrarComiteObra.RegistroCompletoObservacionApoyo != true)
                    return false;
            }

            return true;
        }

        private bool ValidateCompleteObservationValidarRegistroFotografico(SeguimientoSemanalRegistroFotografico seguimientoSemanalRegistroFotografico, bool pEsSupervisor)
        {
            if (pEsSupervisor)
            {
                if (seguimientoSemanalRegistroFotografico.RegistroCompletoObservacionSupervisor != true)
                    return false;
            }
            else
            {
                if (seguimientoSemanalRegistroFotografico.RegistroCompletoObservacionApoyo != true)
                    return false;

            }
            return true;
        }

        private bool ValidateCompleteObservationValidarReporteActividades(SeguimientoSemanalReporteActividad seguimientoSemanalReporteActividad, bool pEsSupervisor)
        {
            if (pEsSupervisor)
            {
                if (seguimientoSemanalReporteActividad.RegistroCompletoObservacionSupervisorActividad != true)
                    return false;

                if (seguimientoSemanalReporteActividad.RegistroCompletoObservacionSupervisorActividadSiguiente != true)
                    return false;

                if (seguimientoSemanalReporteActividad.RegistroCompletoObservacionSupervisorEstadoContrato != true)
                    return false;
            }
            else
            {
                if (seguimientoSemanalReporteActividad.RegistroCompletoObservacionApoyoActividad != true)
                    return false;

                if (seguimientoSemanalReporteActividad.RegistroCompletoObservacionApoyoActividadSiguiente != true)
                    return false;

                if (seguimientoSemanalReporteActividad.RegistroCompletoObservacionApoyoEstadoContrato != true)
                    return false;
            }
            return true;
        }

        private bool ValidateCompleteObservationGestionObraAlertasRelevantes(ICollection<SeguimientoSemanalGestionObraAlerta> seguimientoSemanalGestionObraAlerta, bool pEsSupervisor)
        {
            if (pEsSupervisor)
            {
                foreach (var item in seguimientoSemanalGestionObraAlerta)
                {
                    if (item.RegistroCompletoObservacionSupervisor != true)
                        return false;
                }
            }
            else
            {
                foreach (var item in seguimientoSemanalGestionObraAlerta)
                {
                    if (item.RegistroCompletoObservacionApoyo != true)
                        return false;
                }
            }
            return true;
        }

        private bool ValidateCompleteObservationGestionObraGestionSocial(ICollection<SeguimientoSemanalGestionObraSocial> seguimientoSemanalGestionObraSocial, bool pEsSupervisor)
        {
            if (pEsSupervisor)
            {
                foreach (var item in seguimientoSemanalGestionObraSocial)
                {
                    if (item.RegistroCompletoObservacionSupervisor != true)
                        return false;
                }
            }
            else
            {
                foreach (var item in seguimientoSemanalGestionObraSocial)
                {
                    if (item.RegistroCompletoObservacionApoyo != true)
                        return false;
                }
            }
            return true;
        }

        private bool ValidateCompleteObservationGestionObraGestionSeguridadSalud(ICollection<SeguimientoSemanalGestionObraSeguridadSalud> seguimientoSemanalGestionObraSeguridadSalud, bool pEsSupervisor)
        {
            if (pEsSupervisor)
            {
                foreach (var item in seguimientoSemanalGestionObraSeguridadSalud)
                {
                    if (item.RegistroCompletoObservacionSupervisor != true)
                        return false;
                }
            }
            else
            {
                foreach (var item in seguimientoSemanalGestionObraSeguridadSalud)
                {
                    if (item.RegistroCompletoObservacionApoyo != true)
                        return false;
                }
            }
            return true;
        }

        private bool ValidateCompleteObservationGestionObraGestionCalidadMuestras(ICollection<SeguimientoSemanalGestionObraCalidad> seguimientoSemanalGestionObraCalidad, bool pEsSupervisor)
        {
            if (pEsSupervisor)
            {
                foreach (var item2 in seguimientoSemanalGestionObraCalidad)
                {
                    foreach (var item3 in item2.GestionObraCalidadEnsayoLaboratorio)
                    {
                        if (item3.TieneObservacionSupervisor != true)
                            return false;

                        //foreach (var item in item3.EnsayoLaboratorioMuestra)
                        //{
                        //    if (item.TieneObservacionSupervisor != true)
                        //        return false;
                        //}
                    }
                }
            }
            else
            {
                foreach (var item2 in seguimientoSemanalGestionObraCalidad)
                {
                    foreach (var item3 in item2.GestionObraCalidadEnsayoLaboratorio)
                    {
                        if (item3.TieneObservacionApoyo != true)
                            return false;

                        //foreach (var item in item3.EnsayoLaboratorioMuestra)
                        //{
                        //    if (item.TieneObservacionApoyo != true)
                        //        return false;
                        //}
                    }
                }
            }
            return true;
        }

        private bool ValidateCompleteObservationGestionObraGestionCalidad(ICollection<SeguimientoSemanalGestionObraCalidad> seguimientoSemanalGestionObraCalidad, bool pEsSupervisor)
        {
            if (pEsSupervisor)
            {
                foreach (var item in seguimientoSemanalGestionObraCalidad)
                {
                    if (item.RegistroCompletoObservacionSupervisor != true)
                        return false;
                }
            }
            else
            {
                foreach (var item in seguimientoSemanalGestionObraCalidad)
                {
                    if (item.RegistroCompletoObservacionApoyo != true)
                        return false;
                }
            }
            return true;
        }

        private bool ValidateCompleteObservationGestionObraGestionAmbiental(ICollection<SeguimientoSemanalGestionObraAmbiental> seguimientoSemanalGestionObraAmbiental, bool pEsSupervisor)
        {
            if (pEsSupervisor)
            {
                foreach (var item in seguimientoSemanalGestionObraAmbiental)
                {
                    if (item.ManejoMaterialesInsumoId == 0
                     && item.ManejoResiduosConstruccionDemolicionId == 0
                     && item.ManejoResiduosPeligrososEspecialesId == 0
                     && item.ManejoOtroId == 0)
                    {
                        if (item.RegistroCompletoObservacionSupervisor != true)
                            return false;
                    }
                    else
                    {
                        if (item.ManejoMaterialesInsumoId > 0 && item.ManejoMaterialesInsumo.RegistroCompletoObservacionSupervisor != true)
                            return false;
                        if (item.ManejoResiduosConstruccionDemolicionId > 0 && item.ManejoResiduosConstruccionDemolicion.RegistroCompletoObservacionSupervisor != true)
                            return false;
                        if (item.ManejoResiduosPeligrososEspecialesId > 0 && item.ManejoResiduosPeligrososEspeciales.RegistroCompletoObservacionSupervisor != true)
                            return false;
                        if (item.ManejoOtroId > 0 && item.ManejoOtro.RegistroCompletoObservacionSupervisor != true)
                            return false;
                    }
                }
            }
            else
            {
                foreach (var item in seguimientoSemanalGestionObraAmbiental)
                {
                    if (item.ManejoMaterialesInsumoId == 0
                        && item.ManejoResiduosConstruccionDemolicionId == 0
                        && item.ManejoResiduosPeligrososEspecialesId == 0
                        && item.ManejoOtroId == 0)
                    {
                        if (item.RegistroCompletoObservacionApoyo != true)
                            return false;
                    }
                    else
                    {
                        if (item.ManejoMaterialesInsumoId > 0 && item.ManejoMaterialesInsumo.RegistroCompletoObservacionApoyo != true)
                            return false;
                        if (item.ManejoResiduosConstruccionDemolicionId > 0 && item.ManejoResiduosConstruccionDemolicion.RegistroCompletoObservacionApoyo != true)
                            return false;
                        if (item.ManejoResiduosPeligrososEspecialesId > 0 && item.ManejoResiduosPeligrososEspeciales.RegistroCompletoObservacionApoyo != true)
                            return false;
                        if (item.ManejoOtroId > 0 && item.ManejoOtro.RegistroCompletoObservacionApoyo != true)
                            return false;
                    }
                }
            }
            return true;
        }

        private bool ValidateCompleteObservationAvanceFinanciero(SeguimientoSemanalAvanceFinanciero seguimientoSemanalAvanceFinanciero, bool pEsSupervisor)
        {
            if (pEsSupervisor)
            {
                if (seguimientoSemanalAvanceFinanciero.RegistroCompletoObservacionSupervisor != true)
                    return false;
            }
            else
            {
                if (seguimientoSemanalAvanceFinanciero.RegistroCompletoObservacionApoyo != true)
                    return false;
            }
            return true;
        }

        private bool ValidateCompleteObservationAvanceFisico(SeguimientoSemanalAvanceFisico seguimientoSemanalAvanceFisico, bool pEsSupervisor)
        {
            if (pEsSupervisor)
            {
                if (seguimientoSemanalAvanceFisico.RegistroCompletoObservacionSupervisor != true)
                    return false;
            }
            else
            {
                if (seguimientoSemanalAvanceFisico.RegistroCompletoObservacionApoyo != true)
                    return false;
            }
            return true;
        }

        #endregion

        #region List
        public async Task<SeguimientoSemanal> GetSeguimientoSemanalBySeguimientoSemanalId(int pSeguimientoSemanalId)
        {
            List<Dominio> TipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();
            List<InstitucionEducativaSede> ListInstitucionEducativaSede = _context.InstitucionEducativaSede.ToList();
            List<Localizacion> ListLocalizacion = _context.Localizacion.ToList();
            try
            {
                SeguimientoSemanal seguimientoSemanal = await _context.SeguimientoSemanal
                    .Where(r => r.ContratacionProyectoId == pSeguimientoSemanalId && !(bool)r.Eliminado && !(bool)r.RegistroCompleto)
                  .Include(r => r.ContratacionProyecto)
                          .ThenInclude(r => r.Contratacion)
                              .ThenInclude(r => r.Contrato)
                       .Include(r => r.ContratacionProyecto)
                          .ThenInclude(r => r.Proyecto)
                              .ThenInclude(r => r.InstitucionEducativa)
                       .Include(r => r.SeguimientoDiario)
                              .ThenInclude(r => r.SeguimientoDiarioObservaciones)

                       //Financiero + Observaciones
                       .Include(r => r.SeguimientoSemanalAvanceFinanciero)
                            .ThenInclude(r => r.ObservacionApoyo)
                       .Include(r => r.SeguimientoSemanalAvanceFinanciero)
                            .ThenInclude(r => r.ObservacionSupervisor)

                       //Fisico + Observaciones
                       .Include(r => r.SeguimientoSemanalAvanceFisico)
                          .ThenInclude(r => r.ObservacionApoyo)
                       .Include(r => r.SeguimientoSemanalAvanceFisico)
                          .ThenInclude(r => r.ObservacionSupervisor)

                       //Gestion Obra
                       //Gestion Obra Ambiental + Observaciones
                       .Include(r => r.SeguimientoSemanalGestionObra)
                            .ThenInclude(r => r.SeguimientoSemanalGestionObraAmbiental)
                                  .ThenInclude(r => r.ObservacionApoyo)
                      .Include(r => r.SeguimientoSemanalGestionObra)
                            .ThenInclude(r => r.SeguimientoSemanalGestionObraAmbiental)
                                  .ThenInclude(r => r.ObservacionSupervisor)

                         // Gestion Obra Calidad 
                         //Observaciones Ensayo Laboratorio + Observaciones
                         .Include(r => r.SeguimientoSemanalGestionObra)
                            .ThenInclude(r => r.SeguimientoSemanalGestionObraCalidad)
                                .ThenInclude(r => r.GestionObraCalidadEnsayoLaboratorio)
                                        .ThenInclude(r => r.ObservacionApoyo)
                          .Include(r => r.SeguimientoSemanalGestionObra)
                            .ThenInclude(r => r.SeguimientoSemanalGestionObraCalidad)
                                .ThenInclude(r => r.GestionObraCalidadEnsayoLaboratorio)
                                        .ThenInclude(r => r.ObservacionSupervisor)

                           //Observaciones Ensayo laboratorio Muestras + Observaciones
                           .Include(r => r.SeguimientoSemanalGestionObra)
                            .ThenInclude(r => r.SeguimientoSemanalGestionObraCalidad)
                                .ThenInclude(r => r.GestionObraCalidadEnsayoLaboratorio)
                                    .ThenInclude(r => r.EnsayoLaboratorioMuestra)
                                        .ThenInclude(r => r.ObservacionApoyo)

                           .Include(r => r.SeguimientoSemanalGestionObra)
                            .ThenInclude(r => r.SeguimientoSemanalGestionObraCalidad)
                                .ThenInclude(r => r.GestionObraCalidadEnsayoLaboratorio)
                                    .ThenInclude(r => r.EnsayoLaboratorioMuestra)
                                        .ThenInclude(r => r.ObservacionSupervisor)

                         //Gestion Obra Calidad
                         .Include(r => r.SeguimientoSemanalGestionObra)
                            .ThenInclude(r => r.SeguimientoSemanalGestionObraSeguridadSalud)
                                .ThenInclude(r => r.SeguridadSaludCausaAccidente)

                          //Gestion Obra Calidad + Observaciones
                          .Include(r => r.SeguimientoSemanalGestionObra)
                             .ThenInclude(r => r.SeguimientoSemanalGestionObraSeguridadSalud)
                                 .ThenInclude(r => r.ObservacionApoyo)
                          .Include(r => r.SeguimientoSemanalGestionObra)
                             .ThenInclude(r => r.SeguimientoSemanalGestionObraSeguridadSalud)
                                 .ThenInclude(r => r.ObservacionSupervisor)

                        //Gestion Obra Social + Observaciones
                        .Include(r => r.SeguimientoSemanalGestionObra)
                           .ThenInclude(r => r.SeguimientoSemanalGestionObraSocial)
                             .ThenInclude(r => r.ObservacionApoyo)

                        .Include(r => r.SeguimientoSemanalGestionObra)
                           .ThenInclude(r => r.SeguimientoSemanalGestionObraSocial)
                             .ThenInclude(r => r.ObservacionSupervisor)

                       //Alertas Relevantes  + Observaciones
                       .Include(r => r.SeguimientoSemanalGestionObra)
                           .ThenInclude(r => r.SeguimientoSemanalGestionObraAlerta)
                                 .ThenInclude(r => r.ObservacionApoyo)

                        .Include(r => r.SeguimientoSemanalGestionObra)
                           .ThenInclude(r => r.SeguimientoSemanalGestionObraAlerta)
                                 .ThenInclude(r => r.ObservacionSupervisor)

                       //Reporte Actividades + Observaciones

                       //Estado Contrato + Observaciones
                       .Include(r => r.SeguimientoSemanalReporteActividad)
                              .ThenInclude(r => r.ObservacionApoyoIdEstadoContratoNavigation)
                       .Include(r => r.SeguimientoSemanalReporteActividad)
                              .ThenInclude(r => r.ObservacionSupervisorIdEstadoContratoNavigation)

                       //Actividades Realizadas + Observaciones
                       .Include(r => r.SeguimientoSemanalReporteActividad)
                              .ThenInclude(r => r.ObservacionApoyoIdActividadNavigation)
                       .Include(r => r.SeguimientoSemanalReporteActividad)
                              .ThenInclude(r => r.ObservacionSupervisorIdActividadNavigation)

                         //Actividades Realizadas Siguiente semana  + Observaciones
                         .Include(r => r.SeguimientoSemanalReporteActividad)
                              .ThenInclude(r => r.ObservacionApoyoIdActividadSiguienteNavigation)
                       .Include(r => r.SeguimientoSemanalReporteActividad)
                              .ThenInclude(r => r.ObservacionSupervisorIdActividadSiguienteNavigation)

                       //Registro Fotografico + Observaciones
                       .Include(r => r.SeguimientoSemanalRegistroFotografico)
                            .ThenInclude(r => r.ObservacionApoyo)
                       .Include(r => r.SeguimientoSemanalRegistroFotografico)
                            .ThenInclude(r => r.ObservacionSupervisor)

                       //Comite Obra + Observaciones 
                       .Include(r => r.SeguimientoSemanalRegistrarComiteObra)
                           .ThenInclude(r => r.ObservacionApoyo)
                       .Include(r => r.SeguimientoSemanalRegistrarComiteObra)
                           .ThenInclude(r => r.ObservacionSupervisor)
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

        public SeguimientoSemanal GetSeguimientoSemanalBySeguimientoSemanalIdClean(int pSeguimientoSemanalId)
        {
            List<Dominio> TipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();
            List<InstitucionEducativaSede> ListInstitucionEducativaSede = _context.InstitucionEducativaSede.ToList();
            List<Localizacion> ListLocalizacion = _context.Localizacion.ToList();
            try
            {
                SeguimientoSemanal seguimientoSemanal = _context.SeguimientoSemanal.Where(r => r.SeguimientoSemanalId == pSeguimientoSemanalId)
                  .Include(r => r.ContratacionProyecto)
                          .ThenInclude(r => r.Contratacion)
                              .ThenInclude(r => r.Contrato)
                       .Include(r => r.ContratacionProyecto)
                          .ThenInclude(r => r.Proyecto)
                              .ThenInclude(r => r.InstitucionEducativa)
                       .Include(r => r.SeguimientoDiario)
                              .ThenInclude(r => r.SeguimientoDiarioObservaciones)

                          //Financiero
                          .Include(r => r.SeguimientoSemanalAvanceFinanciero)
                       //Fisico
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

                       .FirstOrDefault();

                seguimientoSemanal.SeguimientoSemanalObservacion = null;

                List<Dominio> CausaBajaDisponibilidadMaterial = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Causa_Baja_Disponibilidad_Material).ToList();

                List<Dominio> CausaBajaDisponibilidadEquipo = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Causa_Baja_Disponibilidad_Equipo).ToList();

                List<Dominio> CausaBajaDisponibilidadProductividad = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Causa_Baja_Disponibilidad_Productividad).ToList();



                //Crear Comite Obra numerador

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

        public async Task<dynamic> GetListReporteSemanal()
        {
            List<SeguimientoSemanal> ListseguimientoSemanal =
                 await _context.SeguimientoSemanal.Where(r => !string.IsNullOrEmpty(r.EstadoSeguimientoSemanalCodigo))
                 .Include(r => r.ContratacionProyecto)
                    .ThenInclude(r => r.Proyecto)
                 .Include(r => r.ContratacionProyecto)
                    .ThenInclude(r => r.Contratacion)
                        .ThenInclude(r => r.Contrato)
                 .Include(r => r.SeguimientoSemanalAvanceFisico)

                 .ToListAsync();

            List<dynamic> ListBitaCora = new List<dynamic>();

            List<Dominio> ListEstadoObra = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Obra_Avance_Semanal).ToList();
            List<Dominio> ListEstadoSeguimientoSemanal = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Reporte_Semanal_Y_Muestras).ToList();

            int UltimaSemana = ListseguimientoSemanal.OrderBy(r => r.SeguimientoSemanalId).LastOrDefault().NumeroSemana;

            foreach (var item in ListseguimientoSemanal.Where(r => r.RegistroCompleto == true))
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

                if (!string.IsNullOrEmpty(item.EstadoMuestrasCodigo))
                    strCodigoEstadoMuestas = ListEstadoSeguimientoSemanal.Where(r => r.Codigo == item.EstadoMuestrasCodigo).FirstOrDefault().Nombre;

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
                    item.ContratacionProyecto?.Contratacion?.Contrato?.FirstOrDefault().NumeroContrato,
                    EstadoReporteSemanal = !string.IsNullOrEmpty(item.EstadoSeguimientoSemanalCodigo) ? ListEstadoSeguimientoSemanal.Where(r => r.Codigo == item.EstadoSeguimientoSemanalCodigo).FirstOrDefault().Nombre : "---",
                    EstadoMuestrasReporteSemanal = strCodigoEstadoMuestas,

                });
            }
            return ListBitaCora;
        }

        public async Task<List<VVerificarSeguimientoSemanal>> GetListReporteSemanalView(List<string> strListCodEstadoSeguimientoSemanal)
        {
            return await _context.VVerificarSeguimientoSemanal.ToListAsync();
        }

        #endregion

        #region Create update
        public async Task<Respuesta> CreateEditSeguimientoSemanalObservacion(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Observacion_Seguimiento_Semanal, (int)EnumeratorTipoDominio.Acciones);
            SeguimientoSemanal seguimientoSemanal = _context.SeguimientoSemanal.Find(pSeguimientoSemanalObservacion.SeguimientoSemanalId);

            try
            {
                if (pSeguimientoSemanalObservacion.TieneObservacion)
                {
                    if (pSeguimientoSemanalObservacion.EsSupervisor)
                    {
                        seguimientoSemanal.TieneObservacionSupervisor = true;
                        seguimientoSemanal.FechaModificacionAvalar = DateTime.Now;
                    }
                    else
                    {
                        seguimientoSemanal.TieneObservacionApoyo = true;
                        seguimientoSemanal.FechaModificacionVerificar = DateTime.Now;
                    }
                }
                else
                {
                    if (pSeguimientoSemanalObservacion.EsSupervisor) 
                        seguimientoSemanal.FechaModificacionAvalar = DateTime.Now; 
                    else 
                        seguimientoSemanal.FechaModificacionVerificar = DateTime.Now;  
                }

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

                    case ConstanCodigoTipoObservacionSeguimientoSemanal.GESTION_SEGURIDAD_Y_SALUD:
                        CreateOrEditObservacionGestionSeguridadSalud(pSeguimientoSemanalObservacion);
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
                seguimientoSemanalGestionObraAlertaOld.RegistroCompleto = false;
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
                seguimientoSemanalGestionObraSocialOld.RegistroCompleto = false;
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
                seguimientoSemanalGestionObraSeguridadSaludOld.RegistroCompleto = false;
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
                SeguimientoSemanalRegistrarComiteObraOld.RegistroCompleto = false;
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

        private void CreateOrEditObservacionRegistroFotografico(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion)
        {
            SeguimientoSemanalRegistroFotografico SeguimientoSemanalRegistroFotograficoOld = _context.SeguimientoSemanalRegistroFotografico.Find(pSeguimientoSemanalObservacion.ObservacionPadreId);
            SeguimientoSemanalRegistroFotograficoOld.FechaModificacion = DateTime.Now;
            SeguimientoSemanalRegistroFotograficoOld.UsuarioModificacion = pSeguimientoSemanalObservacion.UsuarioCreacion;

            if (pSeguimientoSemanalObservacion.EsSupervisor)
            {
                SeguimientoSemanalRegistroFotograficoOld.RegistroCompleto = false;

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
                seguimientoSemanalReporteActividad.RegistroCompleto = false;
                seguimientoSemanalReporteActividad.RegistroCompletoActividadSiguiente = false;

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
                seguimientoSemanalReporteActividad.RegistroCompleto = false;
                seguimientoSemanalReporteActividad.RegistroCompletoActividad = false;

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
                seguimientoSemanalReporteActividad.RegistroCompletoEstadoContrato = false;
                seguimientoSemanalReporteActividad.RegistroCompleto = false;

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
                ensayoLaboratorioMuestraOld.RegistroCompleto = false;
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
                gestionObraCalidadEnsayoLaboratorioOld.RegistroCompleto = false;
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
                seguimientoSemanalGestionObraCalidad.RegistroCompleto = false;
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
                manejoOtroOld.RegistroCompleto = false;
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
                manejoResiduosPeligrososEspecialesOld.RegistroCompleto = false;
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
                manejoResiduosConstruccionDemolicionOld.RegistroCompleto = false;
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
                manejoMaterialesInsumosOld.RegistroCompleto = false;
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
                seguimientoSemanalGestionObraAmbientalOld.RegistroCompleto = false;

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
                seguimientoSemanalAvanceFinancieroOld.RegistroCompleto = false;

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
                seguimientoSemanalAvanceFisicoOld.RegistroCompleto = false;
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

        private bool CompleteRecordObservation(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion)
        {
            if (pSeguimientoSemanalObservacion.TieneObservacion == true && string.IsNullOrEmpty(pSeguimientoSemanalObservacion.Observacion))
                return false;

            if (pSeguimientoSemanalObservacion.TieneObservacion == false)
                return true;
             
            if (pSeguimientoSemanalObservacion.TieneObservacion == true && !string.IsNullOrEmpty(Helpers.Helpers.HtmlConvertirTextoPlano(Helpers.Helpers.HtmlConvertirTextoPlano(pSeguimientoSemanalObservacion.Observacion))))
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

        #endregion
    }
}
