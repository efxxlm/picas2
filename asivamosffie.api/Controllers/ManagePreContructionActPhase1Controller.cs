using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class ManagePreContructionActPhase1Controller : Controller
    {
        private readonly IManagePreContructionActPhase1Service _managePreContruction;
        private readonly IOptions<AppSettings> _settings;
        private readonly PublicController _publicController;

        public ManagePreContructionActPhase1Controller(IManagePreContructionActPhase1Service managePreContructionActPhase1Service, IOptions<AppSettings> settings)
        {
            _managePreContruction = managePreContructionActPhase1Service;
            _settings = settings;
        }

        [Route("GetListContrato")]
        [HttpGet]
        public async Task<List<dynamic>> GetListContrato()
        {
            return await _managePreContruction.GetListContrato();
        }

        [Route("GetContratoByContratoId")]
        [HttpGet]
        public async Task<Contrato> GetContratoByContratoId([FromQuery] int pContratoId)
        {
            int pUserId = Int32.Parse(HttpContext.User.FindFirst("UserId").Value);
            return await _managePreContruction.GetContratoByContratoId(pContratoId, pUserId);
        } 
        [HttpGet]
        [Route("GetListGrillaActaInicio")]
        public async Task<ActionResult<List<GrillaActaInicio>>> GetListGrillaActaInicio(int pPerfilId)
        {
            try
            {
                return await _managePreContruction.GetListGrillaActaInicio(pPerfilId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("EditContrato")]
        [HttpPut]
        public async Task<Respuesta> EditContrato([FromBody] Contrato pContrato)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pContrato.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _managePreContruction.EditContrato(pContrato);
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return respuesta;
            }
        }

        [Route("LoadActa")]
        [HttpPost]
        public async Task<Respuesta> LoadActa([FromForm] Contrato pContrato)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pContrato.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _managePreContruction.LoadActa(pContrato, pContrato.pFile, _settings.Value.DirectoryBase, _settings.Value.DirectoryActaSuscritaContrato,
                    ToAppSettingsService(_settings)
                    );
                return respuesta;

            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return respuesta;
            }
        }

        [Route("CambiarEstadoActa")]
        [HttpPut]
        public async Task<Respuesta> CambiarEstadoActa([FromQuery] int pContratoId, string pEstadoContrato)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _managePreContruction.CambiarEstadoActa(pContratoId, pEstadoContrato,
               HttpContext.User.FindFirst("User").Value, ToAppSettingsService(_settings));
                return respuesta;

            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return respuesta;
            }
        }

        public AppSettingsService ToAppSettingsService(IOptions<AppSettings> appSettings)
        {
            AppSettingsService appSettingsService = new AppSettingsService
            {
                MailPort = appSettings.Value.MailPort,
                MailServer = appSettings.Value.MailServer,
                Password = appSettings.Value.Password,
                Sender = appSettings.Value.Sender
            };
            return appSettingsService;
        }
        [HttpGet]
        [Route("GetActaByIdPerfil")]
        public async Task<FileResult> GetActaByIdPerfil([FromQuery] int pPerfilId, int pContratoId)
        {
            int pUserId = 38; //Int32.Parse(HttpContext.User.FindFirst("UserId").Value);
            return File(await _managePreContruction.GetActaByIdPerfil(pPerfilId, pContratoId, pUserId, ToAppSettingsService(_settings)), "application/pdf");
        }

        [HttpGet]
        [Route("GetListContratoObservacionByContratoId")]
        public async Task<List<ContratoObservacion>> GetListContratoObservacionByContratoId([FromQuery] int pContratoId)
        {
            return await _managePreContruction.GetListContratoObservacionByContratoId(pContratoId);
        }

        [HttpPut]
        [Route("CreateEditObservacionesActa")]
        public async Task<Respuesta> CreateEditObservacionesActa([FromBody] ContratoObservacion pcontratoObservacion)
        {
            try
            {
                pcontratoObservacion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                return await _managePreContruction.CreateEditObservacionesActa(pcontratoObservacion);
            }
            catch (Exception ex)
            {
                Respuesta respuesta = new Respuesta
                {
                    Data = ex.InnerException.ToString()
                };
                return respuesta;
            }
        }

    }
}
