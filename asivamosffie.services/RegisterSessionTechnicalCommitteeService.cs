﻿using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class RegisterSessionTechnicalCommitteeService : IRegisterSessionTechnicalCommitteeService
    {

        #region Constructor
        private readonly ICommonService _commonService;
        private readonly IProjectContractingService _IProjectContractingService;
        private readonly IProjectService _IprojectService;
        private readonly IContractualControversy _IContractualControversy;
        private readonly devAsiVamosFFIEContext _context;
        public readonly IConverter _converter;
        private readonly IContractualNoveltyService _IContractualNoveltyService;
        private readonly IJudicialDefense _judicialDefense;

        public RegisterSessionTechnicalCommitteeService(devAsiVamosFFIEContext context, IProjectService projectService, IConverter converter, ICommonService commonService, IProjectContractingService projectContractingService, IContractualControversy contractualControversy, IContractualNoveltyService contractualNoveltyService, IJudicialDefense judicialDefense)
        {
            _IProjectContractingService = projectContractingService;
            _commonService = commonService;
            _context = context;
            _IprojectService = projectService;
            _converter = converter;
            _IContractualControversy = contractualControversy;
            _judicialDefense = judicialDefense;
            _IContractualNoveltyService = contractualNoveltyService;
        }
        #endregion

        #region Votación
        public async Task<Respuesta> CreateEditSesionSolicitudVoto(SesionComiteSolicitud pSesionComiteSolicitud)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Sesion_Solicitud_Voto, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = string.Empty;
            try
            {
                SesionComiteSolicitud sesionComiteSolicitudOld = _context.SesionComiteSolicitud.Find(pSesionComiteSolicitud.SesionComiteSolicitudId);
                //sesionComiteSolicitudOld.EstadoCodigo = pSesionComiteSolicitud.EstadoCodigo;
                sesionComiteSolicitudOld.RequiereVotacion = true;
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
                    Data = await GetComiteTecnicoByComiteTecnicoId((int)pSesionComiteSolicitud.ComiteTecnicoId),
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccion, pSesionComiteSolicitud.UsuarioCreacion, CreateEdit)
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

        public async Task<Respuesta> GetNoRequiereVotacionSesionComiteSolicitud(SesionComiteSolicitud pSesionComiteSolicitud)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.No_Requiere_Votacion_Sesion_Comite_Solicitud, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                SesionComiteSolicitud sesionComiteSolicitudOld = _context.SesionComiteSolicitud.Find(pSesionComiteSolicitud.SesionComiteSolicitudId);

                sesionComiteSolicitudOld.RequiereVotacion = pSesionComiteSolicitud.RequiereVotacion;
                sesionComiteSolicitudOld.UsuarioModificacion = pSesionComiteSolicitud.UsuarioCreacion;
                sesionComiteSolicitudOld.FechaModificacion = DateTime.Now;

                _context.SaveChanges();
                return

                new Respuesta
                {
                    Data = await GetComiteTecnicoByComiteTecnicoId((int)pSesionComiteSolicitud.ComiteTecnicoId),
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccion, pSesionComiteSolicitud.UsuarioCreacion, "NO REQUIERE VOTACIÓN")
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

        public async Task<List<SesionComentario>> GetCometariosDelActa(int pComietTecnicoId)
        {

            List<SesionComentario> listaComentarios = new List<SesionComentario>();

            listaComentarios = _context.SesionComentario
                                .Where(sc => sc.ComiteTecnicoId == pComietTecnicoId &&
                                             sc.EstadoActaVoto == "4")
                                .Include(r => r.MiembroSesionParticipante)
                                .ToList();

            return listaComentarios;
        }

        public async Task<Respuesta> CreateEditSesionTemaVoto(SesionComiteTema pSesionComiteTema)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Comite_Tema_Voto, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                SesionComiteTema sesionComiteTemaOld = _context.SesionComiteTema.Find(pSesionComiteTema.SesionTemaId);
                string CrearEditar = "";
                sesionComiteTemaOld.RequiereVotacion = true;
                //sesionComiteTemaOld.EstadoTemaCodigo = pSesionComiteTema.EstadoTemaCodigo;
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccion, pSesionComiteTema.UsuarioCreacion, CrearEditar)
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
                   Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccion, pSesionComiteTema.UsuarioCreacion, ex.InnerException.ToString())
               };
            }

        }

        private bool ValidarRegistroCompletoSesionComiteTema(SesionComiteTema sesionComiteTemaOld, List<TemaCompromiso> listaCompromisos = null)
        {
            bool esCompleto = true;

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
                || (sesionComiteTemaOld.GeneraCompromiso == true && sesionComiteTemaOld.CantCompromisos == null)

                )
            {

                esCompleto = false;
            }

            sesionComiteTemaOld.TemaCompromiso.Where(x => x.Eliminado != true).ToList().ForEach(compromiso =>
          {
              if (
                    string.IsNullOrEmpty(compromiso.Tarea) ||
                    compromiso.Responsable == null ||
                    compromiso.FechaCumplimiento == null
              )
              {
                  esCompleto = false;
              }
          });

            listaCompromisos?.Where(x => x.Eliminado != true).ToList().ForEach(compromiso =>
            {
                if (
                      string.IsNullOrEmpty(compromiso.Tarea) ||
                      compromiso.Responsable == null ||
                      compromiso.FechaCumplimiento == null
                )
                {
                    esCompleto = false;
                }
            });

            return esCompleto;

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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccion, pUsuarioCreacion, "NO REQUIERE VOTACIÓN SESIÓN COMITE TEMA")
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
                   Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccion, pUsuarioCreacion, ex.InnerException.ToString())
               };
            }

        }

        public async Task<Respuesta> AplazarSesionComite(ComiteTecnico pComiteTecnico, string pDominio, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Aplazar_Sesion_De_Comite, (int)EnumeratorTipoDominio.Acciones);
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
                string strContenido = plantilla.Contenido;

                List<Dominio> ListaParametricas = _context.Dominio.ToList();

                foreach (Dominio placeholderDominio in placeholders)
                {
                    switch (placeholderDominio.Codigo)
                    {
                        case ConstanCodigoVariablesPlaceHolders.COMITE_NUMERO:
                            strContenido = strContenido.Replace(placeholderDominio.Nombre, comiteTecnicoOld.NumeroComite);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.COMITE_FECHA:
                            strContenido = strContenido.Replace(placeholderDominio.Nombre, fechaAnterior.ToString("dd-MM-yyyy"));
                            break;

                        case ConstanCodigoVariablesPlaceHolders.COMITE_FECHA_APLAZAMIENTO:
                            strContenido = strContenido.Replace(placeholderDominio.Nombre, ((DateTime)comiteTecnicoOld.FechaAplazamiento).ToString("dd-MM-yyyy"));
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
                        if (!(bool)Helpers.Helpers.EnviarCorreo(Usuario.Email, "Aplazar sesión comité técnico", strContenido, pSentender, pPassword, pMailServer, pMailPort))
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
                      Code = ConstantSesionComiteTecnico.AplazarExitoso,
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.AplazarExitoso, idAccion, pComiteTecnico.UsuarioCreacion, "APLAZAR SESIÓN COMITE")
                  };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region Comite Tecnico

        public async Task<List<SesionSolicitudObservacionProyecto>> GetSesionSolicitudObservacionProyecto(int pSesionComiteSolicitudId, int pContratacionProyectoId)
        {
            List<SesionSolicitudObservacionProyecto> lista = new List<SesionSolicitudObservacionProyecto>();

            lista = await _context.SesionSolicitudObservacionProyecto
                                .Where(s => s.SesionComiteSolicitudId == pSesionComiteSolicitudId &&
                                               s.ContratacionProyectoId == pContratacionProyectoId)
                                .Include(r => r.SesionParticipante)
                                    .ThenInclude(r => r.Usuario)
                                .ToListAsync();
            return lista;
        }

        public async Task<ComiteTecnico> GetCompromisosByComiteTecnicoId(int ComiteTecnicoId, bool pEsFiduciario)
        {
            //Dominio estado reportado 48 
            ComiteTecnico comiteTecnico = null;

            if (pEsFiduciario)
            {
                comiteTecnico = await _context.ComiteTecnico.Where(r => r.ComiteTecnicoId == ComiteTecnicoId)
                .Include(r => r.SesionComiteTema)
                   .ThenInclude(r => r.TemaCompromiso)
                       .ThenInclude(r => r.TemaCompromisoSeguimiento)

               .Include(r => r.SesionComiteSolicitudComiteTecnicoFiduciario)
                   .ThenInclude(r => r.SesionSolicitudCompromiso)
                //.ThenInclude(r => r.CompromisoSeguimiento)
                .FirstOrDefaultAsync();

            }
            else
            {
                comiteTecnico = await _context.ComiteTecnico.Where(r => r.ComiteTecnicoId == ComiteTecnicoId)
                .Include(r => r.SesionComiteTema)
                   .ThenInclude(r => r.TemaCompromiso)
                       .ThenInclude(r => r.TemaCompromisoSeguimiento)

               .Include(r => r.SesionComiteSolicitudComiteTecnico)
                   .ThenInclude(r => r.SesionSolicitudCompromiso)
                //.ThenInclude(r => r.CompromisoSeguimiento)

                .FirstOrDefaultAsync();
            }


            if (comiteTecnico == null)
                return new ComiteTecnico();
            List<Dominio> ListEstadoReportado = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Compromisos).ToList();
            List<SesionParticipante> ListSesionParticipantes = _context.SesionParticipante.Where(r => !(bool)r.Eliminado).Include(r => r.Usuario).ToList();

            comiteTecnico.NumeroCompromisos = 0;


            //Sacar los Eliminados
            foreach (var SesionComiteTema in comiteTecnico.SesionComiteTema.Where(r => !(bool)r.Eliminado).ToList())
            {
                SesionComiteTema.TemaCompromiso = SesionComiteTema.TemaCompromiso.Where(r => !(bool)r.Eliminado).ToList();
            }


            foreach (var SesionComiteTema in comiteTecnico.SesionComiteTema.Where(r => !(bool)r.Eliminado))
            {
                foreach (var TemaCompromiso in SesionComiteTema.TemaCompromiso.Where(r => !(bool)r.Eliminado))
                {

                    TemaCompromiso.EstadoCodigo = string.IsNullOrEmpty(TemaCompromiso.EstadoCodigo) ? ConstantStringCompromisos.Sin_Iniciar : ListEstadoReportado.Where(r => r.Codigo == TemaCompromiso.EstadoCodigo).FirstOrDefault().Nombre;

                    if (TemaCompromiso.Responsable != null)
                    {
                        TemaCompromiso.ResponsableNavigation = ListSesionParticipantes.Where(r => r.SesionParticipanteId == TemaCompromiso.Responsable).FirstOrDefault();
                    }
                    if (TemaCompromiso.EstadoCodigo == ConstantCodigoCompromisos.Finalizado)
                        comiteTecnico.NumeroCompromisosCumplidos++;
                    comiteTecnico.NumeroCompromisos++;
                }
            }

            if (!pEsFiduciario)
            {
                if (comiteTecnico.SesionComiteSolicitudComiteTecnico.Where(r => !(bool)r.Eliminado).Count() > 0)
                    comiteTecnico.SesionComiteSolicitudComiteTecnico = comiteTecnico.SesionComiteSolicitudComiteTecnico.Where(r => !(bool)r.Eliminado).ToList();

                foreach (var SesionComiteSolicitudComiteTecnico in comiteTecnico.SesionComiteSolicitudComiteTecnico.Where(r => !(bool)r.Eliminado))
                {
                    SesionComiteSolicitudComiteTecnico.SesionSolicitudCompromiso = SesionComiteSolicitudComiteTecnico.SesionSolicitudCompromiso.Where(r => !(bool)r.Eliminado && r.EsFiduciario != true).ToList();
                }

                foreach (var SesionComiteSolicitud in comiteTecnico.SesionComiteSolicitudComiteTecnico.Where(r => !(bool)r.Eliminado))
                {
                    foreach (var SesionSolicitudCompromiso in SesionComiteSolicitud.SesionSolicitudCompromiso.Where(r => !(bool)r.Eliminado))
                    {

                        SesionSolicitudCompromiso.CompromisoSeguimiento = _context.VCompromisoSeguimiento
                                                                                    .Where(r => r.SesionSolicitudCompromisoId == SesionSolicitudCompromiso.SesionSolicitudCompromisoId)
                                                                                       .ToList()
                                                                                       .ConvertAll(x => new CompromisoSeguimiento
                                                                                       {
                                                                                           CompromisoSeguimientoId = x.CompromisoSeguimientoId,
                                                                                           DescripcionSeguimiento = x.DescripcionSeguimiento,
                                                                                           Eliminado = x.Eliminado,
                                                                                           SesionParticipanteId = x.SesionParticipanteId,
                                                                                           EstadoCompromisoCodigo = x.EstadoCompromisoCodigo,
                                                                                           SesionSolicitudCompromisoId = x.SesionSolicitudCompromisoId,

                                                                                       })
                                                                                       .ToList();

                        SesionSolicitudCompromiso.EstadoCodigo = string.IsNullOrEmpty(SesionSolicitudCompromiso.EstadoCodigo) ? ConstantStringCompromisos.Sin_Iniciar : ListEstadoReportado.Where(r => r.Codigo == SesionSolicitudCompromiso.EstadoCodigo).FirstOrDefault().Nombre;

                        if (SesionSolicitudCompromiso.ResponsableSesionParticipanteId > 0)
                        {
                            SesionSolicitudCompromiso.ResponsableSesionParticipante = ListSesionParticipantes.Where(r => r.SesionParticipanteId == SesionSolicitudCompromiso.ResponsableSesionParticipanteId).FirstOrDefault();
                        }
                        if (SesionSolicitudCompromiso.EstadoCodigo == ConstantCodigoCompromisos.Finalizado)
                            comiteTecnico.NumeroCompromisosCumplidos++;
                        comiteTecnico.NumeroCompromisos++;
                    }
                }
            }

            if (pEsFiduciario)
            {
                if (comiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario.Where(r => !(bool)r.Eliminado).Count() > 0)
                    comiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario = comiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario.Where(r => !(bool)r.Eliminado).ToList();

                foreach (var SesionComiteSolicitudComiteTecnicoFiduciario in comiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario.Where(r => !(bool)r.Eliminado))
                {
                    SesionComiteSolicitudComiteTecnicoFiduciario.SesionSolicitudCompromiso = SesionComiteSolicitudComiteTecnicoFiduciario.SesionSolicitudCompromiso.Where(r => !(bool)r.Eliminado && r.EsFiduciario == true).ToList();
                }

                foreach (var SesionComiteSolicitud in comiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario.Where(r => !(bool)r.Eliminado))
                {
                    foreach (var SesionSolicitudCompromiso in SesionComiteSolicitud.SesionSolicitudCompromiso.Where(r => !(bool)r.Eliminado))
                    {
                        SesionSolicitudCompromiso.CompromisoSeguimiento = _context.VCompromisoSeguimiento
                                                                                    .Where(r => r.SesionSolicitudCompromisoId == SesionSolicitudCompromiso.SesionSolicitudCompromisoId)
                                                                                       .ToList()
                                                                                       .ConvertAll(x => new CompromisoSeguimiento
                                                                                       {
                                                                                           CompromisoSeguimientoId = x.CompromisoSeguimientoId,
                                                                                           DescripcionSeguimiento = x.DescripcionSeguimiento,
                                                                                           Eliminado = x.Eliminado,
                                                                                           SesionParticipanteId = x.SesionParticipanteId,
                                                                                           EstadoCompromisoCodigo = x.EstadoCompromisoCodigo,
                                                                                           SesionSolicitudCompromisoId = x.SesionSolicitudCompromisoId,

                                                                                       })
                                                                                       .ToList();

                        SesionSolicitudCompromiso.EstadoCodigo = string.IsNullOrEmpty(SesionSolicitudCompromiso.EstadoCodigo) ? ConstantStringCompromisos.Sin_Iniciar : ListEstadoReportado.Where(r => r.Codigo == SesionSolicitudCompromiso.EstadoCodigo).FirstOrDefault().Nombre;

                        if (SesionSolicitudCompromiso.ResponsableSesionParticipanteId > 0)
                        {
                            SesionSolicitudCompromiso.ResponsableSesionParticipante = ListSesionParticipantes.Where(r => r.SesionParticipanteId == SesionSolicitudCompromiso.ResponsableSesionParticipanteId).FirstOrDefault();
                        }
                        if (SesionSolicitudCompromiso.EstadoCodigo == ConstantCodigoCompromisos.Finalizado)
                            comiteTecnico.NumeroCompromisosCumplidos++;
                        comiteTecnico.NumeroCompromisos++;
                    }
                }
            }


            return comiteTecnico;
        }

        public async Task<Respuesta> DeleteComiteTecnicoByComiteTecnicoId(int pComiteTecnicoId, string pUsuarioModifico)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Comite_Tecnico, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ComiteTecnico comiteTecnicoOld = _context.ComiteTecnico
                                                            .Where(ct => ct.ComiteTecnicoId == pComiteTecnicoId)
                                                            .Include(r => r.SesionComiteTema)
                                                            .Include(r => r.SesionComiteSolicitudComiteTecnico)
                                                            .FirstOrDefault();

                if (comiteTecnicoOld.SesionComiteTema.Where(t => t.Eliminado != true).ToList().Count > 0)
                {
                    return new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = ConstantSesionComiteTecnico.ErrorEliminarDependencia,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.ErrorEliminarDependencia, idAccion, pUsuarioModifico, "ELIMINAR COMITE TECNICO")
                    };
                }


                comiteTecnicoOld.SesionComiteSolicitudComiteTecnico.ToList().ForEach(ct =>
                {
                    SesionComiteSolicitud solicitud = _context.SesionComiteSolicitud.Find(ct.SesionComiteSolicitudId);

                    solicitud.Eliminado = true;
                });

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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.EliminacionExitosa, idAccion, pUsuarioModifico, "ELIMINAR COMITE TECNICO")
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccion, pUsuarioModifico, ex.InnerException.ToString())
                };
            }
        }

        public async Task<Respuesta> ConvocarComiteTecnico(ComiteTecnico pComiteTecnico, string pDominio, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Convocar_Comite_Tecnico, (int)EnumeratorTipoDominio.Acciones);
            List<Dominio> placeholders = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.PlaceHolder).ToList();

            try
            {
                ComiteTecnico comiteTecnico = await _context.ComiteTecnico
                    .Where(r => r.ComiteTecnicoId == pComiteTecnico.ComiteTecnicoId)
                    .Include(r => r.SesionComiteTema)
                    .Include(r => r.SesionComiteSolicitudComiteTecnico)
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

                foreach (var item in comiteTecnico.SesionComiteSolicitudComiteTecnico)
                {
                    strOrdenDia += TemplateOrdenDia.Contenido;
                    contador++;


                    switch (item.TipoSolicitudCodigo)
                    {
                        case ConstanCodigoTipoSolicitud.Contratacion:
                            item.NumeroSolicitud = _context.Contratacion.Find(item.SolicitudId).NumeroSolicitud;
                            break;
                        case ConstanCodigoTipoSolicitud.Evaluacion_De_Proceso:
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

                        blEnvioCorreo = Helpers.Helpers.EnviarCorreo(Usuario.Email, "Convocatoria sesión de comité técnico", template, pSentender, pPassword, pMailServer, pMailPort);
                    }
                }

                _context.SaveChanges();
                return
                   new Respuesta
                   {
                       Data = await GetComiteTecnicoByComiteTecnicoId(pComiteTecnico.ComiteTecnicoId),
                       IsSuccessful = true,
                       IsException = false,
                       IsValidation = false,
                       Code = ConstantSesionComiteTecnico.OperacionExitosa,
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccion, pComiteTecnico.UsuarioCreacion, "CONVOCAR COMITE TECNICO")
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
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccion, pComiteTecnico.UsuarioCreacion, ex.InnerException.ToString())
                   };
            }
        }

        public async Task<Respuesta> EnviarComiteParaAprobacion(ComiteTecnico pComiteTecnico, string pDominio, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Convocar_Comite_Tecnico, (int)EnumeratorTipoDominio.Acciones);
            List<Dominio> placeholders = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.PlaceHolder).ToList();

            try
            {
                ComiteTecnico comiteTecnico = await _context.ComiteTecnico
                    .Where(r => r.ComiteTecnicoId == pComiteTecnico.ComiteTecnicoId)
                    .Include(r => r.SesionParticipante)
                        .ThenInclude(r => r.Usuario).FirstOrDefaultAsync();

                comiteTecnico.EstadoComiteCodigo = ConstanCodigoEstadoComite.Con_Acta_De_Sesion_Enviada;
                comiteTecnico.EstadoActaCodigo = ConstantCodigoActas.En_proceso_Aprobacion;
                comiteTecnico.UsuarioModificacion = pComiteTecnico.UsuarioCreacion;
                comiteTecnico.FechaModificacion = DateTime.Now;


                Template TemplateConvocar = await _commonService.GetTemplateById((int)enumeratorTemplate.EnviarComiteAprobacion);

                string template = TemplateConvocar.Contenido.Replace("[NUMERO_COMITE]", comiteTecnico.NumeroComite)
                                                            .Replace("[FECHA_COMITE]", comiteTecnico.FechaOrdenDia.Value.ToString("dd/MM/yyyy"))
                                                            .Replace("[URL_APLICACION]", pDominioFront + "compromisosActasComite");



                //Notificar a los participantes
                bool blEnvioCorreo = false;

                //TODO: esta lista debe ser parametrizada de acuerdo a los perfiles Directore de las 4 areas :
                //Director financiero, Director Juridico , Director técnico, y Director administrativo

                foreach (var participante in comiteTecnico.SesionParticipante)
                {
                    if (participante.Usuario != null && !string.IsNullOrEmpty(participante.Usuario.Email))
                    {

                        blEnvioCorreo = Helpers.Helpers.EnviarCorreo(participante.Usuario.Email, comiteTecnico.EsComiteFiduciario.Value ? "Aprobacion acta comité fiduciario" : "Aprobacion acta comité técnico", template, pSentender, pPassword, pMailServer, pMailPort);
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
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccion, pComiteTecnico.UsuarioCreacion, "CONVOCAR COMITE TECNICO")
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
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccion, pComiteTecnico.UsuarioCreacion, ex.InnerException.ToString())
                   };
            }
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
                    _context.SaveChanges();
                }

                return
                new Respuesta
                {
                    Data = await GetComiteTecnicoByComiteTecnicoId((int)ListSesionComiteTemas.FirstOrDefault().ComiteTecnicoId),
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccion, ListSesionComiteTemas.FirstOrDefault().UsuarioCreacion, CreateEdit)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccion, ListSesionComiteTemas.FirstOrDefault().UsuarioCreacion, ex.InnerException.ToString())
                  };
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
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccion, pUsuarioModificacion, "NO SE ENCONTRO SESION INVITADO")
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
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccion, pUsuarioModificacion, "NO SE ENCONTRO SESION INVITADO")
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

        public async Task<List<dynamic>> GetListSesionComiteSolicitudByFechaOrdenDelDia(DateTime pFechaOrdenDelDia)
        {
            List<dynamic> ListValidacionSolicitudesContractualesGrilla = new List<dynamic>();

            try
            {
                int CantidadDiasComite = Int32.Parse(await _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Dias_Comite && (bool)r.Activo).Select(r => r.Descripcion).FirstOrDefaultAsync());
                pFechaOrdenDelDia = pFechaOrdenDelDia.AddDays(-CantidadDiasComite);

                #region Buscar Solicitudes

                List<ProcesoSeleccion> ListProcesoSeleccion =
                    _context.ProcesoSeleccion
                    .Where(r => !(bool)r.Eliminado
                     && r.EstadoProcesoSeleccionCodigo == ConstanCodigoEstadoProcesoSeleccion.Apertura_En_Tramite
                     && r.FechaCreacion < pFechaOrdenDelDia
                     )
                    .OrderByDescending(r => r.ProcesoSeleccionId).ToList();

                List<ProcesoSeleccion> ListProcesoSeleccionEvaluacion =
                    _context.ProcesoSeleccion
                    .Where(r => !(bool)r.Eliminado
                     && r.EstadoProcesoSeleccionCodigo == ConstanCodigoEstadoProcesoSeleccion.AprobacionDeSeleccionEnTramite
                     && r.FechaCreacion < pFechaOrdenDelDia
                     )
                    .OrderByDescending(r => r.ProcesoSeleccionId).ToList();

                List<Contratacion> ListContratacion = _context.Contratacion
                    .Where(r => !(bool)r.Eliminado
                    && r.EstadoSolicitudCodigo == ConstanCodigoEstadoSolicitudContratacion.En_tramite
                    && r.FechaTramite < pFechaOrdenDelDia
                    )
                    .OrderByDescending(r => r.ContratacionId).ToList();

                List<ProcesoSeleccionMonitoreo> ListActualizacionCronograma = _context.ProcesoSeleccionMonitoreo
                    .Where(r => !(bool)r.Eliminado
                    && r.EnviadoComiteTecnico == true
                    && r.FechaCreacion < pFechaOrdenDelDia
                    )
                    .Include(r => r.ProcesoSeleccion)
                    .OrderByDescending(r => r.ProcesoSeleccionMonitoreoId).ToList();

                List<ControversiaContractual> ListControversiasContractuales = _context.ControversiaContractual
                    .Where(r => !(bool)r.Eliminado
                    && r.EsRequiereComite == true
                    && r.EstadoCodigo == ConstanCodigoEstadoControversiasContractuales.EnviadaComiteTecnico
                    && r.TipoControversiaCodigo == "1" // TAI
                    && r.FechaSolicitud < pFechaOrdenDelDia
                    )
                    .OrderByDescending(r => r.ControversiaContractualId).ToList();

                List<ControversiaActuacion> ListControversiasActuaciones = _context.ControversiaActuacion
                    .Where(r => !(bool)r.Eliminado
                    && r.EsRequiereComite == true
                    && r.EstadoCodigo == ConstantCodigoEstadoControversiaActuacion.Enviado_a_comite_tecnico
                    && r.FechaActuacion < pFechaOrdenDelDia
                    )
                    .Include(r => r.ControversiaContractual)
                    .OrderByDescending(r => r.ControversiaContractualId).ToList();


                List<ControversiaActuacion> ListControversiasActuacionReclmacion = _context.ControversiaActuacion
                    .Where(r => !(bool)r.Eliminado
                    && r.EsRequiereComiteReclamacion == true
                    && r.EstadoActuacionReclamacionCodigo == ConstanCodigoEstadosActuacionReclamacion.Enviado_a_comite_tecnico //enviado a comite
                    && r.FechaActuacion < pFechaOrdenDelDia
                    )
                    .Include(r => r.ControversiaContractual)
                    .OrderByDescending(r => r.ControversiaContractualId).ToList();

                List<DefensaJudicial> ListDefensaJudicial = _context.DefensaJudicial
                    .Where(r => !(bool)r.Eliminado
                    && r.FichaEstudio.FirstOrDefault().EsActuacionTramiteComite == true
                    && r.EstadoProcesoCodigo == ConstanCodigoEstadosDefensaJudicial.Enviado_a_comite_tecnico
                    && r.FechaCreacion < pFechaOrdenDelDia // no estoy seguro de esto
                    )
                    .OrderByDescending(r => r.DefensaJudicialId).ToList();

                List<DefensaJudicialSeguimiento> ListDefensaJudicialSeguimiento = _context.DefensaJudicialSeguimiento
                    .Where(r => !(bool)r.Eliminado
                    //&& r.FichaEstudio.FirstOrDefault().EsActuacionTramiteComite == true
                    && r.EstadoProcesoCodigo == ConstanCodigoEstadosDefensaJudicial.Enviado_a_comite_tecnico
                    && r.FechaActuacion < pFechaOrdenDelDia // no estoy seguro de esto
                    )
                    .Include(r => r.DefensaJudicial)
                    .OrderByDescending(r => r.DefensaJudicialId).ToList();

                List<NovedadContractual> ListNovedadContractual = _context.NovedadContractual
                    .Where(r => !(bool)r.Eliminado
                    && r.EstadoCodigo == ConstanCodigoEstadoNovedadContractual.Enviada_a_comite_tecnico
                    && r.FechaSolictud < pFechaOrdenDelDia // no estoy seguro de esto
                    )
                    .OrderByDescending(r => r.FechaSolictud).ToList();

                #endregion Buscar Solicitudes

                #region Quitar los que ya estan en sesionComiteSolicitud

                List<int> LisIdContratacion = _context.SesionComiteSolicitud
                                                .Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion.ToString() &&
                                                       r.Eliminado != true &&
                                                       r.EstadoCodigo != ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_fiduciario &&
                                                       r.EstadoCodigo != ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_tecnico
                                                        )
                                                .Select(r => r.SolicitudId).Distinct().ToList();

                List<int> ListIdProcesosSeleccion = _context.SesionComiteSolicitud
                                                        .Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion &&
                                                            r.Eliminado != true &&
                                                            r.EstadoCodigo != ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_fiduciario &&
                                                            r.EstadoCodigo != ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_tecnico
                                                            )
                                                        .Select(r => r.SolicitudId).Distinct().ToList();

                List<int> ListIdProcesosSeleccionEvaluacion = _context.SesionComiteSolicitud
                                                        .Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Evaluacion_De_Proceso &&
                                                            r.Eliminado != true &&
                                                            r.EstadoCodigo != ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_fiduciario &&
                                                            r.EstadoCodigo != ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_tecnico
                                                            )
                                                        .Select(r => r.SolicitudId).Distinct().ToList();

                List<int> ListIdActualizacionCronograma = _context.SesionComiteSolicitud
                                                            .Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Actualizacion_Cronograma_Proceso_Seleccion &&
                                                                r.Eliminado != true &&
                                                                r.EstadoCodigo != ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_fiduciario &&
                                                                r.EstadoCodigo != ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_tecnico
                                                                 )
                                                            .Select(r => r.SolicitudId).Distinct().ToList();

                List<int> ListIdControversiasContractuales = _context.SesionComiteSolicitud
                                                            .Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.ControversiasContractuales &&
                                                                r.Eliminado != true &&
                                                                r.EstadoCodigo != ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_fiduciario &&
                                                                r.EstadoCodigo != ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_tecnico
                                                                 )
                                                            .Select(r => r.SolicitudId).Distinct().ToList();

                List<int> ListIdControversiasActuaciones = _context.SesionComiteSolicitud
                                                            .Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Actuaciones_Controversias_Contractuales &&
                                                                r.Eliminado != true &&
                                                                r.EstadoCodigo != ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_fiduciario &&
                                                                r.EstadoCodigo != ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_tecnico
                                                                 )
                                                            .Select(r => r.SolicitudId).Distinct().ToList();

                List<int> ListIdControversiasActuacionesReclamaciones = _context.SesionComiteSolicitud
                                                            .Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Actuaciones_Controversias_Reclamaciones &&
                                                                r.Eliminado != true &&
                                                                r.EstadoCodigo != ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_fiduciario &&
                                                                r.EstadoCodigo != ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_tecnico
                                                                 )
                                                            .Select(r => r.SolicitudId).Distinct().ToList();

                List<int> ListIdDefensaJudicial = _context.SesionComiteSolicitud
                                                            .Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Defensa_judicial &&
                                                                r.Eliminado != true &&
                                                                r.EstadoCodigo != ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_fiduciario &&
                                                                r.EstadoCodigo != ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_tecnico
                                                                 )
                                                            .Select(r => r.SolicitudId).Distinct().ToList();

                List<int> ListIdDefensaJudicialSeguimiento = _context.SesionComiteSolicitud
                                                            .Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Actuaciones_Defensa_judicial &&
                                                                r.Eliminado != true &&
                                                                r.EstadoCodigo != ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_fiduciario &&
                                                                r.EstadoCodigo != ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_tecnico
                                                                 )
                                                            .Select(r => r.SolicitudId).Distinct().ToList();

                List<int> ListIdNovedadContractual = _context.SesionComiteSolicitud
                                                            .Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Novedad_Contractual &&
                                                                r.Eliminado != true &&
                                                                r.EstadoCodigo != ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_fiduciario &&
                                                                r.EstadoCodigo != ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_tecnico
                                                                 )
                                                            .Select(r => r.SolicitudId).Distinct().ToList();


                ListContratacion.RemoveAll(item => LisIdContratacion.Contains(item.ContratacionId));
                ListProcesoSeleccion.RemoveAll(item => ListIdProcesosSeleccion.Contains(item.ProcesoSeleccionId));
                ListProcesoSeleccionEvaluacion.RemoveAll(item => ListIdProcesosSeleccionEvaluacion.Contains(item.ProcesoSeleccionId));
                ListActualizacionCronograma.RemoveAll(item => ListIdActualizacionCronograma.Contains(item.ProcesoSeleccionMonitoreoId));
                ListControversiasContractuales.RemoveAll(item => ListIdControversiasContractuales.Contains(item.ControversiaContractualId));
                ListControversiasActuaciones.RemoveAll(item => ListIdControversiasActuaciones.Contains(item.ControversiaActuacionId));
                ListControversiasActuacionReclmacion.RemoveAll(item => ListIdControversiasActuacionesReclamaciones.Contains(item.ControversiaActuacionId));
                ListDefensaJudicial.RemoveAll(item => ListIdDefensaJudicial.Contains(item.DefensaJudicialId));
                ListDefensaJudicialSeguimiento.RemoveAll(item => ListIdDefensaJudicialSeguimiento.Contains(item.DefensaJudicialSeguimientoId));
                ListNovedadContractual.RemoveAll(item => ListIdNovedadContractual.Contains(item.NovedadContractualId));

                #endregion Quitar los que ya estan en sesionComiteSolicitud

                #region Carga las solicitudes

                List<Dominio> ListTipoSolicitud = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud).ToList();

                foreach (var ProcesoSeleccion in ListProcesoSeleccion)
                {
                    ListValidacionSolicitudesContractualesGrilla.Add(new
                    {
                        Id = ProcesoSeleccion.ProcesoSeleccionId,
                        FechaSolicitud = ProcesoSeleccion.FechaCreacion.ToString("yyyy-MM-dd"),
                        NumeroSolicitud = ProcesoSeleccion.NumeroProceso,
                        TipoSolicitud = ListTipoSolicitud.Where(r => r.Codigo == ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion).FirstOrDefault().Nombre,
                        tipoSolicitudNumeroTabla = ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion
                    });
                };

                foreach (var ProcesoSeleccion in ListProcesoSeleccionEvaluacion)
                {
                    ListValidacionSolicitudesContractualesGrilla.Add(new
                    {
                        Id = ProcesoSeleccion.ProcesoSeleccionId,
                        FechaSolicitud = ProcesoSeleccion.FechaCreacion.ToString("yyyy-MM-dd"),
                        NumeroSolicitud = ProcesoSeleccion.NumeroProceso,
                        TipoSolicitud = ListTipoSolicitud.Where(r => r.Codigo == ConstanCodigoTipoSolicitud.Evaluacion_De_Proceso).FirstOrDefault().Nombre,
                        tipoSolicitudNumeroTabla = ConstanCodigoTipoSolicitud.Evaluacion_De_Proceso
                    });
                };

                foreach (var Contratacion in ListContratacion)
                {
                    ListValidacionSolicitudesContractualesGrilla.Add(new
                    {
                        Id = Contratacion.ContratacionId,
                        FechaSolicitud = Contratacion.FechaTramite != null ? Convert.ToDateTime(Contratacion.FechaTramite).ToString("yyyy-MM-dd") : Contratacion.FechaTramite.ToString(),
                        Contratacion.NumeroSolicitud,
                        TipoSolicitud = ListTipoSolicitud.Where(r => r.Codigo == ConstanCodigoTipoSolicitud.Contratacion).FirstOrDefault().Nombre,
                        tipoSolicitudNumeroTabla = ConstanCodigoTipoSolicitud.Contratacion
                    });
                };

                foreach (var Actualizacion in ListActualizacionCronograma)
                {
                    ListValidacionSolicitudesContractualesGrilla.Add(new
                    {
                        Id = Actualizacion.ProcesoSeleccionMonitoreoId,
                        FechaSolicitud = Convert.ToDateTime(Actualizacion.FechaCreacion.ToString("yyyy-MM-dd")),
                        NumeroSolicitud = Actualizacion.ProcesoSeleccion.NumeroProceso,
                        TipoSolicitud = ListTipoSolicitud.Where(r => r.Codigo == ConstanCodigoTipoSolicitud.Actualizacion_Cronograma_Proceso_Seleccion).FirstOrDefault().Nombre,
                        tipoSolicitudNumeroTabla = ConstanCodigoTipoSolicitud.Actualizacion_Cronograma_Proceso_Seleccion
                    });
                };

                foreach (var controversiaContractual in ListControversiasContractuales)
                {
                    ListValidacionSolicitudesContractualesGrilla.Add(new
                    {
                        Id = controversiaContractual.ControversiaContractualId,
                        FechaSolicitud = Convert.ToDateTime(controversiaContractual.FechaSolicitud).ToString("yyyy-MM-dd"),
                        NumeroSolicitud = controversiaContractual.NumeroSolicitud,
                        TipoSolicitud = ListTipoSolicitud.Where(r => r.Codigo == ConstanCodigoTipoSolicitud.ControversiasContractuales).FirstOrDefault().Nombre,
                        tipoSolicitudNumeroTabla = ConstanCodigoTipoSolicitud.ControversiasContractuales
                    });
                };

                foreach (var controversiaActuacion in ListControversiasActuaciones)
                {
                    ListValidacionSolicitudesContractualesGrilla.Add(new
                    {
                        Id = controversiaActuacion.ControversiaActuacionId,
                        FechaSolicitud = Convert.ToDateTime(controversiaActuacion.FechaActuacion.Value.ToString("yyyy-MM-dd")),
                        NumeroSolicitud = controversiaActuacion.ControversiaContractual.NumeroSolicitud + " - " + controversiaActuacion.NumeroActuacion,
                        TipoSolicitud = ListTipoSolicitud.Where(r => r.Codigo == ConstanCodigoTipoSolicitud.Actuaciones_Controversias_Contractuales).FirstOrDefault().Nombre,
                        tipoSolicitudNumeroTabla = ConstanCodigoTipoSolicitud.Actuaciones_Controversias_Contractuales
                    });
                };


                foreach (var controversiaActuacion in ListControversiasActuacionReclmacion)
                {
                    ListValidacionSolicitudesContractualesGrilla.Add(new
                    {
                        Id = controversiaActuacion.ControversiaActuacionId,
                        FechaSolicitud = Convert.ToDateTime(controversiaActuacion.FechaActuacion.Value.ToString("yyyy-MM-dd")),
                        NumeroSolicitud = controversiaActuacion.ControversiaContractual.NumeroSolicitud + " - " + controversiaActuacion.NumeroActuacionReclamacion,
                        TipoSolicitud = ListTipoSolicitud.Where(r => r.Codigo == ConstanCodigoTipoSolicitud.Actuaciones_Controversias_Reclamaciones).FirstOrDefault().Nombre,
                        tipoSolicitudNumeroTabla = ConstanCodigoTipoSolicitud.Actuaciones_Controversias_Reclamaciones
                    });
                };


                foreach (var defensa in ListDefensaJudicial)
                {
                    ListValidacionSolicitudesContractualesGrilla.Add(new
                    {
                        Id = defensa.DefensaJudicialId,
                        FechaSolicitud = Convert.ToDateTime(defensa.FechaCreacion.ToString("yyyy-MM-dd")),
                        NumeroSolicitud = defensa.NumeroProceso,
                        TipoSolicitud = ListTipoSolicitud.Where(r => r.Codigo == ConstanCodigoTipoSolicitud.Defensa_judicial).FirstOrDefault().Nombre,
                        tipoSolicitudNumeroTabla = ConstanCodigoTipoSolicitud.Defensa_judicial
                    });
                };

                foreach (var defensa in ListDefensaJudicialSeguimiento)
                {
                    ListValidacionSolicitudesContractualesGrilla.Add(new
                    {
                        Id = defensa.DefensaJudicialSeguimientoId,
                        FechaSolicitud = Convert.ToDateTime(defensa.FechaActuacion.HasValue ? defensa.FechaActuacion.Value.ToString("yyyy-MM-dd") : defensa.FechaCreacion.ToString("yyyy-MM-dd")),
                        NumeroSolicitud = defensa.DefensaJudicial.NumeroProceso,
                        TipoSolicitud = ListTipoSolicitud.Where(r => r.Codigo == ConstanCodigoTipoSolicitud.Actuaciones_Defensa_judicial).FirstOrDefault().Nombre,
                        tipoSolicitudNumeroTabla = ConstanCodigoTipoSolicitud.Actuaciones_Defensa_judicial
                    });
                };

                foreach (var novedad in ListNovedadContractual)
                {
                    ListValidacionSolicitudesContractualesGrilla.Add(new
                    {
                        Id = novedad.NovedadContractualId,
                        FechaSolicitud = Convert.ToDateTime(novedad.FechaSolictud.HasValue ? novedad.FechaSolictud.Value.ToString("yyyy-MM-dd") : novedad.FechaCreacion.Value.ToString("yyyy-MM-dd")),
                        NumeroSolicitud = novedad.NumeroSolicitud,
                        TipoSolicitud = ListTipoSolicitud.Where(r => r.Codigo == ConstanCodigoTipoSolicitud.Novedad_Contractual).FirstOrDefault().Nombre,
                        tipoSolicitudNumeroTabla = ConstanCodigoTipoSolicitud.Novedad_Contractual
                    });
                };

                #endregion Carga las solicitudes

            }
            catch (Exception ex)
            {
            }

            return ListValidacionSolicitudesContractualesGrilla;
        }

        public async Task<Respuesta> CreateEditComiteTecnicoAndSesionComiteTemaAndSesionComiteSolicitud(ComiteTecnico pComiteTecnico)
        {
            int idAccionCrearComiteTecnico = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Comite_Tecnico_SesionComiteSolicitud_SesionComiteTema, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                string strCreateEdit;
                if (pComiteTecnico.ComiteTecnicoId == 0)
                {
                    //Agregar Tema Proposiciones y Varios
                    // pComiteTecnico.SesionComiteTema.Add(
                    //        new SesionComiteTema
                    //        {
                    //            Eliminado = false,
                    //            UsuarioCreacion = pComiteTecnico.UsuarioCreacion,
                    //            FechaCreacion = DateTime.Now,
                    //            EsProposicionesVarios = true,
                    //            Tema = "",

                    //        });

                    strCreateEdit = "CREAR COMITE TECNICO  + SESIÓN COMITE SOLICITUD + SESIÓN COMITE TEMA";
                    //Auditoria
                    pComiteTecnico.FechaCreacion = DateTime.Now;
                    pComiteTecnico.Eliminado = false;
                    pComiteTecnico.EsComiteFiduciario = false;
                    //Registros
                    pComiteTecnico.EsCompleto = ValidarCamposComiteTecnico(pComiteTecnico);

                    pComiteTecnico.EstadoComiteCodigo = ConstanCodigoEstadoComite.Sin_Convocatoria;
                    pComiteTecnico.NumeroComite = await _commonService.EnumeradorComiteTecnico();


                    foreach (var SesionComiteTema in pComiteTecnico.SesionComiteTema)
                    {
                        //Auditoria
                        SesionComiteTema.FechaCreacion = DateTime.Now;
                        SesionComiteTema.UsuarioCreacion = pComiteTecnico.UsuarioCreacion;
                        SesionComiteTema.RegistroCompleto = ValidarRegistroCompletoSesionComiteTema(SesionComiteTema);
                        SesionComiteTema.Eliminado = false;
                    }
                    foreach (var SesionComiteSolicitud in pComiteTecnico.SesionComiteSolicitudComiteTecnico)
                    {
                        //Auditoria
                        SesionComiteSolicitud.FechaCreacion = DateTime.Now;
                        SesionComiteSolicitud.UsuarioCreacion = pComiteTecnico.UsuarioCreacion;
                        SesionComiteSolicitud.RegistroCompleto = ValidarRegistroCompletoSesionComiteSolicitud(SesionComiteSolicitud);
                        SesionComiteSolicitud.Eliminado = false;
                    }
                    _context.ComiteTecnico.Add(pComiteTecnico);
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

                    foreach (var SesionComiteSolicitud in pComiteTecnico.SesionComiteSolicitudComiteTecnico)
                    {
                        if (SesionComiteSolicitud.SesionComiteSolicitudId == 0)
                        {
                            //Auditoria 
                            SesionComiteSolicitud.UsuarioCreacion = pComiteTecnico.UsuarioCreacion;
                            SesionComiteSolicitud.FechaCreacion = DateTime.Now;
                            SesionComiteSolicitud.Eliminado = false;
                            SesionComiteSolicitud.RegistroCompleto = ValidarRegistroCompletoSesionComiteSolicitud(SesionComiteSolicitud);
                            _context.SesionComiteSolicitud.Add(SesionComiteSolicitud);
                        }
                        else
                        {
                            SesionComiteSolicitud SesionComiteSolicitudOld = _context.SesionComiteSolicitud.Find(SesionComiteSolicitud.SesionComiteSolicitudId);
                            //Auditoria 
                            SesionComiteSolicitudOld.UsuarioModificacion = pComiteTecnico.UsuarioCreacion;
                            SesionComiteSolicitudOld.FechaModificacion = DateTime.Now;

                            //Registros
                            SesionComiteSolicitudOld.TipoSolicitudCodigo = SesionComiteSolicitud.TipoSolicitudCodigo;
                            SesionComiteSolicitudOld.SolicitudId = SesionComiteSolicitud.SolicitudId;
                            SesionComiteSolicitudOld.EstadoCodigo = SesionComiteSolicitud.EstadoCodigo;
                            SesionComiteSolicitudOld.Observaciones = SesionComiteSolicitud.Observaciones;
                            SesionComiteSolicitudOld.RutaSoporteVotacion = SesionComiteSolicitud.RutaSoporteVotacion;
                            SesionComiteSolicitudOld.GeneraCompromiso = SesionComiteSolicitud.GeneraCompromiso;
                            SesionComiteSolicitudOld.CantCompromisos = SesionComiteSolicitud.CantCompromisos;
                            SesionComiteSolicitudOld.RegistroCompleto = ValidarRegistroCompletoSesionComiteSolicitud(SesionComiteSolicitudOld);
                        }
                    }
                }
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccionCrearComiteTecnico, pComiteTecnico.UsuarioCreacion, strCreateEdit)
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccionCrearComiteTecnico, pComiteTecnico.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        private bool ValidarRegistroCompletoSesionComiteSolicitud(SesionComiteSolicitud sesionComiteSolicitud, List<SesionSolicitudCompromiso> listaCompromisos = null)
        {
            bool completo = true;
            if (
               (sesionComiteSolicitud.RequiereVotacion == true && string.IsNullOrEmpty(sesionComiteSolicitud.RutaSoporteVotacion)) ||
               sesionComiteSolicitud.GeneraCompromiso == null ||
               sesionComiteSolicitud.RequiereVotacion == null ||
               sesionComiteSolicitud.EstadoCodigo == null ||
               string.IsNullOrEmpty(sesionComiteSolicitud.Observaciones) ||
               string.IsNullOrEmpty(sesionComiteSolicitud.DesarrolloSolicitud) ||
               (sesionComiteSolicitud.GeneraCompromiso == true && sesionComiteSolicitud.CantCompromisos == null)
                )
            {
                completo = false;
            }

            // vienen con el registro
            sesionComiteSolicitud.SesionSolicitudCompromiso.Where(x => x.Eliminado != true).ToList().ForEach(c =>
           {
               if (
                     string.IsNullOrEmpty(c.Tarea) ||
                     c.ResponsableSesionParticipanteId == null ||
                     c.FechaCumplimiento == null
               )
               {
                   completo = false;
               }

           });

            // lista aparte
            listaCompromisos?.Where(x => x.Eliminado != true).ToList().ForEach(c =>
            {
                if (
                      string.IsNullOrEmpty(c.Tarea) ||
                      c.ResponsableSesionParticipanteId == null ||
                      c.FechaCumplimiento == null
                )
                {
                    completo = false;
                }

            });

            if (sesionComiteSolicitud.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion)
            {
                Contratacion contratacion = _context.Contratacion
                                                        .Where(c => c.ContratacionId == sesionComiteSolicitud.SolicitudId)
                                                        .Include(r => r.ContratacionProyecto)
                                                            .ThenInclude(r => r.Proyecto)
                                                        .FirstOrDefault();
                if (sesionComiteSolicitud.Contratacion != null)
                {
                    contratacion = sesionComiteSolicitud.Contratacion;
                }

                contratacion.ContratacionProyecto.ToList().ForEach(cp =>
               {
                   if (contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
                       if (
                               cp.Proyecto.EstadoProyectoObraCodigo != ConstantCodigoEstadoProyecto.AprobadoComiteTecnico &&
                               cp.Proyecto.EstadoProyectoObraCodigo != ConstantCodigoEstadoProyecto.DevueltoComiteTecnico &&
                               cp.Proyecto.EstadoProyectoObraCodigo != ConstantCodigoEstadoProyecto.RechazadoComiteTecnico
                          )
                           completo = false;

                   if (contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString())
                       if (
                               cp.Proyecto.EstadoProyectoInterventoriaCodigo != ConstantCodigoEstadoProyecto.AprobadoComiteTecnico &&
                               cp.Proyecto.EstadoProyectoInterventoriaCodigo != ConstantCodigoEstadoProyecto.DevueltoComiteTecnico &&
                               cp.Proyecto.EstadoProyectoInterventoriaCodigo != ConstantCodigoEstadoProyecto.RechazadoComiteTecnico
                          )
                           completo = false;
               });

            }

            return completo;
        }

        private bool? ValidarRegistroCompletoSesionComiteTemaActa(SesionComiteTema pSesionComiteTema)
        {
            if (
                    string.IsNullOrEmpty(pSesionComiteTema.Observaciones) &&
                    string.IsNullOrEmpty(pSesionComiteTema.ObservacionesDecision) &&
                    string.IsNullOrEmpty(pSesionComiteTema.EstadoTemaCodigo) &&
                    pSesionComiteTema.GeneraCompromiso == null
                )
            {
                return null;
            }
            else
            {
                return ValidarRegistroCompletoSesionComiteTema(pSesionComiteTema);
            }
        }


        private bool? ValidarRegistroCompletoSesionComiteSolicitudActa(SesionComiteSolicitud sesionComiteSolicitud)
        {
            if (
               ((sesionComiteSolicitud.RequiereVotacion == false) || (sesionComiteSolicitud.RequiereVotacion == true && string.IsNullOrEmpty(sesionComiteSolicitud.RutaSoporteVotacion))) &&
               sesionComiteSolicitud.GeneraCompromiso == null &&
               sesionComiteSolicitud.EstadoCodigo == null &&
               string.IsNullOrEmpty(sesionComiteSolicitud.Observaciones) &&
               string.IsNullOrEmpty(sesionComiteSolicitud.DesarrolloSolicitud)
                )
            {
                return null;
            }
            else
            {
                return ValidarRegistroCompletoSesionComiteSolicitud(sesionComiteSolicitud);
            }
        }

        public async Task<Respuesta> CambiarEstadoComiteTecnico(ComiteTecnico pComiteTecnico)
        {
            int idAccionCambiarEstadoSesion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Comite_Sesion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ComiteTecnico ComiteTecnicoOld = _context.ComiteTecnico.Find(pComiteTecnico.ComiteTecnicoId);

                string NombreEstado = await _commonService.GetNombreDominioByCodigoAndTipoDominio(pComiteTecnico.EstadoComiteCodigo, (int)EnumeratorTipoDominio.Estado_Comite);

                ComiteTecnicoOld.UsuarioModificacion = pComiteTecnico.UsuarioCreacion;
                ComiteTecnicoOld.FechaModificacion = DateTime.Now;

                ComiteTecnicoOld.EstadoComiteCodigo = pComiteTecnico.EstadoComiteCodigo;

                if (ComiteTecnicoOld.EstadoComiteCodigo == ConstanCodigoEstadoComite.Desarrollada_Sin_Acta)
                {
                    ComiteTecnicoOld.EstadoActaCodigo = ConstantCodigoActas.Sin_Acta;
                }

                if (ComiteTecnicoOld.EstadoComiteCodigo == ConstanCodigoEstadoComite.Con_Acta_De_Sesion_Enviada)
                {
                    ComiteTecnicoOld.EstadoActaCodigo = ConstantCodigoActas.En_proceso_Aprobacion;
                }

                if (ComiteTecnicoOld.EstadoComiteCodigo == ConstanCodigoEstadoComite.Fallida)
                {
                    List<SesionComiteSolicitud> listaSolicitudes = _context.SesionComiteSolicitud.Where(cs => cs.ComiteTecnicoId == ComiteTecnicoOld.ComiteTecnicoId).ToList();

                    if (listaSolicitudes != null)
                        listaSolicitudes.ForEach(s =>
                        {
                            s.EstadoCodigo = ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_tecnico;
                        });
                }

                _context.SaveChanges();
                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = ConstantSesionComiteTecnico.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccionCambiarEstadoSesion, pComiteTecnico.UsuarioCreacion, "ESTADO COMITE CAMBIADO A " + NombreEstado.ToUpper())
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccionCambiarEstadoSesion, pComiteTecnico.UsuarioCreacion, ex.InnerException.ToString())
                };
            }

        }

        public async Task<ComiteTecnico> GetComiteTecnicoByComiteTecnicoId(int pComiteTecnicoId)
        {
            if (pComiteTecnicoId == 0)
            {
                return new ComiteTecnico();
            }

            List<Dominio> listaResponsables = _context.Dominio.Where(r => r.TipoDominioId == 46).ToList();

            #region query

            ComiteTecnico comiteTecnico = await _context.ComiteTecnico
                 .Where(r => r.ComiteTecnicoId == pComiteTecnicoId)
                    .Include(r => r.SesionInvitado)
                    .Include(r => r.SesionComiteSolicitudComiteTecnico)
                        .ThenInclude(r => r.SesionSolicitudVoto)
                    .Include(r => r.SesionComiteSolicitudComiteTecnico)
                        .ThenInclude(r => r.SesionSolicitudCompromiso)
                    .Include(r => r.SesionComiteTema)
                    //  .ThenInclude(r => r.SesionTemaVoto)
                    .Include(r => r.SesionComiteTema)
                       .ThenInclude(r => r.TemaCompromiso)
                 .FirstOrDefaultAsync();

            List<VSesionParticipante> listaParticipantes = _context.VSesionParticipante.Where(r => r.ComiteTecnicoId == comiteTecnico.ComiteTecnicoId).ToList();

            comiteTecnico.SesionComiteTema = comiteTecnico.SesionComiteTema.Where(r => r.Eliminado != true).ToList();

            comiteTecnico.SesionInvitado = comiteTecnico.SesionInvitado.Where(r => r.Eliminado != true).ToList();

            #endregion query

            #region tema

            comiteTecnico.SesionComiteTema.ToList().ForEach(ct =>
            {
                Dominio responsable = listaResponsables.Find(lr => lr.Codigo == ct.ResponsableCodigo);

                if (responsable != null)
                    ct.NombreResponsable = responsable.Nombre;

                ct.TemaCompromiso = ct.TemaCompromiso.Where(r => !(bool)r.Eliminado).ToList();

                ct.RegistroCompletoActa = ValidarRegistroCompletoSesionComiteTemaActa(ct);

                ct.SesionTemaVoto = _context.SesionTemaVoto.Where(r => r.SesionTemaId == ct.SesionTemaId && r.Eliminado != true).ToList();

            });

            #endregion tema

            #region participantes

            foreach (var SesionComiteSolicitud in comiteTecnico.SesionComiteSolicitudComiteTecnico)
            {
                SesionComiteSolicitud.RegistroCompletoActa = ValidarRegistroCompletoSesionComiteSolicitudActa(SesionComiteSolicitud);

                SesionComiteSolicitud.SesionSolicitudVoto = SesionComiteSolicitud.SesionSolicitudVoto.Where(r => !(bool)r.Eliminado).ToList();
                SesionComiteSolicitud.SesionSolicitudCompromiso = SesionComiteSolicitud.SesionSolicitudCompromiso.Where(r => !(bool)r.Eliminado).ToList();

                SesionComiteSolicitud.SesionSolicitudCompromiso.ToList().ForEach(ssc =>
               {

                   SesionParticipante participante = new SesionParticipante();
                   participante.Usuario = new Usuario();

                   VSesionParticipante vSesionParticipante = listaParticipantes.Where(r => r.SesionParticipanteId == ssc.ResponsableSesionParticipanteId).FirstOrDefault();

                   if (vSesionParticipante != null)
                   {
                       participante.SesionParticipanteId = vSesionParticipante.SesionParticipanteId;
                       participante.ComiteTecnicoId = vSesionParticipante.ComiteTecnicoId;
                       participante.UsuarioId = vSesionParticipante.UsuarioId;
                       participante.Eliminado = vSesionParticipante.Eliminado;

                       participante.Usuario.UsuarioId = vSesionParticipante.UsuarioId;
                       participante.Usuario.PrimerNombre = vSesionParticipante.Nombres;
                       participante.Usuario.PrimerApellido = vSesionParticipante.Apellidos;
                       participante.Usuario.NumeroIdentificacion = vSesionParticipante.NumeroIdentificacion;

                       ssc.ResponsableSesionParticipante = participante;
                   }
               });
            }

            #endregion participantes

            #region voto

            List<SesionSolicitudVoto> ListSesionSolicitudVotos = _context.SesionSolicitudVoto.Where(r => !(bool)r.Eliminado).Include(r => r.SesionParticipante).ToList();

            foreach (var SesionComiteSolicitud in comiteTecnico.SesionComiteSolicitudComiteTecnico)
            {
                SesionComiteSolicitud.SesionSolicitudVoto = ListSesionSolicitudVotos.Where(r => r.SesionComiteSolicitudId == SesionComiteSolicitud.SesionComiteSolicitudId).ToList();
            }

            #endregion voto

            #region filtroProcesoSeleccion

            List<Dominio> TipoComiteSolicitud = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud).ToList();

            List<ProcesoSeleccion> ListProcesoSeleccion =
                _context.ProcesoSeleccion
                .Where(r => !(bool)r.Eliminado).ToList();

            #endregion filtroProcesoSeleccion

            #region solicitudes

            List<Contratacion> ListContratacion = _context.Contratacion.ToList();

            foreach (SesionComiteSolicitud sesionComiteSolicitud in comiteTecnico.SesionComiteSolicitudComiteTecnico)
            {

                switch (sesionComiteSolicitud.TipoSolicitudCodigo)
                {
                    #region contratacion 
                    case ConstanCodigoTipoSolicitud.Contratacion:

                        Contratacion contratacion = ListContratacion.Where(r => r.ContratacionId == sesionComiteSolicitud.SolicitudId).FirstOrDefault();

                        sesionComiteSolicitud.FechaSolicitud = contratacion.FechaCreacion;

                        sesionComiteSolicitud.NumeroSolicitud = contratacion.NumeroSolicitud;

                        sesionComiteSolicitud.Contratacion = contratacion;

                        break;

                    #endregion contratacion

                    #region Evaluacion Proceso - Inicio Proceso Seleccion

                    case ConstanCodigoTipoSolicitud.Evaluacion_De_Proceso:
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

                    #endregion Evaluacion Proceso - Inicio Proceso Seleccion

                    #region actualizacion cronograma proceso seleccion

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

                    #endregion actualizacion cronograma proceso seleccion

                    #region Controversias Contractuales

                    case ConstanCodigoTipoSolicitud.ControversiasContractuales:

                        ControversiaContractual controversiaContractual = _context.ControversiaContractual
                                                                                .Where(r => r.ControversiaContractualId == sesionComiteSolicitud.SolicitudId)
                                                                                .FirstOrDefault();

                        sesionComiteSolicitud.FechaSolicitud = controversiaContractual.FechaSolicitud;

                        sesionComiteSolicitud.NumeroSolicitud = controversiaContractual.NumeroSolicitud;

                        break;

                    #endregion Controversias Contractuales

                    #region Actuaciones Controversias Contractuales

                    case ConstanCodigoTipoSolicitud.Actuaciones_Controversias_Contractuales:

                        ControversiaActuacion controversiaActuacion = _context.ControversiaActuacion
                                                                                .Where(r => r.ControversiaActuacionId == sesionComiteSolicitud.SolicitudId)
                                                                                .Include(r => r.ControversiaContractual)
                                                                                .FirstOrDefault();

                        sesionComiteSolicitud.FechaSolicitud = controversiaActuacion.FechaActuacion;

                        sesionComiteSolicitud.NumeroSolicitud = controversiaActuacion.ControversiaContractual.NumeroSolicitud;

                        sesionComiteSolicitud.NumeroHijo = "ACT controversia " + controversiaActuacion.ControversiaActuacionId.ToString("000");

                        break;

                    #endregion Actuaciones Controversias Contractuales

                    #region Actuaciones Controversias Reclamaciones

                    case ConstanCodigoTipoSolicitud.Actuaciones_Controversias_Reclamaciones:

                        ControversiaActuacion controversiaActuacionReclamacion = _context.ControversiaActuacion
                                                                                .Where(r => r.ControversiaActuacionId == sesionComiteSolicitud.SolicitudId)
                                                                                .Include(r => r.ControversiaContractual)
                                                                                .FirstOrDefault();

                        sesionComiteSolicitud.FechaSolicitud = controversiaActuacionReclamacion.FechaActuacion;

                        sesionComiteSolicitud.NumeroSolicitud = controversiaActuacionReclamacion.ControversiaContractual.NumeroSolicitud;

                        sesionComiteSolicitud.NumeroHijo = "ACT controversia " + controversiaActuacionReclamacion.NumeroActuacionReclamacion;

                        break;

                    #endregion Actuaciones Controversias Reclamaciones

                    #region Defensa judicial

                    case ConstanCodigoTipoSolicitud.Defensa_judicial:

                        DefensaJudicial defensaJudicial = _context.DefensaJudicial
                                                                                .Where(r => r.DefensaJudicialId == sesionComiteSolicitud.SolicitudId)
                                                                                //.Include(r => r.ControversiaContractual)
                                                                                .FirstOrDefault();

                        sesionComiteSolicitud.FechaSolicitud = defensaJudicial.FechaCreacion;

                        sesionComiteSolicitud.NumeroSolicitud = defensaJudicial.NumeroProceso;

                        break;

                    #endregion Defensa judicial

                    #region Defensa Judicial Seguimiento 

                    case ConstanCodigoTipoSolicitud.Actuaciones_Defensa_judicial:

                        DefensaJudicialSeguimiento defensaJudicialSeguimiento = _context.DefensaJudicialSeguimiento
                                                                                .Where(r => r.DefensaJudicialSeguimientoId == sesionComiteSolicitud.SolicitudId)
                                                                                .Include(r => r.DefensaJudicial)
                                                                                .FirstOrDefault();

                        sesionComiteSolicitud.FechaSolicitud = defensaJudicialSeguimiento.FechaActuacion;

                        sesionComiteSolicitud.NumeroSolicitud = defensaJudicialSeguimiento.DefensaJudicial.NumeroProceso;

                        //sesionComiteSolicitud.NumeroHijo = "ACT controversia " + controversiaActuacionReclamacion.NumeroActuacionReclamacion;
                        sesionComiteSolicitud.NumeroHijo = defensaJudicialSeguimiento.DefensaJudicialSeguimientoId.ToString();

                        break;

                    #endregion Defensa Judicial Seguimiento 

                    #region Novedad Contractual

                    case ConstanCodigoTipoSolicitud.Novedad_Contractual:

                        NovedadContractual novedadContractual = _context.NovedadContractual.Find(sesionComiteSolicitud.SolicitudId);

                        sesionComiteSolicitud.FechaSolicitud = novedadContractual.FechaSolictud;

                        sesionComiteSolicitud.NumeroSolicitud = novedadContractual.NumeroSolicitud;

                        break;

                        #endregion Novedad Contractual

                }

                sesionComiteSolicitud.TipoSolicitud = TipoComiteSolicitud.Where(r => r.Codigo == sesionComiteSolicitud.TipoSolicitudCodigo).FirstOrDefault().Nombre;
            }

            #endregion solicitudes

            return comiteTecnico;
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccionCrearSesionParticipante, pComiteTecnico.UsuarioCreacion, "REGISTRAR PARTICIPANTES SESIÓN")
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccionCrearSesionParticipante, pComiteTecnico.UsuarioCreacion, ex.InnerException.ToString())
                };
            }

        }

        public async Task<Respuesta> EliminarSesionComiteTema(int pSesionComiteTemaId, string pUsuarioModificacion)
        {
            int idAccionEliminarSesionComiteTema = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Sesion_Comite_Tema, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                SesionComiteTema sesionComiteTemaOld = await _context.SesionComiteTema.Where(r => r.SesionTemaId == pSesionComiteTemaId).FirstOrDefaultAsync();
                if (sesionComiteTemaOld == null)
                {
                    return
                    new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = true,
                        IsValidation = false,
                        Code = ConstantSesionComiteTecnico.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccionEliminarSesionComiteTema, pUsuarioModificacion, "NO SE ENCONTRO REGISTRO")
                    };
                }

                sesionComiteTemaOld.Eliminado = true;
                sesionComiteTemaOld.FechaModificacion = DateTime.Now;
                sesionComiteTemaOld.UsuarioCreacion = pUsuarioModificacion;

                _context.SaveChanges();

                return
                 new Respuesta
                 {
                     IsSuccessful = true,
                     IsException = false,
                     IsValidation = false,
                     Code = ConstantSesionComiteTecnico.OperacionExitosa,
                     Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccionEliminarSesionComiteTema, pUsuarioModificacion, "ELIMINAR SESIÓN COMITE TEMA")
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccionEliminarSesionComiteTema, pUsuarioModificacion, ex.InnerException.ToString().Substring(0, 500))
                    };
            }

        }

        public async Task<Respuesta> EliminarCompromisoSolicitud(int pCompromisoId, string pUsuarioModificacion)
        {
            int idAccionEliminarSesionComiteTema = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Compromiso_Solicitud, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                if (pCompromisoId == 0)
                {
                    return
                 new Respuesta
                 {
                     IsSuccessful = true,
                     IsException = false,
                     IsValidation = false,
                     Code = ConstantSesionComiteTecnico.OperacionExitosa,
                     //Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccionEliminarSesionComiteTema, pUsuarioModificacion, "ELIMINAR SESIÓN COMITE COMPROMISO")
                 };
                }

                SesionSolicitudCompromiso sesionSolicitudCompromiso = await _context.SesionSolicitudCompromiso
                                                                                        .Where(r => r.SesionSolicitudCompromisoId == pCompromisoId)
                                                                                        .FirstOrDefaultAsync();

                if (sesionSolicitudCompromiso == null)
                {
                    return
                    new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = true,
                        IsValidation = false,
                        Code = ConstantSesionComiteTecnico.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccionEliminarSesionComiteTema, pUsuarioModificacion, "NO SE ENCONTRO REGISTRO")
                    };
                }

                sesionSolicitudCompromiso.Eliminado = true;
                sesionSolicitudCompromiso.FechaModificacion = DateTime.Now;
                sesionSolicitudCompromiso.UsuarioCreacion = pUsuarioModificacion;

                SesionComiteSolicitud sesionComiteSolicitud = _context.SesionComiteSolicitud
                                                                            .Where(r => r.SesionComiteSolicitudId == sesionSolicitudCompromiso.SesionComiteSolicitudId)
                                                                            .Include(r => r.SesionSolicitudCompromiso)
                                                                            .FirstOrDefault();

                sesionComiteSolicitud.CantCompromisos = sesionComiteSolicitud.SesionSolicitudCompromiso
                                                                                .Where(r => r.Eliminado != true && r.EsFiduciario != true).Count();

                sesionComiteSolicitud.CantCompromisosFiduciario = sesionComiteSolicitud.SesionSolicitudCompromiso
                                                                                            .Where(r => r.Eliminado != true && r.EsFiduciario == true).Count();

                _context.SaveChanges();

                return
                 new Respuesta
                 {
                     IsSuccessful = true,
                     IsException = false,
                     IsValidation = false,
                     Code = ConstantSesionComiteTecnico.OperacionExitosa,
                     Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccionEliminarSesionComiteTema, pUsuarioModificacion, "ELIMINAR SESIÓN COMITE COMPROMISO")
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccionEliminarSesionComiteTema, pUsuarioModificacion, ex.InnerException.ToString().Substring(0, 500))
                    };
            }

        }

        public async Task<Respuesta> EliminarCompromisoTema(int pCompromisoTemaId, string pUsuarioModificacion)
        {
            int idAccionEliminarSesionComiteTema = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Compromiso_Tema, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                if (pCompromisoTemaId == 0)
                {
                    return
                 new Respuesta
                 {
                     IsSuccessful = true,
                     IsException = false,
                     IsValidation = false,
                     Code = ConstantSesionComiteTecnico.OperacionExitosa,
                     //Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccionEliminarSesionComiteTema, pUsuarioModificacion, "ELIMINAR SESIÓN COMITE COMPROMISO")
                 };
                }

                TemaCompromiso temaCompromiso = await _context.TemaCompromiso
                                                                    .Where(r => r.TemaCompromisoId == pCompromisoTemaId)
                                                                    .FirstOrDefaultAsync();

                if (temaCompromiso == null)
                {
                    return
                    new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = true,
                        IsValidation = false,
                        Code = ConstantSesionComiteTecnico.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccionEliminarSesionComiteTema, pUsuarioModificacion, "NO SE ENCONTRO REGISTRO")
                    };
                }

                temaCompromiso.Eliminado = true;
                temaCompromiso.FechaModificacion = DateTime.Now;
                temaCompromiso.UsuarioCreacion = pUsuarioModificacion;

                SesionComiteTema sesionComiteTema = _context.SesionComiteTema
                                                                            .Where(r => r.SesionTemaId == temaCompromiso.SesionTemaId)
                                                                            .Include(r => r.TemaCompromiso)
                                                                            .FirstOrDefault();

                sesionComiteTema.CantCompromisos = sesionComiteTema.TemaCompromiso
                                                                                .Where(r => r.Eliminado != true).Count();


                _context.SaveChanges();

                return
                 new Respuesta
                 {
                     IsSuccessful = true,
                     IsException = false,
                     IsValidation = false,
                     Code = ConstantSesionComiteTecnico.OperacionExitosa,
                     Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccionEliminarSesionComiteTema, pUsuarioModificacion, "ELIMINAR SESIÓN COMITE TEMA")
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccionEliminarSesionComiteTema, pUsuarioModificacion, ex.InnerException.ToString().Substring(0, 500))
                    };
            }

        }

        public static bool ValidarCamposSesionComiteTema(SesionComiteTema pSesionComiteTema)
        {
            if (
                !string.IsNullOrEmpty(pSesionComiteTema.ResponsableCodigo) ||
                !string.IsNullOrEmpty(pSesionComiteTema.TiempoIntervencion.ToString()) ||
                !string.IsNullOrEmpty(pSesionComiteTema.RutaSoporte) ||
                !string.IsNullOrEmpty(pSesionComiteTema.Observaciones) ||
                !string.IsNullOrEmpty(pSesionComiteTema.EsAprobado.ToString()) ||
                !string.IsNullOrEmpty(pSesionComiteTema.ObservacionesDecision) ||
                !string.IsNullOrEmpty(pSesionComiteTema.ObservacionesDecision)
                ) { return false; }

            return true;
        }

        public async Task<List<dynamic>> GetListSesionComiteTemaByComiteTecnicoId(int pComiteTecnicoId)
        {
            var ListSesionComiteTema = await _context.SesionComiteTema.Where(r => r.ComiteTecnicoId == pComiteTecnicoId && !(bool)r.Eliminado).ToListAsync();

            List<dynamic> ListSesionComiteTemaDyn = new List<dynamic>();

            foreach (var sesionComiteTema in ListSesionComiteTema)
            {
                ListSesionComiteTemaDyn.Add(
                                            new
                                            {
                                                sesionComiteTema.SesionTemaId,
                                                sesionComiteTema.ResponsableCodigo,
                                                sesionComiteTema.TiempoIntervencion,
                                                sesionComiteTema.Tema
                                            });
            }
            return ListSesionComiteTemaDyn;
        }

        private int numeroCompromisos(int idComiteTecnico, bool esCumplido)
        {
            ComiteTecnico comiteTecnico = _context.ComiteTecnico.Where(ct => ct.ComiteTecnicoId == idComiteTecnico)
                                                                .Include(r => r.SesionComiteSolicitudComiteTecnico)
                                                                    .ThenInclude(r => r.SesionSolicitudCompromiso)
                                                                .Include(r => r.SesionComiteTema)
                                                                    .ThenInclude(r => r.TemaCompromiso)
                                                                .FirstOrDefault();

            comiteTecnico.NumeroCompromisos = 0;
            comiteTecnico.NumeroCompromisosCumplidos = 0;
            foreach (var SesionComiteTema in comiteTecnico.SesionComiteTema)
            {
                foreach (var TemaCompromiso in SesionComiteTema.TemaCompromiso)
                {
                    if (TemaCompromiso.EstadoCodigo == ConstantCodigoCompromisos.Finalizado)
                        comiteTecnico.NumeroCompromisosCumplidos++;
                    comiteTecnico.NumeroCompromisos++;
                }
            }

            foreach (var SesionComiteSolicitud in comiteTecnico.SesionComiteSolicitudComiteTecnico)
            {
                foreach (var SesionSolicitudCompromiso in SesionComiteSolicitud.SesionSolicitudCompromiso)
                {
                    if (SesionSolicitudCompromiso.EstadoCodigo == ConstantCodigoCompromisos.Finalizado)
                        comiteTecnico.NumeroCompromisosCumplidos++;
                    comiteTecnico.NumeroCompromisos++;
                }
            }

            if (esCumplido)
                return comiteTecnico.NumeroCompromisosCumplidos;
            else
                return comiteTecnico.NumeroCompromisos;

        }

        public async Task<List<ComiteGrilla>> GetListComiteGrilla()
        {
            List<Dominio> ListaEstadoComite = await _context.Dominio
                .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Comite && (bool)r.Activo)
                .ToListAsync();

            List<ComiteGrilla> ListComiteGrilla = new List<ComiteGrilla>();
            List<Dominio> ListaEstadoActa = await _context.Dominio
          .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estados_Acta && (bool)r.Activo)
          .ToListAsync();
            try
            {
                var ListComiteTecnico = await _context.ComiteTecnico.Where(r => !(bool)r.Eliminado && !(bool)r.EsComiteFiduciario)
                                                                    .Include(r => r.SesionComiteTecnicoCompromiso)
                                                                        .Include(r => r.SesionComiteSolicitudComiteTecnico)
                                                                           .ThenInclude(r => r.SesionSolicitudCompromiso)
                                                                               .ThenInclude(r => r.CompromisoSeguimiento)
                                                                    .Distinct().OrderByDescending(r => r.ComiteTecnicoId).ToListAsync();

                foreach (var comite in ListComiteTecnico)
                {
                    string EstadoComite = "";
                    if (!string.IsNullOrEmpty(comite.EstadoComiteCodigo))
                        EstadoComite = ListaEstadoComite.Where(r => r.Codigo == comite.EstadoComiteCodigo).FirstOrDefault().Nombre;

                    ComiteGrilla comiteGrilla = new ComiteGrilla();

                    comiteGrilla.Id = comite.ComiteTecnicoId;
                    comiteGrilla.FechaComite = comite.FechaOrdenDia.Value;
                    comiteGrilla.EstadoComiteCodigo = !string.IsNullOrEmpty(comite.EstadoComiteCodigo) ? comite.EstadoComiteCodigo : "";
                    comiteGrilla.EstadoComite = !string.IsNullOrEmpty(EstadoComite) ? EstadoComite : "";
                    comiteGrilla.NumeroComite = comite.NumeroComite;
                    comiteGrilla.EstadoActa = !string.IsNullOrEmpty(comite.EstadoActaCodigo) ? ListaEstadoActa.Where(r => r.Codigo == comite.EstadoActaCodigo).FirstOrDefault().Nombre : "";
                    comiteGrilla.EstadoActaCodigo = !string.IsNullOrEmpty(comite.EstadoActaCodigo) ? comite.EstadoActaCodigo : "";
                    comiteGrilla.RegistroCompletoNombre = (bool)comite.EsCompleto ? "Completo" : "Incompleto";
                    comiteGrilla.RegistroCompleto = comite.EsCompleto;

                    comiteGrilla.EsComiteFiduciario = comite.EsComiteFiduciario;

                    ListComiteGrilla.Add(comiteGrilla);
                }
            }
            catch (Exception ex)
            {
                string error = "";
                error = ex.InnerException.ToString();
            }

            return ListComiteGrilla;
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

        #endregion

        #region Actas
        public async Task<Respuesta> CreateEditSesionSolicitudObservacionProyecto(SesionSolicitudObservacionProyecto pSesionSolicitudObservacionProyecto)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Edit_Sesion_Observacion_Proyecto, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = string.Empty;
            try
            {
                pSesionSolicitudObservacionProyecto.FechaCreacion = DateTime.Now;
                pSesionSolicitudObservacionProyecto.Eliminado = false;
                _context.SesionSolicitudObservacionProyecto.Add(pSesionSolicitudObservacionProyecto);

                _context.SaveChanges();
                return
                   new Respuesta
                   {
                       IsSuccessful = true,
                       IsException = false,
                       IsValidation = false,
                       Code = ConstantSesionComiteTecnico.OperacionExitosa,
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccion, pSesionSolicitudObservacionProyecto.UsuarioCreacion, CreateEdit)
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
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccion, pSesionSolicitudObservacionProyecto.UsuarioCreacion, ex.InnerException.ToString())
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
                SesionComiteTemadOld.RegistroCompleto = ValidarRegistroCompletoSesionComiteTema(SesionComiteTemadOld, pSesionComiteTema.TemaCompromiso.ToList());

                foreach (var TemaCompromiso in pSesionComiteTema.TemaCompromiso)
                {
                    if (TemaCompromiso.TemaCompromisoId == 0)
                    {
                        CreateEdit = "CREAR TEMA COMPROMISO";
                        TemaCompromiso.UsuarioCreacion = pSesionComiteTema.UsuarioCreacion;
                        TemaCompromiso.FechaCreacion = DateTime.Now;
                        TemaCompromiso.Eliminado = false;

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
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccion, pSesionComiteTema.UsuarioCreacion, CreateEdit)
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
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccion, pSesionComiteTema.UsuarioCreacion, ex.InnerException.ToString())
                   };
            }
        }

        private bool validarcompletosActa(int pComiteTecnicoId)
        {
            bool estaCompleto = true;

            ComiteTecnico comite = _context.ComiteTecnico.Where(ct => ct.ComiteTecnicoId == pComiteTecnicoId)
                                                         .Include(r => r.SesionComiteSolicitudComiteTecnico)
                                                         .Include(r => r.SesionComiteTema)
                                                        .FirstOrDefault();

            comite.SesionComiteSolicitudComiteTecnico.Where(t => t.Eliminado != true).ToList().ForEach(cs =>
          {
              if ((cs.RegistroCompleto.HasValue ? cs.RegistroCompleto.Value : false) == false)
                  estaCompleto = false;
          });

            comite.SesionComiteTema.Where(t => t.Eliminado != true).ToList().ForEach(ct =>
           {
               if ((ct.RegistroCompleto.HasValue ? ct.RegistroCompleto.Value : false) == false)
                   estaCompleto = false;
           });

            if (estaCompleto)
            {
                //comite.EstadoComiteCodigo = ConstanCodigoEstadoComite.Con_Acta_De_Sesion_Enviada;
                comite.EstadoComiteCodigo = ConstanCodigoEstadoComite.Desarrollada_Sin_Acta;
                comite.EsCompleto = true;
                _context.SaveChanges();
            }
            else
            {
                comite.EsCompleto = false;
                _context.SaveChanges();
            }

            return estaCompleto;
        }

        public void CambiarEstadoSolicitudes(int SolicitudId, string TipoSolicitud, string EstadoCodigo)
        {
            #region Contratacion

            if (TipoSolicitud == ConstanCodigoTipoSolicitud.Contratacion)
            {
                Contratacion contratacion = _context.Contratacion.Find(SolicitudId);

                if (contratacion != null)
                {
                    if (EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Aprobada_por_comite_tecnico)
                    {
                        contratacion.EstadoSolicitudCodigo = ConstanCodigoEstadoSolicitudContratacion.AprobadoComiteTecnico;
                    }
                    if (EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_tecnico)
                    {
                        contratacion.EstadoSolicitudCodigo = ConstanCodigoEstadoSolicitudContratacion.DevueltoComiteTecnico;
                    }
                    if (EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Rechazada_por_comite_tecnico)
                    {
                        contratacion.EstadoSolicitudCodigo = ConstanCodigoEstadoSolicitudContratacion.RechazadoComiteTecnico;
                    }
                }
            }

            #endregion Contratacion

            #region  Inicio Proceso Seleccion   

            if (TipoSolicitud == ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion ||
                TipoSolicitud == ConstanCodigoTipoSolicitud.Evaluacion_De_Proceso)
            {
                ProcesoSeleccion procesoSeleccion = _context.ProcesoSeleccion.Find(SolicitudId);
                if (procesoSeleccion != null)
                {
                    if (procesoSeleccion.EstadoProcesoSeleccionCodigo == ConstanCodigoEstadoProcesoSeleccion.Apertura_En_Tramite)
                    {
                        switch (EstadoCodigo)
                        {
                            case ConstanCodigoEstadoSesionComiteSolicitud.Aprobada_por_comite_tecnico:
                                procesoSeleccion.EstadoProcesoSeleccionCodigo = ConstanCodigoEstadoProcesoSeleccion.AprobadaAperturaPorComiteTecnico;
                                break;
                            case ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_tecnico:
                                procesoSeleccion.EstadoProcesoSeleccionCodigo = ConstanCodigoEstadoProcesoSeleccion.DevueltaAperturaPorComiteTecnico;
                                break;
                            case ConstanCodigoEstadoSesionComiteSolicitud.Rechazada_por_comite_tecnico:
                                procesoSeleccion.EstadoProcesoSeleccionCodigo = ConstanCodigoEstadoProcesoSeleccion.RechazadaAperturaPorComiteTecnico;
                                break;
                        }
                    }
                    else if (procesoSeleccion.EstadoProcesoSeleccionCodigo == ConstanCodigoEstadoProcesoSeleccion.AprobacionDeSeleccionEnTramite)
                    {
                        switch (EstadoCodigo)
                        {
                            case ConstanCodigoEstadoSesionComiteSolicitud.Aprobada_por_comite_tecnico:
                                procesoSeleccion.EstadoProcesoSeleccionCodigo = ConstanCodigoEstadoProcesoSeleccion.AprobadaSelecciónPorComiteTecnico;
                                break;
                            case ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_tecnico:
                                procesoSeleccion.EstadoProcesoSeleccionCodigo = ConstanCodigoEstadoProcesoSeleccion.DevueltaSeleccionPorComiteTecnico;
                                break;
                            case ConstanCodigoEstadoSesionComiteSolicitud.Rechazada_por_comite_tecnico:
                                procesoSeleccion.EstadoProcesoSeleccionCodigo = ConstanCodigoEstadoProcesoSeleccion.RechazadaSeleccionPorComiteTecnico;
                                break;
                        }
                    }
                    else if (procesoSeleccion.EstadoProcesoSeleccionCodigo == ConstanCodigoEstadoProcesoSeleccion.EnProcesoDeSeleccion)
                    {
                        switch (EstadoCodigo)
                        {
                            case ConstanCodigoEstadoSesionComiteSolicitud.Aprobada_por_comite_tecnico:
                                procesoSeleccion.EstadoProcesoSeleccionCodigo = ConstanCodigoEstadoProcesoSeleccion.AprobadoPorComiteTecnico;
                                break;
                            case ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_tecnico:
                                procesoSeleccion.EstadoProcesoSeleccionCodigo = ConstanCodigoEstadoProcesoSeleccion.DevueltoPorComiteTecnico;
                                break;
                            case ConstanCodigoEstadoSesionComiteSolicitud.Rechazada_por_comite_tecnico:
                                procesoSeleccion.EstadoProcesoSeleccionCodigo = ConstanCodigoEstadoProcesoSeleccion.RechazadoPorComiteTecnico;
                                break;
                        }
                    }


                }

            }

            #endregion Inicio Proceso Seleccion

            #region Actualizacion Cronograma

            if (TipoSolicitud == ConstanCodigoTipoSolicitud.Actualizacion_Cronograma_Proceso_Seleccion)
            {
                ProcesoSeleccionMonitoreo procesoSeleccionMonitoreo = _context.ProcesoSeleccionMonitoreo.Find(SolicitudId);

                if (procesoSeleccionMonitoreo != null)
                {
                    if (EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Aprobada_por_comite_tecnico)
                    {
                        procesoSeleccionMonitoreo.EstadoActividadCodigo = ConstanCodigoEstadoActividadCronogramaProcesoSeleccion.AprobadoPorComiteTecnico;
                    }
                    if (EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Rechazada_por_comite_tecnico)
                    {
                        procesoSeleccionMonitoreo.EstadoActividadCodigo = ConstanCodigoEstadoActividadCronogramaProcesoSeleccion.RechazadoPorComiteTecnico;
                        procesoSeleccionMonitoreo.EnviadoComiteTecnico = false;
                    }
                    if (EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_tecnico)
                    {
                        procesoSeleccionMonitoreo.EstadoActividadCodigo = ConstanCodigoEstadoActividadCronogramaProcesoSeleccion.DevueltoPorComiteTecnico;
                        procesoSeleccionMonitoreo.EnviadoComiteTecnico = false;
                    }

                }

            }

            #endregion

            #region Controversia Contractual

            if (TipoSolicitud == ConstanCodigoTipoSolicitud.ControversiasContractuales)
            {
                ControversiaContractual controversiaContractual = _context.ControversiaContractual.Find(SolicitudId);

                if (controversiaContractual != null)
                {
                    if (EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Aprobada_por_comite_tecnico)
                    {
                        controversiaContractual.EstadoCodigo = ConstanCodigoEstadoControversiasContractuales.AprobadaPorComiteTecnico;
                    }
                    if (EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Rechazada_por_comite_tecnico)
                    {
                        controversiaContractual.EstadoCodigo = ConstanCodigoEstadoControversiasContractuales.RechazadaPorComiteTecnico;
                    }
                    if (EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_tecnico)
                    {
                        controversiaContractual.EstadoCodigo = ConstanCodigoEstadoControversiasContractuales.DevueltaPorComiteTecnico;
                    }

                }

            }

            #endregion

            #region Actuaciones Controversia Contractual

            if (TipoSolicitud == ConstanCodigoTipoSolicitud.Actuaciones_Controversias_Contractuales)
            {
                ControversiaActuacion controversiaActuacion = _context.ControversiaActuacion.Find(SolicitudId);

                if (controversiaActuacion != null)
                {
                    if (EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Aprobada_por_comite_tecnico)
                    {
                        controversiaActuacion.EstadoCodigo = ConstanCodigoEstadoControversiasContractuales.AprobadaPorComiteTecnico;
                    }
                    if (EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Rechazada_por_comite_tecnico)
                    {
                        controversiaActuacion.EstadoCodigo = ConstanCodigoEstadoControversiasContractuales.RechazadaPorComiteTecnico;
                    }
                    if (EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_tecnico)
                    {
                        controversiaActuacion.EstadoCodigo = ConstanCodigoEstadoControversiasContractuales.DevueltaPorComiteTecnico;
                    }

                }

            }

            #endregion

            #region Actuaciones Controversia Reclamación

            if (TipoSolicitud == ConstanCodigoTipoSolicitud.Actuaciones_Controversias_Reclamaciones)
            {
                ControversiaActuacion controversiaActuacion = _context.ControversiaActuacion.Find(SolicitudId);

                if (controversiaActuacion != null)
                {
                    if (EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Aprobada_por_comite_tecnico)
                    {
                        controversiaActuacion.EstadoActuacionReclamacionCodigo = ConstanCodigoEstadosActuacionReclamacion.Aprobada_por_comite_tecnico;
                    }
                    if (EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Rechazada_por_comite_tecnico)
                    {
                        controversiaActuacion.EstadoActuacionReclamacionCodigo = ConstanCodigoEstadosActuacionReclamacion.Rechazado_por_comite_tecnico;
                    }
                    if (EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_tecnico)
                    {
                        controversiaActuacion.EstadoActuacionReclamacionCodigo = ConstanCodigoEstadosActuacionReclamacion.Devuelta_por_comite_tecnico;
                    }

                }

            }

            #endregion Actuaciones Controversia Reclamación

            #region Defensa Judicial

            if (TipoSolicitud == ConstanCodigoTipoSolicitud.Defensa_judicial)
            {
                DefensaJudicial defensaJudicial = _context.DefensaJudicial.Find(SolicitudId);

                if (defensaJudicial != null)
                {
                    if (EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Aprobada_por_comite_tecnico)
                    {
                        defensaJudicial.EstadoProcesoCodigo = ConstanCodigoEstadosDefensaJudicial.Aprobada_por_comite_tecnico;
                    }
                    if (EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Rechazada_por_comite_tecnico)
                    {
                        defensaJudicial.EstadoProcesoCodigo = ConstanCodigoEstadosDefensaJudicial.Rechazado_por_comite_tecnico;
                    }
                    if (EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_tecnico)
                    {
                        defensaJudicial.EstadoProcesoCodigo = ConstanCodigoEstadosDefensaJudicial.Devuelta_por_comite_tecnico;
                    }

                }

            }

            #endregion Defensa Judicial

            #region Defensa Judicial Seguimiento

            if (TipoSolicitud == ConstanCodigoTipoSolicitud.Actuaciones_Defensa_judicial)
            {
                DefensaJudicialSeguimiento defensaJudicialSeguimiento = _context.DefensaJudicialSeguimiento.Find(SolicitudId);

                if (defensaJudicialSeguimiento != null)
                {
                    if (EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Aprobada_por_comite_tecnico)
                    {
                        defensaJudicialSeguimiento.EstadoProcesoCodigo = ConstanCodigoEstadosDefensaJudicial.Aprobada_por_comite_tecnico;
                    }
                    if (EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Rechazada_por_comite_tecnico)
                    {
                        defensaJudicialSeguimiento.EstadoProcesoCodigo = ConstanCodigoEstadosDefensaJudicial.Rechazado_por_comite_tecnico;
                    }
                    if (EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_tecnico)
                    {
                        defensaJudicialSeguimiento.EstadoProcesoCodigo = ConstanCodigoEstadosDefensaJudicial.Devuelta_por_comite_tecnico;
                    }

                }

            }

            #endregion Defensa Judicial seguimiento

            #region Novedad contractual

            if (TipoSolicitud == ConstanCodigoTipoSolicitud.Novedad_Contractual)
            {
                NovedadContractual novedadContractual = _context.NovedadContractual.Find(SolicitudId);

                if (novedadContractual != null)
                {
                    if (EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Aprobada_por_comite_tecnico)
                    {
                        novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.aprobado_por_comite_tecnico;
                    }
                    if (EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Rechazada_por_comite_tecnico)
                    {
                        novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.rechazado_por_comite_tecnico;
                    }
                    if (EstadoCodigo == ConstanCodigoEstadoSesionComiteSolicitud.Devuelta_por_comite_tecnico)
                    {
                        novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.devuelto_por_comite_tecnico;
                    }

                }

            }

            #endregion Novedad contractual

            _context.SaveChanges();
        }

        public async Task<Respuesta> CreateEditActasSesionSolicitudCompromiso(SesionComiteSolicitud pSesionComiteSolicitud)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Edit_Sesion_Solicitud_Compromisos_ACTAS, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = "";
            try
            {
                #region actualiza con info del acta

                SesionComiteSolicitud sesionComiteSolicitudOld = _context.SesionComiteSolicitud.Find(pSesionComiteSolicitud.SesionComiteSolicitudId);

                sesionComiteSolicitudOld.FechaModificacion = DateTime.Now;
                sesionComiteSolicitudOld.UsuarioModificacion = pSesionComiteSolicitud.UsuarioModificacion;
                sesionComiteSolicitudOld.EstadoCodigo = pSesionComiteSolicitud.EstadoCodigo;
                sesionComiteSolicitudOld.GeneraCompromiso = pSesionComiteSolicitud.GeneraCompromiso;
                sesionComiteSolicitudOld.CantCompromisos = pSesionComiteSolicitud.CantCompromisos;
                sesionComiteSolicitudOld.EstadoCodigo = pSesionComiteSolicitud.EstadoCodigo;
                sesionComiteSolicitudOld.Observaciones = pSesionComiteSolicitud.Observaciones;
                sesionComiteSolicitudOld.RutaSoporteVotacion = pSesionComiteSolicitud.RutaSoporteVotacion;
                sesionComiteSolicitudOld.DesarrolloSolicitud = pSesionComiteSolicitud.DesarrolloSolicitud;
                sesionComiteSolicitudOld.SesionSolicitudCompromiso = new List<SesionSolicitudCompromiso>();

                //para validar si los proyectos tienen estados validos
                sesionComiteSolicitudOld.Contratacion = pSesionComiteSolicitud.Contratacion;

                sesionComiteSolicitudOld.RegistroCompleto = ValidarRegistroCompletoSesionComiteSolicitud(sesionComiteSolicitudOld, pSesionComiteSolicitud.SesionSolicitudCompromiso.ToList());

                #endregion actualiza con info del acta

                #region Contratacion

                if (pSesionComiteSolicitud.TipoSolicitud == ConstanCodigoTipoSolicitud.Contratacion)
                {
                    if (pSesionComiteSolicitud.Contratacion.ContratacionProyecto != null)
                    {
                        pSesionComiteSolicitud.Contratacion.ContratacionProyecto.ToList().ForEach(ct =>
                        {
                            Proyecto proy = _context.Proyecto.Find(ct.Proyecto.ProyectoId);
                            if (pSesionComiteSolicitud.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
                            {
                                if (ct.Proyecto.EstadoProyectoObraCodigo != null)
                                    proy.EstadoProyectoObraCodigo = ct.Proyecto.EstadoProyectoObraCodigo;
                                else
                                {
                                    sesionComiteSolicitudOld.RegistroCompleto = false;
                                }
                            }

                            if (pSesionComiteSolicitud.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString())
                            {
                                if (ct.Proyecto.EstadoProyectoInterventoriaCodigo != null)
                                    proy.EstadoProyectoInterventoriaCodigo = ct.Proyecto.EstadoProyectoInterventoriaCodigo;
                                else
                                {
                                    sesionComiteSolicitudOld.RegistroCompleto = false;
                                }
                            }

                        });

                    }
                }

                #endregion Contratacion

                #region compromisos

                List<SesionSolicitudCompromiso> listaCompromisos = _context.SesionSolicitudCompromiso
                                                                                .Where(x => x.SesionComiteSolicitudId == pSesionComiteSolicitud.SesionComiteSolicitudId)
                                                                                .ToList();

                listaCompromisos.ForEach(c =>
               {
                   if (pSesionComiteSolicitud.SesionSolicitudCompromiso.Where(x => x.SesionSolicitudCompromisoId == c.SesionSolicitudCompromisoId).Count() == 0)
                   {
                       SesionSolicitudCompromiso sesionSolicitudCompromiso = _context.SesionSolicitudCompromiso.Find(c.SesionSolicitudCompromisoId);

                       sesionSolicitudCompromiso.Eliminado = true;

                       _context.SesionSolicitudCompromiso.Update(sesionSolicitudCompromiso);
                   }
               });

                foreach (var compromiso in pSesionComiteSolicitud.SesionSolicitudCompromiso)
                {
                    if (compromiso.SesionSolicitudCompromisoId == 0)
                    {
                        CreateEdit = "CREAR SOLICITUD COMPROMISO";
                        compromiso.UsuarioCreacion = pSesionComiteSolicitud.UsuarioCreacion;
                        compromiso.FechaCreacion = DateTime.Now;
                        compromiso.Eliminado = false;

                        _context.SesionSolicitudCompromiso.Add(compromiso);
                    }
                    else
                    {
                        CreateEdit = "EDITAR SOLICITUD COMPROMISO";
                        SesionSolicitudCompromiso sesionSolicitudCompromisoOld = _context.SesionSolicitudCompromiso.Find(compromiso.SesionSolicitudCompromisoId);

                        sesionSolicitudCompromisoOld.FechaModificacion = compromiso.FechaModificacion;
                        sesionSolicitudCompromisoOld.UsuarioModificacion = pSesionComiteSolicitud.UsuarioCreacion;

                        sesionSolicitudCompromisoOld.Tarea = compromiso.Tarea;
                        sesionSolicitudCompromisoOld.FechaCumplimiento = compromiso.FechaCumplimiento;

                        sesionSolicitudCompromisoOld.ResponsableSesionParticipanteId = compromiso.ResponsableSesionParticipanteId;
                        _context.SesionSolicitudCompromiso.Update(sesionSolicitudCompromisoOld);
                    }
                }

                #endregion compromisos

                _context.SaveChanges();
                return
                   new Respuesta
                   {
                       Data = validarcompletosActa(pSesionComiteSolicitud.ComiteTecnicoId),
                       IsSuccessful = true,
                       IsException = false,
                       IsValidation = false,
                       Code = ConstantSesionComiteTecnico.OperacionExitosa,
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccion, pSesionComiteSolicitud.UsuarioCreacion, CreateEdit)
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

        #endregion

        #region Plantillas 

        public async Task<byte[]> GetPlantillaByTablaIdRegistroId(string pTablaId, int pRegistroId, int pComiteTecnicoId)
        {
            return pTablaId switch
            {
                ConstanCodigoTipoSolicitud.Contratacion => await ReplacePlantillaFichaContratacion(pRegistroId),
                ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion => await ReplacePlantillaProcesosSeleccion(pRegistroId),
                ConstanCodigoTipoSolicitud.ControversiasContractuales => await ReplacePlantillaControversiasContractuales(pRegistroId, pComiteTecnicoId),
                ConstanCodigoTipoSolicitud.Actuaciones_Controversias_Contractuales => await ReplacePlantillaActuacionesControversiasContractuales(pRegistroId, pComiteTecnicoId),
                ConstanCodigoTipoSolicitud.Novedad_Contractual => await ReplacePlantillaNovedadContractual(pRegistroId),
                ConstanCodigoTipoSolicitud.Defensa_judicial => await _judicialDefense.GetPlantillaDefensaJudicial(pRegistroId, 2),
                ConstanCodigoTipoSolicitud.Evaluacion_De_Proceso => await ReplacePlantillaProcesosSeleccion(pRegistroId),
                _ => Array.Empty<byte>(),
            };
        }

        public async Task<byte[]> ReplacePlantillaFichaContratacion(int pContratacionId)
        {
            Contratacion contratacion = await _IProjectContractingService.GetAllContratacionByContratacionId(pContratacionId);

            if (contratacion == null)
            {
                return Array.Empty<byte>();
            }

            string TipoPlantilla = ((int)ConstanCodigoPlantillas.Ficha_De_Contratacion).ToString();

            Plantilla Plantilla = _context.Plantilla.Where(r => r.Codigo == TipoPlantilla).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();
            Plantilla.Contenido = ReemplazarDatosPlantillaContratacion(Plantilla.Contenido, contratacion);
            //return ConvertirPDF(Plantilla);
            return PDF.Convertir(Plantilla);

        }

        public async Task<ProcesoSeleccion> GetProcesosSelecccionByProcesoSeleccionId(int pId)
        {
            ProcesoSeleccion proceso =

            _context.ProcesoSeleccion
               .Where(r => r.ProcesoSeleccionId == pId)
               .IncludeFilter(r => r.ProcesoSeleccionCronograma.Where(r => !(bool)r.Eliminado))
               .IncludeFilter(r => r.ProcesoSeleccionGrupo.Where(r => !(bool)r.Eliminado))
               //Aqui falta filtrarlos proponentes ya que en model y en codigo no de guarda eliminado
               .Include(r => r.ProcesoSeleccionProponente)
               //.Include( r => r.ProcesoSeleccionIntegrante )
               .FirstOrDefault();

            return proceso;

        }

        public async Task<byte[]> ReplacePlantillaProcesosSeleccion(int pProcesoSeleccionId)
        {
            ProcesoSeleccion procesoSeleccion = await GetProcesosSelecccionByProcesoSeleccionId(pProcesoSeleccionId);
            procesoSeleccion.ProcesoSeleccionIntegrante = _context.ProcesoSeleccionIntegrante
                                                                        .Where(r => r.ProcesoSeleccionId == procesoSeleccion.ProcesoSeleccionId)
                                                                        .ToList();

            if (procesoSeleccion == null)
            {
                return Array.Empty<byte>();
            }

            string TipoPlantilla = ((int)ConstanCodigoPlantillas.Ficha_De_Procesos_De_Seleccion).ToString();

            Plantilla Plantilla = _context.Plantilla.Where(r => r.Codigo == TipoPlantilla).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();
            Plantilla.Contenido = ReemplazarDatosPlantillaProcesosSeleccion(Plantilla.Contenido, procesoSeleccion);
            //return ConvertirPDF(Plantilla);
            return PDF.Convertir(Plantilla);

        }

        public string ReemplazarDatosPlantillaProcesosSeleccion(string pPlantilla, ProcesoSeleccion pProcesoSeleccion)
        {
            pProcesoSeleccion.ProcesoSeleccionProponente = _context.ProcesoSeleccionProponente.Where(r => r.ProcesoSeleccionId == pProcesoSeleccion.ProcesoSeleccionId).ToList();
            pProcesoSeleccion.ProcesoSeleccionCotizacion = _context.ProcesoSeleccionCotizacion.Where(r => r.ProcesoSeleccionId == pProcesoSeleccion.ProcesoSeleccionId).ToList();
            pProcesoSeleccion.ProcesoSeleccionIntegrante = _context.ProcesoSeleccionIntegrante.Where(r => r.ProcesoSeleccionId == pProcesoSeleccion.ProcesoSeleccionId).ToList();

            List<Dominio> placeholders = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.PlaceHolder).ToList();
            List<Usuario> listaUsuarios = _context.Usuario.ToList();

            #region plantillas

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

            string TipoPlantillaProponentes = ((int)ConstanCodigoPlantillas.Proponentes_Proceso_Seleccion).ToString();
            string ProponenteProcesoSeleccion = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaProponentes).Select(r => r.Contenido).FirstOrDefault();
            string ProponentesProcesosSeleccion = " ";

            string TipoPlantillaPersonaNatural = ((int)ConstanCodigoPlantillas.Persona_Natural_Proceso_Seleccion).ToString();
            string PersonaNatural = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaPersonaNatural).Select(r => r.Contenido).FirstOrDefault();
            string PersonasNatural = " ";

            string TipoPlantillaPersonaJuridica = ((int)ConstanCodigoPlantillas.Persona_Juridica_Proceso_Seleccion).ToString();
            string PersonaJuridica = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaPersonaJuridica).Select(r => r.Contenido).FirstOrDefault();
            string PersonasJuridica = " ";

            string TipoPlantillaUnionTemporal = ((int)ConstanCodigoPlantillas.Union_Temporal_Proceso_Seleccion).ToString();
            string UnionTemporal = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaUnionTemporal).Select(r => r.Contenido).FirstOrDefault();
            string UnionesTemporal = " ";

            string TipoPlantillaParticipanteUnionTemporal = ((int)ConstanCodigoPlantillas.Participantes_Union_Temporal_Proceso_Seleccion).ToString();
            string ParticipanteUnionTemporal = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaParticipanteUnionTemporal).Select(r => r.Contenido).FirstOrDefault();
            string ParticipantesUnionTemporal = " ";

            #endregion plantillas

            List<Dominio> ListaParametricas = _context.Dominio.ToList();

            #region Grupos de seleccion

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
                            string valor = "";
                            if (!string.IsNullOrEmpty(ProcesoSeleccionGrupo.TipoPresupuestoCodigo))
                            {
                                if (ProcesoSeleccionGrupo.TipoPresupuestoCodigo == ConstantCodigoTipoPresupuesto.Presupuesto_oficial)
                                {
                                    valor = ProcesoSeleccionGrupo.Valor != null ? "$" + String.Format("{0:n0}", ProcesoSeleccionGrupo.Valor) : "";
                                }
                                else if (ProcesoSeleccionGrupo.TipoPresupuestoCodigo == ConstantCodigoTipoPresupuesto.Categoria_de_ejecucion)
                                {
                                    valor = ProcesoSeleccionGrupo.ValorMinimoCategoria != null ? "$" + String.Format("{0:n0}", ProcesoSeleccionGrupo.ValorMinimoCategoria) : "" + " - " + ProcesoSeleccionGrupo.ValorMaximoCategoria != null ? "$" + String.Format("{0:n0}", ProcesoSeleccionGrupo.ValorMaximoCategoria) : "";
                                }
                            }
                            DetallesGrupoProcesosSeleccion = DetallesGrupoProcesosSeleccion
                                 .Replace(placeholderDominio.Nombre, valor);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.PLAZO_EN_MESES_PS:
                            DetallesGrupoProcesosSeleccion = DetallesGrupoProcesosSeleccion
                                .Replace(placeholderDominio.Nombre, ProcesoSeleccionGrupo.PlazoMeses.ToString());
                            break;
                    }
                }
            }

            #endregion Grupos de seleccion

            #region Cronograma

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
                             ProcesoSeleccionCronograma.FechaMaxima.HasValue ? ProcesoSeleccionCronograma.FechaMaxima.Value.ToString("dd-MM-yyyy") : null);
                            break;
                    }
                }
            }

            #endregion Cronograma

            #region Tipo de proceso de solicitud

            //Plantilla que Depende del Tipo de proceso de solicitud

            switch (pProcesoSeleccion.TipoProcesoCodigo)
            {
                #region Invitacion_Abierta
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

                                Usuario responsable = listaUsuarios.Find(r => r.UsuarioId == pProcesoSeleccion.ResponsableTecnicoUsuarioId);

                                if (responsable != null)
                                    NombresPreponente = string.Concat(responsable.PrimerNombre, " ", responsable.PrimerApellido, " - ");

                                responsable = listaUsuarios.Find(r => r.UsuarioId == pProcesoSeleccion.ResponsableEstructuradorUsuarioid);

                                if (responsable != null)
                                    NombresPreponente += string.Concat(responsable.PrimerNombre, " ", responsable.PrimerApellido, " - ");

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
                #endregion Invitacion_Abierta

                #region Invitacion_Cerrada
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

                                Usuario responsable = listaUsuarios.Find(r => r.UsuarioId == pProcesoSeleccion.ResponsableTecnicoUsuarioId);

                                if (responsable != null)
                                    NombresPreponente = string.Concat(responsable.PrimerNombre, " ", responsable.PrimerApellido, " - ");

                                responsable = listaUsuarios.Find(r => r.UsuarioId == pProcesoSeleccion.ResponsableEstructuradorUsuarioid);

                                if (responsable != null)
                                    NombresPreponente += string.Concat(responsable.PrimerNombre, " ", responsable.PrimerApellido, " - ");

                                ProcesosSeleccionCerrada = ProcesosSeleccionCerrada.
                                Replace(placeholderDominio.Nombre, NombresPreponente);
                                break;

                            //[4:02 PM, 8/26/2020] Faber Ivolucion: se campo no tiene descripción
                            //[4:03 PM, 8 / 26 / 2020] Faber Ivolucion: no se si lo quitaron o ya en aparece algo en el control de cambios
                            //    [4:04 PM, 8 / 26 / 2020] JULIÁN MARTÍNEZ C: y el VALOR_CONTIZACION_CERRADA
                            //        [4:12 PM, 8 / 26 / 2020] Faber Ivolucion: Tampoco aparece en CU

                            case ConstanCodigoVariablesPlaceHolders.NOMBRE_ORGANIZACION_CERRADA_PS:

                                string cotizaciones = "";

                                pProcesoSeleccion.ProcesoSeleccionCotizacion.ToList().ForEach(c =>
                                {
                                    cotizaciones = cotizaciones + ProponenteProcesoSeleccion;
                                    cotizaciones = cotizaciones.Replace(placeholderDominio.Nombre, c.NombreOrganizacion)
                                    .Replace("[VALOR_CONTIZACION_CERRADA_PS]", "$" + String.Format("{0:n0}", c.ValorCotizacion));


                                });

                                ProcesosSeleccionCerrada = ProcesosSeleccionCerrada.
                                  Replace("[COTIZACIONES]", cotizaciones);

                                string proponentes = "";
                                int posicion = 1;

                                pProcesoSeleccion.ProcesoSeleccionProponente.ToList().ForEach(pps =>
                               {
                                   switch (pps.TipoProponenteCodigo)
                                   {
                                       case ConstanCodigoTipoProponente.Personal_Natural:
                                           proponentes = proponentes + PersonaNatural;
                                           proponentes = proponentes.Replace("[NOMBRE]", pps.NombreProponente)
                                                                       .Replace("[DOCUMENTO]", pps.NumeroIdentificacion)
                                                                       .Replace("[NUMERO_PROPONENTE]", posicion.ToString());
                                           break;

                                       case ConstanCodigoTipoProponente.Persona_Juridica_Individual:
                                           proponentes = proponentes + PersonaJuridica;
                                           proponentes = proponentes.Replace("[NOMBRE]", pps.NombreProponente)
                                                                    .Replace("[NIT]", pps.NumeroIdentificacion)
                                                                    .Replace("[NOMBRE_REPESENTANTE_LEGAL]", pps.NombreRepresentanteLegal)
                                                                    .Replace("[DOCUMENTO_REPRESENTANTE_LEGAL]", pps.CedulaRepresentanteLegal)
                                                                    .Replace("[NUMERO_PROPONENTE]", posicion.ToString());
                                           break;
                                       case ConstanCodigoTipoProponente.Persona_Juridica_Union_Temporal_o_Consorcio:

                                           string participantes = "";

                                           pProcesoSeleccion.ProcesoSeleccionIntegrante.ToList().ForEach(ppi =>
                                          {
                                              participantes = participantes + ParticipanteUnionTemporal;
                                              participantes = participantes.Replace("[NOMBRE]", ppi.NombreIntegrante)
                                                                            .Replace("[PARTICIPACION]", ppi.PorcentajeParticipacion != null ? ppi.PorcentajeParticipacion.ToString() + " %" : "0 %");
                                          });

                                           proponentes = proponentes + UnionTemporal;
                                           proponentes = proponentes.Replace("[NOMBRE]", pps.NombreProponente)
                                                                    .Replace("[NOMBRE_REPESENTANTE_LEGAL]", pps.NombreRepresentanteLegal)
                                                                    .Replace("[DOCUMENTO_REPRESENTANTE_LEGAL]", pps.CedulaRepresentanteLegal)
                                                                    .Replace("[NUMERO_PROPONENTE]", posicion.ToString())
                                                                    .Replace("[PARTICIPANTES_UNION_TEMPORAL]", participantes);
                                           break;

                                   }


                                   posicion++;
                               });

                                ProcesosSeleccionCerrada = ProcesosSeleccionCerrada.
                                  Replace("[PROPONENTES]", proponentes);


                                break;

                            //case ConstanCodigoVariablesPlaceHolders.VALOR_CONTIZACION_CERRADA_PS:
                            //    pProcesoSeleccion.ProcesoSeleccionCotizacion.ToList().ForEach(c =>
                            //    {
                            //        ProcesosSeleccionCerrada = ProcesosSeleccionCerrada.
                            //      Replace(placeholderDominio.Nombre, "$" + String.Format("{0:n0}", c.ValorCotizacion));
                            //    });

                            //    break;

                            case ConstanCodigoVariablesPlaceHolders.EVALUACION_DESCRIPCION_CERRADA_PS:
                                ProcesosSeleccionCerrada = ProcesosSeleccionCerrada.
                                  Replace(placeholderDominio.Nombre, pProcesoSeleccion.EvaluacionDescripcion);
                                break;
                        }
                    }
                    break;

                #endregion Invitacion_Cerrada

                #region Invitacion_Privada

                case ConstanCodigoTipoProcesoSeleccion.Invitacion_Privada:
                    //ProcesosSeleccionPrivada = ProcesoSeleccionPrivada;
                    foreach (ProcesoSeleccionProponente proponente in pProcesoSeleccion.ProcesoSeleccionProponente)
                    {
                        int cantidad = 1;
                        ProcesosSeleccionPrivada = ProcesosSeleccionPrivada + ProcesoSeleccionPrivada;

                        foreach (Dominio placeholderDominio in placeholders)
                        {
                            ProcesosSeleccionPrivada = ProcesosSeleccionPrivada.
                                      Replace("[NUMERO_PROPONENTE]", cantidad.ToString());

                            cantidad++;

                            switch (placeholderDominio.Codigo)
                            {
                                case ConstanCodigoVariablesPlaceHolders.TIPO_PROPONENTE_PRIVADA_PS:

                                    ProcesosSeleccionPrivada = ProcesosSeleccionPrivada.
                                      Replace(placeholderDominio.Nombre,
                                                ListaParametricas
                                                    .Where(
                                                            r => r.Codigo == proponente.TipoProponenteCodigo &&
                                                            r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Proponente
                                                    )
                                                    .FirstOrDefault()
                                                    .Nombre);
                                    break;

                                case ConstanCodigoVariablesPlaceHolders.NOMBRE_PRIVADA_PS:

                                    ProcesosSeleccionPrivada = ProcesosSeleccionPrivada.
                                      Replace(placeholderDominio.Nombre, proponente.NombreProponente);
                                    break;

                                case ConstanCodigoVariablesPlaceHolders.TIPO_DOCUMENTO_PRIVADA_PS:

                                    string tipoProponente = "";

                                    if (pProcesoSeleccion.ProcesoSeleccionProponente.Count() > 0)
                                    {
                                        if (proponente.TipoProponenteCodigo == "2" ||
                                             proponente.TipoProponenteCodigo == "4"
                                            )
                                        {
                                            tipoProponente = "NIT";
                                        }
                                        else
                                        {
                                            tipoProponente = "CC";
                                        }

                                    }

                                    ProcesosSeleccionPrivada = ProcesosSeleccionPrivada.Replace(placeholderDominio.Nombre, tipoProponente);

                                    break;

                                case ConstanCodigoVariablesPlaceHolders.NOMBRE_REPRESENTANTE_LEGAL_PRIVADA_PS:
                                    ProcesosSeleccionPrivada = ProcesosSeleccionPrivada.
                                   Replace(placeholderDominio.Nombre, (proponente.NombreRepresentanteLegal));

                                    break;
                            }
                        }
                        // switch (placeholderDominio.Codigo)
                        // {
                        //     case ConstanCodigoVariablesPlaceHolders.TIPO_PROPONENTE_PRIVADA_PS:

                        //         ProcesosSeleccionPrivada = ProcesosSeleccionPrivada.
                        //           Replace(placeholderDominio.Nombre, (pProcesoSeleccion.ProcesoSeleccionProponente.Count() > 0) ? ListaParametricas
                        //           .Where(r => r.Codigo == pProcesoSeleccion.ProcesoSeleccionProponente.FirstOrDefault().TipoProponenteCodigo
                        //           && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Proponente
                        //           ).FirstOrDefault().Nombre : " ");
                        //         break;

                        //     case ConstanCodigoVariablesPlaceHolders.NOMBRE_PRIVADA_PS:

                        //         ProcesosSeleccionPrivada = ProcesosSeleccionPrivada.
                        //           Replace(placeholderDominio.Nombre, (pProcesoSeleccion.ProcesoSeleccionProponente.Count() > 0) ? pProcesoSeleccion.ProcesoSeleccionProponente.FirstOrDefault().NombreProponente : "");
                        //         break;

                        //     case ConstanCodigoVariablesPlaceHolders.TIPO_DOCUMENTO_PRIVADA_PS:

                        //         string tipoProponente = "";

                        //         if (pProcesoSeleccion.ProcesoSeleccionProponente.Count() > 0)
                        //         {
                        //             if ( pProcesoSeleccion.ProcesoSeleccionProponente.FirstOrDefault().TipoProponenteCodigo == "2" ||
                        //                  pProcesoSeleccion.ProcesoSeleccionProponente.FirstOrDefault().TipoProponenteCodigo == "4"
                        //                 )    
                        //                 {
                        //                     tipoProponente = "NIT";
                        //                 }else{
                        //                     tipoProponente = "CC";
                        //                 }

                        //         }

                        //         ProcesosSeleccionPrivada = ProcesosSeleccionPrivada.Replace(placeholderDominio.Nombre, tipoProponente );

                        //         break;

                        //     case ConstanCodigoVariablesPlaceHolders.NOMBRE_REPRESENTANTE_LEGAL_PRIVADA_PS:
                        //         ProcesosSeleccionPrivada = ProcesosSeleccionPrivada.
                        //        Replace(placeholderDominio.Nombre, (pProcesoSeleccion.ProcesoSeleccionProponente.Count() > 0) ? pProcesoSeleccion.ProcesoSeleccionProponente.FirstOrDefault().NombreRepresentanteLegal : "");

                        //         break;
                        // }
                    }
                    break;

                    #endregion Invitacion_Privada
            }

            #endregion Tipo de proceso de solicitud

            #region principal

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
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pProcesoSeleccion.FechaCreacion.ToString("dd-MM-yyyy"));
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

            #endregion principal

            return pPlantilla;

        }

        public string ReemplazarDatosPlantillaContratacion(string pPlantilla, Contratacion pContratacion)
        {
            List<Dominio> placeholders = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.PlaceHolder).ToList();

            string TipoPlantillaDetalleProyecto = ((int)ConstanCodigoPlantillas.Detalle_Proyecto).ToString();
            string DetalleProyecto = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaDetalleProyecto).Select(r => r.Contenido).FirstOrDefault();
            string DetallesProyectos = "";

            string TipoPlantillaRegistrosAlcance = ((int)ConstanCodigoPlantillas.Registros_Tabla_Alcance).ToString();
            string RegistroAlcance = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaRegistrosAlcance).Select(r => r.Contenido).FirstOrDefault();

            //  pContratacion.ContratacionProyecto.FirstOrDefault().ContratacionProyectoAportante.FirstOrDefault().ComponenteAportante.

            List<Dominio> ListaParametricas = _context.Dominio.ToList();
            List<Localizacion> ListaLocalizaciones = _context.Localizacion.ToList();
            List<InstitucionEducativaSede> ListaInstitucionEducativaSedes = _context.InstitucionEducativaSede.ToList();

            if (pContratacion.PlazoContratacion == null)
            {
                pContratacion.PlazoContratacion = new PlazoContratacion();
            }
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

                #region Detalle Solicitud

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
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, proyecto.Proyecto.PlazoMesesObra.ToString());
                            break;

                        case ConstanCodigoVariablesPlaceHolders.PLAZO_OBRA_DIAS:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, proyecto.Proyecto.PlazoDiasObra.ToString());
                            break;

                        case ConstanCodigoVariablesPlaceHolders.PLAZO_INTERVENTORIA_MESES:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, proyecto.Proyecto.PlazoMesesInterventoria.ToString());
                            break;

                        case ConstanCodigoVariablesPlaceHolders.PLAZO_INTERVENTORIA_DIAS:
                            DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, proyecto.Proyecto.PlazoDiasInterventoria.ToString());
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

                #endregion Detalle Solicitud

            }

            string TipoPlantillaFuentesFinanciacion = ((int)ConstanCodigoPlantillas.Registros_Fuente_De_Uso).ToString();

            // FUENTES DE FINANCIACION 
            string TipoPlantillaRegistrosFuentes = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaFuentesFinanciacion).Select(r => r.Contenido).FirstOrDefault();
            string RegistrosFuentesUso = string.Empty;


            string TipoPlantillaRegistrosUsosFuenteUsos = ((int)ConstanCodigoPlantillas.Registros_Usos_Registros_Fuente_de_Uso).ToString();
            string PlantillaRegistrosUsosFuenteUsos = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaRegistrosUsosFuenteUsos).Select(r => r.Contenido).FirstOrDefault();
            string RegistrosRegistrosUsosFuenteUsos = string.Empty;

            string row_template = "<td rowspan='[ROWSPAN]'><div>[ROW]</div></td>";


            #region fuentes usos

            foreach (var contratacionProyecto in pContratacion.ContratacionProyecto)
            {
                foreach (var ContratacionProyectoAportante in contratacionProyecto.ContratacionProyectoAportante)
                {
                    bool ind_ya_entro = false;

                    foreach (var ComponenteAportante in ContratacionProyectoAportante.ComponenteAportante)
                    {
                        foreach (var ComponenteUso in ComponenteAportante.ComponenteUso)
                        {
                            RegistrosFuentesUso += TipoPlantillaRegistrosFuentes;

                            string nombreTrAportante = string.Empty;
                            string valorTrAportante = string.Empty;

                            if (!ind_ya_entro)
                            {
                                int total = 0;
                                foreach(var cpa in ContratacionProyectoAportante.ComponenteAportante)
                                {
                                    total += cpa.ComponenteUso.Count();
                                }
                                string rowspan = total.ToString();
                                /*
                                 * Nombre aportante
                                */

                                string nombre_aportante_row = row_template;
                                nombre_aportante_row = nombre_aportante_row.Replace("[ROWSPAN]", rowspan);
                                
                                string strNombreAportante = string.Empty;
                                switch (ContratacionProyectoAportante.CofinanciacionAportante.TipoAportanteId) { 
                                    case ConstanTipoAportante.Ffie:
                                        strNombreAportante = ConstanStringTipoAportante.Ffie;
                                        break;

                                    case ConstanTipoAportante.ET:

                                        if (ContratacionProyectoAportante.CofinanciacionAportante.Departamento != null && ContratacionProyectoAportante.CofinanciacionAportante.Municipio == null)
                                        {
                                            strNombreAportante = "Gobernación de " + ContratacionProyectoAportante.CofinanciacionAportante.Departamento.Descripcion;
                                        }
                                        else if (ContratacionProyectoAportante.CofinanciacionAportante.Municipio != null)
                                        {
                                            strNombreAportante = "Alcaldía de " + ContratacionProyectoAportante.CofinanciacionAportante.Municipio.Descripcion;
                                        }
                                        break;
                                    case ConstanTipoAportante.Tercero:
                                        strNombreAportante = ContratacionProyectoAportante.CofinanciacionAportante.NombreAportante.Nombre;
                                        break;
                                }
                                nombreTrAportante = nombre_aportante_row.Replace("[ROW]", strNombreAportante);
                                /*
                                 * Valor aportante
                                */
                                string valor_aportante_row = row_template;
                                valor_aportante_row = valor_aportante_row.Replace("[ROWSPAN]", rowspan);

                                string ValorAportante = "$" + String.Format("{0:n0}", ContratacionProyectoAportante.CofinanciacionAportante.ProyectoAportante.FirstOrDefault().ValorObra);
                                if (pContratacion.TipoSolicitudCodigo == ((int)ConstanCodigoTipoContratacion.Interventoria).ToString())
                                {
                                    ValorAportante = "$" + String.Format("{0:n0}", ContratacionProyectoAportante.CofinanciacionAportante.ProyectoAportante.FirstOrDefault().ValorInterventoria);
                                }

                                valorTrAportante = valor_aportante_row.Replace("[ROW]", ValorAportante);

                                ind_ya_entro = true;
                            }
                            else
                            {
                                nombreTrAportante = string.Empty;
                                valorTrAportante = string.Empty;
                            }
                            foreach (Dominio placeholderDominio in placeholders)
                            {
                                switch (placeholderDominio.Codigo)
                                {
                                    case ConstanCodigoVariablesPlaceHolders.NOMBRE_APORTANTE_FUENTES_USO:
                                        RegistrosFuentesUso = RegistrosFuentesUso.Replace(placeholderDominio.Nombre, nombreTrAportante);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.VALOR_APORTANTE_PROYECTO_FUENTES_USO:
                                        RegistrosFuentesUso = RegistrosFuentesUso.Replace(placeholderDominio.Nombre, valorTrAportante);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.FASE_FUENTES_USO:
                                        string strFase = string.Empty;

                                        if (ComponenteAportante != null)
                                        {
                                            if (!string.IsNullOrEmpty(ComponenteAportante.FaseCodigo))
                                            {
                                                strFase = ListaParametricas.Where(r => r.Codigo == ComponenteAportante.FaseCodigo &&
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
                                            string strTipoUso = ListaParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Usos && r.Codigo == ComponenteUso.TipoUsoCodigo)?.FirstOrDefault()?.Nombre;

                                            RegistrosFuentesUso = RegistrosFuentesUso.Replace("[USO_FUENTES_USO]", strTipoUso);
                                            RegistrosFuentesUso = RegistrosFuentesUso.Replace("[VALOR_USO_FUENTE_USO]", "$" + String.Format("{0:n0}", ComponenteUso.ValorUso));

                                            //foreach (var ComponenteAportante in ContratacionProyectoAportante.ComponenteAportante)
                                            //for (int i = 1; i < ContratacionProyectoAportante.ComponenteAportante.Count; i++)
                                            // {
                                            //     ContratacionProyectoAportante.ComponenteAportante.ToArray()[i].ComponenteUso.ToList().ForEach(ComponenteUso =>
                                            //    {
                                            //        string strTipoUso2 = ListaParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Usos && r.Codigo == ComponenteUso.TipoUsoCodigo).FirstOrDefault().Nombre;

                                            //        RegistrosRegistrosUsosFuenteUsos += PlantillaRegistrosUsosFuenteUsos;
                                            //        RegistrosRegistrosUsosFuenteUsos = RegistrosRegistrosUsosFuenteUsos.Replace("[USO_FUENTES_USO]", strTipoUso2);
                                            //        RegistrosRegistrosUsosFuenteUsos = RegistrosRegistrosUsosFuenteUsos.Replace("[VALOR_USO_FUENTE_USO]", "$" + String.Format("{0:n0}", ComponenteUso.ValorUso.ToString()));
                                            //        //RegistrosFuentesUso = RegistrosFuentesUso.Replace("[USO_FUENTES_USO]", strTipoUso2);
                                            //        //RegistrosFuentesUso = RegistrosFuentesUso.Replace("[VALOR_USO_FUENTE_USO]", "$" + String.Format("{0:n0}", ComponenteUso.ValorUso.ToString()));
                                            //    });

                                            // }
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

                                        // ContratacionProyectoAportante.ComponenteAportante.Where(r => r.Eliminado != true).ToList().ForEach(ca =>
                                        // {
                                        //     cantidadComponentes++;

                                        //     ca.ComponenteUso.Where(r => r.Eliminado != true).ToList().ForEach(cu =>
                                        //     {
                                        //         cantidadUsos++;
                                        //     });
                                        // });
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
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pContratacion.FechaTramite != null ? ((DateTime)pContratacion.FechaTramite).ToString("dd-MM-yyyy") : " ");
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
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pContratacion.Contratista.RepresentanteLegalNumeroIdentificacion);
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

                    // case ConstanCodigoVariablesPlaceHolders.NUMERO_DE_LICENCIA:
                    //     string numeroLicencia = "";
                    //     if (pContratacion.ContratacionProyecto.Count() > 0)
                    //     {
                    //         numeroLicencia = pContratacion.ContratacionProyecto.FirstOrDefault().NumeroLicencia;
                    //     }
                    //     pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, numeroLicencia);
                    //     break;

                    // case ConstanCodigoVariablesPlaceHolders.FECHA_DE_VIGENCIA:
                    //     string fechaVigencia = "";
                    //     if (pContratacion.ContratacionProyecto.Count() > 0)
                    //     {
                    //         if (pContratacion.ContratacionProyecto.FirstOrDefault().FechaVigencia != null)
                    //         {
                    //             fechaVigencia = ((DateTime)pContratacion.ContratacionProyecto.FirstOrDefault().FechaVigencia).ToString("yy-MM-dd");

                    //         }
                    //     }
                    //     pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, fechaVigencia);
                    //     break;

                    case ConstanCodigoVariablesPlaceHolders.CONSIDERACIONES_ESPECIALES:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pContratacion.ConsideracionDescripcion);
                        break;

                    case ConstanCodigoVariablesPlaceHolders.PLAZO_MESES:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pContratacion.PlazoContratacion?.PlazoMeses.ToString());

                        break;
                    case ConstanCodigoVariablesPlaceHolders.PLAZO_DIAS:
                        pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, pContratacion.PlazoContratacion?.PlazoDias.ToString());
                        break;


                }
            }

            #endregion fuentes usos


            #region caracteristicas tecnicas

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
                ContratacionProyecto contratacionProyecto = pContratacion.ContratacionProyecto.ToArray()[0];

                if (contratacionProyecto.TieneMonitoreoWeb != null && contratacionProyecto.TieneMonitoreoWeb.Value == true)
                {

                    contratacionProyecto.TieneMonitoreoWeb = true;

                    strPregunta_1 = ConstanStringRespuestasBool.SI;

                }
                else
                {

                    contratacionProyecto.TieneMonitoreoWeb = false;

                    strPregunta_1 = ConstanStringRespuestasBool.NO;

                }

                //Reasignacion 
                if (contratacionProyecto.EsReasignacion != null && contratacionProyecto.EsReasignacion.Value == true)
                {

                    contratacionProyecto.EsReasignacion = true;
                    strPregunta_2 = ContenidoPregunta2 + ConstanStringRespuestasBool.SI;

                    strPregunta_3 = ContenidoPregunta3 + (contratacionProyecto.EsAvanceobra != true ? "No" : "Si");
                }
                else
                {
                    contratacionProyecto.EsReasignacion = false;
                    strPregunta_2 = ContenidoPregunta2 + ConstanStringRespuestasBool.NO;

                    strPregunta_5 = ContenidoPregunta5 +
                                    ((contratacionProyecto.RequiereLicencia.HasValue && contratacionProyecto.RequiereLicencia.Value == true) ? ConstanStringRespuestasBool.SI : ConstanStringRespuestasBool.NO);

                }

                //Avance obra
                if (contratacionProyecto.EsAvanceobra != null && contratacionProyecto.EsAvanceobra.Value == true)
                {

                    contratacionProyecto.EsAvanceobra = true;
                    strPregunta_3 = ContenidoPregunta3 + ConstanStringRespuestasBool.SI;

                    strPregunta_4 = ContenidoPregunta4 + contratacionProyecto.PorcentajeAvanceObra;

                }
                else
                {
                    contratacionProyecto.EsAvanceobra = false;
                    //strPregunta_3 = ContenidoPregunta3 + ConstanStringRespuestasBool.NO;

                    strPregunta_5 = ContenidoPregunta5 +
                                    ((contratacionProyecto.RequiereLicencia.HasValue && contratacionProyecto.RequiereLicencia.Value == true) ? ConstanStringRespuestasBool.SI : ConstanStringRespuestasBool.NO);

                }

                //Requiere Licencia
                if (contratacionProyecto.RequiereLicencia != null && contratacionProyecto.RequiereLicencia.Value == true)
                {

                    contratacionProyecto.RequiereLicencia = true;
                    strPregunta_5 = ContenidoPregunta5 + ConstanStringRespuestasBool.SI;

                    strPregunta_6 = ContenidoPregunta6 + (contratacionProyecto.LicenciaVigente != true ? "No" : "Si");

                    //strPregunta_4 = ContenidoPregunta4 + contratacionProyecto.PorcentajeAvanceObra;

                }
                else
                {
                    contratacionProyecto.RequiereLicencia = false;
                    //strPregunta_4 = ContenidoPregunta3 + ConstanStringRespuestasBool.NO;

                    //strPregunta_5 = ContenidoPregunta5 + 
                    //                ((contratacionProyecto.RequiereLicencia.HasValue && contratacionProyecto.RequiereLicencia.Value == true) ? ConstanStringRespuestasBool.SI : ConstanStringRespuestasBool.NO);

                }

                if (contratacionProyecto.LicenciaVigente != null && contratacionProyecto.LicenciaVigente.Value == true)
                {

                    contratacionProyecto.LicenciaVigente = true;
                    strPregunta_5 = ContenidoPregunta5 + ConstanStringRespuestasBool.SI;

                    pPlantilla = pPlantilla.Replace("[NUMERO_DE_LICENCIA]", "N&uacute;mero de licencia: " + contratacionProyecto.NumeroLicencia);
                    pPlantilla = pPlantilla.Replace("[FECHA_DE_VIGENCIA]", "Fecha de vigencia: : " + contratacionProyecto.FechaVigencia.Value.ToString("dd/MM/yyyy"));

                    //strPregunta_4 = ContenidoPregunta4 + contratacionProyecto.PorcentajeAvanceObra;

                }
                else
                {
                    contratacionProyecto.LicenciaVigente = false;

                    pPlantilla = pPlantilla.Replace("[NUMERO_DE_LICENCIA]", "");
                    pPlantilla = pPlantilla.Replace("[FECHA_DE_VIGENCIA]", "");

                }

            }

            //Replace Preguntas
            pPlantilla = pPlantilla.Replace("[PREGUNTA_1]", strPregunta_1);
            pPlantilla = pPlantilla.Replace("[PREGUNTA_2]", strPregunta_2);
            pPlantilla = pPlantilla.Replace("[PREGUNTA_3]", strPregunta_3);
            pPlantilla = pPlantilla.Replace("[PREGUNTA_4]", strPregunta_4);
            pPlantilla = pPlantilla.Replace("[PREGUNTA_5]", strPregunta_5);
            pPlantilla = pPlantilla.Replace("[PREGUNTA_6]", strPregunta_6);

            #endregion caracteristicas tecnicas

            return pPlantilla;
        }

        public byte[] ConvertirPDF(Plantilla pPlantilla)
        {
            string strEncabezado = "";
            if (!string.IsNullOrEmpty(pPlantilla?.Encabezado?.Contenido))
            {
                pPlantilla.Encabezado.Contenido = pPlantilla.Encabezado.Contenido.Replace("[RUTA_ICONO]", Path.Combine(Directory.GetCurrentDirectory(), "assets", "img-FFIE.png"));
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
            string rutaLogo = Path.Combine(Directory.GetCurrentDirectory(), "assets", "img-FFIE.png");
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = pPlantilla.Contenido,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "pdf-styles.css") },
                HeaderSettings = { FontName = "Roboto", FontSize = 8, Center = strEncabezado, Line = false, Spacing = 18, HtmUrl = rutaLogo },
                FooterSettings = { FontName = "Ariel", FontSize = 10, Center = "[page]" },
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings },
            };

            return _converter.Convert(pdf);
        }

        #endregion

        #region  Compromisos

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
                        temaCompromisoOld.EsCumplido = temaCompromiso.EsCumplido;
                        if (!(bool)temaCompromiso.EsCumplido)
                            if (temaCompromisoOld.EstadoCodigo == ConstantCodigoCompromisos.Finalizado)
                                temaCompromisoOld.EstadoCodigo = ConstantCodigoCompromisos.En_proceso;
                    }
                }

                foreach (var SesionComiteSolicitud in pComiteTecnico.SesionComiteSolicitudComiteTecnico)
                {
                    foreach (var pSesionSolicitudCompromiso in SesionComiteSolicitud.SesionSolicitudCompromiso)
                    {
                        SesionSolicitudCompromiso SesionSolicitudCompromisoOld = _context.SesionSolicitudCompromiso.Find(pSesionSolicitudCompromiso.SesionSolicitudCompromisoId);
                        SesionSolicitudCompromisoOld.FechaModificacion = DateTime.Now;
                        SesionSolicitudCompromisoOld.UsuarioModificacion = pComiteTecnico.UsuarioCreacion;
                        SesionSolicitudCompromisoOld.EsCumplido = pSesionSolicitudCompromiso.EsCumplido;
                        if (!(bool)SesionSolicitudCompromisoOld.EsCumplido)
                            if (SesionSolicitudCompromisoOld.EstadoCodigo == ConstantCodigoCompromisos.Finalizado)
                                SesionSolicitudCompromisoOld.EstadoCodigo = ConstantCodigoCompromisos.En_proceso;
                    }
                }

                foreach (var SesionComiteSolicitud in pComiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario)
                {
                    foreach (var pSesionSolicitudCompromiso in SesionComiteSolicitud.SesionSolicitudCompromiso)
                    {
                        SesionSolicitudCompromiso SesionSolicitudCompromisoOld = _context.SesionSolicitudCompromiso.Find(pSesionSolicitudCompromiso.SesionSolicitudCompromisoId);
                        SesionSolicitudCompromisoOld.FechaModificacion = DateTime.Now;
                        SesionSolicitudCompromisoOld.UsuarioModificacion = pComiteTecnico.UsuarioCreacion;
                        SesionSolicitudCompromisoOld.EsCumplido = pSesionSolicitudCompromiso.EsCumplido;
                        if (!(bool)SesionSolicitudCompromisoOld.EsCumplido)
                            if (SesionSolicitudCompromisoOld.EstadoCodigo == ConstantCodigoCompromisos.Finalizado)
                                SesionSolicitudCompromisoOld.EstadoCodigo = ConstantCodigoCompromisos.En_proceso;
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
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccion, pComiteTecnico.UsuarioCreacion, "VERIFICAR TEMA COMPROMISO")
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
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccion, pComiteTecnico.UsuarioCreacion, ex.InnerException.ToString())
                   };
            }
        }

        public async Task<Respuesta> ObservacionesCompromisos(ObservacionComentario pObservacionComentario)
        {
            int idAccionEliminarSesionComiteTema = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Observacion_Compromisos, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                if (pObservacionComentario.TemaCompromisoId != null)
                {
                    TemaCompromiso temaCompromisoOld = _context.TemaCompromiso.Find(pObservacionComentario.TemaCompromisoId);
                    temaCompromisoOld.FechaModificacion = DateTime.Now;
                    temaCompromisoOld.UsuarioModificacion = pObservacionComentario.Usuario;
                    if (temaCompromisoOld.EstadoCodigo == ConstantCodigoCompromisos.Finalizado)
                        temaCompromisoOld.EstadoCodigo = ConstantCodigoCompromisos.En_proceso;

                    TemaCompromisoSeguimiento temaCompromisoSeguimiento = new TemaCompromisoSeguimiento
                    {
                        UsuarioCreacion = pObservacionComentario.Usuario,
                        FechaCreacion = DateTime.Now,

                        TemaCompromisoId = (int)pObservacionComentario.TemaCompromisoId,
                        EstadoCodigo = ConstantCodigoCompromisos.En_proceso,
                        Tarea = pObservacionComentario.Observacion
                    };
                    _context.TemaCompromisoSeguimiento.Add(temaCompromisoSeguimiento);


                }
                else
                {
                    SesionSolicitudCompromiso SesionSolicitudCompromisoOld = _context.SesionSolicitudCompromiso.Find(pObservacionComentario.SesionSolicitudCompromisoId);
                    SesionSolicitudCompromisoOld.FechaModificacion = DateTime.Now;
                    SesionSolicitudCompromisoOld.UsuarioModificacion = pObservacionComentario.Usuario;
                    if (SesionSolicitudCompromisoOld.EstadoCodigo == ConstantCodigoCompromisos.Finalizado)
                        SesionSolicitudCompromisoOld.EstadoCodigo = ConstantCodigoCompromisos.En_proceso;

                    CompromisoSeguimiento compromisoSeguimiento = new CompromisoSeguimiento
                    {
                        UsuarioCreacion = pObservacionComentario.Usuario,
                        FechaCreacion = DateTime.Now,
                        Eliminado = false,

                        SesionParticipanteId = pObservacionComentario.UsuarioId,
                        SesionSolicitudCompromisoId = (int)pObservacionComentario.SesionSolicitudCompromisoId,
                        EstadoCompromisoCodigo = ConstantCodigoCompromisos.En_proceso,
                        DescripcionSeguimiento = pObservacionComentario.Observacion
                    };
                    _context.CompromisoSeguimiento.Add(compromisoSeguimiento);
                }
                await _context.SaveChangesAsync();

                return
                  new Respuesta
                  {
                      IsSuccessful = true,
                      IsException = false,
                      IsValidation = false,
                      Code = ConstantSesionComiteTecnico.OperacionExitosa,
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccionEliminarSesionComiteTema, pObservacionComentario.Usuario, "OBSERVACION SEGUIMIENTO COMPROMISO")
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccionEliminarSesionComiteTema, pObservacionComentario.Usuario, ex.InnerException.ToString().Substring(0, 500))
                  };
            }
        }

        public async Task<Respuesta> EliminarCompromisosSolicitud(int pSesionComiteSolicitudId, string pUsuarioModificacion)
        {
            int idAccionEliminarSesionComiteTema = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Compromisos_Solicitud, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                List<SesionSolicitudCompromiso> listaCompromisos = _context.SesionSolicitudCompromiso.Where(r => r.SesionComiteSolicitudId == pSesionComiteSolicitudId).ToList();
                if (listaCompromisos == null)
                {
                    return
                    new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = true,
                        IsValidation = false,
                        Code = ConstantSesionComiteTecnico.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccionEliminarSesionComiteTema, pUsuarioModificacion, "NO SE ENCONTRO REGISTRO")
                    };
                }

                SesionComiteSolicitud sesionComiteSolicitud = _context.SesionComiteSolicitud.Find(pSesionComiteSolicitudId);
                sesionComiteSolicitud.CantCompromisos = null;
                sesionComiteSolicitud.GeneraCompromiso = false;

                listaCompromisos.ForEach(c =>
                {
                    c.Eliminado = true;
                });

                _context.SaveChanges();

                return
                 new Respuesta
                 {
                     IsSuccessful = true,
                     IsException = false,
                     IsValidation = false,
                     Code = ConstantSesionComiteTecnico.OperacionExitosa,
                     Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccionEliminarSesionComiteTema, pUsuarioModificacion, "ELIMINAR SOLICITUD COMPROMISO")
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccionEliminarSesionComiteTema, pUsuarioModificacion, ex.InnerException.ToString().Substring(0, 500))
                    };
            }

        }

        public async Task<Respuesta> EliminarCompromisosTema(int pSesionTemaId, string pUsuarioModificacion)
        {
            int idAccionEliminarSesionComiteTema = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Compromisos_Tema, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                List<TemaCompromiso> listaCompromisos = _context.TemaCompromiso.Where(r => r.SesionTemaId == pSesionTemaId).ToList();
                if (listaCompromisos == null)
                {
                    return
                    new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = true,
                        IsValidation = false,
                        Code = ConstantSesionComiteTecnico.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccionEliminarSesionComiteTema, pUsuarioModificacion, "NO SE ENCONTRO REGISTRO")
                    };
                }

                SesionComiteTema sesionComiteTema = _context.SesionComiteTema.Find(pSesionTemaId);
                sesionComiteTema.CantCompromisos = null;
                sesionComiteTema.GeneraCompromiso = false;

                listaCompromisos.ForEach(c =>
                {
                    c.Eliminado = true;
                });

                _context.SaveChanges();

                return
                 new Respuesta
                 {
                     IsSuccessful = true,
                     IsException = false,
                     IsValidation = false,
                     Code = ConstantSesionComiteTecnico.OperacionExitosa,
                     Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccionEliminarSesionComiteTema, pUsuarioModificacion, "ELIMINAR SOLICITUD COMPROMISO")
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccionEliminarSesionComiteTema, pUsuarioModificacion, ex.InnerException.ToString().Substring(0, 500))
                    };
            }

        }

        #endregion

        public async Task<Respuesta> CrearObservacionProyecto(ContratacionObservacion pContratacionObservacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Observacion_Contratacion, (int)EnumeratorTipoDominio.Acciones);

            Contratacion contratacion = _context.Contratacion.Find(pContratacionObservacion.ContratacionId);

            try
            {
                Proyecto proyecto = _context.Proyecto.Find(pContratacionObservacion.ContratacionProyecto.Proyecto.ProyectoId);

                if (contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
                    proyecto.EstadoProyectoObraCodigo = pContratacionObservacion.ContratacionProyecto.Proyecto.EstadoProyectoObraCodigo;

                if (contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString())
                    proyecto.EstadoProyectoInterventoriaCodigo = pContratacionObservacion.ContratacionProyecto.Proyecto.EstadoProyectoInterventoriaCodigo;

                pContratacionObservacion.ContratacionProyecto = null;

                if (pContratacionObservacion.ContratacionObservacionId == 0)
                {
                    pContratacionObservacion.FechaCreacion = DateTime.Now;
                    _context.ContratacionObservacion.Add(pContratacionObservacion);
                }
                else
                {
                    ContratacionObservacion observacion = _context.ContratacionObservacion.Find(pContratacionObservacion.ContratacionObservacionId);

                    observacion.Observacion = pContratacionObservacion.Observacion;
                }

                _context.SaveChanges();

                return new Respuesta
                {
                    Data = pContratacionObservacion,
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccion, pContratacionObservacion.UsuarioCreacion, "CREAR OBSERVACION CONTRATACION")
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccion, pContratacionObservacion.UsuarioCreacion, ex.InnerException.ToString())
                };
            }

        }

        public async Task<Respuesta> CambiarEstadoActa(int pSesionComiteSolicitud, string pCodigoEstado, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Acta, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                SesionComiteSolicitud sesionComiteSolicitudOld = _context.SesionComiteSolicitud.Find(pSesionComiteSolicitud);
                sesionComiteSolicitudOld.UsuarioModificacion = pUsuarioModifica;
                sesionComiteSolicitudOld.FechaModificacion = DateTime.Now;
                sesionComiteSolicitudOld.EstadoActaCodigo = pCodigoEstado;

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccion, pUsuarioModifica, "CAMBIAR ESTADO ACTA")
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }

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
                    .Include(r => r.SesionComiteTema)
                        .ThenInclude(r => r.SesionTemaVoto)
                    .Include(r => r.SesionComiteSolicitudComiteTecnico)
                       .ThenInclude(r => r.SesionSolicitudCompromiso)
                           .ThenInclude(r => r.ResponsableSesionParticipante)
                              .ThenInclude(r => r.Usuario)
                    .Include(r => r.SesionComiteSolicitudComiteTecnico)
                        .ThenInclude(r => r.SesionSolicitudVoto)
                    .FirstOrDefaultAsync();

            if (comiteTecnico == null)
            {
                return Array.Empty<byte>();
            }
            Plantilla plantilla = _context.Plantilla.Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Descargar_Acta).ToString()).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();

            plantilla.Contenido = await ReemplazarDatosPlantillaActa(plantilla.Contenido, comiteTecnico);
            //return ConvertirPDF(plantilla);
            return Helpers.PDF.Convertir(plantilla);
        }

        private async Task<string> ReemplazarDatosPlantillaActa(string strContenido, ComiteTecnico pComiteTecnico)
        {
            try
            {
                string msg = "<p style='text-align:left;margin-top:5px;margin-bottom:15px;'>[MSG]</p>";
                List<int> ListProcesoSeleccionIdSolicitudId = pComiteTecnico.SesionComiteSolicitudComiteTecnico.Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion || r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Evaluacion_De_Proceso).Select(r => r.SolicitudId).ToList();
                List<int> ListContratacionId = pComiteTecnico.SesionComiteSolicitudComiteTecnico.Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion).Select(r => r.SolicitudId).ToList();
                List<int> ListNovedadContractual = pComiteTecnico.SesionComiteSolicitudComiteTecnico.Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Novedad_Contractual).Select(r => r.SolicitudId).ToList();
                List<int> ListDefensaJudicialId = pComiteTecnico.SesionComiteSolicitudComiteTecnico.Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Defensa_judicial).Select(r => r.SolicitudId).ToList();

                List<Contratacion> ListContratacion = new List<Contratacion>();

                foreach (var ContratacionId in ListContratacionId)
                {
                    Contratacion contratacion = _context.Contratacion.Where(c => c.ContratacionId == ContratacionId)
                                   .Include(r => r.Contrato)
                                   .Include(r => r.Contratista)
                                   .Include(r => r.ContratacionProyecto)
                                       .ThenInclude(r => r.Proyecto).FirstOrDefault();

                    ListContratacion.Add(contratacion);
                }

                List<NovedadContractual> ListContratacionNovedadContractual = new List<NovedadContractual>();

                foreach (var novedadContractualId in ListNovedadContractual)
                {
                    ListContratacionNovedadContractual.Add(_context.NovedadContractual.Find(novedadContractualId));
                }

                List<ProcesoSeleccion> ListProcesoSeleccion = new List<ProcesoSeleccion>();

                foreach (var idProcesoSeleccion in ListProcesoSeleccionIdSolicitudId)
                {
                    ListProcesoSeleccion.Add(_context.ProcesoSeleccion.Find(idProcesoSeleccion));
                }

                List<DefensaJudicial> ListDefensaJudicial = new List<DefensaJudicial>();

                foreach (var DefensaJudicialId in ListDefensaJudicialId)
                {
                    ListDefensaJudicial.Add(_context.DefensaJudicial.Find(DefensaJudicialId));
                }

                List<Dominio> ListaParametricas = _context.Dominio.ToList();
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
                //Plantilla Novedades
                string PlantillaNovedades = _context.Plantilla
                    .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Tabla_Solicitud_Novedades)
                       .ToString()).FirstOrDefault()
                    .Contenido;

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

                if (pComiteTecnico.SesionComiteSolicitudComiteTecnico.Count() == 0)
                {
                    PlantillaSolicitudesContractuales = string.Empty;
                }
                else
                {
                    foreach (var SesionComiteSolicitud in pComiteTecnico.SesionComiteSolicitudComiteTecnico)
                    {
                        RegistrosSolicitudesContractuales += PlantillaRegistrosSolicitudesContractuales;
                        switch (SesionComiteSolicitud.TipoSolicitudCodigo)
                        {

                            case ConstanCodigoTipoSolicitud.Contratacion:
                                Contratacion contratacion = ListContratacion.Where(c => c.ContratacionId == SesionComiteSolicitud.SolicitudId).FirstOrDefault();

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
                            case ConstanCodigoTipoSolicitud.Evaluacion_De_Proceso:

                                ProcesoSeleccion procesoSeleccion = ListProcesoSeleccion.Where(r => r.ProcesoSeleccionId == SesionComiteSolicitud.SolicitudId).FirstOrDefault();

                                foreach (Dominio placeholderDominio in placeholders)
                                {
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

                            case ConstanCodigoTipoSolicitud.Novedad_Contractual:

                                NovedadContractual novedad = ListContratacionNovedadContractual.Where(c => c.NovedadContractualId == SesionComiteSolicitud.SolicitudId).FirstOrDefault();

                                foreach (Dominio placeholderDominio in placeholders)
                                {
                                    switch (placeholderDominio.Codigo)
                                    {
                                        case ConstanCodigoVariablesPlaceHolders.NUMERO_SOLICITUD:
                                            RegistrosSolicitudesContractuales = RegistrosSolicitudesContractuales
                                                .Replace(placeholderDominio.Nombre, novedad.NumeroSolicitud);
                                            break;

                                        case ConstanCodigoVariablesPlaceHolders.FECHA_SOLICITUD:
                                            string FechaSolicitud = string.Empty;
                                            if (novedad.FechaSolictud.HasValue)
                                            {
                                                FechaSolicitud = ((DateTime)novedad.FechaSolictud).ToString("dd-MM-yyy");
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
                            case ConstanCodigoTipoSolicitud.Defensa_judicial:
                                DefensaJudicial defensaJudicial = _context.DefensaJudicial.Find(SesionComiteSolicitud.SolicitudId);

                                foreach (Dominio placeholderDominio in placeholders)
                                {
                                    switch (placeholderDominio.Codigo)
                                    {
                                        case ConstanCodigoVariablesPlaceHolders.NUMERO_SOLICITUD:
                                            RegistrosSolicitudesContractuales = RegistrosSolicitudesContractuales
                                                .Replace(placeholderDominio.Nombre, defensaJudicial.NumeroProceso);
                                            break;

                                        case ConstanCodigoVariablesPlaceHolders.FECHA_SOLICITUD:
                                            string FechaSolicitud = string.Empty;
                                            if (defensaJudicial.FechaCreacion != null)
                                            {
                                                FechaSolicitud = ((DateTime)defensaJudicial.FechaCreacion).ToString("dd-MM-yyy");
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
                foreach (SesionComiteSolicitud SesionComiteSolicitud in pComiteTecnico.SesionComiteSolicitudComiteTecnico)
                {
                    string RegistrosProyectos = string.Empty;

                    string registrosCompromisosSolicitud = string.Empty;


                    switch (SesionComiteSolicitud.TipoSolicitudCodigo)
                    {
                        //TIPO SOLICITUD CONTRATACION
                        //TIPO SOLICITUD CONTRATACION
                        //TIPO SOLICITUD CONTRATACION
                        case ConstanCodigoTipoSolicitud.Contratacion:
                            registrosContratacion += PlantillaContratacion;
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

                                        StrTipoContrato = ListParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_por_contratar
                                       && r.Codigo == contratacion.TipoSolicitudCodigo).FirstOrDefault().Nombre;

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

                                    case ConstanCodigoVariablesPlaceHolders.DECISIONES_SOLICITUD:

                                        string strRequiereVotacion = "";
                                        int cantidadAprobadas = SesionComiteSolicitud.SesionSolicitudVoto.Where(r => r.Eliminado != true && r.EsAprobado.Value == true && r.ComiteTecnicoFiduciarioId == null).Count();
                                        int cantidadNoAprobadas = SesionComiteSolicitud.SesionSolicitudVoto.Where(r => r.Eliminado != true && r.EsAprobado.Value != true && r.ComiteTecnicoFiduciarioId == null).Count();
                                        if (SesionComiteSolicitud.RequiereVotacion == true)
                                        {
                                            if (cantidadAprobadas > cantidadNoAprobadas)
                                            {
                                                strRequiereVotacion = "Aprobada";
                                            }
                                            else
                                            {
                                                strRequiereVotacion = "No Aprobada";
                                            }
                                        }
                                        else
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

                                    case ConstanCodigoVariablesPlaceHolders.DESARROLLO_SOLICITUD:
                                        registrosContratacion = registrosContratacion
                                            .Replace(placeholderDominio.Nombre, SesionComiteSolicitud.DesarrolloSolicitud);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.RESULTADO_VOTACION:
                                        string TextoResultadoVotacion = "";

                                        if (
                                            SesionComiteSolicitud.RequiereVotacion != null &&
                                            SesionComiteSolicitud.RequiereVotacion.Value == true
                                            )
                                        {

                                            int cantidadAprobado = 0;
                                            int cantidadNoAprobado = 0;

                                            SesionComiteSolicitud.SesionSolicitudVoto.Where(v => v.Eliminado != true && v.ComiteTecnicoFiduciarioId == null).ToList().ForEach(ssv =>
                                            {
                                                if (ssv.EsAprobado == true)
                                                    cantidadAprobado++;
                                                else
                                                    cantidadNoAprobado++;
                                            });

                                            if (cantidadNoAprobado == 0)
                                            {
                                                TextoResultadoVotacion = PlantillaVotacionUnanime;
                                            }
                                            else if (cantidadAprobado > cantidadNoAprobado)
                                            {
                                                TextoResultadoVotacion = PlantillaNoVotacionUnanime;
                                            }

                                            TextoResultadoVotacion = TextoResultadoVotacion.Replace("[URL_SOPORTES_VOTO]", SesionComiteSolicitud.RutaSoporteVotacion);

                                        }

                                        registrosContratacion = registrosContratacion
                                        .Replace(placeholderDominio.Nombre, TextoResultadoVotacion);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.REGISTROS_COMPROMISOS_SOLICITUD:

                                        registrosCompromisosSolicitud = string.Empty;
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
                                                            .Replace(placeholderDominio3.Nombre, compromiso?.ResponsableSesionParticipante?.Usuario?.PrimerNombre
                                                            + " " + compromiso?.ResponsableSesionParticipante?.Usuario.PrimerApellido);
                                                        break;

                                                    case ConstanCodigoVariablesPlaceHolders.FECHA_CUMPLIMIENTO_COMPROMISO:

                                                        registrosCompromisosSolicitud = registrosCompromisosSolicitud
                                                            .Replace(placeholderDominio3.Nombre, compromiso.FechaCumplimiento.HasValue ? compromiso.FechaCumplimiento.Value.ToString("dd-MM-yyyy") : "");
                                                        break;
                                                }
                                            }
                                        }

                                        registrosContratacion = registrosContratacion
                                        .Replace(placeholderDominio.Nombre, string.IsNullOrEmpty(registrosCompromisosSolicitud) ? msg.Replace("[MSG]", "No se tienen <strong>compromisos</strong> para esta solicitud") : registrosCompromisosSolicitud);
                                        break;

                                }
                            }
                            break;

                        //TIPO SOLICITUD PROCESOS DE SELECCION
                        case ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion:
                        case ConstanCodigoTipoSolicitud.Evaluacion_De_Proceso:
                            registrosContratacion += PlantillaProcesosSelecccion;
                            ProcesoSeleccion procesoSeleccion = ListProcesoSeleccion.Where(r => r.ProcesoSeleccionId == SesionComiteSolicitud.SolicitudId).FirstOrDefault();

                            foreach (Dominio placeholderDominio in placeholders)
                            {

                                switch (placeholderDominio.Codigo)
                                {
                                    case ConstanCodigoVariablesPlaceHolders.NUMERO_SOLICITUD:
                                        registrosContratacion = registrosContratacion
                                            .Replace(placeholderDominio.Nombre, enumOrdenDelDia++.ToString());
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.TIPO_SOLICITUD:
                                        registrosContratacion = registrosContratacion
                                            .Replace(placeholderDominio.Nombre,
                                            ListParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud
                                            && r.Codigo == SesionComiteSolicitud.TipoSolicitudCodigo).FirstOrDefault().Nombre);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.FECHA_SOLICITUD:
                                        string FechaSolicitud = string.Empty;
                                        FechaSolicitud = procesoSeleccion.FechaCreacion.ToString("dd-MM-yyyy");

                                        registrosContratacion = registrosContratacion
                                            .Replace(placeholderDominio.Nombre, FechaSolicitud);
                                        break;


                                    case ConstanCodigoVariablesPlaceHolders.NUMERO_SOLICITUD_PROCESO:
                                        registrosContratacion = registrosContratacion
                                          .Replace(placeholderDominio.Nombre, procesoSeleccion.NumeroProceso);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.TIPO_PROCESO:
                                        registrosContratacion = registrosContratacion
                                          .Replace(placeholderDominio.Nombre,
                                          ListParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Proceso_Seleccion
                                          && r.Codigo == procesoSeleccion.TipoProcesoCodigo).FirstOrDefault().Nombre);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.OBJETO_PROCESO:
                                        registrosContratacion = registrosContratacion
                                          .Replace(placeholderDominio.Nombre, procesoSeleccion.Objeto);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.ALCANCE_PARTICULAR_PROCESO:
                                        registrosContratacion = registrosContratacion
                                          .Replace(placeholderDominio.Nombre, procesoSeleccion.AlcanceParticular);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.JUSTIFICACION_PROCESO:
                                        registrosContratacion = registrosContratacion
                                          .Replace(placeholderDominio.Nombre, procesoSeleccion.Justificacion);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.TIPO_INTERVENCION_PROCESO:

                                        registrosContratacion = registrosContratacion
                                        .Replace(placeholderDominio.Nombre,
                                        ListParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion
                                        && r.Codigo == procesoSeleccion.TipoIntervencionCodigo).FirstOrDefault().Nombre);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.TIPO_ALCANCE_PROCESO:
                                        registrosContratacion = registrosContratacion
                                        .Replace(placeholderDominio.Nombre,
                                        ListParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_alcance
                                        && r.Codigo == procesoSeleccion.TipoAlcanceCodigo).FirstOrDefault().Nombre);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.DESARROLLO_SOLICITUD:
                                        registrosContratacion = registrosContratacion
                                            .Replace(placeholderDominio.Nombre, SesionComiteSolicitud.DesarrolloSolicitud);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.DECISIONES_SOLICITUD:

                                        string strRequiereVotacion = "";
                                        int cantidadAprobadas = SesionComiteSolicitud.SesionSolicitudVoto.Where(r => r.Eliminado != true && r.EsAprobado.Value == true && r.ComiteTecnicoFiduciarioId == null).Count();
                                        int cantidadNoAprobadas = SesionComiteSolicitud.SesionSolicitudVoto.Where(r => r.Eliminado != true && r.EsAprobado.Value != true && r.ComiteTecnicoFiduciarioId == null).Count();
                                        if (SesionComiteSolicitud.RequiereVotacion == true)
                                        {
                                            if (cantidadAprobadas > cantidadNoAprobadas)
                                            {
                                                strRequiereVotacion = "Aprobada";
                                            }
                                            else
                                            {
                                                strRequiereVotacion = "No Aprobada";
                                            }
                                        }
                                        else
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

                                        if (
                                            SesionComiteSolicitud.RequiereVotacion != null &&
                                            SesionComiteSolicitud.RequiereVotacion.Value == true
                                            )
                                        {

                                            int cantidadAprobado = 0;
                                            int cantidadNoAprobado = 0;

                                            SesionComiteSolicitud.SesionSolicitudVoto.Where(v => v.Eliminado != true && v.ComiteTecnicoFiduciarioId == null).ToList().ForEach(ssv =>
                                            {
                                                if (ssv.EsAprobado == true)
                                                    cantidadAprobado++;
                                                else
                                                    cantidadNoAprobado++;
                                            });

                                            if (cantidadNoAprobado == 0)
                                            {
                                                TextoResultadoVotacion = PlantillaVotacionUnanime;
                                            }
                                            else if (cantidadAprobado > cantidadNoAprobado)
                                            {
                                                TextoResultadoVotacion = PlantillaNoVotacionUnanime;
                                            }

                                            TextoResultadoVotacion = TextoResultadoVotacion.Replace("[URL_SOPORTES_VOTO]", SesionComiteSolicitud.RutaSoporteVotacion);

                                        }

                                        registrosContratacion = registrosContratacion
                                        .Replace(placeholderDominio.Nombre, TextoResultadoVotacion);




                                        //if (SesionComiteSolicitud.RequiereVotacion == null || !(bool)SesionComiteSolicitud.RequiereVotacion)
                                        //{
                                        //    TextoResultadoVotacion = _context.Plantilla.Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Votacion_No_Unanime).ToString()).FirstOrDefault().Nombre;
                                        //}
                                        //else
                                        //{
                                        //    TextoResultadoVotacion = _context.Plantilla.Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Votacion_Unanime).ToString()).FirstOrDefault().Nombre;
                                        //}
                                        //registrosContratacion = registrosContratacion
                                        //.Replace(placeholderDominio.Nombre, TextoResultadoVotacion);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.REGISTROS_COMPROMISOS_SOLICITUD:

                                        registrosCompromisosSolicitud = string.Empty;
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
                                                            .Replace(placeholderDominio3.Nombre, compromiso?.ResponsableSesionParticipante?.Usuario?.PrimerNombre
                                                            + " " + compromiso.ResponsableSesionParticipante?.Usuario?.PrimerApellido);
                                                        break;

                                                    case ConstanCodigoVariablesPlaceHolders.FECHA_CUMPLIMIENTO_COMPROMISO:
                                                        registrosCompromisosSolicitud = registrosCompromisosSolicitud
                                                            .Replace(placeholderDominio3.Nombre, compromiso.FechaCumplimiento.HasValue ? compromiso.FechaCumplimiento.Value.ToString("dd-MM-yyyy") : "");
                                                        break;
                                                }
                                            }
                                        }

                                        registrosContratacion = registrosContratacion
                                        .Replace(placeholderDominio.Nombre, string.IsNullOrEmpty(registrosCompromisosSolicitud) ? msg.Replace("[MSG]", "No se tienen <strong>compromisos</strong> para esta solicitud") : registrosCompromisosSolicitud);
                                        break;

                                }
                            }
                            break;

                        //TIPO Novedad contractual
                        case ConstanCodigoTipoSolicitud.Novedad_Contractual:
                            registrosContratacion += PlantillaNovedades;

                            NovedadContractual novedad = await _IContractualNoveltyService.GetNovedadContractualById(SesionComiteSolicitud.SolicitudId);


                            string tipoNovedadString = string.Empty;

                            foreach (var item in novedad.NovedadContractualDescripcion)
                            {
                                if (item.Eliminado == null || item.Eliminado == false)
                                {
                                    string codigotipoNovedadTemp = item.TipoNovedadCodigo;
                                    string tipoNovedadTemp = ListaParametricas.Where(r => r.Codigo == item.TipoNovedadCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Novedad_Modificacion_Contractual).FirstOrDefault().Nombre;

                                    if (String.IsNullOrEmpty(tipoNovedadString))
                                    {
                                        tipoNovedadString = tipoNovedadTemp;
                                    }
                                    else
                                    {
                                        tipoNovedadString = tipoNovedadString + ", " + tipoNovedadTemp;
                                    }
                                }
                            }

                            Contrato contrato = novedad != null ? novedad.Contrato : null;

                            Contratacion contratacionNovedad = null;
                            DisponibilidadPresupuestal disponibilidadPresupuestal = null;

                            Contratista contratista = null;

                            if (contrato != null)
                            {
                                contratacionNovedad = await _IProjectContractingService.GetAllContratacionByContratacionId(contrato.ContratacionId);
                            }
                            if (contratacionNovedad != null)
                            {
                                contratista = _context.Contratista
                                    .Where(r => r.ContratistaId == contratacionNovedad.ContratistaId).FirstOrDefault();
                                disponibilidadPresupuestal = _context.DisponibilidadPresupuestal
                                    .Where(r => r.ContratacionId == contratacionNovedad.ContratacionId).FirstOrDefault();
                            }

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
                                            .Replace(placeholderDominio.Nombre, novedad.NumeroSolicitud);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.CANTIDAD_DE_PROYECTOS_ASOCIADOS:
                                        registrosContratacion = registrosContratacion
                                            .Replace(placeholderDominio.Nombre, contratacionNovedad.ContratacionProyecto.Where(R => !(bool)R.Eliminado).Count().ToString());
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.FECHA_SOLICITUD_CONTRATACION:
                                        string FechaSolicitud = string.Empty;
                                        if (contratacionNovedad.FechaTramite.HasValue)
                                        {
                                            FechaSolicitud = ((DateTime)contratacionNovedad.FechaTramite).ToString("dd-MM-yyy");
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

                                        StrTipoContrato = ListParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_por_contratar
                                       && r.Codigo == contratacionNovedad.TipoSolicitudCodigo).FirstOrDefault().Nombre;

                                        registrosContratacion = registrosContratacion
                                            .Replace(placeholderDominio.Nombre, StrTipoContrato
                                           );
                                        break;
                                    //Datos Contratista y contrato

                                    case ConstanCodigoVariablesPlaceHolders.NUMERO_CONTRATO:
                                        registrosContratacion = registrosContratacion.Replace(placeholderDominio.Nombre, contrato != null ? contrato.NumeroContrato : "");
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.TIPO_CONTROVERSIA:
                                        registrosContratacion = registrosContratacion.Replace(placeholderDominio.Nombre, tipoNovedadString);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.CONTRATISTA_NOMBRE:
                                        registrosContratacion = registrosContratacion.Replace(placeholderDominio.Nombre, contratista != null ? contratista.Nombre : "");
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.FECHA_INICIO_CONTRATO:
                                        registrosContratacion = registrosContratacion.Replace(placeholderDominio.Nombre, contrato != null ? contrato.FechaActaInicioFase2.HasValue ? ((DateTime)contrato.FechaActaInicioFase2).ToString("dd-MM-yyyy") : contrato.FechaActaInicioFase1.HasValue ? ((DateTime)contrato.FechaActaInicioFase1).ToString("dd-MM-yyyy") : " " : " ");
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.FECHA_FIN_CONTRATO:
                                        registrosContratacion = registrosContratacion.Replace(placeholderDominio.Nombre, contrato != null ? contrato.FechaTerminacionFase2.HasValue ? ((DateTime)contrato.FechaTerminacionFase2).ToString("dd-MM-yyyy") : contrato.FechaTerminacion.HasValue ? ((DateTime)contrato.FechaTerminacion).ToString("dd-MM-yyyy") : " " : " ");
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.PLAZO_OBRA_MESES:
                                        registrosContratacion = registrosContratacion.Replace(placeholderDominio.Nombre, contratacionNovedad != null ? contratacionNovedad.PlazoContratacion.PlazoMeses + " Meses " + contratacionNovedad.PlazoContratacion.PlazoDias + " Días " : " ");
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.REGISTROS_TABLA_PROYECTO:

                                        foreach (var ContratacionProyecto in contratacionNovedad.ContratacionProyecto.Where(r => !(bool)r.Eliminado))
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

                                    case ConstanCodigoVariablesPlaceHolders.DECISIONES_SOLICITUD:

                                        string strRequiereVotacion = "";
                                        int cantidadAprobadas = SesionComiteSolicitud.SesionSolicitudVoto.Where(r => r.Eliminado != true && r.EsAprobado.Value == true && r.ComiteTecnicoFiduciarioId == null).Count();
                                        int cantidadNoAprobadas = SesionComiteSolicitud.SesionSolicitudVoto.Where(r => r.Eliminado != true && r.EsAprobado.Value != true && r.ComiteTecnicoFiduciarioId == null).Count();
                                        if (SesionComiteSolicitud.RequiereVotacion == true)
                                        {
                                            if (cantidadAprobadas > cantidadNoAprobadas)
                                            {
                                                strRequiereVotacion = "Aprobada";
                                            }
                                            else
                                            {
                                                strRequiereVotacion = "No Aprobada";
                                            }
                                        }
                                        else
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

                                    case ConstanCodigoVariablesPlaceHolders.DESARROLLO_SOLICITUD:
                                        registrosContratacion = registrosContratacion
                                            .Replace(placeholderDominio.Nombre, SesionComiteSolicitud.DesarrolloSolicitud);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.RESULTADO_VOTACION:
                                        string TextoResultadoVotacion = "";

                                        if (
                                            SesionComiteSolicitud.RequiereVotacion != null &&
                                            SesionComiteSolicitud.RequiereVotacion.Value == true
                                            )
                                        {

                                            int cantidadAprobado = 0;
                                            int cantidadNoAprobado = 0;

                                            SesionComiteSolicitud.SesionSolicitudVoto.Where(v => v.Eliminado != true && v.ComiteTecnicoFiduciarioId == null).ToList().ForEach(ssv =>
                                            {
                                                if (ssv.EsAprobado == true)
                                                    cantidadAprobado++;
                                                else
                                                    cantidadNoAprobado++;
                                            });

                                            if (cantidadNoAprobado == 0)
                                            {
                                                TextoResultadoVotacion = PlantillaVotacionUnanime;
                                            }
                                            else if (cantidadAprobado > cantidadNoAprobado)
                                            {
                                                TextoResultadoVotacion = PlantillaNoVotacionUnanime;
                                            }

                                            TextoResultadoVotacion = TextoResultadoVotacion.Replace("[URL_SOPORTES_VOTO]", SesionComiteSolicitud.RutaSoporteVotacion);

                                        }

                                        registrosContratacion = registrosContratacion
                                        .Replace(placeholderDominio.Nombre, TextoResultadoVotacion);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.REGISTROS_COMPROMISOS_SOLICITUD:

                                        registrosCompromisosSolicitud = string.Empty;
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
                                                            .Replace(placeholderDominio3.Nombre, compromiso?.ResponsableSesionParticipante?.Usuario?.PrimerNombre
                                                            + " " + compromiso?.ResponsableSesionParticipante?.Usuario.PrimerApellido);
                                                        break;

                                                    case ConstanCodigoVariablesPlaceHolders.FECHA_CUMPLIMIENTO_COMPROMISO:

                                                        registrosCompromisosSolicitud = registrosCompromisosSolicitud
                                                            .Replace(placeholderDominio3.Nombre, compromiso.FechaCumplimiento.HasValue ? compromiso.FechaCumplimiento.Value.ToString("dd-MM-yyyy") : "");
                                                        break;
                                                }
                                            }
                                        }

                                        registrosContratacion = registrosContratacion
                                        .Replace(placeholderDominio.Nombre, string.IsNullOrEmpty(registrosCompromisosSolicitud) ? msg.Replace("[MSG]", "No se tienen <strong>compromisos</strong> para esta solicitud") : registrosCompromisosSolicitud);
                                        break;

                                }
                            }
                            break;
                        //TIPO DE SOLIDITUD-  DEFENSA JUDICIAL
                        case ConstanCodigoTipoSolicitud.Defensa_judicial:
                            registrosContratacion += PlantillaContratacion;
                            DefensaJudicial defensaJudicial = await _judicialDefense.GetVistaDatosBasicosProceso(SesionComiteSolicitud.SolicitudId);
                            ContratacionProyecto contratacionProyectoDf = null;

                            Contratacion contratacionDefensaJudicial = null;
                            DisponibilidadPresupuestal disponibilidadPresupuestalDefensaJudicial = null;
                            Contratista contratistaDf = null;

                            if (defensaJudicial.DefensaJudicialContratacionProyecto != null)
                            {
                                contratacionProyectoDf = _context.ContratacionProyecto.Where(r => r.ContratacionProyectoId == defensaJudicial.DefensaJudicialContratacionProyecto.FirstOrDefault().ContratacionProyectoId).FirstOrDefault();

                            }
                            if (contratacionProyectoDf != null)
                            {
                                contratacionDefensaJudicial = await _IProjectContractingService.GetAllContratacionByContratacionId(contratacionProyectoDf.ContratacionId);

                            }
                            if (contratacionDefensaJudicial != null)
                            {
                                contratistaDf = _context.Contratista
                                    .Where(r => r.ContratistaId == contratacionDefensaJudicial.ContratistaId).FirstOrDefault();
                                disponibilidadPresupuestalDefensaJudicial = _context.DisponibilidadPresupuestal
                                    .Where(r => r.ContratacionId == contratacionDefensaJudicial.ContratacionId).FirstOrDefault();
                            }

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
                                            .Replace(placeholderDominio.Nombre, defensaJudicial.NumeroProceso);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.CANTIDAD_DE_PROYECTOS_ASOCIADOS:
                                        registrosContratacion = registrosContratacion
                                            .Replace(placeholderDominio.Nombre, defensaJudicial.DefensaJudicialContratacionProyecto.Where(R => !(bool)R.Eliminado).Count().ToString());
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.FECHA_SOLICITUD_CONTRATACION:
                                        string FechaSolicitud = string.Empty;
                                        if (defensaJudicial.FechaCreacion != null)
                                        {
                                            FechaSolicitud = ((DateTime)defensaJudicial.FechaCreacion).ToString("dd-MM-yyy");
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

                                        StrTipoContrato = ListParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_por_contratar
                                       && r.Codigo == (contratacionDefensaJudicial != null ? contratacionDefensaJudicial.TipoSolicitudCodigo : string.Empty)).FirstOrDefault().Nombre;

                                        registrosContratacion = registrosContratacion
                                            .Replace(placeholderDominio.Nombre, StrTipoContrato
                                           );
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.REGISTROS_TABLA_PROYECTO:

                                        foreach (var ContratacionProyecto in contratacionDefensaJudicial.ContratacionProyecto.Where(r => !(bool)r.Eliminado))
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

                                    case ConstanCodigoVariablesPlaceHolders.DECISIONES_SOLICITUD:

                                        string strRequiereVotacion = "";
                                        int cantidadAprobadas = SesionComiteSolicitud.SesionSolicitudVoto.Where(r => r.Eliminado != true && r.EsAprobado.Value == true && r.ComiteTecnicoFiduciarioId == null).Count();
                                        int cantidadNoAprobadas = SesionComiteSolicitud.SesionSolicitudVoto.Where(r => r.Eliminado != true && r.EsAprobado.Value != true && r.ComiteTecnicoFiduciarioId == null).Count();
                                        if (SesionComiteSolicitud.RequiereVotacion == true)
                                        {
                                            if (cantidadAprobadas > cantidadNoAprobadas)
                                            {
                                                strRequiereVotacion = "Aprobada";
                                            }
                                            else
                                            {
                                                strRequiereVotacion = "No Aprobada";
                                            }
                                        }
                                        else
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

                                    case ConstanCodigoVariablesPlaceHolders.DESARROLLO_SOLICITUD:
                                        registrosContratacion = registrosContratacion
                                            .Replace(placeholderDominio.Nombre, SesionComiteSolicitud.DesarrolloSolicitud);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.RESULTADO_VOTACION:
                                        string TextoResultadoVotacion = "";

                                        if (
                                            SesionComiteSolicitud.RequiereVotacion != null &&
                                            SesionComiteSolicitud.RequiereVotacion.Value == true
                                            )
                                        {

                                            int cantidadAprobado = 0;
                                            int cantidadNoAprobado = 0;

                                            SesionComiteSolicitud.SesionSolicitudVoto.Where(v => v.Eliminado != true && v.ComiteTecnicoFiduciarioId == null).ToList().ForEach(ssv =>
                                            {
                                                if (ssv.EsAprobado == true)
                                                    cantidadAprobado++;
                                                else
                                                    cantidadNoAprobado++;
                                            });

                                            if (cantidadNoAprobado == 0)
                                            {
                                                TextoResultadoVotacion = PlantillaVotacionUnanime;
                                            }
                                            else if (cantidadAprobado > cantidadNoAprobado)
                                            {
                                                TextoResultadoVotacion = PlantillaNoVotacionUnanime;
                                            }

                                            TextoResultadoVotacion = TextoResultadoVotacion.Replace("[URL_SOPORTES_VOTO]", SesionComiteSolicitud.RutaSoporteVotacion);

                                        }

                                        registrosContratacion = registrosContratacion
                                        .Replace(placeholderDominio.Nombre, TextoResultadoVotacion);
                                        break;

                                    case ConstanCodigoVariablesPlaceHolders.REGISTROS_COMPROMISOS_SOLICITUD:

                                        registrosCompromisosSolicitud = string.Empty;
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
                                                            .Replace(placeholderDominio3.Nombre, compromiso?.ResponsableSesionParticipante?.Usuario?.PrimerNombre
                                                            + " " + compromiso?.ResponsableSesionParticipante?.Usuario.PrimerApellido);
                                                        break;

                                                    case ConstanCodigoVariablesPlaceHolders.FECHA_CUMPLIMIENTO_COMPROMISO:

                                                        registrosCompromisosSolicitud = registrosCompromisosSolicitud
                                                            .Replace(placeholderDominio3.Nombre, compromiso.FechaCumplimiento.HasValue ? compromiso.FechaCumplimiento.Value.ToString("dd-MM-yyyy") : "");
                                                        break;
                                                }
                                            }
                                        }

                                        registrosContratacion = registrosContratacion
                                        .Replace(placeholderDominio.Nombre, string.IsNullOrEmpty(registrosCompromisosSolicitud) ? msg.Replace("[MSG]", "No se tienen <strong>compromisos</strong> para esta solicitud") : registrosCompromisosSolicitud);
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

                            case ConstanCodigoVariablesPlaceHolders.DECISIONES_SOLICITUD:

                                string strRequiereVotacion = "";
                                int cantidadAprobadas = Tema.SesionTemaVoto.Where(r => r.Eliminado != true && r.EsAprobado.Value == true).Count();
                                int cantidadNoAprobadas = Tema.SesionTemaVoto.Where(r => r.Eliminado != true && r.EsAprobado.Value != true).Count();
                                if (Tema.RequiereVotacion == true)
                                {
                                    if (cantidadAprobadas > cantidadNoAprobadas)
                                    {
                                        strRequiereVotacion = "Aprobada";
                                    }
                                    else
                                    {
                                        strRequiereVotacion = "No Aprobada";
                                    }
                                }
                                else
                                {
                                    strRequiereVotacion = "No fue requerida";
                                }
                                RegistrosNuevosTemas = RegistrosNuevosTemas
                                   .Replace(placeholderDominio.Nombre, strRequiereVotacion);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.RESULTADO_VOTACION:
                                string TextoResultadoVotacion = "";
                                int cantidadAprobado = Tema.SesionTemaVoto.Where(r => r.Eliminado != true && r.EsAprobado.Value == true).Count();
                                int cantidadNoAprobo = Tema.SesionTemaVoto.Where(r => r.Eliminado != true && r.EsAprobado.Value != true).Count();

                                if (cantidadNoAprobo == 0)
                                {
                                    TextoResultadoVotacion = PlantillaVotacionUnanime;
                                }
                                else if (cantidadAprobado > cantidadNoAprobo)
                                {
                                    TextoResultadoVotacion = PlantillaNoVotacionUnanime;
                                }

                                TextoResultadoVotacion = TextoResultadoVotacion.Replace("[URL_SOPORTES_VOTO]", Tema.RutaSoporte);

                                RegistrosNuevosTemas = RegistrosNuevosTemas
                                .Replace(placeholderDominio.Nombre, TextoResultadoVotacion);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.OBSERVACIONES_SOLICITUD:
                                RegistrosNuevosTemas = RegistrosNuevosTemas
                                   .Replace(placeholderDominio.Nombre, Tema.Observaciones);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.REGISTROS_TEMAS:

                                registrosCompromisosSolicitud = string.Empty;

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
                                                    .Replace(placeholderDominio4.Nombre, compromiso.ResponsableNavigation.Usuario.PrimerNombre
                                                    + " " + compromiso.ResponsableNavigation.Usuario.PrimerApellido);
                                                break;

                                            case ConstanCodigoVariablesPlaceHolders.FECHA_CUMPLIMIENTO_COMPROMISO:
                                                registrosCompromisosSolicitud = registrosCompromisosSolicitud
                                                    .Replace(placeholderDominio4.Nombre, compromiso.FechaCumplimiento.Value.ToString("dd-MM-yyyy"));
                                                break;
                                        }
                                    }
                                }
                                RegistrosNuevosTemas = RegistrosNuevosTemas
                                .Replace(placeholderDominio.Nombre, string.IsNullOrEmpty(registrosCompromisosSolicitud) ? msg.Replace("[MSG]", "No se tienen <strong>nuevos temas</strong> para esta solicitud") : registrosCompromisosSolicitud);
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

                            case ConstanCodigoVariablesPlaceHolders.DECISIONES_SOLICITUD:

                                string strRequiereVotacion = "";
                                int cantidadAprobadas = Tema.SesionTemaVoto.Where(r => r.Eliminado != true && r.EsAprobado.Value == true).Count();
                                int cantidadNoAprobadas = Tema.SesionTemaVoto.Where(r => r.Eliminado != true && r.EsAprobado.Value != true).Count();
                                if (Tema.RequiereVotacion == true)
                                {
                                    if (cantidadAprobadas > cantidadNoAprobadas)
                                    {
                                        strRequiereVotacion = "Aprobada";
                                    }
                                    else
                                    {
                                        strRequiereVotacion = "No Aprobada";
                                    }
                                }
                                else
                                {
                                    strRequiereVotacion = "No fue requerida";
                                }
                                RegistrosProposicionVarios = RegistrosProposicionVarios
                                   .Replace(placeholderDominio.Nombre, strRequiereVotacion);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.RESULTADO_VOTACION:
                                string TextoResultadoVotacion = "";
                                int cantidadAprobado = Tema.SesionTemaVoto.Where(r => r.Eliminado != true && r.EsAprobado.Value == true).Count();
                                int cantidadNoAprobo = Tema.SesionTemaVoto.Where(r => r.Eliminado != true && r.EsAprobado.Value != true).Count();

                                if (cantidadNoAprobo == 0)
                                {
                                    TextoResultadoVotacion = PlantillaVotacionUnanime;
                                }
                                else if (cantidadAprobado > cantidadNoAprobo)
                                {
                                    TextoResultadoVotacion = PlantillaNoVotacionUnanime;
                                }

                                TextoResultadoVotacion = TextoResultadoVotacion.Replace("[URL_SOPORTES_VOTO]", Tema.RutaSoporte);

                                RegistrosProposicionVarios = RegistrosProposicionVarios
                                .Replace(placeholderDominio.Nombre, TextoResultadoVotacion);
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
                                                    .Replace(placeholderDominio5.Nombre, compromiso?.ResponsableNavigation?.Usuario?.PrimerNombre
                                                    + " " + compromiso.ResponsableNavigation?.Usuario?.PrimerApellido);
                                                break;

                                            case ConstanCodigoVariablesPlaceHolders.FECHA_CUMPLIMIENTO_COMPROMISO:
                                                registrosCompromisosSolicitud = registrosCompromisosSolicitud
                                                    .Replace(placeholderDominio5.Nombre, compromiso.FechaCumplimiento.HasValue ? compromiso.FechaCumplimiento.Value.ToString("dd-MM-yyyy") : "");
                                                break;
                                        }
                                    }
                                }

                                RegistrosProposicionVarios = RegistrosProposicionVarios
                                .Replace(placeholderDominio.Nombre, string.IsNullOrEmpty(registrosCompromisosSolicitud) ? msg.Replace("[MSG]", "No se tienen <strong>compromisos</strong> para esta solicitud") : registrosCompromisosSolicitud);
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
                                    .Replace(placeholderDominio.Nombre, SesionParticipante.Usuario.PrimerNombre + " " + SesionParticipante.Usuario.PrimerApellido);
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

                string PlantillaNovedadContractual = _context.Plantilla
                 .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Ficha_novedad_contractual)
                    .ToString()).FirstOrDefault()
                 .Contenido;

                string PlantillaDefensaJudicial = _context.Plantilla
                 .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Ficha_Estudio_Defensa_Judicial)
                    .ToString()).FirstOrDefault()
                 .Contenido;

                string RegistrosFichaNovedadContractual = string.Empty;

                string RegistrosFichaProcesosSeleccion = string.Empty;

                string RegistrosDefensaJudicial = string.Empty;

                foreach (var scst in pComiteTecnico.SesionComiteSolicitudComiteTecnico)
                {
                    switch (scst.TipoSolicitudCodigo)
                    {
                        case ConstanCodigoTipoSolicitud.Contratacion:
                            RegistrosFichaContratacion += PlantillaFichaContratacion;
                            RegistrosFichaContratacion = ReemplazarDatosPlantillaContratacion(RegistrosFichaContratacion, await _IProjectContractingService.GetAllContratacionByContratacionId(scst.SolicitudId));
                            break;

                        case ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion:
                        case ConstanCodigoTipoSolicitud.Evaluacion_De_Proceso:
                            RegistrosFichaProcesosSeleccion += PlantillaFichaProcesosSeleccion;
                            RegistrosFichaProcesosSeleccion = ReemplazarDatosPlantillaProcesosSeleccion(RegistrosFichaProcesosSeleccion, await GetProcesosSelecccionByProcesoSeleccionId(scst.SolicitudId));
                            break;

                        case ConstanCodigoTipoSolicitud.Novedad_Contractual:
                            RegistrosFichaNovedadContractual += PlantillaNovedadContractual;
                            RegistrosFichaNovedadContractual = await ReemplazarDatosPlantillaNovedadContractual(RegistrosFichaNovedadContractual, await _IContractualNoveltyService.GetNovedadContractualById(scst.SolicitudId));
                            break;

                        case ConstanCodigoTipoSolicitud.Defensa_judicial:
                            RegistrosDefensaJudicial += PlantillaDefensaJudicial;
                            RegistrosDefensaJudicial = await _judicialDefense.ReemplazarDatosPlantillaDefensaJudicial(RegistrosDefensaJudicial, scst.SolicitudId, 2);
                            break;

                        default:
                            break;
                    }

                }
                //Suma de las fichas 
                RegistrosFichaContratacion += RegistrosFichaProcesosSeleccion;
                RegistrosFichaContratacion += RegistrosFichaNovedadContractual;
                RegistrosFichaContratacion += RegistrosDefensaJudicial;

                //Plantilla Principal 
                foreach (Dominio placeholderDominio in placeholders)
                {
                    switch (placeholderDominio.Codigo)
                    {
                        //Tablas dinamicas  
                        case ConstanCodigoVariablesPlaceHolders.TITULO_SOLICITUDES_CONTRACTUALES:
                            string strTituloSolicitudesContractuales = string.Empty;
                            if (pComiteTecnico.SesionComiteSolicitudComiteTecnico.Count() > 0)
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
                                .Replace(placeholderDominio.Nombre, string.IsNullOrEmpty(RegistrosNuevosTemas) ? msg.Replace("[MSG]", "No se trabajaron <strong>Nuevos temas</strong> para esta solicitud.") : RegistrosNuevosTemas);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.REGISTROS_PROPOSICIONES_VARIOS:
                            strContenido = strContenido
                                .Replace(placeholderDominio.Nombre, string.IsNullOrEmpty(RegistrosProposicionVarios) ? msg.Replace("[MSG]", "No se tienen <strong>proposiciones y varios</strong> para esta solicitud.") : RegistrosProposicionVarios);
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
                                strUsuariosParticipantes += user.Usuario.PrimerNombre + " " + user.Usuario.PrimerApellido + " ";
                            });
                            strContenido = strContenido.Replace(placeholderDominio.Nombre, strUsuariosParticipantes);
                            break;
                    }
                }

                return strContenido;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<dynamic> ListMonitoreo(bool EsFiduciario)
        {
            List<ComiteTecnico> comiteTecnicos =
                await _context.ComiteTecnico
                .Where(r => !(bool)r.Eliminado && r.EsComiteFiduciario == EsFiduciario)
                .Where(r => r.EstadoActaCodigo == ConstantCodigoActas.Aprobada
                       && r.EstadoComiteCodigo == ConstanCodigoEstadoComite.Con_Acta_De_Sesion_Aprobada)
                .Include(r => r.SesionParticipante)
                .Include(r => r.SesionComentario)
                .Include(r => r.SesionComiteSolicitudComiteTecnico)
                      .ThenInclude(r => r.SesionSolicitudCompromiso)
                .Include(r => r.SesionComiteSolicitudComiteTecnicoFiduciario)
                     .ThenInclude(r => r.SesionSolicitudCompromiso)
                .Include(r => r.SesionComiteTema)
                    .ThenInclude(r => r.TemaCompromiso).ToListAsync();

            foreach (var comite in comiteTecnicos.Where(r => !(bool)r.Eliminado))
            {
                foreach (var SesionComiteSolicitudComiteTecnico in comite.SesionComiteSolicitudComiteTecnico.Where(r => !(bool)r.Eliminado))
                {
                    if (EsFiduciario)
                        SesionComiteSolicitudComiteTecnico.SesionSolicitudCompromiso = SesionComiteSolicitudComiteTecnico.SesionSolicitudCompromiso.Where(r => (bool)r.EsFiduciario.HasValue && !(bool)r.Eliminado
                        ).ToList();
                    else
                        SesionComiteSolicitudComiteTecnico.SesionSolicitudCompromiso = SesionComiteSolicitudComiteTecnico.SesionSolicitudCompromiso.Where(r => !(bool)r.EsFiduciario.HasValue && !(bool)r.Eliminado).ToList();
                }
                foreach (var SesionComiteSolicitudComiteTecnico in comite.SesionComiteSolicitudComiteTecnicoFiduciario.Where(r => !(bool)r.Eliminado))
                {
                    if (EsFiduciario)
                        SesionComiteSolicitudComiteTecnico.SesionSolicitudCompromiso = SesionComiteSolicitudComiteTecnico.SesionSolicitudCompromiso.Where(r => (bool)r.EsFiduciario.HasValue && !(bool)r.Eliminado).ToList();
                    else
                        SesionComiteSolicitudComiteTecnico.SesionSolicitudCompromiso = SesionComiteSolicitudComiteTecnico.SesionSolicitudCompromiso.Where(r => !(bool)r.EsFiduciario.HasValue && !(bool)r.Eliminado).ToList();
                }
            }

            List<ListCompromisos> ListCompromisos = new List<ListCompromisos>();

            foreach (var ComiteTecnico in comiteTecnicos.Where(r => !(bool)r.Eliminado).OrderByDescending(r => r.ComiteTecnicoId))
            {
                foreach (var SesionComiteSolicitudComiteTecnico in ComiteTecnico.SesionComiteSolicitudComiteTecnico.Where(r => !(bool)r.Eliminado).OrderByDescending(r => r.SesionComiteSolicitudId))
                {
                    foreach (var SesionSolicitudCompromiso in SesionComiteSolicitudComiteTecnico.SesionSolicitudCompromiso.Where(r => !(bool)r.Eliminado).OrderByDescending(r => r.SesionSolicitudCompromisoId))
                    {
                        ListCompromisos.Add(new ListCompromisos
                        {
                            FechaComite = ComiteTecnico.FechaOrdenDia,
                            NumeroComite = ComiteTecnico.NumeroComite,
                            Compromiso = SesionSolicitudCompromiso.Tarea,
                            EstadoCodigo = string.IsNullOrEmpty(SesionSolicitudCompromiso.EstadoCodigo) ? ConstanStringCodigoCompromisos.Sin_avance : SesionSolicitudCompromiso.EstadoCodigo,
                            FechaCumplimiento = ((DateTime)SesionSolicitudCompromiso.FechaCumplimiento).ToString("dd-MMMM-YY"),
                            TipoSolicitud = ConstanCodigoTipoCompromisos.CompromisosSolicitud.ToString(),
                            CompromisoId = SesionSolicitudCompromiso.SesionSolicitudCompromisoId,
                            ComiteTecnicoId = ComiteTecnico.ComiteTecnicoId,
                            EsCumplido = SesionSolicitudCompromiso.EsCumplido,

                        });
                    }
                }
                foreach (var SesionComiteSolicitudComiteTecnico in ComiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario.Where(r => !(bool)r.Eliminado).OrderByDescending(r => r.SesionComiteSolicitudId))
                {
                    foreach (var SesionSolicitudCompromiso in SesionComiteSolicitudComiteTecnico.SesionSolicitudCompromiso.Where(r => !(bool)r.Eliminado).OrderByDescending(r => r.SesionSolicitudCompromisoId))
                    {
                        ListCompromisos.Add(new ListCompromisos
                        {
                            FechaComite = ComiteTecnico.FechaOrdenDia,
                            NumeroComite = ComiteTecnico.NumeroComite,
                            Compromiso = SesionSolicitudCompromiso.Tarea,
                            EstadoCodigo = string.IsNullOrEmpty(SesionSolicitudCompromiso.EstadoCodigo) ? ConstanStringCodigoCompromisos.Sin_avance : SesionSolicitudCompromiso.EstadoCodigo,
                            FechaCumplimiento = ((DateTime)SesionSolicitudCompromiso.FechaCumplimiento).ToString("dd-MMMM-YY"),
                            TipoSolicitud = ConstanCodigoTipoCompromisos.CompromisosSolicitud.ToString(),
                            CompromisoId = SesionSolicitudCompromiso.SesionSolicitudCompromisoId,
                            ComiteTecnicoId = ComiteTecnico.ComiteTecnicoId,
                            EsCumplido = SesionSolicitudCompromiso.EsCumplido,

                        });
                    }
                }




                foreach (var SesionComiteTema in ComiteTecnico.SesionComiteTema.Where(r => !(bool)r.Eliminado).OrderByDescending(r => r.SesionTemaId))
                {
                    SesionComiteTema.TemaCompromiso = SesionComiteTema.TemaCompromiso.Where(r => !(bool)r.Eliminado).ToList();
                }
                foreach (var SesionComiteTema in ComiteTecnico.SesionComiteTema.Where(r => !(bool)r.Eliminado).OrderByDescending(r => r.SesionTemaId))
                {
                    foreach (var TemaCompromiso in SesionComiteTema.TemaCompromiso.Where(r => !(bool)r.Eliminado).ToList().OrderByDescending(r => r.TemaCompromisoId))
                    {
                        ListCompromisos.Add(new ListCompromisos
                        {
                            FechaComite = ComiteTecnico.FechaOrdenDia,
                            NumeroComite = ComiteTecnico.NumeroComite,
                            Compromiso = TemaCompromiso.Tarea,
                            EstadoCodigo = string.IsNullOrEmpty(TemaCompromiso.EstadoCodigo) ? ConstanStringCodigoCompromisos.Sin_avance : TemaCompromiso.EstadoCodigo,
                            FechaCumplimiento = ((DateTime)TemaCompromiso.FechaCumplimiento).ToString("dd-MMMM-YY"),
                            TipoSolicitud = ConstanCodigoTipoCompromisos.CompromisosTema.ToString(),
                            CompromisoId = TemaCompromiso.TemaCompromisoId,
                            ComiteTecnicoId = ComiteTecnico.ComiteTecnicoId,
                            EsCumplido = TemaCompromiso.EsCumplido,

                        });
                    }
                }
            }
            List<dynamic> ListGrilla = new List<dynamic>();
            foreach (var item in ListCompromisos)
            {
                if (ListGrilla.Where(t => t.ComiteTecnicoId == item.ComiteTecnicoId).Count() == 0)
                {
                    ListGrilla.Add(new
                    {
                        FechaOrdenDia = item.FechaComite,
                        item.NumeroComite,
                        cantidadCompromisos = ListCompromisos.Where(r => r.ComiteTecnicoId == item.ComiteTecnicoId).Count(),
                        cantidadCompromisosCumplidos = ListCompromisos.Where(r => r.ComiteTecnicoId == item.ComiteTecnicoId && r.EsCumplido == true).Count(),
                        item.ComiteTecnicoId
                    }); ;
                }
            }
            return ListGrilla;
        }

        public async Task<byte[]> ReplacePlantillaControversiasContractuales(int pControversiaId, int pComiteTecnicoId)
        {
            ControversiaContractual controversia = await _IContractualControversy.GetControversiaContractualById(pControversiaId);

            if (controversia == null)
            {
                return Array.Empty<byte>();
            }

            string TipoPlantilla = ((int)ConstanCodigoPlantillas.Ficha_De_Controversia_Contractual).ToString();

            Plantilla Plantilla = _context.Plantilla.Where(r => r.Codigo == TipoPlantilla).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();
            Plantilla.Contenido = await ReemplazarDatosPlantillaControversiaContractual(Plantilla.Contenido, controversia, pComiteTecnicoId);
            //return ConvertirPDF(Plantilla);
            return PDF.Convertir(Plantilla);

        }
        public async Task<byte[]> ReplacePlantillaActuacionesControversiasContractuales(int pControversiaId, int pComiteTecnicoId)
        {
            ControversiaActuacion actuacion = _context.ControversiaActuacion.Find(pControversiaId);

            if (actuacion == null)
            {
                return Array.Empty<byte>();
            }

            ControversiaContractual controversia = await _IContractualControversy.GetControversiaContractualById(actuacion.ControversiaContractualId);


            string TipoPlantilla = ((int)ConstanCodigoPlantillas.Ficha_De_Controversia_Contractual).ToString();

            Plantilla Plantilla = _context.Plantilla.Where(r => r.Codigo == TipoPlantilla).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();
            Plantilla.Contenido = await ReemplazarDatosPlantillaControversiaContractual(Plantilla.Contenido, controversia, pComiteTecnicoId);
            //return ConvertirPDF(Plantilla);
            return PDF.Convertir(Plantilla);

        }

        public async Task<byte[]> ReplacePlantillaNovedadContractual(int pNovedadContractual)
        {
            if (pNovedadContractual == null)
            {
                return Array.Empty<byte>();
            }

            //ControversiaContractual controversia = await _IContractualControversy.GetControversiaContractualById(actuacion.ControversiaContractualId);

            NovedadContractual novedadContractual = await _IContractualNoveltyService.GetNovedadContractualById(pNovedadContractual);

            string TipoPlantilla = ((int)ConstanCodigoPlantillas.Ficha_novedad_contractual).ToString();

            Plantilla Plantilla = _context.Plantilla.Where(r => r.Codigo == TipoPlantilla).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();
            Plantilla.Contenido = await ReemplazarDatosPlantillaNovedadContractual(Plantilla.Contenido, novedadContractual);
            return PDF.Convertir(Plantilla);

        }

        public async Task<string> ReemplazarDatosPlantillaControversiaContractual(string pPlantilla, ControversiaContractual controversiaContractual, int pComiteTecnicoId)
        {
            try
            {
                List<Dominio> placeholders = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.PlaceHolder).ToList();

                string TipoPlantillaDetalleProyecto = ((int)ConstanCodigoPlantillas.Detalle_Proyecto).ToString();
                string DetalleProyecto = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaDetalleProyecto).Select(r => r.Contenido).FirstOrDefault();
                string DetallesProyectos = "";

                string TipoPlantillaRegistrosAlcance = ((int)ConstanCodigoPlantillas.Registros_Tabla_Alcance).ToString();
                string RegistroAlcance = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaRegistrosAlcance).Select(r => r.Contenido).FirstOrDefault();

                string TipoPlantillaEjecucionProyecto = ((int)ConstanCodigoPlantillas.Ejecucion_proyecto).ToString();
                string EjecucionProyecto = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaEjecucionProyecto).Select(r => r.Contenido).FirstOrDefault();

                string EjecucionesProyecto = "";

                //TAI
                string TipoPlantillaDetalleSolicitud = ((int)ConstanCodigoPlantillas.Detalle_solicitud_tai).ToString();
                string DetalleSolicitud = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaDetalleSolicitud).Select(r => r.Contenido).FirstOrDefault();

                //NO TAI
                if (controversiaContractual.TipoControversiaCodigo != "1")
                {
                    TipoPlantillaDetalleSolicitud = ((int)ConstanCodigoPlantillas.Detalle_solicitud_no_tai).ToString();
                    DetalleSolicitud = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaDetalleSolicitud).Select(r => r.Contenido).FirstOrDefault();
                }

                string DetallesSolicitudes = "";

                //historial modificaciones
                string TipoPlantillaHistorialModifcaciones = ((int)ConstanCodigoPlantillas.Historial_de_modificaciones_controversias).ToString();
                string HistorialModificaciones = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaHistorialModifcaciones).Select(r => r.Contenido).FirstOrDefault();
                string Historiales = "";
                //tipos de novedad en el historial

                //adicion
                string TipoPlantillaNovedadAdicion = ((int)ConstanCodigoPlantillas.Tipo_de_novedad_adicion).ToString();
                string NovedadAdicion = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaNovedadAdicion).Select(r => r.Contenido).FirstOrDefault();
                string NovedadesAdicion = "";
                //prorroga
                string TipoPlantillaNovedadProrroga = ((int)ConstanCodigoPlantillas.Tipo_de_novedad_prorroga).ToString();
                string NovedadProrroga = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaNovedadProrroga).Select(r => r.Contenido).FirstOrDefault();
                string NovedadesProrroga = "";
                //modificacion contractal
                string TipoPlantillaNovedadModificacion = ((int)ConstanCodigoPlantillas.Tipo_de_novedad_modificacion_contractual).ToString();
                string NovedadModificacion = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaNovedadModificacion).Select(r => r.Contenido).FirstOrDefault();
                string NovedadesModificacion = "";
                //otras
                string TipoPlantillaNovedadOtras = ((int)ConstanCodigoPlantillas.Tipo_de_novedad_otras).ToString();
                string NovedadOtras = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaNovedadOtras).Select(r => r.Contenido).FirstOrDefault();
                string NovedadesOtras = "";

                //APORTANTES

                string TipoPlantillaAportantes = ((int)ConstanCodigoPlantillas.Tipo_de_novedad_adicion_aportante).ToString();
                string Aportante = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaAportantes).Select(r => r.Contenido).FirstOrDefault();
                string Aportantes = " ";
                decimal presupuestoAdicion = 0;

                List<Dominio> ListaParametricas = _context.Dominio.ToList();
                List<Localizacion> ListaLocalizaciones = _context.Localizacion.ToList();
                List<InstitucionEducativaSede> ListaInstitucionEducativaSedes = _context.InstitucionEducativaSede.ToList();
                //Se crea el detalle de los proyectos asociado a contratacion - contratacionProy ecto 
                int enumProyecto = 1;
                int enumProyectoEjecucion = 1;
                int enumHistorial = 1;
                int enumClausula = 1;

                Contrato contrato = controversiaContractual.Contrato;
                Contratacion contratacion = null;
                DisponibilidadPresupuestal disponibilidadPresupuestal = null;

                List<NovedadContractual> novedadContractual = new List<NovedadContractual>();

                Contratista contratista = null;

                if (contrato != null)
                {
                    contratacion = await _IProjectContractingService.GetAllContratacionByContratacionId(contrato.ContratacionId);
                    novedadContractual = _context.NovedadContractual.Where(r => r.ContratoId == contrato.ContratoId)
                                                                    .Include(r => r.NovedadContractualDescripcion)
                                                                        .ThenInclude(r => r.NovedadContractualClausula)
                                                                    .Include(r => r.NovedadContractualAportante)
                                                                        .ThenInclude(r => r.ComponenteAportanteNovedad)
                                                                            .ThenInclude(r => r.ComponenteFuenteNovedad)
                                                                                .ThenInclude(r => r.ComponenteUsoNovedad).ToList();
                }
                if (contratacion != null)
                {
                    contratista = _context.Contratista
                        .Where(r => r.ContratistaId == contratacion.ContratistaId).FirstOrDefault();
                    disponibilidadPresupuestal = _context.DisponibilidadPresupuestal
                        .Where(r => r.ContratacionId == contratacion.ContratacionId).FirstOrDefault();
                }

                foreach (var proyecto in contratacion.ContratacionProyecto)
                {
                    //Se crear una nueva plantilla por cada vez que entra
                    DetallesProyectos += DetalleProyecto;
                    EjecucionesProyecto += EjecucionProyecto;
                    string RegistrosAlcance = "";

                    Localizacion Municipio = ListaLocalizaciones.Where(r => r.LocalizacionId == proyecto.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                    Localizacion Departamento = ListaLocalizaciones.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
                    Localizacion Region = ListaLocalizaciones.Where(r => r.LocalizacionId == Departamento.IdPadre).FirstOrDefault();

                    InstitucionEducativaSede IntitucionEducativa = ListaInstitucionEducativaSedes.Where(r => r.InstitucionEducativaSedeId == proyecto.Proyecto.InstitucionEducativaId).FirstOrDefault();
                    InstitucionEducativaSede Sede = ListaInstitucionEducativaSedes.Where(r => r.InstitucionEducativaSedeId == proyecto.Proyecto.SedeId).FirstOrDefault();

                    #region Detalle Solicitud

                    foreach (Dominio placeholderDominio in placeholders)
                    {
                        switch (placeholderDominio.Codigo)
                        {

                            case ConstanCodigoVariablesPlaceHolders.NOMBRE_PROYECTO:
                                DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, (enumProyecto++).ToString());
                                EjecucionesProyecto = EjecucionesProyecto.Replace(placeholderDominio.Nombre, (enumProyectoEjecucion++).ToString());
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
                                DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, proyecto.Proyecto.PlazoMesesObra.ToString());
                                break;

                            case ConstanCodigoVariablesPlaceHolders.PLAZO_OBRA_DIAS:
                                DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, proyecto.Proyecto.PlazoDiasObra.ToString());
                                break;

                            case ConstanCodigoVariablesPlaceHolders.PLAZO_INTERVENTORIA_MESES:
                                DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, proyecto.Proyecto.PlazoMesesInterventoria.ToString());
                                break;

                            case ConstanCodigoVariablesPlaceHolders.PLAZO_INTERVENTORIA_DIAS:
                                DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, proyecto.Proyecto.PlazoDiasInterventoria.ToString());
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

                            case ConstanCodigoVariablesPlaceHolders.ESTADO_OBRA:
                                EjecucionesProyecto = EjecucionesProyecto.Replace(placeholderDominio.Nombre, proyecto.EstadoObraCodigo != null ? ListaParametricas.Where(r => r.Codigo == proyecto.EstadoObraCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Obra_Avance_Semanal).FirstOrDefault().Nombre : "Sin registro de avance semanal");
                                break;

                            case ConstanCodigoVariablesPlaceHolders.PROGRAMACION_OBRA:
                                EjecucionesProyecto = EjecucionesProyecto.Replace(placeholderDominio.Nombre, proyecto.ProgramacionSemanal != null ? proyecto.ProgramacionSemanal + " %" : "0 %");
                                break;


                            case ConstanCodigoVariablesPlaceHolders.AVANCE_FISICO_ACUMULADO:
                                EjecucionesProyecto = EjecucionesProyecto.Replace(placeholderDominio.Nombre, proyecto.AvanceFisicoSemanal != null ? proyecto.AvanceFisicoSemanal + " %" : "0 %");
                                break;

                            case ConstanCodigoVariablesPlaceHolders.FACTURACION_PROGRAMADA:
                                EjecucionesProyecto = EjecucionesProyecto.Replace(placeholderDominio.Nombre, " ");
                                break;

                            case ConstanCodigoVariablesPlaceHolders.FACTURACION_EJECUTADA:
                                EjecucionesProyecto = EjecucionesProyecto.Replace(placeholderDominio.Nombre, " ");
                                break;
                        }
                    }

                    #endregion Detalle Solicitud

                }

                #region detalle
                DetallesSolicitudes += DetalleSolicitud;
                ControversiaMotivo controversiaMotivo = null;

                controversiaMotivo = _context.ControversiaMotivo.Where(r => r.ControversiaContractualId == controversiaContractual.ControversiaContractualId).FirstOrDefault();

                foreach (Dominio placeholderDominio in placeholders)
                {
                    switch (placeholderDominio.Codigo)
                    {

                        case ConstanCodigoVariablesPlaceHolders.MOTIVOS_SOLICITUD:
                            DetallesSolicitudes = DetallesSolicitudes.Replace(placeholderDominio.Nombre, controversiaMotivo != null ? controversiaMotivo.MotivoSolicitudCodigo != null ? ListaParametricas.Where(r => r.Codigo == controversiaMotivo.MotivoSolicitudCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_controversia).FirstOrDefault().Nombre : " " : " ");
                            break;

                        case ConstanCodigoVariablesPlaceHolders.FECHA_COMITE_PRE_TECNICO:
                            DetallesSolicitudes = DetallesSolicitudes.Replace(placeholderDominio.Nombre, controversiaContractual.FechaComitePreTecnico == null ? "" : Convert.ToDateTime(controversiaContractual.FechaComitePreTecnico).ToString("dd/MM/yyyy"));
                            break;

                        case ConstanCodigoVariablesPlaceHolders.CONCLUSION_COMITE_PRE_TECNICO:
                            DetallesSolicitudes = DetallesSolicitudes.Replace(placeholderDominio.Nombre, controversiaContractual.ConclusionComitePreTecnico != null ? controversiaContractual.ConclusionComitePreTecnico : "");
                            break;

                        case ConstanCodigoVariablesPlaceHolders.URL_SOPORTE_SOLICITUD:
                            DetallesSolicitudes = DetallesSolicitudes.Replace(placeholderDominio.Nombre, controversiaContractual.RutaSoporte != null ? controversiaContractual.RutaSoporte : "");
                            break;
                        //DIFERENTES TAI
                        case ConstanCodigoVariablesPlaceHolders.FECHA_RADICADO_SAC:
                            DetallesSolicitudes = DetallesSolicitudes.Replace(placeholderDominio.Nombre, controversiaContractual.NumeroRadicadoSac != null ? controversiaContractual.NumeroRadicadoSac : "");
                            break;

                        case ConstanCodigoVariablesPlaceHolders.RESUMEN_JUSTIFICACION_SOLICITUD:
                            DetallesSolicitudes = DetallesSolicitudes.Replace(placeholderDominio.Nombre, controversiaContractual.MotivoJustificacionRechazo != null ? controversiaContractual.MotivoJustificacionRechazo : "");
                            break;
                    }
                }

                #endregion

                #region historial
                string tipoNovedadString = string.Empty;

                foreach (var novedad in novedadContractual)
                {
                    //adicion
                    NovedadesAdicion = "";
                    //prorroga
                    NovedadesProrroga = "";
                    //modificacion contractal
                    NovedadesModificacion = "";
                    //otras
                    NovedadesOtras = "";
                    //APORTANTES
                    Aportantes = "";

                    if (novedad.Eliminado == null || novedad.Eliminado == false)
                    {
                        SesionComiteSolicitud sesionComiteSolicitud = _context.SesionComiteSolicitud
                                        .Where(r => r.SolicitudId == novedad.NovedadContractualId && r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Novedad_Contractual && (r.Eliminado == false || r.Eliminado == null))
                                        .FirstOrDefault();
                        ComiteTecnico comiteTecnico = new ComiteTecnico();
                        ComiteTecnico comiteFiduciario = new ComiteTecnico();

                        if (sesionComiteSolicitud != null)
                        {
                            comiteTecnico = _context.ComiteTecnico.Find(sesionComiteSolicitud.ComiteTecnicoId);
                            comiteFiduciario = _context.ComiteTecnico.Find(sesionComiteSolicitud.ComiteTecnicoFiduciarioId);
                        }

                        //Se crear una nueva plantilla por cada vez que entra
                        Historiales += HistorialModificaciones;
                        NovedadesAdicion += NovedadAdicion;
                        NovedadesProrroga += NovedadProrroga;
                        NovedadesModificacion += NovedadModificacion;
                        string numeroComiteTecnico = comiteTecnico != null ? comiteTecnico.NumeroComite : string.Empty;
                        string numeroComiteFiduciario = comiteFiduciario != null ? comiteFiduciario.NumeroComite : string.Empty;
                        string estado = sesionComiteSolicitud != null ? ListaParametricas.Where(r => r.Codigo == sesionComiteSolicitud.EstadoCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Sesion_Comite_Solicitud).FirstOrDefault().Nombre : string.Empty;
                        bool existeAdicion = false;
                        bool existeProrroga = false;
                        bool existeModificacion = false;
                        bool existeOtro = false;

                        List<NovedadContractualDescripcion> novedadContractualDescripcion = _context.NovedadContractualDescripcion.Where(r => r.NovedadContractualId == novedad.NovedadContractualId).ToList();
                        foreach (var item in novedadContractualDescripcion)
                        {
                            if (item.Eliminado == null || novedad.Eliminado == false)
                            {
                                string codigotipoNovedadTemp = item.TipoNovedadCodigo;
                                string tipoNovedadTemp = ListaParametricas.Where(r => r.Codigo == item.TipoNovedadCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Novedad_Modificacion_Contractual).FirstOrDefault().Nombre;

                                if (String.IsNullOrEmpty(tipoNovedadString))
                                {
                                    tipoNovedadString = tipoNovedadTemp;
                                }
                                else
                                {
                                    tipoNovedadString = tipoNovedadString + ", " + tipoNovedadTemp;
                                }

                                if (codigotipoNovedadTemp != ConstanTiposNovedades.Adición && codigotipoNovedadTemp != ConstanTiposNovedades.Prórroga && codigotipoNovedadTemp != ConstanTiposNovedades.Modificación_de_Condiciones_Contractuales)
                                {
                                    existeOtro = true;
                                    NovedadesOtras = NovedadesOtras.Replace("[TP_PLAZO_SOLICITADO]", "");
                                    NovedadesOtras = NovedadesOtras.Replace("[TP_FECHA_INICIO]", item.FechaInicioSuspension != null ? ((DateTime)item.FechaInicioSuspension).ToString("dd-MM-yyyy") : " ");
                                    NovedadesOtras = NovedadesOtras.Replace("[TP_FECHA_FIN]", item.FechaFinSuspension != null ? ((DateTime)item.FechaFinSuspension).ToString("dd-MM-yyyy") : " ");
                                }
                                if (codigotipoNovedadTemp == ConstanTiposNovedades.Adición)
                                {
                                    existeAdicion = true;
                                    presupuestoAdicion += item.PresupuestoAdicionalSolicitado != null ? (decimal)item.PresupuestoAdicionalSolicitado : 0;

                                    NovedadesAdicion = NovedadesAdicion.Replace("[TP_PLAZO_SOLICITADO]", item.PresupuestoAdicionalSolicitado != null ? "$" + String.Format("{0:n0}", item.PresupuestoAdicionalSolicitado) : string.Empty);
                                    Aportantes = Aportantes + Aportante;

                                    #region Aportantes

                                    int enumAportante = 1;

                                    string strNombreAportante = string.Empty;
                                    string ValorAportante = string.Empty;
                                    string strComponente = string.Empty;
                                    string strFase = string.Empty;
                                    string strTipoUso = string.Empty;
                                    string valorUso = string.Empty;

                                    foreach (var aportante in novedad.NovedadContractualAportante)
                                    {
                                        strNombreAportante = aportante.NombreAportante != null ? aportante.NombreAportante : "";
                                        ValorAportante = aportante.ValorAporte != null ? "$" + String.Format("{0:n0}", aportante.ValorAporte) : "";
                                        strComponente = aportante.ComponenteAportanteNovedad.Count > 0 ? aportante.ComponenteAportanteNovedad.FirstOrDefault().NombreTipoComponente : "";
                                        strFase = aportante.ComponenteAportanteNovedad.Count > 0 ? aportante.ComponenteAportanteNovedad.FirstOrDefault().Nombrefase : "";
                                        strTipoUso = aportante.ComponenteAportanteNovedad.FirstOrDefault().ComponenteFuenteNovedad.FirstOrDefault().ComponenteUsoNovedad.Count() > 0 ? aportante.ComponenteAportanteNovedad.FirstOrDefault().ComponenteFuenteNovedad.FirstOrDefault().ComponenteUsoNovedad.FirstOrDefault().NombreUso : "";
                                        valorUso = aportante.ComponenteAportanteNovedad.FirstOrDefault().ComponenteFuenteNovedad.FirstOrDefault().ComponenteUsoNovedad.Count() > 0 ? "$" + String.Format("{0:n0}", aportante.ComponenteAportanteNovedad.FirstOrDefault().ComponenteFuenteNovedad.FirstOrDefault().ComponenteUsoNovedad.FirstOrDefault().ValorUso) : "";
                                        Aportantes = Aportantes.Replace("[TP_NUMERO_APORTANTE]", (enumAportante++).ToString())
                                                                .Replace("[TP_NOMBRE_APORTANTE]", strNombreAportante)
                                                                .Replace("[TP_VALOR_APORTANTE]", ValorAportante)
                                                                .Replace("[TP_FUENTE]", "")
                                                                .Replace("[TP_FASE]", strFase)
                                                                .Replace("[TP_COMPONENTE]", strComponente)
                                                                .Replace("[TP_USO]", strTipoUso)
                                                                .Replace("[TP_VALOR_USO]", valorUso);
                                    }

                                    if (novedad.NovedadContractualAportante.Count > 0)
                                    {
                                        NovedadesAdicion = NovedadesAdicion.Replace("[TP_APORTANTE]", Aportantes);
                                    }
                                    else
                                    {
                                        NovedadesAdicion = NovedadesAdicion.Replace("[TP_APORTANTE]", "");

                                    }

                                    #endregion
                                }
                                if (codigotipoNovedadTemp == ConstanTiposNovedades.Prórroga)
                                {
                                    existeProrroga = true;
                                    NovedadesProrroga = NovedadesProrroga.Replace("[TP_PLAZO_DIAS]", item.PlazoAdicionalDias != null ? Math.Round((decimal)item.PlazoAdicionalDias, 0).ToString() : "");
                                    NovedadesProrroga = NovedadesProrroga.Replace("[TP_PLAZO_MESES]", item.PlazoAdicionalMeses != null ? Math.Round((decimal)item.PlazoAdicionalMeses, 0).ToString() : "");
                                }

                                if (codigotipoNovedadTemp == ConstanTiposNovedades.Modificación_de_Condiciones_Contractuales)
                                {
                                    existeModificacion = true;
                                    enumClausula = 1;

                                    foreach (var clausula in item.NovedadContractualClausula)
                                    {
                                        if (clausula != null)
                                        {
                                            NovedadesModificacion = NovedadesModificacion.Replace("[TP_NUM_CLAUSULA]", (enumClausula++).ToString());
                                            NovedadesModificacion = NovedadesModificacion.Replace("[TP_CLAUSULA]", clausula.ClausulaAmodificar != null ? clausula.ClausulaAmodificar : string.Empty);
                                            NovedadesModificacion = NovedadesModificacion.Replace("[TP_AJUSTE_CLAUSULA]", clausula.AjusteSolicitadoAclausula != null ? clausula.AjusteSolicitadoAclausula : string.Empty);
                                        }
                                    }
                                }
                            }
                        }

                        foreach (Dominio placeholderDominio in placeholders)
                        {
                            switch (placeholderDominio.Codigo)
                            {

                                case ConstanCodigoVariablesPlaceHolders.NOMBRE_MODIFICACION:
                                    Historiales = Historiales.Replace(placeholderDominio.Nombre, (enumHistorial++).ToString());
                                    break;

                                case ConstanCodigoVariablesPlaceHolders.NUMERO_SOLICITUD:
                                    Historiales = Historiales.Replace(placeholderDominio.Nombre, novedad.NumeroSolicitud);
                                    break;

                                case ConstanCodigoVariablesPlaceHolders.TIPO_NOVEDAD:
                                    Historiales = Historiales.Replace(placeholderDominio.Nombre, tipoNovedadString);
                                    break;

                                case ConstanCodigoVariablesPlaceHolders.TP_NUMERO_COMITE_TECNICO:
                                    Historiales = Historiales.Replace(placeholderDominio.Nombre, numeroComiteTecnico);
                                    break;

                                case ConstanCodigoVariablesPlaceHolders.TP_NUMERO_COMITE_FIDUCIARIO:
                                    Historiales = Historiales.Replace(placeholderDominio.Nombre, numeroComiteFiduciario);
                                    break;

                                case ConstanCodigoVariablesPlaceHolders.TP_ESTADO:
                                    Historiales = Historiales.Replace(placeholderDominio.Nombre, estado);
                                    break;

                                case ConstanCodigoVariablesPlaceHolders.TIPO_NOVEDAD_OTRAS:
                                    if (existeOtro)
                                    {
                                        Historiales = Historiales.Replace(placeholderDominio.Nombre, NovedadesOtras);
                                    }
                                    break;

                                case ConstanCodigoVariablesPlaceHolders.TIPO_NOVEDAD_ADICION:
                                    if (existeAdicion)
                                    {
                                        Historiales = Historiales.Replace(placeholderDominio.Nombre, NovedadesAdicion);
                                    }
                                    break;

                                case ConstanCodigoVariablesPlaceHolders.TIPO_NOVEDAD_PRORROGA:
                                    if (existeProrroga)
                                    {
                                        Historiales = Historiales.Replace(placeholderDominio.Nombre, NovedadesProrroga);
                                    }
                                    break;

                                case ConstanCodigoVariablesPlaceHolders.TIPO_NOVEDAD_MODIFICACION:
                                    if (existeModificacion)
                                    {
                                        Historiales = Historiales.Replace(placeholderDominio.Nombre, NovedadesModificacion);
                                    }
                                    break;
                            }
                        }
                    }
                }

                #endregion
                #region fuentes usos

                foreach (Dominio placeholderDominio in placeholders)
                {
                    //ConstanCodigoVariablesPlaceHolders placeholder = (ConstanCodigoVariablesPlaceHolders)placeholderDominio.Codigo.ToString();

                    switch (placeholderDominio.Codigo)
                    {

                        case ConstanCodigoVariablesPlaceHolders.NUMERO_SOLICITUD:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, controversiaContractual.NumeroSolicitud);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.FECHA_SOLICITUD:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, controversiaContractual.FechaSolicitud != null ? ((DateTime)controversiaContractual.FechaSolicitud).ToString("dd-MM-yyyy") : " ");
                            break;

                        case ConstanCodigoVariablesPlaceHolders.TIPO_CONTROVERSIA:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, ListaParametricas.Where(r => r.Codigo == controversiaContractual.TipoControversiaCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_controversia).FirstOrDefault().Nombre);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.CANTIDAD_DE_PROYECTOS_ASOCIADOS:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, contratacion != null ? contratacion.ContratacionProyecto.Count().ToString() : "");
                            break;
                        //Datos Contratista y contrato
                        case ConstanCodigoVariablesPlaceHolders.NUMERO_CONTRATO:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, contrato != null ? contrato.NumeroContrato : "");
                            break;

                        case ConstanCodigoVariablesPlaceHolders.CONTRATISTA_NOMBRE:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, contratista != null ? contratista.Nombre : "");
                            break;

                        case ConstanCodigoVariablesPlaceHolders.FECHA_INICIO_CONTRATO:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, contrato != null ? ((DateTime)contrato.FechaCreacion).ToString("dd-MM-yyyy") : " ");
                            break;

                        case ConstanCodigoVariablesPlaceHolders.FECHA_FIN_CONTRATO:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, contrato != null ? contrato.FechaTerminacionFase2 != null ? ((DateTime)contrato.FechaTerminacionFase2).ToString("dd-MM-yyyy") : " " : " ");
                            break;

                        //
                        case ConstanCodigoVariablesPlaceHolders.DETALLES_PROYECTOS:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, DetallesProyectos);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.EJECUCION_PROYECTO:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, EjecucionesProyecto);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.DETALLE_SOLICITUD_TAI:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, DetallesSolicitudes);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.HISTORIAL_MODIFICACIONES:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, Historiales);
                            break;
                    }
                }

                #endregion fuentes usos

                return pPlantilla;
            }
            catch (Exception ex)
            {
                return ex.InnerException.ToString();
            }
        }

        public async Task<string> ReemplazarDatosPlantillaNovedadContractual(string pPlantilla, NovedadContractual novedadContractual)
        {
            try
            {
                List<Dominio> placeholders = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.PlaceHolder).ToList();

                string TipoPlantillaDetalleProyecto = ((int)ConstanCodigoPlantillas.Detalle_proyecto_no_alcance).ToString();
                string DetalleProyecto = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaDetalleProyecto).Select(r => r.Contenido).FirstOrDefault();
                string DetallesProyectos = "";

                string TipoPlantillaRegistrosAlcance = ((int)ConstanCodigoPlantillas.Registros_Tabla_Alcance).ToString();
                string RegistroAlcance = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaRegistrosAlcance).Select(r => r.Contenido).FirstOrDefault();

                string TipoPlantillaEjecucionProyecto = ((int)ConstanCodigoPlantillas.Ejecucion_proyecto).ToString();
                string EjecucionProyecto = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaEjecucionProyecto).Select(r => r.Contenido).FirstOrDefault();

                string EjecucionesProyecto = "";
                string tipoNovedadString = string.Empty;

                //historial modificaciones
                string TipoPlantillaHistorialModifcaciones = ((int)ConstanCodigoPlantillas.Historial_de_modificaciones_controversias).ToString();
                string HistorialModificaciones = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaHistorialModifcaciones).Select(r => r.Contenido).FirstOrDefault();
                string Historiales = "";
                //tipos de novedad en el historial

                string TipoPlantillaNovedadDetalle = ((int)ConstanCodigoPlantillas.Novedades_detalles).ToString();
                string NovedadDetalle = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaNovedadDetalle).Select(r => r.Contenido).FirstOrDefault();
                string NovedadesDetalles = "";
                //adicion
                string TipoPlantillaNovedadAdicion = ((int)ConstanCodigoPlantillas.Tipo_de_novedad_adicion).ToString();
                string NovedadAdicion = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaNovedadAdicion).Select(r => r.Contenido).FirstOrDefault();
                string NovedadesAdicion = "";
                //prorroga
                string TipoPlantillaNovedadProrroga = ((int)ConstanCodigoPlantillas.Tipo_de_novedad_prorroga).ToString();
                string NovedadProrroga = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaNovedadProrroga).Select(r => r.Contenido).FirstOrDefault();
                string NovedadesProrroga = "";
                //modificacion contractal
                string TipoPlantillaNovedadModificacion = ((int)ConstanCodigoPlantillas.Tipo_de_novedad_modificacion_contractual).ToString();
                string NovedadModificacion = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaNovedadModificacion).Select(r => r.Contenido).FirstOrDefault();
                string NovedadesModificacion = "";
                //otras
                string TipoPlantillaNovedadOtras = ((int)ConstanCodigoPlantillas.Tipo_de_novedad_otras).ToString();
                string NovedadOtras = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaNovedadOtras).Select(r => r.Contenido).FirstOrDefault();
                string NovedadesOtras = "";

                //APORTANTES

                string TipoPlantillaAportantes = ((int)ConstanCodigoPlantillas.Tipo_de_novedad_adicion_aportante).ToString();
                string Aportante = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaAportantes).Select(r => r.Contenido).FirstOrDefault();
                string Aportantes = " ";
                decimal presupuestoAdicion = 0;

                //Detalle solicitud
                string TipoPlantillaDetalleSolicitud = ((int)ConstanCodigoPlantillas.Detalle_solicitud_novedad).ToString();
                string DetalleSolicitud = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaDetalleSolicitud).Select(r => r.Contenido).FirstOrDefault();
                string DetallesSolicitudes = "";

                List<Dominio> ListaParametricas = _context.Dominio.ToList();
                List<Localizacion> ListaLocalizaciones = _context.Localizacion.ToList();
                List<InstitucionEducativaSede> ListaInstitucionEducativaSedes = _context.InstitucionEducativaSede.ToList();
                //Se crea el detalle de los proyectos asociado a contratacion - contratacionProy ecto 
                int enumProyecto = 1;
                int enumProyectoEjecucion = 1;
                int enumHistorial = 1;
                int enumClausula = 1;
                int enumDetalles = 1;

                Contrato contrato = _context.Contrato.Where(r => r.ContratoId == novedadContractual.ContratoId).FirstOrDefault();
                Contratacion contratacion = null;
                DisponibilidadPresupuestal disponibilidadPresupuestal = null;

                string row_template = "<td rowspan='[ROWSPAN]'><div>[ROW]</div></td>";

                #region detalle solicitud
                DetallesSolicitudes += DetalleSolicitud;
                NovedadesAdicion += NovedadAdicion;
                NovedadesProrroga += NovedadProrroga;
                NovedadesModificacion += NovedadModificacion;
                bool existeAdicionDetalle = false;
                bool existeProrrogaDetalle = false;
                bool existeModificacionDetalle = false;
                bool existeOtroDetalle = false;
                string tipoNovedadDetalle = string.Empty;

                List<NovedadContractualDescripcion> novedadContractualDescripcionDetalle = _context.NovedadContractualDescripcion.Where(r => r.NovedadContractualId == novedadContractual.NovedadContractualId).ToList();
                foreach (var item in novedadContractualDescripcionDetalle)
                {
                    string motivoString = string.Empty;

                    if (item.Eliminado == null || item.Eliminado == false)
                    {
                        string codigotipoNovedadTemp = item.TipoNovedadCodigo;
                        string tipoNovedadTemp = ListaParametricas.Where(r => r.Codigo == item.TipoNovedadCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Novedad_Modificacion_Contractual).FirstOrDefault().Nombre;

                        if (String.IsNullOrEmpty(tipoNovedadDetalle))
                        {
                            tipoNovedadDetalle = tipoNovedadTemp;
                        }
                        else
                        {
                            tipoNovedadDetalle = tipoNovedadDetalle + ", " + tipoNovedadTemp;
                        }

                        foreach (var motivo in item.NovedadContractualDescripcionMotivo)
                        {
                            string motivos = ListaParametricas.Where(r => r.Codigo == motivo.MotivoNovedadCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Motivos_Novedad_contractual).FirstOrDefault().Nombre;

                            if (String.IsNullOrEmpty(motivoString))
                            {
                                motivoString = motivos;
                            }
                            else
                            {
                                motivoString = motivoString + ", " + motivos;
                            }
                        }
                        NovedadesDetalles += NovedadDetalle;
                        NovedadesDetalles = NovedadesDetalles.Replace("[TP_NOVEDAD_NUM]", (enumDetalles++).ToString());
                        NovedadesDetalles = NovedadesDetalles.Replace("[TP_MOTIVOS_NOVEDAD]", motivoString);
                        NovedadesDetalles = NovedadesDetalles.Replace("[TP_JUSTIFICACION]", item.ResumenJustificacion);
                        NovedadesDetalles = NovedadesDetalles.Replace("[TP_CONCEPTO_TECNICO]", item.ConceptoTecnico);


                        if (codigotipoNovedadTemp != ConstanTiposNovedades.Adición && codigotipoNovedadTemp != ConstanTiposNovedades.Prórroga && codigotipoNovedadTemp != ConstanTiposNovedades.Modificación_de_Condiciones_Contractuales)
                        {
                            existeOtroDetalle = true;
                            NovedadesOtras = NovedadesOtras.Replace("[TP_PLAZO_SOLICITADO]", "");
                            NovedadesOtras = NovedadesOtras.Replace("[TP_FECHA_INICIO]", item.FechaInicioSuspension != null ? ((DateTime)item.FechaInicioSuspension).ToString("dd-MM-yyyy") : " ");
                            NovedadesOtras = NovedadesOtras.Replace("[TP_FECHA_FIN]", item.FechaFinSuspension != null ? ((DateTime)item.FechaFinSuspension).ToString("dd-MM-yyyy") : " ");
                        }
                        if (codigotipoNovedadTemp == ConstanTiposNovedades.Adición)
                        {
                            existeAdicionDetalle = true;
                            presupuestoAdicion += item.PresupuestoAdicionalSolicitado != null ? (decimal)item.PresupuestoAdicionalSolicitado : 0;
                            NovedadesAdicion = NovedadesAdicion.Replace("[TP_PLAZO_SOLICITADO]", item.PresupuestoAdicionalSolicitado != null ? "$" + String.Format("{0:n0}", item.PresupuestoAdicionalSolicitado) : string.Empty);
                            //Aportantes = Aportantes + Aportante;

                            #region Aportantes

                            int enumAportante = 1;

                            string strNombreAportante = string.Empty;
                            string ValorAportante = string.Empty;
                            string strComponente = string.Empty;
                            string strFase = string.Empty;
                            string strTipoUso = string.Empty;
                            string valorUso = string.Empty;
                            string strFuente = string.Empty;

                            foreach (var aportante in novedadContractual.NovedadContractualAportante)
                            {
                                Aportantes += Aportante;
                                foreach (var cpa in aportante.ComponenteAportanteNovedad)
                                {
                                    List<ComponenteFuenteNovedad> componenteFuenteNovedades = _context.ComponenteFuenteNovedad
                                        .Where(r => r.ComponenteAportanteNovedadId == cpa.ComponenteAportanteNovedadId && (r.Eliminado == false | r.Eliminado == null))
                                        .ToList();

                                    foreach (var cfn in componenteFuenteNovedades)
                                    {
                                        FuenteFinanciacion fuentesFinanciacion = _context.FuenteFinanciacion.Find(cfn.FuenteFinanciacionId);

                                        fuentesFinanciacion.NombreFuente = fuentesFinanciacion != null ? !string.IsNullOrEmpty(fuentesFinanciacion.FuenteRecursosCodigo) ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(fuentesFinanciacion.FuenteRecursosCodigo, (int)EnumeratorTipoDominio.Fuentes_de_financiacion) : "" : "";
                                        cfn.FuenteFinanciacion = fuentesFinanciacion;
                                    }

                                    cpa.ComponenteFuenteNovedad = componenteFuenteNovedades;
                                }
                                strNombreAportante = aportante.NombreAportante != null ? aportante.NombreAportante : "";
                                ValorAportante = aportante.ValorAporte != null ? "$" + String.Format("{0:n0}", aportante.ValorAporte) : "";
                                strComponente = aportante.ComponenteAportanteNovedad.Count > 0 ? aportante.ComponenteAportanteNovedad.FirstOrDefault().NombreTipoComponente : "";
                                strFase = aportante.ComponenteAportanteNovedad.Count > 0 ? aportante.ComponenteAportanteNovedad.FirstOrDefault().Nombrefase : "";
                                strFuente = aportante.ComponenteAportanteNovedad.Count > 0 ? aportante.ComponenteAportanteNovedad.FirstOrDefault().ComponenteFuenteNovedad.FirstOrDefault().FuenteFinanciacion.NombreFuente : "";
                                strTipoUso = aportante.ComponenteAportanteNovedad.FirstOrDefault().ComponenteFuenteNovedad.FirstOrDefault().ComponenteUsoNovedad.Count() > 0 ? aportante.ComponenteAportanteNovedad.FirstOrDefault().ComponenteFuenteNovedad.FirstOrDefault().ComponenteUsoNovedad.FirstOrDefault().NombreUso : "";
                                valorUso = aportante.ComponenteAportanteNovedad.FirstOrDefault().ComponenteFuenteNovedad.FirstOrDefault().ComponenteUsoNovedad.Count() > 0 ? "$" + String.Format("{0:n0}", aportante.ComponenteAportanteNovedad.FirstOrDefault().ComponenteFuenteNovedad.FirstOrDefault().ComponenteUsoNovedad.FirstOrDefault().ValorUso) : "";
                                Aportantes = Aportantes.Replace("[TP_NUMERO_APORTANTE]", (enumAportante++).ToString())
                                                        .Replace("[TP_NOMBRE_APORTANTE]", strNombreAportante)
                                                        .Replace("[TP_VALOR_APORTANTE]", ValorAportante)
                                                        .Replace("[TP_FUENTE]", strFuente)
                                                        .Replace("[TP_FASE]", strFase)
                                                        .Replace("[TP_COMPONENTE]", strComponente)
                                                        .Replace("[TP_USO]", strTipoUso)
                                                        .Replace("[TP_VALOR_USO]", valorUso);
                            }

                            if (novedadContractual.NovedadContractualAportante.Count > 0)
                            {
                                NovedadesAdicion = NovedadesAdicion.Replace("[TP_APORTANTE]", Aportantes);
                            }
                            else
                            {
                                NovedadesAdicion = NovedadesAdicion.Replace("[TP_APORTANTE]", "");

                            }

                            #endregion
                        }
                        if (codigotipoNovedadTemp == ConstanTiposNovedades.Prórroga)
                        {
                            existeProrrogaDetalle = true;
                            NovedadesProrroga = NovedadesProrroga.Replace("[TP_PLAZO_DIAS]", item.PlazoAdicionalDias != null ? Math.Round((decimal)item.PlazoAdicionalDias, 0).ToString() : "");
                            NovedadesProrroga = NovedadesProrroga.Replace("[TP_PLAZO_MESES]", item.PlazoAdicionalMeses != null ? Math.Round((decimal)item.PlazoAdicionalMeses, 0).ToString() : "");
                        }

                        if (codigotipoNovedadTemp == ConstanTiposNovedades.Modificación_de_Condiciones_Contractuales)
                        {
                            existeModificacionDetalle = true;
                            enumClausula = 1;

                            foreach (var clausula in item.NovedadContractualClausula)
                            {
                                if (clausula != null)
                                {
                                    NovedadesModificacion = NovedadesModificacion.Replace("[TP_NUM_CLAUSULA]", (enumClausula++).ToString());
                                    NovedadesModificacion = NovedadesModificacion.Replace("[TP_CLAUSULA]", clausula.ClausulaAmodificar != null ? clausula.ClausulaAmodificar : string.Empty);
                                    NovedadesModificacion = NovedadesModificacion.Replace("[TP_AJUSTE_CLAUSULA]", clausula.AjusteSolicitadoAclausula != null ? clausula.AjusteSolicitadoAclausula : string.Empty);
                                }
                            }
                        }
                    }
                }

                foreach (Dominio placeholderDominio in placeholders)
                {
                    switch (placeholderDominio.Codigo)
                    {

                        case ConstanCodigoVariablesPlaceHolders.TP_NOVEDAD:
                            DetallesSolicitudes = DetallesSolicitudes.Replace(placeholderDominio.Nombre, tipoNovedadDetalle);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.TP_NOVEDAD_DETALLES:
                            DetallesSolicitudes = DetallesSolicitudes.Replace(placeholderDominio.Nombre, NovedadesDetalles);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.TIPO_NOVEDAD_OTRAS:
                            if (existeOtroDetalle)
                            {
                                DetallesSolicitudes = DetallesSolicitudes.Replace(placeholderDominio.Nombre, NovedadesOtras);
                            }
                            break;

                        case ConstanCodigoVariablesPlaceHolders.TIPO_NOVEDAD_ADICION:
                            if (existeAdicionDetalle)
                            {
                                DetallesSolicitudes = DetallesSolicitudes.Replace(placeholderDominio.Nombre, NovedadesAdicion);
                            }
                            break;

                        case ConstanCodigoVariablesPlaceHolders.TIPO_NOVEDAD_PRORROGA:
                            if (existeProrrogaDetalle)
                            {
                                DetallesSolicitudes = DetallesSolicitudes.Replace(placeholderDominio.Nombre, NovedadesProrroga);
                            }
                            break;

                        case ConstanCodigoVariablesPlaceHolders.TIPO_NOVEDAD_MODIFICACION:
                            if (existeModificacionDetalle)
                            {
                                DetallesSolicitudes = DetallesSolicitudes.Replace(placeholderDominio.Nombre, NovedadesModificacion);
                            }
                            break;
                    }
                }

                #endregion
                Contratista contratista = null;

                if (contrato != null)
                {
                    contratacion = await _IProjectContractingService.GetAllContratacionByContratacionId(contrato.ContratacionId);
                }
                if (contratacion != null)
                {
                    contratista = _context.Contratista
                        .Where(r => r.ContratistaId == contratacion.ContratistaId).FirstOrDefault();
                    disponibilidadPresupuestal = _context.DisponibilidadPresupuestal
                        .Where(r => r.ContratacionId == contratacion.ContratacionId).FirstOrDefault();
                }

                string TipoPlantillaFuentesFinanciacion = ((int)ConstanCodigoPlantillas.Registros_Fuente_De_Uso).ToString();

                // FUENTES DE FINANCIACION 
                string TipoPlantillaRegistrosFuentes = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaFuentesFinanciacion).Select(r => r.Contenido).FirstOrDefault();
                string RegistrosFuentesUso = string.Empty;


                string TipoPlantillaRegistrosUsosFuenteUsos = ((int)ConstanCodigoPlantillas.Registros_Usos_Registros_Fuente_de_Uso).ToString();
                string PlantillaRegistrosUsosFuenteUsos = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaRegistrosUsosFuenteUsos).Select(r => r.Contenido).FirstOrDefault();
                string RegistrosRegistrosUsosFuenteUsos = string.Empty;

                #region fuentes usos

                foreach (var contratacionProyecto in contratacion.ContratacionProyecto)
                {
                    foreach (var ContratacionProyectoAportante in contratacionProyecto.ContratacionProyectoAportante)
                    {
                        foreach (var ComponenteAportante in ContratacionProyectoAportante.ComponenteAportante)
                        {
                            bool ind_ya_entro = false;
                            foreach (var ComponenteUso in ComponenteAportante.ComponenteUso)
                            {
                                RegistrosFuentesUso += TipoPlantillaRegistrosFuentes;
                                string nombreTrAportante = string.Empty;
                                string valorTrAportante = string.Empty;

                                if (!ind_ya_entro)
                                {
                                    string rowspan = ComponenteAportante.ComponenteUso.Count().ToString();
                                    /*
                                     * Nombre aportante
                                    */

                                    string nombre_aportante_row = row_template;
                                    nombre_aportante_row = nombre_aportante_row.Replace("[ROWSPAN]", rowspan);

                                    string strNombreAportante = string.Empty;
                                    switch (ContratacionProyectoAportante.CofinanciacionAportante.TipoAportanteId)
                                    {
                                        case ConstanTipoAportante.Ffie:
                                            strNombreAportante = ConstanStringTipoAportante.Ffie;
                                            break;

                                        case ConstanTipoAportante.ET:

                                            if (ContratacionProyectoAportante.CofinanciacionAportante.Departamento != null && ContratacionProyectoAportante.CofinanciacionAportante.Municipio == null)
                                            {
                                                strNombreAportante = "Gobernación de " + ContratacionProyectoAportante.CofinanciacionAportante.Departamento.Descripcion;
                                            }
                                            else if (ContratacionProyectoAportante.CofinanciacionAportante.Municipio != null)
                                            {
                                                strNombreAportante = "Alcaldía de " + ContratacionProyectoAportante.CofinanciacionAportante.Municipio.Descripcion;
                                            }
                                            break;
                                        case ConstanTipoAportante.Tercero:
                                            strNombreAportante = ContratacionProyectoAportante.CofinanciacionAportante.NombreAportante.Nombre;
                                            break;
                                    }
                                    nombreTrAportante = nombre_aportante_row.Replace("[ROW]", strNombreAportante);
                                    /*
                                     * Valor aportante
                                    */
                                    string valor_aportante_row = row_template;
                                    valor_aportante_row = valor_aportante_row.Replace("[ROWSPAN]", rowspan);

                                    string ValorAportante = "$" + String.Format("{0:n0}", ContratacionProyectoAportante.CofinanciacionAportante.ProyectoAportante.FirstOrDefault().ValorObra);
                                    if (contratacion.TipoSolicitudCodigo == ((int)ConstanCodigoTipoContratacion.Interventoria).ToString())
                                    {
                                        ValorAportante = "$" + String.Format("{0:n0}", ContratacionProyectoAportante.CofinanciacionAportante.ProyectoAportante.FirstOrDefault().ValorInterventoria);
                                    }

                                    valorTrAportante = valor_aportante_row.Replace("[ROW]", ValorAportante);

                                    ind_ya_entro = true;
                                }
                                else
                                {
                                    nombreTrAportante = string.Empty;
                                    valorTrAportante = string.Empty;
                                }

                                foreach (Dominio placeholderDominio in placeholders)
                                {
                                    switch (placeholderDominio.Codigo)
                                    {
                                        case ConstanCodigoVariablesPlaceHolders.NOMBRE_APORTANTE_FUENTES_USO:
                                            RegistrosFuentesUso = RegistrosFuentesUso.Replace(placeholderDominio.Nombre, nombreTrAportante);
                                            break;

                                        case ConstanCodigoVariablesPlaceHolders.VALOR_APORTANTE_PROYECTO_FUENTES_USO:
                                            RegistrosFuentesUso = RegistrosFuentesUso.Replace(placeholderDominio.Nombre, valorTrAportante); 
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
                                                string strTipoUso = ListaParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Usos && r.Codigo == ComponenteUso.TipoUsoCodigo)?.FirstOrDefault()?.Nombre;

                                                RegistrosFuentesUso = RegistrosFuentesUso.Replace("[USO_FUENTES_USO]", strTipoUso);
                                                RegistrosFuentesUso = RegistrosFuentesUso.Replace("[VALOR_USO_FUENTE_USO]", "$" + String.Format("{0:n0}", ComponenteUso.ValorUso));

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
                #endregion fuentes usos

                foreach (var proyecto in contratacion.ContratacionProyecto)
                {
                    //Se crear una nueva plantilla por cada vez que entra
                    DetallesProyectos += DetalleProyecto;
                    EjecucionesProyecto += EjecucionProyecto;
                    string RegistrosAlcance = "";

                    Localizacion Municipio = ListaLocalizaciones.Where(r => r.LocalizacionId == proyecto.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                    Localizacion Departamento = ListaLocalizaciones.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
                    Localizacion Region = ListaLocalizaciones.Where(r => r.LocalizacionId == Departamento.IdPadre).FirstOrDefault();

                    InstitucionEducativaSede IntitucionEducativa = ListaInstitucionEducativaSedes.Where(r => r.InstitucionEducativaSedeId == proyecto.Proyecto.InstitucionEducativaId).FirstOrDefault();
                    InstitucionEducativaSede Sede = ListaInstitucionEducativaSedes.Where(r => r.InstitucionEducativaSedeId == proyecto.Proyecto.SedeId).FirstOrDefault();

                    #region Detalle Solicitud

                    foreach (Dominio placeholderDominio in placeholders)
                    {
                        switch (placeholderDominio.Codigo)
                        {

                            case ConstanCodigoVariablesPlaceHolders.NOMBRE_PROYECTO:
                                DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, (enumProyecto++).ToString());
                                EjecucionesProyecto = EjecucionesProyecto.Replace(placeholderDominio.Nombre, (enumProyectoEjecucion++).ToString());
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

                            case ConstanCodigoVariablesPlaceHolders.PLAZO_OBRA_MESES:
                                DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, proyecto.Proyecto.PlazoMesesObra.ToString());
                                break;

                            case ConstanCodigoVariablesPlaceHolders.PLAZO_OBRA_DIAS:
                                DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, proyecto.Proyecto.PlazoDiasObra.ToString());
                                break;

                            case ConstanCodigoVariablesPlaceHolders.PLAZO_INTERVENTORIA_MESES:
                                DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, proyecto.Proyecto.PlazoMesesInterventoria.ToString());
                                break;

                            case ConstanCodigoVariablesPlaceHolders.PLAZO_INTERVENTORIA_DIAS:
                                DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, proyecto.Proyecto.PlazoDiasInterventoria.ToString());
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

                            case ConstanCodigoVariablesPlaceHolders.ESTADO_OBRA:
                                EjecucionesProyecto = EjecucionesProyecto.Replace(placeholderDominio.Nombre, proyecto.EstadoObraCodigo != null ? ListaParametricas.Where(r => r.Codigo == proyecto.EstadoObraCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Obra_Avance_Semanal).FirstOrDefault().Nombre : "Sin registro de avance semanal");
                                break;

                            case ConstanCodigoVariablesPlaceHolders.PROGRAMACION_OBRA:
                                EjecucionesProyecto = EjecucionesProyecto.Replace(placeholderDominio.Nombre, proyecto.ProgramacionSemanal != null ? proyecto.ProgramacionSemanal + " %" : "0 %");
                                break;


                            case ConstanCodigoVariablesPlaceHolders.AVANCE_FISICO_ACUMULADO:
                                EjecucionesProyecto = EjecucionesProyecto.Replace(placeholderDominio.Nombre, proyecto.AvanceFisicoSemanal != null ? proyecto.AvanceFisicoSemanal + " %" : "0 %");
                                break;

                            case ConstanCodigoVariablesPlaceHolders.FACTURACION_PROGRAMADA:
                                EjecucionesProyecto = EjecucionesProyecto.Replace(placeholderDominio.Nombre, " ");
                                break;

                            case ConstanCodigoVariablesPlaceHolders.FACTURACION_EJECUTADA:
                                EjecucionesProyecto = EjecucionesProyecto.Replace(placeholderDominio.Nombre, " ");
                                break;
                        }
                    }

                    #endregion Detalle Solicitud

                }

                #region historial
                //adicion
                NovedadesAdicion = "";
                //prorroga
                NovedadesProrroga = "";
                //modificacion contractal
                NovedadesModificacion = "";
                //otras
                NovedadesOtras = "";
                //APORTANTES
                Aportantes = "";
                tipoNovedadString = string.Empty;

                if (novedadContractual.Eliminado == null || novedadContractual.Eliminado == false)
                {
                    SesionComiteSolicitud sesionComiteSolicitud = _context.SesionComiteSolicitud
                                    .Where(r => r.SolicitudId == novedadContractual.NovedadContractualId && r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Novedad_Contractual && (r.Eliminado == false || r.Eliminado == null))
                                    .FirstOrDefault();
                    ComiteTecnico comiteTecnico = new ComiteTecnico();
                    ComiteTecnico comiteFiduciario = new ComiteTecnico();

                    if (sesionComiteSolicitud != null)
                    {
                        comiteTecnico = _context.ComiteTecnico.Find(sesionComiteSolicitud.ComiteTecnicoId);
                        comiteFiduciario = _context.ComiteTecnico.Find(sesionComiteSolicitud.ComiteTecnicoFiduciarioId);
                    }

                    //Se crear una nueva plantilla por cada vez que entra
                    Historiales += HistorialModificaciones;
                    NovedadesAdicion += NovedadAdicion;
                    NovedadesProrroga += NovedadProrroga;
                    NovedadesModificacion += NovedadModificacion;
                    string numeroComiteTecnico = comiteTecnico != null ? comiteTecnico.NumeroComite : string.Empty;
                    string numeroComiteFiduciario = comiteFiduciario != null ? comiteFiduciario.NumeroComite : string.Empty;
                    string estado = sesionComiteSolicitud.EstadoCodigo != null ? ListaParametricas.Where(r => r.Codigo == sesionComiteSolicitud.EstadoCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Sesion_Comite_Solicitud).FirstOrDefault().Nombre : string.Empty;
                    bool existeAdicion = false;
                    bool existeProrroga = false;
                    bool existeModificacion = false;
                    bool existeOtro = false;

                    List<NovedadContractualDescripcion> novedadContractualDescripcion = _context.NovedadContractualDescripcion.Where(r => r.NovedadContractualId == novedadContractual.NovedadContractualId).ToList();
                    foreach (var item in novedadContractualDescripcion)
                    {
                        if (item.Eliminado == null || item.Eliminado == false)
                        {
                            string codigotipoNovedadTemp = item.TipoNovedadCodigo;
                            string tipoNovedadTemp = ListaParametricas.Where(r => r.Codigo == item.TipoNovedadCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Novedad_Modificacion_Contractual).FirstOrDefault().Nombre;

                            if (String.IsNullOrEmpty(tipoNovedadString))
                            {
                                tipoNovedadString = tipoNovedadTemp;
                            }
                            else
                            {
                                tipoNovedadString = tipoNovedadString + ", " + tipoNovedadTemp;
                            }

                            if (codigotipoNovedadTemp != ConstanTiposNovedades.Adición && codigotipoNovedadTemp != ConstanTiposNovedades.Prórroga && codigotipoNovedadTemp != ConstanTiposNovedades.Modificación_de_Condiciones_Contractuales)
                            {
                                existeOtro = true;
                                NovedadesOtras = NovedadesOtras.Replace("[TP_PLAZO_SOLICITADO]", "");
                                NovedadesOtras = NovedadesOtras.Replace("[TP_FECHA_INICIO]", item.FechaInicioSuspension != null ? ((DateTime)item.FechaInicioSuspension).ToString("dd-MM-yyyy") : " ");
                                NovedadesOtras = NovedadesOtras.Replace("[TP_FECHA_FIN]", item.FechaFinSuspension != null ? ((DateTime)item.FechaFinSuspension).ToString("dd-MM-yyyy") : " ");
                            }
                            if (codigotipoNovedadTemp == ConstanTiposNovedades.Adición)
                            {
                                existeAdicion = true;
                                NovedadesAdicion = NovedadesAdicion.Replace("[TP_PLAZO_SOLICITADO]", item.PresupuestoAdicionalSolicitado != null ? "$" + String.Format("{0:n0}", item.PresupuestoAdicionalSolicitado) : string.Empty);
                                //Aportantes = Aportantes + Aportante;

                                #region Aportantes

                                int enumAportante = 1;

                                string strNombreAportante = string.Empty;
                                string ValorAportante = string.Empty;
                                string strComponente = string.Empty;
                                string strFase = string.Empty;
                                string strFuente = string.Empty;
                                string strTipoUso = string.Empty;
                                string valorUso = string.Empty;

                                foreach (var aportante in novedadContractual.NovedadContractualAportante)
                                {
                                    Aportantes += Aportante;

                                    foreach (var cpa in aportante.ComponenteAportanteNovedad)
                                    {
                                        List<ComponenteFuenteNovedad> componenteFuenteNovedades = _context.ComponenteFuenteNovedad
                                            .Where(r => r.ComponenteAportanteNovedadId == cpa.ComponenteAportanteNovedadId && (r.Eliminado == false | r.Eliminado == null))
                                            .ToList();

                                        foreach (var cfn in componenteFuenteNovedades)
                                        {
                                            FuenteFinanciacion fuentesFinanciacion = _context.FuenteFinanciacion.Find(cfn.FuenteFinanciacionId);

                                            fuentesFinanciacion.NombreFuente = fuentesFinanciacion != null ? !string.IsNullOrEmpty(fuentesFinanciacion.FuenteRecursosCodigo) ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(fuentesFinanciacion.FuenteRecursosCodigo, (int)EnumeratorTipoDominio.Fuentes_de_financiacion) : "" : "";
                                            cfn.FuenteFinanciacion = fuentesFinanciacion;
                                        }

                                        cpa.ComponenteFuenteNovedad = componenteFuenteNovedades;
                                    }

                                    strNombreAportante = aportante.NombreAportante != null ? aportante.NombreAportante : "";
                                    ValorAportante = aportante.ValorAporte != null ? "$" + String.Format("{0:n0}", aportante.ValorAporte) : "";
                                    strComponente = aportante.ComponenteAportanteNovedad.Count > 0 ? aportante.ComponenteAportanteNovedad.FirstOrDefault().NombreTipoComponente : "";
                                    strFase = aportante.ComponenteAportanteNovedad.Count > 0 ? aportante.ComponenteAportanteNovedad.FirstOrDefault().Nombrefase : "";
                                    strFuente = aportante.ComponenteAportanteNovedad.Count > 0 ? aportante.ComponenteAportanteNovedad.FirstOrDefault().ComponenteFuenteNovedad.FirstOrDefault().FuenteFinanciacion.NombreFuente : "";
                                    strTipoUso = aportante.ComponenteAportanteNovedad.FirstOrDefault().ComponenteFuenteNovedad.FirstOrDefault().ComponenteUsoNovedad.Count() > 0 ? aportante.ComponenteAportanteNovedad.FirstOrDefault().ComponenteFuenteNovedad.FirstOrDefault().ComponenteUsoNovedad.FirstOrDefault().NombreUso : "";
                                    valorUso = aportante.ComponenteAportanteNovedad.FirstOrDefault().ComponenteFuenteNovedad.FirstOrDefault().ComponenteUsoNovedad.Count() > 0 ? "$" + String.Format("{0:n0}", aportante.ComponenteAportanteNovedad.FirstOrDefault().ComponenteFuenteNovedad.FirstOrDefault().ComponenteUsoNovedad.FirstOrDefault().ValorUso) : "";
                                    Aportantes = Aportantes.Replace("[TP_NUMERO_APORTANTE]", (enumAportante++).ToString())
                                                            .Replace("[TP_NOMBRE_APORTANTE]", strNombreAportante)
                                                            .Replace("[TP_VALOR_APORTANTE]", ValorAportante)
                                                            .Replace("[TP_FUENTE]", strFuente)
                                                            .Replace("[TP_FASE]", strFase)
                                                            .Replace("[TP_COMPONENTE]", strComponente)
                                                            .Replace("[TP_USO]", strTipoUso)
                                                            .Replace("[TP_VALOR_USO]", valorUso);
                                }

                                if (novedadContractual.NovedadContractualAportante.Count > 0)
                                {
                                    NovedadesAdicion = NovedadesAdicion.Replace("[TP_APORTANTE]", Aportantes);
                                }
                                else
                                {
                                    NovedadesAdicion = NovedadesAdicion.Replace("[TP_APORTANTE]", "");

                                }

                                #endregion
                            }
                            if (codigotipoNovedadTemp == ConstanTiposNovedades.Prórroga)
                            {
                                existeProrroga = true;
                                NovedadesProrroga = NovedadesProrroga.Replace("[TP_PLAZO_DIAS]", item.PlazoAdicionalDias != null ? Math.Round((decimal)item.PlazoAdicionalDias, 0).ToString() : "");
                                NovedadesProrroga = NovedadesProrroga.Replace("[TP_PLAZO_MESES]", item.PlazoAdicionalMeses != null ? Math.Round((decimal)item.PlazoAdicionalMeses, 0).ToString() : "");
                            }

                            if (codigotipoNovedadTemp == ConstanTiposNovedades.Modificación_de_Condiciones_Contractuales)
                            {
                                existeModificacion = true;
                                enumClausula = 1;

                                foreach (var clausula in item.NovedadContractualClausula)
                                {
                                    if (clausula != null)
                                    {
                                        NovedadesModificacion = NovedadesModificacion.Replace("[TP_NUM_CLAUSULA]", (enumClausula++).ToString());
                                        NovedadesModificacion = NovedadesModificacion.Replace("[TP_CLAUSULA]", clausula.ClausulaAmodificar != null ? clausula.ClausulaAmodificar : string.Empty);
                                        NovedadesModificacion = NovedadesModificacion.Replace("[TP_AJUSTE_CLAUSULA]", clausula.AjusteSolicitadoAclausula != null ? clausula.AjusteSolicitadoAclausula : string.Empty);
                                    }
                                }
                            }
                        }
                    }

                    foreach (Dominio placeholderDominio in placeholders)
                    {
                        switch (placeholderDominio.Codigo)
                        {

                            case ConstanCodigoVariablesPlaceHolders.NOMBRE_MODIFICACION:
                                Historiales = Historiales.Replace(placeholderDominio.Nombre, (enumHistorial++).ToString());
                                break;

                            case ConstanCodigoVariablesPlaceHolders.NUMERO_SOLICITUD:
                                Historiales = Historiales.Replace(placeholderDominio.Nombre, novedadContractual.NumeroSolicitud);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.TIPO_NOVEDAD:
                                Historiales = Historiales.Replace(placeholderDominio.Nombre, tipoNovedadString);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.TP_NUMERO_COMITE_TECNICO:
                                Historiales = Historiales.Replace(placeholderDominio.Nombre, numeroComiteTecnico);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.TP_NUMERO_COMITE_FIDUCIARIO:
                                Historiales = Historiales.Replace(placeholderDominio.Nombre, numeroComiteFiduciario);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.TP_ESTADO:
                                Historiales = Historiales.Replace(placeholderDominio.Nombre, estado);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.TIPO_NOVEDAD_OTRAS:
                                if (existeOtro)
                                {
                                    Historiales = Historiales.Replace(placeholderDominio.Nombre, NovedadesOtras);
                                }
                                break;

                            case ConstanCodigoVariablesPlaceHolders.TIPO_NOVEDAD_ADICION:
                                if (existeAdicion)
                                {
                                    Historiales = Historiales.Replace(placeholderDominio.Nombre, NovedadesAdicion);
                                }
                                break;

                            case ConstanCodigoVariablesPlaceHolders.TIPO_NOVEDAD_PRORROGA:
                                if (existeProrroga)
                                {
                                    Historiales = Historiales.Replace(placeholderDominio.Nombre, NovedadesProrroga);
                                }
                                break;

                            case ConstanCodigoVariablesPlaceHolders.TIPO_NOVEDAD_MODIFICACION:
                                if (existeModificacion)
                                {
                                    Historiales = Historiales.Replace(placeholderDominio.Nombre, NovedadesModificacion);
                                }
                                break;
                        }
                    }
                }

                #endregion
                #region fuentes usos

                foreach (Dominio placeholderDominio in placeholders)
                {
                    //ConstanCodigoVariablesPlaceHolders placeholder = (ConstanCodigoVariablesPlaceHolders)placeholderDominio.Codigo.ToString();

                    switch (placeholderDominio.Codigo)
                    {

                        case ConstanCodigoVariablesPlaceHolders.NUMERO_SOLICITUD:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, novedadContractual.NumeroSolicitud);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.FECHA_SOLICITUD:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, novedadContractual.FechaSolictud != null ? ((DateTime)novedadContractual.FechaSolictud).ToString("dd-MM-yyyy") : " ");
                            break;

                        case ConstanCodigoVariablesPlaceHolders.TIPO_CONTROVERSIA:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, tipoNovedadString);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.TIPO_CONTRATO_CONTRATACION:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, ListaParametricas.Where(r => r.Codigo == contratacion.TipoSolicitudCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Componentes).FirstOrDefault().Nombre);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.CANTIDAD_DE_PROYECTOS_ASOCIADOS:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, contratacion != null ? contratacion.ContratacionProyecto.Count().ToString() : "");
                            break;
                        //Datos Contratista y contrato
                        case ConstanCodigoVariablesPlaceHolders.NUMERO_CONTRATO:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, contrato != null ? contrato.NumeroContrato : "");
                            break;

                        case ConstanCodigoVariablesPlaceHolders.CONTRATISTA_NOMBRE:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, contratista != null ? contratista.Nombre : "");
                            break;

                        case ConstanCodigoVariablesPlaceHolders.FECHA_INICIO_CONTRATO:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, contrato != null ? contrato.FechaActaInicioFase2.HasValue ? ((DateTime)contrato.FechaActaInicioFase2).ToString("dd-MM-yyyy") : contrato.FechaActaInicioFase1.HasValue ? ((DateTime)contrato.FechaActaInicioFase1).ToString("dd-MM-yyyy") : " " : " ");
                            break;

                        case ConstanCodigoVariablesPlaceHolders.FECHA_FIN_CONTRATO:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, contrato != null ? contrato.FechaTerminacionFase2.HasValue ? ((DateTime)contrato.FechaTerminacionFase2).ToString("dd-MM-yyyy") : contrato.FechaTerminacion.HasValue ? ((DateTime)contrato.FechaTerminacion).ToString("dd-MM-yyyy") : " " : " ");
                            break;

                        case ConstanCodigoVariablesPlaceHolders.PLAZO_OBRA_MESES:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, contratacion != null ? contratacion.PlazoContratacion.PlazoMeses + " Meses " + contratacion.PlazoContratacion.PlazoDias + " Días " : " ");
                            break;

                        //
                        case ConstanCodigoVariablesPlaceHolders.NUMERO_DDP:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, disponibilidadPresupuestal != null ? !string.IsNullOrEmpty(disponibilidadPresupuestal.NumeroDdp) ? disponibilidadPresupuestal.NumeroDdp : " " : " ");
                            break;

                        case ConstanCodigoVariablesPlaceHolders.NUMERO_DRP:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, disponibilidadPresupuestal != null ? !string.IsNullOrEmpty(disponibilidadPresupuestal.NumeroDrp) ? disponibilidadPresupuestal.NumeroDrp : " " : " ");
                            break;

                        case ConstanCodigoVariablesPlaceHolders.SALDO_APORTANTE:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, disponibilidadPresupuestal != null ? "$" + String.Format("{0:n0}", disponibilidadPresupuestal.ValorSolicitud + presupuestoAdicion) : " ");
                            break;

                        case ConstanCodigoVariablesPlaceHolders.DETALLES_PROYECTOS:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, DetallesProyectos);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.EJECUCION_PROYECTO:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, EjecucionesProyecto);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.REGISTRO_FUENTE_USO:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, RegistrosFuentesUso);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.HISTORIAL_MODIFICACIONES:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, Historiales);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.DETALLE_SOLICITUD_TAI:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, DetallesSolicitudes);
                            break;
                    }
                }

                #endregion fuentes usos

                return pPlantilla;
            }
            catch (Exception ex)
            {
                return ex.InnerException.ToString();
            }
        }

    }
}