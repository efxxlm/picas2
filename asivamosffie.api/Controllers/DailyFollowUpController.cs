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

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyFollowUpController : ControllerBase
    {
        public readonly IDailyFollowUpService  _dailyFollowUp;
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
            var result = await _dailyFollowUp.gridRegisterDailyFollowUp();
            return result;
        }

        [HttpGet]
        [Route("gridVerifyDailyFollowUp")]
        public async Task<List<VProyectosXcontrato>> gridVerifyDailyFollowUp()
        {
            var result = await _dailyFollowUp.gridVerifyDailyFollowUp();
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
                respuesta = await _dailyFollowUp.CreateEditDailyFollowUp( pSeguimientoDiario );
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
                respuesta = await _dailyFollowUp.DeleteDailyFollowUp( pId, usuario );
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
                respuesta = await _dailyFollowUp.SendToSupervisionSupport( pId, usuario );
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
        public async Task< SeguimientoDiario > GetDailyFollowUpById([FromQuery] int pId )
        {
            var result = await _dailyFollowUp.GetDailyFollowUpById( pId );
            return result;
        }

        [HttpGet]
        [Route("GetDatesAvailableByContratacioProyectoId")]
        public async Task<List<string>> GetDatesAvailableByContratacioProyectoId([FromQuery] int pId )
        {
            var result = await _dailyFollowUp.GetDatesAvailableByContratacioProyectoId( pId );
            return result;
        }

        [HttpGet]
        [Route("GetDailyFollowUpByContratacionProyectoId")]
        public async Task<List<SeguimientoDiario>> GetDailyFollowUpByContratacionProyectoId([FromQuery] int pId)
        {
            var result = await _dailyFollowUp.GetDailyFollowUpByContratacionProyectoId( pId);
            return result;
        }
 
    }
}