using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;




namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class PruebaConceptoController : Controller
    {

        private readonly IGenerarGraficoService _pruebaConceptoService;
        private readonly IOptions<AppSettings> _settings;


        public PruebaConceptoController(IGenerarGraficoService pruebaConceptoService, IOptions<AppSettings> settings)
        {
            _pruebaConceptoService = pruebaConceptoService;
            _settings = settings;
        }

        [Route("ChartasFile")]
        [HttpGet]
        public async Task<IActionResult> ChartasFile()
        {
            try
            {
                var path = _settings.Value.DirectoryBase + _settings.Value.DirectoryCharts + "Chart1.png";
                var result = await _pruebaConceptoService.CreateChartasFile(path);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Route("ChartasURL")]
        [HttpGet]
        public async Task<IActionResult> ChartasURL()
        {
            try
            {
                var result = await _pruebaConceptoService.CreateChartasURL();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
