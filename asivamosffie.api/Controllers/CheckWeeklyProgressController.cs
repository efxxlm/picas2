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
    public class CheckWeeklyProgressController : Controller
    {
        private readonly ICheckWeeklyProgressService _checkWeeklyProgressService;
        private readonly IOptions<AppSettings> _settings;

        public CheckWeeklyProgressController(ICheckWeeklyProgressService checkWeeklyProgressService, IOptions<AppSettings> settings)
        {
            _checkWeeklyProgressService = checkWeeklyProgressService;
            _settings = settings;
        }

        [Route("GetValidarRegistroCompletoObservaciones")]
        [HttpGet]
        public async Task<bool> GetValidarRegistroCompletoObservaciones([FromQuery] int pSeguimientoSemanalId, bool esSupervisor)
        {
            return await _checkWeeklyProgressService.GetValidarRegistroCompletoObservaciones(pSeguimientoSemanalId, esSupervisor);
        }

        [Route("CreateEditSeguimientoSemanalObservacion")]
        [HttpPost]
        public async Task<IActionResult> CreateEditSeguimientoSemanalObservacion([FromBody] SeguimientoSemanalObservacion pSeguimientoSemanalObservacion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pSeguimientoSemanalObservacion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _checkWeeklyProgressService.CreateEditSeguimientoSemanalObservacion(pSeguimientoSemanalObservacion, false);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("GetSeguimientoSemanalBySeguimientoSemanalId")]
        [HttpGet]
        public async Task<ActionResult<SeguimientoSemanal>> GetSeguimientoSemanalBySeguimientoSemanalId([FromQuery] int pSeguimientoSemanalId)
        {
            return await _checkWeeklyProgressService.GetSeguimientoSemanalBySeguimientoSemanalId(pSeguimientoSemanalId);
        }

        [Route("GetListReporteSemanal")]
        [HttpGet]
        public async Task<dynamic> GetListReporteSemanal()
        {
            return await _checkWeeklyProgressService.GetListReporteSemanal();
        }

        [Route("GetListReporteSemanalView")]
        [HttpGet]
        public async Task<List<VVerificarSeguimientoSemanal>> GetListReporteSemanalView([FromQuery] List<string> strListCodEstadoSeguimientoSemanal)
        {
            return await _checkWeeklyProgressService.GetListReporteSemanalView(strListCodEstadoSeguimientoSemanal);
        }
    }
}
