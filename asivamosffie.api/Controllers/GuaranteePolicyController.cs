using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GuaranteePolicyController : ControllerBase
    {
        public readonly IGuaranteePolicyService _guaranteePolicy;
        private readonly IOptions<AppSettings> _settings;
        public AppSettingsService toAppSettingsService(IOptions<AppSettings> appSettings)
        {
            AppSettingsService appSettingsService = new AppSettingsService();
            appSettingsService.MailPort = appSettings.Value.MailPort;
            appSettingsService.MailServer = appSettings.Value.MailServer;
            appSettingsService.Password = appSettings.Value.Password;
            appSettingsService.Sender = appSettings.Value.Sender;

            return appSettingsService;

        }
         
        [Route("GetContratoByContratoId")]
        [HttpGet]
        public async Task<Contrato> GetContratoByContratoId(int pContratoId)
        {
            return await _guaranteePolicy.GetContratoByContratoId(pContratoId);
        }
         
         
        #region Old
        public GuaranteePolicyController(IGuaranteePolicyService guaranteePolicy, IOptions<AppSettings> settings)
        {
            _guaranteePolicy = guaranteePolicy;
            _settings = settings;
        }

        [HttpPost]
        [Route("CreateEditPolizaObservacion")]
        public async Task<IActionResult> InsertEditPolizaObservacion(PolizaObservacion polizaObservacion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                polizaObservacion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                polizaObservacion.UsuarioModificacion = HttpContext.User.FindFirst("User").Value;

                AppSettingsService _appSettingsService = toAppSettingsService(_settings);
                respuesta = await _guaranteePolicy.InsertEditPolizaObservacion(polizaObservacion, _appSettingsService);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("CreateEditPolizaGarantia")]
        public async Task<IActionResult> InsertEditPolizaGarantia(PolizaGarantia polizaGarantia)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                polizaGarantia.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                polizaGarantia.UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _guaranteePolicy.InsertEditPolizaGarantia(polizaGarantia);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("EditarContratoPoliza")]
        public async Task<IActionResult> EditarContratoPoliza(ContratoPoliza contratoPoliza)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                contratoPoliza.UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _guaranteePolicy.EditarContratoPoliza(contratoPoliza);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("CreateContratoPoliza")]
        public async Task<IActionResult> InsertContratoPoliza(ContratoPoliza contratoPoliza)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                AppSettingsService _appSettingsService = toAppSettingsService(_settings);
                contratoPoliza.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _guaranteePolicy.InsertContratoPoliza(contratoPoliza, _appSettingsService);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("GetListPolizaObservacionByContratoPolizaId")]
        [HttpGet]
        public async Task<ActionResult<List<PolizaObservacion>>> GetListPolizaObservacionByContratoPolizaId(int pContratoPolizaId)
        {
            try
            {
                return await _guaranteePolicy.GetListPolizaObservacionByContratoPolizaId(pContratoPolizaId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetListPolizaGarantiaByContratoPolizaId")]
        [HttpGet]
        public async Task<ActionResult<List<PolizaGarantia>>> GetListPolizaGarantiaByContratoPolizaId(int pContratoPolizaId)
        {
            try
            {
                return await _guaranteePolicy.GetListPolizaGarantiaByContratoPolizaId(pContratoPolizaId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("AprobarContratoByIdContrato")]
        public async Task<IActionResult> AprobarContratoByIdContrato(int pIdContrato)
        {
            Respuesta rta = new Respuesta();
            try
            {
                AppSettingsService _appSettingsService = toAppSettingsService(_settings);
                rta = await _guaranteePolicy.AprobarContratoByIdContrato(pIdContrato, _appSettingsService, HttpContext.User.FindFirst("User").Value);

                return Ok(rta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [Route("GetContratoPolizaByIdContratoPolizaId")]
        [HttpGet]
        public async Task<ContratoPoliza> GetContratoPolizaByIdContratoPolizaId(int pContratoPolizaId)
        {
            return await _guaranteePolicy.GetContratoPolizaByIdContratoPolizaId(pContratoPolizaId);
        }

        [Route("ConsultarRegistroCompletoCumple")]
        [HttpGet]
        public async Task<bool> ConsultarRegistroCompletoCumple(int pContratoPolizaId)
        {
            return await _guaranteePolicy.ConsultarRegistroCompletoCumple(pContratoPolizaId);
        }

        [Route("GetContratoPolizaByIdContratoId")]
        [HttpGet]
        public async Task<ContratoPoliza> GetContratoPolizaByIdContratoId(int pContratoId)
        {
            return await _guaranteePolicy.GetContratoPolizaByIdContratoId(pContratoId);
        }

        [Route("GetNotificacionContratoPolizaByIdContratoId")]
        [HttpGet]
        public async Task<NotificacionMensajeGestionPoliza> GetNotificacionContratoPolizaByIdContratoId(int pContratoId)
        {
            return await _guaranteePolicy.GetNotificacionContratoPolizaByIdContratoId(pContratoId);
        }

        [HttpPut]
        [Route("CambiarEstadoPoliza")]
        public async Task<IActionResult> CambiarEstadoPoliza([FromQuery] int pContratoPolizaId, string pCodigoNuevoEstadoPoliza)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _guaranteePolicy.CambiarEstadoPoliza(pContratoPolizaId, pCodigoNuevoEstadoPoliza, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPut]
        [Route("CambiarEstadoPolizaByContratoId")]
        public async Task<IActionResult> CambiarEstadoPolizaByContratoId([FromQuery] int pContratoId, string pCodigoNuevoEstadoPoliza)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _guaranteePolicy.CambiarEstadoPolizaByContratoId(pContratoId, pCodigoNuevoEstadoPoliza, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpGet]
        [Route("GetListVistaContratoGarantiaPoliza")]
        public async Task<ActionResult<List<VistaContratoGarantiaPoliza>>> GetListVistaContratoGarantiaPoliza(int pContratoId)
        {
            try
            {
                return await _guaranteePolicy.ListVistaContratoGarantiaPoliza(pContratoId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetListGrillaContratoGarantiaPolizaOld")]
        public async Task<ActionResult<List<GrillaContratoGarantiaPoliza>>> GetListGrillaContratoGarantiaPoliza()
        {
            try
            {
                return await _guaranteePolicy.ListGrillaContratoGarantiaPoliza();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetListGrillaContratoGarantiaPoliza")]
        public async Task<List<VGestionarGarantiasPolizas>> ListGrillaContratoGarantiaPolizaOptz()
        {
            try
            {
                return await _guaranteePolicy.ListGrillaContratoGarantiaPolizaOptz();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
