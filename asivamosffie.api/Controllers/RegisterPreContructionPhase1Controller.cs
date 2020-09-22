using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using asivamosffie.model.APIModels;
using System.IO;
using Microsoft.Extensions.Options;
using System.Reflection;
using Newtonsoft.Json;


namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterPreContructionPhase1Controller : Controller
    {
        public readonly IRegisterPreContructionPhase1Service _registerPreContructionPhase1Service;


        public RegisterPreContructionPhase1Controller(IRegisterPreContructionPhase1Service registerPreContructionPhase1Service)
        {
            _registerPreContructionPhase1Service = registerPreContructionPhase1Service;
        }


        [HttpGet]
        [Route("GetListContratacion")]
        public async Task<List<dynamic>> GetListContratacion()
        {
            return await _registerPreContructionPhase1Service.GetListContratacion();
        }

        [HttpGet]
        [Route("GetContratacionByContratoId")]
        public async Task<Contrato> GetContratacionByContratoId(int pContratoId)
        {
            return await _registerPreContructionPhase1Service.GetContratacionByContratoId(pContratoId);
        }
         
        [Route("CreateEditContratoPerfil")]
        [HttpPost]
        public async Task<IActionResult> CreateEditContratoPerfil(Contrato pContrato)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pContrato.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerPreContructionPhase1Service.CreateEditContratoPerfil(pContrato);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("DeleteContratoPerfil")]
        [HttpDelete]
        public async Task<IActionResult> DeleteContratoPerfil(int ContratoPerfilId)
        {
            Respuesta respuesta = new Respuesta();
            try
            { 
                respuesta = await _registerPreContructionPhase1Service.DeleteContratoPerfil(ContratoPerfilId,HttpContext.User.FindFirst("User").Value);
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
