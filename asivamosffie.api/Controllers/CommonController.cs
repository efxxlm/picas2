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
    public class CommonController : ControllerBase
    {
        public readonly ICommonService common;
        public CommonController(ICommonService prmCommon)
        {
            common = prmCommon;
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
            return "ok";
        }
    }
}