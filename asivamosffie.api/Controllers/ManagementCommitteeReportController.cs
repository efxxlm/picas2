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
    public class ManagementCommitteeReportController : ControllerBase
    {
        private readonly IManagementCommitteeReportService _managementCommitteeReportService;
        private readonly IOptions<AppSettings> _settings;
        private readonly IConverter _converter;


        public ManagementCommitteeReportController(IOptions<AppSettings> settings, IConverter converter, IManagementCommitteeReportService managementCommitteeReportService)
        {
            _managementCommitteeReportService = managementCommitteeReportService;
            _settings = settings;
            _converter = converter;

        }

        [Route("GetManagementCommitteeReport")]
        [HttpGet]
        public async Task<ActionResult<List<GrillaSesionComiteTecnicoCompromiso>>> GetManagementCommitteeReport()
        {
            try
            {
                return await _managementCommitteeReportService.GetManagementCommitteeReport();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetManagementReport")]
        [HttpGet]
        public async Task<ActionResult<List<ComiteTecnico>>> GetManagementReport()
        {
            try
            {
                return await _managementCommitteeReportService.GetManagementReport();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetManagementReportById")]
        [HttpGet]
        public async Task<ActionResult<List<ComiteTecnico>>> GetManagementReportById(int comiteTecnicoId)
        {
            try
            {
                return await _managementCommitteeReportService.GetManagementReportById(comiteTecnicoId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [Route("GetManagementCommitteeReportById")]
        [HttpGet]
        public async Task<ActionResult<List<CompromisoSeguimiento>>> GetManagementCommitteeReportById(int sesionComiteTecnicoCompromisoId)
        {
            try
            {
                return await _managementCommitteeReportService.GetManagementCommitteeReportById(sesionComiteTecnicoCompromisoId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [Route("CreateOrEditReportProgress")]
        [HttpPost]
        public async Task<IActionResult> CreateOrEditReportProgress([FromBody] CompromisoSeguimiento compromisoSeguimiento, string estadoCompromiso)
        {
            Respuesta respuesta = new Respuesta();
            try
            {

                compromisoSeguimiento.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _managementCommitteeReportService.CreateOrEditReportProgress(compromisoSeguimiento, estadoCompromiso);
                return Ok(respuesta);

            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }


        [Route("CreateOrEditCommentReport")]
        [HttpPost]
        public async Task<IActionResult> CreateOrEditCommentReport([FromBody] SesionComentario SesionComentario)
        {
            Respuesta respuesta = new Respuesta();
            try
            {

                SesionComentario.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _managementCommitteeReportService.CreateOrEditCommentReport(SesionComentario);
                return Ok(respuesta);

            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }


        //Descargar acta
        [HttpGet]
        [Route("StartDownloadPDF")]
        public async Task<IActionResult> StartDownloadPDF(int comiteTecnicoId)
        {
            try
            {

                //var result = await _managementCommitteeReportService.GetHTMLString(actaComite);
                var globalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings { Top = 10 },
                    DocumentTitle = "Acata Comite Tecnico"
                    //detailValidarDisponibilidadPresupuesal.NumeroSolicitud != null ? detailValidarDisponibilidadPresupuesal.NumeroSolicitud.ToString() : "",
                };

                var objectSettings = new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = "<html><body><h1>HTML Cargando... </h1></body> </html>", //detailValidarDisponibilidadPresupuesal.htmlContent.ToString(),
                    WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "pdf-styles.css") },
                    HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Pagina [page] de [toPage]", Line = false },
                    //FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Footer" }
                };

                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings },

                };

                var file = _converter.Convert(pdf);

                //return Ok("El documento PDF fue descargado.");
                //return File(file, "application/pdf", "DDP_.pdf");
                return File(file, "application/pdf", "Acta Preliminar" + ".pdf"); //detailValidarDisponibilidadPresupuesal.NumeroSolicitud.ToString()
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

    }
}
