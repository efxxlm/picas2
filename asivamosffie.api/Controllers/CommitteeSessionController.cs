using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommitteeSessionController : ControllerBase
    {
        private readonly ICommitteeSessionService _committeeSessionService;
        private readonly IOptions<AppSettings> _settings;

        public CommitteeSessionController(IOptions<AppSettings> settings, ICommitteeSessionService committeeSessionService)
        {
            _committeeSessionService = committeeSessionService;
            _settings = settings;

        }

        [Route("GetSesion")]
        public async Task<IActionResult> GetSesion(int? sessionId)
        {   
            try
            {
                var result = await _committeeSessionService.GetCommitteeSession(sessionId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Route("GetCommitteeSessionTemaById")]
        [HttpGet]
        public async Task<IActionResult> GetCommitteeSessionTemaById(int sessionTemaId)
        {
            try
            {
                var result = await _committeeSessionService.GetCommitteeSessionTemaById(sessionTemaId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Route("GetCommitteeSessionFiduciario")]
        public async Task<IActionResult> GetCommitteeSessionFiduciario()
        {
            try
            {
                var result = await _committeeSessionService.GetCommitteeSessionFiduciario();
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Route("CreateOrEditCommitteeSession")]
        [HttpPost]
        public async Task<IActionResult> CreateOrEditCommitteeSession([FromBody]  SesionComiteTema sesionComiteTema)
        {
            Respuesta respuesta = new Respuesta();
            try
            {

                sesionComiteTema.UsuarioCreacion = "jsorozcof";//HttpContext.User.FindFirst("User").Value;
                respuesta = await _committeeSessionService.CreateOrEditCommitteeSession(sesionComiteTema);
                return Ok(respuesta);
                
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }




        [Route("DeleteTema")]
        [HttpGet]
        public async Task<bool> DeleteTema(int temaId)
        {
            var respuesta = await _committeeSessionService.DeleteTema(temaId);
            return respuesta;
        }
    }
}
