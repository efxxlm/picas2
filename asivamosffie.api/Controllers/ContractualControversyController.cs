using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractualControversyController : ControllerBase
    {
        public readonly IContractualControversy _contractualControversy;

        public ContractualControversyController(IContractualControversy contractualControversy)
        {
            _contractualControversy = contractualControversy;
        }

        [HttpPost]
        [Route("CreateEditarControversiaTAI")]
        public async Task<IActionResult> CreateEditarControversiaTAI(ControversiaContractual controversiaContractual)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                //controversiaContractual.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _contractualControversy.CreateEditarControversiaTAI(controversiaContractual);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("CreateEditNuevaActualizacionTramite")]
        
        public async Task<IActionResult> CreateEditNuevaActualizacionTramite(ControversiaActuacion controversiaActuacion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                //controversiaContractual.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _contractualControversy.CreateEditNuevaActualizacionTramite(controversiaActuacion);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpGet]
        [Route("GetListGrillaTipoSolicitudControversiaContractual")]
        public async Task<ActionResult<List<GrillaTipoSolicitudControversiaContractual>>> GetListGrillaTipoSolicitudControversiaContractual()
        {
            try
            {
                return await _contractualControversy.ListGrillaTipoSolicitudControversiaContractual();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
