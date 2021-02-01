﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using asivamosffie.model.APIModels;
using System.IO;
using Microsoft.Extensions.Options; 
using System.Reflection;
using Newtonsoft.Json;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageContractualProcessesController : ControllerBase
    {
        public readonly IManageContractualProcessesService _manageContractualProcessesService;
        private readonly IOptions<AppSettings> _settings;

        public ManageContractualProcessesController(IOptions<AppSettings> settings, IManageContractualProcessesService IManageContractualProcessesService)
        {
            _manageContractualProcessesService = IManageContractualProcessesService;
            _settings = settings;
        }


        [Route("GetListSesionComiteSolicitud")]
        [HttpGet]
        public async Task<List<VListaContratacionModificacionContractual>> GetListSesionComiteSolicitudV2()
        {
            var result = await _manageContractualProcessesService.GetListSesionComiteSolicitudV2();
            return result;
        }

        [Route("GetListSesionComiteSolicitudOLd")]
        [HttpGet]
        public async Task<List<SesionComiteSolicitud>> GetListSesionComiteSolicitud()
        {
            var result = await _manageContractualProcessesService.GetListSesionComiteSolicitud();
            return result;
        }

        [Route("GetContratacionByContratacionId")]
        [HttpGet]
        public async Task<Contratacion> GetContratacionByContratacionId([FromQuery] int pContratacionId)
        {
            var result = await _manageContractualProcessesService.GetContratacionByContratacionId(pContratacionId);
            return result;
        }
        [HttpGet]
        [Route("GetDDPBySesionComiteSolicitudID")]
        public async Task<FileResult> GetDDPBySesionComiteSolicitudID([FromQuery] int  pSesionComiteSolicitudID)
        {
            string pPatchLogo = Path.Combine(_settings.Value.Dominio, _settings.Value.RutaLogo);

            return File(await _manageContractualProcessesService.GetDDPBySesionComiteSolicitudID(pSesionComiteSolicitudID, pPatchLogo), "application/pdf");
        }


        [Route("CambiarEstadoSesionComiteSolicitud")]
        [HttpPost]
        public async Task<IActionResult> CambiarEstadoSesionComiteSolicitud([FromBody] SesionComiteSolicitud pSesionComiteSolicitud)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pSesionComiteSolicitud.UsuarioCreacion =  HttpContext.User.FindFirst("User").Value;
                respuesta = await _manageContractualProcessesService.CambiarEstadoSesionComiteSolicitud(pSesionComiteSolicitud, _settings.Value.DominioFront, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        public object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperties()
                  .Single(pi => pi.Name == propName)
                  .GetValue(src, null);
        }


        [HttpPost]
        [Route("RegistrarTramiteContratacion")] 
        public async Task<IActionResult> RegistrarTramiteContratacion([FromForm] Contratacion pContratacion, string FechaEnvioDocumentacion)
        {
            Respuesta respuesta = new Respuesta(); 
            try
            {
                if (!string.IsNullOrEmpty(FechaEnvioDocumentacion)) {
                    pContratacion.FechaEnvioDocumentacion = DateTime.Parse(FechaEnvioDocumentacion);
                    }
        
               pContratacion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value; 
               respuesta = await _manageContractualProcessesService.RegistrarTramiteContratacion(pContratacion, pContratacion.pFile
                  , _settings.Value.DirectoryBase, _settings.Value.DirectoryBaseContratacionMinuta);

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
