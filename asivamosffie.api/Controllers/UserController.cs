using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asivamosffie.services.Interfaces;
using asivamosffie.model.Models;
using Microsoft.Extensions.Options;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUser _user;
        private readonly IOptions<AppSettings> _settings;

        public UserController(IOptions<AppSettings> settings , IUser user)
        {
              _user = user;
              _settings = settings;
        }



        public string GetIp()
        {
            return HttpContext.Connection.RemoteIpAddress.ToString();
        }


        [Route("emailRecover")]
        [HttpPost]
        public IActionResult RecoverPasswordByEmail([FromBody]Usuario userparam)
        {
            try
            {
                var usuario = _user.RecoverPasswordByEmailAsync(userparam.Email, GetIp(),_settings.Value.Dominio , _settings.Value.MailServer , _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password , _settings.Value.Sender );
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
