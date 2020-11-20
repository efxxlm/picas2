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

 
    }
}