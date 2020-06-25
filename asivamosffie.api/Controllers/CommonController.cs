using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asivamosffie.services.Interfaces;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;
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
            var userId = HttpContext.User.FindFirst("UserId").Value;
            var result = common.GetProfile().Result;
            return result;
        }
        [HttpGet]
        public async Task<ActionResult<string>> GetTest()
        {             
            return "ok "+_settings.Value.MailServer;
        }

        [HttpGet]
        [Route("dominioByIdDominio")]
        public async Task<ActionResult<List<Dominio>>> GetDominioByIdDominio(int pIdDominio)
        {
            var result = await common.GetListDominioByIdTipoDominio(pIdDominio);
            return result;
        }


        [HttpGet]
        [Route("ListDepartamento")]
        public async Task<ActionResult<List<Localicacion>>> GetListDepartamento()
        {
            var result = await common.GetListDepartamento();
            return result;
        }


        [HttpGet]
        [Route("ListMunicipiosByIdDepartamento")]
        public async Task<ActionResult<List<Localicacion>>> GetListDepartamento(string idDepartamento)
        {
            var result = await common.GetListMunicipioByIdDepartamento(idDepartamento);
            return result;
        }


        [HttpGet]
        [Route("ListVigenciaAporte")]
        public async Task<ActionResult<List<int>>> GetListVigenciaAportes()
        {
            var result = await common.GetListVigenciaAportes(_settings.Value.yearVigente , _settings.Value.yearSiguienteEsVigente);
            return result;
        }
    }
}