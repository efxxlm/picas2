using asivamosffie.model.APIModels;
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
    public class RegisterPreContructionPhase1Controller : ControllerBase
    {
        public readonly IRegisterPreContructionPhase1Service _registerPreContructionPhase1Service;
        private readonly IOptions<AppSettings> _settings;

        public RegisterPreContructionPhase1Controller(IRegisterPreContructionPhase1Service registerPreContructionPhase1Service, IOptions<AppSettings> settings)
        {
            _settings = settings;
            _registerPreContructionPhase1Service = registerPreContructionPhase1Service;
        }

        [HttpGet]
        [Route("GetListContratacion")]
        public async Task<List<VRegistrarFase1>> GetListContratacion2()
        {
            return await _registerPreContructionPhase1Service.GetListContratacion2(Int32.Parse(HttpContext.User.FindFirst("UserId").Value));
        }

        [HttpGet]
        [Route("GetListContratacionOld")]
        public async Task<List<dynamic>> GetListContratacion()
        {
            return await _registerPreContructionPhase1Service.GetListContratacion();
        }


        [HttpGet]
        [Route("GetContratoByContratoId")]
        public async Task<Contrato> GetContratoByContratoId([FromQuery] int pContratoId)
        {
            return await _registerPreContructionPhase1Service.GetContratoByContratoId(pContratoId);
        }

        [Route("CreateEditContratoPerfil")]
        [HttpPost]
        public async Task<IActionResult> CreateEditContratoPerfil([FromBody] Contrato pContrato)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pContrato.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerPreContructionPhase1Service.CreateEditContratoPerfil(pContrato);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("ChangeStateContrato")]
        [HttpPost]
        public async Task<IActionResult> ChangeStateContrato([FromQuery] int pContratoId, string pEstadoVerificacionContratoCodigo)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _registerPreContructionPhase1Service.ChangeStateContrato(pContratoId, HttpContext.User.FindFirst("User").Value, pEstadoVerificacionContratoCodigo
                   , _settings.Value.DominioFront, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender


                    );
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("DeleteContratoPerfilNumeroRadicado")]
        [HttpPost]
        public async Task<IActionResult> DeleteContratoPerfilNumeroRadicado([FromQuery] int ContratoPerfilNumeroRadicadoId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _registerPreContructionPhase1Service.DeleteContratoPerfilNumeroRadicado(ContratoPerfilNumeroRadicadoId, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("DeleteContratoPerfil")]
        [HttpDelete]
        public async Task<IActionResult> DeleteContratoPerfil([FromQuery] int ContratoPerfilId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _registerPreContructionPhase1Service.DeleteContratoPerfil(ContratoPerfilId, HttpContext.User.FindFirst("User").Value);
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
