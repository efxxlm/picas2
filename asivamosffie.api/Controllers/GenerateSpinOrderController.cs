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
    public class GenerateSpinOrderController : Controller
    {
        public readonly IGenerateSpinOrderService _generateSpinOrderService;
        private readonly IOptions<AppSettings> _settings;

        public GenerateSpinOrderController(IGenerateSpinOrderService _GenerateSpinOrderService, IOptions<AppSettings> settings)
        {
            _generateSpinOrderService = _GenerateSpinOrderService;
            _settings = settings;
        }

        [Route("GetSolicitudPagoBySolicitudPagoId")]
        [HttpGet]
        public async Task<SolicitudPago> GetSolicitudPagoBySolicitudPagoId([FromQuery] int SolicitudPagoId)
        {
            try
            {
                return await _generateSpinOrderService.GetSolicitudPagoBySolicitudPagoId(SolicitudPagoId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetListOrdenGiro")]
        [HttpGet]
        public async Task<dynamic> GetListOrdenGiro([FromQuery] int pMenuId)
        {
            return await _generateSpinOrderService.GetListOrdenGiro(pMenuId);
        }

        [Route("GetValorConceptoByAportanteId")]
        [HttpGet]
        public async Task<dynamic> GetValorConceptoByAportanteId([FromQuery] int pAportanteId, int pSolicitudPagoId, string pConceptoPago)
        {
            return await _generateSpinOrderService.GetValorConceptoByAportanteId(pAportanteId, pSolicitudPagoId, pConceptoPago);
        }

        [Route("GetFuentesDeRecursosPorAportanteId")]
        [HttpGet]
        public async Task<dynamic> GetFuentesDeRecursosPorAportanteId([FromQuery] int pAportanteId)
        {
            return await _generateSpinOrderService.GetFuentesDeRecursosPorAportanteId(pAportanteId);
        }

        [HttpPost]
        [Route("CreateEditOrdenGiro")]
        public async Task<IActionResult> CreateEditOrdenGiro([FromBody] OrdenGiro pOrdenGiro)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pOrdenGiro.UsuarioCreacion = User.Identity.Name;
                respuesta = await _generateSpinOrderService.CreateEditOrdenGiro(pOrdenGiro);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("DeleteOrdenGiroDetalleDescuentoTecnicaAportante")]
        public async Task<IActionResult> DeleteOrdenGiroDetalleDescuentoTecnicaAportante([FromQuery] int pOrdenGiroDetalleDescuentoTecnicaAportanteId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _generateSpinOrderService.DeleteOrdenGiroDetalleDescuentoTecnicaAportante(pOrdenGiroDetalleDescuentoTecnicaAportanteId, User.Identity.Name);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }


    }
}
