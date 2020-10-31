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
    public class RegisterSessionTechnicalCommitteeController : ControllerBase
    {
        public readonly IRegisterSessionTechnicalCommitteeService _registerSessionTechnicalCommitteeService;
        private readonly IOptions<AppSettings> _settings;

        public RegisterSessionTechnicalCommitteeController(IOptions<AppSettings> settings, IRegisterSessionTechnicalCommitteeService registerSessionTechnicalCommitteeService)
        {
            _settings = settings;
            _registerSessionTechnicalCommitteeService = registerSessionTechnicalCommitteeService;
        }

        [HttpGet]
        [Route("GetPlantillaActaIdComite")]
        public async Task<FileResult> GetPlantillaActaIdComite(int IdComite)
        {
            return File(await _registerSessionTechnicalCommitteeService.GetPlantillaActaIdComite(IdComite), "application/pdf");
        }

        [HttpGet]
        [Route("GetProcesoSeleccionMonitoreo")]
        public async Task<ProcesoSeleccionMonitoreo> GetProcesoSeleccionMonitoreo( int pProcesoSeleccionMonitoreoId )
        {
            return await _registerSessionTechnicalCommitteeService.GetProcesoSeleccionMonitoreo( pProcesoSeleccionMonitoreoId );
        }
 
        [HttpGet]
        [Route("ListMonitoreo")]
        public async Task<dynamic> ListMonitoreo()
        {
            return   await _registerSessionTechnicalCommitteeService.ListMonitoreo();
       
        }

        [HttpDelete]
        [Route("DeleteComiteTecnicoByComiteTecnicoId")]
        public async Task<Respuesta> DeleteComiteTecnicoByComiteTecnicoId([FromQuery] int pComiteTecnicoId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _registerSessionTechnicalCommitteeService.DeleteComiteTecnicoByComiteTecnicoId(pComiteTecnicoId, HttpContext.User.FindFirst("User").Value);
                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [Route("EliminarCompromisosSolicitud")]
        public async Task<Respuesta> EliminarCompromisosSolicitud([FromQuery] int pSesionComiteSolicitudId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _registerSessionTechnicalCommitteeService.EliminarCompromisosSolicitud(pSesionComiteSolicitudId, HttpContext.User.FindFirst("User").Value);
                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [Route("EliminarCompromisosTema")]
        public async Task<Respuesta> EliminarCompromisosTema([FromQuery] int pSesionTemaId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _registerSessionTechnicalCommitteeService.EliminarCompromisosTema(pSesionTemaId, HttpContext.User.FindFirst("User").Value);
                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("CreateEditSesionSolicitudObservacionProyecto")]
        public async Task<IActionResult> CreateEditSesionSolicitudObservacionProyecto([FromBody] SesionSolicitudObservacionProyecto pSesionSolicitudObservacionProyecto)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pSesionSolicitudObservacionProyecto.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerSessionTechnicalCommitteeService.CreateEditSesionSolicitudObservacionProyecto(pSesionSolicitudObservacionProyecto);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("CreateEditTemasCompromiso")]
        public async Task<IActionResult> CreateEditTemasCompromiso([FromBody] SesionComiteTema pSesionComiteTema)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pSesionComiteTema.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerSessionTechnicalCommitteeService.CreateEditTemasCompromiso(pSesionComiteTema);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("CrearObservacionProyecto")]
        public async Task<IActionResult> CrearObservacionProyecto([FromBody] ContratacionObservacion pContratacionObservacion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pContratacionObservacion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerSessionTechnicalCommitteeService.CrearObservacionProyecto(pContratacionObservacion);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("CreateEditActasSesionSolicitudCompromiso")]
        public async Task<IActionResult> CreateEditActasSesionSolicitudCompromiso([FromBody] SesionComiteSolicitud pSesionComiteSolicitud)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pSesionComiteSolicitud.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerSessionTechnicalCommitteeService.CreateEditActasSesionSolicitudCompromiso(pSesionComiteSolicitud);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPut]
        [Route("CambiarEstadoActa")]
        public async Task<IActionResult> CambiarEstadoActa([FromQuery] int pSesionComiteSolicitud, string pCodigoEstado)
        {
            Respuesta respuesta = new Respuesta();
            try
            { 
                respuesta = await _registerSessionTechnicalCommitteeService.CambiarEstadoActa(pSesionComiteSolicitud,  pCodigoEstado , HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("AplazarSesionComite")]
        public async Task<IActionResult> AplazarSesionComite([FromBody] ComiteTecnico pComiteTecnico)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pComiteTecnico.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerSessionTechnicalCommitteeService.AplazarSesionComite(pComiteTecnico, _settings.Value.Dominio, _settings.Value.DominioFront, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("NoRequiereVotacionSesionComiteTema")]
        public async Task<IActionResult> NoRequiereVotacionSesionComiteTema([FromQuery] int idSesionComiteTema, bool pRequiereVotacion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _registerSessionTechnicalCommitteeService.NoRequiereVotacionSesionComiteTema(idSesionComiteTema, pRequiereVotacion, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("CreateEditSesionTemaVoto")]
        public async Task<IActionResult> CreateEditSesionTemaVoto([FromBody] SesionComiteTema pSesionComiteTema)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pSesionComiteTema.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerSessionTechnicalCommitteeService.CreateEditSesionTemaVoto(pSesionComiteTema);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("NoRequiereVotacionSesionComiteSolicitud")]
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
        [Route("ConvocarComiteTecnico")]
        public async Task<IActionResult> ConvocarComiteTecnico([FromBody] ComiteTecnico pComiteTecnico)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pComiteTecnico.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerSessionTechnicalCommitteeService.ConvocarComiteTecnico(pComiteTecnico, _settings.Value.Dominio, _settings.Value.DominioFront, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("EnviarComiteParaAprobacion")]
        public async Task<IActionResult> EnviarComiteParaAprobacion([FromBody] ComiteTecnico pComiteTecnico)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pComiteTecnico.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerSessionTechnicalCommitteeService.EnviarComiteParaAprobacion(pComiteTecnico, _settings.Value.Dominio, _settings.Value.DominioFront, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);
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
        [Route("VerificarTemasCompromisos")]
        public async Task<IActionResult> CreateEditSesionComiteTema([FromBody] ComiteTecnico pComiteTecnico)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pComiteTecnico.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerSessionTechnicalCommitteeService.VerificarTemasCompromisos(pComiteTecnico);
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
        [Route("GetSesionSolicitudObservacionProyecto")]
        public async Task<List<SesionSolicitudObservacionProyecto>> GetSesionSolicitudObservacionProyecto([FromQuery] int pSesionComiteSolicitudId, int pContratacionProyectoId)
        {
            return await _registerSessionTechnicalCommitteeService.GetSesionSolicitudObservacionProyecto(pSesionComiteSolicitudId, pContratacionProyectoId);
        }

        [HttpGet]
        [Route("GetCompromisosByComiteTecnicoId")]
        public async Task<ComiteTecnico> GetCompromisosByComiteTecnicoId([FromQuery] int ComiteTecnicoId)
        {
            return await _registerSessionTechnicalCommitteeService.GetCompromisosByComiteTecnicoId(ComiteTecnicoId);
        }
         
        [Route("GetComiteTecnicoByComiteTecnicoId")]
        public async Task<ComiteTecnico> GetComiteTecnicoByComiteTecnicoId([FromQuery] int pComiteTecnicoId)
        {
            return await _registerSessionTechnicalCommitteeService.GetComiteTecnicoByComiteTecnicoId(pComiteTecnicoId);
        }

        [HttpPost]
        [Route("CreateEditSesionInvitadoAndParticipante")]
        public async Task<IActionResult> CreateEditSesionInvitadoAndParticipante([FromBody] ComiteTecnico pComiteTecnico)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pComiteTecnico.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerSessionTechnicalCommitteeService.CreateEditSesionInvitadoAndParticipante(pComiteTecnico);
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
        [Route("GetSesionParticipantesByIdComite")]
        public async Task<List<SesionParticipante>> GetSesionParticipantesByIdComite( int pComiteId )
        {
            return await _registerSessionTechnicalCommitteeService.GetSesionParticipantesByIdComite( pComiteId );
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

        [HttpGet]
        [Route("GetCometariosDelActa")]
        public async Task<List<SesionComentario>> GetCometariosDelActa(int pComietTecnicoId)
        {
            return await _registerSessionTechnicalCommitteeService.GetCometariosDelActa( pComietTecnicoId );
        }

        

    }
}
