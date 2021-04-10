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
            return await _context.ProcesoSeleccionCronograma.Where(r=> !(bool)r.Eliminado && r.ProcesoSeleccionId == pProcesoSeleccionId).Include(x=>x.CronogramaSeguimiento).ToListAsync();
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
            List<ProcesoSeleccionMonitoreo> listaMonitoreo = new List<ProcesoSeleccionMonitoreo>();

            

            listaMonitoreo = _context.ProcesoSeleccionMonitoreo.Where(r => !(bool)r.Eliminado && r.ProcesoSeleccionId == pProcesoSeleccionId).Include(x => x.ProcesoSeleccionCronogramaMonitoreo).Include(x => x.ProcesoSeleccion).ToList();

            foreach (var monitoreo in listaMonitoreo)
            {
                List<string> listaEstadosTecnico = new List<string> { "4", "6" };
                List<string> listaEstadosFiduciario = new List<string> { "5", "7" };
                string observacion = "";
                SesionComiteSolicitud sesionComiteSolicitud = _context.SesionComiteSolicitud
                                                                    .Where(x => x.SolicitudId == monitoreo.ProcesoSeleccionMonitoreoId &&
                                                                           x.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Actualizacion_Cronograma_Proceso_Seleccion 
                                                                           //&&
                                                                           //listaEstadosTecnico.Contains( x.EstadoCodigo )
                                                                           )

                                                                    ?.FirstOrDefault();

                observacion = string.IsNullOrEmpty(sesionComiteSolicitud?.ObservacionesFiduciario) ? sesionComiteSolicitud?.Observaciones : sesionComiteSolicitud?.ObservacionesFiduciario;

                //if ( string.IsNullOrEmpty(observacion)){
                //    observacion = _context.SesionComiteSolicitud
                //                                                    .Where(x => x.SolicitudId == monitoreo.ProcesoSeleccionMonitoreoId &&
                //                                                           x.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Actualizacion_Cronograma_Proceso_Seleccion &&
                //                                                           listaEstadosFiduciario.Contains(x.EstadoCodigo))
                //                                                    ?.FirstOrDefault()
                //                                                    ?.ObservacionesFiduciario;
                //}

                monitoreo.ObservacionDevolucionRechazo = observacion;
            }

            return listaMonitoreo; 
        }

        public async Task<Respuesta> setProcesoSeleccionMonitoreoCronograma(ProcesoSeleccionMonitoreo procesoSeleccionCronograma
            , string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender
            )
        {
            Respuesta _response = new Respuesta();
            int IdAccionCrearCuentaBancaria = _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Acciones && x.Codigo.Equals(ConstantCodigoAcciones.Crear_Cronograma_monitoreo)).Select(x => x.DominioId).First();
            try
            {
                if (procesoSeleccionCronograma != null)
                {
                    //Editar
                    if (procesoSeleccionCronograma.ProcesoSeleccionMonitoreoId>0)
                    {
                        ProcesoSeleccionMonitoreo procesoSeleccionMonitoreo = _context.ProcesoSeleccionMonitoreo.Find(procesoSeleccionCronograma.ProcesoSeleccionMonitoreoId);

                        procesoSeleccionMonitoreo.FechaModificacion = DateTime.Now;
                        procesoSeleccionMonitoreo.EnviadoComiteTecnico = procesoSeleccionCronograma.EnviadoComiteTecnico;

                        if (procesoSeleccionMonitoreo.EnviadoComiteTecnico == true)
                        {
                            procesoSeleccionMonitoreo.EstadoActividadCodigo = ConstanCodigoEstadoActividadCronogramaProcesoSeleccion.EnTramite;
                        }

                        // lista de cronograma monitoreo existentes
                        List<ProcesoSeleccionCronogramaMonitoreo> listaCronogramaMonitoreo = _context.ProcesoSeleccionCronogramaMonitoreo
                                                                                                        .Where(r => r.ProcesoSeleccionMonitoreoId == procesoSeleccionCronograma.ProcesoSeleccionMonitoreoId)
                                                                                                        .ToList();

                        // marco eliminados los que no vengan en el servicio
                        foreach (ProcesoSeleccionCronogramaMonitoreo proceso in listaCronogramaMonitoreo)
                        {
                            if (procesoSeleccionCronograma.ProcesoSeleccionCronogramaMonitoreo.Where(r => r.ProcesoSeleccionCronogramaMonitoreoId == proceso.ProcesoSeleccionCronogramaMonitoreoId).Count() == 0)
                            {
                                proceso.Eliminado = true;
                            }
                        }


                        foreach (var proceso in procesoSeleccionCronograma.ProcesoSeleccionCronogramaMonitoreo)
                        {
                            if (proceso.ProcesoSeleccionCronogramaMonitoreoId > 0)
                            {
                                ProcesoSeleccionCronogramaMonitoreo procesoSeleccionCronogramaMonitoreo = _context.ProcesoSeleccionCronogramaMonitoreo.Find(proceso.ProcesoSeleccionCronogramaMonitoreoId);

                                procesoSeleccionCronogramaMonitoreo.Descripcion = proceso.Descripcion;
                                procesoSeleccionCronogramaMonitoreo.Eliminado = proceso.Eliminado;
                                procesoSeleccionCronogramaMonitoreo.EstadoActividadCodigo = proceso.EstadoActividadCodigo;
                                procesoSeleccionCronogramaMonitoreo.FechaMaxima = proceso.FechaMaxima;
                                procesoSeleccionCronogramaMonitoreo.FechaModificacion = DateTime.Now;
                                procesoSeleccionCronogramaMonitoreo.NumeroActividad = proceso.NumeroActividad;
                                procesoSeleccionCronogramaMonitoreo.UsuarioModificacion = procesoSeleccionCronograma.UsuarioCreacion;

                            }
                            else
                            {
                                proceso.FechaCreacion = DateTime.Now;
                                proceso.UsuarioCreacion = procesoSeleccionCronograma.UsuarioCreacion;

                                procesoSeleccionMonitoreo.ProcesoSeleccionCronogramaMonitoreo.Add(proceso);
                                
                            }
                            
                        }
                    }
                    // nuevo
                    else
                    {
                        
                        procesoSeleccionCronograma.FechaCreacion = DateTime.Now;
                        procesoSeleccionCronograma.Eliminado = false;
                        procesoSeleccionCronograma.NumeroProceso = Helpers.Helpers.Consecutive("ACTCRONO", _context.ProcesoSeleccionMonitoreo.Count());
                        procesoSeleccionCronograma.EstadoActividadCodigo = ConstanCodigoEstadoActividadCronogramaProcesoSeleccion.Creado;

                        List<ProcesoSeleccionCronograma> listaCronogramas = _context.ProcesoSeleccionCronograma
                                                                                        .Where(r => r.ProcesoSeleccionId == procesoSeleccionCronograma.ProcesoSeleccionId)
                                                                                        .ToList();

                        // marco eliminados los que no vengan en el servicio
                        foreach (ProcesoSeleccionCronograma proceso in listaCronogramas)
                        {
                            if (procesoSeleccionCronograma.ProcesoSeleccionCronogramaMonitoreo.Where(r => r.ProcesoSeleccionCronogramaId == proceso.ProcesoSeleccionCronogramaId).Count() == 0)
                            {
                                ProcesoSeleccionCronogramaMonitoreo procesoSeleccionCronogramaMonitoreo = new ProcesoSeleccionCronogramaMonitoreo()
                                {
                                    Descripcion = proceso.Descripcion,
                                    Eliminado = true,
                                    EstadoActividadCodigo = proceso.EstadoActividadCodigo,
                                    //FechaCreacion = DateTime.Now,
                                    FechaMaxima = proceso.FechaMaxima,
                                    NumeroActividad = proceso.NumeroActividad,
                                    ProcesoSeleccionCronogramaId = proceso.ProcesoSeleccionCronogramaId,
                                    //ProcesoSeleccionMonitoreoId = proceso.
                                    
                                };


                                procesoSeleccionCronograma.ProcesoSeleccionCronogramaMonitoreo.Add(procesoSeleccionCronogramaMonitoreo);
                            }
                        }

                        foreach (var proceso in procesoSeleccionCronograma.ProcesoSeleccionCronogramaMonitoreo)
                        {                            
                            proceso.FechaCreacion = DateTime.Now;
                            proceso.UsuarioCreacion = procesoSeleccionCronograma.UsuarioCreacion;
                        }
                        _context.Add(procesoSeleccionCronograma);
                    }
                    

                    //por aqui actualizo el estado de la solicitud si lo estoy envaiando a comite y se envia notificacion
                    if(procesoSeleccionCronograma.EnviadoComiteTecnico==true)
                    {
                        //jflorez ya no le cambio el estado porque no debería afectar al papá
                        /*var solicitud = _context.ProcesoSeleccion.Find(procesoSeleccionCronograma.ProcesoSeleccionId);
                        solicitud.EstadoProcesoSeleccionCodigo = ConstanCodigoEstadoProcesoSeleccion.Apertura_En_Tramite;*/
                        var usuariosecretario = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Secretario_Comite).Select(x => x.Usuario.Email).ToList();
                        foreach (var usuario in usuariosecretario)
                        {
                            Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.SolicitarApertura);
                            string template = TemplateRecoveryPassword.Contenido.Replace("_LinkF_", pDominioFront).Replace("[NumeroSol]", procesoSeleccionCronograma.NumeroProceso).Replace("[FechaSol]", procesoSeleccionCronograma.FechaCreacion.ToString("dd/MM/yy"));
                            bool blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuario, "Proceso de selección en tramite", template, pSentender, pPassword, pMailServer, pMailPort);
                        }
                    }
                    

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
