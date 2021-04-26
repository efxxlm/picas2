using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValidateFinalReportController : Controller
    {

        private readonly IValidateFinalReportService _validateFinalReportService;
        private readonly IOptions<AppSettings> _settings;

        public ValidateFinalReportController(IValidateFinalReportService validateFinalReportService, IOptions<AppSettings> settings)
        {
            _validateFinalReportService = validateFinalReportService;
            _settings = settings;
        }
  
        [Route("GetListInformeFinal")]
        [HttpGet]
        public async Task<List<InformeFinal>> GetListInformeFinal()
        {
            return await _validateFinalReportService.GetListInformeFinal();
        }

        [HttpGet]
        [Route("GetInformeFinalByProyectoId")]
        public async Task<List<dynamic>> GetInformeFinalByProyectoId([FromQuery] int pProyectoId)
        {
            try
            {
                return await _validateFinalReportService.GetInformeFinalByProyectoId(pProyectoId);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetInformeFinalListaChequeoByInformeFinalId")]
        public async Task<List<InformeFinalInterventoria>> GetInformeFinalListaChequeoByInformeFinalId([FromQuery] int pInformeFinalId)
        {
            try
            {
                return await _validateFinalReportService.GetInformeFinalListaChequeoByInformeFinalId(pInformeFinalId);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetInformeFinalInterventoriaObservacionByInformeFinalInterventoria")]
        public async Task<InformeFinalInterventoriaObservaciones> GetInformeFinalInterventoriaObservacionByInformeFinalInterventoria([FromQuery] int pInformeFinalInterventoriaId)
        {
            try
            {
                return await _validateFinalReportService.GetInformeFinalInterventoriaObservacionByInformeFinalInterventoria(pInformeFinalInterventoriaId);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Creaciones y modificaciones

        [HttpPost]
        [Route("UpdateStateApproveInformeFinalInterventoriaByInformeFinal")]
        public async Task<IActionResult> UpdateStateValidateInformeFinalInterventoriaByInformeFinal([FromBody] InformeFinal informeFinal)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                string user = HttpContext.User.FindFirst("User").Value;
                respuesta = await _validateFinalReportService.UpdateStateApproveInformeFinalInterventoriaByInformeFinal(informeFinal, user);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("UpdateStateApproveInformeFinalInterventoria")]
        public async Task<IActionResult> UpdateStateApproveInformeFinalInterventoria([FromQuery] int pInformeFinalInterventoriaId,[FromQuery] string code)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                string user = HttpContext.User.FindFirst("User").Value;
                //pInformeFinal.UsuarioCreacion = "LCT";
                respuesta = await _validateFinalReportService.UpdateStateApproveInformeFinalInterventoria(pInformeFinalInterventoriaId, code, user);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("CreateEditObservacionInformeFinal")]

        public async Task<IActionResult> EditObservacionInformeFinal([FromQuery] bool tieneObservacion, [FromBody] InformeFinalObservaciones pObservacion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pObservacion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                //pObservacion.UsuarioCreacion = "LCT";
                respuesta = await _validateFinalReportService.CreateEditObservacionInformeFinal(pObservacion, tieneObservacion);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("CreateEditInformeFinalInterventoriaObservacion")]
        public async Task<IActionResult> CreateEditInformeFinalInterventoriaObservacion([FromBody] InformeFinalInterventoriaObservaciones pObservacion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pObservacion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                //pObservacion.UsuarioCreacion = "LCT";
                respuesta = await _validateFinalReportService.CreateEditInformeFinalInterventoriaObservacion(pObservacion);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("SendFinalReportToInterventor")]
        public async Task<IActionResult> SendFinalReportToInterventor([FromQuery] int pProyectoId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _validateFinalReportService.SendFinalReportToInterventor(pProyectoId, HttpContext.User.FindFirst("User").Value);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("SendFinalReportToFinalVerification")]
        public async Task<IActionResult> SendFinalReportToFinalVerification([FromQuery] int pProyectoId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _validateFinalReportService.SendFinalReportToFinalVerification(pProyectoId, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpGet]
        [Route("GetInformeFinalInterventoriaObservacionByInformeFinalObservacion")]
        public async Task<InformeFinalInterventoriaObservaciones> GetInformeFinalInterventoriaObservacionByInformeFinalObservacion([FromQuery] int pObservacionId)
        {
            try
            {
                return await _validateFinalReportService.GetInformeFinalInterventoriaObservacionByInformeFinalObservacion(pObservacionId);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("GetListInformeFinalObservacionesInterventoria")]
        public async Task<InformeFinal> GetListInformeFinalObservacionesInterventoria([FromQuery] int informeFinalId)
        {
            try
            {
                return await _validateFinalReportService.GetListInformeFinalObservacionesInterventoria(informeFinalId);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
