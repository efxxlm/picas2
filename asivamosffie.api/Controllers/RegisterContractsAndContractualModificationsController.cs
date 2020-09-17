﻿using System;
using System.Collections.Generic;
using System.IO;
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
    public class RegisterContractsAndContractualModificationsController : Controller
    {
        public readonly IRegisterContractsAndContractualModificationsService _registerContractsService;
        private readonly IOptions<AppSettings> _settings;
        public RegisterContractsAndContractualModificationsController(
            IRegisterContractsAndContractualModificationsService registerContractsService,
            IOptions<AppSettings> settings
            )
        {
            _registerContractsService = registerContractsService;
            _settings = settings;
        }
    
        [HttpGet]
        [Route("GetListSesionComiteSolicitud")]
        public async Task<List<SesionComiteSolicitud>> GetListSesionComiteSolicitud()
        { 
            return await _registerContractsService.GetListSesionComiteSolicitud(); 
        }
         
        [HttpGet]
        [Route("GetContratacionByContratacionId")]
        public async Task<Contratacion> GetContratacionByContratacionId([FromQuery] int ContratacionId)
        {
            return await _registerContractsService.GetContratacionByContratacionId(ContratacionId);
        }
     
        [Route("RegistrarTramiteContrato")]
        [HttpPost]
        public async Task<IActionResult> RegistrarTramiteContrato([FromForm] Contrato pContrato)
        {
            Respuesta respuesta = new Respuesta();
            try
            { 
                string pUsuarioModifico = HttpContext.User.FindFirst("User").Value;
                pContrato.UsuarioCreacion = pUsuarioModifico;
                respuesta = await _registerContractsService.RegistrarTramiteContrato(pContrato,
               Path.Combine(_settings.Value.DirectoryBase ,_settings.Value.DirectoryBaseRutaDocumentoContrato));
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }
          
    }
}
