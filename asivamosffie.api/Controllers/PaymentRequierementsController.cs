﻿using System;
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
using Microsoft.AspNetCore.Authorization;

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
        public async Task<dynamic> GetObservacionSolicitudPagoByMenuIdAndSolicitudPagoId([FromQuery] int pMenuId, int pSolicitudPagoId, int pPadreId)
        {
            return await _paymentRequierementsService.GetObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(pMenuId, pSolicitudPagoId, pPadreId);
        }

        [Route("GetListSolicitudPago")]
        [HttpGet]
        public async Task<dynamic> GetListSolicitudPago([FromQuery] int pMenuId)
        {
            return await _paymentRequierementsService.GetListSolicitudPago(pMenuId);
        }

    }
}