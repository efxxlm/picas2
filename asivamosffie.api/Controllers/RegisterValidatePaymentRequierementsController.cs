﻿using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

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
        [Route("GetMontoMaximoMontoPendiente")]
        public async Task<IActionResult> GetMontoMaximoMontoPendiente([FromQuery] int SolicitudPagoId, string strFormaPago, bool EsPreConstruccion)
        {
            return Ok(await _registerValidatePaymentRequierementsService.GetMontoMaximoMontoPendiente(SolicitudPagoId, strFormaPago, EsPreConstruccion));
        }

        [HttpGet]
        [Route("GetMontoMaximo")]
        public async Task<IActionResult> GetMontoMaximo([FromQuery] int SolicitudPagoId, bool EsPreConstruccion)
        {
            return Ok(await _registerValidatePaymentRequierementsService.GetMontoMaximo(SolicitudPagoId, EsPreConstruccion));
        }

        [HttpGet]
        [Route("GetValidateSolicitudPagoId")]
        public async Task GetValidateSolicitudPagoId([FromQuery] int pSolicitudPagoId)
        {
            await _registerValidatePaymentRequierementsService.GetValidateSolicitudPagoId(pSolicitudPagoId);
        }

        [HttpGet]
        [Route("GetMontoMaximoProyecto")]
        public async Task<IActionResult> GetMontoMaximoProyecto([FromQuery] int pContrato, int pContratacionProyectoId, bool EsPreConstruccion)
        {
            return Ok(await _registerValidatePaymentRequierementsService.GetMontoMaximoProyecto(pContrato, pContratacionProyectoId, EsPreConstruccion));

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
        [Route("ReturnSolicitudPago")]
        public async Task<IActionResult> ReturnSolicitudPago([FromBody] SolicitudPago pSolicitudPago)
        {
            pSolicitudPago.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
            return Ok(await _registerValidatePaymentRequierementsService.ReturnSolicitudPago(pSolicitudPago));
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
        [Route("GetFormaPagoCodigoByFase")]
        public async Task<IActionResult> GetFormaPagoCodigoByFase([FromQuery] bool pEsPreconstruccion)
        {
            return Ok(await _registerValidatePaymentRequierementsService.GetFormaPagoCodigoByFase(pEsPreconstruccion));
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
        [Route("GetUsoByConceptoPagoCriterioCodigo")]
        public async Task<IActionResult> GetUsoByConceptoPagoCriterioCodigo([FromQuery] string pConceptoPagoCodigo, int pContratoId)
        {
            return Ok(await _registerValidatePaymentRequierementsService.GetUsoByConceptoPagoCriterioCodigo(pConceptoPagoCodigo, pContratoId));
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
