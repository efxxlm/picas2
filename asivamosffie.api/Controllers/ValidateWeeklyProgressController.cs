﻿using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValidateWeeklyProgressController : Controller
    {

        private readonly IValidateWeeklyProgressService _ValidateWeeklyProgressService;
        private readonly IOptions<AppSettings> _settings;

        public ValidateWeeklyProgressController(IValidateWeeklyProgressService ValidateWeeklyProgressService, IOptions<AppSettings> settings)
        {
            _ValidateWeeklyProgressService = ValidateWeeklyProgressService;
            _settings = settings;
        }

        [Route("GetListReporteSemanalView")]
        [HttpGet]
        public async Task<List<VValidarSeguimientoSemanal>> GetListReporteSemanalView([FromQuery] List<string> strListCodEstadoSeguimientoSemanal)
        {
            return await _ValidateWeeklyProgressService.GetListReporteSemanalView(strListCodEstadoSeguimientoSemanal);
        }



        [HttpPost]
        [Route("ReturnSeguimientoSemanal")]
        public async Task<IActionResult> ReturnSeguimientoSemanal([FromQuery] int pSeguimientoSemanalId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _ValidateWeeklyProgressService.ReturnSeguimientoSemanal(pSeguimientoSemanalId, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(respuesta);
            }
        }

    }
}
