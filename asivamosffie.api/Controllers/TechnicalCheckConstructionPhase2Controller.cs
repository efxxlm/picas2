using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asivamosffie.services.Interfaces;
using asivamosffie.model.Models;
using Microsoft.Extensions.Options;
using lalupa.Authorization.JwtHelpers;
using asivamosffie.services.Exceptions;
using asivamosffie.model.APIModels;
 
namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnicalCheckConstructionPhase2Controller : Controller
    {
        private readonly ITechnicalCheckConstructionPhase2Service _technicalCheckConstructionPhase2Service;


        public TechnicalCheckConstructionPhase2Controller(ITechnicalCheckConstructionPhase2Service technicalCheckConstructionPhase2Service)
        { 
          _technicalCheckConstructionPhase2Service = technicalCheckConstructionPhase2Service;
        }

        [HttpGet]
        [Route("GetContractsGrid")]
        public Task<List<VRequisitosTecnicosConstruccionAprobar>> GetContractsGrid([FromQuery] string pTipoContrato)
        {
            return _technicalCheckConstructionPhase2Service.GetContractsGrid(" ", pTipoContrato);
        }


        //[Route("ChangeStateContrato")]
        //[HttpPost]
        //public async Task<IActionResult> ChangeStateContrato([FromQuery] int pContratoId, string pEstadoVerificacionContratoCodigo)
        //{
        //    Respuesta respuesta = new Respuesta();
        //    try
        //    {
        //        respuesta = await _registerPreContructionPhase1Service.ChangeStateContrato(pContratoId, HttpContext.User.FindFirst("User").Value, pEstadoVerificacionContratoCodigo
        //           , _settings.Value.DominioFront, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender


        //            );
        //        return Ok(respuesta);
        //    }
        //    catch (Exception ex)
        //    {
        //        respuesta.Data = ex.ToString();
        //        return BadRequest(respuesta);
        //    }
        //}
    }
}
