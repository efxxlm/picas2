using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using asivamosffie.model.APIModels;
using System.IO;
using Microsoft.Extensions.Options;
using System.Reflection;
using Newtonsoft.Json;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterValidatePaymentRequierementsController : Controller
    {
        public readonly IRegisterValidatePaymentRequierementsService _registerValidatePaymentRequierementsService;
        private readonly IOptions<AppSettings> _settings;

        public RegisterValidatePaymentRequierementsController(IRegisterValidatePaymentRequierementsService registerValidatePaymentRequierementsService, IOptions<AppSettings> settings)
        {
            _registerValidatePaymentRequierementsService = registerValidatePaymentRequierementsService;
            _settings = settings;
        } 
        [HttpGet]
        [Route("GetTipoPagoByCriterioCodigo")]
        public async Task<IActionResult> GetTipoPagoByCriterioCodigo([FromQuery] string pCriterioCodigo)
        {
            return Ok(await _registerValidatePaymentRequierementsService.GetTipoPagoByCriterioCodigo(pCriterioCodigo));
        }

        [HttpGet]
        [Route("GetListSolicitudPago")]
        public async Task<IActionResult> GetListSolicitudPago()
        {
            return Ok(await _registerValidatePaymentRequierementsService.GetListSolicitudPago());
        }

        [HttpGet]
        [Route("GetCriterioByFormaPagoCodigo")]
        public async Task<IActionResult> GetCriterioByFormaPagoCodigo([FromQuery] string pFormaPagoCodigo)
        {
            return Ok(await _registerValidatePaymentRequierementsService.GetCriterioByFormaPagoCodigo(pFormaPagoCodigo));
        }

        [HttpPost]
        [Route("CreateEditNewPayment")]
        public async Task<IActionResult> CreateEditNewPayment([FromBody] SolicitudPago pSolicitudPago)
        {
            pSolicitudPago.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
            return Ok(await _registerValidatePaymentRequierementsService.CreateEditNewPayment(pSolicitudPago));
        }

        [HttpGet]
        [Route("GetContratoByTipoSolicitudCodigoModalidadContratoCodigoOrNumeroContrato")]
        public async Task<dynamic> GetContratoByTipoSolicitudCodigoModalidadContratoCodigoOrNumeroContrato([FromQuery] string pTipoSolicitud, string pModalidadContrato, string pNumeroContrato)
        {
            return await _registerValidatePaymentRequierementsService.GetContratoByTipoSolicitudCodigoModalidadContratoCodigoOrNumeroContrato(pTipoSolicitud, pModalidadContrato, pNumeroContrato);
        }

        [HttpGet]
        [Route("GetContratoByContratoId")]
        public async Task<IActionResult> GetContratoByContratoId([FromQuery] int pContratoId)
        {
            return Ok(await _registerValidatePaymentRequierementsService.GetContratoByContratoId(pContratoId));
        }

        [HttpGet]
        [Route("GetProyectosByIdContrato")]
        public async Task<IActionResult> GetProyectosByIdContrato([FromQuery] int pContratoId)
        {
            return Ok(await _registerValidatePaymentRequierementsService.GetProyectosByIdContrato(pContratoId));
        }
    }
}
