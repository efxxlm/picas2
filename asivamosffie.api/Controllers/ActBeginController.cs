using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ActBeginController : ControllerBase
    {
        public readonly IActBeginService _ActBegin;
        private readonly IOptions<AppSettings> _settings;

        public ActBeginController(IActBeginService actBegin, IOptions<AppSettings> settings)
        {
            _ActBegin = actBegin;
            _settings = settings;

        }

        //public async Task<ActionResult<VistaGenerarActaInicioContrato>> GetListVistaGenerarActaInicio(int pContratoId)
        [Route("GetVistaGenerarActaInicio")]
        [HttpGet]        
        public async Task<ActionResult<VistaGenerarActaInicioContrato>> GetVistaGenerarActaInicio(int pContratoId)
        {
            try
            {
                int pUserId = Int32.Parse(HttpContext.User.FindFirst("UserId").Value);
                return await _ActBegin.GetListVistaGenerarActaInicio(pContratoId, pUserId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Route("GetContratoObservacionByIdContratoId")]
        [HttpGet]        
        public async Task<ContratoObservacion> GetContratoObservacionByIdContratoId(int pContratoId, bool pEsSupervisor)
        {
            var respuesta = await _ActBegin.GetContratoObservacionByIdContratoId(pContratoId,  pEsSupervisor);
            return respuesta;
        }

        [Route("GetContratoObservacionByIdContratoIdUltimaArchivada")]
        [HttpGet]        
        public async Task<ContratoObservacion> GetContratoObservacionByIdContratoIdUltimaArchivada(int pContratoId, bool pEsSupervisor)
        {
            var respuesta = await _ActBegin.GetContratoObservacionByIdContratoIdUltimaArchivada(pContratoId,  pEsSupervisor);
            return respuesta;
        }

        [Route("GetContratoByIdContratoId")]
        [HttpGet]        
        public async Task<Contrato> GetContratoByIdContratoId(int pContratoId)
        {
            var respuesta = await _ActBegin.GetContratoByIdContratoId(pContratoId);
            return respuesta;
        }


        [HttpPost]
        [Route("CreateEditContratoObservacion")]        
        public async Task<IActionResult> InsertEditContratoObservacion( Contrato pContrato )
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pContrato.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                    
                respuesta = await _ActBegin.InsertEditContratoObservacion( pContrato );
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("EnviarCorreoSupervisorContratista")]             
        public async Task<IActionResult> EnviarCorreoSupervisorContratista(int pContratoId, int pPerfilId)
            //public async Task<IActionResult> EnviarCorreoSupervisor(int pContratoId)
        {
            Respuesta rta = new Respuesta();
            try
            {
                HttpContext.Connection.RemoteIpAddress.ToString();
                //string UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                //AppSettings _appSettingsService;
                asivamosffie.model.APIModels.AppSettingsService _appSettingsService;

                _appSettingsService = toAppSettingsService(_settings);
                rta = await _ActBegin.EnviarCorreoSupervisorContratista(pContratoId, _appSettingsService,pPerfilId);

                return Ok(rta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
          
        }

        public AppSettingsService toAppSettingsService(IOptions<AppSettings> appSettings)
        {
            AppSettingsService appSettingsService = new AppSettingsService();
            appSettingsService.MailPort = appSettings.Value.MailPort;
            appSettingsService.MailServer = appSettings.Value.MailServer;
            appSettingsService.Password = appSettings.Value.Password;
            appSettingsService.Sender = appSettings.Value.Sender;

            return appSettingsService;

        }

        [HttpGet]
        [Route("GetListGrillaActaInicio")]
        public async Task<ActionResult<List<GrillaActaInicio>>> GetListGrillaActaInicio(int pPerfilId)
        {
            try
            {
                return await _ActBegin.GetListGrillaActaInicio(pPerfilId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("CambiarEstadoActa")]
        
        public async Task<IActionResult> CambiarEstadoActa(int pContratoId, string pNuevoCodigoEstadoActa, string pUsuarioModifica)
        {
            Respuesta respuesta = new Respuesta();
            try
            {

                pUsuarioModifica=HttpContext.User.FindFirst("User").Value;
                respuesta = await _ActBegin.CambiarEstadoActa( pContratoId,  pNuevoCodigoEstadoActa,  pUsuarioModifica);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }


        [HttpPut]
        [Route("CambiarEstadoVerificacionActa")]
        
        public async Task<IActionResult> CambiarEstadoVerificacionActa(int pContratoId, string pNuevoCodigoEstadoVerificacionActa, string pUsuarioModifica)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                //respuesta = await _ActBegin.CambiarEstadoActa(pSesionComiteSolicitud, pCodigoEstado, HttpContext.User.FindFirst("User").Value);
                pUsuarioModifica = HttpContext.User.FindFirst("User").Value;
                respuesta = await _ActBegin.CambiarEstadoVerificacionActa(pContratoId, pNuevoCodigoEstadoVerificacionActa, pUsuarioModifica);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }





        [HttpGet]
        [Route("GetPlantillaActaInicio")]
        public async Task<FileResult> GetPlantillaActaInicio(int pContratoId)
        {
            return File(await _ActBegin.GetPlantillaActaInicio(pContratoId), "application/pdf");
        }


        [HttpPost]
        [Route("EditCargarActaSuscritaContrato")]       
        public async Task<IActionResult> EditCargarActaSuscritaContrato(  int pContratoId,  DateTime pFechaFirmaContratista,  DateTime pFechaFirmaActaContratistaInterventoria
             , [FromForm] IFormFile pFile,  string pUsuarioModificacion
          )
        {
            Respuesta respuesta = new Respuesta();
            try
            {

                asivamosffie.model.APIModels.AppSettingsService _appSettingsService;

                _appSettingsService = toAppSettingsService(_settings);

                //cuentaBancaria.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                pUsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _ActBegin.EditarCargarActaSuscritaContrato( pContratoId,  pFechaFirmaContratista,  pFechaFirmaActaContratistaInterventoria                     
             ,  pUsuarioModificacion
             //, _appSettingsService
             );
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("CreatePlazoEjecucionFase2Construccion")]
        
        public async Task<IActionResult> CreatePlazoEjecucionFase2Construccion(int pContratoId, int pPlazoFase2PreMeses, int pPlazoFase2PreDias, string pObservacionesConsideracionesEspeciales, string pUsuarioModificacion, DateTime pFechaActaInicioFase1, DateTime pFechaTerminacionFase2, bool pEsSupervisor, bool pEsActa)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                //cuentaBancaria.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _ActBegin.GuardarPlazoEjecucionFase2Construccion( pContratoId,  pPlazoFase2PreMeses,  pPlazoFase2PreDias,  pObservacionesConsideracionesEspeciales,  pUsuarioModificacion,  pFechaActaInicioFase1,  pFechaTerminacionFase2,  pEsSupervisor,  pEsActa);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        

        [HttpPost]
        [Route("CreateTieneObservacionesActaInicio")]

        public async Task<IActionResult> CreateTieneObservacionesActaInicio(int pContratoId, string pObservacionesActa, string pUsuarioModificacion, bool pEsSupervisor, bool pEsActa
          )
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                //cuentaBancaria.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                pUsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _ActBegin.GuardarTieneObservacionesActaInicio( pContratoId,  pObservacionesActa,  pUsuarioModificacion,  pEsSupervisor,  pEsActa);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("EditarContratoObservacion")]        
        public async Task<IActionResult> EditarContratoObservacion(int pContratoId, int pPlazoFase2PreMeses, int pPlazoFase2PreDias, string pObservacion, string pUsuarioModificacion, DateTime pFechaActaInicioFase1, DateTime pFechaTerminacionFase2, bool pEsSupervisor, bool pEsActa)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                //cuentaBancaria.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                pUsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _ActBegin.EditarContratoObservacion( pContratoId,  pPlazoFase2PreMeses,  pPlazoFase2PreDias,  pObservacion,  pUsuarioModificacion,  pFechaActaInicioFase1,  pFechaTerminacionFase2,  pEsSupervisor,  pEsActa);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }


    }

}
