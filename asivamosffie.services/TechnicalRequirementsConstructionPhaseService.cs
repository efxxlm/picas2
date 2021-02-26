using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using lalupa.Authorization.JwtHelpers;
using asivamosffie.api.Controllers;
using AuthorizationTest.JwtHelpers;
using asivamosffie.services.Exceptions;
using asivamosffie.services.Helpers;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.model.APIModels;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using System.IO;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Globalization;
using OfficeOpenXml;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using asivamosffie.services.Helpers.Constants;

namespace asivamosffie.services
{
    public class TechnicalRequirementsConstructionPhaseService : ITechnicalRequirementsConstructionPhaseService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly IDocumentService _documentService;
        public readonly IConverter _converter;
        public readonly IRegisterPreContructionPhase1Service _registerPreContructionPhase1Service;
        public readonly IBudgetAvailabilityService _budgetAvailabilityService;

        public TechnicalRequirementsConstructionPhaseService(IConverter converter,
                                                            devAsiVamosFFIEContext context,
                                                            ICommonService commonService,
                                                            IDocumentService documentService,
                                                            IRegisterPreContructionPhase1Service registerPreContructionPhase1Service,
                                                            IBudgetAvailabilityService budgetAvailabilityService)
        {
            _converter = converter;
            _context = context;
            _documentService = documentService;
            _commonService = commonService;
            _registerPreContructionPhase1Service = registerPreContructionPhase1Service;
            _budgetAvailabilityService = budgetAvailabilityService;
        }



        #region gets

        public async Task<List<dynamic>> GetContractsGrid(int pUsuarioId)
        {
            List<dynamic> listaContrats = new List<dynamic>();

            List<VRequisitosTecnicosInicioConstruccion> lista = _context.VRequisitosTecnicosInicioConstruccion.ToList();

            lista.Where(c => c.TipoContratoCodigo == "1").ToList() // tipo contrato obra
                .ForEach(c =>
                {
                    if (c.TieneFaseConstruccion > 0)
                    { // fase construccion
                        if (c.TieneFasePreconstruccion > 0)
                        {
                            if (!string.IsNullOrEmpty(c.RutaActaFase1) && c.FechaActaInicioFase1 != null)
                            {
                                listaContrats.Add(new
                                {
                                    ContratoId = c.ContratoId,
                                    FechaAprobacion = c.FechaAprobacion,
                                    NumeroContrato = c.NumeroContrato,
                                    CantidadProyectosAsociados = c.CantidadProyectosAsociados,
                                    CantidadProyectosRequisitosAprobados = c.CantidadProyectosRequisitosAprobados,
                                    CantidadProyectosRequisitosPendientes = c.CantidadProyectosAsociados - c.CantidadProyectosRequisitosAprobados,
                                    EstadoCodigo = c.EstadoCodigo,
                                    EstadoNombre = c.EstadoNombre, //string.IsNullOrEmpty( c.EstadoCodigo ) ? "Sin verificación de requisitos técnicos" : c.EstadoNombre,
                                    Existeregistro = c.ExisteRegistro,
                                    c.EstaDevuelto,
                                    c.RegistroCompletoConstruccion,

                                });
                            }
                        }
                        else
                        { // sin preconstruccion
                            listaContrats.Add(new
                            {
                                ContratoId = c.ContratoId,
                                FechaAprobacion = c.FechaAprobacion,
                                NumeroContrato = c.NumeroContrato,
                                CantidadProyectosAsociados = c.CantidadProyectosAsociados,
                                CantidadProyectosRequisitosAprobados = c.CantidadProyectosRequisitosAprobados,
                                CantidadProyectosRequisitosPendientes = c.CantidadProyectosAsociados - c.CantidadProyectosRequisitosAprobados,
                                EstadoCodigo = c.EstadoCodigo,
                                EstadoNombre = c.EstadoNombre, //string.IsNullOrEmpty( c.EstadoCodigo ) ? "Sin verificación de requisitos técnicos" : c.EstadoNombre,
                                Existeregistro = c.ExisteRegistro,
                                c.EstaDevuelto,
                                c.RegistroCompletoConstruccion,

                            });
                        }
                    }
                });
            return listaContrats;
        }

        public async Task<List<dynamic>> GetContractsGridApoyoObra()
        {
            List<dynamic> listaContrats = new List<dynamic>();

            List<VRequisitosTecnicosInicioConstruccion> lista = _context.VRequisitosTecnicosInicioConstruccion.ToList();

            lista.Where(
                        c => c.TipoContratoCodigo == "1" &&
                        c.FechaAprobacionRequisitosConstruccionInterventor != null &&
                        (c.EstadoCodigo != null ? int.Parse(c.EstadoCodigo) : 0) >= 3
                       )
                .ToList() // tipo contrato obra
                .ForEach(c =>
                {
                    if (c.TieneFaseConstruccion > 0)
                    { // fase construccion
                        if (c.TieneFasePreconstruccion > 0)
                        {
                            if (!string.IsNullOrEmpty(c.RutaActaFase1) && c.FechaActaInicioFase1 != null)
                            {
                                listaContrats.Add(new
                                {
                                    ContratoId = c.ContratoId,
                                    FechaAprobacion = c.FechaAprobacion,
                                    NumeroContrato = c.NumeroContrato,
                                    CantidadProyectosAsociados = c.CantidadProyectosAsociados,
                                    CantidadProyectosRequisitosVerificados = c.CantidadProyectosRequisitosVerificados,
                                    CantidadProyectosRequisitosPendientes = c.CantidadProyectosAsociados - c.CantidadProyectosRequisitosVerificados,
                                    EstadoCodigo = c.EstadoCodigo,
                                    EstadoNombre = c.EstadoNombre, //string.IsNullOrEmpty( c.EstadoCodigo ) ? "Sin verificación de requisitos técnicos" : c.EstadoNombre,
                                    Existeregistro = c.ExisteRegistro,
                                    c.EstaDevuelto,
                                    c.RegistroCompletoConstruccion,
                                    c.EstadoNombreVerificacion,

                                });
                            }
                        }
                        else
                        { // sin preconstruccion
                            listaContrats.Add(new
                            {
                                ContratoId = c.ContratoId,
                                FechaAprobacion = c.FechaAprobacion,
                                NumeroContrato = c.NumeroContrato,
                                CantidadProyectosAsociados = c.CantidadProyectosAsociados,
                                CantidadProyectosRequisitosVerificados = c.CantidadProyectosRequisitosVerificados,
                                CantidadProyectosRequisitosPendientes = c.CantidadProyectosAsociados - c.CantidadProyectosRequisitosVerificados,
                                EstadoCodigo = c.EstadoCodigo,
                                EstadoNombre = c.EstadoNombre, //string.IsNullOrEmpty( c.EstadoCodigo ) ? "Sin verificación de requisitos técnicos" : c.EstadoNombre,
                                Existeregistro = c.ExisteRegistro,
                                c.EstaDevuelto,
                                c.RegistroCompletoConstruccion,
                                c.EstadoNombreVerificacion,

                            });
                        }
                    }
                });

            return listaContrats;

        }

        public async Task<List<dynamic>> GetContractsGridApoyoInterventoria()
        {
            List<dynamic> listaContrats = new List<dynamic>();

            List<VRequisitosTecnicosInicioConstruccion> lista = _context.VRequisitosTecnicosInicioConstruccion.ToList();

            lista.Where(c => c.TipoContratoCodigo == "2").ToList() // tipo contrato interventoria
                .ForEach(c =>
                {
                    if (c.TieneFaseConstruccion > 0)
                    { // fase construccion
                        if (c.TieneFasePreconstruccion > 0)
                        {
                            if (!string.IsNullOrEmpty(c.RutaActaFase1) && c.FechaActaInicioFase1 != null)
                            {
                                listaContrats.Add(new
                                {
                                    ContratoId = c.ContratoId,
                                    FechaAprobacion = c.FechaAprobacion,
                                    NumeroContrato = c.NumeroContrato,
                                    CantidadProyectosAsociados = c.CantidadProyectosAsociados,
                                    CantidadProyectosRequisitosAprobados = c.CantidadProyectosRequisitosAprobados,
                                    CantidadProyectosRequisitosPendientes = c.CantidadProyectosAsociados - c.CantidadProyectosRequisitosAprobados,
                                    EstadoCodigo = c.EstadoCodigo,
                                    EstadoNombre = c.EstadoNombre, //string.IsNullOrEmpty( c.EstadoCodigo ) ? "Sin verificación de requisitos técnicos" : c.EstadoNombre,
                                    Existeregistro = c.ExisteRegistro,
                                    c.EstaDevuelto,
                                    c.EstadoNombreVerificacion,

                                });
                            }
                        }
                        else
                        { // sin preconstruccion
                            listaContrats.Add(new
                            {
                                ContratoId = c.ContratoId,
                                FechaAprobacion = c.FechaAprobacion,
                                NumeroContrato = c.NumeroContrato,
                                CantidadProyectosAsociados = c.CantidadProyectosAsociados,
                                CantidadProyectosRequisitosAprobados = c.CantidadProyectosRequisitosAprobados,
                                CantidadProyectosRequisitosPendientes = c.CantidadProyectosAsociados - c.CantidadProyectosRequisitosAprobados,
                                EstadoCodigo = c.EstadoCodigo,
                                EstadoNombre = c.EstadoNombre, //string.IsNullOrEmpty( c.EstadoCodigo ) ? "Sin verificación de requisitos técnicos" : c.EstadoNombre,
                                Existeregistro = c.ExisteRegistro,
                                c.EstaDevuelto,
                                c.EstadoNombreVerificacion,

                            });
                        }
                    }

                });

            return listaContrats;

        }

        public async Task<ContratoConstruccion> GetContratoConstruccionById(int pIdContratoConstruccion)
        {
            ContratoConstruccion contratoConstruccion = new ContratoConstruccion();

            contratoConstruccion = _context.ContratoConstruccion.Find(pIdContratoConstruccion);
            contratoConstruccion.EsCompletoDiagnostico = this.EsCompletoDiagnostico(contratoConstruccion);

            return contratoConstruccion;

        }

        public async Task<Contrato> GetContratoByContratoId(int pContratoId, string pUsuarioCreacion)
        {
            try
            {
                List<Dominio> ListParametricas = _context.Dominio.ToList();
                List<Localizacion> Listlocalizacion = _context.Localizacion.ToList();
                List<Dominio> ListPerfilesDominio = _context.Dominio.Where(d => d.TipoDominioId == 11).ToList();


                Contrato contrato = await _context.Contrato.Where(r => r.ContratoId == pContratoId)
                     .Include(r => r.ContratoPoliza)
                     .Include(r => r.Contratacion)
                        .ThenInclude(r => r.ContratacionProyecto)
                              .ThenInclude(r => r.Proyecto)
                                 .ThenInclude(r => r.InstitucionEducativa)
                    .Include(r => r.Contratacion)
                        .ThenInclude(r => r.ContratacionProyecto)
                              .ThenInclude(r => r.Proyecto)
                                  .ThenInclude(r => r.Sede)
                     .Include(r => r.Contratacion)
                         .ThenInclude(r => r.Contratista)
                    .FirstOrDefaultAsync();

                

                contrato.ContratoConstruccion = _context.ContratoConstruccion.Where(cc => cc.ContratoId == pContratoId)
                                                                                .Include(r => r.ConstruccionPerfil)
                                                                                    .ThenInclude(r => r.ConstruccionPerfilObservacion)
                                                                                .Include(r => r.ConstruccionPerfil)
                                                                                    .ThenInclude(r => r.ConstruccionPerfilNumeroRadicado)
                                                                                .Include(r => r.ConstruccionObservacion)
                                                                                .ToList();

                //contrato.ContratoConstruccion.ToList().RemoveAll( r => r.eliminado == true )
                contrato.FechaPolizaAprobacion = contrato.ContratoPoliza.LastOrDefault().FechaAprobacion.HasValue ? contrato.ContratoPoliza.LastOrDefault().FechaAprobacion : DateTime.Now;

                contrato.ContratoConstruccion.ToList().ForEach(cc =>
                {
                    cc.ConstruccionPerfil = cc.ConstruccionPerfil.Where(cp => cp.Eliminado != true).ToList();
                    cc.ConstruccionObservacion = cc.ConstruccionObservacion.Where(co => co.Eliminado != true).ToList();

                    cc.ConstruccionPerfil.ToList().ForEach(cp =>
                    {
                        cp.ConstruccionPerfilObservacion = cp.ConstruccionPerfilObservacion
                                                                            .Where(cpo => cpo.Eliminado != true)
                                                                            .OrderByDescending(cpo => cpo.FechaCreacion)
                                                                            .ToList();

                        cp.ConstruccionPerfilNumeroRadicado = cp.ConstruccionPerfilNumeroRadicado.Where(cpr => cpr.Eliminado != true).ToList();

                        Dominio nombrePerfil = ListPerfilesDominio.Find(p => p.Codigo == cp.PerfilCodigo);

                        cp.NombrePerfil = nombrePerfil != null ? nombrePerfil.Nombre : "";

                        cp.ObservacionApoyo = getObservacionPerfil(cp, false);
                        cp.ObservacionSupervisor = getObservacionPerfil(cp, true);

                        cp.ObservacionDevolucion = _context.ConstruccionPerfilObservacion.Find(cp.ObservacionSupervisorId);

                    });

                    cc.ObservacionDiagnosticoApoyo = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.Diagnostico, false);
                    cc.ObservacionDiagnosticoSupervisor = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.Diagnostico, true);

                    cc.ObservacionPlanesProgramasApoyo = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.PlanesProgramas, false);
                    cc.ObservacionPlanesProgramasSupervisor = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.PlanesProgramas, true);

                    cc.ObservacionManejoAnticipoApoyo = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.ManejoAnticipo, false);
                    cc.ObservacionManejoAnticipoSupervisor = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.ManejoAnticipo, true);

                    cc.ObservacionProgramacionObraApoyo = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.ProgramacionObra, false);
                    cc.ObservacionProgramacionObraSupervisor = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.ProgramacionObra, true);

                    cc.ObservacionFlujoInversionApoyo = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.FlujoInversion, false);
                    cc.ObservacionFlujoInversionSupervisor = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.FlujoInversion, true);

                    // Observaciones devoluciones

                    cc.ObservacionDevolucionDiagnostico = _context.ConstruccionObservacion.Find(cc.ObservacionDiagnosticoSupervisorId);
                    cc.ObservacionDevolucionPlanesProgramas = _context.ConstruccionObservacion.Find(cc.ObservacionPlanesProgramasSupervisorId);
                    cc.ObservacionDevolucionManejoAnticipo = _context.ConstruccionObservacion.Find(cc.ObservacionManejoAnticipoSupervisorId);
                    cc.ObservacionDevolucionProgramacionObra = _context.ConstruccionObservacion.Find(cc.ObservacionProgramacionObraSupervisorId);
                    cc.ObservacionDevolucionFlujoInversion = _context.ConstruccionObservacion.Find(cc.ObservacionFlujoInversionSupervisorId);

                    //Proyecto proyectoTemp = CalcularFechaInicioContrato(cc.ContratoConstruccionId);
                    
                    

                });



                foreach (var ContratacionProyecto in contrato.Contratacion.ContratacionProyecto)
                {
                    Localizacion Municipio = Listlocalizacion.Where(r => r.LocalizacionId == ContratacionProyecto.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                    ContratacionProyecto.Proyecto.Departamento = Listlocalizacion.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault().Descripcion;
                    ContratacionProyecto.Proyecto.Municipio = Municipio.Descripcion;
                    ContratacionProyecto.Proyecto.TipoIntervencionCodigo = ListParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion && r.Codigo == ContratacionProyecto.Proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;

                    Proyecto proyectoTemp = CalcularFechasContrato(ContratacionProyecto.ProyectoId);

                        contrato.Contratacion.ContratacionProyecto.Where(cp => cp.ProyectoId == ContratacionProyecto.ProyectoId).FirstOrDefault().Proyecto.FechaInicioEtapaObra = proyectoTemp.FechaInicioEtapaObra;
                        contrato.Contratacion.ContratacionProyecto.Where(cp => cp.ProyectoId == ContratacionProyecto.ProyectoId).FirstOrDefault().Proyecto.FechaFinEtapaObra = proyectoTemp.FechaFinEtapaObra;
                        contrato.Contratacion.ContratacionProyecto.Where(cp => cp.ProyectoId == ContratacionProyecto.ProyectoId).FirstOrDefault().Proyecto.PlazoEnSemanas = proyectoTemp.PlazoEnSemanas;
                    }
                    else
                    {
                        Proyecto proyectoTemp = CalcularFechasContratoTemporales(ContratacionProyecto.ProyectoId, contrato.ContratoId);



                    List<ComponenteAportante> listaComponenteAportante = new List<ComponenteAportante>();

                    _context.ContratacionProyectoAportante.Where( cap => cap.ContratacionProyectoId == ContratacionProyecto.ContratacionProyectoId ).ToList().ForEach( cpa => {
                        listaComponenteAportante.AddRange( _context.ComponenteAportante.Where( ca => ca.ContratacionProyectoAportanteId == cpa.ContratacionProyectoAportanteId ).Include( r => r.ComponenteUso ) );
                    });

                    bool construccion = false;
                    bool preconstruccion = false;
                    if (listaComponenteAportante.Where(x => x.FaseCodigo == ConstanCodigoFaseContrato.Construccion).Count() > 0)
                    {
                        construccion = true;
                    }
                    if (listaComponenteAportante.Where(x => x.FaseCodigo == ConstanCodigoFaseContrato.Preconstruccion).Count() > 0)
                    {
                        preconstruccion = true;
                    }
                    ContratacionProyecto.faseConstruccionNotMapped = construccion;
                    ContratacionProyecto.fasePreConstruccionNotMapped = preconstruccion;

                    ContratacionProyecto.Proyecto.ValorFaseConstruccion = 0;
                    listaComponenteAportante.Where(x => x.FaseCodigo == ConstanCodigoFaseContrato.Construccion).ToList().ForEach( ca => {
                        ca.ComponenteUso.ToList().ForEach( cu => {
                            ContratacionProyecto.Proyecto.ValorFaseConstruccion += cu.ValorUso;
                        });
                    });


                }

                // elimino el proyecto que no tiene construccion
                contrato.Contratacion.ContratacionProyecto = contrato.Contratacion.ContratacionProyecto.Where(r => r.faseConstruccionNotMapped == true).ToList();

                // elimino el contratoConstruccion que no tiene construccion
                foreach (ContratacionProyecto contratacionProyecto in contrato.Contratacion.ContratacionProyecto)
                {
                    if ( contratacionProyecto.faseConstruccionNotMapped == false)
                    {
                        contrato.ContratoConstruccion = contrato.ContratoConstruccion.Where(r => r.ProyectoId != contratacionProyecto.ProyectoId).ToList();
                    }
                    
                }

                return contrato;
            }
            catch (Exception ex)
            {
                return new Contrato();
            }
        }

        private bool TieneFasePreconstruccion(int pIdContrato)
        {
            bool tieneFasePreconstruccion = false;

            List<VRequisitosTecnicosInicioConstruccion> listaProyectos = _context.VRequisitosTecnicosInicioConstruccion.Where(r => r.ContratoId == pIdContrato).ToList();

            if (listaProyectos.Where(r => r.TieneFasePreconstruccion > 0).Count() > 0)
            {
                tieneFasePreconstruccion = true;
            }

            return tieneFasePreconstruccion;
        }

        public async Task<ContratoConstruccion> GetContratoConstruccionByContratoconstruccionId(int pContratoconstruccionId)
        {
            try
            {
                List<Dominio> ListParametricas = _context.Dominio.ToList();
                List<Localizacion> Listlocalizacion = _context.Localizacion.ToList();
                List<Dominio> ListPerfilesDominio = _context.Dominio.Where(d => d.TipoDominioId == 11).ToList();
                var contratoConstruccion = _context.ContratoConstruccion.Where(x => x.ContratoConstruccionId == pContratoconstruccionId).
                    Include(x => x.ConstruccionObservacion).
                    Include(x => x.Contrato).
                        ThenInclude(x => x.Contratacion)
                    .FirstOrDefault();
                    

                contratoConstruccion.ConstruccionPerfil = contratoConstruccion.ConstruccionPerfil.Where(cp => cp.Eliminado != true).ToList();
                contratoConstruccion.ConstruccionObservacion = contratoConstruccion.ConstruccionObservacion.Where(co => co.Eliminado != true).ToList();

                contratoConstruccion.ConstruccionPerfil.ToList().ForEach(cp =>
                {
                    cp.ConstruccionPerfilObservacion = cp.ConstruccionPerfilObservacion
                                                                        .Where(cpo => cpo.Eliminado != true)
                                                                        .OrderByDescending(cpo => cpo.FechaCreacion)
                                                                        .ToList();

                    cp.ConstruccionPerfilNumeroRadicado = cp.ConstruccionPerfilNumeroRadicado.Where(cpr => cpr.Eliminado != true).ToList();

                    Dominio nombrePerfil = ListPerfilesDominio.Find(p => p.Codigo == cp.PerfilCodigo);

                    cp.NombrePerfil = nombrePerfil != null ? nombrePerfil.Nombre : "";

                    cp.ObservacionApoyo = getObservacionPerfil(cp, false);
                    cp.ObservacionSupervisor = getObservacionPerfil(cp, true);

                    cp.ObservacionDevolucion = _context.ConstruccionPerfilObservacion.Find(cp.ObservacionSupervisorId);

                });

                contratoConstruccion.ObservacionDiagnosticoApoyo = getObservacion(contratoConstruccion, ConstanCodigoTipoObservacionConstruccion.Diagnostico, false);
                contratoConstruccion.ObservacionDiagnosticoSupervisor = getObservacion(contratoConstruccion, ConstanCodigoTipoObservacionConstruccion.Diagnostico, true);

                contratoConstruccion.ObservacionPlanesProgramasApoyo = getObservacion(contratoConstruccion, ConstanCodigoTipoObservacionConstruccion.PlanesProgramas, false);
                contratoConstruccion.ObservacionPlanesProgramasSupervisor = getObservacion(contratoConstruccion, ConstanCodigoTipoObservacionConstruccion.PlanesProgramas, true);

                contratoConstruccion.ObservacionManejoAnticipoApoyo = getObservacion(contratoConstruccion, ConstanCodigoTipoObservacionConstruccion.ManejoAnticipo, false);
                contratoConstruccion.ObservacionManejoAnticipoSupervisor = getObservacion(contratoConstruccion, ConstanCodigoTipoObservacionConstruccion.ManejoAnticipo, true);

                contratoConstruccion.ObservacionProgramacionObraApoyo = getObservacion(contratoConstruccion, ConstanCodigoTipoObservacionConstruccion.ProgramacionObra, false);
                contratoConstruccion.ObservacionProgramacionObraSupervisor = getObservacion(contratoConstruccion, ConstanCodigoTipoObservacionConstruccion.ProgramacionObra, true);

                contratoConstruccion.ObservacionFlujoInversionApoyo = getObservacion(contratoConstruccion, ConstanCodigoTipoObservacionConstruccion.FlujoInversion, false);
                contratoConstruccion.ObservacionFlujoInversionSupervisor = getObservacion(contratoConstruccion, ConstanCodigoTipoObservacionConstruccion.FlujoInversion, true);

                // Observaciones devoluciones

                contratoConstruccion.ObservacionDevolucionDiagnostico = _context.ConstruccionObservacion.Find(contratoConstruccion.ObservacionDiagnosticoSupervisorId);
                contratoConstruccion.ObservacionDevolucionPlanesProgramas = _context.ConstruccionObservacion.Find(contratoConstruccion.ObservacionPlanesProgramasSupervisorId);
                contratoConstruccion.ObservacionDevolucionManejoAnticipo = _context.ConstruccionObservacion.Find(contratoConstruccion.ObservacionManejoAnticipoSupervisorId);
                contratoConstruccion.ObservacionDevolucionProgramacionObra = _context.ConstruccionObservacion.Find(contratoConstruccion.ObservacionProgramacionObraSupervisorId);
                contratoConstruccion.ObservacionDevolucionFlujoInversion = _context.ConstruccionObservacion.Find(contratoConstruccion.ObservacionFlujoInversionSupervisorId);




                return contratoConstruccion;
            }
            catch (Exception ex)
            {
                return new ContratoConstruccion();
            }
        }

        public async Task<List<ArchivoCargue>> GetLoadProgrammingGrid(int pContratoConstruccionId)
        {
            List<ArchivoCargue> listaCargas = _context.ArchivoCargue
                                                            .Where(a => a.ReferenciaId == pContratoConstruccionId &&
                                                                        a.Eliminado != true &&
                                                                        a.OrigenId == int.Parse(OrigenArchivoCargue.ProgramacionObra))
                                                            .ToList();


            listaCargas.ForEach(archivo =>
            {
                archivo.estadoCargue = archivo.CantidadRegistros == archivo.CantidadRegistrosValidos ? "Válidos" : "Fallido";

            });

            return listaCargas;

        }

        public async Task<List<ArchivoCargue>> GetLoadInvestmentFlowGrid(int pContratoConstruccionId)
        {
            List<ArchivoCargue> listaCargas = _context.ArchivoCargue
                                                            .Where(a => a.ReferenciaId == pContratoConstruccionId &&
                                                                   a.Eliminado != true &&
                                                                   a.OrigenId == int.Parse(OrigenArchivoCargue.FlujoInversion))
                                                            .ToList();


            listaCargas.ForEach(archivo =>
            {
                archivo.estadoCargue = archivo.CantidadRegistros == archivo.CantidadRegistrosValidos ? "Válidos" : "Fallido";

            });

            return listaCargas;


        }

        #endregion gets

        #region createEdit

        public async Task<Respuesta> CreateEditDiagnostico(ContratoConstruccion pConstruccion)
        {
            string CreateEdit = string.Empty;
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Contrato_Construccion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                if (pConstruccion.ContratoConstruccionId > 0)
                {
                    CreateEdit = "EDITAR CONTRATO CONSTRUCCION";

                    ContratoConstruccion contratoConstruccion = _context.ContratoConstruccion.Find(pConstruccion.ContratoConstruccionId);

                    contratoConstruccion.FechaModificacion = DateTime.Now;
                    contratoConstruccion.UsuarioCreacion = pConstruccion.UsuarioCreacion;

                    contratoConstruccion.EsInformeDiagnostico = pConstruccion.EsInformeDiagnostico;
                    contratoConstruccion.RutaInforme = pConstruccion.RutaInforme;
                    contratoConstruccion.CostoDirecto = pConstruccion.CostoDirecto;
                    contratoConstruccion.Administracion = pConstruccion.Administracion;
                    contratoConstruccion.Imprevistos = pConstruccion.Imprevistos;
                    contratoConstruccion.Utilidad = pConstruccion.Utilidad;
                    contratoConstruccion.ValorTotalFaseConstruccion = pConstruccion.ValorTotalFaseConstruccion;
                    contratoConstruccion.RequiereModificacionContractual = pConstruccion.RequiereModificacionContractual;
                    contratoConstruccion.NumeroSolicitudModificacion = pConstruccion.NumeroSolicitudModificacion;

                    contratoConstruccion.RegistroCompletoDiagnostico = VerificarRegistroCompletoDiagnostico(contratoConstruccion);

                }
                else
                {
                    CreateEdit = "CREAR CONTRATO CONSTRUCCION";

                    ContratoConstruccion contratoConstruccion = new ContratoConstruccion();

                    contratoConstruccion.FechaCreacion = DateTime.Now;
                    contratoConstruccion.UsuarioCreacion = pConstruccion.UsuarioCreacion;
                    contratoConstruccion.RegistroCompletoDiagnostico = false;

                    contratoConstruccion.ContratoId = pConstruccion.ContratoId;
                    contratoConstruccion.ProyectoId = pConstruccion.ProyectoId;
                    contratoConstruccion.EsInformeDiagnostico = pConstruccion.EsInformeDiagnostico;
                    contratoConstruccion.RutaInforme = pConstruccion.RutaInforme;
                    contratoConstruccion.CostoDirecto = pConstruccion.CostoDirecto;
                    contratoConstruccion.Administracion = pConstruccion.Administracion;
                    contratoConstruccion.Imprevistos = pConstruccion.Imprevistos;
                    contratoConstruccion.Utilidad = pConstruccion.Utilidad;
                    contratoConstruccion.ValorTotalFaseConstruccion = pConstruccion.ValorTotalFaseConstruccion;
                    contratoConstruccion.RequiereModificacionContractual = pConstruccion.RequiereModificacionContractual;
                    contratoConstruccion.NumeroSolicitudModificacion = pConstruccion.NumeroSolicitudModificacion;

                    contratoConstruccion.RegistroCompletoDiagnostico = VerificarRegistroCompletoDiagnostico(contratoConstruccion);

                    _context.ContratoConstruccion.Add(contratoConstruccion);
                }

                Contrato contrato = _context.Contrato.Find(pConstruccion.ContratoId);
                if (contrato.EstadoVerificacionConstruccionCodigo == null || contrato.EstadoVerificacionConstruccionCodigo == ConstanCodigoEstadoConstruccion.Sin_aprobacion_de_requisitos_tecnicos)
                    contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.En_proceso_de_aprobacion_de_requisitos_tecnicos;

                _context.SaveChanges();
                VerificarRegistroCompletoContratoObra(pConstruccion.ContratoId);
                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.OperacionExitosa, idAccion, pConstruccion.UsuarioCreacion, CreateEdit)
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.Error, idAccion, pConstruccion.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        public async Task<Respuesta> CreateEditPlanesProgramas(ContratoConstruccion pConstruccion)
        {
            string CreateEdit = string.Empty;
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Contrato_Construccion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                if (pConstruccion.ContratoConstruccionId > 0)
                {
                    CreateEdit = "EDITAR CONTRATO CONSTRUCCION";

                    ContratoConstruccion contratoConstruccion = _context.ContratoConstruccion.Find(pConstruccion.ContratoConstruccionId);

                    contratoConstruccion.FechaModificacion = DateTime.Now;
                    contratoConstruccion.UsuarioCreacion = pConstruccion.UsuarioCreacion;

                    contratoConstruccion.PlanLicenciaVigente = pConstruccion.PlanLicenciaVigente;
                    contratoConstruccion.LicenciaFechaRadicado = pConstruccion.LicenciaFechaRadicado;
                    contratoConstruccion.LicenciaFechaAprobacion = pConstruccion.LicenciaFechaAprobacion;
                    contratoConstruccion.LicenciaConObservaciones = pConstruccion.LicenciaConObservaciones;
                    contratoConstruccion.PlanCambioConstructorLicencia = pConstruccion.PlanCambioConstructorLicencia;
                    contratoConstruccion.CambioFechaRadicado = pConstruccion.CambioFechaRadicado;
                    contratoConstruccion.CambioFechaAprobacion = pConstruccion.CambioFechaAprobacion;
                    contratoConstruccion.CambioConObservaciones = pConstruccion.CambioConObservaciones;
                    contratoConstruccion.PlanActaApropiacion = pConstruccion.PlanActaApropiacion;
                    contratoConstruccion.ActaApropiacionFechaRadicado = pConstruccion.ActaApropiacionFechaRadicado;
                    contratoConstruccion.ActaApropiacionFechaAprobacion = pConstruccion.ActaApropiacionFechaAprobacion;
                    contratoConstruccion.ActaApropiacionConObservaciones = pConstruccion.ActaApropiacionConObservaciones;
                    contratoConstruccion.PlanResiduosDemolicion = pConstruccion.PlanResiduosDemolicion;
                    contratoConstruccion.ResiduosDemolicionFechaRadicado = pConstruccion.ResiduosDemolicionFechaRadicado;
                    contratoConstruccion.ResiduosDemolicionFechaAprobacion = pConstruccion.ResiduosDemolicionFechaAprobacion;
                    contratoConstruccion.ResiduosDemolicionConObservaciones = pConstruccion.ResiduosDemolicionConObservaciones;
                    contratoConstruccion.PlanManejoTransito = pConstruccion.PlanManejoTransito;
                    contratoConstruccion.ManejoTransitoFechaRadicado = pConstruccion.ManejoTransitoFechaRadicado;
                    contratoConstruccion.ManejoTransitoFechaAprobacion = pConstruccion.ManejoTransitoFechaAprobacion;
                    contratoConstruccion.ManejoTransitoConObservaciones1 = pConstruccion.ManejoTransitoConObservaciones1;
                    contratoConstruccion.PlanManejoAmbiental = pConstruccion.PlanManejoAmbiental;
                    contratoConstruccion.ManejoAmbientalFechaRadicado = pConstruccion.ManejoAmbientalFechaRadicado;
                    contratoConstruccion.ManejoAmbientalFechaAprobacion = pConstruccion.ManejoAmbientalFechaAprobacion;
                    contratoConstruccion.ManejoAmbientalConObservaciones = pConstruccion.ManejoAmbientalConObservaciones;
                    contratoConstruccion.PlanAseguramientoCalidad = pConstruccion.PlanAseguramientoCalidad;
                    contratoConstruccion.AseguramientoCalidadFechaRadicado = pConstruccion.AseguramientoCalidadFechaRadicado;
                    contratoConstruccion.AseguramientoCalidadFechaAprobacion = pConstruccion.AseguramientoCalidadFechaAprobacion;
                    contratoConstruccion.AseguramientoCalidadConObservaciones = pConstruccion.AseguramientoCalidadConObservaciones;
                    contratoConstruccion.PlanProgramaSeguridad = pConstruccion.PlanProgramaSeguridad;
                    contratoConstruccion.ProgramaSeguridadFechaRadicado = pConstruccion.ProgramaSeguridadFechaRadicado;
                    contratoConstruccion.ProgramaSeguridadFechaAprobacion = pConstruccion.ProgramaSeguridadFechaAprobacion;
                    contratoConstruccion.ProgramaSeguridadConObservaciones = pConstruccion.ProgramaSeguridadConObservaciones;
                    contratoConstruccion.PlanProgramaSalud = pConstruccion.PlanProgramaSalud;
                    contratoConstruccion.ProgramaSaludFechaRadicado = pConstruccion.ProgramaSaludFechaRadicado;
                    contratoConstruccion.ProgramaSaludFechaAprobacion = pConstruccion.ProgramaSaludFechaAprobacion;
                    contratoConstruccion.ProgramaSaludConObservaciones = pConstruccion.ProgramaSaludConObservaciones;
                    contratoConstruccion.PlanInventarioArboreo = pConstruccion.PlanInventarioArboreo;
                    contratoConstruccion.InventarioArboreoFechaRadicado = pConstruccion.InventarioArboreoFechaRadicado;
                    contratoConstruccion.InventarioArboreoFechaAprobacion = pConstruccion.InventarioArboreoFechaAprobacion;
                    contratoConstruccion.InventarioArboreoConObservaciones = pConstruccion.InventarioArboreoConObservaciones;
                    contratoConstruccion.PlanAprovechamientoForestal = pConstruccion.PlanAprovechamientoForestal;
                    contratoConstruccion.AprovechamientoForestalApropiacionFechaRadicado = pConstruccion.AprovechamientoForestalApropiacionFechaRadicado;
                    contratoConstruccion.AprovechamientoForestalFechaAprobacion = pConstruccion.AprovechamientoForestalFechaAprobacion;
                    contratoConstruccion.AprovechamientoForestalConObservaciones = pConstruccion.AprovechamientoForestalConObservaciones;
                    contratoConstruccion.PlanManejoAguasLluvias = pConstruccion.PlanManejoAguasLluvias;
                    contratoConstruccion.ManejoAguasLluviasFechaRadicado = pConstruccion.ManejoAguasLluviasFechaRadicado;
                    contratoConstruccion.ManejoAguasLluviasFechaAprobacion = pConstruccion.ManejoAguasLluviasFechaAprobacion;
                    contratoConstruccion.ManejoAguasLluviasConObservaciones = pConstruccion.ManejoAguasLluviasConObservaciones;
                    contratoConstruccion.PlanRutaSoporte = pConstruccion.PlanRutaSoporte;
                    contratoConstruccion.LicenciaObservaciones = pConstruccion.LicenciaObservaciones;
                    contratoConstruccion.CambioObservaciones = pConstruccion.CambioObservaciones;
                    contratoConstruccion.ActaApropiacionObservaciones = pConstruccion.ActaApropiacionObservaciones;
                    contratoConstruccion.ResiduosDemolicionObservaciones = pConstruccion.ResiduosDemolicionObservaciones;
                    contratoConstruccion.ManejoTransitoObservaciones = pConstruccion.ManejoTransitoObservaciones;
                    contratoConstruccion.ManejoAmbientalObservaciones = pConstruccion.ManejoAmbientalObservaciones;
                    contratoConstruccion.AseguramientoCalidadObservaciones = pConstruccion.AseguramientoCalidadObservaciones;
                    contratoConstruccion.ProgramaSeguridadObservaciones = pConstruccion.ProgramaSeguridadObservaciones;
                    contratoConstruccion.ProgramaSaludObservaciones = pConstruccion.ProgramaSaludObservaciones;
                    contratoConstruccion.InventarioArboreoObservaciones = pConstruccion.InventarioArboreoObservaciones;
                    contratoConstruccion.AprovechamientoForestalObservaciones = pConstruccion.AprovechamientoForestalObservaciones;
                    contratoConstruccion.ManejoAguasLluviasObservaciones = pConstruccion.ManejoAguasLluviasObservaciones;

                    contratoConstruccion.RegistroCompletoPlanesProgramas = VerificarRegistroCompletoPlanesProgramas(contratoConstruccion);

                }
                else
                {
                    CreateEdit = "CREAR CONTRATO CONSTRUCCION";

                    ContratoConstruccion contratoConstruccion = new ContratoConstruccion();

                    contratoConstruccion.FechaCreacion = DateTime.Now;
                    contratoConstruccion.UsuarioCreacion = pConstruccion.UsuarioCreacion;
                    contratoConstruccion.RegistroCompletoPlanesProgramas = false;

                    contratoConstruccion.ContratoId = pConstruccion.ContratoId;
                    contratoConstruccion.ProyectoId = pConstruccion.ProyectoId;

                    contratoConstruccion.PlanLicenciaVigente = pConstruccion.PlanLicenciaVigente;
                    contratoConstruccion.LicenciaFechaRadicado = pConstruccion.LicenciaFechaRadicado;
                    contratoConstruccion.LicenciaFechaAprobacion = pConstruccion.LicenciaFechaAprobacion;
                    contratoConstruccion.LicenciaConObservaciones = pConstruccion.LicenciaConObservaciones;
                    contratoConstruccion.PlanCambioConstructorLicencia = pConstruccion.PlanCambioConstructorLicencia;
                    contratoConstruccion.CambioFechaRadicado = pConstruccion.CambioFechaRadicado;
                    contratoConstruccion.CambioFechaAprobacion = pConstruccion.CambioFechaAprobacion;
                    contratoConstruccion.CambioConObservaciones = pConstruccion.CambioConObservaciones;
                    contratoConstruccion.PlanActaApropiacion = pConstruccion.PlanActaApropiacion;
                    contratoConstruccion.ActaApropiacionFechaRadicado = pConstruccion.ActaApropiacionFechaRadicado;
                    contratoConstruccion.ActaApropiacionFechaAprobacion = pConstruccion.ActaApropiacionFechaAprobacion;
                    contratoConstruccion.ActaApropiacionConObservaciones = pConstruccion.ActaApropiacionConObservaciones;
                    contratoConstruccion.PlanResiduosDemolicion = pConstruccion.PlanResiduosDemolicion;
                    contratoConstruccion.ResiduosDemolicionFechaRadicado = pConstruccion.ResiduosDemolicionFechaRadicado;
                    contratoConstruccion.ResiduosDemolicionFechaAprobacion = pConstruccion.ResiduosDemolicionFechaAprobacion;
                    contratoConstruccion.ResiduosDemolicionConObservaciones = pConstruccion.ResiduosDemolicionConObservaciones;
                    contratoConstruccion.PlanManejoTransito = pConstruccion.PlanManejoTransito;
                    contratoConstruccion.ManejoTransitoFechaRadicado = pConstruccion.ManejoTransitoFechaRadicado;
                    contratoConstruccion.ManejoTransitoFechaAprobacion = pConstruccion.ManejoTransitoFechaAprobacion;
                    contratoConstruccion.ManejoTransitoConObservaciones1 = pConstruccion.ManejoTransitoConObservaciones1;
                    contratoConstruccion.PlanManejoAmbiental = pConstruccion.PlanManejoAmbiental;
                    contratoConstruccion.ManejoAmbientalFechaRadicado = pConstruccion.ManejoAmbientalFechaRadicado;
                    contratoConstruccion.ManejoAmbientalFechaAprobacion = pConstruccion.ManejoAmbientalFechaAprobacion;
                    contratoConstruccion.ManejoAmbientalConObservaciones = pConstruccion.ManejoAmbientalConObservaciones;
                    contratoConstruccion.PlanAseguramientoCalidad = pConstruccion.PlanAseguramientoCalidad;
                    contratoConstruccion.AseguramientoCalidadFechaRadicado = pConstruccion.AseguramientoCalidadFechaRadicado;
                    contratoConstruccion.AseguramientoCalidadFechaAprobacion = pConstruccion.AseguramientoCalidadFechaAprobacion;
                    contratoConstruccion.AseguramientoCalidadConObservaciones = pConstruccion.AseguramientoCalidadConObservaciones;
                    contratoConstruccion.PlanProgramaSeguridad = pConstruccion.PlanProgramaSeguridad;
                    contratoConstruccion.ProgramaSeguridadFechaRadicado = pConstruccion.ProgramaSeguridadFechaRadicado;
                    contratoConstruccion.ProgramaSeguridadFechaAprobacion = pConstruccion.ProgramaSeguridadFechaAprobacion;
                    contratoConstruccion.ProgramaSeguridadConObservaciones = pConstruccion.ProgramaSeguridadConObservaciones;
                    contratoConstruccion.PlanProgramaSalud = pConstruccion.PlanProgramaSalud;
                    contratoConstruccion.ProgramaSaludFechaRadicado = pConstruccion.ProgramaSaludFechaRadicado;
                    contratoConstruccion.ProgramaSaludFechaAprobacion = pConstruccion.ProgramaSaludFechaAprobacion;
                    contratoConstruccion.ProgramaSaludConObservaciones = pConstruccion.ProgramaSaludConObservaciones;
                    contratoConstruccion.PlanInventarioArboreo = pConstruccion.PlanInventarioArboreo;
                    contratoConstruccion.InventarioArboreoFechaRadicado = pConstruccion.InventarioArboreoFechaRadicado;
                    contratoConstruccion.InventarioArboreoFechaAprobacion = pConstruccion.InventarioArboreoFechaAprobacion;
                    contratoConstruccion.InventarioArboreoConObservaciones = pConstruccion.InventarioArboreoConObservaciones;
                    contratoConstruccion.PlanAprovechamientoForestal = pConstruccion.PlanAprovechamientoForestal;
                    contratoConstruccion.AprovechamientoForestalApropiacionFechaRadicado = pConstruccion.AprovechamientoForestalApropiacionFechaRadicado;
                    contratoConstruccion.AprovechamientoForestalFechaAprobacion = pConstruccion.AprovechamientoForestalFechaAprobacion;
                    contratoConstruccion.AprovechamientoForestalConObservaciones = pConstruccion.AprovechamientoForestalConObservaciones;
                    contratoConstruccion.PlanManejoAguasLluvias = pConstruccion.PlanManejoAguasLluvias;
                    contratoConstruccion.ManejoAguasLluviasFechaRadicado = pConstruccion.ManejoAguasLluviasFechaRadicado;
                    contratoConstruccion.ManejoAguasLluviasFechaAprobacion = pConstruccion.ManejoAguasLluviasFechaAprobacion;
                    contratoConstruccion.ManejoAguasLluviasConObservaciones = pConstruccion.ManejoAguasLluviasConObservaciones;
                    contratoConstruccion.PlanRutaSoporte = pConstruccion.PlanRutaSoporte;
                    contratoConstruccion.LicenciaObservaciones = pConstruccion.LicenciaObservaciones;
                    contratoConstruccion.CambioObservaciones = pConstruccion.CambioObservaciones;
                    contratoConstruccion.ActaApropiacionObservaciones = pConstruccion.ActaApropiacionObservaciones;
                    contratoConstruccion.ResiduosDemolicionObservaciones = pConstruccion.ResiduosDemolicionObservaciones;
                    contratoConstruccion.ManejoTransitoObservaciones = pConstruccion.ManejoTransitoObservaciones;
                    contratoConstruccion.ManejoAmbientalObservaciones = pConstruccion.ManejoAmbientalObservaciones;
                    contratoConstruccion.AseguramientoCalidadObservaciones = pConstruccion.AseguramientoCalidadObservaciones;
                    contratoConstruccion.ProgramaSeguridadObservaciones = pConstruccion.ProgramaSeguridadObservaciones;
                    contratoConstruccion.ProgramaSaludObservaciones = pConstruccion.ProgramaSaludObservaciones;
                    contratoConstruccion.InventarioArboreoObservaciones = pConstruccion.InventarioArboreoObservaciones;
                    contratoConstruccion.AprovechamientoForestalObservaciones = pConstruccion.AprovechamientoForestalObservaciones;
                    contratoConstruccion.ManejoAguasLluviasObservaciones = pConstruccion.ManejoAguasLluviasObservaciones;

                    contratoConstruccion.RegistroCompletoPlanesProgramas = VerificarRegistroCompletoPlanesProgramas(contratoConstruccion);

                    _context.ContratoConstruccion.Add(contratoConstruccion);
                }

                Contrato contrato = _context.Contrato.Find(pConstruccion.ContratoId);
                if (contrato.EstadoVerificacionConstruccionCodigo == null || contrato.EstadoVerificacionConstruccionCodigo == ConstanCodigoEstadoConstruccion.Sin_aprobacion_de_requisitos_tecnicos)
                    contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.En_proceso_de_aprobacion_de_requisitos_tecnicos;

                _context.SaveChanges();

                VerificarRegistroCompletoContratoObra(pConstruccion.ContratoId);

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.OperacionExitosa, idAccion, pConstruccion.UsuarioCreacion, CreateEdit)
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.Error, idAccion, pConstruccion.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        public async Task<Respuesta> CreateEditManejoAnticipo(ContratoConstruccion pConstruccion)
        {
            string CreateEdit = string.Empty;
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Contrato_Construccion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                if (pConstruccion.ContratoConstruccionId > 0)
                {
                    CreateEdit = "EDITAR CONTRATO CONSTRUCCION";

                    ContratoConstruccion contratoConstruccion = _context.ContratoConstruccion.Find(pConstruccion.ContratoConstruccionId);

                    contratoConstruccion.FechaModificacion = DateTime.Now;
                    contratoConstruccion.UsuarioCreacion = pConstruccion.UsuarioCreacion;

                    contratoConstruccion.ManejoAnticipoRequiere = pConstruccion.ManejoAnticipoRequiere;
                    contratoConstruccion.ManejoAnticipoPlanInversion = pConstruccion.ManejoAnticipoPlanInversion;
                    contratoConstruccion.ManejoAnticipoCronogramaAmortizacion = pConstruccion.ManejoAnticipoCronogramaAmortizacion;
                    contratoConstruccion.ManejoAnticipoRutaSoporte = pConstruccion.ManejoAnticipoRutaSoporte;

                    contratoConstruccion.RegistroCompletoManejoAnticipo = VerificarRegistroCompletoManejoAnticipo(contratoConstruccion);

                }
                else
                {
                    CreateEdit = "CREAR CONTRATO CONSTRUCCION";

                    ContratoConstruccion contratoConstruccion = new ContratoConstruccion();

                    contratoConstruccion.FechaCreacion = DateTime.Now;
                    contratoConstruccion.UsuarioCreacion = pConstruccion.UsuarioCreacion;
                    contratoConstruccion.RegistroCompletoManejoAnticipo = false;

                    contratoConstruccion.ContratoId = pConstruccion.ContratoId;
                    contratoConstruccion.ProyectoId = pConstruccion.ProyectoId;

                    contratoConstruccion.ManejoAnticipoRequiere = pConstruccion.ManejoAnticipoRequiere;
                    contratoConstruccion.ManejoAnticipoPlanInversion = pConstruccion.ManejoAnticipoPlanInversion;
                    contratoConstruccion.ManejoAnticipoCronogramaAmortizacion = pConstruccion.ManejoAnticipoCronogramaAmortizacion;
                    contratoConstruccion.ManejoAnticipoRutaSoporte = pConstruccion.ManejoAnticipoRutaSoporte;

                    contratoConstruccion.RegistroCompletoManejoAnticipo = VerificarRegistroCompletoManejoAnticipo(contratoConstruccion);

                    _context.ContratoConstruccion.Add(contratoConstruccion);
                }

                Contrato contrato = _context.Contrato.Find(pConstruccion.ContratoId);
                if (contrato.EstadoVerificacionConstruccionCodigo == null || contrato.EstadoVerificacionConstruccionCodigo == ConstanCodigoEstadoConstruccion.Sin_aprobacion_de_requisitos_tecnicos)
                    contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.En_proceso_de_aprobacion_de_requisitos_tecnicos;

                _context.SaveChanges();
                VerificarRegistroCompletoContratoObra(pConstruccion.ContratoId);
                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.OperacionExitosa, idAccion, pConstruccion.UsuarioCreacion, CreateEdit)
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.Error, idAccion, pConstruccion.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        public async Task<Respuesta> CreateEditConstruccionPerfil(ContratoConstruccion pConstruccion)
        {
            string CreateEdit = string.Empty;
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Construccion_Perfil, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                if (pConstruccion.ContratoConstruccionId == 0)
                {
                    ContratoConstruccion contratoConstruccion = new ContratoConstruccion();

                    contratoConstruccion.UsuarioCreacion = pConstruccion.UsuarioCreacion;
                    contratoConstruccion.FechaCreacion = DateTime.Now;
                    contratoConstruccion.ContratoId = pConstruccion.ContratoId;
                    contratoConstruccion.ProyectoId = pConstruccion.ProyectoId;

                    _context.ContratoConstruccion.Add(contratoConstruccion);
                    _context.SaveChanges();

                    pConstruccion.ContratoConstruccionId = contratoConstruccion.ContratoConstruccionId;
                }

                foreach (var perfil in pConstruccion.ConstruccionPerfil)
                {
                    if (perfil.ConstruccionPerfilId > 0)
                    {
                        CreateEdit = "EDITAR CONSTRUCCION PERFIL";
                        ConstruccionPerfil construccionPerfil = _context.ConstruccionPerfil.Find(perfil.ConstruccionPerfilId);

                        construccionPerfil.UsuarioModificacion = pConstruccion.UsuarioModificacion;
                        construccionPerfil.FechaModificacion = DateTime.Now;

                        construccionPerfil.PerfilCodigo = perfil.PerfilCodigo;
                        construccionPerfil.CantidadHvRequeridas = perfil.CantidadHvRequeridas;
                        construccionPerfil.CantidadHvRecibidas = perfil.CantidadHvRecibidas;
                        construccionPerfil.CantidadHvAprobadas = perfil.CantidadHvAprobadas;
                        construccionPerfil.FechaAprobacion = perfil.FechaAprobacion;
                        construccionPerfil.RutaSoporte = perfil.RutaSoporte;
                        construccionPerfil.ConObervacionesSupervision = perfil.ConObervacionesSupervision;
                        construccionPerfil.Observaciones = perfil.Observaciones;

                        construccionPerfil.RegistroCompleto = ValidarRegistroCompletoConstruccionPerfil(construccionPerfil);

                        foreach (var radicado in perfil.ConstruccionPerfilNumeroRadicado)
                        {
                            if (radicado.ConstruccionPerfilNumeroRadicadoId == 0)
                            {
                                radicado.UsuarioCreacion = pConstruccion.UsuarioCreacion;
                                radicado.FechaCreacion = DateTime.Now;

                                radicado.Eliminado = false;

                                construccionPerfil.ConstruccionPerfilNumeroRadicado.Add(radicado);
                            }
                            else
                            {
                                ConstruccionPerfilNumeroRadicado construccionPerfilNumeroRadicado = _context.ConstruccionPerfilNumeroRadicado.Find(radicado.ConstruccionPerfilNumeroRadicadoId);
                                construccionPerfilNumeroRadicado.UsuarioModificacion = pConstruccion.UsuarioCreacion;
                                construccionPerfilNumeroRadicado.FechaModificacion = DateTime.Now;

                                construccionPerfilNumeroRadicado.NumeroRadicado = radicado.NumeroRadicado;

                            }
                        }

                    }
                    else
                    {
                        CreateEdit = "CREAR CONSTRUCCION PERFIL";
                        perfil.UsuarioCreacion = pConstruccion.UsuarioCreacion;
                        perfil.FechaCreacion = DateTime.Now;

                        perfil.Eliminado = false;
                        perfil.ContratoConstruccionId = pConstruccion.ContratoConstruccionId;
                        perfil.RegistroCompleto = ValidarRegistroCompletoConstruccionPerfil(perfil);



                        //foreach (var observacion in perfil.ConstruccionPerfilObservacion)
                        //{

                        //    observacion.UsuarioCreacion = pConstruccion.UsuarioCreacion;
                        //    observacion.FechaCreacion = DateTime.Now;
                        //    observacion.TipoObservacionCodigo = ConstanCodigoTipoObservacion.Interventoria;

                        //    //perfil.ConstruccionPerfilObservacion.Add(observacion);
                        //}

                        foreach (var radicado in perfil.ConstruccionPerfilNumeroRadicado)
                        {
                            radicado.UsuarioCreacion = pConstruccion.UsuarioCreacion;
                            radicado.FechaCreacion = DateTime.Now;

                            radicado.Eliminado = false;

                            //_context.ConstruccionPerfilNumeroRadicado.Add(radicado);
                        }

                        _context.ConstruccionPerfil.Add(perfil);
                    }
                }

                Contrato contrato = _context.Contrato
                                                .Where(c => c.ContratoId == pConstruccion.ContratoId)
                                                .Include(r => r.Contratacion)
                                                .FirstOrDefault();

                if (contrato.Contratacion.TipoSolicitudCodigo == "1")
                {// contrato de obra
                    if (contrato.EstadoVerificacionConstruccionCodigo == null || contrato.EstadoVerificacionConstruccionCodigo == ConstanCodigoEstadoConstruccion.Sin_aprobacion_de_requisitos_tecnicos)
                        contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.En_proceso_de_aprobacion_de_requisitos_tecnicos;

                    VerificarRegistroCompletoContratoObra(pConstruccion.ContratoId);
                }

                if (contrato.Contratacion.TipoSolicitudCodigo == "2")
                { // contrato de interventoria
                    if (contrato.EstadoVerificacionConstruccionCodigo == null || contrato.EstadoVerificacionConstruccionCodigo == ConstanCodigoEstadoConstruccion.Con_requisitos_tecnicos_aprobados)
                        contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.En_proceso_de_verificacion_de_requisitos_tecnicos;

                    List<ContratoConstruccion> listaConstruccion = _context.ContratoConstruccion
                                                                        .Where(cp => cp.ContratoId == pConstruccion.ContratoId)
                                                                        .Include(r => r.ConstruccionPerfil)
                                                                        .ToList();

                    string estadoTempContrato = contrato.EstadoVerificacionConstruccionCodigo;
                    contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.Con_requisitos_tecnicos_verificados;

                    listaConstruccion.ForEach(c =>
                    {

                        c.RegistroCompleto = true;
                        c.ConstruccionPerfil.ToList().ForEach(cp =>
                        {
                            if (cp.RegistroCompleto != true)
                            {
                                c.RegistroCompleto = false;
                                contrato.EstadoVerificacionConstruccionCodigo = estadoTempContrato;
                            }
                        });

                    });

                    VerificarRegistroCompletoContratoInterventoria(pConstruccion.ContratoId);
                }

                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.OperacionExitosa, idAccion, pConstruccion.UsuarioCreacion, CreateEdit)
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.Error, idAccion, pConstruccion.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        public async Task<Respuesta> CreateEditObservacionesCarga(int pArchivoCargueId, string pObservacion, string pUsuarioCreacion)
        {
            string CreateEdit = string.Empty;
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Observacion_Archivo, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                CreateEdit = "EDITAR OBSERVACION ARCHIVO";

                ArchivoCargue archivoCargue = _context.ArchivoCargue.Find(pArchivoCargueId);

                if (archivoCargue != null)
                {
                    archivoCargue.Observaciones = pObservacion;
                }


                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioCreacion, CreateEdit)
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.Error, idAccion, pUsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }


        #endregion createEdit

        #region private 

        private void VerificarRegistroCompletoValidacionContratoObra(int pIdContrato)
        {
            bool esCompleto = true;

            Contrato contrato = _context.Contrato
                                            .Where(c => c.ContratoId == pIdContrato)
                                            .Include(r => r.ContratoConstruccion)
                                                .ThenInclude(r => r.ConstruccionPerfil)
                                                    .ThenInclude(r => r.ConstruccionPerfilObservacion)
                                            .Include(r => r.Contratacion)
                                            .Include(r => r.ContratoConstruccion)
                                                .ThenInclude( r => r.ConstruccionObservacion )
                                            .FirstOrDefault();

            if (contrato.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
            {
                contrato.ContratoConstruccion.ToList().ForEach(cc =>
                {
                    ContratoConstruccion construccionTemp = _context.ContratoConstruccion.Find(cc.ContratoConstruccionId);
                    bool completoConstruccion = true;

                    cc.ObservacionDiagnosticoSupervisor = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.Diagnostico, true);
                    cc.ObservacionPlanesProgramasSupervisor = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.PlanesProgramas, true);
                    cc.ObservacionManejoAnticipoSupervisor = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.ManejoAnticipo, true);
                    cc.ObservacionProgramacionObraSupervisor = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.ProgramacionObra, true);
                    cc.ObservacionFlujoInversionSupervisor = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.FlujoInversion, true);

                    if (
                            ( TieneFasePreconstruccion(construccionTemp.ContratoId) && cc.TieneObservacionesDiagnosticoSupervisor == null ) ||
                            ( (TieneFasePreconstruccion(construccionTemp.ContratoId) && cc.TieneObservacionesDiagnosticoSupervisor == true ) && string.IsNullOrEmpty(cc.ObservacionDiagnosticoSupervisor != null ? cc.ObservacionDiagnosticoSupervisor.Observaciones : null)) ||
                            cc.TieneObservacionesFlujoInversionSupervisor == null ||
                            (cc.TieneObservacionesFlujoInversionSupervisor == true && string.IsNullOrEmpty(cc.ObservacionFlujoInversionSupervisor != null ? cc.ObservacionFlujoInversionSupervisor.Observaciones : null)) ||
                            cc.TieneObservacionesManejoAnticipoSupervisor == null ||
                            (cc.TieneObservacionesManejoAnticipoSupervisor == true && string.IsNullOrEmpty(cc.ObservacionManejoAnticipoSupervisor != null ? cc.ObservacionManejoAnticipoSupervisor.Observaciones : null)) ||
                            cc.TieneObservacionesPlanesProgramasSupervisor == null ||
                            (cc.TieneObservacionesPlanesProgramasSupervisor == true && string.IsNullOrEmpty(cc.ObservacionPlanesProgramasSupervisor != null ? cc.ObservacionPlanesProgramasSupervisor.Observaciones : null)) ||
                            cc.TieneObservacionesProgramacionObraSupervisor == null ||
                            (cc.TieneObservacionesProgramacionObraSupervisor == true && string.IsNullOrEmpty(cc.ObservacionProgramacionObraSupervisor != null ? cc.ObservacionProgramacionObraSupervisor.Observaciones : null))
                         )
                    {
                        esCompleto = false;
                        completoConstruccion = false;
                    }


                    //construccionTemp.RegistroCompleto = completoConstruccion;
                    _context.SaveChanges();

                });
            }
            else if (contrato.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString())
            {
                contrato.ContratoConstruccion.ToList().ForEach(cc =>
                {
                    ContratoConstruccion construccionTemp = _context.ContratoConstruccion.Find(cc.ContratoConstruccionId);
                    bool completoConstruccion = true;

                    cc.ConstruccionPerfil.Where(cp => cp.Eliminado != true).ToList().ForEach(cp =>
                    {
                        ConstruccionPerfilObservacion UltimaObservacionSupervisor = getObservacionPerfil(cp, true);// ConstruccionPerfil.ConstruccionPerfilObservacion.OrderBy(r => r.ConstruccionPerfilObservacionId).Where(r => (bool)r.EsSupervision).LastOrDefault().Observacion;

                            if (
                                cp.TieneObservacionesSupervisor == null ||
                                (cp.TieneObservacionesSupervisor == true && string.IsNullOrEmpty(UltimaObservacionSupervisor != null ? UltimaObservacionSupervisor.Observacion : null))
                        )
                        {
                            esCompleto = false;
                            completoConstruccion = false;
                        }
                    });


                    //construccionTemp.RegistroCompleto = completoConstruccion;
                    _context.SaveChanges();

                });
            }

            //contrato.RegistroCompletoConstruccion = esCompleto;
            if (esCompleto == true)
            {
                contrato.FechaAprobacionRequisitosConstruccionSupervisor = DateTime.Now;
                contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.Con_requisitos_tecnicos_validados;
            }
            else
            {
                contrato.FechaAprobacionRequisitosConstruccionSupervisor = null;
                contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.En_proceso_de_validacion_de_requisitos_tecnicos;
            }

            _context.SaveChanges();
        }

        private void VerificarRegistroCompletoContratoObra(int pIdContrato)
        {
            bool esCompleto = true;

            Contrato contrato = _context.Contrato
                                            .Where(c => c.ContratoId == pIdContrato)
                                            .Include(r => r.ContratoConstruccion)
                                                .ThenInclude(r => r.ConstruccionPerfil)
                                            .FirstOrDefault();

            contrato.ContratoConstruccion.ToList().ForEach(cc =>
            {
                ContratoConstruccion construccionTemp = _context.ContratoConstruccion.Find(cc.ContratoConstruccionId);
                bool completoConstruccion = true;

                if (
                        (TieneFasePreconstruccion(construccionTemp.ContratoId) && cc.RegistroCompletoDiagnostico != true) ||
                        cc.RegistroCompletoPlanesProgramas != true ||
                        cc.RegistroCompletoManejoAnticipo != true ||
                        cc.RegistroCompletoProgramacionObra != true ||
                        cc.RegistroCompletoFlujoInversion != true
                    )
                {
                    esCompleto = false;
                    completoConstruccion = false;
                }
                else
                {
                    cc.ConstruccionPerfil.Where(cp => cp.Eliminado != true).ToList().ForEach(cp =>
                    {
                        if (cp.RegistroCompleto != true)
                        {
                            esCompleto = false;
                            completoConstruccion = false;
                        }
                    });
                }

                construccionTemp.RegistroCompleto = completoConstruccion;
                _context.SaveChanges();

            });

            contrato.RegistroCompletoConstruccion = esCompleto;
            if (contrato.RegistroCompletoConstruccion == true)
            {
                contrato.FechaAprobacionRequisitosConstruccionInterventor = DateTime.Now;
            }
            else
            {
                contrato.FechaAprobacionRequisitosConstruccionInterventor = null;
            }

            _context.SaveChanges();
        }

        private void VerificarRegistroCompletoContratoInterventoria(int pIdContrato)
        {
            bool esCompleto = true;

            Contrato contrato = _context.Contrato
                                            .Where(c => c.ContratoId == pIdContrato)
                                            .Include(r => r.ContratoConstruccion)
                                                .ThenInclude(r => r.ConstruccionPerfil)
                                            .FirstOrDefault();

            contrato.ContratoConstruccion.ToList().ForEach(cc =>
            {
                ContratoConstruccion construccionTemp = _context.ContratoConstruccion.Find(cc.ContratoConstruccionId);
                bool completoConstruccion = true;

                cc.ConstruccionPerfil.Where(cp => cp.Eliminado != true).ToList().ForEach(cp =>
                {
                    if (cp.RegistroCompleto != true)
                    {
                        esCompleto = false;
                        completoConstruccion = false;
                    }
                });

                construccionTemp.RegistroCompleto = completoConstruccion;
                _context.SaveChanges();

            });

            contrato.RegistroCompletoConstruccion = esCompleto;
            if (contrato.RegistroCompletoConstruccion == true)
            {
                contrato.FechaAprobacionRequisitosConstruccionApoyo = DateTime.Now;
                contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.Con_requisitos_tecnicos_verificados;
            }
            else
            {
                contrato.FechaAprobacionRequisitosConstruccionApoyo = null;
                contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.En_proceso_de_verificacion_de_requisitos_tecnicos;
            }

            _context.SaveChanges();
        }

        private bool VerificarEsSupervisor(string pUsuarioCreacion)
        {
            List<UsuarioPerfil> usuarioPerfil = _context.UsuarioPerfil.Where(r => r.PerfilId == 8)
            .Include(r => r.Usuario).ToList();

            if (usuarioPerfil.Where(r => r.Usuario.Email == pUsuarioCreacion).ToList().Count > 0)
                return true;
            else
                return false;
        }

        private bool VerificarRegistroCompletoDiagnostico(ContratoConstruccion pConstruccion)
        {
            bool completo = true;

            if (
                pConstruccion.EsInformeDiagnostico == null ||
                ( pConstruccion.EsInformeDiagnostico == true && string.IsNullOrEmpty(pConstruccion.RutaInforme) ) ||
                pConstruccion.CostoDirecto == null ||
                pConstruccion.Administracion == null ||
                pConstruccion.Imprevistos == null ||
                pConstruccion.Utilidad == null ||
                pConstruccion.ValorTotalFaseConstruccion == null ||
                pConstruccion.RequiereModificacionContractual == null ||
                (pConstruccion.RequiereModificacionContractual == true && pConstruccion.NumeroSolicitudModificacion == null)
            )
            {
                completo = false;
            }

            return completo;

        }

        private async Task<bool> ValidarRegistroCompletoVerificacionPreconstruccion(int id, bool pEsSupervicion)
        {
            bool esCompleto = true;

            ContratoConstruccion cc = await _context.ContratoConstruccion.Where(cc => cc.ContratoConstruccionId == id)
                                                                .Include(r => r.ConstruccionPerfil)
                                                                    .ThenInclude( r => r.ConstruccionPerfilObservacion )
                                                                .FirstOrDefaultAsync();

            cc.ObservacionDiagnosticoSupervisor = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.Diagnostico, pEsSupervicion);
            cc.ObservacionPlanesProgramasSupervisor = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.PlanesProgramas, pEsSupervicion);
            cc.ObservacionManejoAnticipoSupervisor = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.ManejoAnticipo, pEsSupervicion);
            cc.ObservacionProgramacionObraSupervisor = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.ProgramacionObra, pEsSupervicion);
            cc.ObservacionFlujoInversionSupervisor = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.FlujoInversion, pEsSupervicion);

            if (cc.TieneObservacionesDiagnosticoSupervisor == null ||
                 (cc.TieneObservacionesDiagnosticoSupervisor == true && string.IsNullOrEmpty(cc.ObservacionDiagnosticoSupervisor != null ? cc.ObservacionDiagnosticoSupervisor.Observaciones : null)) ||
                 cc.TieneObservacionesFlujoInversionSupervisor == null ||
                 (cc.TieneObservacionesFlujoInversionSupervisor == true && string.IsNullOrEmpty(cc.ObservacionFlujoInversionSupervisor != null ? cc.ObservacionFlujoInversionSupervisor.Observaciones : null)) ||
                 cc.TieneObservacionesManejoAnticipoSupervisor == null ||
                 (cc.TieneObservacionesManejoAnticipoSupervisor == true && string.IsNullOrEmpty(cc.ObservacionManejoAnticipoSupervisor != null ? cc.ObservacionManejoAnticipoSupervisor.Observaciones : null)) ||
                 cc.TieneObservacionesPlanesProgramasSupervisor == null ||
                 (cc.TieneObservacionesPlanesProgramasSupervisor == true && string.IsNullOrEmpty(cc.ObservacionPlanesProgramasSupervisor != null ? cc.ObservacionPlanesProgramasSupervisor.Observaciones : null)) ||
                 cc.TieneObservacionesProgramacionObraSupervisor == null ||
                 (cc.TieneObservacionesProgramacionObraSupervisor == true && string.IsNullOrEmpty(cc.ObservacionProgramacionObraSupervisor != null ? cc.ObservacionProgramacionObraSupervisor.Observaciones : null))
                 )
            {
                esCompleto = false;
            }

            cc.ConstruccionPerfil.ToList().ForEach( cp => {
                
                 cp.ObservacionApoyo = getObservacionPerfil(cp, false);                

                 if (
                         cp.TieneObservacionesApoyo == null 
                     ||  cp.TieneObservacionesApoyo == true && string.IsNullOrEmpty( cp.ObservacionApoyo != null ? cp.ObservacionApoyo.Observacion : null )
                 ){
                     esCompleto = false;
                 }

             });

            return esCompleto;
        }

        private async Task<bool> ValidarRegistroCompletoValidacionObraInterventoria(int id)
        {
            bool esCompleto = true;

            ContratoConstruccion cc = await _context.ContratoConstruccion.Where(cc => cc.ContratoConstruccionId == id)
                                                          .Include(r => r.ConstruccionPerfil)
                                                              .ThenInclude(r => r.ConstruccionPerfilObservacion)
                                                                 .Include(r => r.Contrato)
                                                                   .ThenInclude(r => r.Contratacion)
                                                          .FirstOrDefaultAsync();

            if (cc.Contrato.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
            {
                cc.ObservacionDiagnosticoApoyo = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.Diagnostico, true);
                cc.ObservacionPlanesProgramasApoyo = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.PlanesProgramas, true);
                cc.ObservacionManejoAnticipoApoyo = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.ManejoAnticipo, true);
                cc.ObservacionProgramacionObraApoyo = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.ProgramacionObra, true);
                cc.ObservacionFlujoInversionApoyo = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.FlujoInversion, true);

                if ( ( TieneFasePreconstruccion( cc.ContratoId) && cc.TieneObservacionesDiagnosticoApoyo == null ) ||
                     ( ( TieneFasePreconstruccion( cc.ContratoId) && cc.TieneObservacionesDiagnosticoApoyo == true ) && string.IsNullOrEmpty(cc.ObservacionDiagnosticoApoyo != null ? cc.ObservacionDiagnosticoApoyo.Observaciones : null)) ||
                     cc.TieneObservacionesFlujoInversionSupervisor == null ||
                     (cc.TieneObservacionesFlujoInversionSupervisor == true && string.IsNullOrEmpty(cc.ObservacionFlujoInversionApoyo != null ? cc.ObservacionFlujoInversionApoyo.Observaciones : null)) ||
                     cc.TieneObservacionesManejoAnticipoSupervisor == null ||
                     (cc.TieneObservacionesManejoAnticipoSupervisor == true && string.IsNullOrEmpty(cc.ObservacionManejoAnticipoApoyo != null ? cc.ObservacionManejoAnticipoApoyo.Observaciones : null)) ||
                     cc.TieneObservacionesPlanesProgramasSupervisor == null ||
                     (cc.TieneObservacionesPlanesProgramasSupervisor == true && string.IsNullOrEmpty(cc.ObservacionPlanesProgramasApoyo != null ? cc.ObservacionPlanesProgramasApoyo.Observaciones : null)) ||
                     cc.TieneObservacionesProgramacionObraSupervisor == null ||
                     (cc.TieneObservacionesProgramacionObraSupervisor == true && string.IsNullOrEmpty(cc.ObservacionProgramacionObraApoyo != null ? cc.ObservacionProgramacionObraApoyo.Observaciones : null))
                     )
                {
                    esCompleto = false;
                }
            }
            //Validar si Los perfiles ya tienen Obsevacion
            else if (cc.Contrato.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString())
            {
                foreach (var ConstruccionPerfil in cc.ConstruccionPerfil.Where(r => r.Eliminado != true))
                {
                    ConstruccionPerfilObservacion UltimaObservacionSupervisor = getObservacionPerfil(ConstruccionPerfil, true);// ConstruccionPerfil.ConstruccionPerfilObservacion.OrderBy(r => r.ConstruccionPerfilObservacionId).Where(r => (bool)r.EsSupervision).LastOrDefault().Observacion;

                    if (
                            ConstruccionPerfil.TieneObservacionesSupervisor == null ||
                            (ConstruccionPerfil.TieneObservacionesSupervisor == true && string.IsNullOrEmpty(UltimaObservacionSupervisor != null ? UltimaObservacionSupervisor.Observacion : null))
                       )
                        esCompleto = false;
                }
            }

            return esCompleto;
        }

        private async Task<bool> ValidarRegistroCompletoVerificacionContruccion(int id, bool pEsSupervicion)
        {
            bool esCompleto = true;
            var cc = await GetContratoConstruccionByContratoconstruccionId(id);

            if (
                 cc.TieneObservacionesFlujoInversionApoyo == null ||
                 (cc.TieneObservacionesFlujoInversionApoyo == true && string.IsNullOrEmpty(cc.ObservacionFlujoInversionApoyo != null ? cc.ObservacionFlujoInversionApoyo.Observaciones : null)) ||
                 cc.TieneObservacionesManejoAnticipoApoyo == null ||
                 (cc.TieneObservacionesManejoAnticipoApoyo == true && string.IsNullOrEmpty(cc.ObservacionManejoAnticipoApoyo != null ? cc.ObservacionManejoAnticipoApoyo.Observaciones : null)) ||
                 cc.TieneObservacionesPlanesProgramasApoyo == null ||
                 (cc.TieneObservacionesPlanesProgramasApoyo == true && string.IsNullOrEmpty(cc.ObservacionPlanesProgramasApoyo != null ? cc.ObservacionPlanesProgramasApoyo.Observaciones : null)) ||
                 cc.TieneObservacionesProgramacionObraApoyo == null ||
                 (cc.TieneObservacionesProgramacionObraApoyo == true && string.IsNullOrEmpty(cc.ObservacionProgramacionObraApoyo != null ? cc.ObservacionProgramacionObraApoyo.Observaciones : null))
                 )
            {
                esCompleto = false;
            }

             cc.ConstruccionPerfil.ToList().ForEach( cp => {
                
                 cp.ObservacionApoyo = getObservacionPerfil(cp, false);                

                 if (
                         cp.TieneObservacionesApoyo == null 
                     ||  cp.TieneObservacionesApoyo == true && string.IsNullOrEmpty( cp.ObservacionApoyo != null ? cp.ObservacionApoyo.Observacion : null )
                 ){
                     esCompleto = false;
                 }

             });

            return esCompleto;
        }

        private bool VerificarRegistroCompletoPlanesProgramas(ContratoConstruccion pConstruccion)
        {
            bool completo = true;

            if (
                    pConstruccion.PlanLicenciaVigente == null ||
                    pConstruccion.PlanLicenciaVigente == false ||

                    pConstruccion.PlanCambioConstructorLicencia == null ||
                    pConstruccion.PlanCambioConstructorLicencia == false ||

                    pConstruccion.PlanActaApropiacion == null ||
                    pConstruccion.PlanActaApropiacion == false ||

                    pConstruccion.PlanResiduosDemolicion == null ||
                    pConstruccion.PlanResiduosDemolicion == false ||

                    pConstruccion.PlanManejoTransito == null ||
                    pConstruccion.PlanManejoTransito == false ||

                    pConstruccion.PlanManejoAmbiental == null ||
                    pConstruccion.PlanManejoAmbiental == false ||

                    pConstruccion.PlanAseguramientoCalidad == null ||
                    pConstruccion.PlanAseguramientoCalidad == false ||

                    pConstruccion.PlanProgramaSeguridad == null ||
                    pConstruccion.PlanProgramaSeguridad == false ||

                    pConstruccion.PlanProgramaSalud == null ||
                    pConstruccion.PlanProgramaSalud == false ||

                    pConstruccion.PlanInventarioArboreo == null ||
                    pConstruccion.PlanInventarioArboreo == 1 ||

                    pConstruccion.PlanAprovechamientoForestal == null ||
                    pConstruccion.PlanAprovechamientoForestal == 1 ||

                    pConstruccion.PlanManejoAguasLluvias == null ||
                    pConstruccion.PlanManejoAguasLluvias == 1 ||

                    string.IsNullOrEmpty(pConstruccion.PlanRutaSoporte)
            )
            {
                completo = false;
            }

            return completo;
        }

        private bool VerificarRegistroCompletoManejoAnticipo(ContratoConstruccion pConstruccion)
        {
            bool completo = true;

            if (
                    pConstruccion.ManejoAnticipoRequiere == null ||
                    (pConstruccion.ManejoAnticipoRequiere == true && pConstruccion.ManejoAnticipoPlanInversion == null) ||
                    (pConstruccion.ManejoAnticipoRequiere == true && pConstruccion.ManejoAnticipoPlanInversion == false) ||
                    (pConstruccion.ManejoAnticipoRequiere == true && pConstruccion.ManejoAnticipoCronogramaAmortizacion == null) ||
                    (pConstruccion.ManejoAnticipoRequiere == true && pConstruccion.ManejoAnticipoCronogramaAmortizacion == false)
                //( pConstruccion.ManejoAnticipoRequiere == true && string.IsNullOrEmpty(pConstruccion.ManejoAnticipoRutaSoporte) )
                )
            {
                completo = false;
            }

            return completo;
        }

        private bool ValidarRegistroCompletoConstruccionPerfil(ConstruccionPerfil pPerfil)
        {
            if (
                    string.IsNullOrEmpty(pPerfil.PerfilCodigo)
                 || string.IsNullOrEmpty(pPerfil.CantidadHvRequeridas.ToString())
                 || string.IsNullOrEmpty(pPerfil.CantidadHvRecibidas.ToString())
                 || string.IsNullOrEmpty(pPerfil.CantidadHvAprobadas.ToString())
                 || string.IsNullOrEmpty(pPerfil.FechaAprobacion.ToString())
                 || string.IsNullOrEmpty(pPerfil.RutaSoporte)
                 || string.IsNullOrEmpty(pPerfil.Observaciones)

                )
            {
                return false;
            }
            return true;
        }

        private bool EsCompletoDiagnostico(ContratoConstruccion pContratoConstruccion)
        {
            bool esCompleto = true;

            if (pContratoConstruccion.EsInformeDiagnostico == null ||
                    (pContratoConstruccion.EsInformeDiagnostico == true && string.IsNullOrEmpty(pContratoConstruccion.RutaInforme)) ||
                    pContratoConstruccion.CostoDirecto == null ||
                    pContratoConstruccion.Administracion == null ||
                    pContratoConstruccion.Imprevistos == null ||
                    pContratoConstruccion.Utilidad == null ||
                    pContratoConstruccion.ValorTotalFaseConstruccion == null ||
                    pContratoConstruccion.RequiereModificacionContractual == null ||
                    (pContratoConstruccion.RequiereModificacionContractual == true && string.IsNullOrEmpty(pContratoConstruccion.NumeroSolicitudModificacion))
               )
            {
                esCompleto = false;
            }

            return esCompleto;
        }

        private Proyecto CalcularFechasContrato( int pProyectoId ){
            Proyecto proyecto = _context.Proyecto
                                            .Where( p => p.ProyectoId == pProyectoId )
                                                .Include( r => r.ContratacionProyecto )
                                                    .ThenInclude( r => r.Contratacion )
                                                        .ThenInclude( r => r.Contrato )
                                                            .ThenInclude( r => r.ContratoPoliza )
                                                .Include( r => r.ContratacionProyecto )
                                                    .ThenInclude( r => r.Contratacion )
                                                        .ThenInclude( r => r.DisponibilidadPresupuestal )
                                            .FirstOrDefault();
            Contrato contrato = new Contrato();
            if ( proyecto?.ContratacionProyecto?.FirstOrDefault()?.Contratacion?.Contrato?.FirstOrDefault() != null )
                contrato = proyecto.ContratacionProyecto.FirstOrDefault().Contratacion.Contrato.FirstOrDefault();

            // obtengo información de las fechas de las polizas y DRP
            DateTime? fechaInicioContrato = proyecto?.ContratacionProyecto?.FirstOrDefault()?.Contratacion?.DisponibilidadPresupuestal?.FirstOrDefault()?.FechaDrp;
            DateTime? fechaPoliza = contrato.ContratoPoliza?.OrderByDescending(r => r.FechaAprobacion)?.FirstOrDefault()?.FechaAprobacion;

            VRequisitosTecnicosInicioConstruccion proyectoInfo = _context.VRequisitosTecnicosInicioConstruccion
                                                                                .Where(r => r.ContratoId == contrato.ContratoId)
                                                                                .FirstOrDefault();

            //proyecto = contratoConstruccion.Proyecto;

            // valida la fecha mayor 
            if (fechaInicioContrato != null && fechaPoliza != null)
            {

                if (fechaPoliza >= fechaInicioContrato)
                    proyecto.FechaInicioEtapaObra = fechaPoliza.Value;
                else
                    proyecto.FechaInicioEtapaObra = fechaInicioContrato.Value;

            }


            // agrega el plazo de preconstruccion si tiene esta fase
            if (proyectoInfo.TieneFasePreconstruccion > 0)
            {
                proyecto.FechaInicioEtapaObra = proyecto.FechaInicioEtapaObra.AddMonths(proyecto.PlazoMesesInterventoria.Value);
                proyecto.FechaInicioEtapaObra = proyecto.FechaInicioEtapaObra.AddMonths(proyecto.PlazoDiasInterventoria.Value + 1);
            }

            // calcula la fecha final del contrato
            DateTime fechaFinalContrato = proyecto.FechaInicioEtapaObra.AddMonths(proyecto.PlazoMesesObra.Value);
            proyecto.FechaFinEtapaObra = fechaFinalContrato.AddDays(proyecto.PlazoDiasObra.Value);

            proyecto.PlazoEnSemanas = ( proyecto.FechaFinEtapaObra - proyecto.FechaInicioEtapaObra ).TotalDays / 7;

            return proyecto;
        }

        private Proyecto CalcularFechasContratoTemporales(int pProyectoId, int pContratoId)
        {
            Proyecto proyecto = _context.Proyecto
                                            .Where(p => p.ProyectoId == pProyectoId)
                                                .Include( r => r.ContratacionProyecto )
                                                    .ThenInclude( r => r.Contratacion )
                                                        .ThenInclude( r => r.Contrato )
                                                            .ThenInclude( r => r.ContratoPoliza )
                                                .Include( r => r.ContratacionProyecto )
                                                    .ThenInclude( r => r.Contratacion )
                                                        .ThenInclude( r => r.DisponibilidadPresupuestal )
                                            .FirstOrDefault();

            Contrato contrato = new Contrato();
            DateTime? fechaInicioContrato = DateTime.MinValue;
            DateTime? fechaPoliza = DateTime.MinValue;

            foreach ( ContratacionProyecto contratacionProyecto in proyecto?.ContratacionProyecto)
            {
                if ( contratacionProyecto?.Contratacion?.Contrato?.FirstOrDefault()?.ContratoId == pContratoId)
                {
                    if ( contratacionProyecto?.Contratacion.Contrato?.FirstOrDefault() != null)
                    {
                        contrato = contratacionProyecto.Contratacion.Contrato.FirstOrDefault();
                        // obtengo información de las fechas de las polizas y DRP
                        fechaInicioContrato = contratacionProyecto.Contratacion?.DisponibilidadPresupuestal?.FirstOrDefault()?.FechaDrp;
                        fechaPoliza = contrato.ContratoPoliza?.OrderByDescending(r => r.FechaAprobacion)?.FirstOrDefault()?.FechaAprobacion;
                    }
                }
            } 

            

            VRequisitosTecnicosInicioConstruccion proyectoInfo = _context.VRequisitosTecnicosInicioConstruccion
                                                                                .Where(r => r.ContratoId == contrato.ContratoId)
                                                                                .FirstOrDefault();

            // valida la fecha mayor 
            if (fechaInicioContrato != null && fechaPoliza != null)
            {

                if (fechaPoliza >= fechaInicioContrato)
                    proyecto.FechaInicioEtapaObra = fechaPoliza.Value;
                else
                    proyecto.FechaInicioEtapaObra = fechaInicioContrato.Value;

            }


            //// agrega el plazo de preconstruccion si tiene esta fase
            if (proyectoInfo.TieneFasePreconstruccion > 0)
            {
                proyecto.FechaInicioEtapaObra = contrato.FechaTerminacion.HasValue ? contrato.FechaTerminacion.Value.AddDays(1) : proyecto.FechaInicioEtapaObra;
            //    proyecto.FechaInicioEtapaObra = proyecto.FechaInicioEtapaObra.AddMonths(proyecto.PlazoMesesInterventoria.Value);
            //    proyecto.FechaInicioEtapaObra = proyecto.FechaInicioEtapaObra.AddMonths(proyecto.PlazoDiasInterventoria.Value + 1);
            }

            //proyecto.FechaInicioEtapaObra = pFechaInicioObra.HasValue ? pFechaInicioObra.Value : DateTime.MinValue;

            // calcula la fecha final del contrato
            DateTime fechaFinalContrato = proyecto.FechaInicioEtapaObra.AddMonths(proyecto.PlazoMesesObra.Value);
            proyecto.FechaFinEtapaObra = fechaFinalContrato.AddDays(proyecto.PlazoDiasObra.Value);

            proyecto.PlazoEnSemanas = (proyecto.FechaFinEtapaObra - proyecto.FechaInicioEtapaObra).TotalDays / 7;

            return proyecto;
        }

        public Proyecto CalcularFechaInicioContrato(int pContratoConstruccionId)
        {

            Proyecto proyecto = new Proyecto();

            // obtengo la informacion del proyecto
            ContratoConstruccion contratoConstruccion = _context.ContratoConstruccion
                                                                        .Find(pContratoConstruccionId);

            return CalcularFechasContrato( contratoConstruccion.ProyectoId );

            
        }

        

        #endregion private

        #region business

        public async Task<Respuesta> CambiarEstadoContratoEstadoVerificacionConstruccionCodigo(int ContratoId, string pEstado, string pUsuarioMod)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Contrato_Construccion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                Contrato contratoCambiarEstado = await _context.Contrato
                    .Where(r => r.ContratoId == ContratoId)
                    .Include(r => r.ContratoConstruccion)
                          .ThenInclude(r => r.ConstruccionObservacion)
                    .Include( r => r.Contratacion )
                    .FirstOrDefaultAsync();

                contratoCambiarEstado.UsuarioModificacion = pUsuarioMod;
                contratoCambiarEstado.FechaModificacion = DateTime.Now;

                contratoCambiarEstado.EstadoVerificacionConstruccionCodigo = pEstado;

                if (pEstado == ConstanCodigoEstadoConstruccion.Enviado_al_interventor.ToString() || pEstado == ConstanCodigoEstadoConstruccion.Enviado_al_apoyo.ToString())
                {
                    foreach (var ContratoConstruccion in contratoCambiarEstado.ContratoConstruccion)
                    {

                        //if ( contratoCambiarEstado.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString() ){

                            List<ConstruccionPerfil> listaPerfiles = _context.ConstruccionPerfil
                                                                                .Where( cp => cp.ContratoConstruccionId == ContratoConstruccion.ContratoConstruccionId)
                                                                                .Include( r => r.ConstruccionPerfilObservacion )
                                                                                .ToList();

                            listaPerfiles.ForEach( cp => {

                                if ( cp.TieneObservacionesSupervisor == true ){
                                
                                    // observacion supervisor
                                    ConstruccionPerfilObservacion construccionPerfilObservacionSupervisor = cp.ConstruccionPerfilObservacion
                                        .Where( r => r.EsSupervision == true &&
                                                r.Eliminado != true &&
                                                r.Archivada != true
                                            )
                                        .FirstOrDefault();

                                    construccionPerfilObservacionSupervisor.Archivada = true;
                                    
                                    if ( cp.TieneObservacionesApoyo == true ){
                                        
                                        // observacion apoyo
                                        ConstruccionPerfilObservacion construccionPerfilObservacionApoyo = cp.ConstruccionPerfilObservacion
                                        .Where( r => r.EsSupervision == false &&
                                                r.Eliminado != true &&
                                                r.Archivada != true
                                            )
                                        .FirstOrDefault();

                                    construccionPerfilObservacionApoyo.Archivada = true;
                                    }

                                    //Construccion Perfil
                                    cp.ObservacionSupervisorId = construccionPerfilObservacionSupervisor.ConstruccionPerfilObservacionId;
                                    cp.TieneObservacionesApoyo = null;
                                    cp.TieneObservacionesSupervisor = null;
                                    cp.RegistroCompleto = false;

                                }

                            });
                        //}

                        //Observaciones Diagnostico

                        if ( TieneFasePreconstruccion( contratoCambiarEstado.ContratoId ) && contratoCambiarEstado.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString() ){
                            if (ContratoConstruccion.TieneObservacionesDiagnosticoSupervisor == true)
                            {
                                //Observacion Supervisor
                                ConstruccionObservacion construccionObservacionSupervisor = ContratoConstruccion.ConstruccionObservacion
                                    .Where(r => r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.Diagnostico.ToString()
                                            && r.EsSupervision == true
                                            && r.Eliminado != true
                                            && r.Archivada != true
                                        )
                                    .FirstOrDefault();

                                construccionObservacionSupervisor.Archivada = true;

                                //Observacion Apoyo
                                if ( ContratoConstruccion.TieneObservacionesDiagnosticoApoyo == true ){
                                    ConstruccionObservacion construccionObservacionApoyo = ContratoConstruccion.ConstruccionObservacion
                                    .Where(r => r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.Diagnostico.ToString()
                                        && r.EsSupervision == false
                                        && r.Eliminado != true
                                        && r.Archivada != true
                                        )
                                    .FirstOrDefault();

                                    construccionObservacionApoyo.Archivada = true;
                                }

                                //Contrato Construccion
                                ContratoConstruccion.ObservacionDiagnosticoSupervisorId = construccionObservacionSupervisor.ConstruccionObservacionId;
                                ContratoConstruccion.TieneObservacionesDiagnosticoApoyo = null;
                                ContratoConstruccion.TieneObservacionesDiagnosticoSupervisor = null;
                                ContratoConstruccion.RegistroCompletoDiagnostico = false;
                            }
                        }

                        //Observaciones Planes Y Programas

                        if (ContratoConstruccion.TieneObservacionesPlanesProgramasSupervisor == true && contratoCambiarEstado.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
                        {
                            //Observacion Supervisor
                            ConstruccionObservacion construccionObservacionSupervisor = ContratoConstruccion.ConstruccionObservacion
                                .Where(r => r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.PlanesProgramas.ToString()
                                    && r.EsSupervision == true
                                    && r.Eliminado != true
                                    && r.Archivada != true
                                    )
                                .FirstOrDefault();

                            construccionObservacionSupervisor.Archivada = true;

                            //Observacion Apoyo
                            if (ContratoConstruccion.TieneObservacionesPlanesProgramasApoyo == true) {
                                ConstruccionObservacion construccionObservacionApoyo = ContratoConstruccion.ConstruccionObservacion
                                .Where(r => r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.PlanesProgramas.ToString()
                                    && r.EsSupervision == false
                                    && r.Eliminado != true
                                    && r.Archivada != true
                                    )
                                .FirstOrDefault();

                                construccionObservacionApoyo.Archivada = true;

                            }

                            //Contrato Construccion
                            ContratoConstruccion.ObservacionPlanesProgramasSupervisorId = construccionObservacionSupervisor.ConstruccionObservacionId;
                            ContratoConstruccion.TieneObservacionesPlanesProgramasApoyo = null;
                            ContratoConstruccion.TieneObservacionesPlanesProgramasSupervisor = null;
                            ContratoConstruccion.RegistroCompletoPlanesProgramas = false;
                        }

                        //Observaciones Manejo de Anticipo

                        if (ContratoConstruccion.TieneObservacionesManejoAnticipoSupervisor == true && contratoCambiarEstado.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
                        {
                            //Observacion Supervisor
                            ConstruccionObservacion construccionObservacionSupervisor = ContratoConstruccion.ConstruccionObservacion
                                .Where(r => r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.ManejoAnticipo.ToString()
                                    && r.EsSupervision == true
                                    && r.Eliminado != true
                                    && r.Archivada != true)
                                .FirstOrDefault();

                            construccionObservacionSupervisor.Archivada = true;

                            //Observacion Apoyo
                            if (ContratoConstruccion.TieneObservacionesManejoAnticipoApoyo == true){
                                ConstruccionObservacion construccionObservacionApoyo = ContratoConstruccion.ConstruccionObservacion
                                .Where(r => r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.ManejoAnticipo.ToString()
                                    &&  r.EsSupervision == false
                                    && r.Eliminado != true
                                    && r.Archivada != true
                                )
                                .FirstOrDefault();

                                construccionObservacionApoyo.Archivada = true;
                            }

                            //Contrato Construccion
                            ContratoConstruccion.ObservacionManejoAnticipoSupervisorId = construccionObservacionSupervisor.ConstruccionObservacionId;
                            ContratoConstruccion.TieneObservacionesManejoAnticipoApoyo = null;
                            ContratoConstruccion.TieneObservacionesManejoAnticipoSupervisor = null;
                            ContratoConstruccion.RegistroCompletoManejoAnticipo = false;
                        }

                        //Observaciones Programacion Obra  

                        if (ContratoConstruccion.TieneObservacionesProgramacionObraSupervisor == true && contratoCambiarEstado.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
                        {
                            //Observacion Supervisor
                            ConstruccionObservacion construccionObservacionSupervisor = ContratoConstruccion.ConstruccionObservacion
                                .Where(r => r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.ProgramacionObra.ToString()
                                    && r.EsSupervision == true
                                    && r.Eliminado != true
                                    && r.Archivada != true )
                                ?.FirstOrDefault();

                            construccionObservacionSupervisor.Archivada = true;

                            //Observacion Apoyo
                            if (ContratoConstruccion.TieneObservacionesProgramacionObraApoyo == true){
                                ConstruccionObservacion construccionObservacionApoyo = ContratoConstruccion.ConstruccionObservacion
                                .Where(r => r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.ProgramacionObra.ToString()
                                    && r.EsSupervision == false
                                    && r.Eliminado != true
                                    && r.Archivada != true
                                        )
                                .FirstOrDefault();

                                construccionObservacionApoyo.Archivada = true;

                            }

                            //Contrato Construccion
                            ContratoConstruccion.ObservacionProgramacionObraSupervisorId = construccionObservacionSupervisor.ConstruccionObservacionId;
                            ContratoConstruccion.TieneObservacionesProgramacionObraApoyo = null;
                            ContratoConstruccion.TieneObservacionesProgramacionObraSupervisor = null;
                            ContratoConstruccion.RegistroCompletoProgramacionObra = false;
                        }

                        //Observaciones Flujo Inversion 

                        if (ContratoConstruccion.TieneObservacionesFlujoInversionSupervisor == true && contratoCambiarEstado.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
                        {
                            //Observacion Supervisor
                            ConstruccionObservacion construccionObservacionSupervisor = ContratoConstruccion.ConstruccionObservacion
                                .Where(r => r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.FlujoInversion.ToString()
                                    && r.EsSupervision == true
                                    && r.Eliminado != true
                                    && r.Archivada != true
                                    )
                                .FirstOrDefault();

                            construccionObservacionSupervisor.Archivada = true;

                            //Observacion Apoyo
                            if (ContratoConstruccion.TieneObservacionesFlujoInversionSupervisor == true ){
                                ConstruccionObservacion construccionObservacionApoyo = ContratoConstruccion.ConstruccionObservacion
                                .Where(r => r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.FlujoInversion.ToString()
                                    && r.EsSupervision == false
                                    && r.Eliminado != true
                                    && r.Archivada != true
                                        )
                                .FirstOrDefault();

                                construccionObservacionApoyo.Archivada = true;

                            }

                            //Contrato Construccion
                            ContratoConstruccion.ObservacionFlujoInversionSupervisorId = construccionObservacionSupervisor.ConstruccionObservacionId;
                            ContratoConstruccion.TieneObservacionesFlujoInversionApoyo = null;
                            ContratoConstruccion.TieneObservacionesFlujoInversionSupervisor = null;
                            ContratoConstruccion.RegistroCompletoFlujoInversion = false;
                        }

                    }
                }


                //Logica de actas cuando se aprueba
                if (pEstado == ConstanCodigoEstadoContrato.Con_requisitos_tecnicos_aprobados_por_supervisor)
                {
                    if (contratoCambiarEstado.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
                        contratoCambiarEstado.EstadoActaFase2 = ConstanCodigoEstadoActaContrato.Sin_Revision;
                    else
                        contratoCambiarEstado.EstadoActaFase2 = ConstanCodigoEstadoActaContrato.Sin_acta_generada;
                }

                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioMod, "CAMBIAR ESTADO CONTRATO CONSTRUCCION")
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.Error, idAccion, pUsuarioMod, ex.InnerException.ToString())
                    };
            }



        }

        public async Task<Respuesta> EnviarAlSupervisor(int pContratoId, string pUsuarioCreacion, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            string CreateEdit = string.Empty;
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Enviar_Al_Supervisor, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                Contrato contrato = _context.Contrato.Where(c => c.ContratoId == pContratoId).Include(x => x.Contratacion).FirstOrDefault();
                //envio correo
                //envio correo a supervisor
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.VerificacionRequisitosTecnicosConstruccionFase2);

                string ncontrato = "";
                string fechaContrato = "";
                string template = TemplateRecoveryPassword.Contenido.
                    Replace("[NUMEROCONTRATO]", contrato.NumeroContrato).
                    Replace("_LinkF_", pDominioFront).
                    Replace("[FECHAVERIFICACION]", contrato.FechaAprobacionRequisitosConstruccionApoyo == null ? "" : Convert.ToDateTime(contrato.FechaAprobacionRequisitosConstruccionApoyo).ToString("dd/MM/yyyy")).
                    Replace("[CANTIDADPROYECTOSASOCIADOS]", _context.ContratacionProyecto.Where(x => x.ContratacionId == contrato.ContratacionId).Count().ToString()).
                    Replace("[CANTIDADPROYECTOSVERIFICADOS]", contrato.Contratacion.TipoSolicitudCodigo == "1" ? _context.ContratoConstruccion.Where(x => x.RegistroCompletoVerificacion == true && x.ContratoId == contrato.ContratoId).Count().ToString() :
                    _context.ContratoConstruccion.Where(x => x.RegistroCompleto == true && x.ContratoId == contrato.ContratoId).Count().ToString()).
                    Replace("[TIPOCONTRATO]", contrato.Contratacion.TipoSolicitudCodigo == "1" ? "obra" : "interventoría");//OBRA O INTERVENTORIA


                var usuariosadmin = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Supervisor).Include(y => y.Usuario).ToList();
                foreach (var usuarioadmin in usuariosadmin)
                {
                    bool blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuarioadmin.Usuario.Email, "Verificación de requisitos técnicos para fase 2-construcción", template, pSender, pPassword, pMailServer, pMailPort);
                }

                contrato.UsuarioModificacion = pUsuarioCreacion;
                contrato.FechaModificacion = DateTime.Now;

                contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.Enviado_al_supervisor;


                _context.SaveChanges();
                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Verificar_Requisitos_Tecnicos_Construccion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioCreacion, CreateEdit)
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Verificar_Requisitos_Tecnicos_Construccion, GeneralCodes.Error, idAccion, pUsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        public async Task<Respuesta> AprobarInicio(int pContratoId, string pUsuarioCreacion)
        {
            string CreateEdit = string.Empty;
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Aprobar_Inicio_Construccion, (int)EnumeratorTipoDominio.Acciones);

            try
            {

                Contrato contrato = _context.Contrato.Find(pContratoId);
                //jflorez, este evento solo sucede cuando esta completo y se aprueban los requisitos, por ello seteo el dato 20201202
                contrato.FechaAprobacionRequisitosConstruccionInterventor = DateTime.Now;
                contrato.UsuarioModificacion = pUsuarioCreacion;
                contrato.FechaModificacion = DateTime.Now;

                contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.Con_requisitos_tecnicos_aprobados;

                _context.SaveChanges();
                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Verificar_Requisitos_Tecnicos_Construccion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioCreacion, CreateEdit)
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Verificar_Requisitos_Tecnicos_Construccion, GeneralCodes.Error, idAccion, pUsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        public async Task<byte[]> GetPDFDRP(int pContratoId, string usuarioModificacion)
        {
            if (pContratoId == 0)
            {
                throw new Exception("Debe enviar el id del contrato");
            }

            Contrato contrato = _context.Contrato.Where(c => c.ContratoId == pContratoId)
                                                    .Include(r => r.Contratacion)
                                                        .ThenInclude(r => r.DisponibilidadPresupuestal)
                                                    .FirstOrDefault();

            if (contrato?.Contratacion?.DisponibilidadPresupuestal != null)
                return await _budgetAvailabilityService.GetPDFDRP(contrato.Contratacion.DisponibilidadPresupuestal.FirstOrDefault().DisponibilidadPresupuestalId, usuarioModificacion);
            else
            {
                throw new Exception("El contrato no tiene DRP");
            }

        }

        #endregion business

        #region deletes

        public async Task<Respuesta> DeleteConstruccionPerfil(int pConstruccionPerfilId, string pUsuarioModificacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Construccion_Perfil, (int)EnumeratorTipoDominio.Acciones);

            if (pConstruccionPerfilId == 0)
            {
                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioModificacion, "CONTRATO PERFIL ELIMINADO")
                    };

            }


            try
            {
                ConstruccionPerfil perfil = _context.ConstruccionPerfil.Find(pConstruccionPerfilId);

                perfil.UsuarioModificacion = pUsuarioModificacion;
                perfil.FechaModificacion = DateTime.Now;
                perfil.Eliminado = true;

                ContratoConstruccion contratoConstruccion = _context.ContratoConstruccion.Find( perfil.ContratoConstruccionId );

                Contrato contrato = _context.Contrato
                                                .Where(c => c.ContratoId == contratoConstruccion.ContratoId)
                                                .Include(r => r.Contratacion)
                                                .FirstOrDefault();

                if (contrato.Contratacion.TipoSolicitudCodigo == "1")
                {// contrato de obra
                    if (contrato.EstadoVerificacionConstruccionCodigo == null || contrato.EstadoVerificacionConstruccionCodigo == ConstanCodigoEstadoConstruccion.Sin_aprobacion_de_requisitos_tecnicos)
                        contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.En_proceso_de_aprobacion_de_requisitos_tecnicos;

                    VerificarRegistroCompletoContratoObra(contratoConstruccion.ContratoId);
                }

                if (contrato.Contratacion.TipoSolicitudCodigo == "2")
                { // contrato de interventoria
                    if (contrato.EstadoVerificacionConstruccionCodigo == null || contrato.EstadoVerificacionConstruccionCodigo == ConstanCodigoEstadoConstruccion.Con_requisitos_tecnicos_aprobados)
                        contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.En_proceso_de_verificacion_de_requisitos_tecnicos;

                    List<ContratoConstruccion> listaConstruccion = _context.ContratoConstruccion
                                                                        .Where(cp => cp.ContratoId == contratoConstruccion.ContratoId)
                                                                        .Include(r => r.ConstruccionPerfil)
                                                                        .ToList();

                    string estadoTempContrato = contrato.EstadoVerificacionConstruccionCodigo;
                    contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.Con_requisitos_tecnicos_verificados;

                    listaConstruccion.ForEach(c =>
                    {

                        c.RegistroCompleto = true;
                        c.ConstruccionPerfil.ToList().ForEach(cp =>
                        {
                            if (cp.RegistroCompleto != true)
                            {
                                c.RegistroCompleto = false;
                                contrato.EstadoVerificacionConstruccionCodigo = estadoTempContrato;
                            }
                        });

                    });

                    VerificarRegistroCompletoContratoInterventoria(contratoConstruccion.ContratoId);
                }

                _context.SaveChanges();



                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioModificacion, "CONTRATO PERFIL ELIMINADO")
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.Error, idAccion, pUsuarioModificacion, ex.InnerException.ToString().ToUpper())
                    };
            }
        }

        public async Task<Respuesta> DeleteConstruccionPerfilNumeroRadicado(int pConstruccionPerfilNumeroRadicadoId, string pUsuarioModificacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Construccion_Perfil_Numero_Radicado, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ConstruccionPerfilNumeroRadicado radicado = _context.ConstruccionPerfilNumeroRadicado.Find(pConstruccionPerfilNumeroRadicadoId);

                radicado.UsuarioModificacion = pUsuarioModificacion;
                radicado.FechaModificacion = DateTime.Now;
                radicado.Eliminado = true;

                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioModificacion, "CONTRATO PERFIL ELIMINADO")
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.Error, idAccion, pUsuarioModificacion, ex.InnerException.ToString().ToUpper())
                    };
            }
        }

        public async Task<Respuesta> DeleteArchivoCargue(int pArchivocargue, int pContratoConstruccionId, bool pEsFlujoInvserion, string pUsuarioModificacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Archivo_Cargue, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ArchivoCargue archivoCargue = _context.ArchivoCargue.Find(pArchivocargue);

                archivoCargue.UsuarioModificacion = pUsuarioModificacion;
                archivoCargue.FechaModificacion = DateTime.Now;
                archivoCargue.Eliminado = true;

                ContratoConstruccion contratoConstruccion = _context.ContratoConstruccion.Find(pContratoConstruccionId);

                

                if (pEsFlujoInvserion)
                {
                    List<FlujoInversion> listaFlujoInversion = _context.FlujoInversion.Where( f => f.ContratoConstruccionId == pContratoConstruccionId ).ToList();
                    _context.FlujoInversion.RemoveRange( listaFlujoInversion );

                    contratoConstruccion.ArchivoCargueIdFlujoInversion = null;
                    contratoConstruccion.RegistroCompletoFlujoInversion = false;
                }
                else
                {
                    contratoConstruccion.ArchivoCargueIdProgramacionObra = null;
                    contratoConstruccion.RegistroCompletoProgramacionObra = false;
                }

                _context.SaveChanges();
                VerificarRegistroCompletoContratoObra(contratoConstruccion.ContratoId);

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioModificacion, "ARCHIVO CARGUE ELIMINADO")
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.Error, idAccion, pUsuarioModificacion, ex.InnerException.ToString().ToUpper())
                    };
            }
        }

        #endregion deletes

        #region loads

        public async Task<Respuesta> UploadFileToValidateProgramming(IFormFile pFile, string pFilePatch, string pUsuarioCreo, int pContratoConstruccionId, int pContratoId, int pProyectoId)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Validar_Excel_Programacion_Obra, (int)EnumeratorTipoDominio.Acciones);

            if ( pContratoConstruccionId == 0 ){
                ContratoConstruccion contratoTemp = new ContratoConstruccion();

                contratoTemp.UsuarioCreacion = pUsuarioCreo;
                contratoTemp.FechaCreacion = DateTime.Now;

                contratoTemp.ContratoId = pContratoId;
                contratoTemp.ProyectoId = pProyectoId;

                _context.ContratoConstruccion.Add( contratoTemp);
                _context.SaveChanges();

                pContratoConstruccionId = contratoTemp.ContratoConstruccionId;
            }


            ContratoConstruccion contratoConstruccion = _context.ContratoConstruccion
                                                                        .Where(cc => cc.ContratoConstruccionId == pContratoConstruccionId)
                                                                        .Include(r => r.Contrato)
                                                                            .ThenInclude(r => r.Contratacion)
                                                                                .ThenInclude(r => r.DisponibilidadPresupuestal)
                                                                        .Include(r => r.Contrato)
                                                                            .ThenInclude(r => r.ContratoPoliza)
                                                                        .Include(r => r.Proyecto)
                                                                        .FirstOrDefault();

        
            //Proyecto proyectoTemp = CalcularFechaInicioContrato( pContratoConstruccionId );
            Proyecto proyectoTemp = CalcularFechasContrato( pProyectoId );
            

            DateTime? fechaInicioContrato = proyectoTemp.FechaInicioEtapaObra;
            DateTime fechaFinalContrato = proyectoTemp.FechaFinEtapaObra;

            int CantidadRegistrosVacios = 0;
            int CantidadResgistrosValidos = 0;
            int CantidadRegistrosInvalidos = 0;
            int cantidadRutaCritica = 0;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //int OrigenId = await _commonService.GetDominioIdByCodigoAndTipoDominio(OrigenArchivoCargue.Proyecto, (int)EnumeratorTipoDominio.Origen_Documento_Cargue);
            DocumentService _documentService = new DocumentService(_context, _commonService);

            ArchivoCargue archivoCarge = await _documentService.getSaveFile(pFile, pFilePatch, Int32.Parse(OrigenArchivoCargue.ProgramacionObra), pContratoConstruccionId);

            // if (!string.IsNullOrEmpty(archivoCarge.ArchivoCargueId.ToString()))
            if (archivoCarge != null)
            {
                using (var stream = new MemoryStream())
                {
                    await pFile.CopyToAsync(stream);

                    using var package = new ExcelPackage(stream);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    //Controlar Registros
                    //Filas <=
                    //No comienza desde 0 por lo tanto el = no es necesario
                    for (int i = 2; i <= worksheet.Dimension.Rows; i++)
                    {
                        bool tieneErrores = false;
                        try
                        {

                            TempProgramacion temp = new TempProgramacion();
                            //Auditoria
                            temp.ArchivoCargueId = archivoCarge.ArchivoCargueId;
                            temp.EstaValidado = false;
                            temp.FechaCreacion = DateTime.Now;
                            temp.UsuarioCreacion = pUsuarioCreo;
                            temp.ContratoConstruccionId = pContratoConstruccionId;

                            List<FlujoInversion> listaFlujo = _context.FlujoInversion.Where( r => r.ContratoConstruccionId == pContratoConstruccionId ).ToList();

                            if ( listaFlujo.Count() > 1 ) {
                                worksheet.Cells[1, 1].AddComment("se debe eliminar una carga de flujo de inversión asociada a este Proyecto", "Admin");
                                worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;
                                CantidadResgistrosValidos--;
                            }

                            
                            // #1
                            //Tipo Actividad
                            if (string.IsNullOrEmpty(worksheet.Cells[i, 1].Text))
                            {
                                worksheet.Cells[i, 1].AddComment("Dato Obligatorio", "Admin");
                                worksheet.Cells[i, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;
                            }
                            else if ( new string[3] { "C", "SC", "I" }.Where( r => r == worksheet.Cells[i, 1].Text).Count() == 0)
                            {
                                worksheet.Cells[i, 1].AddComment("Tipo de actividad invalido", "Admin");
                                worksheet.Cells[i, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;
                            }
                            else
                            {
                                temp.TipoActividadCodigo = worksheet.Cells[i, 1].Text;
                            }


                            //#2
                            //Actividad
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 2].Text))
                            {
                                temp.Actividad = worksheet.Cells[i, 2].Text;
                            }
                            else
                            {
                                worksheet.Cells[i, 2].AddComment("Dato Obligatorio", "Admin");
                                worksheet.Cells[i, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;
                            }

                            //#3
                            //Marca de ruta critica
                            if (string.IsNullOrEmpty(worksheet.Cells[i, 3].Text))
                            {
                                temp.EsRutaCritica = false;
                            }
                            else
                            {
                                if (temp.TipoActividadCodigo == "I" && worksheet.Cells[i, 3].Text == "1")
                                {
                                    temp.EsRutaCritica = true;
                                    cantidadRutaCritica++;
                                }
                                else if (temp.TipoActividadCodigo != "I" && worksheet.Cells[i, 3].Text == "1")
                                {
                                    worksheet.Cells[i, 3].AddComment("No se puede asignar ruta critica a este tipo de actividad", "Admin");
                                    worksheet.Cells[i, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    worksheet.Cells[i, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                    tieneErrores = true;
                                }

                            }


                            //#4
                            //Fecha Inicio
                            DateTime fechaTemp;
                            if (string.IsNullOrEmpty(worksheet.Cells[i, 4].Text))
                            {
                                worksheet.Cells[i, 4].AddComment("Dato Obligatorio", "Admin");
                                worksheet.Cells[i, 4].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;
                            }
                            else
                            {
                                temp.FechaInicio = DateTime.TryParse(worksheet.Cells[i, 4].Text, out fechaTemp) ? fechaTemp : DateTime.MinValue;
                            }

                            //#5
                            //Fecha final
                            if (string.IsNullOrEmpty(worksheet.Cells[i, 5].Text))
                            {
                                worksheet.Cells[i, 5].AddComment("Dato Obligatorio", "Admin");
                                worksheet.Cells[i, 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 5].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;
                            }
                            else
                            {
                                temp.FechaFin = DateTime.TryParse(worksheet.Cells[i, 5].Text, out fechaTemp) ? fechaTemp : DateTime.MinValue;
                            }

                            // validacion fechas
                            if (temp.FechaInicio > temp.FechaFin)
                            {
                                worksheet.Cells[i, 5].AddComment("La fecha no puede ser inferior a la fecha inicial", "Admin");
                                worksheet.Cells[i, 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 5].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;

                            }

                            // fechas contrato
                            if (temp.FechaInicio < fechaInicioContrato)
                            {
                                worksheet.Cells[i, 4].AddComment("La fecha Inicial de la actividad no puede ser inferior a la fecha inicial del contrato", "Admin");
                                worksheet.Cells[i, 4].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;
                            }

                            if (temp.FechaFin > fechaFinalContrato)
                            {
                                worksheet.Cells[i, 5].AddComment("La fecha final de la actividad no puede ser mayor a la fecha final del contrato", "Admin");
                                worksheet.Cells[i, 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 5].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;
                            }

                            //#6
                            //Duracion
                            if (string.IsNullOrEmpty(worksheet.Cells[i, 6].Text))
                            {
                                worksheet.Cells[i, 6].AddComment("Dato Obligatorio", "Admin");
                                worksheet.Cells[i, 6].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 6].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;
                            }
                            else
                            {
                                temp.Duracion = Int32.Parse(worksheet.Cells[i, 6].Text);
                            }


                            //Guarda Cambios en una tabla temporal

                            if (!tieneErrores)
                            {
                                _context.TempProgramacion.Add(temp);
                                _context.SaveChanges();
                            }

                            if (temp.TempProgramacionId > 0)
                            {
                                CantidadResgistrosValidos++;
                            }
                            else
                            {
                                CantidadRegistrosInvalidos++;
                            }

                        }
                        catch (Exception ex)
                        {
                            CantidadRegistrosInvalidos++;
                        }
                    }

                    if (cantidadRutaCritica == 0 && worksheet.Cells[1, 1].Comment == null)
                    {
                        worksheet.Cells[1, 1].AddComment("Debe existir por lo menos una ruta critica", "Admin");
                        worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        CantidadRegistrosInvalidos++;
                        CantidadResgistrosValidos--;
                    }

                    //Actualizo el archivoCarge con la cantidad de registros validos , invalidos , y el total;
                    //-2 ya los registros comienzan desde esta fila
                    archivoCarge.CantidadRegistrosInvalidos = CantidadRegistrosInvalidos;
                    archivoCarge.CantidadRegistrosValidos = CantidadResgistrosValidos;
                    archivoCarge.CantidadRegistros = (worksheet.Dimension.Rows - CantidadRegistrosVacios - 1);
                    _context.ArchivoCargue.Update(archivoCarge);


                    ArchivoCargueRespuesta archivoCargueRespuesta = new ArchivoCargueRespuesta
                    {
                        CantidadDeRegistros = archivoCarge.CantidadRegistros.ToString(),
                        CantidadDeRegistrosInvalidos = archivoCarge.CantidadRegistrosInvalidos.ToString(),
                        CantidadDeRegistrosValidos = archivoCarge.CantidadRegistrosValidos.ToString(),
                        LlaveConsulta = archivoCarge.Nombre

                    };

                    byte[] bin = package.GetAsByteArray();
                    string pathFile = archivoCarge.Ruta + "/" + archivoCarge.Nombre + ".xlsx";
                    File.WriteAllBytes(pathFile, bin);


                    return new Respuesta
                    {
                        Data = archivoCargueRespuesta,
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioCreo, "VALIDAR EXCEL PROGRAMACION")
                    };
                }




            }
            else
            {
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesCargueElegibilidad.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.Error, idAccion, pUsuarioCreo, "VALIDAR EXCEL PROGRAMACION")
                };
            }


        }

        public async Task<Respuesta> TransferMassiveLoadProgramming(string pIdDocument, string pUsuarioModifico)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Load_Data_Programacion_Obra, (int)EnumeratorTipoDominio.Acciones);

            Respuesta respuesta = new Respuesta();

            int contratoConstruccionId = 0;

            if (string.IsNullOrEmpty(pIdDocument))
            {
                return respuesta =
                 new Respuesta
                 {
                     IsSuccessful = false,
                     IsException = false,
                     IsValidation = true,
                     Code = GeneralCodes.CamposVacios,
                     Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.CamposVacios, idAccion, pUsuarioModifico, "")
                 };
            }
            try
            {


                int OrigenId = await _commonService.GetDominioIdByCodigoAndTipoDominio(OrigenArchivoCargue.ProgramacionObra, (int)EnumeratorTipoDominio.Origen_Documento_Cargue);

                ArchivoCargue archivoCargue = _context.ArchivoCargue
                                                .Where(r => r.OrigenId == 3 &&
                                                        r.Nombre.Trim().ToUpper().Equals(pIdDocument.ToUpper().Trim())
                                                      )
                                                .FirstOrDefault();

                List<TempProgramacion> listTempProgramacion = await _context.TempProgramacion
                                                                .Where(r => r.ArchivoCargueId == archivoCargue.ArchivoCargueId && !(bool)r.EstaValidado)
                                                                .ToListAsync();



                if (listTempProgramacion.Count() > 0)
                {

                    contratoConstruccionId = listTempProgramacion.FirstOrDefault().ContratoConstruccionId;

                    // Eliminar meses ya cargados
                    List<MesEjecucion> listaMeses = _context.MesEjecucion.Where(m => m.ContratoConstruccionId == contratoConstruccionId).ToList();
                    _context.MesEjecucion.RemoveRange(listaMeses);

                    //eliminar Programacion
                    List<Programacion> listaProgramacion = _context.Programacion.Where(p => p.ContratoConstruccionId == contratoConstruccionId).ToList();
                    _context.Programacion.RemoveRange(listaProgramacion);

                    // copia la información
                    foreach (TempProgramacion tempProgramacion in listTempProgramacion)
                    {

                        Programacion programacion = new Programacion()
                        {
                            ContratoConstruccionId = tempProgramacion.ContratoConstruccionId,
                            TipoActividadCodigo = tempProgramacion.TipoActividadCodigo,
                            Actividad = tempProgramacion.Actividad,
                            EsRutaCritica = tempProgramacion.EsRutaCritica,
                            FechaInicio = tempProgramacion.FechaInicio,
                            FechaFin = tempProgramacion.FechaFin,
                            Duracion = tempProgramacion.Duracion

                        };

                        _context.Programacion.Add(programacion);
                        _context.SaveChanges();



                        //Temporal proyecto update
                        tempProgramacion.EstaValidado = true;
                        tempProgramacion.FechaModificacion = DateTime.Now;
                        tempProgramacion.UsuarioModificacion = pUsuarioModifico;
                        _context.TempProgramacion.Update(tempProgramacion);
                        _context.SaveChanges();
                    }

                    ContratoConstruccion contratoConstruccion = _context.ContratoConstruccion.Find(contratoConstruccionId);

                    Proyecto proyecto = CalcularFechaInicioContrato(contratoConstruccionId);

                    int numeroMes = 1;
                    int idMes = 0;
                    for (DateTime fecha = proyecto.FechaInicioEtapaObra; fecha <= proyecto.FechaFinEtapaObra; fecha = fecha.AddMonths(1))
                    {

                        MesEjecucion mes = new MesEjecucion()
                        {
                            ContratoConstruccionId = contratoConstruccionId,
                            Numero = numeroMes,
                            FechaInicio = fecha,
                            FechaFin = fecha.AddMonths(1).AddDays(-1),

                        };

                        _context.MesEjecucion.Add(mes);
                        numeroMes++;
                    }
                    _context.SaveChanges();

                    MesEjecucion ultimoMes = _context.MesEjecucion.Where(m => m.ContratoConstruccionId == contratoConstruccionId).OrderByDescending(m => m.Numero).FirstOrDefault();
                    ultimoMes.FechaFin = proyecto.FechaFinEtapaObra;

                    _context.SaveChanges();

                    if (contratoConstruccion != null)
                    {
                        contratoConstruccion.ArchivoCargueIdProgramacionObra = archivoCargue.ArchivoCargueId;
                        contratoConstruccion.RegistroCompletoProgramacionObra = true;

                        VerificarRegistroCompletoContratoObra(contratoConstruccion.ContratoId);
                    }



                    return respuesta =
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = true,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioModifico, "Cantidad de registros subidos : " + listTempProgramacion.Count())
                    };
                }
                else
                {
                    return respuesta =
                        new Respuesta
                        {
                            IsSuccessful = false,
                            IsException = false,
                            IsValidation = true,
                            Code = GeneralCodes.OperacionExitosa,
                            Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.NoExitenArchivos, idAccion, pUsuarioModifico, "")
                        };
                }
            }
            catch (Exception ex)
            {
                return respuesta =
                    new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = false,
                        IsValidation = true,
                        Code = ConstantMessagesCargueElegibilidad.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoProyecto, ConstantMessagesCargueElegibilidad.Error, (int)enumeratorAccion.CargueProyectosMasivos, pUsuarioModifico, ex.InnerException.ToString())
                    };
            }

        }

        public async Task<Respuesta> UploadFileToValidateInvestmentFlow(IFormFile pFile, string pFilePatch, string pUsuarioCreo, int pContratoConstruccionId, int pContratoId, int pProyectoId)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Validar_Excel_Flujo_Inversion, (int)EnumeratorTipoDominio.Acciones);

            if ( pContratoConstruccionId == 0 ){
                ContratoConstruccion contratoTemp = new ContratoConstruccion();

                contratoTemp.UsuarioCreacion = pUsuarioCreo;
                contratoTemp.FechaCreacion = DateTime.Now;

                contratoTemp.ContratoId = pContratoId;
                contratoTemp.ProyectoId = pProyectoId;

                _context.ContratoConstruccion.Add( contratoTemp);
                _context.SaveChanges();
                pContratoConstruccionId = contratoTemp.ContratoConstruccionId;
            }

            int CantidadRegistrosVacios = 0;
            int CantidadResgistrosValidos = 0;
            int CantidadRegistrosInvalidos = 0;

            // rango de fechas
            //Proyecto proyecto = CalcularFechaInicioContrato(pContratoConstruccionId);
            Proyecto proyecto = CalcularFechasContrato(pProyectoId);

            //Numero semanas
            int numberOfWeeks = Convert.ToInt32(Math.Floor((proyecto.FechaFinEtapaObra - proyecto.FechaInicioEtapaObra).TotalDays / 7));
            if (Convert.ToInt32(Math.Round((proyecto.FechaFinEtapaObra - proyecto.FechaInicioEtapaObra).TotalDays % 7)) > 0)
                numberOfWeeks++;

            //Capitulos cargados
            Programacion[] listaProgramacion = _context.Programacion
                                                                .Where(
                                                                        p => p.ContratoConstruccionId == pContratoConstruccionId &&
                                                                        p.TipoActividadCodigo == "C")
                                                                .OrderBy(p => p.ProgramacionId)
                                                                .ToArray();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //int OrigenId = await _commonService.GetDominioIdByCodigoAndTipoDominio(OrigenArchivoCargue.Proyecto, (int)EnumeratorTipoDominio.Origen_Documento_Cargue);
            DocumentService _documentService = new DocumentService(_context, _commonService);

            ArchivoCargue archivoCarge = await _documentService.getSaveFile(pFile, pFilePatch, Int32.Parse(OrigenArchivoCargue.FlujoInversion), pContratoConstruccionId);

            // if (!string.IsNullOrEmpty(archivoCarge.ArchivoCargueId.ToString()))
            if (archivoCarge != null)
            {
                using (var stream = new MemoryStream())
                {
                    await pFile.CopyToAsync(stream);

                    using var package = new ExcelPackage(stream);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

                    int cantidadCapitulos = 0;
                    int cantidadSemnas = 0;

                    int posicion = 2;
                    while (!string.IsNullOrEmpty(worksheet.Cells[1, posicion++].Text))
                    {
                        cantidadSemnas++;
                    }

                    posicion = 2;
                    while (!string.IsNullOrEmpty(worksheet.Cells[posicion++, 1].Text))
                    {
                        cantidadCapitulos++;
                    }

                    bool tieneErrores = false;

                    // valida numero semanas
                    if (numberOfWeeks != cantidadSemnas)
                    {
                        worksheet.Cells[1, 1].AddComment("Numero de semanas no es igual al del proyecto", "Admin");
                        worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        CantidadRegistrosInvalidos++;
                        CantidadResgistrosValidos--;
                        tieneErrores = true;
                    }

                    //valida numero capitulos
                    if (listaProgramacion.Count() != cantidadCapitulos && worksheet.Cells[1, 1].Comment == null)
                    {
                        worksheet.Cells[1, 1].AddComment("Numero de capitulos no es igual a la programacion", "Admin");
                        worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        CantidadRegistrosInvalidos++;
                        CantidadResgistrosValidos--;
                        tieneErrores = true;
                    }

                    decimal sumaTotal = 0;

                    // Capitulos
                    //int i = 2;
                    for (int i = 2; i <= cantidadCapitulos + 1; i++)
                    {
                        try
                        {
                            // semanas
                            //int k = 2;
                            for (int k = 2; k < cantidadSemnas + 2; k++)
                            {

                                TempFlujoInversion temp = new TempFlujoInversion();
                                //Auditoria
                                temp.ArchivoCargueId = archivoCarge.ArchivoCargueId;
                                temp.EstaValidado = false;
                                temp.FechaCreacion = DateTime.Now;
                                temp.UsuarioCreacion = pUsuarioCreo;
                                temp.ContratoConstruccionId = pContratoConstruccionId;
                                temp.Posicion = k - 2;
                                temp.PosicionCapitulo = i - 2;

                                //Valores
                                // Mes
                                if (string.IsNullOrEmpty(worksheet.Cells[1, k].Text))
                                {
                                    worksheet.Cells[1, k].AddComment("Dato Obligatorio", "Admin");
                                    worksheet.Cells[1, k].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    worksheet.Cells[1, k].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                    tieneErrores = true;

                                }
                                else
                                {
                                    temp.Semana = worksheet.Cells[1, k].Text;
                                }

                                //Capitulo
                                if (string.IsNullOrEmpty(worksheet.Cells[i, 1].Text))
                                {
                                    worksheet.Cells[i, 1].AddComment("Dato Obligatorio", "Admin");
                                    worksheet.Cells[i, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    worksheet.Cells[i, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                    tieneErrores = true;
                                }
                                else
                                {
                                    temp.ProgramacionId = listaProgramacion[i - 2].ProgramacionId;
                                }

                                //Valor
                                temp.Valor = string.IsNullOrEmpty(worksheet.Cells[i, k].Text) ? 0 : decimal.Parse(worksheet.Cells[i, k].Text);
                                sumaTotal += temp.Valor.Value;

                                //Guarda Cambios en una tabla temporal

                                if (!tieneErrores)
                                {
                                    _context.TempFlujoInversion.Add(temp);
                                    _context.SaveChanges();
                                }



                                if (temp.TempFlujoInversionId > 0)
                                {
                                    CantidadResgistrosValidos++;
                                }
                                else
                                {
                                    CantidadRegistrosInvalidos++;
                                }

                            }


                        }
                        catch (Exception ex)
                        {
                            CantidadRegistrosInvalidos++;
                        }

                    }

                    if (proyecto.ValorObra != sumaTotal && worksheet.Cells[1, 1].Comment == null)
                    {
                        worksheet.Cells[1, 1].AddComment("La suma de los valores no es igual al valor total de obra del proyecto", "Admin");
                        worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        CantidadRegistrosInvalidos++;
                        CantidadResgistrosValidos--;
                    }

                    //Actualizo el archivoCarge con la cantidad de registros validos , invalidos , y el total;
                    //-2 ya los registros comienzan desde esta fila
                    archivoCarge.CantidadRegistrosInvalidos = CantidadRegistrosInvalidos;
                    archivoCarge.CantidadRegistrosValidos = CantidadResgistrosValidos;
                    archivoCarge.CantidadRegistros = (cantidadCapitulos * cantidadSemnas) - CantidadRegistrosVacios;
                    _context.ArchivoCargue.Update(archivoCarge);


                    ArchivoCargueRespuesta archivoCargueRespuesta = new ArchivoCargueRespuesta
                    {
                        CantidadDeRegistros = archivoCarge.CantidadRegistros.ToString(),
                        CantidadDeRegistrosInvalidos = archivoCarge.CantidadRegistrosInvalidos.ToString(),
                        CantidadDeRegistrosValidos = archivoCarge.CantidadRegistrosValidos.ToString(),
                        LlaveConsulta = archivoCarge.Nombre

                    };

                    byte[] bin = package.GetAsByteArray();
                    string pathFile = archivoCarge.Ruta + "/" + archivoCarge.Nombre + ".xlsx";
                    File.WriteAllBytes(pathFile, bin);

                    return new Respuesta
                    {
                        Data = archivoCargueRespuesta,
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioCreo, "VALIDAR EXCEL FLUJO IN")
                    };
                }
            }
            else
            {
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesCargueElegibilidad.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.Error, idAccion, pUsuarioCreo, "VALIDAR EXCEL FLUJO INV")
                };
            }


        }

        public async Task<Respuesta> TransferMassiveLoadInvestmentFlow(string pIdDocument, string pUsuarioModifico)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Load_Data_Flujo_Inversion, (int)EnumeratorTipoDominio.Acciones);

            Respuesta respuesta = new Respuesta();

            int contratoConstruccionId = 0;

            if (string.IsNullOrEmpty(pIdDocument))
            {
                return respuesta =
                 new Respuesta
                 {
                     IsSuccessful = false,
                     IsException = false,
                     IsValidation = true,
                     Code = GeneralCodes.CamposVacios,
                     Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.CamposVacios, idAccion, pUsuarioModifico, "")
                 };
            }
            try
            {


                int OrigenId = await _commonService.GetDominioIdByCodigoAndTipoDominio(OrigenArchivoCargue.FlujoInversion, (int)EnumeratorTipoDominio.Origen_Documento_Cargue);

                ArchivoCargue archivoCargue = _context.ArchivoCargue
                                                .Where(r => r.OrigenId == 4 &&
                                                        r.Nombre.Trim().ToUpper().Equals(pIdDocument.ToUpper().Trim())
                                                      )
                                                .FirstOrDefault();

                List<TempFlujoInversion> listTempFlujoInversion = await _context.TempFlujoInversion
                                                                .Where(r => r.ArchivoCargueId == archivoCargue.ArchivoCargueId && !(bool)r.EstaValidado)
                                                                .ToListAsync();

                if (listTempFlujoInversion.Count() > 0)
                {
                    contratoConstruccionId = listTempFlujoInversion.FirstOrDefault().ContratoConstruccionId;

                    Proyecto proyecto = CalcularFechaInicioContrato(contratoConstruccionId);

                    ContratoConstruccion contratoConstruccion = _context.ContratoConstruccion
                                                                            .Where(r => r.ContratoConstruccionId == contratoConstruccionId)
                                                                            .Include(r => r.Contrato)
                                                                                .ThenInclude(r => r.Contratacion)
                                                                                    .ThenInclude(r => r.ContratacionProyecto)
                                                                            .Include(r => r.Proyecto)

                                                                            .FirstOrDefault();

                    //carga el seguimiento semanal 
                    ContratacionProyecto contratacionProyecto = contratoConstruccion.Contrato.Contratacion.ContratacionProyecto.Where(p => p.ProyectoId == contratoConstruccion.ProyectoId).FirstOrDefault();
                    if (contratacionProyecto != null)
                    {
                        List<dynamic> listaFechas = new List<dynamic>();

                        DateTime fechaTemp = proyecto.FechaInicioEtapaObra;

                        while (proyecto.FechaFinEtapaObra >= fechaTemp)
                        {
                            listaFechas.Add(new { fechaInicio = fechaTemp, fechaFin = fechaTemp.AddDays(6) });
                            fechaTemp = fechaTemp.AddDays(7);
                        }

                        int idContratacionproyecto = contratacionProyecto.ContratacionProyectoId;

                        List<SeguimientoSemanal> listaSeguimientos = _context.SeguimientoSemanal
                                                                    .Where(p => p.ContratacionProyectoId == idContratacionproyecto).ToList();

                        // eliminar registros cargados
                        List<FlujoInversion> listaFlujo = _context.FlujoInversion.Where(r => r.ContratoConstruccionId == contratoConstruccionId).ToList();
                        _context.FlujoInversion.RemoveRange(listaFlujo);

                        // elimina los existentes
                        _context.SeguimientoSemanal.RemoveRange(listaSeguimientos);

                        int i = 1;
                        listaFechas.OrderBy(p => p.fechaInicio).ToList().ForEach(f =>
                        {

                            SeguimientoSemanal seguimientoSemanal = new SeguimientoSemanal()
                            {
                                ContratacionProyectoId = idContratacionproyecto,
                                Eliminado = false,
                                UsuarioCreacion = pUsuarioModifico,
                                FechaCreacion = DateTime.Now,
                                NumeroSemana = i,
                                FechaInicio = f.fechaInicio,
                                FechaFin = f.fechaFin,
                                RegistroCompleto = false,
                            };

                            _context.SeguimientoSemanal.Add(seguimientoSemanal);
                            _context.SaveChanges();

                            i++;

                        });

                        SeguimientoSemanal seguimientoSemanal = _context.SeguimientoSemanal
                                                                            .Where(s => s.ContratacionProyectoId == idContratacionproyecto)
                                                                            .OrderByDescending(s => s.NumeroSemana)
                                                                            .FirstOrDefault();

                        seguimientoSemanal.FechaFin = proyecto.FechaFinEtapaObra;
                        _context.SaveChanges();

                    }

                    SeguimientoSemanal[] listaSeguimientoSemanal = _context.SeguimientoSemanal
                                                                                    .Where(s => s.ContratacionProyectoId == contratacionProyecto.ContratacionProyectoId)
                                                                                    .OrderBy(s => s.NumeroSemana)
                                                                                    .ToArray();

                    MesEjecucion[] listaMeses = _context.MesEjecucion
                                                            .Where(s => s.ContratoConstruccionId == contratoConstruccionId)
                                                            .OrderBy(s => s.Numero)
                                                            .ToArray();

                    Programacion[] listaProgramacion = _context.Programacion
                                                                .Where(
                                                                        p => p.ContratoConstruccionId == contratoConstruccionId &&
                                                                        p.TipoActividadCodigo == "C")
                                                                .OrderBy(p => p.ProgramacionId)
                                                                .ToArray();



                    _context.SaveChanges();

                    listTempFlujoInversion.ForEach(tempFlujo =>
                    {

                        int? mesId = 0;

                        listaMeses.ToList().ForEach(m =>
                        {
                            if (
                                    listaSeguimientoSemanal[tempFlujo.Posicion.Value].FechaInicio.Value.Date >= m.FechaInicio.Date &&
                                    listaSeguimientoSemanal[tempFlujo.Posicion.Value].FechaFin.Value.Date <= m.FechaFin.Date
                                )
                            {
                                mesId = m.MesEjecucionId;
                            }
                            else if (
                                       listaSeguimientoSemanal[tempFlujo.Posicion.Value].FechaInicio.Value.Date <= m.FechaFin &&
                                       listaSeguimientoSemanal[tempFlujo.Posicion.Value].FechaFin.Value.Date >= m.FechaFin
                                    )
                            {
                                mesId = m.MesEjecucionId;
                            }
                        });
                        FlujoInversion flujo = new FlujoInversion()
                        {
                            ContratoConstruccionId = tempFlujo.ContratoConstruccionId,
                            Semana = tempFlujo.Semana,
                            Valor = tempFlujo.Valor,
                            SeguimientoSemanalId = listaSeguimientoSemanal[tempFlujo.Posicion.Value].SeguimientoSemanalId,
                            MesEjecucionId = mesId.Value == 0 ? null : mesId,
                            ProgramacionId = listaProgramacion[tempFlujo.PosicionCapitulo.Value].ProgramacionId,

                        };

                        _context.FlujoInversion.Add(flujo);
                        //_context.SaveChanges();



                        //Temporal proyecto update
                        tempFlujo.EstaValidado = true;
                        tempFlujo.FechaModificacion = DateTime.Now;
                        tempFlujo.UsuarioModificacion = pUsuarioModifico;
                        //_context.TempFlujoInversion.Update(tempFlujo);
                        _context.SaveChanges();

                    });



                    if (contratoConstruccion != null)
                    {
                        List<dynamic> listaFechas = new List<dynamic>();

                        contratoConstruccion.ArchivoCargueIdFlujoInversion = archivoCargue.ArchivoCargueId;
                        contratoConstruccion.RegistroCompletoFlujoInversion = true;

                        VerificarRegistroCompletoContratoObra(contratoConstruccion.ContratoId);


                    }

                    return respuesta =
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = true,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioModifico, "Cantidad de registros subidos : " + listTempFlujoInversion.Count())
                    };
                }
                else
                {
                    return respuesta =
                        new Respuesta
                        {
                            IsSuccessful = false,
                            IsException = false,
                            IsValidation = true,
                            Code = GeneralCodes.OperacionExitosa,
                            Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.NoExitenArchivos, idAccion, pUsuarioModifico, "")
                        };
                }
            }
            catch (Exception ex)
            {
                return respuesta =
                    new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = false,
                        IsValidation = true,
                        Code = ConstantMessagesCargueElegibilidad.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoProyecto, ConstantMessagesCargueElegibilidad.Error, (int)enumeratorAccion.CargueProyectosMasivos, pUsuarioModifico, ex.InnerException.ToString())
                    };
            }

        }

        #endregion

        #region Observaciones 

        private ConstruccionObservacion getObservacion(ContratoConstruccion pContratoConstruccion, string pTipoObservacion, bool pEsSupervicion)
        {
            ConstruccionObservacion observacion = new ConstruccionObservacion();

            ConstruccionObservacion construccionObservacion = pContratoConstruccion.ConstruccionObservacion.ToList()
                        .Where(r => r.TipoObservacionConstruccion == pTipoObservacion &&
                                    r.EsSupervision == pEsSupervicion &&
                                    r.Archivada != true
                              )
                        //.OrderByDescending(o => o.FechaCreacion)
                        .FirstOrDefault();

            if (construccionObservacion != null)
                observacion = construccionObservacion;

            return construccionObservacion;
        }

        private ConstruccionPerfilObservacion getObservacionPerfil(ConstruccionPerfil pPerfil, bool pEsSupervicion)
        {
            ConstruccionPerfilObservacion observacion = new ConstruccionPerfilObservacion();

            ConstruccionPerfilObservacion construccionObservacionPerfil = pPerfil.ConstruccionPerfilObservacion.ToList()
                        .Where(r => r.EsSupervision == pEsSupervicion &&
                                    r.Archivada != true
                              )
                        //.OrderByDescending(o => o.FechaCreacion)
                        .FirstOrDefault();

            if (construccionObservacionPerfil != null)
                observacion = construccionObservacionPerfil;

            return construccionObservacionPerfil;
        }

        public async Task<Respuesta> CreateEditObservacion(ContratoConstruccion pContratoConstruccion, string pTipoObservacion, bool pEsSupervicion)
        {
            return pTipoObservacion switch
            {
                ConstanCodigoTipoObservacionConstruccion.Diagnostico => await this.CreateEditObservacionDiagnostico(pContratoConstruccion, pEsSupervicion),
                ConstanCodigoTipoObservacionConstruccion.PlanesProgramas => await this.CreateEditObservacionPlanesProgramas(pContratoConstruccion, pEsSupervicion),
                ConstanCodigoTipoObservacionConstruccion.ManejoAnticipo => await this.CreateEditObservacionManejoAnticipo(pContratoConstruccion, pEsSupervicion),
                ConstanCodigoTipoObservacionConstruccion.ProgramacionObra => await this.CreateEditObservacionProgramacionObra(pContratoConstruccion, pEsSupervicion),
                ConstanCodigoTipoObservacionConstruccion.FlujoInversion => await this.CreateEditObservacionFlujoInversion(pContratoConstruccion, pEsSupervicion),
                _ => new Respuesta(),
            };
        }

        private async Task<Respuesta> CreateEditObservacionConstruccion(ConstruccionObservacion pObservacion, string pUsuarioCreacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Observacion_Construccion, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = "";

            Respuesta respuesta = new Respuesta();
            try
            {
                if (pObservacion.ConstruccionObservacionId > 0)
                {
                    strCrearEditar = "EDITAR OBSERVACION CONSTRUCCION";
                    ConstruccionObservacion construccionObservacion = _context.ConstruccionObservacion.Find(pObservacion.ConstruccionObservacionId);

                    construccionObservacion.FechaModificacion = DateTime.Now;
                    construccionObservacion.UsuarioModificacion = pUsuarioCreacion;

                    construccionObservacion.Observaciones = pObservacion.Observaciones;

                }
                else
                {
                    strCrearEditar = "CREAR OBSERVACION CONSTRUCCION";

                    ConstruccionObservacion construccionObservacion = new ConstruccionObservacion
                    {
                        FechaCreacion = DateTime.Now,
                        UsuarioCreacion = pUsuarioCreacion,

                        ContratoConstruccionId = pObservacion.ContratoConstruccionId,
                        TipoObservacionConstruccion = pObservacion.TipoObservacionConstruccion,
                        Observaciones = pObservacion.Observaciones,
                        EsSupervision = pObservacion.EsSupervision,
                        EsActa = pObservacion.EsActa
                    };

                    _context.ConstruccionObservacion.Add(construccionObservacion);
                }

                return respuesta;
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Verificar_Requisitos_Tecnicos_Construccion, GeneralCodes.Error, idAccion, pObservacion.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        public async Task<Respuesta> CreateEditObservacionConstruccionPerfilSave(ConstruccionPerfilObservacion pObservacion, string pUsuarioCreacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Observacion_Construccion, (int)EnumeratorTipoDominio.Acciones);
            Respuesta respuesta = new Respuesta();
            try
            {

                string strCrearEditar;
                if (pObservacion.ConstruccionPerfilObservacionId > 0)
                {
                    strCrearEditar = "EDITAR OBSERVACION CONSTRUCCION PERFIL";
                    ConstruccionPerfilObservacion construccionObservacionPerfil = _context.ConstruccionPerfilObservacion.Where(r => r.ConstruccionPerfilObservacionId == pObservacion.ConstruccionPerfilObservacionId).FirstOrDefault();

                    construccionObservacionPerfil.FechaModificacion = DateTime.Now;
                    construccionObservacionPerfil.UsuarioModificacion = pUsuarioCreacion;

                    construccionObservacionPerfil.Observacion = pObservacion.Observacion;

                }
                else
                {
                    strCrearEditar = "CREAR OBSERVACION CONSTRUCCION PERFIL";

                    ConstruccionPerfilObservacion construccionObservacionPerfil = new ConstruccionPerfilObservacion
                    {
                        FechaCreacion = DateTime.Now,
                        UsuarioCreacion = pUsuarioCreacion,
                        ConstruccionPerfilId = pObservacion.ConstruccionPerfilId,
                        ConstruccionPerfilObservacionId = pObservacion.ConstruccionPerfilObservacionId,
                        TipoObservacionCodigo = pObservacion.TipoObservacionCodigo, // no se usa
                        Observacion = pObservacion.Observacion,
                        EsSupervision = pObservacion.EsSupervision,
                    };

                    _context.ConstruccionPerfilObservacion.Add(construccionObservacionPerfil);
                }
                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Verificar_Requisitos_Tecnicos_Construccion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioCreacion, strCrearEditar)
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Verificar_Requisitos_Tecnicos_Construccion, GeneralCodes.Error, idAccion, pObservacion.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        public async Task<Respuesta> CreateEditObservacionConstruccionPerfil(ConstruccionPerfilObservacion pObservacion, string pUsuarioCreacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Observacion_Construccion, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = "";

            Respuesta respuesta = new Respuesta();
            try
            {
                if (pObservacion.ConstruccionPerfilObservacionId > 0)
                {
                    strCrearEditar = "EDITAR OBSERVACION CONSTRUCCION PERFIL";
                    ConstruccionPerfilObservacion construccionObservacionPerfil = _context.ConstruccionPerfilObservacion.Find(pObservacion.ConstruccionPerfilObservacionId);

                    construccionObservacionPerfil.FechaModificacion = DateTime.Now;
                    construccionObservacionPerfil.UsuarioModificacion = pUsuarioCreacion;

                    construccionObservacionPerfil.Observacion = pObservacion.Observacion;

                }
                else
                {
                    strCrearEditar = "CREAR OBSERVACION CONSTRUCCION PERFIL";

                    ConstruccionPerfilObservacion construccionObservacionPerfil = new ConstruccionPerfilObservacion
                    {
                        FechaCreacion = DateTime.Now,
                        UsuarioCreacion = pUsuarioCreacion,
                        ConstruccionPerfilId = pObservacion.ConstruccionPerfilId,
                        ConstruccionPerfilObservacionId = pObservacion.ConstruccionPerfilObservacionId,
                        TipoObservacionCodigo = pObservacion.TipoObservacionCodigo, // no se usa
                        Observacion = pObservacion.Observacion,
                        EsSupervision = pObservacion.EsSupervision,
                    };

                    _context.ConstruccionPerfilObservacion.Add(construccionObservacionPerfil);
                }

                return respuesta;
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Verificar_Requisitos_Tecnicos_Construccion, GeneralCodes.Error, idAccion, pObservacion.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        public async Task<Respuesta> CreateEditObservacionDiagnostico(ContratoConstruccion pContratoConstruccion, bool esSupervisor)
        {
            Respuesta respuesta = new Respuesta();

            string CreateEdit = string.Empty;
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Observacion_Construccion_Diagnostico, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                CreateEdit = "EDIT OBSERVACION DIAGNOSTICO";
                int idObservacion = 0;

                if (pContratoConstruccion.ConstruccionObservacion.Count() > 0)
                    idObservacion = pContratoConstruccion.ConstruccionObservacion.FirstOrDefault().ConstruccionObservacionId;

                ContratoConstruccion contratoConstruccion = _context.ContratoConstruccion.Find(pContratoConstruccion.ContratoConstruccionId);

                contratoConstruccion.UsuarioModificacion = pContratoConstruccion.UsuarioCreacion;
                contratoConstruccion.FechaModificacion = DateTime.Now;

                if (esSupervisor)
                {

                    contratoConstruccion.TieneObservacionesDiagnosticoSupervisor = pContratoConstruccion.TieneObservacionesDiagnosticoSupervisor;

                    if (contratoConstruccion.TieneObservacionesDiagnosticoSupervisor.Value)
                    {

                        await CreateEditObservacionConstruccion(pContratoConstruccion.ConstruccionObservacion.FirstOrDefault(), pContratoConstruccion.UsuarioCreacion);
                    }
                    else
                    {
                        ConstruccionObservacion observacionDelete = _context.ConstruccionObservacion.Find(idObservacion);

                        if (observacionDelete != null)
                            observacionDelete.Eliminado = true;
                    }

                }
                else
                {
                    contratoConstruccion.TieneObservacionesDiagnosticoApoyo = pContratoConstruccion.TieneObservacionesDiagnosticoApoyo;

                    if (contratoConstruccion.TieneObservacionesDiagnosticoApoyo.Value)
                    {
                        await CreateEditObservacionConstruccion(pContratoConstruccion.ConstruccionObservacion.FirstOrDefault(), pContratoConstruccion.UsuarioCreacion);
                    }
                    else
                    {
                        ConstruccionObservacion observacionDelete = _context.ConstruccionObservacion.Find(idObservacion);

                        if (observacionDelete != null)
                            observacionDelete.Eliminado = true;
                    }
                }
                var contrato = _context.Contrato
                    .Where(x => x.ContratoId == contratoConstruccion.ContratoId)
                    .Include(x => x.Contratacion).FirstOrDefault();

                //3.1.11
                if (!esSupervisor){
                    if (contrato.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString())//cambiar esto, no encontre la constante inteventoria
                    {
                        contratoConstruccion.RegistroCompletoVerificacion = await ValidarRegistroCompletoVerificacionPreconstruccion(contratoConstruccion.ContratoConstruccionId, esSupervisor);
                    }
                    else
                    {
                        contratoConstruccion.RegistroCompletoVerificacion = await ValidarRegistroCompletoVerificacionContruccion(contratoConstruccion.ContratoConstruccionId, esSupervisor);
                    }

                    if (contratoConstruccion.RegistroCompletoVerificacion.Value)
                    {
                        contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.Con_requisitos_tecnicos_verificados;
                        contrato.FechaAprobacionRequisitosConstruccionApoyo = DateTime.Now;
                    }
                    else
                    {
                        contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.En_proceso_de_verificacion_de_requisitos_tecnicos;
                        contrato.FechaAprobacionRequisitosConstruccionApoyo = null;
                    }
                }

                //3.1.12
                if (esSupervisor)
                {
                    contratoConstruccion.RegistroCompletoValidacion = await ValidarRegistroCompletoValidacionObraInterventoria(contratoConstruccion.ContratoConstruccionId);

                    if ((bool)contratoConstruccion.RegistroCompletoValidacion)
                        contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.En_proceso_de_validacion_de_requisitos_tecnicos;
                    else
                        contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.Con_requisitos_tecnicos_validados;

                    VerificarRegistroCompletoValidacionContratoObra(contratoConstruccion.ContratoId);
                }
                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Verificar_Requisitos_Tecnicos_Construccion, GeneralCodes.OperacionExitosa, idAccion, pContratoConstruccion.UsuarioCreacion, CreateEdit)
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Verificar_Requisitos_Tecnicos_Construccion, GeneralCodes.Error, idAccion, pContratoConstruccion.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        public async Task<Respuesta> CreateEditObservacionPlanesProgramas(ContratoConstruccion pContratoConstruccion, bool esSupervisor)
        {
            Respuesta respuesta = new Respuesta();

            string CreateEdit = string.Empty;
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Observacion_Construccion_PlanesProgramas, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                CreateEdit = "EDIT OBSERVACION PLANESPROGRAMAS";
                int idObservacion = 0;

                if (pContratoConstruccion.ConstruccionObservacion.Count() > 0)
                    idObservacion = pContratoConstruccion.ConstruccionObservacion.FirstOrDefault().ConstruccionObservacionId;

                ContratoConstruccion contratoConstruccion = _context.ContratoConstruccion.Find(pContratoConstruccion.ContratoConstruccionId);

                contratoConstruccion.UsuarioModificacion = pContratoConstruccion.UsuarioCreacion;
                contratoConstruccion.FechaModificacion = DateTime.Now;

                if (esSupervisor)
                {

                    contratoConstruccion.TieneObservacionesPlanesProgramasSupervisor = pContratoConstruccion.TieneObservacionesPlanesProgramasSupervisor;

                    if (contratoConstruccion.TieneObservacionesPlanesProgramasSupervisor.Value)
                    {

                        await CreateEditObservacionConstruccion(pContratoConstruccion.ConstruccionObservacion.FirstOrDefault(), pContratoConstruccion.UsuarioCreacion);
                    }
                    else
                    {
                        ConstruccionObservacion observacionDelete = _context.ConstruccionObservacion.Find(idObservacion);
                        
                        if (observacionDelete != null)
                            observacionDelete.Eliminado = true;
                    }

                }
                else
                {
                    contratoConstruccion.TieneObservacionesPlanesProgramasApoyo = pContratoConstruccion.TieneObservacionesPlanesProgramasApoyo;

                    if (contratoConstruccion.TieneObservacionesPlanesProgramasApoyo.Value)
                    {
                        await CreateEditObservacionConstruccion(pContratoConstruccion.ConstruccionObservacion.FirstOrDefault(), pContratoConstruccion.UsuarioCreacion);
                    }
                    else
                    {
                        ConstruccionObservacion observacionDelete = _context.ConstruccionObservacion.Find(idObservacion);
                        
                        if (observacionDelete != null)
                            observacionDelete.Eliminado = true;
                    }
                }

                var contrato = _context.Contrato.Where(x => x.ContratoId == contratoConstruccion.ContratoId).Include(x => x.Contratacion).FirstOrDefault();

                //3.1.11
                if (!esSupervisor){
                    if (contrato.Contratacion.TipoSolicitudCodigo == "2")//cambiar esto, no encontre la constante inteventoria
                    {
                        contratoConstruccion.RegistroCompletoVerificacion = await ValidarRegistroCompletoVerificacionPreconstruccion(contratoConstruccion.ContratoConstruccionId, esSupervisor);
                    }
                    else
                    {
                        contratoConstruccion.RegistroCompletoVerificacion = await ValidarRegistroCompletoVerificacionContruccion(contratoConstruccion.ContratoConstruccionId, esSupervisor);
                    }
                    if (contratoConstruccion.RegistroCompletoVerificacion.Value)
                    {
                        contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.Con_requisitos_tecnicos_verificados;
                        contrato.FechaAprobacionRequisitosConstruccionApoyo = DateTime.Now;
                    }
                    else
                    {
                        contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.En_proceso_de_verificacion_de_requisitos_tecnicos;
                        contrato.FechaAprobacionRequisitosConstruccionApoyo = null;
                    }
                }

                //3.1.12
                if (esSupervisor)
                {
                    contratoConstruccion.RegistroCompletoValidacion = await ValidarRegistroCompletoValidacionObraInterventoria(contratoConstruccion.ContratoConstruccionId);

                    if ((bool)contratoConstruccion.RegistroCompletoValidacion)
                        contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.En_proceso_de_validacion_de_requisitos_tecnicos;
                    else
                        contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.Con_requisitos_tecnicos_validados;

                    VerificarRegistroCompletoValidacionContratoObra(contratoConstruccion.ContratoId);
                }

                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Verificar_Requisitos_Tecnicos_Construccion, GeneralCodes.OperacionExitosa, idAccion, pContratoConstruccion.UsuarioCreacion, CreateEdit)
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Verificar_Requisitos_Tecnicos_Construccion, GeneralCodes.Error, idAccion, pContratoConstruccion.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        public async Task<Respuesta> CreateEditObservacionManejoAnticipo(ContratoConstruccion pContratoConstruccion, bool esSupervisor)
        {
            Respuesta respuesta = new Respuesta();

            string CreateEdit = string.Empty;
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Observacion_Construccion_ManejoAnticipo, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                CreateEdit = "EDIT OBSERVACION MANEJOANTICIPO";

                ContratoConstruccion contratoConstruccion = _context.ContratoConstruccion.Find(pContratoConstruccion.ContratoConstruccionId);

                contratoConstruccion.UsuarioModificacion = pContratoConstruccion.UsuarioCreacion;
                contratoConstruccion.FechaModificacion = DateTime.Now;

                if (esSupervisor)
                {

                    contratoConstruccion.TieneObservacionesManejoAnticipoSupervisor = pContratoConstruccion.TieneObservacionesManejoAnticipoSupervisor;

                    if (contratoConstruccion.TieneObservacionesManejoAnticipoSupervisor.Value)
                    {

                        await CreateEditObservacionConstruccion(pContratoConstruccion.ConstruccionObservacion.FirstOrDefault(), pContratoConstruccion.UsuarioCreacion);
                    }
                    else
                    {
                        ConstruccionObservacion observacionDelete = _context.ConstruccionObservacion
                                                                        .Where(r => r.ContratoConstruccionId == pContratoConstruccion.ContratoConstruccionId &&
                                                                                   r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.ManejoAnticipo &&
                                                                                   r.Eliminado != true &&
                                                                                   r.EsSupervision == true)
                                                                        .OrderByDescending(r => r.FechaCreacion)
                                                                        .FirstOrDefault();
                        if (observacionDelete != null)
                            observacionDelete.Eliminado = true;
                    }

                }
                else
                {
                    contratoConstruccion.TieneObservacionesManejoAnticipoApoyo = pContratoConstruccion.TieneObservacionesManejoAnticipoApoyo;

                    if (contratoConstruccion.TieneObservacionesManejoAnticipoApoyo.Value)
                    {
                        await CreateEditObservacionConstruccion(pContratoConstruccion.ConstruccionObservacion.FirstOrDefault(), pContratoConstruccion.UsuarioCreacion);
                    }
                    else
                    {
                        ConstruccionObservacion observacionDelete = _context.ConstruccionObservacion
                                                                        .Where(r => r.ContratoConstruccionId == pContratoConstruccion.ContratoConstruccionId &&
                                                                                   r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.ManejoAnticipo &&
                                                                                   r.Eliminado != true &&
                                                                                   r.EsSupervision != true)
                                                                        .OrderByDescending(r => r.FechaCreacion)
                                                                        .FirstOrDefault();
                        if (observacionDelete != null)
                            observacionDelete.Eliminado = true;
                    }
                }

                var contrato = _context.Contrato.Where(x => x.ContratoId == contratoConstruccion.ContratoId).Include(x => x.Contratacion).FirstOrDefault();

                //3.1.11
                if (!esSupervisor){
                    if (contrato.Contratacion.TipoSolicitudCodigo == "2")//cambiar esto, no encontre la constante inteventoria
                    {
                        contratoConstruccion.RegistroCompletoVerificacion = await ValidarRegistroCompletoVerificacionPreconstruccion(contratoConstruccion.ContratoConstruccionId, esSupervisor);
                    }
                    else
                    {
                        contratoConstruccion.RegistroCompletoVerificacion = await ValidarRegistroCompletoVerificacionContruccion(contratoConstruccion.ContratoConstruccionId, esSupervisor);
                    }
                    
                    if (contratoConstruccion.RegistroCompletoVerificacion.Value)
                    {
                        contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.Con_requisitos_tecnicos_verificados;
                        contrato.FechaAprobacionRequisitosConstruccionApoyo = DateTime.Now;
                    }
                    else
                    {
                        contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.En_proceso_de_verificacion_de_requisitos_tecnicos;
                        contrato.FechaAprobacionRequisitosConstruccionApoyo = null;
                    }
                }

                //3.1.12
                if (esSupervisor)
                {
                    contratoConstruccion.RegistroCompletoValidacion = await ValidarRegistroCompletoValidacionObraInterventoria(contratoConstruccion.ContratoConstruccionId);

                    if ((bool)contratoConstruccion.RegistroCompletoValidacion)
                        contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.En_proceso_de_validacion_de_requisitos_tecnicos;
                    else
                        contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.Con_requisitos_tecnicos_validados;

                    VerificarRegistroCompletoValidacionContratoObra(contratoConstruccion.ContratoId);
                }

                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Verificar_Requisitos_Tecnicos_Construccion, GeneralCodes.OperacionExitosa, idAccion, pContratoConstruccion.UsuarioCreacion, CreateEdit)
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Verificar_Requisitos_Tecnicos_Construccion, GeneralCodes.Error, idAccion, pContratoConstruccion.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        public async Task<Respuesta> CreateEditObservacionProgramacionObra(ContratoConstruccion pContratoConstruccion, bool esSupervisor)
        {
            Respuesta respuesta = new Respuesta();

            string CreateEdit = string.Empty;
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Observacion_Construccion_ProgramacionObra, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                CreateEdit = "EDIT OBSERVACION PROGRAMACIONOBRA";

                ContratoConstruccion contratoConstruccion = _context.ContratoConstruccion.Find(pContratoConstruccion.ContratoConstruccionId);

                contratoConstruccion.UsuarioModificacion = pContratoConstruccion.UsuarioCreacion;
                contratoConstruccion.FechaModificacion = DateTime.Now;

                if (esSupervisor)
                {

                    contratoConstruccion.TieneObservacionesProgramacionObraSupervisor = pContratoConstruccion.TieneObservacionesProgramacionObraSupervisor;

                    if (contratoConstruccion.TieneObservacionesProgramacionObraSupervisor.Value)
                    {

                        await CreateEditObservacionConstruccion(pContratoConstruccion.ConstruccionObservacion.FirstOrDefault(), pContratoConstruccion.UsuarioCreacion);
                    }
                    else
                    {
                        ConstruccionObservacion observacionDelete = _context.ConstruccionObservacion
                                                                        .Where(r => r.ContratoConstruccionId == pContratoConstruccion.ContratoConstruccionId &&
                                                                                   r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.ProgramacionObra &&
                                                                                   r.Eliminado != true &&
                                                                                   r.EsSupervision == true)
                                                                        .OrderByDescending(r => r.FechaCreacion)
                                                                        .FirstOrDefault();
                        if (observacionDelete != null)
                            observacionDelete.Eliminado = true;
                    }

                }
                else
                {
                    contratoConstruccion.TieneObservacionesProgramacionObraApoyo = pContratoConstruccion.TieneObservacionesProgramacionObraApoyo;

                    if (contratoConstruccion.TieneObservacionesProgramacionObraApoyo.Value)
                    {
                        await CreateEditObservacionConstruccion(pContratoConstruccion.ConstruccionObservacion.FirstOrDefault(), pContratoConstruccion.UsuarioCreacion);
                    }
                    else
                    {
                        ConstruccionObservacion observacionDelete = _context.ConstruccionObservacion
                                                                        .Where(r => r.ContratoConstruccionId == pContratoConstruccion.ContratoConstruccionId &&
                                                                                   r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.ProgramacionObra &&
                                                                                   r.Eliminado != true &&
                                                                                   r.EsSupervision != true)
                                                                        .OrderByDescending(r => r.FechaCreacion)
                                                                        .FirstOrDefault();
                        if (observacionDelete != null)
                            observacionDelete.Eliminado = true;
                    }
                }

                var contrato = _context.Contrato.Where(x => x.ContratoId == contratoConstruccion.ContratoId).Include(x => x.Contratacion).FirstOrDefault();

                //3.1.11
                if (!esSupervisor){
                    if (contrato.Contratacion.TipoSolicitudCodigo == "2")//cambiar esto, no encontre la constante inteventoria
                    {
                        contratoConstruccion.RegistroCompletoVerificacion = await ValidarRegistroCompletoVerificacionPreconstruccion(contratoConstruccion.ContratoConstruccionId, esSupervisor);
                    }
                    else
                    {
                        contratoConstruccion.RegistroCompletoVerificacion = await ValidarRegistroCompletoVerificacionContruccion(contratoConstruccion.ContratoConstruccionId, esSupervisor);
                    }
            
                    if (contratoConstruccion.RegistroCompletoVerificacion.Value)
                    {
                        contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.Con_requisitos_tecnicos_verificados;
                        contrato.FechaAprobacionRequisitosConstruccionApoyo = DateTime.Now;
                    }
                    else
                    {
                        contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.En_proceso_de_verificacion_de_requisitos_tecnicos;
                        contrato.FechaAprobacionRequisitosConstruccionApoyo = DateTime.Now;
                    }
                }

                //3.1.12
                if (esSupervisor)
                {
                    contratoConstruccion.RegistroCompletoValidacion = await ValidarRegistroCompletoValidacionObraInterventoria(contratoConstruccion.ContratoConstruccionId);

                    if ((bool)contratoConstruccion.RegistroCompletoValidacion)
                        contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.En_proceso_de_validacion_de_requisitos_tecnicos;
                    else
                        contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.Con_requisitos_tecnicos_validados;

                    VerificarRegistroCompletoValidacionContratoObra(contratoConstruccion.ContratoId);
                }

                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Verificar_Requisitos_Tecnicos_Construccion, GeneralCodes.OperacionExitosa, idAccion, pContratoConstruccion.UsuarioCreacion, CreateEdit)
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Verificar_Requisitos_Tecnicos_Construccion, GeneralCodes.Error, idAccion, pContratoConstruccion.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        public async Task<Respuesta> CreateEditObservacionFlujoInversion(ContratoConstruccion pContratoConstruccion, bool esSupervisor)
        {
            Respuesta respuesta = new Respuesta();

            string CreateEdit = string.Empty;
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Observacion_Construccion_FlujoInversion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                CreateEdit = "EDIT OBSERVACION FLUJOINVERSION";

                ContratoConstruccion contratoConstruccion = _context.ContratoConstruccion.Find(pContratoConstruccion.ContratoConstruccionId);
                contratoConstruccion.UsuarioModificacion = pContratoConstruccion.UsuarioCreacion;
                contratoConstruccion.FechaModificacion = DateTime.Now;

                if (esSupervisor)
                {

                    contratoConstruccion.TieneObservacionesFlujoInversionSupervisor = pContratoConstruccion.TieneObservacionesFlujoInversionSupervisor;

                    if (contratoConstruccion.TieneObservacionesFlujoInversionSupervisor.Value)
                    {

                        await CreateEditObservacionConstruccion(pContratoConstruccion.ConstruccionObservacion.FirstOrDefault(), pContratoConstruccion.UsuarioCreacion);
                    }
                    else
                    {
                        ConstruccionObservacion observacionDelete = _context.ConstruccionObservacion
                                                                        .Where(r => r.ContratoConstruccionId == pContratoConstruccion.ContratoConstruccionId &&
                                                                                   r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.FlujoInversion &&
                                                                                   r.Eliminado != true &&
                                                                                   r.EsSupervision == true)
                                                                        .OrderByDescending(r => r.FechaCreacion)
                                                                        .FirstOrDefault();
                        if (observacionDelete != null)
                            observacionDelete.Eliminado = true;
                    }

                }
                else
                {
                    contratoConstruccion.TieneObservacionesFlujoInversionApoyo = pContratoConstruccion.TieneObservacionesFlujoInversionApoyo;

                    if (contratoConstruccion.TieneObservacionesFlujoInversionApoyo.Value)
                    {
                        await CreateEditObservacionConstruccion(pContratoConstruccion.ConstruccionObservacion.FirstOrDefault(), pContratoConstruccion.UsuarioCreacion);
                    }
                    else
                    {
                        ConstruccionObservacion observacionDelete = _context.ConstruccionObservacion
                                                                        .Where(r => r.ContratoConstruccionId == pContratoConstruccion.ContratoConstruccionId &&
                                                                                   r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.FlujoInversion &&
                                                                                   r.Eliminado != true &&
                                                                                   r.EsSupervision != true)
                                                                        .OrderByDescending(r => r.FechaCreacion)
                                                                        .FirstOrDefault();
                        if (observacionDelete != null)
                            observacionDelete.Eliminado = true;
                    }
                }
                
                var contrato = _context.Contrato.Where(x => x.ContratoId == contratoConstruccion.ContratoId).Include(x => x.Contratacion).FirstOrDefault();

                // 3.1.11
                if (!esSupervisor){

                    if (contrato.Contratacion.TipoSolicitudCodigo == "2")//cambiar esto, no encontre la constante inteventoria
                    {
                        contratoConstruccion.RegistroCompletoVerificacion = await ValidarRegistroCompletoVerificacionPreconstruccion(contratoConstruccion.ContratoConstruccionId, esSupervisor);
                    }
                    else
                    {
                        contratoConstruccion.RegistroCompletoVerificacion = await ValidarRegistroCompletoVerificacionContruccion(contratoConstruccion.ContratoConstruccionId, esSupervisor);
                    }
                
                    if (contratoConstruccion.RegistroCompletoVerificacion.Value)
                    {
                        contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.Con_requisitos_tecnicos_verificados;
                        contrato.FechaAprobacionRequisitosConstruccionApoyo = DateTime.Now;
                    }
                    else
                    {
                        contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.En_proceso_de_verificacion_de_requisitos_tecnicos;
                        contrato.FechaAprobacionRequisitosConstruccionApoyo = DateTime.Now;
                    }
                }

                //3.1.12
                if (esSupervisor)
                {
                    contratoConstruccion.RegistroCompletoValidacion = await ValidarRegistroCompletoValidacionObraInterventoria(contratoConstruccion.ContratoConstruccionId);

                    if ((bool)contratoConstruccion.RegistroCompletoValidacion)
                        contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.En_proceso_de_validacion_de_requisitos_tecnicos;
                    else
                        contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.Con_requisitos_tecnicos_validados;

                    VerificarRegistroCompletoValidacionContratoObra(contratoConstruccion.ContratoId);
                }

                _context.ContratoConstruccion.Update(contratoConstruccion);
                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Verificar_Requisitos_Tecnicos_Construccion, GeneralCodes.OperacionExitosa, idAccion, pContratoConstruccion.UsuarioCreacion, CreateEdit)
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Verificar_Requisitos_Tecnicos_Construccion, GeneralCodes.Error, idAccion, pContratoConstruccion.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        public async Task<Respuesta> CreateEditObservacionPerfil(ConstruccionPerfil pPerfil, bool esSupervisor)
        {
            Respuesta respuesta = new Respuesta();

            string CreateEdit = string.Empty;
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Observacion_Perfil, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                CreateEdit = "EDIT OBSERVACION PERFIL";
                int idObservacion = 0;

                if (pPerfil.ConstruccionPerfilObservacion.Count() > 0)
                    idObservacion = pPerfil.ConstruccionPerfilObservacion.FirstOrDefault().ConstruccionPerfilObservacionId;

                ConstruccionPerfil construccionPerfil = _context.ConstruccionPerfil.Find(pPerfil.ConstruccionPerfilId);

                construccionPerfil.UsuarioModificacion = pPerfil.UsuarioCreacion;
                construccionPerfil.FechaModificacion = DateTime.Now;

                if (esSupervisor)
                {

                    construccionPerfil.TieneObservacionesSupervisor = pPerfil.TieneObservacionesSupervisor;

                    if (construccionPerfil.TieneObservacionesSupervisor.Value)
                    {

                        await CreateEditObservacionConstruccionPerfil(pPerfil.ConstruccionPerfilObservacion.FirstOrDefault(), pPerfil.UsuarioCreacion);
                    }
                    else
                    {
                        ConstruccionPerfilObservacion observacionDelete = _context.ConstruccionPerfilObservacion.Find(idObservacion);
                        if (observacionDelete != null)
                            observacionDelete.Eliminado = true;
                    }

                }
                else
                {
                    construccionPerfil.TieneObservacionesApoyo = pPerfil.TieneObservacionesApoyo;

                    if (construccionPerfil.TieneObservacionesApoyo.Value)
                    {
                        await CreateEditObservacionConstruccionPerfil(pPerfil.ConstruccionPerfilObservacion.FirstOrDefault(), pPerfil.UsuarioCreacion);
                    }
                    else
                    {
                        ConstruccionPerfilObservacion observacionDelete = _context.ConstruccionPerfilObservacion.Find(idObservacion);
                        if (observacionDelete != null)
                            observacionDelete.Eliminado = true;
                    }
                }

                var contratoConstruccion = _context.ContratoConstruccion.Find( construccionPerfil.ContratoConstruccionId );
                var contrato = _context.Contrato.Where(x => x.ContratoId == contratoConstruccion.ContratoId).Include(x => x.Contratacion).FirstOrDefault();

                // 3.1.11
                if (!esSupervisor){

                    if (contrato.Contratacion.TipoSolicitudCodigo == "2")//cambiar esto, no encontre la constante inteventoria
                    {
                        contratoConstruccion.RegistroCompletoVerificacion = await ValidarRegistroCompletoVerificacionPreconstruccion(contratoConstruccion.ContratoConstruccionId, esSupervisor);
                    }
                    else
                    {
                        contratoConstruccion.RegistroCompletoVerificacion = await ValidarRegistroCompletoVerificacionContruccion(contratoConstruccion.ContratoConstruccionId, esSupervisor);
                    }
                
                    if (contratoConstruccion.RegistroCompletoVerificacion.Value)
                    {
                        contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.Con_requisitos_tecnicos_verificados;
                        contrato.FechaAprobacionRequisitosConstruccionApoyo = DateTime.Now;
                    }
                    else
                    {
                        contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.En_proceso_de_verificacion_de_requisitos_tecnicos;
                        contrato.FechaAprobacionRequisitosConstruccionApoyo = DateTime.Now;
                    }
                }

                //3.1.12
                if (esSupervisor)
                {
                    contratoConstruccion.RegistroCompletoValidacion = await ValidarRegistroCompletoValidacionObraInterventoria(construccionPerfil.ContratoConstruccionId);

                    if ((bool)contratoConstruccion.RegistroCompletoValidacion)
                        contratoConstruccion.Contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.Con_requisitos_tecnicos_validados;
                    else
                        contratoConstruccion.Contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.En_proceso_de_validacion_de_requisitos_tecnicos;

                    VerificarRegistroCompletoValidacionContratoObra(contratoConstruccion.ContratoId);
                }
                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Verificar_Requisitos_Tecnicos_Construccion, GeneralCodes.OperacionExitosa, idAccion, pPerfil.UsuarioCreacion, CreateEdit)
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Verificar_Requisitos_Tecnicos_Construccion, GeneralCodes.Error, idAccion, pPerfil.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        #endregion Observaciones


    }

}
