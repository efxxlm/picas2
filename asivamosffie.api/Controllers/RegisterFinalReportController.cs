using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asivamosffie.services.Interfaces;
using asivamosffie.model.Models;
using Microsoft.Extensions.Options;
using lalupa.Authorization.JwtHelpers;
using asivamosffie.services.Exceptions;
using asivamosffie.model.APIModels;
using Microsoft.AspNetCore.Authorization;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegisterFinalReportController : ControllerBase
    {
        public readonly IRegisterFinalReportService _registerFinalReport;
        private readonly IOptions<AppSettings> _settings;

        public RegisterFinalReportController(IRegisterFinalReportService pRegisterFinalReportService, IOptions<AppSettings> settings)
        {
            _registerFinalReport = pRegisterFinalReportService;
            _settings = settings;
        }

        [Route("gridRegisterFinalReport")]
        [HttpGet]
        public async Task<ActionResult<List<VProyectosCierre>>> gridRegisterFinalReport()
        {
            try
            {
                return await _registerFinalReport.gridRegisterFinalReport();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetInformeFinalByProyectoId")]
        public async Task<List<dynamic>> GetInformeFinalByProyectoId([FromQuery] int pProyectoId)
        {
            try
            {
                return await _registerFinalReport.GetInformeFinalByProyectoId(pProyectoId);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("VerificarInformeFinalEstadoCompleto")]
        public async Task<bool> VerificarInformeFinalEstadoCompleto(int pInformeFinalId)
        {
            try
            {
                return await _registerFinalReport.VerificarInformeFinalEstadoCompleto(pInformeFinalId);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }



        [HttpGet]
        [Route("GetInformeFinalListaChequeoByProyectoId")]
        public async Task<List<dynamic>> GetInformeFinalListaChequeoByContratacionProyectoId([FromQuery] int pProyectoId)
        {
            try
            {
                return await _registerFinalReport.GetInformeFinalListaChequeoByProyectoId(pProyectoId);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetInformeFinalAnexoByInformeFinalInterventoriaId")]
        public async Task<InformeFinalInterventoria> GetInformeFinalAnexoByInformeFinalInterventoriaId([FromQuery] int pInformeFinalInterventoriaId)
        {
            try
            {
                return await _registerFinalReport.GetInformeFinalAnexoByInformeFinalInterventoriaId(pInformeFinalInterventoriaId);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetInformeFinalAnexoByInformeFinalAnexoId")]
        public async Task<InformeFinalAnexo> GetInformeFinalAnexoByInformeFinalAnexoId([FromQuery] int pInformeFinalAnexoId)
        {
            try
            {
                return await _registerFinalReport.GetInformeFinalAnexoByInformeFinalAnexoId(pInformeFinalAnexoId);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetInformeFinalByInformeFinalId")]
        public async Task<InformeFinal> GetInformeFinalByInformeFinalId([FromQuery] int pInformeFinalId)
        {
            try
            {
                return await _registerFinalReport.GetInformeFinalByInformeFinalId(pInformeFinalId);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetInformeFinalInterventoriaObservacionByInformeFinalObservacion")]
        public async Task<InformeFinalInterventoriaObservaciones> GetInformeFinalInterventoriaObservacionByInformeFinalObservacion([FromQuery] int pObservacionId)
        {
            try
            {
                return await _registerFinalReport.GetInformeFinalInterventoriaObservacionByInformeFinalObservacion(pObservacionId);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Creaciones y modificaciones

        [HttpPost]
        [Route("CreateEditInformeFinal")]
        public async Task<IActionResult> CreateEditInformeFinal([FromBody] InformeFinal pInformeFinal)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pInformeFinal.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                //pInformeFinal.UsuarioCreacion = "LCT";
                respuesta = await _registerFinalReport.CreateEditInformeFinal(pInformeFinal);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("CreateEditInformeFinalInterventoria")]
        public async Task<IActionResult> CreateEditInformeFinalInterventoria([FromBody] InformeFinalInterventoria pInformeFinalInterventoriaId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pInformeFinalInterventoriaId.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                //pInformeFinalInterventoriaId.UsuarioCreacion = "LCT";
                respuesta = await _registerFinalReport.CreateEditInformeFinalInterventoria(pInformeFinalInterventoriaId);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("CreateEditInformeFinalAnexo/{pInformeFinalInterventoriaId:int}")]

        public async Task<IActionResult> CreateEditInformeFinalAnexo([FromBody] InformeFinalAnexo pInformeFinalAnexoId,[FromRoute()] int pInformeFinalInterventoriaId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pInformeFinalAnexoId.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                //pInformeFinalAnexoId.UsuarioCreacion = "LCT";
                respuesta = await _registerFinalReport.CreateEditInformeFinalAnexo(pInformeFinalAnexoId, pInformeFinalInterventoriaId);
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
                respuesta = await _registerFinalReport.CreateEditInformeFinalInterventoriaObservacion(pObservacion);
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
                respuesta = await _registerFinalReport.SendFinalReportToSupervision(pProyectoId, HttpContext.User.FindFirst("User").Value);
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