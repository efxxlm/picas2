using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{

    public class SelectionProcessService: ISelectionProcessService
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
                return await _context.ProcesoSeleccion
                                            .Where(r => !(bool)r.Eliminado)
                                            .Include(r => r.ProcesoSeleccionIntegrante)
                                            .Include(r => r.ProcesoSeleccionObservacion)
                                            .Include(r => r.ProcesoSeleccionProponente)
                                            .Include(r => r.ProcesoSeleccionCotizacion)
                                            .Include(r => r.ProcesoSeleccionCronograma)
                                            .Include(r => r.ProcesoSeleccionGrupo)
                                            .ToListAsync();
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
                return await _context.ProcesoSeleccion.Where(r => !(bool)r.Eliminado)
                                            .Include(r => r.ProcesoSeleccionIntegrante)
                                            .Include(r => r.ProcesoSeleccionObservacion)
                                            .Include(r => r.ProcesoSeleccionProponente)
                                            .Include(r => r.ProcesoSeleccionCotizacion)
                                            .Include(r => r.ProcesoSeleccionCronograma)
                                            .Include(r => r.ProcesoSeleccionGrupo)
                                            .FirstOrDefaultAsync( proceso => proceso.ProcesoSeleccionId == id );
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
            string strCrearEditar = "";
            ProcesoSeleccion ProcesoSeleccionAntiguo = null;
            try
            {

                if (string.IsNullOrEmpty(procesoSeleccion.ProcesoSeleccionId.ToString()) || procesoSeleccion.ProcesoSeleccionId == 0)
                {
                    //Auditoria
                    strCrearEditar = "CREAR PPROCESO SELECCION";
                    procesoSeleccion.FechaCreacion = DateTime.Now;
                    procesoSeleccion.Eliminado = false;

                    _context.ProcesoSeleccion.Add(procesoSeleccion);
                    
                }
                else
                {
                    strCrearEditar = "EDIT PROCESO CELECCION";
                    ProcesoSeleccionAntiguo = _context.ProcesoSeleccion.Find(procesoSeleccion.ProcesoSeleccionId);
                    //Auditoria
                    //ProcesoSeleccionAntiguo.UsuarioModificacion = procesoSeleccion.UsuarioCreacion;
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
                    ProcesoSeleccionAntiguo.EsCompleto = procesoSeleccion.EsCompleto;
                    ProcesoSeleccionAntiguo.EstadoProcesoSeleccionCodigo = procesoSeleccion.EstadoProcesoSeleccionCodigo;
                    ProcesoSeleccionAntiguo.EtapaProcesoSeleccionCodigo = procesoSeleccion.EtapaProcesoSeleccionCodigo;
                    ProcesoSeleccionAntiguo.EvaluacionDescripcion = procesoSeleccion.EvaluacionDescripcion;
                    ProcesoSeleccionAntiguo.UrlSoporteEvaluacion = procesoSeleccion.UrlSoporteEvaluacion;
                    ProcesoSeleccionAntiguo.TipoOrdenEligibilidadCodigo = procesoSeleccion.TipoOrdenEligibilidadCodigo;
                    //ProcesoSeleccionAntiguo.UsuarioCreacion = "forozco"; ////HttpContext.User.FindFirst("User").Value
                    ProcesoSeleccionAntiguo.Eliminado = false;
                    //ProcesoSeleccionAntiguo.FechaModificacion = DateTime.Now;

                    _context.ProcesoSeleccion.Update(ProcesoSeleccionAntiguo);
                }

                foreach( ProcesoSeleccionGrupo grupo in procesoSeleccion.ProcesoSeleccionGrupo )
                {
                    grupo.UsuarioCreacion = procesoSeleccion.UsuarioCreacion;
                    await this.CreateEditarProcesoSeleccionGrupo( grupo );
                }

                foreach( ProcesoSeleccionCronograma cronograma in procesoSeleccion.ProcesoSeleccionCronograma )
                {
                    cronograma.UsuarioCreacion = procesoSeleccion.UsuarioCreacion;
                    await this.CreateEditarProcesoSeleccionCronograma( cronograma, true );
                }
                
                foreach( ProcesoSeleccionCotizacion cotizacion in procesoSeleccion.ProcesoSeleccionCotizacion )
                {
                    cotizacion.UsuarioCreacion = procesoSeleccion.UsuarioCreacion;
                    await this.CreateEditarProcesoSeleccionCotizacion( cotizacion );
                }

                foreach( ProcesoSeleccionProponente proponente in procesoSeleccion.ProcesoSeleccionProponente )
                {
                    //proponente.UsuarioCreacion = procesoSeleccion.UsuarioCreacion;
                    await this.CreateEditarProcesoSeleccionProponente( proponente );
                }

                foreach( ProcesoSeleccionIntegrante integrante in procesoSeleccion.ProcesoSeleccionIntegrante )
                {
                    integrante.UsuarioCreacion = procesoSeleccion.UsuarioCreacion;
                    await this.CreateEditarProcesoSeleccionIntegrante( integrante );
                }

                await _context.SaveChangesAsync();

                return respuesta = new Respuesta
                       {
                           IsSuccessful = true,IsException = false,
                           IsValidation = false, Data = procesoSeleccion,
                           Code = ConstantMessagesProcesoSeleccion.OperacionExitosa,
                           Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.OperacionExitosa, idAccionCrearProcesoSeleccion, procesoSeleccion.UsuarioCreacion, strCrearEditar)

                       };
            }
            catch (Exception ex)
            {
                return respuesta = new Respuesta
                       {
                           IsSuccessful = false, IsException = true,
                           IsValidation = false, Data = null,
                           Code = ConstantMessagesProcesoSeleccion.ErrorInterno,
                           Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.ErrorInterno, idAccionCrearProcesoSeleccion, procesoSeleccion.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
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
                    TipoProcesoCodigoText = ProcesoSeleccion.TipoProcesoCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(ProcesoSeleccion.TipoProcesoCodigo,(int)EnumeratorTipoDominio.Tipo_Proceso_Seleccion) : "",
                    NumeroProceso = ProcesoSeleccion.NumeroProceso,
                    EtapaProcesoSeleccionCodigo = ProcesoSeleccion.EtapaProcesoSeleccionCodigo,
                    EEtapaProcesoSeleccionText = ProcesoSeleccion.EtapaProcesoSeleccionCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(ProcesoSeleccion.EtapaProcesoSeleccionCodigo, (int)EnumeratorTipoDominio.Etapa_Proceso_Seleccion) : "",
                    EstadoProcesoSeleccionCodigo = ProcesoSeleccion.EstadoProcesoSeleccionCodigo,
                    EstadoProcesoSeleccionText = ProcesoSeleccion.EstadoProcesoSeleccionCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(ProcesoSeleccion.EstadoProcesoSeleccionCodigo, (int)EnumeratorTipoDominio.Estado_Proceso_Seleccion) : "",
                    EsCompleto = ProcesoSeleccion.EsCompleto.HasValue ? ProcesoSeleccion.EsCompleto.Value : false,
                    EsCompletoText = ProcesoSeleccion.EsCompleto.Value ? await  _commonService.GetNombreDominioByCodigoAndTipoDominio(Convert.ToInt32(ProcesoSeleccion.EsCompleto).ToString(), (int)EnumeratorTipoDominio.Estado_Registro) : "Incompleto",
                };
                ListGrillaControlCronograma.Add(ControlCronogramaGrilla);
            }

            return ListGrillaControlCronograma;
        }

        public async Task<Respuesta> CreateEditarProcesoSeleccionCronograma(ProcesoSeleccionCronograma procesoSeleccionCronograma, bool esTransaccion)
        {
            Respuesta respuesta = new Respuesta();

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_ProcesoSeleccion_Cronograma, (int)EnumeratorTipoDominio.Acciones);//ERROR VALIDAR ACCIONES

            string strCrearEditar = "";
            ProcesoSeleccionCronograma procesoSeleccionCronogramaAntiguo = null;
            try
            {

                if (string.IsNullOrEmpty(procesoSeleccionCronograma.ProcesoSeleccionCronogramaId.ToString()) || procesoSeleccionCronograma.ProcesoSeleccionCronogramaId == 0)
                {
                    //Auditoria
                    strCrearEditar = "CREAR PROCESO SELECCION CRONOGRAMA";
                    procesoSeleccionCronograma.FechaCreacion = DateTime.Now;
                    procesoSeleccionCronograma.Eliminado = false;
                    procesoSeleccionCronograma.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;


                    _context.ProcesoSeleccionCronograma.Add(procesoSeleccionCronograma);
                    
                    }
                else
                {
                    strCrearEditar = "EDIT PROCESO SELECCION CRONOGRAMA";
                    procesoSeleccionCronogramaAntiguo = _context.ProcesoSeleccionCronograma.Find(procesoSeleccionCronograma.ProcesoSeleccionCronogramaId);
                    //Auditoria
                    procesoSeleccionCronogramaAntiguo.UsuarioModificacion = procesoSeleccionCronograma.UsuarioCreacion;
                    procesoSeleccionCronogramaAntiguo.FechaModificacion = DateTime.Now;

                    //Registros
                    procesoSeleccionCronogramaAntiguo.ProcesoSeleccionId = procesoSeleccionCronograma.ProcesoSeleccionId;

                    procesoSeleccionCronogramaAntiguo.NumeroActividad = procesoSeleccionCronograma.NumeroActividad;
                    procesoSeleccionCronogramaAntiguo.Descripcion = procesoSeleccionCronograma.Descripcion;
                    procesoSeleccionCronogramaAntiguo.FechaMaxima = procesoSeleccionCronograma.FechaMaxima;
                    procesoSeleccionCronogramaAntiguo.EstadoActividadCodigo = procesoSeleccionCronograma.EstadoActividadCodigo;
                    //procesoSeleccionCronogramaAntiguo.FechaCreacion = procesoSeleccionCronograma.FechaCreacion;
                    //procesoSeleccionCronogramaAntiguo.UsuarioCreacion = "forozco"; ////HttpContext.User.FindFirst("User").Value
                    procesoSeleccionCronogramaAntiguo.Eliminado = false;
                    procesoSeleccionCronogramaAntiguo.FechaModificacion = DateTime.Now;

                    _context.ProcesoSeleccionCronograma.Update(procesoSeleccionCronogramaAntiguo);
                }

                if( esTransaccion )
                    return respuesta;
                else{

                    await _context.SaveChangesAsync();

                    return respuesta = new Respuesta
                        {
                            IsSuccessful = true,IsException = false,
                            IsValidation = false, Data = procesoSeleccionCronograma,
                            Code = ConstantMessagesProcesoSeleccion.OperacionExitosa,
                            Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.OperacionExitosa, idAccion, procesoSeleccionCronograma.UsuarioCreacion, strCrearEditar)

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
                    Code = ConstantMessagesProcesoSeleccion.ErrorInterno,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion_Cronograma, ConstantMessagesProcesoSeleccion.ErrorInterno, idAccion, procesoSeleccionCronograma.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        #endregion

        #region Servicios Proceso Seleccion Grupo;
        public async Task<Respuesta> CreateEditarProcesoSeleccionGrupo(ProcesoSeleccionGrupo procesoSeleccionGrupo)
        {
            Respuesta respuesta = new Respuesta();

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_ProcesoSeleccion_Grupo, (int)EnumeratorTipoDominio.Acciones);//ERROR VALIDAR ACCIONES

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
                    _context.ProcesoSeleccionGrupo.Add(procesoSeleccionGrupo);
                }
                else
                {
                    strCrearEditar = "EDIT PROCESO SELECCION GRUPO";
                    ProcesoSeleccionGrupoAntiguo = _context.ProcesoSeleccionGrupo.Find(procesoSeleccionGrupo.ProcesoSeleccionGrupoId);
                    //Auditoria
                    ProcesoSeleccionGrupoAntiguo.UsuarioModificacion = procesoSeleccionGrupo.UsuarioCreacion;
                    ProcesoSeleccionGrupoAntiguo.FechaModificacion = DateTime.Now;


                    //Registros

                    ProcesoSeleccionGrupoAntiguo.ProcesoSeleccionId = procesoSeleccionGrupo.ProcesoSeleccionId;
                    ProcesoSeleccionGrupoAntiguo.NombreGrupo = procesoSeleccionGrupo.NombreGrupo;
                    ProcesoSeleccionGrupoAntiguo.TipoPresupuestoCodigo = procesoSeleccionGrupo.TipoPresupuestoCodigo;
                    ProcesoSeleccionGrupoAntiguo.Valor = procesoSeleccionGrupo.Valor;
                    ProcesoSeleccionGrupoAntiguo.ValorMinimoCategoria = procesoSeleccionGrupo.ValorMinimoCategoria;
                    ProcesoSeleccionGrupoAntiguo.ValorMaximoCategoria = procesoSeleccionGrupo.ValorMaximoCategoria;
                    ProcesoSeleccionGrupoAntiguo.PlazoMeses = procesoSeleccionGrupo.PlazoMeses;
                    ProcesoSeleccionGrupoAntiguo.Eliminado = false;

                    _context.ProcesoSeleccionGrupo.Update(ProcesoSeleccionGrupoAntiguo);
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion_Grupo, ConstantMessagesProcesoSeleccion.ErrorInterno, idAccion, ProcesoSeleccionGrupoAntiguo.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
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

        public async Task<Respuesta> CreateEditarProcesoSeleccionCotizacion(ProcesoSeleccionCotizacion procesoSeleccionCotizacion)
        {
            Respuesta respuesta = new Respuesta();

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Proceso_Seleccion, (int)EnumeratorTipoDominio.Acciones);//ERROR VALIDAR ACCIONES

            string strCrearEditar = "";
            ProcesoSeleccionCotizacion ProcesoSeleccionCotizacionAntiguo = null;
            try
            {

                if (string.IsNullOrEmpty(procesoSeleccionCotizacion.ProcesoSeleccionCotizacionId.ToString()) || procesoSeleccionCotizacion.ProcesoSeleccionCotizacionId == 0)
                {
                    //Auditoria
                    strCrearEditar = "CREAR PROCESO SELECCION COTIZACION";
                    procesoSeleccionCotizacion.FechaCreacion = DateTime.Now;
                    procesoSeleccionCotizacion.Eliminado = false;
                    _context.ProcesoSeleccionCotizacion.Add(procesoSeleccionCotizacion);
                    
                }
                else
                {
                    strCrearEditar = "EDIT PROCESO SELECCION COTIZACION";
                    ProcesoSeleccionCotizacionAntiguo = _context.ProcesoSeleccionCotizacion.Find(procesoSeleccionCotizacion.ProcesoSeleccionCotizacionId);
                    //Auditoria
                    ProcesoSeleccionCotizacionAntiguo.UsuarioModificacion = procesoSeleccionCotizacion.UsuarioCreacion;
                    ProcesoSeleccionCotizacionAntiguo.FechaModificacion = DateTime.Now;


                    //Registros

                    ProcesoSeleccionCotizacionAntiguo.ProcesoSeleccionCotizacionId = procesoSeleccionCotizacion.ProcesoSeleccionCotizacionId;


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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.ErrorInterno, idAccion, ProcesoSeleccionCotizacionAntiguo.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }
        #endregion


          #region Servicios Proceso Seleccion Proponente;

        public async Task<ProcesoSeleccionCronograma> GetProcesoSeleccionProponenteById(int id)
        {
            return await _context.ProcesoSeleccionCronograma.FindAsync(id);
        }

        public async Task<ActionResult<List<GrillaProcesoSeleccionProponente>>> GetGridProcesoSeleccionProponente(int? procesoSeleccionId)
        {
            List<ProcesoSeleccionProponente> ListProcesoSeleccionProponente =
                (procesoSeleccionId != null  ? await _context.ProcesoSeleccionProponente.Where(x => x.ProcesoSeleccionId == procesoSeleccionId).ToListAsync()
                : await _context.ProcesoSeleccionProponente.ToListAsync());


            List <GrillaProcesoSeleccionProponente> ListGrillaProcesoSeleccionProponente = new List<GrillaProcesoSeleccionProponente>();

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

        public async Task<Respuesta> CreateEditarProcesoSeleccionProponente(ProcesoSeleccionProponente procesoSeleccionProponente)
        {
            Respuesta respuesta = new Respuesta();

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Proceso_Seleccion, (int)EnumeratorTipoDominio.Acciones);//ERROR VALIDAR ACCIONES

            string strCrearEditar = "";
            string userAction = "jsorozcof";//httpContext.User.FindFirst("User").Value;
            ProcesoSeleccionProponente ProcesoSeleccionProponenteAntiguo = null;
            try
            {

                if (string.IsNullOrEmpty(procesoSeleccionProponente.ProcesoSeleccionProponenteId.ToString()) || procesoSeleccionProponente.ProcesoSeleccionProponenteId == 0)
                {
                    //Auditoria
                    strCrearEditar = "CREAR PROCESO SELECCION PROPONENTE";
                    _context.ProcesoSeleccionProponente.Add(procesoSeleccionProponente);
                }
                else
                {
                    strCrearEditar = "EDIT PROCESO SELECCION PROPONENTE";
                    ProcesoSeleccionProponenteAntiguo = _context.ProcesoSeleccionProponente.Find(procesoSeleccionProponente.ProcesoSeleccionProponenteId);

                    //Registros

                    ProcesoSeleccionProponenteAntiguo.ProcesoSeleccionId = procesoSeleccionProponente.ProcesoSeleccionId;
                    ProcesoSeleccionProponenteAntiguo.TipoProponenteCodigo = procesoSeleccionProponente.TipoProponenteCodigo;
                    ProcesoSeleccionProponenteAntiguo.NombreProponente = procesoSeleccionProponente.NombreProponente;
                    ProcesoSeleccionProponenteAntiguo.TipoIdentificacionCodigo = procesoSeleccionProponente.TipoIdentificacionCodigo;
                    ProcesoSeleccionProponenteAntiguo.NumeroIdentificacion = procesoSeleccionProponente.NumeroIdentificacion;
                    ProcesoSeleccionProponenteAntiguo.LocalizacionIdMunicipio = procesoSeleccionProponente.LocalizacionIdMunicipio;
                    ProcesoSeleccionProponenteAntiguo.DireccionProponente = procesoSeleccionProponente.DireccionProponente;
                    ProcesoSeleccionProponenteAntiguo.TelefonoProponente = procesoSeleccionProponente.TelefonoProponente;
                    ProcesoSeleccionProponenteAntiguo.EmailProponente = procesoSeleccionProponente.EmailProponente;
                    _context.ProcesoSeleccionProponente.Update(ProcesoSeleccionProponenteAntiguo);
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.ErrorInterno, idAccion, userAction, ex.InnerException.ToString().Substring(0, 500))
                };
            }
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

        public async Task<Respuesta> CreateEditarProcesoSeleccionIntegrante(ProcesoSeleccionIntegrante procesoSeleccionIntegrante)
        {
            Respuesta respuesta = new Respuesta();

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Proceso_Seleccion, (int)EnumeratorTipoDominio.Acciones);//ERROR VALIDAR ACCIONES

            string strCrearEditar = "";
            ProcesoSeleccionIntegrante procesoSeleccionIntegranteAntiguo = null;
            try
            {

                if (string.IsNullOrEmpty(procesoSeleccionIntegrante.ProcesoSeleccionIntegranteId.ToString()) || procesoSeleccionIntegrante.ProcesoSeleccionIntegranteId == 0)
                {
                    //Auditoria
                    strCrearEditar = "CREAR PROCESO SELECCION INTEGRANTE";
                    procesoSeleccionIntegrante.FechaCreacion = DateTime.Now;
                    procesoSeleccionIntegrante.UsuarioCreacion = procesoSeleccionIntegrante.UsuarioCreacion;
                    procesoSeleccionIntegrante.Eliminado = false;

                    _context.ProcesoSeleccionIntegrante.Add(procesoSeleccionIntegrante);

                }
                else
                {
                    strCrearEditar = "EDIT PROCESO SELECCION INTEGRANTE";
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.ErrorInterno, idAccion, procesoSeleccionIntegrante.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }
        #endregion




        public async Task<bool> Delete(int id)
        {
            try
            {
                ProcesoSeleccion entity = await GetSelectionProcessById(id);
                _context.ProcesoSeleccion.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
