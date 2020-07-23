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
                return await _context.ProcesoSeleccion.Where(r => !(bool)r.Eliminado).ToListAsync();
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
                return await _context.ProcesoSeleccion.FindAsync(id);
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
                    strCrearEditar = "CREAR CUENTA BANCARIA";
                    procesoSeleccion.FechaCreacion = DateTime.Now;
                    procesoSeleccion.Eliminado = false;

                    _context.ProcesoSeleccion.Add(procesoSeleccion);
                    return respuesta = new Respuesta
                    {
                        IsSuccessful = true, IsException = false,
                        IsValidation = false, Data = procesoSeleccion,
                        Code = ConstantMessagesProcesoSeleccion.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.OperacionExitosa, idAccionCrearProcesoSeleccion, procesoSeleccion.UsuarioCreacion, strCrearEditar)
                    };
                   
                }
                else
                {
                    strCrearEditar = "EDIT PROCESO CELECCION";
                    ProcesoSeleccionAntiguo = _context.ProcesoSeleccion.Find(procesoSeleccion.ProcesoSeleccionId);
                    //Auditoria
                    //ProcesoSeleccionAntiguo.UsuarioModificacion = procesoSeleccion.UsuarioModificacion;
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
                    ProcesoSeleccionAntiguo.UsuarioCreacion = "forozco"; ////HttpContext.User.FindFirst("User").Value
                    ProcesoSeleccionAntiguo.Eliminado = false;
                    ProcesoSeleccionAntiguo.FechaModificacion = DateTime.Now;

                    _context.ProcesoSeleccion.Update(ProcesoSeleccionAntiguo);
                }

                await _context.SaveChangesAsync();

                return respuesta = new Respuesta
                       {
                           IsSuccessful = true,IsException = false,
                           IsValidation = false, Data = ProcesoSeleccionAntiguo,
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

        //Listados de actvidades creadas
        public async Task<ActionResult<List<ProcesoSeleccionCronograma>>> GetRecordActivities(int ProcesoSeleccionId)
        {
            return await _context.ProcesoSeleccionCronograma.Where(r => !(bool)r.Eliminado && r.ProcesoSeleccionId == ProcesoSeleccionId).Include(x => x.ProcesoSeleccion).ToListAsync();
        }

        public async Task<Respuesta> CreateEditarProcesoSeleccionCronograma(ProcesoSeleccionCronograma procesoSeleccionCronograma)
        {
            Respuesta respuesta = new Respuesta();
            int idAccionCrearProcesoSeleccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Proceso_Seleccion, (int)EnumeratorTipoDominio.Acciones);
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

                    _context.ProcesoSeleccionCronograma.Add(procesoSeleccionCronograma);
                    return respuesta = new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Data = procesoSeleccionCronograma,
                        Code = ConstantMessagesProcesoSeleccion.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.OperacionExitosa, idAccionCrearProcesoSeleccion, procesoSeleccionCronograma.UsuarioCreacion, strCrearEditar)
                    };

                }
                else
                {
                    strCrearEditar = "EDIT PROCESO SELECCION CRONOGRAMA";
                    procesoSeleccionCronogramaAntiguo = _context.ProcesoSeleccionCronograma.Find(procesoSeleccionCronograma.ProcesoSeleccionCronogramaId);
                    //Auditoria
                    //ProcesoSeleccionAntiguo.UsuarioModificacion = procesoSeleccion.UsuarioModificacion;
                    procesoSeleccionCronogramaAntiguo.FechaModificacion = DateTime.Now;

                    //Registros
                    procesoSeleccionCronogramaAntiguo.ProcesoSeleccionId = procesoSeleccionCronograma.ProcesoSeleccionId;

                    procesoSeleccionCronogramaAntiguo.NumeroActividad = procesoSeleccionCronograma.NumeroActividad;
                    procesoSeleccionCronogramaAntiguo.Descripcion = procesoSeleccionCronograma.Descripcion;
                    procesoSeleccionCronogramaAntiguo.FechaMaxima = procesoSeleccionCronograma.FechaMaxima;
                    procesoSeleccionCronogramaAntiguo.EstadoActividadCodigo = procesoSeleccionCronograma.EstadoActividadCodigo;
                    procesoSeleccionCronogramaAntiguo.FechaCreacion = procesoSeleccionCronograma.FechaCreacion;
                    procesoSeleccionCronogramaAntiguo.UsuarioCreacion = "forozco"; ////HttpContext.User.FindFirst("User").Value
                    procesoSeleccionCronogramaAntiguo.Eliminado = false;
                    procesoSeleccionCronogramaAntiguo.FechaModificacion = DateTime.Now;

                    _context.ProcesoSeleccionCronograma.Update(procesoSeleccionCronogramaAntiguo);
                }

                await _context.SaveChangesAsync();

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = procesoSeleccionCronogramaAntiguo,
                    Code = ConstantMessagesProcesoSeleccion.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.OperacionExitosa, idAccionCrearProcesoSeleccion, procesoSeleccionCronograma.UsuarioCreacion, strCrearEditar)
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion, ConstantMessagesProcesoSeleccion.ErrorInterno, idAccionCrearProcesoSeleccion, procesoSeleccionCronograma.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
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
