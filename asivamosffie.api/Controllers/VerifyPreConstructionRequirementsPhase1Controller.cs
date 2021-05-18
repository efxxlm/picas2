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
    public class VerifyPreConstructionRequirementsPhase1Controller : ControllerBase
    {
        public readonly IVerifyPreConstructionRequirementsPhase1Service _verifyPreConstruction;

        public VerifyPreConstructionRequirementsPhase1Controller(IVerifyPreConstructionRequirementsPhase1Service verifyPreConstructionRequirementsPhase1Service)
        {
            _verifyPreConstruction = verifyPreConstructionRequirementsPhase1Service;
        }

        [HttpGet]
        [Route("GetListContratacionInterventoria")]
        public async Task<List<VRegistrarFase1>> GetListContratacionInterventoria2()
        {
            return await _verifyPreConstruction.GetListContratacionInterventoria2(Int32.Parse(HttpContext.User.FindFirst("UserId").Value));
        }
        [HttpGet]
        [Route("GetListContratacionInterventoriaOld")]
        public async Task<List<dynamic>> GetListContratacionInterventoria()
        {
            return await _verifyPreConstruction.GetListContratacionInterventoria();
        }
        [HttpGet]
        [Route("GetListContratacion")]
        public async Task<List<dynamic>> GetListContratacion()
        {
            return await _verifyPreConstruction.GetListContratacion(Int32.Parse(HttpContext.User.FindFirst("UserId").Value));
        }
        [HttpGet]
        [Route("GetContratoByContratoId")]
        public async Task<Contrato> GetContratoByContratoId(int pContratoId)
        {
            return await _verifyPreConstruction.GetContratoByContratoId(pContratoId);
        }

        [HttpPost]
        [Route("CrearContratoPerfilObservacion")]
        public async Task<IActionResult> CrearContratoPerfilObservacion(ContratoPerfilObservacion pContratoPerfilObservacion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pContratoPerfilObservacion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _verifyPreConstruction.CrearContratoPerfilObservacion(pContratoPerfilObservacion);

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
