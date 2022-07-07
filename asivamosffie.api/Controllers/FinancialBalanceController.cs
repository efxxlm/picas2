using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FinancialBalanceController : ControllerBase
    {
        private readonly IOptions<AppSettings> _settings;
        private readonly IFinalBalanceService _finalBalanceService;
        public FinancialBalanceController(IOptions<AppSettings> settings, IFinalBalanceService finalBalanceService)
        {
            _finalBalanceService = finalBalanceService;
            _settings = settings;
        }
        //        Task<OrdenGiro> SeeDetailOdg(int pOrdenGiroId);
        [AllowAnonymous]
        [HttpGet]
        [Route("SeeDetailOdg")]
        public async Task<IActionResult> SeeDetailOdg([FromQuery]int pOrdenGiroId)
        {
            try
            {
                return Ok(await _finalBalanceService.SeeDetailOdg(pOrdenGiroId));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("GetEjecucionFinancieraXProyectoId")]
        public async Task<IActionResult> GetEjecucionFinancieraXProyectoId([FromQuery] int pProyectoId)
        {
            try
            {
                return Ok(await _finalBalanceService.GetEjecucionFinancieraXProyectoId(pProyectoId));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        } 

        [HttpPost]
        [Route("ChangeStatudBalanceFinanciero")]
        public async Task<IActionResult> ChangeStatudBalanceFinanciero([FromBody] BalanceFinanciero pBalanceFinanciero)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pBalanceFinanciero.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _finalBalanceService.ChangeStatudBalanceFinanciero(pBalanceFinanciero);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("ChangeStatudBalanceFinancieroTraslado")]
        public async Task<IActionResult> ChangeStatudBalanceFinancieroTraslado([FromBody] BalanceFinancieroTraslado pBalanceFinancieroTraslado)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pBalanceFinancieroTraslado.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _finalBalanceService.ChangeStatudBalanceFinancieroTraslado(pBalanceFinancieroTraslado);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("ValidateCompleteBalanceFinanciero")]
        public async Task<IActionResult> ValidateCompleteBalanceFinanciero([FromQuery] int pBalanceFinancieroTrasladoId, bool pEstaCompleto)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _finalBalanceService.ValidateCompleteBalanceFinanciero(pBalanceFinancieroTrasladoId, pEstaCompleto);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }
         
        [HttpGet]
        [Route("GetOrdenGiroByNumeroOrdenGiro")]
        public async Task<IActionResult> GetOrdenGiroByNumeroOrdenGiro([FromQuery] string pTipoSolicitudCodigo, string pNumeroOrdenGiro, string pLLaveMen)
        {
            try
            {
                return Ok(await _finalBalanceService.GetOrdenGiroByNumeroOrdenGiro(pTipoSolicitudCodigo, pNumeroOrdenGiro, pLLaveMen));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

         
        [HttpGet]
        [Route("GetTablaUsoFuenteAportanteXContratoId")]
        public async Task<TablaUsoFuenteAportante> GetTablaUsoFuenteAportanteXContratoId([FromQuery] int pContratoId)
        {
            try
            {
                return await _finalBalanceService.GetTablaUsoFuenteAportanteXContratoId(pContratoId);
            }
            catch (Exception e)
            {
                return new TablaUsoFuenteAportante();
            }
        }    
        
        [HttpGet]
        [Route("GetTablaUsoFuenteAportanteXContratoIdXProyectoId")]
        public async Task<TablaUsoFuenteAportante> GetTablaUsoFuenteAportanteXContratoIdXProyectoId([FromQuery] int pContratoId, int pProyectoId)
        {
            try
            {
                return await _finalBalanceService.GetTablaUsoFuenteAportanteXContratoId(pContratoId , pProyectoId);
            }
            catch (Exception e)
            {
                return new TablaUsoFuenteAportante();
            }
        }

        [HttpGet]
        [Route("GridBalance")]
        public async Task<IActionResult> GridBalance()
        {
            try
            {
                return Ok(await _finalBalanceService.GridBalance());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet]
        [Route("GetOrdenGiroBy")]
        public async Task<IActionResult> GetOrdenGiroBy([FromQuery] string pTipoSolicitudCodigo, string pNumeroOrdenGiro, string pLLaveMen)
        {
            try
            {
                return Ok(await _finalBalanceService.GetOrdenGiroBy(pTipoSolicitudCodigo, pNumeroOrdenGiro, pLLaveMen));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetContratoByProyectoId")]
        public async Task<IActionResult> GetContratoByProyectoId([FromQuery] int pProyectoId)
        {
            try
            {
                return Ok(await _finalBalanceService.GetContratoByProyectoId(pProyectoId));
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetDataByProyectoId")]
        public async Task<IActionResult> GetDataByProyectoId([FromQuery] int pProyectoId)
        {
            try
            {
                return Ok(await _finalBalanceService.GetDataByProyectoId(pProyectoId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("CreateEditBalanceFinanciero")]
        public async Task<IActionResult> CreateEditBalanceFinanciero([FromBody] BalanceFinanciero pBalanceFinanciero)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pBalanceFinanciero.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _finalBalanceService.CreateEditBalanceFinanciero(pBalanceFinanciero);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpGet]
        [Route("GetBalanceFinanciero")]
        public async Task<IActionResult> GetBalanceFinanciero([FromQuery] int pProyectoId)
        {
            try
            {
                return Ok(await _finalBalanceService.GetBalanceFinanciero(pProyectoId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("ApproveBalance")]
        public async Task<IActionResult> ApproveBalance([FromQuery] int pProyectoId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _finalBalanceService.ApproveBalance(pProyectoId, HttpContext.User.FindFirst("User").Value);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }
    }
}
