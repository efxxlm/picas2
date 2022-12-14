using asivamosffie.model.APIModels;
using asivamosffie.services.Interfaces;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AvailabilityBudgetProyectController : ControllerBase
    {
        private readonly IRequestBudgetAvailabilityService _availabilityBudgetProyectService;
        private readonly IOptions<AppSettings> _settings;
        private readonly IConverter _converter;

        public AvailabilityBudgetProyectController(IOptions<AppSettings> settings, IConverter converter, IRequestBudgetAvailabilityService availabilityBudgetProyectService)
        {
            _availabilityBudgetProyectService = availabilityBudgetProyectService;
            _settings = settings;
            _converter = converter;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetDetailAvailabilityBudgetProyectNew")]
        public async Task<List<DetailValidarDisponibilidadPresupuesal>> GetDetailAvailabilityBudgetProyectNew([FromQuery] int disponibilidadPresupuestalId, bool esNovedad, int RegistroNovedadId, bool esGenerar)
        {
            try
            {
                return await _availabilityBudgetProyectService.GetDetailAvailabilityBudgetProyectNew(disponibilidadPresupuestalId, esNovedad, RegistroNovedadId, esGenerar);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [Route("GetAvailabilityBudgetProyect")]
        public async Task<IActionResult> GetAvailabilityBudgetProyect()
        {
            try
            {
                var result = await _availabilityBudgetProyectService.GetBudgetavailabilityRequests();
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //Detalle de la solicitud
        [Route("GetDetailAvailabilityBudgetProyect")]
        public async Task<IActionResult> GetDetailAvailabilityBudgetProyect(int disponibilidadPresupuestalId, bool esNovedad, int RegistroNovedadId)
        {
            try
            {
                var result = await _availabilityBudgetProyectService.GetDetailAvailabilityBudgetProyect(disponibilidadPresupuestalId, esNovedad, RegistroNovedadId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("StartDownloadPDF")]
        public async Task<IActionResult> StartDownloadPDF([FromBody] DetailValidarDisponibilidadPresupuesal detailValidarDisponibilidadPresupuesal)
        {
            try
            {

                //    var result = await  _availabilityBudgetProyectService.GetHTMLString(detailValidarDisponibilidadPresupuesal);

                var globalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings { Top = 10 },
                    DocumentTitle = detailValidarDisponibilidadPresupuesal.NumeroSolicitud != null ? detailValidarDisponibilidadPresupuesal.NumeroSolicitud.ToString() : "",
                };

                var objectSettings = new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = detailValidarDisponibilidadPresupuesal.htmlContent.ToString(),
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
                return File(file, "application/pdf", detailValidarDisponibilidadPresupuesal.NumeroSolicitud.ToString() + ".pdf");
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        [HttpGet]
        [Route("GetDetailAvailabilityBudgetProyectHistorical")]
        public async Task<List<DetailValidarDisponibilidadPresupuesal>> GetDetailAvailabilityBudgetProyectHistorical([FromQuery] int disponibilidadPresupuestalId, bool esNovedad, int RegistroNovedadId, bool esGenerar)
        {
            try
            {
                return await _availabilityBudgetProyectService.GetDetailAvailabilityBudgetProyectHistorical(disponibilidadPresupuestalId, esNovedad, RegistroNovedadId, esGenerar);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
