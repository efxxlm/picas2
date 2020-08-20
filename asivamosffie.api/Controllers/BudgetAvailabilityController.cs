using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asivamosffie.services.Interfaces;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;
using Microsoft.Extensions.Options;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetAvailabilityController : ControllerBase
    {
        public readonly ICommonService common;
        private readonly IOptions<AppSettings> _settings;
        public BudgetAvailabilityController(ICommonService prmCommon, IOptions<AppSettings> settings)
        {
            common = prmCommon;
            _settings = settings;
        }

        [HttpGet]
        [Route("GetMenuByRol")]
        public async Task<ActionResult<List<MenuPerfil>>> GetMenuByRol()
        { 
            int pUserId = Int32.Parse(HttpContext.User.FindFirst("UserId").Value);
            var result = await common.GetMenuByRol(pUserId);
            return result;
        }
        

        [Route("CreateorUpdateCofinancing")]
        [HttpPost]
        public async Task<IActionResult> CreateCofinancing([FromBody] Cofinanciacion pCofinanciacion)
        {
            try
            {
                HttpContext.Connection.RemoteIpAddress.ToString();
                pCofinanciacion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                Task<Respuesta> result = _Cofinancing.CreateorUpdateCofinancing(pCofinanciacion);
                object respuesta = await result;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}