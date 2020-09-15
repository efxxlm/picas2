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
    public class RegisterContractsAndContractualModifications : Controller
    {
        public readonly IRegisterContractsAndContractualModificationsService _registerContractsService;

        public RegisterContractsAndContractualModifications(IRegisterContractsAndContractualModificationsService registerContractsService)
        {
            _registerContractsService = registerContractsService;
        }
    
        [HttpGet]
        [Route("GetListSesionComiteSolicitud")]
        public async Task<List<SesionComiteSolicitud>> GetListSesionComiteSolicitud()
        { 
            return await _registerContractsService.GetListSesionComiteSolicitud(); 
        }
         
        [HttpGet]
        [Route("GetContratacionByContratacionId")]
        public async Task<Contratacion> GetContratacionByContratacionId([FromQuery] int ContratacionId)
        {
            return await _registerContractsService.GetContratacionByContratacionId(ContratacionId);
        }
     
        [Route("RegistrarTramiteContrato")]
        [HttpPost]
        public async Task<IActionResult> RegistrarTramiteContrato([FromBody] Contrato pContrato)
        {
            Respuesta respuesta = new Respuesta();
            try
            { 
                string pUsuarioModifico = HttpContext.User.FindFirst("User").Value;
                pContrato.UsuarioCreacion = pUsuarioModifico;
                respuesta = await _registerContractsService.RegistrarTramiteContrato(pContrato);
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
