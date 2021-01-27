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
        [Route("GetSolicitudPago")]
        public async Task<IActionResult> GetSolicitudPago([FromQuery] int pSolicitudPagoId)
        {
            return Ok(await _registerValidatePaymentRequierementsService.GetSolicitudPago(pSolicitudPagoId));
        }

        [HttpGet]
        [Route("GetValidateSolicitudPagoId")]
        public async Task<IActionResult> GetValidateSolicitudPagoId([FromQuery] int pSolicitudPagoId)
        {
            return Ok(await _registerValidatePaymentRequierementsService.GetSolicitudPago(pSolicitudPagoId));
        }

        [HttpGet]
        [Route("GetListProyectosByLlaveMen")]
        public async Task<IActionResult> GetListProyectosByLlaveMen([FromQuery] string pLlaveMen)
        {
            return Ok(await _registerValidatePaymentRequierementsService.GetListProyectosByLlaveMen(pLlaveMen));
        }

        [HttpGet]
        [Route("GetProyectosByIdContrato")]
        public async Task<IActionResult> GetProyectosByIdContrato([FromQuery] int pContratoId)
        {
            return Ok(await _registerValidatePaymentRequierementsService.GetProyectosByIdContrato(pContratoId));
        }

        [HttpPost]
        [Route("DeleteSolicitudPago")]
        public async Task<IActionResult> DeleteSolicitudPago([FromQuery] int pSolicitudPagoId)
        {
            return Ok(await _registerValidatePaymentRequierementsService.DeleteSolicitudPago(pSolicitudPagoId, HttpContext.User.FindFirst("User").Value));
        }

        [HttpPost]
        [Route("DeleteSolicitudPagoFaseFacturaDescuento")]
        public async Task<IActionResult> DeleteSolicitudPagoFaseFacturaDescuento([FromQuery] int pSolicitudPagoFaseFacturaDescuentoId)
        {
            return Ok(await _registerValidatePaymentRequierementsService.DeleteSolicitudPagoFaseFacturaDescuento(pSolicitudPagoFaseFacturaDescuentoId, HttpContext.User.FindFirst("User").Value));
        }
        [HttpPost]
        [Route("DeleteSolicitudPagoFaseCriterio")]
        public async Task<IActionResult> DeleteSolicitudPagoFaseCriterio([FromQuery] int pSolicitudPagoFaseCriterioId)
        {
            return Ok(await _registerValidatePaymentRequierementsService.DeleteSolicitudPagoFaseCriterio(pSolicitudPagoFaseCriterioId, HttpContext.User.FindFirst("User").Value));
        }
        [HttpPost]
        [Route("DeleteSolicitudPagoFaseCriterioProyecto")]
        public async Task<IActionResult> DeleteSolicitudPagoFaseCriterioProyecto([FromQuery] int pSolicitudPagoFaseCriterioId)
        {
            return Ok(await _registerValidatePaymentRequierementsService.DeleteSolicitudPagoFaseCriterioProyecto(pSolicitudPagoFaseCriterioId, HttpContext.User.FindFirst("User").Value));
        }

        [HttpPost]
        [Route("DeleteSolicitudLlaveCriterioProyecto")]
        public async Task<IActionResult> DeleteSolicitudLlaveCriterioProyecto([FromQuery] int pContratacionProyectoId)
        {
            return Ok(await _registerValidatePaymentRequierementsService.DeleteSolicitudLlaveCriterioProyecto(pContratacionProyectoId, HttpContext.User.FindFirst("User").Value));
        }

        [HttpPost]
        [Route("CreateEditExpensas")]
        public async Task<IActionResult> CreateEditExpensas([FromBody] SolicitudPago pSolicitudPago)
        {
            pSolicitudPago.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
            return Ok(await _registerValidatePaymentRequierementsService.CreateEditExpensas(pSolicitudPago));
        }

        [HttpPost]
        [Route("CreateEditOtrosCostosServicios")]
        public async Task<IActionResult> CreateEditOtrosCostosServicios([FromBody] SolicitudPago pSolicitudPago)
        {
            pSolicitudPago.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
            return Ok(await _registerValidatePaymentRequierementsService.CreateEditOtrosCostosServicios(pSolicitudPago));
        }

        [HttpPost]
        [Route("CreateEditNewPayment")]
        public async Task<IActionResult> CreateEditNewPayment([FromBody] SolicitudPago pSolicitudPago)
        {
            pSolicitudPago.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
            return Ok(await _registerValidatePaymentRequierementsService.CreateEditNewPayment(pSolicitudPago));
        }

        [HttpGet]
        [Route("GetTipoPagoByCriterioCodigo")]
        public async Task<IActionResult> GetTipoPagoByCriterioCodigo([FromQuery] string pCriterioCodigo)
        {
            return Ok(await _registerValidatePaymentRequierementsService.GetTipoPagoByCriterioCodigo(pCriterioCodigo));
        }

        [HttpGet]
        [Route("GetConceptoPagoCriterioCodigoByTipoPagoCodigo")]
        public async Task<IActionResult> GetConceptoPagoCriterioCodigoByTipoPagoCodigo([FromQuery] string TipoPagoCodigo)
        {
            return Ok(await _registerValidatePaymentRequierementsService.GetConceptoPagoCriterioCodigoByTipoPagoCodigo(TipoPagoCodigo));
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

        [HttpGet]
        [Route("GetContratoByTipoSolicitudCodigoModalidadContratoCodigoOrNumeroContrato")]
        public async Task<dynamic> GetContratoByTipoSolicitudCodigoModalidadContratoCodigoOrNumeroContrato([FromQuery] string pTipoSolicitud, string pModalidadContrato, string pNumeroContrato)
        {
            return await _registerValidatePaymentRequierementsService.GetContratoByTipoSolicitudCodigoModalidadContratoCodigoOrNumeroContrato(pTipoSolicitud, pModalidadContrato, pNumeroContrato);
        }

        [HttpGet]
        [Route("GetContratoByContratoId")]
        public async Task<IActionResult> GetContratoByContratoId([FromQuery] int pContratoId, int pSolicitudPago)
        {
            return Ok(await _registerValidatePaymentRequierementsService.GetContratoByContratoId(pContratoId, pSolicitudPago));
        }



    }
}
