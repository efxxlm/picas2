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

        public RegisterSessionTechnicalCommitteeController(IRegisterSessionTechnicalCommitteeService registerSessionTechnicalCommitteeService)
        {

            _registerSessionTechnicalCommitteeService = registerSessionTechnicalCommitteeService;
        }

        [HttpPost]
        [Route("GetNoRequiereVotacionSesionComiteSolicitud")]
        public async Task<IActionResult> GetNoRequiereVotacionSesionComiteSolicitud([FromBody] SesionComiteSolicitud pSesionComiteSolicitud)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pSesionComiteSolicitud.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerSessionTechnicalCommitteeService.GetNoRequiereVotacionSesionComiteSolicitud(pSesionComiteSolicitud);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }



        [HttpPost]
        [Route("CreateEditSesionSolicitudVoto")]
        public async Task<IActionResult> CreateEditSesionSolicitudVoto([FromBody] SesionComiteSolicitud pSesionComiteSolicitud)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pSesionComiteSolicitud.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerSessionTechnicalCommitteeService.CreateEditSesionSolicitudVoto(pSesionComiteSolicitud);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }



        [HttpPost]
        [Route("CreateEditSesionComiteTema")]
        public async Task<IActionResult> CreateEditSesionComiteTema([FromBody] List<SesionComiteTema> ListSesionComiteTemas)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                ListSesionComiteTemas.FirstOrDefault().UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerSessionTechnicalCommitteeService.CreateEditSesionComiteTema(ListSesionComiteTemas);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }



        [HttpDelete]
        [Route("DeleteSesionInvitado")]
        public async Task<IActionResult> DeleteSesionInvitado([FromQuery] int pSesionInvitadoId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _registerSessionTechnicalCommitteeService.DeleteSesionInvitado(pSesionInvitadoId, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("CreateEditComiteTecnicoAndSesionComiteTemaAndSesionComiteSolicitud")]
        public async Task<IActionResult> CreateEditComiteTecnicoAndSesionComiteTemaAndSesionComiteSolicitud([FromBody] ComiteTecnico pComiteTecnico)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pComiteTecnico.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerSessionTechnicalCommitteeService.CreateEditComiteTecnicoAndSesionComiteTemaAndSesionComiteSolicitud(pComiteTecnico);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPut]
        [Route("CambiarEstadoComiteTecnico")]
        public async Task<IActionResult> CambiarEstadoComiteTecnico([FromBody] ComiteTecnico pComiteTecnico)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pComiteTecnico.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerSessionTechnicalCommitteeService.CambiarEstadoComiteTecnico(pComiteTecnico);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpGet]
        [Route("GetListSesionComiteSolicitudByFechaOrdenDelDia")]
        public async Task<List<dynamic>> GetListSesionComiteSolicitudByFechaOrdenDelDia([FromQuery] string pFechaComite)
        {
            return await _registerSessionTechnicalCommitteeService.GetListSesionComiteSolicitudByFechaOrdenDelDia(DateTime.Parse(pFechaComite));
        }

        [HttpGet]
        [Route("GetComiteTecnicoByComiteTecnicoId")]
        public async Task<ComiteTecnico> GetComiteTecnicoByComiteTecnicoId([FromQuery] int pComiteTecnicoId)
        {
            return await _registerSessionTechnicalCommitteeService.GetComiteTecnicoByComiteTecnicoId(pComiteTecnicoId);
        }

        [HttpPost]
        [Route("CreateSesionInvitadoAndParticipante")]
        public async Task<IActionResult> CreateSesionInvitadoAndParticipante([FromBody] ComiteTecnico pComiteTecnico)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pComiteTecnico.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerSessionTechnicalCommitteeService.CreateSesionInvitadoAndParticipante(pComiteTecnico);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpDelete]
        [Route("DeleteSesionComiteTema")]
        public async Task<IActionResult> DeleteSesionComiteTema([FromQuery] int pSesionComiteTemaId)
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

        [HttpGet]
        [Route("GetPlantillaByTablaIdRegistroId")]
        public async Task<FileResult> GetPlantillaByTablaIdRegistroId(string pTablaId, int pRegistroId)
        {
            return File(await _registerSessionTechnicalCommitteeService.GetPlantillaByTablaIdRegistroId(pTablaId, pRegistroId), "application/pdf");
        }

        [HttpGet]
        [Route("GetListComiteGrilla")]
        public async Task<List<ComiteGrilla>> GetListComiteGrilla()
        {
            return await _registerSessionTechnicalCommitteeService.GetListComiteGrilla();
        }

        [HttpGet]
        [Route("GetListSesionComiteTemaByComiteTecnicoId")]
        public async Task<List<dynamic>> GetListSesionComiteTemaByComiteTecnicoId([FromQuery] int pComiteTecnicoId)
        {
            return await _registerSessionTechnicalCommitteeService.GetListSesionComiteTemaByComiteTecnicoId(pComiteTecnicoId);
        }

    }
}
