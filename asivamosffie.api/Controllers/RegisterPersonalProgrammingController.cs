﻿using System;
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
    public class RegisterPersonalProgrammingController : Controller
    {
        public readonly IRegisterPersonalProgrammingService _IRegisterPersonalProgrammingService;


        public RegisterPersonalProgrammingController(IRegisterPersonalProgrammingService registerPersonalProgrammingService)
        {

            _IRegisterPersonalProgrammingService = registerPersonalProgrammingService;
        }
        [Route("GetListProyectos")]
        [HttpGet]
        public async Task<List<dynamic>> GetListProyectos()
        {
            var result = await _IRegisterPersonalProgrammingService.GetListProyectos();
            return result;
        }

        [Route("GetProgramacionPersonalByContratoConstruccionId")]
        [HttpGet]
        public async Task<List<ProgramacionPersonalContratoConstruccion>> GetProgramacionPersonalByContratoConstruccionId([FromQuery] int pContratoConstruccionId)
        {
            var result = await _IRegisterPersonalProgrammingService.GetProgramacionPersonalByContratoConstruccionId(pContratoConstruccionId, HttpContext.User.FindFirst("User").Value.ToUpper());
            return result;
        }
         
        [Route("UpdateProgramacionContratoPersonal")]
        [HttpPost]
        public async Task<IActionResult> UpdateProgramacionContratoPersonal([FromBody] ContratoConstruccion pContratoConstruccion)
        {
            try
            {
                Task<Respuesta> result = _IRegisterPersonalProgrammingService.UpdateProgramacionContratoPersonal(pContratoConstruccion);
                object respuesta = await result;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
         
        [Route("ChangeStatusProgramacionContratoPersonal")]
        [HttpPost]
        public async Task<IActionResult> ChangeStatusProgramacionContratoPersonal([FromQuery] int pContratoConstruccionId, string pEstadoProgramacionCodigo)
        {
            try
            {
                Task<Respuesta> result = _IRegisterPersonalProgrammingService.ChangeStatusProgramacionContratoPersonal(pContratoConstruccionId, pEstadoProgramacionCodigo, HttpContext.User.FindFirst("User").Value);
                object respuesta = await result;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
