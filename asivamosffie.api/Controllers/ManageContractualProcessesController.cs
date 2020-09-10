using System;
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
 

        [Route("CambiarEstadoSesionComiteSolicitud")]
        [HttpPut]
        public async Task<IActionResult> CambiarEstadoSesionComiteSolicitud([FromBody] SesionComiteSolicitud pSesionComiteSolicitud)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pSesionComiteSolicitud.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _manageContractualProcessesService.CambiarEstadoSesionComiteSolicitud(pSesionComiteSolicitud);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }


        [Route("RegistrarTramiteContratacion")]
        [HttpPut]
        public async Task<IActionResult> RegistrarTramiteContratacion([FromBody] Contratacion pContratacion , IFormFile pFile)
        {
            Respuesta respuesta = new Respuesta();
            try
            { 
                pContratacion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value; 
                respuesta = await _manageContractualProcessesService.RegistrarTramiteContratacion(pContratacion , pFile , _settings.Value.DirectoryBase , _settings.Value.DirectoryBaseContratacionMinuta);
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
