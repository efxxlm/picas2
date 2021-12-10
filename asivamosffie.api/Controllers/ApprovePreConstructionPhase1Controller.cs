using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApprovePreConstructionPhase1Controller : Controller
    {
        public readonly IApprovePreConstructionPhase1Service _approvePreConstruction;

        public ApprovePreConstructionPhase1Controller(IApprovePreConstructionPhase1Service approvePreConstructionPhase1Service)
        {
            _approvePreConstruction = approvePreConstructionPhase1Service;
        }

        [Route("GetListContratacion")]
        [HttpGet]
        public async Task<List<dynamic>> GetListContratacion()
        {
            var result = await _approvePreConstruction.GetListContratacion(Int32.Parse(HttpContext.User.FindFirst("UserId").Value));
            return result;
        }

        [HttpPost]
        [Route("CrearContratoPerfilObservacion")]
        public async Task<IActionResult> CrearContratoPerfilObservacion(ContratoPerfilObservacion pContratoPerfilObservacion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pContratoPerfilObservacion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _approvePreConstruction.CrearContratoPerfilObservacion(pContratoPerfilObservacion);

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
