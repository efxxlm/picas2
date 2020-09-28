using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommitteeSessionFiduciarioController : ControllerBase
    {
        private readonly ICommitteeSessionFiduciarioService _committeeSessionFiduciarioService;
        private readonly IOptions<AppSettings> _settings;
        private readonly IConverter _converter;

        public CommitteeSessionFiduciarioController(IOptions<AppSettings> settings, IConverter converter, ICommitteeSessionFiduciarioService committeeSessionFiduciarioService)
        {
            _committeeSessionFiduciarioService = committeeSessionFiduciarioService;
            _settings = settings;
            _converter = converter;

        }


        #region "ORDEN DEL DIA";

        [Route("GetCommitteeSessionFiduciario")]
         public async Task<IActionResult> GetCommitteeSessionFiduciario()
         {
             try
             {
                 var result = await _committeeSessionFiduciarioService.GetCommitteeSessionFiduciario();
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Route("CreateEditComiteTecnicoAndSesionComiteTemaAndSesionComiteSolicitud")]
        [HttpPost]
        public async Task<IActionResult> CreateEditComiteTecnicoAndSesionComiteTemaAndSesionComiteSolicitud([FromBody]  ComiteTecnico pComiteTecnico)
        {
            Respuesta respuesta = new Respuesta();
            try
            {

                pComiteTecnico.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _committeeSessionFiduciarioService.CreateEditComiteTecnicoAndSesionComiteTemaAndSesionComiteSolicitud( pComiteTecnico );
                return Ok(respuesta);
                
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

       
        [Route("GetCommitteeSession")]
        public async Task<IActionResult> GetCommitteeSession()
        {
            try
            {
                var result = await _committeeSessionFiduciarioService.GetCommitteeSession();
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [Route("GetRequestCommitteeSessionById")]
        public async Task<IActionResult> GetRequestCommitteeSessionById(int comiteTecnicoId)
        {
            try
            {
                var result = await _committeeSessionFiduciarioService.GetRequestCommitteeSessionById(comiteTecnicoId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
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
                respuesta = await _committeeSessionFiduciarioService.ConvocarComiteTecnico(pComiteTecnico, _settings.Value.Dominio, _settings.Value.DominioFront, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }
        

        [Route("GetComiteTecnicoByComiteTecnicoId")]
        public async Task<ComiteTecnico> GetComiteTecnicoByComiteTecnicoId([FromQuery] int pComiteTecnicoId)
        {
            return await _committeeSessionFiduciarioService.GetComiteTecnicoByComiteTecnicoId(pComiteTecnicoId);
        }

        [HttpGet]
        [Route("GetPlantillaByTablaIdRegistroId")]
        public async Task<FileResult> GetPlantillaByTablaIdRegistroId(string pTablaId, int pRegistroId)
        {
            return File(await _committeeSessionFiduciarioService.GetPlantillaByTablaIdRegistroId(pTablaId, pRegistroId), "application/pdf");
        }

        [HttpPost]
        [Route("CreateEditSesionComiteTema")]
        public async Task<IActionResult> CreateEditSesionComiteTema([FromBody] List<SesionComiteTema> ListSesionComiteTemas)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                ListSesionComiteTemas.FirstOrDefault().UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _committeeSessionFiduciarioService.CreateEditSesionComiteTema(ListSesionComiteTemas);
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
                respuesta = await _committeeSessionFiduciarioService.AplazarSesionComite(pComiteTecnico, _settings.Value.Dominio, _settings.Value.DominioFront, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpGet]
        [Route("GetSesionParticipantesByIdComite")]
        public async Task<List<SesionParticipante>> GetSesionParticipantesByIdComite( int pComiteId )
        {
            return await _committeeSessionFiduciarioService.GetSesionParticipantesByIdComite( pComiteId );
        }

        [HttpDelete]
        [Route("DeleteSesionInvitado")]
        public async Task<IActionResult> DeleteSesionInvitado([FromQuery] int pSesionInvitadoId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _committeeSessionFiduciarioService.DeleteSesionInvitado(pSesionInvitadoId, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("CreateEditSesionInvitadoAndParticipante")]
        public async Task<IActionResult> CreateEditSesionInvitadoAndParticipante([FromBody] ComiteTecnico pComiteTecnico)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pComiteTecnico.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _committeeSessionFiduciarioService.CreateEditSesionInvitadoAndParticipante(pComiteTecnico);
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
                respuesta = await _committeeSessionFiduciarioService.GetNoRequiereVotacionSesionComiteSolicitud(pSesionComiteSolicitud);
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
                respuesta = await _committeeSessionFiduciarioService.NoRequiereVotacionSesionComiteTema(idSesionComiteTema, pRequiereVotacion, HttpContext.User.FindFirst("User").Value);
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
                respuesta = await _committeeSessionFiduciarioService.CreateEditSesionSolicitudVoto(pSesionComiteSolicitud);
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
                respuesta = await _committeeSessionFiduciarioService.CambiarEstadoComiteTecnico(pComiteTecnico);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpGet]
        [Route("GetPlantillaActaIdComite")]
        public async Task<FileResult> GetPlantillaActaIdComite(int IdComite)
        {
            return File(await _committeeSessionFiduciarioService.GetPlantillaActaIdComite(IdComite), "application/pdf");
        }

        [HttpPost]
        [Route("CreateEditActasSesionSolicitudCompromiso")]
        public async Task<IActionResult> CreateEditActasSesionSolicitudCompromiso([FromBody] SesionComiteSolicitud pSesionComiteSolicitud)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pSesionComiteSolicitud.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _committeeSessionFiduciarioService.CreateEditActasSesionSolicitudCompromiso(pSesionComiteSolicitud);
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
                respuesta = await _committeeSessionFiduciarioService.CreateEditTemasCompromiso(pSesionComiteTema);
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
                respuesta = await _committeeSessionFiduciarioService.CreateEditSesionTemaVoto(pSesionComiteTema);
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
                respuesta = await _committeeSessionFiduciarioService.VerificarTemasCompromisos(pComiteTecnico);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpGet]
        [Route("GetCompromisosByComiteTecnicoId")]
        public async Task<ComiteTecnico> GetCompromisosByComiteTecnicoId([FromQuery] int ComiteTecnicoId)
        {
            return await _committeeSessionFiduciarioService.GetCompromisosByComiteTecnicoId(ComiteTecnicoId);
        }
        // [Route("GetCommitteeSessionByComiteTecnicoId")]
        // public async Task<IActionResult> GetCommitteeSessionByComiteTecnicoId(int comiteTecnicoId)
        // {
        //     try
        //     {
        //         var result = await _committeeSessionFiduciarioService.GetCommitteeSessionByComiteTecnicoId(comiteTecnicoId);
        //         return Ok(result);
        //     }
        //     catch (Exception ex)
        //     {

        //         throw ex;
        //     }
        // }

        


        // [Route("CreateOrEditTema")]
        // [HttpPost]
        // public async Task<IActionResult> CreateOrEditTema([FromBody] SesionComiteTema sesionComiteTema, DateTime fechaComite)
        // {
        //     Respuesta respuesta = new Respuesta();
        //     try
        //     {

        //         sesionComiteTema.UsuarioCreacion = "forozco";//HttpContext.User.FindFirst("User").Value;
        //         respuesta = await _committeeSessionFiduciarioService.CreateOrEditTema(sesionComiteTema, fechaComite);
        //         return Ok(respuesta);

        //     }
        //     catch (Exception ex)
        //     {
        //         respuesta.Data = ex.InnerException.ToString();
        //         return BadRequest(respuesta);
        //     }
        // }

        // [Route("CallCommitteeSession")]
        // [HttpPost]
        // public async Task<IActionResult> CallCommitteeSession(int comiteTecnicoId)
        // {
        //     Respuesta respuesta = new Respuesta();
        //     try
        //     {

        //         string user = "forozco"; //HttpContext.User.FindFirst("User").Value;
        //         respuesta = await _committeeSessionFiduciarioService.CallCommitteeSession(comiteTecnicoId, user);
        //         return Ok(respuesta);

        //     }
        //     catch (Exception ex)
        //     {
        //         respuesta.Data = ex.InnerException.ToString();
        //         return BadRequest(respuesta);
        //     }
        // }

        // //DeleteTema(int temaId, string user);
        // [Route("DeleteTema")]
        // [HttpGet]
        // public async Task<bool> DeleteTema(int sesionTemaId)
        // {
        //     string user = "forozco"; //HttpContext.User.FindFirst("User").Value;
        //     var respuesta = await _committeeSessionFiduciarioService.DeleteTema(sesionTemaId, user);
        //     return respuesta;
        // }
        // #endregion


        // #region "SESIONES DE COMITE FIDUCIARIO";

        // [Route("GetConvokeSessionFiduciario")]
        // public async Task<IActionResult> GetConvokeSessionFiduciario(int? estadoComiteCodigo)
        // {
        //     try
        //     {
        //         var result = await _committeeSessionFiduciarioService.GetConvokeSessionFiduciario(estadoComiteCodigo);
        //         return Ok(result);
        //     }
        //     catch (Exception ex)
        //     {

        //         throw ex;
        //     }
        // }

        // [Route("GetListParticipantes")]
        // public async Task<IActionResult> GetListParticipantes()
        // {
        //     try
        //     {
        //         var result = await _committeeSessionFiduciarioService.GetListParticipantes();
        //         return Ok(result);
        //     }
        //     catch (Exception ex)
        //     {

        //         throw ex;
        //     }
        // }


        // [Route("GetValidationRequests")]
        // public async Task<IActionResult> GetValidationRequests(string tipoSolicitudCodigo)
        // {
        //     try
        //     {
        //         var result = await _committeeSessionFiduciarioService.GetValidationRequests(tipoSolicitudCodigo);
        //         return Ok(result);
        //     }
        //     catch (Exception ex)
        //     {

        //         throw ex;
        //     }
        // }


        // [Route("GetCompromisosSolicitud")]
        // public async Task<IActionResult> GetCompromisosSolicitud()
        // {
        //     try
        //     {
        //         var result = await _committeeSessionFiduciarioService.GetCompromisosSolicitud();
        //         return Ok(result);
        //     }
        //     catch (Exception ex)
        //     {

        //         throw ex;
        //     }
        // }

        
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

        // [Route("GetSesionGuesById")]
        // public async Task<IActionResult> GetSesionGuesById(int sesionInvitadoId)
        // {
        //     try
        //     {
        //         var result = await _committeeSessionFiduciarioService.GetSesionGuesById(sesionInvitadoId);
        //         return Ok(result);
        //     }
        //     catch (Exception ex)
        //     {

        //         throw ex;
        //     }
        // }

        // [Route("GetSesionSinActa")]
        // public async Task<IActionResult> GetSesionSinActa()
        // {
        //     try
        //     {
        //         var result = await _committeeSessionFiduciarioService.GetSesionSinActa();
        //         return Ok(result);
        //     }
        //     catch (Exception ex)
        //     {

        //         throw ex;
        //     }
        // }



        // [Route("GetCommitteeSessionTemaById")]
        // [HttpGet]
        // public async Task<IActionResult> GetCommitteeSessionTemaById(int sessionTemaId)
        // {
        //     try
        //     {
        //         var result = await _committeeSessionFiduciarioService.GetCommitteeSessionTemaById(sessionTemaId);
        //         return Ok(result);
        //     }
        //     catch (Exception ex)
        //     {

        //         throw ex;
        //     }
        // }

         

        




        // // Registrar tema compromiso
        // [Route("CreateOrEditSubjects")]
        // [HttpPost]
        // public async Task<IActionResult> CreateOrEditSubjects([FromBody] TemaCompromiso temaCompromiso)
        // {
        //     Respuesta respuesta = new Respuesta();
        //     try
        //     {

        //         temaCompromiso.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
        //         respuesta = await _committeeSessionFiduciarioService.CreateOrEditSubjects(temaCompromiso);
        //         return Ok(respuesta);

        //     }
        //     catch (Exception ex)
        //     {
        //         respuesta.Data = ex.InnerException.ToString();
        //         return BadRequest(respuesta);
        //     }
        // }





        // [Route("CreateOrEditGuest")]
        // [HttpPost]
        // public async Task<IActionResult> CreateOrEditGuest([FromBody] SesionInvitado sesionInvitado)
        // {
        //     Respuesta respuesta = new Respuesta();
        //     try
        //     {

        //         sesionInvitado.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
        //         respuesta = await _committeeSessionFiduciarioService.CreateOrEditGuest(sesionInvitado);
        //         return Ok(respuesta);

        //     }
        //     catch (Exception ex)
        //     {
        //         respuesta.Data = ex.InnerException.ToString();
        //         return BadRequest(respuesta);
        //     }
        // }

        // [Route("CreateOrEditInvitedMembers")]
        // [HttpPost]
        // public async Task<IActionResult> CreateOrEditInvitedMembers([FromBody] SesionParticipante sesionParticipante)
        // {
        //     Respuesta respuesta = new Respuesta();
        //     try
        //     {

        //         sesionParticipante.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
        //         respuesta = await _committeeSessionFiduciarioService.CreateOrEditInvitedMembers(sesionParticipante);
        //         return Ok(respuesta);

        //     }
        //     catch (Exception ex)
        //     {
        //         respuesta.Data = ex.InnerException.ToString();
        //         return BadRequest(respuesta);
        //     }
        // }

        // //Aplazar sesion
        // [Route("SessionPostpone")]
        // [HttpGet]
        // public async Task<bool> SessionPostpone(int ComiteTecnicoId, DateTime newDate)
        // {
        //     try
        //     {

        //         string usuarioModifico = HttpContext.User.FindFirst("User").Value;
        //         return await _committeeSessionFiduciarioService.SessionPostpone(ComiteTecnicoId, newDate, usuarioModifico);

        //     }
        //     catch (Exception)
        //     {
        //         return false;
        //     }
        // }

        // [Route("CreateOrEditVotacionSolicitud")]
        // [HttpPost]
        // public async Task<IActionResult> CreateOrEditVotacionSolicitud([FromBody] List<SesionSolicitudVoto> listSolicitudVoto)
        // {
        //     Respuesta respuesta = new Respuesta();
        //     try
        //     {

        //         listSolicitudVoto.FirstOrDefault().UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
        //         respuesta = await _committeeSessionFiduciarioService.CreateOrEditVotacionSolicitud(listSolicitudVoto);
        //         return Ok(respuesta);

        //     }
        //     catch (Exception ex)
        //     {
        //         respuesta.Data = ex.InnerException.ToString();
        //         return BadRequest(respuesta);
        //     }
        // }
        
        // //Declarar fallida
        // [Route("SessionDeclaredFailed")]
        // [HttpGet]
        // public async Task<bool> SessionDeclaredFailed(int ComiteTecnicoId)
        // {
        //     try
        //     {

        //         string usuarioModifico = HttpContext.User.FindFirst("User").Value;
        //         return await _committeeSessionFiduciarioService.SessionDeclaredFailed(ComiteTecnicoId, usuarioModifico);

        //     }
        //     catch (Exception)
        //     {
        //         return false;
        //     }
        // }


        // #region "Descargas PDF";
        // //Descargar acta
        // [HttpGet]
        // [Route("StartDownloadResumenFichaSolicitud")]
        // public async Task<IActionResult> StartDownloadResumenFichaSolicitud(int sesionTemaId)
        // {
        //     try
        //     {

        //         //var result = await _managementCommitteeReportService.GetHTMLString(actaComite);
        //         var globalSettings = new GlobalSettings
        //         {
        //             ColorMode = ColorMode.Color,
        //             Orientation = Orientation.Portrait,
        //             PaperSize = PaperKind.A4,
        //             Margins = new MarginSettings { Top = 10 },
        //             DocumentTitle = "Acata Comite Tecnico"
        //             //detailValidarDisponibilidadPresupuesal.NumeroSolicitud != null ? detailValidarDisponibilidadPresupuesal.NumeroSolicitud.ToString() : "",
        //         };

        //         var objectSettings = new ObjectSettings
        //         {
        //             PagesCount = true,
        //             HtmlContent = "<html><body><h1>HTML Pendiente Back... </h1></body> </html>", //detailValidarDisponibilidadPresupuesal.htmlContent.ToString(),
        //             WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "pdf-styles.css") },
        //             HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Pagina [page] de [toPage]", Line = false },
        //             //FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Footer" }
        //         };

        //         var pdf = new HtmlToPdfDocument()
        //         {
        //             GlobalSettings = globalSettings,
        //             Objects = { objectSettings },

        //         };

        //         var file = _converter.Convert(pdf);

        //         //return Ok("El documento PDF fue descargado.");
        //         //return File(file, "application/pdf", "DDP_.pdf");
        //         return File(file, "application/pdf", "Ficha Resumen" + ".pdf"); //detailValidarDisponibilidadPresupuesal.NumeroSolicitud.ToString()
        //     }
        //     catch (Exception ex)
        //     {
        //         throw ex;
        //     }


        // }



        //#endregion


    }
}
