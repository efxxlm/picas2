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

        public async Task<List<dynamic>> GetContractsGrid(int pUsuarioId)
        {
            List<dynamic> listaContrats = new List<dynamic>();

            List<VRequisitosTecnicosInicioConstruccion> lista = _context.VRequisitosTecnicosInicioConstruccion.ToList();

            lista.Where(c => c.TipoContratoCodigo == "1").ToList() // tipo contrato obra
                .ForEach(c =>
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
                    EstadoNombre = c.EstadoNombre,
                    Existeregistro = c.ExisteRegistro,

                });
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

        public async Task<Contrato> GetContratoByContratoId(int pContratoId)
        {
            try
            {
                Contrato contrato = await _registerPreContructionPhase1Service.GetContratoByContratoId(pContratoId);

                contrato.ContratoConstruccion = _context.ContratoConstruccion.Where(cc => cc.ContratoId == pContratoId)
                                                                                .Include(r => r.ConstruccionPerfil)
                                                                                    .ThenInclude(r => r.ConstruccionPerfilObservacion)
                                                                                .Include(r => r.ConstruccionPerfil)
                                                                                    .ThenInclude(r => r.ConstruccionPerfilNumeroRadicado)
                                                                                .Include(r => r.ConstruccionObservacion)

                                                                             .ToList();


                return contrato;
            }
            catch (Exception ex)
            {
                return new Contrato();
            }
        }

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

                }
                else
                {
                    CreateEdit = "CREAR CONTRATO CONSTRUCCION";

                    ContratoConstruccion contratoConstruccion = new ContratoConstruccion();

                    contratoConstruccion.FechaCreacion = DateTime.Now;
                    contratoConstruccion.UsuarioCreacion = pConstruccion.UsuarioCreacion;

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

                    _context.ContratoConstruccion.Add(contratoConstruccion);
                }

                Contrato contrato = _context.Contrato.Find(pConstruccion.ContratoId);
                if (contrato.EstadoVerificacionConstruccionCodigo == null || contrato.EstadoVerificacionConstruccionCodigo == ConstanCodigoEstadoConstruccion.Sin_aprobacion_de_requisitos_tecnicos)
                    contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.En_proceso_de_aprobacion_de_requisitos_tecnicos;

                _context.SaveChanges();
                return
                    new Respuesta
                    {
                        //Data = this.GetContratoByContratoId( pConstruccion.ContratoId ),
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

                }
                else
                {
                    CreateEdit = "CREAR CONTRATO CONSTRUCCION";

                    ContratoConstruccion contratoConstruccion = new ContratoConstruccion();

                    contratoConstruccion.FechaCreacion = DateTime.Now;
                    contratoConstruccion.UsuarioCreacion = pConstruccion.UsuarioCreacion;

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

                    _context.ContratoConstruccion.Add(contratoConstruccion);
                }

                Contrato contrato = _context.Contrato.Find(pConstruccion.ContratoId);
                if (contrato.EstadoVerificacionConstruccionCodigo == null || contrato.EstadoVerificacionConstruccionCodigo == ConstanCodigoEstadoConstruccion.Sin_aprobacion_de_requisitos_tecnicos)
                    contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.En_proceso_de_aprobacion_de_requisitos_tecnicos;

                _context.SaveChanges();
                return
                    new Respuesta
                    {
                        //Data = this.GetContratoByContratoId( pConstruccion.ContratoId ),
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
                    contratoConstruccion.ManejoAnticipoConObservaciones = pConstruccion.ManejoAnticipoConObservaciones;

                }
                else
                {
                    CreateEdit = "CREAR CONTRATO CONSTRUCCION";

                    ContratoConstruccion contratoConstruccion = new ContratoConstruccion();

                    contratoConstruccion.FechaCreacion = DateTime.Now;
                    contratoConstruccion.UsuarioCreacion = pConstruccion.UsuarioCreacion;

                    contratoConstruccion.ContratoId = pConstruccion.ContratoId;
                    contratoConstruccion.ProyectoId = pConstruccion.ProyectoId;

                    contratoConstruccion.ManejoAnticipoRequiere = pConstruccion.ManejoAnticipoRequiere;
                    contratoConstruccion.ManejoAnticipoPlanInversion = pConstruccion.ManejoAnticipoPlanInversion;
                    contratoConstruccion.ManejoAnticipoCronogramaAmortizacion = pConstruccion.ManejoAnticipoCronogramaAmortizacion;
                    contratoConstruccion.ManejoAnticipoRutaSoporte = pConstruccion.ManejoAnticipoRutaSoporte;
                    contratoConstruccion.ManejoAnticipoConObservaciones = pConstruccion.ManejoAnticipoConObservaciones;

                    _context.ContratoConstruccion.Add(contratoConstruccion);
                }

                Contrato contrato = _context.Contrato.Find(pConstruccion.ContratoId);
                if (contrato.EstadoVerificacionConstruccionCodigo == null || contrato.EstadoVerificacionConstruccionCodigo == ConstanCodigoEstadoConstruccion.Sin_aprobacion_de_requisitos_tecnicos)
                    contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.En_proceso_de_aprobacion_de_requisitos_tecnicos;

                _context.SaveChanges();
                return
                    new Respuesta
                    {
                        //Data = this.GetContratoByContratoId( pConstruccion.ContratoId ),
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

                        construccionPerfil.RegistroCompleto = ValidarRegistroCompletoConstruccionPerfil(construccionPerfil);

                        foreach (var observacion in perfil.ConstruccionPerfilObservacion)
                        {
                            if (observacion.ConstruccionPerfilObservacionId > 0)
                            {
                                ConstruccionPerfilObservacion construccionPerfilObservacion = _context.ConstruccionPerfilObservacion.Find(observacion.ConstruccionPerfilObservacionId);

                                construccionPerfilObservacion.UsuarioModificacion = pConstruccion.UsuarioCreacion;
                                construccionPerfilObservacion.FechaModificacion = DateTime.Now;

                                construccionPerfilObservacion.Observacion = observacion.Observacion;
                                //construccionPerfilObservacion.TipoObservacionCodigo = observacion.TipoObservacionCodigo;

                            }
                            else
                            {
                                observacion.UsuarioCreacion = pConstruccion.UsuarioCreacion;
                                observacion.FechaCreacion = DateTime.Now;

                                observacion.TipoObservacionCodigo = ConstanCodigoTipoObservacion.Interventoria;
                                observacion.Eliminado = false;

                                construccionPerfil.ConstruccionPerfilObservacion.Add(observacion);
                            }
                        }

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

                Contrato contrato = _context.Contrato.Find(pConstruccion.ContratoId);
                
                if ( contrato.TipoContratoCodigo == "1" ) // contrato de obra
                    if (contrato.EstadoVerificacionConstruccionCodigo == null || contrato.EstadoVerificacionConstruccionCodigo == ConstanCodigoEstadoConstruccion.Sin_aprobacion_de_requisitos_tecnicos)
                        contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.En_proceso_de_aprobacion_de_requisitos_tecnicos;
                
                if ( contrato.TipoContratoCodigo == "2" ) // contrato de interventoria
                    if (contrato.EstadoVerificacionConstruccionCodigo == null || contrato.EstadoVerificacionConstruccionCodigo == ConstanCodigoEstadoConstruccion.Con_requisitos_tecnicos_aprobados)
                        contrato.EstadoVerificacionConstruccionCodigo = ConstanCodigoEstadoConstruccion.En_proceso_de_verificacion_de_requisitos_tecnicos;

                _context.SaveChanges();
                return
                    new Respuesta
                    {
                        //Data = await this.GetContratoByContratoId( pConstruccion.ContratoId ),
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

        public async Task<Respuesta> DeleteConstruccionPerfil(int pConstruccionPerfilId, string pUsuarioModificacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Construccion_Perfil, (int)EnumeratorTipoDominio.Acciones);

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

        public async Task<Respuesta> UploadFileToValidateProgramming(IFormFile pFile, string pFilePatch, string pUsuarioCreo, int pContratoConstruccionId)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Validar_Excel_Programacion_Obra, (int)EnumeratorTipoDominio.Acciones);

            int CantidadRegistrosVacios = 0;
            int CantidadResgistrosValidos = 0;
            int CantidadRegistrosInvalidos = 0;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //int OrigenId = await _commonService.GetDominioIdByCodigoAndTipoDominio(OrigenArchivoCargue.Proyecto, (int)EnumeratorTipoDominio.Origen_Documento_Cargue);
            DocumentService _documentService = new DocumentService(_context, _commonService);

            ArchivoCargue archivoCarge = await _documentService.getSaveFile(pFile, pFilePatch, Int32.Parse(OrigenArchivoCargue.ProgramacionObra));

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
                        try
                        {
                            /* Columnas Obligatorias de excel
                             2	3	5	6	7	8	10	11	12	13	14 28 29 30 31 32		
                            Campos Obligatorios Validos   */
                            if (
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 1].Text) &&
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 2].Text) &&
                                    //!string.IsNullOrEmpty(worksheet.Cells[i, 3].Text) &&
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 4].Text) &&
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 5].Text) &&
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 6].Text)


                                )
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
                                temp.TipoActividadCodigo = worksheet.Cells[i, 1].Text;

                                //#2
                                //Nombre proponente
                                if (!string.IsNullOrEmpty(worksheet.Cells[i, 2].Text))
                                {
                                    temp.Actividad = worksheet.Cells[i, 2].Text;
                                }

                                //#3
                                //Marca de ruta critica
                                if (string.IsNullOrEmpty(worksheet.Cells[i, 3].Text))
                                {
                                    temp.EsRutaCritica = false;
                                }
                                else
                                {
                                    temp.EsRutaCritica = worksheet.Cells[i, 3].Text == "1" ? true : false;
                                }


                                //#4
                                //Fecha Inicio
                                DateTime fechaTemp;
                                temp.FechaInicio = DateTime.TryParse(worksheet.Cells[i, 4].Text, out fechaTemp) ? fechaTemp : DateTime.MinValue;

                                //#5
                                //Fecha final
                                temp.FechaFin = DateTime.TryParse(worksheet.Cells[i, 5].Text, out fechaTemp) ? fechaTemp : DateTime.MinValue;

                                //#6
                                //Duracion
                                temp.Duracion = Int32.Parse(worksheet.Cells[i, 6].Text);


                                //Guarda Cambios en una tabla temporal

                                _context.TempProgramacion.Add(temp);
                                _context.SaveChanges();

                                if (temp.TempProgramacionId > 0)
                                {
                                    CantidadResgistrosValidos++;
                                }
                                else
                                {
                                    CantidadRegistrosInvalidos++;
                                }

                            }
                            else
                            {
                                //Aqui entra cuando alguno de los campos obligatorios no viene diligenciado
                                string strValidateCampNullsOrEmpty = "";
                                //Valida que todos los campos esten vacios porque las validaciones del excel hacen que lea todos los rows como ingresado información 

                                for (int j = 1; j < 7; j++)
                                {
                                    strValidateCampNullsOrEmpty += (worksheet.Cells[i, j].Text);
                                }
                                if (string.IsNullOrEmpty(strValidateCampNullsOrEmpty))
                                {
                                    CantidadRegistrosVacios++;
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

                        //ProcesoSeleccionProponente
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

        public async Task<List<ArchivoCargue>> GetLoadProgrammingGrid(int pContratoConstruccionId)
        {
            List<ArchivoCargue> listaCargas = new List<ArchivoCargue>();

            List<TempProgramacion> lista = _context.TempProgramacion.Where(tp => tp.ContratoConstruccionId == pContratoConstruccionId).ToList();

            lista.ForEach(c =>
            {
                ArchivoCargue archivo = _context.ArchivoCargue.Where(a => a.ArchivoCargueId == c.ArchivoCargueId && a.Eliminado != true).FirstOrDefault();
                if (archivo != null)
                {
                    archivo.estadoCargue = archivo.CantidadRegistros == archivo.CantidadRegistrosValidos ? "Validos" : "Fallido";

                    listaCargas.Add(archivo);
                }
            });

            return listaCargas;

        }

        public async Task<Respuesta> UploadFileToValidateInvestmentFlow(IFormFile pFile, string pFilePatch, string pUsuarioCreo, int pContratoConstruccionId)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Validar_Excel_Flujo_Inversion, (int)EnumeratorTipoDominio.Acciones);

            int CantidadRegistrosVacios = 0;
            int CantidadResgistrosValidos = 0;
            int CantidadRegistrosInvalidos = 0;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //int OrigenId = await _commonService.GetDominioIdByCodigoAndTipoDominio(OrigenArchivoCargue.Proyecto, (int)EnumeratorTipoDominio.Origen_Documento_Cargue);
            DocumentService _documentService = new DocumentService(_context, _commonService);

            ArchivoCargue archivoCarge = await _documentService.getSaveFile(pFile, pFilePatch, Int32.Parse(OrigenArchivoCargue.FlujoInversion));

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
                        try
                        {

                            /* Columnas Obligatorias de excel
                             2	3	5	6	7	8	10	11	12	13	14 28 29 30 31 32		
                            Campos Obligatorios Validos   */
                            if (
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 1].Text)
                               )
                            {
                                for (int k = 2; k < worksheet.Dimension.Columns; k++)
                                {


                                    TempFlujoInversion temp = new TempFlujoInversion();
                                    //Auditoria
                                    temp.ArchivoCargueId = archivoCarge.ArchivoCargueId;
                                    temp.EstaValidado = false;
                                    temp.FechaCreacion = DateTime.Now;
                                    temp.UsuarioCreacion = pUsuarioCreo;
                                    temp.ContratoConstruccionId = pContratoConstruccionId;


                                    //Valores
                                    temp.Mes = worksheet.Cells[1, k].Text;
                                    temp.Capitulo = worksheet.Cells[i, 1].Text;
                                    temp.Valor = string.IsNullOrEmpty(worksheet.Cells[i, k].Text) ? 0 : decimal.Parse(worksheet.Cells[i, k].Text);


                                    //Guarda Cambios en una tabla temporal

                                    _context.TempFlujoInversion.Add(temp);
                                    _context.SaveChanges();

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
                            else
                            {
                                //Aqui entra cuando alguno de los campos obligatorios no viene diligenciado
                                //string strValidateCampNullsOrEmpty = "";
                                //Valida que todos los campos esten vacios porque las validaciones del excel hacen que lea todos los rows como ingresado información 

                                for (int j = 2; j < worksheet.Dimension.Columns; j++)
                                {
                                    CantidadRegistrosInvalidos++;
                                }


                                // for (int j = 1; j < 7; j++)
                                // {
                                //     strValidateCampNullsOrEmpty += (worksheet.Cells[i, j].Text);
                                // }
                                // if (string.IsNullOrEmpty(strValidateCampNullsOrEmpty))
                                // {
                                //     CantidadRegistrosVacios++;
                                // }
                                // else
                                // {
                                //     CantidadRegistrosInvalidos++;

                                // }
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
                    archivoCarge.CantidadRegistros = ((worksheet.Dimension.Rows - 1) * (worksheet.Dimension.Columns - 2) - CantidadRegistrosVacios);
                    _context.ArchivoCargue.Update(archivoCarge);


                    ArchivoCargueRespuesta archivoCargueRespuesta = new ArchivoCargueRespuesta
                    {
                        CantidadDeRegistros = archivoCarge.CantidadRegistros.ToString(),
                        CantidadDeRegistrosInvalidos = archivoCarge.CantidadRegistrosInvalidos.ToString(),
                        CantidadDeRegistrosValidos = archivoCarge.CantidadRegistrosValidos.ToString(),
                        LlaveConsulta = archivoCarge.Nombre

                    };
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

        public async Task<List<ArchivoCargue>> GetLoadInvestmentFlowGrid(int pContratoConstruccionId)
        {
            List<ArchivoCargue> listaCargas = new List<ArchivoCargue>();

            List<TempFlujoInversion> lista = _context.TempFlujoInversion.Where(tp => tp.ContratoConstruccionId == pContratoConstruccionId).ToList();

            lista.ForEach(c =>
            {
                ArchivoCargue archivo = _context.ArchivoCargue.Where(a => a.ArchivoCargueId == c.ArchivoCargueId && a.Eliminado != true).FirstOrDefault();
                if (archivo != null)
                {
                    archivo.estadoCargue = archivo.CantidadRegistros == archivo.CantidadRegistrosValidos ? "Validos" : "Fallido";

                    listaCargas.Add(archivo);
                }
            });

            return listaCargas;

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
                        //Data = this.GetContratoByContratoId( pConstruccion.ContratoId ),
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

        public async Task<Respuesta> DeleteArchivoCargue(int pArchivocargue, string pUsuarioModificacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Archivo_Cargue, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ArchivoCargue archivoCargue = _context.ArchivoCargue.Find(pArchivocargue);

                archivoCargue.UsuarioModificacion = pUsuarioModificacion;
                archivoCargue.FechaModificacion = DateTime.Now;
                archivoCargue.Eliminado = true;

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

        public async Task<byte[]> GetPDFDRP(int pContratoId, string usuarioModificacion)
        {
            if (pContratoId == 0)
            {
                throw new Exception("Debe enviar el id del contrato");
            }

            Contrato contrato = _context.Contrato.Where( c => c.ContratoId == pContratoId )
                                                    .Include( r => r.Contratacion )
                                                        .ThenInclude( r => r.DisponibilidadPresupuestal )
                                                    .FirstOrDefault();

            if ( contrato?.Contratacion?.DisponibilidadPresupuestal != null )
                return await _budgetAvailabilityService.GetPDFDRP( contrato.Contratacion.DisponibilidadPresupuestal.FirstOrDefault().DisponibilidadPresupuestalId, usuarioModificacion  );
            else{
                throw new Exception("El contrato no tiene DRP");
            }

        }

        private string CambiarValidarEstados( string pTipoContrato, string pEstadoCodigo ){

        }


    }

}
