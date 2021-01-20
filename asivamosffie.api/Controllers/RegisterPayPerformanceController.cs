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
    public class RegisterPayPerformanceController : ControllerBase
    {
        private readonly IRegisterPayPerformanceService _registerPayPerformanceService;
        private readonly IOptions<AppSettings> _settings;
        private readonly IConverter _converter;



        public RegisterPayPerformanceController(IOptions<AppSettings> settings, IConverter converter, IRegisterPayPerformanceService registerPayPerformanceService)
        {
            _registerPayPerformanceService = registerPayPerformanceService;
            _settings = settings;
            _converter = converter;

        }


        [Route("uploadFileToValidate")]
        [HttpPost]
        public async Task<IActionResult> uploadFileToValidate(IFormFile file, [FromQuery] string typeFile)
        {
            try
            {
                Respuesta respuesta = new Respuesta();

                if (file.Length > 0 && file.FileName.Contains(".xls"))
                {
                    string strUsuario = HttpContext.User.FindFirst("User").Value;
                    respuesta = await _registerPayPerformanceService.uploadFileToValidate(file, strUsuario, typeFile);
                }
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [Route("getPaymentsPerformances")]
        [HttpGet]
        public async Task<List<dynamic>> getPaymentsPerformances([FromQuery] string typeFile)
        {
            try
            {
                return await _registerPayPerformanceService.getPaymentsPerformances(typeFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
