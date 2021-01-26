using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asivamosffie.services.Interfaces;
using asivamosffie.model.Models;
using Microsoft.Extensions.Options;
using lalupa.Authorization.JwtHelpers;
using asivamosffie.services.Exceptions;
using asivamosffie.model.APIModels;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterFinalReportController : ControllerBase
    {
        public readonly IRegisterFinalReportService _registerFinalReport;
        private readonly IOptions<AppSettings> _settings;

        public RegisterFinalReportController(IRegisterFinalReportService pRegisterFinalReportService, IOptions<AppSettings> settings)
        {
            _registerFinalReport = pRegisterFinalReportService;
            _settings = settings;
        }

        [Route("gridRegisterFinalReport")]
        [HttpGet]
        public async Task<ActionResult<List<VProyectosCierre>>> gridRegisterFinalReport()
        {
            try
            {
                return await _registerFinalReport.gridRegisterFinalReport();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetInformeFinalByContratacionProyectoId")]
        public async Task<List<InformeFinal>> GetInformeFinalByContratacionProyectoId([FromQuery] int pContratacionProyectoId)
        {
            try
            {
                return await _registerFinalReport.GetInformeFinalByContratacionProyectoId(pContratacionProyectoId);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("CreateEditInformeFinal")]
        public async Task<IActionResult> CreateEditInformeFinal([FromBody] InformeFinal pInformeFinal)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pInformeFinal.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerFinalReport.CreateEditInformeFinal( pInformeFinal );
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpGet]
        [Route("GetInformeFinalListaChequeoByInformeFinalId")]
        public async Task<List<InformeFinalInterventoria>> GetInformeFinalListaChequeoByInformeFinalId([FromQuery] int pInformeFinalId)
        {
            try
            {
                return await _registerFinalReport.GetInformeFinalListaChequeoByInformeFinalId(pInformeFinalId);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}