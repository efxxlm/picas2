using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asivamosffie.services.Interfaces;
using asivamosffie.model.Models;
using Microsoft.Extensions.Options;
using asivamosffie.api.Responses;
using System.Security.Claims;
using asivamosffie.model.APIModels;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

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
            return await _verifyPreConstruction.GetListContratacionInterventoria2();
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
            return await _verifyPreConstruction.GetListContratacion();
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
