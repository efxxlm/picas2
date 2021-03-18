using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asivamosffie.services.Interfaces;
using asivamosffie.model.Models;
using Microsoft.Extensions.Options;
using asivamosffie.api.Responses;
using System.Security.Claims;
using asivamosffie.model.APIModels;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUser _user;
        private readonly IOptions<AppSettings> _settings;

        public UserController(IOptions<AppSettings> settings, IUser user)
        {
            _user = user;
            _settings = settings;
        }

        #region Loggin
        [Route("emailRecover")]
        [HttpPost]
        public async Task<IActionResult> RecoverPasswordByEmailAsync([FromBody] Usuario userparam)
        {
            try
            {
                userparam.Ip = HttpContext.Connection.RemoteIpAddress.ToString();
                //   userparam.UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                Task<Respuesta> result = _user.RecoverPasswordByEmailAsync(userparam, _settings.Value.Dominio, _settings.Value.DominioFront, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);
                Respuesta respuesta = await result;
                return Ok(respuesta);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
 

        [Route("ActivateDeActivateUsuario")]
        [HttpPost]
        public async Task<IActionResult> ActivateDeActivateUsuario([FromBody] Usuario pUsuario)
        {
            try
            {
                pUsuario.UsuarioCreacion = HttpContext.User.FindFirst("UserId").Value;
                var result = await _user.ActivateDeActivateUsuario(pUsuario);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("ChangePasswordUser")]
        [HttpPost]
        public async Task<IActionResult> ChangePasswordUser([FromQuery] string Oldpwd, [FromQuery] string Newpwd)
        {
            try
            {
                var userId = HttpContext.User.FindFirst("UserId").Value;
                var result = await _user.ChangePasswordUser(Convert.ToInt32(userId), Oldpwd, Newpwd);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("ValidateCurrentPassword")]
        [HttpPost]
        public async Task<IActionResult> ValidateCurrentPassword([FromQuery] string Oldpwd)
        {
            try
            {
                var userId = HttpContext.User.FindFirst("UserId").Value;
                var result = await _user.ValidateCurrentPassword(Convert.ToInt32(userId), Oldpwd);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("CloseSesion")]
        [HttpPost]
        public async Task<IActionResult> CloseSesion([FromQuery] string Oldpwd, [FromQuery] string Newpwd)
        {
            try
            {
                var userId = HttpContext.User.FindFirst("UserId") == null ? "SESIÓN CERRADA" : HttpContext.User.FindFirst("UserId").Value;
                var result = await _user.CloseSesion(Convert.ToInt32(userId));
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        [HttpPost]
        [Route("ValidateExistEmail")]
        public async Task<bool> ValidateExistEmail([FromBody] Usuario pUsuario)
        {
            return await _user.ValidateExistEmail(pUsuario);
        }

        [HttpPost]
        [Route("CreateEditUsuario")]
        public async Task<Respuesta> CreateEditUsuario([FromBody] Usuario pUsuario)
        {
            pUsuario.UsuarioCreacion = User.Identity.Name;
            var result = await _user.CreateEditUsuario(pUsuario);
            return result;
        }

        [HttpGet]
        [Route("GetContratoByTipo")]
        public Task<dynamic> GetContratoByTipo([FromQuery]string strTipoRolAsignadoContratoCodigo , int pUsuarioId)
        {
            return _user.GetContratoByTipo(strTipoRolAsignadoContratoCodigo , pUsuarioId);
        }

        [HttpGet]
        [Route("GetListUsuario")]
        public Task<List<VUsuarioRol>> GetListUsuario()
        {
            var result = _user.GetListUsuario();
            return result;
        }

        [HttpGet]
        [Route("GetListPerfil")]
        public Task<dynamic> GetListPerfil()
        {
            return _user.GetListPerfil();
        }

        [HttpGet]
        [Route("GetUsuario")]
        public Task<Usuario> GetUsuario([FromQuery] int pUsuarioId)
        {
            return _user.GetUsuario(pUsuarioId);
        }
    }
}
