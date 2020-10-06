using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        public GuaranteePolicyController(IGuaranteePolicyService guaranteePolicy, IOptions<AppSettings> settings)
        {
            _guaranteePolicy = guaranteePolicy;
            _settings = settings;

        } 
        [HttpPost]
        [Route("CreatePolizaObservacion")]
        public async Task<IActionResult> InsertPolizaObservacion(PolizaObservacion polizaObservacion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                //cuentaBancaria.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _guaranteePolicy.InsertPolizaObservacion(polizaObservacion);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }
         
        [HttpPost]
        [Route("CreatePolizaGarantia")]
        public async Task<IActionResult> InsertPolizaGarantia(PolizaGarantia polizaGarantia)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                //cuentaBancaria.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _guaranteePolicy.InsertPolizaGarantia(polizaGarantia);
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
                //contratoPoliza.UsuarioModificacion= HttpContext.User.FindFirst("User").Value;
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
                asivamosffie.model.APIModels.AppSettingsService _appSettingsService;

                _appSettingsService = toAppSettingsService(_settings);
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
        public async Task<ActionResult<List<PolizaGarantia>>> GetListPolizaObservacionByContratoPolizaId(int pContratoPolizaId)
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
         
        //[Route("GetContratoPolizaByIdContratoPolizaId")]
        //[HttpGet]
        ////public async Task<List<ContratoPoliza>> GetContratoPolizaByIdContratoPolizaId(int pContratoPolizaId)
        //public async Task<ContratoPoliza> GetContratoPolizaByIdContratoPolizaId(int pContratoPolizaId)
        //{
        //    var respuesta = await _guaranteePolicy.GetContratoPolizaByIdContratoPolizaId(pContratoPolizaId);
        //    return respuesta;
        //}


        //[HttpPost]

        //[HttpGet("{pIdContrato}")]

        //public async Task<Respuesta> AprobarContratoByIdContrato(int pIdContrato)
        //public async Task<ActionResult<Respuesta>> AprobarContratoByIdContrato(int pIdContrato)

        [HttpPost]
        [Route("AprobarContratoByIdContrato")]
        public async Task<IActionResult> AprobarContratoByIdContrato(int pIdContrato)
        {
            Respuesta rta = new Respuesta();
            try
            {
                HttpContext.Connection.RemoteIpAddress.ToString();
                //string UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                //AppSettings _appSettingsService;
                asivamosffie.model.APIModels.AppSettingsService _appSettingsService;

                _appSettingsService = toAppSettingsService(_settings);
                rta = await _guaranteePolicy.AprobarContratoByIdContrato(pIdContrato, _appSettingsService);

                return Ok(rta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
            // id - idContrato
            //var respuesta = await _guaranteePolicy.AprobarContratoByIdContrato(pIdContrato);

            //return respuesta;
        }

      
        [Route("GetContratoPolizaByIdContratoPolizaId")]
        [HttpGet]
        //public async Task<List<ContratoPoliza>> GetContratoPolizaByIdContratoPolizaId(int pContratoPolizaId)
        public async Task<ContratoPoliza> GetContratoPolizaByIdContratoPolizaId(int pContratoPolizaId)
        {
            var respuesta = await _guaranteePolicy.GetContratoPolizaByIdContratoPolizaId(pContratoPolizaId);
            return respuesta;
        }

        [HttpGet]
        [Route("GetListVistaContratoGarantiaPoliza")]
        public async Task<ActionResult<List<VistaContratoGarantiaPoliza>>> GetListVistaContratoGarantiaPoliza()
        {
            try
            {
                return await _guaranteePolicy.ListVistaContratoGarantiaPoliza();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         
        [HttpGet]
        [Route("GetListGrillaContratoGarantiaPoliza")]
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
    }
}
