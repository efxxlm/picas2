using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.api.Models;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DerivativeActionController : ControllerBase
    {

        public readonly IDerivativeActionService _derivativeActionService;

        public DerivativeActionController(IDerivativeActionService seguimientoActuacionDerivada)
        {
            _derivativeActionService = seguimientoActuacionDerivada;
        }

        [HttpPost]
        [Route("CreateEditarSeguimientoActuacionDerivada")]
        //public async Task<Respuesta> CreateEditarSeguimientoActuacionDerivada(SeguimientoActuacionDerivada seguimientoActuacionDerivada)
        public async Task<IActionResult> CreateEditarSeguimientoActuacionDerivada(SeguimientoActuacionDerivada seguimientoActuacionDerivada)
        {

            Respuesta respuesta = new Respuesta();
            try
            {
                
                    seguimientoActuacionDerivada.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
               
                respuesta = await _derivativeActionService.CreateEditarSeguimientoActuacionDerivada(seguimientoActuacionDerivada);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }



    }
}
