﻿using System;
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
using System.IO;
using System.Text;
using ClosedXML.Excel;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Globalization;
using asivamosffie.services.Validators;
using asivamosffie.services.Filters;
using System.Data.Common;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class ProjectContractingService : IProjectContractingService
    {

        private readonly ICommonService _commonService;
        private readonly IDocumentService _documentService;
        private readonly devAsiVamosFFIEContext _context;

        public ProjectContractingService(devAsiVamosFFIEContext context, ICommonService commonService, IDocumentService documentService)
        {
            _documentService = documentService;
            _commonService = commonService;
            _context = context;
        }

        public async Task<List<ContratacionProyecto>> GetListContratacionProyectoByContratacionId(int idContratacion)
        {

            //devuelto = array ContratacionProyecto  + proyecto + contratista + ProyectoAportante + CofinanciacionAportante

            List<ContratacionProyecto> ListContratacionProyecto = new List<ContratacionProyecto>();

            ListContratacionProyecto = await _context.ContratacionProyecto.
                Where(r => !(bool)r.Eliminado && r.ContratacionId == idContratacion).
                IncludeFilter(r => r.Proyecto).Where(r => !(bool)r.Eliminado).
                IncludeFilter(r => r.Contratacion.Contratista).Where(r => !(bool)r.Eliminado).
                IncludeFilter(r => r.ContratacionProyectoAportante.Where(r => !(bool)r.Eliminado)).ToListAsync();

            foreach (var item in ListContratacionProyecto)
            {
                foreach (var ContratacionProyectoAportante in item.ContratacionProyectoAportante)
                {
                    ContratacionProyectoAportante.Aportante = await _context.CofinanciacionAportante.Where(r => !(bool)r.Eliminado && r.CofinanciacionAportanteId == ContratacionProyectoAportante.AportanteId).FirstOrDefaultAsync();
                }
            }
            return ListContratacionProyecto;


        }

        public async Task<List<Contratacion>> GetListContratacion()
        {

            List<Contratacion> ListContratacion = await _context.Contratacion.ToListAsync();

            foreach (var Contratacion in ListContratacion)
            {
                if (!string.IsNullOrEmpty(Contratacion.TipoSolicitudCodigo))
                {
                    Contratacion.TipoSolicitudCodigo = await _commonService.GetNombreDominioByCodigoAndTipoDominio(Contratacion.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_de_Solicitud);
                }
                if (!string.IsNullOrEmpty(Contratacion.EstadoSolicitudCodigo))
                {
                    Contratacion.EstadoSolicitudCodigo = await _commonService.GetNombreDominioByCodigoAndTipoDominio(Contratacion.EstadoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_de_Solicitud);
                }
            }
            return ListContratacion;
        }

        public async Task<List<ContratistaGrilla>> GetListContractingByFilters(string pTipoIdentificacionCodigo, string pNumeroIdentidicacion, string pNombre, bool? EsConsorcio)
        {
            List<ContratistaGrilla> ListContratistaGrillas = new List<ContratistaGrilla>();

            IQueryable<Contratista> contratistas = _context.Contratista.Where(
                r => (bool)r.Activo);

            if (!string.IsNullOrEmpty(pTipoIdentificacionCodigo))
            {
                contratistas = contratistas.Where(r => r.TipoIdentificacionCodigo.Equals(pTipoIdentificacionCodigo));
            }
            if (!string.IsNullOrEmpty(pNumeroIdentidicacion))
            {
                contratistas = contratistas.Where(r => r.NumeroIdentificacion.Contains(pNumeroIdentidicacion));
            }
            if (!string.IsNullOrEmpty(pNombre))
            {
                contratistas = contratistas.Where(r => r.Nombre.ToUpper().Contains(pNombre.ToUpper()));
            }
            //TODO: Validar si se compara asi los bool
            if (EsConsorcio != null)
            {
                contratistas = contratistas.Where(r => r.EsConsorcio == EsConsorcio);
            }

            foreach (var contratista in contratistas)
            {
                ContratistaGrilla contratistaGrilla = new ContratistaGrilla
                {
                    IdContratista = contratista.ContratistaId,
                    Nombre = contratista.Nombre,
                    NumeroIdentificacion = contratista.NumeroIdentificacion,
                    EsConsorcio = (bool)contratista.EsConsorcio,
                    NumeroInvitacion = contratista.NumeroInvitacion,
                    RepresentanteLegal = contratista.RepresentanteLegal
                };
                ListContratistaGrillas.Add(contratistaGrilla);
            }
            return ListContratistaGrillas;
        }

        public async Task<List<ProyectoGrilla>> GetListProyectsByFilters(
            string pTipoIntervencion,
            string pLlaveMen,
            string pMunicipio,
            int pIdInstitucionEducativa,
            int pIdSede)
        {
            //Listar Los proyecto segun caso de uso solo trae los ue estado
            //estado de registro “Completo”, que tienen viabilidad jurídica y técnica

            string strCodigoEstadoJuridicoAprobado = ConstantCodigoEstadoJuridico.Aprobado;

            List<Proyecto> ListProyectos =
                _context.Proyecto.Where(
                    r => !(bool)r.Eliminado &&
                    r.EstadoJuridicoCodigo.Equals(strCodigoEstadoJuridicoAprobado) &&
                    (bool)r.RegistroCompleto).Distinct().ToList();

            if (!string.IsNullOrEmpty(pTipoIntervencion))
            {
                ListProyectos = ListProyectos.Where(r => r.TipoIntervencionCodigo.Equals(pTipoIntervencion)).ToList();
            }
            if (!string.IsNullOrEmpty(pLlaveMen))
            {
                ListProyectos = ListProyectos.Where(r => r.LlaveMen.Contains(pLlaveMen)).ToList();
            }
            if (!string.IsNullOrEmpty(pMunicipio))
            {
                ListProyectos = ListProyectos.Where(r => r.LocalizacionIdMunicipio.Equals(pMunicipio)).ToList();
            }
            if (pIdInstitucionEducativa > 0)
            {
                ListProyectos = ListProyectos.Where(r => r.InstitucionEducativaId.Equals(pIdInstitucionEducativa)).ToList();
            }
            if (pIdSede > 0)
            {
                ListProyectos = ListProyectos.Where(r => r.SedeId.Equals(pIdSede)).ToList();
            }
            //
            List<ProyectoGrilla> ListProyectoGrilla = new List<ProyectoGrilla>();
            foreach (var proyecto in ListProyectos)
            {
                //Validacion
                if (!string.IsNullOrEmpty(proyecto.LocalizacionIdMunicipio) 
                    && !string.IsNullOrEmpty(proyecto.TipoIntervencionCodigo)
                    && !string.IsNullOrEmpty(proyecto.SedeId.ToString())
                    && !string.IsNullOrEmpty(proyecto.InstitucionEducativaId.ToString())
                 )
                {
                    try
                    {
                        ProyectoGrilla proyectoGrilla = new ProyectoGrilla
                        {
                            TipoIntervencion = await _commonService.GetNombreDominioByCodigoAndTipoDominio(proyecto.TipoIntervencionCodigo, (int)EnumeratorTipoDominio.Tipo_de_Intervencion),
                            LlaveMen = proyecto.LlaveMen,
                            Region = _commonService.GetNombreRegionByIdMunicipio(proyecto.LocalizacionIdMunicipio),
                            Departamento = _commonService.GetNombreDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio),
                            Municipio = _commonService.GetNombreLocalizacionByLocalizacionId(proyecto.LocalizacionIdMunicipio),
                            InstitucionEducativa = _context.InstitucionEducativaSede.Find(proyecto.InstitucionEducativaId).Nombre,
                            Sede = _context.InstitucionEducativaSede.Find(proyecto.SedeId).Nombre,

                            ProyectoId = proyecto.ProyectoId,
                          
                        };
                        ListProyectoGrilla.Add(proyectoGrilla);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return ListProyectoGrilla.OrderByDescending(r=> r.ProyectoId).ToList();
        }

        public async Task<Respuesta> CreateContratacionProyecto(int[] idsProyectos, string tipoSolicitudCodigo, string usuarioCreacion)
        {
            Respuesta respuesta = new Respuesta();
            int idAccionCrearContratacionProyecto = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Proyecto, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                foreach (var idProyecto in idsProyectos)
                {
                    //Crear contratacion 
                    Contratacion contratacion = new Contratacion
                    {
                        //Auditoria
                        FechaCreacion = DateTime.Now,
                        UsuarioCreacion = usuarioCreacion,
                        Eliminado = false,

                        //Registros

                        //TODO: Poner contratistaID y los demas campos
                        TipoSolicitudCodigo = tipoSolicitudCodigo,
                        FechaSolicitud = DateTime.Now,
                        NumeroSolicitud = await _commonService.GetNumeroSolicitudContratacion()
                        //Contratista = ContratistaId 
                        //EsObligacionEspecial = (bool),
                        //ConsideracionDescripcion = "" 
                    };
                    contratacion.RegistroCompleto = ValidarEstado(contratacion);
                    //Se guarda para tener idContratacion y relacionarlo con la tabla contratacionProyecto
                    _context.Contratacion.Add(contratacion);
                    _context.SaveChanges();

                    //Crear contratacionProyecto
                    ContratacionProyecto contratacionProyecto = new ContratacionProyecto
                    {
                        //Auditoria
                        FechaCreacion = DateTime.Now,
                        UsuarioCreacion = usuarioCreacion,
                        Eliminado = false,
                        //Registros
                        ContratacionId = contratacion.ContratacionId,
                        ProyectoId = idProyecto,

                    };
                    _context.ContratacionProyecto.Add(contratacionProyecto);
                    _context.SaveChanges();
                }
                return respuesta =
                 new Respuesta
                 {
                     IsSuccessful = true,
                     IsException = false,
                     IsValidation = true,
                     Code = ConstantMessagesContratacionProyecto.OperacionExitosa,
                     Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Contratacion_Proyecto, ConstantMessagesContratacionProyecto.Error, idAccionCrearContratacionProyecto, usuarioCreacion, "")
                 };
            }
            catch (Exception ex)
            {
                return respuesta =
                 new Respuesta
                 {
                     IsSuccessful = false,
                     IsException = true,
                     IsValidation = false,
                     Code = ConstantMessagesProyecto.Error,
                     Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Proyecto, ConstantMessagesProyecto.Error, idAccionCrearContratacionProyecto, usuarioCreacion, ex.InnerException.ToString())
                 };
            }
        }


        public async Task<Respuesta> CreateEditContratacion(Contratacion Pcontratacion)
        {

            Respuesta respuesta = new Respuesta();
            int idAccionCrearContratacionProyecto = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Proyecto, (int)EnumeratorTipoDominio.Acciones);

            try
            {

                if (Pcontratacion.ContratacionId == null || Pcontratacion.ContratacionId == 0)
                {
                    Pcontratacion.Eliminado = false;
                    Pcontratacion.FechaCreacion = DateTime.Now;
                    Pcontratacion.FechaSolicitud = DateTime.Now;
                    //Metodo que valida si todos los registros estan completos retorna true si completos
                    Pcontratacion.RegistroCompleto = ValidarEstado(Pcontratacion);
                    Pcontratacion.NumeroSolicitud = await _commonService.GetNumeroSolicitudContratacion();
                    _context.Contratacion.Add(Pcontratacion);
                }
                else
                {
                    Contratacion contratacionVieja = await _context.Contratacion.Where(r => r.ContratacionId == Pcontratacion.ContratacionId).Include(r => r.Contratista).FirstOrDefaultAsync();
                    contratacionVieja.TipoSolicitudCodigo = Pcontratacion.TipoSolicitudCodigo;
                    contratacionVieja.EstadoSolicitudCodigo = Pcontratacion.EstadoSolicitudCodigo;
                    contratacionVieja.RegistroCompleto = ValidarEstado(contratacionVieja);
                    _context.Update(contratacionVieja);
                }
                _context.SaveChanges();

                return respuesta =
           new Respuesta
           {
               IsSuccessful = true,
               IsException = false,
               IsValidation = false,
               Code = ConstantMessagesProyecto.Error,
               Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Proyecto, ConstantMessagesProyecto.Error, idAccionCrearContratacionProyecto, Pcontratacion.UsuarioCreacion, "")
           };
            }
            catch (Exception ex)
            {
                return respuesta =
                           new Respuesta
                           {
                               IsSuccessful = false,
                               IsException = true,
                               IsValidation = false,
                               Code = ConstantMessagesProyecto.Error,
                               Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Proyecto, ConstantMessagesProyecto.Error, idAccionCrearContratacionProyecto, Pcontratacion.UsuarioCreacion, ex.InnerException.ToString())
                           };
            }

        }

        public bool ValidarEstado(Contratacion contratacion)
        {

            if (!string.IsNullOrEmpty(contratacion.TipoSolicitudCodigo)
             || !string.IsNullOrEmpty(contratacion.NumeroSolicitud)
             || !string.IsNullOrEmpty(contratacion.EstadoSolicitudCodigo)
             || !string.IsNullOrEmpty(contratacion.ContratacionId.ToString())
             || (contratacion.EsObligacionEspecial != null)
             || !string.IsNullOrEmpty(contratacion.ConsideracionDescripcion))
            {
                return true;
            }
            return false;
        }

        public async Task<Respuesta> CreateEditContratacionProyecto(ContratacionProyecto contratacionProyecto)
        {
            Respuesta respuesta = new Respuesta();
            int idAccionCrearContratacionContrataicionProyecto = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Contratacion_Proyecto, (int)EnumeratorTipoDominio.Acciones);
            string strAccion = " ";

            try
            {

                if (contratacionProyecto.ContratacionProyectoId != null || contratacionProyecto.ContratacionProyectoId == 0)
                {
                    strAccion = "CREAR CONTRATACION PROYECTO";

                    //Auditoria No guardo usuario Creacion Porque ya viene desde el controller

                    contratacionProyecto.FechaCreacion = DateTime.Now;
                    contratacionProyecto.Eliminado = false;

                    _context.ContratacionProyecto.Add(contratacionProyecto);
                    _context.SaveChanges();
                }

                else
                {
                    strAccion = "EDITAR CONTRATACION PROYECTO";

                    ContratacionProyecto contratacionProyectoAntiguo = _context.ContratacionProyecto.Find(contratacionProyecto.ContratacionProyectoId);
                    //Auditoria 
                    contratacionProyectoAntiguo.FechaModificacion = DateTime.Now;
                    contratacionProyectoAntiguo.UsuarioModificacion = contratacionProyecto.UsuarioCreacion;

                    //registros
                    contratacionProyectoAntiguo.ContratacionId = contratacionProyecto.ContratacionId;
                    contratacionProyectoAntiguo.ProyectoId = contratacionProyecto.ProyectoId;
                    contratacionProyectoAntiguo.EsReasignacion = contratacionProyecto.EsReasignacion;
                    contratacionProyectoAntiguo.EsAvanceObra = contratacionProyecto.EsAvanceObra;

                    contratacionProyectoAntiguo.RequiereLicencia = contratacionProyecto.RequiereLicencia;
                    contratacionProyectoAntiguo.LicenciaVigente = contratacionProyecto.LicenciaVigente;
                    contratacionProyectoAntiguo.NumeroLicencia = contratacionProyecto.NumeroLicencia;
                    contratacionProyectoAntiguo.FechaVigencia = contratacionProyecto.FechaVigencia;

                    _context.SaveChanges();
                }
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesProyecto.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Contratacion_Proyecto, ConstantMessagesProyecto.OperacionExitosa, idAccionCrearContratacionContrataicionProyecto, contratacionProyecto.UsuarioCreacion, strAccion)
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesProyecto.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Contratacion_Proyecto, ConstantMessagesProyecto.Error, idAccionCrearContratacionContrataicionProyecto, contratacionProyecto.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }


        public async Task<Respuesta> CreateEditContratacionProyectoAportante(ContratacionProyectoAportante pContratacionProyectoAportante)
        {
            Respuesta respuesta = new Respuesta();
            int idAccionCrearContratacionContrataicionProyectoAportante = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Contratacion_Proyecto_Aportante, (int)EnumeratorTipoDominio.Acciones);
            string strAccion = " ";

            try
            {

                if (pContratacionProyectoAportante.ContratacionProyectoAportanteId != null || pContratacionProyectoAportante.ContratacionProyectoAportanteId == 0)
                {
                    strAccion = "CREAR CONTRATACION PROYECTO APORTANTE";

                    //Auditoria No guardo usuario Creacion Porque ya viene desde el controller

                    pContratacionProyectoAportante.FechaCreacion = DateTime.Now;
                    pContratacionProyectoAportante.Eliminado = false;

                    _context.ContratacionProyectoAportante.Add(pContratacionProyectoAportante);
                    _context.SaveChanges();
                }

                else
                {
                    strAccion = "EDITAR CONTRATACION  PROYECTO APORTANTE";

                    ContratacionProyectoAportante contratacionProyectoAportanteAntiguo = _context.ContratacionProyectoAportante.Find(pContratacionProyectoAportante.ContratacionProyectoAportanteId);
                    //Auditoria 
                    contratacionProyectoAportanteAntiguo.FechaModificacion = DateTime.Now;
                    contratacionProyectoAportanteAntiguo.UsuarioModificacion = contratacionProyectoAportanteAntiguo.UsuarioCreacion;

                    //registros
                    contratacionProyectoAportanteAntiguo.ContratacionProyectoId = pContratacionProyectoAportante.ContratacionProyectoId;
                    contratacionProyectoAportanteAntiguo.AportanteId = pContratacionProyectoAportante.AportanteId;
                    contratacionProyectoAportanteAntiguo.ValorAporte = pContratacionProyectoAportante.ValorAporte;

                    _context.SaveChanges();
                }
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesProyecto.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Contratacion_Proyecto, ConstantMessagesProyecto.OperacionExitosa, idAccionCrearContratacionContrataicionProyectoAportante, pContratacionProyectoAportante.UsuarioCreacion, strAccion)
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesProyecto.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Contratacion_Proyecto, ConstantMessagesProyecto.Error, idAccionCrearContratacionContrataicionProyectoAportante, pContratacionProyectoAportante.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }



    }
}
