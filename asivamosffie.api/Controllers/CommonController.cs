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
using Microsoft.AspNetCore.Authorization;

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

        [Route("GetFiferenciaMesesDias")]
        [HttpGet]
        public async Task<List<dynamic>> GetFiferenciaMesesDias([FromQuery] double pMesesContrato, double pDiasContrato, double pMesesFase1, double pDiasFase1)
        {

            List<dynamic> Lista = new List<dynamic>();
            pMesesContrato = (30 * pMesesContrato) + pDiasContrato;
            pMesesFase1 = (30 * pMesesFase1) + pDiasFase1;
            int Meses = (int)Math.Truncate((pMesesContrato - pMesesFase1) / 30);
            double dias = (pMesesContrato - pMesesFase1) - (Meses * 30);
            Lista.Add(Meses);
            Lista.Add(dias);
            return Lista;
           


        }

        [HttpGet]
        [Route("CalculardiasLaborales")]
        public Task<DateTime> CalculardiasLaborales([FromQuery] int pDias, DateTime pFechaCalcular)
        {
            return common.CalculardiasLaborales(pDias, pFechaCalcular);
        }

        [HttpGet]
        [Route("GetUsuarioByPerfil")]
        public Task<List<dynamic>> GetUsuarioByPerfil(int idPerfil)
        {
            return common.GetUsuarioByPerfil(idPerfil);
        }


        [HttpGet]
        [Route("GetMenuByRol")]
        public async Task<ActionResult<List<MenuPerfil>>> GetMenuByRol()
        {
            int pUserId = Int32.Parse(HttpContext.User.FindFirst("UserId").Value);
            var result = await common.GetMenuByRol(pUserId);
            return result;
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
            return "ok " + _settings.Value.MailServer;
        }

        [HttpGet]
        [Route("dominioByIdDominio")]
        public async Task<ActionResult<List<Dominio>>> GetDominioByIdDominio(int pIdDominio)
        {
            var result = await common.GetListDominioByIdTipoDominio(pIdDominio);
            return result;
        }

        [HttpGet]
        [Route("dominioByIdDominioNotCode")]
        public async Task<ActionResult<List<Dominio>>> GetDominioByIdDominioNotCode(int pIdDominio, string pMinCode)
        {
            var result = await common.GetListDominioByIdTipoDominio(pIdDominio);
            return result.Where(x => x.Codigo != pMinCode).ToList();
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
        [Route("ListMunicipiosByIdMunicipio")]
        public async Task<ActionResult<List<Localicacion>>> GetListMunicipioByMunicipio(string idMunicipio)
        {
            var result = await common.GetListMunicipioByIdMunicipio(idMunicipio);
            return result;
        }

        [HttpGet]
        [Route("ListDepartamentoByIdMunicipio")]
        public async Task<ActionResult<List<Localicacion>>> GetListDepartamentoByMunicipio(string idMunicipio)
        {
            var result = await common.GetListDepartamentoByIdMunicipio(idMunicipio);
            return result;
        }

        [HttpGet]
        [Route("ListDepartamentoByRegionId")]
        public async Task<ActionResult<List<Localicacion>>> ListDepartamentoByRegionId(string idRegion)
        {
            var result = await common.ListDepartamentoByRegionId(idRegion);
            return result;
        }

        [HttpGet]

        [Route("ListRegion")]
        public async Task<ActionResult<List<Localicacion>>> ListRegion(string idDepartamento)
        {
            var result = await common.ListRegion();
            return result;
        }

        [HttpGet]
        [Route("ListVigenciaAporte")]
        public async Task<ActionResult<List<int>>> GetListVigenciaAportes()
        {

            var result = await common.GetListVigenciaAportes(_settings.Value.YearVigente, _settings.Value.YearSiguienteEsVigente);
            return result;
        }

        [HttpGet]
        [Route("ListIntitucionEducativaByMunicipioId")]
        public async Task<ActionResult<List<InstitucionEducativaSede>>> ListIntitucionEducativaByMunicipioId(string idMunicipio)
        {
            var result = await common.ListIntitucionEducativaByMunicipioId(idMunicipio);
            return result;
        }

        [HttpGet]
        [Route("ListSedeByInstitucionEducativaId")]
        public async Task<ActionResult<List<InstitucionEducativaSede>>> ListSedeByInstitucionEducativaId(int idInstitucionEducativaId)
        {
            var result = await common.ListSedeByInstitucionEducativaId(idInstitucionEducativaId);
            return result;
        }

        [HttpGet]
        [Route("GetInstitucionEducativaById")]
        public async Task<InstitucionEducativaSede> GetInstitucionEducativaById(int idInstitucionEducativaId)
        {
            var result = await common.GetInstitucionEducativaById(idInstitucionEducativaId);
            return result;
        }

        [HttpGet]
        [Route("GetUsuariosByPerfil")]
        public async Task<List<Usuario>> GetUsuariosByPerfil(int pIdPerfil)
        {
            var result = await common.GetUsuariosByPerfil(pIdPerfil);
            return result;
        }
    }
}