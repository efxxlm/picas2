using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asivamosffie.services
{
    public class SelectionProcessScheduleService: ISelectionProcessScheduleService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public SelectionProcessScheduleService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _context = context;
            _commonService = commonService;
        }

        public async Task<List<ProcesoSeleccionCronograma>> GetListProcesoSeleccionCronogramaBypProcesoSeleccionId(int pProcesoSeleccionId)
        {
            return await _context.ProcesoSeleccionCronograma.Where(r=> !(bool)r.Eliminado && r.ProcesoSeleccionId == pProcesoSeleccionId).ToListAsync();
        }

        public async Task<ActionResult<List<ProcesoSeleccionCronograma>>> GetSelectionProcessSchedule()
        {
            return await _context.ProcesoSeleccionCronograma.ToListAsync();
        }

        public async Task<ProcesoSeleccionCronograma> GetSelectionProcessScheduleById(int id)
        {
            return await _context.ProcesoSeleccionCronograma.FindAsync(id);
        }

        public async Task<ActionResult<List<ProcesoSeleccionObservacion>>> GetRecordActivities()
        {
            return await _context.ProcesoSeleccionObservacion.ToListAsync();
        }


        //Registrar procesos de seleccion
        public async Task<Respuesta> Insert(ProcesoSeleccionCronograma procesoSeleccionCronograma)
        {
            Respuesta _response = new Respuesta();
            // int IdAccionCrearCuentaBancaria = _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Acciones && x.Codigo.Equals(ConstantCodigoAcciones.CrearCuentaBancaria)).Select(x => x.DominioId).First();
            try
            {
                if (procesoSeleccionCronograma != null)
                {
                    _context.Add(procesoSeleccionCronograma);
                    await _context.SaveChangesAsync();

                    return _response = new Respuesta
                    {
                        IsSuccessful = true,
                        IsValidation = false,
                        Data = procesoSeleccionCronograma,
                        Code = ConstantMessagesProcessSchedule.OperacionExitosa,
                        //      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Aportantes, ConstantMessagesContributor.OperacionExitosa, IdAccionCrearCuentaBancaria, cuentaBancaria.UsuarioCreacion.ToString(), ConstantMessagesContributor.OperacionExitosa)
                    };
                }
                else
                {
                    return _response = new Respuesta
                    {
                        IsSuccessful = false,
                        IsValidation = false,
                        Data = null,
                        Code = ConstantMessagesProcessSchedule.RecursoNoEncontrado,
                        //Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Aportantes, ConstantMessagesContributor.RecursoNoEncontrado, IdAccionCrearCuentaBancaria, cuentaBancaria.UsuarioCreacion.ToString(), ConstantMessagesContributor.RecursoNoEncontrado)
                    };
                }

            }
            catch (Exception ex)
            {
                return _response = new Respuesta
                {
                    IsSuccessful = false,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcessSchedule.ErrorInterno,
                    // Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Aportantes, ConstantMessagesContributor.ErrorInterno, IdAccionCrearCuentaBancaria, cuentaBancaria.UsuarioCreacion.ToString(), ex.InnerException.ToString()),

                };
            }
        }


        //Registro de actividedes
        public async Task<Respuesta> RecordActivities(ProcesoSeleccionObservacion pocesoSeleccionObservacion)
        {
            Respuesta _response = new Respuesta();
            try
            {
                if (pocesoSeleccionObservacion != null)
                {
                    _context.Add(pocesoSeleccionObservacion);
                    await _context.SaveChangesAsync();

                    return _response = new Respuesta { IsSuccessful = true, IsValidation = false, Data = pocesoSeleccionObservacion, Code = ConstantMessagesProcesoSeleccion.OperacionExitosa };
                }
                else
                {
                    return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesProcesoSeleccion.RecursoNoEncontrado };
                }

            }
            catch (Exception ex)
            {
                return _response = new Respuesta
                {
                    IsSuccessful = false,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcesoSeleccion.ErrorInterno,
                };
            }
        }

        public async Task<Respuesta> Update(ProcesoSeleccionCronograma procesoSeleccionCronograma)
        {
            Respuesta _response = new Respuesta();


            ProcesoSeleccionCronograma updateObj = await _context.ProcesoSeleccionCronograma.FindAsync(procesoSeleccionCronograma.ProcesoSeleccionCronogramaId);
            updateObj.ProcesoSeleccionId = procesoSeleccionCronograma.ProcesoSeleccionId;
            updateObj.NumeroActividad = procesoSeleccionCronograma.NumeroActividad;
            updateObj.Descripcion = procesoSeleccionCronograma.Descripcion;
            updateObj.FechaMaxima = procesoSeleccionCronograma.FechaMaxima;
            updateObj.EstadoActividadCodigo = procesoSeleccionCronograma.EstadoActividadCodigo;
            updateObj.FechaModificacion = DateTime.Now;
            updateObj.UsuarioModificacion = procesoSeleccionCronograma.UsuarioModificacion; //HttpContext.User.FindFirst("User").Value


            try
            {
                _context.Update(updateObj);
                 await _context.SaveChangesAsync();
                return _response = new Respuesta
                {
                    IsSuccessful = true,
                    IsValidation = false,
                    Data = updateObj,
                    Code = ConstantMessagesProcessSchedule.EditadoCorrrectamente,

                };
            }
            catch (Exception ex)
            {
                return _response = new Respuesta
                {
                    IsSuccessful = false,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcessSchedule.ErrorInterno,

                };
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                ProcesoSeleccionCronograma entity = await GetSelectionProcessScheduleById(id);
                _context.ProcesoSeleccionCronograma.Remove(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<ActionResult<List<ProcesoSeleccionMonitoreo>>> GetListProcesoSeleccionMonitoreoCronogramaByProcesoSeleccionId(int pProcesoSeleccionId)
        {
            return await _context.ProcesoSeleccionMonitoreo.Where(r => !(bool)r.Eliminado && r.ProcesoSeleccionId == pProcesoSeleccionId).Include(x=>x.ProcesoSeleccionCronogramaMonitoreo).Include(x=>x.ProcesoSeleccion).ToListAsync();
        }

        public async Task<Respuesta> setProcesoSeleccionMonitoreoCronograma(ProcesoSeleccionMonitoreo procesoSeleccionCronograma)
        {
            Respuesta _response = new Respuesta();
            int IdAccionCrearCuentaBancaria = _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Acciones && x.Codigo.Equals(ConstantCodigoAcciones.Crear_Cronograma_monitoreo)).Select(x => x.DominioId).First();
            try
            {
                if (procesoSeleccionCronograma != null)
                {
                    procesoSeleccionCronograma.FechaCreacion = DateTime.Now;
                    procesoSeleccionCronograma.Eliminado = false;
                    procesoSeleccionCronograma.NumeroProceso = "ACTCRONO" + _context.ProcesoSeleccionMonitoreo.Count().ToString();
                    procesoSeleccionCronograma.EstadoActividadCodigo = ConstanCodigoEstadoActividadCronogramaProcesoSeleccion.Creado;
                    foreach(var proceso in procesoSeleccionCronograma.ProcesoSeleccionCronogramaMonitoreo)
                    {
                        proceso.FechaCreacion = DateTime.Now;
                        proceso.UsuarioCreacion = procesoSeleccionCronograma.UsuarioCreacion;
                    }
                    _context.Add(procesoSeleccionCronograma);

                    //por aqui actualizo el estado de la solicitud
                    var solicitud = _context.ProcesoSeleccion.Find(procesoSeleccionCronograma.ProcesoSeleccionId);
                    solicitud.EstadoProcesoSeleccionCodigo = ConstanCodigoEstadoProcesoSeleccion.Apertura_En_Tramite;

                    await _context.SaveChangesAsync();

                    return _response = new Respuesta
                    {
                        IsSuccessful = true,
                        IsValidation = false,
                        Data = procesoSeleccionCronograma,
                        Code = ConstantMessagesProcessSchedule.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion_Cronograma, ConstantMessagesContributor.OperacionExitosa, IdAccionCrearCuentaBancaria, procesoSeleccionCronograma.UsuarioCreacion.ToString(), "CREAR EDITAR CRONOGRAMA EN MONITOREO")
                    };
                }
                else
                {
                    return _response = new Respuesta
                    {
                        IsSuccessful = false,
                        IsValidation = false,
                        Data = null,
                        Code = ConstantMessagesProcessSchedule.RecursoNoEncontrado,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion_Cronograma, ConstantMessagesContributor.RecursoNoEncontrado, IdAccionCrearCuentaBancaria, procesoSeleccionCronograma.UsuarioCreacion.ToString(), "ERROR EN LA CREACION EDICION EN CRONOGRAMA EN MONITOREO")
                    };
                }

            }
            catch (Exception ex)
            {
                return _response = new Respuesta
                {
                    IsSuccessful = false,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcessSchedule.ErrorInterno,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion_Cronograma, ConstantMessagesContributor.ErrorInterno, IdAccionCrearCuentaBancaria, procesoSeleccionCronograma.UsuarioCreacion.ToString(), ex.InnerException.ToString()),

                };
            }
        }

        public async Task<ActionResult<List<ProcesoSeleccionCronogramaMonitoreo>>> GetListProcesoSeleccionMonitoreoCronogramaByMonitoreoId(int pProcesoSeleccionId)
        {
            return await _context.ProcesoSeleccionCronogramaMonitoreo.Where(r => !(bool)r.Eliminado && r.ProcesoSeleccionMonitoreoId == pProcesoSeleccionId).ToListAsync();
        }
    }
}
