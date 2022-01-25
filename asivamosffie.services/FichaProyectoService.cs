﻿using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class FichaProyectoService : IFichaProyectoService
    {
        private readonly devAsiVamosFFIEContext _context;


        public FichaProyectoService(devAsiVamosFFIEContext context)
        {
            _context = context;
        }

        #region Preparacion
        public async Task<dynamic> GetInfoPreparacionByProyectoId(int pProyectoId)
        {
            return new
            {

                Informacion = await _context.VFichaProyectoInfoContratacionProyecto.Where(r => r.ProyectoId == pProyectoId)
                                                                                  .Select(r => new
                                                                                  {
                                                                                      r.LlaveMen,
                                                                                      r.InstitucionEducativa,
                                                                                      r.Sede,
                                                                                      r.Departamento,
                                                                                      r.Municipio,
                                                                                      r.TipoIntervencion
                                                                                  }
                                                                                         )
                                                                                .FirstOrDefaultAsync(),

                Preconstruccion = GetGetInfoPreparacionPreConstruccionByProyectoId(pProyectoId),

                Construccion = GetGetInfoPreparacionConstruccionByProyectoId(pProyectoId)
            };
        }

        public async Task<dynamic> GetGetInfoPreparacionConstruccionByProyectoId(int pProyectoId)
        {

            List<VProyectosXcontrato> ListContratosXProyecto = _context.VProyectosXcontrato.Where(v => v.ProyectoId == pProyectoId).ToList();

            List<dynamic> Info = new List<dynamic>();

            foreach (var Contrato in ListContratosXProyecto)
            {
                ContratoConstruccion ContratoConstruccion = _context.ContratoConstruccion.Where(v => v.ContratoId == Contrato.ContratoId && v.ProyectoId == pProyectoId).FirstOrDefault();

                if (ContratoConstruccion != null)
                {
                    Info.Add(new
                    {
                        CodigoTipoContrato = Contrato.TipoContratoCodigo,
                        NombreTipoContrato = Contrato.NombreTipoContrato,
                        Diagnostico = GetDiagnosticoByContratoConstruccion(ContratoConstruccion),
                        PlanesYProgramas = GetPlantesYProgramasByContratoConstruccion(ContratoConstruccion),
                        ManejoAnticipo = GetManejoAnticipo(ContratoConstruccion),
                        HojasDeVida = GetHojasDeVida(ContratoConstruccion)
                    });
                }
            }
            return Info;
        }

        private object GetHojasDeVida(ContratoConstruccion pContratoConstruccion)
        {
            List<VFichaProyectoPreparacionConstruccion> ListFichaProyectoPreparacionConstruccion = _context.VFichaProyectoPreparacionConstruccion.Where(c => c.ContratoConstruccionId == pContratoConstruccion.ContratoConstruccionId).ToList();

            foreach (var item in ListFichaProyectoPreparacionConstruccion)
            {
                item.ListRadicados = _context.ConstruccionPerfilNumeroRadicado.Where(r => r.ConstruccionPerfilId == item.ConstruccionPerfilId)
                                                                              .Select(c => new { c.NumeroRadicado })
                                                                              .ToList();
            }

            return ListFichaProyectoPreparacionConstruccion;
        }

        private object GetManejoAnticipo(ContratoConstruccion pContratoConstruccion)
        {
            return new
            {
                RequiereAnticipo = pContratoConstruccion.ManejoAnticipoRequiere,
                TienePlanInversionAprobado = pContratoConstruccion.ManejoAnticipoPlanInversion,
                TieneCronogramaAmortizacion = pContratoConstruccion.ManejoAnticipoCronogramaAmortizacion,
                UrlSoporte = pContratoConstruccion.ManejoAnticipoRutaSoporte
            };
        }

        private object GetPlantesYProgramasByContratoConstruccion(ContratoConstruccion pContratoConstruccion)
        {
            return new
            {
                PlanLicenciaVigente = new
                {
                    RecibioRequisito = pContratoConstruccion.PlanLicenciaVigente,
                    FechaRadicado = pContratoConstruccion.LicenciaFechaRadicado,
                    FechaAprobacion = pContratoConstruccion.LicenciaFechaAprobacion,
                    TieneObservaciones = pContratoConstruccion.LicenciaConObservaciones
                },
                PlanCambioConstructorLicencia = new
                {
                    RecibioRequisito = pContratoConstruccion.PlanCambioConstructorLicencia,
                    FechaRadicado = pContratoConstruccion.CambioFechaRadicado,
                    FechaAprobacion = pContratoConstruccion.CambioFechaAprobacion,
                    TieneObservaciones = pContratoConstruccion.CambioConObservaciones
                },
                PlanActaApropiacion = new
                {
                    RecibioRequisito = pContratoConstruccion.PlanActaApropiacion,
                    FechaRadicado = pContratoConstruccion.ActaApropiacionFechaRadicado,
                    FechaAprobacion = pContratoConstruccion.ActaApropiacionFechaAprobacion,
                    TieneObservaciones = pContratoConstruccion.ActaApropiacionConObservaciones
                },
                PlanResiduosDemolicion = new
                {
                    RecibioRequisito = pContratoConstruccion.PlanResiduosDemolicion,
                    FechaRadicado = pContratoConstruccion.ResiduosDemolicionFechaRadicado,
                    FechaAprobacion = pContratoConstruccion.ResiduosDemolicionFechaAprobacion,
                    TieneObservaciones = pContratoConstruccion.ResiduosDemolicionConObservaciones
                },
                PlanManejoTransito = new
                {
                    RecibioRequisito = pContratoConstruccion.PlanManejoTransito,
                    FechaRadicado = pContratoConstruccion.ManejoTransitoFechaRadicado,
                    FechaAprobacion = pContratoConstruccion.ManejoTransitoFechaAprobacion,
                    TieneObservaciones = pContratoConstruccion.ManejoTransitoConObservaciones1
                },
                PlanManejoAmbiental = new
                {
                    RecibioRequisito = pContratoConstruccion.PlanManejoAmbiental,
                    FechaRadicado = pContratoConstruccion.ManejoAmbientalFechaRadicado,
                    FechaAprobacion = pContratoConstruccion.ManejoAmbientalFechaAprobacion,
                    TieneObservaciones = pContratoConstruccion.ManejoAmbientalConObservaciones
                },
                PlanProgramaSeguridad = new
                {
                    RecibioRequisito = pContratoConstruccion.PlanProgramaSeguridad,
                    FechaRadicado = pContratoConstruccion.ProgramaSeguridadFechaRadicado,
                    FechaAprobacion = pContratoConstruccion.ProgramaSeguridadFechaAprobacion,
                    TieneObservaciones = pContratoConstruccion.ProgramaSeguridadConObservaciones
                },
                PlanProgramaSalud = new
                {
                    RecibioRequisito = pContratoConstruccion.PlanProgramaSalud,
                    FechaRadicado = pContratoConstruccion.ProgramaSaludFechaRadicado,
                    FechaAprobacion = pContratoConstruccion.ProgramaSaludFechaAprobacion,
                    TieneObservaciones = pContratoConstruccion.ProgramaSaludConObservaciones
                },
                PlanInventarioArboreo = new
                {
                    RecibioRequisito = pContratoConstruccion.PlanInventarioArboreo,
                    FechaRadicado = pContratoConstruccion.InventarioArboreoFechaRadicado,
                    FechaAprobacion = pContratoConstruccion.InventarioArboreoFechaAprobacion,
                    TieneObservaciones = pContratoConstruccion.InventarioArboreoConObservaciones
                },
                PlanAprovechamientoForestal = new
                {
                    RecibioRequisito = pContratoConstruccion.PlanAprovechamientoForestal,
                    FechaRadicado = pContratoConstruccion.AprovechamientoForestalApropiacionFechaRadicado,
                    FechaAprobacion = pContratoConstruccion.AprovechamientoForestalFechaAprobacion,
                    TieneObservaciones = pContratoConstruccion.AprovechamientoForestalConObservaciones
                },
                PlanManejoAguasLluvias = new
                {
                    RecibioRequisito = pContratoConstruccion.PlanManejoAguasLluvias,
                    FechaRadicado = pContratoConstruccion.ManejoAguasLluviasFechaRadicado,
                    FechaAprobacion = pContratoConstruccion.ManejoAguasLluviasFechaAprobacion,
                    TieneObservaciones = pContratoConstruccion.ManejoAguasLluviasConObservaciones
                }
            };
        }

        private object GetDiagnosticoByContratoConstruccion(ContratoConstruccion pContratoConstruccion)
        {
            return new
            {
                CuentaConInformeDiagnosticoAprobado = pContratoConstruccion.EsInformeDiagnostico,
                UrlSoporte = pContratoConstruccion.RutaInforme,
                CostoDirecto = pContratoConstruccion.CostoDirecto,
                Administracion = pContratoConstruccion.Administracion,
                Imprevistos = pContratoConstruccion.Imprevistos,
                Utilidad = pContratoConstruccion.Utilidad,
                ValorTotalConstruccion = pContratoConstruccion.ValorTotalFaseConstruccion,
                TieneModificacionContractual = pContratoConstruccion.RequiereModificacionContractual,
                NumeroModificacion = pContratoConstruccion.NumeroSolicitudModificacion
            };

        }

        public async Task<dynamic> GetGetInfoPreparacionPreConstruccionByProyectoId(int pProyectoId)
        {

            List<VProyectosXcontrato> ListContratosXProyecto = _context.VProyectosXcontrato.Where(v => v.ProyectoId == pProyectoId).ToList();

            List<dynamic> Info = new List<dynamic>();

            foreach (var Contrato in ListContratosXProyecto)
            {
                Info.Add(new
                {
                    NombreTipoContrato = Contrato.NombreTipoContrato,
                    TipoContratoCodigo = Contrato.TipoContratoCodigo,
                    ActaSuscrita = Contrato.ActaSuscrita,
                    NumeroContrato = Contrato.NumeroContrato,
                    TablaPreconstruccion = _context.VFichaProyectoPreparacionPreconstruccion.Where(r => r.ProyectoId == pProyectoId
                                                                                                     && r.ContratacionId == Contrato.ContratacionId)
                                                                                            .ToList()
                });
            }
            return Info;
        }


        #endregion

        #region Busqueda
        public async Task<dynamic> GetFlujoProyectoByProyectoId(int pProyectoId)
        {
            return new
            {
                Informacion = _context.VFichaProyectoInfoContratacionProyecto.Where(x => x.ProyectoId == pProyectoId).FirstOrDefault(),
                TieneResumen = _context.VFichaProyectoTieneResumen.Any(cp => cp.ProyectoId == pProyectoId),
                TieneContratacion = _context.VFichaProyectoTieneContratacion.Any(cp => cp.ProyectoId == pProyectoId),
                TienePreparacion = _context.VFichaProyectoTienePreparacion.Any(cp => cp.ProyectoId == pProyectoId),
                TieneSeguimientoTecnico = _context.VFichaProyectoTieneSeguimientoTecnico.Any(cp => cp.ProyectoId == pProyectoId),
                TieneSeguimientoFinanciero = _context.VFichaProyectoTieneSeguimientoFinanciero.Any(cp => cp.ProyectoId == pProyectoId),
                TieneEntrega = _context.VFichaProyectoTieneEntrega.Any(cp => cp.ProyectoId == pProyectoId),
            };
        }

        public async Task<dynamic> GetVigencias()
        {
            return await _context.Cofinanciacion.Select(c => c.VigenciaCofinanciacionId)
                                                .OrderByDescending(c => c.Value)
                                                .ToListAsync();
        }
        public async Task<dynamic> GetProyectoIdByLlaveMen(string pLlaveMen)
        {
            return await _context.VFichaProyectoBusquedaProyecto.Where(f => f.LlaveMen.ToUpper().Contains(pLlaveMen.ToUpper()))
                                                                .OrderByDescending(p => p.ProyectoId).ToListAsync();
        }
        public async Task<dynamic> GetTablaProyectosByProyectoIdTipoContratacionVigencia(int pProyectoId, string pTipoIntervencion, int pVigencia)
        {

            List<VFichaProyectoBusquedaProyectoTabla> ListProyectos = await _context.VFichaProyectoBusquedaProyectoTabla.Where(p => p.ProyectoId == pProyectoId).ToListAsync();

            if (!string.IsNullOrEmpty(pTipoIntervencion))
                ListProyectos = ListProyectos.Where(p => p.CodigoTipoIntervencion == pTipoIntervencion).ToList();

            if (pVigencia > 0)
                ListProyectos = ListProyectos.Where(p => p.Vigencia == pVigencia).ToList();

            return ListProyectos;
        }

        #endregion
    }
}
