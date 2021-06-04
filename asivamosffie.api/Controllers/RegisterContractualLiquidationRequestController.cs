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

        [Route("GetContratacionByContratacionId")]
        [HttpGet]
        public async Task<ActionResult<List<dynamic>>> GetContratacionByContratacionId([FromQuery] int pContratacionId)
        {
            try
            {
                return await _registerContractualLiquidationRequest.GetContratacionByContratacionId(pContratacionId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GridInformeFinal")]
        [HttpGet]
        public async Task<ActionResult<List<dynamic>>> GridInformeFinal([FromQuery] int pContratacionId, int pMenuId)
        {
            try
            {
                return await _registerContractualLiquidationRequest.GridInformeFinal(pContratacionId, pMenuId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetInformeFinalByProyectoId")]
        [HttpGet]
        public async Task<ActionResult<List<dynamic>>> GetInformeFinalByProyectoId([FromQuery] int pProyectoId, int pContratacionId, int pMenuId)
        {
            try
            {
                return await _registerContractualLiquidationRequest.GetInformeFinalByProyectoId(pProyectoId, pContratacionId, pMenuId);
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

        [Route("GetObservacionLiquidacionContratacionByMenuIdAndContratacionId")]
        [HttpGet]
        public async Task<dynamic> GetObservacionLiquidacionContratacionByMenuIdAndContratacionId([FromQuery] int pMenuId, int pContratacionId, int pPadreId, string pTipoObservacionCodigo)
        {
            return await _registerContractualLiquidationRequest.GetObservacionLiquidacionContratacionByMenuIdAndContratacionId(pMenuId, pContratacionId, pPadreId, pTipoObservacionCodigo);
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

        [Route("ChangeStatusLiquidacionContratacion")]
        public async Task<IActionResult> ChangeStatusLiquidacionContratacionProyecto([FromBody] Contratacion pContratacion, [FromQuery] int menuId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pContratacion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerContractualLiquidationRequest.ChangeStatusLiquidacionContratacion(pContratacion, menuId);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("GetHistoricoObservacionLiquidacionContratacionByMenuIdAndContratacionId")]
        [HttpGet]
        public async Task<dynamic> GetHistoricoObservacionLiquidacionContratacionByMenuIdAndContratacionId([FromQuery] int pMenuId, int pContratacionId, int pPadreId, string pTipoObservacionCodigo)
        {
            return await _registerContractualLiquidationRequest.GetHistoricoObservacionLiquidacionContratacionByMenuIdAndContratacionId(pMenuId, pContratacionId, pPadreId, pTipoObservacionCodigo);
        }

        [Route("GetContratoPoliza")]
        [HttpGet]
        public async Task<dynamic> GetContratoPoliza([FromQuery] int pContratoPolizaId, int pMenuId, int pContratacionId)
        {
            var respuesta = await _registerContractualLiquidationRequest.GetContratoPoliza(pContratoPolizaId, pMenuId, pContratacionId);
            return respuesta;
        }


        [HttpGet]
        [Route("GetBalanceByContratacionId")]
        public async Task<dynamic> GetBalanceByContratacionId([FromQuery] int pContratacionId, int pMenuId)
        {
            try
            {
                return await _registerContractualLiquidationRequest.GetBalanceByContratacionId(pContratacionId, pMenuId);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetAllNoveltyByContratacion")]
        public async Task<List<NovedadContractual>> GetAllNoveltyByContratacion([FromQuery] int pContratacionId)
        {
            try
            {
                return await _registerContractualLiquidationRequest.GetAllNoveltyByContratacion(pContratacionId);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetPolizaByContratacionId")]
        public async Task<VContratacionProyectoSolicitudLiquidacion> GetPolizaByContratacionId([FromQuery] int pContratacionId)
        {
            try
            {
                return await _registerContractualLiquidationRequest.GetPolizaByContratacionId(pContratacionId);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}