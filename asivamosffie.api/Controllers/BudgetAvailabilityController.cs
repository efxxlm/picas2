using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asivamosffie.services.Interfaces;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;
using Microsoft.Extensions.Options;
=======
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using asivamosffie.model.APIModels;
>>>>>>> 3.3.2_Validar-disponibilidad-de-presupuesto-para-ejecución-de-proyecto

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetAvailabilityController : ControllerBase
    {
<<<<<<< HEAD
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
=======

        private readonly IBudgetAvailabilityService _budgetAvailabilityService;

        public BudgetAvailabilityController(IBudgetAvailabilityService budgetAvailabilityService)
        {
            _budgetAvailabilityService = budgetAvailabilityService; 
        }

        [Route("ListAdministrativeProject")]
        [HttpGet]
        public async Task<List<DisponibilidadPresupuestalGrilla>> ListAdministrativeProjects()
        {
            // string pUsuarioModifico = HttpContext.User.FindFirst("User").Value;
            var respuesta = await _budgetAvailabilityService.GetListDisponibilidadPresupuestal();
            return respuesta; 
        }

        [Route("GetFuenteFinanciacionByIdAportanteId")]
        [HttpGet]
        public async Task<FuenteFinanciacion> GetFuenteFinanciacionByIdAportanteId(int pAportanteId)
        {
            // string pUsuarioModifico = HttpContext.User.FindFirst("User").Value;
            var respuesta = await _budgetAvailabilityService.GetFuenteFinanciacionByIdAportanteId(pAportanteId);
            return respuesta;
        }

        [Route("GetListDisponibilidadPresupuestalByCodigoEstadoSolicitud")]
        [HttpGet]
        public async Task<List<DisponibilidadPresupuestalGrilla>> GetListDisponibilidadPresupuestalByCodigoEstadoSolicitud(string pCodigoEstadoSolicitud)
        { 
            var respuesta = await _budgetAvailabilityService.GetListDisponibilidadPresupuestalByCodigoEstadoSolicitud(pCodigoEstadoSolicitud);
            return respuesta;
        }


 
    }
}
>>>>>>> 3.3.2_Validar-disponibilidad-de-presupuesto-para-ejecución-de-proyecto
