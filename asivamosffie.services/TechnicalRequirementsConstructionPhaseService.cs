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

                });



                foreach (var ContratacionProyecto in contrato.Contratacion.ContratacionProyecto)
                {
                    Localizacion Municipio = Listlocalizacion.Where(r => r.LocalizacionId == ContratacionProyecto.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                    ContratacionProyecto.Proyecto.Departamento = Listlocalizacion.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault().Descripcion;
                    ContratacionProyecto.Proyecto.Municipio = Municipio.Descripcion;
                    ContratacionProyecto.Proyecto.TipoIntervencionCodigo = ListParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion && r.Codigo == ContratacionProyecto.Proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
                    //verifico que fases tiene
                    var componentes=_context.ComponenteAportante.Where(x => x.ContratacionProyectoAportante.ContratacionProyectoId == ContratacionProyecto.ContratacionProyectoId && !(bool)x.Eliminado).ToList();
                    bool construccion = false;
                    bool preconstruccion = false;
                    if(componentes.Where(x=>x.FaseCodigo == ConstanCodigoFaseContrato.Construccion).Count()>0)
                    {
                        construccion = true;
                    }
                    if (componentes.Where(x => x.FaseCodigo == ConstanCodigoFaseContrato.Preconstruccion).Count() > 0)
                    {
                        preconstruccion = true;
                    }
                    ContratacionProyecto.faseConstruccionNotMapped = construccion;
                    ContratacionProyecto.fasePreConstruccionNotMapped = preconstruccion;
                }

                return contrato;
            }
            catch (Exception ex)
            {
                return new Contrato();
            }
        }

        public async Task<List<ArchivoCargue>> GetLoadProgrammingGrid(int pContratoConstruccionId)
        {
            List<ArchivoCargue> listaCargas = _context.ArchivoCargue
                                                            .Where(a => a.ReferenciaId == pContratoConstruccionId && 
                                                                        a.Eliminado != true && 
                                                                        a.OrigenId ==int.Parse( OrigenArchivoCargue.ProgramacionObra ) )
                                                            .ToList();


            listaCargas.ForEach(archivo =>
            {
                    archivo.estadoCargue = archivo.CantidadRegistros == archivo.CantidadRegistrosValidos ? "Validos" : "Fallido";

            });

            return listaCargas;

        }

        public async Task<List<ArchivoCargue>> GetLoadInvestmentFlowGrid(int pContratoConstruccionId)
        {
            List<ArchivoCargue> listaCargas = _context.ArchivoCargue
                                                            .Where( a => a.ReferenciaId == pContratoConstruccionId && 
                                                                    a.Eliminado != true && 
                                                                    a.OrigenId ==int.Parse( OrigenArchivoCargue.FlujoInversion ))
                                                            .ToList();


            listaCargas.ForEach(archivo =>
            {
                    archivo.estadoCargue = archivo.CantidadRegistros == archivo.CantidadRegistrosValidos ? "Validos" : "Fallido";

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



                        foreach (var observacion in perfil.ConstruccionPerfilObservacion)
                        {

                            observacion.UsuarioCreacion = pConstruccion.UsuarioCreacion;
                            observacion.FechaCreacion = DateTime.Now;
                            observacion.TipoObservacionCodigo = ConstanCodigoTipoObservacion.Interventoria;

                            //perfil.ConstruccionPerfilObservacion.Add(observacion);
                        }

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
                                                .Where( c => c.ContratoId == pConstruccion.ContratoId)
                                                .Include( r => r.Contratacion )
                                                .FirstOrDefault();

                if (contrato.Contratacion.TipoSolicitudCodigo == "1") {// contrato de obra
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
                ContratoConstruccion construccionTemp = _context.ContratoConstruccion.Find( cc.ContratoConstruccionId );
                bool completoConstruccion = true;

                if (
                        cc.RegistroCompletoDiagnostico != true ||
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
                    cc.ConstruccionPerfil.Where( cp => cp.Eliminado != true ).ToList().ForEach(cp =>
                    {
                        if (cp.RegistroCompleto != true){
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
                ContratoConstruccion construccionTemp = _context.ContratoConstruccion.Find( cc.ContratoConstruccionId );
                bool completoConstruccion = true;

                cc.ConstruccionPerfil.Where( cp => cp.Eliminado != true ).ToList().ForEach(cp =>
                {
                    if (cp.RegistroCompleto != true){
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
                string.IsNullOrEmpty(pConstruccion.RutaInforme) ||
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

        private async Task<bool> ValidarRegistroCompletoVerificacion(int id, bool pEsSupervicion)
        {
            bool esCompleto = true;

            ContratoConstruccion cc = await _context.ContratoConstruccion.Where(cc => cc.ContratoConstruccionId == id)
                                                                .Include(r => r.ConstruccionPerfil)
                                                                .FirstOrDefaultAsync();

            cc.ObservacionDiagnosticoApoyo = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.Diagnostico, pEsSupervicion);
            cc.ObservacionPlanesProgramasApoyo = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.PlanesProgramas, pEsSupervicion);
            cc.ObservacionManejoAnticipoApoyo = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.ManejoAnticipo, pEsSupervicion);
            cc.ObservacionProgramacionObraApoyo = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.ProgramacionObra, pEsSupervicion);
            cc.ObservacionFlujoInversionApoyo = getObservacion(cc, ConstanCodigoTipoObservacionConstruccion.FlujoInversion, pEsSupervicion);

            if (cc.TieneObservacionesDiagnosticoApoyo == null ||
                 (cc.TieneObservacionesDiagnosticoApoyo == true && string.IsNullOrEmpty(cc.ObservacionDiagnosticoApoyo != null ? cc.ObservacionDiagnosticoApoyo.Observaciones : null)) ||
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

            return esCompleto;
        }

        private bool VerificarRegistroCompletoPlanesProgramas(ContratoConstruccion pConstruccion)
        {
            bool completo = true;

            if (
                    pConstruccion.PlanLicenciaVigente == null ||
                    pConstruccion.PlanCambioConstructorLicencia == null ||
                    pConstruccion.PlanActaApropiacion == null ||
                    pConstruccion.PlanResiduosDemolicion == null ||
                    pConstruccion.PlanManejoTransito == null ||
                    pConstruccion.PlanManejoAmbiental == null ||
                    pConstruccion.PlanAseguramientoCalidad == null ||
                    pConstruccion.PlanProgramaSeguridad == null ||
                    pConstruccion.PlanProgramaSalud == null ||
                    pConstruccion.PlanInventarioArboreo == null ||
                    pConstruccion.PlanAprovechamientoForestal == null ||
                    pConstruccion.PlanManejoAguasLluvias == null ||
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

                //|| string.IsNullOrEmpty(contratoPerfilOld.ConObervacionesSupervision.ToString() 
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

        private Proyecto CalcularFechaInicioContrato( int pContratoConstruccionId )
        {

            Proyecto proyecto = new Proyecto();

            ContratoConstruccion contratoConstruccion = _context.ContratoConstruccion
                                                                        .Where( cc => cc.ContratoConstruccionId == pContratoConstruccionId )
                                                                        .Include( r => r.Contrato )
                                                                            .ThenInclude( r => r.Contratacion )
                                                                                .ThenInclude( r => r.DisponibilidadPresupuestal )
                                                                        .Include( r => r.Contrato )
                                                                            .ThenInclude( r => r.ContratoPoliza )
                                                                        .Include( r => r.Proyecto )
                                                                        .FirstOrDefault();


            DateTime? fechaInicioContrato = contratoConstruccion?.Contrato?.Contratacion?.DisponibilidadPresupuestal?.FirstOrDefault()?.FechaDrp;
            DateTime? fechaPoliza = contratoConstruccion.Contrato?.ContratoPoliza?.OrderByDescending( r => r.FechaAprobacion )?.FirstOrDefault()?.FechaAprobacion;

            if ( fechaInicioContrato != null && fechaPoliza != null ){
                if ( fechaPoliza >= fechaInicioContrato)
                    proyecto.FechaInicioEtapaObra = fechaPoliza.Value;
                else
                    proyecto.FechaInicioEtapaObra = fechaInicioContrato.Value;
            }

            DateTime fechaFinalContrato = proyecto.FechaInicioEtapaObra.AddMonths( contratoConstruccion.Proyecto.PlazoMesesObra.Value );
            fechaFinalContrato = fechaFinalContrato.AddDays( contratoConstruccion.Proyecto.PlazoDiasObra.Value ); 

            return proyecto;
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
                    .FirstOrDefaultAsync();

                contratoCambiarEstado.UsuarioModificacion = pUsuarioMod;
                contratoCambiarEstado.FechaModificacion = DateTime.Now;

                contratoCambiarEstado.EstadoVerificacionConstruccionCodigo = pEstado;

                if (pEstado == ConstanCodigoEstadoConstruccion.Enviado_al_interventor.ToString() || pEstado == ConstanCodigoEstadoConstruccion.Enviado_al_apoyo.ToString())
                {

                    foreach (var ContratoConstruccion in contratoCambiarEstado.ContratoConstruccion)
                    {
                        //Observaciones Diagnostico

                        if ((bool)ContratoConstruccion.TieneObservacionesDiagnosticoSupervisor)
                        {
                            //Observacion Supervisor
                            ConstruccionObservacion construccionObservacionSupervisor = ContratoConstruccion.ConstruccionObservacion
                                .Where(r => r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.Diagnostico.ToString()
                                        && (bool)r.EsSupervision
                                        && !(bool)r.Eliminado
                                        && !(bool)r.Archivada
                                    )
                                .FirstOrDefault();

                            construccionObservacionSupervisor.Archivada = true;

                            //Observacion Apoyo
                            ConstruccionObservacion construccionObservacionApoyo = ContratoConstruccion.ConstruccionObservacion
                               .Where(r => r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.Diagnostico.ToString()
                                   && !(bool)r.EsSupervision
                                   && !(bool)r.Eliminado
                                   && !(bool)r.Archivada)
                               .FirstOrDefault();

                            construccionObservacionApoyo.Archivada = true;

                            //Contrato Construccion
                            ContratoConstruccion.ObservacionDiagnosticoSupervisorId = construccionObservacionSupervisor.ConstruccionObservacionId;
                            ContratoConstruccion.TieneObservacionesDiagnosticoApoyo = null;
                            ContratoConstruccion.TieneObservacionesDiagnosticoSupervisor = null;
                            ContratoConstruccion.RegistroCompletoDiagnostico = false;
                        }

                        //Observaciones Planes Y Programas

                        if ((bool)ContratoConstruccion.TieneObservacionesPlanesProgramasSupervisor)
                        {
                            //Observacion Supervisor
                            ConstruccionObservacion construccionObservacionSupervisor = ContratoConstruccion.ConstruccionObservacion
                                .Where(r => r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.PlanesProgramas.ToString()
                                    && (bool)r.EsSupervision
                                    && !(bool)r.Eliminado
                                    && !(bool)r.Archivada)
                                .FirstOrDefault();

                            construccionObservacionSupervisor.Archivada = true;

                            //Observacion Apoyo
                            ConstruccionObservacion construccionObservacionApoyo = ContratoConstruccion.ConstruccionObservacion
                               .Where(r => r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.PlanesProgramas.ToString()
                                   && !(bool)r.EsSupervision
                                   && !(bool)r.Eliminado
                                   && !(bool)r.Archivada)
                               .FirstOrDefault();

                            construccionObservacionApoyo.Archivada = true;

                            //Contrato Construccion
                            ContratoConstruccion.ObservacionPlanesProgramasSupervisorId = construccionObservacionSupervisor.ConstruccionObservacionId;
                            ContratoConstruccion.TieneObservacionesPlanesProgramasApoyo = null;
                            ContratoConstruccion.TieneObservacionesPlanesProgramasSupervisor = null;
                            ContratoConstruccion.RegistroCompletoPlanesProgramas = false;
                        }

                        //Observaciones Manejo de Anticipo

                        if ((bool)ContratoConstruccion.TieneObservacionesManejoAnticipoSupervisor)
                        {
                            //Observacion Supervisor
                            ConstruccionObservacion construccionObservacionSupervisor = ContratoConstruccion.ConstruccionObservacion
                                .Where(r => r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.ManejoAnticipo.ToString()
                                    && (bool)r.EsSupervision
                                    && !(bool)r.Eliminado
                                    && !(bool)r.Archivada)
                                .FirstOrDefault();

                            construccionObservacionSupervisor.Archivada = true;

                            //Observacion Apoyo
                            ConstruccionObservacion construccionObservacionApoyo = ContratoConstruccion.ConstruccionObservacion
                               .Where(r => r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.ManejoAnticipo.ToString()
                                   && !(bool)r.EsSupervision
                                   && !(bool)r.Eliminado
                                   && !(bool)r.Archivada)
                               .FirstOrDefault();

                            construccionObservacionApoyo.Archivada = true;

                            //Contrato Construccion
                            ContratoConstruccion.ObservacionManejoAnticipoSupervisorId = construccionObservacionSupervisor.ConstruccionObservacionId;
                            ContratoConstruccion.TieneObservacionesManejoAnticipoApoyo = null;
                            ContratoConstruccion.TieneObservacionesManejoAnticipoSupervisor = null;
                            ContratoConstruccion.RegistroCompletoManejoAnticipo = false;
                        }

                        //Observaciones Programacion Obra  

                        if ((bool)ContratoConstruccion.TieneObservacionesProgramacionObraSupervisor)
                        {
                            //Observacion Supervisor
                            ConstruccionObservacion construccionObservacionSupervisor = ContratoConstruccion.ConstruccionObservacion
                                .Where(r => r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.ProgramacionObra.ToString()
                                    && (bool)r.EsSupervision
                                    && !(bool)r.Eliminado
                                    && !(bool)r.Archivada)
                                .FirstOrDefault();

                            construccionObservacionSupervisor.Archivada = true;

                            //Observacion Apoyo
                            ConstruccionObservacion construccionObservacionApoyo = ContratoConstruccion.ConstruccionObservacion
                               .Where(r => r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.ProgramacionObra.ToString()
                                   && !(bool)r.EsSupervision
                                   && !(bool)r.Eliminado
                                   && !(bool)r.Archivada)
                               .FirstOrDefault();

                            construccionObservacionApoyo.Archivada = true;

                            //Contrato Construccion
                            ContratoConstruccion.ObservacionProgramacionObraSupervisorId = construccionObservacionSupervisor.ConstruccionObservacionId;
                            ContratoConstruccion.TieneObservacionesProgramacionObraApoyo = null;
                            ContratoConstruccion.TieneObservacionesProgramacionObraSupervisor = null;
                            ContratoConstruccion.RegistroCompletoProgramacionObra = false;
                        }

                        //Observaciones Flujo Inversion 

                        if ((bool)ContratoConstruccion.TieneObservacionesFlujoInversionSupervisor)
                        {
                            //Observacion Supervisor
                            ConstruccionObservacion construccionObservacionSupervisor = ContratoConstruccion.ConstruccionObservacion
                                .Where(r => r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.FlujoInversion.ToString()
                                    && (bool)r.EsSupervision
                                    && !(bool)r.Eliminado
                                    && !(bool)r.Archivada)
                                .FirstOrDefault();

                            construccionObservacionSupervisor.Archivada = true;

                            //Observacion Apoyo
                            ConstruccionObservacion construccionObservacionApoyo = ContratoConstruccion.ConstruccionObservacion
                               .Where(r => r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.FlujoInversion.ToString()
                                   && !(bool)r.EsSupervision
                                   && !(bool)r.Eliminado
                                   && !(bool)r.Archivada)
                               .FirstOrDefault();

                            construccionObservacionApoyo.Archivada = true;

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

        public async Task<Respuesta> EnviarAlSupervisor(int pContratoId, string pUsuarioCreacion)
        {
            string CreateEdit = string.Empty;
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Enviar_Al_Supervisor, (int)EnumeratorTipoDominio.Acciones);

            try
            {

                Contrato contrato = _context.Contrato.Find(pContratoId);

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

            if ( pConstruccionPerfilId == 0 ){
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
                    contratoConstruccion.ArchivoCargueIdFlujoInversion = null;
                    contratoConstruccion.RegistroCompletoFlujoInversion = false;
                }
                else
                {
                    contratoConstruccion.ArchivoCargueIdProgramacionObra = null;
                    contratoConstruccion.RegistroCompletoProgramacionObra = false;
                }

                _context.SaveChanges();

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

        public async Task<Respuesta> UploadFileToValidateProgramming(IFormFile pFile, string pFilePatch, string pUsuarioCreo, int pContratoConstruccionId)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Validar_Excel_Programacion_Obra, (int)EnumeratorTipoDominio.Acciones);

            ContratoConstruccion contratoConstruccion = _context.ContratoConstruccion
                                                                        .Where( cc => cc.ContratoConstruccionId == pContratoConstruccionId )
                                                                        .Include( r => r.Contrato )
                                                                            .ThenInclude( r => r.Contratacion )
                                                                                .ThenInclude( r => r.DisponibilidadPresupuestal )
                                                                        .Include( r => r.Contrato )
                                                                            .ThenInclude( r => r.ContratoPoliza )
                                                                        .Include( r => r.Proyecto )
                                                                        .FirstOrDefault();

            DateTime? fechaInicioContrato = contratoConstruccion?.Contrato?.Contratacion?.DisponibilidadPresupuestal?.FirstOrDefault()?.FechaDrp.Value;
            DateTime fechaFinalContrato = fechaInicioContrato.Value.AddMonths( contratoConstruccion.Proyecto.PlazoMesesObra.Value );
            fechaFinalContrato = fechaFinalContrato.AddDays( contratoConstruccion.Proyecto.PlazoDiasObra.Value );

            int CantidadRegistrosVacios = 0;
            int CantidadResgistrosValidos = 0;
            int CantidadRegistrosInvalidos = 0;

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

                            // #1
                            //Tipo Actividad
                            if (string.IsNullOrEmpty(worksheet.Cells[i, 1].Text))
                            {
                                worksheet.Cells[i, 1].AddComment("Dato Obligatorio", "Admin");
                                worksheet.Cells[i, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
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
                            if ( temp.FechaInicio < fechaInicioContrato ){
                                worksheet.Cells[i, 4].AddComment("La fecha Inicial de la actividad no puede ser inferior a la fecha inicial del contrato", "Admin");
                                worksheet.Cells[i, 4].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;
                            }

                            if ( temp.FechaFin > fechaFinalContrato ){
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
                    foreach (TempProgramacion tempProgramacion in listTempProgramacion)
                    {

                        contratoConstruccionId = tempProgramacion.ContratoConstruccionId;

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

                    Proyecto proyecto = CalcularFechaInicioContrato( contratoConstruccionId );

                    int numeroMes = 1;
                    for ( DateTime fecha = proyecto.FechaInicioEtapaObra; fecha <= proyecto.FechaFinEtapaObra; fecha = fecha.AddMonths(1)){
                        
                        MesEjecucion mes = new MesEjecucion() {
                            ContratoConstruccionId = contratoConstruccionId,
                            Numero = numeroMes,
                            FechaInicio = fecha,
                            FechaFin = fecha.AddMonths(1),

                        };

                        _context.MesEjecucion.Add( mes );
                        numeroMes++;
                    } 

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

        public async Task<Respuesta> UploadFileToValidateInvestmentFlow(IFormFile pFile, string pFilePatch, string pUsuarioCreo, int pContratoConstruccionId)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Validar_Excel_Flujo_Inversion, (int)EnumeratorTipoDominio.Acciones);

            int CantidadRegistrosVacios = 0;
            int CantidadResgistrosValidos = 0;
            int CantidadRegistrosInvalidos = 0;

            ContratoConstruccion contratoConstruccion = _context.ContratoConstruccion
                                                                        .Where( cc => cc.ContratoConstruccionId == pContratoConstruccionId )
                                                                        .Include( r => r.Contrato )
                                                                            .ThenInclude( r => r.Contratacion )
                                                                                .ThenInclude( r => r.DisponibilidadPresupuestal )
                                                                        .Include( r => r.Contrato )
                                                                            .ThenInclude( r => r.ContratoPoliza )
                                                                        .Include( r => r.Proyecto )
                                                                        .FirstOrDefault();


            DateTime? fechaInicioContrato = contratoConstruccion?.Contrato?.Contratacion?.DisponibilidadPresupuestal?.FirstOrDefault()?.FechaDrp.Value;
            DateTime fechaFinalContrato = fechaInicioContrato.Value.AddMonths( contratoConstruccion.Proyecto.PlazoMesesObra.Value );
            fechaFinalContrato = fechaFinalContrato.AddDays( contratoConstruccion.Proyecto.PlazoDiasObra.Value );

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
                    //Controlar Registros
                    //Filas <=
                    //No comienza desde 0 por lo tanto el = no es necesario

                    decimal sumaTotal = 0;

                    for (int i = 2; i <= worksheet.Dimension.Rows; i++)
                    {
                        try
                        {
                            for (int k = 2; k < worksheet.Dimension.Columns; k++)
                            {
                                bool tieneErrores = false;

                                TempFlujoInversion temp = new TempFlujoInversion();
                                //Auditoria
                                temp.ArchivoCargueId = archivoCarge.ArchivoCargueId;
                                temp.EstaValidado = false;
                                temp.FechaCreacion = DateTime.Now;
                                temp.UsuarioCreacion = pUsuarioCreo;
                                temp.ContratoConstruccionId = pContratoConstruccionId;

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
                                    temp.Mes = worksheet.Cells[1, k].Text;
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
                                    temp.Capitulo = worksheet.Cells[i, 1].Text;
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

                    if ( contratoConstruccion.Proyecto.ValorObra != sumaTotal ){
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
                    archivoCarge.CantidadRegistros = ((worksheet.Dimension.Rows - 1) * (worksheet.Dimension.Columns - 2) - CantidadRegistrosVacios);
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
                    listTempFlujoInversion.ForEach(tempFlujo =>
                   {

                       contratoConstruccionId = tempFlujo.ContratoConstruccionId;
                       FlujoInversion flujo = new FlujoInversion()
                       {
                           ContratoConstruccionId = tempFlujo.ContratoConstruccionId,
                           Capitulo = tempFlujo.Capitulo,
                           Mes = tempFlujo.Mes,
                           Valor = tempFlujo.Valor,

                       };

                       _context.FlujoInversion.Add(flujo);
                       _context.SaveChanges();



                       //Temporal proyecto update
                       tempFlujo.EstaValidado = true;
                       tempFlujo.FechaModificacion = DateTime.Now;
                       tempFlujo.UsuarioModificacion = pUsuarioModifico;
                       _context.TempFlujoInversion.Update(tempFlujo);
                       _context.SaveChanges();
                   });

                    ContratoConstruccion contratoConstruccion = _context.ContratoConstruccion
                                                                            .Where(r => r.ContratoConstruccionId == contratoConstruccionId)
                                                                            .Include(r => r.Contrato)
                                                                                .ThenInclude(r => r.Contratacion)
                                                                                    .ThenInclude(r => r.ContratacionProyecto)
                                                                            .Include(r => r.Proyecto)

                                                                            .FirstOrDefault();

                    if (contratoConstruccion != null)
                    {
                        List<dynamic> listaFechas = new List<dynamic>();

                        contratoConstruccion.ArchivoCargueIdFlujoInversion = archivoCargue.ArchivoCargueId;
                        contratoConstruccion.RegistroCompletoFlujoInversion = true;

                        VerificarRegistroCompletoContratoObra(contratoConstruccion.ContratoId);

                        DateTime? fechaInicio = contratoConstruccion.Contrato.FechaActaInicioFase2;
                        if (fechaInicio != null)
                        {

                            DateTime fechaFin = fechaInicio.Value.AddMonths(contratoConstruccion.Proyecto.PlazoMesesObra.Value);
                            fechaFin = fechaFin.AddDays(contratoConstruccion.Proyecto.PlazoDiasObra.Value);

                            DateTime fechaTemp = fechaInicio.Value;

                            while (fechaFin >= fechaTemp)
                            {
                                listaFechas.Add(new { fechaInicio = fechaTemp, fechaFin = fechaTemp.AddDays(6) });
                                fechaTemp = fechaTemp.AddDays(7);
                            }


                            ContratacionProyecto contratacionProyecto = contratoConstruccion.Contrato.Contratacion.ContratacionProyecto.Where(p => p.ProyectoId == contratoConstruccion.ProyectoId).FirstOrDefault();
                            if (contratacionProyecto != null)
                            {
                                int idContratacionproyecto = contratacionProyecto.ContratacionProyectoId;

                                List<SeguimientoSemanal> listaSeguimientos = _context.SeguimientoSemanal
                                                                            .Where(p => p.ContratacionProyectoId == idContratacionproyecto).ToList();

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

                            }
                        }
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
                        // .Where(r => r.ContratoConstruccionId == pContratoConstruccion.ContratoConstruccionId &&
                        //            r.EsSupervision == true &&
                        //            r.Eliminado != false &&
                        //            r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.Diagnostico)
                        // .OrderByDescending(r => r.FechaCreacion)
                        //.FirstOrDefault();
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
                        // &&
                        //            r.EsSupervision != true &&
                        //            r.Eliminado != false &&
                        //            r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.Diagnostico)
                        // .OrderByDescending(r => r.FechaCreacion)
                        // .FirstOrDefault();
                        if (observacionDelete != null)
                            observacionDelete.Eliminado = true;
                    }
                }

                contratoConstruccion.RegistroCompletoVerificacion = await ValidarRegistroCompletoVerificacion(contratoConstruccion.ContratoConstruccionId, esSupervisor);

                Contrato contrato = _context.Contrato.Find(contratoConstruccion.ContratoId);
                
                if (contratoConstruccion.RegistroCompletoVerificacion.Value)
                {
                    contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.Con_requisitos_tecnicos_verificados;
                }else{
                    contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.En_proceso_de_verificacion_de_requisitos_tecnicos;
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
                        // &&
                        //            r.EsSupervision != true &&
                        //            r.Eliminado != false &&
                        //            r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.Diagnostico)
                        // .OrderByDescending(r => r.FechaCreacion)
                        // .FirstOrDefault();
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
                        // &&
                        //            r.EsSupervision != true &&
                        //            r.Eliminado != false &&
                        //            r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.Diagnostico)
                        // .OrderByDescending(r => r.FechaCreacion)
                        // .FirstOrDefault();
                        if (observacionDelete != null)
                            observacionDelete.Eliminado = true;
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
                        // &&
                        //            r.EsSupervision != true &&
                        //            r.Eliminado != false &&
                        //            r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.Diagnostico)
                        // .OrderByDescending(r => r.FechaCreacion)
                        // .FirstOrDefault();
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
                        // &&
                        //            r.EsSupervision != true &&
                        //            r.Eliminado != false &&
                        //            r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.Diagnostico)
                        // .OrderByDescending(r => r.FechaCreacion)
                        // .FirstOrDefault();
                        if (observacionDelete != null)
                            observacionDelete.Eliminado = true;
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
