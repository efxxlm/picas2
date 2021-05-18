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
    public class RegisterContractualLiquidationRequestController : ControllerBase
    {
        public readonly IRegisterContractualLiquidationRequestService _registerContractualLiquidationRequest;
        private readonly IOptions<AppSettings> _settings;

        public RegisterContractualLiquidationRequestController(IRegisterContractualLiquidationRequestService pRegisterContractualLiquidationRequestService, IOptions<AppSettings> settings)
        {
            _registerContractualLiquidationRequest = pRegisterContractualLiquidationRequestService;
            _settings = settings;
        }

        [Route("GridRegisterContractualLiquidationObra")]
        [HttpGet]
        public async Task<ActionResult<List<VContratacionProyectoSolicitudLiquidacion>>> GridRegisterContractualLiquidationObra([FromQuery] int pMenuId)
        {
            try
            {
                return await _registerContractualLiquidationRequest.GridRegisterContractualLiquidationObra(pMenuId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GridRegisterContractualLiquidationInterventoria")]
        [HttpGet]
        public async Task<ActionResult<List<VContratacionProyectoSolicitudLiquidacion>>> GridRegisterContractualLiquidationInterventoria([FromQuery] int pMenuId)
        {
            try
            {
                return await _registerContractualLiquidationRequest.GridRegisterContractualLiquidationInterventoria(pMenuId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetContratacionProyectoByContratacionProyectoId")]
        [HttpGet]
        public async Task<ActionResult<List<dynamic>>> GetContratacionProyectoByContratacionProyectoId([FromQuery] int pContratacionProyectoId)
        {
            try
            {
                return await _registerContractualLiquidationRequest.GetContratacionProyectoByContratacionProyectoId(pContratacionProyectoId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GridInformeFinal")]
        [HttpGet]
        public async Task<ActionResult<List<dynamic>>> GridInformeFinal([FromQuery] int pContratacionProyectoId, int pMenuId)
        {
            try
            {
                return await _registerContractualLiquidationRequest.GridInformeFinal(pContratacionProyectoId, pMenuId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetInformeFinalByProyectoId")]
        [HttpGet]
        public async Task<ActionResult<List<dynamic>>> GetInformeFinalByProyectoId([FromQuery] int pProyectoId, int pContratacionProyectoId, int pMenuId)
        {
            try
            {
                return await _registerContractualLiquidationRequest.GetInformeFinalByProyectoId(pProyectoId, pContratacionProyectoId, pMenuId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetInformeFinalAnexoByInformeFinalId")]
        [HttpGet]
        public async Task<ActionResult<List<InformeFinalInterventoria>>> GetInformeFinalAnexoByInformeFinalId([FromQuery] int pInformeFinalId)
        {
            try
            {
                return await _registerContractualLiquidationRequest.GetInformeFinalAnexoByInformeFinalId(pInformeFinalId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetObservacionLiquidacionContratacionByMenuIdAndContratacionProyectoId")]
        [HttpGet]
        public async Task<dynamic> GetObservacionLiquidacionContratacionByMenuIdAndContratacionProyectoId([FromQuery] int pMenuId, int pContratacionProyectoId, int pPadreId, string pTipoObservacionCodigo)
        {
            return await _registerContractualLiquidationRequest.GetObservacionLiquidacionContratacionByMenuIdAndContratacionProyectoId(pMenuId, pContratacionProyectoId, pPadreId, pTipoObservacionCodigo);
        }

        [HttpPost]
        [Route("CreateUpdateLiquidacionContratacionObservacion")]
        public async Task<IActionResult> CreateUpdateLiquidacionContratacionObservacion([FromBody] LiquidacionContratacionObservacion pLiquidacionContratacionObservacion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pLiquidacionContratacionObservacion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerContractualLiquidationRequest.CreateUpdateLiquidacionContratacionObservacion(pLiquidacionContratacionObservacion);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("ChangeStatusLiquidacionContratacionProyecto")]
        public async Task<IActionResult> ChangeStatusLiquidacionContratacionProyecto([FromBody] ContratacionProyecto pContratacionProyecto, [FromQuery] int menuId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pContratacionProyecto.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerContractualLiquidationRequest.ChangeStatusLiquidacionContratacionProyecto(pContratacionProyecto, menuId);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("GetHistoricoObservacionLiquidacionContratacionByMenuIdAndContratacionProyectoId")]
        [HttpGet]
        public async Task<dynamic> GetHistoricoObservacionLiquidacionContratacionByMenuIdAndContratacionProyectoId([FromQuery] int pMenuId, int pContratacionProyectoId, int pPadreId, string pTipoObservacionCodigo)
        {
            return await _registerContractualLiquidationRequest.GetHistoricoObservacionLiquidacionContratacionByMenuIdAndContratacionProyectoId(pMenuId, pContratacionProyectoId, pPadreId, pTipoObservacionCodigo);
        }

        [Route("GetContratoPoliza")]
        [HttpGet]
        public async Task<dynamic> GetContratoPoliza([FromQuery] int pContratoPolizaId, int pMenuId, int pContratacionProyectoId)
        {
            var respuesta = await _registerContractualLiquidationRequest.GetContratoPoliza(pContratoPolizaId, pMenuId, pContratacionProyectoId);
            return respuesta;
        }

    }
}