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
    public class CommitteeSessionFiduciarioController : ControllerBase
    {
        private readonly ICommitteeSessionFiduciarioService _committeeSessionService;
        private readonly IOptions<AppSettings> _settings;

        public CommitteeSessionFiduciarioController(IOptions<AppSettings> settings, ICommitteeSessionFiduciarioService committeeSessionService)
        {
            _committeeSessionService = committeeSessionService;
            _settings = settings;

        }


        #region "ORDEN DEL DIA";

        [Route("GetRequestCommitteeSessionById")]
        public async Task<IActionResult> GetRequestCommitteeSessionById(int comiteTecnicoId)
        {
            try
            {
                var result = await _committeeSessionService.GetRequestCommitteeSessionById(comiteTecnicoId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        [Route("GetCommitteeSessionByComiteTecnicoId")]
        public async Task<IActionResult> GetCommitteeSessionByComiteTecnicoId(int comiteTecnicoId)
        {
            try
            {
                var result = await _committeeSessionService.GetCommitteeSessionByComiteTecnicoId(comiteTecnicoId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Route("GetCommitteeSession")]
        public async Task<IActionResult> GetCommitteeSession()
        {
            try
            {
                var result = await _committeeSessionService.GetCommitteeSession();
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [Route("CreateOrEditTema")]
        [HttpPost]
        public async Task<IActionResult> CreateOrEditTema([FromBody] SesionComiteTema sesionComiteTema, DateTime fechaComite)
        {
            Respuesta respuesta = new Respuesta();
            try
            {

                sesionComiteTema.UsuarioCreacion = "forozco";//HttpContext.User.FindFirst("User").Value;
                respuesta = await _committeeSessionService.CreateOrEditTema(sesionComiteTema, fechaComite);
                return Ok(respuesta);

            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("CallCommitteeSession")]
        [HttpPost]
        public async Task<IActionResult> CallCommitteeSession(int comiteTecnicoId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {

                string user = "forozco"; //HttpContext.User.FindFirst("User").Value;
                respuesta = await _committeeSessionService.CallCommitteeSession(comiteTecnicoId, user);
                return Ok(respuesta);

            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        #endregion











        //[Route("GetSesion")]
        //public async Task<IActionResult> GetSesion(int? sessionId)
        //{   
        //    try
        //    {
        //        var result = await _committeeSessionService.GetCommitteeSession(sessionId);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        [Route("GetSesionGuesById")]
        public async Task<IActionResult> GetSesionGuesById(int sesionInvitadoId)
        {
            try
            {
                var result = await _committeeSessionService.GetSesionGuesById(sesionInvitadoId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Route("GetSesionSinActa")]
        public async Task<IActionResult> GetSesionSinActa()
        {
            try
            {
                var result = await _committeeSessionService.GetSesionSinActa();
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

                sesionComiteTema.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _committeeSessionService.CreateOrEditCommitteeSession(sesionComiteTema);
                return Ok(respuesta);
                
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }




        // Registrar tema compromiso
        [Route("CreateOrEditSubjects")]
        [HttpPost]
        public async Task<IActionResult> CreateOrEditSubjects([FromBody] TemaCompromiso temaCompromiso)
        {
            Respuesta respuesta = new Respuesta();
            try
            {

                temaCompromiso.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _committeeSessionService.CreateOrEditSubjects(temaCompromiso);
                return Ok(respuesta);

            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }





        [Route("CreateOrEditGuest")]
        [HttpPost]
        public async Task<IActionResult> CreateOrEditGuest([FromBody] SesionInvitado sesionInvitado)
        {
            Respuesta respuesta = new Respuesta();
            try
            {

                sesionInvitado.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _committeeSessionService.CreateOrEditGuest(sesionInvitado);
                return Ok(respuesta);

            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }


        //Aplazar sesion
        [Route("SessionPostpone")]
        [HttpGet]
        public async Task<bool> SessionPostpone(int ComiteTecnicoId, DateTime newDate)
        {
            try
            {

                string usuarioModifico = HttpContext.User.FindFirst("User").Value;
                return await _committeeSessionService.SessionPostpone(ComiteTecnicoId, newDate, usuarioModifico);

            }
            catch (Exception)
            {
                return false;
            }
        }


        //Declarar fallida
        [Route("SessionDeclaredFailed")]
        [HttpGet]
        public async Task<bool> SessionDeclaredFailed(int ComiteTecnicoId)
        {
            try
            {

                string usuarioModifico = HttpContext.User.FindFirst("User").Value;
                return await _committeeSessionService.SessionDeclaredFailed(ComiteTecnicoId, usuarioModifico);

            }
            catch (Exception)
            {
                return false;
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
