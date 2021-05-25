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
    public class DailyFollowUpController : ControllerBase
    {
        public readonly IDailyFollowUpService _dailyFollowUp;
        private readonly IOptions<AppSettings> _settings;

        public DailyFollowUpController(IDailyFollowUpService pDailyFollowUpService, IOptions<AppSettings> settings)
        {
            _dailyFollowUp = pDailyFollowUpService;
            _settings = settings;
        }

        [HttpGet]
        [Route("gridRegisterDailyFollowUp")]
        public async Task<List<VProyectosXcontrato>> gridRegisterDailyFollowUp()
        {
            var result = await _dailyFollowUp.gridRegisterDailyFollowUp(Int32.Parse(HttpContext.User.FindFirst("UserId").Value));
            return result;
        }

        [HttpGet]
        [Route("gridVerifyDailyFollowUp")]
        public async Task<List<VProyectosXcontrato>> gridVerifyDailyFollowUp()
        {
            var result = await _dailyFollowUp.gridVerifyDailyFollowUp(Int32.Parse(HttpContext.User.FindFirst("UserId").Value));
            return result;
        }

        [HttpGet]
        [Route("gridValidateDailyFollowUp")]
        public async Task<List<VProyectosXcontrato>> gridValidateDailyFollowUp()
        {
            var result = await _dailyFollowUp.gridValidateDailyFollowUp(Int32.Parse(HttpContext.User.FindFirst("UserId").Value));
            return result;
        }

        [HttpPost]
        [Route("CreateEditDailyFollowUp")]
        public async Task<IActionResult> CreateEditDailyFollowUp([FromBody] SeguimientoDiario pSeguimientoDiario)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pSeguimientoDiario.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _dailyFollowUp.CreateEditDailyFollowUp(pSeguimientoDiario);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpDelete]
        [Route("DeleteDailyFollowUp")]
        public async Task<IActionResult> DeleteDailyFollowUp([FromQuery] int pId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                string usuario = HttpContext.User.FindFirst("User").Value;
                respuesta = await _dailyFollowUp.DeleteDailyFollowUp(pId, usuario);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPut]
        [Route("SendToSupervisionSupport")]
        public async Task<IActionResult> SendToSupervisionSupport([FromQuery] int pId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                string usuario = HttpContext.User.FindFirst("User").Value;
                respuesta = await _dailyFollowUp.SendToSupervisionSupport(pId, usuario);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPut]
        [Route("SendToSupervision")]
        public async Task<IActionResult> SendToSupervision([FromQuery] int pId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                string usuario = HttpContext.User.FindFirst("User").Value;
                respuesta = await _dailyFollowUp.SendToSupervision(pId, usuario);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpGet]
        [Route("GetDailyFollowUpById")]
        public async Task<SeguimientoDiario> GetDailyFollowUpById([FromQuery] int pId)
        {
            var result = await _dailyFollowUp.GetDailyFollowUpById(pId);
            return result;
        }

        [HttpGet]
        [Route("GetDatesAvailableByContratacioProyectoId")]
        public async Task<List<string>> GetDatesAvailableByContratacioProyectoId([FromQuery] int pId)
        {
            var result = await _dailyFollowUp.GetDatesAvailableByContratacioProyectoId(pId);
            return result;
        }

        [HttpGet]
        [Route("GetDailyFollowUpByContratacionProyectoId")]
        public async Task<List<SeguimientoDiario>> GetDailyFollowUpByContratacionProyectoId([FromQuery] int pId)
        {
            var result = await _dailyFollowUp.GetDailyFollowUpByContratacionProyectoId(pId);
            return result;
        }

        [Route("CreateEditObservacion")]
        [HttpPost]
        public async Task<Respuesta> CreateEditObservacion([FromBody] SeguimientoDiario pSeguimientoDiario, [FromQuery] bool esSupervisor)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pSeguimientoDiario.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _dailyFollowUp.CreateEditObservacion(pSeguimientoDiario, esSupervisor);
                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("ApproveDailyFollowUp")]
        public async Task<IActionResult> ApproveDailyFollowUp([FromQuery] int pId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                string usuario = HttpContext.User.FindFirst("User").Value;
                respuesta = await _dailyFollowUp.ApproveDailyFollowUp(pId, usuario);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPut]
        [Route("ReturnToComptroller")]
        public async Task<IActionResult> ReturnToComptroller([FromQuery] int pId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                string usuario = HttpContext.User.FindFirst("User").Value;
                respuesta = await _dailyFollowUp.ReturnToComptroller(pId, usuario);
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