using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asivamosffie.services.Interfaces;
using asivamosffie.model.Models;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        public readonly IUser _user;



        public string GetIp()
        {
            return HttpContext.Connection.RemoteIpAddress.ToString();
        }


        [Route("emailRecoverSender")]
        [HttpPost]
        public async Task<ActionResult<Usuario>> recoverPasswordByEmail([FromBody]Usuario userparam)
        {
            try
            {
                var usuario = await _user.RecoverPasswordByEmailAsync(userparam.Email, GetIp());
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
