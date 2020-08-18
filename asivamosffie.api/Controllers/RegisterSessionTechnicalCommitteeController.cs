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
    public class RegisterSessionTechnicalCommitteeController : ControllerBase
    {
        public readonly IRegisterSessionTechnicalCommitteeService _registerSessionTechnicalCommitteeService;

        public RegisterSessionTechnicalCommitteeController(IRegisterSessionTechnicalCommitteeService registerSessionTechnicalCommitteeService) {

            _registerSessionTechnicalCommitteeService = registerSessionTechnicalCommitteeService; 
        }


     

        //[HttpGet]
        //[Route("GetListSesionComiteTemaByIdSesion")]
        //public async Task<List<dynamic>> GetListSesionComiteTemaByIdSesion()
        //{
        //    return await _registerSessionTechnicalCommitteeService.GetListSesionComiteTemaByIdSesion();
        //}

        [HttpGet]
        [Route("GetListSolicitudesContractuales")] 
        public async Task<List<dynamic>> GetListSolicitudesContractuales() {
            return await _registerSessionTechnicalCommitteeService.GetListSolicitudesContractuales(); 
        }

        [HttpPost]
        [Route("SaveEditSesionComiteTema")]
        public async Task<IActionResult> SaveEditSesionComiteTema(List<SesionComiteTema> pListSesionComiteTema ,DateTime pFechaProximoComite)
        {
            Respuesta respuesta = new Respuesta();
            try
            { 
                pListSesionComiteTema.FirstOrDefault().UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerSessionTechnicalCommitteeService.SaveEditSesionComiteTema(pListSesionComiteTema , pFechaProximoComite);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }  
        }


        [HttpGet]
        [Route("GetComiteGrilla")]
        public async Task<List<ComiteGrilla>> GetComiteGrilla()
        {
            return await _registerSessionTechnicalCommitteeService.GetComiteGrilla();
        }
         
        [HttpGet]
        [Route("GetSesionBySesionId")]
        public async Task<Sesion> GetSesionBySesionId(int pSesionId)
        {
            return await _registerSessionTechnicalCommitteeService.GetSesionBySesionId(pSesionId);
        }

        [HttpPut]
        [Route("DeleteSesionComiteTema")]
        public async Task<IActionResult> DeleteSesionComiteTema(int pSesionComiteTemaId)
        {
            Respuesta respuesta = new Respuesta();
            try
            { 
                respuesta = await _registerSessionTechnicalCommitteeService.EliminarSesionComiteTema(pSesionComiteTemaId, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            { 
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            } 
        }


        [HttpPut]
        [Route("CambiarEstadoComite")]
        public async Task<IActionResult> CambiarEstadoComite(Sesion pSesion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pSesion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerSessionTechnicalCommitteeService.CambiarEstadoComite(pSesion);
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
