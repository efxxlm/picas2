using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
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

    public class ParametricController : Controller
    {
        private readonly IParametricService _parametricService;
        private readonly IOptions<AppSettings> _settings;

        public ParametricController(IParametricService parametricService, IOptions<AppSettings> settings)
        {
            _parametricService = parametricService;
            _settings = settings;
        }
        [HttpPost]
        [Route("CreateDominio")]
        public async Task<IActionResult> CreateDominio(TipoDominio pTipoDominio)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pTipoDominio.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _parametricService.CreateDominio(pTipoDominio);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpGet]
        [Route("GetParametricas")]
        public async Task<List<VParametricas>> GetParametricas()
        {
            return await _parametricService.GetParametricas();
        }

        [HttpGet]
        [Route("GetDominioByTipoDominioId")]
        public async Task<List<VDominio>> GetDominioByTipoDominioId([FromQuery] int TipoDominioId)
        {
            return await _parametricService.GetDominioByTipoDominioId(TipoDominioId);
        }

    }
}
