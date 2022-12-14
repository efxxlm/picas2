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
    public class ProjectContractingController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IProjectContractingService _projectContractingService;
        private readonly IOptions<AppSettings> _settings;

        public ProjectContractingController(IDocumentService documentService, IOptions<AppSettings> settings, IProjectContractingService projectContractingService)
        {
            _projectContractingService = projectContractingService;
            _settings = settings;
            _documentService = documentService;
        }


        [Route("DeleteComponenteUso")]
        [HttpPost]
        public async Task<IActionResult> DeleteComponenteUso([FromBody] int pComponenteUsoId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _projectContractingService.DeleteComponenteUso(pComponenteUsoId, HttpContext.User.FindFirst("User").Value.ToUpper());
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("DeleteComponenteAportante")]
        [HttpPost]
        public async Task<IActionResult> DeleteComponenteAportante([FromBody] int pComponenteAportanteId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _projectContractingService.DeleteComponenteAportante(pComponenteAportanteId, HttpContext.User.FindFirst("User").Value.ToUpper());
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }


        [Route("ChangeStateContratacionByIdContratacion")]
        [HttpPost]
        public async Task<Respuesta> ChangeStateContratacionByIdContratacion(int idContratacion, string PCodigoEstado)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _projectContractingService.ChangeStateContratacionByIdContratacion(idContratacion, PCodigoEstado, HttpContext.User.FindFirst("User").Value.ToUpper()
                , _settings.Value.DominioFront, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender
                    );
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return respuesta;
            }
        }

        [Route("DeleteContratacionByIdContratacion")]
        [HttpDelete]
        public async Task<Respuesta> DeleteContratacionByIdContratacion(int idContratacion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _projectContractingService.DeleteContratacionByIdContratacion(idContratacion, HttpContext.User.FindFirst("User").Value.ToUpper());
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return respuesta;
            }
        }
        [AllowAnonymous]
        [Route("GetContratacionByContratacionId")]
        [HttpGet]
        public async Task<Contratacion> GetContratacionByContratacionId(int pContratacionId)
        {
            return await _projectContractingService.GetContratacionByContratacionId(pContratacionId);
        }


        [Route("GetListContratacionObservacion")]
        [HttpGet]
        public async Task<Contratacion> GetListContratacionObservacion(int pContratacionId)
        {
            return await _projectContractingService.GetListContratacionObservacion(pContratacionId);
        }

        [Route("GetContratacionByContratacionIdWithGrillaProyecto")]
        [HttpGet]
        public async Task<Contratacion> GetContratacionByContratacionIdWithGrillaProyecto(int pContratacionId)
        {
            return await _projectContractingService.GetContratacionByContratacionIdWithGrillaProyecto(pContratacionId);
        }


        [Route("GetListProyectsByFilters")]
        [HttpGet]
        public async Task<List<ProyectoGrilla>> GetListProyectsByFilters(string pTipoIntervencion, string pLlaveMen, string pRegion, string pDepartamento, string pMunicipio, int pIdInstitucionEducativa, int pIdSede)
        {
            var respuesta = await _projectContractingService.GetListProyectsByFilters(pTipoIntervencion, pLlaveMen, pRegion, pDepartamento, pMunicipio, pIdInstitucionEducativa, pIdSede);
            return respuesta;
        }

        [Route("GetListContractingByFilters")]
        [HttpGet]
        public async Task<List<ContratistaGrilla>> GetListContractingByFilters(string pTipoIdentificacionCodigo, string pNumeroIdentidicacion, string pNombre, bool? EsConsorcio)
        {
            var respuesta = await _projectContractingService.GetListContractingByFilters(pTipoIdentificacionCodigo, pNumeroIdentidicacion, pNombre, EsConsorcio);
            return respuesta;
        }

        [AllowAnonymous]
        [Route("GetListContratacion")]
        [HttpGet]
        public async Task<List<Contratacion>> GetListContratacion()
        {
            var respuesta = await _projectContractingService.GetListContratacion();
            return respuesta;
        }

        [Route("CreateContratacionProyecto")]
        [HttpPost]
        public async Task<Respuesta> CreateContratacionProyecto(Contratacion pContratacion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _projectContractingService.CreateContratacionProyecto(pContratacion, HttpContext.User.FindFirst("User").Value.ToUpper());
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return respuesta;
            }
        }

        [Route("GetListContratacionProyectoByContratacionId")]
        [HttpGet]
        public async Task<List<ContratacionProyecto>> GetListContratacionProyectoByContratacionId(int idContratacion)
        {
            var respuesta = await _projectContractingService.GetListContratacionProyectoByContratacionId(idContratacion);
            return respuesta;
        }

        [Route("CreateEditContratacion")]
        [HttpPost]
        public async Task<IActionResult> CreateEditContratacion(Contratacion pContratacion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pContratacion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value.ToUpper();
                respuesta = await _projectContractingService.CreateEditContratacion(pContratacion);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return Ok(respuesta);
            }
        }

        [Route("{contratacionId}/plazo")]
        [HttpPost]
        public async Task<IActionResult> CreateEditContratacion(int contratacionId , TermLimit termLimit)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                termLimit.Usuario = HttpContext.User.FindFirst("User").Value.ToUpper();
                respuesta = await _projectContractingService.CreateEditContratacionTermLimit(contratacionId, termLimit);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return Ok(respuesta);
            }
        }

        [Route("CreateEditContratacionProyecto")]
        [HttpPost]
        public async Task<IActionResult> CreateEditContratacionProyecto(ContratacionProyecto pContratacionProyecto)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pContratacionProyecto.UsuarioCreacion = HttpContext.User.FindFirst("User").Value.ToUpper();
                respuesta = await _projectContractingService.CreateEditContratacionProyecto(pContratacionProyecto, false);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return Ok(respuesta);
            }
        }

        [Route("CreateEditContratacionProyectoAportanteByContratacionproyecto")]
        [HttpPost]
        public async Task<IActionResult> CreateEditContratacionProyectoAportanteByContratacionproyecto(ContratacionProyecto pContratacionProyecto, bool esTransaccion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pContratacionProyecto.UsuarioCreacion = HttpContext.User.FindFirst("User").Value.ToUpper();
                respuesta = await _projectContractingService.CreateEditContratacionProyectoAportanteByContratacionproyecto(pContratacionProyecto, false);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return Ok(respuesta);
            }
        }

        [Route("CreateEditContratacionProyectoAportante")]
        [HttpPost]
        public async Task<IActionResult> CreateEditContratacionProyectoAportante(ContratacionProyectoAportante pContratacionProyectoAportante)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pContratacionProyectoAportante.UsuarioCreacion = HttpContext.User.FindFirst("User").Value.ToUpper();
                respuesta = await _projectContractingService.CreateEditContratacionProyectoAportante(pContratacionProyectoAportante, false);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return Ok(respuesta);
            }
        }
        [AllowAnonymous]
        [Route("GetContratacionProyectoById")]
        [HttpGet]
        public async Task<ContratacionProyecto> GetContratacionProyectoById(int idContratacionProyecto)
        {
            var respuesta = await _projectContractingService.GetContratacionProyectoById(idContratacionProyecto);
            return respuesta;
        }

        [Route("GetListFaseComponenteUso")]
        [HttpGet]
        public async Task<List<FaseComponenteUso>> GetListFaseComponenteUso()
        {
            return await _projectContractingService.GetListFaseComponenteUso();
        }
    }
}
