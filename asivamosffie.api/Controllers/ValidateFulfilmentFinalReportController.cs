using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValidateFulfilmentFinalReportController : Controller
    {

        private readonly IValidateFulfilmentFinalReportService _validateFinalReportService;
        private readonly IOptions<AppSettings> _settings;

        public ValidateFulfilmentFinalReportController(IValidateFulfilmentFinalReportService validateFinalReportService, IOptions<AppSettings> settings)
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

        //Creaciones y modificaciones

        [HttpPost]
        [Route("CreateEditObservacionInformeFinal")]

        public async Task<IActionResult> CreateEditObservacionInformeFinal([FromQuery] bool tieneObservacion, [FromBody] InformeFinalObservaciones pObservacion)
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
        [Route("CreateEditObservacionInformeFinalInterventoria")]

        public async Task<IActionResult> CreateEditObservacionInformeFinalInterventoria([FromQuery] bool tieneObservacion, [FromBody] InformeFinalObservaciones pObservacion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pObservacion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                //pObservacion.UsuarioCreacion = "LCT";
                respuesta = await _validateFinalReportService.CreateEditObservacionInformeFinalInterventoria(pObservacion, tieneObservacion);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("SendFinalReportToSupervision")]
        public async Task<IActionResult> SendFinalReportToSupervision([FromQuery] int pProyectoId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _validateFinalReportService.SendFinalReportToSupervision(pProyectoId, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("ApproveFinalReportByFulfilment")]
        public async Task<IActionResult> ApproveFinalReportByFulfilment([FromQuery] int pProyectoId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _validateFinalReportService.ApproveFinalReportByFulfilment(pProyectoId, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

    }
}
