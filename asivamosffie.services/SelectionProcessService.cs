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

                    //int? countMaxId = _context.ProcesoSeleccion.Max(p => (int?)p.ProcesoSeleccionId);
                    int countMax = _context.ProcesoSeleccion.Count(p => p.TipoProcesoCodigo == procesoSeleccion.TipoProcesoCodigo);

                    //Auditoria
                    strCrearEditar = "CREAR PPROCESO SELECCION";
                    procesoSeleccion.FechaCreacion = DateTime.Now;
                    procesoSeleccion.Eliminado = false;
                    procesoSeleccion.EsCompleto = EsCompleto(procesoSeleccion);
                    procesoSeleccion.NumeroProceso = Helpers.Helpers.Consecutive(procesoSeleccion.TipoProcesoCodigo, countMax);
                    procesoSeleccion.EstadoProcesoSeleccionCodigo = "1";

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
                    ProcesoSeleccionAntiguo.CantidadProponentesInvitados = procesoSeleccion.CantidadProponentesInvitados;
                    ProcesoSeleccionAntiguo.UrlSoporteProponentesSeleccionados = procesoSeleccion.UrlSoporteProponentesSeleccionados;
                    
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

        public async Task<Respuesta> DeleteProcesoSeleccion( int pId, string pUsuarioModificacion )
        {
            Respuesta respuesta = new Respuesta();
            int idAccionCrearProcesoSeleccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Proceso_Seleccion, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = "";
            ProcesoSeleccion ProcesoSeleccionAntiguo = null;
            try
            {
                    strCrearEditar = "ELIMINAR PROCESO CELECCION";
                    ProcesoSeleccionAntiguo = _context.ProcesoSeleccion.Find( pId );
                    //Auditoria
                    //ProcesoSeleccionAntiguo.UsuarioModificacion = pUsuarioModificacion;
                    ProcesoSeleccionAntiguo.FechaModificacion = DateTime.Now;

                    //Registros
                    ProcesoSeleccionAntiguo.Eliminado = true;

                    _context.ProcesoSeleccion.Update(ProcesoSeleccionAntiguo);

                

                await _context.SaveChangesAsync();

                return respuesta = new Respuesta
                       {
                           IsSuccessful = true,IsException = false,
                           IsValidation = false, Data = null,
                           Code = ConstantMessagesProcesoSeleccion.OperacionExitosa,
                           Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.OperacionExitosa, idAccionCrearProcesoSeleccion, pUsuarioModificacion, strCrearEditar)

                       };
            }
            catch (Exception ex)
            {
                return respuesta = new Respuesta
                       {
                           IsSuccessful = false, IsException = true,
                           IsValidation = false, Data = null,
                           Code = ConstantMessagesProcesoSeleccion.ErrorInterno,
                           Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.ErrorInterno, idAccionCrearProcesoSeleccion, pUsuarioModificacion, ex.InnerException.ToString().Substring(0, 500))
                       };
            }
        }

        public async Task<Respuesta> ChangeStateProcesoSeleccion( int pId, string pUsuarioModificacion, string pCodigoEstado )
        {
            Respuesta respuesta = new Respuesta();
            int idAccionCrearProcesoSeleccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Proceso_Seleccion, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = "";
            ProcesoSeleccion ProcesoSeleccionAntiguo = null;
            try
            {
                    strCrearEditar = "CAMBIAR ESTADO PROCESO SELECCION";
                    ProcesoSeleccionAntiguo = _context.ProcesoSeleccion.Find( pId );
                    //Auditoria
                    ProcesoSeleccionAntiguo.UsuarioModificacion = pUsuarioModificacion;
                    ProcesoSeleccionAntiguo.FechaModificacion = DateTime.Now;

                    //Registros
                    ProcesoSeleccionAntiguo.EstadoProcesoSeleccionCodigo = pCodigoEstado;

                    //_context.ProcesoSeleccion.Update(ProcesoSeleccionAntiguo);

                

                await _context.SaveChangesAsync();

                return respuesta = new Respuesta
                       {
                           IsSuccessful = true,IsException = false,
                           IsValidation = false, Data = null,
                           Code = ConstantMessagesProcesoSeleccion.OperacionExitosa,
                           Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.OperacionExitosa, idAccionCrearProcesoSeleccion, pUsuarioModificacion, strCrearEditar)

                       };
            }
            catch (Exception ex)
            {
                return respuesta = new Respuesta
                       {
                           IsSuccessful = false, IsException = true,
                           IsValidation = false, Data = null,
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

        public async Task<List<ProcesoSeleccionProponente>> GetProcesoSeleccionProponentes()
        {
            return await _context.ProcesoSeleccionProponente.Where( p => p.ProcesoSeleccionProponenteId == p.ProcesoSeleccionProponenteId ).ToListAsync();
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
                    procesoSeleccionProponente.FechaCreacion = DateTime.Now;
                    procesoSeleccionProponente.UsuarioCreacion = procesoSeleccionProponente.UsuarioCreacion;
                    procesoSeleccionProponente.Eliminado = false;
                    _context.ProcesoSeleccionProponente.Add(procesoSeleccionProponente);
                }
                else
                {
                    strCrearEditar = "EDIT PROCESO SELECCION PROPONENTE";
                    ProcesoSeleccionProponenteAntiguo.FechaModificacion = DateTime.Now;
                    ProcesoSeleccionProponenteAntiguo.Eliminado = false;
                    ProcesoSeleccionProponenteAntiguo.UsuarioModificacion = procesoSeleccionProponente.UsuarioModificacion;
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
                    //Auditoria
                    strCrearEditar = "CREAR CRONOGRAMA SEGUIMIENTO";
                    cronogramaSeguimiento.FechaCreacion = DateTime.Now;
                    cronogramaSeguimiento.Eliminado = false;
                    cronogramaSeguimiento.UsuarioCreacion = cronogramaSeguimiento.UsuarioCreacion;


                    _context.CronogramaSeguimiento.Add(cronogramaSeguimiento);
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

                pProcesoSeleccion.ProcesoSeleccionProponente.ToList().ForEach( p => {
                    Contratista contratista = new Contratista();

                    contratista.TipoIdentificacionCodigo =  ( p.TipoProponenteCodigo == "4" || p.TipoProponenteCodigo == "2" ) ? "3" : "1"; //Nit - cedula
                    contratista.NumeroIdentificacion =  string.IsNullOrEmpty( p.NumeroIdentificacion ) ? "0" : p.NumeroIdentificacion;
                    contratista.Nombre = p.NombreProponente;
                    contratista.RepresentanteLegal = string.IsNullOrEmpty( p.NombreRepresentanteLegal ) ? p.NombreProponente : p.NombreRepresentanteLegal;
                    contratista.NumeroInvitacion = pProcesoSeleccion.NumeroProceso;
                    contratista.EsConsorcio = p.TipoProponenteCodigo == "4" ? true : false;
                    contratista.Activo = true;
                    contratista.FechaCreacion = DateTime.Now;
                    contratista.UsuarioCreacion = pUsuarioCreo;
                    
                    _context.Contratista.Add( contratista );

                });

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



     public async Task<Respuesta> SetValidateCargueMasivo(IFormFile pFile, string pFilePatch, string pUsuarioCreo)
        {
            int CantidadRegistrosVacios = 0;
            int CantidadResgistrosValidos = 0;
            int CantidadRegistrosInvalidos = 0;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //int OrigenId = await _commonService.GetDominioIdByCodigoAndTipoDominio(OrigenArchivoCargue.Proyecto, (int)EnumeratorTipoDominio.Origen_Documento_Cargue);
            DocumentService _documentService = new DocumentService(_context, _commonService);

            ArchivoCargue archivoCarge = await _documentService.getSaveFile(pFile, pFilePatch, Int32.Parse(OrigenArchivoCargue.OrdeELegibilidad));

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
                    for (int i = 2; i < worksheet.Dimension.Rows; i++)
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
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 9].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 10].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 11].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 23].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 13].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 14].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 15].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 16].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 17].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 18].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 19].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 20].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 21].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 22].Text) |

                                    !string.IsNullOrEmpty(worksheet.Cells[i, 23].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 24].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 25].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 26].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 27].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 28].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 29].Text) |
                                    !string.IsNullOrEmpty(worksheet.Cells[i, 32].Text)

                                )
                            {

                                TempOrdenLegibilidad temp = new TempOrdenLegibilidad();
                                //Auditoria
                                temp.ArchivoCargueId = archivoCarge.ArchivoCargueId;
                                temp.EstaValidado = false;
                                temp.FechaCreacion = DateTime.Now;
                                temp.UsuarioCreacion = pUsuarioCreo;

                                // #1
                                //Tipo proponente
                                temp.TipoProponenteId = Int32.Parse(await _commonService.GetDominioCodigoByNombreDominioAndTipoDominio(worksheet.Cells[i, 1].Text, (int)EnumeratorTipoDominio.Tipo_Proponente));
                                temp.Departamento = await _commonService.GetLocalizacionIdByName(worksheet.Cells[i, 4].Text, "0");

                                // #1
                                //Tipo proponente
                                temp.TipoProponenteId = Int32.Parse(await _commonService.GetDominioCodigoByNombreDominioAndTipoDominio(worksheet.Cells[i, 1].Text, (int)EnumeratorTipoDominio.Tipo_Proponente));


                                //#2
                                //Nombre proponente
                                if (!string.IsNullOrEmpty(worksheet.Cells[i, 2].Text))
                                {
                                    temp.NombreProponente = worksheet.Cells[i, 2].Text;
                                }

                                //#3
                                //Identificacion del proponente
                                if (!string.IsNullOrEmpty(worksheet.Cells[i, 3].Text)) { temp.NumeroIddentificacionProponente = worksheet.Cells[i, 3].Text; } else { temp.NumeroIddentificacionProponente = string.Empty; }


                                //#5
                                //Departamento domicilio proponente
                                if (!string.IsNullOrEmpty(worksheet.Cells[i, 4].Text)) { temp.Departamento = await _commonService.GetLocalizacionIdByName(worksheet.Cells[i, 4].Text, "0"); } else { temp.Departamento = int.Parse(null); }

                                //#5
                                //Municipio proponente///aqui debe recibir el parametro iddepartamento, pueden haber municipios del mismo nombre para diferente departamento
                                if (!string.IsNullOrEmpty(worksheet.Cells[i, 5].Text))
                                {
                                    int DepartamentoId = temp.Departamento = await _commonService.GetLocalizacionIdByName(worksheet.Cells[i, 4].Text, "0");
                                    temp.Minicipio = await _commonService.GetLocalizacionIdByName(worksheet.Cells[i, 5].Text, DepartamentoId.ToString());//temp.Departamento.ToString()

                                }
                                else
                                {
                                    temp.Minicipio = int.Parse(null);
                                }

                                //#6
                                //Direccion del proponente
                                if (!string.IsNullOrEmpty(worksheet.Cells[i, 6].Text)) { temp.Direccion = Convert.ToString(worksheet.Cells[i, 6].Text); } else { temp.Direccion = string.Empty; }

                                //#7
                                //Telefono del proponente
                                if (!string.IsNullOrEmpty(worksheet.Cells[i, 7].Text)) { temp.Telefono = Convert.ToString(worksheet.Cells[i, 7].Text); } else { temp.Telefono = string.Empty; }

                                //#8
                                //Correo del proponente
                                if (!string.IsNullOrEmpty(worksheet.Cells[i, 8].Text)) { temp.Correo = Convert.ToString(worksheet.Cells[i, 8].Text); } else { temp.Correo = string.Empty; }

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
                                    temp.RepresentanteLegal = Convert.ToString(worksheet.Cells[i, 11].Text) != string.Empty ? Convert.ToString(worksheet.Cells[i, 11].Text) : "";
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
                                    int DepartamentoIdRL = temp.Departamento = await _commonService.GetLocalizacionIdByName(worksheet.Cells[i, 12].Text, "0");
                                    temp.MunucipioRl = await _commonService.GetLocalizacionIdByName(worksheet.Cells[i, 13].Text, DepartamentoIdRL.ToString());//temp.Departamento.ToString()
                                }


                                //#14
                                //Legal
                                if (!string.IsNullOrEmpty(worksheet.Cells[i, 13].Text))
                                {
                                    temp.Legal = Convert.ToString(await _commonService.GetLocalizacionIdByName(worksheet.Cells[i, 13].Text, "0"));
                                }
                                else
                                {
                                    temp.Legal = string.Empty;
                                }

                                //#15
                                //Direccion Representante Legal
                                if (!string.IsNullOrEmpty(worksheet.Cells[i, 15].Text)) { temp.DireccionRl = worksheet.Cells[i, 15].Text; } else { temp.DireccionRl = string.Empty; }

                                //#16
                                //Telefono Representante Legal
                                if (!string.IsNullOrEmpty(worksheet.Cells[i, 16].Text)) { temp.TelefonoRl = worksheet.Cells[i, 16].Text; } else { temp.TelefonoRl = string.Empty; }

                                //#17
                                //Correo Representante Legal
                                if (!string.IsNullOrEmpty(worksheet.Cells[i, 17].Text)) { temp.CorreoRl = worksheet.Cells[i, 17].Text; } else { temp.CorreoRl = string.Empty; }

                                //#18
                                //Nombre del representante legal del la UT o consorcio
                                if (!string.IsNullOrEmpty(worksheet.Cells[i, 18].Text)) { temp.NombreOtoConsorcio = worksheet.Cells[i, 18].Text; } else { temp.NombreOtoConsorcio = string.Empty; }

                                //#19
                                //Entiddaes que integran la union temporal
                                if (!string.IsNullOrEmpty(worksheet.Cells[i, 19].Text))
                                {
                                    temp.EntiddaesQueIntegranLaUnionTemporal = Convert.ToInt32(worksheet.Cells[i, 19].Text) >= 0 ? Int32.Parse(worksheet.Cells[i, 19].Text) : 0;
                                }


                                //#20
                                //Nombre integrante
                                if (!string.IsNullOrEmpty(worksheet.Cells[i, 20].Text)) { temp.NombreIntegrante = worksheet.Cells[i, 20].Text; } else { temp.NombreIntegrante = string.Empty; }


                                //#21
                                //Porcentaje participacion
                                if (!string.IsNullOrEmpty(worksheet.Cells[i, 21].Text))
                                {
                                    temp.PorcentajeParticipacion = Convert.ToDecimal(worksheet.Cells[i, 21].Text) >= 0 ? Int32.Parse(worksheet.Cells[i, 21].Text) : 0;
                                }


                                //#22
                                //Nombre  del representante legal de la UT o consorcio
                                if (!string.IsNullOrEmpty(worksheet.Cells[i, 22].Text)) { temp.NombreRlutoConsorcio = worksheet.Cells[i, 22].Text; } else { temp.NombreRlutoConsorcio = string.Empty; }

                                //#23
                                //Cedula  del representante legal de la UT o consorcio
                                if (!string.IsNullOrEmpty(worksheet.Cells[i, 23].Text))
                                {
                                    temp.CcrlutoConsorcio = Convert.ToInt32(worksheet.Cells[i, 23].Text) >= 0 ? Int32.Parse(worksheet.Cells[i, 23].Text) : 0;
                                }


                                //#24
                                //Cedula  del representante legal de la UT o consorcio
                                if (!string.IsNullOrEmpty(worksheet.Cells[i, 24].Text)) { temp.DepartamentoRlutoConsorcio = await _commonService.GetLocalizacionIdByName(worksheet.Cells[i, 24].Text, "0"); }

                                //#25
                                //Municipio  del representante legal de la UT o consorcio
                                if (!string.IsNullOrEmpty(worksheet.Cells[i, 25].Text))
                                {
                                    int DepartamentoIdConsorcio = temp.Departamento = await _commonService.GetLocalizacionIdByName(worksheet.Cells[i, 24].Text, "0");
                                    temp.MinicipioRlutoConsorcio = await _commonService.GetLocalizacionIdByName(worksheet.Cells[i, 25].Text, DepartamentoIdConsorcio.ToString());
                                }

                                //#26
                                //Direccion  del representante legal de la UT o consorcio
                                if (!string.IsNullOrEmpty(worksheet.Cells[i, 26].Text)) { temp.DireccionRlutoConsorcio = worksheet.Cells[i, 26].Text; } else { temp.DireccionRlutoConsorcio = string.Empty; }

                                //#27
                                //Telefono  del representante legal de la UT o consorcio
                                if (!string.IsNullOrEmpty(worksheet.Cells[i, 27].Text)) { temp.TelefonoRlutoConsorcio = worksheet.Cells[i, 27].Text; } else { temp.TelefonoRlutoConsorcio = string.Empty; }

                                //#28
                                //Correo  del representante legal de la UT o consorcio
                                if (!string.IsNullOrEmpty(worksheet.Cells[i, 28].Text)) { temp.CorreoRlutoConsorcio = worksheet.Cells[i, 28].Text; } else { temp.CorreoRlutoConsorcio = string.Empty; }


                                //Guarda Cambios en una tabla temporal

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
                        catch (Exception)
                        {
                            CantidadRegistrosInvalidos++;
                        }
                    }

                    //Actualizo el archivoCarge con la cantidad de registros validos , invalidos , y el total;
                    //-2 ya los registros comienzan desde esta fila
                    archivoCarge.CantidadRegistrosInvalidos = CantidadRegistrosInvalidos;
                    archivoCarge.CantidadRegistrosValidos = CantidadResgistrosValidos;
                    archivoCarge.CantidadRegistros = (worksheet.Dimension.Rows - CantidadRegistrosVacios - 2);
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
                        Code = ConstantMessagesCargueElegibilidad.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoProyecto, ConstantMessagesCargueElegibilidad.OperacionExitosa, (int)enumeratorAccion.ValidarExcel, pUsuarioCreo, "")
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoProyecto, ConstantMessagesCargueElegibilidad.Error, (int)enumeratorAccion.ValidarExcel, pUsuarioCreo, "")
                };
            }


        }

        public bool EsCompleto(ProcesoSeleccion procesoSeleccion)
        {
            if (
                 string.IsNullOrEmpty(procesoSeleccion.Objeto)
                 || !string.IsNullOrEmpty(procesoSeleccion.AlcanceParticular)
                 || !string.IsNullOrEmpty(procesoSeleccion.Justificacion)
                 || !string.IsNullOrEmpty(procesoSeleccion.CriteriosSeleccion)
                 || !string.IsNullOrEmpty(procesoSeleccion.TipoIntervencionCodigo)
                 || !string.IsNullOrEmpty(procesoSeleccion.TipoAlcanceCodigo)
                 || !string.IsNullOrEmpty(procesoSeleccion.TipoProcesoCodigo)
                 || !string.IsNullOrEmpty(Convert.ToString(procesoSeleccion.EsDistribucionGrupos))
                 || !string.IsNullOrEmpty(Convert.ToString(procesoSeleccion.CantGrupos))
                 || !string.IsNullOrEmpty(Convert.ToString(procesoSeleccion.ResponsableTecnicoUsuarioId))
                 || !string.IsNullOrEmpty(Convert.ToString(procesoSeleccion.ResponsableEstructuradorUsuarioid))
                 || !string.IsNullOrEmpty(procesoSeleccion.CondicionesJuridicasHabilitantes)
                 || !string.IsNullOrEmpty(procesoSeleccion.CondicionesFinancierasHabilitantes)
                 || !string.IsNullOrEmpty(procesoSeleccion.CondicionesAsignacionPuntaje)
                 || !string.IsNullOrEmpty(Convert.ToString(procesoSeleccion.CantidadCotizaciones))
                 || !string.IsNullOrEmpty(Convert.ToString(procesoSeleccion.CantidadProponentes))
                 || !string.IsNullOrEmpty(procesoSeleccion.EstadoProcesoSeleccionCodigo)
                 || !string.IsNullOrEmpty(procesoSeleccion.EtapaProcesoSeleccionCodigo)
                 || !string.IsNullOrEmpty(procesoSeleccion.EvaluacionDescripcion)
                 || !string.IsNullOrEmpty(procesoSeleccion.UrlSoporteEvaluacion)
                 || !string.IsNullOrEmpty(procesoSeleccion.TipoOrdenEligibilidadCodigo)

                )
                 return false;
            else
                return true;
        }


        public async Task<Respuesta> UploadMassiveLoadElegibilidad(string pIdDocument, string pUsuarioModifico)
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
                     Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoOrdenes, ConstantMessagesCargueElegibilidad.CamposVacios, (int)enumeratorAccion.CargueOrdenesMasivos, pUsuarioModifico, "")
                 };
            }
            try
            {


                int OrigenId = await _commonService.GetDominioIdByCodigoAndTipoDominio(OrigenArchivoCargue.OrdeELegibilidad, (int)EnumeratorTipoDominio.Origen_Documento_Cargue);

                ArchivoCargue archivoCargue = _context.ArchivoCargue.Where(r => r.OrigenId == 2 && r.Nombre.Trim().ToUpper().Equals(pIdDocument.ToUpper().Trim())).FirstOrDefault();

                List<TempOrdenLegibilidad> ListTempOrdenLegibilidad = await _context.TempOrdenLegibilidad.Where(r => r.ArchivoCargueId == archivoCargue.ArchivoCargueId && !(bool)r.EstaValidado).ToListAsync();

                if (ListTempOrdenLegibilidad.Count() > 0)
                {
                    foreach (var tempOrdenLegibilidad in ListTempOrdenLegibilidad)
                    {

                        //ProcesoSeleccionProponente
                        ProcesoSeleccionProponente procesoSeleccionProponente = new ProcesoSeleccionProponente()
                        {

                            //procesoSeleccionProponente Registros 
                            ProcesoSeleccionId = 50,
                            TipoProponenteCodigo = tempOrdenLegibilidad.TipoProponenteId.ToString(),
                            NombreProponente = tempOrdenLegibilidad.NombreProponente,
                            //TipoIdentificacionCodigo = tempOrdenLegibilidad.id ?
                            NumeroIdentificacion = tempOrdenLegibilidad.NumeroIddentificacionProponente,
                            LocalizacionIdMunicipio = tempOrdenLegibilidad.Minicipio.ToString(),
                            DireccionProponente = tempOrdenLegibilidad.Direccion,
                            TelefonoProponente = tempOrdenLegibilidad.Telefono,
                            EmailProponente = tempOrdenLegibilidad.Correo,
                            NombreRepresentanteLegal = tempOrdenLegibilidad.RepresentanteLegal,
                            CedulaRepresentanteLegal = tempOrdenLegibilidad.CedulaRepresentanteLegal.ToString()
                        };

                        _context.ProcesoSeleccionProponente.Add(procesoSeleccionProponente);
                        _context.SaveChanges();


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
                          
                        }
                        //Temporal proyecto update
                        tempOrdenLegibilidad.EstaValidado = true;
                        tempOrdenLegibilidad.FechaModificacion = DateTime.Now;
                        tempOrdenLegibilidad.UsuarioModificacion = pUsuarioModifico;
                        _context.TempOrdenLegibilidad.Update(tempOrdenLegibilidad);
                        _context.SaveChanges();
                    }


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
                            Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoOrdenes, ConstantMessagesCargueElegibilidad.NoExitenArchivos, (int)enumeratorAccion.CargueOrdenesMasivos, pUsuarioModifico, "")
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


    }
}
