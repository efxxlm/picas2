﻿using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using DinkToPdf;
using System.IO;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{

    /*
       PARAMETRICAS

     Tipo tema = 1. Solicitudes contractuales, 2. Tema nuevo
     TipoDominioId = 42
    ------------------------------------

     MiembrosComiteTecnico = 1.	Dirección técnica, 2.	Dirección financiera, 3.	Dirección jurídica, 4.	Fiduciaria, 5.	Dirección administrativa
     TipoDominioId = 46
    ------------------------------------

    */

    public class CommitteeSessionFiduciarioService : ICommitteeSessionFiduciarioService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly IProjectContractingService _IProjectContractingService;
        public readonly IConverter _converter;

        public CommitteeSessionFiduciarioService(devAsiVamosFFIEContext context, ICommonService commonService, IProjectContractingService projectContractingService, IConverter converter)
        {
            _IProjectContractingService = projectContractingService;
            _context = context;
            _commonService = commonService;
            _converter = converter;
        }



        //todas las solicitudes que fueron aprobadas por el comite tecnico.
        //TipoDominioId = 38, Codigo = 2, Nombre = Convocada
        public async Task<List<dynamic>> GetCommitteeSessionFiduciario()
        {
            List<dynamic> listaSolicitudesGrilla = new List<dynamic>();
            List<dynamic> listaComitesGrilla = new List<dynamic>();

            try
            {
                List<ComiteTecnico> listaComites = await _context.ComiteTecnico.Where(ct => (ct.EsComiteFiduciario == null || ct.EsComiteFiduciario == false)
                                                                                            && ct.EstadoActaCodigo == "3") // aprobada
                                                                                .Include(r => r.SesionComiteSolicitudComiteTecnico)
                                                                                //.Include( r => r.SesionComiteSolicitudComiteTecnicoFiduciario )
                                                                                .ToListAsync(); //Aprobadas

                List<Dominio> ListTipoSolicitud = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud).ToList();

                listaComites.ForEach(c =>
                {
                    dynamic comite = new
                    {
                        nombreSesion = c.NumeroComite,
                        fecha = c.FechaOrdenDia,
                        comiteTecnicoId = c.ComiteTecnicoId,
                        data = new List<dynamic>()
                    };

                    foreach (var ss in c.SesionComiteSolicitudComiteTecnico)
                    {
                        if (ss.EstadoCodigo == "1" && ss.ComiteTecnicoFiduciarioId == null)
                            switch (ss.TipoSolicitudCodigo)
                            {
                                case ConstanCodigoTipoSolicitud.Contratacion:
                                    {
                                        Contratacion contratacion = _context.Contratacion.Find(ss.SolicitudId);

                                        if (contratacion != null)
                                            comite.data.Add(new
                                            {
                                                Id = contratacion.ContratacionId,
                                                IdSolicitud = ss.SesionComiteSolicitudId,
                                                FechaSolicitud = contratacion.FechaTramite.HasValue ? (DateTime?)contratacion.FechaTramite.Value : null,
                                                NumeroSolicitud = contratacion.NumeroSolicitud,
                                                TipoSolicitud = ListTipoSolicitud.Where(r => r.Codigo == ConstanCodigoTipoSolicitud.Contratacion).FirstOrDefault().Nombre,
                                                tipoSolicitudNumeroTabla = ConstanCodigoTipoSolicitud.Contratacion
                                            });
                                        break;
                                    }

                                case ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion:
                                    {
                                        ProcesoSeleccion procesoSeleccion = _context.ProcesoSeleccion.Find(ss.SolicitudId);

                                        if (procesoSeleccion != null)
                                            comite.data.Add(new
                                            {
                                                Id = procesoSeleccion.ProcesoSeleccionId,
                                                IdSolicitud = ss.SesionComiteSolicitudId,
                                                FechaSolicitud = (DateTime?)(procesoSeleccion.FechaCreacion),
                                                NumeroSolicitud = procesoSeleccion.NumeroProceso,
                                                TipoSolicitud = ListTipoSolicitud.Where(r => r.Codigo == ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion).FirstOrDefault().Nombre,
                                                tipoSolicitudNumeroTabla = ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion
                                            });

                                        break;
                                    }
                                case ConstanCodigoTipoSolicitud.Actualizacion_Cronograma_Proceso_Seleccion:
                                    {
                                        ProcesoSeleccionMonitoreo procesoSeleccionMonitoreo = _context.ProcesoSeleccionMonitoreo
                                                                                                .Where(r => r.ProcesoSeleccionMonitoreoId == ss.SolicitudId)
                                                                                                .Include(r => r.ProcesoSeleccion)
                                                                                                .FirstOrDefault();

                                        if (procesoSeleccionMonitoreo != null)
                                            comite.data.Add(new
                                            {
                                                Id = procesoSeleccionMonitoreo.ProcesoSeleccionMonitoreoId,
                                                IdSolicitud = ss.SesionComiteSolicitudId,
                                                FechaSolicitud = (DateTime?)(procesoSeleccionMonitoreo.FechaCreacion),
                                                NumeroSolicitud = procesoSeleccionMonitoreo.ProcesoSeleccion.NumeroProceso,
                                                TipoSolicitud = ListTipoSolicitud.Where(r => r.Codigo == ConstanCodigoTipoSolicitud.Actualizacion_Cronograma_Proceso_Seleccion).FirstOrDefault().Nombre,
                                                tipoSolicitudNumeroTabla = ConstanCodigoTipoSolicitud.Actualizacion_Cronograma_Proceso_Seleccion,

                                            });

                                        break;
                                    }
                            }
                    }
                    if (comite.data.Count > 0)
                        listaComitesGrilla.Add(comite);

                });



                return listaComitesGrilla;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Respuesta> CreateEditComiteTecnicoAndSesionComiteTemaAndSesionComiteSolicitud(ComiteTecnico pComiteTecnico)
        {
            int idAccionCrearComiteTecnico = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Comite_Fiduciario_SesionComiteSolicitud_SesionComiteTema, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                string strCreateEdit;
                if (pComiteTecnico.ComiteTecnicoId == 0)
                {
                    // //Agregar Tema Proposiciones y Varios
                    // pComiteTecnico.SesionComiteTema.Add(
                    //        new SesionComiteTema
                    //        {
                    //            Eliminado = false,
                    //            UsuarioCreacion = pComiteTecnico.UsuarioCreacion,
                    //            FechaCreacion = DateTime.Now,
                    //            EsProposicionesVarios = true,
                    //            Tema = "",

                    //        });

                    strCreateEdit = "CREAR COMITE FIDUCIARIO  + SESIÓN COMITE SOLICITUD + SESIÓN COMITE TEMA";
                    //Auditoria
                    pComiteTecnico.FechaCreacion = DateTime.Now;
                    pComiteTecnico.Eliminado = false;
                    pComiteTecnico.EsComiteFiduciario = true;
                    //Registros
                    pComiteTecnico.EsCompleto = ValidarCamposComiteTecnico(pComiteTecnico);
                    pComiteTecnico.EsComiteFiduciario = true;

                    pComiteTecnico.EstadoComiteCodigo = ConstanCodigoEstadoComite.Sin_Convocatoria;
                    pComiteTecnico.NumeroComite = await _commonService.EnumeradorComiteFiduciario();


                    foreach (var SesionComiteTema in pComiteTecnico.SesionComiteTema)
                    {
                        //Auditoria
                        SesionComiteTema.FechaCreacion = DateTime.Now;
                        SesionComiteTema.UsuarioCreacion = pComiteTecnico.UsuarioCreacion;
                        SesionComiteTema.RegistroCompleto = ValidarRegistroCompletoSesionComiteTema(SesionComiteTema);
                        SesionComiteTema.Eliminado = false;
                    }

                    List<int> listaSolicitudes = new List<int>();

                    pComiteTecnico.SesionComiteSolicitudComiteTecnico.ToList().ForEach(ct => { listaSolicitudes.Add(ct.SesionComiteSolicitudId); });
                    pComiteTecnico.SesionComiteSolicitudComiteTecnico = new List<SesionComiteSolicitud>();

                    _context.ComiteTecnico.Add(pComiteTecnico);
                    _context.SaveChanges();

                    foreach (var id in listaSolicitudes)
                    {
                        SesionComiteSolicitud SesionComiteSolicitudOld = _context.SesionComiteSolicitud.Find(id);
                        //Auditoria 
                        SesionComiteSolicitudOld.UsuarioComiteFiduciario = pComiteTecnico.UsuarioCreacion;
                        SesionComiteSolicitudOld.FechaComiteFiduciario = DateTime.Now;

                        //Registros
                        SesionComiteSolicitudOld.ComiteTecnicoFiduciarioId = pComiteTecnico.ComiteTecnicoId;
                        SesionComiteSolicitudOld.RegistroCompleto = ValidarRegistroCompletoSesionComiteSolicitud(SesionComiteSolicitudOld);

                        //_context.SesionComiteSolicitud.Update( SesionComiteSolicitudOld );
                    }

                }
                else
                {
                    strCreateEdit = "EDITAR COMITE TECNICO  + SESIÓN COMITE SOLICITUD + SESIÓN COMITE TEMA";

                    ComiteTecnico comiteTecnicoOld = _context.ComiteTecnico
                        .Where(r => r.ComiteTecnicoId == pComiteTecnico.ComiteTecnicoId)
                            .Include(r => r.SesionComiteSolicitudComiteTecnico)
                            .Include(r => r.SesionComiteTema).FirstOrDefault();

                    //Auditoria 
                    comiteTecnicoOld.UsuarioModificacion = pComiteTecnico.UsuarioCreacion;
                    comiteTecnicoOld.FechaModificacion = DateTime.Now;

                    //Registros
                    comiteTecnicoOld.EsCompleto = ValidarCamposComiteTecnico(comiteTecnicoOld);
                    comiteTecnicoOld.RequiereVotacion = comiteTecnicoOld.RequiereVotacion;
                    comiteTecnicoOld.Justificacion = comiteTecnicoOld.Justificacion;
                    comiteTecnicoOld.EsAprobado = comiteTecnicoOld.EsAprobado;
                    comiteTecnicoOld.FechaAplazamiento = comiteTecnicoOld.FechaAplazamiento;
                    comiteTecnicoOld.Observaciones = comiteTecnicoOld.Observaciones;
                    comiteTecnicoOld.RutaSoporteVotacion = comiteTecnicoOld.RutaSoporteVotacion;
                    comiteTecnicoOld.TieneCompromisos = comiteTecnicoOld.TieneCompromisos;
                    comiteTecnicoOld.CantCompromisos = comiteTecnicoOld.CantCompromisos;
                    comiteTecnicoOld.RutaActaSesion = comiteTecnicoOld.RutaActaSesion;
                    comiteTecnicoOld.FechaOrdenDia = comiteTecnicoOld.FechaOrdenDia;
                    comiteTecnicoOld.NumeroComite = comiteTecnicoOld.NumeroComite;
                    comiteTecnicoOld.EstadoComiteCodigo = comiteTecnicoOld.EstadoComiteCodigo;
                    comiteTecnicoOld.TipoTemaFiduciarioCodigo = pComiteTecnico.TipoTemaFiduciarioCodigo;

                    foreach (var SesionComiteTema in pComiteTecnico.SesionComiteTema)
                    {
                        if (SesionComiteTema.SesionTemaId == 0)
                        {

                            //Auditoria 
                            SesionComiteTema.UsuarioCreacion = pComiteTecnico.UsuarioCreacion;
                            SesionComiteTema.FechaCreacion = DateTime.Now;
                            SesionComiteTema.Eliminado = false;
                            SesionComiteTema.RegistroCompleto = ValidarRegistroCompletoSesionComiteTema(SesionComiteTema);
                            //Registros
                            SesionComiteTema.ComiteTecnicoId = pComiteTecnico.ComiteTecnicoId;
                            _context.SesionComiteTema.Add(SesionComiteTema);
                        }
                        else
                        {
                            SesionComiteTema sesionComiteTemaOld = _context.SesionComiteTema.Find(SesionComiteTema.SesionTemaId);
                            //Auditoria 
                            sesionComiteTemaOld.UsuarioModificacion = pComiteTecnico.UsuarioCreacion;
                            sesionComiteTemaOld.FechaModificacion = DateTime.Now;

                            //Registros
                            sesionComiteTemaOld.Tema = SesionComiteTema.Tema;
                            sesionComiteTemaOld.ResponsableCodigo = SesionComiteTema.ResponsableCodigo;
                            sesionComiteTemaOld.TiempoIntervencion = SesionComiteTema.TiempoIntervencion;
                            sesionComiteTemaOld.RutaSoporte = SesionComiteTema.RutaSoporte;
                            sesionComiteTemaOld.Observaciones = SesionComiteTema.Observaciones;
                            sesionComiteTemaOld.EsAprobado = SesionComiteTema.EsAprobado;
                            sesionComiteTemaOld.ObservacionesDecision = SesionComiteTema.Observaciones;
                            sesionComiteTemaOld.EsProposicionesVarios = SesionComiteTema.EsProposicionesVarios;
                            sesionComiteTemaOld.RegistroCompleto = ValidarRegistroCompletoSesionComiteTema(sesionComiteTemaOld);
                        }
                    }

                    List<SesionComiteSolicitud> SolicitudesExistentes = _context.SesionComiteSolicitud
                                                                                            .Where(r => r.ComiteTecnicoFiduciarioId == pComiteTecnico.ComiteTecnicoId)
                                                                                            .ToList();

                    SolicitudesExistentes.ForEach(se =>
                    {
                        if (pComiteTecnico.SesionComiteSolicitudComiteTecnico.Where(r => r.SesionComiteSolicitudId == se.SesionComiteSolicitudId).Count() == 0)
                        {
                            se.ComiteTecnicoFiduciarioId = null;
                        }
                    });

                    foreach (var SesionComiteSolicitud in pComiteTecnico.SesionComiteSolicitudComiteTecnico)
                    {
                        {
                            if (SolicitudesExistentes.Where(r => r.SesionComiteSolicitudId == SesionComiteSolicitud.SesionComiteSolicitudId).Count() > 0)
                            {
                                SesionComiteSolicitud SesionComiteSolicitudOld = _context.SesionComiteSolicitud.Find(SesionComiteSolicitud.SesionComiteSolicitudId);
                                //Auditoria 
                                SesionComiteSolicitudOld.UsuarioModificacion = pComiteTecnico.UsuarioCreacion;
                                SesionComiteSolicitudOld.FechaModificacion = DateTime.Now;

                                //Registros
                                SesionComiteSolicitudOld.DesarrolloSolicitudFiduciario = SesionComiteSolicitud.DesarrolloSolicitudFiduciario;
                                SesionComiteSolicitudOld.EstadoActaCodigoFiduciario = SesionComiteSolicitud.EstadoActaCodigoFiduciario;
                                SesionComiteSolicitudOld.ComiteTecnicoFiduciarioId = pComiteTecnico.ComiteTecnicoId;
                                SesionComiteSolicitudOld.ObservacionesFiduciario = SesionComiteSolicitud.ObservacionesFiduciario;
                                SesionComiteSolicitudOld.RutaSoporteVotacionFiduciario = SesionComiteSolicitud.RutaSoporteVotacionFiduciario;
                                SesionComiteSolicitudOld.GeneraCompromisoFiduciario = SesionComiteSolicitud.GeneraCompromisoFiduciario;
                                SesionComiteSolicitudOld.CantCompromisosFiduciario = SesionComiteSolicitud.CantCompromisosFiduciario;
                                SesionComiteSolicitudOld.RequiereVotacionFiduciario = SesionComiteSolicitud.RequiereVotacionFiduciario;
                                SesionComiteSolicitudOld.RegistroCompleto = ValidarRegistroCompletoSesionComiteSolicitud(SesionComiteSolicitudOld);

                            }
                            else
                            {
                                SesionComiteSolicitud SesionComiteSolicitudOld = _context.SesionComiteSolicitud.Find(SesionComiteSolicitud.SesionComiteSolicitudId);

                                SesionComiteSolicitudOld.UsuarioComiteFiduciario = pComiteTecnico.UsuarioCreacion;
                                SesionComiteSolicitudOld.FechaComiteFiduciario = DateTime.Now;

                                //Registros
                                SesionComiteSolicitudOld.ComiteTecnicoFiduciarioId = pComiteTecnico.ComiteTecnicoId;
                                SesionComiteSolicitudOld.RegistroCompleto = ValidarRegistroCompletoSesionComiteSolicitud(SesionComiteSolicitudOld);
                            }


                        }
                    }
                }
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantSesionComiteFiduciario.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.OperacionExitosa, idAccionCrearComiteTecnico, pComiteTecnico.UsuarioCreacion, strCreateEdit)
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
                        Code = ConstantSesionComiteTecnico.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.Error, idAccionCrearComiteTecnico, pComiteTecnico.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        //Get all seseion comite tecnico fiduciario
        public async Task<List<ComiteGrilla>> GetCommitteeSession()
        {
            List<ComiteGrilla> ListComiteGrilla = new List<ComiteGrilla>();

            List<Dominio> ListaEstadoComite = await _context.Dominio
                .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Comite && (bool)r.Activo)
                .ToListAsync();

            List<Dominio> ListaEstadoActa = await _context.Dominio
                .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estados_Acta && (bool)r.Activo)
                .ToListAsync();

            try
            {
                var ListComiteTecnico = await _context.ComiteTecnico.Where(r => !(bool)r.Eliminado && (bool)r.EsComiteFiduciario)
                                                                    .Include(r => r.SesionComiteTecnicoCompromiso)
                                                                    .Select(x => new
                                                                    {
                                                                        Id = x.ComiteTecnicoId,
                                                                        FechaComite = x.FechaOrdenDia,
                                                                        EstadoComite = x.EstadoComiteCodigo,
                                                                        x.NumeroComite,
                                                                        x.EsComiteFiduciario,
                                                                        x.EstadoActaCodigo,
                                                                        x.EsCompleto

                                                                    }).Distinct().OrderByDescending(r => r.Id).ToListAsync();

                foreach (var comite in ListComiteTecnico)
                {
                    ComiteGrilla comiteGrilla = new ComiteGrilla
                    {
                        Id = comite.Id,
                        FechaComite = comite.FechaComite.Value,
                        EstadoComiteCodigo = comite.EstadoComite,
                        EstadoComite = !string.IsNullOrEmpty(comite.EstadoComite) ? ListaEstadoComite.Where(r => r.Codigo == comite.EstadoComite).FirstOrDefault().Nombre : "",
                        NumeroComite = comite.NumeroComite,
                        EstadoActa = !string.IsNullOrEmpty(comite.EstadoActaCodigo) ? ListaEstadoActa.Where(r => r.Codigo == comite.EstadoActaCodigo).FirstOrDefault().Nombre : "",
                        EstadoActaCodigo = comite.EstadoActaCodigo,
                        RegistroCompletoNombre = (bool)comite.EsCompleto ? "Completo" : "Incompleto",
                        RegistroCompleto = comite.EsCompleto,
                        //NumeroCompromisos = numeroCompromisos(comite.Id, false),
                        //NumeroCompromisosCumplidos = numeroCompromisos(comite.Id, true),
                        EsComiteFiduciario = comite.EsComiteFiduciario,
                    };

                    ListComiteGrilla.Add(comiteGrilla);
                }
            }
            catch (Exception ex)
            {
            }
            return ListComiteGrilla;
        }

        //Solicitudes acordeon => Para seleccion de solicitudes contractuales, temas y solicitudes
        public async Task<ComiteTecnico> GetRequestCommitteeSessionById(int comiteTecnicoId)
        {
            try
            {
                List<Dominio> ListTipoSolicitud = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud).ToList();

                ComiteTecnico comite = await _context.ComiteTecnico
                                .Where(sc => sc.ComiteTecnicoId == comiteTecnicoId && !(bool)sc.Eliminado)
                                .Include(cm => cm.SesionComiteTema)
                                .Include(r => r.SesionComiteSolicitudComiteTecnicoFiduciario)
                                .FirstOrDefaultAsync();

                comite.SesionComiteSolicitudComiteTecnicoFiduciario.ToList().ForEach(cf =>
                {
                    comite.SesionComiteSolicitudComiteTecnico.Add(_context.SesionComiteSolicitud
                                                                    .Where(r => r.SesionComiteSolicitudId == cf.SesionComiteSolicitudId)
                                                                    .Include(r => r.ComiteTecnico)
                                                                    .FirstOrDefault()
                    );
                });

                foreach (var ss in comite.SesionComiteSolicitudComiteTecnico)
                {
                    switch (ss.TipoSolicitudCodigo)
                    {
                        case ConstanCodigoTipoSolicitud.Contratacion:
                            {
                                Contratacion contratacion = _context.Contratacion.Find(ss.SolicitudId);

                                if (contratacion != null)
                                {
                                    ss.NumeroSolicitud = contratacion.NumeroSolicitud;
                                    ss.FechaSolicitud = contratacion.FechaTramite.HasValue ? (DateTime?)contratacion.FechaTramite.Value : null;
                                    ss.TipoSolicitud = ListTipoSolicitud.Where(r => r.Codigo == ConstanCodigoTipoSolicitud.Contratacion).FirstOrDefault().Nombre;
                                }
                                break;
                            }

                        case ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion:
                            {
                                ProcesoSeleccion procesoSeleccion = _context.ProcesoSeleccion.Find(ss.SolicitudId);

                                if (procesoSeleccion != null)
                                {
                                    ss.NumeroSolicitud = procesoSeleccion.NumeroProceso;
                                    ss.FechaSolicitud = (DateTime?)(procesoSeleccion.FechaCreacion);
                                    ss.TipoSolicitud = ListTipoSolicitud.Where(r => r.Codigo == ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion).FirstOrDefault().Nombre;
                                }
                                break;
                            }
                        case ConstanCodigoTipoSolicitud.Actualizacion_Cronograma_Proceso_Seleccion:
                            {
                                ProcesoSeleccionMonitoreo procesoSeleccionMonitoreo = _context.ProcesoSeleccionMonitoreo
                                                                                        .Where(r => r.ProcesoSeleccionMonitoreoId == ss.SolicitudId)
                                                                                        .Include(r => r.ProcesoSeleccion)
                                                                                        .FirstOrDefault();

                                if (procesoSeleccionMonitoreo != null)
                                {
                                    ss.FechaSolicitud = (DateTime?)(procesoSeleccionMonitoreo.FechaCreacion);
                                    ss.NumeroSolicitud = procesoSeleccionMonitoreo.ProcesoSeleccion.NumeroProceso;
                                    ss.TipoSolicitud = ListTipoSolicitud.Where(r => r.Codigo == ConstanCodigoTipoSolicitud.Actualizacion_Cronograma_Proceso_Seleccion).FirstOrDefault().Nombre;
                                }
                                break;
                            }
                    }
                }

                comite.SesionComiteTema = comite.SesionComiteTema.Where(te => !(bool)te.Eliminado).ToList();

                return comite;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Respuesta> ConvocarComiteTecnico(ComiteTecnico pComiteTecnico, string pDominio, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Convocar_Comite_Fiduciario, (int)EnumeratorTipoDominio.Acciones);
            List<Dominio> placeholders = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.PlaceHolder).ToList();

            try
            {
                ComiteTecnico comiteTecnico = await _context.ComiteTecnico
                    .Where(r => r.ComiteTecnicoId == pComiteTecnico.ComiteTecnicoId)
                    .Include(r => r.SesionComiteTema)
                    .Include(r => r.SesionComiteSolicitudComiteTecnicoFiduciario)
                    .Include(r => r.SesionParticipante)
                        .ThenInclude(r => r.Usuario).FirstOrDefaultAsync();

                comiteTecnico.SesionParticipante = comiteTecnico.SesionParticipante.Where(r => !(bool)r.Eliminado).ToList();
                comiteTecnico.SesionComiteTema = comiteTecnico.SesionComiteTema.Where(r => !(bool)r.Eliminado).ToList();

                comiteTecnico.EstadoComiteCodigo = ConstanCodigoEstadoComite.Convocada;
                comiteTecnico.UsuarioModificacion = pComiteTecnico.UsuarioCreacion;
                comiteTecnico.FechaModificacion = DateTime.Now;

                Template TemplateConvocar = await _commonService.GetTemplateById((int)enumeratorTemplate.ConvocarSesionComite);
                Template TemplateOrdenDia = await _commonService.GetTemplateById((int)enumeratorTemplate.OrdenDia);
                string strOrdenDia = "";
                int contador = 0;

                foreach (var item in comiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario)
                {
                    strOrdenDia += TemplateOrdenDia.Contenido;
                    contador++;


                    switch (item.TipoSolicitudCodigo)
                    {
                        case ConstanCodigoTipoSolicitud.Contratacion:
                            item.NumeroSolicitud = _context.Contratacion.Find(item.SolicitudId).NumeroSolicitud;
                            break;
                        case ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion:
                            item.NumeroSolicitud = _context.ProcesoSeleccion.Find(item.SolicitudId).NumeroProceso;
                            break;
                        case ConstanCodigoTipoSolicitud.Actualizacion_Cronograma_Proceso_Seleccion:

                            ProcesoSeleccionMonitoreo proceso = _context.ProcesoSeleccionMonitoreo
                                                                           .Where(r => r.ProcesoSeleccionMonitoreoId == item.SolicitudId)
                                                                           .Include(r => r.ProcesoSeleccion)
                                                                           .FirstOrDefault();
                            item.NumeroSolicitud = string.Concat(proceso.ProcesoSeleccion.NumeroProceso, " - ", proceso.NumeroProceso);
                            break;
                    }

                    strOrdenDia = strOrdenDia.Replace("[NUMERO]", contador.ToString())
                                             .Replace("[ORDEN]", item.NumeroSolicitud);

                }

                foreach (var item in comiteTecnico.SesionComiteTema.Where(r => r.Eliminado != true))
                {
                    strOrdenDia += TemplateOrdenDia.Contenido;
                    contador++;

                    strOrdenDia = strOrdenDia.Replace("[NUMERO]", contador.ToString())
                                             .Replace("[ORDEN]", item.Tema);

                }

                string template = TemplateConvocar.Contenido.Replace("[NUMERO_COMITE]", comiteTecnico.NumeroComite)
                                                            .Replace("[FECHA_COMITE]", comiteTecnico.FechaOrdenDia.Value.ToString("dd/MM/yyyy"))
                                                            .Replace("[ORDEN_DIA]", strOrdenDia);



                //Notificar a los participantes
                bool blEnvioCorreo = false;

                //TODO: esta lista debe ser parametrizada de acuerdo a los perfiles Directore de las 4 areas :
                //Director financiero, Director Juridico , Director técnico, y Director administrativo
                List<Usuario> ListMiembrosComite = await _commonService.GetUsuariosByPerfil((int)EnumeratorPerfil.Miembros_Comite);

                foreach (var Usuario in ListMiembrosComite)
                {
                    if (!string.IsNullOrEmpty(Usuario.Email))
                    {

                        blEnvioCorreo = Helpers.Helpers.EnviarCorreo(Usuario.Email, "Convocatoria sesión de comité Fiduciario", template, pSentender, pPassword, pMailServer, pMailPort);
                    }
                }

                _context.SaveChanges();
                return
                   new Respuesta
                   {
                       //Data = await GetComiteTecnicoByComiteTecnicoId(pComiteTecnico.ComiteTecnicoId),
                       IsSuccessful = true,
                       IsException = false,
                       IsValidation = false,
                       Code = ConstantSesionComiteTecnico.OperacionExitosa,
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.OperacionExitosa, idAccion, pComiteTecnico.UsuarioCreacion, "CONVOCAR COMITE TECNICO")
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
                       Code = ConstantSesionComiteTecnico.Error,
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.Error, idAccion, pComiteTecnico.UsuarioCreacion, ex.InnerException.ToString())
                   };
            }
        }

        public async Task<Respuesta> CambiarEstadoComiteTecnico(ComiteTecnico pComiteTecnico)
        {
            int idAccionCambiarEstadoSesion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Comite_Fiduciario, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ComiteTecnico ComiteTecnicoOld = _context.ComiteTecnico.Find(pComiteTecnico.ComiteTecnicoId);

                string NombreEstado = await _commonService.GetNombreDominioByCodigoAndTipoDominio(pComiteTecnico.EstadoComiteCodigo, (int)EnumeratorTipoDominio.Estado_Comite);

                ComiteTecnicoOld.UsuarioModificacion = pComiteTecnico.UsuarioCreacion;
                ComiteTecnicoOld.FechaModificacion = DateTime.Now;

                ComiteTecnicoOld.EstadoComiteCodigo = pComiteTecnico.EstadoComiteCodigo;

                if (ComiteTecnicoOld.EstadoComiteCodigo == ConstanCodigoEstadoComite.Desarrollada_Sin_Acta)
                {
                    ComiteTecnicoOld.EstadoActaCodigo = "1"; //Sin Acta
                }

                if (ComiteTecnicoOld.EstadoComiteCodigo == ConstanCodigoEstadoComite.Con_Acta_De_Sesion_Enviada)
                {
                    ComiteTecnicoOld.EstadoActaCodigo = "2"; //En proceso de aprobación
                }

                _context.SaveChanges();
                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = ConstantSesionComiteTecnico.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.OperacionExitosa, idAccionCambiarEstadoSesion, pComiteTecnico.UsuarioCreacion, "ESTADO COMITE CAMBIADO A " + NombreEstado.ToUpper())
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
                    Code = ConstantSesionComiteTecnico.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.Error, idAccionCambiarEstadoSesion, pComiteTecnico.UsuarioCreacion, ex.InnerException.ToString())
                };
            }

        }

        public async Task<ComiteTecnico> GetComiteTecnicoByComiteTecnicoId(int pComiteTecnicoId)
        {
            if (pComiteTecnicoId == 0)
            {

                return new ComiteTecnico();
            }
            //  List<SesionParticipante> sesionParticipantes = _context.SesionParticipante.Where(r=> r.ComiteTecnicoId == pComiteTecnicoId).ToList();

            ComiteTecnico comiteTecnico = await _context.ComiteTecnico
                 .Where(r => r.ComiteTecnicoId == pComiteTecnicoId)

                  .Include(r => r.SesionInvitado)
                  .Include(r => r.SesionComiteSolicitudComiteTecnicoFiduciario)
                     .ThenInclude(r => r.SesionSolicitudVoto)
                   //   .Include(r => r.SesionComiteSolicitudComiteTecnicoFiduciario)
                   //      .ThenInclude(r => r.SesionSolicitudCompromiso)
                   .Include(r => r.SesionComiteTema)
                     .ThenInclude(r => r.SesionTemaVoto)
                  .Include(r => r.SesionComiteTema)
                     .ThenInclude(r => r.TemaCompromiso)
                 .FirstOrDefaultAsync();

            //    comiteTecnico.SesionParticipante = sesionParticipantes;

            comiteTecnico.SesionComiteTema = comiteTecnico.SesionComiteTema.Where(r => !(bool)r.Eliminado).ToList();
            comiteTecnico.SesionInvitado = comiteTecnico.SesionInvitado.Where(r => !(bool)r.Eliminado).ToList();


            foreach (var SesionComiteSolicitud in comiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario)
            {
                SesionComiteSolicitud.SesionSolicitudVoto = SesionComiteSolicitud.SesionSolicitudVoto.Where(r => !(bool)r.Eliminado).ToList();
                SesionComiteSolicitud.SesionSolicitudCompromiso = _context.SesionSolicitudCompromiso
                                                                            .Where(r => r.SesionComiteSolicitudId == SesionComiteSolicitud.SesionComiteSolicitudId &&
                                                                                r.EsFiduciario == true &&
                                                                                !(bool)r.Eliminado)
                                                                            .ToList();
            }

            List<SesionSolicitudVoto> ListSesionSolicitudVotos = _context.SesionSolicitudVoto.Where(r => !(bool)r.Eliminado).ToList();
            foreach (var SesionComiteSolicitud in comiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario)
            {
                SesionComiteSolicitud.SesionSolicitudVoto = ListSesionSolicitudVotos.Where(r => r.SesionComiteSolicitudId == SesionComiteSolicitud.SesionComiteSolicitudId).ToList();
            }
            List<Dominio> TipoComiteSolicitud = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud).ToList();

            List<ProcesoSeleccion> ListProcesoSeleccion =
                _context.ProcesoSeleccion
                .Where(r => !(bool)r.Eliminado).ToList();

            List<Contratacion> ListContratacion = _context.Contratacion.ToList();

            foreach (SesionComiteSolicitud sesionComiteSolicitud in comiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario)
            {

                switch (sesionComiteSolicitud.TipoSolicitudCodigo)
                {
                    case ConstanCodigoTipoSolicitud.Contratacion:

                        Contratacion contratacion = ListContratacion.Where(r => r.ContratacionId == sesionComiteSolicitud.SolicitudId).FirstOrDefault();

                        sesionComiteSolicitud.FechaSolicitud = (DateTime)contratacion.FechaCreacion;

                        sesionComiteSolicitud.NumeroSolicitud = contratacion.NumeroSolicitud;

                        sesionComiteSolicitud.Contratacion = contratacion;

                        break;

                    case ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion:
                        sesionComiteSolicitud.FechaSolicitud = ListProcesoSeleccion
                          .Where(r => r.ProcesoSeleccionId == sesionComiteSolicitud.SolicitudId)
                          .FirstOrDefault()
                          .FechaCreacion;

                        sesionComiteSolicitud.NumeroSolicitud = ListProcesoSeleccion
                          .Where(r => r.ProcesoSeleccionId == sesionComiteSolicitud.SolicitudId)
                          .FirstOrDefault()
                          .NumeroProceso;

                        sesionComiteSolicitud.ProcesoSeleccion = ListProcesoSeleccion.Where(r => r.ProcesoSeleccionId == sesionComiteSolicitud.SolicitudId).FirstOrDefault();

                        break;

                    case ConstanCodigoTipoSolicitud.Actualizacion_Cronograma_Proceso_Seleccion:

                        ProcesoSeleccionMonitoreo actualizacionCronograma = _context.ProcesoSeleccionMonitoreo
                                                                                .Where(r => r.ProcesoSeleccionMonitoreoId == sesionComiteSolicitud.SolicitudId)
                                                                                .Include(r => r.ProcesoSeleccion)
                                                                                .FirstOrDefault();

                        sesionComiteSolicitud.FechaSolicitud = actualizacionCronograma.FechaCreacion;

                        sesionComiteSolicitud.NumeroSolicitud = actualizacionCronograma.ProcesoSeleccion.NumeroProceso;

                        sesionComiteSolicitud.ProcesoSeleccionMonitoreo = actualizacionCronograma;

                        sesionComiteSolicitud.NumeroHijo = actualizacionCronograma.NumeroProceso;

                        break;
                }

                sesionComiteSolicitud.TipoSolicitud = TipoComiteSolicitud.Where(r => r.Codigo == sesionComiteSolicitud.TipoSolicitudCodigo).FirstOrDefault().Nombre;
            }

            return comiteTecnico;
        }

        public async Task<ProcesoSeleccionMonitoreo> GetProcesoSeleccionMonitoreo(int pProcesoSeleccionMonitoreoId)
        {
            ProcesoSeleccionMonitoreo procesoSeleccionMonitoreo = await _context.ProcesoSeleccionMonitoreo
                                                                    .Where(r => r.ProcesoSeleccionMonitoreoId == pProcesoSeleccionMonitoreoId &&
                                                                           r.Eliminado != true
                                                                     )
                                                                     .Include(r => r.ProcesoSeleccionCronogramaMonitoreo)
                                                                        .ThenInclude(r => r.SesionSolicitudObservacionActualizacionCronograma)
                                                                     .FirstOrDefaultAsync();

            procesoSeleccionMonitoreo.ProcesoSeleccionCronogramaMonitoreo.ToList().RemoveAll(r => r.Eliminado == true);

            procesoSeleccionMonitoreo.ProcesoSeleccionCronogramaMonitoreo.ToList().ForEach(p =>
            {
                p.SesionSolicitudObservacionActualizacionCronograma.ToList().RemoveAll(r => r.Eliminado == true);
            });

            return procesoSeleccionMonitoreo;
        }

        private string ReemplazarDatosPlantillaContratacion(string pPlantilla, Contratacion pContratacion)
        {
            List<Dominio> placeholders = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.PlaceHolder).ToList();

            string TipoPlantillaDetalleProyecto = ((int)ConstanCodigoPlantillas.Detalle_Proyecto).ToString();
            string DetalleProyecto = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaDetalleProyecto).Select(r => r.Contenido).FirstOrDefault();
            string DetallesProyectos = "";

            string TipoPlantillaRegistrosAlcance = ((int)ConstanCodigoPlantillas.Registros_Tabla_Alcance).ToString();
            string RegistroAlcance = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaRegistrosAlcance).Select(r => r.Contenido).FirstOrDefault();

            List<Dominio> ListaParametricas = _context.Dominio.ToList();
            List<Localizacion> ListaLocalizaciones = _context.Localizacion.ToList();
            List<InstitucionEducativaSede> ListaInstitucionEducativaSedes = _context.InstitucionEducativaSede.ToList();
            //Se crea el detalle de los proyectos asociado a contratacion - contratacionProyecto 
            int enumProyecto = 1;

            foreach (var proyecto in pContratacion.ContratacionProyecto)
            {
                //Se crear una nueva plantilla por cada vez que entra
                DetallesProyectos += DetalleProyecto;
                string RegistrosAlcance = "";

                Localizacion Municipio = ListaLocalizaciones.Where(r => r.LocalizacionId == proyecto.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                Localizacion Departamento = ListaLocalizaciones.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
                Localizacion Region = ListaLocalizaciones.Where(r => r.LocalizacionId == Departamento.IdPadre).FirstOrDefault();

                InstitucionEducativaSede IntitucionEducativa = ListaInstitucionEducativaSedes.Where(r => r.InstitucionEducativaSedeId == proyecto.Proyecto.InstitucionEducativaId).FirstOrDefault();
                InstitucionEducativaSede Sede = ListaInstitucionEducativaSedes.Where(r => r.InstitucionEducativaSedeId == proyecto.Proyecto.SedeId).FirstOrDefault();



                foreach (Dominio placeholderDominio in placeholders)
                {
                    switch (placeholderDominio.Codigo)
                    {

                        case ConstanCodigoVariablesPlaceHolders.NOMBRE_PROYECTO:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, (enumProyecto++).ToString());
                            break;

                        case ConstanCodigoVariablesPlaceHolders.TIPO_DE_INTERVENCION:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, ListaParametricas.Where(r => r.Codigo == proyecto.Proyecto.TipoIntervencionCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).FirstOrDefault().Nombre);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.LLAVE_MEN:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, proyecto.Proyecto.LlaveMen);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.REGION:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, Region.Descripcion);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.DEPARTAMENTO:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, Departamento.Descripcion);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.MUNICIPIO:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, Municipio.Descripcion);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.INSTITUCION_EDUCATIVA:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, IntitucionEducativa.Nombre);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.CODIGO_DANE_IE:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, IntitucionEducativa.CodigoDane);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.SEDE:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, Sede.Nombre);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.CODIGO_DANE_SEDE:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, Sede.CodigoDane);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.REGISTROS_ALCANCE:

                            foreach (var infraestructura in proyecto.Proyecto.InfraestructuraIntervenirProyecto)
                            {
                                RegistrosAlcance += RegistroAlcance;

                                RegistrosAlcance = RegistrosAlcance.Replace("[ALCANCE_ESPACIOS_A_INTERVENIR]", ListaParametricas
                                    .Where(r => r.Codigo == infraestructura.InfraestructuraCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Espacios_Intervenir)
                                    .FirstOrDefault().Nombre);
                                RegistrosAlcance = RegistrosAlcance.Replace("[ALCANCE_CANTIDAD]", infraestructura.Cantidad.ToString());
                            }

                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, RegistrosAlcance);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.PLAZO_OBRA_MESES:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, proyecto.Proyecto.InfraestructuraIntervenirProyecto.Sum(r => r.PlazoMesesObra).ToString());
                            break;

                        case ConstanCodigoVariablesPlaceHolders.PLAZO_OBRA_DIAS:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, proyecto.Proyecto.InfraestructuraIntervenirProyecto.Sum(r => r.PlazoDiasObra).ToString());
                            break;

                        case ConstanCodigoVariablesPlaceHolders.VALOR_OBRA:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, "$" + String.Format("{0:n0}", proyecto.Proyecto.ValorObra));
                            break;

                        case ConstanCodigoVariablesPlaceHolders.VALOR_INTERVENTORIA:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, "$" + String.Format("{0:n0}", proyecto.Proyecto.ValorInterventoria));
                            break;

                        case ConstanCodigoVariablesPlaceHolders.VALOR_TOTAL_PROYECTO:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, "$" + String.Format("{0:n0}", proyecto.Proyecto.ValorTotal));
                            break;

                        case ConstanCodigoVariablesPlaceHolders.REGISTROS_FUENTES_USO:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, " ");
                            break;
                    }
                }
            }

            string TipoPlantillaFuentesFinanciacion = ((int)ConstanCodigoPlantillas.Registros_Fuente_De_Uso).ToString();

            // FUENTES DE FINANCIACION 
            string TipoPlantillaRegistrosFuentes = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaFuentesFinanciacion).Select(r => r.Contenido).FirstOrDefault();
            string RegistrosFuentesUso = string.Empty;


            string TipoPlantillaRegistrosUsosFuenteUsos = ((int)ConstanCodigoPlantillas.Registros_Usos_Registros_Fuente_de_Uso).ToString();
            string PlantillaRegistrosUsosFuenteUsos = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaRegistrosUsosFuenteUsos).Select(r => r.Contenido).FirstOrDefault();
            string RegistrosRegistrosUsosFuenteUsos = string.Empty;

            foreach (var contratacionProyecto in pContratacion.ContratacionProyecto)
            {
                foreach (var ContratacionProyectoAportante in contratacionProyecto.ContratacionProyectoAportante)
                {
                    foreach (var ComponenteAportante in ContratacionProyectoAportante.ComponenteAportante)
                    {
                        foreach (var ComponenteUso in ComponenteAportante.ComponenteUso)
                        {
                            RegistrosFuentesUso += TipoPlantillaRegistrosFuentes;
                            foreach (Dominio placeholderDominio in placeholders)
                            {
                                switch (placeholderDominio.Codigo)
                                {
                                    case ConstanCodigoVariablesPlaceHolders.NOMBRE_APORTANTE_FUENTES_USO:
                                        string strNombreAportante = string.Empty;
                                        switch (ContratacionProyectoAportante.CofinanciacionAportante.TipoAportanteId)
                                        {
                                            case ConstanTipoAportante.Ffie:
                                                strNombreAportante = ConstanStringTipoAportante.Ffie;
                                                break;

                                            case ConstanTipoAportante.ET:

                                                if (ContratacionProyectoAportante.CofinanciacionAportante.Departamento != null)
                                                {
                                                    strNombreAportante = ContratacionProyectoAportante.CofinanciacionAportante.Departamento.Descripcion;
                                                }
                                                else
                                                {
                                                    strNombreAportante = ContratacionProyectoAportante.CofinanciacionAportante.Municipio.Descripcion;
                                                }
                                                break;
                                            case ConstanTipoAportante.Tercero:
                                                strNombreAportante = ContratacionProyectoAportante.CofinanciacionAportante.NombreAportante.Nombre;
                                                break;
                                        }
                                        RegistrosFuentesUso = RegistrosFuentesUso.Replace(placeholderDominio.Nombre, strNombreAportante);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.VALOR_APORTANTE_PROYECTO_FUENTES_USO:
                                        string ValorAportante = "$" + String.Format("{0:n0}", ContratacionProyectoAportante.CofinanciacionAportante.ProyectoAportante.FirstOrDefault().ValorObra);
                                        if (pContratacion.TipoSolicitudCodigo == ((int)ConstanCodigoTipoContratacion.Interventoria).ToString())
                                        {
                                            ValorAportante = "$" + String.Format("{0:n0}", ContratacionProyectoAportante.CofinanciacionAportante.ProyectoAportante.FirstOrDefault().ValorInterventoria);
                                        }
                                        RegistrosFuentesUso = RegistrosFuentesUso.Replace(placeholderDominio.Nombre, ValorAportante);
                                        break;


                                    case ConstanCodigoVariablesPlaceHolders.FASE_FUENTES_USO:
                                        string strFase = string.Empty;

                                        if (ContratacionProyectoAportante.ComponenteAportante.Count() > 0)
                                        {
                                            if (!string.IsNullOrEmpty(ContratacionProyectoAportante.ComponenteAportante.FirstOrDefault().FaseCodigo))
                                            {
                                                strFase = ListaParametricas.Where(r => r.Codigo == ContratacionProyectoAportante.ComponenteAportante.FirstOrDefault().FaseCodigo &&
                                                r.TipoDominioId == (int)EnumeratorTipoDominio.Fases).FirstOrDefault().Nombre;
                                            }

                                        }
                                        RegistrosFuentesUso = RegistrosFuentesUso.Replace(placeholderDominio.Nombre, strFase);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.COMPONENTE_FUENTES_USO:

                                        string strTipoComponente = string.Empty;
                                        if (ContratacionProyectoAportante.ComponenteAportante.Count() > 0)
                                        {
                                            if (!string.IsNullOrEmpty(ContratacionProyectoAportante.ComponenteAportante.FirstOrDefault().TipoComponenteCodigo))
                                                strTipoComponente = ListaParametricas.Where(r => r.Codigo == ContratacionProyectoAportante.ComponenteAportante.FirstOrDefault().TipoComponenteCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Componentes).FirstOrDefault().Nombre;
                                        }


                                        RegistrosFuentesUso = RegistrosFuentesUso.Replace(placeholderDominio.Nombre, strTipoComponente);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.USO_FUENTES_USO:
                                        RegistrosRegistrosUsosFuenteUsos = string.Empty;

                                        if (ContratacionProyectoAportante.ComponenteAportante.Count() > 0)
                                        {
                                            string strTipoUso = ListaParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Usos && r.Codigo == ComponenteUso.TipoUsoCodigo).FirstOrDefault().Nombre;

                                            RegistrosFuentesUso = RegistrosFuentesUso.Replace("[USO_FUENTES_USO]", strTipoUso);
                                            RegistrosFuentesUso = RegistrosFuentesUso.Replace("[VALOR_USO_FUENTE_USO]", "$" + String.Format("{0:n0}", ComponenteUso.ValorUso.ToString()));

                                        }
                                        RegistrosRegistrosUsosFuenteUsos = RegistrosRegistrosUsosFuenteUsos.Replace(placeholderDominio.Nombre, "USO FUENTES USO");
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.REGISTROS_USOS:
                                        RegistrosFuentesUso = RegistrosFuentesUso.Replace(placeholderDominio.Nombre, RegistrosRegistrosUsosFuenteUsos);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.ROWSPAN_CANTIDAD_USOS:
                                        RegistrosFuentesUso = RegistrosFuentesUso.Replace(placeholderDominio.Nombre, (ContratacionProyectoAportante.ComponenteAportante.ToList().Count()).ToString());
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.ROWSPAN_CANTIDAD_USOS_COMPONENTES:
                                        int cantidadComponentes = 0;
                                        int cantidadUsos = 1;

                                        RegistrosFuentesUso = RegistrosFuentesUso.Replace(placeholderDominio.Nombre, cantidadUsos.ToString());
                                        break;
                                }
                            }
                        }
                    }


                }
            }

            foreach (Dominio placeholderDominio in placeholders)
            {
                //ConstanCodigoVariablesPlaceHolders placeholder = (ConstanCodigoVariablesPlaceHolders)placeholderDominio.Codigo.ToString();

                switch (placeholderDominio.Codigo)
                {
                    case ConstanCodigoVariablesPlaceHolders.REGISTRO_FUENTE_USO:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, RegistrosFuentesUso);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.NUMERO_SOLICITUD:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pContratacion.NumeroSolicitud);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.FECHA_SOLICITUD:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pContratacion.FechaTramite != null ? ((DateTime)pContratacion.FechaTramite).ToString("yyyy-MM-dd") : " ");
                        break;

                    case ConstanCodigoVariablesPlaceHolders.OPCION_POR_CONTRATAR:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, ListaParametricas.Where(r => r.Codigo == pContratacion.TipoSolicitudCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud_Obra_Interventorias).FirstOrDefault().Nombre);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.VALOR_TOTAL_DE_LA_SOLICITUD:
                        decimal? ValorTotal = 0;
                        pContratacion.ContratacionProyecto.ToList().ForEach(cp =>
                        {
                            cp.ContratacionProyectoAportante.ToList().ForEach(cpa =>
                            {
                                cpa.ComponenteAportante.ToList().ForEach(ca =>
                                {
                                    ca.ComponenteUso.ToList().ForEach(cu =>
                                    {
                                        ValorTotal = ValorTotal + cu.ValorUso;
                                    });
                                });
                            });
                        });
                        //decimal? ValorTotal = pContratacion.ContratacionProyecto.Sum(r => r.Proyecto.ValorTotal);
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, "$" + String.Format("{0:n0}", ValorTotal));
                        break;

                    case ConstanCodigoVariablesPlaceHolders.CANTIDAD_DE_PROYECTOS_ASOCIADOS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pContratacion.ContratacionProyecto.Count().ToString());
                        break;
                    //Datos Contratista 
                    case ConstanCodigoVariablesPlaceHolders.CONTRATISTA_NOMBRE:

                        if (pContratacion.Contratista != null)
                        {
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pContratacion.Contratista.Nombre);
                        }
                        else
                        {
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, " ");
                        }
                        break;

                    case ConstanCodigoVariablesPlaceHolders.CONTRATISTA_NUMERO_IDENTIFICACION:
                        if (pContratacion.Contratista != null)
                        {
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pContratacion.Contratista.NumeroIdentificacion);
                        }
                        else
                        {
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, " ");
                        }

                        break;

                    case ConstanCodigoVariablesPlaceHolders.CONTRATISTA_NOMBRE_RE_LEGAL:
                        if (pContratacion.Contratista != null)
                        {
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pContratacion.Contratista.RepresentanteLegal);
                        }
                        else
                        {
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, " ");
                        }
                        break;

                    case ConstanCodigoVariablesPlaceHolders.CONTRATISTA_NUMERO_IDENTIFICACION_RE_LEGAL:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, " ");
                        break;

                    case ConstanCodigoVariablesPlaceHolders.CONTRATISTA_NUMERO_INVITACION:
                        if (pContratacion.Contratista != null)
                        {

                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pContratacion.Contratista.NumeroInvitacion);
                        }
                        else
                        {
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, "");
                        }

                        break;
                    //
                    case ConstanCodigoVariablesPlaceHolders.DETALLES_PROYECTOS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, DetallesProyectos);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.NUMERO_DE_LICENCIA:
                        string numeroLicencia = "";
                        if (pContratacion.ContratacionProyecto.Count() > 0)
                        {
                            numeroLicencia = pContratacion.ContratacionProyecto.FirstOrDefault().NumeroLicencia;
                        }
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, numeroLicencia);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.FECHA_DE_VIGENCIA:
                        string fechaVigencia = "";
                        if (pContratacion.ContratacionProyecto.Count() > 0)
                        {
                            if (pContratacion.ContratacionProyecto.FirstOrDefault().FechaVigencia != null)
                            {
                                fechaVigencia = ((DateTime)pContratacion.ContratacionProyecto.FirstOrDefault().FechaVigencia).ToString("yy-MM-dd");

                            }
                        }
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, fechaVigencia);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.CONSIDERACIONES_ESPECIALES:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pContratacion.ConsideracionDescripcion);
                        break;


                }
            }
            //Preguntas    //Preguntas 
            //Preguntas    //Preguntas 
            string strPregunta_1 = " ";

            string strPregunta_2 = "";
            string ContenidoPregunta2 = _context.Plantilla.Where(r => r.Codigo.Equals(((int)ConstanCodigoPlantillas.dos_pregunta_tecnica_o_juridica).ToString())).FirstOrDefault().Contenido;

            string strPregunta_3 = "";
            string ContenidoPregunta3 = _context.Plantilla.Where(r => r.Codigo.Equals(((int)ConstanCodigoPlantillas.tres_pregunta_tecnica_o_juridica).ToString())).FirstOrDefault().Contenido;

            string strPregunta_4 = "";
            string ContenidoPregunta4 = _context.Plantilla.Where(r => r.Codigo.Equals(((int)ConstanCodigoPlantillas.cuatro_pregunta_tecnica_o_juridica).ToString())).FirstOrDefault().Contenido;

            string strPregunta_5 = "";
            string ContenidoPregunta5 = _context.Plantilla.Where(r => r.Codigo.Equals(((int)ConstanCodigoPlantillas.cinco_pregunta_tecnica_o_juridica).ToString())).FirstOrDefault().Contenido;

            string strPregunta_6 = "";
            string ContenidoPregunta6 = _context.Plantilla.Where(r => r.Codigo.Equals(((int)ConstanCodigoPlantillas.seis_pregunta_tecnica_o_juridica).ToString())).FirstOrDefault().Contenido;


            if (pContratacion.ContratacionProyecto.Count() > 0)
            {

                //Pregunta 1 
                if (pContratacion.ContratacionProyecto.FirstOrDefault().TieneMonitoreoWeb == null ||
                    !(bool)pContratacion.ContratacionProyecto.FirstOrDefault().TieneMonitoreoWeb)
                {
                    //Si la respuesta a la pregunta 1, fue “No”, el sistema mostrará la pregunta 4
                    strPregunta_1 = " no";
                    // strPregunta_4 = ContenidoPregunta4 + " " + (pContratacion.ContratacionProyecto.FirstOrDefault().PorcentajeAvanceObra).ToString() + "%";
                }
                else
                {
                    //Si la respuesta fue “Si”, el sistema mostrará la pregunta 2.  
                    strPregunta_1 = " si";

                    //pregunta 2
                    if (pContratacion.ContratacionProyecto.FirstOrDefault().EsReasignacion == null
                        || !(bool)pContratacion.ContratacionProyecto.FirstOrDefault().EsReasignacion)
                    {
                        //pregunta 5
                        if (pContratacion.ContratacionProyecto.FirstOrDefault().RequiereLicencia == null
                            || !(bool)pContratacion.ContratacionProyecto.FirstOrDefault().RequiereLicencia)
                        {

                        }
                        else
                        {
                            strPregunta_5 = ContenidoPregunta5 + " si";
                            strPregunta_6 = ContenidoPregunta6 + " " + pContratacion.ContratacionProyecto.FirstOrDefault().LicenciaVigente;
                        }
                    }
                    else
                    {
                        strPregunta_2 = ContenidoPregunta2 + " si";
                        //pregunta 3
                        if (pContratacion.ContratacionProyecto.FirstOrDefault().EsAvanceobra == null
                           || !(bool)pContratacion.ContratacionProyecto.FirstOrDefault().EsAvanceobra)
                        {

                        }
                        else
                        {

                            strPregunta_3 = ContenidoPregunta3 + " si";
                            strPregunta_4 = ContenidoPregunta4 + " " + (pContratacion.ContratacionProyecto.FirstOrDefault().PorcentajeAvanceObra).ToString() + "%";
                            //pregunta 5
                            if (pContratacion.ContratacionProyecto.FirstOrDefault().RequiereLicencia == null
                                || !(bool)pContratacion.ContratacionProyecto.FirstOrDefault().RequiereLicencia)
                            {

                            }
                            else
                            {
                                strPregunta_5 = ContenidoPregunta5 + " si";
                                if (pContratacion.ContratacionProyecto.FirstOrDefault().LicenciaVigente == null || !(bool)pContratacion.ContratacionProyecto.FirstOrDefault().LicenciaVigente)
                                {
                                    strPregunta_6 = ContenidoPregunta6 + " no";
                                }
                                {
                                    strPregunta_6 = ContenidoPregunta6 + " si";
                                }

                            }

                        }
                    }

                }
            }




            pPlantilla = pPlantilla.Replace("[PREGUNTA_1]", strPregunta_1);
            pPlantilla = pPlantilla.Replace("[PREGUNTA_2]", strPregunta_2);
            pPlantilla = pPlantilla.Replace("[PREGUNTA_3]", strPregunta_3);
            pPlantilla = pPlantilla.Replace("[PREGUNTA_4]", strPregunta_4);
            pPlantilla = pPlantilla.Replace("[PREGUNTA_5]", strPregunta_5);
            pPlantilla = pPlantilla.Replace("[PREGUNTA_6]", strPregunta_6);
            return pPlantilla;
        }

        private byte[] ConvertirPDF(Plantilla pPlantilla)
        {
            string strEncabezado = "";
            if (!string.IsNullOrEmpty(pPlantilla.Encabezado.Contenido))
            {
                strEncabezado = Helpers.Helpers.HtmlStringLimpio(pPlantilla.Encabezado.Contenido);
            }

            var globalSettings = new GlobalSettings
            {
                ImageQuality = 1080,
                PageOffset = 0,
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings
                {
                    Top = pPlantilla.MargenArriba,
                    Left = pPlantilla.MargenIzquierda,
                    Right = pPlantilla.MargenDerecha,
                    Bottom = pPlantilla.MargenAbajo
                },
                DocumentTitle = DateTime.Now.ToString(),
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = pPlantilla.Contenido,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "pdf-styles.css") },
                HeaderSettings = { FontName = "Roboto", FontSize = 8, Center = strEncabezado, Line = false, Spacing = 18 },
                FooterSettings = { FontName = "Ariel", FontSize = 10, Center = "[page]" },
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings },
            };

            return _converter.Convert(pdf);
        }

        private async Task<byte[]> ReplacePlantillaFichaContratacion(int pContratacionId)
        {
            Contratacion contratacion = await _IProjectContractingService.GetAllContratacionByContratacionId(pContratacionId);

            if (contratacion == null)
            {
                return Array.Empty<byte>();
            }

            string TipoPlantilla = ((int)ConstanCodigoPlantillas.Ficha_De_Contratacion).ToString();

            Plantilla Plantilla = _context.Plantilla.Where(r => r.Codigo == TipoPlantilla).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();
            Plantilla.Contenido = ReemplazarDatosPlantillaContratacion(Plantilla.Contenido, contratacion);
            return ConvertirPDF(Plantilla);

        }

        private string ReemplazarDatosPlantillaProcesosSeleccion(string pPlantilla, ProcesoSeleccion pProcesoSeleccion)
        {
            pProcesoSeleccion.ProcesoSeleccionProponente = _context.ProcesoSeleccionProponente.Where( r => r.ProcesoSeleccionId == pProcesoSeleccion.ProcesoSeleccionId).ToList();
            pProcesoSeleccion.ProcesoSeleccionCotizacion = _context.ProcesoSeleccionCotizacion.Where( r => r.ProcesoSeleccionId == pProcesoSeleccion.ProcesoSeleccionId).ToList();

            List<Dominio> placeholders = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.PlaceHolder).ToList();
            List<Usuario> listaUsuarios = _context.Usuario.ToList();

            string TipoPlantillaRegistrosGruposProcesoSeleccion = ((int)ConstanCodigoPlantillas.Registros_Grupos_Proceso_Seleccion).ToString();
            string DetalleGrupoProcesosSeleccion = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaRegistrosGruposProcesoSeleccion).Select(r => r.Contenido).FirstOrDefault();
            string DetallesGrupoProcesosSeleccion = "";

            string TipoPlantillaRegistrosCronograma = ((int)ConstanCodigoPlantillas.Registros_Cronograma_Proceso_seleccion).ToString();
            string RegistroCronograma = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaRegistrosCronograma).Select(r => r.Contenido).FirstOrDefault();
            string RegistrosCronogramas = "";

            string TipoPlantillaProcesoSeleccionPrivada = ((int)ConstanCodigoPlantillas.Proceso_de_seleccion_Privada).ToString();
            string ProcesoSeleccionPrivada = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaProcesoSeleccionPrivada).Select(r => r.Contenido).FirstOrDefault();
            string ProcesosSeleccionPrivada = "";

            string TipoPlantillaProcesoSeleccionCerrada = ((int)ConstanCodigoPlantillas.Proceso_de_seleccion_Cerrada).ToString();
            string ProcesoSeleccionCerrada = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaProcesoSeleccionCerrada).Select(r => r.Contenido).FirstOrDefault();
            string ProcesosSeleccionCerrada = "";

            string TipoPlantillaProcesoSeleccionAbierta = ((int)ConstanCodigoPlantillas.Proceso_de_seleccion_Abierta).ToString();
            string ProcesoSeleccionAbierta = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaProcesoSeleccionAbierta).Select(r => r.Contenido).FirstOrDefault();
            string ProcesosSeleccionAbierta = " ";

            List<Dominio> ListaParametricas = _context.Dominio.ToList();

            //Plantilla Grupos de seleccion
            foreach (var ProcesoSeleccionGrupo in pProcesoSeleccion.ProcesoSeleccionGrupo)
            {
                DetallesGrupoProcesosSeleccion += DetalleGrupoProcesosSeleccion;

                foreach (Dominio placeholderDominio in placeholders)
                {
                    switch (placeholderDominio.Codigo)
                    {
                        case ConstanCodigoVariablesPlaceHolders.NOMBRE_GRUPO_PS:
                            DetallesGrupoProcesosSeleccion = DetallesGrupoProcesosSeleccion
                                .Replace(placeholderDominio.Nombre, ProcesoSeleccionGrupo.NombreGrupo);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.PRESUPUESTO_OFICIAL_PS:
                            DetallesGrupoProcesosSeleccion = DetallesGrupoProcesosSeleccion
                                 .Replace(placeholderDominio.Nombre,
                                 !string.IsNullOrEmpty(ProcesoSeleccionGrupo.TipoPresupuestoCodigo) ?
                            ListaParametricas.Where(r => r.Codigo == ProcesoSeleccionGrupo.TipoPresupuestoCodigo
                            && r.TipoDominioId == (int)EnumeratorTipoDominio.Presupuesto_Proceso_de_Selección)
                            .FirstOrDefault().Nombre
                            : "");
                            break;

                        case ConstanCodigoVariablesPlaceHolders.PLAZO_EN_MESES_PS:
                            DetallesGrupoProcesosSeleccion = DetallesGrupoProcesosSeleccion
                                .Replace(placeholderDominio.Nombre, ProcesoSeleccionGrupo.PlazoMeses.ToString());
                            break;
                    }
                }
            }

            //Plantilla Cronograma 
            foreach (var ProcesoSeleccionCronograma in pProcesoSeleccion.ProcesoSeleccionCronograma)
            {
                RegistrosCronogramas += RegistroCronograma;

                foreach (Dominio placeholderDominio in placeholders)
                {
                    switch (placeholderDominio.Codigo)
                    {
                        case ConstanCodigoVariablesPlaceHolders.ACTIVIDAD_CRONOGRAMA_PS:
                            // RegistrosCronogramas = RegistrosCronogramas.Replace(placeholderDominio.Nombre,
                            // !string.IsNullOrEmpty(ProcesoSeleccionCronograma.EstadoActividadCodigo) ?
                            // ListaParametricas
                            // .Where(r => r.Codigo == ProcesoSeleccionCronograma.EstadoActividadCodigo
                            // && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_alcance)
                            // .FirstOrDefault().Nombre : " ");
                            RegistrosCronogramas = RegistrosCronogramas.Replace(placeholderDominio.Nombre, ProcesoSeleccionCronograma.Descripcion);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.FECHA_CRONOGRAMA_PS:
                            RegistrosCronogramas = RegistrosCronogramas.Replace(placeholderDominio.Nombre,
                             ProcesoSeleccionCronograma.FechaMaxima.Value.ToString("dd-MM-yyyy"));
                            break;
                    }
                }
            }

            //Plantilla que Depende del Tipo de proceso de solicitud

            switch (pProcesoSeleccion.TipoProcesoCodigo)
            {
                case ConstanCodigoTipoProcesoSeleccion.Invitacion_Abierta:
                    ProcesosSeleccionAbierta = ProcesoSeleccionAbierta;
                    foreach (Dominio placeholderDominio in placeholders)
                    {
                        switch (placeholderDominio.Codigo)
                        {
                            case ConstanCodigoVariablesPlaceHolders.CONDICIONES_JURIDICAS_HABILITANTES_ABIERTA_PS:
                                ProcesosSeleccionAbierta = ProcesosSeleccionAbierta.
                                  Replace(placeholderDominio.Nombre, pProcesoSeleccion.CondicionesJuridicasHabilitantes);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.CONDICIONES_FINANCIERAS_HABILITANTES_ABIERTA_PS:
                                ProcesosSeleccionAbierta = ProcesosSeleccionAbierta.
                                  Replace(placeholderDominio.Nombre, pProcesoSeleccion.CondicionesFinancierasHabilitantes);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.CONDICIONES_TECNICAS_HABILITANTES_ABIERTA_PS:
                                ProcesosSeleccionAbierta = ProcesosSeleccionAbierta.
                                  Replace(placeholderDominio.Nombre, pProcesoSeleccion.CondicionesTecnicasHabilitantes);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.CONDICIONES_ASIGNACION_PUNTAJE_ABIERTA_PS:
                                ProcesosSeleccionAbierta = ProcesosSeleccionAbierta.
                                  Replace(placeholderDominio.Nombre, pProcesoSeleccion.CondicionesAsignacionPuntaje);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.RESPONSABLES_ABIERTA_PS:
                                string NombresPreponente = "";

                                Usuario responsable = listaUsuarios.Find( r => r.UsuarioId == pProcesoSeleccion.ResponsableTecnicoUsuarioId );

                                if ( responsable != null )
                                    NombresPreponente = string.Concat(responsable.Nombres, " ", responsable.Apellidos, " - ");  

                                responsable = listaUsuarios.Find( r => r.UsuarioId == pProcesoSeleccion.ResponsableEstructuradorUsuarioid );

                                if ( responsable != null )
                                    NombresPreponente += string.Concat(responsable.Nombres, " ", responsable.Apellidos, " - ");  

                                ProcesosSeleccionAbierta = ProcesosSeleccionAbierta.
                                Replace(placeholderDominio.Nombre, NombresPreponente);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.EVALUACION_DESCRIPCION_ABIERTA_PS:
                                ProcesosSeleccionAbierta = ProcesosSeleccionAbierta.
                                  Replace(placeholderDominio.Nombre, pProcesoSeleccion.EvaluacionDescripcion);
                                break;
                        }
                    }

                    break;
                case ConstanCodigoTipoProcesoSeleccion.Invitacion_Cerrada:
                    ProcesosSeleccionCerrada = ProcesoSeleccionCerrada;
                    foreach (Dominio placeholderDominio in placeholders)
                    {
                        switch (placeholderDominio.Codigo)
                        {
                            case ConstanCodigoVariablesPlaceHolders.CRITERIOS_SELECCION_CERRADA_PS:
                                ProcesosSeleccionCerrada = ProcesosSeleccionCerrada.
                                  Replace(placeholderDominio.Nombre, pProcesoSeleccion.CriteriosSeleccion);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.RESPONSABLES_CERRADA_PS:
                                string NombresPreponente = "";

                                Usuario responsable = listaUsuarios.Find( r => r.UsuarioId == pProcesoSeleccion.ResponsableTecnicoUsuarioId );

                                if ( responsable != null )
                                    NombresPreponente = string.Concat(responsable.Nombres, " ", responsable.Apellidos, " - ");  

                                responsable = listaUsuarios.Find( r => r.UsuarioId == pProcesoSeleccion.ResponsableEstructuradorUsuarioid );

                                if ( responsable != null )
                                    NombresPreponente += string.Concat(responsable.Nombres, " ", responsable.Apellidos, " - ");  

                                ProcesosSeleccionCerrada = ProcesosSeleccionCerrada.
                                Replace(placeholderDominio.Nombre, NombresPreponente);
                                break;

                            //[4:02 PM, 8/26/2020] Faber Ivolucion: se campo no tiene descripción
                            //[4:03 PM, 8 / 26 / 2020] Faber Ivolucion: no se si lo quitaron o ya en aparece algo en el control de cambios
                            //    [4:04 PM, 8 / 26 / 2020] JULIÁN MARTÍNEZ C: y el VALOR_CONTIZACION_CERRADA
                            //        [4:12 PM, 8 / 26 / 2020] Faber Ivolucion: Tampoco aparece en CU

                            case ConstanCodigoVariablesPlaceHolders.NOMBRE_ORGANIZACION_CERRADA_PS:

                                pProcesoSeleccion.ProcesoSeleccionCotizacion.ToList().ForEach( c => {
                                    ProcesosSeleccionCerrada = ProcesosSeleccionCerrada.
                                  Replace(placeholderDominio.Nombre, c.NombreOrganizacion);                
                                });

                                break; 

                            case ConstanCodigoVariablesPlaceHolders.VALOR_CONTIZACION_CERRADA_PS:
                                pProcesoSeleccion.ProcesoSeleccionCotizacion.ToList().ForEach( c => {
                                    ProcesosSeleccionCerrada = ProcesosSeleccionCerrada.
                                  Replace(placeholderDominio.Nombre,"$" + String.Format("{0:n0}", c.ValorCotizacion) );                
                                });

                               break;


                            case ConstanCodigoVariablesPlaceHolders.EVALUACION_DESCRIPCION_CERRADA_PS:
                                ProcesosSeleccionCerrada = ProcesosSeleccionCerrada.
                                  Replace(placeholderDominio.Nombre, pProcesoSeleccion.EvaluacionDescripcion);
                                break;
                        }
                    }
                    break;

                case ConstanCodigoTipoProcesoSeleccion.Invitacion_Privada:
                    ProcesosSeleccionPrivada = ProcesoSeleccionPrivada;
                    foreach (Dominio placeholderDominio in placeholders)
                    {
                        switch (placeholderDominio.Codigo)
                        {
                            case ConstanCodigoVariablesPlaceHolders.TIPO_PROPONENTE_PRIVADA_PS:

                                ProcesosSeleccionPrivada = ProcesosSeleccionPrivada.
                                  Replace(placeholderDominio.Nombre, (pProcesoSeleccion.ProcesoSeleccionProponente.Count() > 0) ? ListaParametricas
                                  .Where(r => r.Codigo == pProcesoSeleccion.ProcesoSeleccionProponente.FirstOrDefault().TipoProponenteCodigo
                                  && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Proponente
                                  ).FirstOrDefault().Nombre : " ");
                                break;

                            case ConstanCodigoVariablesPlaceHolders.NOMBRE_PRIVADA_PS:

                                ProcesosSeleccionPrivada = ProcesosSeleccionPrivada.
                                  Replace(placeholderDominio.Nombre, (pProcesoSeleccion.ProcesoSeleccionProponente.Count() > 0) ? pProcesoSeleccion.ProcesoSeleccionProponente.FirstOrDefault().NombreProponente : "");
                                break;

                            case ConstanCodigoVariablesPlaceHolders.TIPO_DOCUMENTO_PRIVADA_PS:
                            if ( pProcesoSeleccion.ProcesoSeleccionProponente.FirstOrDefault().TipoIdentificacionCodigo != null )
                                {
                                ProcesosSeleccionPrivada = ProcesosSeleccionPrivada.
                                Replace(placeholderDominio.Nombre, (pProcesoSeleccion.ProcesoSeleccionProponente.Count() > 0)
                                ? ListaParametricas.Where(r => r.Codigo == pProcesoSeleccion.ProcesoSeleccionProponente.FirstOrDefault().TipoIdentificacionCodigo
                                && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Documento
                                ).FirstOrDefault().Nombre : " ");
                                }else {
                                    ProcesosSeleccionPrivada = ProcesosSeleccionPrivada.
                                    Replace(placeholderDominio.Nombre, "");
                                }
                                break;

                            case ConstanCodigoVariablesPlaceHolders.NOMBRE_REPRESENTANTE_LEGAL_PRIVADA_PS:
                                ProcesosSeleccionPrivada = ProcesosSeleccionPrivada.
                               Replace(placeholderDominio.Nombre, (pProcesoSeleccion.ProcesoSeleccionProponente.Count() > 0) ? pProcesoSeleccion.ProcesoSeleccionProponente.FirstOrDefault().NombreRepresentanteLegal : "");

                                break;
                        }
                    }
                    break;
            }

            //Plantilla Principal
            foreach (Dominio placeholderDominio in placeholders)
            {
                switch (placeholderDominio.Codigo)
                {
                    case ConstanCodigoVariablesPlaceHolders.NUMERO_PROCESO_PS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pProcesoSeleccion.NumeroProceso);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.FECHA_SOLICITUD_PS:
                        //TODO: DOnde se guarda la fecha de solicitud = fecha creacion
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pProcesoSeleccion.FechaCreacion.ToString("yyyy-MM-dd"));
                        break;

                    case ConstanCodigoVariablesPlaceHolders.TIPO_SOLICITUD_PS:
                        pPlantilla = pPlantilla.Replace
                            (placeholderDominio.Nombre,
                            !string.IsNullOrEmpty(pProcesoSeleccion.TipoProcesoCodigo) ?
                            ListaParametricas
                            .Where(r => r.Codigo == pProcesoSeleccion.TipoProcesoCodigo
                            && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Proceso_Seleccion)
                            .FirstOrDefault().Nombre : "");
                        break;

                    case ConstanCodigoVariablesPlaceHolders.OBJETO_PS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pProcesoSeleccion.Objeto);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.ALCANCE_PARTICULAR_PS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pProcesoSeleccion.AlcanceParticular);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.JUSTIFICACION_PS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pProcesoSeleccion.Justificacion);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.TIPO_INTERVENCION_PS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre,
                            !string.IsNullOrEmpty(pProcesoSeleccion.TipoIntervencionCodigo) ?
                            ListaParametricas.Where(r => r.Codigo == pProcesoSeleccion.TipoIntervencionCodigo
                            && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion
                            ).FirstOrDefault().Nombre
                            : "");
                        break;

                    case ConstanCodigoVariablesPlaceHolders.TIPO_ALCANCE_PS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre,
                             !string.IsNullOrEmpty(pProcesoSeleccion.TipoAlcanceCodigo) ?
                             ListaParametricas
                             .Where(r => r.Codigo == pProcesoSeleccion.TipoAlcanceCodigo
                             && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_alcance)
                             .FirstOrDefault().Nombre
                             : ""); break;

                    case ConstanCodigoVariablesPlaceHolders.DISTRIBUCION_TERRITORIO_PS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pProcesoSeleccion.EsDistribucionGrupos != null ?
                            (bool)pProcesoSeleccion.EsDistribucionGrupos ? "Si" : "No" : " ");
                        break;

                    case ConstanCodigoVariablesPlaceHolders.CUANTOS_GRUPOS_PS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pProcesoSeleccion.CantGrupos.ToString());
                        break;

                    ///Plantillas dinamicas
                    ///
                    case ConstanCodigoVariablesPlaceHolders.REGISTROS_GRUPOS_PS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, DetallesGrupoProcesosSeleccion);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.REGISTROS_CRONOGRAMA_PS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, RegistrosCronogramas);
                        break;
                    case ConstanCodigoVariablesPlaceHolders.PROCESO_PRIVADA_PS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, ProcesosSeleccionPrivada);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.PROCESO_CERRADA_PS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, ProcesosSeleccionCerrada);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.PROCESO_ABIERTA_PS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, ProcesosSeleccionAbierta);
                        break;
                }
            }


            return pPlantilla;

        }

        private async Task<byte[]> ReplacePlantillaProcesosSeleccion(int pProcesoSeleccionId)
        {
            ProcesoSeleccion procesoSeleccion = await _context.ProcesoSeleccion
                .Where(r => r.ProcesoSeleccionId == pProcesoSeleccionId)
                .IncludeFilter(r => r.ProcesoSeleccionCronograma.Where(r => !(bool)r.Eliminado))
                .IncludeFilter(r => r.ProcesoSeleccionGrupo.Where(r => !(bool)r.Eliminado))
                //Aqui falta filtrarlos proponentes ya que en model y en codigo no de guarda eliminado
                .Include(r => r.ProcesoSeleccionProponente)
                .FirstOrDefaultAsync();

            if (procesoSeleccion == null)
            {
                return Array.Empty<byte>();
            }

            string TipoPlantilla = ((int)ConstanCodigoPlantillas.Ficha_De_Procesos_De_Seleccion).ToString();

            Plantilla Plantilla = _context.Plantilla.Where(r => r.Codigo == TipoPlantilla).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();
            Plantilla.Contenido = ReemplazarDatosPlantillaProcesosSeleccion(Plantilla.Contenido, procesoSeleccion);
            return ConvertirPDF(Plantilla);

        }

        public async Task<byte[]> GetPlantillaByTablaIdRegistroId(string pTablaId, int pRegistroId)
        {
            return pTablaId switch
            {
                ConstanCodigoTipoSolicitud.Contratacion => await ReplacePlantillaFichaContratacion(pRegistroId),
                ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion => await ReplacePlantillaProcesosSeleccion(pRegistroId),
                _ => Array.Empty<byte>(),
            };
        }

        public async Task<Respuesta> CreateEditSesionComiteTema(List<SesionComiteTema> ListSesionComiteTemas)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Sesion_Comite_Tema, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = "";
            try
            {
                foreach (var SesionComiteTema in ListSesionComiteTemas)
                {
                    if (SesionComiteTema.SesionTemaId == 0)
                    {
                        CreateEdit = "CREAR SESIÓN COMITE TEMA";
                        SesionComiteTema.FechaCreacion = DateTime.Now;
                        SesionComiteTema.UsuarioCreacion = ListSesionComiteTemas.FirstOrDefault().UsuarioCreacion;
                        SesionComiteTema.Eliminado = false;
                        _context.SesionComiteTema.Add(SesionComiteTema);
                    }
                    else
                    {
                        CreateEdit = "EDITAR SESIÓN COMITE TEMA";
                        SesionComiteTema sesionComiteTemaOld = _context.SesionComiteTema.Find(SesionComiteTema.SesionTemaId);
                        sesionComiteTemaOld.UsuarioModificacion = ListSesionComiteTemas.FirstOrDefault().UsuarioCreacion;
                        sesionComiteTemaOld.FechaModificacion = DateTime.Now;

                        sesionComiteTemaOld.Tema = SesionComiteTema.Tema;
                        sesionComiteTemaOld.ResponsableCodigo = SesionComiteTema.ResponsableCodigo;
                        sesionComiteTemaOld.TiempoIntervencion = SesionComiteTema.TiempoIntervencion;
                        sesionComiteTemaOld.RutaSoporte = SesionComiteTema.RutaSoporte;
                        sesionComiteTemaOld.Observaciones = SesionComiteTema.Observaciones;
                        sesionComiteTemaOld.EsAprobado = SesionComiteTema.EsAprobado;
                        sesionComiteTemaOld.ObservacionesDecision = SesionComiteTema.ObservacionesDecision;
                        sesionComiteTemaOld.ComiteTecnicoId = SesionComiteTema.ComiteTecnicoId;
                        //sesionComiteTemaOld.EsProposicionesVarios = SesionComiteTema.EsProposicionesVarios;
                    }
                }
                _context.SaveChanges();

                return
                new Respuesta
                {
                    Data = await GetComiteTecnicoByComiteTecnicoId((int)ListSesionComiteTemas.FirstOrDefault().ComiteTecnicoId),
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantSesionComiteFiduciario.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.OperacionExitosa, idAccion, ListSesionComiteTemas.FirstOrDefault().UsuarioCreacion, CreateEdit)
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
                      Code = ConstantSesionComiteFiduciario.Error,
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.Error, idAccion, ListSesionComiteTemas.FirstOrDefault().UsuarioCreacion, ex.InnerException.ToString())
                  };
            }

        }

        public async Task<Respuesta> AplazarSesionComite(ComiteTecnico pComiteTecnico, string pDominio, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Aplazar_Sesion_De_Comite_Fiduciario, (int)EnumeratorTipoDominio.Acciones);
            List<Dominio> placeholders = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.PlaceHolder).ToList();
            DateTime fechaAnterior;

            try
            {
                ComiteTecnico comiteTecnicoOld = _context.ComiteTecnico.Find(pComiteTecnico.ComiteTecnicoId);

                fechaAnterior = comiteTecnicoOld.FechaOrdenDia.Value;

                comiteTecnicoOld.UsuarioModificacion = pComiteTecnico.UsuarioCreacion;
                comiteTecnicoOld.FechaModificacion = DateTime.Now;
                comiteTecnicoOld.FechaOrdenDia = pComiteTecnico.FechaAplazamiento;
                comiteTecnicoOld.FechaAplazamiento = pComiteTecnico.FechaAplazamiento;
                comiteTecnicoOld.EstadoComiteCodigo = ConstanCodigoEstadoComite.Aplazada;



                _context.SaveChanges();
                //Plantilla
                string TipoPlantilla = ((int)ConstanCodigoPlantillas.Aplazar_Comite_Tecnico).ToString();
                Plantilla plantilla = _context.Plantilla.Where(r => r.Codigo == TipoPlantilla).Include(r => r.Encabezado).FirstOrDefault();

                List<Dominio> ListaParametricas = _context.Dominio.ToList();

                foreach (Dominio placeholderDominio in placeholders)
                {
                    switch (placeholderDominio.Codigo)
                    {
                        case ConstanCodigoVariablesPlaceHolders.COMITE_NUMERO:
                            plantilla.Contenido = plantilla.Contenido.Replace(placeholderDominio.Nombre, comiteTecnicoOld.NumeroComite);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.COMITE_FECHA:
                            plantilla.Contenido = plantilla.Contenido.Replace(placeholderDominio.Nombre, fechaAnterior.ToString("dd-MM-yyyy"));
                            break;

                        case ConstanCodigoVariablesPlaceHolders.COMITE_FECHA_APLAZAMIENTO:
                            plantilla.Contenido = plantilla.Contenido.Replace(placeholderDominio.Nombre, ((DateTime)comiteTecnicoOld.FechaAplazamiento).ToString("dd-MM-yyyy"));
                            break;
                    }
                }


                //Notificar a los participantes 
                //TODO: esta lista debe ser parametrizada de acuerdo a los perfiles Directore de las 4 areas :
                //Director financiero, Director Juridico , Director técnico, y Director administrativo
                List<Usuario> ListMiembrosComite = await _commonService.GetUsuariosByPerfil((int)EnumeratorPerfil.Miembros_Comite);

                List<Usuario> UsuarioNoNotificados = new List<Usuario>();
                foreach (var Usuario in ListMiembrosComite)
                {
                    if (!string.IsNullOrEmpty(Usuario.Email))
                    {
                        if (!(bool)Helpers.Helpers.EnviarCorreo(Usuario.Email, "Aplazar sesión comité fiduciario", plantilla.Contenido, pSentender, pPassword, pMailServer, pMailPort))
                        {

                            UsuarioNoNotificados.Add(Usuario);
                        }
                    }
                }
                return
                  new Respuesta
                  {
                      Data = new List<dynamic>{
                         UsuarioNoNotificados
                      },
                      IsSuccessful = true,
                      IsException = false,
                      IsValidation = false,
                      Code = ConstantSesionComiteFiduciario.AplazarExitoso,
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.AplazarExitoso, idAccion, pComiteTecnico.UsuarioCreacion, "APLAZAR SESIÓN COMITE")
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
                   Code = ConstantSesionComiteTecnico.Error,
                   Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.Error, idAccion, pComiteTecnico.UsuarioCreacion, ex.InnerException.ToString())
               };
            }

        }

        public async Task<Respuesta> DeleteComiteTecnicoByComiteTecnicoId(int pComiteTecnicoId, string pUsuarioModifico)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Comite_Tecnico, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ComiteTecnico comiteTecnicoOld = _context.ComiteTecnico
                                                            .Where(ct => ct.ComiteTecnicoId == pComiteTecnicoId)
                                                            .Include(r => r.SesionComiteTema)
                                                            .Include(r => r.SesionComiteSolicitudComiteTecnicoFiduciario)
                                                            .FirstOrDefault();

                if (comiteTecnicoOld.SesionComiteTema.Where(t => t.Eliminado != true).ToList().Count > 0 ||
                     comiteTecnicoOld.SesionComiteSolicitudComiteTecnicoFiduciario.Where(c => c.Eliminado != true).ToList().Count > 0
                    )
                {
                    return new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = ConstantSesionComiteTecnico.ErrorEliminarDependencia,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.ErrorEliminarDependencia, idAccion, pUsuarioModifico, "ELIMINAR COMITE TECNICO")
                    };
                }


                comiteTecnicoOld.UsuarioModificacion = pUsuarioModifico;
                comiteTecnicoOld.FechaModificacion = DateTime.Now;
                comiteTecnicoOld.Eliminado = true;
                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.EliminacionExitosa, idAccion, pUsuarioModifico, "ELIMINAR COMITE TECNICO")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.Error, idAccion, pUsuarioModifico, ex.InnerException.ToString())
                };
            }

        }


        public async Task<List<SesionParticipante>> GetSesionParticipantesByIdComite(int pComiteId)
        {
            List<SesionParticipante> ListaParticipantes = new List<SesionParticipante>();
            try
            {

                ListaParticipantes = await _context.SesionParticipante
                .Where(r => r.ComiteTecnicoId == pComiteId && !(bool)r.Eliminado)
                .Include(r => r.SesionSolicitudObservacionProyecto)
                .ToListAsync();

                return ListaParticipantes;

            }
            catch (Exception)
            {
                return ListaParticipantes;
            }
        }

        public async Task<Respuesta> DeleteSesionInvitado(int pSesionInvitadoId, string pUsuarioModificacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Sesion_Invitado, (int)EnumeratorTipoDominio.Acciones);
            try
            {

                if (pSesionInvitadoId == 0)
                {
                    return
                     new Respuesta
                     {
                         IsSuccessful = false,
                         IsException = false,
                         IsValidation = true,
                         Code = ConstantSesionComiteTecnico.Error,
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.Error, idAccion, pUsuarioModificacion, "NO SE ENCONTRO SESION INVITADO")
                     };

                }
                SesionInvitado sesionInvitadoOld = await _context.SesionInvitado.FindAsync(pSesionInvitadoId);

                if (sesionInvitadoOld == null)
                {
                    return
                     new Respuesta
                     {
                         IsSuccessful = false,
                         IsException = false,
                         IsValidation = true,
                         Code = ConstantSesionComiteTecnico.Error,
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.Error, idAccion, pUsuarioModificacion, "NO SE ENCONTRO SESION INVITADO")
                     };
                }
                sesionInvitadoOld.UsuarioModificacion = pUsuarioModificacion;
                sesionInvitadoOld.FechaModificacion = DateTime.Now;
                sesionInvitadoOld.Eliminado = true;
                _context.SaveChanges();
                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccion, pUsuarioModificacion, "ELIMINAR SESIÓN INVITADO")
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
                      Code = ConstantSesionComiteTecnico.Error,
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccion, pUsuarioModificacion, ex.InnerException.ToString())
                  };
            }

        }

        public async Task<Respuesta> CreateEditSesionInvitadoAndParticipante(ComiteTecnico pComiteTecnico)
        {
            int idAccionCrearSesionParticipante = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Registrar_Participantes_Sesion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                foreach (var SesionParticipante in pComiteTecnico.SesionParticipante)
                {
                    if (SesionParticipante.SesionParticipanteId == 0)
                    {
                        _context.SesionParticipante.Add(new SesionParticipante
                        {
                            FechaCreacion = DateTime.Now,
                            Eliminado = false,
                            UsuarioCreacion = pComiteTecnico.UsuarioCreacion,
                            UsuarioId = SesionParticipante.UsuarioId,
                            ComiteTecnicoId = pComiteTecnico.ComiteTecnicoId,
                        });
                    }
                    else
                    {
                        SesionParticipante sesionParticipanteOld = _context.SesionParticipante.Find(SesionParticipante.SesionParticipanteId);
                        sesionParticipanteOld.UsuarioId = SesionParticipante.UsuarioId;
                        sesionParticipanteOld.UsuarioModificacion = pComiteTecnico.UsuarioCreacion;
                        sesionParticipanteOld.FechaModificacion = DateTime.Now;
                    }
                }
                foreach (var SesionInvitado in pComiteTecnico.SesionInvitado)
                {

                    if (SesionInvitado.SesionInvitadoId == 0)
                    {
                        _context.SesionInvitado.Add(new SesionInvitado
                        {
                            FechaCreacion = DateTime.Now,
                            UsuarioCreacion = pComiteTecnico.UsuarioCreacion,
                            Eliminado = false,
                            Cargo = SesionInvitado.Cargo,
                            ComiteTecnicoId = pComiteTecnico.ComiteTecnicoId,
                            Entidad = SesionInvitado.Entidad,
                            Nombre = SesionInvitado.Nombre,
                        });
                    }
                    else
                    {
                        SesionInvitado SesionInvitadoOld = _context.SesionInvitado.Find(SesionInvitado.SesionInvitadoId);
                        SesionInvitadoOld.FechaModificacion = DateTime.Now;
                        SesionInvitadoOld.UsuarioModificacion = pComiteTecnico.UsuarioCreacion;
                        SesionInvitadoOld.Nombre = SesionInvitado.Nombre;
                        SesionInvitadoOld.Cargo = SesionInvitado.Cargo;
                        SesionInvitadoOld.Entidad = SesionInvitado.Entidad;
                    }
                }
                _context.SaveChanges();
                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = ConstantSesionComiteTecnico.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.OperacionExitosa, idAccionCrearSesionParticipante, pComiteTecnico.UsuarioCreacion, "REGISTRAR PARTICIPANTES SESIÓN")
                    };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.Error, idAccionCrearSesionParticipante, pComiteTecnico.UsuarioCreacion, ex.InnerException.ToString())
                };
            }

        }

        public async Task<Respuesta> GetNoRequiereVotacionSesionComiteSolicitud(SesionComiteSolicitud pSesionComiteSolicitud)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.No_Requiere_Votacion_Sesion_Comite_Solicitud, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                SesionComiteSolicitud sesionComiteSolicitudOld = _context.SesionComiteSolicitud.Find(pSesionComiteSolicitud.SesionComiteSolicitudId);

                sesionComiteSolicitudOld.RequiereVotacionFiduciario = pSesionComiteSolicitud.RequiereVotacionFiduciario;
                sesionComiteSolicitudOld.UsuarioModificacion = pSesionComiteSolicitud.UsuarioCreacion;
                sesionComiteSolicitudOld.FechaModificacion = DateTime.Now;

                _context.SaveChanges();
                return

                new Respuesta
                {
                    //Data = await GetComiteTecnicoByComiteTecnicoId((int)pSesionComiteSolicitud.ComiteTecnicoId),
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.OperacionExitosa, idAccion, pSesionComiteSolicitud.UsuarioCreacion, "NO REQUIERE VOTACIÓN")
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
                      Code = ConstantSesionComiteTecnico.Error,
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.Error, idAccion, pSesionComiteSolicitud.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }

        }

        public async Task<Respuesta> NoRequiereVotacionSesionComiteTema(int idSesionComiteTema, bool pRequiereVotacion, string pUsuarioCreacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.No_Requiere_Votacion_Sesion_Comite_Tema, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                SesionComiteTema sesionComiteTemaOld = _context.SesionComiteTema.Find(idSesionComiteTema);
                sesionComiteTemaOld.RequiereVotacion = pRequiereVotacion;
                sesionComiteTemaOld.UsuarioModificacion = pUsuarioCreacion;
                sesionComiteTemaOld.FechaModificacion = DateTime.Now;

                _context.SaveChanges();
                return
                  new Respuesta
                  {
                      IsSuccessful = true,
                      IsException = false,
                      IsValidation = false,
                      Code = ConstantSesionComiteTecnico.OperacionExitosa,
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.OperacionExitosa, idAccion, pUsuarioCreacion, "NO REQUIERE VOTACIÓN SESIÓN COMITE TEMA")
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
                   Code = ConstantSesionComiteTecnico.Error,
                   Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.Error, idAccion, pUsuarioCreacion, ex.InnerException.ToString())
               };
            }

        }

        public async Task<Respuesta> CreateEditSesionSolicitudVoto(SesionComiteSolicitud pSesionComiteSolicitud)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Sesion_Solicitud_Voto, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = "";
            try
            {
                SesionComiteSolicitud sesionComiteSolicitudOld = _context.SesionComiteSolicitud.Find(pSesionComiteSolicitud.SesionComiteSolicitudId);
                sesionComiteSolicitudOld.EstadoCodigo = pSesionComiteSolicitud.EstadoCodigo;
                sesionComiteSolicitudOld.RequiereVotacionFiduciario = true;
                sesionComiteSolicitudOld.UsuarioModificacion = pSesionComiteSolicitud.UsuarioCreacion;
                sesionComiteSolicitudOld.FechaModificacion = DateTime.Now;

                foreach (var sesionSolicitudVoto in pSesionComiteSolicitud.SesionSolicitudVoto)
                {
                    if (sesionSolicitudVoto.SesionSolicitudVotoId == 0)
                    {
                        CreateEdit = "CREAR SOLICITUD VOTO";
                        sesionSolicitudVoto.UsuarioCreacion = pSesionComiteSolicitud.UsuarioCreacion;
                        sesionSolicitudVoto.Eliminado = false;
                        sesionSolicitudVoto.FechaCreacion = DateTime.Now;
                        _context.SesionSolicitudVoto.Add(sesionSolicitudVoto);
                        //sesionComiteSolicitudOld.SesionSolicitudVoto.Add(sesionSolicitudVoto);
                    }
                    else
                    {
                        CreateEdit = "EDITAR SOLICITUD VOTO";
                        SesionSolicitudVoto sesionSolicitudVotoOld = _context.SesionSolicitudVoto.Find(sesionSolicitudVoto.SesionSolicitudVotoId);

                        sesionSolicitudVotoOld.UsuarioModificacion = pSesionComiteSolicitud.UsuarioCreacion;
                        sesionSolicitudVotoOld.FechaModificacion = DateTime.Now;

                        sesionSolicitudVotoOld.EsAprobado = sesionSolicitudVoto.EsAprobado;
                        sesionSolicitudVotoOld.Observacion = sesionSolicitudVoto.Observacion;
                    }
                }

                //
                foreach (var SesionSolicitudObservacionProyecto in pSesionComiteSolicitud.SesionSolicitudObservacionProyecto)
                {
                    if (SesionSolicitudObservacionProyecto.SesionSolicitudObservacionProyectoId == 0)
                    {
                        SesionSolicitudObservacionProyecto.UsuarioCreacion = pSesionComiteSolicitud.UsuarioCreacion;
                        SesionSolicitudObservacionProyecto.FechaCreacion = DateTime.Now;
                        SesionSolicitudObservacionProyecto.Eliminado = false;
                        _context.SesionSolicitudObservacionProyecto.Add(SesionSolicitudObservacionProyecto);
                        //sesionComiteSolicitudOld.SesionSolicitudObservacionProyecto.Add( SesionSolicitudObservacionProyecto );
                    }
                    else
                    {
                        SesionSolicitudObservacionProyecto SesionSolicitudObservacionProyectoOld = _context.SesionSolicitudObservacionProyecto.Find(SesionSolicitudObservacionProyecto.SesionSolicitudObservacionProyectoId);
                        SesionSolicitudObservacionProyectoOld.Observacion = SesionSolicitudObservacionProyecto.Observacion;
                        SesionSolicitudObservacionProyectoOld.UsuarioModificacion = pSesionComiteSolicitud.UsuarioCreacion;
                        SesionSolicitudObservacionProyectoOld.FechaModificacion = DateTime.Now;
                    }

                }

                foreach (var observacionActualizacionCronograma in pSesionComiteSolicitud.SesionSolicitudObservacionActualizacionCronograma)
                {
                    if (observacionActualizacionCronograma.SesionSolicitudObservacionActualizacionCronogramaId == 0)
                    {
                        observacionActualizacionCronograma.UsuarioCreacion = pSesionComiteSolicitud.UsuarioCreacion;
                        observacionActualizacionCronograma.FechaCreacion = DateTime.Now;
                        observacionActualizacionCronograma.Eliminado = false;
                        _context.SesionSolicitudObservacionActualizacionCronograma.Add(observacionActualizacionCronograma);
                        //sesionComiteSolicitudOld.SesionSolicitudObservacionProyecto.Add( SesionSolicitudObservacionProyecto );
                    }
                    else
                    {
                        SesionSolicitudObservacionActualizacionCronograma observacionActualizacionCronogramaOld = _context.SesionSolicitudObservacionActualizacionCronograma.Find(observacionActualizacionCronograma.SesionSolicitudObservacionActualizacionCronogramaId);
                        observacionActualizacionCronogramaOld.Observacion = observacionActualizacionCronograma.Observacion;
                        observacionActualizacionCronogramaOld.UsuarioModificacion = pSesionComiteSolicitud.UsuarioCreacion;
                        observacionActualizacionCronogramaOld.FechaModificacion = DateTime.Now;
                    }
                }
                _context.SaveChanges();
                return
                new Respuesta
                {
                    //Data = await GetComiteTecnicoByComiteTecnicoId((int)pSesionComiteSolicitud.ComiteTecnicoId),
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.OperacionExitosa, idAccion, pSesionComiteSolicitud.UsuarioCreacion, CreateEdit)
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
                      Code = ConstantSesionComiteTecnico.Error,
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccion, pSesionComiteSolicitud.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }

        }

        private bool validarcompletosActa(int pComiteTecnicoId)
        {
            bool estaCompleto = true;

            ComiteTecnico comite = _context.ComiteTecnico.Where(ct => ct.ComiteTecnicoId == pComiteTecnicoId)
                                                         .Include(r => r.SesionComiteSolicitudComiteTecnicoFiduciario)
                                                         .Include(r => r.SesionComiteTema)
                                                        .FirstOrDefault();

            comite.SesionComiteSolicitudComiteTecnicoFiduciario.Where( t => t.Eliminado != true ).ToList().ForEach(cs =>
            {
                if ((cs.RegistroCompletoFiduciaria.HasValue ? cs.RegistroCompletoFiduciaria.Value : false) == false)
                    estaCompleto = false;
            });

            comite.SesionComiteTema.Where( t => t.Eliminado != true ).ToList().ForEach(ct =>
            {
                if ((ct.RegistroCompleto.HasValue ? ct.RegistroCompleto.Value : false) == false)
                    estaCompleto = false;
            });

            if (estaCompleto)
            {
                comite.EstadoComiteCodigo = ConstanCodigoEstadoComite.Con_Acta_De_Sesion_Enviada;
                comite.EsCompleto = true;
                _context.SaveChanges();
            }

            return estaCompleto;
        }

        public async Task<Respuesta> CreateEditActasSesionSolicitudCompromiso(SesionComiteSolicitud pSesionComiteSolicitud)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Edit_Sesion_Solicitud_Compromisos_ACTAS, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = "";
            try
            {
                SesionComiteSolicitud sesionComiteSolicitudOld = _context.SesionComiteSolicitud.Find(pSesionComiteSolicitud.SesionComiteSolicitudId);

                sesionComiteSolicitudOld.FechaModificacion = DateTime.Now;
                sesionComiteSolicitudOld.UsuarioModificacion = pSesionComiteSolicitud.UsuarioCreacion;
                sesionComiteSolicitudOld.EstadoCodigo = pSesionComiteSolicitud.EstadoCodigo;
                sesionComiteSolicitudOld.GeneraCompromisoFiduciario = pSesionComiteSolicitud.GeneraCompromisoFiduciario;
                sesionComiteSolicitudOld.DesarrolloSolicitudFiduciario = pSesionComiteSolicitud.DesarrolloSolicitudFiduciario;
                sesionComiteSolicitudOld.CantCompromisosFiduciario = pSesionComiteSolicitud.CantCompromisosFiduciario;
                sesionComiteSolicitudOld.ObservacionesFiduciario = pSesionComiteSolicitud.ObservacionesFiduciario;
                sesionComiteSolicitudOld.RutaSoporteVotacionFiduciario = pSesionComiteSolicitud.RutaSoporteVotacionFiduciario;
                sesionComiteSolicitudOld.RegistroCompletoFiduciaria = ValidarRegistroCompletoSesionComiteSolicitud(sesionComiteSolicitudOld);

                #region Contratacion

                if (pSesionComiteSolicitud.TipoSolicitud == ConstanCodigoTipoSolicitud.Contratacion)
                {
                    if (pSesionComiteSolicitud.Contratacion.ContratacionProyecto != null)
                    {
                        pSesionComiteSolicitud.Contratacion.ContratacionProyecto.ToList().ForEach(ct =>
                        {
                            Proyecto proy = _context.Proyecto.Find(ct.Proyecto.ProyectoId);
                            if (ct.Proyecto.EstadoProyectoCodigo != null)
                                if (ct.Proyecto.EstadoProyectoCodigo == ConstantCodigoEstadoProyecto.RechazadoComiteTecnico)
                                    proy.EstadoProyectoCodigo = ConstantCodigoEstadoProyecto.Disponible;
                                else
                                    proy.EstadoProyectoCodigo = ct.Proyecto.EstadoProyectoCodigo;
                            else
                            {
                                sesionComiteSolicitudOld.RegistroCompletoFiduciaria = false;
                            }
                        });

                    }

                    Contratacion contratacion = _context.Contratacion.Find(sesionComiteSolicitudOld.SolicitudId);

                    if (contratacion != null)
                    {
                        if (sesionComiteSolicitudOld.EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Aprobada_por_comite_fiduciario)
                        {
                            contratacion.EstadoSolicitudCodigo = ConstanCodigoEstadoSolicitudContratacion.AprobadoComiteFiduciario;
                        }
                        if (sesionComiteSolicitudOld.EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_fiduciario)
                        {
                            contratacion.EstadoSolicitudCodigo = ConstanCodigoEstadoSolicitudContratacion.DevueltoComiteFiduciario;
                        }
                        if (sesionComiteSolicitudOld.EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Rechazada_por_comite_fiduciario)
                        {
                            contratacion.EstadoSolicitudCodigo = ConstanCodigoEstadoSolicitudContratacion.RechazadoComiteFiduciario;
                        }
                    }

                }

                #endregion Contratacion

                #region Inicio Proceso Seleccion

                if (pSesionComiteSolicitud.TipoSolicitud == ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion)
                {
                    ProcesoSeleccion procesoSeleccion = _context.ProcesoSeleccion.Find(sesionComiteSolicitudOld.SolicitudId);
                    if (procesoSeleccion != null)
                    {
                        if (procesoSeleccion.EstadoProcesoSeleccionCodigo == ConstanCodigoEstadoProcesoSeleccion.AprobadaAperturaPorComiteTecnico)
                        {
                            switch (sesionComiteSolicitudOld.EstadoCodigo)
                            {
                                case ConstanCodigoEstadoSesionComiteSolicitud.Aprobada_por_comite_fiduciario:
                                    procesoSeleccion.EstadoProcesoSeleccionCodigo = ConstanCodigoEstadoProcesoSeleccion.AprobadaAperturaPorComiteFiduciario;
                                    break;
                                case ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_fiduciario:
                                    procesoSeleccion.EstadoProcesoSeleccionCodigo = ConstanCodigoEstadoProcesoSeleccion.DevueltaAperturaPorComiteFiduciario;
                                    break;
                                case ConstanCodigoEstadoSesionComiteSolicitud.Rechazada_por_comite_fiduciario:
                                    procesoSeleccion.EstadoProcesoSeleccionCodigo = ConstanCodigoEstadoProcesoSeleccion.RechazadaAperturaPorComiteFiduciario;
                                    break;
                            }
                        }
                        else if (procesoSeleccion.EstadoProcesoSeleccionCodigo == ConstanCodigoEstadoProcesoSeleccion.AprobadaSelecciónPorComiteTecnico)
                        {
                            switch (sesionComiteSolicitudOld.EstadoCodigo)
                            {
                                case ConstanCodigoEstadoSesionComiteSolicitud.Aprobada_por_comite_fiduciario:
                                    procesoSeleccion.EstadoProcesoSeleccionCodigo = ConstanCodigoEstadoProcesoSeleccion.AprobadaSelecciónPorComiteFiduciario;
                                    break;
                                case ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_fiduciario:
                                    procesoSeleccion.EstadoProcesoSeleccionCodigo = ConstanCodigoEstadoProcesoSeleccion.DevueltaSeleccionPorComiteFiduciario;
                                    break;
                                case ConstanCodigoEstadoSesionComiteSolicitud.Rechazada_por_comite_fiduciario:
                                    procesoSeleccion.EstadoProcesoSeleccionCodigo = ConstanCodigoEstadoProcesoSeleccion.RechazadaSeleccionPorComiteFiduciario;
                                    break;
                            }
                        }
                        else if (procesoSeleccion.EstadoProcesoSeleccionCodigo == ConstanCodigoEstadoProcesoSeleccion.AprobadoPorComiteTecnico)
                        {
                            switch (sesionComiteSolicitudOld.EstadoCodigo)
                            {
                                case ConstanCodigoEstadoSesionComiteSolicitud.Aprobada_por_comite_fiduciario:
                                    procesoSeleccion.EstadoProcesoSeleccionCodigo = ConstanCodigoEstadoProcesoSeleccion.AprobadoPorComiteFiduciario;
                                    break;
                                case ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_fiduciario:
                                    procesoSeleccion.EstadoProcesoSeleccionCodigo = ConstanCodigoEstadoProcesoSeleccion.DevueltoPorComiteFiduciario;
                                    break;
                                case ConstanCodigoEstadoSesionComiteSolicitud.Rechazada_por_comite_fiduciario:
                                    procesoSeleccion.EstadoProcesoSeleccionCodigo = ConstanCodigoEstadoProcesoSeleccion.RechazadoPorComiteFiduciario;
                                    break;
                            }
                        }


                    }

                }

                #endregion Inicio Proceo Seleccion

                #region Actualizacion Cronograma

                if (pSesionComiteSolicitud.TipoSolicitud == ConstanCodigoTipoSolicitud.Actualizacion_Cronograma_Proceso_Seleccion)
                {
                    ProcesoSeleccionMonitoreo procesoSeleccionMonitoreo = _context.ProcesoSeleccionMonitoreo.Find(sesionComiteSolicitudOld.SolicitudId);



                    if (procesoSeleccionMonitoreo != null)
                    {
                        if (sesionComiteSolicitudOld.EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Aprobada_por_comite_fiduciario)
                        {
                            procesoSeleccionMonitoreo.EstadoActividadCodigo = ConstanCodigoEstadoActividadCronogramaProcesoSeleccion.AprobadoPorComiteFiduciario;

                            List<ProcesoSeleccionCronogramaMonitoreo> listaCronograma = _context.ProcesoSeleccionCronogramaMonitoreo
                                                                                        .Where(r => r.ProcesoSeleccionMonitoreoId == procesoSeleccionMonitoreo.ProcesoSeleccionMonitoreoId)
                                                                                        .ToList();


                            listaCronograma.ForEach(crono =>
                                {
                                    if (crono.ProcesoSeleccionCronogramaId > 0)
                                    {
                                        ProcesoSeleccionCronograma cronograma = _context.ProcesoSeleccionCronograma.Find(crono.ProcesoSeleccionCronogramaId);
                                        if (cronograma != null)
                                        {
                                            cronograma.UsuarioModificacion = pSesionComiteSolicitud.UsuarioCreacion;
                                            cronograma.FechaModificacion = DateTime.Now;

                                            cronograma.NumeroActividad = crono.NumeroActividad;
                                            cronograma.Descripcion = crono.Descripcion;
                                            cronograma.FechaMaxima = crono.FechaMaxima;
                                            cronograma.EstadoActividadCodigo = crono.EstadoActividadCodigo;

                                        }
                                    }
                                    else
                                    {
                                        ProcesoSeleccionCronograma cronograma = new ProcesoSeleccionCronograma()
                                        {

                                            UsuarioCreacion = pSesionComiteSolicitud.UsuarioCreacion,
                                            FechaCreacion = DateTime.Now,

                                            ProcesoSeleccionId = procesoSeleccionMonitoreo.ProcesoSeleccionId,
                                            Eliminado = false,
                                            NumeroActividad = crono.NumeroActividad,
                                            Descripcion = crono.Descripcion,
                                            FechaMaxima = crono.FechaMaxima,
                                            EstadoActividadCodigo = crono.EstadoActividadCodigo,
                                        };

                                        _context.ProcesoSeleccionCronograma.Add(cronograma);
                                    }
                                });

                        }
                        if (sesionComiteSolicitudOld.EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Rechazada_por_comite_fiduciario)
                        {
                            procesoSeleccionMonitoreo.EstadoActividadCodigo = ConstanCodigoEstadoActividadCronogramaProcesoSeleccion.RechazadoPorComiteFiduciario;
                        }
                        if (sesionComiteSolicitudOld.EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_fiduciario)
                        {
                            procesoSeleccionMonitoreo.EstadoActividadCodigo = ConstanCodigoEstadoActividadCronogramaProcesoSeleccion.DevueltoPorComiteFiduciario;
                        }





                    }

                }

                #endregion

                foreach (var SesionSolicitudCompromiso in pSesionComiteSolicitud.SesionSolicitudCompromiso)
                {
                    if (SesionSolicitudCompromiso.SesionSolicitudCompromisoId == 0)
                    {
                        CreateEdit = "CREAR SOLICITUD COMPROMISO";
                        SesionSolicitudCompromiso.UsuarioCreacion = pSesionComiteSolicitud.UsuarioCreacion;
                        SesionSolicitudCompromiso.FechaCreacion = DateTime.Now;
                        SesionSolicitudCompromiso.Eliminado = false;
                        SesionSolicitudCompromiso.EsFiduciario = true;

                        _context.SesionSolicitudCompromiso.Add(SesionSolicitudCompromiso);
                    }
                    else
                    {
                        CreateEdit = "EDITAR SOLICITUD COMPROMISO";
                        SesionSolicitudCompromiso sesionSolicitudCompromisoOld = _context.SesionSolicitudCompromiso.Find(SesionSolicitudCompromiso.SesionSolicitudCompromisoId);

                        SesionSolicitudCompromiso.FechaModificacion = SesionSolicitudCompromiso.FechaModificacion;
                        SesionSolicitudCompromiso.UsuarioModificacion = pSesionComiteSolicitud.UsuarioModificacion;

                        SesionSolicitudCompromiso.Tarea = SesionSolicitudCompromiso.Tarea;
                        SesionSolicitudCompromiso.FechaCumplimiento = SesionSolicitudCompromiso.FechaCumplimiento;

                        SesionSolicitudCompromiso.ResponsableSesionParticipanteId = SesionSolicitudCompromiso.ResponsableSesionParticipanteId;

                    }
                }
                _context.SaveChanges();
                return
                   new Respuesta
                   {
                       Data = validarcompletosActa(pSesionComiteSolicitud.ComiteTecnicoFiduciarioId.Value),
                       IsSuccessful = true,
                       IsException = false,
                       IsValidation = false,
                       Code = ConstantSesionComiteTecnico.OperacionExitosa,
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.OperacionExitosa, idAccion, pSesionComiteSolicitud.UsuarioCreacion, CreateEdit)
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
                       Code = ConstantSesionComiteTecnico.Error,
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.Error, idAccion, pSesionComiteSolicitud.UsuarioCreacion, ex.InnerException.ToString())
                   };
            }
        }

        public async Task<Respuesta> CreateEditTemasCompromiso(SesionComiteTema pSesionComiteTema)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Temas_Compromiso, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = "";
            try
            {
                SesionComiteTema SesionComiteTemadOld = _context.SesionComiteTema.Find(pSesionComiteTema.SesionTemaId);

                SesionComiteTemadOld.FechaModificacion = DateTime.Now;
                SesionComiteTemadOld.UsuarioModificacion = pSesionComiteTema.UsuarioCreacion;

                SesionComiteTemadOld.EstadoTemaCodigo = pSesionComiteTema.EstadoTemaCodigo;
                SesionComiteTemadOld.CantCompromisos = pSesionComiteTema.CantCompromisos;
                SesionComiteTemadOld.GeneraCompromiso = pSesionComiteTema.GeneraCompromiso;
                SesionComiteTemadOld.Observaciones = pSesionComiteTema.Observaciones;
                SesionComiteTemadOld.ObservacionesDecision = pSesionComiteTema.ObservacionesDecision;
                SesionComiteTemadOld.RegistroCompleto = ValidarRegistroCompletoSesionComiteTema(SesionComiteTemadOld);

                foreach (var TemaCompromiso in pSesionComiteTema.TemaCompromiso)
                {
                    if (TemaCompromiso.TemaCompromisoId == 0)
                    {
                        CreateEdit = "CREAR TEMA COMPROMISO";
                        TemaCompromiso.UsuarioCreacion = pSesionComiteTema.UsuarioCreacion;
                        TemaCompromiso.FechaCreacion = DateTime.Now;
                        TemaCompromiso.Eliminado = true;

                        _context.TemaCompromiso.Add(TemaCompromiso);
                    }
                    else
                    {
                        CreateEdit = "EDITAR TEMA COMPROMISO";
                        TemaCompromiso temaCompromisoOld = _context.TemaCompromiso.Find(TemaCompromiso.TemaCompromisoId);

                        temaCompromisoOld.Tarea = TemaCompromiso.Tarea;
                        temaCompromisoOld.Responsable = TemaCompromiso.Responsable;
                        temaCompromisoOld.FechaCumplimiento = TemaCompromiso.FechaCumplimiento;

                        temaCompromisoOld.FechaModificacion = TemaCompromiso.FechaModificacion;
                        temaCompromisoOld.UsuarioModificacion = TemaCompromiso.UsuarioModificacion;

                    }
                }
                _context.SaveChanges();
                return
                   new Respuesta
                   {
                       Data = validarcompletosActa(pSesionComiteTema.ComiteTecnicoId.Value),
                       IsSuccessful = true,
                       IsException = false,
                       IsValidation = false,
                       Code = ConstantSesionComiteTecnico.OperacionExitosa,
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.OperacionExitosa, idAccion, pSesionComiteTema.UsuarioCreacion, CreateEdit)
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
                       Code = ConstantSesionComiteTecnico.Error,
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.Error, idAccion, pSesionComiteTema.UsuarioCreacion, ex.InnerException.ToString())
                   };
            }
        }

        public async Task<Respuesta> CreateEditSesionTemaVoto(SesionComiteTema pSesionComiteTema)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Comite_Tema_Voto, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                SesionComiteTema sesionComiteTemaOld = _context.SesionComiteTema.Find(pSesionComiteTema.SesionTemaId);
                string CrearEditar = "";
                sesionComiteTemaOld.RequiereVotacion = true;
                sesionComiteTemaOld.EstadoTemaCodigo = pSesionComiteTema.EstadoTemaCodigo;
                sesionComiteTemaOld.EsAprobado = (pSesionComiteTema.EstadoTemaCodigo == "1") ? true : false;
                sesionComiteTemaOld.UsuarioModificacion = pSesionComiteTema.UsuarioCreacion;
                sesionComiteTemaOld.FechaModificacion = DateTime.Now;
                sesionComiteTemaOld.RegistroCompleto = ValidarRegistroCompletoSesionComiteTema(sesionComiteTemaOld);
                foreach (var SesionTemaVoto in pSesionComiteTema.SesionTemaVoto)
                {
                    if (SesionTemaVoto.SesionTemaVotoId == 0)
                    {
                        CrearEditar = "CREAR SESIÓN TEMA VOTO";
                        SesionTemaVoto.UsuarioCreacion = pSesionComiteTema.UsuarioCreacion;
                        SesionTemaVoto.FechaCreacion = DateTime.Now;
                        //SesionTemaVoto.Eliminado = false;
                        _context.SesionTemaVoto.Add(SesionTemaVoto);
                    }
                    else
                    {
                        CrearEditar = "EDITAR SESIÓN TEMA VOTO";
                        SesionTemaVoto SesionTemaVotoOld = _context.SesionTemaVoto.Find(SesionTemaVoto.SesionTemaVotoId);
                        //SesionTemaVotoOld.FechaModificacion = DateTime.Now;
                        //SesionTemaVotoOld.UsuarioModificacion = pSesionComiteTema.UsuarioCreacion;

                        SesionTemaVotoOld.EsAprobado = SesionTemaVoto.EsAprobado;
                        SesionTemaVotoOld.Observacion = SesionTemaVoto.Observacion;
                    }
                }
                _context.SaveChanges();
                return
                  new Respuesta
                  {
                      IsSuccessful = true,
                      IsException = false,
                      IsValidation = false,
                      Code = ConstantSesionComiteTecnico.OperacionExitosa,
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.OperacionExitosa, idAccion, pSesionComiteTema.UsuarioCreacion, CrearEditar)
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
                   Code = ConstantSesionComiteTecnico.Error,
                   Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.Error, idAccion, pSesionComiteTema.UsuarioCreacion, ex.InnerException.ToString())
               };
            }

        }

        public async Task<ComiteTecnico> GetCompromisosByComiteTecnicoId(int ComiteTecnicoId)
        {
            //Dominio estado reportado 48 
            ComiteTecnico comiteTecnico = await _context.ComiteTecnico.Where(r => r.ComiteTecnicoId == ComiteTecnicoId)
                .Include(r => r.SesionComiteTema)
                   .ThenInclude(r => r.TemaCompromiso)
               .Include(r => r.SesionComiteSolicitudComiteTecnicoFiduciario)
                   .ThenInclude(r => r.SesionSolicitudCompromiso)
                      .FirstOrDefaultAsync();



            List<Dominio> ListEstadoReportado = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Compromisos).ToList();
            List<SesionParticipante> ListSesionParticipantes = _context.SesionParticipante.Where(r => !(bool)r.Eliminado).Include(r => r.Usuario).ToList();
            comiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario.ToList().ForEach(s =>
            {
                s.SesionSolicitudCompromiso = s.SesionSolicitudCompromiso.Where(c => c.EsFiduciario == true).ToList();
            });

            comiteTecnico.NumeroCompromisos = 0;
            foreach (var SesionComiteTema in comiteTecnico.SesionComiteTema)
            {
                foreach (var TemaCompromiso in SesionComiteTema.TemaCompromiso)
                {
                    if (!string.IsNullOrEmpty(TemaCompromiso.EstadoCodigo))
                    {
                        TemaCompromiso.EstadoCodigo = ListEstadoReportado.Where(r => r.Codigo == TemaCompromiso.EstadoCodigo).FirstOrDefault().Nombre;
                    }
                    if (TemaCompromiso.Responsable != null)
                    {
                        TemaCompromiso.ResponsableNavigation = ListSesionParticipantes.Where(r => r.SesionParticipanteId == TemaCompromiso.Responsable).FirstOrDefault();
                    }
                    if (TemaCompromiso.EstadoCodigo == "3")
                        comiteTecnico.NumeroCompromisosCumplidos++;
                    comiteTecnico.NumeroCompromisos++;
                }
            }

            foreach (var SesionComiteSolicitud in comiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario)
            {
                foreach (var SesionSolicitudCompromiso in SesionComiteSolicitud.SesionSolicitudCompromiso.Where(c => c.EsFiduciario == true))
                {
                    if (!string.IsNullOrEmpty(SesionSolicitudCompromiso.EstadoCodigo))
                    {
                        SesionSolicitudCompromiso.EstadoCodigo = ListEstadoReportado.Where(r => r.Codigo == SesionSolicitudCompromiso.EstadoCodigo).FirstOrDefault().Nombre;
                    }

                    if (SesionSolicitudCompromiso.ResponsableSesionParticipanteId != null)
                    {
                        SesionSolicitudCompromiso.ResponsableSesionParticipante = ListSesionParticipantes.Where(r => r.SesionParticipanteId == SesionSolicitudCompromiso.ResponsableSesionParticipanteId).FirstOrDefault();
                    }
                    if (SesionSolicitudCompromiso.EstadoCodigo == "3")
                        comiteTecnico.NumeroCompromisosCumplidos++;
                    comiteTecnico.NumeroCompromisos++;
                }
            }



            return comiteTecnico;
        }

        public async Task<Respuesta> VerificarTemasCompromisos(ComiteTecnico pComiteTecnico)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Vertificar_Tema_Compromisos, (int)EnumeratorTipoDominio.Acciones);

            try
            {

                foreach (var SesionComiteTema in pComiteTecnico.SesionComiteTema)
                {

                    foreach (var temaCompromiso in SesionComiteTema.TemaCompromiso)
                    {
                        TemaCompromiso temaCompromisoOld = _context.TemaCompromiso.Find(temaCompromiso.TemaCompromisoId);
                        temaCompromisoOld.FechaModificacion = DateTime.Now;
                        temaCompromisoOld.UsuarioModificacion = pComiteTecnico.UsuarioCreacion;
                        temaCompromisoOld.EstadoCodigo = temaCompromiso.EstadoCodigo;
                    }
                }

                foreach (var SesionComiteSolicitud in pComiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario)
                {

                    foreach (var pSesionSolicitudCompromiso in SesionComiteSolicitud.SesionSolicitudCompromiso)
                    {
                        SesionSolicitudCompromiso SesionSolicitudCompromisoOld = _context.SesionSolicitudCompromiso.Find(pSesionSolicitudCompromiso.SesionSolicitudCompromisoId);
                        SesionSolicitudCompromisoOld.FechaModificacion = DateTime.Now;
                        SesionSolicitudCompromisoOld.UsuarioModificacion = pComiteTecnico.UsuarioCreacion;
                        SesionSolicitudCompromisoOld.EstadoCodigo = pSesionSolicitudCompromiso.EstadoCodigo;
                    }
                }


                _context.SaveChanges();
                return
                   new Respuesta
                   {
                       IsSuccessful = true,
                       IsException = false,
                       IsValidation = false,
                       Code = ConstantSesionComiteTecnico.OperacionExitosa,
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.OperacionExitosa, idAccion, pComiteTecnico.UsuarioCreacion, "VERIFICAR TEMA COMPROMISO")
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
                       Code = ConstantSesionComiteTecnico.Error,
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarSesionComiteFiduciario, ConstantSesionComiteFiduciario.Error, idAccion, pComiteTecnico.UsuarioCreacion, ex.InnerException.ToString())
                   };
            }
        }
         
        private bool ValidarRegistroCompletoSesionComiteSolicitud(SesionComiteSolicitud sesionComiteSolicitud)
        {
            if (
                (sesionComiteSolicitud.RequiereVotacionFiduciario == true && string.IsNullOrEmpty(sesionComiteSolicitud.RutaSoporteVotacionFiduciario)) ||
               sesionComiteSolicitud.GeneraCompromisoFiduciario == null ||
               sesionComiteSolicitud.RequiereVotacionFiduciario == null ||
               sesionComiteSolicitud.EstadoCodigo == null ||
               string.IsNullOrEmpty(sesionComiteSolicitud.ObservacionesFiduciario)
                )
            {
                return false;
            }
            return true;
        }

        private bool ValidarRegistroCompletoSesionComiteTema(SesionComiteTema sesionComiteTemaOld)
        {
            if (string.IsNullOrEmpty(sesionComiteTemaOld.Tema)
                || string.IsNullOrEmpty(sesionComiteTemaOld.ResponsableCodigo)
                || string.IsNullOrEmpty(sesionComiteTemaOld.TiempoIntervencion.ToString())
                //|| !string.IsNullOrEmpty(sesionComiteTemaOld.RutaSoporte)
                || string.IsNullOrEmpty(sesionComiteTemaOld.Observaciones)
                || (sesionComiteTemaOld.RequiereVotacion == true && sesionComiteTemaOld.EsAprobado == null)
                || sesionComiteTemaOld.RequiereVotacion == null
                //|| sesionComiteTemaOld.EsProposicionesVarios == null
                || sesionComiteTemaOld.GeneraCompromiso == null
                || string.IsNullOrEmpty(sesionComiteTemaOld.ObservacionesDecision)
                || sesionComiteTemaOld.EstadoTemaCodigo == null

                )
            {

                return false;
            }

            return true;

        }

        public static bool ValidarCamposComiteTecnico(ComiteTecnico pComiteTecnico)
        {
            if (
                    pComiteTecnico.RequiereVotacion == null ||
                    pComiteTecnico.RequiereVotacion == null ||
                    string.IsNullOrEmpty(pComiteTecnico.Justificacion) ||
                    pComiteTecnico.EsAprobado == null ||
                    string.IsNullOrEmpty(pComiteTecnico.FechaAplazamiento.ToString()) ||
                    string.IsNullOrEmpty(pComiteTecnico.Observaciones) ||
                    string.IsNullOrEmpty(pComiteTecnico.RutaSoporteVotacion) ||
                    pComiteTecnico.TieneCompromisos == null ||
                    string.IsNullOrEmpty(pComiteTecnico.CantCompromisos.ToString()) ||
                    string.IsNullOrEmpty(pComiteTecnico.RutaActaSesion) ||
                    string.IsNullOrEmpty(pComiteTecnico.FechaOrdenDia.ToString()) ||
                    string.IsNullOrEmpty(pComiteTecnico.NumeroComite) ||
                    string.IsNullOrEmpty(pComiteTecnico.EstadoComiteCodigo)
                )
            {
                return false;
            }
            return true;
        }
        
        public async Task<ProcesoSeleccion> GetProcesosSelecccionByProcesoSeleccionId(int pId)
        {

            return await _context.ProcesoSeleccion
               .Where(r => r.ProcesoSeleccionId == pId)
               .IncludeFilter(r => r.ProcesoSeleccionCronograma.Where(r => !(bool)r.Eliminado))
               .IncludeFilter(r => r.ProcesoSeleccionGrupo.Where(r => !(bool)r.Eliminado))
               //Aqui falta filtrarlos proponentes ya que en model y en codigo no de guarda eliminado
               .Include(r => r.ProcesoSeleccionProponente)
               .FirstOrDefaultAsync();

        }

        public async Task<byte[]> GetPlantillaActaIdComite(int ComiteId)
        {
            if (ComiteId == 0)
            {
                return Array.Empty<byte>();
            }
            ComiteTecnico comiteTecnico = await _context.ComiteTecnico
                .Where(r => r.ComiteTecnicoId == ComiteId)
                    .Include(r => r.SesionComiteTema)
                              .ThenInclude(r => r.TemaCompromiso)
                               .ThenInclude(r => r.ResponsableNavigation)
                                 .ThenInclude(r => r.Usuario)
                    .Include(r => r.SesionComiteSolicitudComiteTecnicoFiduciario)
                       .ThenInclude(r => r.SesionSolicitudCompromiso)
                           .ThenInclude(r => r.ResponsableSesionParticipante)
                              .ThenInclude(r => r.Usuario)
                    .FirstOrDefaultAsync();

            if (comiteTecnico == null)
            {
                return Array.Empty<byte>();
            }
            Plantilla plantilla = _context.Plantilla.Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Descargar_Acta_Comite_Fiduciario).ToString()).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();

            plantilla.Contenido = await ReemplazarDatosPlantillaActa(plantilla.Contenido, comiteTecnico);
            return ConvertirPDF(plantilla);
        }

        private async Task<string> ReemplazarDatosPlantillaActa(string strContenido, ComiteTecnico pComiteTecnico)
        {
            try
            {
                List<Contratacion> ListContratacion = await
                        _context.Contratacion
                        .Include(r => r.Contrato)
                        .Include(r => r.Contratista)
                        .Include(r => r.ContratacionProyecto)
                            .ThenInclude(r => r.Proyecto).ToListAsync();

                List<ProcesoSeleccion> ListProcesoSeleccion = _context.ProcesoSeleccion.ToList();
                List<Localizacion> localizacions = _context.Localizacion.ToList();
                List<Dominio> ListParametricas = _context.Dominio.Where(r => r.TipoDominioId != (int)EnumeratorTipoDominio.PlaceHolder).ToList();
                List<Dominio> placeholders = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.PlaceHolder).ToList();
                List<InstitucionEducativaSede> ListIntitucionEducativa = _context.InstitucionEducativaSede.ToList();
                List<SesionParticipante> ListSesionParticipante = _context.SesionParticipante
                    .Where(r => r.ComiteTecnicoId == pComiteTecnico.ComiteTecnicoId)
                    .Include(r => r.Usuario)
                    .ToList();

                List<SesionInvitado> ListInvitados = _context.SesionInvitado
                    .Where(r => r.ComiteTecnicoId == pComiteTecnico.ComiteTecnicoId).ToList();
                //Tablas Dinamicas

                //Plantilla orden dia
                string PlantillaSolicitudesContractuales = _context.Plantilla
                 .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Tabla_Solicitudes_Contractuales)
                    .ToString()).FirstOrDefault()
                 .Contenido;
                //Plantilla Registros  orden dia
                string PlantillaRegistrosSolicitudesContractuales = _context.Plantilla
                 .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Registros_Tabla_Solicitudes_Contractuales)
                    .ToString()).FirstOrDefault()
                 .Contenido;
                //Registros Orden del dia
                string RegistrosSolicitudesContractuales = string.Empty;

                //Logica Invitados
                string PlantillaInvitados = _context.Plantilla
                    .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Registros_Tabla_Invitados)
                       .ToString()).FirstOrDefault()
                    .Contenido;
                string RegistrosInvitados = string.Empty;

                //Plantilla Contratacion
                string PlantillaContratacion = _context.Plantilla
                    .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Tabla_Solicitud_Contratacion)
                       .ToString()).FirstOrDefault()
                    .Contenido;
                string registrosContratacion = string.Empty;

                //Plantilla Proyectos
                string PlantillaRegistrosProyectos = _context.Plantilla
                    .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Registros_Tabla_proyectos)
                       .ToString()).FirstOrDefault()
                    .Contenido;

                //Plantilla Tipo De votaciones 
                string PlantillaVotacionUnanime = _context.Plantilla
                    .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Votacion_Unanime)
                       .ToString()).FirstOrDefault()
                    .Contenido;

                string PlantillaNoVotacionUnanime = _context.Plantilla
                .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Votacion_No_Unanime)
                   .ToString()).FirstOrDefault()
                .Contenido;

                //Plantilla Procesos de Seleccion
                string PlantillaProcesosSelecccion = _context.Plantilla
                    .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Tabla_Solicitud_Proceso_de_Seleccion)
                       .ToString()).FirstOrDefault()
                    .Contenido;

                //Plantilla Nuevos Temas
                string PlantillaNuevosTemas = _context.Plantilla
                    .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Registros_Temas)
                       .ToString()).FirstOrDefault()
                    .Contenido;
                string RegistrosNuevosTemas = string.Empty;

                //Plantilla Compromisos Solicitud
                string PlantillaCompromisosSolicitud = _context.Plantilla
                 .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Registros_Compromiso_Solicitud)
                    .ToString()).FirstOrDefault()
                 .Contenido;


                //Plantilla Temas
                string PlantillaTemas = _context.Plantilla
                 .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Tabla_Registros_Temas_Y_Proposiciones_y_Varios)
                    .ToString()).FirstOrDefault()
                 .Contenido;
                string RegistrosTemas = string.Empty;

                //Plantilla Temas  
                string RegistrosProposicionesVarios = string.Empty;


                //Plantilla Firmas
                string PlantillaFirmas = _context.Plantilla
                 .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Registros_Firmas)
                    .ToString()).FirstOrDefault()
                 .Contenido;

                string RegistrosFirmas = string.Empty;

                string registrosProcesosSelecccion = string.Empty;

                //Orden del dia 

                if (pComiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario.Count() == 0)
                {
                    PlantillaSolicitudesContractuales = string.Empty;
                }
                else
                {
                    foreach (var SesionComiteSolicitud in pComiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario)
                    {
                        RegistrosSolicitudesContractuales += PlantillaRegistrosSolicitudesContractuales;
                        switch (SesionComiteSolicitud.TipoSolicitudCodigo)
                        {

                            case ConstanCodigoTipoSolicitud.Contratacion:
                                Contratacion contratacion = _context.Contratacion.Find(SesionComiteSolicitud.SolicitudId);

                                foreach (Dominio placeholderDominio in placeholders)
                                {
                                    switch (placeholderDominio.Codigo)
                                    {
                                        case ConstanCodigoVariablesPlaceHolders.NUMERO_SOLICITUD:
                                            RegistrosSolicitudesContractuales = RegistrosSolicitudesContractuales
                                                .Replace(placeholderDominio.Nombre, contratacion.NumeroSolicitud);
                                            break;

                                        case ConstanCodigoVariablesPlaceHolders.FECHA_SOLICITUD:
                                            string FechaSolicitud = string.Empty;
                                            if (contratacion.FechaTramite.HasValue)
                                            {
                                                FechaSolicitud = ((DateTime)contratacion.FechaTramite).ToString("dd-MM-yyy");
                                            }
                                            RegistrosSolicitudesContractuales = RegistrosSolicitudesContractuales
                                                .Replace(placeholderDominio.Nombre, FechaSolicitud);
                                            break;

                                        case ConstanCodigoVariablesPlaceHolders.TIPO_SOLICITUD:
                                            RegistrosSolicitudesContractuales = RegistrosSolicitudesContractuales
                                                .Replace(placeholderDominio.Nombre,
                                                ListParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud
                                                && r.Codigo == SesionComiteSolicitud.TipoSolicitudCodigo).FirstOrDefault().Nombre);
                                            break;
                                    }
                                }

                                break;

                            //TIPO SOLICITUD PROCESOS DE SELECCION
                            case ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion:

                                ProcesoSeleccion procesoSeleccion = ListProcesoSeleccion.Where(r => r.ProcesoSeleccionId == SesionComiteSolicitud.SolicitudId).FirstOrDefault();

                                foreach (Dominio placeholderDominio in placeholders)
                                {
                                    RegistrosSolicitudesContractuales += PlantillaRegistrosSolicitudesContractuales;
                                    switch (placeholderDominio.Codigo)
                                    {
                                        case ConstanCodigoVariablesPlaceHolders.NUMERO_SOLICITUD:
                                            RegistrosSolicitudesContractuales = RegistrosSolicitudesContractuales
                                              .Replace(placeholderDominio.Nombre, procesoSeleccion.NumeroProceso);
                                            break;

                                        case ConstanCodigoVariablesPlaceHolders.FECHA_SOLICITUD:
                                            string FechaSolicitud = string.Empty;
                                            if (procesoSeleccion.FechaCreacion != null)
                                            {
                                                FechaSolicitud = ((DateTime)procesoSeleccion.FechaCreacion).ToString("dd-MM-yyy");
                                            }
                                            RegistrosSolicitudesContractuales = RegistrosSolicitudesContractuales
                                                .Replace(placeholderDominio.Nombre, FechaSolicitud);
                                            break;

                                        case ConstanCodigoVariablesPlaceHolders.TIPO_SOLICITUD:
                                            RegistrosSolicitudesContractuales = RegistrosSolicitudesContractuales
                                                .Replace(placeholderDominio.Nombre,
                                                ListParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud
                                                && r.Codigo == SesionComiteSolicitud.TipoSolicitudCodigo).FirstOrDefault().Nombre);
                                            break;
                                    }
                                }
                                break;

                            default:
                                break;
                        }

                    }

                    //Registros en tabla  
                    PlantillaSolicitudesContractuales = PlantillaSolicitudesContractuales.Replace("[REGISTROS_SOLICITUDES_CONTRACTUALES]", RegistrosSolicitudesContractuales);
                }

                //Tabla Invitados
                foreach (var invitado in ListInvitados.Where(r => !(bool)r.Eliminado).ToList())
                {
                    RegistrosInvitados += PlantillaInvitados;
                    foreach (Dominio placeholderDominio in placeholders)
                    {
                        switch (placeholderDominio.Codigo)
                        {
                            case ConstanCodigoVariablesPlaceHolders.INVITADO_NOMBRE:
                                RegistrosInvitados = RegistrosInvitados
                                    .Replace(placeholderDominio.Nombre, invitado.Nombre);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.INVITADO_CARGO:
                                RegistrosInvitados = RegistrosInvitados
                                    .Replace(placeholderDominio.Nombre, invitado.Cargo);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.INVITADO_ENTIDAD:
                                RegistrosInvitados = RegistrosInvitados
                                    .Replace(placeholderDominio.Nombre, invitado.Entidad);
                                break;
                        }
                    }
                }
                //Logica Orden Del Dia
                int enumOrdenDelDia = 1;
                foreach (var SesionComiteSolicitud in pComiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario)
                {
                    string RegistrosProyectos = string.Empty;

                    string registrosCompromisosSolicitud = string.Empty;

                    registrosContratacion += PlantillaContratacion;
                    switch (SesionComiteSolicitud.TipoSolicitudCodigo)
                    {
                        //TIPO SOLICITUD CONTRATACION
                        //TIPO SOLICITUD CONTRATACION
                        //TIPO SOLICITUD CONTRATACION
                        case ConstanCodigoTipoSolicitud.Contratacion:
                            Contratacion contratacion = await _IProjectContractingService.GetAllContratacionByContratacionId(SesionComiteSolicitud.SolicitudId);

                            foreach (Dominio placeholderDominio in placeholders)
                            {
                                switch (placeholderDominio.Codigo)
                                {
                                    case ConstanCodigoVariablesPlaceHolders.NUMERO_SOLICITUD:
                                        registrosContratacion = registrosContratacion
                                            .Replace(placeholderDominio.Nombre, enumOrdenDelDia++.ToString());
                                        break;


                                    case ConstanCodigoVariablesPlaceHolders.NUMERO_SOLICITUD_CONTRATACION:
                                        registrosContratacion = registrosContratacion
                                            .Replace(placeholderDominio.Nombre, contratacion.NumeroSolicitud);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.CANTIDAD_DE_PROYECTOS_ASOCIADOS:
                                        registrosContratacion = registrosContratacion
                                            .Replace(placeholderDominio.Nombre, contratacion.ContratacionProyecto.Where(R => !(bool)R.Eliminado).Count().ToString());
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.FECHA_SOLICITUD_CONTRATACION:
                                        string FechaSolicitud = string.Empty;
                                        if (contratacion.FechaTramite.HasValue)
                                        {
                                            FechaSolicitud = ((DateTime)contratacion.FechaTramite).ToString("dd-MM-yyy");
                                        }
                                        registrosContratacion = registrosContratacion
                                            .Replace(placeholderDominio.Nombre, FechaSolicitud);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.TIPO_SOLICITUD_CONTRATACION:
                                        registrosContratacion = registrosContratacion
                                            .Replace(placeholderDominio.Nombre,
                                            ListParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud
                                            && r.Codigo == SesionComiteSolicitud.TipoSolicitudCodigo).FirstOrDefault().Nombre);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.TIPO_CONTRATO_CONTRATACION:
                                        string StrTipoContrato = string.Empty;

                                        if (contratacion.Contrato.Count() > 0)
                                        {
                                            StrTipoContrato = ListParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_por_contratar
                                           && r.Codigo == contratacion.Contrato.FirstOrDefault().TipoContratoCodigo).FirstOrDefault().Nombre;
                                        }
                                        registrosContratacion = registrosContratacion
                                            .Replace(placeholderDominio.Nombre, StrTipoContrato
                                           );
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.REGISTROS_TABLA_PROYECTO:

                                        foreach (var ContratacionProyecto in contratacion.ContratacionProyecto.Where(r => !(bool)r.Eliminado))
                                        {
                                            RegistrosProyectos += PlantillaRegistrosProyectos;
                                            Localizacion Municipio = localizacions.Where(r => r.LocalizacionId == ContratacionProyecto.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                                            Localizacion Departamento = localizacions.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
                                            InstitucionEducativaSede InstitucionEducativa = ListIntitucionEducativa.Where(r => r.InstitucionEducativaSedeId == ContratacionProyecto.Proyecto.InstitucionEducativaId).FirstOrDefault();
                                            InstitucionEducativaSede Sede = ListIntitucionEducativa.Where(r => r.InstitucionEducativaSedeId == ContratacionProyecto.Proyecto.SedeId).FirstOrDefault();

                                            foreach (Dominio placeholderDominio2 in placeholders)
                                            {
                                                switch (placeholderDominio2.Codigo)
                                                {

                                                    case ConstanCodigoVariablesPlaceHolders.LLAVE_MEN:
                                                        RegistrosProyectos = RegistrosProyectos
                                                            .Replace(placeholderDominio2.Nombre,
                                                                  ContratacionProyecto.Proyecto.LlaveMen);
                                                        break;

                                                    case ConstanCodigoVariablesPlaceHolders.TIPO_INTERVENCION:
                                                        RegistrosProyectos = RegistrosProyectos
                                                            .Replace(placeholderDominio2.Nombre,
                                                               ListParametricas
                                                               .Where(r => r.Codigo == ContratacionProyecto.Proyecto.TipoIntervencionCodigo
                                                               && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).FirstOrDefault().Nombre);
                                                        break;

                                                    case ConstanCodigoVariablesPlaceHolders.DEPARTAMENTO:
                                                        RegistrosProyectos = RegistrosProyectos
                                                            .Replace(placeholderDominio2.Nombre, Departamento.Descripcion);
                                                        break;

                                                    case ConstanCodigoVariablesPlaceHolders.MUNICIPIO:
                                                        RegistrosProyectos = RegistrosProyectos
                                                            .Replace(placeholderDominio2.Nombre, Municipio.Descripcion);
                                                        break;

                                                    case ConstanCodigoVariablesPlaceHolders.INSTITUCION_EDUCATIVA:
                                                        RegistrosProyectos = RegistrosProyectos
                                                            .Replace(placeholderDominio2.Nombre, InstitucionEducativa.Nombre);
                                                        break;

                                                    case ConstanCodigoVariablesPlaceHolders.SEDE:
                                                        RegistrosProyectos = RegistrosProyectos
                                                            .Replace(placeholderDominio2.Nombre, Sede.Nombre);
                                                        break;
                                                }
                                            }
                                        }

                                        registrosContratacion = registrosContratacion
                                            .Replace(placeholderDominio.Nombre, RegistrosProyectos);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.DESARROLLO_SOLICITUD:
                                        registrosContratacion = registrosContratacion
                                            .Replace(placeholderDominio.Nombre, SesionComiteSolicitud.DesarrolloSolicitudFiduciario);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.DECISIONES_SOLICITUD:

                                        string strRequiereVotacion = "Sí fue requerida";
                                        if (SesionComiteSolicitud.RequiereVotacion == null || !(bool)SesionComiteSolicitud.RequiereVotacionFiduciario)
                                        {
                                            strRequiereVotacion = "No fue requerida";
                                        }
                                        registrosContratacion = registrosContratacion
                                        .Replace(placeholderDominio.Nombre, strRequiereVotacion);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.OBSERVACIONES_SOLICITUD:
                                        registrosContratacion = registrosContratacion
                                        .Replace(placeholderDominio.Nombre, SesionComiteSolicitud.ObservacionesFiduciario);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.RESULTADO_VOTACION:
                                        string TextoResultadoVotacion = PlantillaVotacionUnanime;

                                        if (SesionComiteSolicitud.RequiereVotacion == null || !(bool)SesionComiteSolicitud.RequiereVotacionFiduciario)
                                        {

                                            TextoResultadoVotacion = PlantillaNoVotacionUnanime;
                                        }
                                        TextoResultadoVotacion = TextoResultadoVotacion.Replace("[URL_SOPORTES_VOTO]", SesionComiteSolicitud.RutaSoporteVotacionFiduciario);

                                        registrosContratacion = registrosContratacion
                                        .Replace(placeholderDominio.Nombre, TextoResultadoVotacion);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.REGISTROS_COMPROMISOS_SOLICITUD:

                                        foreach (var compromiso in SesionComiteSolicitud.SesionSolicitudCompromiso)
                                        {
                                            // bool ReplaceComplete = false;
                                            registrosCompromisosSolicitud += PlantillaCompromisosSolicitud;
                                            foreach (Dominio placeholderDominio3 in placeholders)
                                            {
                                                //if (ReplaceComplete) { break; }
                                                switch (placeholderDominio3.Codigo)
                                                {
                                                    case ConstanCodigoVariablesPlaceHolders.TAREA_COMPROMISO:
                                                        registrosCompromisosSolicitud = registrosCompromisosSolicitud
                                                            .Replace(placeholderDominio3.Nombre, compromiso.Tarea);
                                                        break;

                                                    case ConstanCodigoVariablesPlaceHolders.RESPONSABLE_COMPROMISO:
                                                        registrosCompromisosSolicitud = registrosCompromisosSolicitud
                                                            .Replace(placeholderDominio3.Nombre, compromiso.ResponsableSesionParticipante.Usuario.Nombres
                                                            + " " + compromiso.ResponsableSesionParticipante.Usuario.Apellidos);
                                                        break;

                                                    case ConstanCodigoVariablesPlaceHolders.FECHA_CUMPLIMIENTO_COMPROMISO:
                                                        registrosCompromisosSolicitud = registrosCompromisosSolicitud
                                                            .Replace(placeholderDominio3.Nombre, compromiso.FechaCumplimiento.Value.ToString("dd-MM-yyyy"));
                                                        break;
                                                }
                                            }
                                        }

                                        registrosContratacion = registrosContratacion
                                        .Replace(placeholderDominio.Nombre, registrosCompromisosSolicitud);
                                        break;

                                }
                            }
                            break;

                        //TIPO SOLICITUD PROCESOS DE SELECCION
                        case ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion:

                            ProcesoSeleccion procesoSeleccion = ListProcesoSeleccion.Where(r => r.ProcesoSeleccionId == SesionComiteSolicitud.SolicitudId).FirstOrDefault();

                            foreach (Dominio placeholderDominio in placeholders)
                            {
                                registrosProcesosSelecccion += PlantillaProcesosSelecccion;
                                switch (placeholderDominio.Codigo)
                                {
                                    case ConstanCodigoVariablesPlaceHolders.NUMERO_SOLICITUD:
                                        registrosProcesosSelecccion = registrosProcesosSelecccion
                                          .Replace(placeholderDominio.Nombre, procesoSeleccion.NumeroProceso);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.TIPO_PROCESO:
                                        registrosProcesosSelecccion = registrosProcesosSelecccion
                                          .Replace(placeholderDominio.Nombre,
                                          ListParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Proceso_Seleccion
                                          && r.Codigo == procesoSeleccion.TipoProcesoCodigo).FirstOrDefault().Nombre);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.OBJETO_PROCESO:
                                        registrosProcesosSelecccion = registrosProcesosSelecccion
                                          .Replace(placeholderDominio.Nombre, procesoSeleccion.Objeto);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.ALCANCE_PARTICULAR_PROCESO:
                                        registrosProcesosSelecccion = registrosProcesosSelecccion
                                          .Replace(placeholderDominio.Nombre, procesoSeleccion.AlcanceParticular);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.JUSTIFICACION_PROCESO:
                                        registrosProcesosSelecccion = registrosProcesosSelecccion
                                          .Replace(placeholderDominio.Nombre, procesoSeleccion.Justificacion);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.TIPO_INTERVENCION:
                                        registrosProcesosSelecccion = registrosProcesosSelecccion
                                        .Replace(placeholderDominio.Nombre,
                                        ListParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion
                                        && r.Codigo == procesoSeleccion.TipoIntervencionCodigo).FirstOrDefault().Nombre);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.TIPO_ALCANCE_PROCESO:
                                        registrosProcesosSelecccion = registrosProcesosSelecccion
                                        .Replace(placeholderDominio.Nombre,
                                        ListParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_alcance
                                        && r.Codigo == procesoSeleccion.TipoAlcanceCodigo).FirstOrDefault().Nombre);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.DESARROLLO_SOLICITUD:
                                        registrosContratacion = registrosContratacion
                                            .Replace(placeholderDominio.Nombre, SesionComiteSolicitud.DesarrolloSolicitud);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.DECISIONES_SOLICITUD:

                                        string strRequiereVotacion = "Sí fue requerida";
                                        if (SesionComiteSolicitud.RequiereVotacion == null || !(bool)SesionComiteSolicitud.RequiereVotacion)
                                        {
                                            strRequiereVotacion = "No fue requerida";
                                        }
                                        registrosContratacion = registrosContratacion
                                        .Replace(placeholderDominio.Nombre, strRequiereVotacion);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.OBSERVACIONES_SOLICITUD:
                                        registrosContratacion = registrosContratacion
                                        .Replace(placeholderDominio.Nombre, SesionComiteSolicitud.Observaciones);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.RESULTADO_VOTACION:
                                        string TextoResultadoVotacion = string.Empty;

                                        if (SesionComiteSolicitud.RequiereVotacion == null || !(bool)SesionComiteSolicitud.RequiereVotacion)
                                        {
                                            TextoResultadoVotacion = _context.Plantilla.Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Votacion_No_Unanime).ToString()).FirstOrDefault().Nombre;
                                        }
                                        else
                                        {
                                            TextoResultadoVotacion = _context.Plantilla.Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Votacion_Unanime).ToString()).FirstOrDefault().Nombre;
                                        }
                                        registrosContratacion = registrosContratacion
                                        .Replace(placeholderDominio.Nombre, TextoResultadoVotacion);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.REGISTROS_COMPROMISOS_SOLICITUD:

                                        foreach (var compromiso in SesionComiteSolicitud.SesionSolicitudCompromiso)
                                        {
                                            // bool ReplaceComplete = false; 
                                            registrosCompromisosSolicitud += PlantillaCompromisosSolicitud;
                                            foreach (Dominio placeholderDominio3 in placeholders)
                                            {
                                                //if (ReplaceComplete) { break; }
                                                switch (placeholderDominio3.Codigo)
                                                {
                                                    case ConstanCodigoVariablesPlaceHolders.TAREA_COMPROMISO:
                                                        registrosCompromisosSolicitud = registrosCompromisosSolicitud
                                                            .Replace(placeholderDominio.Nombre, compromiso.Tarea);
                                                        break;

                                                    case ConstanCodigoVariablesPlaceHolders.RESPONSABLE_COMPROMISO:
                                                        registrosCompromisosSolicitud = registrosCompromisosSolicitud
                                                            .Replace(placeholderDominio.Nombre, compromiso.ResponsableSesionParticipante.Usuario.Nombres
                                                            + " " + compromiso.ResponsableSesionParticipante.Usuario.Apellidos);
                                                        break;

                                                    case ConstanCodigoVariablesPlaceHolders.FECHA_CUMPLIMIENTO_COMPROMISO:
                                                        registrosCompromisosSolicitud = registrosCompromisosSolicitud
                                                            .Replace(placeholderDominio.Nombre, compromiso.FechaCumplimiento.Value.ToString("dd-MM-yyyy"));
                                                        break;
                                                }
                                            }
                                        }

                                        registrosContratacion = registrosContratacion
                                        .Replace(placeholderDominio.Nombre, registrosCompromisosSolicitud);
                                        break;

                                }
                            }
                            break;

                        default:
                            break;
                    }

                }

                //Nuevos Temas
                int EnumTema = 1;
                foreach (var Tema in pComiteTecnico.SesionComiteTema.Where(r => r.EsProposicionesVarios == null).ToList())
                {
                    string registrosCompromisosSolicitud = string.Empty;
                    RegistrosNuevosTemas += PlantillaNuevosTemas;
                    foreach (Dominio placeholderDominio in placeholders)
                    {
                        switch (placeholderDominio.Codigo)
                        {
                            case ConstanCodigoVariablesPlaceHolders.NUMERO_DE_TEMA:
                                RegistrosNuevosTemas = RegistrosNuevosTemas
                                    .Replace(placeholderDominio.Nombre, EnumTema++.ToString());
                                break;

                            case ConstanCodigoVariablesPlaceHolders.NOMBRE_DE_TEMA:
                                RegistrosNuevosTemas = RegistrosNuevosTemas
                                    .Replace(placeholderDominio.Nombre, Tema.Tema);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.RESPONSABLE_DE_TEMA:
                                RegistrosNuevosTemas = RegistrosNuevosTemas
                                    .Replace(placeholderDominio.Nombre,
                                    !string.IsNullOrEmpty(Tema.ResponsableCodigo)
                                    ? ListParametricas.
                                    Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Miembros_Comite_Tecnico
                                    && r.Codigo == Tema.ResponsableCodigo).FirstOrDefault().Nombre
                                    : " "
                                    ); ;
                                break;

                            case ConstanCodigoVariablesPlaceHolders.DESARROLLO_DE_TEMA:
                                RegistrosNuevosTemas = RegistrosNuevosTemas
                                   .Replace(placeholderDominio.Nombre, Tema.Observaciones);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.RESULTADO_VOTACION:

                                string strRequiereVotacion = "Sí fue requerida";
                                if (Tema.RequiereVotacion == null || !(bool)Tema.RequiereVotacion)
                                {
                                    strRequiereVotacion = "No fue requerida";
                                }
                                RegistrosNuevosTemas = RegistrosNuevosTemas
                                   .Replace(placeholderDominio.Nombre, strRequiereVotacion);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.OBSERVACIONES_SOLICITUD:
                                RegistrosNuevosTemas = RegistrosNuevosTemas
                                   .Replace(placeholderDominio.Nombre, Tema.Observaciones);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.REGISTROS_TEMAS:

                                foreach (var compromiso in Tema.TemaCompromiso)
                                {
                                    // bool ReplaceComplete = false;
                                    registrosCompromisosSolicitud += PlantillaCompromisosSolicitud;
                                    foreach (Dominio placeholderDominio4 in placeholders)
                                    {
                                        //if (ReplaceComplete) { break; }
                                        switch (placeholderDominio4.Codigo)
                                        {
                                            case ConstanCodigoVariablesPlaceHolders.TAREA_COMPROMISO:
                                                registrosCompromisosSolicitud = registrosCompromisosSolicitud
                                                    .Replace(placeholderDominio4.Nombre, compromiso.Tarea);
                                                break;

                                            case ConstanCodigoVariablesPlaceHolders.RESPONSABLE_COMPROMISO:
                                                registrosCompromisosSolicitud = registrosCompromisosSolicitud
                                                    .Replace(placeholderDominio4.Nombre, compromiso.ResponsableNavigation.Usuario.Nombres
                                                    + " " + compromiso.ResponsableNavigation.Usuario.Apellidos);
                                                break;

                                            case ConstanCodigoVariablesPlaceHolders.FECHA_CUMPLIMIENTO_COMPROMISO:
                                                registrosCompromisosSolicitud = registrosCompromisosSolicitud
                                                    .Replace(placeholderDominio4.Nombre, compromiso.FechaCumplimiento.Value.ToString("dd-MM-yyyy"));
                                                break;
                                        }
                                    }
                                }
                                RegistrosNuevosTemas = RegistrosNuevosTemas
                                .Replace(placeholderDominio.Nombre, registrosCompromisosSolicitud);
                                break;
                        }
                    }
                }


                //Temas para ordel del dia
                int enumTemaOrdelDia = 1;
                foreach (var Tema in pComiteTecnico.SesionComiteTema.Where(r => r.EsProposicionesVarios == null).ToList())
                {
                    RegistrosTemas += PlantillaTemas;

                    foreach (Dominio placeholderDominio in placeholders)
                    {
                        switch (placeholderDominio.Codigo)
                        {
                            case ConstanCodigoVariablesPlaceHolders.NUMERADOR_TEMA:
                                RegistrosTemas = RegistrosTemas
                                    .Replace(placeholderDominio.Nombre, enumTemaOrdelDia++.ToString());
                                break;

                            case ConstanCodigoVariablesPlaceHolders.NOMBRE_TEMA:
                                RegistrosTemas = RegistrosTemas
                                    .Replace(placeholderDominio.Nombre, Tema.Tema);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.RESPONSABLE_TEMA:
                                RegistrosTemas = RegistrosTemas
                                    .Replace(placeholderDominio.Nombre,
                                    !string.IsNullOrEmpty(Tema.ResponsableCodigo)
                                    ? ListParametricas.
                                    Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Miembros_Comite_Tecnico
                                    && r.Codigo == Tema.ResponsableCodigo).FirstOrDefault().Nombre
                                    : " "
                                    ); ;
                                break;

                            case ConstanCodigoVariablesPlaceHolders.TIEMPO_INTERVENCION_MINUTOS_TEMA:
                                RegistrosTemas = RegistrosTemas
                                    .Replace(placeholderDominio.Nombre, Tema.TiempoIntervencion.ToString());
                                break;

                            case ConstanCodigoVariablesPlaceHolders.URL_CON_SOPORTE_TEMA:
                                RegistrosTemas = RegistrosTemas
                                    .Replace(placeholderDominio.Nombre, Tema.RutaSoporte);
                                break;

                        }
                    }
                }

                //Proposiciones y varios para ordel del dia
                int enumPropisicionesVarios = 1;
                foreach (var Tema in pComiteTecnico.SesionComiteTema.Where(r => r.EsProposicionesVarios != null).ToList())
                {
                    RegistrosProposicionesVarios += PlantillaTemas;

                    foreach (Dominio placeholderDominio in placeholders)
                    {
                        switch (placeholderDominio.Codigo)
                        {
                            case ConstanCodigoVariablesPlaceHolders.NUMERADOR_TEMA:
                                RegistrosProposicionesVarios = RegistrosProposicionesVarios
                                    .Replace(placeholderDominio.Nombre, enumPropisicionesVarios++.ToString());
                                break;

                            case ConstanCodigoVariablesPlaceHolders.NOMBRE_TEMA:
                                RegistrosProposicionesVarios = RegistrosProposicionesVarios
                                    .Replace(placeholderDominio.Nombre, Tema.Tema);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.RESPONSABLE_TEMA:
                                RegistrosProposicionesVarios = RegistrosProposicionesVarios
                                    .Replace(placeholderDominio.Nombre,
                                    !string.IsNullOrEmpty(Tema.ResponsableCodigo)
                                    ? ListParametricas.
                                    Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Miembros_Comite_Tecnico
                                    && r.Codigo == Tema.ResponsableCodigo).FirstOrDefault().Nombre
                                    : " "
                                    ); ;
                                break;

                            case ConstanCodigoVariablesPlaceHolders.TIEMPO_INTERVENCION_MINUTOS_TEMA:
                                RegistrosProposicionesVarios = RegistrosProposicionesVarios
                                    .Replace(placeholderDominio.Nombre, Tema.TiempoIntervencion.ToString());
                                break;

                            case ConstanCodigoVariablesPlaceHolders.URL_CON_SOPORTE_TEMA:
                                RegistrosProposicionesVarios = RegistrosProposicionesVarios
                                    .Replace(placeholderDominio.Nombre, Tema.RutaSoporte);
                                break;

                        }
                    }
                }



                //Proposiciones y varios
                int EnumProposiciones = 1;
                string RegistrosProposicionVarios = string.Empty;
                foreach (var Tema in pComiteTecnico.SesionComiteTema.Where(r => r.EsProposicionesVarios != null))
                {
                    string registrosCompromisosSolicitud = string.Empty;
                    RegistrosProposicionVarios += PlantillaNuevosTemas;
                    foreach (Dominio placeholderDominio in placeholders)
                    {
                        switch (placeholderDominio.Codigo)
                        {
                            case ConstanCodigoVariablesPlaceHolders.NUMERO_DE_TEMA:
                                RegistrosProposicionVarios = RegistrosProposicionVarios
                                    .Replace(placeholderDominio.Nombre, EnumProposiciones++.ToString());
                                break;

                            case ConstanCodigoVariablesPlaceHolders.NOMBRE_DE_TEMA:
                                RegistrosProposicionVarios = RegistrosProposicionVarios
                                    .Replace(placeholderDominio.Nombre, Tema.Tema);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.RESPONSABLE_DE_TEMA:
                                RegistrosProposicionVarios = RegistrosProposicionVarios
                                    .Replace(placeholderDominio.Nombre,
                                    !string.IsNullOrEmpty(Tema.ResponsableCodigo)
                                    ? ListParametricas.
                                    Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Miembros_Comite_Tecnico
                                    && r.Codigo == Tema.ResponsableCodigo).FirstOrDefault().Nombre
                                    : " "
                                    ); ;
                                break;

                            case ConstanCodigoVariablesPlaceHolders.DESARROLLO_DE_TEMA:
                                RegistrosProposicionVarios = RegistrosProposicionVarios
                                   .Replace(placeholderDominio.Nombre, Tema.Observaciones);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.RESULTADO_VOTACION:

                                string strRequiereVotacion = "Sí fue requerida";
                                if (Tema.RequiereVotacion == null || !(bool)Tema.RequiereVotacion)
                                {
                                    strRequiereVotacion = "No fue requerida";
                                }
                                RegistrosProposicionVarios = RegistrosProposicionVarios
                                   .Replace(placeholderDominio.Nombre, strRequiereVotacion);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.OBSERVACIONES_SOLICITUD:
                                RegistrosProposicionVarios = RegistrosProposicionVarios
                                   .Replace(placeholderDominio.Nombre, Tema.Observaciones);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.REGISTROS_TEMAS:
                                registrosCompromisosSolicitud = string.Empty;

                                foreach (var compromiso in Tema.TemaCompromiso)
                                {
                                    // bool ReplaceComplete = false;
                                    registrosCompromisosSolicitud += PlantillaCompromisosSolicitud;
                                    foreach (Dominio placeholderDominio5 in placeholders)
                                    {
                                        //if (ReplaceComplete) { break; }
                                        switch (placeholderDominio5.Codigo)
                                        {
                                            case ConstanCodigoVariablesPlaceHolders.TAREA_COMPROMISO:
                                                registrosCompromisosSolicitud = registrosCompromisosSolicitud
                                                    .Replace(placeholderDominio5.Nombre, compromiso.Tarea);
                                                break;

                                            case ConstanCodigoVariablesPlaceHolders.RESPONSABLE_COMPROMISO:
                                                registrosCompromisosSolicitud = registrosCompromisosSolicitud
                                                    .Replace(placeholderDominio5.Nombre, compromiso.ResponsableNavigation.Usuario.Nombres
                                                    + " " + compromiso.ResponsableNavigation.Usuario.Apellidos);
                                                break;

                                            case ConstanCodigoVariablesPlaceHolders.FECHA_CUMPLIMIENTO_COMPROMISO:
                                                registrosCompromisosSolicitud = registrosCompromisosSolicitud
                                                    .Replace(placeholderDominio5.Nombre, compromiso.FechaCumplimiento.Value.ToString("dd-MM-yyyy"));
                                                break;
                                        }
                                    }
                                }

                                RegistrosProposicionVarios = RegistrosProposicionVarios
                             .Replace(placeholderDominio.Nombre, registrosCompromisosSolicitud);
                                break;
                        }
                    }
                }

                //Firmas 
                int enumFirmar = 1;
                foreach (var SesionParticipante in ListSesionParticipante)
                {
                    RegistrosFirmas += PlantillaFirmas;
                    foreach (Dominio placeholderDominio in placeholders)
                    {
                        switch (placeholderDominio.Codigo)
                        {
                            case ConstanCodigoVariablesPlaceHolders.ENUM_PARTICIPANTE:
                                RegistrosFirmas = RegistrosFirmas
                                    .Replace(placeholderDominio.Nombre, enumFirmar++.ToString());
                                break;

                            case ConstanCodigoVariablesPlaceHolders.NOMBRE_PARTICIPANTE:
                                RegistrosFirmas = RegistrosFirmas
                                    .Replace(placeholderDominio.Nombre, SesionParticipante.Usuario.Nombres + " " + SesionParticipante.Usuario.Apellidos);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.CARGO_PARTICIPANTE:
                                RegistrosFirmas = RegistrosFirmas
                                    .Replace(placeholderDominio.Nombre, "");
                                break;

                            case ConstanCodigoVariablesPlaceHolders.FIRMA_PARTICIPANTE:
                                RegistrosFirmas = RegistrosFirmas
                                    .Replace(placeholderDominio.Nombre, "______________________");
                                break;
                        }

                    }
                }

                //Anexos
                string Anexos = string.Empty;

                //Plantilla Compromisos Solicitud
                string PlantillaFichaContratacion = _context.Plantilla
                 .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Ficha_De_Contratacion)
                    .ToString()).FirstOrDefault()
                 .Contenido;
                string RegistrosFichaContratacion = string.Empty;
                //Plantilla proceso de seleccion
                string PlantillaFichaProcesosSeleccion = _context.Plantilla
                 .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Ficha_De_Procesos_De_Seleccion)
                    .ToString()).FirstOrDefault()
                 .Contenido;

                string RegistrosFichaProcesosSeleccion = string.Empty;

                foreach (var scst in pComiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario)
                {
                    switch (scst.TipoSolicitudCodigo)
                    {
                        case ConstanCodigoTipoSolicitud.Contratacion:
                            RegistrosFichaContratacion += PlantillaFichaContratacion;
                            RegistrosFichaContratacion = ReemplazarDatosPlantillaContratacion(RegistrosFichaContratacion, await _IProjectContractingService.GetAllContratacionByContratacionId(scst.SolicitudId));
                            break;

                        case ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion:
                            RegistrosFichaProcesosSeleccion += PlantillaFichaProcesosSeleccion;
                            RegistrosFichaProcesosSeleccion = ReemplazarDatosPlantillaProcesosSeleccion(RegistrosFichaProcesosSeleccion, await GetProcesosSelecccionByProcesoSeleccionId(scst.SolicitudId));
                            break;

                        default:
                            break;
                    }

                }
                //Suma de las fichas 
                RegistrosFichaContratacion += RegistrosFichaProcesosSeleccion;

                //Plantilla Principal 
                foreach (Dominio placeholderDominio in placeholders)
                {
                    switch (placeholderDominio.Codigo)
                    {
                        //Tablas dinamicas  
                        case ConstanCodigoVariablesPlaceHolders.TITULO_SOLICITUDES_CONTRACTUALES:
                            string strTituloSolicitudesContractuales = string.Empty;
                            if (pComiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario.Count() > 0)
                            {
                                strTituloSolicitudesContractuales = _context.Plantilla.Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Titulo_solicitudes_contractuales)
                                .ToString()).FirstOrDefault().Contenido;
                            }
                            strContenido = strContenido
                                .Replace(placeholderDominio.Nombre, strTituloSolicitudesContractuales);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.TITULOS_TEMAS_NUEVOS:
                            string strTituloTemasNuevos = string.Empty;
                            if (pComiteTecnico.SesionComiteTema.Where(r => r.EsProposicionesVarios == null).Count() > 0)
                            {
                                strTituloTemasNuevos = _context.Plantilla.Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Titulo_temas_nuevos)
                                .ToString()).FirstOrDefault().Contenido;
                            }
                            strContenido = strContenido
                                .Replace(placeholderDominio.Nombre, strTituloTemasNuevos);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.TITULO_PROPOSICIONES_VARIOS:
                            string strTituloProposicionVarios = string.Empty;

                            if (pComiteTecnico.SesionComiteTema.Where(r => r.EsProposicionesVarios != null).Count() > 0)
                            {
                                strTituloProposicionVarios = _context.Plantilla.Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Titulo_proposicione_varios)
                                .ToString()).FirstOrDefault().Contenido;
                            }
                            strContenido = strContenido
                                .Replace(placeholderDominio.Nombre, strTituloProposicionVarios);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.TABLA_VALIDACION_CONTRACTUAL:
                            strContenido = strContenido
                                .Replace(placeholderDominio.Nombre, PlantillaSolicitudesContractuales);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.TABLA_TEMAS:
                            strContenido = strContenido
                                .Replace(placeholderDominio.Nombre, RegistrosTemas);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.TABLA_PROPOSICIONES_VARIOS:
                            strContenido = strContenido
                                .Replace(placeholderDominio.Nombre, RegistrosProposicionesVarios);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.REGISTROS_TABLA_INVITADOS:
                            strContenido = strContenido
                                .Replace(placeholderDominio.Nombre, RegistrosInvitados);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.SOLICITUDES_CONTRATO:
                            strContenido = strContenido
                                .Replace(placeholderDominio.Nombre, registrosContratacion);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.SOLICITUDES_PROCESO_SELECCION:
                            strContenido = strContenido
                                .Replace(placeholderDominio.Nombre, registrosProcesosSelecccion);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.REGISTROS_TEMAS:
                            strContenido = strContenido
                                .Replace(placeholderDominio.Nombre, RegistrosNuevosTemas);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.REGISTROS_PROPOSICIONES_VARIOS:
                            strContenido = strContenido
                                .Replace(placeholderDominio.Nombre, RegistrosProposicionVarios);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.REGISTROS_FIRMAS:
                            strContenido = strContenido
                                .Replace(placeholderDominio.Nombre, RegistrosFirmas);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.REGISTROS_ANEXOS:
                            strContenido = strContenido
                                .Replace(placeholderDominio.Nombre, RegistrosFichaContratacion);
                            break;

                        //Registros de plantilla
                        //Registros de plantilla
                        case ConstanCodigoVariablesPlaceHolders.NUMERO_COMITE:
                            strContenido = strContenido
                                .Replace(placeholderDominio.Nombre, pComiteTecnico.NumeroComite);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.FECHA_COMITE:
                            strContenido = strContenido
                                .Replace(placeholderDominio.Nombre, ((DateTime)pComiteTecnico.FechaOrdenDia).ToString("dd-MM-yyyy"));
                            break;

                        case ConstanCodigoVariablesPlaceHolders.MIEMBROS_PARTICIPANTES:
                            string strUsuariosParticipantes = string.Empty;
                            ListSesionParticipante.ForEach(user =>
                            {
                                strUsuariosParticipantes += user.Usuario.Nombres + " " + user.Usuario.Apellidos + " ";
                            });
                            strContenido = strContenido.Replace(placeholderDominio.Nombre, strUsuariosParticipantes);
                            break;
                    }
                }

                return strContenido;
            }
            catch (Exception ex)
            {
                return ex.InnerException.ToString();
            }
        }

    }
}
