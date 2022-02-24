using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
    public class GenerateSpinOrderController : Controller
    {
        public readonly IGenerateSpinOrderService _generateSpinOrderService;
        private readonly IOptions<AppSettings> _settings;

        public GenerateSpinOrderController(IGenerateSpinOrderService _GenerateSpinOrderService, IOptions<AppSettings> settings)
        {
            _generateSpinOrderService = _GenerateSpinOrderService;
            _settings = settings;
        }
        [AllowAnonymous]
        [Route("GetInfoPlantilla")]
        [HttpGet]
        public async Task<List<VPlantillaOrdenGiro>> GetInfoPlantilla([FromQuery] int pOrdenGiroId)
        {
            try
            {
                return await _generateSpinOrderService.GetInfoPlantilla(pOrdenGiroId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("DeleteOrdenGiroDetalleTerceroCausacionDescuento")]
        public async Task<IActionResult> DeleteOrdenGiroDetalleTerceroCausacionDescuento([FromBody] List<int> pOrdenGiroDetalleTerceroCausacionDescuentoId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _generateSpinOrderService.DeleteOrdenGiroDetalleTerceroCausacionDescuento(pOrdenGiroDetalleTerceroCausacionDescuentoId, User.Identity.Name);
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
        [HttpPost]
        [Route("DeleteOrdenGiroDetalleDescuentoTecnicaByConcepto")]
        public async Task<IActionResult> DeleteOrdenGiroDetalleDescuentoTecnicaByConcepto([FromQuery] int pOrdenGiroDetalleDescuentoTecnicaId, [FromQuery] string pConceptoPagoCodigo)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _generateSpinOrderService.DeleteOrdenGiroDetalleDescuentoTecnicaByConcepto(pOrdenGiroDetalleDescuentoTecnicaId, pConceptoPagoCodigo, User.Identity.Name);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("DeleteOrdenGiroDetalleTerceroCausacionAportante")]
        [HttpPost]
        public async Task<Respuesta> DeleteOrdenGiroDetalleTerceroCausacionAportante([FromQuery] int pOrdenGiroDetalleTerceroCausacionAportanteId)
        {
            try
            {
                return await _generateSpinOrderService.DeleteOrdenGiroDetalleTerceroCausacionAportante(pOrdenGiroDetalleTerceroCausacionAportanteId, User.Identity.Name);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("DeleteOrdenGiroDetalleDescuentoTecnica")]
        [HttpPost]
        public async Task<Respuesta> DeleteOrdenGiroDetalleDescuentoTecnica([FromQuery] int pOrdenGiroDetalleDescuentoTecnicaId)
        {
            try
            {
                return await _generateSpinOrderService.DeleteOrdenGiroDetalleDescuentoTecnica(pOrdenGiroDetalleDescuentoTecnicaId, User.Identity.Name);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [AllowAnonymous]
        [Route("GetSolicitudPagoBySolicitudPagoId")]
        [HttpGet]
        public async Task<SolicitudPago> GetSolicitudPagoBySolicitudPagoId([FromQuery] int SolicitudPagoId, bool esSolicitudPago)
        {
            try
            {
                return await _generateSpinOrderService.GetSolicitudPagoBySolicitudPagoId(SolicitudPagoId, esSolicitudPago);
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
    }
}
