using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class PaymentRequierementsController : Controller
    {
        public readonly IPaymentRequierementsService _paymentRequierementsService;
        private readonly IOptions<AppSettings> _settings;

        public PaymentRequierementsController(IPaymentRequierementsService paymentRequierementsService, IOptions<AppSettings> settings)
        {
            _paymentRequierementsService = paymentRequierementsService;
            _settings = settings;
        }

        [HttpPost]
        [Route("CreateUpdateSolicitudPagoObservacion")]
        public async Task<IActionResult> CreateUpdateSolicitudPagoObservacion([FromBody] SolicitudPagoObservacion pSolicitudPagoObservacion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pSolicitudPagoObservacion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _paymentRequierementsService.CreateUpdateSolicitudPagoObservacion(pSolicitudPagoObservacion);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("CreateEditObservacionFinancieraListaChequeo")]
        public async Task<IActionResult> CreateEditObservacionFinancieraListaChequeo([FromBody] List<SolicitudPagoListaChequeo> pSolicitudPagoListaChequeo)
        {
            Respuesta respuesta = new Respuesta();
            try
            {

                respuesta = await _paymentRequierementsService.CreateEditObservacionFinancieraListaChequeo(pSolicitudPagoListaChequeo, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("ChangueStatusSolicitudPago")]
        public async Task<IActionResult> ChangueStatusSolicitudPago([FromBody] SolicitudPago pSolicitudPago)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pSolicitudPago.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _paymentRequierementsService.ChangueStatusSolicitudPago(pSolicitudPago);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("GetObservacionSolicitudPagoByMenuIdAndSolicitudPagoId")]
        [HttpGet]
        public async Task<dynamic> GetObservacionSolicitudPagoByMenuIdAndSolicitudPagoId([FromQuery] int pMenuId, int pSolicitudPagoId, int pPadreId, string pTipoObservacionCodigo)
        {
            return await _paymentRequierementsService.GetObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(pMenuId, pSolicitudPagoId, pPadreId, pTipoObservacionCodigo);
        }

        [Route("GetListSolicitudPago")]
        [HttpGet]
        public async Task<dynamic> GetListSolicitudPago([FromQuery] int pMenuId)
        {
            return await _paymentRequierementsService.GetListSolicitudPago(pMenuId);
        }

    }
}
