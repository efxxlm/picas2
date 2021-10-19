﻿using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //quito el autorize porque las pruebas de frontend usan este controller 20201229
    //[Authorize]
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
        [Route("GetVideos")]
        public async Task<ActionResult<dynamic>> GetVideos()
        {
            var result = await common.GetVideos();
            return result;
        }


        [HttpPost]
        [Route("GetHtmlToPdf")]
        public async Task<FileResult> GetHtmlToPdf([FromBody] Plantilla pPlantilla)
        {
            try
            {
                return File(await common.GetHtmlToPdf(pPlantilla), "application/pdf");
            }
            catch (Exception e)
            {
                return File(e.InnerException.ToString(), "application/pdf");
            }
        }

        [Route("GetFiferenciaMesesDias")]
        [HttpGet]
        public async Task<List<dynamic>> GetFiferenciaMesesDias([FromQuery] double pMesesContrato, double pDiasContrato, double pMesesFase1, double pDiasFase1)
        {
            List<dynamic> Lista = new List<dynamic>();
            pMesesContrato = (30 * pMesesContrato) + pDiasContrato;
            pMesesFase1 = (30 * pMesesFase1) + pDiasFase1;
            Lista.Add((int)Math.Truncate((pMesesContrato - pMesesFase1) / 30));
            Lista.Add(((pMesesContrato - pMesesFase1) - (Lista[0] * 30)));
            return Lista;
        }
        [Route("StringReplace")]
        [HttpPost]
        public async Task<string> StringReplace([FromQuery] string pstring)
        {
            pstring = pstring.Replace(@"\", "");
            return pstring.Replace("/", "");
        }

        [HttpGet]
        [Route("CalculardiasLaborales")]
        public Task<DateTime> CalculardiasLaborales([FromQuery] int pDias, DateTime pFechaCalcular)
        {
            return common.CalculardiasLaborales(pDias, pFechaCalcular);
        }

        [HttpGet]
        [Route("GetPlantillaById")]
        public Task<Plantilla> GetPlantillaById([FromQuery] int pPlantillaId)
        {
            return common.GetPlantillaById(pPlantillaId);
        }

        [HttpGet]
        [Route("TienePermisos")]
        public Task<VPermisosMenus> VPermisosMenus([FromQuery] int idPerfil, string pRuta)
        {
            return common.TienePermisos(idPerfil, pRuta);
        }

        [HttpGet]
        [Route("GetUsuarioByPerfil")]
        public Task<List<dynamic>> GetUsuarioByPerfil(int idPerfil)
        {
            return common.GetUsuarioByPerfil(idPerfil);
        }

        [HttpGet]
        [Route("GetMenuByRol")]
        [Authorize]
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
        [Route("GetListMenu")]
        public async Task<dynamic> GetListMenu()
        {
            var result = await common.GetListMenu();
            return result;
        }

        [HttpGet]
        [Route("GetUsuariosByPerfil")]
        public async Task<List<Usuario>> GetUsuariosByPerfil(int pIdPerfil)
        {
            var result = await common.GetUsuariosByPerfil(pIdPerfil);
            return result;
        }


        [Route("BestDeveloper")]
        [HttpGet]
        public async Task<string> BestDeveloper([FromQuery] int codigo, string Key = "zkbyqd cqhjydup sqijqñutq")
        {
            if (Key.Length == 0) return String.Empty;

            char chr = Key[0].ToString().ToLower()[0];

            var code = IsBasicLetter(chr) ? (char)('z' - (('z' - chr + codigo) % 26)) : chr;
            return code + await BestDeveloper(codigo, Key[1..]);
        }
        private static bool IsBasicLetter(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }

        [Route("GetValorTotalDisponibilidad")]
        [HttpGet]
        public decimal GetValorTotalDisponibilidad([FromQuery] int pDisponibilidadPresupuestalId)
        {
            try
            {
                return common.GetValorTotalDisponibilidad(pDisponibilidadPresupuestalId,false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}