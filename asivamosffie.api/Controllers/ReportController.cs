using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;




namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportController : Controller
    {

        private readonly IReportService _reportService;


        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Returns Embed token, Embed URL, and Embed token expiry to the client
        /// </summary>
        /// <returns>JSON containing parameters for embedding</returns>

        [Route("GetReportEmbedInfo")]
        [HttpGet]
        public async Task<List<IndicadorReporte>> GetReportEmbedInfo()
        {
            try
            {
                return await _reportService.GetReportEmbedInfo();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetReportEmbedInfoByIndicadorReporteId")]
        [HttpGet]
        public async Task<IndicadorReporte> GetReportEmbedInfoByIndicadorReporteId([FromQuery] int indicadorReporteId)
        {
            return await _reportService.GetReportEmbedInfoByIndicadorReporteId(indicadorReporteId);
        }

        [Route("GetIndicadorReporte")]
        [HttpGet]
        public async Task<List<IndicadorReporte>> GetIndicadorReporte()
        {
            return await _reportService.GetIndicadorReporte();
        }
    }
}
