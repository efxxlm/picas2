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
    public class ManagementCommitteeReportController : ControllerBase
    {
        private readonly IManagementCommitteeReportService _managementCommitteeReportService;
        private readonly IOptions<AppSettings> _settings;


        public ManagementCommitteeReportController(IOptions<AppSettings> settings, IManagementCommitteeReportService managementCommitteeReportService)
        {
            _managementCommitteeReportService = managementCommitteeReportService;
            _settings = settings;

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

        [Route("GetManagementCommitteeReportById")]
        [HttpGet]
        public async Task<ActionResult<List<SesionComiteTecnicoCompromiso>>> GetManagementCommitteeReportById(int SesionComiteTecnicoCompromisoId)
        {
            try
            {
                return await _managementCommitteeReportService.GetManagementCommitteeReportById(SesionComiteTecnicoCompromisoId);
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
    }
}
