using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnicalRequirementsConstructionPhaseController : ControllerBase
    {
        private readonly ITechnicalRequirementsConstructionPhaseService _technicalRequirementsConstructionPhaseService;
        private readonly IOptions<AppSettings> _settings;
        private readonly IConverter _converter;


        public TechnicalRequirementsConstructionPhaseController(IOptions<AppSettings> settings, IConverter converter, ITechnicalRequirementsConstructionPhaseService technicalRequirementsConstructionPhaseService)
        {
            _technicalRequirementsConstructionPhaseService = technicalRequirementsConstructionPhaseService;
            _settings = settings;
            _converter = converter;

        }

        [Route("GetContractsGrid")]
        [HttpGet]
        public async Task<List<dynamic>> GetContractsGrid(int pUsuarioId)
        {
            try
            {
                return await _technicalRequirementsConstructionPhaseService.GetContractsGrid(pUsuarioId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [Route("CambiarEstadoContratoEstadoVerificacionConstruccionCodigo")]
        [HttpPost]
        public async Task<IActionResult> CambiarEstadoContratoEstadoVerificacionConstruccionCodigo([FromQuery] int ContratoId, string pEstado)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta =await _technicalRequirementsConstructionPhaseService.CambiarEstadoContratoEstadoVerificacionConstruccionCodigo(ContratoId, pEstado, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }
 

        [Route("CreateEditObservacion")]
        [HttpPost]
        public async Task<Respuesta> CreateEditObservacion([FromBody] ContratoConstruccion pContratoConstruccion, string pTipoObservacion, bool pEsSupervicion)
        {
            try
            {
                pContratoConstruccion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                return await _technicalRequirementsConstructionPhaseService.CreateEditObservacion(pContratoConstruccion, pTipoObservacion, pEsSupervicion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetContractsGridApoyoObra")]
        [HttpGet]
        public async Task<List<dynamic>> GetContractsGridApoyoObra()
        {
            try
            {
                return await _technicalRequirementsConstructionPhaseService.GetContractsGridApoyoObra();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetContractsGridApoyoInterventoria")]
        [HttpGet]
        public async Task<List<dynamic>> GetContractsGridApoyoInterventoria()
        {
            try
            {
                return await _technicalRequirementsConstructionPhaseService.GetContractsGridApoyoInterventoria();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [Route("GetContratoByContratoId")]
        [HttpGet]
        public async Task<Contrato> GetContratoByContratoId(int pContratoId)
        {
            try
            {
                string usuarioCreacion = HttpContext.User.FindFirst("User").Value;
                return await _technicalRequirementsConstructionPhaseService.GetContratoByContratoId(pContratoId, usuarioCreacion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [Route("GetLoadProgrammingGrid")]
        [HttpGet]
        public async Task<List<ArchivoCargue>> GetLoadProgrammingGrid(int pContratoConstruccionId)
        {
            try
            {
                return await _technicalRequirementsConstructionPhaseService.GetLoadProgrammingGrid(pContratoConstruccionId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [Route("GetLoadInvestmentFlowGrid")]
        [HttpGet]
        public async Task<List<ArchivoCargue>> GetLoadInvestmentFlowGrid(int pContratoConstruccionId)
        {
            try
            {
                return await _technicalRequirementsConstructionPhaseService.GetLoadInvestmentFlowGrid(pContratoConstruccionId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [Route("CreateEditDiagnostico")]
        [HttpPost]
        public async Task<Respuesta> CreateEditDiagnostico(ContratoConstruccion pConstruccion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pConstruccion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _technicalRequirementsConstructionPhaseService.CreateEditDiagnostico(pConstruccion);
                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("CreateEditObservacionDiagnostico")]
        [HttpPost]
        public async Task<Respuesta> CreateEditObservacionDiagnostico(ContratoConstruccion pContratoConstruccion, bool esSupervisor)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pContratoConstruccion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _technicalRequirementsConstructionPhaseService.CreateEditObservacionDiagnostico(pContratoConstruccion, esSupervisor);
                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("CreateEditObservacionPlanesProgramas")]
        [HttpPost]
        public async Task<Respuesta> CreateEditObservacionPlanesProgramas(ContratoConstruccion pContratoConstruccion, bool esSupervisor)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pContratoConstruccion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _technicalRequirementsConstructionPhaseService.CreateEditObservacionPlanesProgramas(pContratoConstruccion, esSupervisor);
                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("CreateEditObservacionManejoAnticipo")]
        [HttpPost]
        public async Task<Respuesta> CreateEditObservacionManejoAnticipo(ContratoConstruccion pContratoConstruccion, bool esSupervisor)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pContratoConstruccion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _technicalRequirementsConstructionPhaseService.CreateEditObservacionManejoAnticipo(pContratoConstruccion, esSupervisor);
                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("CreateEditObservacionProgramacionObra")]
        [HttpPost]
        public async Task<Respuesta> CreateEditObservacionProgramacionObra(ContratoConstruccion pContratoConstruccion, bool esSupervisor)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pContratoConstruccion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _technicalRequirementsConstructionPhaseService.CreateEditObservacionProgramacionObra(pContratoConstruccion, esSupervisor);
                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("CreateEditObservacionFlujoInversion")]
        [HttpPost]
        public async Task<Respuesta> CreateEditObservacionFlujoInversion(ContratoConstruccion pContratoConstruccion, bool esSupervisor)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pContratoConstruccion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _technicalRequirementsConstructionPhaseService.CreateEditObservacionFlujoInversion(pContratoConstruccion, esSupervisor);
                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("CreateEditObservacionPerfil")]
        [HttpPost]
        public async Task<Respuesta> CreateEditObservacionPerfil(ConstruccionPerfil pPerfil, bool esSupervisor)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pPerfil.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _technicalRequirementsConstructionPhaseService.CreateEditObservacionPerfil(pPerfil, esSupervisor);
                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [Route("CreateEditPlanesProgramas")]
        [HttpPost]
        public async Task<Respuesta> CreateEditPlanesProgramas(ContratoConstruccion pConstruccion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pConstruccion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _technicalRequirementsConstructionPhaseService.CreateEditPlanesProgramas(pConstruccion);
                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [Route("CreateEditManejoAnticipo")]
        [HttpPost]
        public async Task<Respuesta> CreateEditManejoAnticipo(ContratoConstruccion pConstruccion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pConstruccion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _technicalRequirementsConstructionPhaseService.CreateEditManejoAnticipo(pConstruccion);
                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [Route("CreateEditConstruccionPerfil")]
        [HttpPost]
        public async Task<Respuesta> CreateEditConstruccionPerfil(ContratoConstruccion pConstruccion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pConstruccion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _technicalRequirementsConstructionPhaseService.CreateEditConstruccionPerfil(pConstruccion);
                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [Route("DeleteConstruccionPerfil")]
        [HttpDelete]
        public async Task<IActionResult> DeleteConstruccionPerfil([FromQuery] int pConstruccioPerfilId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _technicalRequirementsConstructionPhaseService.DeleteConstruccionPerfil(pConstruccioPerfilId, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }


        [Route("CreateEditObservacionesCarga")]
        [HttpPost]
        public async Task<IActionResult> CreateEditObservacionesCarga([FromQuery] int pArchivoCargueId, string pObservacion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _technicalRequirementsConstructionPhaseService.CreateEditObservacionesCarga(pArchivoCargueId, pObservacion, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("EnviarAlSupervisor")]
        [HttpPost]
        public async Task<IActionResult> EnviarAlSupervisor([FromQuery] int pContratoId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _technicalRequirementsConstructionPhaseService.EnviarAlSupervisor(pContratoId, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("AprobarInicio")]
        [HttpPost]
        public async Task<IActionResult> AprobarInicio([FromQuery] int pContratoId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _technicalRequirementsConstructionPhaseService.AprobarInicio(pContratoId, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }


        [Route("DeleteConstruccionPerfilNumeroRadicado")]
        [HttpDelete]
        public async Task<IActionResult> DeleteConstruccionPerfilNumeroRadicado([FromQuery] int pConstruccionPerfilNumeroRadicadoId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _technicalRequirementsConstructionPhaseService.DeleteConstruccionPerfilNumeroRadicado(pConstruccionPerfilNumeroRadicadoId, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }


        [Route("DeleteArchivoCargue")]
        [HttpDelete]
        public async Task<IActionResult> DeleteArchivoCargue(int pArchivoCargueId, int pContratoConstruccionId, bool pEsFlujoInvserion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _technicalRequirementsConstructionPhaseService.DeleteArchivoCargue(pArchivoCargueId, pContratoConstruccionId, pEsFlujoInvserion, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }


        [Route("UploadFileToValidateProgramming")]
        [HttpPost]
        public async Task<IActionResult> UploadFileToValidateProgramming(IFormFile file, [FromQuery] int pContratoConstruccinId)
        {
            try
            {
                Respuesta respuesta = new Respuesta();

                if (file.Length > 0 && file.FileName.Contains(".xls"))
                {
                    //string strUsuario = "";
                    string strUsuario = HttpContext.User.FindFirst("User").Value;
                    respuesta = await _technicalRequirementsConstructionPhaseService.UploadFileToValidateProgramming(file, Path.Combine(_settings.Value.DirectoryBase, _settings.Value.DirectoryBaseCargue, _settings.Value.DirectoryBaseProgramacionObra), strUsuario, pContratoConstruccinId);
                }
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }


        [Route("TransferMassiveLoadProgramming")]
        [HttpPost]
        public async Task<IActionResult> TransferMassiveLoadProgramming([FromQuery] string pIdDocument)
        {
            try
            {
                Respuesta respuesta = new Respuesta();
                string pUsuarioModifico = HttpContext.User.FindFirst("User").Value;
                respuesta = await _technicalRequirementsConstructionPhaseService.TransferMassiveLoadProgramming(pIdDocument, pUsuarioModifico);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }


        [Route("UploadFileToValidateInvestmentFlow")]
        [HttpPost]
        public async Task<IActionResult> UploadFileToValidateInvestmentFlow(IFormFile file, [FromQuery] int pContratoConstruccinId)
        {
            try
            {
                Respuesta respuesta = new Respuesta();

                if (file.Length > 0 && file.FileName.Contains(".xls"))
                {
                    //string strUsuario = "";
                    string strUsuario = HttpContext.User.FindFirst("User").Value;
                    respuesta = await _technicalRequirementsConstructionPhaseService.UploadFileToValidateInvestmentFlow(file, Path.Combine(_settings.Value.DirectoryBase, _settings.Value.DirectoryBaseCargue, _settings.Value.DirectoryBaseFlujoInversion), strUsuario, pContratoConstruccinId);
                }
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }


        [Route("TransferMassiveLoadInvestmentFlow")]
        [HttpPost]
        public async Task<IActionResult> TransferMassiveLoadInvestmentFlow([FromQuery] string pIdDocument)
        {
            try
            {
                Respuesta respuesta = new Respuesta();
                string pUsuarioModifico = HttpContext.User.FindFirst("User").Value;
                respuesta = await _technicalRequirementsConstructionPhaseService.TransferMassiveLoadInvestmentFlow(pIdDocument, pUsuarioModifico);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }



        [Route("GenerateDRP")]
        [HttpGet]
        public async Task<IActionResult> GenerateDRP(int pContratoId)
        {
            try
            {
                HttpContext.Connection.RemoteIpAddress.ToString();
                string UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                return File(await _technicalRequirementsConstructionPhaseService.GetPDFDRP(pContratoId, UsuarioModificacion), "application/pdf");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
