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
using System.Reflection.Metadata;

namespace asivamosffie.services
{
    public class ProjectService : IProjectService
    {
        private readonly ICommonService _commonService;
        private readonly IDocumentService _documentService;
        private readonly devAsiVamosFFIEContext _context;

        public ProjectService(devAsiVamosFFIEContext context, ICommonService commonService, IDocumentService documentService)
        {
            _documentService = documentService;
            _commonService = commonService;
            _context = context;
        }
        public async Task<ProyectoGrilla> GetProyectoGrillaByProyectoId(int idProyecto)
        {
            Proyecto pProyecto = _context.Proyecto.Find(idProyecto);
            if (pProyecto != null)
            {

                List<Localizacion> ListLocalizacion = _context.Localizacion.ToList();
                List<Dominio> ListParametricas = _context.Dominio
                    .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Juridico_Predios
                    || r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion
                    || r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Proyecto
                    ).ToList();
                try
                {
                    Localizacion municipio = ListLocalizacion.Where(r => r.LocalizacionId == pProyecto.LocalizacionIdMunicipio).FirstOrDefault();
                    Localizacion departamento = ListLocalizacion.Where(r => r.LocalizacionId == municipio.IdPadre).FirstOrDefault();

                    Dominio EstadoJuridicoPredios = await _commonService.GetDominioByNombreDominioAndTipoDominio(pProyecto.EstadoJuridicoCodigo, (int)EnumeratorTipoDominio.Estado_Juridico_Predios);
                    ProyectoGrilla proyectoGrilla = new ProyectoGrilla
                    {
                        LlaveMen = pProyecto.LlaveMen,
                        ProyectoId = pProyecto.ProyectoId,
                        Departamento = departamento.Descripcion,
                        Municipio = municipio.Descripcion,
                        InstitucionEducativa = _context.InstitucionEducativaSede.Find(pProyecto.InstitucionEducativaId).Nombre,
                        Sede = _context.InstitucionEducativaSede.Find(pProyecto.SedeId).Nombre,
                        EstadoJuridicoPredios = ListParametricas.Where(r => r.TipoDominioId == ((int)EnumeratorTipoDominio.Estado_Juridico_Predios) && r.Codigo == pProyecto.EstadoJuridicoCodigo).FirstOrDefault().Nombre,
                        TipoIntervencion = ListParametricas.Where(r => r.TipoDominioId == ((int)EnumeratorTipoDominio.Tipo_de_Intervencion) && r.Codigo == pProyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre,
                        EstadoProyecto = ListParametricas.Where(r => r.TipoDominioId == ((int)EnumeratorTipoDominio.Estado_Proyecto) && r.Codigo == pProyecto.EstadoProyectoCodigo).FirstOrDefault().Nombre,
                        Fecha = pProyecto.FechaCreacion != null ? Convert.ToDateTime(pProyecto.FechaCreacion).ToString("yyyy-MM-dd") : pProyecto.FechaCreacion.ToString(),
                        EstadoRegistro = "COMPLETO",
                        EstadoProyectoCodigo = pProyecto.EstadoProyectoCodigo,

                    };

                    if (!(bool)pProyecto.RegistroCompleto)
                    {
                        proyectoGrilla.EstadoRegistro = "INCOMPLETO";
                    }
                    return proyectoGrilla;
                }
                catch (Exception ex)
                {
                    return new ProyectoGrilla();
                }
            }
            else
            {
                return new ProyectoGrilla();
            }
        }

        public static bool ValidarRegistroEDITAR(Proyecto proyecto)
        {
            try
            {

                if (
                       string.IsNullOrEmpty(proyecto.FechaSesionJunta.ToString())
                    || string.IsNullOrEmpty(proyecto.NumeroActaJunta.ToString())
                    || string.IsNullOrEmpty(proyecto.TipoIntervencionCodigo.ToString())
                    || string.IsNullOrEmpty(proyecto.LlaveMen.ToString())
                    || string.IsNullOrEmpty(proyecto.LocalizacionIdMunicipio.ToString())
                    || string.IsNullOrEmpty(proyecto.InstitucionEducativaId.ToString())
                    || string.IsNullOrEmpty(proyecto.SedeId.ToString())
                    || string.IsNullOrEmpty(proyecto.EnConvocatoria.ToString())
                    //|| string.IsNullOrEmpty(proyecto.ConvocatoriaId.ToString())
                    || string.IsNullOrEmpty(proyecto.CantPrediosPostulados.ToString())
                    || string.IsNullOrEmpty(proyecto.TipoPredioCodigo.ToString())
                    || string.IsNullOrEmpty(proyecto.PredioPrincipalId.ToString())
                    //|| string.IsNullOrEmpty(proyecto.ValorObra.ToString())
                    //|| string.IsNullOrEmpty(proyecto.ValorInterventoria.ToString())
                    //|| string.IsNullOrEmpty(proyecto.ValorTotal.ToString())
                    // || string.IsNullOrEmpty(proyecto.EstadoProyectoCodigo.ToString())
                    || string.IsNullOrEmpty(proyecto.EstadoJuridicoCodigo.ToString())
                    || string.IsNullOrEmpty(proyecto.PlazoMesesObra.ToString())
                    || string.IsNullOrEmpty(proyecto.PlazoDiasObra.ToString())
                    || string.IsNullOrEmpty(proyecto.PlazoMesesInterventoria.ToString())
                    || string.IsNullOrEmpty(proyecto.PlazoDiasInterventoria.ToString())
                    || string.IsNullOrEmpty(proyecto.CoordinacionResponsableCodigo.ToString())
                    )
                {
                    return false;
                }

                //VALIDAR PREDIO PRINCIPAL
                if (
                     /*string.IsNullOrEmpty(proyecto.PredioPrincipal.TipoPredioCodigo)
                  || */string.IsNullOrEmpty(proyecto.PredioPrincipal.UbicacionLatitud)
                  || string.IsNullOrEmpty(proyecto.PredioPrincipal.UbicacionLongitud)
                  || string.IsNullOrEmpty(proyecto.PredioPrincipal.Direccion)
                  || string.IsNullOrEmpty(proyecto.PredioPrincipal.DocumentoAcreditacionCodigo)
                  || string.IsNullOrEmpty(proyecto.PredioPrincipal.NumeroDocumento)
                    //|| string.IsNullOrEmpty(proyecto.PredioPrincipal.CedulaCatastral)
                    )
                {
                    return false;
                }


                //VALIDAR TODOS LOS PREDIOS 
                foreach (var Predio in proyecto.ProyectoPredio)
                {
                    if (
                       //   string.IsNullOrEmpty(Predio.Predio.InstitucionEducativaSede.ToString())
                       //|| string.IsNullOrEmpty(Predio.Predio.TipoPredioCodigo)
                       //|| string.IsNullOrEmpty(Predio.Predio.UbicacionLatitud)
                       //|| string.IsNullOrEmpty(Predio.Predio.UbicacionLongitud)
                       //|| string.IsNullOrEmpty(Predio.Predio.Direccion)
                       string.IsNullOrEmpty(Predio.Predio.DocumentoAcreditacionCodigo)
                        || string.IsNullOrEmpty(Predio.Predio.NumeroDocumento)
                        || (Predio.Predio.DocumentoAcreditacionCodigo == "1" && string.IsNullOrEmpty(Predio.Predio.CedulaCatastral))
                       )
                    {
                        return false;
                    }
                }
                foreach (var proyectoAportante in proyecto.ProyectoAportante)
                {
                    if (
                           string.IsNullOrEmpty(proyectoAportante.AportanteId.ToString())
                        || string.IsNullOrEmpty(proyectoAportante.ValorObra.ToString())
                        || string.IsNullOrEmpty(proyectoAportante.ValorInterventoria.ToString())
                        || string.IsNullOrEmpty(proyectoAportante.ValorTotalAportante.ToString())
                        )
                    {
                        return false;
                    }
                }
                //VALIDAR INFRAESTRUCTURA A INTERVENIR
                foreach (var infraestructuraIntervenirProyecto in proyecto.InfraestructuraIntervenirProyecto)
                {
                    if (
                         string.IsNullOrEmpty(infraestructuraIntervenirProyecto.InfraestructuraCodigo)
                         || string.IsNullOrEmpty(infraestructuraIntervenirProyecto.Cantidad.ToString())
                         
                        )
                    {
                        return false;
                    }
                }
                // Si llega hasta aqui es que tiene todo completo
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool ValidarRegistroCREAR(Proyecto proyecto, Predio predio)
        {
            try
            {

                if (
                       string.IsNullOrEmpty(proyecto.FechaSesionJunta.ToString())
                    || string.IsNullOrEmpty(proyecto.NumeroActaJunta.ToString())
                    || string.IsNullOrEmpty(proyecto.TipoIntervencionCodigo.ToString())
                    || string.IsNullOrEmpty(proyecto.LlaveMen.ToString())
                    || string.IsNullOrEmpty(proyecto.LocalizacionIdMunicipio.ToString())
                    || string.IsNullOrEmpty(proyecto.InstitucionEducativaId.ToString())
                    || string.IsNullOrEmpty(proyecto.SedeId.ToString())
                    || string.IsNullOrEmpty(proyecto.EnConvocatoria.ToString())
                    //|| string.IsNullOrEmpty(proyecto.ConvocatoriaId.ToString())
                    || string.IsNullOrEmpty(proyecto.CantPrediosPostulados.ToString())
                    || string.IsNullOrEmpty(proyecto.TipoPredioCodigo.ToString())
                    //|| string.IsNullOrEmpty(proyecto.PredioPrincipalId.ToString())
                    //|| string.IsNullOrEmpty(proyecto.ValorObra.ToString())
                    //|| string.IsNullOrEmpty(proyecto.ValorInterventoria.ToString())
                    //|| string.IsNullOrEmpty(proyecto.ValorTotal.ToString())
                    // || string.IsNullOrEmpty(proyecto.EstadoProyectoCodigo.ToString())
                    // || string.IsNullOrEmpty(proyecto.EstadoJuridicoCodigo.ToString())
                    || string.IsNullOrEmpty(proyecto.PlazoMesesObra.ToString())
                    || string.IsNullOrEmpty(proyecto.PlazoDiasObra.ToString())
                    || string.IsNullOrEmpty(proyecto.PlazoMesesInterventoria.ToString())
                    || string.IsNullOrEmpty(proyecto.PlazoDiasInterventoria.ToString())
                    || string.IsNullOrEmpty(proyecto.CoordinacionResponsableCodigo.ToString())

                    )
                {
                    return false;
                }

                //VALIDAR PREDIO PRINCIPAL
                if (
                //     string.IsNullOrEmpty(predio.TipoPredioCodigo)
                  string.IsNullOrEmpty(predio.UbicacionLatitud)
                  || string.IsNullOrEmpty(predio.UbicacionLongitud)
                  || string.IsNullOrEmpty(predio.Direccion)
                  || string.IsNullOrEmpty(predio.DocumentoAcreditacionCodigo)
                  || string.IsNullOrEmpty(predio.NumeroDocumento)
                    //|| string.IsNullOrEmpty(predio.CedulaCatastral)
                    )
                {
                    return false;
                }


                //VALIDAR TODOS LOS PREDIOS 
                foreach (var Predio in proyecto.ProyectoPredio)
                {
                    if (
                         //!string.IsNullOrEmpty(Predio.Predio.TipoPredioCodigo)
                         // string.IsNullOrEmpty(Predio.Predio.UbicacionLatitud)
                         //|| string.IsNullOrEmpty(Predio.Predio.UbicacionLongitud)
                         //|| string.IsNullOrEmpty(Predio.Predio.Direccion)
                         string.IsNullOrEmpty(Predio.Predio.DocumentoAcreditacionCodigo)
                        || string.IsNullOrEmpty(Predio.Predio.NumeroDocumento)
                       //|| string.IsNullOrEmpty(Predio.Predio.CedulaCatastral)
                       )
                    {
                        return false;
                    }
                }
                foreach (var proyectoAportante in proyecto.ProyectoAportante)
                {
                    if (
                           string.IsNullOrEmpty(proyectoAportante.AportanteId.ToString())
                        || string.IsNullOrEmpty(proyectoAportante.ValorObra.ToString())
                        || string.IsNullOrEmpty(proyectoAportante.ValorInterventoria.ToString())
                        || string.IsNullOrEmpty(proyectoAportante.ValorTotalAportante.ToString())
                        )
                    {
                        return false;
                    }
                }
                //VALIDAR INFRAESTRUCTURA A INTERVENIR
                foreach (var infraestructuraIntervenirProyecto in proyecto.InfraestructuraIntervenirProyecto)
                {
                    if (
                         string.IsNullOrEmpty(infraestructuraIntervenirProyecto.InfraestructuraCodigo)
                         || string.IsNullOrEmpty(infraestructuraIntervenirProyecto.Cantidad.ToString())
                        )
                    {
                        return false;
                    }
                }
                // Si llega hasta aqui es que tiene todo completo
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<ProyectoGrilla>> ListProyectos()
        {
            List<ProyectoGrilla> ListProyectoGrilla = new List<ProyectoGrilla>();

            List<Proyecto> ListProyectos = await _context.Proyecto.Where(r => !(bool)r.Eliminado).Include(r => r.InstitucionEducativa).Include(r => r.ProyectoPredio).Distinct().ToListAsync();

            List<Localizacion> ListLocalizacion = _context.Localizacion.ToList();
            List<Dominio> ListParametricas = _context.Dominio
                .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Juridico_Predios
                || r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion
                || r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud
                ).ToList();


            foreach (var proyecto in ListProyectos)
            {
                try
                {

                    Localizacion municipio = ListLocalizacion.Where(r => r.LocalizacionId == proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                    Localizacion departamento = municipio==null?null:ListLocalizacion.Where(r => r.LocalizacionId == municipio.IdPadre).FirstOrDefault();
                    var estado = proyecto.EstadoProyectoCodigo;
                    var estaso = "";
                    if (estado != null)
                    {
                        estaso = _context.Dominio.Where(r => r.TipoDominioId == ((int)EnumeratorTipoDominio.Estado_Proyecto) && r.Codigo == proyecto.EstadoProyectoCodigo).FirstOrDefault().Nombre;
                    }

                    ProyectoGrilla proyectoGrilla = new ProyectoGrilla
                    {

                        ProyectoId = proyecto.ProyectoId,
                        Departamento = departamento==null?"": departamento.Descripcion,
                        Municipio = municipio==null?"": municipio.Descripcion,
                        InstitucionEducativa = proyecto.InstitucionEducativaId==null?"":_context.InstitucionEducativaSede.Find(proyecto.InstitucionEducativaId).Nombre,
                        Sede = proyecto.SedeId==null?"":_context.InstitucionEducativaSede.Find(proyecto.SedeId).Nombre,
                        EstadoJuridicoPredios = proyecto.EstadoJuridicoCodigo==null?"": ListParametricas.Where(r => r.TipoDominioId == ((int)EnumeratorTipoDominio.Estado_Juridico_Predios) && r.Codigo == proyecto.EstadoJuridicoCodigo).FirstOrDefault().Nombre,
                        TipoIntervencion = proyecto.TipoIntervencionCodigo==null?"": ListParametricas.Where(r => r.TipoDominioId == ((int)EnumeratorTipoDominio.Tipo_de_Intervencion) && r.Codigo == proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre,
                        EstadoProyecto = estaso == null ? "" : estaso,
                        Fecha = proyecto.FechaCreacion != null ? Convert.ToDateTime(proyecto.FechaCreacion).ToString("yyyy-MM-dd") : "",
                        EstadoRegistro = "COMPLETO"
                    };
                    if (!(bool)proyecto.RegistroCompleto)
                    {
                        proyectoGrilla.EstadoRegistro = "INCOMPLETO";
                    }
                    ListProyectoGrilla.Add(proyectoGrilla);

                }
                catch (Exception)
                {
                }
            }
            return ListProyectoGrilla.OrderByDescending(r => r.ProyectoId).ToList();

        }

        public async Task<List<VListaProyectos>> ListProyectoOpt() 
        {
            return await _context.VListaProyectos.OrderByDescending(r => r.ProyectoId).ToListAsync(); 
        }
        public async Task<ProyectoGrilla> GetProyectoGrillaByProyecto(Proyecto pProyecto)
        {
            if (!(bool)pProyecto.Eliminado)
            {
                List<Localizacion> ListLocalizacion = _context.Localizacion.ToList();
                List<Dominio> ListParametricas = _context.Dominio
                    .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Juridico_Predios
                    || r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion
                    || r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud
                    ).ToList();
                try
                {
                    Localizacion municipio = ListLocalizacion.Where(r => r.LocalizacionId == pProyecto.LocalizacionIdMunicipio).FirstOrDefault();
                    Localizacion departamento = ListLocalizacion.Where(r => r.LocalizacionId == municipio.IdPadre).FirstOrDefault();

                    Dominio EstadoJuridicoPredios = await _commonService.GetDominioByNombreDominioAndTipoDominio(pProyecto.EstadoJuridicoCodigo, (int)EnumeratorTipoDominio.Estado_Juridico_Predios);
                    ProyectoGrilla proyectoGrilla = new ProyectoGrilla
                    {

                        ProyectoId = pProyecto.ProyectoId,
                        Departamento = departamento.Descripcion,
                        Municipio = municipio.Descripcion,
                        InstitucionEducativa = _context.InstitucionEducativaSede.Find(pProyecto.InstitucionEducativaId).Nombre,
                        Sede = _context.InstitucionEducativaSede.Find(pProyecto.SedeId).Nombre,
                        EstadoJuridicoPredios = ListParametricas.Where(r => r.TipoDominioId == ((int)EnumeratorTipoDominio.Estado_Juridico_Predios) && r.Codigo == pProyecto.EstadoJuridicoCodigo).FirstOrDefault().Nombre,
                        TipoIntervencion = ListParametricas.Where(r => r.TipoDominioId == ((int)EnumeratorTipoDominio.Tipo_de_Intervencion) && r.Codigo == pProyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre,
                        EstadoProyecto = ListParametricas.Where(r => r.TipoDominioId == ((int)EnumeratorTipoDominio.Estado_Proyecto) && r.Codigo == pProyecto.EstadoProyectoCodigo).FirstOrDefault().Nombre,
                        Fecha = pProyecto.FechaCreacion != null ? Convert.ToDateTime(pProyecto.FechaCreacion).ToString("yyyy-MM-dd") : pProyecto.FechaCreacion.ToString(),
                        EstadoRegistro = "COMPLETO"
                    };

                    if (!(bool)pProyecto.RegistroCompleto)
                    {
                        proyectoGrilla.EstadoRegistro = "INCOMPLETO";
                    }
                    return proyectoGrilla;
                }
                catch (Exception e)
                {
                    return new ProyectoGrilla();
                }
            }
            else
            {
                return new ProyectoGrilla();
            }
        }

        public async Task<Respuesta> CreateProyect(Proyecto pProyecto)
        {
            int idAccionCrearProyecto = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Proyecto, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                //Predio PRINCIPAL 


                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = true,
                    Code = ConstantMessagesProyecto.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Proyecto, ConstantMessagesProyecto.OperacionExitosa, idAccionCrearProyecto, pProyecto.UsuarioCreacion, "CREAR PROYECTO")
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
                    Code = ConstantMessagesProyecto.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Proyecto, ConstantMessagesProyecto.Error, idAccionCrearProyecto, pProyecto.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> CreateOrEditProyect(Proyecto pProyecto)
        {
            Respuesta respuesta = new Respuesta();
            int idAccionCrearProyecto = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Proyecto, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                string CrearEditar;
                //CREAR PROYECTO 
                if (pProyecto.ProyectoId == 0)
                {
                    //agrego una condición para validar llave MEN unica, si no estalla por bd
                    var proyectoexistetne = _context.Proyecto.Where(x=>x.LlaveMen==pProyecto.LlaveMen).ToList();
                    if(proyectoexistetne.Count()>0)
                    {
                        return respuesta =
                          new Respuesta
                          {
                              IsSuccessful = false,
                              IsException = false,
                              IsValidation = true,
                              Code = ConstantMessagesProyecto.ErrorLLAVEMEN,
                              Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Proyecto, 
                                ConstantMessagesProyecto.ErrorLLAVEMEN, idAccionCrearProyecto, pProyecto.UsuarioCreacion, "LLAVE MEN EXISTENTE")
                          };
                    }

                    CrearEditar = "CREAR PROYECTO";
                    int? predioid = null;
                    Predio pPredioPrincipal = new Predio();
                    if (pProyecto.InstitucionEducativaId>0)
                    {
                        //Predio Principal
                        Predio predioPrincipal = new Predio
                        {
                            //Predio Auditoria
                            FechaCreacion = DateTime.Now,
                            Activo = true,
                            UsuarioCreacion = pProyecto.UsuarioCreacion,
                            //Predio Registros
                            InstitucionEducativaSedeId = pProyecto.InstitucionEducativaId,
                            TipoPredioCodigo = pProyecto.TipoPredioCodigo.ToString(),
                            UbicacionLatitud = pProyecto.PredioPrincipal.UbicacionLatitud,
                            UbicacionLongitud = pProyecto.PredioPrincipal.UbicacionLongitud,
                            Direccion = pProyecto.PredioPrincipal.Direccion,
                            DocumentoAcreditacionCodigo = pProyecto.PredioPrincipal.DocumentoAcreditacionCodigo,
                            NumeroDocumento = pProyecto.PredioPrincipal.NumeroDocumento,
                            CedulaCatastral = pProyecto.PredioPrincipal.CedulaCatastral,
                        };
                        _context.Predio.Add(predioPrincipal);
                        _context.SaveChanges();
                        predioid = predioPrincipal.PredioId;
                        pPredioPrincipal = predioPrincipal;
                    }
                    Proyecto ProyectoCreado = new Proyecto();
                    //Proyecto 
                    
                        Proyecto proyecto = new Proyecto
                        {
                            //Proyecto Auditoria
                            Eliminado = false,
                            FechaCreacion = DateTime.Now,
                            UsuarioCreacion = pProyecto.UsuarioCreacion,
                            //Proyecto Registros 
                            NumeroActaJunta = pProyecto.NumeroActaJunta,
                            FechaSesionJunta = pProyecto.FechaSesionJunta,
                            TipoIntervencionCodigo = pProyecto.TipoIntervencionCodigo.ToString(),
                            LlaveMen = pProyecto.LlaveMen,
                            LocalizacionIdMunicipio = pProyecto.LocalizacionIdMunicipio!=null? pProyecto.LocalizacionIdMunicipio.ToString():null,
                            InstitucionEducativaId = pProyecto.InstitucionEducativaId,
                            SedeId = pProyecto.SedeId,
                            EnConvocatoria = pProyecto.EnConvocatoria,
                            ConvocatoriaId = pProyecto.ConvocatoriaId,
                            CantPrediosPostulados = pProyecto.CantPrediosPostulados,
                            PredioPrincipalId = predioid,
                            ValorObra = pProyecto.ValorObra,
                            ValorInterventoria = pProyecto.ValorInterventoria,
                            ValorTotal = pProyecto.ValorTotal,
                            TipoPredioCodigo = pProyecto.TipoPredioCodigo.ToString(),
                            PlazoMesesObra = pProyecto.PlazoMesesObra,
                            PlazoDiasObra = pProyecto.PlazoDiasObra,
                            PlazoMesesInterventoria = pProyecto.PlazoMesesInterventoria,
                            PlazoDiasInterventoria = pProyecto.PlazoDiasInterventoria,
                            CoordinacionResponsableCodigo = pProyecto.CoordinacionResponsableCodigo
                        };
                        //si el tipo de intervancion es nuevo el estado juridico es sin revision 
                        //jflorez20201004 todos van en aprovado segun iaranda
                        /*
                        if (proyecto.TipoIntervencionCodigo.Equals(ConstantCodigoTipoIntervencion.Nuevo))
                        {
                            proyecto.EstadoJuridicoCodigo = ConstantCodigoEstadoJuridico.Sin_Revision;
                        }
                        else
                        {
                            proyecto.EstadoJuridicoCodigo = ConstantCodigoEstadoJuridico.Aprobado;
                        }*/
                        proyecto.EstadoJuridicoCodigo = ConstantCodigoEstadoJuridico.Aprobado;
                        proyecto.EstadoProyectoCodigo = ConstantCodigoEstadoProyecto.Disponible;
                        proyecto.RegistroCompleto = ValidarRegistroCREAR(pProyecto, pPredioPrincipal);
                        _context.Proyecto.Add(proyecto);
                        _context.SaveChanges();
                        ProyectoCreado = proyecto;
                    
                    


                    //Agregar Todos los predios que tenga  proyecto
                    foreach (var predio in pProyecto.ProyectoPredio)
                    {
                        Predio predio1 = new Predio();

                        if(pProyecto.InstitucionEducativaId>0)
                        {
                            //Predio Auditoria
                            predio1.FechaCreacion = DateTime.Now;
                            predio1.Activo = true;
                            predio1.UsuarioCreacion = pProyecto.UsuarioCreacion;
                            //Predio Registros
                            predio1.InstitucionEducativaSedeId = pProyecto.InstitucionEducativaId;
                            predio1.TipoPredioCodigo = pProyecto.TipoPredioCodigo.ToString();
                            predio1.UbicacionLatitud = predio.Predio.UbicacionLatitud;
                            predio1.UbicacionLongitud = predio.Predio.UbicacionLongitud;
                            predio1.Direccion = predio.Predio.Direccion;
                            predio1.DocumentoAcreditacionCodigo = predio.Predio.DocumentoAcreditacionCodigo;
                            predio1.NumeroDocumento = predio.Predio.NumeroDocumento;
                            predio1.CedulaCatastral = predio.Predio.CedulaCatastral;


                            _context.Predio.Add(predio1);
                            _context.SaveChanges();
                            //Relacion Proyecto Predio 
                            ProyectoPredio proyectoPredio = new ProyectoPredio();

                            //ProyectoPredio Auditoria
                            proyectoPredio.Activo = true;
                            proyectoPredio.UsuarioCreacion = pProyecto.UsuarioCreacion;
                            proyectoPredio.FechaCreacion = DateTime.Now;
                            //ProyectoPredio Registros

                            proyectoPredio.PredioId = predio1.PredioId;
                            proyectoPredio.ProyectoId = ProyectoCreado.ProyectoId;
                            //Definir que poner
                            proyectoPredio.EstadoJuridicoCodigo = " ";

                            _context.ProyectoPredio.Add(proyectoPredio);
                            _context.SaveChanges();
                        }
                        
                    }

                    decimal valortotal = 0;
                    decimal valorobra = 0;
                    decimal valorinterventoria = 0;
                    //Crear relacion proyectoAportante 
                    foreach (var proyectoAportante in pProyecto.ProyectoAportante)
                    {
                        if(proyectoAportante.AportanteId>0)
                        {
                            ProyectoAportante proyectoAportante1 = new ProyectoAportante
                            {
                                //ProyectoAportante Auditoria
                                Eliminado = false,
                                UsuarioCreacion = ProyectoCreado.UsuarioCreacion,
                                FechaCreacion = DateTime.Now,
                                //ProyectoAportante REGISTROS
                                AportanteId = proyectoAportante.AportanteId,
                                ProyectoId = ProyectoCreado.ProyectoId,
                                ValorObra = proyectoAportante.ValorObra,
                                ValorInterventoria = proyectoAportante.ValorInterventoria,
                                ValorTotalAportante = proyectoAportante.ValorTotalAportante,
                            };
                            valorobra += proyectoAportante.ValorObra != null ? Convert.ToDecimal(proyectoAportante.ValorObra) : 0;
                            valorinterventoria += proyectoAportante.ValorInterventoria != null ? Convert.ToDecimal(proyectoAportante.ValorInterventoria) : 0;
                            valortotal += valorobra + valorinterventoria;
                            _context.ProyectoAportante.Add(proyectoAportante1);

                            //por cada aportante relaciono las fuentes del aportante al proyecto
                            var fuentes = _context.FuenteFinanciacion.Where(x => x.AportanteId == proyectoAportante.AportanteId && !(bool)x.Eliminado).ToList();
                            foreach (var fuente in fuentes)
                            {
                                var proyectosfuenteExiste = _context.ProyectoFuentes.Where(x => x.ProyectoId == ProyectoCreado.ProyectoId
                                      && x.FuenteId == fuente.FuenteFinanciacionId);
                                if(proyectosfuenteExiste.FirstOrDefault()==null)
                                {
                                    _context.ProyectoFuentes.Add(new ProyectoFuentes
                                    {
                                        Eliminado = false,
                                        FechaCreacion = DateTime.Now,
                                        UsuarioCreacion = ProyectoCreado.UsuarioCreacion,
                                        FuenteId = fuente.FuenteFinanciacionId,
                                        ProyectoId = ProyectoCreado.ProyectoId
                                    }); ;
                                }
                                else
                                {
                                    var proyectoFuenteAEditar = proyectosfuenteExiste.FirstOrDefault();
                                    proyectoFuenteAEditar.UsuarioModificacion = ProyectoCreado.UsuarioCreacion;
                                    proyectoFuenteAEditar.FechaModificacion = DateTime.Now;
                                    proyectoFuenteAEditar.Eliminado = false;
                                    _context.ProyectoFuentes.Update(proyectoFuenteAEditar);
                                }
                                
                            }
                            _context.SaveChanges();
                        }
                        
                    }
                   

                    ProyectoCreado.ValorInterventoria = valorinterventoria;
                    ProyectoCreado.ValorObra = valorobra;
                    ProyectoCreado.ValorTotal = valortotal;

                    //Agregar Infraestructura  a intervenir 
                    foreach (var infraestructuraIntervenirProyecto in pProyecto.InfraestructuraIntervenirProyecto)
                    {              
                        
                        InfraestructuraIntervenirProyecto infraestructuraIntervenirProyecto1 = new InfraestructuraIntervenirProyecto
                        {
                            //InfraestructuraIntervenirProyecto Auditoria 
                            Eliminado = false,
                            UsuarioCreacion = pProyecto.UsuarioCreacion,
                            FechaCreacion = DateTime.Now,
                            //InfraestructuraIntervenirProyecto REGISTROS
                            ProyectoId = ProyectoCreado.ProyectoId,
                            InfraestructuraCodigo = infraestructuraIntervenirProyecto.InfraestructuraCodigo,
                            Cantidad = infraestructuraIntervenirProyecto.Cantidad,
                            
                        };
                        _context.InfraestructuraIntervenirProyecto.Add(infraestructuraIntervenirProyecto1);
                        _context.SaveChanges();
                    }
                }
                //Editar Proyecto  
                else
                {
                    CrearEditar = "EDITAR PROYECTO";
                    Predio predio1 = _context.Predio.Find(pProyecto.PredioPrincipal.PredioId);
                    //predio.InstitucionEducativaSedeId = pProyecto.PredioPrincipal.InstitucionEducativaSedeId;
                    int nuevopredioprinciiap = 0;
                    if(predio1!=null)
                    {
                        predio1.TipoPredioCodigo = pProyecto.PredioPrincipal.TipoPredioCodigo;
                        predio1.UbicacionLatitud = pProyecto.PredioPrincipal.UbicacionLatitud;
                        predio1.UbicacionLongitud = pProyecto.PredioPrincipal.UbicacionLongitud;
                        predio1.Direccion = pProyecto.PredioPrincipal.Direccion;
                        predio1.DocumentoAcreditacionCodigo = pProyecto.PredioPrincipal.DocumentoAcreditacionCodigo;
                        predio1.NumeroDocumento = pProyecto.PredioPrincipal.NumeroDocumento;
                        predio1.CedulaCatastral = pProyecto.PredioPrincipal.CedulaCatastral;
                        _context.Update(predio1);
                    }
                    else
                    {
                        //Predio Principal
                        Predio predioPrincipal = new Predio
                        {
                            //Predio Auditoria
                            FechaCreacion = DateTime.Now,
                            Activo = true,
                            UsuarioCreacion = pProyecto.UsuarioCreacion,
                            //Predio Registros
                            InstitucionEducativaSedeId = pProyecto.InstitucionEducativaId,
                            TipoPredioCodigo = pProyecto.TipoPredioCodigo.ToString(),
                            UbicacionLatitud = pProyecto.PredioPrincipal.UbicacionLatitud,
                            UbicacionLongitud = pProyecto.PredioPrincipal.UbicacionLongitud,
                            Direccion = pProyecto.PredioPrincipal.Direccion,
                            DocumentoAcreditacionCodigo = pProyecto.PredioPrincipal.DocumentoAcreditacionCodigo,
                            NumeroDocumento = pProyecto.PredioPrincipal.NumeroDocumento,
                            CedulaCatastral = pProyecto.PredioPrincipal.CedulaCatastral,
                        };
                        _context.Predio.Add(predioPrincipal);                        
                        _context.SaveChanges();
                        nuevopredioprinciiap = predioPrincipal.PredioId;
                    }
                    

                    //PRoyecto
                    //Proyecto 
                    Proyecto proyectoAntiguo = _context.Proyecto.Where(r => r.ProyectoId == pProyecto.ProyectoId).FirstOrDefault();

                    //Proyecto Auditoria

                    proyectoAntiguo.FechaModificacion = DateTime.Now;
                    proyectoAntiguo.UsuarioModificacion = pProyecto.UsuarioCreacion;
                    //Proyecto Registros 
                    proyectoAntiguo.NumeroActaJunta = pProyecto.NumeroActaJunta;
                    proyectoAntiguo.FechaSesionJunta = pProyecto.FechaSesionJunta;
                    proyectoAntiguo.TipoIntervencionCodigo = pProyecto.TipoIntervencionCodigo.ToString();
                    proyectoAntiguo.LlaveMen = pProyecto.LlaveMen;
                    proyectoAntiguo.LocalizacionIdMunicipio = pProyecto.LocalizacionIdMunicipio.ToString();
                    proyectoAntiguo.InstitucionEducativaId = pProyecto.InstitucionEducativaId;
                    proyectoAntiguo.SedeId = pProyecto.SedeId;
                    proyectoAntiguo.EnConvocatoria = pProyecto.EnConvocatoria;
                    proyectoAntiguo.ConvocatoriaId = pProyecto.ConvocatoriaId;
                    proyectoAntiguo.CantPrediosPostulados = pProyecto.CantPrediosPostulados;

                    proyectoAntiguo.PlazoMesesObra = pProyecto.PlazoMesesObra;
                    proyectoAntiguo.PlazoDiasObra = pProyecto.PlazoDiasObra;
                    proyectoAntiguo.PlazoMesesInterventoria = pProyecto.PlazoMesesInterventoria;
                    proyectoAntiguo.PlazoDiasInterventoria = pProyecto.PlazoDiasInterventoria;
                    proyectoAntiguo.CoordinacionResponsableCodigo = pProyecto.CoordinacionResponsableCodigo;
                    
                    if(nuevopredioprinciiap>0)
                    {
                        proyectoAntiguo.PredioPrincipalId = nuevopredioprinciiap;
                    }
                    //proyectoAntiguo.PredioPrincipalId = predio1==null?null:predio1.PredioId;
                    proyectoAntiguo.ValorObra = pProyecto.ValorObra;
                    proyectoAntiguo.ValorInterventoria = pProyecto.ValorInterventoria;
                    proyectoAntiguo.ValorTotal = pProyecto.ValorTotal;
                    proyectoAntiguo.TipoPredioCodigo = pProyecto.TipoPredioCodigo.ToString();

                    //si el tipo de intervancion es nuevo el estado juridico es sin revision 
                    if (pProyecto.TipoIntervencionCodigo.Equals(ConstantCodigoTipoIntervencion.Nuevo))
                    {
                        //jflorez, siempre aprobado, si no no sale en ningun lado
                        proyectoAntiguo.EstadoJuridicoCodigo = ConstantCodigoEstadoJuridico.Aprobado;
                    }
                    else
                    {
                        proyectoAntiguo.EstadoJuridicoCodigo = ConstantCodigoEstadoJuridico.Aprobado;
                    }

                    proyectoAntiguo.RegistroCompleto = ValidarRegistroEDITAR(pProyecto);
                    _context.Update(proyectoAntiguo);

                    //Los otros predios  
                    foreach (var predio in pProyecto.ProyectoPredio)
                    {
                        if (predio.Predio.PredioId == 0)
                        {
                            Predio predioNuevo = new Predio();

                            //Predio Auditoria
                            predioNuevo.FechaCreacion = DateTime.Now;
                            predioNuevo.Activo = true;
                            predioNuevo.UsuarioCreacion = pProyecto.UsuarioCreacion;
                            //Predio Registros
                            predioNuevo.InstitucionEducativaSedeId = pProyecto.InstitucionEducativaId;
                            predioNuevo.TipoPredioCodigo = pProyecto.TipoPredioCodigo.ToString();
                            predioNuevo.UbicacionLatitud = predio.Predio.UbicacionLatitud;
                            predioNuevo.UbicacionLongitud = predio.Predio.UbicacionLongitud;
                            predioNuevo.Direccion = predio.Predio.Direccion;
                            predioNuevo.DocumentoAcreditacionCodigo = predio.Predio.DocumentoAcreditacionCodigo;
                            predioNuevo.NumeroDocumento = predio.Predio.NumeroDocumento;
                            predioNuevo.CedulaCatastral = predio.Predio.CedulaCatastral;

                            _context.Predio.Add(predioNuevo);
                            _context.SaveChanges();
                            //Relacion Proyecto Predio 
                            ProyectoPredio proyectoPredioNuevo = new ProyectoPredio();

                            //ProyectoPredio Auditoria
                            proyectoPredioNuevo.Activo = true;
                            proyectoPredioNuevo.UsuarioCreacion = pProyecto.UsuarioCreacion;
                            proyectoPredioNuevo.FechaCreacion = DateTime.Now;
                            //ProyectoPredio Registros

                            proyectoPredioNuevo.PredioId = predioNuevo.PredioId;
                            proyectoPredioNuevo.ProyectoId = pProyecto.ProyectoId;

                            _context.ProyectoPredio.Add(proyectoPredioNuevo);
                            _context.SaveChanges();

                        }
                        else
                        {
                            Predio predioAntiguo = _context.Predio.Find(predio.Predio.PredioId);
                            //Predio Auditoria
                            predioAntiguo.FechaModificacion = DateTime.Now;
                            predioAntiguo.UsuarioModificacion = pProyecto.UsuarioCreacion;
                            //Predio Registros
                            predioAntiguo.InstitucionEducativaSedeId = pProyecto.InstitucionEducativaId;
                            predioAntiguo.TipoPredioCodigo = pProyecto.TipoPredioCodigo.ToString();
                            predioAntiguo.UbicacionLatitud = predio.Predio.UbicacionLatitud;
                            predioAntiguo.UbicacionLongitud = predio.Predio.UbicacionLongitud;
                            predioAntiguo.Direccion = predio.Predio.Direccion;
                            predioAntiguo.DocumentoAcreditacionCodigo = predio.Predio.DocumentoAcreditacionCodigo;
                            predioAntiguo.NumeroDocumento = predio.Predio.NumeroDocumento;
                            predioAntiguo.CedulaCatastral = predio.Predio.CedulaCatastral;
                            _context.Update(predioAntiguo);
                        }


                    }
                    decimal valorinterventoria = 0;
                    decimal valorobra = 0;
                    decimal valortotal = 0;
                    //Aportantes 
                    foreach (var proyectoAportante in pProyecto.ProyectoAportante)
                    {
                        if (proyectoAportante.ProyectoAportanteId == 0)
                        {
                            ProyectoAportante proyectoAportante1 = new ProyectoAportante
                            {
                                //ProyectoAportante Auditoria
                                Eliminado = false,
                                UsuarioCreacion = pProyecto.UsuarioCreacion,
                                FechaCreacion = DateTime.Now,
                                //ProyectoAportante REGISTROS
                                AportanteId = proyectoAportante.AportanteId,
                                ProyectoId = pProyecto.ProyectoId,
                                ValorObra = proyectoAportante.ValorObra,
                                ValorInterventoria = proyectoAportante.ValorInterventoria,
                                ValorTotalAportante = proyectoAportante.ValorTotalAportante,
                            };
                            valorobra += proyectoAportante.ValorObra != null ? Convert.ToDecimal(proyectoAportante.ValorObra) : 0;
                            valorinterventoria += proyectoAportante.ValorInterventoria != null ? Convert.ToDecimal(proyectoAportante.ValorInterventoria) : 0;
                            valortotal += valorobra + valorinterventoria;

                            _context.ProyectoAportante.Add(proyectoAportante1);

                            //por cada aportante relaciono las fuentes del aportante al proyecto
                            var fuentes = _context.FuenteFinanciacion.Where(x => x.AportanteId == proyectoAportante.AportanteId && !(bool)x.Eliminado).ToList();
                            foreach (var fuente in fuentes)
                            {
                                var proyectosfuenteExiste = _context.ProyectoFuentes.Where(x => x.ProyectoId == pProyecto.ProyectoId
                                      && x.FuenteId == fuente.FuenteFinanciacionId);
                                if (proyectosfuenteExiste.FirstOrDefault() == null)
                                {
                                    _context.ProyectoFuentes.Add(new ProyectoFuentes
                                    {
                                        Eliminado = false,
                                        FechaCreacion = DateTime.Now,
                                        UsuarioCreacion = pProyecto.UsuarioCreacion,
                                        FuenteId = fuente.FuenteFinanciacionId,
                                        ProyectoId = pProyecto.ProyectoId
                                    }); ;
                                }
                                else
                                {
                                    var proyectoFuenteAEditar = proyectosfuenteExiste.FirstOrDefault();
                                    proyectoFuenteAEditar.UsuarioModificacion = pProyecto.UsuarioCreacion;
                                    proyectoFuenteAEditar.FechaModificacion = DateTime.Now;
                                    proyectoFuenteAEditar.Eliminado = false;
                                    _context.ProyectoFuentes.Update(proyectoFuenteAEditar);
                                }

                            }
                            _context.SaveChanges();
                        }

                        else
                        {
                            ProyectoAportante proyectoAportanteAntiguo = _context.ProyectoAportante.Find(proyectoAportante.ProyectoAportanteId);

                            proyectoAportanteAntiguo.Eliminado = false;
                            proyectoAportanteAntiguo.UsuarioModificacion = pProyecto.UsuarioCreacion;
                            proyectoAportanteAntiguo.FechaModificacion = DateTime.Now;
                            //ProyectoAportante REGISTROS
                            proyectoAportanteAntiguo.AportanteId = proyectoAportante.AportanteId;
                            proyectoAportanteAntiguo.ProyectoId = pProyecto.ProyectoId;
                            proyectoAportanteAntiguo.ValorObra = proyectoAportante.ValorObra;
                            proyectoAportanteAntiguo.ValorInterventoria = proyectoAportante.ValorInterventoria;
                            proyectoAportanteAntiguo.ValorTotalAportante = proyectoAportante.ValorTotalAportante;
                            valorobra += proyectoAportante.ValorObra != null ? Convert.ToDecimal(proyectoAportanteAntiguo.ValorObra) : 0;
                            valorinterventoria += proyectoAportante.ValorInterventoria != null ? Convert.ToDecimal(proyectoAportanteAntiguo.ValorInterventoria) : 0;
                            valortotal += valorobra+valorinterventoria;

                            _context.Update(proyectoAportanteAntiguo);
                        }


                    }
                    proyectoAntiguo.ValorInterventoria = valorinterventoria;
                    proyectoAntiguo.ValorObra = valorobra;
                    proyectoAntiguo.ValorTotal = valortotal;


                    // Infraestructura  a intervenir 
                    foreach (var infraestructuraIntervenirProyecto in pProyecto.InfraestructuraIntervenirProyecto)
                    {

                        if (infraestructuraIntervenirProyecto.InfraestrucutraIntervenirProyectoId == 0)
                        {
                            InfraestructuraIntervenirProyecto infraestructuraIntervenirProyecto1 = new InfraestructuraIntervenirProyecto
                            {
                                //InfraestructuraIntervenirProyecto Auditoria 
                                Eliminado = false,
                                UsuarioCreacion = pProyecto.UsuarioCreacion,
                                FechaCreacion = DateTime.Now,
                                //InfraestructuraIntervenirProyecto REGISTROS
                                ProyectoId = pProyecto.ProyectoId,
                                InfraestructuraCodigo = infraestructuraIntervenirProyecto.InfraestructuraCodigo,
                                Cantidad = infraestructuraIntervenirProyecto.Cantidad,
                                
                            };
                            _context.InfraestructuraIntervenirProyecto.Add(infraestructuraIntervenirProyecto1);
                            _context.SaveChanges();
                        }
                        else
                        {

                            InfraestructuraIntervenirProyecto infraestructuraIntervenirProyecto1 = _context.InfraestructuraIntervenirProyecto.Find(infraestructuraIntervenirProyecto.InfraestrucutraIntervenirProyectoId);
                            {
                                //InfraestructuraIntervenirProyecto Auditoria 
                                infraestructuraIntervenirProyecto1.Eliminado = false;
                                infraestructuraIntervenirProyecto1.UsuarioEliminacion = pProyecto.UsuarioCreacion;
                                infraestructuraIntervenirProyecto1.FechaEliminacion = DateTime.Now;
                                //InfraestructuraIntervenirProyecto REGISTROS
                                infraestructuraIntervenirProyecto1.ProyectoId = pProyecto.ProyectoId;
                                infraestructuraIntervenirProyecto1.InfraestructuraCodigo = infraestructuraIntervenirProyecto.InfraestructuraCodigo;
                                infraestructuraIntervenirProyecto1.Cantidad = infraestructuraIntervenirProyecto.Cantidad;
                                
                            };
                            _context.Update(infraestructuraIntervenirProyecto1);
                            // _context.SaveChanges(); 
                        }

                    }
                    _context.SaveChanges();
                }

                return respuesta =
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = pProyecto,
                    Code = ConstantMessagesProyecto.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Proyecto, ConstantMessagesProyecto.OperacionExitosa, idAccionCrearProyecto, pProyecto.UsuarioCreacion, CrearEditar)
                };

            }
            catch (Exception ex)
            {
                return respuesta =
                   new Respuesta
                   {
                       IsSuccessful = false,
                       IsException = false,
                       IsValidation = true,
                       Code = ConstantMessagesProyecto.Error,
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Proyecto, ConstantMessagesProyecto.Error, idAccionCrearProyecto, pProyecto.UsuarioCreacion, ex.InnerException.ToString())
                   };
            }
        }

        public async Task<Respuesta> SetValidateCargueMasivo(IFormFile pFile, string pFilePatch, string pUsuarioCreo)
        {
            int CantidadRegistrosVacios = 0;
            int CantidadResgistrosValidos = 0;
            int CantidadRegistrosInvalidos = 0;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //int OrigenId = await _commonService.GetDominioIdByCodigoAndTipoDominio(OrigenArchivoCargue.Proyecto, (int)EnumeratorTipoDominio.Origen_Documento_Cargue);

            ArchivoCargue archivoCarge = await _documentService.getSaveFile(pFile, pFilePatch, Int32.Parse(OrigenArchivoCargue.Proyecto), 0);

            // if (!string.IsNullOrEmpty(archivoCarge.ArchivoCargueId.ToString()))
            if (archivoCarge != null)
            {
                using var stream = new MemoryStream();
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
                         2	3	4	5	6	7	8	10	11	12	13	14 28 29 30 31 32		
                        Campos Obligatorios Validos   */
                        if (
                            !string.IsNullOrEmpty(worksheet.Cells[i, 2].Text) |
                            !string.IsNullOrEmpty(worksheet.Cells[i, 3].Text) |
                            !string.IsNullOrEmpty(worksheet.Cells[i, 4].Text) |
                            !string.IsNullOrEmpty(worksheet.Cells[i, 5].Text) |
                            !string.IsNullOrEmpty(worksheet.Cells[i, 6].Text) |
                            !string.IsNullOrEmpty(worksheet.Cells[i, 7].Text) |
                            !string.IsNullOrEmpty(worksheet.Cells[i, 8].Text) |
                            !string.IsNullOrEmpty(worksheet.Cells[i, 10].Text) |
                            !string.IsNullOrEmpty(worksheet.Cells[i, 12].Text) |
                            !string.IsNullOrEmpty(worksheet.Cells[i, 13].Text) |
                            !string.IsNullOrEmpty(worksheet.Cells[i, 14].Text) |
                            !string.IsNullOrEmpty(worksheet.Cells[i, 28].Text) |
                            !string.IsNullOrEmpty(worksheet.Cells[i, 29].Text) |
                            !string.IsNullOrEmpty(worksheet.Cells[i, 30].Text) |
                            !string.IsNullOrEmpty(worksheet.Cells[i, 31].Text) |
                            !string.IsNullOrEmpty(worksheet.Cells[i, 32].Text)
                            )
                        {

                            TemporalProyecto temporalProyecto = new TemporalProyecto
                            {
                                //Auditoria
                                ArchivoCargueId = archivoCarge.ArchivoCargueId,
                                EstaValidado = false,
                                FechaCreacion = DateTime.Now,
                                UsuarioCreacion = pUsuarioCreo
                            };

                            // #1
                            //Fecha sesion Junta 
                            string strDateTime = (worksheet.Cells[i, 1].Text).ToString();
                            if (!string.IsNullOrEmpty(strDateTime))
                            {
                                temporalProyecto.FechaSesionJunta = DateTime.Parse(strDateTime);
                            }

                            //#2
                            //Número de acta de la junta 
                            int intNumeroActaJunta = Int32.Parse(worksheet.Cells[i, 2].Text);
                            temporalProyecto.NumeroActaJunta = intNumeroActaJunta;

                            //#3
                            // Tipo de Intervención 
                            temporalProyecto.TipoIntervencionId = Int32.Parse(await _commonService.GetDominioCodigoByNombreDominioAndTipoDominio(worksheet.Cells[i, 3].Text, (int)EnumeratorTipoDominio.Tipo_de_Intervencion));

                            //#4
                            // Llave MEN  
                            temporalProyecto.LlaveMen = worksheet.Cells[i, 4].Text;

                            //#5
                            //Región

                            //#6
                            //Departamento
                            temporalProyecto.Departamento = await _commonService.GetLocalizacionIdByName(worksheet.Cells[i, 6].Text, "0");

                            //#7
                            //Municipio ///aqui debe recibir el parametro iddepartamento, pueden haber municipios del mismo nombre para diferente departamento
                            temporalProyecto.Municipio = await _commonService.GetLocalizacionIdByName(worksheet.Cells[i, 7].Text, temporalProyecto.Departamento.ToString());


                            //#8
                            //Institución Educativa 
                            //Validar si existe institucion educativa y guardar id el codigo dane es mejor identificador de id, es único, así el Excel es menos complejo con la lista
                            //Volver a dejar como estaba buscando con nombre
                            int idInstitucionEducativaSede = await _commonService.getInstitucionEducativaIdByName((worksheet.Cells[i, 8].Text));
                            if (idInstitucionEducativaSede > 0)
                            {
                                temporalProyecto.InstitucionEducativaId = idInstitucionEducativaSede;
                            }
                            else
                            {
                                archivoCarge.CantidadRegistrosInvalidos++;
                                break;
                            }

                            //#9
                            //Código DANE IE 
                            //     temporalProyecto.CodigoDaneIe = Int32.TryParse(worksheet.Cells[i, 9].Text, out 1);

                            //#10
                            //Código DANE IE  
                            //Validar si existe la sede y poner id si no crear sede y poner id  el codigo dane es mejor identificador de id, es único, así el Excel es menos complejo con la lista
                            //Volver a dejar como estaba buscando con nombre
                            int SedeId = await _commonService.getSedeInstitucionEducativaIdByNameAndInstitucionPadre(worksheet.Cells[i, 10].Text, idInstitucionEducativaSede);
                            if (SedeId > 0)
                            { temporalProyecto.SedeId = SedeId; }
                            else
                            {
                                CantidadRegistrosInvalidos++;
                                break;
                            }

                            //#11
                            //Código DANE SEDE 
                            //          temporalProyecto.CodigoDaneSede = Int32.Parse(worksheet.Cells[i, 11].Text);

                            //#12
                            //¿Se encuentra dentro de una convocatoria? 
                            if ((worksheet.Cells[i, 12].Text).ToString().ToUpper().Contains("SI") || Int32.Parse(worksheet.Cells[i, 12].Text).ToString().ToUpper().Contains("VERDADERO"))
                            { temporalProyecto.EnConvotatoria = true; }
                            else { temporalProyecto.EnConvotatoria = false; };

                            //#13
                            //Convocatoria
                            temporalProyecto.ConvocatoriaId = Int32.Parse(await _commonService.GetDominioCodigoByNombreDominioAndTipoDominio(worksheet.Cells[i, 13].Text, (int)EnumeratorTipoDominio.Convocatoria));

                            //#14
                            //Número de predios postulados
                            temporalProyecto.CantPrediosPostulados = Int32.Parse(worksheet.Cells[i, 14].Text);

                            //#15
                            //Tipo de predio(s) 
                            temporalProyecto.TipoPredioId = Int32.Parse(await _commonService.GetDominioCodigoByNombreDominioAndTipoDominio(worksheet.Cells[i, 15].Text, (int)EnumeratorTipoDominio.Tipo_de_Predios));

                            //#16
                            //Ubicación del predio principal latitud
                            temporalProyecto.UbicacionPredioPrincipalLatitud = worksheet.Cells[i, 16].Text;

                            //#17
                            //Ubicación del predio principal longitud
                            temporalProyecto.UbicacionPredioPrincipalLontitud = worksheet.Cells[i, 17].Text;

                            //#18
                            //Dirección del predio principal 
                            temporalProyecto.DireccionPredioPrincipal = worksheet.Cells[i, 18].Text;

                            //#19
                            //Documento de acreditación del predio 
                            temporalProyecto.DocumentoAcreditacionPredioId = Int32.Parse(await _commonService.GetDominioCodigoByNombreDominioAndTipoDominio(worksheet.Cells[i, 19].Text, (int)EnumeratorTipoDominio.Documento_Acreditacion));

                            //#20
                            //Número del documento de acreditación 
                            temporalProyecto.NumeroDocumentoAcreditacion = worksheet.Cells[i, 20].Text;

                            //#21
                            //Cédula Catastral del predio 
                            temporalProyecto.CedulaCatastralPredio = worksheet.Cells[i, 21].Text;

                            //#22
                            //Tipo de aportante 1 
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 22].Text))
                            {
                                temporalProyecto.TipoAportanteId1 = Int32.Parse(await _commonService.GetDominioCodigoByNombreDominioAndTipoDominio(worksheet.Cells[i, 22].Text, (int)EnumeratorTipoDominio.Tipo_de_aportante));

                                //#23
                                //Aportante 1 
                                temporalProyecto.Aportante1 = Int32.Parse(await _commonService.GetDominioCodigoByNombreDominioAndTipoDominio(worksheet.Cells[i, 23].Text, (int)EnumeratorTipoDominio.Nombre_Aportante_Aportante));
                            }
                            //#24
                            //Tipo de aportante 2
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 24].Text))
                            {
                                temporalProyecto.TipoAportanteId2 = Int32.Parse(await _commonService.GetDominioCodigoByNombreDominioAndTipoDominio(worksheet.Cells[i, 24].Text, (int)EnumeratorTipoDominio.Tipo_de_aportante));

                                //#25
                                //Aportante 2
                                temporalProyecto.Aportante2 = Int32.Parse(await _commonService.GetDominioCodigoByNombreDominioAndTipoDominio(worksheet.Cells[i, 25].Text, (int)EnumeratorTipoDominio.Nombre_Aportante_Aportante));
                            }
                            //#26
                            //Tipo de aportante 3
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 26].Text))
                            {
                                temporalProyecto.TipoAportanteId3 = Int32.Parse(await _commonService.GetDominioCodigoByNombreDominioAndTipoDominio(worksheet.Cells[i, 26].Text, (int)EnumeratorTipoDominio.Tipo_de_aportante));

                                //#27
                                //Aportante 3
                                temporalProyecto.Aportante3 = Int32.Parse(await _commonService.GetDominioCodigoByNombreDominioAndTipoDominio(worksheet.Cells[i, 27].Text, (int)EnumeratorTipoDominio.Nombre_Aportante_Aportante));
                            }
                            //#28
                            //Vigencia del acuerdo de cofinanciación 
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 28].Text))
                            {
                                temporalProyecto.VigenciaAcuerdoCofinanciacion = Int32.Parse(worksheet.Cells[i, 28].Text);
                            }

                            //#29
                            //Valor obra 
                            string onlyNumber = worksheet.Cells[i, 29].Text.Replace("$", "");
                            temporalProyecto.ValorObra = decimal.Parse(worksheet.Cells[i, 29].Text.Replace("$", ""));

                            //#30
                            //Valor interventoría 
                            temporalProyecto.ValorInterventoria = decimal.Parse(worksheet.Cells[i, 30].Text.Replace("$", ""));

                            //#31
                            //Valor Total 
                            temporalProyecto.ValorTotal = decimal.Parse(worksheet.Cells[i, 31].Text.Replace("$", ""));

                            //#32
                            //Infraestructura para intervenir 
                            temporalProyecto.EspacioIntervenirId = Int32.Parse(await _commonService.GetDominioCodigoByNombreDominioAndTipoDominio(worksheet.Cells[i, 32].Text, (int)EnumeratorTipoDominio.Espacios_Intervenir));

                            //#33
                            //Cantidad 
                            temporalProyecto.Cantidad = (Int32.Parse(worksheet.Cells[i, 33].Text));

                            //#34
                            //Plazo en meses Obra 
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 34].Text))
                            {
                                temporalProyecto.PlazoMesesObra = (Int32.Parse(worksheet.Cells[i, 34].Text));
                            }
                            //#35
                            //Plazo en días Obra 
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 35].Text))
                            {
                                temporalProyecto.PlazoDiasObra = (Int32.Parse(worksheet.Cells[i, 35].Text));
                            }
                            //#36
                            //Plazo en meses Interventoría 
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 36].Text))
                            {
                                temporalProyecto.PlazoMesesInterventoria = (Int32.Parse(worksheet.Cells[i, 36].Text));
                            }
                            //#37
                            //Plazo en meses Interventoría 
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 37].Text))
                            {
                                temporalProyecto.PlazoDiasInterventoria = (Int32.Parse(worksheet.Cells[i, 37].Text));
                            }
                            //#38
                            //Coordinación responsable 
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 38].Text))
                            {
                                temporalProyecto.CoordinacionResponsableId = Int32.Parse(await _commonService.GetDominioCodigoByNombreDominioAndTipoDominio(worksheet.Cells[i, 38].Text, (int)EnumeratorTipoDominio.Coordinaciones));
                            }

                            //Guarda Cambios en una tabla temporal

                            _context.TemporalProyecto.Add(temporalProyecto);
                            _context.SaveChanges();

                            if (temporalProyecto.TemporalProyectoId > 0)
                            {
                                CantidadResgistrosValidos++;
                            }
                            else
                            {
                                CantidadRegistrosInvalidos++;
                                worksheet.Cells[i, 1].Value = "Estructura invalida";
                            }
                        }
                        else
                        {
                            //Aqui entra cuando alguno de los campos obligatorios no viene diligenciado
                            string strValidateCampNullsOrEmpty = "";
                            //Valida que todos los campos esten vacios porque las validaciones del excel hacen que lea todos los rows como ingresado información 

                            for (int j = 1; j < 37; j++)
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
                                worksheet.Cells[i, 1].Value = "Campos vacios";
                            }
                        }

                    }
                    catch (Exception)
                    {
                        CantidadRegistrosInvalidos++;
                    }
                }


                //como ya quedo en temporal, voy a consultarla y generar el archvio revisado
                var streams = new MemoryStream();

                using (var packages = new ExcelPackage(streams))
                {
                    var workSheet = packages.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(_context.TemporalProyecto.Where(x => x.ArchivoCargueId == archivoCarge.ArchivoCargueId).ToList(), true);
                    packages.Save();
                    //convert the excel package to a byte array
                    byte[] bin = packages.GetAsByteArray();

                    //the path of the file
                    string filePath = pFilePatch + "/" + archivoCarge.Nombre + "_rev.xlsx";

                    //write the file to the disk
                    File.WriteAllBytes(filePath, bin);
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
                    Code = ConstantMessagesCargueProyecto.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoProyecto, ConstantMessagesCargueProyecto.OperacionExitosa, (int)enumeratorAccion.ValidarExcel, pUsuarioCreo, "")
                };
            }
            else
            {
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesCargueProyecto.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoProyecto, ConstantMessagesCargueProyecto.Error, (int)enumeratorAccion.ValidarExcel, pUsuarioCreo, "")
                };
            }
        }

        public async Task<Respuesta> UploadMassiveLoadProjects(string pIdDocument, string pUsuarioModifico)
        {
            Respuesta respuesta = new Respuesta();

            if (string.IsNullOrEmpty(pIdDocument))
            {
                return respuesta =
                 new Respuesta
                 {
                     IsSuccessful = false,
                     IsException = false,
                     IsValidation = true,
                     Code = ConstantMessagesCargueProyecto.CamposVacios,
                     Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoProyecto, ConstantMessagesCargueProyecto.CamposVacios, (int)enumeratorAccion.CargueProyectosMasivos, pUsuarioModifico, "")
                 };
            }
            try
            {


                int OrigenId = await _commonService.GetDominioIdByCodigoAndTipoDominio(OrigenArchivoCargue.Proyecto, (int)EnumeratorTipoDominio.Origen_Documento_Cargue);

                ArchivoCargue archivoCargue = _context.ArchivoCargue.Where(r => r.OrigenId == 1 && r.Nombre.Trim().ToUpper().Equals(pIdDocument.ToUpper().Trim())).FirstOrDefault();

                List<TemporalProyecto> ListTemporalProyecto = await _context.TemporalProyecto.Where(r => r.ArchivoCargueId == archivoCargue.ArchivoCargueId && !(bool)r.EstaValidado).ToListAsync();

                if (ListTemporalProyecto.Count() > 0)
                {
                    foreach (var temporalProyecto in ListTemporalProyecto)
                    {

                        //Predio 
                        //Predio Auditoria
                        Predio predio = new Predio
                        {
                            FechaCreacion = DateTime.Now,
                            Activo = true,
                            UsuarioCreacion = temporalProyecto.UsuarioCreacion,

                            //Predio Registros
                            InstitucionEducativaSedeId = temporalProyecto.InstitucionEducativaId,
                            TipoPredioCodigo = temporalProyecto.TipoPredioId.ToString(),
                            UbicacionLatitud = temporalProyecto.UbicacionPredioPrincipalLatitud,
                            UbicacionLongitud = temporalProyecto.UbicacionPredioPrincipalLontitud,
                            Direccion = temporalProyecto.DireccionPredioPrincipal,
                            DocumentoAcreditacionCodigo = temporalProyecto.DocumentoAcreditacionPredioId.ToString(),
                            NumeroDocumento = temporalProyecto.NumeroDocumentoAcreditacion,
                            CedulaCatastral = temporalProyecto.CedulaCatastralPredio
                        };
                        //
                        _context.Predio.Add(predio);
                        _context.SaveChanges();

                        //Proyecto

                        Proyecto proyecto = new Proyecto
                        {
                            //Proyecto Auditoria
                            Eliminado = false,
                            FechaCreacion = DateTime.Now,
                            UsuarioCreacion = temporalProyecto.UsuarioCreacion,
                            //Proyecto Registros 
                            FechaSesionJunta = temporalProyecto.FechaSesionJunta,
                            TipoIntervencionCodigo = temporalProyecto.TipoIntervencionId.ToString(),
                            LlaveMen = temporalProyecto.LlaveMen,
                            LocalizacionIdMunicipio = temporalProyecto.Municipio.ToString(),
                            InstitucionEducativaId = temporalProyecto.InstitucionEducativaId,
                            SedeId = temporalProyecto.SedeId,
                            EnConvocatoria = temporalProyecto.EnConvotatoria,
                            ConvocatoriaId = temporalProyecto.ConvocatoriaId,
                            CantPrediosPostulados = temporalProyecto.CantPrediosPostulados,
                            PredioPrincipalId = predio.PredioId,
                            ValorObra = temporalProyecto.ValorObra,
                            ValorInterventoria = temporalProyecto.ValorInterventoria,
                            ValorTotal = temporalProyecto.ValorTotal,
                            TipoPredioCodigo = temporalProyecto.EspacioIntervenirId.ToString()
                        };


                        //si el tipo de intervancion es nuevo el estado juridico es sin revision 
                        if (proyecto.TipoIntervencionCodigo.Equals(ConstantCodigoTipoIntervencion.Nuevo))
                        {
                            //jflorez, simpre aprobado o si no no sale en ningun lado
                            proyecto.EstadoJuridicoCodigo = ConstantCodigoEstadoJuridico.Aprobado;
                        }
                        else
                        {
                            proyecto.EstadoJuridicoCodigo = ConstantCodigoEstadoJuridico.Aprobado;
                        }

                        proyecto.RegistroCompleto = ValidarRegistroEDITAR(proyecto);
                        _context.Proyecto.Add(proyecto);
                        _context.SaveChanges();

                        //ProyectoPredio

                        ProyectoPredio proyectoPredio = new ProyectoPredio
                        {
                            //Proyecto  Auditoria
                            FechaCreacion = DateTime.Now,
                            UsuarioCreacion = temporalProyecto.UsuarioCreacion,
                            Activo = true,

                            //Registros
                            //proyectoPredio.EstadoJuridicoCodigo = "";
                            ProyectoId = proyecto.ProyectoId,
                            PredioId = predio.PredioId
                        };
                        //
                        _context.ProyectoPredio.Add(proyectoPredio);
                        _context.SaveChanges();

                        //Relacionar Ids


                        //Cofinanciacion
                        Cofinanciacion cofinanciacion = new Cofinanciacion
                        {
                            Eliminado = false,
                            FechaCreacion = DateTime.Now,
                            VigenciaCofinanciacionId = temporalProyecto.VigenciaAcuerdoCofinanciacion,
                            UsuarioCreacion = temporalProyecto.UsuarioCreacion
                        };
                        //
                        _context.Cofinanciacion.Add(cofinanciacion);
                        _context.SaveChanges();

                        //CofinanciacionAportante 1 
                        if (!string.IsNullOrEmpty(temporalProyecto.TipoAportanteId1.ToString()))
                        {

                            CofinanciacionAportante cofinanciacionAportante1 = new CofinanciacionAportante
                            {
                                //Auditoria
                                FechaCreacion = DateTime.Now,
                                Eliminado = false,
                                //Registros
                                UsuarioCreacion = temporalProyecto.UsuarioCreacion,
                                CofinanciacionId = cofinanciacion.CofinanciacionId,
                                TipoAportanteId = (int)temporalProyecto.TipoAportanteId1,
                                NombreAportanteId = (int)temporalProyecto.Aportante1
                            };
                            //
                            _context.CofinanciacionAportante.Add(cofinanciacionAportante1);
                            _context.SaveChanges();
                            //ProyectoAportante

                            ProyectoAportante proyectoAportante = new ProyectoAportante
                            {
                                //Auditoria
                                FechaCreacion = DateTime.Now,
                                UsuarioCreacion = temporalProyecto.UsuarioCreacion,
                                Eliminado = false,

                                //Registros
                                ProyectoId = proyecto.ProyectoId,
                                AportanteId = cofinanciacionAportante1.CofinanciacionAportanteId
                            };

                            //
                            _context.ProyectoAportante.Add(proyectoAportante);
                            _context.SaveChanges();
                            // proyectoAportante.
                        }
                        //CofinanciacionAportante 2
                        if (!string.IsNullOrEmpty(temporalProyecto.TipoAportanteId2.ToString()))
                        {
                            //Auditoria
                            CofinanciacionAportante cofinanciacionAportante2 = new CofinanciacionAportante
                            {
                                FechaCreacion = DateTime.Now,
                                Eliminado = false,
                                //Registros 
                                UsuarioCreacion = temporalProyecto.UsuarioCreacion,
                                CofinanciacionId = cofinanciacion.CofinanciacionId,
                                TipoAportanteId = (int)temporalProyecto.TipoAportanteId2,
                                NombreAportanteId = (int)temporalProyecto.Aportante2
                            };
                            //
                            _context.CofinanciacionAportante.Add(cofinanciacionAportante2);
                            _context.SaveChanges();
                            //ProyectoAportante
                            //Auditoria
                            ProyectoAportante proyectoAportante = new ProyectoAportante
                            {
                                FechaCreacion = DateTime.Now,
                                UsuarioCreacion = temporalProyecto.UsuarioCreacion,
                                Eliminado = false,

                                //Registros
                                ProyectoId = proyecto.ProyectoId,
                                AportanteId = cofinanciacionAportante2.CofinanciacionAportanteId
                            };

                            //
                            _context.ProyectoAportante.Add(proyectoAportante);
                            _context.SaveChanges();
                        }
                        //CofinanciacionAportante 3
                        if (!string.IsNullOrEmpty(temporalProyecto.TipoAportanteId2.ToString()))
                        {
                            //Auditoria
                            CofinanciacionAportante cofinanciacionAportante3 = new CofinanciacionAportante
                            {

                                FechaCreacion = DateTime.Now,
                                Eliminado = false,
                                //Registros
                                UsuarioCreacion = temporalProyecto.UsuarioCreacion,
                                CofinanciacionId = cofinanciacion.CofinanciacionId,
                                TipoAportanteId = (int)temporalProyecto.TipoAportanteId3,
                                NombreAportanteId = (int)temporalProyecto.Aportante3
                            };
                            //
                            _context.CofinanciacionAportante.Add(cofinanciacionAportante3);
                            _context.SaveChanges();
                            //ProyectoAportante
                            //Auditoria
                            ProyectoAportante proyectoAportante = new ProyectoAportante
                            {
                                FechaCreacion = DateTime.Now,
                                UsuarioCreacion = temporalProyecto.UsuarioCreacion,
                                Eliminado = false,

                                //Registros
                                ProyectoId = proyecto.ProyectoId,
                                AportanteId = cofinanciacionAportante3.CofinanciacionAportanteId
                            };

                            //
                            _context.ProyectoAportante.Add(proyectoAportante);
                            _context.SaveChanges();
                        }

                        //Temporal proyecto update
                        temporalProyecto.EstaValidado = true;
                        temporalProyecto.FechaModificacion = DateTime.Now;
                        temporalProyecto.UsuarioModificacion = pUsuarioModifico;
                        _context.TemporalProyecto.Update(temporalProyecto);
                        _context.SaveChanges();
                    }


                    return respuesta =
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = true,
                        Code = ConstantMessagesCargueProyecto.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoProyecto, ConstantMessagesCargueProyecto.OperacionExitosa, (int)enumeratorAccion.CargueProyectosMasivos, pUsuarioModifico, "Cantidad de Proyectos subidos : " + ListTemporalProyecto.Count())
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
                            Code = ConstantMessagesCargueProyecto.NoExitenArchivos,
                            Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoProyecto, ConstantMessagesCargueProyecto.NoExitenArchivos, (int)enumeratorAccion.CargueProyectosMasivos, pUsuarioModifico, "")
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
                        Code = ConstantMessagesCargueProyecto.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoProyecto, ConstantMessagesCargueProyecto.Error, (int)enumeratorAccion.CargueProyectosMasivos, pUsuarioModifico, ex.InnerException.ToString())
                    };
            }
        }

        public async Task<Proyecto> GetProyectoByProyectoId(int idProyecto)
        {
            Proyecto proyecto = await _context.Proyecto.Where(r => r.ProyectoId == idProyecto)
                                                        .Include(y => y.InstitucionEducativa)
                                                        .Include(y => y.Sede)
                                                        .Include(y => y.LocalizacionIdMunicipioNavigation)
                                                        .Include(y => y.ContratacionProyecto)
                                                             .ThenInclude(r => r.Contratacion)
                                                        .FirstOrDefaultAsync();

            proyecto.ProyectoAportante = _context.ProyectoAportante.Where(x => x.ProyectoId == proyecto.ProyectoId && x.Eliminado == false)
                                                                    .Include(y => y.Aportante)
                                                                       .ThenInclude(r => r.NombreAportante)
                                                                    .Include(y => y.Aportante)
                                                                       .ThenInclude(r => r.Departamento)
                                                                    .Include(y => y.Aportante)
                                                                       .ThenInclude(r => r.Municipio)
                                                                    .Include(z => z.CofinanciacionDocumento)
                                                                    .ToList();

            if (proyecto.ContratacionProyecto.Count() > 0)
            {
                SesionComiteSolicitud sesionComiteSolicitud = _context.SesionComiteSolicitud
                    .Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion && r.SolicitudId == proyecto.ContratacionProyecto.FirstOrDefault().ContratacionId)
                    .Include(r => r.ComiteTecnico).FirstOrDefault();
                if(sesionComiteSolicitud != null)
                     proyecto.FechaComite = sesionComiteSolicitud.ComiteTecnico.FechaOrdenDia;
                Localizacion municipio = _context.Localizacion.Where(r => r.LocalizacionId == proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                proyecto.Municipio = municipio.Descripcion;
                proyecto.Departamento = _context.Localizacion.Where(r => r.LocalizacionId == municipio.IdPadre).FirstOrDefault().Descripcion;
                proyecto.NumeroSolicitud = proyecto.ContratacionProyecto.FirstOrDefault().Contratacion.NumeroSolicitud; 
            }

            foreach (var ProyectoAportante in proyecto.ProyectoAportante)
            {

                if (ProyectoAportante.Aportante.Municipio != null)
                {
                    ProyectoAportante.NombreAportante = ProyectoAportante.Aportante.Municipio.Descripcion;
                    ProyectoAportante.TipoAportanteNombre = ConstanStringTipoAportanteNombre.Municipio;
                }


                if (ProyectoAportante.Aportante.Departamento != null && ProyectoAportante.Aportante.Departamento == null)
                {
                    ProyectoAportante.NombreAportante = ProyectoAportante.Aportante.Departamento.Descripcion;
                    ProyectoAportante.TipoAportanteNombre = ConstanStringTipoAportanteNombre.Departamento;
                }
                /*
                 jflorez, ajussto esto, si tiene nombre de aportante realmente es un tercero, pero lo valido sería tener en cuenta el tipo de aportante
                 if (ProyectoAportante.Aportante.NombreAportante != null)
                {
                    ProyectoAportante.NombreAportante = ProyectoAportante.Aportante.NombreAportante.Nombre;
                    ProyectoAportante.TipoAportanteNombre = ConstanStringTipoAportanteNombre.Ffie;
                }*/
                if(ProyectoAportante.Aportante.TipoAportanteId==ConstanTipoAportante.Ffie)
                {
                    ProyectoAportante.NombreAportante = ConstanStringTipoAportante.Ffie;
                    ProyectoAportante.TipoAportanteNombre = ConstanStringTipoAportanteNombre.Ffie;
                }
                else if(ProyectoAportante.Aportante.TipoAportanteId==ConstanTipoAportante.Tercero)
                {
                    ProyectoAportante.NombreAportante = ProyectoAportante.Aportante.NombreAportante.Nombre;
                    ProyectoAportante.TipoAportanteNombre = ConstanStringTipoAportanteNombre.Tercero;
                }
                //el else son las entidades territoriales que ya se tuvieron en cuenta con el municipio y depto id
                

            }

            proyecto.PredioPrincipal = _context.Predio.Where(x => x.PredioId == proyecto.PredioPrincipalId && x.Activo == true).FirstOrDefault();
            List<InfraestructuraIntervenirProyecto> infraestructuras = _context.InfraestructuraIntervenirProyecto.Where(x => x.ProyectoId == proyecto.ProyectoId && x.Eliminado == false).ToList();
            foreach (var infraestructura in infraestructuras)
            {
                infraestructura.Proyecto = null;
            }
            proyecto.InfraestructuraIntervenirProyecto = infraestructuras;
            proyecto.ProyectoPredio = _context.ProyectoPredio.Where(x => x.Activo == true && x.ProyectoId == proyecto.ProyectoId).Include(x => x.Predio).ToList();
            //proyecto.PredioPrincipal = _context.Predio.Where(x => x.PredioId == proyecto.PredioPrincipalId && x.Activo == true).ToList();
            return proyecto;
        }

        public async Task<bool> DeleteProyectoByProyectoId(int idProyecto, string usuario)
        {
            Proyecto proyecto = await _context.Proyecto.FindAsync(idProyecto);
            bool retorno = true;
            if (_context.ContratacionProyecto.Where(x => x.ProyectoId == idProyecto && !(bool)x.Eliminado).Count() > 0 || _context.DisponibilidadPresupuestalProyecto.Where(x => x.ProyectoId == idProyecto && !(bool)x.Eliminado).Count() > 0)
            {
                return false;
            }
            else
            {
                try
                {
                    proyecto.Eliminado = true;
                    proyecto.UsuarioModificacion = usuario;
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return retorno;
        }

        public async Task<Respuesta> CreateOrEditAdministrativeProject(ProyectoAdministrativo pProyectoAdministrativo)
        {

            int idAccionCrearProyectoAdministrativo = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Proyecto_Administrativo, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                //Crear Proyecto Administrativo 
                //Es nuevo 
                if (pProyectoAdministrativo.ProyectoAdministrativoId == 0)
            {                //Auditoria
                pProyectoAdministrativo.Eliminado = false;
                pProyectoAdministrativo.Enviado = false;

                bool boleanEstaConFuente = true;
                foreach(var aportp in pProyectoAdministrativo.ProyectoAdministrativoAportante)
                {
                    foreach (var fuente in aportp.AportanteFuenteFinanciacion)
                    {
                        if (fuente.FuenteFinanciacionId==null || fuente.ValorFuente ==null || fuente.ValorFuente==0)
                        {
                            boleanEstaConFuente = false;
                        }
                    }
                }
                pProyectoAdministrativo.RegistroCompleto = pProyectoAdministrativo.ProyectoAdministrativoAportante.Count() > 0 && boleanEstaConFuente;

                pProyectoAdministrativo.FechaCreado = DateTime.Now;
                //pProyectoAdministrativo.RegistroCompleto = ValidarRegistroCompletoProyectoAdministrativo(pProyectoAdministrativo);
                //Como es nuevo creo la relacion en la tabla  
                //Proyecto Administrativo aportante
                foreach (var ProyectoAdministrativo in pProyectoAdministrativo.ProyectoAdministrativoAportante)
                {
                    ProyectoAdministrativo.Eliminado = false;
                    ProyectoAdministrativo.FechaCreacion = DateTime.Now;
                    ProyectoAdministrativo.UsuarioCreacion = pProyectoAdministrativo.UsuarioCreacion;
                    //ProyectoAdministrativo.ProyectoAdminstrativoId = ProyectoAdministrativo.ProyectoAdminstrativoId;
                    //_context.ProyectoAdministrativoAportante.Add(proyectoAdministrativoAportante);
                    //Guarda relacion
                    //_context.SaveChanges();

                    //Como la relacion es de 3 niveles dentro de este esta el otro nivel fuentes de financiación
                    foreach (var FuenteFinanciacion in ProyectoAdministrativo.AportanteFuenteFinanciacion)
                    {
                        FuenteFinanciacion.UsuarioCreacion = pProyectoAdministrativo.UsuarioCreacion;
                        FuenteFinanciacion.FechaCreacion = DateTime.Now;
                        FuenteFinanciacion.Eliminado = false;
                        FuenteFinanciacion.ProyectoAdministrativoAportanteId = ProyectoAdministrativo.ProyectoAdministrativoAportanteId;
                        FuenteFinanciacion.FuenteFinanciacionId = FuenteFinanciacion.FuenteFinanciacionId == 0 ? null : FuenteFinanciacion.FuenteFinanciacionId;
                        //_context.AportanteFuenteFinanciacion.Add(FuenteFinanciacion);
                        //Guarda relacion
                        //_context.SaveChanges();
                    }
                }
                _context.ProyectoAdministrativo.Add(pProyectoAdministrativo);
                _context.SaveChanges();
            }
            //Editar Proyecto Administrativo
            else
            {
                //Auditoria
                ProyectoAdministrativo proyectoAdministrativoAntiguo = _context.ProyectoAdministrativo.Find(pProyectoAdministrativo.ProyectoAdministrativoId);
                proyectoAdministrativoAntiguo.FechaModificacion = DateTime.Now;
                proyectoAdministrativoAntiguo.UsuarioModificacion = pProyectoAdministrativo.UsuarioCreacion;
                //Cambio Campos
                proyectoAdministrativoAntiguo.Eliminado = pProyectoAdministrativo.Eliminado;
                proyectoAdministrativoAntiguo.Enviado = pProyectoAdministrativo.Enviado;
                foreach (var varProyectoAdministrativo in pProyectoAdministrativo.ProyectoAdministrativoAportante)
                {
                    varProyectoAdministrativo.FechaEdicion = DateTime.Now;
                    varProyectoAdministrativo.UsuarioEdicion = pProyectoAdministrativo.UsuarioCreacion;
                    
                    //Guarda relacion
                    //_context.SaveChanges();

                    //Como la relacion es de 3 niveles dentro de este esta el otro nivel fuentes de financiación
                    foreach (var varFuenteFinanciacion in varProyectoAdministrativo.AportanteFuenteFinanciacion)
                    {
                        if(varFuenteFinanciacion.FuenteFinanciacionId>0)
                        {
                            varFuenteFinanciacion.UsuarioEdicion = pProyectoAdministrativo.UsuarioCreacion;
                            varFuenteFinanciacion.FechaEdicion = DateTime.Now;
                            varFuenteFinanciacion.Eliminado = false;
                            varFuenteFinanciacion.ProyectoAdministrativoAportanteId = varProyectoAdministrativo.ProyectoAdministrativoAportanteId;
                            _context.AportanteFuenteFinanciacion.Update(varFuenteFinanciacion);
                        }
                        else
                        {
                            varFuenteFinanciacion.UsuarioCreacion = pProyectoAdministrativo.UsuarioCreacion;
                            varFuenteFinanciacion.FechaCreacion = DateTime.Now;
                            varFuenteFinanciacion.Eliminado = false;
                            varFuenteFinanciacion.ProyectoAdministrativoAportanteId = varProyectoAdministrativo.ProyectoAdministrativoAportanteId;
                            //_context.AportanteFuenteFinanciacion.Add(varFuenteFinanciacion);
                        }

                        //_context.AportanteFuenteFinanciacion.Add(FuenteFinanciacion);
                        //Guarda relacion
                        //_context.SaveChanges();
                    }
                        bool boleanEstaConFuente = true;
                        foreach (var aportp in pProyectoAdministrativo.ProyectoAdministrativoAportante)
                        {
                            foreach (var fuente in aportp.AportanteFuenteFinanciacion)
                            {
                                if (fuente.FuenteFinanciacionId == null || fuente.ValorFuente == null || fuente.ValorFuente == 0)
                                {
                                    boleanEstaConFuente = false;
                                }
                            }
                        }
                        //varProyectoAdministrativo.RegistroCompleto = pProyectoAdministrativo.ProyectoAdministrativoAportante.Count() > 0 && boleanEstaConFuente;
                        proyectoAdministrativoAntiguo.RegistroCompleto = pProyectoAdministrativo.ProyectoAdministrativoAportante.Count() > 0 && boleanEstaConFuente;
                        _context.ProyectoAdministrativoAportante.Update(varProyectoAdministrativo);
                }

                _context.Update(proyectoAdministrativoAntiguo);
            }


                return
                   new Respuesta
                   {
                       IsSuccessful = true,
                       IsException = false,
                       IsValidation = false,
                       Code = ConstantMessagesProyecto.OperacionExitosa,
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Proyecto, ConstantMessagesProyecto.OperacionExitosa, idAccionCrearProyectoAdministrativo, pProyectoAdministrativo.UsuarioCreacion, "PROYECTO ADMINISTRATIVO CREADO EXITOSAMENTE")
                   };
            }
            catch (Exception ex)
            {
                return
                     new Respuesta
                     {
                         IsSuccessful = false,
                         IsException = true,
                         IsValidation = true,
                         Code = ConstantMessagesProyecto.Error,
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Proyecto, ConstantMessagesProyecto.Error, idAccionCrearProyectoAdministrativo, pProyectoAdministrativo.UsuarioCreacion, ex.InnerException.ToString())
                     };
            }
        }

        public static bool ValidarRegistroCompletoProyectoAdministrativo(ProyectoAdministrativo pProyectoAdministrativo)
        {

            if (pProyectoAdministrativo.Enviado != null)
            {
                return true;
            }
            return false;
        }

        public async Task<List<ProyectoAdministracionGrilla>> ListAdministrativeProyectos(string pUsuarioConsulto)
        {
            List<ProyectoAdministracionGrilla> ListProyectoAdministrativoGrilla = new List<ProyectoAdministracionGrilla>();

            try
            {
                List<ProyectoAdministrativo> ListProyectosAdministrativo = await _context.ProyectoAdministrativo.Where(r => !(bool)r.Eliminado).
                    Include(x => x.ProyectoAdministrativoAportante).ToListAsync();

                foreach (var proyecto in ListProyectosAdministrativo)
                {
                    /*Localizacion municipio = await _commonService.GetLocalizacionByLocalizacionId(proyecto.LocalizacionIdMunicipio);
                    Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    Dominio estadoRegistro = await _commonService.GetDominioByNombreDominioAndTipoDominio(proyecto.EstadoProyectoCodigo, (int)EnumeratorTipoDominio.Estado_Registro);
                    // Dominio EstadoJuridicoPredios = await _commonService.GetDominioByNombreDominioAndTipoDominio(proyecto.ProyectoPredio.FirstOrDefault().EstadoJuridicoCodigo, (int)EnumeratorTipoDominio.Estado_Registro);
                    */
                    foreach (var admin in proyecto.ProyectoAdministrativoAportante)
                    {
                        admin.AportanteFuenteFinanciacion = _context.AportanteFuenteFinanciacion.Where(x => x.ProyectoAdministrativoAportanteId == admin.ProyectoAdministrativoAportanteId && !(bool)x.Eliminado).ToList();
                    }
                    ProyectoAdministracionGrilla proyectoAdministrativoGrilla = new ProyectoAdministracionGrilla
                    {
                        ProyectoAdminitracionId = proyecto.ProyectoAdministrativoId,
                        Enviado = (bool)proyecto.Enviado,
                        Estado = proyecto.RegistroCompleto,
                        Proyecto = proyecto
                    };
                    ListProyectoAdministrativoGrilla.Add(proyectoAdministrativoGrilla);
                }
                return ListProyectoAdministrativoGrilla.OrderByDescending(r => r.ProyectoAdminitracionId).ToList();
            }
            catch (Exception ex)
            {
                return ListProyectoAdministrativoGrilla;

            }
        }

        public async Task<bool> DeleteProyectoAdministrativoByProyectoId(int pProyectoId, string pUsuario)
        {
            int idAccionCrearProyectoAdministrativo = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Editar_Proyecto, (int)EnumeratorTipoDominio.Acciones);

            bool retorno = true;
            try
            {
                var disponibilidad = _context.DisponibilidadPresupuestalProyecto.Where(x => x.ProyectoAdministrativoId == pProyectoId).Count();
                if (disponibilidad > 0)
                {
                    _ = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Proyecto, ConstantMessagesProyecto.Error, idAccionCrearProyectoAdministrativo, pUsuario, "ELIMINACIÓN NO APLICADA, DEPENDE DE ALGUIEN.");
                    return false;
                }
                ProyectoAdministrativo proyecto = _context.ProyectoAdministrativo.Find(pProyectoId);
                proyecto.Eliminado = true;
                proyecto.UsuarioModificacion = pUsuario;
                proyecto.FechaModificacion = DateTime.Now;
                _context.SaveChanges();
                //auditoria
                string msg = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Proyecto, ConstantMessagesProyecto.OperacionExitosa, idAccionCrearProyectoAdministrativo, pUsuario, "ELIMINACIÓN DE PROYECTO");
            }
            catch (Exception ex)
            {
                _ = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Proyecto, ConstantMessagesProyecto.Error, idAccionCrearProyectoAdministrativo, pUsuario, ex.InnerException.ToString());
                return false;
            }
            return retorno;
        }

        public async Task<bool> EnviarProyectoAdministrativoByProyectoId(int pProyectoId, string pUsuario, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            int idAccionCrearProyectoAdministrativo = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Editar_Proyecto, (int)EnumeratorTipoDominio.Acciones);
            ProyectoAdministrativo proyecto = _context.ProyectoAdministrativo.Find(pProyectoId);
            bool retorno = true;
            try
            {

                proyecto.Enviado = true;
                proyecto.UsuarioModificacion = pUsuario;
                proyecto.FechaModificacion = DateTime.Now;
                _context.SaveChanges();

                //envio correo a administrativo
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.proyectoadministrativocreado);
                string template = TemplateRecoveryPassword.Contenido.Replace("[proyecto]", proyecto.ProyectoAdministrativoId.ToString()).Replace("_LinkF_", pDominioFront).Replace("[fecha]", Convert.ToDateTime(proyecto.FechaCreado).ToString("dd/MM/yyyy"));

                //template = template.Replace("_Link_", urlDestino);                
                var usuariosadmin = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Administrador).Include(y => y.Usuario).ToList();
                foreach (var usuarioadmin in usuariosadmin)
                {
                    bool blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuarioadmin.Usuario.Email, "Proyecto administrativo creado", template, pSender, pPassword, pMailServer, pMailPort);
                    string msg = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Proyecto, ConstantMessagesProyecto.OperacionExitosa, idAccionCrearProyectoAdministrativo, pUsuario, "ENVIAR PROYECTO");
                }
            }
            catch (Exception ex)
            {
                _ = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Proyecto, ConstantMessagesProyecto.Error, idAccionCrearProyectoAdministrativo, pUsuario, ex.InnerException.ToString());
                return false;
            }
            return retorno;
        }

        public async Task<List<FuenteFinanciacion>> GetFontsByAportantId(int pAportanteId)
        {
            var resultado = await _context.FuenteFinanciacion.
                Where(x => x.Aportante.TipoAportanteId == pAportanteId && !(bool)x.Eliminado).
                Include(x => x.CofinanciacionDocumento).
                OrderByDescending(r => r.FuenteFinanciacionId).ToListAsync();
            foreach (var res in resultado)
            {
                if(res.FuenteRecursosCodigo!=null)
                {
                    res.FuenteRecursosString = _context.Dominio.Where(x => x.Codigo == res.FuenteRecursosCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre;
                }
                
            }
            return resultado;
        }

        public async Task<bool> deleteFontByID(int pAportanteProyectoId, string pUsuarioModifico)
        {
            bool retorno = true;
            var fontproyecto = _context.AportanteFuenteFinanciacion.Find(pAportanteProyectoId);
            try
            {
                fontproyecto.Eliminado = true;
                fontproyecto.UsuarioEdicion = pUsuarioModifico;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }
            return retorno;
        }

        public async Task<bool> deletePredioByID(int pAportanteProyectoId, string pUsuarioModifico)
        {
            bool retorno = true;
            var fontproyecto = _context.ProyectoPredio.Find(pAportanteProyectoId);
            try
            {
                fontproyecto.Activo = false;
                //fontproyecto.UsuarioEdicion = pUsuarioModifico;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }
            return retorno;
        }

        public async Task<bool> deleteAportantesByID(int pAportanteProyectoId, string pUsuarioModifico)
        {
            bool retorno = true;
            var fontproyecto = _context.ProyectoAportante.Find(pAportanteProyectoId);
            try
            {
                fontproyecto.Eliminado = true;
                //fontproyecto.UsuarioEdicion = pUsuarioModifico;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }
            return retorno;
        }

        public async Task<bool> deleteInfraestructuraByID(int pAportanteProyectoId, string pUsuarioModifico)
        {
            bool retorno = true;
            var fontproyecto = _context.InfraestructuraIntervenirProyecto.Find(pAportanteProyectoId);
            try
            {
                fontproyecto.Eliminado = true;
                //fontproyecto.UsuarioEdicion = pUsuarioModifico;
                _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return false;
            }
            return retorno;
        }
    }
}
