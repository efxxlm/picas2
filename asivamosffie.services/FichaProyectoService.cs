using asivamosffie.model.APIModels;
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
        private readonly IRequestBudgetAvailabilityService _requestBudgetAvailabilityService;
        public FichaProyectoService(devAsiVamosFFIEContext context, IRequestBudgetAvailabilityService requestBudgetAvailabilityService)
        {
            _requestBudgetAvailabilityService = requestBudgetAvailabilityService;
            _context = context;
        }


        #region Entrega

        public async Task<dynamic> GetInfoSeguimientoEntregaETCByProyectoId(int pProyectoId)
        {

            Proyecto proyecto = await _context.Proyecto.Where(p => p.ProyectoId == pProyectoId)
                                                      .Include(r=> r.InstitucionEducativa)
                                                      .Include(r => r.Sede)
                                                      .Include(inf => inf.InformeFinal).ThenInclude(etc => etc.ProyectoEntregaEtc)
                                                      .FirstOrDefaultAsync();

            List<Dominio> ListTipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();


            return new
            {
                LlaveMen = proyecto.LlaveMen,
                InstitucionEducativa = proyecto.InstitucionEducativa.Nombre,
                Sede = proyecto.Sede.Nombre,
                TipoIntervencion = ListTipoIntervencion.Where(r => r.Codigo == proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre, 
                FechaRecorridoObra = proyecto.InformeFinal?.FirstOrDefault().ProyectoEntregaEtc?.FechaRecorridoObra,
                FechaFirmaActaEntregaFisica = proyecto.InformeFinal?.FirstOrDefault().ProyectoEntregaEtc?.FechaFirmaActaEngregaFisica,
                FechaFirmaActaEntregaBienesServicios = proyecto.InformeFinal?.FirstOrDefault().ProyectoEntregaEtc?.FechaFirmaActaBienesServicios,
                NumeroRadicadoEntrega = proyecto.InformeFinal?.FirstOrDefault().ProyectoEntregaEtc?.NumRadicadoDocumentosEntregaEtc
            };

        }
        #endregion

        #region Seguimiento Financiero 

        public async Task<dynamic> GetInfoSeguimientoFinancieroByProyectoId(int pProyectoId)
        {
            List<VProyectosXcontrato> ListContratosXProyecto = _context.VProyectosXcontrato.Where(v => v.ProyectoId == pProyectoId)
                                                                                           .ToList();

            foreach (var Contrato in ListContratosXProyecto)
            {
                var SeguimientoDiario = _context.VFichaProyectoSeguimientoTecnicoDiario.Where(v => v.ContratoId == Contrato.ContratoId)
                                                                                       .OrderByDescending(v => v.SeguimientoDiarioId)
                                                                                       .ToList();

                var SeguimientoSemanal = _context.VFichaProyectoSeguimientoTecnicoSemanal.Where(v => v.ContratoId == Contrato.ContratoId)
                                                                                         .OrderByDescending(v => v.SeguimientoSemanalAvanceFisicoId)
                                                                                         .ToList();

                Contrato.SeguimientoTecnico = new
                {
                    SeguimientoDiario,
                    SeguimientoSemanal
                };
            }

            return new
            {
                InfoProyecto = await _context.VFichaProyectoInfoContratacionProyecto.Where(r => r.ProyectoId == pProyectoId)
                                                                                    .Select(r => new
                                                                                    {
                                                                                        r.NumeroContrato,
                                                                                        r.NumeroContratacion,
                                                                                        r.LlaveMen,
                                                                                        r.InstitucionEducativa,
                                                                                        r.Sede,
                                                                                        r.TipoIntervencion
                                                                                    }
                                                                                            )
                                                                                   .FirstOrDefaultAsync(),

                SeguimientoTecnico = ListContratosXProyecto.Select(r => new
                {
                    r.NumeroContrato,
                    r.TipoSolicitudCodigo,
                    r.NombreTipoContrato,
                    r.SeguimientoTecnico
                }).ToList()
            };
        }

        #endregion

        #region seguimiento Tecnico
        public async Task<dynamic> GetInfoSeguimientoTecnicoByProyectoId(int pProyectoId)
        {
            List<VProyectosXcontrato> ListContratosXProyecto = _context.VProyectosXcontrato.Where(v => v.ProyectoId == pProyectoId
                                                                                                    && v.TipoContratoCodigo == ConstanCodigoTipoContratacionString.Obra)
                                                                                           .ToList();

            foreach (var Contrato in ListContratosXProyecto)
            {
                var SeguimientoDiario = _context.VFichaProyectoSeguimientoTecnicoDiario.Where(v => v.ContratoId == Contrato.ContratoId)
                                                                                       .OrderByDescending(v => v.SeguimientoDiarioId)
                                                                                       .ToList();

                var SeguimientoSemanal = _context.VFichaProyectoSeguimientoTecnicoSemanal.Where(v => v.ContratoId == Contrato.ContratoId)
                                                                                         .OrderByDescending(v => v.SeguimientoSemanalAvanceFisicoId)
                                                                                         .ToList();

                Contrato.SeguimientoTecnico = new
                {
                    SeguimientoDiario,
                    SeguimientoSemanal
                };
            }

            return new
            {
                InfoProyecto = await _context.VFichaProyectoInfoContratacionProyecto.Where(r => r.ProyectoId == pProyectoId)
                                                                                    .Select(r => new
                                                                                    {
                                                                                        r.NumeroContrato,
                                                                                        r.NumeroContratacion,
                                                                                        r.LlaveMen,
                                                                                        r.InstitucionEducativa,
                                                                                        r.Sede,
                                                                                        r.TipoIntervencion
                                                                                    }
                                                                                            )
                                                                                   .FirstOrDefaultAsync(),

                SeguimientoTecnico = ListContratosXProyecto.Select(r => new
                {
                    r.NumeroContrato,
                    r.TipoSolicitudCodigo,
                    r.NombreTipoContrato,
                    r.SeguimientoTecnico
                }).ToList()
            };
        }
        #endregion

        #region contratacion
        public async Task<dynamic> GetInfoContratoByProyectoId(int pProyectoId)
        {
            List<VProyectosXcontrato> ListContratosXProyecto = _context.VProyectosXcontrato.Where(v => v.ProyectoId == pProyectoId).ToList();

            foreach (var Contrato in ListContratosXProyecto)
            {
                Contrato.CantidadProyectosAsosiados = _context.VProyectosXcontrato.Count(v => v.ContratoId == Contrato.ContratoId);

                Contrato.InfoProyectos = _context.VProyectosXcontrato.Where(v => v.ContratoId == Contrato.ContratoId)
                                                                     .Select(v => new
                                                                     {
                                                                         TipoIntervencion = v.TipoIntervencion,
                                                                         LlaveMen = v.LlaveMen,
                                                                         Region = v.Region,
                                                                         Departamento = v.Departamento,
                                                                         Muncipio = v.Municipio,
                                                                         InstitucionEducativaSede = v.InstitucionEducativa + "/ " + v.Sede,
                                                                     })
                                                                     .ToList();

                Contrato.ListProcesoSeleccion = _context.VFichaProyectoContratacionProcesoSeleccion.Where(c => c.ContratoId == Contrato.ContratoId).ToList();

                foreach (var ProcesoSeleccion in Contrato.ListProcesoSeleccion)
                {

                    ProcesoSeleccion.ProcesoSeleccionCronograma = _context.VFichaProyectoContratacionProcesoSeleccionCronograma.Where(p => p.ProcesoSeleccionId == ProcesoSeleccion.ProcesoSeleccionId)
                                                                                                                               .Select(p => new
                                                                                                                               {
                                                                                                                                   p.NombreEtapa,
                                                                                                                                   p.EstapaCodigo,
                                                                                                                                   p.FechaEtapa
                                                                                                                               }
                                                                                                                                      )
                                                                                                                               .OrderBy(p => p.EstapaCodigo)
                                                                                                                               .ToList();
                }
            }

            return new
            {
                InfoProyecto = await _context.VFichaProyectoInfoContratacionProyecto.Where(r => r.ProyectoId == pProyectoId)
                                                                                    .Select(r => new
                                                                                    {
                                                                                        r.NumeroContrato,
                                                                                        r.NumeroContratacion,
                                                                                        r.LlaveMen,
                                                                                        r.InstitucionEducativa,
                                                                                        r.Sede,
                                                                                        r.TipoIntervencion
                                                                                    }
                                                                                            )
                                                                                   .FirstOrDefaultAsync(),

                InfoProyectosXContrato = ListContratosXProyecto.Select(s => new
                {
                    s.NumeroSolicitud,
                    s.FechaSuscripcion,
                    s.UrlSoporteGestionar,
                    s.NumeroPoliza,
                    s.FechaSolicitud,
                    s.TipoContratoCodigo,
                    s.ValorTotal,
                    s.CantidadProyectosAsosiados,
                    s.NumeroContrato,
                    s.ContratoId,
                    s.ContratoSuscrito,
                    s.NombreTipoContrato,
                    s.InfoProyectos,
                    s.ListProcesoSeleccion
                }
                                                                      )
                                                               .ToList()
            };
        }
        #endregion

        #region Resumen
        public async Task<dynamic> GetInfoResumenByProyectoId(int pProyectoId)
        {
            //alcance 
            List<VProyectosXcontrato> ListContratosXProyecto = _context.VProyectosXcontrato.Where(v => v.ProyectoId == pProyectoId).ToList();

            foreach (var contrato in ListContratosXProyecto)
            {
                List<VFichaProyectoResumenFuentesYusos> ListVFichaProyectoResumenFuentesYusos = await _context.VFichaProyectoResumenFuentesYusos.Where(p => p.ProyectoId == pProyectoId
                                                                                                && p.ContratacionId == contrato.ContratacionId)
                                                                                        .ToListAsync();


                foreach (var ProyectoResumenFuentesYusos in ListVFichaProyectoResumenFuentesYusos)
                {
                    ProyectoResumenFuentesYusos.NombreAportante = _requestBudgetAvailabilityService.getNombreAportante(_context.CofinanciacionAportante.Find(ProyectoResumenFuentesYusos.ContratacionProyectoAportanteId));
                }


                contrato.InfoContrato = ListVFichaProyectoResumenFuentesYusos;
            }

            var Infraestructura = _context.VFichaProyectoResumenInfraestructura.Where(d => d.ProyectoId == pProyectoId).ToList();
            return new
            {
                InfoProyecto = await _context.VFichaProyectoInfoContratacionProyecto.Where(r => r.ProyectoId == pProyectoId)
                                                                                    .Select(r => new
                                                                                    {
                                                                                        r.TipoIntervencion,
                                                                                        r.LlaveMen,
                                                                                        r.Departamento,
                                                                                        r.Municipio,
                                                                                        r.InstitucionEducativa,
                                                                                        r.CodigoDaneInstitucionEducativa,
                                                                                        r.Sede,
                                                                                        r.CodigoDaneSede,
                                                                                        r.UbicacionLatitud,
                                                                                        r.UbicacionLongitud,
                                                                                        r.PlazoMesesObra,
                                                                                        r.PlazoDiasObra,
                                                                                        r.ValorObra,
                                                                                        r.PlazoMesesInterventoria,
                                                                                        r.PlazoDiasInterventoria,
                                                                                        r.ValorInterventoria,
                                                                                        Infraestructura
                                                                                    })
                                                                                   .FirstOrDefaultAsync(),

                InfoContratos = ListContratosXProyecto.Select(i => new
                {
                    i.NombreContratista,
                    i.NumeroIdentificacion,
                    i.RepresentanteLegal,
                    i.RepresentanteLegalNumeroIdentificacion,
                    i.NumeroInvitacion,
                    i.NumeroContrato,
                    i.TipoSolicitudCodigo,
                    i.NombreTipoContrato,
                    i.InfoContrato,
                    i.ValorTotal
                }
                                                              )
                                                      .ToList()

            };
        }


        #endregion

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

        public object GetGetInfoPreparacionConstruccionByProyectoId(int pProyectoId)
        {

            List<VProyectosXcontrato> ListContratosXProyecto = _context.VProyectosXcontrato.Where(v => v.ProyectoId == pProyectoId).ToList();

            List<dynamic> Info = new List<dynamic>();

            foreach (var Contrato in ListContratosXProyecto)
            {
                ContratoConstruccion ContratoConstruccion = _context.ContratoConstruccion.Where(v => v.ContratoId == Contrato.ContratoId && v.ProyectoId == pProyectoId)
                                                                                         .Include(c => c.Contrato)
                                                                                         .FirstOrDefault();

                if (ContratoConstruccion != null)
                {
                    Info.Add(new
                    {
                        NumeroContrato = Contrato.NumeroContrato,
                        CodigoTipoContrato = Contrato.TipoContratoCodigo,
                        NombreTipoContrato = Contrato.NombreTipoContrato,
                        Diagnostico = GetDiagnosticoByContratoConstruccion(ContratoConstruccion),
                        PlanesYProgramas = GetPlantesYProgramasByContratoConstruccion(ContratoConstruccion),
                        ManejoAnticipo = GetManejoAnticipo(ContratoConstruccion),
                        HojasDeVida = GetHojasDeVida(ContratoConstruccion),
                        ProgramacionObra = GetProgramacionObra(ContratoConstruccion),
                        FlujoInversion = GetFlujoInversion(ContratoConstruccion),
                        ActaSuscrita = ContratoConstruccion.Contrato.RutaActaSuscrita
                    });
                }
            }
            return Info;
        }

        private object GetFlujoInversion(ContratoConstruccion pContratoConstruccion)
        {
            return new
            {
                RutaArchivo = pContratoConstruccion.ArchivoCargueIdFlujoInversion
            };
        }

        private object GetProgramacionObra(ContratoConstruccion pContratoConstruccion)
        {
            return new
            {
                RutaArchivo = pContratoConstruccion.ArchivoCargueIdProgramacionObra
            };
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
                UrlConSoporte = pContratoConstruccion.PlanRutaSoporte,

                PlanLicenciaVigente = new
                {
                    Nombre = "Licencia vigente",
                    RecibioRequisito = pContratoConstruccion.PlanLicenciaVigente,
                    FechaRadicado = pContratoConstruccion.LicenciaFechaRadicado,
                    FechaAprobacion = pContratoConstruccion.LicenciaFechaAprobacion,
                    TieneObservaciones = pContratoConstruccion.LicenciaConObservaciones
                },
                PlanCambioConstructorLicencia = new
                {
                    Nombre = "Cambio constructor responsable de la licencia",
                    RecibioRequisito = pContratoConstruccion.PlanCambioConstructorLicencia,
                    FechaRadicado = pContratoConstruccion.CambioFechaRadicado,
                    FechaAprobacion = pContratoConstruccion.CambioFechaAprobacion,
                    TieneObservaciones = pContratoConstruccion.CambioConObservaciones
                },
                PlanActaApropiacion = new
                {
                    Nombre = "Acta aceptación y apropiación diseños",
                    RecibioRequisito = pContratoConstruccion.PlanActaApropiacion,
                    FechaRadicado = pContratoConstruccion.ActaApropiacionFechaRadicado,
                    FechaAprobacion = pContratoConstruccion.ActaApropiacionFechaAprobacion,
                    TieneObservaciones = pContratoConstruccion.ActaApropiacionConObservaciones
                },
                PlanResiduosDemolicion = new
                {
                    Nombre = "¿Cuenta con plan de residuos de construcción y demolición (RCD) aprobado?",
                    RecibioRequisito = pContratoConstruccion.PlanResiduosDemolicion,
                    FechaRadicado = pContratoConstruccion.ResiduosDemolicionFechaRadicado,
                    FechaAprobacion = pContratoConstruccion.ResiduosDemolicionFechaAprobacion,
                    TieneObservaciones = pContratoConstruccion.ResiduosDemolicionConObservaciones
                },
                PlanManejoTransito = new
                {
                    Nombre = "¿Cuenta con plan de manejo de tránsito (PMT) aprobado?",
                    RecibioRequisito = pContratoConstruccion.PlanManejoTransito,
                    FechaRadicado = pContratoConstruccion.ManejoTransitoFechaRadicado,
                    FechaAprobacion = pContratoConstruccion.ManejoTransitoFechaAprobacion,
                    TieneObservaciones = pContratoConstruccion.ManejoTransitoConObservaciones1
                },
                PlanManejoAmbiental = new
                {
                    Nombre = "¿Cuenta con plan de manejo ambiental aprobado?",
                    RecibioRequisito = pContratoConstruccion.PlanManejoAmbiental,
                    FechaRadicado = pContratoConstruccion.ManejoAmbientalFechaRadicado,
                    FechaAprobacion = pContratoConstruccion.ManejoAmbientalFechaAprobacion,
                    TieneObservaciones = pContratoConstruccion.ManejoAmbientalConObservaciones
                },
                PlanMansejoAmbiental = new
                {
                    Nombre = "¿Cuenta con plan de aseguramiento de la calidad de obra aprobado?",
                    RecibioRequisito = pContratoConstruccion.PlanAseguramientoCalidad,
                    FechaRadicado = pContratoConstruccion.AseguramientoCalidadFechaRadicado,
                    FechaAprobacion = pContratoConstruccion.AseguramientoCalidadFechaAprobacion,
                    TieneObservaciones = pContratoConstruccion.AseguramientoCalidadConObservaciones
                },
                PlanProgramaSeguridad = new
                {
                    Nombre = "¿Cuenta con programa de Seguridad industrial aprobado?",
                    RecibioRequisito = pContratoConstruccion.PlanProgramaSeguridad,
                    FechaRadicado = pContratoConstruccion.ProgramaSeguridadFechaRadicado,
                    FechaAprobacion = pContratoConstruccion.ProgramaSeguridadFechaAprobacion,
                    TieneObservaciones = pContratoConstruccion.ProgramaSeguridadConObservaciones
                },
                PlanProgramaSalud = new
                {
                    Nombre = "¿Cuenta con programa de salud ocupacional aprobado?",
                    RecibioRequisito = pContratoConstruccion.PlanProgramaSalud,
                    FechaRadicado = pContratoConstruccion.ProgramaSaludFechaRadicado,
                    FechaAprobacion = pContratoConstruccion.ProgramaSaludFechaAprobacion,
                    TieneObservaciones = pContratoConstruccion.ProgramaSaludConObservaciones
                },
                PlanInventarioArboreo = new
                {
                    Nombre = "¿Cuenta con un plan inventario arbóreo (talas) aprobado?",
                    RecibioRequisito = pContratoConstruccion.PlanInventarioArboreo,
                    FechaRadicado = pContratoConstruccion.InventarioArboreoFechaRadicado,
                    FechaAprobacion = pContratoConstruccion.InventarioArboreoFechaAprobacion,
                    TieneObservaciones = pContratoConstruccion.InventarioArboreoConObservaciones
                },
                PlanAprovechamientoForestal = new
                {
                    Nombre = "¿Cuenta con plan de aprovechamiento forestal aprobado?",
                    RecibioRequisito = pContratoConstruccion.PlanAprovechamientoForestal,
                    FechaRadicado = pContratoConstruccion.AprovechamientoForestalApropiacionFechaRadicado,
                    FechaAprobacion = pContratoConstruccion.AprovechamientoForestalFechaAprobacion,
                    TieneObservaciones = pContratoConstruccion.AprovechamientoForestalConObservaciones
                },
                PlanManejoAguasLluvias = new
                {
                    Nombre = "¿Cuenta con plan de manejo de aguas lluvias aprobado?",
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

        public object GetGetInfoPreparacionPreConstruccionByProyectoId(int pProyectoId)
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
                                                .Distinct().ToListAsync();
        }
        public async Task<dynamic> GetProyectoIdByLlaveMen(string pLlaveMen)
        {
            return await _context.VFichaProyectoBusquedaProyecto.Where(f => f.LlaveMen.ToUpper().Contains(pLlaveMen.ToUpper()))
                                                                .OrderByDescending(p => p.ProyectoId).ToListAsync();
        }
        public async Task<dynamic> GetTablaProyectosByProyectoIdTipoContratacionVigencia(int pProyectoId, string pTipoIntervencion, int pVigencia)
        { 
            return await _context.VFichaProyectoBusquedaProyectoTabla
                                                                    .Where
                                                                    (p => p.ProyectoId == pProyectoId
                                                                       && p.CodigoTipoIntervencion == (string.IsNullOrEmpty(pTipoIntervencion) ? p.CodigoTipoIntervencion : pTipoIntervencion)
                                                                       && p.Vigencia == (pVigencia > 0 ? pVigencia :p.Vigencia)
                                                                    ).ToListAsync(); 
        }

        #endregion  
    }
}
