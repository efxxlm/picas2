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
using System.IO;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReprogrammingController : Controller
    {

        private readonly IReprogrammingService _reprogrammingService;
        private readonly IOptions<AppSettings> _settings;

        public ReprogrammingController(IReprogrammingService reprogrammingService, IOptions<AppSettings> settings)
        {
            _reprogrammingService = reprogrammingService;
            _settings = settings;
        }


        [Route("GetAjusteProgramacionGrid")]
        [HttpGet]
        public async Task<List<VAjusteProgramacion>> GetAjusteProgramacionGrid()
        {
            try
            {
                return await _reprogrammingService.GetAjusteProgramacionGrid();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetAjusteProgramacionById")]
        [HttpGet]
        public async Task<AjusteProgramacion> GetAjusteProgramacionById([FromQuery] int pAjusteProgramacionId)
        {
            try
            {
                string usuario = HttpContext.User.FindFirst("User").Value;
                return await _reprogrammingService.GetAjusteProgramacionById(pAjusteProgramacionId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("AprobarAjusteProgramacion")]
        [HttpPost]
        public async Task<Respuesta> AprobarAjusteProgramacion([FromQuery] int pAjusteProgramacionId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _reprogrammingService.AprobarAjusteProgramacion(pAjusteProgramacionId, HttpContext.User.FindFirst("User").Value
                    , _settings.Value.DominioFront, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);
                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("CreateEditObservacionAjusteProgramacion")]
        [HttpPost]
        public async Task<Respuesta> CreateEditObservacionAjusteProgramacion([FromBody] AjusteProgramacion pAjusteProgramacion, bool esObra)
        {
            try
            {
                string usuario = HttpContext.User.FindFirst("User").Value;
                return await _reprogrammingService.CreateEditObservacionAjusteProgramacion(pAjusteProgramacion, esObra, usuario);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("EnviarAlSupervisorAjusteProgramacion")]
        [HttpPost]
        public async Task<IActionResult> EnviarAlSupervisorAjusteProgramacion([FromQuery] int pAjusteProgramacionId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _reprogrammingService.EnviarAlSupervisorAjusteProgramacion(pAjusteProgramacionId, HttpContext.User.FindFirst("User").Value
                    , _settings.Value.DominioFront, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("UploadFileToValidateAdjustmentProgramming")]
        [HttpPost]
        public async Task<IActionResult> UploadFileToValidateAdjustmentProgramming(IFormFile file, [FromQuery] int pAjusteProgramacionId, int pContratacionProyectId, int pNovedadContractualId,
                                                                            int pContratoId, int pProyectoId)
        {
            try
            {
                Respuesta respuesta = new Respuesta();

                if (file.Length > 0 && file.FileName.Contains(".xls"))
                {
                    //string strUsuario = "";
                    string strUsuario = HttpContext.User.FindFirst("User").Value;
                    respuesta = await _reprogrammingService.UploadFileToValidateAdjustmentProgramming(file, Path.Combine(_settings.Value.DirectoryBase, _settings.Value.DirectoryBaseCargue, _settings.Value.DirectoryBaseAjusteProgramacionObra), strUsuario, pAjusteProgramacionId, pContratacionProyectId, pNovedadContractualId, pContratoId, pProyectoId);
                }
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [Route("TransferMassiveLoadAdjustmentProgramming")]
        [HttpPost]
        public async Task<IActionResult> TransferMassiveLoadAdjustmentProgramming([FromQuery] string pIdDocument, int pProyectoId, int pContratoId)
        {
            try
            {
                Respuesta respuesta = new Respuesta();
                string pUsuarioModifico = HttpContext.User.FindFirst("User").Value;
                respuesta = await _reprogrammingService.TransferMassiveLoadAdjustmentProgramming(pIdDocument, pUsuarioModifico, pProyectoId, pContratoId);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [Route("UploadFileToValidateAdjustmentInvestmentFlow")]
        [HttpPost]
        public async Task<IActionResult> UploadFileToValidateAdjustmentInvestmentFlow(IFormFile file, [FromQuery] int pAjusteProgramacionId, int pContratacionProyectId, int pNovedadContractualId,
                                                                                    int pContratoId, int pProyectoId)
        {
            try
            {
                Respuesta respuesta = new Respuesta();

                if (file.Length > 0 && file.FileName.Contains(".xls"))
                {
                    //string strUsuario = "";
                    string strUsuario = HttpContext.User.FindFirst("User").Value;
                    respuesta = await _reprogrammingService.UploadFileToValidateAdjustmentInvestmentFlow(file, Path.Combine(_settings.Value.DirectoryBase, _settings.Value.DirectoryBaseCargue, _settings.Value.DirectoryBaseAjusteProgramacionObra), strUsuario, pAjusteProgramacionId, pContratacionProyectId, pNovedadContractualId, pContratoId, pProyectoId);
                }
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [Route("TransferMassiveLoadAdjustmentInvestmentFlow")]
        [HttpPost]
        public async Task<IActionResult> TransferMassiveLoadAdjustmentInvestmentFlow([FromQuery] string pIdDocument, int pProyectoId, int pContratoId)
        {
            try
            {
                Respuesta respuesta = new Respuesta();
                string pUsuarioModifico = HttpContext.User.FindFirst("User").Value;
                respuesta = await _reprogrammingService.TransferMassiveLoadAdjustmentInvestmentFlow(pIdDocument, pUsuarioModifico, pProyectoId, pContratoId);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

    }
}
