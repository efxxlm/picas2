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
            return File(await _manageContractualProcessesService.GetDDPBySesionComiteSolicitudID(pSesionComiteSolicitudID), "application/pdf");
        }


        [Route("CambiarEstadoSesionComiteSolicitud")]
        [HttpPost]
        public async Task<IActionResult> CambiarEstadoSesionComiteSolicitud([FromBody] SesionComiteSolicitud pSesionComiteSolicitud)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pSesionComiteSolicitud.UsuarioCreacion = "";//HttpContext.User.FindFirst("User").Value;
                respuesta = await _manageContractualProcessesService.CambiarEstadoSesionComiteSolicitud(pSesionComiteSolicitud);
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

        [Route("RegistrarTramiteContratacion")]
        [HttpPost]
        public async Task<IActionResult> RegistrarTramiteContratacion([FromForm] Contratacion pContratacion, string FechaEnvioDocumentacion)
        {
            Respuesta respuesta = new Respuesta();
            string JsonContratacion = JsonConvert.SerializeObject(pContratacion);
            try
            {
                Type myType = pContratacion.GetType();
                IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());


                Contratacion contratacion =JsonConvert.DeserializeObject<Contratacion>(JsonContratacion);


                contratacion.UsuarioCreacion = "";//HttpContext.User.FindFirst("User").Value;
                respuesta = await _manageContractualProcessesService.RegistrarTramiteContratacion(contratacion, null
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
