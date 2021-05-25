using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

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
        public async Task<FileResult> GetDDPBySesionComiteSolicitudID([FromQuery] int pSesionComiteSolicitudID)
        {
            string pPatchLogo = Path.Combine(_settings.Value.Dominio, _settings.Value.RutaLogo);

            return File(await _manageContractualProcessesService.GetDDPBySesionComiteSolicitudID(pSesionComiteSolicitudID, pPatchLogo), "application/pdf");
        }


        [Route("CambiarEstadoSesionComiteSolicitud")]
        [HttpPost]
        public async Task<IActionResult> CambiarEstadoSesionComiteSolicitud([FromQuery] string pEstadoCodigo, int pSesionComiteSolicitudId, int pSolicitudId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                SesionComiteSolicitud pSesionComiteSolicitud = new SesionComiteSolicitud
                {
                    SolicitudId = pSolicitudId,
                    SesionComiteSolicitudId = pSesionComiteSolicitudId,
                    UsuarioCreacion = HttpContext.User.FindFirst("User").Value,
                    EstadoCodigo = pEstadoCodigo,
                };

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
                if (!string.IsNullOrEmpty(FechaEnvioDocumentacion))
                {
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

        [Route("GetNovedadById")]
        [HttpGet]
        public async Task<NovedadContractual> GetNovedadById(int id)
        {
            var result = await _manageContractualProcessesService.GetNovedadById(id);
            return result;
        }

        [Route("RegistrarTramiteNovedadContractual")]
        [HttpPost]
        public async Task<IActionResult> RegistrarTramiteNovedadContractual([FromBody] NovedadContractual novedadContractual)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                novedadContractual.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _manageContractualProcessesService.RegistrarTramiteNovedadContractual(novedadContractual);

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
