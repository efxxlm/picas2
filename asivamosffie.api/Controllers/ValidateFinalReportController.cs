﻿using System;
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
using Microsoft.AspNetCore.Authorization;

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
        [Route("VerificarInformeFinalValidacion")]
        public async Task<bool> VerificarInformeFinalEstadoCompleto([FromQuery] int pInformeFinalId)
        {
            try
            {
                return await _validateFinalReportService.VerificarInformeFinalValidacion(pInformeFinalId);
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
        [Route("UpdateStateValidateInformeFinalInterventoriaByInformeFinal")]
        public async Task<IActionResult> UpdateStateValidateInformeFinalInterventoriaByInformeFinal([FromBody] InformeFinal informeFinal)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                string user = HttpContext.User.FindFirst("User").Value;
                respuesta = await _validateFinalReportService.UpdateStateValidateInformeFinalInterventoriaByInformeFinal(informeFinal, user);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("UpdateStateValidateInformeFinalInterventoria")]
        public async Task<IActionResult> UpdateStateValidateInformeFinalInterventoria([FromQuery] int pInformeFinalInterventoriaId,[FromQuery] string code)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                string user = HttpContext.User.FindFirst("User").Value;
                //pInformeFinal.UsuarioCreacion = "LCT";
                respuesta = await _validateFinalReportService.UpdateStateValidateInformeFinalInterventoria(pInformeFinalInterventoriaId, code, user);
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

        public async Task<IActionResult> EditObservacionInformeFinal([FromBody] InformeFinalObservaciones pObservacion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pObservacion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                //pObservacion.UsuarioCreacion = "LCT";
                respuesta = await _validateFinalReportService.CreateEditObservacionInformeFinal(pObservacion);
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
        [Route("SendFinalReportToSupervision")]
        public async Task<IActionResult> SendFinalReportToSupervision([FromQuery] int pProyectoId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                //pObservacion.UsuarioCreacion = "LCT";
                respuesta = await _validateFinalReportService.SendFinalReportToSupervision(pProyectoId, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("ApproveInformeFinal")]
        public async Task<IActionResult> ApproveInformeFinal([FromQuery] int pProyectoId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                //pObservacion.UsuarioCreacion = "LCT";
                respuesta = await _validateFinalReportService.ApproveInformeFinal(pProyectoId, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("NoApprovedInformeFinal")]
        public async Task<IActionResult> NoApprovedInformeFinal([FromQuery] int pProyectoId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                //pObservacion.UsuarioCreacion = "LCT";
                respuesta = await _validateFinalReportService.NoApprovedInformeFinal(pProyectoId, HttpContext.User.FindFirst("User").Value);
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

    }
}
