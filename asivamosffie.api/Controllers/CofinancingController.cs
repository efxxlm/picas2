using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CofinancingController : ControllerBase
    {
        private readonly ICofinancingService _Cofinancing;

        public CofinancingController(ICofinancingService cofinancingService)
        {
            _Cofinancing = cofinancingService;
        }


        [Route("CreateorUpdateCofinancing")]
        [HttpPost]
        public async Task<IActionResult> CreateCofinancing([FromBody] Cofinanciacion pCofinanciacion)
        {
            try
            {
                HttpContext.Connection.RemoteIpAddress.ToString();
                pCofinanciacion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value.ToUpper();
                Task<Respuesta> result = _Cofinancing.CreateorUpdateCofinancing(pCofinanciacion);
                object respuesta = await result;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [Route("GetListCofinancing")]
        [HttpGet]
        public async Task<List<Cofinanciacion>> GetCofinancing()
        {
            var result = await _Cofinancing.GetListCofinancing();
            return result;
        }


        [Route("GetDocument")]
        [HttpGet]
        public async Task<ActionResult<List<CofinanciacionDocumento>>> GetDocument(int ContributorId)
        {
            try
            {
                return await _Cofinancing.GetDocument(ContributorId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetListAportanteByTipoAportanteId")]
        [HttpGet]
        public async Task<ActionResult<List<CofinanicacionAportanteGrilla>>> GetListAportanteByTipoAportanteId(int pTipoAportanteID)
        {
            try
            {
                return await _Cofinancing.GetListAportanteByTipoAportanteId(pTipoAportanteID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetCofinancingByIdCofinancing")]
        [HttpGet]
        public async Task<Cofinanciacion> GetCofinancing(int IdCofinancing)
        {
            var result = await _Cofinancing.GetCofinanciacionByIdCofinanciacion(IdCofinancing);
            return result;
        }

        [Route("EliminarCofinanciacionByCofinanciacionId")]
        [HttpPost]
        public async Task<IActionResult> EliminarCofinanciacionByCofinanciacionId(int pCofinancicacionId)
        {
            try
            {
                Respuesta respuesta = new Respuesta();
                string pUsuarioModifico = HttpContext.User.FindFirst("User").Value.ToUpper();
                respuesta = await _Cofinancing.EliminarCofinanciacionByCofinanciacionId(pCofinancicacionId, pUsuarioModifico);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }


        [Route("EliminarVigenciaAportanteId")]
        [HttpPost]
        public async Task<IActionResult> EliminarVigenciaAportanteId(int pVigenciaAportanteId)
        {
            try
            {
                Respuesta respuesta = new Respuesta();
                string pUsuarioModifico = HttpContext.User.FindFirst("User").Value.ToUpper();
                respuesta = await _Cofinancing.EliminarVigenciaAportanteId(pVigenciaAportanteId, pUsuarioModifico);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [Route("EliminarCofinanciacionAportanteByCofinanciacionAportanteId")]
        [HttpPost]
        public async Task<IActionResult> EliminarCofinanciacionAportanteByCofinanciacionAportanteId(int pCofinancicacionId)
        {
            try
            {
                Respuesta respuesta = new Respuesta();
                string pUsuarioModifico = HttpContext.User.FindFirst("User").Value.ToUpper();
                respuesta = await _Cofinancing.EliminarCofinanciacionAportanteByCofinanciacionAportanteId(pCofinancicacionId, pUsuarioModifico);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [Route("GetListDocumentoByAportanteId")]
        [HttpGet]
        public async Task<ActionResult<List<CofinanciacionDocumento>>> GetListDocumentoByAportanteId(int pAportanteID)
        {
            try
            {
                return await _Cofinancing.GetListDocumentoByAportanteId(pAportanteID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [Route("GetAportantesByTipoAportante")]
        [HttpGet]
        public async Task<ActionResult<List<CofinanciacionAportante>>> GetListTipoAportante(int pTipoAportanteID)
        {
            try
            {
                return await _Cofinancing.GetListTipoAportante(pTipoAportanteID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Route("EliminarDocumentoAportanteId")]
        [HttpPost]
        public async Task<IActionResult> EliminarDocumentoAportanteId(int pDocumentID)
        {
            try
            {
                Respuesta respuesta = new Respuesta();
                string pUsuarioModifico = HttpContext.User.FindFirst("User").Value.ToUpper();
                respuesta = await _Cofinancing.EliminarDocumentoAportanteId(pDocumentID, pUsuarioModifico);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
