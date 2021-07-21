using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{

    public class SelectionProcessService : ISelectionProcessService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public SelectionProcessService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _context = context;
            _commonService = commonService;
        }


        #region "Servicios Proceso Seleccion";

        public async Task<ActionResult<List<ProcesoSeleccion>>> GetSelectionProcess()
        {

            try
            {
                var procesosSeleccion = await _context.ProcesoSeleccion
                                            .Where(r => !(bool)r.Eliminado)
                                            .Include(r => r.ProcesoSeleccionIntegrante)
                                            .Include(r => r.ProcesoSeleccionObservacion)
                                            .Include(r => r.ProcesoSeleccionProponente)
                                            .Include(r => r.ProcesoSeleccionCotizacion)
                                            .Include(r => r.ProcesoSeleccionCronograma)
                                            .Include(r => r.ProcesoSeleccionGrupo)
                                            .OrderByDescending(x => x.FechaCreacion)
                                            .ToListAsync();


                List<Contratista> ListaContratistas = _context.Contratista.ToList();// Where(x => x.NumeroInvitacion == proceso.NumeroProceso).ToList();

                foreach (var proceso in procesosSeleccion)
                {
                    if (proceso.EstadoProcesoSeleccionCodigo == ConstanCodigoEstadoProcesoSeleccion.AprobadaAperturaPorComiteFiduciario
                        && proceso.TipoProcesoCodigo != ConstanCodigoTipoProcesoSeleccion.Invitacion_Privada)
                    {
                        if (
                                string.IsNullOrEmpty(proceso.EvaluacionDescripcion) ||
                                string.IsNullOrEmpty(proceso.UrlSoporteEvaluacion) ||
                                proceso.ProcesoSeleccionProponente.Count() == 0
                                )
                        {
                            proceso.EsCompleto = false;
                        }
                    }

                    proceso.ListaContratistas = ListaContratistas.Where(x => x.NumeroInvitacion == proceso.NumeroProceso).ToList();
                }
                return procesosSeleccion;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<ProcesoSeleccion> GetSelectionProcessById(int id)
        {
            try
            {
                var procesoSeleccion = await _context.ProcesoSeleccion.Where(r => !(bool)r.Eliminado)
                                            .Include(r => r.ProcesoSeleccionObservacion)
                                            .IncludeFilter(r => r.ProcesoSeleccionProponente.Where(r => !(bool)r.Eliminado))
                                            .IncludeFilter(r => r.ProcesoSeleccionIntegrante.Where(r => !(bool)r.Eliminado))
                                            .IncludeFilter(r => r.ProcesoSeleccionCotizacion.Where(r => !(bool)r.Eliminado))
                                            .IncludeFilter(r => r.ProcesoSeleccionCronograma.Where(r => !(bool)r.Eliminado))
                                            .IncludeFilter(r => r.ProcesoSeleccionGrupo.Where(r => !(bool)r.Eliminado))
                                            .FirstOrDefaultAsync(proceso => proceso.ProcesoSeleccionId == id);
                procesoSeleccion.ListaContratistas = _context.Contratista.Where(x => x.NumeroInvitacion == procesoSeleccion.NumeroProceso).ToList();
                procesoSeleccion.ProcesoSeleccionGrupo = procesoSeleccion.ProcesoSeleccionGrupo.Where(r => r.Eliminado != true).ToList();


                foreach (var proces in procesoSeleccion.ProcesoSeleccionProponente)
                {
                    if (proces.LocalizacionIdMunicipio == null)
                    {
                        proces.municipioString = string.Empty;
                        proces.departamentoString = string.Empty;
                    }
                    else
                    {
                        var municipio = _context.Localizacion.Find(proces.LocalizacionIdMunicipio);
                        if (municipio != null)
                        {
                            var departamento = _context.Localizacion.Find(municipio.IdPadre);
                            proces.municipioString = municipio.Descripcion;
                            proces.departamentoString = departamento.Descripcion;
                        }
                    }
                }

                return procesoSeleccion;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<Respuesta> CreateEditarProcesoSeleccion(ProcesoSeleccion procesoSeleccion)
        {
            Respuesta respuesta = new Respuesta();
            int idAccionCrearProcesoSeleccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Proceso_Seleccion, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            ProcesoSeleccion ProcesoSeleccionAntiguo = null;
            try
            {
                if (procesoSeleccion.ProcesoSeleccionId == 0)
                {
                    int countMax = _context.ProcesoSeleccion.Count(p => p.TipoProcesoCodigo == procesoSeleccion.TipoProcesoCodigo);

                    //Auditoria
                    strCrearEditar = "CREAR PROCESO SELECCION";
                    ProcesoSeleccion procesoSeleccionNew = new ProcesoSeleccion
                    {
                        NumeroProceso = Helpers.Helpers.Consecutive(procesoSeleccion.TipoProcesoCodigo, countMax),
                        Objeto = procesoSeleccion.Objeto,
                        AlcanceParticular = procesoSeleccion.AlcanceParticular,
                        Justificacion = procesoSeleccion.Justificacion,
                        CriteriosSeleccion = procesoSeleccion.CriteriosSeleccion,
                        TipoIntervencionCodigo = procesoSeleccion.TipoIntervencionCodigo,
                        TipoAlcanceCodigo = procesoSeleccion.TipoAlcanceCodigo, 
                        TipoProcesoCodigo = procesoSeleccion.TipoProcesoCodigo,
                        EsDistribucionGrupos = procesoSeleccion.EsDistribucionGrupos,
                        CantGrupos = procesoSeleccion.CantGrupos,
                        ResponsableTecnicoUsuarioId = procesoSeleccion.ResponsableTecnicoUsuarioId,
                        ResponsableEstructuradorUsuarioid = procesoSeleccion.ResponsableEstructuradorUsuarioid,
                        CondicionesJuridicasHabilitantes = procesoSeleccion.CondicionesJuridicasHabilitantes,
                        CondicionesTecnicasHabilitantes = procesoSeleccion.CondicionesTecnicasHabilitantes,
                        CondicionesAsignacionPuntaje = procesoSeleccion.CondicionesAsignacionPuntaje,
                        CantidadCotizaciones = procesoSeleccion.CantidadCotizaciones,
                        CantidadProponentes = procesoSeleccion.CantidadProponentes,
                        EstadoProcesoSeleccionCodigo = "1",
                        EtapaProcesoSeleccionCodigo = "1",
                        EvaluacionDescripcion = procesoSeleccion.EvaluacionDescripcion,
                        UrlSoporteEvaluacion = procesoSeleccion.UrlSoporteEvaluacion,
                        TipoOrdenEligibilidadCodigo = procesoSeleccion.TipoOrdenEligibilidadCodigo,
                        CantidadProponentesInvitados = procesoSeleccion.CantidadProponentesInvitados,
                        UrlSoporteProponentesSeleccionados = procesoSeleccion.UrlSoporteProponentesSeleccionados,
                        SolicitudId = procesoSeleccion.SolicitudId, 
                        UsuarioCreacion = procesoSeleccion.UsuarioCreacion,
                        FechaCreacion = DateTime.Now,
                        Eliminado = false,
                        EsCompleto = EsCompleto(procesoSeleccion), 
                    };
                    _context.ProcesoSeleccion.Add(procesoSeleccionNew);
                    _context.SaveChanges();
                    procesoSeleccion.ProcesoSeleccionId = procesoSeleccionNew.ProcesoSeleccionId;
                }
                else
                {
                    strCrearEditar = "EDIT PROCESO SELECCION";
                    ProcesoSeleccionAntiguo = _context.ProcesoSeleccion.Find(procesoSeleccion.ProcesoSeleccionId);
                    //Auditoria
                    //ProcesoSeleccionAntiguo.UsuarioModificacion = procesoSeleccion.UsuarioCreacion.ToUpper();
                    ProcesoSeleccionAntiguo.FechaModificacion = DateTime.Now;

                    //Registros
                    ProcesoSeleccionAntiguo.NumeroProceso = procesoSeleccion.NumeroProceso;
                    ProcesoSeleccionAntiguo.Objeto = procesoSeleccion.Objeto;
                    ProcesoSeleccionAntiguo.AlcanceParticular = procesoSeleccion.AlcanceParticular;
                    ProcesoSeleccionAntiguo.Justificacion = procesoSeleccion.Justificacion;
                    ProcesoSeleccionAntiguo.CriteriosSeleccion = procesoSeleccion.CriteriosSeleccion;
                    ProcesoSeleccionAntiguo.TipoIntervencionCodigo = procesoSeleccion.TipoIntervencionCodigo;
                    ProcesoSeleccionAntiguo.TipoAlcanceCodigo = procesoSeleccion.TipoAlcanceCodigo;
                    ProcesoSeleccionAntiguo.TipoProcesoCodigo = procesoSeleccion.TipoProcesoCodigo;
                    ProcesoSeleccionAntiguo.EsDistribucionGrupos = procesoSeleccion.EsDistribucionGrupos;
                    ProcesoSeleccionAntiguo.CantGrupos = procesoSeleccion.CantGrupos;
                    ProcesoSeleccionAntiguo.ResponsableTecnicoUsuarioId = procesoSeleccion.ResponsableTecnicoUsuarioId;
                    ProcesoSeleccionAntiguo.ResponsableEstructuradorUsuarioid = procesoSeleccion.ResponsableEstructuradorUsuarioid;
                    ProcesoSeleccionAntiguo.CondicionesJuridicasHabilitantes = procesoSeleccion.CondicionesJuridicasHabilitantes;
                    ProcesoSeleccionAntiguo.CondicionesFinancierasHabilitantes = procesoSeleccion.CondicionesFinancierasHabilitantes;
                    ProcesoSeleccionAntiguo.CondicionesTecnicasHabilitantes = procesoSeleccion.CondicionesTecnicasHabilitantes;
                    ProcesoSeleccionAntiguo.CondicionesAsignacionPuntaje = procesoSeleccion.CondicionesAsignacionPuntaje;
                    ProcesoSeleccionAntiguo.CantidadCotizaciones = procesoSeleccion.CantidadCotizaciones;
                    ProcesoSeleccionAntiguo.CantidadProponentes = procesoSeleccion.CantidadProponentes;
                    ProcesoSeleccionAntiguo.EstadoProcesoSeleccionCodigo = procesoSeleccion.EstadoProcesoSeleccionCodigo;
                    ProcesoSeleccionAntiguo.EtapaProcesoSeleccionCodigo = procesoSeleccion.EtapaProcesoSeleccionCodigo;
                    ProcesoSeleccionAntiguo.EvaluacionDescripcion = procesoSeleccion.EvaluacionDescripcion;
                    ProcesoSeleccionAntiguo.UrlSoporteEvaluacion = procesoSeleccion.UrlSoporteEvaluacion;
                    ProcesoSeleccionAntiguo.TipoOrdenEligibilidadCodigo = procesoSeleccion.TipoOrdenEligibilidadCodigo;
                    ProcesoSeleccionAntiguo.CantidadProponentesInvitados = procesoSeleccion.CantidadProponentesInvitados;
                    ProcesoSeleccionAntiguo.UrlSoporteProponentesSeleccionados = procesoSeleccion.UrlSoporteProponentesSeleccionados;
                    ProcesoSeleccionAntiguo.RegistroCompletoProponentes = ValidarRegistroCompletoProponente(ProcesoSeleccionAntiguo.ProcesoSeleccionProponente.ToList());

                    ProcesoSeleccionAntiguo.Eliminado = false;

                }

                if (procesoSeleccion.ProcesoSeleccionGrupo.Count() == 0 && procesoSeleccion.EsDistribucionGrupos != true)
                {
                    ProcesoSeleccionGrupo grupo = new ProcesoSeleccionGrupo
                    {
                        ProcesoSeleccionId = procesoSeleccion.ProcesoSeleccionId,
                        UsuarioCreacion = procesoSeleccion.UsuarioCreacion.ToUpper(),
                        FechaCreacion = DateTime.Now
                    };
                    procesoSeleccion.ProcesoSeleccionGrupo.Add(grupo);
                }

                foreach (ProcesoSeleccionGrupo grupo in procesoSeleccion.ProcesoSeleccionGrupo)
                {
                    grupo.UsuarioCreacion = procesoSeleccion.UsuarioCreacion.ToUpper();
                    grupo.ProcesoSeleccionId = procesoSeleccion.ProcesoSeleccionId;
                    this.CreateEditarProcesoSeleccionGrupo(grupo);
                }

                foreach (ProcesoSeleccionCronograma cronograma in procesoSeleccion.ProcesoSeleccionCronograma)
                {
                    cronograma.UsuarioCreacion = procesoSeleccion.UsuarioCreacion.ToUpper();
                    cronograma.ProcesoSeleccionId = procesoSeleccion.ProcesoSeleccionId;
                    this.CreateEditarProcesoSeleccionCronograma(cronograma, true);
                }

                foreach (ProcesoSeleccionCotizacion cotizacion in procesoSeleccion.ProcesoSeleccionCotizacion)
                {
                    cotizacion.ProcesoSeleccionId = procesoSeleccion.ProcesoSeleccionId;
                    cotizacion.UsuarioCreacion = procesoSeleccion.UsuarioCreacion.ToUpper();
                    cotizacion.NombreOrganizacion = cotizacion.NombreOrganizacion == null ? "" : cotizacion.NombreOrganizacion.ToUpper();
                    cotizacion.UrlSoporte = cotizacion.UrlSoporte == null ? "" : cotizacion.UrlSoporte.ToUpper();
                    this.CreateEditarProcesoSeleccionCotizacion(cotizacion);
                }

                //si la cantidad que recibe de parametros no es la misma que tiene en datos, borro los anteriores
                if (procesoSeleccion.ProcesoSeleccionProponente.Count() < _context.ProcesoSeleccionProponente.Where(x => x.ProcesoSeleccionId == procesoSeleccion.ProcesoSeleccionId && x.Eliminado != true).Count())
                {
                    foreach (var procesoseleccionprop in _context.ProcesoSeleccionProponente.Where(x => x.ProcesoSeleccionId == procesoSeleccion.ProcesoSeleccionId && !(bool)x.Eliminado))
                    {
                        procesoseleccionprop.Eliminado = true;
                        _context.ProcesoSeleccionProponente.Update(procesoseleccionprop);
                    }
                    _context.SaveChanges();
                }
                foreach (ProcesoSeleccionProponente proponente in procesoSeleccion.ProcesoSeleccionProponente)
                {
                    proponente.UsuarioCreacion = procesoSeleccion.UsuarioCreacion;
                    proponente.NombreRepresentanteLegal = proponente.NombreRepresentanteLegal;
                    proponente.NombreProponente = proponente.NombreProponente;
                    proponente.DireccionProponente = proponente.DireccionProponente;
                    proponente.EmailProponente = proponente.EmailProponente;
                    proponente.Eliminado = false;
                    proponente.ProcesoSeleccionId = procesoSeleccion.ProcesoSeleccionId;
                    this.CreateEditarProcesoSeleccionProponente(proponente);
                }

                foreach (ProcesoSeleccionIntegrante integrante in procesoSeleccion.ProcesoSeleccionIntegrante)
                {
                    integrante.ProcesoSeleccionId = procesoSeleccion.ProcesoSeleccionId;
                    integrante.UsuarioCreacion = procesoSeleccion.UsuarioCreacion.ToUpper();
                    integrante.NombreIntegrante = integrante.NombreIntegrante;
                    this.CreateEditarProcesoSeleccionIntegrante(integrante);
                }

                _context.SaveChanges();

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = procesoSeleccion,
                    Code = ConstantMessagesProcesoSeleccion.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.OperacionExitosa, idAccionCrearProcesoSeleccion, procesoSeleccion.UsuarioCreacion, strCrearEditar)

                };
            }
            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcesoSeleccion.ErrorInterno,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.ErrorInterno, idAccionCrearProcesoSeleccion, procesoSeleccion.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> DeleteProcesoSeleccion(int pId, string pUsuarioModificacion)
        {
            Respuesta respuesta = new Respuesta();
            int idAccionCrearProcesoSeleccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Proceso_Seleccion, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = "";
            ProcesoSeleccion ProcesoSeleccionAntiguo = null;
            try
            {
                //si tiene relacion con algo, no lo dejo eliminar
                var comite = _context.SesionComiteSolicitud.Where(x => x.SolicitudId == pId && !(bool)x.Eliminado && x.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion).Count();//jflorez. no me cuadra el nombre de la constante pero la pregunte 20201021
                if (comite > 0)
                {
                    return respuesta = new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Data = null,
                        Code = ConstantMessagesProcesoSeleccion.DependenciaEnEliminacion,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.DependenciaEnEliminacion, idAccionCrearProcesoSeleccion, pUsuarioModificacion, "ELIMINACIÓN CON DEPENDENCIA.")
                    };
                }

                strCrearEditar = "ELIMINAR PROCESO SELECCION";
                ProcesoSeleccionAntiguo = _context.ProcesoSeleccion.Find(pId);
                //Auditoria
                //ProcesoSeleccionAntiguo.UsuarioModificacion = pUsuarioModificacion;
                ProcesoSeleccionAntiguo.FechaModificacion = DateTime.Now;

                //Registros
                ProcesoSeleccionAntiguo.Eliminado = true;

                _context.ProcesoSeleccion.Update(ProcesoSeleccionAntiguo);



                await _context.SaveChangesAsync();

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcesoSeleccion.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.OperacionExitosa, idAccionCrearProcesoSeleccion, pUsuarioModificacion, strCrearEditar)

                };
            }
            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcesoSeleccion.ErrorInterno,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.ErrorInterno, idAccionCrearProcesoSeleccion, pUsuarioModificacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> ChangeStateProcesoSeleccion(int pId, string pUsuarioModificacion, string pCodigoEstado, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {
            Respuesta respuesta = new Respuesta();
            int idAccionCrearProcesoSeleccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Proceso_Seleccion, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = "";
            ProcesoSeleccion ProcesoSeleccionAntiguo = null;
            try
            {
                strCrearEditar = "CAMBIAR ESTADO PROCESO SELECCION";
                ProcesoSeleccionAntiguo = _context.ProcesoSeleccion.Find(pId);
                //Auditoria
                ProcesoSeleccionAntiguo.UsuarioModificacion = pUsuarioModificacion;
                ProcesoSeleccionAntiguo.FechaModificacion = DateTime.Now;

                //Registros
                ProcesoSeleccionAntiguo.EstadoProcesoSeleccionCodigo = pCodigoEstado;

                //_context.ProcesoSeleccion.Update(ProcesoSeleccionAntiguo);
                //si el estado es apertura tramite se debe enviar un mensaje a la secretaria de comite jflorez
                if (pCodigoEstado == ConstanCodigoEstadoProcesoSeleccion.Apertura_En_Tramite ||
                    pCodigoEstado == ConstanCodigoEstadoProcesoSeleccion.AprobacionDeSeleccionEnTramite)
                {
                    var usuariosecretario = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Secretario_Comite).Select(x => x.Usuario.Email).ToList();
                    foreach (var usuario in usuariosecretario)
                    {
                        Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.SolicitarApertura);
                        string template = TemplateRecoveryPassword.Contenido.Replace("_LinkF_", pDominioFront).Replace("[NumeroSol]", ProcesoSeleccionAntiguo.NumeroProceso).Replace("[FechaSol]", ProcesoSeleccionAntiguo.FechaCreacion.ToString("dd/MM/yy"));
                        bool blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuario, "Proceso de selección en tramite", template, pSentender, pPassword, pMailServer, pMailPort);
                    }


                }






                await _context.SaveChangesAsync();

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcesoSeleccion.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.OperacionExitosa, idAccionCrearProcesoSeleccion, pUsuarioModificacion, strCrearEditar)

                };
            }
            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcesoSeleccion.ErrorInterno,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.ErrorInterno, idAccionCrearProcesoSeleccion, pUsuarioModificacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        #endregion


        #region Servicios Cronograma;

        public async Task<ActionResult<List<ProcesoSeleccionCronograma>>> GetSelectionProcessSchedule()
        {
            return await _context.ProcesoSeleccionCronograma.Where(r => !(bool)r.Eliminado).ToListAsync();
        }

        public async Task<ProcesoSeleccionCronograma> GetSelectionProcessScheduleById(int id)
        {
            return await _context.ProcesoSeleccionCronograma.FindAsync(id);
        }

        public async Task<ActionResult<List<ProcesoSeleccionCronograma>>> GetScheduleBySelectionProcessId(int ProcesoSeleccionId)
        {
            return await _context.ProcesoSeleccionCronograma.Where(x => !(bool)x.Eliminado && x.ProcesoSeleccionId == ProcesoSeleccionId).ToListAsync();
        }

        //Listados de actvidades/Cronograma creadas
        public async Task<ActionResult<List<ProcesoSeleccionCronograma>>> GetRecordActivities(int ProcesoSeleccionId)
        {
            return await _context.ProcesoSeleccionCronograma.Where(r => !(bool)r.Eliminado && r.ProcesoSeleccionId == ProcesoSeleccionId).ToListAsync();
        }

        //Grilla de control Ajuste del cronograma
        public async Task<ActionResult<List<GrillaControlCronograma>>> GetControlGridSchedule()
        {
            List<ProcesoSeleccion> ListProcesoSeleccion = await _context.ProcesoSeleccion.Where(r => !(bool)r.Eliminado).ToListAsync();

            List<GrillaControlCronograma> ListGrillaControlCronograma = new List<GrillaControlCronograma>();

            foreach (var ProcesoSeleccion in ListProcesoSeleccion)
            {
                GrillaControlCronograma ControlCronogramaGrilla = new GrillaControlCronograma
                {
                    ProcesoSeleccionId = ProcesoSeleccion.ProcesoSeleccionId,
                    FechaCreacion = ProcesoSeleccion.FechaCreacion,
                    TipoProcesoCodigo = ProcesoSeleccion.TipoProcesoCodigo,
                    TipoProcesoCodigoText = ProcesoSeleccion.TipoProcesoCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(ProcesoSeleccion.TipoProcesoCodigo, (int)EnumeratorTipoDominio.Tipo_Proceso_Seleccion) : "",
                    NumeroProceso = ProcesoSeleccion.NumeroProceso,
                    EtapaProcesoSeleccionCodigo = ProcesoSeleccion.EtapaProcesoSeleccionCodigo,
                    EEtapaProcesoSeleccionText = ProcesoSeleccion.EtapaProcesoSeleccionCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(ProcesoSeleccion.EtapaProcesoSeleccionCodigo, (int)EnumeratorTipoDominio.Etapa_Proceso_Seleccion) : "",
                    EstadoProcesoSeleccionCodigo = ProcesoSeleccion.EstadoProcesoSeleccionCodigo,
                    EstadoProcesoSeleccionText = ProcesoSeleccion.EstadoProcesoSeleccionCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(ProcesoSeleccion.EstadoProcesoSeleccionCodigo, (int)EnumeratorTipoDominio.Estado_Proceso_Seleccion) : "",
                    EsCompleto = ProcesoSeleccion.EsCompleto.HasValue ? ProcesoSeleccion.EsCompleto.Value : false,
                    EsCompletoText = ProcesoSeleccion.EsCompleto.Value ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(Convert.ToInt32(ProcesoSeleccion.EsCompleto).ToString(), (int)EnumeratorTipoDominio.Estado_Registro) : "Incompleto",
                };
                ListGrillaControlCronograma.Add(ControlCronogramaGrilla);
            }

            return ListGrillaControlCronograma;
        }

        public Respuesta CreateEditarProcesoSeleccionCronograma(ProcesoSeleccionCronograma procesoSeleccionCronograma, bool esTransaccion)
        {
            Respuesta respuesta = new Respuesta();

            int idAccion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Acciones && r.Codigo == ConstantCodigoAcciones.Crear_Editar_ProcesoSeleccion_Cronograma).FirstOrDefault().DominioId;

            string strCrearEditar = "";
            ProcesoSeleccionCronograma procesoSeleccionCronogramaAntiguo = null;
            try
            {

                if (procesoSeleccionCronograma.ProcesoSeleccionCronogramaId == 0)
                {
                    //Auditoria
                    strCrearEditar = "CREAR PROCESO SELECCION CRONOGRAMA";
                    procesoSeleccionCronograma.FechaCreacion = DateTime.Now;
                    procesoSeleccionCronograma.Eliminado = false;
                    // procesoSeleccionCronograma.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;


                    _context.ProcesoSeleccionCronograma.Add(procesoSeleccionCronograma);
             
                }
                else
                {
                    strCrearEditar = "EDIT PROCESO SELECCION CRONOGRAMA";
                    procesoSeleccionCronogramaAntiguo = _context.ProcesoSeleccionCronograma.Find(procesoSeleccionCronograma.ProcesoSeleccionCronogramaId);
                    //Auditoria
                    procesoSeleccionCronogramaAntiguo.UsuarioModificacion = procesoSeleccionCronograma.UsuarioCreacion.ToUpper();
                    procesoSeleccionCronogramaAntiguo.FechaModificacion = DateTime.Now;

                    //Registros
                    procesoSeleccionCronogramaAntiguo.ProcesoSeleccionId = procesoSeleccionCronograma.ProcesoSeleccionId;

                    procesoSeleccionCronogramaAntiguo.NumeroActividad = procesoSeleccionCronograma.NumeroActividad;
                    procesoSeleccionCronogramaAntiguo.Descripcion = procesoSeleccionCronograma.Descripcion;
                    procesoSeleccionCronogramaAntiguo.FechaMaxima = procesoSeleccionCronograma.FechaMaxima;
                    procesoSeleccionCronogramaAntiguo.EstadoActividadCodigo = procesoSeleccionCronograma.EstadoActividadCodigo;
                    procesoSeleccionCronogramaAntiguo.EtapaActualProcesoCodigo = procesoSeleccionCronograma.EtapaActualProcesoCodigo;
                    //procesoSeleccionCronogramaAntiguo.FechaCreacion = procesoSeleccionCronograma.FechaCreacion;
                    //procesoSeleccionCronogramaAntiguo.UsuarioCreacion = "forozco"; ////HttpContext.User.FindFirst("User").Value
                    procesoSeleccionCronogramaAntiguo.Eliminado = false;
                    procesoSeleccionCronogramaAntiguo.FechaModificacion = DateTime.Now;


                    _context.ProcesoSeleccionCronograma.Update(procesoSeleccionCronogramaAntiguo);
                }

                if (esTransaccion)
                    return respuesta;
                else
                {

                    _context.SaveChangesAsync();

                    return respuesta = new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Data = procesoSeleccionCronograma,
                        Code = ConstantMessagesProcesoSeleccion.OperacionExitosa
                        //  Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.OperacionExitosa, idAccion, procesoSeleccionCronograma.UsuarioCreacion, strCrearEditar)

                    };

                }
            }
            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcesoSeleccion.ErrorInterno
                    // Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion_Cronograma, ConstantMessagesProcesoSeleccion.ErrorInterno, idAccion, procesoSeleccionCronograma.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        #endregion

        #region Servicios Proceso Seleccion Grupo;
        public Respuesta CreateEditarProcesoSeleccionGrupo(ProcesoSeleccionGrupo procesoSeleccionGrupo)
        {
            int idAccion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Acciones && r.Codigo == ConstantCodigoAcciones.Crear_Editar_ProcesoSeleccion_Grupo).FirstOrDefault().DominioId;

            string strCrearEditar = "";
            ProcesoSeleccionGrupo ProcesoSeleccionGrupoAntiguo = null;
            try
            {

                if (string.IsNullOrEmpty(procesoSeleccionGrupo.ProcesoSeleccionGrupoId.ToString()) || procesoSeleccionGrupo.ProcesoSeleccionGrupoId == 0)
                {
                    //Auditoria
                    strCrearEditar = "CREAR PROCESO SELECCION GRUPO";
                    procesoSeleccionGrupo.FechaCreacion = DateTime.Now;
                    procesoSeleccionGrupo.Eliminado = false;
                    procesoSeleccionGrupo.NombreGrupo = procesoSeleccionGrupo.NombreGrupo != null ? procesoSeleccionGrupo.NombreGrupo.ToUpper() : "";
                    _context.ProcesoSeleccionGrupo.Add(procesoSeleccionGrupo);
                }
                else
                {
                    strCrearEditar = "EDIT PROCESO SELECCION GRUPO";
                    ProcesoSeleccionGrupoAntiguo = _context.ProcesoSeleccionGrupo.Find(procesoSeleccionGrupo.ProcesoSeleccionGrupoId);
                    //Auditoria
                    ProcesoSeleccionGrupoAntiguo.UsuarioModificacion = procesoSeleccionGrupo.UsuarioCreacion.ToUpper();
                    ProcesoSeleccionGrupoAntiguo.NombreGrupo = procesoSeleccionGrupo.NombreGrupo == null ? "" : procesoSeleccionGrupo.NombreGrupo.ToUpper();
                    ProcesoSeleccionGrupoAntiguo.FechaModificacion = DateTime.Now;


                    //Registros

                    ProcesoSeleccionGrupoAntiguo.ProcesoSeleccionId = procesoSeleccionGrupo.ProcesoSeleccionId;
                    ProcesoSeleccionGrupoAntiguo.TipoPresupuestoCodigo = procesoSeleccionGrupo.TipoPresupuestoCodigo;
                    ProcesoSeleccionGrupoAntiguo.Valor = procesoSeleccionGrupo.Valor;
                    ProcesoSeleccionGrupoAntiguo.ValorMinimoCategoria = procesoSeleccionGrupo.ValorMinimoCategoria;
                    ProcesoSeleccionGrupoAntiguo.ValorMaximoCategoria = procesoSeleccionGrupo.ValorMaximoCategoria;
                    ProcesoSeleccionGrupoAntiguo.PlazoMeses = procesoSeleccionGrupo.PlazoMeses;
                    ProcesoSeleccionGrupoAntiguo.Eliminado = false;

                    _context.ProcesoSeleccionGrupo.Update(ProcesoSeleccionGrupoAntiguo);
                }

                return new Respuesta();
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcesoSeleccion.ErrorInterno
                    //    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion_Grupo, ConstantMessagesProcesoSeleccion.ErrorInterno, idAccion, ProcesoSeleccionGrupoAntiguo.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }
        #endregion

        #region Servicios Proceso Seleccion Cotizacion;

        public async Task<ActionResult<List<ProcesoSeleccionCotizacion>>> GetProcesoSeleccionCotizacion()
        {

            try
            {
                return await _context.ProcesoSeleccionCotizacion.Where(r => !(bool)r.Eliminado).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<ProcesoSeleccionCotizacion> GetProcesoSeleccionCotizacionById(int id)
        {
            return await _context.ProcesoSeleccionCotizacion.FindAsync(id);
        }

        public async Task<ActionResult<List<ProcesoSeleccionCotizacion>>> GetCotizacionByProcesoSeleccionId(int ProcesoSeleccionId)
        {
            try
            {
                return await _context.ProcesoSeleccionCotizacion.Where(x => !(bool)x.Eliminado && x.ProcesoSeleccionId == ProcesoSeleccionId).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Respuesta CreateEditarProcesoSeleccionCotizacion(ProcesoSeleccionCotizacion procesoSeleccionCotizacion)
        {
            Respuesta respuesta = new Respuesta();

            int idAccion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Acciones && r.Codigo == ConstantCodigoAcciones.Crear_Proceso_Seleccion).FirstOrDefault().DominioId;

            ProcesoSeleccionCotizacion ProcesoSeleccionCotizacionAntiguo = null;
            try
            {

                if (string.IsNullOrEmpty(procesoSeleccionCotizacion.ProcesoSeleccionCotizacionId.ToString()) || procesoSeleccionCotizacion.ProcesoSeleccionCotizacionId == 0)
                {
                    procesoSeleccionCotizacion.FechaCreacion = DateTime.Now;
                    procesoSeleccionCotizacion.Eliminado = false;
                    _context.ProcesoSeleccionCotizacion.Add(procesoSeleccionCotizacion);

                }
                else
                {
                    ProcesoSeleccionCotizacionAntiguo = _context.ProcesoSeleccionCotizacion.Find(procesoSeleccionCotizacion.ProcesoSeleccionCotizacionId);
                    //Auditoria
                    ProcesoSeleccionCotizacionAntiguo.UsuarioModificacion = procesoSeleccionCotizacion.UsuarioCreacion.ToUpper();
                    ProcesoSeleccionCotizacionAntiguo.FechaModificacion = DateTime.Now;

                    //Registros 
                    ProcesoSeleccionCotizacionAntiguo.ProcesoSeleccionCotizacionId = procesoSeleccionCotizacion.ProcesoSeleccionCotizacionId;
                    ProcesoSeleccionCotizacionAntiguo.NombreOrganizacion = procesoSeleccionCotizacion.NombreOrganizacion;
                    ProcesoSeleccionCotizacionAntiguo.ValorCotizacion = procesoSeleccionCotizacion.ValorCotizacion;
                    ProcesoSeleccionCotizacionAntiguo.Descripcion = procesoSeleccionCotizacion.Descripcion;
                    ProcesoSeleccionCotizacionAntiguo.UrlSoporte = procesoSeleccionCotizacion.UrlSoporte;
                    ProcesoSeleccionCotizacionAntiguo.Eliminado = false;
                    _context.ProcesoSeleccionCotizacion.Update(ProcesoSeleccionCotizacionAntiguo);
                }

                //await _context.SaveChangesAsync();

                return respuesta;
            }
            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcesoSeleccion.ErrorInterno,
                    // Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.ErrorInterno, idAccion, ProcesoSeleccionCotizacionAntiguo.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }
        #endregion

        #region Servicios Proceso Seleccion Proponente;

        public async Task<ProcesoSeleccionCronograma> GetProcesoSeleccionProponenteById(int id)
        {
            return await _context.ProcesoSeleccionCronograma.FindAsync(id);
        }

        public async Task<List<ProcesoSeleccionProponente>> GetProcesoSeleccionProponentes()
        {
            var proceso = await _context.ProcesoSeleccionProponente.Where(p => !(bool)p.Eliminado).ToListAsync();
            foreach (var proces in proceso)
            {
                if (proces.LocalizacionIdMunicipio == null)
                {
                    proces.municipioString = "Error municipio";
                    proces.departamentoString = "Error departamento";
                }
                else
                {
                    var municipio = _context.Localizacion.Find(proces.LocalizacionIdMunicipio);
                    if (municipio != null)
                    {
                        var departamento = _context.Localizacion.Find(municipio.IdPadre);
                        proces.municipioString = municipio.Descripcion;
                        proces.departamentoString = departamento.Descripcion;
                    } 
                } 
            }
            return proceso;

        }

        public async Task<ActionResult<List<GrillaProcesoSeleccionProponente>>> GetGridProcesoSeleccionProponente(int? procesoSeleccionId)
        {
            List<ProcesoSeleccionProponente> ListProcesoSeleccionProponente =
                (procesoSeleccionId != null ? await _context.ProcesoSeleccionProponente.Where(x => x.ProcesoSeleccionId == procesoSeleccionId).ToListAsync()
                : await _context.ProcesoSeleccionProponente.ToListAsync());


            List<GrillaProcesoSeleccionProponente> ListGrillaProcesoSeleccionProponente = new List<GrillaProcesoSeleccionProponente>();

            foreach (var PSP in ListProcesoSeleccionProponente)
            {
                GrillaProcesoSeleccionProponente ProcesoSeleccionProponenteGrilla = new GrillaProcesoSeleccionProponente
                {
                    ProcesoSeleccionProponenteId = PSP.ProcesoSeleccionProponenteId,
                    TipoProponenteCodigo = PSP.TipoProponenteCodigo,
                    TipoProponenteText = PSP.TipoProponenteCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(PSP.TipoProponenteCodigo, (int)EnumeratorTipoDominio.Tipo_Proponente) : "",
                    NombreProponente = PSP.NombreProponente != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(PSP.TipoProponenteCodigo, (int)EnumeratorTipoDominio.Nombre_Aportante_Aportante) : "",
                    TipoIdentificacionCodigo = PSP.TipoIdentificacionCodigo,
                    TipoIdentificaciontext = PSP.TipoIdentificacionCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(PSP.TipoIdentificacionCodigo, (int)EnumeratorTipoDominio.Tipo_Documento) : "",
                    NumeroIdentificacion = PSP.NumeroIdentificacion,
                    LocalizacionIdMunicipio = PSP.LocalizacionIdMunicipio,
                    DireccionProponente = PSP.DireccionProponente,
                    TelefonoProponente = PSP.TelefonoProponente,
                    EmailProponente = PSP.EmailProponente
                };

                ListGrillaProcesoSeleccionProponente.Add(ProcesoSeleccionProponenteGrilla);
            }

            return ListGrillaProcesoSeleccionProponente;
        }

        public Respuesta CreateEditarProcesoSeleccionProponente(ProcesoSeleccionProponente procesoSeleccionProponente)
        {
            Respuesta respuesta = new Respuesta();

            try
            { 
                if (procesoSeleccionProponente.ProcesoSeleccionProponenteId == 0)
                {
                    procesoSeleccionProponente.FechaCreacion = DateTime.Now;
                    procesoSeleccionProponente.UsuarioCreacion = procesoSeleccionProponente.UsuarioCreacion.ToUpper();
                    procesoSeleccionProponente.Eliminado = false;
                    procesoSeleccionProponente.RegistroCompleto = ValidarRegistroCompletoProponente(procesoSeleccionProponente);
                    _context.ProcesoSeleccionProponente.Add(procesoSeleccionProponente);
                    _context.SaveChanges();
                }
                else
                { 
                    _context.Set<ProcesoSeleccionProponente>()
                            .Where(r => r.ProcesoSeleccionProponenteId == procesoSeleccionProponente.ProcesoSeleccionProponenteId)
                            .Update(r => new ProcesoSeleccionProponente
                            {
                                FechaModificacion = DateTime.Now,
                                Eliminado = false,
                                UsuarioModificacion = procesoSeleccionProponente.UsuarioModificacion,
                                RegistroCompleto = ValidarRegistroCompletoProponente(procesoSeleccionProponente),
                                ProcesoSeleccionId = procesoSeleccionProponente.ProcesoSeleccionId,
                                TipoProponenteCodigo = procesoSeleccionProponente.TipoProponenteCodigo,
                                NombreProponente = procesoSeleccionProponente.NombreProponente,
                                TipoIdentificacionCodigo = procesoSeleccionProponente.TipoIdentificacionCodigo,
                                NombreRepresentanteLegal = procesoSeleccionProponente.NombreRepresentanteLegal,
                                CedulaRepresentanteLegal = procesoSeleccionProponente.CedulaRepresentanteLegal,
                                NumeroIdentificacion = procesoSeleccionProponente.NumeroIdentificacion,
                                LocalizacionIdMunicipio = procesoSeleccionProponente.LocalizacionIdMunicipio,
                                DireccionProponente= procesoSeleccionProponente.DireccionProponente,
                                TelefonoProponente = procesoSeleccionProponente.TelefonoProponente,
                                EmailProponente = procesoSeleccionProponente.EmailProponente
                            });
                     
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcesoSeleccion.ErrorInterno,
                    // Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.ErrorInterno, idAccion, procesoSeleccionProponente.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        private bool? ValidarRegistroCompletoProponente(ProcesoSeleccionProponente procesoSeleccionProponente)
        {
            return (
                  !string.IsNullOrEmpty(procesoSeleccionProponente.NombreProponente)
               || !string.IsNullOrEmpty(procesoSeleccionProponente.NumeroIdentificacion)
               || !string.IsNullOrEmpty(procesoSeleccionProponente.LocalizacionIdMunicipio)
               || !string.IsNullOrEmpty(procesoSeleccionProponente.DireccionProponente)
               || !string.IsNullOrEmpty(procesoSeleccionProponente.TelefonoProponente)
               || !string.IsNullOrEmpty(procesoSeleccionProponente.EmailProponente)
                );
        }

        private bool? ValidarRegistroCompletoProponente(List<ProcesoSeleccionProponente> ListProcesoSeleccionProponente)
        {
            foreach (var procesoSeleccionProponente in ListProcesoSeleccionProponente)
            {
                if (ValidarRegistroCompletoProponente(procesoSeleccionProponente) == false)
                    return false;
            }
            return true;
        }
        #endregion

        #region Servicios Proceso Seleccion Integrante;

        public async Task<ProcesoSeleccionIntegrante> GetProcesoSeleccionIntegranteById(int id)
        {
            return await _context.ProcesoSeleccionIntegrante.FindAsync(id);
        }

        public async Task<ActionResult<List<GrillaProcesoSeleccionIntegrante>>> GetGridProcesoSeleccionIntegrante(int? procesoSeleccionId)
        {
            List<ProcesoSeleccionIntegrante> ListProcesoSeleccionIntegrante =
                (procesoSeleccionId != null ? await _context.ProcesoSeleccionIntegrante.Where(r => !(bool)r.Eliminado && r.ProcesoSeleccionId == procesoSeleccionId).ToListAsync()
                : await _context.ProcesoSeleccionIntegrante.Where(r => !(bool)r.Eliminado).ToListAsync());


            List<GrillaProcesoSeleccionIntegrante> ListGrillaProcesoSeleccionIntegrante = new List<GrillaProcesoSeleccionIntegrante>();

            foreach (var PSI in ListProcesoSeleccionIntegrante)
            {
                GrillaProcesoSeleccionIntegrante ProcesoSeleccionIntegranteGrilla = new GrillaProcesoSeleccionIntegrante
                {
                    ProcesoSeleccionIntegranteId = PSI.ProcesoSeleccionIntegranteId,
                    ProcesoSeleccionId = PSI.ProcesoSeleccionId,
                    PorcentajeParticipacion = PSI.PorcentajeParticipacion,
                    NombreIntegrante = PSI.NombreIntegrante,
                    FechaCreacion = PSI.FechaCreacion,
                };

                ListGrillaProcesoSeleccionIntegrante.Add(ProcesoSeleccionIntegranteGrilla);
            }

            return ListGrillaProcesoSeleccionIntegrante;
        }

        public Respuesta CreateEditarProcesoSeleccionIntegrante(ProcesoSeleccionIntegrante procesoSeleccionIntegrante)
        {
            try
            {

                if (string.IsNullOrEmpty(procesoSeleccionIntegrante.ProcesoSeleccionIntegranteId.ToString()) || procesoSeleccionIntegrante.ProcesoSeleccionIntegranteId == 0)
                {
                    procesoSeleccionIntegrante.FechaCreacion = DateTime.Now;
                    procesoSeleccionIntegrante.UsuarioCreacion = procesoSeleccionIntegrante.UsuarioCreacion.ToUpper();
                    procesoSeleccionIntegrante.Eliminado = false;
                    _context.ProcesoSeleccionIntegrante.Add(procesoSeleccionIntegrante);
                }
                else
                {
                    ProcesoSeleccionIntegrante procesoSeleccionIntegranteAntiguo = null;
                    procesoSeleccionIntegranteAntiguo = _context.ProcesoSeleccionIntegrante.Find(procesoSeleccionIntegrante.ProcesoSeleccionIntegranteId);
                    //Registros 
                    procesoSeleccionIntegranteAntiguo.ProcesoSeleccionId = procesoSeleccionIntegrante.ProcesoSeleccionId;
                    procesoSeleccionIntegranteAntiguo.ProcesoSeleccionId = procesoSeleccionIntegrante.ProcesoSeleccionId;
                    procesoSeleccionIntegranteAntiguo.PorcentajeParticipacion = procesoSeleccionIntegrante.PorcentajeParticipacion;
                    procesoSeleccionIntegranteAntiguo.NombreIntegrante = procesoSeleccionIntegrante.NombreIntegrante;
                    procesoSeleccionIntegranteAntiguo.Eliminado = false;
                    procesoSeleccionIntegranteAntiguo.FechaModificacion = DateTime.Now;
                    procesoSeleccionIntegranteAntiguo.UsuarioModificacion = procesoSeleccionIntegrante.UsuarioModificacion;
                    _context.ProcesoSeleccionIntegrante.Update(procesoSeleccionIntegranteAntiguo);
                }

                return new Respuesta();
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcesoSeleccion.ErrorInterno,
                    //  Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.ErrorInterno, idAccion, procesoSeleccionIntegrante.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }
        #endregion

        //Registrar Seguimiento cronograma
        public async Task<Respuesta> CreateEditarCronogramaSeguimiento(CronogramaSeguimiento cronogramaSeguimiento)
        {
            Respuesta respuesta = new Respuesta();

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Cronograma_Seguimiento, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = "";
            CronogramaSeguimiento cronogramaSeguimientoAntiguo = null;
            try
            {

                if (string.IsNullOrEmpty(cronogramaSeguimiento.CronogramaSeguimientoId.ToString()) || cronogramaSeguimiento.CronogramaSeguimientoId == 0)
                {
                    if (cronogramaSeguimiento.EstadoActividadFinalCodigo != null)
                    {
                        //Auditoria
                        strCrearEditar = "CREAR CRONOGRAMA SEGUIMIENTO";
                        cronogramaSeguimiento.FechaCreacion = DateTime.Now;
                        cronogramaSeguimiento.Eliminado = false;
                        cronogramaSeguimiento.EstadoActividadInicialCodigo = cronogramaSeguimiento.EstadoActividadInicialCodigo == null ? "1" : cronogramaSeguimiento.EstadoActividadInicialCodigo;//revisar
                        cronogramaSeguimiento.UsuarioCreacion = cronogramaSeguimiento.UsuarioCreacion.ToUpper();


                        _context.CronogramaSeguimiento.Add(cronogramaSeguimiento);
                        _context.SaveChanges();
                        return respuesta = new Respuesta
                        {
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Data = cronogramaSeguimiento,
                            Code = ConstantMessagesProcesoSeleccion.OperacionExitosa,
                            Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion_Cronograma, ConstantMessagesProcesoSeleccion.OperacionExitosa, idAccion, cronogramaSeguimiento.UsuarioCreacion, strCrearEditar)
                        };
                    }
                    else
                    {
                        return respuesta = new Respuesta
                        {
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Data = cronogramaSeguimiento,
                            Code = ConstantMessagesProcesoSeleccion.CamposIncompletos,
                            Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion_Cronograma, ConstantMessagesProcesoSeleccion.CamposIncompletos, idAccion, cronogramaSeguimiento.UsuarioCreacion, strCrearEditar)
                        };
                    }


                }
                else
                {
                    strCrearEditar = "EDITAR CRONOGRAMA SEGUIMIENTO";
                    cronogramaSeguimientoAntiguo = _context.CronogramaSeguimiento.Find(cronogramaSeguimiento.CronogramaSeguimientoId);
                    //Auditoria
                    cronogramaSeguimientoAntiguo.UsuarioModificacion = cronogramaSeguimiento.UsuarioModificacion;
                    cronogramaSeguimientoAntiguo.FechaModificacion = DateTime.Now;
                    cronogramaSeguimientoAntiguo.Eliminado = false;


                    //Registros
                    cronogramaSeguimientoAntiguo.ProcesoSeleccionCronogramaId = cronogramaSeguimiento.ProcesoSeleccionCronogramaId;
                    cronogramaSeguimientoAntiguo.EstadoActividadInicialCodigo = cronogramaSeguimiento.EstadoActividadInicialCodigo;
                    cronogramaSeguimientoAntiguo.EstadoActividadFinalCodigo = cronogramaSeguimiento.EstadoActividadFinalCodigo;
                    cronogramaSeguimientoAntiguo.Observacion = cronogramaSeguimiento.Observacion;

                    _context.CronogramaSeguimiento.Update(cronogramaSeguimientoAntiguo);
                }

                await _context.SaveChangesAsync();

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = cronogramaSeguimientoAntiguo,
                    Code = ConstantMessagesProcesoSeleccion.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CronogramaSeguimiento, ConstantMessagesProcesoSeleccion.OperacionExitosa, idAccion, cronogramaSeguimiento.UsuarioCreacion, strCrearEditar)
                };
            }
            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcesoSeleccion.ErrorInterno,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CronogramaSeguimiento, ConstantMessagesProcesoSeleccion.ErrorInterno, idAccion, cronogramaSeguimiento.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        //Registrar Seguimiento cronograma
        public async Task<Respuesta> CreateContractorsFromProponent(ProcesoSeleccion pProcesoSeleccion, string pUsuarioCreo)
        {
            Respuesta respuesta = new Respuesta();

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_contratistas_desde_proponentes, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = "";
            CronogramaSeguimiento cronogramaSeguimientoAntiguo = null;
            try
            {

                pProcesoSeleccion.ProcesoSeleccionProponente.ToList().ForEach(p =>
                {
                    Contratista contratista = _context.Contratista.Where(x => x.ProcesoSeleccionProponenteId == p.ProcesoSeleccionProponenteId).FirstOrDefault();

                    if (contratista != null)
                    {
                        contratista.Activo = !p.Eliminado;
                    }
                    else
                    {
                        contratista = new Contratista();

                        contratista.TipoIdentificacionCodigo = (p.TipoProponenteCodigo == "4" || p.TipoProponenteCodigo == "2") ? "3" : "1"; //Nit - cedula
                        contratista.NumeroIdentificacion = string.IsNullOrEmpty(p.NumeroIdentificacion) ? "0" : p.NumeroIdentificacion;
                        contratista.Nombre = p.NombreProponente;
                        contratista.RepresentanteLegal = string.IsNullOrEmpty(p.NombreRepresentanteLegal) ? p.NombreProponente : p.NombreRepresentanteLegal;
                        contratista.RepresentanteLegalNumeroIdentificacion = string.IsNullOrEmpty(p.NombreRepresentanteLegal) ? "" : p.CedulaRepresentanteLegal;
                        contratista.NumeroInvitacion = pProcesoSeleccion.NumeroProceso;
                        contratista.TipoProponenteCodigo = p.TipoProponenteCodigo;
                        contratista.Activo = !p.Eliminado;
                        contratista.FechaCreacion = DateTime.Now;
                        contratista.UsuarioCreacion = pUsuarioCreo.ToUpper();
                        contratista.ProcesoSeleccionProponenteId = p.ProcesoSeleccionProponenteId;

                        _context.Contratista.Add(contratista);
                    }



                });

                var procesosel = _context.ProcesoSeleccion.Where(x => x.NumeroProceso == pProcesoSeleccion.NumeroProceso).FirstOrDefault();
                procesosel.CantidadProponentes = pProcesoSeleccion.CantidadProponentes;
                procesosel.UrlSoporteProponentesSeleccionados = pProcesoSeleccion.UrlSoporteProponentesSeleccionados;
                _context.ProcesoSeleccion.Update(procesosel);


                await _context.SaveChangesAsync();

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = cronogramaSeguimientoAntiguo,
                    Code = ConstantMessagesProcesoSeleccion.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.OperacionExitosa, idAccion, pUsuarioCreo, strCrearEditar)
                };
            }
            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcesoSeleccion.ErrorInterno,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CronogramaSeguimiento, ConstantMessagesProcesoSeleccion.ErrorInterno, idAccion, pUsuarioCreo, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        //Grilla Seguimiento a cronograma
        // public async Task<ActionResult<List<GrillaCronogramaSeguimiento>>> GetViewSchedules(int? ProcesoSeleccionCronogramaId)
        // {
        //     List<CronogramaSeguimiento> ListCronogramaSeguimiento = (ProcesoSeleccionCronogramaId != null ? await _context.CronogramaSeguimiento.Where(r => !(bool)r.Eliminado && r.ProcesoSeleccionCronogramaId == ProcesoSeleccionCronogramaId).ToListAsync()
        //       : await _context.CronogramaSeguimiento.Where(r => !(bool)r.Eliminado).ToListAsync());


        //     List<GrillaCronogramaSeguimiento> ListGrillaCronogramaSeguimiento = new List<GrillaCronogramaSeguimiento>();

        //     foreach (var cronogramaSeguimiento in ListCronogramaSeguimiento)
        //     {
        //         GrillaCronogramaSeguimiento CronogramaSeguimiento = new GrillaCronogramaSeguimiento
        //         {
        //             CronogramaSeguimientoId = cronogramaSeguimiento.CronogramaSeguimientoId,
        //             ProcesoSeleccionCronogramaId = cronogramaSeguimiento.ProcesoSeleccionCronogramaId,
        //             EstadoActividadInicialCodigo = cronogramaSeguimiento.EstadoActividadInicialCodigo,
        //             EstadoActividadInicialText = cronogramaSeguimiento.EstadoActividadInicialCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(cronogramaSeguimiento.EstadoActividadInicialCodigo, (int)EnumeratorTipoDominio.Estado_Cronograma_Seguimiento) : "",
        //             EstadoActividadFinalCodigo = cronogramaSeguimiento.EstadoActividadFinalCodigo,
        //             EstadoActividadFinalText = cronogramaSeguimiento.EstadoActividadFinalCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(cronogramaSeguimiento.EstadoActividadFinalCodigo, (int)EnumeratorTipoDominio.Estado_Cronograma_Seguimiento) : "",
        //             Observacion = cronogramaSeguimiento.Observacion,
        //             FechaCreacion = cronogramaSeguimiento.FechaCreacion,

        //         };
        //         ListGrillaCronogramaSeguimiento.Add(CronogramaSeguimiento);
        //     }

        //     return ListGrillaCronogramaSeguimiento;
        // }

        public async Task<Respuesta> SetValidateCargueMasivo(IFormFile pFile, string pFilePatch, string pUsuarioCreo, int pProcesoSeleccion)
        {
            int CantidadRegistrosVacios = 0;
            int CantidadResgistrosValidos = 0;
            int CantidadRegistrosInvalidos = 0;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //int OrigenId = await _commonService.GetDominioIdByCodigoAndTipoDominio(OrigenArchivoCargue.Proyecto, (int)EnumeratorTipoDominio.Origen_Documento_Cargue);
            DocumentService _documentService = new DocumentService(_context, _commonService);

            ArchivoCargue archivoCarge = await _documentService.getSaveFile(pFile, pFilePatch, Int32.Parse(OrigenArchivoCargue.OrdeELegibilidad), pProcesoSeleccion);

            // if (!string.IsNullOrEmpty(archivoCarge.ArchivoCargueId.ToString()))
            if (archivoCarge != null)
            {
                using (var stream = new MemoryStream())
                {
                    await pFile.CopyToAsync(stream);

                    using var package = new ExcelPackage(stream);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.LastOrDefault();
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


                            TempOrdenLegibilidad temp = new TempOrdenLegibilidad();
                            //Auditoria
                            temp.ArchivoCargueId = archivoCarge.ArchivoCargueId;
                            temp.EstaValidado = false;
                            temp.FechaCreacion = DateTime.Now;
                            temp.UsuarioCreacion = pUsuarioCreo.ToUpper();

                            // #1
                            //Tipo proponente
                            temp.TipoProponenteId = Int32.Parse(await _commonService.GetDominioCodigoByNombreDominioAndTipoDominio(worksheet.Cells[i, 1].Text, (int)EnumeratorTipoDominio.Tipo_Proponente));


                            //#2
                            //Nombre proponente
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 2].Text))
                            {
                                temp.NombreProponente = worksheet.Cells[i, 2].Text.ToUpper();
                            }

                            //#3
                            //Identificacion del proponente
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 3].Text)) { temp.NumeroIddentificacionProponente = worksheet.Cells[i, 3].Text; } else { temp.NumeroIddentificacionProponente = string.Empty; }


                            //#5
                            //Departamento domicilio proponente
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 4].Text)) { temp.Departamento = await _commonService.GetLocalizacionIdByName(worksheet.Cells[i, 4].Text, "0"); } else { temp.Departamento = 0; }

                            //#5
                            //Municipio proponente///aqui debe recibir el parametro iddepartamento, pueden haber municipios del mismo nombre para diferente departamento
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 5].Text))
                            {
                                int DepartamentoId = temp.Departamento = await _commonService.GetLocalizacionIdByName(worksheet.Cells[i, 4].Text, "0");
                                temp.Minicipio = await _commonService.GetLocalizacionIdByName(worksheet.Cells[i, 5].Text, DepartamentoId.ToString());//temp.Departamento.ToString()

                            }
                            else
                            {
                                temp.Minicipio = 0;
                            }

                            //#6
                            //Direccion del proponente
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 6].Text)) { temp.Direccion = Convert.ToString(worksheet.Cells[i, 6].Text).ToUpper(); } else { temp.Direccion = string.Empty; }

                            //#7
                            //Telefono del proponente
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 7].Text)) { temp.Telefono = Convert.ToString(worksheet.Cells[i, 7].Text); } else { temp.Telefono = string.Empty; }

                            //#8
                            //Correo del proponente
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 8].Text)) { temp.Correo = Convert.ToString(worksheet.Cells[i, 8].Text).ToUpper(); } else { temp.Correo = string.Empty; }

                            //#9
                            //Correo del proponente
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 9].Text)) { temp.NombreEntidad = Convert.ToString(worksheet.Cells[i, 9].Text); } else { temp.NombreEntidad = string.Empty; }

                            //#10
                            //Correo del proponente
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 10].Text))
                            {
                                temp.IdentificacionTributaria = Convert.ToInt32(worksheet.Cells[i, 10].Text) > 0 ? Int32.Parse(worksheet.Cells[i, 10].Text) : 0;
                            }

                            //#11
                            //Correo del proponente
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 11].Text))
                            {
                                temp.RepresentanteLegal = Convert.ToString(worksheet.Cells[i, 11].Text) != string.Empty ? Convert.ToString(worksheet.Cells[i, 11].Text).ToUpper() : "";
                            }
                            else
                            {
                                temp.RepresentanteLegal = string.Empty;
                            }

                            //#12
                            //Correo del proponente
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 12].Text))
                            {
                                temp.CedulaRepresentanteLegal = Convert.ToInt32(worksheet.Cells[i, 12].Text) > 0 ? Int32.Parse(worksheet.Cells[i, 12].Text) : 0;
                            }


                            //#13
                            //Correo del proponente
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 13].Text))
                            {
                                temp.DepartamentoRl = await _commonService.GetLocalizacionIdByName(worksheet.Cells[i, 13].Text, "0");
                            }

                            //#14
                            //Municipio representante legal
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 14].Text))
                            {
                                int DepartamentoIdRL = temp.Departamento = await _commonService.GetLocalizacionIdByName(worksheet.Cells[i, 13].Text, "0");
                                temp.MunucipioRl = await _commonService.GetLocalizacionIdByName(worksheet.Cells[i, 14].Text, DepartamentoIdRL.ToString());//temp.Departamento.ToString()
                            }

                            //#15
                            //Direccion Representante Legal
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 15].Text)) { temp.DireccionRl = worksheet.Cells[i, 15].Text.ToUpper(); } else { temp.DireccionRl = string.Empty; }

                            //#16
                            //Telefono Representante Legal
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 16].Text)) { temp.TelefonoRl = worksheet.Cells[i, 16].Text; } else { temp.TelefonoRl = string.Empty; }

                            //#17
                            //Correo Representante Legal
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 17].Text)) { temp.CorreoRl = worksheet.Cells[i, 17].Text.ToUpper(); } else { temp.CorreoRl = string.Empty; }

                            //#18
                            //Nombre del representante legal del la UT o consorcio
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 18].Text)) { temp.NombreOtoConsorcio = worksheet.Cells[i, 18].Text.ToUpper(); } else { temp.NombreOtoConsorcio = string.Empty; }

                            //#19
                            //Entiddaes que integran la union temporal
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 19].Text))
                            {
                                temp.EntiddaesQueIntegranLaUnionTemporal = Convert.ToInt32(worksheet.Cells[i, 19].Text) >= 0 ? Int32.Parse(worksheet.Cells[i, 19].Text) : 0;
                            }


                            //#20
                            //Nombre integrante
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 20].Text)) { temp.NombreIntegrante = worksheet.Cells[i, 20].Text.ToUpper(); } else { temp.NombreIntegrante = string.Empty; }


                            //#21
                            //Porcentaje participacion
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 21].Text))
                            {
                                temp.PorcentajeParticipacion = Convert.ToDecimal(worksheet.Cells[i, 21].Text.Replace("%", "")) >= 0 ? Int32.Parse(worksheet.Cells[i, 21].Text.Replace("%", "")) : 0;
                            }


                            //#26
                            //Nombre  del representante legal de la UT o consorcio
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 26].Text)) { temp.NombreRlutoConsorcio = worksheet.Cells[i, 26].Text.ToUpper(); } else { temp.NombreRlutoConsorcio = string.Empty; }

                            //#27
                            //Cedula  del representante legal de la UT o consorcio
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 27].Text))
                            {
                                temp.CcrlutoConsorcio = Convert.ToInt32(worksheet.Cells[i, 27].Text) > 0 ? Int32.Parse(worksheet.Cells[i, 27].Text) : 0;
                            }

                            //#28
                            //Departamento union union temporal
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 28].Text))
                            {
                                temp.DepartamentoRlutoConsorcio = await _commonService.GetLocalizacionIdByName(worksheet.Cells[i, 28].Text, "0");
                            }

                            //#29
                            //Nombre integrante 3
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 29].Text))
                            {
                                int DepartamentoIdConsorcio = temp.Departamento = await _commonService.GetLocalizacionIdByName(worksheet.Cells[i, 28].Text, "0");
                                temp.MinicipioRlutoConsorcio = await _commonService.GetLocalizacionIdByName(worksheet.Cells[i, 29].Text, DepartamentoIdConsorcio.ToString());
                            }

                            //#30
                            //Direccion  del representante legal de la UT o consorcio
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 30].Text)) { temp.DireccionRlutoConsorcio = worksheet.Cells[i, 30].Text.ToUpper(); } else { temp.DireccionRlutoConsorcio = string.Empty; }

                            //#31
                            //Telefono  del representante legal de la UT o consorcio
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 31].Text)) { temp.TelefonoRlutoConsorcio = worksheet.Cells[i, 31].Text; } else { temp.TelefonoRlutoConsorcio = string.Empty; }

                            //#32
                            //Correo  del representante legal de la UT o consorcio
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 32].Text)) { temp.CorreoRlutoConsorcio = worksheet.Cells[i, 32].Text.ToUpper(); } else { temp.CorreoRlutoConsorcio = string.Empty; }



                            //#28
                            //Municipio  del representante legal de la UT o consorcio
                            //if (!string.IsNullOrEmpty(worksheet.Cells[i, 28].Text))
                            //{
                            //    int DepartamentoIdConsorcio = temp.Departamento = await _commonService.GetLocalizacionIdByName(worksheet.Cells[i, 28].Text, "0");
                            //    temp.MinicipioRlutoConsorcio = await _commonService.GetLocalizacionIdByName(worksheet.Cells[i, 28].Text, DepartamentoIdConsorcio.ToString());
                            //}




                            //Guarda Cambios en una tabla temporal
                            if ((worksheet.Cells[i, 1].Text == "Persona Natural" && (
                             !string.IsNullOrEmpty(worksheet.Cells[i, 2].Text) |
                                !string.IsNullOrEmpty(worksheet.Cells[i, 3].Text) |
                                !string.IsNullOrEmpty(worksheet.Cells[i, 4].Text) |
                                !string.IsNullOrEmpty(worksheet.Cells[i, 5].Text) |
                                !string.IsNullOrEmpty(worksheet.Cells[i, 6].Text) |
                                !string.IsNullOrEmpty(worksheet.Cells[i, 7].Text) |
                                !string.IsNullOrEmpty(worksheet.Cells[i, 8].Text)))
                                ||
                            (worksheet.Cells[i, 1].Text == "Persona Juridica - Individual" && (
                                !string.IsNullOrEmpty(worksheet.Cells[i, 9].Text) |
                                !string.IsNullOrEmpty(worksheet.Cells[i, 10].Text) |
                                !string.IsNullOrEmpty(worksheet.Cells[i, 11].Text) |
                                !string.IsNullOrEmpty(worksheet.Cells[i, 12].Text) |
                                !string.IsNullOrEmpty(worksheet.Cells[i, 13].Text) |
                                !string.IsNullOrEmpty(worksheet.Cells[i, 14].Text) |
                                !string.IsNullOrEmpty(worksheet.Cells[i, 15].Text) |
                                !string.IsNullOrEmpty(worksheet.Cells[i, 16].Text) |
                                !string.IsNullOrEmpty(worksheet.Cells[i, 17].Text)))
                                ||
                            (worksheet.Cells[i, 1].Text == "Persona Juridica - Unión Temporal o Consorcio" && (
                                !string.IsNullOrEmpty(worksheet.Cells[i, 18].Text) |
                                !string.IsNullOrEmpty(worksheet.Cells[i, 19].Text) |
                                !string.IsNullOrEmpty(worksheet.Cells[i, 20].Text) |
                                !string.IsNullOrEmpty(worksheet.Cells[i, 21].Text) |
                                //!string.IsNullOrEmpty(worksheet.Cells[i, 22].Text) |
                                //!string.IsNullOrEmpty(worksheet.Cells[i, 23].Text) |
                                //!string.IsNullOrEmpty(worksheet.Cells[i, 24].Text) |
                                //!string.IsNullOrEmpty(worksheet.Cells[i, 25].Text) |
                                !string.IsNullOrEmpty(worksheet.Cells[i, 26].Text) |
                                !string.IsNullOrEmpty(worksheet.Cells[i, 27].Text) |
                                !string.IsNullOrEmpty(worksheet.Cells[i, 28].Text) |
                                !string.IsNullOrEmpty(worksheet.Cells[i, 29].Text) |
                                !string.IsNullOrEmpty(worksheet.Cells[i, 30].Text) |
                                !string.IsNullOrEmpty(worksheet.Cells[i, 31].Text) |
                                !string.IsNullOrEmpty(worksheet.Cells[i, 32].Text)
                                ))
                            )
                            {
                                //worksheet.Cells[2, 2].AddComment("Es valido", "Admin");

                                temp.EstaValidado = true;
                                _context.TempOrdenLegibilidad.Add(temp);
                                _context.SaveChanges();

                                if (temp.TempOrdenLegibilidadId > 0)
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

                                for (int j = 1; j < 37; j++)
                                {
                                    strValidateCampNullsOrEmpty += (worksheet.Cells[i, j].Text.Trim());
                                }
                                if (string.IsNullOrEmpty(strValidateCampNullsOrEmpty))
                                {
                                    CantidadRegistrosVacios++;
                                }
                                else
                                {
                                    CantidadRegistrosInvalidos++;
                                }

                                //worksheet.Cells[2, 2].AddComment("Invalido", "Admin");

                                //Auditoria
                                temp.ArchivoCargueId = archivoCarge.ArchivoCargueId;
                                temp.EstaValidado = false;
                                temp.FechaCreacion = DateTime.Now;
                                temp.UsuarioCreacion = pUsuarioCreo.ToUpper();
                                _context.TempOrdenLegibilidad.Add(temp);
                                _context.SaveChanges();

                            }

                        }
                        catch (Exception ex)
                        {
                            CantidadRegistrosVacios++;
                        }
                    }

                    //Actualizo el archivoCarge con la cantidad de registros validos , invalidos , y el total;
                    //-1 ya los registros comienzan desde esta fila
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

                    //byte[] bin = package.GetAsByteArray();
                    //string pathFile = archivoCarge.Ruta + "/" + archivoCarge.Nombre + ".xlsx";
                    //File.WriteAllBytes(pathFile, bin);

                    return new Respuesta
                    {
                        Data = archivoCargueRespuesta,
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = ConstantMessagesCargueElegibilidad.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoProyecto, ConstantMessagesCargueElegibilidad.OperacionExitosa, (int)enumeratorAccion.ValidarExcel, pUsuarioCreo, "VALIDAR EXCEL")
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoProyecto, ConstantMessagesCargueElegibilidad.Error, (int)enumeratorAccion.ValidarExcel, pUsuarioCreo, "VALIDAR EXCEL")
                };
            }


        }

        public bool EsCompleto(ProcesoSeleccion procesoSeleccion)
        {
            //depende del tipo
            if (procesoSeleccion.TipoProcesoCodigo == ConstanCodigoTipoProcesoSeleccion.Invitacion_Abierta)
            {
                if (
                     string.IsNullOrEmpty(procesoSeleccion.Objeto) ||
                     string.IsNullOrEmpty(procesoSeleccion.AlcanceParticular) ||
                     string.IsNullOrEmpty(procesoSeleccion.Justificacion) ||
                     string.IsNullOrEmpty(procesoSeleccion.TipoIntervencionCodigo) ||
                     string.IsNullOrEmpty(procesoSeleccion.TipoAlcanceCodigo) ||
                     string.IsNullOrEmpty(procesoSeleccion.TipoProcesoCodigo) ||
                     string.IsNullOrEmpty(Convert.ToString(procesoSeleccion.EsDistribucionGrupos)) ||
                     string.IsNullOrEmpty(Convert.ToString(procesoSeleccion.ResponsableTecnicoUsuarioId)) ||
                     string.IsNullOrEmpty(Convert.ToString(procesoSeleccion.ResponsableEstructuradorUsuarioid)) ||
                     string.IsNullOrEmpty(Convert.ToString(procesoSeleccion.CondicionesAsignacionPuntaje)) ||
                     string.IsNullOrEmpty(Convert.ToString(procesoSeleccion.CondicionesFinancierasHabilitantes)) ||
                     string.IsNullOrEmpty(Convert.ToString(procesoSeleccion.CondicionesJuridicasHabilitantes)) ||
                     string.IsNullOrEmpty(Convert.ToString(procesoSeleccion.CondicionesTecnicasHabilitantes))
                )
                    return false;

                foreach (var psg in procesoSeleccion.ProcesoSeleccionGrupo.Where(psg => psg.Eliminado != true))
                {
                    if (
                       string.IsNullOrEmpty(psg.NombreGrupo) ||
                       string.IsNullOrEmpty(psg.TipoPresupuestoCodigo) ||
                       (psg.TipoPresupuestoCodigo == "2" && psg.ValorMaximoCategoria == null) ||
                       (psg.TipoPresupuestoCodigo == "2" && psg.ValorMinimoCategoria == null) ||
                       (psg.TipoPresupuestoCodigo == "1" && psg.Valor == null) ||
                       psg.PlazoMeses == null
                     )
                        return false;
                }


                if (procesoSeleccion.EstadoProcesoSeleccionCodigo == ConstanCodigoEstadoProcesoSeleccion.AprobadaAperturaPorComiteFiduciario)
                {
                    if (
                         string.IsNullOrEmpty(procesoSeleccion.EvaluacionDescripcion) ||
                         string.IsNullOrEmpty(procesoSeleccion.UrlSoporteEvaluacion) ||
                         procesoSeleccion.ProcesoSeleccionProponente == null ||
                         procesoSeleccion.ProcesoSeleccionProponente.Count() == 0
                       )
                        return false;
                }
                return true;
            }
            else if (procesoSeleccion.TipoProcesoCodigo == ConstanCodigoTipoProcesoSeleccion.Invitacion_Privada)
            {
                if (
                     string.IsNullOrEmpty(procesoSeleccion.Objeto) ||
                     string.IsNullOrEmpty(procesoSeleccion.AlcanceParticular) ||
                     string.IsNullOrEmpty(procesoSeleccion.Justificacion) ||
                     string.IsNullOrEmpty(procesoSeleccion.TipoIntervencionCodigo) ||
                     string.IsNullOrEmpty(procesoSeleccion.TipoAlcanceCodigo) ||
                     //string.IsNullOrEmpty(procesoSeleccion.TipoProcesoCodigo) ||
                     //string.IsNullOrEmpty(Convert.ToString(procesoSeleccion.ResponsableTecnicoUsuarioId)) ||
                     //string.IsNullOrEmpty(Convert.ToString(procesoSeleccion.ResponsableEstructuradorUsuarioid)) ||
                     string.IsNullOrEmpty(Convert.ToString(procesoSeleccion.CantidadCotizaciones)) ||
                     string.IsNullOrEmpty(procesoSeleccion.EstadoProcesoSeleccionCodigo) ||
                     string.IsNullOrEmpty(procesoSeleccion.EtapaProcesoSeleccionCodigo)
                )
                    return false;

                if (procesoSeleccion.ProcesoSeleccionProponente.Count() == 0)
                    return false;


                foreach (var psp in procesoSeleccion.ProcesoSeleccionProponente)
                {

                    if (psp.TipoProponenteCodigo == ConstanCodigoTipoProponente.Persona_Juridica_Union_Temporal_o_Consorcio)
                        if (
                                      string.IsNullOrEmpty(psp.NombreProponente) ||
                                      string.IsNullOrEmpty(psp.CedulaRepresentanteLegal) ||
                                      string.IsNullOrEmpty(psp.TipoProponenteCodigo) ||
                                      psp.LocalizacionIdMunicipio == null ||
                                      string.IsNullOrEmpty(psp.DireccionProponente) ||
                                      string.IsNullOrEmpty(psp.TelefonoProponente) ||
                                      string.IsNullOrEmpty(psp.EmailProponente)
                                )
                            return false;

                    if (psp.TipoProponenteCodigo == ConstanCodigoTipoProponente.Persona_Juridica_Individual)
                        if (
                              string.IsNullOrEmpty(psp.NombreProponente) ||
                              string.IsNullOrEmpty(psp.NombreRepresentanteLegal) ||
                              string.IsNullOrEmpty(psp.CedulaRepresentanteLegal) ||
                              string.IsNullOrEmpty(psp.NumeroIdentificacion) ||
                              string.IsNullOrEmpty(psp.TipoProponenteCodigo) ||
                              psp.LocalizacionIdMunicipio == null ||
                              string.IsNullOrEmpty(psp.DireccionProponente) ||
                              string.IsNullOrEmpty(psp.TelefonoProponente) ||
                              string.IsNullOrEmpty(psp.EmailProponente)
                        )
                            return false;

                    if (psp.TipoProponenteCodigo == ConstanCodigoTipoProponente.Personal_Natural)
                        if (
                              string.IsNullOrEmpty(psp.NombreProponente) ||
                              string.IsNullOrEmpty(psp.NumeroIdentificacion) ||
                              string.IsNullOrEmpty(psp.TipoProponenteCodigo) ||
                              psp.LocalizacionIdMunicipio == null ||
                              string.IsNullOrEmpty(psp.DireccionProponente) ||
                              string.IsNullOrEmpty(psp.TelefonoProponente) ||
                              string.IsNullOrEmpty(psp.EmailProponente)
                        )
                            return false;

                }

                foreach (var psg in procesoSeleccion.ProcesoSeleccionGrupo.Where(psg => psg.Eliminado != true))
                {
                    if (
                             string.IsNullOrEmpty(psg.NombreGrupo) ||
                             string.IsNullOrEmpty(psg.TipoPresupuestoCodigo) ||
                             (psg.TipoPresupuestoCodigo == "2" && psg.ValorMaximoCategoria == null) ||
                             (psg.TipoPresupuestoCodigo == "2" && psg.ValorMinimoCategoria == null) ||
                             (psg.TipoPresupuestoCodigo == "1" && psg.Valor == null) ||
                             psg.PlazoMeses == null
                      )
                        return false;
                }


                foreach (var psc in procesoSeleccion.ProcesoSeleccionCotizacion.Where(psc => psc.Eliminado != true))
                {
                    if (
                        string.IsNullOrEmpty(psc.NombreOrganizacion) ||
                        psc.ValorCotizacion == null ||
                        string.IsNullOrEmpty(psc.Descripcion) ||
                        string.IsNullOrEmpty(psc.UrlSoporte)
                        )
                        return false;
                }
                return true;
            }
            else if (procesoSeleccion.TipoProcesoCodigo == ConstanCodigoTipoProcesoSeleccion.Invitacion_Cerrada)
            {
                if (
                     string.IsNullOrEmpty(procesoSeleccion.Objeto) ||
                     string.IsNullOrEmpty(procesoSeleccion.AlcanceParticular) ||
                     string.IsNullOrEmpty(procesoSeleccion.Justificacion) ||
                     string.IsNullOrEmpty(procesoSeleccion.TipoIntervencionCodigo) ||
                     string.IsNullOrEmpty(procesoSeleccion.TipoAlcanceCodigo) ||
                     string.IsNullOrEmpty(procesoSeleccion.TipoProcesoCodigo) ||
                     string.IsNullOrEmpty(Convert.ToString(procesoSeleccion.ResponsableTecnicoUsuarioId)) ||
                     string.IsNullOrEmpty(Convert.ToString(procesoSeleccion.ResponsableEstructuradorUsuarioid)) ||
                     string.IsNullOrEmpty(Convert.ToString(procesoSeleccion.CantidadCotizaciones)) ||
                     string.IsNullOrEmpty(procesoSeleccion.EstadoProcesoSeleccionCodigo) ||
                     string.IsNullOrEmpty(procesoSeleccion.EtapaProcesoSeleccionCodigo)
                   )
                    return false;


                if (procesoSeleccion.ProcesoSeleccionProponente.Count() == 0)
                    return false;

                if (procesoSeleccion.ProcesoSeleccionProponente.Count(psp => psp.Eliminado != true) != procesoSeleccion.CantidadProponentesInvitados)

                    foreach (var psc in procesoSeleccion.ProcesoSeleccionCotizacion.Where(psc => psc.Eliminado != true))
                    {
                        if (
                             string.IsNullOrEmpty(psc.NombreOrganizacion) ||
                             psc.ValorCotizacion == null ||
                             string.IsNullOrEmpty(psc.Descripcion) ||
                             string.IsNullOrEmpty(psc.UrlSoporte)
                        )
                            return false;
                    }

                if (procesoSeleccion.ProcesoSeleccionGrupo.Count(psg => psg.Eliminado != true) == 0)
                    return false;

                foreach (var psg in procesoSeleccion.ProcesoSeleccionGrupo.Where(ps => ps.Eliminado != true))
                {
                    if (
                       string.IsNullOrEmpty(psg.NombreGrupo) ||
                       string.IsNullOrEmpty(psg.TipoPresupuestoCodigo) ||
                       (psg.TipoPresupuestoCodigo == "2" && psg.ValorMaximoCategoria == null) ||
                       (psg.TipoPresupuestoCodigo == "2" && psg.ValorMinimoCategoria == null) ||
                       (psg.TipoPresupuestoCodigo == "1" && psg.Valor == null) ||
                       psg.PlazoMeses == null
                     )
                        return false;
                }

                foreach (var ProcesoSeleccionProponente in procesoSeleccion.ProcesoSeleccionProponente)
                {
                    if (ValidarRegistroCompletoProponente(ProcesoSeleccionProponente) == false)
                        return false;
                }

                return true;
            }
            return false;
        }


        public async Task<Respuesta> UploadMassiveLoadElegibilidad(string pIdDocument, int procesoSeleccionId, string pUsuarioModifico)
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
                     Code = ConstantMessagesCargueElegibilidad.CamposVacios,
                     Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoOrdenes, ConstantMessagesCargueElegibilidad.CamposVacios, (int)enumeratorAccion.CargueOrdenesMasivos, pUsuarioModifico, "CARGUE MASIVO ORDENES")
                 };
            }
            try
            {

                int OrigenId = await _commonService.GetDominioIdByCodigoAndTipoDominio(OrigenArchivoCargue.OrdeELegibilidad, (int)EnumeratorTipoDominio.Origen_Documento_Cargue);

                ArchivoCargue archivoCargue = _context.ArchivoCargue.Where(r => r.OrigenId == 2 && r.Nombre.Trim().ToUpper().Equals(pIdDocument.ToUpper().Trim())).FirstOrDefault();

                List<TempOrdenLegibilidad> ListTempOrdenLegibilidad = await _context.TempOrdenLegibilidad.Where(r => r.ArchivoCargueId == archivoCargue.ArchivoCargueId /*&& (bool)r.EstaValidado*/).ToListAsync();

                if (ListTempOrdenLegibilidad.Count() > 0)
                {
                    foreach (var tempOrdenLegibilidad in ListTempOrdenLegibilidad)
                    {

                        //ProcesoSeleccionProponente
                        ProcesoSeleccionProponente procesoSeleccionProponente = new ProcesoSeleccionProponente()
                        {

                            //procesoSeleccionProponente Registros 
                            ProcesoSeleccionId = procesoSeleccionId,
                            TipoProponenteCodigo = tempOrdenLegibilidad.TipoProponenteId.ToString(),
                            NombreProponente = tempOrdenLegibilidad.NombreProponente,
                            //TipoIdentificacionCodigo = tempOrdenLegibilidad.id ?
                            NumeroIdentificacion = tempOrdenLegibilidad.NumeroIddentificacionProponente,
                            LocalizacionIdMunicipio = tempOrdenLegibilidad.Minicipio.ToString(),
                            DireccionProponente = tempOrdenLegibilidad.Direccion,
                            TelefonoProponente = tempOrdenLegibilidad.Telefono,
                            EmailProponente = tempOrdenLegibilidad.Correo,
                            NombreRepresentanteLegal = tempOrdenLegibilidad.RepresentanteLegal,
                            CedulaRepresentanteLegal = tempOrdenLegibilidad.CedulaRepresentanteLegal.ToString(),
                            Eliminado = false,
                            FechaCreacion = DateTime.Now,
                            UsuarioCreacion = pUsuarioModifico
                        };

                        _context.ProcesoSeleccionProponente.Add(procesoSeleccionProponente);
                        _context.SaveChanges();

                        /* no se debería crear
                        //Cofinanciacion
                        Cofinanciacion cofinanciacion = new Cofinanciacion
                        {
                            Eliminado = false,
                            FechaCreacion = DateTime.Now,
                            //VigenciaCofinanciacionId = tempOrdenLegibilidad. ?,
                            UsuarioCreacion = tempOrdenLegibilidad.UsuarioCreacion
                        };
                        //
                        _context.Cofinanciacion.Add(cofinanciacion);
                        _context.SaveChanges();

                        //CofinanciacionAportante 1 
                        if (!string.IsNullOrEmpty(tempOrdenLegibilidad.TipoProponenteId.ToString()))
                        {

                            CofinanciacionAportante cofinanciacionAportante = new CofinanciacionAportante
                            {
                                //Auditoria
                                FechaCreacion = DateTime.Now,
                                Eliminado = false,
                                //Registros
                                UsuarioCreacion = tempOrdenLegibilidad.UsuarioCreacion,
                                CofinanciacionId = cofinanciacion.CofinanciacionId,
                                TipoAportanteId = (int)tempOrdenLegibilidad.TipoProponenteId,
                                //NombreAportanteId = (int)tempOrdenLegibilidad.nom ?
                            };
                            //
                            _context.CofinanciacionAportante.Add(cofinanciacionAportante);
                            _context.SaveChanges();

                        }*/
                        //Temporal proyecto update
                        tempOrdenLegibilidad.EstaValidado = true;
                        tempOrdenLegibilidad.FechaModificacion = DateTime.Now;
                        tempOrdenLegibilidad.UsuarioModificacion = pUsuarioModifico;
                        _context.TempOrdenLegibilidad.Update(tempOrdenLegibilidad);
                        _context.SaveChanges();
                    }

                    ProcesoSeleccion procesoSeleccion = _context.ProcesoSeleccion.Find(procesoSeleccionId);
                    procesoSeleccion.EsCompleto = EsCompleto(procesoSeleccion);

                    _context.SaveChanges();


                    return respuesta =
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = true,
                        Code = ConstantMessagesCargueElegibilidad.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoOrdenes, ConstantMessagesCargueElegibilidad.OperacionExitosa, (int)enumeratorAccion.CargueOrdenesMasivos, pUsuarioModifico, "Cantidad de Ordenes subidas : " + ListTempOrdenLegibilidad.Count())
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
                            Code = ConstantMessagesCargueElegibilidad.NoExitenArchivos,
                            Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoOrdenes, ConstantMessagesCargueElegibilidad.NoExitenArchivos, (int)enumeratorAccion.CargueOrdenesMasivos, pUsuarioModifico, "CARGUE MASIVO ORDENES")
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

        /*autor: jflorez
            descripción: borra las cotizacines en editar
            impacto: CU 3.1.3*/
        public async Task<Respuesta> deleteProcesoSeleccionCotizacionByID(int procesoSeleccionCotizacionId, string usuarioModificacion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                var procesoSeleccionCot = _context.ProcesoSeleccionCotizacion.Find(procesoSeleccionCotizacionId);

                if (procesoSeleccionCot != null)
                {
                    procesoSeleccionCot.Eliminado = true;
                    procesoSeleccionCot.UsuarioModificacion = usuarioModificacion;
                    procesoSeleccionCot.FechaModificacion = DateTime.Now;
                    _context.Update(procesoSeleccionCot);
                    _context.SaveChanges();

                    var procesoSeleccion = _context.ProcesoSeleccion
                                                        .Where(r => r.ProcesoSeleccionId == procesoSeleccionCot.ProcesoSeleccionId)
                                                        .Include(r => r.ProcesoSeleccionCotizacion)
                                                        .FirstOrDefault();

                    procesoSeleccion.CantidadCotizaciones = procesoSeleccion.ProcesoSeleccionCotizacion.Where(r => r.Eliminado != true).Count();
                }


                return respuesta =
                    new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = false,
                        IsValidation = true,
                        Code = ConstantMessagesCargueElegibilidad.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesCargueElegibilidad.OperacionExitosa, (int)enumeratorAccion.Crear_Editar_ProcesoSeleccion_Grupo, usuarioModificacion, "ELIMINACION GRUPO")
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
                        Code = ConstantMessagesCargueElegibilidad.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesCargueElegibilidad.Error, (int)enumeratorAccion.Crear_Editar_ProcesoSeleccion_Grupo, usuarioModificacion, ex.InnerException.ToString())
                    };
            }
        }

        /*autor: jflorez
            descripción: borra lOS GRUPOS en editar
            impacto: CU 3.1.3*/
        public async Task<Respuesta> deleteProcesoSeleccionGrupoByID(int procesoSeleccionGrupoId, string usuarioModificacion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                var procesoSeleccionCot = _context.ProcesoSeleccionGrupo.Find(procesoSeleccionGrupoId);

                if (procesoSeleccionCot != null)
                {


                    procesoSeleccionCot.Eliminado = true;
                    procesoSeleccionCot.UsuarioModificacion = usuarioModificacion;
                    procesoSeleccionCot.FechaModificacion = DateTime.Now;
                    _context.SaveChanges();

                    ProcesoSeleccion procesoSeleccion = _context.ProcesoSeleccion
                                                                    .Where(r => r.ProcesoSeleccionId == procesoSeleccionCot.ProcesoSeleccionId)
                                                                    .Include(r => r.ProcesoSeleccionGrupo)
                                                                    .FirstOrDefault();

                    procesoSeleccion.CantGrupos = procesoSeleccion.ProcesoSeleccionGrupo.Where(r => r.Eliminado != true).Count();
                    _context.SaveChanges();
                }

                return respuesta =
                    new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = false,
                        IsValidation = true,
                        Code = ConstantMessagesCargueElegibilidad.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesCargueElegibilidad.OperacionExitosa, (int)enumeratorAccion.Crear_Editar_ProcesoSeleccion_Grupo, usuarioModificacion, "ELIMINACION GRUPO")
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
                        Code = ConstantMessagesCargueElegibilidad.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesCargueElegibilidad.Error, (int)enumeratorAccion.Crear_Editar_ProcesoSeleccion_Grupo, usuarioModificacion, ex.InnerException.ToString())
                    };
            }
        }

        /*autor: jflorez
            descripción: borra las actividades en editar
            impacto: CU 3.1.3*/
        public async Task<Respuesta> deleteProcesoSeleccionActividadesByID(int procesoSeleccionCotizacionId, string usuarioModificacion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                var procesoSeleccionCot = _context.ProcesoSeleccionCronograma.Find(procesoSeleccionCotizacionId);

                if (procesoSeleccionCot != null)
                {
                    procesoSeleccionCot.Eliminado = true;
                    procesoSeleccionCot.UsuarioModificacion = usuarioModificacion;
                    procesoSeleccionCot.FechaModificacion = DateTime.Now;
                    _context.Update(procesoSeleccionCot);
                    _context.SaveChanges();
                }

                return respuesta =
                    new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = false,
                        IsValidation = true,
                        Code = ConstantMessagesCargueElegibilidad.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesCargueElegibilidad.OperacionExitosa, (int)enumeratorAccion.Crear_Editar_ProcesoSeleccion_Grupo, usuarioModificacion, "ELIMINACION CRONOGRAMA")
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
                        Code = ConstantMessagesCargueElegibilidad.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesCargueElegibilidad.Error, (int)enumeratorAccion.Crear_Editar_ProcesoSeleccion_Grupo, usuarioModificacion, ex.InnerException.ToString())
                    };
            }
        }

        /*jflorez
         impacto: 3.1.3
         resumen: trae listado de  observaciones*/
        public async Task<List<string>> getObservacionesProcesoSeleccionProponentes(int id)
        {
            //  return _context.ProcesoSeleccionObservacion.Where(x => x.ProcesoSeleccionId == id).ToList();
            return _context.SesionComiteSolicitud.Where(x => x.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion && !(bool)x.Eliminado && x.SolicitudId == id).Select(y => y.Observaciones).ToList();
        }

        public async Task<Respuesta> DeleteProcesoSeleccionCronogramaMonitoreo(int pId, string usuarioCreacion)
        {
            Respuesta respuesta = new Respuesta();
            int idAccionCrearProcesoSeleccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Proceso_Seleccion, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = "";
            ProcesoSeleccionMonitoreo ProcesoSeleccionAntiguo = null;
            try
            {
                //si tiene relacion con algo, no lo dejo eliminar
                /*var comite = _context.SesionComiteSolicitud.Where(x => x.SolicitudId == pId && !(bool)x.Eliminado && x.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion).Count();//jflorez. no me cuadra el nombre de la constante pero la pregunte 20201021
                if (comite > 0)
                {
                    return respuesta = new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Data = null,
                        Code = ConstantMessagesProcesoSeleccion.DependenciaEnEliminacion,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.DependenciaEnEliminacion, idAccionCrearProcesoSeleccion, pUsuarioModificacion, "ELIMINACIÓN CON DEPENDENCIA.")
                    };
                }
                */
                strCrearEditar = "ELIMINAR PROCESO SELECCION CRONOGRAMA";
                ProcesoSeleccionAntiguo = _context.ProcesoSeleccionMonitoreo.Find(pId);
                //Auditoria
                //ProcesoSeleccionAntiguo.UsuarioModificacion = pUsuarioModificacion;
                ProcesoSeleccionAntiguo.FechaModificacion = DateTime.Now;

                //Registros
                ProcesoSeleccionAntiguo.Eliminado = true;

                _context.ProcesoSeleccionMonitoreo.Update(ProcesoSeleccionAntiguo);



                await _context.SaveChangesAsync();

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcesoSeleccion.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.OperacionExitosa, idAccionCrearProcesoSeleccion, usuarioCreacion, strCrearEditar)

                };
            }
            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcesoSeleccion.ErrorInterno,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.ErrorInterno, idAccionCrearProcesoSeleccion, usuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> DeleteProcesoSeleccionIntegrante(int pId, string usuarioCreacion)
        {
            Respuesta respuesta = new Respuesta();
            int idAccionCrearProcesoSeleccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Proceso_Seleccion, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = "";
            ProcesoSeleccionIntegrante ProcesoSeleccionAntiguo = null;
            try
            {
                //si tiene relacion con algo, no lo dejo eliminar
                /*var comite = _context.SesionComiteSolicitud.Where(x => x.SolicitudId == pId && !(bool)x.Eliminado && x.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion).Count();//jflorez. no me cuadra el nombre de la constante pero la pregunte 20201021
                if (comite > 0)
                {
                    return respuesta = new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Data = null,
                        Code = ConstantMessagesProcesoSeleccion.DependenciaEnEliminacion,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.DependenciaEnEliminacion, idAccionCrearProcesoSeleccion, pUsuarioModificacion, "ELIMINACIÓN CON DEPENDENCIA.")
                    };
                }
                */
                strCrearEditar = "ELIMINAR PROCESO SELECCION INTEGRANTE";
                ProcesoSeleccionAntiguo = _context.ProcesoSeleccionIntegrante.Find(pId);
                //Auditoria
                //ProcesoSeleccionAntiguo.UsuarioModificacion = pUsuarioModificacion;
                ProcesoSeleccionAntiguo.FechaModificacion = DateTime.Now;
                //Registros
                ProcesoSeleccionAntiguo.Eliminado = true;

                _context.ProcesoSeleccionIntegrante.Update(ProcesoSeleccionAntiguo);
                await _context.SaveChangesAsync();

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcesoSeleccion.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.OperacionExitosa, idAccionCrearProcesoSeleccion, usuarioCreacion, strCrearEditar)

                };
            }
            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcesoSeleccion.ErrorInterno,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.ErrorInterno, idAccionCrearProcesoSeleccion, usuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        /*jflorez
         impacto: 3.1.3
         resumen: tarea programada para enviar mensaje al equipo estructurador cuando se vence una actividad*/
        public async Task getActividadesVencidas(string dominioFront, string mailServer, int mailPort, bool enableSSL, string password, string sender)
        {
            var tareas = _context.ProcesoSeleccionCronograma.Where(x => x.FechaMaxima < DateTime.Now && !(bool)x.Eliminado && x.ProcesoSeleccion.ResponsableEstructuradorUsuarioid != null && x.CronogramaSeguimiento.Count() == 0).Select(x => new { x.ProcesoSeleccion.NumeroProceso, x.ProcesoSeleccion.ResponsableEstructuradorUsuarioid }).ToList();

            foreach (var tarea in tareas.Select(x => x.ResponsableEstructuradorUsuarioid).Distinct())
            {
                string texto = string.Join("<br>", tareas.Where(x => x.ResponsableEstructuradorUsuarioid == tarea).Select(x => x.NumeroProceso));
                var usuarioenvio = _context.Usuario.Find(tarea);
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.ActividadesNoMonitoreadasProcesoSeleccion);
                string template = TemplateRecoveryPassword.Contenido.Replace("_LinkF_", dominioFront).Replace("[TablaSolicitudes]", texto);
                bool blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuarioenvio.Email, "Actividades procesos de selección", template, sender, password, mailServer, mailPort);
            }
        }
    }
}
