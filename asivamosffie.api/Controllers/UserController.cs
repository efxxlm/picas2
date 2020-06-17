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


 

        [Route("emailRecover")]
        [HttpPost]
        public async Task<IActionResult> RecoverPasswordByEmailAsync([FromBody]Usuario userparam)
        {
            try
            {
                userparam.Ip = HttpContext.Connection.RemoteIpAddress.ToString();
                Task<object> result = _user.RecoverPasswordByEmailAsync(userparam, _settings.Value.Dominio, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);
                object respuesta = await result;
                return Ok(respuesta);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [Route("ChangePasswordUser")]
        [HttpGet]
        public async Task<IActionResult> ChangePasswordUser(string pcontrasenavieja, string pcontrasenanueva)
        {
            var userId = HttpContext.User.FindFirst("UserId").Value;
            var result = await _user.ChangePasswordUser2(Convert.ToInt32(userId),pcontrasenavieja, pcontrasenanueva);
            var response = new ApiResponse<Usuario>(result);
            return Ok(response);
        }

    }
}
