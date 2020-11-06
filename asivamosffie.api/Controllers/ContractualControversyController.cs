using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractualControversyController : ControllerBase
    {
        public readonly IContractualControversy _contractualControversy;

        public ContractualControversyController(IContractualControversy contractualControversy)
        {
            _contractualControversy = contractualControversy;
        }

        [HttpPost]
        [Route("CreateEditarControversiaTAI")]
        public async Task<IActionResult> CreateEditarControversiaTAI(ControversiaContractual controversiaContractual)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                if(controversiaContractual.ControversiaContractualId==0)
                    controversiaContractual.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                else
                    controversiaContractual.UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _contractualControversy.CreateEditarControversiaTAI(controversiaContractual);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("CreateEditNuevaActualizacionTramite")]
        
        public async Task<IActionResult> CreateEditNuevaActualizacionTramite(ControversiaActuacion controversiaActuacion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                //controversiaContractual.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                if (controversiaActuacion.ControversiaActuacionId == 0)
                    controversiaActuacion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                else
                    controversiaActuacion.UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _contractualControversy.CreateEditNuevaActualizacionTramite(controversiaActuacion);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpGet]
        [Route("GetListGrillaTipoSolicitudControversiaContractual")]
        public async Task<ActionResult<List<GrillaTipoSolicitudControversiaContractual>>> GetListGrillaTipoSolicitudControversiaContractual()
        {
            try
            {
                return await _contractualControversy.ListGrillaTipoSolicitudControversiaContractual();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetVistaContratoContratista")]
        public async Task<ActionResult<VistaContratoContratista>> GetVistaContratoContratista(int pContratoId)
        {
            try
            {
                return await _contractualControversy.GetVistaContratoContratista(pContratoId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
     
        [HttpPut]
        [Route("CambiarEstadoControversiaContractual")]
        public async Task<IActionResult> CambiarEstadoControversiaContractual([FromQuery] int pControversiaContractualId, string pNuevoCodigoEstado)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _contractualControversy.CambiarEstadoControversiaContractual(pControversiaContractualId,  pNuevoCodigoEstado, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        



        [Route("EliminarControversiaContractual")]
        [HttpPost]
        public async Task<IActionResult> EliminarControversiaContractual(int pControversiaContractualId)
        {
            try
            {
                Respuesta respuesta = new Respuesta();
                string pUsuarioModifico = HttpContext.User.FindFirst("User").Value.ToUpper();
                respuesta = await _contractualControversy.EliminarControversiaContractual(pControversiaContractualId, pUsuarioModifico);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }



        [Route("EliminarControversiaActuacion")]
        [HttpPost]
        public async Task<IActionResult> EliminarControversiaActuacion(int pControversiaActuacionId)
        {
            try
            {
                Respuesta respuesta = new Respuesta();
                string pUsuarioModifico = HttpContext.User.FindFirst("User").Value.ToUpper();
                respuesta = await _contractualControversy.EliminarControversiaActuacion(pControversiaActuacionId, pUsuarioModifico);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }


        [HttpPut]
        [Route("CambiarEstadoControversiaActuacion")]
        public async Task<IActionResult> CambiarEstadoControversiaActuacion([FromQuery] int pControversiaActuacionId, string pNuevoCodigoEstadoAvance)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _contractualControversy.CambiarEstadoControversiaActuacion(pControversiaActuacionId, pNuevoCodigoEstadoAvance, HttpContext.User.FindFirst("User").Value);
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
