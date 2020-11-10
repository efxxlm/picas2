using System;
using System.Collections.Generic;
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
    [Route("api/[controller]")]
    [ApiController]
    public class ManagePreContructionActPhase1Controller : Controller
    {
        private readonly IManagePreContructionActPhase1Service _managePreContruction;
        private readonly IOptions<AppSettings> _settings;

        public ManagePreContructionActPhase1Controller(IManagePreContructionActPhase1Service managePreContructionActPhase1Service, IOptions<AppSettings> settings) {
            _managePreContruction = managePreContructionActPhase1Service;
            _settings = settings;
        }
         
        [Route("GetListContrato")]
        [HttpGet]
        public async Task <List<dynamic>> GetListContrato()
        {
            return await _managePreContruction.GetListContrato();
        }

        [Route("GetContratoByContratoId")]
        [HttpGet]
        public async Task<Contrato> GetContratoByContratoId([FromQuery] int pContratoId)
        {
            return await _managePreContruction.GetContratoByContratoId(pContratoId);
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
        [HttpPut]
        public async Task<Respuesta> LoadActa([FromBody] Contrato pContrato, IFormFile pFile)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pContrato.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _managePreContruction.LoadActa( pContrato, pFile, _settings.Value.DirectoryBase, _settings.Value.DirectoryActaSuscritaContrato);
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
               HttpContext.User.FindFirst("User").Value);
                return respuesta;

            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return respuesta;
            }
        }

        [HttpGet]
        [Route("GetActaByIdPerfil")]
        public async Task<FileResult> GetActaByIdPerfil([FromQuery] int pPerfilId, int pContratoId)
        {
            return File(await _managePreContruction.GetActaByIdPerfil(pPerfilId, pContratoId), "application/pdf");
        }

        [HttpGet]
        [Route("GetListContratoObservacionByContratoId")]
        public async Task<List<ContratoObservacion>> GetListContratoObservacionByContratoId([FromQuery] int pContratoId)
        {
            return await _managePreContruction.GetListContratoObservacionByContratoId(pContratoId);
        }


        [Route("CreateEditObservacionesActa")]
        [HttpPut]
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
