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
    public class CommonController : ControllerBase
    {
        public readonly ICommonService common;
        private readonly IOptions<AppSettings> _settings;
        public CommonController(ICommonService prmCommon, IOptions<AppSettings> settings)
        {
            common = prmCommon;
            _settings = settings;
        }
        [HttpGet]
        [Route("perfiles")]
        public async Task<ActionResult<List<Perfil>>> GetProfile()
        {
            var result = common.GetProfile().Result;
            return result;
        }
        [HttpGet]
        public async Task<ActionResult<string>> GetTest()
        {            

            return "ok "+_settings.Value.MailServer;
        }
    }
}