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

namespace asivamosffie.services
{
    public class TechnicalRequirementsConstructionPhaseService : ITechnicalRequirementsConstructionPhaseService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly IDocumentService _documentService;
        public readonly IConverter _converter;
        public readonly IRegisterPreContructionPhase1Service _registerPreContructionPhase1Service;

        public TechnicalRequirementsConstructionPhaseService(IConverter converter,
                                                            devAsiVamosFFIEContext context,
                                                            ICommonService commonService,
                                                            IDocumentService documentService,
                                                            IRegisterPreContructionPhase1Service registerPreContructionPhase1Service)
        {
            _converter = converter;
            _context = context;
            _documentService = documentService;
            _commonService = commonService;
            _registerPreContructionPhase1Service = registerPreContructionPhase1Service;
        }

        public async Task<List<dynamic>> GetContractsGrid(int pUsuarioId)
        {
            List<dynamic> listaContrats = new List<dynamic>();

            List<VRequisitosTecnicosInicioConstruccion> lista = _context.VRequisitosTecnicosInicioConstruccion.ToList();

            lista.ForEach(c =>
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
                return await _registerPreContructionPhase1Service.GetContratoByContratoId(pContratoId);
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

                    _context.ContratoConstruccion.Add( contratoConstruccion );
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

                    _context.ContratoConstruccion.Add( contratoConstruccion );
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

    }

}
